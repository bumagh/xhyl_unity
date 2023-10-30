using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DCDF_GameManager : DCDF_MB_Singleton<DCDF_GameManager>
{
	private bool _isReconnecting;

	private Coroutine _coLogin;

	private Coroutine _coLoading;

	private AudioListener _audio;

	private bool _hasLogined;

	private Coroutine _coKeepHeart;

	private bool m_hasVersionChecked;

	private void Awake()
	{
		_isReconnecting = false;
		if (DCDF_MB_Singleton<DCDF_GameManager>._instance != null)
		{
			DCDF_MB_Singleton<DCDF_GameManager>.GetInstance().InitSingletonStep2();
			DCDF_MB_Singleton<DCDF_GameManager>.GetInstance().HideAllViewButLoading();
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
		else if (DCDF_MB_Singleton<DCDF_GameManager>._instance == null)
		{
			DCDF_MB_Singleton<DCDF_GameManager>.SetInstance(this);
			UnityEngine.Debug.Log("SceneManager.GetActiveScene().name: " + SceneManager.GetActiveScene().name);
			PreInit();
			InitSingletonStep2();
		}
	}

	private void Start()
	{
		HideAllViewButLoading();
		DCDF_MB_Singleton<DCDF_LoadingViewController>.GetInstance().Hide();
		_init();
	}

	private void Update()
	{
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectTimeCount += Time.deltaTime;
		if (!(DCDF_MySqlConnection.curView == "LoadingView") && DCDF_MySqlConnection.lockOnePoint)
		{
			DCDF_MySqlConnection.lockOnePoint = false;
		}
	}

	public void PreInit()
	{
		_initSingletonStep1();
	}

	private void _init()
	{
		UnityEngine.Debug.Log("_init");
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("heart", delegate
		{
		});
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("netDown", Handle_NetDown);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("userLogin", HandleNetMsg_UserLogin);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("reLogin", HandleNetMsg_ReLogin);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("overflow", HandleNetMsg_Overflow);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("levelUp", HandleNetMsg_LevelUp);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("quitToLogin", HandleNetMsg_QuitToLogin);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("quitToRoom", HandleNetMsg_QuitToRoom);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("syncGold", HandleNetMsg_SyncGold);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("checkVersion", HandleNetMsg_CheckVersion);
		if (!DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().IsMethodRegistered("updateRoomInfo"))
		{
			DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("updateRoomInfo", delegate
			{
			});
		}
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("userAward", HandleNetMsg_UserAward);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("gameGold", HandleNetMsg_GameGold);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("sendNotice", HandleNetMsg_Notice);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("notice", HandleNetMsg_Notice);
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("timeout", delegate(object[] args)
		{
			UnityEngine.Debug.Log(DCDF_LogHelper.Magenta("timeout>  id: " + (int)args[0]));
		});
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("heart timeout", delegate
		{
			UnityEngine.Debug.Log(DCDF_LogHelper.Magenta("heart timeout"));
			Handle_NetDown(new object[1]
			{
				new Exception("heart timeout")
			});
		});
		DCDF_MB_Singleton<DCDF_LoadingViewController>.GetInstance().Show();
		_coLoading = StartCoroutine(DCDF_MB_Singleton<DCDF_LoadingViewController>.GetInstance().LoadingAni());
		Login();
	}

	public void Login()
	{
		_coLogin = StartCoroutine(_connectAndLoginCoroutine());
	}

	public void LogDeviceInfo()
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.internetReachability: " + Application.internetReachability));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.isEditor: " + Application.isEditor));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.isMobilePlatform: " + Application.isMobilePlatform));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.isPlaying: " + Application.isPlaying));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.persistentDataPath: " + Application.persistentDataPath));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.platform: " + Application.platform));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.productName: " + Application.productName));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.runInBackground: " + Application.runInBackground));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.streamingAssetsPath: " + Application.streamingAssetsPath));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.systemLanguage: " + Application.systemLanguage));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.targetFrameRate: " + Application.targetFrameRate));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.temporaryCachePath: " + Application.temporaryCachePath));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.unityVersion: " + Application.unityVersion));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.version: " + Application.version));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.isShowingSplashScreen: " + Application.isShowingSplashScreen));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Application.stackTraceLogType: " + Application.stackTraceLogType));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("SystemInfo.deviceModel: " + SystemInfo.deviceModel));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("SystemInfo.deviceName: " + SystemInfo.deviceName));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("SystemInfo.deviceType: " + SystemInfo.deviceType));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("SystemInfo.deviceUniqueIdentifier: " + SystemInfo.deviceUniqueIdentifier));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("SystemInfo.graphicsDeviceID: " + SystemInfo.graphicsDeviceID));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("SystemInfo.graphicsDeviceName: " + SystemInfo.graphicsDeviceName));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("SystemInfo.operatingSystem: " + SystemInfo.operatingSystem));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("SystemInfo.processorCount: " + SystemInfo.processorCount));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("SystemInfo.processorType: " + SystemInfo.processorType));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("SystemInfo.systemMemorySize: " + SystemInfo.systemMemorySize));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("SystemInfo.graphicsDeviceType: " + SystemInfo.graphicsDeviceType));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("SystemInfo.processorFrequency: " + SystemInfo.processorFrequency));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Screen.autorotateToLandscapeLeft: " + Screen.autorotateToLandscapeLeft));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Screen.autorotateToPortrait: " + Screen.autorotateToPortrait));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Screen.currentResolution: " + Screen.currentResolution));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Screen.dpi: " + Screen.dpi));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Screen.fullScreen: " + Screen.fullScreen));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Screen.height: " + Screen.height));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Screen.orientation: " + Screen.orientation));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Screen.resolutions: " + Screen.resolutions));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Screen.sleepTimeout: " + Screen.sleepTimeout));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Screen.width: " + Screen.width));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("QualitySettings.names: " + QualitySettings.names));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("QualitySettings.masterTextureLimit: " + QualitySettings.masterTextureLimit));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("QualitySettings.maximumLODLevel: " + QualitySettings.maximumLODLevel));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("QualitySettings.maxQueuedFrames: " + QualitySettings.maxQueuedFrames));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("QualitySettings.vSyncCount: " + QualitySettings.vSyncCount));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("QualitySettings.shadowDistance: " + QualitySettings.shadowDistance));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("QualitySettings.GetQualityLevel(): " + QualitySettings.GetQualityLevel()));
		UnityEngine.Debug.Log("这里被注释了");
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Input.stylusTouchSupported: " + Input.stylusTouchSupported));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Input.touchPressureSupported: " + Input.touchPressureSupported));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Input.touchSupported: " + Input.touchSupported));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Debug.developerConsoleVisible: " + UnityEngine.Debug.developerConsoleVisible));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Debug.isDebugBuild: " + UnityEngine.Debug.isDebugBuild));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Caching.expirationDelay: " + Caching.expirationDelay));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Caching.maximumAvailableDiskSpace: " + Caching.maximumAvailableDiskSpace));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Caching.ready: " + Caching.ready));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Caching.spaceFree: " + Caching.spaceFree));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Caching.spaceOccupied: " + Caching.spaceOccupied));
		UnityEngine.Debug.Log(DCDF_LogHelper.Orange("Display.touchSupported.Length: " + Display.displays.Length));
	}

	public void HideAllViewButLoading()
	{
		UnityEngine.Debug.Log("HideAllViewButLoading");
		DCDF_MB_Singleton<DCDF_LobbyViewController>.GetInstance().Hide();
		DCDF_MB_Singleton<DCDF_SettingsController>.GetInstance().Hide();
		DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance().Hide();
		DCDF_MB_Singleton<DCDF_NoticeController>.GetInstance().Hide();
		DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().Hide();
	}

	private void _initSingletonStep1()
	{
		UnityEngine.Debug.Log("_intSingletonStep1");
		Transform transform = GameObject.Find("Canvas").transform;
		DCDF_MB_Singleton<DCDF_LoadingViewController>.SetInstanceByGameObject(transform.Find("Mask/Loading").gameObject).PreInit();
		DCDF_MB_Singleton<DCDF_AlertDialog>.SetInstanceByGameObject(transform.Find("Mask/AlertDialog").gameObject).PreInit();
		DCDF_MB_Singleton<DCDF_LoadingViewController>.GetInstance().loadingFinishAction += _onLoadingFinish;
	}

	public void InitSingletonStep2()
	{
		UnityEngine.Debug.Log("_intSingletonStep2");
		DCDF_SoundManager.Instance = GameObject.Find("SoundManager").GetComponent<DCDF_SoundManager>();
		DCDF_MB_Singleton<DCDF_LobbyViewController>.SetInstanceByGameObject(base.transform.Find("Mask/LobbyUI").gameObject).PreInit();
		DCDF_MB_Singleton<DCDF_SettingsController>.SetInstanceByGameObject(base.transform.Find("Mask/Setting").gameObject).PreInit();
		DCDF_MB_Singleton<DCDF_ReconnectHint>.SetInstanceByGameObject(base.transform.Find("Mask/ReconnectHint").gameObject).PreInit();
		DCDF_MB_Singleton<DCDF_NoticeController>.SetInstanceByGameObject(GameObject.Find("CanvasFront/Notice").gameObject).PreInit();
		DCDF_MB_Singleton<DCDF_SettingsController>.GetInstance().Init();
	}

	private void _prepareInitData()
	{
		UnityEngine.Debug.Log("Version: " + Application.version);
		DCDF_MySqlConnection.versionCode = "9.0.1";
		if (!DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().useFake)
		{
			DCDF_MySqlConnection.IPPort = 10016;
			UseDebugData();
		}
	}

	private void UseDebugData()
	{
		DCDF_MySqlConnection.username = "qwe123";
		DCDF_MySqlConnection.pwd = "qwe123";
		DCDF_MySqlConnection.IPAddress = "1.14.133.202";
		DCDF_MySqlConnection.IPPort = 10016;
	}

	private IEnumerator _connectAndLoginCoroutine()
	{
		m_hasVersionChecked = false;
		UnityEngine.Debug.LogError($"_isReconnecting: {_isReconnecting}, isConnected: {DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().isConnected}, isReady: {DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().isReady}");
		if (!_hasLogined)
		{
			DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectMaxTimes = 10;
			DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectMaxTimeout = 5f;
		}
		if (!_isReconnecting)
		{
			_prepareInitData();
		}
		else
		{
			DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().Disconnect();
			yield return new WaitForSeconds(0.5f);
		}
		UnityEngine.Debug.Log("ZH2_GVars.IPAddress: " + DCDF_MySqlConnection.IPAddress);
		if (!_hasLogined)
		{
			DCDF_MySqlConnection.IPAddress = DCDF_CheckIP.DoGetHostAddresses(DCDF_MySqlConnection.IPAddress);
		}
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().Connect(DCDF_MySqlConnection.IPAddress, DCDF_MySqlConnection.IPPort);
		UnityEngine.Debug.Log("wait connected");
		while (!DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().isConnected)
		{
			yield return null;
		}
		while (!m_hasVersionChecked)
		{
			yield return null;
		}
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().SendPublicKey();
		while (!DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().isReady)
		{
			yield return null;
		}
		float waitTime = _isReconnecting ? 0.2f : 0.2f;
		yield return new WaitForSeconds(waitTime);
		if (_hasLogined)
		{
			Send_ReLogin();
			yield break;
		}
		Send_Login();
		if (!_isReconnecting)
		{
			StartCoroutine(_loadingLoginTimeout());
		}
		yield return null;
	}

	private IEnumerator _loadingLoginTimeout()
	{
		yield return new WaitForSeconds(5f);
		if (!_hasLogined && !DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().useFake)
		{
			UnityEngine.Debug.Log(DCDF_LogHelper.Magenta("_loadingLoginTimeout"));
			_timeoutQuit();
		}
	}

	public void QuitToHallGame()
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.Magenta("QuitToHallGame"));
		Send_QuitGame();
	}

	public void QuitToHallLogin()
	{
		Send_QuitGame();
	}

	public void OverflowProcess()
	{
		string content = (DCDF_MySqlConnection.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge";
		PrepareQuitGame();
		DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			QuitToHallGame();
		});
	}

	public void Send_Login()
	{
		object[] args = new object[2]
		{
			DCDF_MySqlConnection.username,
			DCDF_MySqlConnection.pwd
		};
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().Send("userService/userLogin", args);
	}

	public void Send_QuitGame()
	{
		object[] args = new object[0];
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().Send("userService/quitGame", args);
	}

	public void HandleNetMsg_ReLogin(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string[] array2;
		if (DCDF_MySqlConnection.language == "zh")
		{
			string[] array = new string[5]
			{
				"网络异常，请重新进入",
				"没有此用户名",
				"密码错误",
				"账号被封",
				null
			};
			array2 = array;
			array[4] = "系统维护中";
		}
		else
		{
			string[] array3 = new string[5]
			{
				"Network error, please re-enter",
				"No user name",
				"Password error",
				"Account closure",
				null
			};
			array2 = array3;
			array3[4] = "System maintenance";
		}
		string[] array4 = array2;
		if (!(bool)dictionary["success"])
		{
			int num = (int)dictionary["msgCode"];
			int num2 = 0;
			switch (num)
			{
			case 0:
				num2 = 0;
				break;
			case 2:
				num2 = 1;
				break;
			case 4:
				num2 = 2;
				break;
			case 6:
				num2 = 3;
				break;
			case 7:
				num2 = 4;
				break;
			}
			PrepareQuitGame();
			DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog(array4[num2], showOkCancel: false, delegate
			{
				QuitToHallGame();
			});
			return;
		}
		if (!(bool)dictionary["reLogin"])
		{
			int num3 = (int)dictionary["msgCode"];
			int num4 = 0;
			switch (num3)
			{
			case 0:
				num4 = 0;
				break;
			case 2:
				num4 = 1;
				break;
			case 4:
				num4 = 2;
				break;
			case 6:
				num4 = 3;
				break;
			case 7:
				num4 = 4;
				break;
			}
			PrepareQuitGame();
			DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog(array4[num4], showOkCancel: false, delegate
			{
				QuitToHallGame();
			});
			return;
		}
		int num5 = (int)dictionary["uiid"];
		UnityEngine.Debug.Log("uiid: " + num5);
		if (num5 == 1)
		{
			DCDF_MySqlConnection.user = DCDF_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
		}
		if (num5 == 2)
		{
			object[] array5 = (object[])dictionary["roomInfo"];
			int num6 = array5.Length;
			DCDF_Desk[] array6 = new DCDF_Desk[num6];
			for (int i = 0; i < num6; i++)
			{
				array6[i] = DCDF_Desk.CreateWithDic((Dictionary<string, object>)array5[i]);
			}
		}
		if (num5 == 3)
		{
			Dictionary<string, object> dictionary2 = dictionary["deskInfo"] as Dictionary<string, object>;
			int num7 = (int)dictionary2["userScore"];
			int num8 = (int)dictionary2["userGold"];
		}
		if (DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance() != null)
		{
			DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance().Hide();
		}
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().isLogined = true;
		if (_coKeepHeart != null)
		{
			StopCoroutine(_coKeepHeart);
		}
		_coKeepHeart = StartCoroutine(DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().KeepHeart());
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectCount = 0;
	}

	public void HandleNetMsg_UserLogin(object[] args)
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.NetHandle("HandleNetMsg_UserLogin"));
		_hasLogined = true;
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectMaxTimes = 50;
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectMaxTimeout = 30f;
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectMaxTimes = 30;
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectMaxTimeout = 20f;
		if (_coLogin != null)
		{
			StopCoroutine(_coLogin);
			_coLogin = null;
		}
		if (DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance() != null)
		{
			DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance().Hide();
		}
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			MonoBehaviour.print(item.Key + "," + item.Value.ToString() + "----");
		}
		if ((bool)dictionary["success"])
		{
			DCDF_MySqlConnection.user = DCDF_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
			if (dictionary.ContainsKey("repeat") && (bool)dictionary["repeat"])
			{
				if (dictionary.ContainsKey("roomId"))
				{
					int num = (int)dictionary["roomId"];
					if (dictionary.ContainsKey("deskId"))
					{
						int num2 = (int)dictionary["deskId"];
						Dictionary<string, object> dictionary2 = dictionary["seat"] as Dictionary<string, object>;
						int num3 = (int)dictionary2["userScore"];
						int num4 = (int)dictionary2["userGold"];
						UnityEngine.Debug.Log(DCDF_LogHelper.Magenta("roomId: {0},deskId: {1},userScore: {2},userGold: {3}", num, num2, num3, num4));
					}
					else
					{
						UnityEngine.Debug.Log(DCDF_LogHelper.Magenta("roomId: {0}", num));
					}
				}
				else
				{
					UnityEngine.Debug.Log(DCDF_LogHelper.Magenta("not in room"));
				}
			}
			else
			{
				UnityEngine.Debug.Log(DCDF_LogHelper.Magenta("not reconnect login"));
			}
			DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().isLogined = true;
			if (_coKeepHeart != null)
			{
				StopCoroutine(_coKeepHeart);
			}
			_coKeepHeart = StartCoroutine(DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().KeepHeart());
			DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectCount = 0;
		}
		else
		{
			int code = (int)dictionary["msgCode"];
			Action callback = delegate
			{
				QuitToHallLogin();
			};
			StartCoroutine(DCDF_Utils.DelayCall(0f, delegate
			{
				if (_coLogin != null)
				{
					StopCoroutine(_coLogin);
				}
			}));
			string errorMsg = DCDF_ErrorCode.GetErrorMsg(code, DCDF_MySqlConnection.language.Equals("zh"));
			PrepareQuitGame();
			DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog(errorMsg, showOkCancel: false, callback);
		}
	}

	public int GetUIId()
	{
		int result = 0;
		if (DCDF_MySqlConnection.curView == "LoadingView")
		{
			result = 0;
		}
		else if (DCDF_MySqlConnection.curView == "RoomSelectionView")
		{
			result = 1;
		}
		else if (DCDF_MySqlConnection.curView == "MajorGame")
		{
			result = 2;
		}
		return result;
	}

	public void HandleNetMsg_LevelUp(object[] args)
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.NetHandle("HandleNetMsg_LevelUp"));
		int num = DCDF_MySqlConnection.user.level + 1;
	}

	public void HandleNetMsg_Overflow(object[] args)
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.NetHandle("HandleNetMsg_Overflow"));
		OverflowProcess();
	}

	public void HandleNetMsg_QuitToLogin(object[] args)
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.NetHandle("HandleNetMsg_QuitToLogin"));
		MonoBehaviour.print(args);
		if (args == null)
		{
			Login();
			return;
		}
		int num = (int)args[0];
		string empty = string.Empty;
		switch (num)
		{
		case 1:
			empty = ZH2_GVars.ShowTip("服务器升级维护", "Disconnected because of server maintenance", "การบำรุงรักษาการอัพเกรดเซิร์ฟเวอร์", "Bảo trì nâng cấp máy chủ");
			break;
		case 2:
			empty = ZH2_GVars.ShowTip("账号被冻结,请联系管理员", "This account has been frozen", "เลขบัญชีถูกระงับ", "Tài khoản bị đóng băng, vui lòng liên hệ với quản trị viên");
			break;
		case 3:
			empty = ZH2_GVars.ShowTip("玩家账号被删除", "Your account has been deleted, please contact customer service", "หมายเลขบัญชีของผู้เล่นถูกลบ", "Tài khoản người chơi bị xóa");
			break;
		case 4:
                empty = ZH2_GVars.ShowTip("该账号已在别处登录，您被迫下线", "The account has been elsewhere logging in, you have been forced to logoff ", "บัญชีนี้ได้เข้าสู่ระบบที่อื่นและคุณถูกบังคับให้ออฟไลน์", "Tài khoản đã được đăng nhập ở nơi khác và bạn buộc phải offline");
                break;
		case 5:
                empty = ZH2_GVars.ShowTip("后台修改了玩家密码", "The password has been changed", "Backoffice แก้ไขรหัสผ่านของผู้เล่น", "Thay đổi mật khẩu người chơi");
                break;
            case 6:
                empty = ZH2_GVars.ShowTip("断线重连失败，网络连接已断开", "Network disconnected", "การเชื่อมต่อสายหักล้มเหลวและการเชื่อมต่อเครือข่ายถูกตัด", "Mất kết nối lại không thành công, kết nối mạng bị ngắt");
                break;
            default:
			empty = $"QuitToLogin> unknown type: {num}";
			break;
		}
		PrepareQuitGame();
		DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog(empty, showOkCancel: false, delegate
		{
			QuitToHallLogin();
		});
	}

	public void PrepareQuitGame()
	{
		if (DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance() != null)
		{
			DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance().Hide();
		}
		if (_coLogin != null)
		{
			StopCoroutine(_coLogin);
			_coLogin = null;
		}
		if (_coLoading != null)
		{
			StopCoroutine(_coLoading);
			_coLoading = null;
		}
		if (_coKeepHeart != null)
		{
			StopCoroutine(_coKeepHeart);
			_coKeepHeart = null;
		}
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().Disconnect();
	}

	public void HandleNetMsg_Notice(object[] args)
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.NetHandle("HandleNetMsg_Notice"));
		string text = args[0] as string;
		UnityEngine.Debug.Log(DCDF_LogHelper.NetHandle($"msg: {text}"));
		DCDF_NoticeController instance = DCDF_MB_Singleton<DCDF_NoticeController>.GetInstance();
		if (instance != null)
		{
			instance.AddMessage(text);
		}
	}

	public void HandleNetMsg_QuitToRoom(object[] args)
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.NetHandle("HandleNetMsg_QuitToRoom"));
		int num = (int)args[0];
		bool flag = false;
		string empty = string.Empty;
		switch (num)
		{
		case 1:
		{
			string text2 = (DCDF_MySqlConnection.language == "zh") ? "桌子已经被系统删除" : "Game table not exist";
			empty = ((DCDF_MySqlConnection.language == "zh") ? "网络异常，已自动结算，请重新进入游戏" : "Network error, credits have been automatically settled");
			flag = true;
			break;
		}
		case 2:
		{
			string text = (DCDF_MySqlConnection.language == "zh") ? "桌子参数已修改" : "Network is not stable please re-enter";
			empty = ((DCDF_MySqlConnection.language == "zh") ? "网络异常，已自动结算，请重新进入游戏" : "Network error, credits have been automatically settled");
			break;
		}
		case 3:
			empty = ((DCDF_MySqlConnection.language == "zh") ? "由于您长时间未游戏，已自动退出游戏" : "Game quit automatically because you did not play for a long time");
			break;
		case 4:
			empty = ((DCDF_MySqlConnection.language == "zh") ? "桌子爆机" : "Your account is blasting, please excharge");
			break;
		default:
			empty = $"QuitToRoom> unknown type: [{num}]";
			break;
		}
		DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().Hide();
		DCDF_MB_Singleton<DCDF_LobbyViewController>.GetInstance().Show();
		DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog(empty);
	}

	public void HandleNetMsg_SyncGold(object[] args)
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.NetHandle("HandleNetMsg_SyncGold"));
		if (DCDF_MySqlConnection.user != null)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			DCDF_MySqlConnection.user.expeGold = (int)dictionary["expeGold"];
			DCDF_MySqlConnection.user.gameGold = (int)dictionary["gameGold"];
			if (!(DCDF_MySqlConnection.curView == "LoadingView"))
			{
				DCDF_MB_Singleton<DCDF_LobbyViewController>.GetInstance().UpdateView();
			}
		}
	}

	public void HandleNetMsg_UserAward(object[] args)
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.NetHandle("HandleNetMsg_UserAward"));
		int num = (int)args[0];
		string content = (DCDF_MySqlConnection.language == "zh") ? $"恭喜您获得客服赠送的{num}游戏币!" : $"Congratulations , you get {num} coins from customer service!";
		DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
		});
	}

	public void HandleNetMsg_GameGold(object[] args)
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.NetHandle("HandleNetMsg_GameGold"));
		if (DCDF_MySqlConnection.user != null)
		{
			int gameGold = (int)args[0];
			DCDF_MySqlConnection.user.gameGold = gameGold;
			if (DCDF_MB_Singleton<DCDF_LobbyViewController>.GetInstance() != null)
			{
				DCDF_MB_Singleton<DCDF_LobbyViewController>.GetInstance().UpdateView();
			}
		}
	}

	public void HandleNetMsg_CheckVersion(object[] args)
	{
		UnityEngine.Debug.Log(DCDF_LogHelper.NetHandle("HandleNetMsg_CheckVersion"));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary = (args[0] as Dictionary<string, object>);
		if (!(bool)dictionary["success"])
		{
		}
		if ((bool)dictionary["haveNewVersion"])
		{
			Dictionary<string, object> dictionary2 = dictionary["downloadAddress"] as Dictionary<string, object>;
			string text = (string)dictionary2["downloadWindows"];
			string text2 = (string)dictionary2["downloadAndroid"];
		}
		else
		{
			m_hasVersionChecked = true;
		}
	}

	public void Handle_NetDown(object[] args)
	{
		UnityEngine.Debug.Log("Handle_NetDown");
		UnityEngine.Debug.Log(string.Format("curView: {0}, isReady: {1}, _lockDic.HandleNetDown: {2}, connectCount: {3}", new object[3]
		{
			DCDF_MySqlConnection.curView,
			DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().isReady,
			DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectCount
		}));
		if (DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().isReady)
		{
			if (DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance() != null)
			{
				DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance().Hide();
			}
			return;
		}
		Exception ex = null;
		if (args != null)
		{
			ex = (args[0] as Exception);
		}
		_isReconnecting = true;
		UnityEngine.Debug.Log(ex);
		bool flag = false;
		if (DCDF_MySqlConnection.curView == "LoadingView" && flag)
		{
			string content = string.Format((DCDF_MySqlConnection.language == "zh") ? "网络连接已断开，错误代码为 [{0}]" : "Net Error [{0}]", (ex == null) ? "9999" : ex.Message);
			DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
			{
				QuitToHallLogin();
			});
			return;
		}
		if (_coLogin != null)
		{
			StopCoroutine(_coLogin);
			_coLogin = null;
		}
		if ((DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectCount > DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectMaxTimes && DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectTimeCount > DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectMaxTimeout / 2f) || DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectTimeCount > DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectMaxTimeout)
		{
			_timeoutQuit();
			return;
		}
		_coLogin = StartCoroutine(_connectAndLoginCoroutine());
		if (DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance() != null)
		{
			DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance().Show();
		}
	}

	public void Send_ReLogin()
	{
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().Send("userService/reLogin", new object[3]
		{
			DCDF_MySqlConnection.username,
			DCDF_MySqlConnection.pwd,
			GetUIId()
		});
	}

	public void ChangeView(string newView)
	{
		UnityEngine.Debug.Log($"changeView> from [{DCDF_MySqlConnection.curView}] to [{newView}]");
		DCDF_MySqlConnection.curView = newView;
		if (DCDF_MB_Singleton<DCDF_NoticeController>.GetInstance() != null)
		{
			DCDF_MB_Singleton<DCDF_NoticeController>.GetInstance().ChangeScene();
		}
	}

	private void _timeoutQuit()
	{
		UnityEngine.Debug.Log($"connectCount: {DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectCount}, connectTimeCount: {DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectTimeCount}");
		UnityEngine.Debug.Log($"connectMaxTimes: {DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectMaxTimes}, connectMaxTimeout: {DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().connectMaxTimeout}");
		string content = (DCDF_MySqlConnection.language == "zh") ? "网络连接错误，请检查网络连接" : "Unable to connect to server，Please check your network connection";
		if (DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance() != null)
		{
			DCDF_MB_Singleton<DCDF_ReconnectHint>.GetInstance().Hide();
		}
		PrepareQuitGame();
		DCDF_MB_Singleton<DCDF_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			QuitToHallLogin();
		});
	}

	private void _onLoadingFinish()
	{
		UnityEngine.Debug.Log("_onLoadingFinish");
		DCDF_MB_Singleton<DCDF_LoadingViewController>.GetInstance().Hide();
		DCDF_MB_Singleton<DCDF_LobbyViewController>.GetInstance().Show();
		DCDF_SoundManager.Instance.PlayNormalBGM();
		Transform transform = GameObject.Find("Canvas").transform;
		DCDF_MB_Singleton<DCDF_AlertDialog>.SetInstanceByGameObject(transform.Find("Mask/AlertDialog").gameObject, force: true).PreInit();
		Transform transform2 = transform.Find("Loading");
		if ((bool)transform2)
		{
			transform2.gameObject.SetActive(value: false);
		}
		DCDF_MB_Singleton<DCDF_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}
}
