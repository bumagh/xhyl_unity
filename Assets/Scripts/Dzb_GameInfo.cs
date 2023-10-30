using JsonFx.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dzb_GameInfo : Dzb_Singleton<Dzb_GameInfo>
{
	private bool _isDirectLoad;

	private bool _isDevelopment;

	private bool _ishasVersionChecked;

	public bool _IsTest;

	public bool _IsHasLogined;

	public bool _IsReconnecting;

	public bool _IsQuitGame;

	private Coroutine _coLogin;

	private Coroutine _coLoading;

	private Coroutine _coKeepHeart;

	public static float SoundVolume;

	public static float MusicVolume;

	[SerializeField]
	private GameObject _touchForbid;

	private Dictionary<string, object> _lockDic;

	public int RoomId;

	public int DeskId;

	private bool IsInit;

	private void Awake()
	{
		if (Dzb_Singleton<Dzb_GameInfo>._instance == null)
		{
			Dzb_Singleton<Dzb_GameInfo>.SetInstance(this);
			IsInit = true;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			_isDirectLoad = !SceneManager.GetActiveScene().name.StartsWith("DzbLoad");
			_lockDic = new Dictionary<string, object>();
			Application.runInBackground = true;
			if (!_isDirectLoad)
			{
			}
		}
		else
		{
			UnityEngine.Debug.Log("走着了？");
			Dzb_Singleton<Dzb_GameInfo>._instance._IsHasLogined = false;
			Dzb_Singleton<Dzb_GameInfo>._instance._IsReconnecting = false;
			Dzb_Singleton<Dzb_GameInfo>._instance._ishasVersionChecked = false;
			Dzb_Singleton<Dzb_NetManager>.GetInstance().isConnected = false;
			Dzb_Singleton<Dzb_NetManager>.GetInstance().isReady = false;
			Dzb_Singleton<Dzb_NetManager>.GetInstance().Connect(Dzb_MySqlConnection.IPAddress, Dzb_MySqlConnection.IPPort);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		if (IsInit)
		{
			if (!_isDirectLoad)
			{
				_Init();
			}
			else if (_isDevelopment)
			{
				_devInit();
			}
			else
			{
				_Init();
			}
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.A))
		{
			Send_ReLogin();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.S))
		{
			Dzb_MySqlConnection.curView = "DeskSelectionView";
			Send_ReLogin();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.D))
		{
			Dzb_Singleton<Dzb_NetManager>.GetInstance().Connect(Dzb_MySqlConnection.IPAddress, Dzb_MySqlConnection.IPPort);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.F))
		{
		}
		Dzb_Singleton<Dzb_NetManager>.GetInstance().connectTimeCount += Time.deltaTime;
		if (!(Dzb_MySqlConnection.curView == "LoadingView") && Dzb_MySqlConnection.lockOnePoint)
		{
			Dzb_MySqlConnection.lockOnePoint = false;
		}
	}

	private void _Init()
	{
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("heart", delegate
		{
		});
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("checkVersion", HandleNetMsg_CheckVersion);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("userLogin", HandleNetMsg_UserLogin);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("reLogin", Handle_ReLogin);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("getRoomInfo", Handle_GetRoomInfo);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("roomInfo", Handle_RoomInfo);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("updateRoomInfo", Handle_UpdateRoomInfo);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("enterDesk", Handle_EnterDesk);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("getDesksInfo", Handle_GetDesksInfo);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("syncGold", Handle_SyncGold);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("quitToRoom", Handle_QuitToRoom);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("updateGoldAndScore", Handle_UpdateGoldAndScore);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("daJiangBan", Handle_GetDaJiangBan);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("getDaJiangBan", Handle_GetDaJiangBan);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("getDeskHistory", Handle_GtDeskHistory);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("oneCardResult", Handle_OneCards);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("twoCardResult", Handle_TwiceCard);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("netDown", Handle_NetDown);
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("timeout", delegate(object[] args)
		{
			UnityEngine.Debug.Log(Dzb_LogHelper.Magenta("timeout>  id: " + (int)args[0]));
		});
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("heart timeout", delegate
		{
			UnityEngine.Debug.Log(Dzb_LogHelper.Magenta("heart timeout"));
			Handle_NetDown(new object[1]
			{
				new Exception("heart timeout")
			});
		});
		Dzb_Singleton<Dzb_NetManager>.GetInstance().RegisterHandler("quitToLogin", HandleNetMsg_QuitToLogin);
		if (!_isDirectLoad)
		{
		}
		if (_coLogin == null)
		{
			Login();
		}
	}

	private void _devInit()
	{
	}

	public void Login()
	{
		_coLogin = StartCoroutine(Login_IE());
	}

	public void HideAllViewButLoading()
	{
		UnityEngine.Debug.Log("HideAllViewButLoading");
		if (!_lockDic.GetBoolValue("HandleNetDown") && !_lockDic.GetBoolValue("Alert"))
		{
			Dzb_Singleton<Dzb_AlertDialog>.GetInstance().Hide();
		}
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
				Dzb_LockManager.UnLock("Esc", Dzb_LockManager.GetValue("Esc") < 10);
			}
			else
			{
				Dzb_LockManager.Lock("Esc");
			}
		}
		UnityEngine.Debug.Log($"SetTouchEnable> [{Dzb_LogHelper.Key(isEnable)}], reason: [{Dzb_LogHelper.Key(reason)}]");
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

	private void CallbackMethod(object[] args)
	{
	}

	private void HandleNetMsg_CheckVersion(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("检测版本:" + JsonWriter.Serialize(args)));
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
			Dzb_LockManager.Lock("Quit");
			Dzb_IOSGameStart.GetSingleton().UpdateGameVesion(downloadadress);
		}
		else
		{
			_ishasVersionChecked = true;
		}
	}

	private void HandleNetMsg_UserLogin(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("登录:" + JsonWriter.Serialize(args)));
		_IsHasLogined = true;
		Dzb_Singleton<Dzb_NetManager>.GetInstance().connectMaxTimes = 50;
		Dzb_Singleton<Dzb_NetManager>.GetInstance().connectMaxTimeout = 30f;
		Dzb_Singleton<Dzb_NetManager>.GetInstance().connectMaxTimes = 30;
		Dzb_Singleton<Dzb_NetManager>.GetInstance().connectMaxTimeout = 20f;
		if (_coLogin != null)
		{
			StopCoroutine(_coLogin);
			_coLogin = null;
		}
		if (Dzb_Singleton<Dzb_ReconnectHint>.GetInstance() != null)
		{
			Dzb_Singleton<Dzb_ReconnectHint>.GetInstance().Hide();
		}
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			MonoBehaviour.print(item.Key + "," + item.Value.ToString() + "----");
		}
		if ((bool)dictionary["success"])
		{
			Dzb_MySqlConnection.user = Dzb_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
			Dzb_Singleton<Dzb_NetManager>.GetInstance().isLogined = true;
			if (_coKeepHeart != null)
			{
				StopCoroutine(_coKeepHeart);
			}
			_coKeepHeart = StartCoroutine(Dzb_Singleton<Dzb_NetManager>.GetInstance().KeepHeart());
			Dzb_Singleton<Dzb_NetManager>.GetInstance().connectCount = 0;
		}
		else
		{
			int code = (int)dictionary["msgCode"];
			Action callback = delegate
			{
				QuitToHallLogin();
				_lockDic["Alert"] = false;
			};
			StartCoroutine(Dzb_Utils.DelayCall(0f, delegate
			{
				if (_coLogin != null)
				{
					StopCoroutine(_coLogin);
				}
			}));
			Dzb_Singleton<Dzb_NetManager>.GetInstance().isLogined = true;
			string errorMsg = Dzb_ErrorCode.GetErrorMsg(code, Dzb_MySqlConnection.language.Equals("zh"));
			_lockDic["Alert"] = true;
			Dzb_LockManager.Lock("Quit");
			SetTouchEnable(isEnable: true, "quit alert", includeEsc: false);
			PrepareQuitGame();
			Dzb_Singleton<Dzb_AlertDialog>.GetInstance().ShowDialog(errorMsg, showOkCancel: false, callback);
		}
	}

	private void Handle_ReLogin(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("重新登录:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string[] array2;
		if (Dzb_MySqlConnection.language == "zh")
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
		if (!(bool)dictionary["reLogin"])
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
			Dzb_Singleton<Dzb_AlertDialog>.GetInstance().ShowDialog(array4[num2], showOkCancel: false, delegate
			{
				QuitToHallLogin();
			});
			return;
		}
		int num3 = (int)dictionary["uiid"];
		if (num3 == 0)
		{
		}
		if (num3 == 1)
		{
			Dzb_MySqlConnection.user = Dzb_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
		}
		if (num3 == 2)
		{
			object[] array5 = (object[])dictionary["roomInfo"];
			int num4 = array5.Length;
			Dzb_Desk[] array6 = new Dzb_Desk[num4];
			for (int i = 0; i < num4; i++)
			{
				array6[i] = Dzb_Desk.CreateWithDic((Dictionary<string, object>)array5[i]);
			}
		}
		if (num3 == 3)
		{
			Dictionary<string, object> dictionary2 = dictionary["deskInfo"] as Dictionary<string, object>;
			int num5 = (int)dictionary2["userScore"];
			int num6 = (int)dictionary2["userGold"];
		}
		if (Dzb_Singleton<Dzb_ReconnectHint>.GetInstance() != null)
		{
			Dzb_Singleton<Dzb_ReconnectHint>.GetInstance().Hide();
		}
		Dzb_Singleton<Dzb_NetManager>.GetInstance().isLogined = true;
		if (_coKeepHeart != null)
		{
			StopCoroutine(_coKeepHeart);
		}
		_coKeepHeart = StartCoroutine(Dzb_Singleton<Dzb_NetManager>.GetInstance().KeepHeart());
		Dzb_Singleton<Dzb_NetManager>.GetInstance().connectCount = 0;
	}

	private void Handle_GetRoomInfo(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("接收房间:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		object[] array = dictionary["RoomInfo"] as object[];
		Dzb_MySqlConnection.roomList.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> data = array[i] as Dictionary<string, object>;
			Dzb_MySqlConnection.roomList.Add(Dzb_Room.CreateWithDic(data));
		}
		Dzb_MySqlConnection.curView = "RoomSelectionView";
		Dzb_Singleton<Dzb_ChooseGame>.GetInstance().UpdateRoomCell();
	}

	public void Handle_RoomInfo(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("接收房间:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if ((int)dictionary["message"] == 0 && Dzb_Singleton<Dzb_ChooseSeat>.GetInstance() != null)
		{
			Dzb_Singleton<Dzb_ChooseGame>.GetInstance().gameObject.SetActive(value: false);
			Dzb_Singleton<Dzb_ChooseSeat>.GetInstance().gameObject.SetActive(value: true);
			Dzb_Singleton<Dzb_UIScene>.GetInstance().UpdateScore();
			object[] array = dictionary["roomInfo"] as object[];
			Dzb_MySqlConnection.seatList.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				Dictionary<string, object> data = array[i] as Dictionary<string, object>;
				Dzb_MySqlConnection.seatList.Add(Dzb_Seat.CreateWithDic(data));
			}
			Dzb_MySqlConnection.curView = "DeskSelectionView";
			Dzb_Singleton<Dzb_ChooseSeat>.GetInstance().UpdateSeatCell();
		}
	}

	public void Handle_UpdateRoomInfo(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("接收更新房间:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		for (int i = 0; i < Dzb_MySqlConnection.roomList.Count; i++)
		{
			if (Dzb_MySqlConnection.roomList[i].roomId == (int)dictionary["roomId"])
			{
				Dzb_MySqlConnection.roomList[i].PeopleNum = (int)dictionary["roomPlayCount"];
				break;
			}
		}
		if (Dzb_MySqlConnection.room != null && Dzb_MySqlConnection.room.roomId == (int)dictionary["roomId"])
		{
			object[] array = dictionary["desks"] as object[];
			Dzb_MySqlConnection.seatList.Clear();
			for (int j = 0; j < array.Length; j++)
			{
				Dictionary<string, object> data = array[j] as Dictionary<string, object>;
				Dzb_MySqlConnection.seatList.Add(Dzb_Seat.CreateWithDic(data));
			}
		}
		if (Dzb_MySqlConnection.curView == "RoomSelectionView")
		{
			Dzb_Singleton<Dzb_ChooseGame>.GetInstance().UpdateRoomCell();
		}
		if (Dzb_MySqlConnection.curView == "DeskSelectionView")
		{
			Dzb_Singleton<Dzb_ChooseSeat>.GetInstance().UpdateSeatCell();
		}
	}

	public void Handle_GetDesksInfo(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("接收转台房间:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			object[] array = dictionary["DesksInfo"] as object[];
			Dzb_MySqlConnection.seatList.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				Dictionary<string, object> data = array[i] as Dictionary<string, object>;
				Dzb_MySqlConnection.seatList.Add(Dzb_Seat.CreateWithDic(data));
			}
			Dzb_Singleton<Dzb_GameManager>.GetInstance().UpdateSeatCell();
		}
	}

	public void Handle_EnterDesk(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("接收桌子（座位）:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if (Dzb_MySqlConnection.room.RoomType == 4)
		{
			if (dictionary.ContainsKey("UserGold"))
			{
				Dzb_MySqlConnection.user.expeGold = (int)dictionary["UserGold"];
			}
			if (dictionary.ContainsKey("gameScore"))
			{
				Dzb_MySqlConnection.user.expeScore = (int)dictionary["gameScore"];
			}
		}
		else
		{
			if (dictionary.ContainsKey("UserGold"))
			{
				Dzb_MySqlConnection.user.gameGold = (int)dictionary["UserGold"];
			}
			if (dictionary.ContainsKey("gameScore"))
			{
				Dzb_MySqlConnection.user.gameScore = (int)dictionary["gameScore"];
			}
		}
		if (dictionary.ContainsKey("message"))
		{
			if ((int)dictionary["message"] == 1)
			{
				Dzb_Singleton<Dzb_AlertDialog>.GetInstance().ShowDialog("用户进入房间失败,找不到房间", showOkCancel: false, delegate
				{
				});
			}
			else if ((int)dictionary["message"] == 2)
			{
				Dzb_Singleton<Dzb_AlertDialog>.GetInstance().ShowDialog("金币不足", showOkCancel: false, delegate
				{
				});
			}
			Dzb_Singleton<Dzb_AlertDialogText>.GetInstance().Hide();
		}
		if ((bool)dictionary["success"])
		{
			SceneManager.LoadSceneAsync("DzbGame");
		}
	}

	public void Handle_SyncGold(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("接收退出桌子（座位）:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		Dzb_MySqlConnection.user.expeGold = (int)dictionary["expeGold"];
		Dzb_MySqlConnection.user.expeScore = 0;
		Dzb_MySqlConnection.user.gameGold = (int)dictionary["gameGold"];
		Dzb_MySqlConnection.user.gameScore = 0;
		Dzb_Singleton<Dzb_GameInfo>.GetInstance()._IsQuitGame = true;
		if (dictionary.ContainsKey("msg"))
		{
			if ((int)dictionary["msg"] == 1)
			{
				Dzb_Singleton<Dzb_AlertDialog>.GetInstance().ShowDialog("长时间未操作，已被踢出座位", showOkCancel: false, delegate
				{
					SceneManager.LoadSceneAsync("DzbHall");
				});
			}
			else
			{
				SceneManager.LoadSceneAsync("DzbHall");
			}
		}
	}

	public void Handle_QuitToRoom(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("接收退出桌子（座位）:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
	}

	public void Handle_UpdateGoldAndScore(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("接收更新分数（存取分）:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if (Dzb_MySqlConnection.room.RoomType == 4)
		{
			Dzb_MySqlConnection.user.expeScore = (int)dictionary["gameScore"];
			Dzb_MySqlConnection.user.expeGold = (int)dictionary["gold"];
		}
		else
		{
			Dzb_MySqlConnection.user.gameScore = (int)dictionary["gameScore"];
			Dzb_MySqlConnection.user.gameGold = (int)dictionary["gold"];
		}
		Dzb_Singleton<Dzb_GameManager>.GetInstance().UpdateScore();
	}

	public void Handle_GetDaJiangBan(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("接收大奖榜:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		object[] jiangs = new object[0];
		if (dictionary.ContainsKey("daJiangBan"))
		{
			jiangs = (dictionary["daJiangBan"] as object[]);
		}
		Dzb_Singleton<Dzb_GameManager>.GetInstance().UpdateAwards(jiangs);
	}

	public void Handle_GtDeskHistory(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("接收历史记录:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		Dzb_Singleton<Dzb_GameManager>.GetInstance().UpdateQuery((int[])dictionary["historyCard"]);
	}

	private void Handle_OneCards(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle(" 第一手牌:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		Dzb_Singleton<Dzb_GameManager>.GetInstance().MyCards = (int[])dictionary["once_cards"];
		string text = string.Empty;
		Dzb_CardType dzb_CardType = new Dzb_CardType();
		for (int i = 0; i < Dzb_Singleton<Dzb_GameManager>.GetInstance().MyCards.Length; i++)
		{
			string text2 = text;
			text = text2 + Dzb_Singleton<Dzb_GameManager>.GetInstance().MyCards[i] + "(" + dzb_CardType.CardValueToString(Dzb_Singleton<Dzb_GameManager>.GetInstance().MyCards[i]) + ") ";
		}
		UnityEngine.Debug.Log(text);
		if (Dzb_MySqlConnection.room.RoomType == 4)
		{
			Dzb_MySqlConnection.user.expeScore = (int)dictionary["userScore"];
		}
		else
		{
			Dzb_MySqlConnection.user.gameScore = (int)dictionary["userScore"];
		}
		Dzb_Singleton<Dzb_GameManager>.GetInstance().DrawCards();
		Dzb_Singleton<Dzb_GameManager>.GetInstance().UpdateScore();
		Dzb_Singleton<Dzb_GameManager>.GetInstance().CardTypeNum = (int)dictionary["type_expect"];
		Dzb_Singleton<Dzb_GameManager>.GetInstance().ShowEffect((int)dictionary["type_expect"]);
		Dzb_Singleton<Dzb_GameManager>.GetInstance().IsGetOneCards = true;
	}

	private void Handle_TwiceCard(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle(" 第二手牌:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		Dzb_Singleton<Dzb_GameManager>.GetInstance().MyCards = (int[])dictionary["twice_cards"];
		string text = string.Empty;
		Dzb_CardType dzb_CardType = new Dzb_CardType();
		for (int i = 0; i < Dzb_Singleton<Dzb_GameManager>.GetInstance().MyCards.Length; i++)
		{
			string text2 = text;
			text = text2 + Dzb_Singleton<Dzb_GameManager>.GetInstance().MyCards[i] + "(" + dzb_CardType.CardValueToString(Dzb_Singleton<Dzb_GameManager>.GetInstance().MyCards[i]) + ") ";
		}
		UnityEngine.Debug.Log(text);
		if (Dzb_MySqlConnection.room.RoomType == 4)
		{
			Dzb_MySqlConnection.user.expeScore = (int)dictionary["userScore"];
		}
		else
		{
			Dzb_MySqlConnection.user.gameScore = (int)dictionary["userScore"];
		}
		Dzb_Singleton<Dzb_GameManager>.GetInstance().userWin = (int)dictionary["userWin"];
		Dzb_Singleton<Dzb_GameManager>.GetInstance().CardTypeNum = (int)dictionary["twice_type"];
		Dzb_Singleton<Dzb_GameManager>.GetInstance().DrawCards_Twice();
		Dzb_Singleton<Dzb_GameManager>.GetInstance().ShowEffect((int)dictionary["twice_type"]);
		Dzb_Singleton<Dzb_GameManager>.GetInstance().ShowScore((int)dictionary["userScore"] - (int)dictionary["userWin"]);
		if (dictionary.ContainsKey("reward"))
		{
			Dzb_Singleton<Dzb_GameManager>.GetInstance().ShowAwards((int)dictionary["reward"]);
		}
		int zhuanshi = 0;
		int heart = 0;
		int box = 0;
		if (dictionary.ContainsKey("ZuanShiNum"))
		{
			zhuanshi = (int)dictionary["ZuanShiNum"];
		}
		if (dictionary.ContainsKey("HeartNum"))
		{
			heart = (int)dictionary["HeartNum"];
		}
		if (dictionary.ContainsKey("BaoXiangSongWin"))
		{
			box = (int)dictionary["BaoXiangSongWin"];
		}
		Dzb_Singleton<Dzb_GameManager>.GetInstance().UpdateHeartAndDiamond(heart, zhuanshi, box);
		Dzb_Singleton<Dzb_GameManager>.GetInstance().IsGetTwoCards = true;
	}

	public void HandleNetMsg_QuitToLogin(object[] args)
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.NetHandle("HandleNetMsg_QuitToLogin：") + JsonWriter.Serialize(args));
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
		Dzb_Singleton<Dzb_AlertDialog>.GetInstance().ShowDialog(empty, showOkCancel: false, delegate
		{
			QuitToHallLogin();
			_lockDic["Alert"] = false;
		});
	}

	private void Handle_NetDown(object[] args)
	{
		UnityEngine.Debug.Log("Handle_NetDown");
		UnityEngine.Debug.Log(string.Format("curView: {0}, isReady: {1}, _lockDic.HandleNetDown: {2}, connectCount: {3}", Dzb_MySqlConnection.curView, Dzb_Singleton<Dzb_NetManager>.GetInstance().isReady, _lockDic.GetBoolValue("HandleNetDown"), Dzb_Singleton<Dzb_NetManager>.GetInstance().connectCount));
		if (Dzb_LockManager.IsLocked("Overflow") || Dzb_LockManager.IsLocked("Quit"))
		{
			return;
		}
		if (Dzb_Singleton<Dzb_NetManager>.GetInstance().isReady)
		{
			if (Dzb_Singleton<Dzb_ReconnectHint>.GetInstance() != null)
			{
				Dzb_Singleton<Dzb_ReconnectHint>.GetInstance().Hide();
			}
		}
		else
		{
			if (Dzb_LockManager.IsLocked("HandleNetDown_Quit"))
			{
				return;
			}
			_lockDic["HandleNetDown"] = true;
			Exception ex = null;
			if (args != null)
			{
				ex = (args[0] as Exception);
			}
			_IsReconnecting = true;
			UnityEngine.Debug.Log(ex);
			bool flag = false;
			if (Dzb_MySqlConnection.curView == "LoadingView" && flag)
			{
				string content = string.Format((Dzb_MySqlConnection.language == "zh") ? "网络连接已断开，错误代码为 [{0}]" : "Net Error [{0}]", (ex == null) ? "9999" : ex.Message);
				Dzb_Singleton<Dzb_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
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
			if ((Dzb_Singleton<Dzb_NetManager>.GetInstance().connectCount > Dzb_Singleton<Dzb_NetManager>.GetInstance().connectMaxTimes && Dzb_Singleton<Dzb_NetManager>.GetInstance().connectTimeCount > Dzb_Singleton<Dzb_NetManager>.GetInstance().connectMaxTimeout / 2f) || Dzb_Singleton<Dzb_NetManager>.GetInstance().connectTimeCount > Dzb_Singleton<Dzb_NetManager>.GetInstance().connectMaxTimeout)
			{
				_timeoutQuit();
				return;
			}
			_coLogin = StartCoroutine(Login_IE());
			if (Dzb_Singleton<Dzb_ReconnectHint>.GetInstance() != null)
			{
				Dzb_Singleton<Dzb_ReconnectHint>.GetInstance().Show();
			}
		}
	}

	private void GetInitData()
	{
		Dzb_MySqlConnection.versionCode = "9.9.1";
		Dzb_MySqlConnection.username = ZH2_GVars.username;
		Dzb_MySqlConnection.pwd = ZH2_GVars.pwd;
		Dzb_MySqlConnection.IPAddress = ZH2_GVars.IPAddress_Game;
		Dzb_MySqlConnection.language = "zh";
		Dzb_MySqlConnection.IPPort = 10042;
		Dzb_MySqlConnection.username = ZH2_GVars.username;
		Dzb_MySqlConnection.pwd = ZH2_GVars.pwd;
		Dzb_MySqlConnection.IPAddress = ZH2_GVars.IPAddress_Game;
		Dzb_MySqlConnection.language = "zh";
		Dzb_MySqlConnection.IPPort = 10042;
		if (Dzb_MySqlConnection.language != "zh" && Dzb_MySqlConnection.language != "en")
		{
			Dzb_MySqlConnection.language = "zh";
		}
	}

	private int GetSceneId()
	{
		int result = 0;
		if (Dzb_MySqlConnection.curView == "LoadingView")
		{
			result = 0;
		}
		else if (Dzb_MySqlConnection.curView == "RoomSelectionView")
		{
			result = 1;
		}
		else if (Dzb_MySqlConnection.curView == "DeskSelectionView")
		{
			result = 2;
		}
		else if (Dzb_MySqlConnection.curView == "DzbGame")
		{
			result = 3;
		}
		return result;
	}

	private void _timeoutQuit()
	{
		UnityEngine.Debug.Log($"connectCount: {Dzb_Singleton<Dzb_NetManager>.GetInstance().connectCount}, connectTimeCount: {Dzb_Singleton<Dzb_NetManager>.GetInstance().connectTimeCount}");
		UnityEngine.Debug.Log($"connectMaxTimes: {Dzb_Singleton<Dzb_NetManager>.GetInstance().connectMaxTimes}, connectMaxTimeout: {Dzb_Singleton<Dzb_NetManager>.GetInstance().connectMaxTimeout}");
		string content = (Dzb_MySqlConnection.language == "zh") ? "网络连接错误，请检查网络连接" : "Unable to connect to server，Please check your network connection";
		if (Dzb_Singleton<Dzb_ReconnectHint>.GetInstance() != null)
		{
			Dzb_Singleton<Dzb_ReconnectHint>.GetInstance().Hide();
		}
		Dzb_LockManager.Lock("HandleNetDown_Quit");
		PrepareQuitGame();
		Dzb_Singleton<Dzb_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			QuitToHallLogin();
			_lockDic["HandleNetDown"] = false;
		});
	}

	public void PrepareQuitGame()
	{
		_lockDic["Alert"] = true;
		Dzb_LockManager.Lock("Quit");
		if (Dzb_Singleton<Dzb_ReconnectHint>.GetInstance() != null)
		{
			Dzb_Singleton<Dzb_ReconnectHint>.GetInstance().Hide();
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
		Dzb_Singleton<Dzb_NetManager>.GetInstance().Disconnect();
	}

	public void PrepareQuitGame2()
	{
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
		Dzb_Singleton<Dzb_NetManager>.GetInstance().Disconnect();
	}

	public void QuitToHallLogin()
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.Magenta("QuitToHallLogin"));
		SetTouchEnable(isEnable: false, "QuitToHallLogin");
		_lockDic.Clear();
		Dzb_LockManager.ClearLock();
		UnityEngine.Object.Destroy(Dzb_Singleton<Dzb_GameInfo>.GetInstance().gameObject);
		UnityEngine.Object.Destroy(Dzb_Singleton<Dzb_GameRoot>.GetInstance().gameObject);
		ZH2_GVars.isStartedFromGame = false;
		SceneManager.LoadScene("MainScene");
	}

	private void Send_Login()
	{
		UnityEngine.Debug.Log("发送登录了：" + Dzb_MySqlConnection.username + "||" + Dzb_MySqlConnection.pwd);
		object[] args = new object[2]
		{
			Dzb_MySqlConnection.username,
			Dzb_MySqlConnection.pwd
		};
		Dzb_Singleton<Dzb_NetManager>.GetInstance().Send("userService/userLogin", args);
	}

	private void Send_ReLogin()
	{
		UnityEngine.Debug.Log("发送重新登录");
		Dzb_Singleton<Dzb_NetManager>.GetInstance().Send("userService/reLogin", new object[3]
		{
			Dzb_MySqlConnection.username,
			Dzb_MySqlConnection.pwd,
			GetSceneId()
		});
	}

	public void Send_QuitGame()
	{
		object[] args = new object[0];
		Dzb_Singleton<Dzb_NetManager>.GetInstance().Send("userService/quitGame", args);
		Dzb_LockManager.Lock("Quit");
		SetTouchEnable(isEnable: false, "Send_QuitGame");
	}

	private IEnumerator Login_IE()
	{
		_ishasVersionChecked = false;
		if (!_IsHasLogined)
		{
			Dzb_Singleton<Dzb_NetManager>.GetInstance().connectMaxTimes = 10;
			Dzb_Singleton<Dzb_NetManager>.GetInstance().connectMaxTimeout = 5f;
		}
		if (!_IsReconnecting)
		{
			GetInitData();
		}
		else
		{
			Dzb_Singleton<Dzb_NetManager>.GetInstance().Disconnect();
			yield return new WaitForSeconds(0.5f);
		}
		if (!_IsHasLogined)
		{
			Dzb_MySqlConnection.IPAddress = Dzb_CheckIP.DoGetHostAddresses(Dzb_MySqlConnection.IPAddress);
		}
		Dzb_Singleton<Dzb_NetManager>.GetInstance().Connect(Dzb_MySqlConnection.IPAddress, Dzb_MySqlConnection.IPPort);
		while (!Dzb_Singleton<Dzb_NetManager>.GetInstance().isConnected)
		{
			yield return null;
		}
		while (!_ishasVersionChecked)
		{
			yield return null;
		}
		Dzb_Singleton<Dzb_NetManager>.GetInstance().SendPublicKey();
		while (!Dzb_Singleton<Dzb_NetManager>.GetInstance().isReady)
		{
			yield return null;
		}
		float waitTime = _IsReconnecting ? 0.2f : 0.2f;
		yield return new WaitForSeconds(waitTime);
		if (_IsTest)
		{
			yield break;
		}
		if (_IsHasLogined)
		{
			Send_ReLogin();
			yield break;
		}
		Send_Login();
		if (!_IsReconnecting)
		{
			StartCoroutine(_loadingLoginTimeout());
		}
		yield return null;
	}

	private IEnumerator _loadingLoginTimeout()
	{
		yield return new WaitForSeconds(5f);
		if (!_IsHasLogined && !Dzb_Singleton<Dzb_NetManager>.GetInstance().useFake)
		{
			UnityEngine.Debug.Log(Dzb_LogHelper.Magenta("_loadingLoginTimeout"));
			_timeoutQuit();
		}
	}
}
