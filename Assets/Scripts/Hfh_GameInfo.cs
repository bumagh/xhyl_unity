using JsonFx.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hfh_GameInfo : Hfh_Singleton<Hfh_GameInfo>
{
	private bool _ishasVersionChecked;

	private bool _ishasReconnecting;

	private Coroutine _coLoading;

	private Coroutine _coLogin;

	private Coroutine _coKeepHeart;

	public static float SoundVolume;

	public static float MusicVolume;

	public int RoomId;

	public int DeskId;

	public bool _IsQuitGame;

	private bool IsInit;

	private void Awake()
	{
		if (Hfh_Singleton<Hfh_GameInfo>._instance == null)
		{
			Hfh_Singleton<Hfh_GameInfo>.SetInstance(this);
			IsInit = true;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			Application.runInBackground = true;
		}
		else
		{
			Hfh_Singleton<Hfh_GameInfo>._instance._ishasReconnecting = false;
			Hfh_Singleton<Hfh_GameInfo>._instance._ishasVersionChecked = false;
			Hfh_Singleton<Hfh_NetManager>.GetInstance().isConnected = false;
			Hfh_Singleton<Hfh_NetManager>.GetInstance().isReady = false;
			Hfh_Singleton<Hfh_NetManager>.GetInstance().Connect(Hfh_GVars.IPAddress, Hfh_GVars.IPPort);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		if (IsInit)
		{
			_Init();
			IsInit = false;
		}
	}

	private void _Init()
	{
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("heart", delegate
		{
		});
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("checkVersion", HandleNetMsg_CheckVersion);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("userLogin", HandleNetMsg_UserLogin);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("reLogin", Handle_ReLogin);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("getRoomInfo", Handle_GetRoomInfo);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("roomInfo", Handle_RoomInfo);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("updateRoomInfo", Handle_UpdateRoomInfo);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("enterDesk", Handle_EnterDesk);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("getDesksInfo", Handle_GetDesksInfo);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("syncGold", Handle_SyncGold);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("quitToRoom", Handle_QuitToRoom);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("updateGoldAndScore", Handle_UpdateGoldAndScore);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("getDeskHistory", Handle_GtDeskHistory);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("oneCardResult", Handle_OneCards);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("twoCardResult", Handle_TwiceCard);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("startMultiple", Handle_StartMultiple);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("multipResult", Handle_MultipResult);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("EndMultiple", Handle_EndMultiple);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("netDown", Handle_NetDown);
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("timeout", delegate(object[] args)
		{
			UnityEngine.Debug.Log(Hfh_LogHelper.Magenta("timeout>  id: " + (int)args[0]));
		});
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("heart timeout", delegate
		{
			UnityEngine.Debug.Log(Hfh_LogHelper.Magenta("heart timeout"));
			Handle_NetDown(new object[1]
			{
				new Exception("heart timeout")
			});
		});
		Hfh_Singleton<Hfh_NetManager>.GetInstance().RegisterHandler("quitToLogin", HandleNetMsg_QuitToLogin);
		if (_coLogin != null)
		{
			StopCoroutine(_coLogin);
			_coLogin = null;
		}
		_coLogin = StartCoroutine(Login_IE());
	}

	private void GetInitData()
	{
		Hfh_GVars.versionCode = "9.9.1";
		Hfh_GVars.username = ZH2_GVars.username;
		Hfh_GVars.pwd = ZH2_GVars.pwd;
		Hfh_GVars.IPAddress = ZH2_GVars.IPAddress_Game;
		Hfh_GVars.language = "zh";
		Hfh_GVars.IPPort = 10048;
		Hfh_GVars.username = ZH2_GVars.username;
		Hfh_GVars.pwd = ZH2_GVars.pwd;
		Hfh_GVars.IPAddress = ZH2_GVars.IPAddress_Game;
		Hfh_GVars.language = "zh";
		Hfh_GVars.IPPort = 10048;
		if (Hfh_GVars.language != "zh" && Hfh_GVars.language != "en")
		{
			Hfh_GVars.language = "zh";
		}
	}

	private int GetSceneId()
	{
		int result = 0;
		if (Hfh_GVars.curView == "LoadingView")
		{
			result = 0;
		}
		else if (Hfh_GVars.curView == "RoomSelectionView")
		{
			result = 1;
		}
		else if (Hfh_GVars.curView == "DeskSelectionView")
		{
			result = 2;
		}
		else if (Hfh_GVars.curView == "HfhGame")
		{
			result = 3;
		}
		return result;
	}

	private void _timeoutQuit()
	{
		UnityEngine.Debug.Log($"connectCount: {Hfh_Singleton<Hfh_NetManager>.GetInstance().connectCount}, connectTimeCount: {Hfh_Singleton<Hfh_NetManager>.GetInstance().connectTimeCount}");
		UnityEngine.Debug.Log($"connectMaxTimes: {Hfh_Singleton<Hfh_NetManager>.GetInstance().connectMaxTimes}, connectMaxTimeout: {Hfh_Singleton<Hfh_NetManager>.GetInstance().connectMaxTimeout}");
		string text = (Hfh_GVars.language == "zh") ? "网络连接错误，请检查网络连接" : "Unable to connect to server，Please check your network connection";
	}

	public void PrepareQuitGame()
	{
		if (_coLoading != null)
		{
			StopCoroutine(_coLoading);
			_coLoading = null;
		}
		if (_coLogin != null)
		{
			StopCoroutine(_coLogin);
			_coLogin = null;
		}
		if (_coKeepHeart != null)
		{
			StopCoroutine(_coKeepHeart);
			_coKeepHeart = null;
		}
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Disconnect();
	}

	public void QuitToHallLogin()
	{
		UnityEngine.Object.Destroy(Hfh_Singleton<Hfh_GameInfo>.GetInstance().gameObject);
		ZH2_GVars.isStartedFromGame = false;
		SceneManager.LoadScene("MainScene");
	}

	private void CallbackMethod(object[] args)
	{
	}

	private void HandleNetMsg_CheckVersion(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("检测版本:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary = (args[0] as Dictionary<string, object>);
		if (!(bool)dictionary["success"])
		{
		}
		if (!(bool)dictionary["haveNewVersion"])
		{
			_ishasVersionChecked = true;
		}
	}

	private void HandleNetMsg_UserLogin(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("登录:" + JsonWriter.Serialize(args)));
		Hfh_Singleton<Hfh_NetManager>.GetInstance().connectMaxTimes = 50;
		Hfh_Singleton<Hfh_NetManager>.GetInstance().connectMaxTimeout = 30f;
		Hfh_Singleton<Hfh_NetManager>.GetInstance().connectMaxTimes = 30;
		Hfh_Singleton<Hfh_NetManager>.GetInstance().connectMaxTimeout = 20f;
		if (_coLogin != null)
		{
			StopCoroutine(_coLogin);
			_coLogin = null;
		}
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			Hfh_GVars.user = Hfh_User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
			Hfh_Singleton<Hfh_NetManager>.GetInstance().isLogined = true;
			if (_coKeepHeart != null)
			{
				StopCoroutine(_coKeepHeart);
			}
			_coKeepHeart = StartCoroutine(Hfh_Singleton<Hfh_NetManager>.GetInstance().KeepHeart());
			Hfh_Singleton<Hfh_NetManager>.GetInstance().connectCount = 0;
		}
		else
		{
			int code = (int)dictionary["msgCode"];
			Action callback = delegate
			{
				QuitToHallLogin();
			};
			StartCoroutine(Hfh_Utils.DelayCall(0f, delegate
			{
				if (_coLogin != null)
				{
					StopCoroutine(_coLogin);
				}
			}));
			Hfh_Singleton<Hfh_NetManager>.GetInstance().isLogined = true;
			string errorMsg = Hfh_ErrorCode.GetErrorMsg(code, Hfh_GVars.language.Equals("zh"));
			PrepareQuitGame();
			Hfh_Singleton<Hfh_AlertDialog>.GetInstance().ShowDialog(errorMsg, showOkCancel: false, callback);
		}
	}

	private void Handle_ReLogin(object[] args)
	{
	}

	private void Handle_GetRoomInfo(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("接收房间:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		object[] array = dictionary["RoomInfo"] as object[];
		Hfh_GVars.roomList.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> data = array[i] as Dictionary<string, object>;
			Hfh_GVars.roomList.Add(Hfh_Room.CreateWithDic(data));
		}
		Hfh_GVars.curView = "RoomSelectionView";
		Hfh_Singleton<Hfh_ChooseGame>.GetInstance().UpdateRoomCell();
	}

	public void Handle_RoomInfo(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("接收房间:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if ((int)dictionary["message"] == 0)
		{
			Hfh_Singleton<Hfh_ChooseGame>.GetInstance().gameObject.SetActive(value: false);
			Hfh_Singleton<Hfh_ChooseSeat>.GetInstance().gameObject.SetActive(value: true);
			Hfh_Singleton<Hfh_UIScene>.GetInstance().UpdateScore();
			object[] array = dictionary["roomInfo"] as object[];
			Hfh_GVars.seatList.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				Dictionary<string, object> data = array[i] as Dictionary<string, object>;
				Hfh_GVars.seatList.Add(Hfh_Seat.CreateWithDic(data));
			}
			Hfh_GVars.curView = "DeskSelectionView";
			Hfh_Singleton<Hfh_ChooseSeat>.GetInstance().UpdateSeatCell();
		}
	}

	public void Handle_UpdateRoomInfo(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("接收更新房间:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		for (int i = 0; i < Hfh_GVars.roomList.Count; i++)
		{
			if (Hfh_GVars.roomList[i].roomId == (int)dictionary["roomId"])
			{
				Hfh_GVars.roomList[i].PeopleNum = (int)dictionary["roomPlayCount"];
				break;
			}
		}
		if (Hfh_GVars.room != null && Hfh_GVars.room.roomId == (int)dictionary["roomId"])
		{
			object[] array = dictionary["desks"] as object[];
			Hfh_GVars.seatList.Clear();
			for (int j = 0; j < array.Length; j++)
			{
				Dictionary<string, object> data = array[j] as Dictionary<string, object>;
				Hfh_GVars.seatList.Add(Hfh_Seat.CreateWithDic(data));
			}
		}
		if (Hfh_GVars.curView == "RoomSelectionView")
		{
			Hfh_Singleton<Hfh_ChooseGame>.GetInstance().UpdateRoomCell();
		}
		if (Hfh_GVars.curView == "DeskSelectionView")
		{
			Hfh_Singleton<Hfh_ChooseSeat>.GetInstance().UpdateSeatCell();
		}
	}

	public void Handle_EnterDesk(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("接收桌子（座位）:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if (dictionary.ContainsKey("message"))
		{
			if ((int)dictionary["message"] == 1)
			{
				Hfh_Singleton<Hfh_AlertDialog>.GetInstance().ShowDialog("用户进入房间失败,找不到房间", showOkCancel: false, delegate
				{
				});
			}
			else if ((int)dictionary["message"] == 2)
			{
				Hfh_Singleton<Hfh_AlertDialog>.GetInstance().ShowDialog("金币不足", showOkCancel: false, delegate
				{
				});
			}
		}
		if (dictionary.ContainsKey("gameScore"))
		{
			if (Hfh_GVars.room.RoomType == 4)
			{
				Hfh_GVars.user.expeScore = (int)dictionary["gameScore"];
				if (dictionary.ContainsKey("gold"))
				{
					Hfh_GVars.user.expeGold = (int)dictionary["gold"];
				}
			}
			else
			{
				Hfh_GVars.user.gameScore = (int)dictionary["gameScore"];
				if (dictionary.ContainsKey("gold"))
				{
					Hfh_GVars.user.gameGold = (int)dictionary["gold"];
				}
			}
		}
		if ((bool)dictionary["success"])
		{
			SceneManager.LoadSceneAsync("HfhGame");
		}
	}

	public void Handle_GetDesksInfo(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("接收转台房间:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			object[] array = dictionary["DesksInfo"] as object[];
			Hfh_GVars.seatList.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				Dictionary<string, object> data = array[i] as Dictionary<string, object>;
				Hfh_GVars.seatList.Add(Hfh_Seat.CreateWithDic(data));
			}
			Hfh_Singleton<Hfh_GameManager>.GetInstance().UpdateSeatCell();
		}
	}

	public void Handle_SyncGold(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("接收退出桌子（座位）:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		Hfh_GVars.user.expeGold = (int)dictionary["expeGold"];
		Hfh_GVars.user.expeScore = 0;
		Hfh_GVars.user.gameGold = (int)dictionary["gameGold"];
		Hfh_GVars.user.gameScore = 0;
		Hfh_Singleton<Hfh_GameInfo>.GetInstance()._IsQuitGame = true;
		if (dictionary.ContainsKey("msg"))
		{
			if ((int)dictionary["msg"] == 1)
			{
				Hfh_Singleton<Hfh_AlertDialog>.GetInstance().ShowDialog("长时间未操作，已被踢出座位", showOkCancel: false, delegate
				{
					SceneManager.LoadSceneAsync("HfhHall");
				});
			}
			else
			{
				SceneManager.LoadSceneAsync("HfhHall");
			}
		}
	}

	public void Handle_QuitToRoom(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("接收退出桌子（座位）:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
	}

	public void Handle_UpdateGoldAndScore(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("接收更新分数（存取分）:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if (Hfh_GVars.room.RoomType == 4)
		{
			Hfh_GVars.user.expeScore = (int)dictionary["gameScore"];
			Hfh_GVars.user.expeGold = (int)dictionary["gold"];
		}
		else
		{
			Hfh_GVars.user.gameScore = (int)dictionary["gameScore"];
			Hfh_GVars.user.gameGold = (int)dictionary["gold"];
		}
		Hfh_Singleton<Hfh_GameManager>.GetInstance().UpdateScore();
	}

	public void Handle_GtDeskHistory(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("接收历史记录:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		Hfh_Singleton<Hfh_GameManager>.GetInstance().UpdateQuery(dictionary);
	}

	private void Handle_OneCards(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle(" 第一手牌:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		Hfh_Singleton<Hfh_GameManager>.GetInstance().IsGetOneCards = true;
		Hfh_Singleton<Hfh_GameManager>.GetInstance().MyCards = (int[])dictionary["once_cards"];
		string text = string.Empty;
		Hfh_CardType hfh_CardType = new Hfh_CardType();
		for (int i = 0; i < Hfh_Singleton<Hfh_GameManager>.GetInstance().MyCards.Length; i++)
		{
			string text2 = text;
			text = text2 + Hfh_Singleton<Hfh_GameManager>.GetInstance().MyCards[i] + "(" + hfh_CardType.CardValueToString(Hfh_Singleton<Hfh_GameManager>.GetInstance().MyCards[i]) + ") ";
		}
		UnityEngine.Debug.Log(text);
		if (Hfh_GVars.room.RoomType == 4)
		{
			Hfh_GVars.user.expeScore = (int)dictionary["userScore"];
		}
		else
		{
			Hfh_GVars.user.gameScore = (int)dictionary["userScore"];
		}
		Hfh_Singleton<Hfh_GameManager>.GetInstance().UpdateScore();
		Hfh_Singleton<Hfh_GameManager>.GetInstance().DrawCards();
	}

	private void Handle_TwiceCard(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle(" 第二手牌:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		Hfh_Singleton<Hfh_GameManager>.GetInstance().IsGetTwoCards = true;
		Hfh_Singleton<Hfh_GameManager>.GetInstance().MyCards = (int[])dictionary["twice_cards"];
		string text = string.Empty;
		Hfh_CardType hfh_CardType = new Hfh_CardType();
		for (int i = 0; i < Hfh_Singleton<Hfh_GameManager>.GetInstance().MyCards.Length; i++)
		{
			string text2 = text;
			text = text2 + Hfh_Singleton<Hfh_GameManager>.GetInstance().MyCards[i] + "(" + hfh_CardType.CardValueToString(Hfh_Singleton<Hfh_GameManager>.GetInstance().MyCards[i]) + ") ";
		}
		UnityEngine.Debug.Log(text);
		if (Hfh_GVars.room.RoomType == 4)
		{
			Hfh_GVars.user.expeScore = (int)dictionary["userScore"];
		}
		else
		{
			Hfh_GVars.user.gameScore = (int)dictionary["userScore"];
		}
		Hfh_Singleton<Hfh_GameManager>.GetInstance().userWin = (int)dictionary["userWin"];
		Hfh_Singleton<Hfh_GameManager>.GetInstance().CardTypeNum = (int)dictionary["twice_type"];
		Hfh_Singleton<Hfh_GameManager>.GetInstance().DrawCards_Twice();
		Hfh_Singleton<Hfh_GameManager>.GetInstance().ShowEffect((int)dictionary["twice_type"]);
		if (dictionary.ContainsKey("reward"))
		{
			Hfh_Singleton<Hfh_GameManager>.GetInstance().ShowAwards((int)dictionary["reward"]);
		}
	}

	private void Handle_StartMultiple(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle(" 开始比倍:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		object[] array = dictionary["historyCard"] as object[];
		Queue<int> queue = new Queue<int>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == null)
			{
				continue;
			}
			object[] array2 = (array[i] as Dictionary<string, object>)["historyMultipleList"] as object[];
			for (int j = 0; j < array2.Length; j++)
			{
				Dictionary<string, object> dictionary2 = array2[j] as Dictionary<string, object>;
				if (queue.Count >= 6)
				{
					queue.Dequeue();
					queue.Enqueue((int)dictionary2["Card"]);
				}
				else
				{
					queue.Enqueue((int)dictionary2["Card"]);
				}
			}
		}
		Hfh_Singleton<Hfh_GameManager>.GetInstance().ShowBiBeiPanel(queue.ToArray());
	}

	private void Handle_MultipResult(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle(" 比倍结果:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if (Hfh_GVars.room.RoomType == 4)
		{
			Hfh_GVars.user.expeScore = (int)dictionary["UserScore"];
		}
		else
		{
			Hfh_GVars.user.gameScore = (int)dictionary["UserScore"];
		}
		Hfh_Singleton<Hfh_GameManager>.GetInstance().userWin = (int)dictionary["winScore"];
		Hfh_Singleton<Hfh_GameManager>.GetInstance().BiBeiResult((int)dictionary["BigOrSmall"], (int)dictionary["card"]);
	}

	private void Handle_EndMultiple(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle(" 得分:" + JsonWriter.Serialize(args)));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		if (Hfh_GVars.room.RoomType == 4)
		{
			Hfh_GVars.user.expeScore = (int)dictionary["UserScore"];
		}
		else
		{
			Hfh_GVars.user.gameScore = (int)dictionary["UserScore"];
		}
		Hfh_Singleton<Hfh_GameManager>.GetInstance().userWin = (int)dictionary["winScore"];
		Hfh_Singleton<Hfh_GameManager>.GetInstance().EndMultiple();
	}

	public void HandleNetMsg_QuitToLogin(object[] args)
	{
		UnityEngine.Debug.Log(Hfh_LogHelper.NetHandle("退出登录：") + JsonWriter.Serialize(args));
	}

	private void Handle_NetDown(object[] args)
	{
		UnityEngine.Debug.Log("断开连接！");
	}

	private IEnumerator Login_IE()
	{
		_ishasVersionChecked = false;
		GetInitData();
		if (_ishasReconnecting)
		{
			Hfh_Singleton<Hfh_NetManager>.GetInstance().Disconnect();
			yield return new WaitForSeconds(0.5f);
		}
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Connect(Hfh_GVars.IPAddress, Hfh_GVars.IPPort);
		while (!Hfh_Singleton<Hfh_NetManager>.GetInstance().isConnected)
		{
			yield return null;
		}
		while (!_ishasVersionChecked)
		{
			yield return null;
		}
		Hfh_Singleton<Hfh_NetManager>.GetInstance().SendPublicKey();
		while (!Hfh_Singleton<Hfh_NetManager>.GetInstance().isReady)
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.2f);
		UnityEngine.Debug.Log("正常登录：" + Hfh_GVars.username + "||" + Hfh_GVars.pwd);
		object[] args = new object[2]
		{
			Hfh_GVars.username,
			Hfh_GVars.pwd
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/userLogin", args);
	}

	private IEnumerator _loadingLoginTimeout()
	{
		yield return new WaitForSeconds(5f);
		if (!Hfh_Singleton<Hfh_NetManager>.GetInstance().isLogined)
		{
			UnityEngine.Debug.Log(Hfh_LogHelper.Magenta("_loadingLoginTimeout"));
			_timeoutQuit();
		}
	}
}
