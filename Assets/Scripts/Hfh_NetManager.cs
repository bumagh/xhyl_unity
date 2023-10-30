using JsonFx.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Hfh_NetManager : Hfh_Singleton<Hfh_NetManager>
{
	public delegate void NetMessageHandler(object[] args);

	public Hfh_ClientSocket _pClient;

	private Hfh_DataEncrypt _pCrypto;

	private Queue<Hfh_NetMessage> _netMsgQueue = new Queue<Hfh_NetMessage>();

	private Dictionary<string, NetMessageHandler> _netMsgHandlerDic = new Dictionary<string, NetMessageHandler>();

	public bool isConnected;

	public bool isReady;

	public bool isLogined;

	public bool isError;

	private bool isHeartPong = true;

	public int connectCount;

	public float connectTimeCount;

	public int connectMaxTimes;

	public float connectMaxTimeout;

	private void Awake()
	{
		if (Hfh_Singleton<Hfh_NetManager>._instance == null)
		{
			Hfh_Singleton<Hfh_NetManager>.SetInstance(this);
		}
		RegisterHandler("netConnected", _onConnected);
	}

	private void Update()
	{
		Dispatching();
	}

	private void Dispatching()
	{
		Hfh_NetMessage netMsg;
		while (GetNetMessage(out netMsg))
		{
			DispatchNetMessage(netMsg);
		}
	}

	private bool GetNetMessage(out Hfh_NetMessage netMsg)
	{
		Queue<Hfh_NetMessage> netMsgQueue = _netMsgQueue;
		lock (netMsgQueue)
		{
			if (_netMsgQueue.Count == 0)
			{
				netMsg = null;
				return false;
			}
			netMsg = _netMsgQueue.Dequeue();
			return true;
		}
	}

	private void DispatchNetMessage(Hfh_NetMessage netMsg)
	{
		if (_netMsgHandlerDic.ContainsKey(netMsg.method))
		{
			_netMsgHandlerDic[netMsg.method](netMsg.args);
		}
		else
		{
			UnityEngine.Debug.Log("DispatchNetMessage>unknown msg: " + netMsg.method);
		}
	}

	public void Connect(string ip, int port)
	{
		connectCount++;
		_initClientSocket();
		_pClient.Connect(ip, port);
	}

	public void Disconnect()
	{
		isConnected = false;
		isReady = false;
		isLogined = false;
		if (_pClient != null)
		{
			_pClient.Close();
			_pClient = null;
		}
	}

	private void _initClientSocket()
	{
		isHeartPong = true;
		isConnected = false;
		isReady = false;
		isLogined = false;
		if (_pClient != null)
		{
			_pClient.Close();
			_pClient = null;
		}
		if (_pCrypto != null)
		{
			_pCrypto = null;
		}
		_pClient = new Hfh_ClientSocket(useTcpClient: true);
		_pCrypto = new Hfh_DataEncrypt();
		_pClient.dataHandler = _handleData;
		_pClient.exceptionHandler = _handleException;
		_pClient.connectHandler = _handleConnected;
		_pClient.checkTimeoutHandler = _checkTimeoutStart;
	}

	protected void _handleData(byte[] msgBuffer, int packetId, int endIndex)
	{
		_pCrypto.GetKey();
		if (_pCrypto.GetKey() != "none")
		{
			msgBuffer = _pCrypto.Decrypt(msgBuffer);
		}
		Hashtable hashtable = new Hashtable();
		string @string = Encoding.UTF8.GetString(msgBuffer);
		UnityEngine.Debug.Log(@string);
		hashtable = JsonReader.Deserialize<Hashtable>(@string.ToString());
		string text = hashtable["method"].ToString();
		object[] array = hashtable["args"] as object[];
		connectTimeCount = 0f;
		if (text == "publicKey")
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary = (array[0] as Dictionary<string, object>);
			if ((bool)dictionary["success"])
			{
				long serverTime = Convert.ToInt64(dictionary["time"]);
				string str = (string)dictionary["key"];
				_pCrypto.setServerTime(serverTime);
				_pCrypto.DecryptKey(str);
				isReady = true;
			}
			return;
		}
		if (text == "heart")
		{
			isHeartPong = true;
			return;
		}
		if (text == "quitToLogin")
		{
			int[] array2 = hashtable["args"] as int[];
			if (array2[0] > 0)
			{
				array = new object[1]
				{
					array2[0]
				};
			}
		}
		Queue<Hfh_NetMessage> netMsgQueue = _netMsgQueue;
		lock (netMsgQueue)
		{
			Hfh_NetMessage hfh_NetMessage = new Hfh_NetMessage(text, array);
			hfh_NetMessage.jsonString = @string;
			hfh_NetMessage.packetId = packetId;
			hfh_NetMessage.endIndex = endIndex;
			_netMsgQueue.Enqueue(hfh_NetMessage);
		}
	}

	private void _handleException(Exception ex)
	{
		isConnected = false;
		isReady = false;
		isLogined = false;
		Queue<Hfh_NetMessage> netMsgQueue = _netMsgQueue;
		lock (netMsgQueue)
		{
			_netMsgQueue.Enqueue(new Hfh_NetMessage("netDown", new object[1]
			{
				ex
			}));
		}
	}

	private void _handleConnected()
	{
		try
		{
			isConnected = true;
			Queue<Hfh_NetMessage> netMsgQueue = _netMsgQueue;
			lock (netMsgQueue)
			{
				_netMsgQueue.Enqueue(new Hfh_NetMessage("netConnected", new object[0]));
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("_onConnected> exception: " + ex.Message);
			_handleException(ex);
		}
	}

	public void _checkTimeoutStart(int id, int timeout, Hfh_ClientSocket cs)
	{
		StartCoroutine(_checkTimeout(id, timeout, cs));
	}

	private void _onConnected(object[] args)
	{
		UnityEngine.Debug.Log("连接成功！");
		SendVersion();
	}

	public void RegisterHandler(string method, NetMessageHandler hander)
	{
		if (!_netMsgHandlerDic.ContainsKey(method))
		{
			NetMessageHandler value = hander.Invoke;
			_netMsgHandlerDic.Add(method, value);
		}
		else
		{
			_netMsgHandlerDic[method] = hander;
			UnityEngine.Debug.Log(method + " register again");
		}
	}

	public void SendPublicKey()
	{
		string[] array = new string[2];
		array = _pCrypto.NetConnectGetRsaKey();
		object[] args = new object[2]
		{
			array[0],
			array[1]
		};
		Send("userService/publicKey", args);
	}

	public void SendVersion()
	{
		UnityEngine.Debug.Log("ZH2_GVars.versionCode: " + Hfh_GVars.versionCode);
		Send("userService/checkVersion", new object[1]
		{
			Hfh_GVars.versionCode
		});
	}

	public void Send(string method, object[] args)
	{
		if (method != "userService/heart")
		{
			DateTime now = DateTime.Now;
			UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("发送内容:" + method + "(" + now.Hour + ":" + now.Minute + ":" + now.Second + "):" + JsonWriter.Serialize(args)));
		}
		else
		{
			StartCoroutine(_checkHeartTimeout(3000));
		}
		Hashtable hashtable = new Hashtable();
		hashtable.Add("method", method);
		hashtable.Add("args", args);
		hashtable.Add("version", Hfh_GVars.Version);
		if (isReady)
		{
			hashtable.Add("time", _pCrypto.GetUnixTime());
		}
		string s = JsonWriter.Serialize(hashtable);
		byte[] array = Encoding.UTF8.GetBytes(s);
		if (_pCrypto.GetKey() != "none")
		{
			array = _pCrypto.Encrypt(array);
		}
		_pClient.Send(array);
	}

	private IEnumerator _checkHeartTimeout(int timeout)
	{
		yield return new WaitForSeconds((float)timeout / 1000f);
		bool isTimeout = false;
		if (isReady && !isHeartPong)
		{
			Queue<Hfh_NetMessage> netMsgQueue = _netMsgQueue;
			lock (netMsgQueue)
			{
				isConnected = false;
				isReady = false;
				isLogined = false;
				_netMsgQueue.Enqueue(new Hfh_NetMessage("heart timeout", new object[0]));
				isTimeout = true;
			}
		}
		if (isTimeout)
		{
			UnityEngine.Debug.Log(Hfh_LogHelper.Aqua("_checkHeartTimeout is timeout"));
		}
	}

	private IEnumerator _checkTimeout(int id, int timeout, Hfh_ClientSocket cs)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.Aqua("_checkTimeout start id: {0}", id));
		yield return new WaitForSeconds((float)timeout / 1000f);
		bool isTimeout = false;
		if (cs != null && cs.doneList.Count > id && !cs.doneList[id])
		{
			Queue<Hfh_NetMessage> netMsgQueue = _netMsgQueue;
			lock (netMsgQueue)
			{
				_netMsgQueue.Enqueue(new Hfh_NetMessage("timeout", new object[1]
				{
					id
				}));
				isTimeout = true;
			}
		}
		if (isTimeout)
		{
			UnityEngine.Debug.Log(Hfh_LogHelper.Aqua("_checkTimeout start id: {0} timeout: {1}", id, isTimeout));
		}
	}

	public IEnumerator KeepHeart()
	{
		while (true)
		{
			if (isConnected && isLogined)
			{
				Send("userService/heart", new object[0]);
			}
			yield return new WaitForSeconds(5f);
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
	}

	public void OnDestroy()
	{
	}
}
