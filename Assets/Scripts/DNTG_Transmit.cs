using GameCommon;
using GameConfig;
using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNTG_Transmit : MonoBehaviour
{
	private static DNTG_Transmit _MyTransmit;

	private DNTG_Sockets m_CreateSocket;

	private int fishGroupNum;

	private Coroutine waitNewFishGroup;

	private bool isLightningFish;

	private bool isHeaven;

	private bool isMonkeyKing;

	public static DNTG_Transmit GetSingleton()
	{
		return _MyTransmit;
	}

	private void Awake()
	{
		if (_MyTransmit == null)
		{
			_MyTransmit = this;
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.F1))
		{
			DNTG_Formation.GetSingleton().ShowFormation(DNTG_FORMATION.Formation_MonkeyByCar);
		}
		if (UnityEngine.Input.GetKey(KeyCode.F2) && Input.GetMouseButtonDown(2))
		{
			if (fishGroupNum > 9)
			{
				fishGroupNum = 0;
			}
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			UnityEngine.Debug.LogError("鼠标位置: " + vector);
			TestFishGroup(vector, fishGroupNum);
			fishGroupNum++;
		}
	}

	public void TransmitGetPoint(DNTG_Sockets MyCreateSocket)
	{
		m_CreateSocket = MyCreateSocket;
	}

	public void PostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] array = table["args"] as object[];
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			try
			{
				if (text != "newFish" && text != "gameScore" && text != "scrollMessage" && text != "roomInfo" && text != "userShutup" && text != "lockFish" && text != "fired" && text != "gameGold")
				{
					string empty = string.Empty;
					empty = JsonFx.Json.JsonWriter.Serialize(array);
					UnityEngine.Debug.LogError("收到: " + text + "  " + empty);
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.Log("打印报错：" + arg);
			}
		}
		if (text == "sendServerTime")
		{
			if (DNTG_GameInfo.getInstance().Key != string.Empty)
			{
				DNTG_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(DNTG_GameInfo.getInstance().UserId, DNTG_GameInfo.getInstance().Pwd, (int)((DNTG_GameInfo.getInstance().currentState < DNTG_GameState.On_SelectRoom) ? DNTG_GameState.On_SelectRoom : DNTG_GameInfo.getInstance().currentState), string.Empty);
			}
			DNTG_GameInfo.getInstance().Key = "sendServerTime";
		}
		else if (text == "checkVersion")
		{
			DoCheckVersion(array);
		}
		else if (text == "notUpdate")
		{
			DoNotUpdate(array);
		}
		else if (text == "checkLogin")
		{
			DoCheckLogin(array);
		}
		else if (text == "roomInfo")
		{
			DoRoomInfo(array);
		}
		else if (text == "selectHall")
		{
			DoSelectHallInfo(array);
		}
		else if (text == "updateRoomInfo")
		{
			DoUpdateRoomInfo(array);
		}
		else if (text == "fishBet")
		{
			DoFishBet(array);
		}
		else if (text == "checkSafeBoxPwd")
		{
			ZH2_GVars.getCheckSafeBoxPwd(array);
		}
		else if (text == "changeSafeBoxPwd")
		{
			ZH2_GVars.getChangeSafeBoxPwd(array);
		}
		else if (text == "deposit")
		{
			ZH2_GVars.getDeposit(array);
		}
		else if (text == "extract")
		{
			ZH2_GVars.getExtract(array);
		}
		else if (text == "getTransactionRecord")
		{
			ZH2_GVars.getTransactionRecord(array);
		}
		else if (text == "updateHallInfo")
		{
			DoUpdateHallInfo(array);
		}
		else if (text == "deskOnlineNumber")
		{
			DoDeskOnlineNumber(array);
		}
		else if (text == "requestSeat")
		{
			DoRequestSeat(array);
		}
		else if (text == "updateDeskInfo")
		{
			DoUpdateDeskInfo(array);
		}
		else if (text == "gameScore")
		{
			DoGameScore(table["args"] as int[]);
		}
		else if (text == "notFired")
		{
			DoNotFired(table["args"] as int[]);
		}
		else if (text == "fired")
		{
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange((object[])table["args"]);
			DoFired(arrayList);
		}
		else if (text == "gunHitFish")
		{
			ArrayList arrayList2 = new ArrayList();
			arrayList2.AddRange((object[])table["args"]);
			DoGunHitFish(arrayList2);
		}
		else if (text == "newFishGroup")
		{
			DoNewFishGroup(table["args"] as int[]);
		}
		else if (text == "newFish")
		{
			try
			{
				DoNewFish(array);
			}
			catch (Exception arg2)
			{
				UnityEngine.Debug.LogError("es: " + arg2);
			}
		}
		else if (text == "forbitFired")
		{
			DoForbitFired(array);
		}
		else if (text == "clearFish")
		{
			DoClearFish(array);
		}
		else if (text == "scrollMessage")
		{
			DoSendNotice(array);
		}
		else if (text == "playerInfo")
		{
			DoPlayerInfo(array);
		}
		else if (text == "sendChat")
		{
			DoSendChat(array);
		}
		else if (text == "gameShutup")
		{
			DoGameShutup(table["args"] as bool[]);
		}
		else if (text == "userShutup")
		{
			DoUserShutup(table["args"] as bool[]);
		}
		else if (text == "userAward")
		{
			DoUserAward(table["args"] as int[]);
		}
		else if (!(text == "heart"))
		{
			if (text == "gameGold")
			{
				DoGameGold(table["args"] as int[]);
			}
			else if (text == "expeGold")
			{
				DoExpeGold(table["args"] as int[]);
			}
			else if (text == "addExpeGoldAuto")
			{
				DoAddExpeGoldAuto(table["args"] as bool[]);
			}
			else if (text == "overflow")
			{
				DoOverflow(array);
			}
			else if (text == "quitToLogin")
			{
				DoQuitToLogin(table["args"] as int[]);
			}
			else if (text == "quitToRoom")
			{
				DoQuitToRoom(table["args"] as int[]);
			}
			else if (text == "quitToDesk")
			{
				DoQuitToDesk(array);
			}
			else if (text == "applyExchange")
			{
				HandleNetMsg_ApplyExchange(array);
			}
			else if (text == "applyPay")
			{
				HandleNetMsg_ApplyPay(array);
			}
			else if (text == "changeScore")
			{
				HandleNetMsg_changeScoreNotice(array);
			}
			else if (text == "NetThread/NetDown")
			{
				DoNetDown(array);
			}
			else if (text == "lockFish")
			{
				ArrayList arrayList3 = new ArrayList();
				arrayList3.AddRange((object[])table["args"]);
				DoLockFish(arrayList3);
			}
			else if (text == "unLockFish")
			{
				DoUnLockFish(table["args"] as int[]);
			}
			else if (text == "unLockScreen")
			{
				DoUnLockScreen(array);
			}
			else
			{
				Console.WriteLine("recv No Message Type!");
			}
		}
	}

	public void SelectPostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] args = table["args"] as object[];
		if (text == "sendServerTime")
		{
			if (DNTG_GameInfo.getInstance().Key != string.Empty)
			{
				DNTG_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(DNTG_GameInfo.getInstance().UserId, DNTG_GameInfo.getInstance().Pwd, (int)DNTG_GameInfo.getInstance().currentState, string.Empty);
			}
			DNTG_GameInfo.getInstance().Key = "sendServerTime";
		}
		else if (text == "checkVersion")
		{
			DoCheckVersion(args);
		}
		else if (text == "notUpdate")
		{
			DoNotUpdate(args);
		}
		else if (text == "checkLogin")
		{
			DoCheckLogin(args);
		}
		else if (text == "roomInfo")
		{
			MonoBehaviour.print(text);
			DoRoomInfo(args);
		}
		else if (text == "updateRoomInfo")
		{
			MonoBehaviour.print(text);
			DoUpdateRoomInfo(args);
		}
		else if (text == "deskOnlineNumber")
		{
			DoDeskOnlineNumber(args);
		}
		else if (text == "requestSeat")
		{
			DoRequestSeat(args);
		}
		else if (text == "updateDeskInfo")
		{
			DoUpdateDeskInfo(args);
		}
		else if (text == "gameScore")
		{
			DoGameScore(table["args"] as int[]);
		}
		else
		{
			if (text == "notFired" || text == "fired" || text == "gunHitFish" || text == "newFishGroup" || text == "newFish" || text == "forbitFired" || text == "clearFish")
			{
				return;
			}
			if (text == "scrollMessage")
			{
				UnityEngine.Debug.LogError("=====公告未完成======");
			}
			else if (text == "playerInfo")
			{
				DoPlayerInfo(args);
			}
			else if (text == "sendChat")
			{
				DoSendChat(args);
			}
			else if (text == "gameShutup")
			{
				DoGameShutup(table["args"] as bool[]);
			}
			else if (text == "userShutup")
			{
				DoUserShutup(table["args"] as bool[]);
			}
			else if (text == "userAward")
			{
				DoUserAward(table["args"] as int[]);
			}
			else
			{
				if (text == "heart")
				{
					return;
				}
				if (text == "gameGold")
				{
					DoGameGold(table["args"] as int[]);
				}
				else if (text == "expeGold")
				{
					DoExpeGold(table["args"] as int[]);
				}
				else if (text == "addExpeGoldAuto")
				{
					DoAddExpeGoldAuto(table["args"] as bool[]);
				}
				else if (text == "overflow")
				{
					DoOverflow(args);
				}
				else if (text == "quitToLogin")
				{
					DoQuitToLogin(table["args"] as int[]);
				}
				else if (text == "quitToRoom")
				{
					DoQuitToRoom(table["args"] as int[]);
				}
				else if (text == "NetThread/NetDown")
				{
					DoNetDown(args);
				}
				else if (text != "lockFish")
				{
					if (text != "unLockFish" && text == "unLockScreen")
					{
						DoUnLockScreen(args);
					}
				}
				else
				{
					Console.WriteLine("recv No Message Type!");
				}
			}
		}
	}

	private void DoNetDown(object[] args)
	{
		if (DNTG_Sockets.GetSingleton().isReconnect)
		{
			if (m_CreateSocket.GetRelineCount() < 5)
			{
				m_CreateSocket.CreateReceiveThread();
				return;
			}
			Console.WriteLine("30秒重连失败，网络断开，请重新登录网络大厅...");
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.Net_ConnectionError, 0, string.Empty);
		}
	}

	private void DoCheckLogin(object[] args)
	{
		Console.WriteLine("**********1*********DoCheckLogin**************************************");
		DNTG_User dNTG_User = new DNTG_User();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["isLogin"];
		Console.WriteLine("isLogin: " + flag);
		int num = (int)dictionary["messageStatus"];
		Console.WriteLine("messageStatus: " + num);
		if (flag)
		{
			ZH2_GVars.hallInfo = new Dictionary<string, object>();
			ZH2_GVars.hallInfo = (dictionary["hallInfo"] as Dictionary<string, object>);
			Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
			dNTG_User.id = (int)dictionary2["id"];
			dNTG_User.username = (string)dictionary2["username"];
			dNTG_User.nickname = (string)dictionary2["nickname"];
			dNTG_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
			dNTG_User.level = (int)dictionary2["level"];
			dNTG_User.gameGold = (int)dictionary2["gameGold"];
			dNTG_User.expeGold = (int)dictionary2["expeGold"];
			dNTG_User.photoId = (int)dictionary2["photoId"];
			dNTG_User.overflow = (int)dictionary2["overflow"];
			dNTG_User.gameScore = (int)dictionary2["gameScore"];
			dNTG_User.expeScore = (int)dictionary2["expeScore"];
			dNTG_User.type = (int)dictionary2["type"];
			ZH2_GVars.userId = dNTG_User.id;
			if (dNTG_User.overflow == 1)
			{
				DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.CoinOverFlow, 0, string.Empty);
				return;
			}
			DNTG_GameInfo.getInstance().IsSpecial = false;
			DNTG_GameInfo.getInstance().createUser(dNTG_User);
			DNTG_GameInfo.getInstance().IsGameShuUp = false;
		}
		else
		{
			switch (num)
			{
			case 0:
				DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.UserIdDeleted, 0, string.Empty);
				break;
			case 1:
				DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.Game_UserIdFrozen, 0, string.Empty);
				break;
			default:
				DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.ServerUpdate, 0, string.Empty);
				break;
			}
		}
	}

	private void DoCheckVersion(object[] args)
	{
		Console.WriteLine("**********1*********DoCheckVersion**************************************");
		string text = (string)args[0];
		Console.WriteLine("haveNewVersionIDFlag: " + text);
		DNTG_IOSGameStart.UpdateGameVesion(text);
		Console.WriteLine("**********1*********DoCheckVersion**************************************");
	}

	private void DoNotUpdate(object[] args)
	{
		DNTG_NetMngr.GetSingleton().MyCreateSocket.SendPublicKey();
	}

	private void DoRoomInfo(object[] args)
	{
	}

	private void DoSelectHallInfo(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		UpdateOnline(dictionary);
		SetFiedInfo(dictionary);
		int length = (dictionary["deskInfo"] as Array).Length;
		if (length > 0)
		{
			DNTG_FishDesk[] array = new DNTG_FishDesk[length];
			int[] array2 = new int[length];
			for (int i = 0; i < length; i++)
			{
				array2[i] = 0;
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["deskInfo"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new DNTG_FishDesk();
				array[i].id = (int)dictionary2["id"];
				array[i].roomId = (int)dictionary2["roomId"];
				array[i].name = (string)dictionary2["name"];
				array[i].minGold = (int)dictionary2["minGold"];
				array[i].minGunValue = (int)dictionary2["minGunValue"];
				array[i].maxGunValue = (int)dictionary2["maxGunValue"];
				array[i].addstepGunValue = (int)dictionary2["addstepGunValue"];
				array[i].exchange = (int)dictionary2["exchange"];
				array[i].onceExchangeValue = (int)dictionary2["onceExchangeValue"];
				for (int j = 0; j < 4; j++)
				{
					Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
					dictionary3 = ((dictionary2["seats"] as Array).GetValue(j) as Dictionary<string, object>);
					array[i].seats[j] = new DNTG_Seat();
					array[i].seats[j].seatId = (int)dictionary3["id"];
					array[i].seats[j].isFree = (bool)dictionary3["isFree"];
					array[i].seats[j].gunValue = (int)dictionary3["gunValue"];
					if (!array[i].seats[j].isFree)
					{
						array2[i]++;
						Dictionary<string, object> dictionary4 = dictionary3["user"] as Dictionary<string, object>;
						array[i].seats[j].user.id = (int)dictionary4["id"];
						array[i].seats[j].user.username = (string)dictionary4["username"];
						array[i].seats[j].user.nickname = (string)dictionary4["nickname"];
						array[i].seats[j].user.sex = ((string)dictionary4["sex"]).ToCharArray()[0];
						array[i].seats[j].user.level = (int)dictionary4["level"];
						array[i].seats[j].user.gameGold = (int)dictionary4["gameGold"];
						array[i].seats[j].user.expeGold = (int)dictionary4["expeGold"];
						array[i].seats[j].user.photoId = (int)dictionary4["photoId"];
						array[i].seats[j].user.overflow = (int)dictionary4["overflow"];
						array[i].seats[j].user.gameScore = (int)dictionary4["gameScore"];
						array[i].seats[j].user.expeScore = (int)dictionary4["expeScore"];
						array[i].seats[j].user.type = (int)dictionary4["type"];
					}
				}
			}
			array = Test(array);
			DNTG_GameInfo.getInstance().updateTableList(array);
			DNTG_GameInfo.getInstance().SetNoHall(isHavHall: true);
			for (int k = 0; k < length; k++)
			{
				DNTG_GameInfo.getInstance().updateTableUserNumber(array[k].id, array2[k]);
			}
		}
		else
		{
			DNTG_GameInfo.getInstance().SetNoHall(isHavHall: false);
			DNTG_GameInfo.getInstance().updateTableList(null);
		}
	}

	private DNTG_FishDesk[] Test(DNTG_FishDesk[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = 0; j < array.Length - 1; j++)
			{
				if (array[j].id > array[j + 1].id)
				{
					DNTG_FishDesk dNTG_FishDesk = array[j];
					array[j] = array[j + 1];
					array[j + 1] = dNTG_FishDesk;
				}
			}
		}
		return array;
	}

	private void UpdateOnline(Dictionary<string, object> dictionary)
	{
		if (dictionary.ContainsKey("onlineNumber"))
		{
			Dictionary<string, object> dictionary2 = dictionary["onlineNumber"] as Dictionary<string, object>;
			if (DNTG_GameInfo.getInstance() != null && DNTG_GameInfo.getInstance().UIScene != null)
			{
				for (int i = 0; i < dictionary2.Count; i++)
				{
					for (int j = 0; j < DNTG_GameInfo.getInstance().UIScene.selectBtnList.Count; j++)
					{
						if (i == DNTG_GameInfo.getInstance().UIScene.selectBtnList[j].hallId)
						{
							DNTG_GameInfo.getInstance().UIScene.selectBtnList[j].onlinePeople = dictionary2[i.ToString()].ToString();
							DNTG_GameInfo.getInstance().UIScene.selectBtnList[j].UpdateText();
							break;
						}
					}
				}
			}
			else
			{
				UnityEngine.Debug.Log("更新在线人数失败");
			}
		}
		else
		{
			UnityEngine.Debug.Log("更新在线人数失败 不存在减值");
		}
	}

	private void SetFiedInfo(Dictionary<string, object> dictionary)
	{
		ZH2_GVars.firingInterval = float.Parse(dictionary["fireInterval"].ToString());
		ZH2_GVars.shellMultiple = float.Parse(dictionary["flightSpeed"].ToString());
	}

	private void DoUpdateRoomInfo(object[] args)
	{
	}

	private void DoUpdateHallInfo(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		UpdateOnline(dictionary);
		int length = (dictionary["deskInfo"] as Array).Length;
		if (length > 0)
		{
			DNTG_FishDesk[] array = new DNTG_FishDesk[length];
			int[] array2 = new int[length];
			for (int i = 0; i < length; i++)
			{
				array2[i] = 0;
				int num = 0;
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["deskInfo"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new DNTG_FishDesk();
				array[i].id = (int)dictionary2["id"];
				array[i].roomId = (int)dictionary2["roomId"];
				array[i].name = (string)dictionary2["name"];
				array[i].minGold = (int)dictionary2["minGold"];
				array[i].minGunValue = (int)dictionary2["minGunValue"];
				array[i].maxGunValue = (int)dictionary2["maxGunValue"];
				array[i].addstepGunValue = (int)dictionary2["addstepGunValue"];
				array[i].exchange = (int)dictionary2["exchange"];
				array[i].onceExchangeValue = (int)dictionary2["onceExchangeValue"];
				for (int j = 0; j < 4; j++)
				{
					Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
					dictionary3 = ((dictionary2["seats"] as Array).GetValue(j) as Dictionary<string, object>);
					array[i].seats[j] = new DNTG_Seat();
					array[i].seats[j].seatId = (int)dictionary3["id"];
					array[i].seats[j].isFree = (bool)dictionary3["isFree"];
					array[i].seats[j].gunValue = (int)dictionary3["gunValue"];
					if (!array[i].seats[j].isFree)
					{
						array2[i]++;
						num++;
						Dictionary<string, object> dictionary4 = dictionary3["user"] as Dictionary<string, object>;
						array[i].seats[j].user.id = (int)dictionary4["id"];
						array[i].seats[j].user.username = (string)dictionary4["username"];
						array[i].seats[j].user.nickname = (string)dictionary4["nickname"];
						array[i].seats[j].user.sex = ((string)dictionary4["sex"]).ToCharArray()[0];
						array[i].seats[j].user.level = (int)dictionary4["level"];
						array[i].seats[j].user.gameGold = (int)dictionary4["gameGold"];
						array[i].seats[j].user.expeGold = (int)dictionary4["expeGold"];
						array[i].seats[j].user.photoId = (int)dictionary4["photoId"];
						array[i].seats[j].user.overflow = (int)dictionary4["overflow"];
						array[i].seats[j].user.gameScore = (int)dictionary4["gameScore"];
						array[i].seats[j].user.expeScore = (int)dictionary4["expeScore"];
						array[i].seats[j].user.type = (int)dictionary4["type"];
					}
					DNTG_GameInfo.getInstance().updateTableUserNumber(j, num);
				}
			}
			array = Test(array);
			DNTG_GameInfo.getInstance().updateTableList(array);
			DNTG_GameInfo.getInstance().SetNoHall(isHavHall: true);
			for (int k = 0; k < length; k++)
			{
				DNTG_GameInfo.getInstance().updateTableUserNumber(array[k].id, array2[k]);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("没有房间");
			DNTG_GameInfo.getInstance().SetNoHall(isHavHall: false);
			DNTG_GameInfo.getInstance().updateTableList(null);
		}
	}

	private void DoDeskOnlineNumber(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int tableId = (int)dictionary["deskId"];
		int number = (int)dictionary["onlineNumber"];
		DNTG_GameInfo.getInstance().updateTableUserNumber(tableId, number);
	}

	private void DoRequestSeat(object[] args)
	{
		int num = 0;
		int num2 = 0;
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["bCanSeat"];
		if (!flag)
		{
			num = (int)dictionary["messageStatus"];
		}
		else
		{
			num2 = (int)dictionary["bgId"];
			int seatIndex = (int)dictionary["seatId"];
			DNTG_GameInfo.getInstance().User.SeatIndex = seatIndex;
		}
		if (!DNTG_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			DNTG_NetMngr.GetSingleton().mSceneBg = num2;
		}
		else
		{
			DNTG_SceneBgMngr.GetSingleton().SetScene(num2);
		}
		if (flag)
		{
			DNTG_GameInfo.getInstance().UIScene.EnterGame();
			return;
		}
		switch (num)
		{
		case 0:
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.TableDeleted, 0, string.Empty);
			break;
		case 1:
			if (DNTG_GameInfo.getInstance().User.RoomId == 1)
			{
				DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.SelectTable_CreditBelowRistrict, 0, string.Empty);
			}
			else
			{
				DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.SelectTable_SendExpCoin, 0, string.Empty);
			}
			break;
		case 2:
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.SelectSeat_NotEmpty, 0, string.Empty);
			break;
		}
	}

	private void DoUpdateDeskInfo(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["deskId"];
		int length = (dictionary["Seat"] as Array).Length;
		if (length <= 0)
		{
			return;
		}
		DNTG_Seat[] array = new DNTG_Seat[length];
		int num2 = 0;
		for (int i = 0; i < length; i++)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2 = ((dictionary["Seat"] as Array).GetValue(i) as Dictionary<string, object>);
			array[i] = new DNTG_Seat();
			array[i].seatId = (int)dictionary2["id"];
			array[i].isFree = (bool)dictionary2["isFree"];
			array[i].gunValue = (int)dictionary2["gunValue"];
			if (!array[i].isFree)
			{
				num2++;
				Dictionary<string, object> dictionary3 = dictionary2["user"] as Dictionary<string, object>;
				array[i].user = new DNTG_User();
				array[i].user.id = (int)dictionary3["id"];
				array[i].user.username = (string)dictionary3["username"];
				array[i].user.nickname = (string)dictionary3["nickname"];
				array[i].user.sex = ((string)dictionary3["sex"]).ToCharArray()[0];
				array[i].user.level = (int)dictionary3["level"];
				array[i].user.gameGold = (int)dictionary3["gameGold"];
				array[i].user.expeGold = (int)dictionary3["expeGold"];
				array[i].user.photoId = (int)dictionary3["photoId"];
				array[i].user.overflow = (int)dictionary3["overflow"];
				array[i].user.gameScore = (int)dictionary3["gameScore"];
				array[i].user.expeScore = (int)dictionary3["expeScore"];
				array[i].user.type = (int)dictionary3["type"];
			}
		}
		DNTG_GameInfo.getInstance().updateOtherUsers(array, num);
		DNTG_GameInfo.getInstance().updateTableUserNumber(num, num2);
	}

	private void DoGameScore(int[] args)
	{
		int num = args[0];
		DNTG_GameInfo.getInstance().updateUser("gameScore", num);
	}

	private void DoNotFired(int[] args)
	{
		UnityEngine.Debug.LogError("===停止开火===" + JsonMapper.ToJson(args));
	}

	private void DoFired(ArrayList args)
	{
		double num = Convert.ToDouble(args[1]);
		int num2 = int.Parse((string)args[2]);
		int num3 = int.Parse((string)args[3]);
		bool isLizi = false;
		int newScore = int.Parse((string)args[5]);
		bool isSpeed = Convert.ToBoolean(args[6]);
		if (DNTG_NetMngr.GetSingleton().IsGameSceneLoadOk && !DNTG_GameParameter.G_bTest && num2 != DNTG_GameMngr.GetSingleton().mPlayerSeatID)
		{
			DNTG_BulletPoolMngr.GetSingleton().LanchBullet2(num2, (float)num, num3, isLizi, isSpeed);
		}
		if (DNTG_GameInfo.getInstance().currentState == DNTG_GameState.On_Game)
		{
			DNTG_GameInfo.getInstance().GameScene.UpdateWhenFired(num2, newScore, num3, (float)num, isSpeed);
		}
	}

	private void DoGunHitFish(ArrayList args)
	{
		int num = int.Parse((string)args[0]);
		int num2 = int.Parse((string)args[1]);
		int num3 = int.Parse((string)args[2]);
		bool flag = Convert.ToBoolean(args[3]);
		bool flag2 = Convert.ToBoolean(args[4]);
		int gunScore = int.Parse((string)args[5]);
		int newScore = int.Parse((string)args[6]);
		if (DNTG_GameInfo.getInstance().currentState == DNTG_GameState.On_Game)
		{
			DNTG_GameInfo.getInstance().GameScene.UpdateWhenDead(num3, newScore, gunScore, num2);
		}
		if (flag && num3 == DNTG_GameMngr.GetSingleton().mPlayerSeatID && DNTG_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			DNTG_BulletPoolMngr.GetSingleton().SetLizi();
		}
		string text = (string)args[7];
		string[] array = text.Split('|');
		DNTG_HitFish[] array2 = new DNTG_HitFish[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string[] array3 = array[i].Split('#');
			array2[i] = new DNTG_HitFish();
			array2[i].fishid = int.Parse(array3[0]);
			array2[i].fishtype = int.Parse(array3[1]);
			array2[i].fx = Convert.ToDouble(array3[2]);
			array2[i].fy = Convert.ToDouble(array3[3]);
			array2[i].bet = int.Parse(array3[4]);
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (!DNTG_GameParameter.G_bTest && DNTG_NetMngr.GetSingleton().IsGameSceneLoadOk && DNTG_FishPoolMngr.GetSingleton() != null && DNTG_EffectMngr.GetSingleton() != null)
			{
				Vector3 pos = Vector3.right * (float)array2[j].fx + Vector3.up * (float)array2[j].fy;
				DNTG_FishPoolMngr.GetSingleton().SetFishDie(array2[j].fishid, num2, array2[j].fishtype, array2[j].bet, num3, pos);
				if (flag && j == 0)
				{
					UnityEngine.Debug.LogError("====显示粒子卡片====");
				}
			}
		}
		if (!flag2)
		{
			return;
		}
		string text2 = (string)args[8];
		if (!(text2 != string.Empty) || text2.Length < 1)
		{
			return;
		}
		string[] array4 = text2.Split('|');
		DNTG_HitFish[] array5 = new DNTG_HitFish[array4.Length];
		for (int k = 0; k < array4.Length; k++)
		{
			string[] array6 = array4[k].Split('#');
			array5[k] = new DNTG_HitFish();
			array5[k].fishid = int.Parse(array6[0]);
			array5[k].fishtype = int.Parse(array6[1]);
			array5[k].fx = Convert.ToDouble(array6[2]);
			array5[k].fy = Convert.ToDouble(array6[3]);
			array5[k].bet = int.Parse(array6[4]);
		}
		for (int l = 0; l < array4.Length; l++)
		{
			if (!DNTG_GameParameter.G_bTest && DNTG_FishPoolMngr.GetSingleton() != null && DNTG_EffectMngr.GetSingleton() != null)
			{
				Vector3 pos2 = Vector3.right * (float)array5[l].fx + Vector3.up * (float)array5[l].fy;
				DNTG_FishPoolMngr.GetSingleton().SetFishDie(array5[l].fishid, num2, array5[l].fishtype, array5[l].bet, num3, pos2);
			}
		}
	}

	private void DoFishBet(object[] args)
	{
		if (ZH2_GVars.setMonkeyKingBet != null)
		{
			ZH2_GVars.setMonkeyKingBet(args);
		}
	}

	private void DoNewFishGroup(int[] args)
	{
		int num = args[0];
		if (num == 2)
		{
			num = 10;
		}
		UnityEngine.Debug.Log("出鱼阵: " + num);
		if (waitNewFishGroup != null)
		{
			StopCoroutine(waitNewFishGroup);
		}
		waitNewFishGroup = StartCoroutine(WaitNewFishGroup(num));
	}

	private IEnumerator WaitNewFishGroup(int num)
	{
		while (DNTG_SceneBgMngr.GetSingleton() == null)
		{
			UnityEngine.Debug.LogError("====等待SceneBgMngr=====");
			yield return null;
		}
		yield return new WaitForSeconds(0.2f);
		DNTG_SceneBgMngr.GetSingleton().BigFishFormatReset();
		DNTG_Formation.GetSingleton().ShowFormation((DNTG_FORMATION)num);
		if (DNTG_GameInfo.getInstance().currentState == DNTG_GameState.On_Game)
		{
			DNTG_GameInfo.getInstance().GameScene.HideTip();
		}
	}

	public void TestFishGroup(Vector2 vector2, int index = 0)
	{
		index = 9;
		int num = UnityEngine.Random.Range(0, 5);
		int num2 = num + 20;
		int num3 = UnityEngine.Random.Range(0, 30);
		UnityEngine.Debug.LogError("鱼王: " + (DNTG_FISH_TYPE)num2 + " ====位置: " + num3);
		DNTG_Formation.GetSingleton().ShowFormation((DNTG_FORMATION)index, vector2, (DNTG_FISH_TYPE)num, (DNTG_FISH_TYPE)num2, num3);
	}

	private void DoNewFish(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args));
		int @int = GetInt(jsonData[0]);
		int int2 = GetInt(jsonData[1]);
		int int3 = GetInt(jsonData[2]);
		int newFishPathType = -1;
		int[] array = null;
		DNTG_FISH_TYPE[] array2 = null;
		isLightningFish = (int2 == 39);
		isHeaven = (int2 == 40);
		isMonkeyKing = (int2 == 19);
		int fishKingPos = -1;
		int fishType = -1;
		int monkeyKingBet = 40;
		if (!isLightningFish && !isHeaven)
		{
			newFishPathType = GetInt(jsonData[3]);
			if (isMonkeyKing)
			{
				monkeyKingBet = GetInt(jsonData[4][0]);
			}
		}
		else if (isLightningFish)
		{
			JsonData jsonData2 = jsonData[3];
			array = new int[jsonData2.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = GetInt(jsonData2[i]);
			}
			JsonData jsonData3 = jsonData[4];
			array2 = new DNTG_FISH_TYPE[jsonData3.Count];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = (DNTG_FISH_TYPE)GetInt(jsonData3[j]);
			}
		}
		else
		{
			newFishPathType = GetInt(jsonData[3][0]);
			fishKingPos = GetInt(jsonData[3][1]);
			fishType = GetInt(jsonData[4][0]);
		}
		int num = int3;
		if (num <= 0)
		{
			return;
		}
		DNTG_FishPathType[] array3 = new DNTG_FishPathType[num];
		if (isLightningFish)
		{
			for (int k = 0; k < num; k++)
			{
				array3[k] = new DNTG_FishPathType
				{
					fishId = @int + k,
					newFishPathType = array[k]
				};
			}
		}
		else
		{
			for (int l = 0; l < num; l++)
			{
				array3[l] = new DNTG_FishPathType
				{
					fishId = @int + l,
					newFishPathType = newFishPathType
				};
			}
		}
		if (int2 != 48 && DNTG_NetMngr.GetSingleton().IsGameSceneLoadOk && DNTG_FishPoolMngr.GetSingleton() != null && !DNTG_GameParameter.G_bTest)
		{
			if (isLightningFish)
			{
				for (int m = 0; m < num; m++)
				{
					DNTG_FishPoolMngr.GetSingleton().CreateLightningFishFish(array2[m], array3[m].newFishPathType, array3[m].fishId, SpecialFishType.LightningFish);
				}
			}
			else if (isHeaven)
			{
				DNTG_FishPoolMngr.GetSingleton().CreateFish((DNTG_FISH_TYPE)fishType, array3[0].newFishPathType, array3[0].fishId, int3, SpecialFishType.HeavenFish, fishKingPos, -1);
			}
			else if (isMonkeyKing)
			{
				DNTG_FishPoolMngr.GetSingleton().CreateFish((DNTG_FISH_TYPE)int2, array3[0].newFishPathType, array3[0].fishId, int3, SpecialFishType.MonkeyKing, -1, monkeyKingBet);
			}
			else
			{
				DNTG_FishPoolMngr.GetSingleton().CreateFish((DNTG_FISH_TYPE)int2, array3[0].newFishPathType, array3[0].fishId, int3, SpecialFishType.CommonFish, -1, -1);
			}
			if (DNTG_GameInfo.getInstance().CountTime)
			{
				UnityEngine.Debug.LogError("=========冻结状态出新鱼,解开冻结========");
				DNTG_FishPoolMngr.GetSingleton().UnFixAllFish();
			}
		}
		if (DNTG_GameInfo.getInstance().currentState == DNTG_GameState.On_Game)
		{
			DNTG_GameInfo.getInstance().GameScene.HideTip();
		}
	}

	private int GetInt(JsonData data)
	{
		return int.Parse(data.ToString());
	}

	private void DoClearFish(object[] args)
	{
		if (DNTG_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			DNTG_SceneBgMngr.GetSingleton().ChangeScene();
		}
	}

	private void DoForbitFired(object[] args)
	{
		if (DNTG_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			DNTG_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = false;
		}
	}

	private void DoSendNotice(object[] args)
	{
		if (args != null)
		{
			if (args.Length >= 1)
			{
				JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
				UnityEngine.Debug.LogError("通知: " + jsonData.ToJson());
				if (All_NoticePanel.GetInstance() != null)
				{
					All_NoticePanel.GetInstance().AddTip(jsonData);
				}
				else
				{
					UnityEngine.Debug.LogError("=====All_NoticePanel为空=====");
				}
			}
			else
			{
				UnityEngine.Debug.LogError("===公告长度小于1===");
				JsonData jsonData2 = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args));
				UnityEngine.Debug.LogError("通知2: " + jsonData2.ToJson());
			}
		}
		else
		{
			UnityEngine.Debug.LogError("====公告为空=====");
		}
	}

	private void DoScrollMessage(string[] args)
	{
		Console.WriteLine("**********1*********DoScrollMessage**************************************");
		string text = args[0];
		Console.WriteLine("message: " + text);
		Console.WriteLine("**********2*********DoScrollMessage**************************************");
		if (DNTG_GameInfo.getInstance().currentState == DNTG_GameState.On_Game)
		{
			DNTG_GameInfo.getInstance().GameScene.AddNotice(text);
		}
	}

	private void DoPlayerInfo(object[] args)
	{
		Console.WriteLine("**********1*********DoPlayerInfo**************************************");
		DNTG_User dNTG_User = new DNTG_User();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["seatId"];
		int num2 = (int)dictionary["honor"];
		Console.WriteLine("seatid: " + num);
		Console.WriteLine("honor: " + num2);
		Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
		dNTG_User.id = (int)dictionary2["id"];
		dNTG_User.username = (string)dictionary2["username"];
		dNTG_User.nickname = (string)dictionary2["nickname"];
		dNTG_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
		dNTG_User.level = (int)dictionary2["level"];
		dNTG_User.gameGold = (int)dictionary2["gameGold"];
		dNTG_User.expeGold = (int)dictionary2["expeGold"];
		dNTG_User.photoId = (int)dictionary2["photoId"];
		dNTG_User.overflow = (int)dictionary2["overflow"];
		dNTG_User.gameScore = (int)dictionary2["gameScore"];
		dNTG_User.expeScore = (int)dictionary2["expeScore"];
		dNTG_User.type = (int)dictionary2["type"];
		DNTG_GameInfo.getInstance().getPersonInfo(new DNTG_UserInfo(dNTG_User), num2);
	}

	private void DoSendChat(object[] args)
	{
		Console.WriteLine("**********1*********DoSendChat**************************************");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["chatType"];
		Console.WriteLine("chatType: " + num);
		int num2 = (int)dictionary["senderSeatId"];
		Console.WriteLine("senderSeatId: " + num2);
		string str = (string)dictionary["chatMessage"];
		Console.WriteLine("chatMessage: " + str);
		Console.WriteLine("**********2*********DoSendChat**************************************");
		if (DNTG_GameInfo.getInstance().currentState != DNTG_GameState.On_Game)
		{
		}
	}

	private void DoGameShutup(bool[] args)
	{
		DNTG_GameInfo.getInstance().IsGameShuUp = true;
	}

	private void DoUserShutup(bool[] args)
	{
		DNTG_GameInfo.getInstance().IsUserShutUp = true;
	}

	private void DoUserAward(int[] args)
	{
		int num = args[0];
		DNTG_GameInfo.getInstance().updateUser("gameCoin", num);
		DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.GivingCoin, num, string.Empty);
	}

	private void DoGameGold(int[] args)
	{
		int num = args[0];
		DNTG_GameInfo.getInstance().updateUser("gameCoin", num);
	}

	private void DoExpeGold(int[] args)
	{
		int num = args[0];
		DNTG_GameInfo.getInstance().updateUser("testCoin", num);
	}

	private void DoAddExpeGoldAuto(bool[] args)
	{
		bool flag = args[0];
		DNTG_SoundManage.getInstance().playButtonMusic(DNTG_ButtonMusicType.getTestCoin);
		DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.ApplyForExpCoin_Success, 0, string.Empty);
	}

	private void DoOverflow(object[] args)
	{
		DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.CoinOverFlow, 0, string.Empty);
	}

	private void DoQuitToLogin(int[] args)
	{
		switch (args[0])
		{
		case 1:
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.ServerUpdate, 0, string.Empty);
			break;
		case 2:
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.Game_UserIdFrozen, 0, string.Empty);
			break;
		case 3:
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.UserIdDeleted, 0, string.Empty);
			break;
		case 4:
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.UserIdRepeative, 0, string.Empty);
			DNTG_Sockets.GetSingleton().isReconnect = false;
			break;
		case 5:
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.UserPwdChanged, 0, string.Empty);
			break;
		case 6:
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.LoseTheServer, 0, string.Empty);
			break;
		}
	}

	private void DoQuitToRoom(int[] args)
	{
		switch (args[0])
		{
		case 1:
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.TableDeleted, 0, string.Empty);
			break;
		case 2:
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.TableConfigChanged, 0, string.Empty);
			break;
		default:
			DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.LongTimeNoHandle, 0, string.Empty);
			break;
		}
	}

	private void DoQuitToDesk(object[] args)
	{
		DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.QuitToDesk, 0, string.Empty);
	}

	private void HandleNetMsg_ApplyExchange(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		DNTG_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (DNTG_GameInfo.getInstance() != null && DNTG_GameInfo.getInstance().UIScene != null)
		{
			DNTG_GameInfo.getInstance().UIScene.UpdateUserInfo();
		}
		string ch = (!flag) ? "兑换被拒绝" : "兑换成功";
		string en = (!flag) ? "Failure of exchange" : "Successful exchange";
		DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.Custom, 0, ZH2_GVars.ShowTip(ch, en, string.Empty));
	}

	private void HandleNetMsg_ApplyPay(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		DNTG_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (DNTG_GameInfo.getInstance() != null && DNTG_GameInfo.getInstance().UIScene != null)
		{
			DNTG_GameInfo.getInstance().UIScene.UpdateUserInfo();
		}
		string ch = (!flag) ? "充值被拒绝" : "充值成功";
		string en = (!flag) ? "Failure of TopUp" : "Successful TopUp";
		DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.Custom, 0, ZH2_GVars.ShowTip(ch, en, string.Empty));
	}

	private void HandleNetMsg_changeScoreNotice(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		DNTG_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (DNTG_GameInfo.getInstance() != null && DNTG_GameInfo.getInstance().UIScene != null)
		{
			DNTG_GameInfo.getInstance().UIScene.UpdateUserInfo();
		}
		int num = (int)jsonData["amount"];
		int num2 = (int)jsonData["status"];
		object obj;
		switch (num2)
		{
		case 0:
			obj = "充值";
			break;
		case 1:
			obj = "兑换";
			break;
		default:
			obj = "赠送";
			break;
		}
		string arg = (string)obj;
		object obj2;
		switch (num2)
		{
		case 0:
			obj2 = "Successful";
			break;
		case 1:
			obj2 = "Successful";
			break;
		default:
			obj2 = "Gift";
			break;
		}
		string arg2 = (string)obj2;
		DNTG_TipManager.getInstance().ShowTip(DNTG_TipType.Custom, 0, ZH2_GVars.ShowTip($"{arg}成功! 金币{num}", $"Successful {arg2}! GoldCoin {num}", string.Empty));
	}

	private void DoLockFish(ArrayList args)
	{
		bool locking = (bool)args[0];
		int fishid = (int)args[1];
		int seatid = (int)args[2];
		if (DNTG_FishPoolMngr.GetSingleton() != null)
		{
			DNTG_FishPoolMngr.GetSingleton().LockFish(fishid, seatid, locking);
		}
	}

	private void DoUnLockFish(int[] args)
	{
		int fishid = args[0];
		int seatid = args[1];
		DNTG_FishPoolMngr.GetSingleton().UnLockFish(fishid, seatid);
	}

	private void DoUnLockScreen(object[] args)
	{
		DNTG_FishPoolMngr.GetSingleton().UnFixAllFish();
	}
}
