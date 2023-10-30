using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class STWM_GameManager : STWM_MB_Singleton<STWM_GameManager>
{
	public bool isDevelopment;

	public bool isEnglish;

	public STWM_DevSceneType devSceneType;

	private bool _isDirectLoad;

	private bool _isReconnecting;

	[SerializeField]
	private GameObject _touchForbid;

	private bool _last_isEnglish;

	private Dictionary<string, object> _lockDic;

	private Coroutine _coLogin;

	private Coroutine _coLoading;

	private AudioListener _audio;

	private bool isWhiteDomainName = true;

	private bool _hasLogined;

	[Obsolete]
	private TouchInputModule _touchInputModule;

	private StandaloneInputModule _standaloneInputModule;

	private Coroutine _coKeepHeart;

	private bool m_hasVersionChecked;

	public bool HasDesk => STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().HasDesk;

	private void Awake()
	{
		_last_isEnglish = isEnglish;
		_isReconnecting = false;
		if (STWM_MB_Singleton<STWM_GameManager>._instance != null)
		{
			STWM_MB_Singleton<STWM_GameManager>.GetInstance().InitSingletonStep2();
			STWM_MB_Singleton<STWM_GameManager>.GetInstance().HideAllViewButLoading();
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
		else if (STWM_MB_Singleton<STWM_GameManager>._instance == null)
		{
			STWM_MB_Singleton<STWM_GameManager>.SetInstance(this);
			_isDirectLoad = !SceneManager.GetActiveScene().name.StartsWith("Loading");
			PreInit();
			if (_isDirectLoad)
			{
				InitSingletonStep2();
			}
		}
	}

	private void Start()
	{
		if (!_isDirectLoad)
		{
			_init();
			return;
		}
		if (isDevelopment)
		{
			_devInit();
			return;
		}
		HideAllViewButLoading();
		STWM_MB_Singleton<STWM_LoadingViewController>.GetInstance().Hide();
		_init();
	}

	private void Update()
	{
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectTimeCount += Time.deltaTime;
		if (STWM_GVars.curView == "LoadingView")
		{
			return;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			UnityEngine.Debug.Log(string.Format("Esc: {0}", STWM_LockManager.GetValue("Esc")));
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance()._IsAning && !STWM_LockManager.IsLocked("Esc") && !STWM_LockManager.IsLocked("Quit"))
		{
			if (STWM_GVars.curView == "RoomSelectionView" || STWM_GVars.curView == "DeskSelectinView")
			{
				Handle_BtnReturn();
			}
			else if ((STWM_GVars.curView == "DiceGame" || STWM_GVars.curView == "MajorGame") && !STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().bEnterMary)
			{
				Handle_ItemReturn();
			}
		}
		if (STWM_GVars.lockOnePoint)
		{
			STWM_GVars.lockOnePoint = false;
		}
	}

	public void PreInit()
	{
		_lockDic = new Dictionary<string, object>();
		_initSingletonStep1();
	}

	private void _init()
	{
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("heart", delegate
		{
		});
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("netDown", Handle_NetDown);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("userLogin", HandleNetMsg_UserLogin);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("reLogin", HandleNetMsg_ReLogin);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("overflow", HandleNetMsg_Overflow);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("levelUp", HandleNetMsg_LevelUp);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("quitToLogin", HandleNetMsg_QuitToLogin);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("quitToRoom", HandleNetMsg_QuitToRoom);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("syncGold", HandleNetMsg_SyncGold);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("checkVersion", HandleNetMsg_CheckVersion);
		if (!STWM_MB_Singleton<STWM_NetManager>.GetInstance().IsMethodRegistered("updateRoomInfo"))
		{
			STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("updateRoomInfo", delegate
			{
			});
		}
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("userAward", HandleNetMsg_UserAward);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("gameGold", HandleNetMsg_GameGold);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("sendNotice", HandleNetMsg_Notice);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("notice", HandleNetMsg_Notice);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("timeout", delegate(object[] args)
		{
			UnityEngine.Debug.Log(STWM_LogHelper.Magenta("timeout>  id: " + (int)args[0]));
		});
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("heart timeout", delegate
		{
			UnityEngine.Debug.Log(STWM_LogHelper.Magenta("heart timeout"));
			Handle_NetDown(new object[1]
			{
				new Exception("heart timeout")
			});
		});
		STWM_MB_Singleton<STWM_LoadingViewController>.GetInstance().Show();
		if (!_isDirectLoad)
		{
			STWM_MB_Singleton<STWM_LoadingViewController>.GetInstance().loadLevelFinishAction += _onloadLevelFinish;
			_coLoading = StartCoroutine(STWM_MB_Singleton<STWM_LoadingViewController>.GetInstance().LoadingAni2());
		}
		else
		{
			_coLoading = StartCoroutine(STWM_MB_Singleton<STWM_LoadingViewController>.GetInstance().LoadingAni());
		}
		if (STWM_MB_Singleton<STWM_LoginController>.GetInstance() == null || !STWM_MB_Singleton<STWM_LoginController>.GetInstance().IsShow())
		{
			Login();
		}
	}

	public void Login()
	{
		_coLogin = StartCoroutine(_connectAndLoginCoroutine());
	}

	public void RefreshLocalization()
	{
		STWM_I2LocalizeText[] array = UnityEngine.Object.FindObjectsOfType<STWM_I2LocalizeText>();
		STWM_I2LocalizeText[] array2 = array;
		foreach (STWM_I2LocalizeText sTWM_I2LocalizeText in array2)
		{
			sTWM_I2LocalizeText.Refresh();
		}
		STWM_I2LocalizeImage[] array3 = UnityEngine.Object.FindObjectsOfType<STWM_I2LocalizeImage>();
		STWM_I2LocalizeImage[] array4 = array3;
		foreach (STWM_I2LocalizeImage sTWM_I2LocalizeImage in array4)
		{
			sTWM_I2LocalizeImage.Refresh();
		}
	}

	public void HideAllViewButLoading()
	{
		STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_DiceGameController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_HUDController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_OptionsController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_SettingsController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_MaryMovieController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_NoticeController>.GetInstance().Hide();
		if (!_lockDic.GetBoolValue("HandleNetDown") && !_lockDic.GetBoolValue("Alert"))
		{
			STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().Hide();
		}
	}

	private void _initSingletonStep1()
	{
		Transform transform = GameObject.Find("Canvas").transform;
		STWM_MB_Singleton<STWM_LoadingViewController>.SetInstanceByGameObject(transform.Find("Mask/LobbyLoad").gameObject).PreInit();
		STWM_MB_Singleton<STWM_AlertDialog>.SetInstanceByGameObject(transform.Find("Mask/AlertDialog").gameObject).PreInit();
		Transform transform2 = transform.Find("LoginPanel");
		if (transform2 != null)
		{
			STWM_MB_Singleton<STWM_LoginController>.SetInstanceByGameObject(transform2.gameObject).PreInit();
		}
		STWM_MB_Singleton<STWM_LoadingViewController>.GetInstance().loadingFinishAction += _onLoadingFinish;
	}

	public void InitSingletonStep2()
	{
		Transform transform = GameObject.Find("Canvas").transform;
		_audio = GameObject.Find("Main Camera").GetComponent<AudioListener>();
		_audio.enabled = false;
		if (_isDirectLoad)
		{
			_audio.enabled = true;
		}
		STWM_SoundManager.Instance = GameObject.Find("SoundManager").GetComponent<STWM_SoundManager>();
		STWM_MB_Singleton<STWM_LobbyViewController>.SetInstanceByGameObject(transform.Find("Mask/Lobby").gameObject).PreInit();
		STWM_MB_Singleton<STWM_HeadViewController>.SetInstanceByGameObject(transform.Find("Mask/Lobby/STWM_Title").gameObject).PreInit();
		STWM_MB_Singleton<STWM_MajorGameController>.SetInstanceByGameObject(transform.Find("Mask/Major").gameObject).PreInit();
		STWM_MB_Singleton<STWM_MaryGameController>.SetInstanceByGameObject(transform.Find("Mask/Mary").gameObject).PreInit();
		STWM_MB_Singleton<STWM_DiceGameController>.SetInstanceByGameObject(transform.Find("Mask/Dice").gameObject).PreInit();
		STWM_MB_Singleton<STWM_DiceGameController2>.SetInstanceByGameObject(transform.Find("Mask/Dice").gameObject).PreInit();
		STWM_MB_Singleton<STWM_HUDController>.SetInstanceByGameObject(transform.Find("Mask/HUD").gameObject).PreInit();
		STWM_MB_Singleton<STWM_OptionsController>.SetInstanceByGameObject(transform.Find("Mask/Options").gameObject).PreInit();
		STWM_MB_Singleton<STWM_SettingsController>.SetInstanceByGameObject(transform.Find("Mask/Setting").gameObject).PreInit();
		STWM_MB_Singleton<STWM_ScoreBank>.SetInstanceByGameObject(transform.Find("Mask/InOut").gameObject).PreInit();
		STWM_MB_Singleton<STWM_MaryMovieController>.SetInstanceByGameObject(transform.Find("Mask/MaryMovie").gameObject).PreInit();
		STWM_MB_Singleton<STWM_NoticeController>.SetInstanceByGameObject(transform.Find("Mask/Notice").gameObject).PreInit();
		STWM_MB_Singleton<STWM_DiceGameController2>.GetInstance().onExitAction = _onDiceExit;
		STWM_MB_Singleton<STWM_SettingsController>.GetInstance().Init();
		STWM_MB_Singleton<STWM_HeadViewController>.GetInstance().btnReturnAction = Handle_BtnReturn;
		STWM_MB_Singleton<STWM_OptionsController>.GetInstance().onItemReturn = Handle_ItemReturn;
	}

	private void _prepareInitData()
	{
		STWM_GVars.versionCode = "9.9.1";
		if (!STWM_MB_Singleton<STWM_NetManager>.GetInstance().useFake)
		{
			STWM_GVars.IPPort = 10016;
			UseDebugData();
		}
		STWM_GVars.language = ((ZH2_GVars.language_enum != 0) ? "en" : "zh");
	}

	private void OnGUI()
	{
		if (!isWhiteDomainName)
		{
			GUI.Box(new Rect(10f, 10f, (float)Screen.width * 0.05f, (float)Screen.height * 0.5f), "is not white IP!");
		}
	}

	private void UseDebugData()
	{
		STWM_GVars.username = ZH2_GVars.username;
		STWM_GVars.pwd = ZH2_GVars.pwd;
		STWM_GVars.IPAddress = ZH2_GVars.IPAddress_Game;
		UnityEngine.Debug.LogError("账号: " + STWM_GVars.username + " 密码: " + STWM_GVars.pwd + " IP: " + STWM_GVars.IPAddress);
		STWM_GVars.language = ((ZH2_GVars.language_enum != 0) ? "en" : "zh");
		STWM_GVars.IPPort = 10016;
	}

	private IEnumerator _connectAndLoginCoroutine()
	{
		m_hasVersionChecked = false;
		if (!_hasLogined)
		{
			STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectMaxTimes = 10;
			STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectMaxTimeout = 5f;
		}
		if (!_isReconnecting)
		{
			_prepareInitData();
		}
		else
		{
			STWM_MB_Singleton<STWM_NetManager>.GetInstance().Disconnect();
			UnityEngine.Debug.LogError("====等待0.25f");
			yield return new WaitForSeconds(0.25f);
		}
		if (!_hasLogined)
		{
			STWM_GVars.IPAddress = STWM_CheckIP.DoGetHostAddresses(STWM_GVars.IPAddress);
		}
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Connect(STWM_GVars.IPAddress, STWM_GVars.IPPort);
		while (!STWM_MB_Singleton<STWM_NetManager>.GetInstance().isConnected)
		{
			yield return null;
		}
		while (!m_hasVersionChecked)
		{
			yield return null;
		}
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().SendPublicKey();
		while (!STWM_MB_Singleton<STWM_NetManager>.GetInstance().isReady)
		{
			yield return null;
		}
		float waitTime = 0.1f;
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
		if (!_hasLogined && !STWM_MB_Singleton<STWM_NetManager>.GetInstance().useFake)
		{
			UnityEngine.Debug.Log(STWM_LogHelper.Magenta("_loadingLoginTimeout"));
			_timeoutQuit();
		}
	}

	public void QuitToHallGame()
	{
		SetTouchEnable(isEnable: false, "QuitToHallGame");
		Send_QuitGame();
		ZH2_GVars.isStartedFromGame = true;
		SceneManager.LoadScene("MainScene");
	}

	public void QuitToHallLogin()
	{
		UnityEngine.Debug.Log(STWM_LogHelper.Magenta("QuitToHallLogin"));
		SetTouchEnable(isEnable: false, "QuitToHallLogin");
		Send_QuitGame();
		if (AssetBundleManager.GetInstance() != null)
		{
			AssetBundleManager.GetInstance().UnloadAB("STWM");
		}
		ZH2_GVars.isStartedFromGame = false;
		SceneManager.LoadScene("MainScene");
	}

	public void SetTouchEnable(bool isEnable, string reason = "", bool includeEsc = true)
	{
		if (_touchForbid == null)
		{
			GameObject gameObject = GameObject.Find("Canvas");
			if (gameObject == null)
			{
				return;
			}
			Transform transform = gameObject.transform.Find("TouchForbid");
			if (transform == null)
			{
				return;
			}
			_touchForbid = transform.gameObject;
		}
		_touchForbid.SetActive(!isEnable);
		if (includeEsc)
		{
			if (isEnable)
			{
				STWM_LockManager.UnLock("Esc", STWM_LockManager.GetValue("Esc") < 10);
			}
			else
			{
				STWM_LockManager.Lock("Esc");
			}
		}
	}

	public bool GetTouchEnable()
	{
		if (_touchForbid == null)
		{
			GameObject gameObject = GameObject.Find("Canvas");
			if (gameObject == null)
			{
				return true;
			}
			Transform transform = gameObject.transform.Find("TouchForbid");
			if (transform == null)
			{
				return true;
			}
			_touchForbid = transform.gameObject;
		}
		return !_touchForbid.activeSelf;
	}

	public void OverflowProcess()
	{
		_lockDic["Alert"] = false;
		string content = (STWM_GVars.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge";
		STWM_LockManager.Lock("Quit");
		PrepareQuitGame();
		STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			QuitToHallGame();
		});
	}

	public void Send_Login()
	{
		object[] args = new object[2]
		{
			STWM_GVars.username,
			STWM_GVars.pwd
		};
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/userLogin", args);
	}

	private void OnApplicationQuit()
	{
		Send_QuitGame();
	}

	public void Send_QuitGame()
	{
		object[] args = new object[0];
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/quitGame", args);
		STWM_LockManager.Lock("Quit");
		SetTouchEnable(isEnable: false, "Send_QuitGame");
	}

	public void HandleNetMsg_ReLogin(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string[] array2;
		if (STWM_GVars.language == "zh")
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
			STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(array4[num2], showOkCancel: false, delegate
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
			STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(array4[num4], showOkCancel: false, delegate
			{
				QuitToHallGame();
			});
			return;
		}
		int num5 = (int)dictionary["uiid"];
		UnityEngine.Debug.Log("uiid: " + num5);
		if (num5 == 1)
		{
			STWM_GVars.user = STWM_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
			if (STWM_LockManager.IsLocked("btn_room"))
			{
				STWM_LockManager.UnLock("btn_room");
				SetTouchEnable(isEnable: true, "reconnect success");
			}
		}
		if (num5 == 2)
		{
			object[] array5 = (object[])dictionary["roomInfo"];
			int num6 = array5.Length;
			STWM_Desk[] array6 = new STWM_Desk[num6];
			for (int i = 0; i < num6; i++)
			{
				array6[i] = STWM_Desk.CreateWithDic((Dictionary<string, object>)array5[i]);
			}
			if (STWM_LockManager.IsLocked("EnterDesk"))
			{
				STWM_LockManager.UnLock("EnterDesk");
				SetTouchEnable(isEnable: true, "reconnect success");
			}
			if (STWM_LockManager.IsLocked("btn_room"))
			{
				STWM_LockManager.UnLock("btn_room");
				SetTouchEnable(isEnable: true, "reconnect success");
			}
		}
		if (num5 == 3)
		{
			Dictionary<string, object> dictionary2 = dictionary["deskInfo"] as Dictionary<string, object>;
			int credit = (int)dictionary2["userScore"];
			int num7 = (int)dictionary2["userGold"];
			STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().JudgeResetGame(credit);
		}
		if (STWM_MB_Singleton<STWM_ReconnectHint>.GetInstance() != null)
		{
			STWM_MB_Singleton<STWM_ReconnectHint>.GetInstance().Hide();
		}
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().isLogined = true;
		if (_coKeepHeart != null)
		{
			StopCoroutine(_coKeepHeart);
		}
		_coKeepHeart = StartCoroutine(STWM_MB_Singleton<STWM_NetManager>.GetInstance().KeepHeart());
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectCount = 0;
	}

	public void HandleNetMsg_UserLogin(object[] args)
	{
		UnityEngine.Debug.LogError("收到登录返回: " + JsonMapper.ToJson(args));
		_hasLogined = true;
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectMaxTimes = 20;
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectMaxTimeout = 20f;
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectMaxTimes = 20;
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectMaxTimeout = 20f;
		if (_coLogin != null)
		{
			StopCoroutine(_coLogin);
			_coLogin = null;
		}
		if (STWM_MB_Singleton<STWM_ReconnectHint>.GetInstance() != null)
		{
			STWM_MB_Singleton<STWM_ReconnectHint>.GetInstance().Hide();
		}
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			ZH2_GVars.hallInfo = new Dictionary<string, object>();
			ZH2_GVars.hallInfo = (dictionary["hallInfo"] as Dictionary<string, object>);
			STWM_GVars.user = STWM_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
			ZH2_GVars.userId = STWM_GVars.user.id;
			if (dictionary.ContainsKey("repeat") && (bool)dictionary["repeat"])
			{
				if (dictionary.ContainsKey("roomId"))
				{
					if (STWM_LockManager.IsLocked("EnterDesk"))
					{
						STWM_LockManager.UnLock("EnterDesk");
						SetTouchEnable(isEnable: true, "reconnect success");
					}
					int num = (int)dictionary["roomId"];
					if (dictionary.ContainsKey("deskId"))
					{
						int num2 = (int)dictionary["deskId"];
						Dictionary<string, object> dictionary2 = dictionary["seat"] as Dictionary<string, object>;
						int num3 = (int)dictionary2["userScore"];
						int num4 = (int)dictionary2["userGold"];
						UnityEngine.Debug.Log(STWM_LogHelper.Magenta("roomId: {0},deskId: {1},userScore: {2},userGold: {3}", num, num2, num3, num4));
						STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().JudgeResetGame(num3);
					}
					else
					{
						UnityEngine.Debug.Log(STWM_LogHelper.Magenta("roomId: {0}", num));
					}
				}
				else
				{
					UnityEngine.Debug.Log(STWM_LogHelper.Magenta("not in room"));
					if (STWM_LockManager.IsLocked("btn_room"))
					{
						STWM_LockManager.UnLock("btn_room");
						SetTouchEnable(isEnable: true, "reconnect success");
					}
				}
			}
			STWM_MB_Singleton<STWM_NetManager>.GetInstance().isLogined = true;
			if (_coKeepHeart != null)
			{
				StopCoroutine(_coKeepHeart);
			}
			_coKeepHeart = StartCoroutine(STWM_MB_Singleton<STWM_NetManager>.GetInstance().KeepHeart());
			STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectCount = 0;
		}
		else
		{
			int code = (int)dictionary["msgCode"];
			Action callback = delegate
			{
				QuitToHallLogin();
				_lockDic["Alert"] = false;
			};
			StartCoroutine(STWM_Utils.DelayCall(0f, delegate
			{
				if (_coLogin != null)
				{
					StopCoroutine(_coLogin);
				}
			}));
			string errorMsg = STWM_ErrorCode.GetErrorMsg(code, STWM_GVars.language.Equals("zh"));
			_lockDic["Alert"] = true;
			STWM_LockManager.Lock("Quit");
			SetTouchEnable(isEnable: true, "quit alert", includeEsc: false);
			PrepareQuitGame();
			STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(errorMsg, showOkCancel: false, callback);
		}
	}

	public int GetUIId()
	{
		int result = 0;
		if (STWM_GVars.curView == "LoadingView")
		{
			result = 0;
		}
		else if (STWM_GVars.curView == "RoomSelectionView")
		{
			result = 1;
		}
		else if (STWM_GVars.curView == "DeskSelectionView")
		{
			result = 2;
		}
		else if (STWM_GVars.curView == "MajorGame" || STWM_GVars.curView == "DiceGame" || STWM_GVars.curView == "MaryGame")
		{
			result = 3;
		}
		return result;
	}

	public void HandleNetMsg_LevelUp(object[] args)
	{
		UnityEngine.Debug.Log(STWM_LogHelper.NetHandle("HandleNetMsg_LevelUp"));
		int levelInfo = STWM_GVars.user.level + 1;
		STWM_MB_Singleton<STWM_HeadViewController>.GetInstance().SetLevelInfo(levelInfo);
	}

	public void HandleNetMsg_Overflow(object[] args)
	{
		UnityEngine.Debug.Log(STWM_LogHelper.NetHandle("HandleNetMsg_Overflow"));
		STWM_LockManager.Lock("Overflow");
		if (!STWM_LockManager.IsLocked("Delay_Overflow"))
		{
			OverflowProcess();
		}
	}

	public void HandleNetMsg_QuitToLogin(object[] args)
	{
		UnityEngine.Debug.Log("HandleNetMsg_QuitToLogin: " + JsonFx.Json.JsonWriter.Serialize(args));
		if (args == null)
		{
			UnityEngine.Debug.LogError("==========收到空的要求重新登录=========");
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
		STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(empty, showOkCancel: false, delegate
		{
			QuitToHallLogin();
			_lockDic["Alert"] = false;
		});
	}

	public void PrepareQuitGame()
	{
		_lockDic["Alert"] = true;
		STWM_LockManager.Lock("Quit");
		if (STWM_MB_Singleton<STWM_ReconnectHint>.GetInstance() != null)
		{
			STWM_MB_Singleton<STWM_ReconnectHint>.GetInstance().Hide();
		}
		SetTouchEnable(isEnable: true, "quit alert", includeEsc: false);
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
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Disconnect();
	}

	public void HandleNetMsg_Notice(object[] args)
	{
		UnityEngine.Debug.Log(STWM_LogHelper.NetHandle("HandleNetMsg_Notice"));
		string text = args[0] as string;
		UnityEngine.Debug.Log(STWM_LogHelper.NetHandle($"msg: {text}"));
		STWM_NoticeController instance = STWM_MB_Singleton<STWM_NoticeController>.GetInstance();
		if (instance != null)
		{
			instance.AddMessage(text);
		}
	}

	public void HandleNetMsg_QuitToRoom(object[] args)
	{
		UnityEngine.Debug.Log(STWM_LogHelper.NetHandle("HandleNetMsg_QuitToRoom"));
		int num = (int)args[0];
		bool flag = false;
		string empty = string.Empty;
		switch (num)
		{
		case 1:
		{
			string text2 = (STWM_GVars.language == "zh") ? "桌子已经被系统删除" : "Game table not exist";
			empty = ((STWM_GVars.language == "zh") ? "网络异常，已自动结算，请重新进入游戏" : "Network error, credits have been automatically settled");
			flag = true;
			break;
		}
		case 2:
		{
			string text = (STWM_GVars.language == "zh") ? "桌子参数已修改" : "Network is not stable please re-enter";
			empty = ((STWM_GVars.language == "zh") ? "网络异常，已自动结算，请重新进入游戏" : "Network error, credits have been automatically settled");
			break;
		}
		case 3:
			empty = ((STWM_GVars.language == "zh") ? "由于您长时间未游戏，已自动退出游戏" : "Game quit automatically because you did not play for a long time");
			break;
		case 4:
			empty = ((STWM_GVars.language == "zh") ? "桌子爆机" : "Your account is blasting, please excharge");
			break;
		default:
			empty = $"QuitToRoom> unknown type: [{num}]";
			break;
		}
		STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().ResetGame();
		STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().ResetGame();
		STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_MaryMovieController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().Show();
		if (flag)
		{
			STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().curDeskIndex = 0;
			STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().UpdateAllDeskWidgets();
		}
		_lockDic["Alert"] = true;
		SetTouchEnable(isEnable: true, "QuitToRoom");
		STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(empty, showOkCancel: false, delegate
		{
			_lockDic["Alert"] = false;
			STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().isTouchForbidden = false;
		});
	}

	public void HandleNetMsg_SyncGold(object[] args)
	{
		if (STWM_GVars.user != null)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			STWM_GVars.user.expeGold = (int)dictionary["expeGold"];
			STWM_GVars.user.gameGold = (int)dictionary["gameGold"];
			if (!(STWM_GVars.curView == "LoadingView"))
			{
				STWM_MB_Singleton<STWM_HeadViewController>.GetInstance().UpdateView();
			}
		}
	}

	public void HandleNetMsg_UserAward(object[] args)
	{
		UnityEngine.Debug.Log(STWM_LogHelper.NetHandle("HandleNetMsg_UserAward"));
		int num = (int)args[0];
		string content = (STWM_GVars.language == "zh") ? $"恭喜您获得客服赠送的{num}游戏币!" : $"Congratulations , you get {num} coins from customer service!";
		STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
		});
	}

	public void HandleNetMsg_GameGold(object[] args)
	{
		if (STWM_GVars.user != null)
		{
			try
			{
				UnityEngine.Debug.LogError(" HandleNetMsg_GameGold " + JsonMapper.ToJson(args));
				int gameGold = (int)args[0];
				STWM_GVars.user.gameGold = gameGold;
			}
			catch (Exception)
			{
			}
			if (STWM_MB_Singleton<STWM_HeadViewController>.GetInstance() != null)
			{
				STWM_MB_Singleton<STWM_HeadViewController>.GetInstance().UpdateView();
			}
		}
	}

	public void HandleNetMsg_CheckVersion(object[] args)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary = (args[0] as Dictionary<string, object>);
		if (!(bool)dictionary["success"])
		{
		}
		if (false)
		{
			Dictionary<string, object> dictionary2 = dictionary["downloadAddress"] as Dictionary<string, object>;
			string text = (string)dictionary2["downloadWindows"];
			string str = (string)dictionary2["downloadAndroid"];
			STWM_LockManager.Lock("Quit");
			UnityEngine.Debug.LogError("===需要更新===更新地址: " + text + " text: " + str);
			STWM_IOSGameStart.GetSingleton().UpdateGameVesion(text);
		}
		else
		{
			m_hasVersionChecked = true;
		}
	}

	public void AfterReconnect_DoUnlock()
	{
		STWM_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
	}

	public void Handle_NetDown(object[] args)
	{
		UnityEngine.Debug.Log("Handle_NetDown");
		if (STWM_LockManager.IsLocked("Overflow") || STWM_LockManager.IsLocked("Quit"))
		{
			return;
		}
		if (STWM_MB_Singleton<STWM_NetManager>.GetInstance().isReady)
		{
			if (STWM_MB_Singleton<STWM_ReconnectHint>.GetInstance() != null)
			{
				STWM_MB_Singleton<STWM_ReconnectHint>.GetInstance().Hide();
			}
		}
		else
		{
			if (STWM_LockManager.IsLocked("HandleNetDown_Quit"))
			{
				return;
			}
			_lockDic["HandleNetDown"] = true;
			Exception ex = null;
			if (args != null)
			{
				ex = (args[0] as Exception);
			}
			_isReconnecting = true;
			UnityEngine.Debug.Log(ex.Message);
			bool flag = false;
			if (STWM_GVars.curView == "LoadingView" && flag)
			{
				string content = string.Format((STWM_GVars.language == "zh") ? "网络连接已断开，错误代码为 [{0}]" : "Net Error [{0}]", (ex == null) ? "9999" : ex.Message);
				STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
				{
					QuitToHallLogin();
					_lockDic["HandleNetDown"] = false;
				});
				return;
			}
			if (_coLogin != null)
			{
				StopCoroutine(_coLogin);
				_coLogin = null;
			}
			if ((STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectCount > STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectMaxTimes && STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectTimeCount > STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectMaxTimeout / 2f) || STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectTimeCount > STWM_MB_Singleton<STWM_NetManager>.GetInstance().connectMaxTimeout)
			{
				_timeoutQuit();
				return;
			}
			_coLogin = StartCoroutine(_connectAndLoginCoroutine());
			if (STWM_MB_Singleton<STWM_ReconnectHint>.GetInstance() != null)
			{
				STWM_MB_Singleton<STWM_ReconnectHint>.GetInstance().Show();
			}
		}
	}

	public void Send_ReLogin()
	{
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/reLogin", new object[3]
		{
			STWM_GVars.username,
			STWM_GVars.pwd,
			GetUIId()
		});
	}

	public void ChangeView(string newView)
	{
		STWM_GVars.curView = newView;
		if (STWM_MB_Singleton<STWM_NoticeController>.GetInstance() != null)
		{
			STWM_MB_Singleton<STWM_NoticeController>.GetInstance().ChangeScene();
		}
	}

	private void _timeoutQuit()
	{
		string content = (STWM_GVars.language == "zh") ? "网络连接错误，请检查网络连接" : "Unable to connect to server，Please check your network connection";
		if (STWM_MB_Singleton<STWM_ReconnectHint>.GetInstance() != null)
		{
			STWM_MB_Singleton<STWM_ReconnectHint>.GetInstance().Hide();
		}
		STWM_LockManager.Lock("HandleNetDown_Quit");
		PrepareQuitGame();
		STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			QuitToHallLogin();
			_lockDic["HandleNetDown"] = false;
		});
	}

	public void Handle_BtnReturn(int id)
	{
		QuitToHallGame();
	}

	public void Handle_BtnReturn()
	{
		if (STWM_GVars.curView == "RoomSelectionView")
		{
			STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog((STWM_GVars.language == "zh") ? "是否退出程序？" : "Quit the game?", showOkCancel: true, delegate
			{
				StartCoroutine(STWM_Utils.DelayCall(0.1f, delegate
				{
					QuitToHallGame();
				}));
			});
		}
		else if (STWM_GVars.curView == "DeskSelectionView")
		{
			if (HasDesk)
			{
				STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().HideDeskViewAni();
				STWM_MB_Singleton<STWM_RoomSelectionViewController>.GetInstance().Show();
				STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().ResetRoomView();
			}
			else
			{
				STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().OnBtnReturn_Click();
			}
		}
		else
		{
			UnityEngine.Debug.LogError("==========当前状态: " + STWM_GVars.curView);
			QuitToHallGame();
		}
	}

	public void Handle_ItemReturn()
	{
		if (!STWM_LockManager.IsLocked("btn_options"))
		{
			STWM_SoundManager.Instance.StopMaryAudio();
			STWM_MB_Singleton<STWM_HUDController>.GetInstance().ResetSprite();
			STWM_MB_Singleton<STWM_HUDController>.GetInstance().HideRules();
			STWM_MB_Singleton<STWM_OptionsController>.GetInstance().Hide();
			STWM_MB_Singleton<STWM_SettingsController>.GetInstance().Hide();
			STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().Hide();
			if (STWM_GVars.curView == "DiceGame")
			{
				STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog((STWM_GVars.language == "zh") ? "是否退出比倍？" : "Quit the Dice?", showOkCancel: true, delegate
				{
					STWM_MB_Singleton<STWM_DiceGameController2>.GetInstance().ExitGame();
				});
			}
			else if (STWM_GVars.curView == "MajorGame")
			{
				STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog((STWM_GVars.language == "zh") ? "是否退出游戏？" : "Quit the game?", showOkCancel: true, delegate
				{
					STWM_SoundManager.Instance.PlayLobbyBGM();
					STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().Hide();
					STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().ExitGame();
					STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().ResetGame();
					STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().Show();
				});
			}
		}
	}

	private void _onLoadingFinish()
	{
		STWM_MB_Singleton<STWM_LoadingViewController>.GetInstance().Hide();
		UnityEngine.Debug.LogError("======大厅加载完毕=====");
		STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().Show();
		STWM_SoundManager.Instance.PlayLobbyBGM();
		_audio.enabled = true;
		Transform transform = GameObject.Find("Canvas").transform;
		STWM_MB_Singleton<STWM_AlertDialog>.SetInstanceByGameObject(transform.Find("Mask/AlertDialog").gameObject, force: true).PreInit();
		Transform transform2 = transform.Find("Loading");
		if ((bool)transform2)
		{
			transform2.gameObject.SetActive(value: false);
		}
		STWM_MB_Singleton<STWM_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	private void _onloadLevelFinish()
	{
		UnityEngine.Debug.Log("_onloadLevelFinish");
		_coLoading = null;
		InitSingletonStep2();
	}

	private void _onDiceExit()
	{
		STWM_MB_Singleton<STWM_DiceGameController2>.GetInstance().Hide();
		StartCoroutine(waitCurtainPullDown());
	}

	private IEnumerator waitCurtainPullDown()
	{
		yield return new WaitForSeconds(0.6f);
		STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().Show();
		STWM_MB_Singleton<STWM_MajorGameController>.GetInstance().OnDiceExit();
	}

	private void _makeFakeUser()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("id", 12345);
		dictionary.Add("username", "FakeUser");
		dictionary.Add("nickname", "FakeUser nick");
		dictionary.Add("sex", "shemale");
		dictionary.Add("level", 10);
		dictionary.Add("gameGold", 19999);
		dictionary.Add("expeGold", 999);
		dictionary.Add("photoId", 3);
		dictionary.Add("overflow", 0);
		dictionary.Add("type", 0);
		dictionary.Add("promoterName", "FakeUser推广员");
		STWM_GVars.user = STWM_User.CreateWithDic(dictionary);
	}

	private void _makeFakeDesk()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("id", 5);
		dictionary.Add("roomId", 2);
		dictionary.Add("name", "dev房间");
		dictionary.Add("minGold", 10);
		dictionary.Add("exchange", 10);
		dictionary.Add("onceExchangeValue", 100);
		dictionary.Add("maxSinglelineBet", 10);
		dictionary.Add("minSinglelineBet", 10);
		dictionary.Add("singlechangeScore", 10);
		dictionary.Add("diceSwitch", 1);
		dictionary.Add("diceDirectSwitch", 1);
		dictionary.Add("diceOverflow", 50000);
		dictionary.Add("full", true);
		dictionary.Add("userId", 12345);
		dictionary.Add("userPhotoId", 3);
		dictionary.Add("nickname", "fake占位");
		STWM_GVars.desk = STWM_Desk.CreateWithDic(dictionary);
	}

	private void _makePrepareMaryGame()
	{
		Dictionary<string, object> dictionary = _makeMaryGameResult() as Dictionary<string, object>;
		int[] photoNumberArray = (int[])dictionary["photoNumber"];
		int[][] photosArray = (int[][])dictionary["photos"];
		int credit = (int)dictionary["credit"];
		int totalBet = (int)dictionary["totalBet"];
		int[] totalWinArray = (int[])dictionary["totalWin"];
		int times = 3;
		STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().PrepareGame(times, credit, totalBet, photoNumberArray, photosArray, totalWinArray);
	}

	private object _makeMaryGameResult()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		int[] value = new int[7]
		{
			1,
			5,
			12,
			10,
			18,
			20,
			6
		};
		int[][] value2 = new int[7][]
		{
			new int[4]
			{
				1,
				1,
				1,
				4
			},
			new int[4]
			{
				5,
				6,
				7,
				8
			},
			new int[4]
			{
				1,
				1,
				1,
				2
			},
			new int[4]
			{
				1,
				1,
				1,
				2
			},
			new int[4]
			{
				1,
				1,
				1,
				2
			},
			new int[4]
			{
				1,
				1,
				1,
				2
			},
			new int[4]
			{
				1,
				1,
				1,
				2
			}
		};
		int[] value3 = new int[3]
		{
			1000,
			4000,
			5000
		};
		int num = 12000;
		int num2 = 1000;
		dictionary.Add("photoNumber", value);
		dictionary.Add("photos", value2);
		dictionary.Add("totalWin", value3);
		dictionary.Add("credit", num);
		dictionary.Add("totalBet", num2);
		return JsonFx.Json.JsonReader.Deserialize(JsonFx.Json.JsonWriter.Serialize(dictionary)) as Dictionary<string, object>;
	}

	private void _devInit()
	{
		HideAllViewButLoading();
		STWM_MB_Singleton<STWM_LoadingViewController>.GetInstance().Hide();
		if (devSceneType == STWM_DevSceneType.DiceGame)
		{
			_makeFakeUser();
			_makeFakeDesk();
			STWM_MB_Singleton<STWM_DiceGameController>.GetInstance().Show();
			STWM_MB_Singleton<STWM_DiceGameController2>.GetInstance().InitGame(20000, 450);
		}
		else if (devSceneType == STWM_DevSceneType.MaryGame)
		{
			UnityEngine.Debug.Log(LayerMask.NameToLayer("MaryGame"));
			UnityEngine.Debug.Log(Camera.current);
			UnityEngine.Debug.Log(Camera.main);
			Camera component = GameObject.Find("/Canvas/UICamera").GetComponent<Camera>();
			component.cullingMask |= 1 << LayerMask.NameToLayer("MaryGame");
			STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().Show();
			_makePrepareMaryGame();
			STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().StartGame(990);
		}
		else if (devSceneType == STWM_DevSceneType.Lobby)
		{
			_makeFakeUser();
			UnityEngine.Debug.LogError("======大厅初始化完毕=====");
			STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().Show();
		}
		else if (devSceneType == STWM_DevSceneType.Options)
		{
			STWM_MB_Singleton<STWM_OptionsController>.GetInstance().onItemReturn = delegate
			{
				UnityEngine.Debug.Log("onItemReturn");
			};
			STWM_MB_Singleton<STWM_OptionsController>.GetInstance().onItemSettings = delegate
			{
				UnityEngine.Debug.Log("onItemSettings");
			};
			STWM_MB_Singleton<STWM_OptionsController>.GetInstance().onItemBank = delegate
			{
				UnityEngine.Debug.Log("onItemBank");
			};
			STWM_MB_Singleton<STWM_OptionsController>.GetInstance().Show();
		}
		else if (devSceneType == STWM_DevSceneType.Settings)
		{
			STWM_MB_Singleton<STWM_SettingsController>.GetInstance().Show();
		}
		else if (devSceneType == STWM_DevSceneType.ScoreBank)
		{
			_makeFakeUser();
			_makeFakeDesk();
			STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().Show();
			STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().InitBank();
		}
		else if (devSceneType == STWM_DevSceneType.AlertDialog)
		{
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog("我是开发者消息!!!\n下方应该有一个确认按钮。");
			}
			else
			{
				STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog("我是开发者消息!!!\n下方应该有一个好按钮和一个取消按钮。", showOkCancel: true);
			}
		}
		else
		{
			_init();
		}
	}
}
