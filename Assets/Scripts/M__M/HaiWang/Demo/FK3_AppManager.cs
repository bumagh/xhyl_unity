using JsonFx.Json;
using LitJson;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.NetMsgDefine;
using M__M.HaiWang.UIDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Demo
{
	public class FK3_AppManager : FK3_MB_Singleton<FK3_AppManager>
	{
		private bool _hasLogined;

		private bool m_hasVersionChecked;

		private Coroutine _coLogin;

		public Action InGame_NetDown_Handler;

		public Action InGame_ReconnectSuccess_Handler;

		private bool isProcessed;

		private bool _isReconnecting;

		private void Awake()
		{
			if (FK3_MB_Singleton<FK3_AppManager>._instance == null)
			{
				FK3_MB_Singleton<FK3_AppManager>.SetInstance(this);
			}
		}

		private void Start()
		{
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("checkLogin", HandleNetMsg_Login);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("login", HandleNetMsg_Login);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("checkVersion", HandleNetMsg_CheckVersion);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("netDown", HandleNetMsg_NetDown);
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("timeout", delegate(object[] args)
			{
				UnityEngine.Debug.Log(FK3_LogHelper.Magenta("timeout>  id: " + (int)args[0]));
			});
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("heart timeout", delegate
			{
				UnityEngine.Debug.Log(FK3_LogHelper.Magenta("heart timeout"));
				HandleNetMsg_NetDown(new object[1]
				{
					new Exception("heart timeout")
				});
			});
		}

		public void StartApp()
		{
			UnityEngine.Debug.LogError("========StartApp======");
			PrepareInitData();
			StartConnect();
		}

		public void StartConnect()
		{
			_coLogin = StartCoroutine(_connectAndLoginCoroutine(willLogin: true));
		}

		private void PrepareInitData()
		{
			FK3_GVars.username = ZH2_GVars.username;
			FK3_GVars.pwd = ZH2_GVars.pwd;
			FK3_GVars.IPAddress = ZH2_GVars.IPAddress_Game;
			FK3_GVars.IPPort = 30100;
			FK3_GVars.language = ((ZH2_GVars.language_enum != 0) ? "en" : "zh");
			FK3_GVars.versionCode = string.Empty;
			UnityEngine.Debug.LogError("获取大厅的账号密码: " + ZH2_GVars.username + "===" + ZH2_GVars.pwd + "===" + ZH2_GVars.IPAddress_Game + ":30100");
		}

		private IEnumerator _connectAndLoginCoroutine(bool willLogin = false)
		{
			m_hasVersionChecked = false;
			if (!_hasLogined)
			{
				FK3_MB_Singleton<FK3_NetManager>.GetInstance().connectMaxTimes = 10;
				FK3_MB_Singleton<FK3_NetManager>.GetInstance().connectMaxTimeout = 15f;
			}
			if (!_isReconnecting)
			{
				PrepareInitData();
			}
			else
			{
				FK3_MB_Singleton<FK3_NetManager>.GetInstance().Disconnect();
				yield return new WaitForSeconds(0.5f);
			}
			if (!_hasLogined)
			{
				FK3_GVars.IPAddress = FK3_CheckIP.DoGetHostAddresses(FK3_GVars.IPAddress);
			}
			UnityEngine.Debug.LogError($"==========connectAndLoginCoroutine: {FK3_GVars.IPAddress}:{FK3_GVars.IPPort}");
			FK3_MB_Singleton<FK3_NetManager>.GetInstance().Connect(FK3_GVars.IPAddress, FK3_GVars.IPPort);
			yield return new WaitUntil(() => FK3_MB_Singleton<FK3_NetManager>.Get().isConnected);
			FK3_MB_Singleton<FK3_NetManager>.Get().SendVersion();
			yield return new WaitUntil(() => FK3_MB_Singleton<FK3_NetManager>.Get().isReady);
			float waitTime = _isReconnecting ? 0.2f : 0.2f;
			yield return new WaitForSeconds(waitTime);
			if (_hasLogined)
			{
				Send_ReLogin();
			}
			else
			{
				Send_Login();
				yield return null;
			}
			if (!_isReconnecting)
			{
				StartCoroutine(_loadingLoginTimeout());
			}
			_isReconnecting = false;
		}

		public void PrepareQuitGame()
		{
			if (_coLogin != null)
			{
				StopCoroutine(_coLogin);
				_coLogin = null;
			}
			if (FK3_MB_Singleton<FK3_ReconnectHint>.GetInstance() != null)
			{
				FK3_MB_Singleton<FK3_ReconnectHint>.GetInstance().Hide();
			}
			FK3_MB_Singleton<FK3_NetManager>.Get().Disconnect();
			FK3_MB_Singleton<FK3_NetManager>.Get().ClearNetMsgQueue();
			FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("netDown", delegate
			{
				UnityEngine.Debug.Log("clear netDown to PrepareQuitGame");
			});
		}

		private IEnumerator _loadingLoginTimeout()
		{
			yield return new WaitForSeconds(5f);
			if (!_hasLogined && !FK3_MB_Singleton<FK3_NetManager>.GetInstance().useFake)
			{
				UnityEngine.Debug.Log(FK3_LogHelper.Magenta("_loadingLoginTimeout"));
				HandleNetMsg_NetDown(new object[1]
				{
					new Exception("loading timeout")
				});
			}
		}

		private void Send_ReLogin()
		{
			UnityEngine.Debug.Log($"m_curState:[{(int)FK3_GVars.m_curState}]");
			FK3_MB_Singleton<FK3_NetManager>.GetInstance().Send("userService/login", new object[3]
			{
				FK3_GVars.username,
				FK3_GVars.pwd,
				(int)FK3_GVars.m_curState
			}, check: false);
		}

		private void Send_Login()
		{
			object[] args = new object[3]
			{
				FK3_GVars.username,
				FK3_GVars.pwd,
				1
			};
			FK3_MB_Singleton<FK3_NetManager>.GetInstance().Send("userService/login", args, check: false);
		}

		private void HandleNetMsg_CheckVersion(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			string text = (string)dictionary["path"];
			if (text == string.Empty)
			{
				FK3_MB_Singleton<FK3_NetManager>.Get().SendPublicKey();
			}
			else
			{
				FK3_IOSGameStart.GetSingleton().UpdateGameVesion(text);
			}
		}

		private void HandleNetMsg_Login(object[] args)
		{
			try
			{
				JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
				ZH2_GVars.hallInfo2 = new JsonData();
				ZH2_GVars.hallInfo2 = jsonData["hallInfo"];
				UnityEngine.Debug.LogError("hallInfo: " + ZH2_GVars.hallInfo2.ToJson());
				FK3_MB_Singleton<FK3_GameUIController>.Get().ShowHall();
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("es: " + arg);
			}
			FK3_GVars.lockRelogin = false;
			FK3_NetMsgInfo_RecvLogin fK3_NetMsgInfo_RecvLogin = new FK3_NetMsgInfo_RecvLogin(args);
			fK3_NetMsgInfo_RecvLogin.Parse();
			fK3_NetMsgInfo_RecvLogin.user.CopyTo(FK3_MB_Singleton<FK3_GameUIController>.Get().myLobbyContext.user);
			if (fK3_NetMsgInfo_RecvLogin.valid && fK3_NetMsgInfo_RecvLogin.code == 0)
			{
				FK3_MB_Singleton<FK3_NetManager>.GetInstance().connectMaxTimes = 50;
				FK3_MB_Singleton<FK3_NetManager>.GetInstance().connectMaxTimeout = 30f;
				FK3_MB_Singleton<FK3_NetManager>.GetInstance().isLogined = true;
				FK3_MB_Singleton<FK3_NetManager>.GetInstance().SendHeartBeat();
				if (!_hasLogined)
				{
					_hasLogined = true;
				}
				else
				{
					_isReconnecting = false;
					FK3_GVars.lockReconnect = false;
				}
				FK3_GVars.isShutup = fK3_NetMsgInfo_RecvLogin.isShutup;
				FK3_GVars.isUserShutup = (fK3_NetMsgInfo_RecvLogin.user.shutupStatus != 0);
				FK3_GVars.SetUserInfo(fK3_NetMsgInfo_RecvLogin.user);
				if (FK3_GVars.m_curState == FK3_Demo_UI_State.StartupLoading)
				{
					FK3_MB_Singleton<FK3_GameUIController>.Get().RefreshGameUI(FK3_Demo_UI_State.RoomSelection);
				}
				ReconnectSuccess();
			}
			else if (fK3_NetMsgInfo_RecvLogin.code == 30)
			{
				FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("错误的版本号", "Incorrect version number", string.Empty));
			}
			else
			{
				FK3_AlertDialog.Get().ShowDialog(fK3_NetMsgInfo_RecvLogin.message);
			}
		}

		private void HandleNetMsg_NetDown(object[] args)
		{
			if (FK3_MB_Singleton<FK3_NetManager>.GetInstance().isReady)
			{
				return;
			}
			if (FK3_GVars.m_curState == FK3_Demo_UI_State.InGame && !isProcessed)
			{
				UnityEngine.Debug.Log("InGame_NetDown_Handler");
				if (InGame_NetDown_Handler != null)
				{
					InGame_NetDown_Handler();
				}
			}
			if (FK3_GVars.lockReconnect)
			{
				UnityEngine.Debug.Log("lockReconnect");
				return;
			}
			Exception ex = null;
			if (args != null)
			{
				ex = (args[0] as Exception);
			}
			_isReconnecting = true;
			UnityEngine.Debug.LogError("ex: " + ex.Message);
			bool flag = false;
			if (FK3_GVars.m_curState == FK3_Demo_UI_State.StartupLoading && flag)
			{
				string content = string.Format((FK3_GVars.language == "zh") ? "网络连接已断开，错误代码为 [{0}]" : "Net Error [{0}]", (ex == null) ? "9999" : ex.Message);
				FK3_AlertDialog.GetInstance().ShowDialog(content, showOkCancel: false, delegate
				{
					FK3_MB_Singleton<FK3_GameUIController>.Get().QuitToLogin();
				});
				return;
			}
			if (_coLogin != null)
			{
				StopCoroutine(_coLogin);
				_coLogin = null;
			}
			if ((FK3_MB_Singleton<FK3_NetManager>.GetInstance().connectCount > FK3_MB_Singleton<FK3_NetManager>.GetInstance().connectMaxTimes && FK3_MB_Singleton<FK3_NetManager>.GetInstance().connectTimeCount > FK3_MB_Singleton<FK3_NetManager>.GetInstance().connectMaxTimeout / 2f) || FK3_MB_Singleton<FK3_NetManager>.GetInstance().connectTimeCount > FK3_MB_Singleton<FK3_NetManager>.GetInstance().connectMaxTimeout)
			{
				_timeoutQuit();
				return;
			}
			_coLogin = StartCoroutine(_connectAndLoginCoroutine(!FK3_GVars.lockRelogin && _hasLogined));
			if (FK3_MB_Singleton<FK3_ReconnectHint>.Get() != null)
			{
				FK3_MB_Singleton<FK3_ReconnectHint>.GetInstance().Show();
			}
			if (FK3_MB_Singleton<FK3_SynchronizeHint>.Get() != null)
			{
				FK3_MB_Singleton<FK3_SynchronizeHint>.Get().Hide();
			}
		}

		private void ReconnectSuccess()
		{
			if (FK3_MB_Singleton<FK3_ReconnectHint>.GetInstance() != null)
			{
				FK3_MB_Singleton<FK3_ReconnectHint>.GetInstance().Hide();
			}
			_isReconnecting = false;
			if (_coLogin != null)
			{
				StopCoroutine(_coLogin);
				_coLogin = null;
			}
			isProcessed = false;
			if (FK3_GVars.m_curState == FK3_Demo_UI_State.InGame && InGame_ReconnectSuccess_Handler != null)
			{
				InGame_ReconnectSuccess_Handler();
			}
		}

		public void _timeoutQuit()
		{
			string content = (FK3_GVars.language == "zh") ? "网络异常，请检查网络连接" : "Unable to connect to server，Please check your network connection";
			FK3_GVars.lockReconnect = true;
			PrepareQuitGame();
			try
			{
				if (FK3_AlertDialog.GetInstance() == null)
				{
					UnityEngine.Debug.LogError("HW2_AlertDialog为空");
					FK3_UIRoomManager.GetInstance().OpenUI("AlertDialog");
				}
				FK3_AlertDialog.GetInstance().ShowDialog(content, showOkCancel: false, delegate
				{
					UnityEngine.Debug.LogError("返回登录");
					FK3_MB_Singleton<FK3_GameUIController>.Get().QuitToLogin();
				});
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}
	}
}
