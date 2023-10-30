using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LLD_NetManager : LLD_MB_Singleton<LLD_NetManager>
{
	public delegate void NetMessageHandler(object[] args);

	private LLD_ClientSocket _pClient;

	private LLD_DataEncrypt _pCrypto;

	[HideInInspector]
	public bool isConnected;

	[HideInInspector]
	public bool isReady;

	[HideInInspector]
	public bool isLogined;

	[HideInInspector]
	public int connectCount;

	[HideInInspector]
	public float connectTimeCount;

	[HideInInspector]
	public int connectMaxTimes;

	[HideInInspector]
	public float connectMaxTimeout;

	[HideInInspector]
	public bool dropSend;

	[HideInInspector]
	public bool dropDispatch;

	[HideInInspector]
	public bool isHeartPong = true;

	private Queue<LLD_NetMessage> _netMsgQueue = new Queue<LLD_NetMessage>();

	private Dictionary<string, NetMessageHandler> _netMsgHandlerDic = new Dictionary<string, NetMessageHandler>();

	private void Awake()
	{
		if (LLD_MB_Singleton<LLD_NetManager>._instance == null)
		{
			LLD_MB_Singleton<LLD_NetManager>.SetInstance(this);
		}
		RegisterHandler("netConnected", _onConnected);
	}

	private void _initClientSocket()
	{
		UnityEngine.Debug.Log("_initClientSocket");
		isHeartPong = true;
		isConnected = false;
		isReady = false;
		isLogined = false;
		if (_pClient != null)
		{
			_pClient.Close();
		}
		if (_pCrypto != null)
		{
			_pCrypto = null;
		}
		_pClient = new LLD_ClientSocket(useTcpClient: true);
		_pCrypto = new LLD_DataEncrypt();
		_pClient.dataHandler = _handleData;
		_pClient.exceptionHandler = _handleException;
		_pClient.connectHandler = _handleConnected;
		_pClient.checkTimeoutHandler = _checkTimeoutStart;
	}

	private void _onConnected(object[] args)
	{
		UnityEngine.Debug.Log("Connect success!!!");
		SendVersion();
	}

	private void _handleException(Exception ex)
	{
		UnityEngine.Debug.Log("_handleException");
		isConnected = false;
		isReady = false;
		isLogined = false;
		Queue<LLD_NetMessage> netMsgQueue = _netMsgQueue;
		lock (netMsgQueue)
		{
			_netMsgQueue.Enqueue(new LLD_NetMessage("netDown", new object[1]
			{
				ex
			}));
		}
	}

	public void Connect(string ip, int port)
	{
		connectCount++;
		_initClientSocket();
		_pClient.Connect(ip, port);
	}

	private void _handleConnected()
	{
		try
		{
			isConnected = true;
			Queue<LLD_NetMessage> netMsgQueue = _netMsgQueue;
			lock (netMsgQueue)
			{
				_netMsgQueue.Enqueue(new LLD_NetMessage("netConnected", new object[0]));
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("_onConnected> exception: " + ex.Message);
			_handleException(ex);
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
		UnityEngine.Debug.Log("ZH2_GVars.versionCode: " + LLD_GVars.versionCode);
		Send("userService/checkVersion", new object[1]
		{
			LLD_GVars.versionCode
		});
	}

	public IEnumerator KeepHeart()
	{
		while (true)
		{
			if (isConnected && isLogined)
			{
				Send("userService/heart", new object[0]);
				isHeartPong = false;
			}
			yield return new WaitForSeconds(3f);
		}
	}

	public void Handle_sendServerTime(object[] args)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary = (args[0] as Dictionary<string, object>);
		if ((bool)dictionary["success"])
		{
			long serverTime = Convert.ToInt64(dictionary["time"]);
			string str = (string)dictionary["key"];
			_pCrypto.setServerTime(serverTime);
			_pCrypto.DecryptKey(str);
			isReady = true;
		}
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
		UnityEngine.Debug.LogError("@string: " + @string);
		hashtable = JsonFx.Json.JsonReader.Deserialize<Hashtable>(@string);
		string text = hashtable["method"].ToString();
		object[] args = hashtable["args"] as object[];
		connectTimeCount = 0f;
		if (text == "publicKey")
		{
			Handle_sendServerTime(args);
			return;
		}
		if (text == "heart")
		{
			isHeartPong = true;
			return;
		}
		Queue<LLD_NetMessage> netMsgQueue = _netMsgQueue;
		lock (netMsgQueue)
		{
			LLD_NetMessage lLD_NetMessage = new LLD_NetMessage(text, args);
			lLD_NetMessage.jsonString = @string;
			lLD_NetMessage.packetId = packetId;
			lLD_NetMessage.endIndex = endIndex;
			_netMsgQueue.Enqueue(lLD_NetMessage);
		}
	}

	private IEnumerator _checkTimeout(int id, int timeout, LLD_ClientSocket cs)
	{
		UnityEngine.Debug.Log(LLD_LogHelper.Aqua("_checkTimeout start id: {0}", id));
		yield return new WaitForSeconds((float)timeout / 1000f);
		bool isTimeout = false;
		if (cs != null && cs.doneList.Count > id && !cs.doneList[id])
		{
			Queue<LLD_NetMessage> netMsgQueue = _netMsgQueue;
			lock (netMsgQueue)
			{
				_netMsgQueue.Enqueue(new LLD_NetMessage("timeout", new object[1]
				{
					id
				}));
				isTimeout = true;
			}
		}
		if (isTimeout)
		{
			UnityEngine.Debug.Log(LLD_LogHelper.Aqua("_checkTimeout start id: {0} timeout: {1}", id, isTimeout));
		}
	}

	private IEnumerator _checkHeartTimeout(int timeout)
	{
		yield return new WaitForSeconds((float)timeout / 1000f);
		bool isTimeout = false;
		if (isReady && !isHeartPong)
		{
			Queue<LLD_NetMessage> netMsgQueue = _netMsgQueue;
			lock (netMsgQueue)
			{
				isConnected = false;
				isReady = false;
				isLogined = false;
				_netMsgQueue.Enqueue(new LLD_NetMessage("heart timeout", new object[0]));
				isTimeout = true;
			}
		}
		if (isTimeout)
		{
			UnityEngine.Debug.Log(LLD_LogHelper.Aqua("_checkHeartTimeout is timeout"));
		}
	}

	public void _checkTimeoutStart(int id, int timeout, LLD_ClientSocket cs)
	{
		StartCoroutine(_checkTimeout(id, timeout, cs));
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

	public bool IsMethodRegistered(string method)
	{
		return _netMsgHandlerDic.ContainsKey(method);
	}

	public void Send(string method, object[] args)
	{
		if (method != "userService/heart")
		{
			DateTime now = DateTime.Now;
			UnityEngine.Debug.LogError("发送: " + method + "  ---  " + now.Hour + ":" + now.Minute + ":" + now.Second + " 数据: " + JsonMapper.ToJson(args));
		}
		else
		{
			StartCoroutine(_checkHeartTimeout(3000));
		}
		if (dropSend && method != "userService/heart")
		{
			UnityEngine.Debug.Log(LLD_LogHelper.Magenta("drop send>>method: {0}", method));
			return;
		}
		Hashtable hashtable = new Hashtable();
		hashtable.Add("method", method);
		hashtable.Add("args", args);
		hashtable.Add("version", LLD_GVars.Version);
		if (isReady)
		{
			hashtable.Add("time", _pCrypto.GetUnixTime());
		}
		string s = JsonFx.Json.JsonWriter.Serialize(hashtable);
		byte[] array = Encoding.UTF8.GetBytes(s);
		if (method != "userService/heart" && method == "userService/publicKey")
		{
			UnityEngine.Debug.Log("send>>json: " + "*".Repeat(10));
		}
		if (_pCrypto.GetKey() != "none")
		{
			array = _pCrypto.Encrypt(array);
		}
		_pClient.Send(array);
	}

	private void Update()
	{
		Dispatching();
	}

	private void Dispatching()
	{
		LLD_NetMessage netMsg;
		while (GetNetMessage(out netMsg))
		{
			DispatchNetMessage(netMsg);
		}
	}

	private bool GetNetMessage(out LLD_NetMessage netMsg)
	{
		Queue<LLD_NetMessage> netMsgQueue = _netMsgQueue;
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

	private void DispatchNetMessage(LLD_NetMessage netMsg)
	{
		if (dropDispatch)
		{
			UnityEngine.Debug.Log(LLD_LogHelper.Magenta("drop send>>method: {0}", netMsg.method));
		}
		else if (_netMsgHandlerDic.ContainsKey(netMsg.method))
		{
			_netMsgHandlerDic[netMsg.method](netMsg.args);
		}
		else
		{
			UnityEngine.Debug.Log("DispatchNetMessage>unknown msg: " + netMsg.method);
		}
	}

	public void OnDestroy()
	{
		UnityEngine.Debug.Log("NetworkManager OnDestory");
		if (_pClient != null)
		{
			_pClient.Close();
		}
	}

	public void Disconnect()
	{
		if (_pClient != null)
		{
			_pClient.Close();
		}
	}
}
