using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class STWM_NetManager : STWM_MB_Singleton<STWM_NetManager>
{
	public delegate void NetMessageHandler(object[] args);

	private STWM_ClientSocket _pClient;

	private STWM_DataEncrypt _pCrypto;

	public bool isConnected;

	public bool isReady;

	public bool isLogined;

	private STWM_FakeNet _fake;

	public bool useFake = true;

	public int connectCount;

	public float connectTimeCount;

	public int connectMaxTimes;

	public float connectMaxTimeout;

	public bool dropSend;

	public bool dropDispatch;

	public bool isHeartPong = true;

	private Queue<STWM_NetMessage> _netMsgQueue = new Queue<STWM_NetMessage>();

	private Dictionary<string, NetMessageHandler> _netMsgHandlerDic = new Dictionary<string, NetMessageHandler>();

	private void Awake()
	{
		if (STWM_MB_Singleton<STWM_NetManager>._instance == null)
		{
			STWM_MB_Singleton<STWM_NetManager>.SetInstance(this);
		}
		RegisterHandler("netConnected", _onConnected);
	}

	private void _initClientSocket()
	{
		isHeartPong = true;
		if (useFake)
		{
			UnityEngine.Debug.Log("use <color=magenta>fake</color> net");
			isConnected = true;
			isReady = true;
			isLogined = true;
			_fake = new STWM_FakeNet(this, _netMsgQueue);
			_fake.Init();
			return;
		}
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
		_pClient = new STWM_ClientSocket(useTcpClient: true);
		_pCrypto = new STWM_DataEncrypt();
		_pClient.dataHandler = _handleData;
		_pClient.exceptionHandler = _handleException;
		_pClient.connectHandler = _handleConnected;
		_pClient.checkTimeoutHandler = _checkTimeoutStart;
	}

	private void _onConnected(object[] args)
	{
		UnityEngine.Debug.Log("====链接成功====");
		SendVersion();
	}

	private void _handleException(Exception ex)
	{
		UnityEngine.Debug.Log("_handleException: " + ex.Message);
		isConnected = false;
		isReady = false;
		isLogined = false;
		Queue<STWM_NetMessage> netMsgQueue = _netMsgQueue;
		lock (netMsgQueue)
		{
			_netMsgQueue.Enqueue(new STWM_NetMessage("netDown", new object[1]
			{
				ex
			}));
		}
	}

	public void Connect(string ip, int port)
	{
		connectCount++;
		_initClientSocket();
		if (!useFake)
		{
			_pClient.Connect(ip, port);
		}
	}

	private void _handleConnected()
	{
		try
		{
			isConnected = true;
			Queue<STWM_NetMessage> netMsgQueue = _netMsgQueue;
			lock (netMsgQueue)
			{
				_netMsgQueue.Enqueue(new STWM_NetMessage("netConnected", new object[0]));
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
		Send("userService/checkVersion", new object[1]
		{
			STWM_GVars.versionCode
		});
	}

	public IEnumerator KeepHeart()
	{
		while (true)
		{
			if (isConnected && isLogined)
			{
				Send("userService/heart", new object[0]);
				if (!useFake)
				{
					isHeartPong = false;
				}
			}
			yield return new WaitForSeconds(5f);
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
		hashtable = JsonFx.Json.JsonReader.Deserialize<Hashtable>(@string);
		string text = hashtable["method"].ToString();
		object[] args = null;
		if (text == "quitToLogin")
		{
			try
			{
				JsonData jsonData = JsonMapper.ToObject(@string);
				UnityEngine.Debug.LogError("收到踢出: " + jsonData.ToJson());
				int num = (int)jsonData["args"][0];
				UnityEngine.Debug.LogError("收到踢出: " + num);
				args = new object[1]
				{
					num
				};
			}
			catch
			{
			}
		}
		else
		{
			args = (hashtable["args"] as object[]);
		}
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
		Queue<STWM_NetMessage> netMsgQueue = _netMsgQueue;
		lock (netMsgQueue)
		{
			STWM_NetMessage sTWM_NetMessage = new STWM_NetMessage(text, args);
			sTWM_NetMessage.jsonString = @string;
			sTWM_NetMessage.packetId = packetId;
			sTWM_NetMessage.endIndex = endIndex;
			_netMsgQueue.Enqueue(sTWM_NetMessage);
		}
	}

	private IEnumerator _checkTimeout(int id, int timeout, STWM_ClientSocket cs)
	{
		UnityEngine.Debug.Log(STWM_LogHelper.Aqua("_checkTimeout start id: {0}", id));
		yield return new WaitForSeconds((float)timeout / 1000f);
		bool isTimeout = false;
		if (cs != null && cs.doneList.Count > id && !cs.doneList[id])
		{
			Queue<STWM_NetMessage> netMsgQueue = _netMsgQueue;
			lock (netMsgQueue)
			{
				_netMsgQueue.Enqueue(new STWM_NetMessage("timeout", new object[1]
				{
					id
				}));
				isTimeout = true;
			}
		}
		if (isTimeout)
		{
			UnityEngine.Debug.Log(STWM_LogHelper.Aqua("_checkTimeout start id: {0} timeout: {1}", id, isTimeout));
		}
	}

	private IEnumerator _checkHeartTimeout(int timeout)
	{
		yield return new WaitForSeconds((float)timeout / 1000f);
		bool isTimeout = false;
		if (isReady && !isHeartPong)
		{
			Queue<STWM_NetMessage> netMsgQueue = _netMsgQueue;
			lock (netMsgQueue)
			{
				isConnected = false;
				isReady = false;
				isLogined = false;
				_netMsgQueue.Enqueue(new STWM_NetMessage("heart timeout", new object[0]));
				isTimeout = true;
			}
		}
		if (isTimeout)
		{
			UnityEngine.Debug.Log(STWM_LogHelper.Aqua("_checkHeartTimeout is timeout"));
		}
	}

	public void _checkTimeoutStart(int id, int timeout, STWM_ClientSocket cs)
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
			UnityEngine.Debug.Log("发送 " + method + "  " + JsonMapper.ToJson(args));
		}
		else
		{
			StartCoroutine(_checkHeartTimeout(3000));
		}
		if (dropSend && method != "userService/heart")
		{
			UnityEngine.Debug.Log(STWM_LogHelper.Magenta("drop send>>method: {0}", method));
			return;
		}
		if (useFake)
		{
			StartCoroutine(_fake.MakeResponse(method, args));
			return;
		}
		Hashtable hashtable = new Hashtable();
		hashtable.Add("method", method);
		hashtable.Add("args", args);
		hashtable.Add("version", STWM_GVars.Version);
		if (isReady)
		{
			hashtable.Add("time", _pCrypto.GetUnixTime());
		}
		string s = JsonFx.Json.JsonWriter.Serialize(hashtable);
		byte[] array = Encoding.UTF8.GetBytes(s);
		if (!(method != "userService/heart") || method == "userService/publicKey")
		{
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
		STWM_NetMessage netMsg;
		while (GetNetMessage(out netMsg))
		{
			DispatchNetMessage(netMsg);
		}
	}

	private bool GetNetMessage(out STWM_NetMessage netMsg)
	{
		Queue<STWM_NetMessage> netMsgQueue = _netMsgQueue;
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

	private void DispatchNetMessage(STWM_NetMessage netMsg)
	{
		if (dropDispatch)
		{
			UnityEngine.Debug.Log(STWM_LogHelper.Magenta("drop send>>method: {0}", netMsg.method));
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
