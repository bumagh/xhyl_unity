using JsonFx.Json;
using LitJson;
using M__M.GameHall.Common;
using M__M.HaiWang.Demo;
using M__M.HaiWang.Message;
using M__M.HaiWang.UIDefine;
using M__Mou.mobile.arcade.network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace M__M.GameHall.Net
{
	public class FK3_NetManager : FK3_MB_Singleton<FK3_NetManager>
	{
		public delegate void NetMessageHandler(object[] args);

		private FK3_ClientSocket _pClient;

		private FK3_DataEncrypt _pCrypto;

		public bool isConnected;

		public bool isReady;

		public bool isLogined;

		public bool useFake;

		public int connectCount;

		public float connectTimeCount;

		public int connectMaxTimes;

		public float connectMaxTimeout;

		public bool dropSend;

		public bool dropDispatch;

		public bool isHeartPong = true;

		public bool printSendLog;

		public bool printRecvLog;

		public bool useCrypto;

		public FK3_LogControl logControl = new FK3_LogControl();

		private string m_version = "1.2.14";

		public Func<string, bool> filterSendMsgLogFunc;

		public Func<string, bool> filterRecvMsgLogFunc;

		private Coroutine _coKeepHeart;

		private Queue<FK3_NetMessage> _netMsgQueue = new Queue<FK3_NetMessage>();

		private Queue<FK3_NetMessage> _netMsgQueueFishBirthPause = new Queue<FK3_NetMessage>();

		private Dictionary<string, NetMessageHandler> _netMsgHandlerDic = new Dictionary<string, NetMessageHandler>();

		private bool isApplicationPause;

		private float lastTime;

		private void Awake()
		{
			if (FK3_MB_Singleton<FK3_NetManager>._instance == null)
			{
				FK3_MB_Singleton<FK3_NetManager>.SetInstance(this);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			RegisterHandler("netConnected", _onConnected);
		}

		private void _initClientSocket()
		{
			if (logControl.enable)
			{
			}
			isHeartPong = true;
			if (useFake)
			{
				if (logControl.enable)
				{
					UnityEngine.Debug.Log("use <color=magenta>fake</color> net");
				}
				isConnected = true;
				isReady = true;
				isLogined = true;
			}
			else
			{
				try
				{
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
					_pClient = new FK3_ClientSocket(useTcpClient: true);
					_pCrypto = new FK3_DataEncrypt();
					_pClient.dataHandler = _handleData;
					_pClient.exceptionHandler = _handleException;
					_pClient.connectHandler = _handleConnected;
					_pClient.checkTimeoutHandler = _checkTimeoutStart;
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("错误: " + arg);
				}
			}
		}

		private void _onConnected(object[] args)
		{
			if (logControl.enable)
			{
				UnityEngine.Debug.Log("链接成功!!!");
			}
			FK3_MB_Singleton<FK3_NetManager>.GetInstance().connectCount = 0;
		}

		public void SendHeartBeat()
		{
			if (_coKeepHeart != null)
			{
				StopCoroutine(_coKeepHeart);
			}
			_coKeepHeart = StartCoroutine(KeepHeart());
		}

		private void _handleException(Exception ex)
		{
			UnityEngine.Debug.LogError("消息: " + ex.Message);
			if (logControl.enable)
			{
			}
			isConnected = false;
			isReady = false;
			isLogined = false;
			object netMsgQueue = _netMsgQueue;
			lock (netMsgQueue)
			{
				_netMsgQueue.Enqueue(new FK3_NetMessage("netDown", new object[1]
				{
					ex
				}));
			}
		}

		public void Connect(string ip, int port)
		{
			if (logControl.enable)
			{
				UnityEngine.Debug.Log("======开始连接======");
			}
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
				object netMsgQueue = _netMsgQueue;
				lock (netMsgQueue)
				{
					_netMsgQueue.Enqueue(new FK3_NetMessage("netConnected", new object[0]));
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("==错误: " + ex.Message);
				_handleException(ex);
			}
		}

		public void SendPublicKey()
		{
			string[] array = new string[2];
			array = _pCrypto.NetConnectGetRsaKey();
			try
			{
				UnityEngine.Debug.LogError("秘钥: " + JsonMapper.ToJson(array));
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
			object[] args = new object[2]
			{
				array[0],
				array[1]
			};
			Send("userService/publicKey", args, check: false, secret: false);
		}

		public void SendVersion()
		{
			Send("userService/checkVersion", new object[1]
			{
				FK3_GVars.versionCode
			}, check: false, secret: false);
		}

		public IEnumerator KeepHeart()
		{
			yield return new WaitForSeconds(3.5f);
			while (true)
			{
				if (isConnected && isReady)
				{
					Send("userService/heart", new object[0]);
					isHeartPong = false;
				}
				yield return new WaitForSeconds(5f);
			}
		}

		public void Handle_sendServerTime(object[] args)
		{
			if (logControl.enable)
			{
				UnityEngine.Debug.Log("Handle_sendServerTime");
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary = (args[0] as Dictionary<string, object>);
			long serverTime = Convert.ToInt64(dictionary["time"]);
			string str = (string)dictionary["key"];
			_pCrypto.setServerTime(serverTime);
			_pCrypto.DecryptKey(str);
			isReady = true;
		}

		protected void _handleData(byte[] msgBuffer, int packetId, int endIndex)
		{
			if (_pCrypto.GetKey() != "none")
			{
				msgBuffer = _pCrypto.Decrypt(msgBuffer);
			}
			Hashtable hashtable = new Hashtable();
			bool flag = false;
			string @string;
			try
			{
				@string = Encoding.UTF8.GetString(msgBuffer);
				hashtable = JsonFx.Json.JsonReader.Deserialize<Hashtable>(@string);
			}
			catch (Exception)
			{
				byte[] bytes = _pCrypto.DecompressBytes(msgBuffer);
				@string = Encoding.UTF8.GetString(bytes);
				hashtable = JsonFx.Json.JsonReader.Deserialize<Hashtable>(@string);
				flag = true;
			}
			string text = hashtable["method"].ToString();
			if (flag)
			{
				UnityEngine.Debug.Log($"compressed method:{text}");
			}
			object[] array = hashtable["args"] as object[];
			connectTimeCount = 0f;
			if (text == "publicKey")
			{
				if (Application.platform == RuntimePlatform.WindowsEditor)
				{
					try
					{
						UnityEngine.Debug.LogError("args: " + JsonMapper.ToJson(array));
					}
					catch (Exception arg)
					{
						UnityEngine.Debug.LogError("错误: " + arg);
					}
				}
				Handle_sendServerTime(array);
				return;
			}
			if (text == "heart")
			{
				isHeartPong = true;
				return;
			}
			object netMsgQueue = _netMsgQueue;
			lock (netMsgQueue)
			{
				FK3_NetMessage fK3_NetMessage = new FK3_NetMessage(text, array);
				fK3_NetMessage.jsonString = @string;
				fK3_NetMessage.packetId = packetId;
				fK3_NetMessage.endIndex = endIndex;
				if (isApplicationPause && (fK3_NetMessage.method == "newFishPush" || fK3_NetMessage.method == "newFishGroupPush" || fK3_NetMessage.method == "newFishFormationPush" || fK3_NetMessage.method == "newKingCrabBossPush" || fK3_NetMessage.method == "newDeepSeaOctopusBossPush"))
				{
					if (fK3_NetMessage.method != "newKingCrabBossPush" && fK3_NetMessage.method != "newDeepSeaOctopusBossPush")
					{
						_netMsgQueueFishBirthPause.Enqueue(fK3_NetMessage);
					}
				}
				else
				{
					_netMsgQueue.Enqueue(fK3_NetMessage);
				}
			}
			if (logControl.recvBinLog)
			{
				PrintBin(msgBuffer, string.Empty);
			}
		}

		private IEnumerator _checkTimeout(int id, int timeout, FK3_ClientSocket cs)
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Aqua("_checkTimeout start id: {0}", id));
			yield return new WaitForSeconds((float)timeout / 1000f);
			bool isTimeout = false;
			if (cs != null && cs.doneList.Count > id && !cs.doneList[id])
			{
				object netMsgQueue = _netMsgQueue;
				lock (netMsgQueue)
				{
					_netMsgQueue.Enqueue(new FK3_NetMessage("timeout", new object[1]
					{
						id
					}));
					isTimeout = true;
				}
			}
			if (isTimeout)
			{
				UnityEngine.Debug.Log(FK3_LogHelper.Aqua("_checkTimeout start id: {0} timeout: {1}", id, isTimeout));
			}
		}

		private IEnumerator _checkHeartTimeout(int timeout)
		{
			yield return new WaitForSeconds((float)timeout / 1000f);
			bool isTimeout = false;
			if (isReady && !isHeartPong)
			{
				object netMsgQueue = _netMsgQueue;
				lock (netMsgQueue)
				{
					isConnected = false;
					isReady = false;
					isLogined = false;
					_netMsgQueue.Enqueue(new FK3_NetMessage("heart timeout", new object[0]));
					isTimeout = true;
				}
			}
			if (isTimeout && logControl.enable)
			{
				UnityEngine.Debug.Log(FK3_LogHelper.Aqua("_checkHeartTimeout is timeout"));
			}
		}

		public void _checkTimeoutStart(int id, int timeout, FK3_ClientSocket cs)
		{
			StartCoroutine(_checkTimeout(id, timeout, cs));
		}

		public void ClearNetMsgQueue()
		{
			_netMsgQueue.Clear();
		}

		public void RegisterHandler(string method, NetMessageHandler hander)
		{
			if (!_netMsgHandlerDic.ContainsKey(method))
			{
				NetMessageHandler value = hander.Invoke;
				_netMsgHandlerDic.Add(method, value);
				return;
			}
			_netMsgHandlerDic[method] = hander;
			if (!logControl.enable)
			{
			}
		}

		public bool IsMethodRegistered(string method)
		{
			return _netMsgHandlerDic.ContainsKey(method);
		}

		public void Send(string method, object[] args, bool check = true, bool secret = true)
		{
			if (method == "userService/heart")
			{
				StartCoroutine(_checkHeartTimeout(3000));
			}
			if (dropSend && method != "userService/heart")
			{
				if (logControl.enable)
				{
					UnityEngine.Debug.Log(FK3_LogHelper.Magenta("drop send >>> method: {0}", method));
				}
			}
			else if ((!check || (isLogined && isReady)) && !useFake)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add("method", method);
				hashtable.Add("args", args);
				hashtable.Add("version", m_version);
				hashtable.Add("time", _pCrypto.GetUnixTime());
				string s = JsonFx.Json.JsonWriter.Serialize(hashtable);
				try
				{
					byte[] array = Encoding.UTF8.GetBytes(s);
					if (!logControl.send || logControl.heart || method != "userService/heart")
					{
					}
					if (useCrypto && _pCrypto.GetKey() != "none" && secret)
					{
						array = _pCrypto.Encrypt(array);
					}
					_pClient.Send(array);
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("发送错误: " + arg);
				}
			}
		}

		public bool CanSend()
		{
			return isReady && isLogined;
		}

		private void Update()
		{
			connectTimeCount += Time.deltaTime;
			Dispatching();
			if (Application.isEditor && UnityEngine.Input.GetKey(KeyCode.LeftShift) && UnityEngine.Input.GetKeyDown(KeyCode.N))
			{
				UnityEngine.Debug.Log("isEditor:" + Application.isEditor);
				UnityEngine.Debug.Log(FK3_LogHelper.Red("force netdown"));
				FK3_MB_Singleton<FK3_NetManager>.Get().Disconnect();
			}
		}

		private void Dispatching()
		{
			FK3_NetMessage netMsg;
			while (GetNetMessage(out netMsg))
			{
				DispatchNetMessage(netMsg);
			}
		}

		private bool GetNetMessage(out FK3_NetMessage netMsg)
		{
			object netMsgQueue = _netMsgQueue;
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

		private void DispatchNetMessage(FK3_NetMessage netMsg)
		{
			if (!netMsg.method.Equals("newFishPush") && !netMsg.method.Equals("newFishPufferPush"))
			{
				if (logControl.recvPacketInfo)
				{
				}
				if (netMsg.jsonString != null && netMsg.jsonString.Length > 0 && !logControl.recv)
				{
				}
			}
			if (dropDispatch)
			{
				if (logControl.enable)
				{
					UnityEngine.Debug.Log(FK3_LogHelper.Magenta("drop dispatch <<< method: {0}", netMsg.method));
				}
			}
			else if (_netMsgHandlerDic.ContainsKey(netMsg.method))
			{
				_netMsgHandlerDic[netMsg.method](netMsg.args);
			}
			else if (logControl.enable)
			{
				UnityEngine.Debug.LogError("消息 > 未知消息: " + netMsg.method);
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (FK3_GVars.m_curState != FK3_Demo_UI_State.InGame)
			{
				return;
			}
			if (pauseStatus)
			{
				isApplicationPause = true;
				lastTime = Time.realtimeSinceStartup;
				return;
			}
			isApplicationPause = false;
			if (Time.realtimeSinceStartup - lastTime < 5f)
			{
				FK3_MessageCenter.SendMessage("SwithInCleanSceneShort", null);
				while (_netMsgQueueFishBirthPause.Count > 0)
				{
					object netMsgQueue = _netMsgQueue;
					lock (netMsgQueue)
					{
						FK3_NetMessage item = _netMsgQueueFishBirthPause.Dequeue();
						_netMsgQueue.Enqueue(item);
					}
				}
			}
			else
			{
				FK3_MessageCenter.SendMessage("SwithInCleanSceneLong", null);
				FK3_MB_Singleton<FK3_SynchronizeHint>.Get().Show();
			}
		}

		public void OnDestroy()
		{
			if (logControl.enable)
			{
			}
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

		private void PrintBin(byte[] bytes, string head = "")
		{
		}

		private bool FilterSendMsgLog(string method)
		{
			return filterSendMsgLogFunc == null || filterSendMsgLogFunc(method);
		}

		private bool FilterRecvMsgLog(string method)
		{
			return filterRecvMsgLogFunc == null || filterSendMsgLogFunc(method);
		}
	}
}
