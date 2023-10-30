using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class USW_GameManager : USW_MB_Singleton<USW_GameManager>
{
	public bool isDevelopment;

	public bool isEnglish;

	public USW_DevSceneType devSceneType;

	private bool _isDirectLoad;

	private bool _isReconnecting;

	public Image_Prefab ImagePani;

	private Dictionary<string, Sprite> AniSprlist = new Dictionary<string, Sprite>();

	private Dictionary<string, Sprite> Spritelist = new Dictionary<string, Sprite>();

	[SerializeField]
	private GameObject _touchForbid;

	private bool _last_isEnglish;

	public Dictionary<string, object> _lockDic;

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

	public bool HasDesk => USW_MB_Singleton<USW_LobbyViewController>.GetInstance().HasDesk;

	private void Awake()
	{
		_last_isEnglish = isEnglish;
		_isReconnecting = false;
		if (USW_MB_Singleton<USW_GameManager>._instance != null)
		{
			USW_MB_Singleton<USW_GameManager>.GetInstance().InitSingletonStep2();
			USW_MB_Singleton<USW_GameManager>.GetInstance().HideAllViewButLoading();
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		if (USW_MB_Singleton<USW_GameManager>._instance == null)
		{
			USW_MB_Singleton<USW_GameManager>.SetInstance(this);
			_isDirectLoad = !SceneManager.GetActiveScene().name.StartsWith("Loading");
			PreInit();
			if (_isDirectLoad)
			{
				InitSingletonStep2();
			}
		}
		foreach (Sprite item in ImagePani.Sprlist)
		{
			if (AniSprlist.ContainsKey(item.name))
			{
				item.name += "_";
			}
			AniSprlist.Add(item.name, item);
		}
		Cinstance<USW_Gcore>.Instance.Normallist = new List<int>();
		for (int i = 0; i < 15; i++)
		{
			Cinstance<USW_Gcore>.Instance.Normallist.Add(UnityEngine.Random.Range(1, 10));
		}
		Cinstance<USW_Gcore>.Instance.Result = Cinstance<USW_Gcore>.Instance.Normallist;
		UnityEngine.Debug.LogError("初始结果: " + JsonMapper.ToJson(Cinstance<USW_Gcore>.Instance.Result));
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
		USW_MB_Singleton<USW_LoadingViewController>.GetInstance().Hide();
		_init();
	}

	private void Update()
	{
		USW_MB_Singleton<USW_NetManager>.GetInstance().connectTimeCount += Time.deltaTime;
		if (USW_GVars.curView == "LoadingView")
		{
			return;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			UnityEngine.Debug.Log(string.Format("Esc: {0}", USW_LockManager.GetValue("Esc")));
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !USW_MB_Singleton<USW_LobbyViewController>.GetInstance()._IsAning && !USW_LockManager.IsLocked("Esc") && !USW_LockManager.IsLocked("Quit"))
		{
			if (USW_GVars.curView == "RoomSelectionView" || USW_GVars.curView == "DeskSelectinView")
			{
				Handle_BtnReturn();
			}
			else if ((USW_GVars.curView == "DiceGame" || USW_GVars.curView == "MajorGame") && !USW_MB_Singleton<USW_MaryGameController>.GetInstance().bEnterMary)
			{
				Handle_ItemReturn();
			}
		}
		if (USW_GVars.lockOnePoint)
		{
			USW_GVars.lockOnePoint = false;
		}
	}

	public Sprite Getanisprite(string spritename)
	{
		Sprite result = null;
		if (AniSprlist.ContainsKey(spritename))
		{
			result = AniSprlist[spritename];
		}
		return result;
	}

	public Sprite GetSprite(string spritename)
	{
		Sprite result = null;
		if (Spritelist.ContainsKey(spritename))
		{
			result = Spritelist[spritename];
		}
		return result;
	}

	public void PreInit()
	{
		_lockDic = new Dictionary<string, object>();
		_initSingletonStep1();
	}

	private void _init()
	{
		UnityEngine.Debug.Log("_init");
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("heart", delegate
		{
		});
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("netDown", Handle_NetDown);
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("userLogin", HandleNetMsg_UserLogin);
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("reLogin", HandleNetMsg_ReLogin);
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("overflow", HandleNetMsg_Overflow);
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("levelUp", HandleNetMsg_LevelUp);
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("quitToLogin", HandleNetMsg_QuitToLogin);
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("quitToRoom", HandleNetMsg_QuitToRoom);
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("syncGold", HandleNetMsg_SyncGold);
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("checkVersion", HandleNetMsg_CheckVersion);
		if (!USW_MB_Singleton<USW_NetManager>.GetInstance().IsMethodRegistered("updateRoomInfo"))
		{
			USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("updateRoomInfo", delegate
			{
			});
		}
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("userAward", HandleNetMsg_UserAward);
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("gameGold", HandleNetMsg_GameGold);
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("sendNotice", HandleNetMsg_Notice);
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("notice", HandleNetMsg_Notice);
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("timeout", delegate(object[] args)
		{
			UnityEngine.Debug.Log(USW_LogHelper.Magenta("timeout>  id: " + (int)args[0]));
		});
		USW_MB_Singleton<USW_NetManager>.GetInstance().RegisterHandler("heart timeout", delegate
		{
			UnityEngine.Debug.Log(USW_LogHelper.Magenta("heart timeout"));
			Handle_NetDown(new object[1]
			{
				new Exception("heart timeout")
			});
		});
		USW_MB_Singleton<USW_LoadingViewController>.GetInstance().Show();
		if (!_isDirectLoad)
		{
			USW_MB_Singleton<USW_LoadingViewController>.GetInstance().loadLevelFinishAction += _onloadLevelFinish;
			_coLoading = StartCoroutine(USW_MB_Singleton<USW_LoadingViewController>.GetInstance().LoadingAni2());
		}
		else
		{
			_coLoading = StartCoroutine(USW_MB_Singleton<USW_LoadingViewController>.GetInstance().LoadingAni());
		}
		if (USW_MB_Singleton<USW_LoginController>.GetInstance() == null || !USW_MB_Singleton<USW_LoginController>.GetInstance().IsShow())
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
		USW_I2LocalizeText[] array = UnityEngine.Object.FindObjectsOfType<USW_I2LocalizeText>();
		USW_I2LocalizeText[] array2 = array;
		foreach (USW_I2LocalizeText uSW_I2LocalizeText in array2)
		{
			uSW_I2LocalizeText.Refresh();
		}
		USW_I2LocalizeImage[] array3 = UnityEngine.Object.FindObjectsOfType<USW_I2LocalizeImage>();
		USW_I2LocalizeImage[] array4 = array3;
		foreach (USW_I2LocalizeImage uSW_I2LocalizeImage in array4)
		{
			uSW_I2LocalizeImage.Refresh();
		}
	}

	public void HideAllViewButLoading()
	{
		UnityEngine.Debug.Log("HideAllViewButLoading");
		USW_MB_Singleton<USW_LobbyViewController>.GetInstance().Hide();
		USW_MB_Singleton<USW_MajorGameController>.GetInstance().Hide();
		USW_MB_Singleton<USW_DiceGameController2>.GetInstance().Hide();
		USW_MB_Singleton<USW_HUDController>.GetInstance().Hide();
		USW_MB_Singleton<USW_OptionsController>.GetInstance().Hide();
		USW_MB_Singleton<USW_SettingsController>.GetInstance().Hide();
		USW_MB_Singleton<USW_ScoreBank>.GetInstance().Hide();
		USW_MB_Singleton<USW_MaryMovieController>.GetInstance().Hide();
		USW_MB_Singleton<USW_NoticeController>.GetInstance().Hide();
		if (!_lockDic.GetBoolValue("HandleNetDown") && !_lockDic.GetBoolValue("Alert"))
		{
			USW_MB_Singleton<USW_AlertDialog>.GetInstance().Hide();
		}
	}

	private void _initSingletonStep1()
	{
		UnityEngine.Debug.Log("_intSingletonStep1");
		Transform transform = GameObject.Find("Canvas").transform;
		USW_MB_Singleton<USW_LoadingViewController>.SetInstanceByGameObject(transform.Find("Mask/Loading").gameObject).PreInit();
		USW_MB_Singleton<USW_AlertDialog>.SetInstanceByGameObject(transform.Find("Mask/AlertDialog").gameObject).PreInit();
		Transform transform2 = transform.Find("LoginPanel");
		if (transform2 != null)
		{
			USW_MB_Singleton<USW_LoginController>.SetInstanceByGameObject(transform2.gameObject).PreInit();
		}
		USW_MB_Singleton<USW_LoadingViewController>.GetInstance().loadingFinishAction += _onLoadingFinish;
	}

	public void InitSingletonStep2()
	{
		UnityEngine.Debug.Log("_intSingletonStep2");
		Transform transform = GameObject.Find("Canvas").transform;
		_audio = GameObject.Find("Main Camera").GetComponent<AudioListener>();
		_audio.enabled = false;
		if (_isDirectLoad)
		{
			_audio.enabled = true;
		}
		USW_SoundManager.Instance = GameObject.Find("SoundManager").GetComponent<USW_SoundManager>();
		USW_MB_Singleton<USW_LobbyViewController>.SetInstanceByGameObject(transform.Find("Mask/Lobby").gameObject).PreInit();
		USW_MB_Singleton<USW_HeadViewController>.SetInstanceByGameObject(transform.Find("Mask/Lobby/Title").gameObject).PreInit();
		USW_MB_Singleton<USW_MajorGameController>.SetInstanceByGameObject(transform.Find("Mask/Major").gameObject).PreInit();
		USW_MB_Singleton<USW_DiceGameController2>.SetInstanceByGameObject(transform.Find("Mask/Gameble").gameObject).PreInit();
		USW_MB_Singleton<USW_HUDController>.SetInstanceByGameObject(transform.Find("Mask/HUD").gameObject).PreInit();
		USW_MB_Singleton<USW_OptionsController>.SetInstanceByGameObject(transform.Find("Mask/Options").gameObject).PreInit();
		USW_MB_Singleton<USW_SettingsController>.SetInstanceByGameObject(transform.Find("Mask/Setting").gameObject).PreInit();
		USW_MB_Singleton<USW_ScoreBank>.SetInstanceByGameObject(transform.Find("Mask/InOut").gameObject).PreInit();
		USW_MB_Singleton<USW_NoticeController>.SetInstanceByGameObject(transform.Find("Mask/Notice").gameObject).PreInit();
		USW_MB_Singleton<USW_DiceGameController2>.GetInstance().onExitAction = _onDiceExit;
		USW_MB_Singleton<USW_SettingsController>.GetInstance().Init();
		USW_MB_Singleton<USW_HeadViewController>.GetInstance().btnReturnAction = Handle_BtnReturn;
		USW_MB_Singleton<USW_OptionsController>.GetInstance().onItemReturn = Handle_ItemReturn;
	}

	private void _prepareInitData()
	{
		USW_GVars.versionCode = "9.9.1";
		UnityEngine.Debug.Log("Application.Version: " + Application.version + "  USW_GVars.versionCode: " + USW_GVars.versionCode);
		USW_GVars.IPPort = 10047;
		UseDebugData();
		if (USW_GVars.language != "zh" && USW_GVars.language != "en")
		{
			UnityEngine.Debug.LogWarning("ZH2_GVars.language is not correct: " + USW_GVars.language);
			USW_GVars.language = "zh";
		}
		UnityEngine.Debug.LogError("链接地址: " + USW_GVars.IPAddress + " 端口: " + USW_GVars.IPPort);
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
		USW_GVars.username = ZH2_GVars.username;
		USW_GVars.pwd = ZH2_GVars.pwd;
		USW_GVars.IPAddress = ZH2_GVars.IPAddress_Game;
		USW_GVars.language = "zh";
		USW_GVars.IPPort = 10052;
	}

	private IEnumerator _connectAndLoginCoroutine()
	{
		m_hasVersionChecked = false;
		UnityEngine.Debug.Log($"_isReconnecting: {_isReconnecting}, isConnected: {USW_MB_Singleton<USW_NetManager>.GetInstance().isConnected}, isReady: {USW_MB_Singleton<USW_NetManager>.GetInstance().isReady}");
		if (!_hasLogined)
		{
			USW_MB_Singleton<USW_NetManager>.GetInstance().connectMaxTimes = 10;
			USW_MB_Singleton<USW_NetManager>.GetInstance().connectMaxTimeout = 5f;
		}
		if (!_isReconnecting)
		{
			_prepareInitData();
		}
		else
		{
			USW_MB_Singleton<USW_NetManager>.GetInstance().Disconnect();
			yield return new WaitForSeconds(0.5f);
		}
		if (!_hasLogined)
		{
			USW_GVars.IPAddress = USW_CheckIP.DoGetHostAddresses(USW_GVars.IPAddress);
		}
		USW_MB_Singleton<USW_NetManager>.GetInstance().Connect(USW_GVars.IPAddress, USW_GVars.IPPort);
		UnityEngine.Debug.Log("连接IP" + USW_GVars.IPAddress + " wait connected !");
		while (!USW_MB_Singleton<USW_NetManager>.GetInstance().isConnected)
		{
			yield return null;
		}
		while (!m_hasVersionChecked)
		{
			yield return null;
		}
		USW_MB_Singleton<USW_NetManager>.GetInstance().SendPublicKey();
		while (!USW_MB_Singleton<USW_NetManager>.GetInstance().isReady)
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
		if (!_hasLogined)
		{
			UnityEngine.Debug.Log(USW_LogHelper.Magenta("_loadingLoginTimeout"));
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
		UnityEngine.Debug.Log(USW_LogHelper.Magenta("QuitToHallLogin"));
		SetTouchEnable(isEnable: false, "QuitToHallLogin");
		Send_QuitGame();
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
				USW_LockManager.UnLock("Esc", USW_LockManager.GetValue("Esc") < 10);
			}
			else
			{
				USW_LockManager.Lock("Esc");
			}
		}
		UnityEngine.Debug.Log($"SetTouchEnable> [{USW_LogHelper.Key(isEnable)}], reason: [{USW_LogHelper.Key(reason)}]");
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
		string content = (USW_GVars.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge";
		USW_LockManager.Lock("Quit");
		PrepareQuitGame();
		USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			QuitToHallGame();
		});
	}

	public void Send_Login()
	{
		object[] args = new object[2]
		{
			USW_GVars.username,
			USW_GVars.pwd
		};
		USW_MB_Singleton<USW_NetManager>.GetInstance().Send("userService/userLogin", args);
	}

	private void OnApplicationQuit()
	{
		Send_QuitGame();
	}

	public void Send_QuitGame()
	{
		object[] args = new object[0];
		USW_MB_Singleton<USW_NetManager>.GetInstance().Send("userService/quitGame", args);
		USW_LockManager.Lock("Quit");
		SetTouchEnable(isEnable: false, "Send_QuitGame");
	}

	public void HandleNetMsg_ReLogin(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string[] array2;
		if (USW_GVars.language == "zh")
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
			USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog(array4[num2], showOkCancel: false, delegate
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
			USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog(array4[num4], showOkCancel: false, delegate
			{
				QuitToHallGame();
			});
			return;
		}
		int num5 = (int)dictionary["uiid"];
		UnityEngine.Debug.Log("uiid: " + num5);
		if (num5 == 1)
		{
			USW_GVars.user = USW_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
			if (USW_LockManager.IsLocked("btn_room"))
			{
				USW_LockManager.UnLock("btn_room");
				SetTouchEnable(isEnable: true, "reconnect success");
			}
		}
		if (num5 == 2)
		{
			object[] array5 = (object[])dictionary["roomInfo"];
			int num6 = array5.Length;
			USW_Desk[] array6 = new USW_Desk[num6];
			for (int i = 0; i < num6; i++)
			{
				array6[i] = USW_Desk.CreateWithDic((Dictionary<string, object>)array5[i]);
			}
			if (USW_LockManager.IsLocked("EnterDesk"))
			{
				USW_LockManager.UnLock("EnterDesk");
				SetTouchEnable(isEnable: true, "reconnect success");
			}
			if (USW_LockManager.IsLocked("btn_room"))
			{
				USW_LockManager.UnLock("btn_room");
				SetTouchEnable(isEnable: true, "reconnect success");
			}
		}
		if (num5 == 3)
		{
			Dictionary<string, object> dictionary2 = dictionary["deskInfo"] as Dictionary<string, object>;
			int credit = (int)dictionary2["userScore"];
			int num7 = (int)dictionary2["userGold"];
			USW_MB_Singleton<USW_MajorGameController>.GetInstance().JudgeResetGame(credit);
		}
		if (USW_MB_Singleton<USW_ReconnectHint>.GetInstance() != null)
		{
			USW_MB_Singleton<USW_ReconnectHint>.GetInstance().Hide();
		}
		USW_MB_Singleton<USW_NetManager>.GetInstance().isLogined = true;
		if (_coKeepHeart != null)
		{
			StopCoroutine(_coKeepHeart);
		}
		_coKeepHeart = StartCoroutine(USW_MB_Singleton<USW_NetManager>.GetInstance().KeepHeart());
		USW_MB_Singleton<USW_NetManager>.GetInstance().connectCount = 0;
	}

	public void HandleNetMsg_UserLogin(object[] args)
	{
		UnityEngine.Debug.Log(USW_LogHelper.NetHandle("HandleNetMsg_UserLogin"));
		_hasLogined = true;
		USW_MB_Singleton<USW_NetManager>.GetInstance().connectMaxTimes = 50;
		USW_MB_Singleton<USW_NetManager>.GetInstance().connectMaxTimeout = 30f;
		USW_MB_Singleton<USW_NetManager>.GetInstance().connectMaxTimes = 30;
		USW_MB_Singleton<USW_NetManager>.GetInstance().connectMaxTimeout = 20f;
		if (_coLogin != null)
		{
			StopCoroutine(_coLogin);
			_coLogin = null;
		}
		if (USW_MB_Singleton<USW_ReconnectHint>.GetInstance() != null)
		{
			USW_MB_Singleton<USW_ReconnectHint>.GetInstance().Hide();
		}
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			MonoBehaviour.print(item.Key + "," + item.Value.ToString() + "----");
		}
		if ((bool)dictionary["success"])
		{
			USW_GVars.user = USW_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
			if (dictionary.ContainsKey("repeat") && (bool)dictionary["repeat"])
			{
				if (dictionary.ContainsKey("roomId"))
				{
					if (USW_LockManager.IsLocked("EnterDesk"))
					{
						USW_LockManager.UnLock("EnterDesk");
						SetTouchEnable(isEnable: true, "reconnect success");
					}
					int num = (int)dictionary["roomId"];
					if (dictionary.ContainsKey("deskId"))
					{
						int num2 = (int)dictionary["deskId"];
						Dictionary<string, object> dictionary2 = dictionary["seat"] as Dictionary<string, object>;
						int num3 = (int)dictionary2["userScore"];
						int num4 = (int)dictionary2["userGold"];
						UnityEngine.Debug.Log(USW_LogHelper.Magenta("roomId: {0},deskId: {1},userScore: {2},userGold: {3}", num, num2, num3, num4));
						USW_MB_Singleton<USW_MajorGameController>.GetInstance().JudgeResetGame(num3);
					}
					else
					{
						UnityEngine.Debug.Log(USW_LogHelper.Magenta("roomId: {0}", num));
					}
				}
				else
				{
					UnityEngine.Debug.Log(USW_LogHelper.Magenta("not in room"));
					if (USW_LockManager.IsLocked("btn_room"))
					{
						USW_LockManager.UnLock("btn_room");
						SetTouchEnable(isEnable: true, "reconnect success");
					}
				}
			}
			else
			{
				UnityEngine.Debug.Log(USW_LogHelper.Magenta("not reconnect login"));
			}
			USW_MB_Singleton<USW_NetManager>.GetInstance().isLogined = true;
			if (_coKeepHeart != null)
			{
				StopCoroutine(_coKeepHeart);
			}
			_coKeepHeart = StartCoroutine(USW_MB_Singleton<USW_NetManager>.GetInstance().KeepHeart());
			USW_MB_Singleton<USW_NetManager>.GetInstance().connectCount = 0;
		}
		else
		{
			int code = (int)dictionary["msgCode"];
			Action callback = delegate
			{
				QuitToHallLogin();
				_lockDic["Alert"] = false;
			};
			StartCoroutine(USW_Utils.DelayCall(0f, delegate
			{
				if (_coLogin != null)
				{
					StopCoroutine(_coLogin);
				}
			}));
			string errorMsg = USW_ErrorCode.GetErrorMsg(code, USW_GVars.language.Equals("zh"));
			_lockDic["Alert"] = true;
			USW_LockManager.Lock("Quit");
			SetTouchEnable(isEnable: true, "quit alert", includeEsc: false);
			PrepareQuitGame();
			USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog(errorMsg, showOkCancel: false, callback);
		}
	}

	public int GetUIId()
	{
		int result = 0;
		if (USW_GVars.curView == "LoadingView")
		{
			result = 0;
		}
		else if (USW_GVars.curView == "RoomSelectionView")
		{
			result = 1;
		}
		else if (USW_GVars.curView == "DeskSelectionView")
		{
			result = 2;
		}
		else if (USW_GVars.curView == "MajorGame" || USW_GVars.curView == "DiceGame" || USW_GVars.curView == "MaryGame")
		{
			result = 3;
		}
		return result;
	}

	public void HandleNetMsg_LevelUp(object[] args)
	{
		UnityEngine.Debug.Log(USW_LogHelper.NetHandle("HandleNetMsg_LevelUp"));
		int levelInfo = USW_GVars.user.level + 1;
		USW_MB_Singleton<USW_HeadViewController>.GetInstance().SetLevelInfo(levelInfo);
	}

	public void HandleNetMsg_Overflow(object[] args)
	{
		UnityEngine.Debug.Log(USW_LogHelper.NetHandle("HandleNetMsg_Overflow"));
		USW_LockManager.Lock("Overflow");
		if (!USW_LockManager.IsLocked("Delay_Overflow"))
		{
			OverflowProcess();
		}
	}

	public void HandleNetMsg_QuitToLogin(object[] args)
	{
		UnityEngine.Debug.Log(USW_LogHelper.NetHandle("HandleNetMsg_QuitToLogin"));
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
		USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog(empty, showOkCancel: false, delegate
		{
			QuitToHallLogin();
			_lockDic["Alert"] = false;
		});
	}

	public void PrepareQuitGame()
	{
		_lockDic["Alert"] = true;
		USW_LockManager.Lock("Quit");
		if (USW_MB_Singleton<USW_ReconnectHint>.GetInstance() != null)
		{
			USW_MB_Singleton<USW_ReconnectHint>.GetInstance().Hide();
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
		USW_MB_Singleton<USW_NetManager>.GetInstance().Disconnect();
	}

	public void HandleNetMsg_Notice(object[] args)
	{
		UnityEngine.Debug.Log(USW_LogHelper.NetHandle("HandleNetMsg_Notice"));
		string text = args[0] as string;
		UnityEngine.Debug.Log(USW_LogHelper.NetHandle($"msg: {text}"));
		USW_NoticeController instance = USW_MB_Singleton<USW_NoticeController>.GetInstance();
		if (instance != null)
		{
			instance.AddMessage(text);
		}
	}

	public void HandleNetMsg_QuitToRoom(object[] args)
	{
		UnityEngine.Debug.Log(USW_LogHelper.NetHandle("HandleNetMsg_QuitToRoom"));
		int num = (int)args[0];
		bool flag = false;
		string empty = string.Empty;
		switch (num)
		{
		case 1:
		{
			string text2 = (USW_GVars.language == "zh") ? "桌子已经被系统删除" : "Game table not exist";
			empty = ((USW_GVars.language == "zh") ? "网络异常，已自动结算，请重新进入游戏" : "Network error, credits have been automatically settled");
			flag = true;
			break;
		}
		case 2:
		{
			string text = (USW_GVars.language == "zh") ? "桌子参数已修改" : "Network is not stable please re-enter";
			empty = ((USW_GVars.language == "zh") ? "网络异常，已自动结算，请重新进入游戏" : "Network error, credits have been automatically settled");
			break;
		}
		case 3:
			empty = ((USW_GVars.language == "zh") ? "由于您长时间未游戏，已自动退出游戏" : "Game quit automatically because you did not play for a long time");
			break;
		case 4:
			empty = ((USW_GVars.language == "zh") ? "桌子爆机" : "Your account is blasting, please excharge");
			break;
		default:
			empty = $"QuitToRoom> unknown type: [{num}]";
			break;
		}
		USW_MB_Singleton<USW_MajorGameController>.GetInstance().ResetGame();
		USW_MB_Singleton<USW_MaryGameController>.GetInstance().ResetGame();
		USW_MB_Singleton<USW_MajorGameController>.GetInstance().Hide();
		USW_MB_Singleton<USW_AlertDialog>.GetInstance().Hide();
		USW_MB_Singleton<USW_MaryMovieController>.GetInstance().Hide();
		USW_MB_Singleton<USW_LobbyViewController>.GetInstance().Show();
		if (flag)
		{
			USW_MB_Singleton<USW_LobbyViewController>.GetInstance().curDeskIndex = 0;
			USW_MB_Singleton<USW_LobbyViewController>.GetInstance().UpdateAllDeskWidgets();
		}
		_lockDic["Alert"] = true;
		SetTouchEnable(isEnable: true, "QuitToRoom");
		USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog(empty, showOkCancel: false, delegate
		{
			_lockDic["Alert"] = false;
			USW_MB_Singleton<USW_AlertDialog>.GetInstance().isTouchForbidden = false;
		});
	}

	public void HandleNetMsg_SyncGold(object[] args)
	{
		UnityEngine.Debug.Log(USW_LogHelper.NetHandle("HandleNetMsg_SyncGold"));
		if (USW_GVars.user != null)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			USW_GVars.user.expeGold = (int)dictionary["expeGold"];
			USW_GVars.user.gameGold = (int)dictionary["gameGold"];
			if (!(USW_GVars.curView == "LoadingView"))
			{
				USW_MB_Singleton<USW_HeadViewController>.GetInstance().UpdateView();
			}
		}
	}

	public void HandleNetMsg_UserAward(object[] args)
	{
		UnityEngine.Debug.Log(USW_LogHelper.NetHandle("HandleNetMsg_UserAward"));
		int num = (int)args[0];
		string content = (USW_GVars.language == "zh") ? $"恭喜您获得客服赠送的{num}游戏币!" : $"Congratulations , you get {num} coins from customer service!";
		USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
		});
	}

	public void HandleNetMsg_GameGold(object[] args)
	{
		UnityEngine.Debug.Log(USW_LogHelper.NetHandle("HandleNetMsg_GameGold"));
		if (USW_GVars.user != null)
		{
			int gameGold = (int)args[0];
			USW_GVars.user.gameGold = gameGold;
			if (USW_MB_Singleton<USW_HeadViewController>.GetInstance() != null)
			{
				USW_MB_Singleton<USW_HeadViewController>.GetInstance().UpdateView();
			}
		}
	}

	public void HandleNetMsg_CheckVersion(object[] args)
	{
		UnityEngine.Debug.Log(USW_LogHelper.NetHandle("HandleNetMsg_CheckVersion"));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary = (args[0] as Dictionary<string, object>);
		if (!(bool)dictionary["success"])
		{
		}
		if ((bool)dictionary["haveNewVersion"])
		{
			Dictionary<string, object> dictionary2 = dictionary["downloadAddress"] as Dictionary<string, object>;
			string downloadadress = (string)dictionary2["downloadWindows"];
			string text = (string)dictionary2["downloadAndroid"];
			USW_LockManager.Lock("Quit");
			USW_IOSGameStart.GetSingleton().UpdateGameVesion(downloadadress);
		}
		else
		{
			m_hasVersionChecked = true;
		}
	}

	public void AfterReconnect_DoUnlock()
	{
		USW_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
	}

	public void Handle_NetDown(object[] args)
	{
		UnityEngine.Debug.Log("Handle_NetDown");
		UnityEngine.Debug.Log(string.Format("curView: {0}, isReady: {1}, _lockDic.HandleNetDown: {2}, connectCount: {3}", USW_GVars.curView, USW_MB_Singleton<USW_NetManager>.GetInstance().isReady, _lockDic.GetBoolValue("HandleNetDown"), USW_MB_Singleton<USW_NetManager>.GetInstance().connectCount));
		if (USW_LockManager.IsLocked("Overflow") || USW_LockManager.IsLocked("Quit"))
		{
			return;
		}
		if (USW_MB_Singleton<USW_NetManager>.GetInstance().isReady)
		{
			if (USW_MB_Singleton<USW_ReconnectHint>.GetInstance() != null)
			{
				USW_MB_Singleton<USW_ReconnectHint>.GetInstance().Hide();
			}
		}
		else
		{
			if (USW_LockManager.IsLocked("HandleNetDown_Quit"))
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
			UnityEngine.Debug.Log(ex);
			bool flag = false;
			if (USW_GVars.curView == "LoadingView" && flag)
			{
				string content = string.Format((USW_GVars.language == "zh") ? "网络连接已断开，错误代码为 [{0}]" : "Net Error [{0}]", (ex == null) ? "9999" : ex.Message);
				USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
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
			if ((USW_MB_Singleton<USW_NetManager>.GetInstance().connectCount > USW_MB_Singleton<USW_NetManager>.GetInstance().connectMaxTimes && USW_MB_Singleton<USW_NetManager>.GetInstance().connectTimeCount > USW_MB_Singleton<USW_NetManager>.GetInstance().connectMaxTimeout / 2f) || USW_MB_Singleton<USW_NetManager>.GetInstance().connectTimeCount > USW_MB_Singleton<USW_NetManager>.GetInstance().connectMaxTimeout)
			{
				_timeoutQuit();
				return;
			}
			_coLogin = StartCoroutine(_connectAndLoginCoroutine());
			if (USW_MB_Singleton<USW_ReconnectHint>.GetInstance() != null)
			{
				USW_MB_Singleton<USW_ReconnectHint>.GetInstance().Show();
			}
		}
	}

	public void Send_ReLogin()
	{
		USW_MB_Singleton<USW_NetManager>.GetInstance().Send("userService/reLogin", new object[3]
		{
			USW_GVars.username,
			USW_GVars.pwd,
			GetUIId()
		});
	}

	public void ChangeView(string newView)
	{
		UnityEngine.Debug.Log($"changeView> from [{USW_GVars.curView}] to [{newView}]");
		USW_GVars.curView = newView;
		if (USW_MB_Singleton<USW_NoticeController>.GetInstance() != null)
		{
			USW_MB_Singleton<USW_NoticeController>.GetInstance().ChangeScene();
		}
	}

	private void _timeoutQuit()
	{
		UnityEngine.Debug.Log($"connectCount: {USW_MB_Singleton<USW_NetManager>.GetInstance().connectCount}, connectTimeCount: {USW_MB_Singleton<USW_NetManager>.GetInstance().connectTimeCount}");
		UnityEngine.Debug.Log($"connectMaxTimes: {USW_MB_Singleton<USW_NetManager>.GetInstance().connectMaxTimes}, connectMaxTimeout: {USW_MB_Singleton<USW_NetManager>.GetInstance().connectMaxTimeout}");
		string content = (USW_GVars.language == "zh") ? "网络连接错误，请检查网络连接" : "Unable to connect to server，Please check your network connection";
		if (USW_MB_Singleton<USW_ReconnectHint>.GetInstance() != null)
		{
			USW_MB_Singleton<USW_ReconnectHint>.GetInstance().Hide();
		}
		USW_LockManager.Lock("HandleNetDown_Quit");
		PrepareQuitGame();
		USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			QuitToHallLogin();
			_lockDic["HandleNetDown"] = false;
		});
	}

	public void Handle_BtnReturn()
	{
		if (USW_GVars.curView == "RoomSelectionView")
		{
			USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog((USW_GVars.language == "zh") ? "是否退出程序？" : "Quit the game?", showOkCancel: true, delegate
			{
				StartCoroutine(USW_Utils.DelayCall(0.1f, delegate
				{
					QuitToHallGame();
				}));
			});
		}
		else if (USW_GVars.curView == "DeskSelectionView")
		{
			if (HasDesk)
			{
				USW_MB_Singleton<USW_LobbyViewController>.GetInstance().HideDeskViewAni();
				USW_MB_Singleton<USW_RoomSelectionViewController>.GetInstance().Show();
				USW_MB_Singleton<USW_LobbyViewController>.GetInstance().ResetRoomView();
				USW_MB_Singleton<USW_LobbyViewController>.GetInstance().Send_LeaveRoom();
			}
			else
			{
				USW_MB_Singleton<USW_LobbyViewController>.GetInstance().OnBtnReturn_Click();
			}
		}
	}

	public void Handle_ItemReturn()
	{
		if (!USW_LockManager.IsLocked("btn_options"))
		{
			USW_SoundManager.Instance.StopMaryAudio();
			USW_MB_Singleton<USW_HUDController>.GetInstance().HideRules();
			USW_MB_Singleton<USW_OptionsController>.GetInstance().Hide();
			USW_MB_Singleton<USW_SettingsController>.GetInstance().Hide();
			USW_MB_Singleton<USW_ScoreBank>.GetInstance().Hide();
			if (USW_GVars.curView == "DiceGame")
			{
				USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog((USW_GVars.language == "zh") ? "是否退出比倍？" : "Quit the Dice?", showOkCancel: true, delegate
				{
					USW_MB_Singleton<USW_DiceGameController2>.GetInstance().ExitGame();
				});
			}
			else if (USW_GVars.curView == "MajorGame")
			{
				USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog((USW_GVars.language == "zh") ? "是否退出游戏？" : "Quit the game?", showOkCancel: true, delegate
				{
					USW_SoundManager.Instance.PlayLobbyBGM();
					USW_MB_Singleton<USW_MajorGameController>.GetInstance().Hide();
					USW_MB_Singleton<USW_MajorGameController>.GetInstance().ExitGame();
					USW_MB_Singleton<USW_MajorGameController>.GetInstance().ResetGame();
					USW_MB_Singleton<USW_LobbyViewController>.GetInstance().Show();
				});
			}
		}
	}

	private void _onLoadingFinish()
	{
		UnityEngine.Debug.Log("_onLoadingFinish");
		USW_MB_Singleton<USW_LoadingViewController>.GetInstance().Hide();
		USW_MB_Singleton<USW_LobbyViewController>.GetInstance().Show();
		USW_SoundManager.Instance.PlayLobbyBGM();
		_audio.enabled = true;
		Transform transform = GameObject.Find("Canvas").transform;
		USW_MB_Singleton<USW_AlertDialog>.SetInstanceByGameObject(transform.Find("Mask/AlertDialog").gameObject, force: true).PreInit();
		Transform transform2 = transform.Find("Loading");
		if ((bool)transform2)
		{
			transform2.gameObject.SetActive(value: false);
		}
		USW_MB_Singleton<USW_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	private void _onloadLevelFinish()
	{
		UnityEngine.Debug.Log("_onloadLevelFinish");
		_coLoading = null;
		InitSingletonStep2();
	}

	private void _onDiceExit()
	{
		USW_MB_Singleton<USW_DiceGameController2>.GetInstance().Hide();
		StartCoroutine(waitCurtainPullDown());
	}

	private IEnumerator waitCurtainPullDown()
	{
		yield return new WaitForSeconds(0.6f);
		USW_MB_Singleton<USW_MajorGameController>.GetInstance().Show();
		USW_MB_Singleton<USW_MajorGameController>.GetInstance().OnDiceExit();
		yield return new WaitForSeconds(0.25f);
		Cinstance<USW_Gcore>.Instance.IsDic = false;
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
		USW_GVars.user = USW_User.CreateWithDic(dictionary);
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
		USW_GVars.desk = USW_Desk.CreateWithDic(dictionary);
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
		USW_MB_Singleton<USW_MaryGameController>.GetInstance().PrepareGame(times, credit, totalBet, photoNumberArray, photosArray, totalWinArray);
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
		USW_MB_Singleton<USW_LoadingViewController>.GetInstance().Hide();
		if (devSceneType == USW_DevSceneType.DiceGame)
		{
			_makeFakeUser();
			_makeFakeDesk();
			USW_MB_Singleton<USW_DiceGameController2>.GetInstance().Show();
			USW_MB_Singleton<USW_DiceGameController2>.GetInstance().InitGame(20000, 450);
		}
		else if (devSceneType == USW_DevSceneType.MaryGame)
		{
			UnityEngine.Debug.Log(LayerMask.NameToLayer("MaryGame"));
			UnityEngine.Debug.Log(Camera.current);
			UnityEngine.Debug.Log(Camera.main);
			Camera component = GameObject.Find("/Canvas/UICamera").GetComponent<Camera>();
			component.cullingMask |= 1 << LayerMask.NameToLayer("MaryGame");
			USW_MB_Singleton<USW_MaryGameController>.GetInstance().Show();
			_makePrepareMaryGame();
			USW_MB_Singleton<USW_MaryGameController>.GetInstance().StartGame(990);
		}
		else if (devSceneType == USW_DevSceneType.Lobby)
		{
			_makeFakeUser();
			USW_MB_Singleton<USW_LobbyViewController>.GetInstance().Show();
		}
		else if (devSceneType == USW_DevSceneType.Options)
		{
			USW_MB_Singleton<USW_OptionsController>.GetInstance().onItemReturn = delegate
			{
				UnityEngine.Debug.Log("onItemReturn");
			};
			USW_MB_Singleton<USW_OptionsController>.GetInstance().onItemSettings = delegate
			{
				UnityEngine.Debug.Log("onItemSettings");
			};
			USW_MB_Singleton<USW_OptionsController>.GetInstance().onItemBank = delegate
			{
				UnityEngine.Debug.Log("onItemBank");
			};
			USW_MB_Singleton<USW_OptionsController>.GetInstance().Show();
		}
		else if (devSceneType == USW_DevSceneType.Settings)
		{
			USW_MB_Singleton<USW_SettingsController>.GetInstance().Show();
		}
		else if (devSceneType == USW_DevSceneType.ScoreBank)
		{
			_makeFakeUser();
			_makeFakeDesk();
			USW_MB_Singleton<USW_ScoreBank>.GetInstance().Show();
			USW_MB_Singleton<USW_ScoreBank>.GetInstance().InitBank();
		}
		else if (devSceneType == USW_DevSceneType.AlertDialog)
		{
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog("我是开发者消息!!!\n下方应该有一个确认按钮。");
			}
			else
			{
				USW_MB_Singleton<USW_AlertDialog>.GetInstance().ShowDialog("我是开发者消息!!!\n下方应该有一个好按钮和一个取消按钮。", showOkCancel: true);
			}
		}
		else
		{
			_init();
		}
	}
}
