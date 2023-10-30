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
	public class HW2_AppManager : HW2_MB_Singleton<HW2_AppManager>
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
			if (HW2_MB_Singleton<HW2_AppManager>._instance == null)
			{
				HW2_MB_Singleton<HW2_AppManager>.SetInstance(this);
			}
		}

		private void Start()
		{
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("checkLogin", HandleNetMsg_Login);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("login", HandleNetMsg_Login);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("checkVersion", HandleNetMsg_CheckVersion);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("netDown", HandleNetMsg_NetDown);
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("timeout", delegate(object[] args)
			{
				UnityEngine.Debug.Log(HW2_LogHelper.Magenta("timeout>  id: " + (int)args[0]));
			});
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("heart timeout", delegate
			{
				UnityEngine.Debug.Log(HW2_LogHelper.Magenta("heart timeout"));
				HandleNetMsg_NetDown(new object[1]
				{
					new Exception("heart timeout")
				});
			});
		}

		public void StartApp()
		{
			PrepareInitData();
			StartConnect();
		}

		public void StartConnect()
		{
			_coLogin = StartCoroutine(_connectAndLoginCoroutine(willLogin: true));
		}

		private void PrepareInitData()
		{
			UnityEngine.Debug.LogError("获取大厅的账号密码");
			HW2_GVars.username = ZH2_GVars.username;
			HW2_GVars.pwd = ZH2_GVars.pwd;
			HW2_GVars.IPAddress = ZH2_GVars.IPAddress_Game;
			HW2_GVars.IPPort = 10100;
			HW2_GVars.language = "zh";
			HW2_GVars.versionCode = string.Empty;
		}

		private IEnumerator _connectAndLoginCoroutine(bool willLogin = false)
		{
			UnityEngine.Debug.LogError($"name:[{HW2_GVars.username}],pwd:[{HW2_GVars.pwd}],IPAddress:[{HW2_GVars.IPAddress}],IPPort:[{HW2_GVars.IPPort}],willLogin:[{willLogin}]");
			m_hasVersionChecked = false;
			if (!_hasLogined)
			{
				HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectMaxTimes = 10;
				HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectMaxTimeout = 15f;
			}
			if (!_isReconnecting)
			{
				UnityEngine.Debug.LogError("_isReconnecting: " + _isReconnecting);
				PrepareInitData();
			}
			else
			{
				HW2_MB_Singleton<HW2_NetManager>.GetInstance().Disconnect();
				yield return new WaitForSeconds(0.5f);
			}
			if (!_hasLogined)
			{
				UnityEngine.Debug.LogError("HW2_GVars.IPAddress: " + HW2_GVars.IPAddress);
				if (!CheckIP.CheckInput(HW2_GVars.IPAddress))
				{
					UnityEngine.Debug.LogError("检查IP失败!!");
					HW2_AlertDialog.Get().ShowDialog("IP非法!!将终止程序!!");
					yield return new WaitForSeconds(0.5f);
					Application.Quit();
					yield break;
				}
				UnityEngine.Debug.Log("zhengchangIP");
				HW2_GVars.IPAddress = CheckIP.DoGetHostAddresses(HW2_GVars.IPAddress);
				UnityEngine.Debug.LogError(HW2_GVars.IPAddress);
			}
			HW2_MB_Singleton<HW2_NetManager>.GetInstance().Connect(HW2_GVars.IPAddress, HW2_GVars.IPPort);
			UnityEngine.Debug.Log("wait connected");
			yield return new WaitUntil(() => HW2_MB_Singleton<HW2_NetManager>.Get().isConnected);
			HW2_MB_Singleton<HW2_NetManager>.Get().SendVersion();
			yield return new WaitUntil(() => HW2_MB_Singleton<HW2_NetManager>.Get().isReady);
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
			if (HW2_MB_Singleton<ReconnectHint>.GetInstance() != null)
			{
				HW2_MB_Singleton<ReconnectHint>.GetInstance().Hide();
			}
			HW2_MB_Singleton<HW2_NetManager>.Get().Disconnect();
			HW2_MB_Singleton<HW2_NetManager>.Get().ClearNetMsgQueue();
			HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("netDown", delegate
			{
				UnityEngine.Debug.Log("clear netDown to PrepareQuitGame");
			});
		}

		private IEnumerator _loadingLoginTimeout()
		{
			yield return new WaitForSeconds(5f);
			if (!_hasLogined && !HW2_MB_Singleton<HW2_NetManager>.GetInstance().useFake)
			{
				UnityEngine.Debug.Log(HW2_LogHelper.Magenta("_loadingLoginTimeout"));
				HandleNetMsg_NetDown(new object[1]
				{
					new Exception("loading timeout")
				});
			}
		}

		private void Send_ReLogin()
		{
			UnityEngine.Debug.Log($"m_curState:[{(int)HW2_GVars.m_curState}]");
			HW2_MB_Singleton<HW2_NetManager>.GetInstance().Send("userService/login", new object[3]
			{
				HW2_GVars.username,
				HW2_GVars.pwd,
				(int)HW2_GVars.m_curState
			}, check: false);
		}

		private void Send_Login()
		{
			object[] args = new object[3]
			{
				HW2_GVars.username,
				HW2_GVars.pwd,
				1
			};
			HW2_MB_Singleton<HW2_NetManager>.GetInstance().Send("userService/login", args, check: false);
		}

		private void HandleNetMsg_CheckVersion(object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			string text = (string)dictionary["path"];
			UnityEngine.Debug.Log("path:" + text);
			if (text == string.Empty)
			{
				HW2_MB_Singleton<HW2_NetManager>.Get().SendPublicKey();
			}
		}

		private void HandleNetMsg_Login(object[] args)
		{
			UnityEngine.Debug.LogError("NetMsg_Login: " + JsonMapper.ToJson(args));
			HW2_GVars.lockRelogin = false;
			NetMsgInfo_RecvLogin netMsgInfo_RecvLogin = new NetMsgInfo_RecvLogin(args);
			netMsgInfo_RecvLogin.Parse();
			netMsgInfo_RecvLogin.user.CopyTo(HW2_MB_Singleton<GameUIController>.Get().myLobbyContext.user);
			if (netMsgInfo_RecvLogin.valid && netMsgInfo_RecvLogin.code == 0)
			{
				HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectMaxTimes = 50;
				HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectMaxTimeout = 30f;
				HW2_MB_Singleton<HW2_NetManager>.GetInstance().isLogined = true;
				HW2_MB_Singleton<HW2_NetManager>.GetInstance().SendHeartBeat();
				if (!_hasLogined)
				{
					_hasLogined = true;
				}
				else
				{
					_isReconnecting = false;
					HW2_GVars.lockReconnect = false;
				}
				UnityEngine.Debug.LogError("info.isShutup:" + netMsgInfo_RecvLogin.isShutup);
				HW2_GVars.isShutup = netMsgInfo_RecvLogin.isShutup;
				HW2_GVars.isUserShutup = (netMsgInfo_RecvLogin.user.shutupStatus != 0);
				HW2_GVars.SetUserInfo(netMsgInfo_RecvLogin.user);
				if (HW2_GVars.m_curState == Demo_UI_State.StartupLoading)
				{
					HW2_MB_Singleton<GameUIController>.Get().RefreshGameUI(Demo_UI_State.RoomSelection);
				}
				ReconnectSuccess();
			}
			else if (netMsgInfo_RecvLogin.code == 30)
			{
				HW2_AlertDialog.Get().ShowDialog("错误的版本号");
			}
			else
			{
				HW2_AlertDialog.Get().ShowDialog(netMsgInfo_RecvLogin.message);
			}
		}

		private void HandleNetMsg_NetDown(object[] args)
		{
			UnityEngine.Debug.LogError("HandleNetMsg_NetDown@" + DateTime.Now);
			UnityEngine.Debug.Log($"isReady:{HW2_MB_Singleton<HW2_NetManager>.GetInstance().isReady},connectCount: {HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectCount}");
			if (HW2_MB_Singleton<HW2_NetManager>.GetInstance().isReady)
			{
				return;
			}
			if (HW2_GVars.m_curState == Demo_UI_State.InGame && !isProcessed)
			{
				UnityEngine.Debug.Log("InGame_NetDown_Handler");
				if (InGame_NetDown_Handler != null)
				{
					InGame_NetDown_Handler();
				}
			}
			if (HW2_GVars.lockReconnect)
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
			UnityEngine.Debug.LogError("ex: " + ex);
			bool flag = false;
			if (HW2_GVars.m_curState == Demo_UI_State.StartupLoading && flag)
			{
				string content = string.Format((HW2_GVars.language == "zh") ? "网络连接已断开，错误代码为 [{0}]" : "Net Error [{0}]", (ex == null) ? "9999" : ex.Message);
				HW2_AlertDialog.GetInstance().ShowDialog(content, showOkCancel: false, delegate
				{
					HW2_MB_Singleton<GameUIController>.Get().QuitToLogin();
				});
				return;
			}
			if (_coLogin != null)
			{
				StopCoroutine(_coLogin);
				_coLogin = null;
			}
			if ((HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectCount > HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectMaxTimes && HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectTimeCount > HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectMaxTimeout / 2f) || HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectTimeCount > HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectMaxTimeout)
			{
				_timeoutQuit();
				return;
			}
			_coLogin = StartCoroutine(_connectAndLoginCoroutine(!HW2_GVars.lockRelogin && _hasLogined));
			if (HW2_MB_Singleton<ReconnectHint>.Get() != null)
			{
				HW2_MB_Singleton<ReconnectHint>.GetInstance().Show();
			}
			if (HW2_MB_Singleton<SynchronizeHint>.Get() != null)
			{
				HW2_MB_Singleton<SynchronizeHint>.Get().Hide();
			}
		}

		private void ReconnectSuccess()
		{
			UnityEngine.Debug.Log("ReconnectSuccess");
			if (HW2_MB_Singleton<ReconnectHint>.GetInstance() != null)
			{
				HW2_MB_Singleton<ReconnectHint>.GetInstance().Hide();
			}
			_isReconnecting = false;
			if (_coLogin != null)
			{
				StopCoroutine(_coLogin);
				_coLogin = null;
			}
			isProcessed = false;
			if (HW2_GVars.m_curState == Demo_UI_State.InGame && InGame_ReconnectSuccess_Handler != null)
			{
				InGame_ReconnectSuccess_Handler();
			}
		}

		public void _timeoutQuit()
		{
			UnityEngine.Debug.Log($"connectCount: {HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectCount}, connectTimeCount: {HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectTimeCount}");
			UnityEngine.Debug.Log($"connectMaxTimes: {HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectMaxTimes}, connectMaxTimeout: {HW2_MB_Singleton<HW2_NetManager>.GetInstance().connectMaxTimeout}");
			string content = (HW2_GVars.language == "zh") ? "网络异常，请检查网络连接" : "Unable to connect to server，Please check your network connection";
			HW2_GVars.lockReconnect = true;
			PrepareQuitGame();
			try
			{
				if (HW2_AlertDialog.GetInstance() == null)
				{
					UnityEngine.Debug.LogError("HW2_AlertDialog为空");
					UIRoomManager.GetInstance().OpenUI("AlertDialog");
				}
				HW2_AlertDialog.GetInstance().ShowDialog(content, showOkCancel: false, delegate
				{
					UnityEngine.Debug.LogError("返回登录");
					HW2_MB_Singleton<GameUIController>.Get().QuitToLogin();
				});
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}
	}
}
