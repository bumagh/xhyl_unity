using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ESP_GameManager : ESP_MB_Singleton<ESP_GameManager>
{
	public bool isDevelopment;

	public bool isEnglish;

	public ESP_DevSceneType devSceneType;

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

	public bool HasDesk => ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().HasDesk;

	private void Awake()
	{
		_last_isEnglish = isEnglish;
		_isReconnecting = false;
		if (ESP_MB_Singleton<ESP_GameManager>._instance != null)
		{
			ESP_MB_Singleton<ESP_GameManager>.GetInstance().InitSingletonStep2();
			ESP_MB_Singleton<ESP_GameManager>.GetInstance().HideAllViewButLoading();
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		if (ESP_MB_Singleton<ESP_GameManager>._instance == null)
		{
			ESP_MB_Singleton<ESP_GameManager>.SetInstance(this);
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
		Cinstance<ESP_Gcore>.Instance.Normallist = new List<int>();
		for (int i = 0; i < 15; i++)
		{
			Cinstance<ESP_Gcore>.Instance.Normallist.Add(UnityEngine.Random.Range(1, 10));
		}
		Cinstance<ESP_Gcore>.Instance.Result = Cinstance<ESP_Gcore>.Instance.Normallist;
		UnityEngine.Debug.LogError("初始结果: " + JsonMapper.ToJson(Cinstance<ESP_Gcore>.Instance.Result));
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
		ESP_MB_Singleton<ESP_LoadingViewController>.GetInstance().Hide();
		_init();
	}

	private void Update()
	{
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectTimeCount += Time.deltaTime;
		if (ESP_MySqlConnection.curView == "LoadingView")
		{
			return;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			UnityEngine.Debug.Log(string.Format("Esc: {0}", ESP_LockManager.GetValue("Esc")));
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance()._IsAning && !ESP_LockManager.IsLocked("Esc") && !ESP_LockManager.IsLocked("Quit"))
		{
			if (ESP_MySqlConnection.curView == "RoomSelectionView" || ESP_MySqlConnection.curView == "DeskSelectinView")
			{
				Handle_BtnReturn();
			}
			else if ((ESP_MySqlConnection.curView == "DiceGame" || ESP_MySqlConnection.curView == "MajorGame") && !ESP_MB_Singleton<ESP_MaryGameController>.GetInstance().bEnterMary)
			{
				Handle_ItemReturn();
			}
		}
		if (ESP_MySqlConnection.lockOnePoint)
		{
			ESP_MySqlConnection.lockOnePoint = false;
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
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("heart", delegate
		{
		});
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("netDown", Handle_NetDown);
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("userLogin", HandleNetMsg_UserLogin);
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("reLogin", HandleNetMsg_ReLogin);
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("overflow", HandleNetMsg_Overflow);
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("levelUp", HandleNetMsg_LevelUp);
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("quitToLogin", HandleNetMsg_QuitToLogin);
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("quitToRoom", HandleNetMsg_QuitToRoom);
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("syncGold", HandleNetMsg_SyncGold);
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("checkVersion", HandleNetMsg_CheckVersion);
		if (!ESP_MB_Singleton<ESP_NetManager>.GetInstance().IsMethodRegistered("updateRoomInfo"))
		{
			ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("updateRoomInfo", delegate
			{
			});
		}
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("userAward", HandleNetMsg_UserAward);
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("gameGold", HandleNetMsg_GameGold);
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("sendNotice", HandleNetMsg_Notice);
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("notice", HandleNetMsg_Notice);
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("timeout", delegate(object[] args)
		{
			UnityEngine.Debug.Log(ESP_LogHelper.Magenta("timeout>  id: " + (int)args[0]));
		});
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("heart timeout", delegate
		{
			UnityEngine.Debug.Log(ESP_LogHelper.Magenta("heart timeout"));
			Handle_NetDown(new object[1]
			{
				new Exception("heart timeout")
			});
		});
		ESP_MB_Singleton<ESP_LoadingViewController>.GetInstance().Show();
		if (!_isDirectLoad)
		{
			ESP_MB_Singleton<ESP_LoadingViewController>.GetInstance().loadLevelFinishAction += _onloadLevelFinish;
			_coLoading = StartCoroutine(ESP_MB_Singleton<ESP_LoadingViewController>.GetInstance().LoadingAni2());
		}
		else
		{
			_coLoading = StartCoroutine(ESP_MB_Singleton<ESP_LoadingViewController>.GetInstance().LoadingAni());
		}
		if (ESP_MB_Singleton<ESP_LoginController>.GetInstance() == null || !ESP_MB_Singleton<ESP_LoginController>.GetInstance().IsShow())
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
		ESP_I2LocalizeText[] array = UnityEngine.Object.FindObjectsOfType<ESP_I2LocalizeText>();
		ESP_I2LocalizeText[] array2 = array;
		foreach (ESP_I2LocalizeText eSP_I2LocalizeText in array2)
		{
			eSP_I2LocalizeText.Refresh();
		}
		ESP_I2LocalizeImage[] array3 = UnityEngine.Object.FindObjectsOfType<ESP_I2LocalizeImage>();
		ESP_I2LocalizeImage[] array4 = array3;
		foreach (ESP_I2LocalizeImage eSP_I2LocalizeImage in array4)
		{
			eSP_I2LocalizeImage.Refresh();
		}
	}

	public void HideAllViewButLoading()
	{
		UnityEngine.Debug.Log("HideAllViewButLoading");
		ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_MajorGameController>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_DiceGameController2>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_HUDController>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_OptionsController>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_SettingsController>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_ScoreBank>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_MaryMovieController>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_NoticeController>.GetInstance().Hide();
		if (!_lockDic.GetBoolValue("HandleNetDown") && !_lockDic.GetBoolValue("Alert"))
		{
			ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().Hide();
		}
	}

	private void _initSingletonStep1()
	{
		UnityEngine.Debug.Log("_intSingletonStep1");
		Transform transform = GameObject.Find("Canvas").transform;
		ESP_MB_Singleton<ESP_LoadingViewController>.SetInstanceByGameObject(transform.Find("Mask/Loading").gameObject).PreInit();
		ESP_MB_Singleton<ESP_AlertDialog>.SetInstanceByGameObject(transform.Find("Mask/AlertDialog").gameObject).PreInit();
		Transform transform2 = transform.Find("LoginPanel");
		if (transform2 != null)
		{
			ESP_MB_Singleton<ESP_LoginController>.SetInstanceByGameObject(transform2.gameObject).PreInit();
		}
		ESP_MB_Singleton<ESP_LoadingViewController>.GetInstance().loadingFinishAction += _onLoadingFinish;
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
		ESP_SoundManager.Instance = GameObject.Find("SoundManager").GetComponent<ESP_SoundManager>();
		ESP_MB_Singleton<ESP_LobbyViewController>.SetInstanceByGameObject(transform.Find("Mask/Lobby").gameObject).PreInit();
		ESP_MB_Singleton<ESP_HeadViewController>.SetInstanceByGameObject(transform.Find("Mask/Lobby/Title").gameObject).PreInit();
		ESP_MB_Singleton<ESP_MajorGameController>.SetInstanceByGameObject(transform.Find("Mask/Major").gameObject).PreInit();
		ESP_MB_Singleton<ESP_DiceGameController2>.SetInstanceByGameObject(transform.Find("Mask/Gameble").gameObject).PreInit();
		ESP_MB_Singleton<ESP_HUDController>.SetInstanceByGameObject(transform.Find("Mask/HUD").gameObject).PreInit();
		ESP_MB_Singleton<ESP_OptionsController>.SetInstanceByGameObject(transform.Find("Mask/Options").gameObject).PreInit();
		ESP_MB_Singleton<ESP_SettingsController>.SetInstanceByGameObject(transform.Find("Mask/Setting").gameObject).PreInit();
		ESP_MB_Singleton<ESP_ScoreBank>.SetInstanceByGameObject(transform.Find("Mask/InOut").gameObject).PreInit();
		ESP_MB_Singleton<ESP_NoticeController>.SetInstanceByGameObject(transform.Find("Mask/Notice").gameObject).PreInit();
		ESP_MB_Singleton<ESP_DiceGameController2>.GetInstance().onExitAction = _onDiceExit;
		ESP_MB_Singleton<ESP_SettingsController>.GetInstance().Init();
		ESP_MB_Singleton<ESP_HeadViewController>.GetInstance().btnReturnAction = Handle_BtnReturn;
		ESP_MB_Singleton<ESP_OptionsController>.GetInstance().onItemReturn = Handle_ItemReturn;
	}

	private void _prepareInitData()
	{
		ESP_MySqlConnection.versionCode = "9.9.1";
		UnityEngine.Debug.Log("Application.Version: " + Application.version + "  ESP_MySqlConnection.versionCode: " + ESP_MySqlConnection.versionCode);
		ESP_MySqlConnection.IPPort = 10047;
		UseDebugData();
		if (ESP_MySqlConnection.language != "zh" && ESP_MySqlConnection.language != "en")
		{
			UnityEngine.Debug.LogWarning("ZH2_GVars.language is not correct: " + ESP_MySqlConnection.language);
			ESP_MySqlConnection.language = "zh";
		}
		UnityEngine.Debug.LogError("链接地址: " + ESP_MySqlConnection.IPAddress + " 端口: " + ESP_MySqlConnection.IPPort);
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
		ESP_MySqlConnection.username = ZH2_GVars.username;
		ESP_MySqlConnection.pwd = ZH2_GVars.pwd;
		ESP_MySqlConnection.IPAddress = ZH2_GVars.IPAddress_Game;
		ESP_MySqlConnection.language = "zh";
		ESP_MySqlConnection.IPPort = 10050;
	}

	private IEnumerator _connectAndLoginCoroutine()
	{
		m_hasVersionChecked = false;
		UnityEngine.Debug.Log($"_isReconnecting: {_isReconnecting}, isConnected: {ESP_MB_Singleton<ESP_NetManager>.GetInstance().isConnected}, isReady: {ESP_MB_Singleton<ESP_NetManager>.GetInstance().isReady}");
		if (!_hasLogined)
		{
			ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectMaxTimes = 10;
			ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectMaxTimeout = 5f;
		}
		if (!_isReconnecting)
		{
			_prepareInitData();
		}
		else
		{
			ESP_MB_Singleton<ESP_NetManager>.GetInstance().Disconnect();
			yield return new WaitForSeconds(0.5f);
		}
		if (!_hasLogined)
		{
			ESP_MySqlConnection.IPAddress = ESP_CheckIP.DoGetHostAddresses(ESP_MySqlConnection.IPAddress);
		}
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().Connect(ESP_MySqlConnection.IPAddress, ESP_MySqlConnection.IPPort);
		UnityEngine.Debug.Log("连接IP" + ESP_MySqlConnection.IPAddress + " wait connected !");
		while (!ESP_MB_Singleton<ESP_NetManager>.GetInstance().isConnected)
		{
			yield return null;
		}
		while (!m_hasVersionChecked)
		{
			yield return null;
		}
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().SendPublicKey();
		while (!ESP_MB_Singleton<ESP_NetManager>.GetInstance().isReady)
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
			UnityEngine.Debug.Log(ESP_LogHelper.Magenta("_loadingLoginTimeout"));
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
		UnityEngine.Debug.Log(ESP_LogHelper.Magenta("QuitToHallLogin"));
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
				ESP_LockManager.UnLock("Esc", ESP_LockManager.GetValue("Esc") < 10);
			}
			else
			{
				ESP_LockManager.Lock("Esc");
			}
		}
		UnityEngine.Debug.Log($"SetTouchEnable> [{ESP_LogHelper.Key(isEnable)}], reason: [{ESP_LogHelper.Key(reason)}]");
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
		string content = (ESP_MySqlConnection.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge";
		ESP_LockManager.Lock("Quit");
		PrepareQuitGame();
		ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			QuitToHallGame();
		});
	}

	public void Send_Login()
	{
		object[] args = new object[2]
		{
			ESP_MySqlConnection.username,
			ESP_MySqlConnection.pwd
		};
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().Send("userService/userLogin", args);
	}

	private void OnApplicationQuit()
	{
		Send_QuitGame();
	}

	public void Send_QuitGame()
	{
		object[] args = new object[0];
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().Send("userService/quitGame", args);
		ESP_LockManager.Lock("Quit");
		SetTouchEnable(isEnable: false, "Send_QuitGame");
	}

	public void HandleNetMsg_ReLogin(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string[] array2;
		if (ESP_MySqlConnection.language == "zh")
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
			ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog(array4[num2], showOkCancel: false, delegate
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
			ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog(array4[num4], showOkCancel: false, delegate
			{
				QuitToHallGame();
			});
			return;
		}
		int num5 = (int)dictionary["uiid"];
		UnityEngine.Debug.Log("uiid: " + num5);
		if (num5 == 1)
		{
			ESP_MySqlConnection.user = ESP_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
			if (ESP_LockManager.IsLocked("btn_room"))
			{
				ESP_LockManager.UnLock("btn_room");
				SetTouchEnable(isEnable: true, "reconnect success");
			}
		}
		if (num5 == 2)
		{
			object[] array5 = (object[])dictionary["roomInfo"];
			int num6 = array5.Length;
			ESP_Desk[] array6 = new ESP_Desk[num6];
			for (int i = 0; i < num6; i++)
			{
				array6[i] = ESP_Desk.CreateWithDic((Dictionary<string, object>)array5[i]);
			}
			if (ESP_LockManager.IsLocked("EnterDesk"))
			{
				ESP_LockManager.UnLock("EnterDesk");
				SetTouchEnable(isEnable: true, "reconnect success");
			}
			if (ESP_LockManager.IsLocked("btn_room"))
			{
				ESP_LockManager.UnLock("btn_room");
				SetTouchEnable(isEnable: true, "reconnect success");
			}
		}
		if (num5 == 3)
		{
			Dictionary<string, object> dictionary2 = dictionary["deskInfo"] as Dictionary<string, object>;
			int credit = (int)dictionary2["userScore"];
			int num7 = (int)dictionary2["userGold"];
			ESP_MB_Singleton<ESP_MajorGameController>.GetInstance().JudgeResetGame(credit);
		}
		if (ESP_MB_Singleton<ESP_ReconnectHint>.GetInstance() != null)
		{
			ESP_MB_Singleton<ESP_ReconnectHint>.GetInstance().Hide();
		}
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().isLogined = true;
		if (_coKeepHeart != null)
		{
			StopCoroutine(_coKeepHeart);
		}
		_coKeepHeart = StartCoroutine(ESP_MB_Singleton<ESP_NetManager>.GetInstance().KeepHeart());
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectCount = 0;
	}

	public void HandleNetMsg_UserLogin(object[] args)
	{
		UnityEngine.Debug.Log(ESP_LogHelper.NetHandle("HandleNetMsg_UserLogin"));
		_hasLogined = true;
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectMaxTimes = 50;
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectMaxTimeout = 30f;
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectMaxTimes = 30;
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectMaxTimeout = 20f;
		if (_coLogin != null)
		{
			StopCoroutine(_coLogin);
			_coLogin = null;
		}
		if (ESP_MB_Singleton<ESP_ReconnectHint>.GetInstance() != null)
		{
			ESP_MB_Singleton<ESP_ReconnectHint>.GetInstance().Hide();
		}
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			MonoBehaviour.print(item.Key + "," + item.Value.ToString() + "----");
		}
		if ((bool)dictionary["success"])
		{
			ESP_MySqlConnection.user = ESP_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
			if (dictionary.ContainsKey("repeat") && (bool)dictionary["repeat"])
			{
				if (dictionary.ContainsKey("roomId"))
				{
					if (ESP_LockManager.IsLocked("EnterDesk"))
					{
						ESP_LockManager.UnLock("EnterDesk");
						SetTouchEnable(isEnable: true, "reconnect success");
					}
					int num = (int)dictionary["roomId"];
					if (dictionary.ContainsKey("deskId"))
					{
						int num2 = (int)dictionary["deskId"];
						Dictionary<string, object> dictionary2 = dictionary["seat"] as Dictionary<string, object>;
						int num3 = (int)dictionary2["userScore"];
						int num4 = (int)dictionary2["userGold"];
						UnityEngine.Debug.Log(ESP_LogHelper.Magenta("roomId: {0},deskId: {1},userScore: {2},userGold: {3}", num, num2, num3, num4));
						ESP_MB_Singleton<ESP_MajorGameController>.GetInstance().JudgeResetGame(num3);
					}
					else
					{
						UnityEngine.Debug.Log(ESP_LogHelper.Magenta("roomId: {0}", num));
					}
				}
				else
				{
					UnityEngine.Debug.Log(ESP_LogHelper.Magenta("not in room"));
					if (ESP_LockManager.IsLocked("btn_room"))
					{
						ESP_LockManager.UnLock("btn_room");
						SetTouchEnable(isEnable: true, "reconnect success");
					}
				}
			}
			else
			{
				UnityEngine.Debug.Log(ESP_LogHelper.Magenta("not reconnect login"));
			}
			ESP_MB_Singleton<ESP_NetManager>.GetInstance().isLogined = true;
			if (_coKeepHeart != null)
			{
				StopCoroutine(_coKeepHeart);
			}
			_coKeepHeart = StartCoroutine(ESP_MB_Singleton<ESP_NetManager>.GetInstance().KeepHeart());
			ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectCount = 0;
		}
		else
		{
			int code = (int)dictionary["msgCode"];
			Action callback = delegate
			{
				QuitToHallLogin();
				_lockDic["Alert"] = false;
			};
			StartCoroutine(ESP_Utils.DelayCall(0f, delegate
			{
				if (_coLogin != null)
				{
					StopCoroutine(_coLogin);
				}
			}));
			string errorMsg = ESP_ErrorCode.GetErrorMsg(code, ESP_MySqlConnection.language.Equals("zh"));
			_lockDic["Alert"] = true;
			ESP_LockManager.Lock("Quit");
			SetTouchEnable(isEnable: true, "quit alert", includeEsc: false);
			PrepareQuitGame();
			ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog(errorMsg, showOkCancel: false, callback);
		}
	}

	public int GetUIId()
	{
		int result = 0;
		if (ESP_MySqlConnection.curView == "LoadingView")
		{
			result = 0;
		}
		else if (ESP_MySqlConnection.curView == "RoomSelectionView")
		{
			result = 1;
		}
		else if (ESP_MySqlConnection.curView == "DeskSelectionView")
		{
			result = 2;
		}
		else if (ESP_MySqlConnection.curView == "MajorGame" || ESP_MySqlConnection.curView == "DiceGame" || ESP_MySqlConnection.curView == "MaryGame")
		{
			result = 3;
		}
		return result;
	}

	public void HandleNetMsg_LevelUp(object[] args)
	{
		UnityEngine.Debug.Log(ESP_LogHelper.NetHandle("HandleNetMsg_LevelUp"));
		int levelInfo = ESP_MySqlConnection.user.level + 1;
		ESP_MB_Singleton<ESP_HeadViewController>.GetInstance().SetLevelInfo(levelInfo);
	}

	public void HandleNetMsg_Overflow(object[] args)
	{
		UnityEngine.Debug.Log(ESP_LogHelper.NetHandle("HandleNetMsg_Overflow"));
		ESP_LockManager.Lock("Overflow");
		if (!ESP_LockManager.IsLocked("Delay_Overflow"))
		{
			OverflowProcess();
		}
	}

	public void HandleNetMsg_QuitToLogin(object[] args)
	{
		UnityEngine.Debug.Log(ESP_LogHelper.NetHandle("HandleNetMsg_QuitToLogin"));
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
		ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog(empty, showOkCancel: false, delegate
		{
			QuitToHallLogin();
			_lockDic["Alert"] = false;
		});
	}

	public void PrepareQuitGame()
	{
		_lockDic["Alert"] = true;
		ESP_LockManager.Lock("Quit");
		if (ESP_MB_Singleton<ESP_ReconnectHint>.GetInstance() != null)
		{
			ESP_MB_Singleton<ESP_ReconnectHint>.GetInstance().Hide();
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
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().Disconnect();
	}

	public void HandleNetMsg_Notice(object[] args)
	{
		UnityEngine.Debug.Log(ESP_LogHelper.NetHandle("HandleNetMsg_Notice"));
		string text = args[0] as string;
		UnityEngine.Debug.Log(ESP_LogHelper.NetHandle($"msg: {text}"));
		ESP_NoticeController instance = ESP_MB_Singleton<ESP_NoticeController>.GetInstance();
		if (instance != null)
		{
			instance.AddMessage(text);
		}
	}

	public void HandleNetMsg_QuitToRoom(object[] args)
	{
		UnityEngine.Debug.Log(ESP_LogHelper.NetHandle("HandleNetMsg_QuitToRoom"));
		int num = (int)args[0];
		bool flag = false;
		string empty = string.Empty;
		switch (num)
		{
		case 1:
		{
			string text2 = (ESP_MySqlConnection.language == "zh") ? "桌子已经被系统删除" : "Game table not exist";
			empty = ((ESP_MySqlConnection.language == "zh") ? "网络异常，已自动结算，请重新进入游戏" : "Network error, credits have been automatically settled");
			flag = true;
			break;
		}
		case 2:
		{
			string text = (ESP_MySqlConnection.language == "zh") ? "桌子参数已修改" : "Network is not stable please re-enter";
			empty = ((ESP_MySqlConnection.language == "zh") ? "网络异常，已自动结算，请重新进入游戏" : "Network error, credits have been automatically settled");
			break;
		}
		case 3:
			empty = ((ESP_MySqlConnection.language == "zh") ? "由于您长时间未游戏，已自动退出游戏" : "Game quit automatically because you did not play for a long time");
			break;
		case 4:
			empty = ((ESP_MySqlConnection.language == "zh") ? "桌子爆机" : "Your account is blasting, please excharge");
			break;
		default:
			empty = $"QuitToRoom> unknown type: [{num}]";
			break;
		}
		ESP_MB_Singleton<ESP_MajorGameController>.GetInstance().ResetGame();
		ESP_MB_Singleton<ESP_MaryGameController>.GetInstance().ResetGame();
		ESP_MB_Singleton<ESP_MajorGameController>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_MaryMovieController>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().Show();
		if (flag)
		{
			ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().curDeskIndex = 0;
			ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().UpdateAllDeskWidgets();
		}
		_lockDic["Alert"] = true;
		SetTouchEnable(isEnable: true, "QuitToRoom");
		ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog(empty, showOkCancel: false, delegate
		{
			_lockDic["Alert"] = false;
			ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().isTouchForbidden = false;
		});
	}

	public void HandleNetMsg_SyncGold(object[] args)
	{
		UnityEngine.Debug.Log(ESP_LogHelper.NetHandle("HandleNetMsg_SyncGold"));
		if (ESP_MySqlConnection.user != null)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			ESP_MySqlConnection.user.expeGold = (int)dictionary["expeGold"];
			ESP_MySqlConnection.user.gameGold = (int)dictionary["gameGold"];
			if (!(ESP_MySqlConnection.curView == "LoadingView"))
			{
				ESP_MB_Singleton<ESP_HeadViewController>.GetInstance().UpdateView();
			}
		}
	}

	public void HandleNetMsg_UserAward(object[] args)
	{
		UnityEngine.Debug.Log(ESP_LogHelper.NetHandle("HandleNetMsg_UserAward"));
		int num = (int)args[0];
		string content = (ESP_MySqlConnection.language == "zh") ? $"恭喜您获得客服赠送的{num}游戏币!" : $"Congratulations , you get {num} coins from customer service!";
		ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
		});
	}

	public void HandleNetMsg_GameGold(object[] args)
	{
		UnityEngine.Debug.Log(ESP_LogHelper.NetHandle("HandleNetMsg_GameGold"));
		if (ESP_MySqlConnection.user != null)
		{
			int gameGold = (int)args[0];
			ESP_MySqlConnection.user.gameGold = gameGold;
			if (ESP_MB_Singleton<ESP_HeadViewController>.GetInstance() != null)
			{
				ESP_MB_Singleton<ESP_HeadViewController>.GetInstance().UpdateView();
			}
		}
	}

	public void HandleNetMsg_CheckVersion(object[] args)
	{
		UnityEngine.Debug.Log(ESP_LogHelper.NetHandle("HandleNetMsg_CheckVersion"));
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
			ESP_LockManager.Lock("Quit");
			ESP_IOSGameStart.GetSingleton().UpdateGameVesion(downloadadress);
		}
		else
		{
			m_hasVersionChecked = true;
		}
	}

	public void AfterReconnect_DoUnlock()
	{
		ESP_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
	}

	public void Handle_NetDown(object[] args)
	{
		UnityEngine.Debug.Log("Handle_NetDown");
		UnityEngine.Debug.Log(string.Format("curView: {0}, isReady: {1}, _lockDic.HandleNetDown: {2}, connectCount: {3}", ESP_MySqlConnection.curView, ESP_MB_Singleton<ESP_NetManager>.GetInstance().isReady, _lockDic.GetBoolValue("HandleNetDown"), ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectCount));
		if (ESP_LockManager.IsLocked("Overflow") || ESP_LockManager.IsLocked("Quit"))
		{
			return;
		}
		if (ESP_MB_Singleton<ESP_NetManager>.GetInstance().isReady)
		{
			if (ESP_MB_Singleton<ESP_ReconnectHint>.GetInstance() != null)
			{
				ESP_MB_Singleton<ESP_ReconnectHint>.GetInstance().Hide();
			}
		}
		else
		{
			if (ESP_LockManager.IsLocked("HandleNetDown_Quit"))
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
			if (ESP_MySqlConnection.curView == "LoadingView" && flag)
			{
				string content = string.Format((ESP_MySqlConnection.language == "zh") ? "网络连接已断开，错误代码为 [{0}]" : "Net Error [{0}]", (ex == null) ? "9999" : ex.Message);
				ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
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
			if ((ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectCount > ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectMaxTimes && ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectTimeCount > ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectMaxTimeout / 2f) || ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectTimeCount > ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectMaxTimeout)
			{
				_timeoutQuit();
				return;
			}
			_coLogin = StartCoroutine(_connectAndLoginCoroutine());
			if (ESP_MB_Singleton<ESP_ReconnectHint>.GetInstance() != null)
			{
				ESP_MB_Singleton<ESP_ReconnectHint>.GetInstance().Show();
			}
		}
	}

	public void Send_ReLogin()
	{
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().Send("userService/reLogin", new object[3]
		{
			ESP_MySqlConnection.username,
			ESP_MySqlConnection.pwd,
			GetUIId()
		});
	}

	public void ChangeView(string newView)
	{
		UnityEngine.Debug.Log($"changeView> from [{ESP_MySqlConnection.curView}] to [{newView}]");
		ESP_MySqlConnection.curView = newView;
		if (ESP_MB_Singleton<ESP_NoticeController>.GetInstance() != null)
		{
			ESP_MB_Singleton<ESP_NoticeController>.GetInstance().ChangeScene();
		}
	}

	private void _timeoutQuit()
	{
		UnityEngine.Debug.Log($"connectCount: {ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectCount}, connectTimeCount: {ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectTimeCount}");
		UnityEngine.Debug.Log($"connectMaxTimes: {ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectMaxTimes}, connectMaxTimeout: {ESP_MB_Singleton<ESP_NetManager>.GetInstance().connectMaxTimeout}");
		string content = (ESP_MySqlConnection.language == "zh") ? "网络连接错误，请检查网络连接" : "Unable to connect to server，Please check your network connection";
		if (ESP_MB_Singleton<ESP_ReconnectHint>.GetInstance() != null)
		{
			ESP_MB_Singleton<ESP_ReconnectHint>.GetInstance().Hide();
		}
		ESP_LockManager.Lock("HandleNetDown_Quit");
		PrepareQuitGame();
		ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			QuitToHallLogin();
			_lockDic["HandleNetDown"] = false;
		});
	}

	public void Handle_BtnReturn()
	{
		if (ESP_MySqlConnection.curView == "RoomSelectionView")
		{
			ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog((ESP_MySqlConnection.language == "zh") ? "是否退出程序？" : "Quit the game?", showOkCancel: true, delegate
			{
				StartCoroutine(ESP_Utils.DelayCall(0.1f, delegate
				{
					QuitToHallGame();
				}));
			});
		}
		else if (ESP_MySqlConnection.curView == "DeskSelectionView")
		{
			if (HasDesk)
			{
				ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().HideDeskViewAni();
				ESP_MB_Singleton<ESP_RoomSelectionViewController>.GetInstance().Show();
				ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().ResetRoomView();
				ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().Send_LeaveRoom();
			}
			else
			{
				ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().OnBtnReturn_Click();
			}
		}
	}

	public void Handle_ItemReturn()
	{
		if (!ESP_LockManager.IsLocked("btn_options"))
		{
			ESP_SoundManager.Instance.StopMaryAudio();
			ESP_MB_Singleton<ESP_HUDController>.GetInstance().HideRules();
			ESP_MB_Singleton<ESP_OptionsController>.GetInstance().Hide();
			ESP_MB_Singleton<ESP_SettingsController>.GetInstance().Hide();
			ESP_MB_Singleton<ESP_ScoreBank>.GetInstance().Hide();
			if (ESP_MySqlConnection.curView == "DiceGame")
			{
				ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog((ESP_MySqlConnection.language == "zh") ? "是否退出比倍？" : "Quit the Dice?", showOkCancel: true, delegate
				{
					ESP_MB_Singleton<ESP_DiceGameController2>.GetInstance().ExitGame();
				});
			}
			else if (ESP_MySqlConnection.curView == "MajorGame")
			{
				ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog((ESP_MySqlConnection.language == "zh") ? "是否退出游戏？" : "Quit the game?", showOkCancel: true, delegate
				{
					ESP_SoundManager.Instance.PlayLobbyBGM();
					ESP_MB_Singleton<ESP_MajorGameController>.GetInstance().Hide();
					ESP_MB_Singleton<ESP_MajorGameController>.GetInstance().ExitGame();
					ESP_MB_Singleton<ESP_MajorGameController>.GetInstance().ResetGame();
					ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().Show();
				});
			}
		}
	}

	private void _onLoadingFinish()
	{
		UnityEngine.Debug.Log("_onLoadingFinish");
		ESP_MB_Singleton<ESP_LoadingViewController>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().Show();
		ESP_SoundManager.Instance.PlayLobbyBGM();
		_audio.enabled = true;
		Transform transform = GameObject.Find("Canvas").transform;
		ESP_MB_Singleton<ESP_AlertDialog>.SetInstanceByGameObject(transform.Find("Mask/AlertDialog").gameObject, force: true).PreInit();
		Transform transform2 = transform.Find("Loading");
		if ((bool)transform2)
		{
			transform2.gameObject.SetActive(value: false);
		}
		ESP_MB_Singleton<ESP_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	private void _onloadLevelFinish()
	{
		UnityEngine.Debug.Log("_onloadLevelFinish");
		_coLoading = null;
		InitSingletonStep2();
	}

	private void _onDiceExit()
	{
		ESP_MB_Singleton<ESP_DiceGameController2>.GetInstance().Hide();
		StartCoroutine(waitCurtainPullDown());
	}

	private IEnumerator waitCurtainPullDown()
	{
		yield return new WaitForSeconds(0.6f);
		ESP_MB_Singleton<ESP_MajorGameController>.GetInstance().Show();
		ESP_MB_Singleton<ESP_MajorGameController>.GetInstance().OnDiceExit();
		yield return new WaitForSeconds(0.25f);
		Cinstance<ESP_Gcore>.Instance.IsDic = false;
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
		ESP_MySqlConnection.user = ESP_User.CreateWithDic(dictionary);
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
		ESP_MySqlConnection.desk = ESP_Desk.CreateWithDic(dictionary);
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
		ESP_MB_Singleton<ESP_MaryGameController>.GetInstance().PrepareGame(times, credit, totalBet, photoNumberArray, photosArray, totalWinArray);
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
		ESP_MB_Singleton<ESP_LoadingViewController>.GetInstance().Hide();
		if (devSceneType == ESP_DevSceneType.DiceGame)
		{
			_makeFakeUser();
			_makeFakeDesk();
			ESP_MB_Singleton<ESP_DiceGameController2>.GetInstance().Show();
			ESP_MB_Singleton<ESP_DiceGameController2>.GetInstance().InitGame(20000, 450);
		}
		else if (devSceneType == ESP_DevSceneType.MaryGame)
		{
			UnityEngine.Debug.Log(LayerMask.NameToLayer("MaryGame"));
			UnityEngine.Debug.Log(Camera.current);
			UnityEngine.Debug.Log(Camera.main);
			Camera component = GameObject.Find("/Canvas/UICamera").GetComponent<Camera>();
			component.cullingMask |= 1 << LayerMask.NameToLayer("MaryGame");
			ESP_MB_Singleton<ESP_MaryGameController>.GetInstance().Show();
			_makePrepareMaryGame();
			ESP_MB_Singleton<ESP_MaryGameController>.GetInstance().StartGame(990);
		}
		else if (devSceneType == ESP_DevSceneType.Lobby)
		{
			_makeFakeUser();
			ESP_MB_Singleton<ESP_LobbyViewController>.GetInstance().Show();
		}
		else if (devSceneType == ESP_DevSceneType.Options)
		{
			ESP_MB_Singleton<ESP_OptionsController>.GetInstance().onItemReturn = delegate
			{
				UnityEngine.Debug.Log("onItemReturn");
			};
			ESP_MB_Singleton<ESP_OptionsController>.GetInstance().onItemSettings = delegate
			{
				UnityEngine.Debug.Log("onItemSettings");
			};
			ESP_MB_Singleton<ESP_OptionsController>.GetInstance().onItemBank = delegate
			{
				UnityEngine.Debug.Log("onItemBank");
			};
			ESP_MB_Singleton<ESP_OptionsController>.GetInstance().Show();
		}
		else if (devSceneType == ESP_DevSceneType.Settings)
		{
			ESP_MB_Singleton<ESP_SettingsController>.GetInstance().Show();
		}
		else if (devSceneType == ESP_DevSceneType.ScoreBank)
		{
			_makeFakeUser();
			_makeFakeDesk();
			ESP_MB_Singleton<ESP_ScoreBank>.GetInstance().Show();
			ESP_MB_Singleton<ESP_ScoreBank>.GetInstance().InitBank();
		}
		else if (devSceneType == ESP_DevSceneType.AlertDialog)
		{
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog("我是开发者消息!!!\n下方应该有一个确认按钮。");
			}
			else
			{
				ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog("我是开发者消息!!!\n下方应该有一个好按钮和一个取消按钮。", showOkCancel: true);
			}
		}
		else
		{
			_init();
		}
	}
}
