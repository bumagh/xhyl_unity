using DG.Tweening;
using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LRS_GameManager : LRS_MB_Singleton<LRS_GameManager>
{
	public bool isDevelopment;

	public bool isEnglish;

	public LRS_DevSceneType devSceneType;

	private bool _isDirectLoad;

	private bool _isReconnecting;

	public Image_Prefab ImagePani;

	public Image_Prefab ImageP;

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

	public bool HasDesk => LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().HasDesk;

	private void Awake()
	{
		_last_isEnglish = isEnglish;
		_isReconnecting = false;
		if (LRS_MB_Singleton<LRS_GameManager>._instance != null)
		{
			LRS_MB_Singleton<LRS_GameManager>.GetInstance().InitSingletonStep2();
			LRS_MB_Singleton<LRS_GameManager>.GetInstance().HideAllViewButLoading();
			UnityEngine.Object.DestroyImmediate(base.gameObject);
			return;
		}
		if (LRS_MB_Singleton<LRS_GameManager>._instance == null)
		{
			LRS_MB_Singleton<LRS_GameManager>.SetInstance(this);
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
		for (int i = 0; i < 15; i++)
		{
			Cinstance<LRS_Gcore>.Instance.Normallist.Add(UnityEngine.Random.Range(1, 10));
		}
		Cinstance<LRS_Gcore>.Instance.Result = Cinstance<LRS_Gcore>.Instance.Normallist;
		UnityEngine.Debug.LogError("初始结果: " + JsonMapper.ToJson(Cinstance<LRS_Gcore>.Instance.Result));
		Sequence s = DOTween.Sequence();
		s.AppendInterval(0.2f);
		s.AppendCallback(Cinstance<LRS_Gcore>.Instance.Init);
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
		LRS_MB_Singleton<LRS_LoadingViewController>.GetInstance().Hide();
		_init();
	}

	private void Update()
	{
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectTimeCount += Time.deltaTime;
		if (LRS_GVars.curView == "LoadingView")
		{
			return;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			UnityEngine.Debug.Log(string.Format("Esc: {0}", LRS_LockManager.GetValue("Esc")));
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance()._IsAning && !LRS_LockManager.IsLocked("Esc") && !LRS_LockManager.IsLocked("Quit"))
		{
			if (LRS_GVars.curView == "RoomSelectionView" || LRS_GVars.curView == "DeskSelectinView")
			{
				Handle_BtnReturn();
			}
			else if ((LRS_GVars.curView == "DiceGame" || LRS_GVars.curView == "MajorGame") && !LRS_MB_Singleton<LRS_MaryGameController>.GetInstance().bEnterMary)
			{
				Handle_ItemReturn();
			}
		}
		if (LRS_GVars.lockOnePoint)
		{
			LRS_GVars.lockOnePoint = false;
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
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("heart", delegate
		{
		});
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("netDown", Handle_NetDown);
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("userLogin", HandleNetMsg_UserLogin);
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("reLogin", HandleNetMsg_ReLogin);
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("overflow", HandleNetMsg_Overflow);
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("levelUp", HandleNetMsg_LevelUp);
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("quitToLogin", HandleNetMsg_QuitToLogin);
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("quitToRoom", HandleNetMsg_QuitToRoom);
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("syncGold", HandleNetMsg_SyncGold);
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("checkVersion", HandleNetMsg_CheckVersion);
		if (!LRS_MB_Singleton<LRS_NetManager>.GetInstance().IsMethodRegistered("updateRoomInfo"))
		{
			LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("updateRoomInfo", delegate
			{
			});
		}
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("userAward", HandleNetMsg_UserAward);
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("gameGold", HandleNetMsg_GameGold);
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("sendNotice", HandleNetMsg_Notice);
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("notice", HandleNetMsg_Notice);
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("timeout", delegate(object[] args)
		{
			UnityEngine.Debug.Log(LRS_LogHelper.Magenta("timeout>  id: " + (int)args[0]));
		});
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("heart timeout", delegate
		{
			UnityEngine.Debug.Log(LRS_LogHelper.Magenta("heart timeout"));
			Handle_NetDown(new object[1]
			{
				new Exception("heart timeout")
			});
		});
		LRS_MB_Singleton<LRS_LoadingViewController>.GetInstance().Show();
		if (!_isDirectLoad)
		{
			LRS_MB_Singleton<LRS_LoadingViewController>.GetInstance().loadLevelFinishAction += _onloadLevelFinish;
			_coLoading = StartCoroutine(LRS_MB_Singleton<LRS_LoadingViewController>.GetInstance().LoadingAni2());
		}
		else
		{
			_coLoading = StartCoroutine(LRS_MB_Singleton<LRS_LoadingViewController>.GetInstance().LoadingAni());
		}
		if (LRS_MB_Singleton<LRS_LoginController>.GetInstance() == null || !LRS_MB_Singleton<LRS_LoginController>.GetInstance().IsShow())
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
		LRS_I2LocalizeText[] array = UnityEngine.Object.FindObjectsOfType<LRS_I2LocalizeText>();
		LRS_I2LocalizeText[] array2 = array;
		foreach (LRS_I2LocalizeText lRS_I2LocalizeText in array2)
		{
			lRS_I2LocalizeText.Refresh();
		}
		LRS_I2LocalizeImage[] array3 = UnityEngine.Object.FindObjectsOfType<LRS_I2LocalizeImage>();
		LRS_I2LocalizeImage[] array4 = array3;
		foreach (LRS_I2LocalizeImage lRS_I2LocalizeImage in array4)
		{
			lRS_I2LocalizeImage.Refresh();
		}
	}

	public void HideAllViewButLoading()
	{
		UnityEngine.Debug.Log("HideAllViewButLoading");
		LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_MajorGameController>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_DiceGameController2>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_HUDController>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_OptionsController>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_SettingsController>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_ScoreBank>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_MaryMovieController>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_NoticeController>.GetInstance().Hide();
		if (!_lockDic.GetBoolValue("HandleNetDown") && !_lockDic.GetBoolValue("Alert"))
		{
			LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().Hide();
		}
	}

	private void _initSingletonStep1()
	{
		UnityEngine.Debug.Log("_intSingletonStep1");
		Transform transform = GameObject.Find("Canvas").transform;
		LRS_MB_Singleton<LRS_LoadingViewController>.SetInstanceByGameObject(transform.Find("Mask/Loading").gameObject).PreInit();
		LRS_MB_Singleton<LRS_AlertDialog>.SetInstanceByGameObject(transform.Find("Mask/AlertDialog").gameObject).PreInit();
		Transform transform2 = transform.Find("LoginPanel");
		if (transform2 != null)
		{
			LRS_MB_Singleton<LRS_LoginController>.SetInstanceByGameObject(transform2.gameObject).PreInit();
		}
		LRS_MB_Singleton<LRS_LoadingViewController>.GetInstance().loadingFinishAction += _onLoadingFinish;
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
		LRS_SoundManager.Instance = GameObject.Find("SoundManager").GetComponent<LRS_SoundManager>();
		LRS_MB_Singleton<LRS_LobbyViewController>.SetInstanceByGameObject(transform.Find("Mask/Lobby").gameObject).PreInit();
		LRS_MB_Singleton<LRS_HeadViewController>.SetInstanceByGameObject(transform.Find("Mask/Lobby/Title").gameObject).PreInit();
		LRS_MB_Singleton<LRS_MajorGameController>.SetInstanceByGameObject(transform.Find("Mask/Major").gameObject).PreInit();
		LRS_MB_Singleton<LRS_DiceGameController2>.SetInstanceByGameObject(transform.Find("Mask/Gameble").gameObject).PreInit();
		LRS_MB_Singleton<LRS_HUDController>.SetInstanceByGameObject(transform.Find("Mask/HUD").gameObject).PreInit();
		LRS_MB_Singleton<LRS_OptionsController>.SetInstanceByGameObject(transform.Find("Mask/Options").gameObject).PreInit();
		LRS_MB_Singleton<LRS_SettingsController>.SetInstanceByGameObject(transform.Find("Mask/Setting").gameObject).PreInit();
		LRS_MB_Singleton<LRS_ScoreBank>.SetInstanceByGameObject(transform.Find("Mask/InOut").gameObject).PreInit();
		LRS_MB_Singleton<LRS_NoticeController>.SetInstanceByGameObject(transform.Find("Mask/Notice").gameObject).PreInit();
		LRS_MB_Singleton<LRS_DiceGameController2>.GetInstance().onExitAction = _onDiceExit;
		LRS_MB_Singleton<LRS_SettingsController>.GetInstance().Init();
		LRS_MB_Singleton<LRS_HeadViewController>.GetInstance().btnReturnAction = Handle_BtnReturn;
		LRS_MB_Singleton<LRS_OptionsController>.GetInstance().onItemReturn = Handle_ItemReturn;
	}

	private void _prepareInitData()
	{
		LRS_GVars.versionCode = "9.9.1";
		UnityEngine.Debug.Log("Application.Version: " + Application.version + "  LRS_GVars.versionCode: " + LRS_GVars.versionCode);
		LRS_GVars.IPPort = 10047;
		UseDebugData();
		if (LRS_GVars.language != "zh" && LRS_GVars.language != "en")
		{
			UnityEngine.Debug.LogWarning("ZH2_GVars.language is not correct: " + LRS_GVars.language);
			LRS_GVars.language = "zh";
		}
		UnityEngine.Debug.LogError("链接地址: " + LRS_GVars.IPAddress + " 端口: " + LRS_GVars.IPPort);
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
		LRS_GVars.username = ZH2_GVars.username;
		LRS_GVars.pwd = ZH2_GVars.pwd;
		LRS_GVars.IPAddress = ZH2_GVars.IPAddress_Game;
		LRS_GVars.language = "zh";
		LRS_GVars.IPPort = 10054;
	}

	private IEnumerator _connectAndLoginCoroutine()
	{
		m_hasVersionChecked = false;
		UnityEngine.Debug.Log($"_isReconnecting: {_isReconnecting}, isConnected: {LRS_MB_Singleton<LRS_NetManager>.GetInstance().isConnected}, isReady: {LRS_MB_Singleton<LRS_NetManager>.GetInstance().isReady}");
		if (!_hasLogined)
		{
			LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectMaxTimes = 10;
			LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectMaxTimeout = 5f;
		}
		if (!_isReconnecting)
		{
			_prepareInitData();
		}
		else
		{
			LRS_MB_Singleton<LRS_NetManager>.GetInstance().Disconnect();
			yield return new WaitForSeconds(0.5f);
		}
		if (!_hasLogined)
		{
			LRS_GVars.IPAddress = LRS_CheckIP.DoGetHostAddresses(LRS_GVars.IPAddress);
		}
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().Connect(LRS_GVars.IPAddress, LRS_GVars.IPPort);
		UnityEngine.Debug.Log("连接IP" + LRS_GVars.IPAddress + " wait connected !");
		while (!LRS_MB_Singleton<LRS_NetManager>.GetInstance().isConnected)
		{
			yield return null;
		}
		while (!m_hasVersionChecked)
		{
			yield return null;
		}
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().SendPublicKey();
		while (!LRS_MB_Singleton<LRS_NetManager>.GetInstance().isReady)
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
			UnityEngine.Debug.Log(LRS_LogHelper.Magenta("_loadingLoginTimeout"));
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
		UnityEngine.Debug.Log(LRS_LogHelper.Magenta("QuitToHallLogin"));
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
				LRS_LockManager.UnLock("Esc", LRS_LockManager.GetValue("Esc") < 10);
			}
			else
			{
				LRS_LockManager.Lock("Esc");
			}
		}
		UnityEngine.Debug.Log($"SetTouchEnable> [{LRS_LogHelper.Key(isEnable)}], reason: [{LRS_LogHelper.Key(reason)}]");
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
		string content = (LRS_GVars.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge";
		LRS_LockManager.Lock("Quit");
		PrepareQuitGame();
		LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			QuitToHallGame();
		});
	}

	public void Send_Login()
	{
		object[] args = new object[2]
		{
			LRS_GVars.username,
			LRS_GVars.pwd
		};
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().Send("userService/userLogin", args);
	}

	private void OnApplicationQuit()
	{
		Send_QuitGame();
	}

	public void Send_QuitGame()
	{
		object[] args = new object[0];
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().Send("userService/quitGame", args);
		LRS_LockManager.Lock("Quit");
		SetTouchEnable(isEnable: false, "Send_QuitGame");
	}

	public void HandleNetMsg_ReLogin(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string[] array2;
		if (LRS_GVars.language == "zh")
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
			LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog(array4[num2], showOkCancel: false, delegate
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
			LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog(array4[num4], showOkCancel: false, delegate
			{
				QuitToHallGame();
			});
			return;
		}
		int num5 = (int)dictionary["uiid"];
		UnityEngine.Debug.Log("uiid: " + num5);
		if (num5 == 1)
		{
			LRS_GVars.user = LRS_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
			if (LRS_LockManager.IsLocked("btn_room"))
			{
				LRS_LockManager.UnLock("btn_room");
				SetTouchEnable(isEnable: true, "reconnect success");
			}
		}
		if (num5 == 2)
		{
			object[] array5 = (object[])dictionary["roomInfo"];
			int num6 = array5.Length;
			LRS_Desk[] array6 = new LRS_Desk[num6];
			for (int i = 0; i < num6; i++)
			{
				array6[i] = LRS_Desk.CreateWithDic((Dictionary<string, object>)array5[i]);
			}
			if (LRS_LockManager.IsLocked("EnterDesk"))
			{
				LRS_LockManager.UnLock("EnterDesk");
				SetTouchEnable(isEnable: true, "reconnect success");
			}
			if (LRS_LockManager.IsLocked("btn_room"))
			{
				LRS_LockManager.UnLock("btn_room");
				SetTouchEnable(isEnable: true, "reconnect success");
			}
		}
		if (num5 == 3)
		{
			Dictionary<string, object> dictionary2 = dictionary["deskInfo"] as Dictionary<string, object>;
			int credit = (int)dictionary2["userScore"];
			int num7 = (int)dictionary2["userGold"];
			LRS_MB_Singleton<LRS_MajorGameController>.GetInstance().JudgeResetGame(credit);
		}
		if (LRS_MB_Singleton<LRS_ReconnectHint>.GetInstance() != null)
		{
			LRS_MB_Singleton<LRS_ReconnectHint>.GetInstance().Hide();
		}
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().isLogined = true;
		if (_coKeepHeart != null)
		{
			StopCoroutine(_coKeepHeart);
		}
		_coKeepHeart = StartCoroutine(LRS_MB_Singleton<LRS_NetManager>.GetInstance().KeepHeart());
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectCount = 0;
	}

	public void HandleNetMsg_UserLogin(object[] args)
	{
		UnityEngine.Debug.Log(LRS_LogHelper.NetHandle("HandleNetMsg_UserLogin"));
		_hasLogined = true;
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectMaxTimes = 50;
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectMaxTimeout = 30f;
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectMaxTimes = 30;
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectMaxTimeout = 20f;
		if (_coLogin != null)
		{
			StopCoroutine(_coLogin);
			_coLogin = null;
		}
		if (LRS_MB_Singleton<LRS_ReconnectHint>.GetInstance() != null)
		{
			LRS_MB_Singleton<LRS_ReconnectHint>.GetInstance().Hide();
		}
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			MonoBehaviour.print(item.Key + "," + item.Value.ToString() + "----");
		}
		if ((bool)dictionary["success"])
		{
			LRS_GVars.user = LRS_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
			if (dictionary.ContainsKey("repeat") && (bool)dictionary["repeat"])
			{
				if (dictionary.ContainsKey("roomId"))
				{
					if (LRS_LockManager.IsLocked("EnterDesk"))
					{
						LRS_LockManager.UnLock("EnterDesk");
						SetTouchEnable(isEnable: true, "reconnect success");
					}
					int num = (int)dictionary["roomId"];
					if (dictionary.ContainsKey("deskId"))
					{
						int num2 = (int)dictionary["deskId"];
						Dictionary<string, object> dictionary2 = dictionary["seat"] as Dictionary<string, object>;
						int num3 = (int)dictionary2["userScore"];
						int num4 = (int)dictionary2["userGold"];
						UnityEngine.Debug.Log(LRS_LogHelper.Magenta("roomId: {0},deskId: {1},userScore: {2},userGold: {3}", num, num2, num3, num4));
						LRS_MB_Singleton<LRS_MajorGameController>.GetInstance().JudgeResetGame(num3);
					}
					else
					{
						UnityEngine.Debug.Log(LRS_LogHelper.Magenta("roomId: {0}", num));
					}
				}
				else
				{
					UnityEngine.Debug.Log(LRS_LogHelper.Magenta("not in room"));
					if (LRS_LockManager.IsLocked("btn_room"))
					{
						LRS_LockManager.UnLock("btn_room");
						SetTouchEnable(isEnable: true, "reconnect success");
					}
				}
			}
			else
			{
				UnityEngine.Debug.Log(LRS_LogHelper.Magenta("not reconnect login"));
			}
			LRS_MB_Singleton<LRS_NetManager>.GetInstance().isLogined = true;
			if (_coKeepHeart != null)
			{
				StopCoroutine(_coKeepHeart);
			}
			_coKeepHeart = StartCoroutine(LRS_MB_Singleton<LRS_NetManager>.GetInstance().KeepHeart());
			LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectCount = 0;
		}
		else
		{
			int code = (int)dictionary["msgCode"];
			Action callback = delegate
			{
				QuitToHallLogin();
				_lockDic["Alert"] = false;
			};
			StartCoroutine(LRS_Utils.DelayCall(0f, delegate
			{
				if (_coLogin != null)
				{
					StopCoroutine(_coLogin);
				}
			}));
			string errorMsg = LRS_ErrorCode.GetErrorMsg(code, LRS_GVars.language.Equals("zh"));
			_lockDic["Alert"] = true;
			LRS_LockManager.Lock("Quit");
			SetTouchEnable(isEnable: true, "quit alert", includeEsc: false);
			PrepareQuitGame();
			LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog(errorMsg, showOkCancel: false, callback);
		}
	}

	public int GetUIId()
	{
		int result = 0;
		if (LRS_GVars.curView == "LoadingView")
		{
			result = 0;
		}
		else if (LRS_GVars.curView == "RoomSelectionView")
		{
			result = 1;
		}
		else if (LRS_GVars.curView == "DeskSelectionView")
		{
			result = 2;
		}
		else if (LRS_GVars.curView == "MajorGame" || LRS_GVars.curView == "DiceGame" || LRS_GVars.curView == "MaryGame")
		{
			result = 3;
		}
		return result;
	}

	public void HandleNetMsg_LevelUp(object[] args)
	{
		UnityEngine.Debug.Log(LRS_LogHelper.NetHandle("HandleNetMsg_LevelUp"));
		int levelInfo = LRS_GVars.user.level + 1;
		LRS_MB_Singleton<LRS_HeadViewController>.GetInstance().SetLevelInfo(levelInfo);
	}

	public void HandleNetMsg_Overflow(object[] args)
	{
		UnityEngine.Debug.Log(LRS_LogHelper.NetHandle("HandleNetMsg_Overflow"));
		LRS_LockManager.Lock("Overflow");
		if (!LRS_LockManager.IsLocked("Delay_Overflow"))
		{
			OverflowProcess();
		}
	}

	public void HandleNetMsg_QuitToLogin(object[] args)
	{
		UnityEngine.Debug.Log(LRS_LogHelper.NetHandle("HandleNetMsg_QuitToLogin"));
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
		LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog(empty, showOkCancel: false, delegate
		{
			QuitToHallLogin();
			_lockDic["Alert"] = false;
		});
	}

	public void PrepareQuitGame()
	{
		_lockDic["Alert"] = true;
		LRS_LockManager.Lock("Quit");
		if (LRS_MB_Singleton<LRS_ReconnectHint>.GetInstance() != null)
		{
			LRS_MB_Singleton<LRS_ReconnectHint>.GetInstance().Hide();
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
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().Disconnect();
	}

	public void HandleNetMsg_Notice(object[] args)
	{
		UnityEngine.Debug.Log(LRS_LogHelper.NetHandle("HandleNetMsg_Notice"));
		string text = args[0] as string;
		UnityEngine.Debug.Log(LRS_LogHelper.NetHandle($"msg: {text}"));
		LRS_NoticeController instance = LRS_MB_Singleton<LRS_NoticeController>.GetInstance();
		if (instance != null)
		{
			instance.AddMessage(text);
		}
	}

	public void HandleNetMsg_QuitToRoom(object[] args)
	{
		UnityEngine.Debug.Log(LRS_LogHelper.NetHandle("HandleNetMsg_QuitToRoom"));
		int num = (int)args[0];
		bool flag = false;
		string empty = string.Empty;
		switch (num)
		{
		case 1:
		{
			string text2 = (LRS_GVars.language == "zh") ? "桌子已经被系统删除" : "Game table not exist";
			empty = ((LRS_GVars.language == "zh") ? "网络异常，已自动结算，请重新进入游戏" : "Network error, credits have been automatically settled");
			flag = true;
			break;
		}
		case 2:
		{
			string text = (LRS_GVars.language == "zh") ? "桌子参数已修改" : "Network is not stable please re-enter";
			empty = ((LRS_GVars.language == "zh") ? "网络异常，已自动结算，请重新进入游戏" : "Network error, credits have been automatically settled");
			break;
		}
		case 3:
			empty = ((LRS_GVars.language == "zh") ? "由于您长时间未游戏，已自动退出游戏" : "Game quit automatically because you did not play for a long time");
			break;
		case 4:
			empty = ((LRS_GVars.language == "zh") ? "桌子爆机" : "Your account is blasting, please excharge");
			break;
		default:
			empty = $"QuitToRoom> unknown type: [{num}]";
			break;
		}
		LRS_MB_Singleton<LRS_MajorGameController>.GetInstance().ResetGame();
		LRS_MB_Singleton<LRS_MaryGameController>.GetInstance().ResetGame();
		LRS_MB_Singleton<LRS_MajorGameController>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_MaryMovieController>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().Show();
		if (flag)
		{
			LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().curDeskIndex = 0;
			LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().UpdateAllDeskWidgets();
		}
		_lockDic["Alert"] = true;
		SetTouchEnable(isEnable: true, "QuitToRoom");
		LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog(empty, showOkCancel: false, delegate
		{
			_lockDic["Alert"] = false;
			LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().isTouchForbidden = false;
		});
	}

	public void HandleNetMsg_SyncGold(object[] args)
	{
		UnityEngine.Debug.Log(LRS_LogHelper.NetHandle("HandleNetMsg_SyncGold"));
		if (LRS_GVars.user != null)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			LRS_GVars.user.expeGold = (int)dictionary["expeGold"];
			LRS_GVars.user.gameGold = (int)dictionary["gameGold"];
			if (!(LRS_GVars.curView == "LoadingView"))
			{
				LRS_MB_Singleton<LRS_HeadViewController>.GetInstance().UpdateView();
			}
		}
	}

	public void HandleNetMsg_UserAward(object[] args)
	{
		UnityEngine.Debug.Log(LRS_LogHelper.NetHandle("HandleNetMsg_UserAward"));
		int num = (int)args[0];
		string content = (LRS_GVars.language == "zh") ? $"恭喜您获得客服赠送的{num}游戏币!" : $"Congratulations , you get {num} coins from customer service!";
		LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
		});
	}

	public void HandleNetMsg_GameGold(object[] args)
	{
		UnityEngine.Debug.Log(LRS_LogHelper.NetHandle("HandleNetMsg_GameGold"));
		if (LRS_GVars.user != null)
		{
			int gameGold = (int)args[0];
			LRS_GVars.user.gameGold = gameGold;
			if (LRS_MB_Singleton<LRS_HeadViewController>.GetInstance() != null)
			{
				LRS_MB_Singleton<LRS_HeadViewController>.GetInstance().UpdateView();
			}
		}
	}

	public void HandleNetMsg_CheckVersion(object[] args)
	{
		UnityEngine.Debug.Log(LRS_LogHelper.NetHandle("HandleNetMsg_CheckVersion"));
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
			LRS_LockManager.Lock("Quit");
			LRS_IOSGameStart.GetSingleton().UpdateGameVesion(downloadadress);
		}
		else
		{
			m_hasVersionChecked = true;
		}
	}

	public void AfterReconnect_DoUnlock()
	{
		LRS_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
	}

	public void Handle_NetDown(object[] args)
	{
		UnityEngine.Debug.Log("Handle_NetDown");
		UnityEngine.Debug.Log(string.Format("curView: {0}, isReady: {1}, _lockDic.HandleNetDown: {2}, connectCount: {3}", LRS_GVars.curView, LRS_MB_Singleton<LRS_NetManager>.GetInstance().isReady, _lockDic.GetBoolValue("HandleNetDown"), LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectCount));
		if (LRS_LockManager.IsLocked("Overflow") || LRS_LockManager.IsLocked("Quit"))
		{
			return;
		}
		if (LRS_MB_Singleton<LRS_NetManager>.GetInstance().isReady)
		{
			if (LRS_MB_Singleton<LRS_ReconnectHint>.GetInstance() != null)
			{
				LRS_MB_Singleton<LRS_ReconnectHint>.GetInstance().Hide();
			}
		}
		else
		{
			if (LRS_LockManager.IsLocked("HandleNetDown_Quit"))
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
			if (LRS_GVars.curView == "LoadingView" && flag)
			{
				string content = string.Format((LRS_GVars.language == "zh") ? "网络连接已断开，错误代码为 [{0}]" : "Net Error [{0}]", (ex == null) ? "9999" : ex.Message);
				LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
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
			if ((LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectCount > LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectMaxTimes && LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectTimeCount > LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectMaxTimeout / 2f) || LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectTimeCount > LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectMaxTimeout)
			{
				_timeoutQuit();
				return;
			}
			_coLogin = StartCoroutine(_connectAndLoginCoroutine());
			if (LRS_MB_Singleton<LRS_ReconnectHint>.GetInstance() != null)
			{
				LRS_MB_Singleton<LRS_ReconnectHint>.GetInstance().Show();
			}
		}
	}

	public void Send_ReLogin()
	{
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().Send("userService/reLogin", new object[3]
		{
			LRS_GVars.username,
			LRS_GVars.pwd,
			GetUIId()
		});
	}

	public void ChangeView(string newView)
	{
		UnityEngine.Debug.Log($"changeView> from [{LRS_GVars.curView}] to [{newView}]");
		LRS_GVars.curView = newView;
		if (LRS_MB_Singleton<LRS_NoticeController>.GetInstance() != null)
		{
			LRS_MB_Singleton<LRS_NoticeController>.GetInstance().ChangeScene();
		}
	}

	private void _timeoutQuit()
	{
		UnityEngine.Debug.Log($"connectCount: {LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectCount}, connectTimeCount: {LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectTimeCount}");
		UnityEngine.Debug.Log($"connectMaxTimes: {LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectMaxTimes}, connectMaxTimeout: {LRS_MB_Singleton<LRS_NetManager>.GetInstance().connectMaxTimeout}");
		string content = (LRS_GVars.language == "zh") ? "网络连接错误，请检查网络连接" : "Unable to connect to server，Please check your network connection";
		if (LRS_MB_Singleton<LRS_ReconnectHint>.GetInstance() != null)
		{
			LRS_MB_Singleton<LRS_ReconnectHint>.GetInstance().Hide();
		}
		LRS_LockManager.Lock("HandleNetDown_Quit");
		PrepareQuitGame();
		LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			QuitToHallLogin();
			_lockDic["HandleNetDown"] = false;
		});
	}

	public void Handle_BtnReturn()
	{
		if (LRS_GVars.curView == "RoomSelectionView")
		{
			LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog((LRS_GVars.language == "zh") ? "是否退出程序？" : "Quit the game?", showOkCancel: true, delegate
			{
				StartCoroutine(LRS_Utils.DelayCall(0.1f, delegate
				{
					QuitToHallGame();
				}));
			});
		}
		else if (LRS_GVars.curView == "DeskSelectionView")
		{
			if (HasDesk)
			{
				LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().HideDeskViewAni();
				LRS_MB_Singleton<LRS_RoomSelectionViewController>.GetInstance().Show();
				LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().ResetRoomView();
				LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().Send_LeaveRoom();
			}
			else
			{
				LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().OnBtnReturn_Click();
			}
		}
	}

	public void Handle_ItemReturn()
	{
		if (!LRS_LockManager.IsLocked("btn_options"))
		{
			LRS_SoundManager.Instance.StopMaryAudio();
			LRS_MB_Singleton<LRS_HUDController>.GetInstance().HideRules();
			LRS_MB_Singleton<LRS_OptionsController>.GetInstance().Hide();
			LRS_MB_Singleton<LRS_SettingsController>.GetInstance().Hide();
			LRS_MB_Singleton<LRS_ScoreBank>.GetInstance().Hide();
			if (LRS_GVars.curView == "DiceGame")
			{
				LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog((LRS_GVars.language == "zh") ? "是否退出比倍？" : "Quit the Dice?", showOkCancel: true, delegate
				{
					LRS_MB_Singleton<LRS_DiceGameController2>.GetInstance().ExitGame();
				});
			}
			else if (LRS_GVars.curView == "MajorGame")
			{
				LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog((LRS_GVars.language == "zh") ? "是否退出游戏？" : "Quit the game?", showOkCancel: true, delegate
				{
					LRS_SoundManager.Instance.PlayLobbyBGM();
					LRS_MB_Singleton<LRS_MajorGameController>.GetInstance().Hide();
					LRS_MB_Singleton<LRS_MajorGameController>.GetInstance().ExitGame();
					LRS_MB_Singleton<LRS_MajorGameController>.GetInstance().ResetGame();
					LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().Show();
				});
			}
		}
	}

	private void _onLoadingFinish()
	{
		UnityEngine.Debug.Log("_onLoadingFinish");
		LRS_MB_Singleton<LRS_LoadingViewController>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().Show();
		LRS_SoundManager.Instance.PlayLobbyBGM();
		_audio.enabled = true;
		Transform transform = GameObject.Find("Canvas").transform;
		LRS_MB_Singleton<LRS_AlertDialog>.SetInstanceByGameObject(transform.Find("Mask/AlertDialog").gameObject, force: true).PreInit();
		Transform transform2 = transform.Find("Loading");
		if ((bool)transform2)
		{
			transform2.gameObject.SetActive(value: false);
		}
		LRS_MB_Singleton<LRS_GameManager>.GetInstance().ChangeView("RoomSelectionView");
	}

	private void _onloadLevelFinish()
	{
		UnityEngine.Debug.Log("_onloadLevelFinish");
		_coLoading = null;
		InitSingletonStep2();
	}

	private void _onDiceExit()
	{
		LRS_MB_Singleton<LRS_DiceGameController2>.GetInstance().Hide();
		StartCoroutine(waitCurtainPullDown());
	}

	private IEnumerator waitCurtainPullDown()
	{
		yield return new WaitForSeconds(0.6f);
		LRS_MB_Singleton<LRS_MajorGameController>.GetInstance().Show();
		LRS_MB_Singleton<LRS_MajorGameController>.GetInstance().OnDiceExit();
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
		LRS_GVars.user = LRS_User.CreateWithDic(dictionary);
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
		LRS_GVars.desk = LRS_Desk.CreateWithDic(dictionary);
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
		LRS_MB_Singleton<LRS_MaryGameController>.GetInstance().PrepareGame(times, credit, totalBet, photoNumberArray, photosArray, totalWinArray);
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
		LRS_MB_Singleton<LRS_LoadingViewController>.GetInstance().Hide();
		if (devSceneType == LRS_DevSceneType.DiceGame)
		{
			_makeFakeUser();
			_makeFakeDesk();
			LRS_MB_Singleton<LRS_DiceGameController2>.GetInstance().Show();
			LRS_MB_Singleton<LRS_DiceGameController2>.GetInstance().InitGame(20000, 450);
		}
		else if (devSceneType == LRS_DevSceneType.MaryGame)
		{
			UnityEngine.Debug.Log(LayerMask.NameToLayer("MaryGame"));
			UnityEngine.Debug.Log(Camera.current);
			UnityEngine.Debug.Log(Camera.main);
			Camera component = GameObject.Find("/Canvas/UICamera").GetComponent<Camera>();
			component.cullingMask |= 1 << LayerMask.NameToLayer("MaryGame");
			LRS_MB_Singleton<LRS_MaryGameController>.GetInstance().Show();
			_makePrepareMaryGame();
			LRS_MB_Singleton<LRS_MaryGameController>.GetInstance().StartGame(990);
		}
		else if (devSceneType == LRS_DevSceneType.Lobby)
		{
			_makeFakeUser();
			LRS_MB_Singleton<LRS_LobbyViewController>.GetInstance().Show();
		}
		else if (devSceneType == LRS_DevSceneType.Options)
		{
			LRS_MB_Singleton<LRS_OptionsController>.GetInstance().onItemReturn = delegate
			{
				UnityEngine.Debug.Log("onItemReturn");
			};
			LRS_MB_Singleton<LRS_OptionsController>.GetInstance().onItemSettings = delegate
			{
				UnityEngine.Debug.Log("onItemSettings");
			};
			LRS_MB_Singleton<LRS_OptionsController>.GetInstance().onItemBank = delegate
			{
				UnityEngine.Debug.Log("onItemBank");
			};
			LRS_MB_Singleton<LRS_OptionsController>.GetInstance().Show();
		}
		else if (devSceneType == LRS_DevSceneType.Settings)
		{
			LRS_MB_Singleton<LRS_SettingsController>.GetInstance().Show();
		}
		else if (devSceneType == LRS_DevSceneType.ScoreBank)
		{
			_makeFakeUser();
			_makeFakeDesk();
			LRS_MB_Singleton<LRS_ScoreBank>.GetInstance().Show();
			LRS_MB_Singleton<LRS_ScoreBank>.GetInstance().InitBank();
		}
		else if (devSceneType == LRS_DevSceneType.AlertDialog)
		{
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog("我是开发者消息!!!\n下方应该有一个确认按钮。");
			}
			else
			{
				LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog("我是开发者消息!!!\n下方应该有一个好按钮和一个取消按钮。", showOkCancel: true);
			}
		}
		else
		{
			_init();
		}
	}
}
