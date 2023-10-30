using GameCommon;
using GameConfig;
using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DK_Transmit : MonoBehaviour
{
	private static DK_Transmit _MyTransmit;

	private DK_Sockets m_CreateSocket;

	public static DK_Transmit GetSingleton()
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

	public void TransmitGetPoint(DK_Sockets MyCreateSocket)
	{
		m_CreateSocket = MyCreateSocket;
	}

	private void OnEnable()
	{
		if (DK_GameInfo.getInstance() != null)
		{
			DK_GameInfo.getInstance().ClearGameInfo();
			UnityEngine.Debug.LogError("清除了键值");
		}
		else
		{
			UnityEngine.Debug.LogError("===mGameInfo为空===");
		}
	}

	private void OnApplicationQuit()
	{
		if (DK_GameInfo.getInstance() != null)
		{
			DK_GameInfo.getInstance().ClearGameInfo();
			UnityEngine.Debug.LogError("清除了键值");
			DK_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		}
		else
		{
			UnityEngine.Debug.LogError("===mGameInfo为空===");
		}
	}

	public void PostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] array = table["args"] as object[];
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			try
			{
				if (text != "newFish" && text != "gameScore" && text != "scrollMessage" && text != "roomInfo" && text != "userShutup" && text != "fired" && text != "lockFish")
				{
					string empty = string.Empty;
					empty = JsonFx.Json.JsonWriter.Serialize(array);
					UnityEngine.Debug.Log("收到: " + text + "  " + empty);
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.Log("打印报错：" + arg);
			}
		}
		if (text == "sendServerTime")
		{
			if (DK_GameInfo.getInstance().Key != string.Empty)
			{
				DK_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(DK_GameInfo.getInstance().UserId, DK_GameInfo.getInstance().Pwd, (int)((DK_GameInfo.getInstance().currentState < DK_GameState.On_SelectRoom) ? DK_GameState.On_SelectRoom : DK_GameInfo.getInstance().currentState), string.Empty);
			}
			DK_GameInfo.getInstance().Key = "sendServerTime";
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
		else if (text == "pay")
		{
			ZH2_GVars.getGamePay(array);
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
		else if (text == "killScore")
		{
			DoRestBet(table["args"] as int[]);
		}
		else if (text == "newFishGroup")
		{
			DoNewFishGroup(table["args"] as int[]);
		}
		else if (text == "newFish")
		{
			DoNewFish(table["args"] as string[]);
		}
		else if (text == "forbitFired")
		{
			DoForbitFired(array);
		}
		else if (text == "clearFish")
		{
			DoClearFish(table["args"] as int[], isBackGround: false);
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
		object[] array = table["args"] as object[];
		UnityEngine.Debug.Log("接收：" + text + "|||" + JsonFx.Json.JsonWriter.Serialize(array));
		if (text == "sendServerTime")
		{
			if (DK_GameInfo.getInstance().Key != string.Empty)
			{
				DK_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(DK_GameInfo.getInstance().UserId, DK_GameInfo.getInstance().Pwd, (int)DK_GameInfo.getInstance().currentState, string.Empty);
			}
			DK_GameInfo.getInstance().Key = "sendServerTime";
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
			MonoBehaviour.print(text);
			DoRoomInfo(array);
		}
		else if (text == "updateRoomInfo")
		{
			MonoBehaviour.print(text);
			DoUpdateRoomInfo(array);
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
		else
		{
			if (text == "notFired" || text == "fired" || text == "gunHitFish" || text == "newFishGroup" || text == "newFish" || text == "forbitFired")
			{
				return;
			}
			if (text == "clearFish")
			{
				DoClearFish(table["args"] as int[], isBackGround: true);
			}
			else if (text == "scrollMessage")
			{
				DoScrollMessage(table["args"] as string[]);
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
				else if (text == "NetThread/NetDown")
				{
					DoNetDown(array);
				}
				else if (text != "lockFish")
				{
					if (text != "unLockFish" && text == "unLockScreen")
					{
						DoUnLockScreen(array);
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
		if (DK_Sockets.GetSingleton().isReconnect)
		{
			if (m_CreateSocket.GetRelineCount() < 3)
			{
				m_CreateSocket.CreateReceiveThread();
				return;
			}
			Console.WriteLine("30秒重连失败，网络断开，请重新登录网络大厅...");
			DK_TipManager.getInstance().ShowTip(DK_TipType.Net_ConnectionError, 0, string.Empty);
		}
	}

	private void DoCheckLogin(object[] args)
	{
		Console.WriteLine("**********1*********DoCheckLogin**************************************");
		DK_User dK_User = new DK_User();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["isLogin"];
		Console.WriteLine("isLogin: " + flag);
		int num = (int)dictionary["messageStatus"];
		Console.WriteLine("messageStatus: " + num);
		if (flag)
		{
			bool flag2 = (bool)dictionary["isShutup"];
			bool flag3 = (bool)dictionary["special"];
			Console.WriteLine("isShutup: " + flag2);
			Console.WriteLine("special: " + flag3);
			ZH2_GVars.hallInfo = new Dictionary<string, object>();
			ZH2_GVars.hallInfo = (dictionary["hallInfo"] as Dictionary<string, object>);
			Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
			dK_User.id = (int)dictionary2["id"];
			ZH2_GVars.userId = dK_User.id;
			dK_User.username = (string)dictionary2["username"];
			dK_User.nickname = (string)dictionary2["nickname"];
			dK_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
			dK_User.level = (int)dictionary2["level"];
			dK_User.gameGold = (int)dictionary2["gameGold"];
			dK_User.expeGold = (int)dictionary2["expeGold"];
			dK_User.photoId = (int)dictionary2["photoId"];
			dK_User.overflow = (int)dictionary2["overflow"];
			dK_User.gameScore = (int)dictionary2["gameScore"];
			dK_User.expeScore = (int)dictionary2["expeScore"];
			dK_User.type = (int)dictionary2["type"];
			Console.WriteLine("user.id: " + dK_User.id);
			Console.WriteLine("user.username: " + dK_User.username);
			Console.WriteLine("user.nickname: " + dK_User.nickname);
			Console.WriteLine("user.sex: " + dK_User.sex);
			Console.WriteLine("user.level: " + dK_User.level);
			Console.WriteLine("user.gameGold: " + dK_User.gameGold);
			Console.WriteLine("user.expeGold: " + dK_User.expeGold);
			Console.WriteLine("user.photoId: " + dK_User.photoId);
			Console.WriteLine("user.overflow: " + dK_User.overflow);
			Console.WriteLine("user.gameScore: " + dK_User.gameScore);
			Console.WriteLine("user.expeScore: " + dK_User.expeScore);
			Console.WriteLine("user.type: " + dK_User.type);
			if (dK_User.overflow == 1)
			{
				DK_TipManager.getInstance().ShowTip(DK_TipType.CoinOverFlow, 0, string.Empty);
			}
			else
			{
				DK_GameInfo.getInstance().IsSpecial = flag3;
				DK_GameInfo.getInstance().createUser(dK_User);
				DK_GameInfo.getInstance().IsGameShuUp = flag2;
			}
		}
		else
		{
			switch (num)
			{
			case 0:
				DK_TipManager.getInstance().ShowTip(DK_TipType.UserIdDeleted, 0, string.Empty);
				break;
			case 1:
				DK_TipManager.getInstance().ShowTip(DK_TipType.Game_UserIdFrozen, 0, string.Empty);
				break;
			default:
				DK_TipManager.getInstance().ShowTip(DK_TipType.ServerUpdate, 0, string.Empty);
				break;
			}
		}
		Console.WriteLine("**********2*********DoCheckLogin**************************************");
	}

	private void DoCheckVersion(object[] args)
	{
		Console.WriteLine("**********1*********DoCheckVersion**************************************");
		string text = (string)args[0];
		Console.WriteLine("haveNewVersionIDFlag: " + text);
		DK_IOSGameStart.GetSingleton().UpdateGameVesion(text);
		Console.WriteLine("**********1*********DoCheckVersion**************************************");
	}

	private void DoNotUpdate(object[] args)
	{
		UnityEngine.Debug.Log("DoNotUpdate");
		DK_NetMngr.GetSingleton().MyCreateSocket.SendPublicKey();
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
			DK_FishDesk[] array = new DK_FishDesk[length];
			int[] array2 = new int[length];
			for (int i = 0; i < length; i++)
			{
				array2[i] = 0;
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["deskInfo"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new DK_FishDesk();
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
					array[i].seats[j] = new DK_Seat();
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
			DK_GameInfo.getInstance().updateTableList(array);
			DK_GameInfo.getInstance().SetNoHall(isHavHall: true);
			for (int k = 0; k < length; k++)
			{
				DK_GameInfo.getInstance().updateTableUserNumber(array[k].id, array2[k]);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("没有房间");
			DK_GameInfo.getInstance().updateTableList(null);
			DK_GameInfo.getInstance().SetNoHall(isHavHall: false);
			DK_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(1);
		}
	}

	private DK_FishDesk[] Test(DK_FishDesk[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = 0; j < array.Length - 1; j++)
			{
				if (array[j].id > array[j + 1].id)
				{
					DK_FishDesk dK_FishDesk = array[j];
					array[j] = array[j + 1];
					array[j + 1] = dK_FishDesk;
				}
			}
		}
		return array;
	}

	private void UpdateOnline(Dictionary<string, object> dictionary)
	{
		if (!dictionary.ContainsKey("onlineNumber"))
		{
			return;
		}
		Dictionary<string, object> dictionary2 = dictionary["onlineNumber"] as Dictionary<string, object>;
		if (DK_GameInfo.getInstance() == null || !(DK_GameInfo.getInstance().UIScene != null))
		{
			return;
		}
		for (int i = 0; i < dictionary2.Count; i++)
		{
			for (int j = 0; j < DK_GameInfo.getInstance().UIScene.selectBtnList.Count; j++)
			{
				if (i == DK_GameInfo.getInstance().UIScene.selectBtnList[j].hallId)
				{
					DK_GameInfo.getInstance().UIScene.selectBtnList[j].onlinePeople = dictionary2[i.ToString()].ToString();
					DK_GameInfo.getInstance().UIScene.selectBtnList[j].UpdateText();
					break;
				}
			}
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
			DK_FishDesk[] array = new DK_FishDesk[length];
			int[] array2 = new int[length];
			for (int i = 0; i < length; i++)
			{
				array2[i] = 0;
				int num = 0;
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["deskInfo"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new DK_FishDesk();
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
					array[i].seats[j] = new DK_Seat();
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
					DK_GameInfo.getInstance().updateTableUserNumber(j, num);
				}
			}
			array = Test(array);
			DK_GameInfo.getInstance().updateTableList(array);
			DK_GameInfo.getInstance().SetNoHall(isHavHall: true);
			for (int k = 0; k < length; k++)
			{
				DK_GameInfo.getInstance().updateTableUserNumber(array[k].id, array2[k]);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("没有房间");
			DK_GameInfo.getInstance().updateTableList(null);
			DK_GameInfo.getInstance().SetNoHall(isHavHall: false);
			DK_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(1);
		}
	}

	private void DoDeskOnlineNumber(object[] args)
	{
		Console.WriteLine("**********1*********DoDeskOnlineNumber**************************************");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["deskId"];
		int num2 = (int)dictionary["onlineNumber"];
		Console.WriteLine("deskId:" + num);
		Console.WriteLine("onlineNumber:" + num2);
		Console.WriteLine("**********2*********DoDeskOnlineNumber**************************************");
		DK_GameInfo.getInstance().updateTableUserNumber(num, num2);
	}

	private void DoRequestSeat(object[] args)
	{
		ZH2_GVars.isStartGame = true;
		Console.WriteLine("**********1*********DoRequestSeat**************************************");
		int num = 0;
		int num2 = 0;
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["bCanSeat"];
		Console.WriteLine("bCanSeat:" + flag);
		if (!flag)
		{
			num = (int)dictionary["messageStatus"];
			Console.WriteLine("messageStatus:" + num);
		}
		else
		{
			num2 = (int)dictionary["bgId"];
			int num3 = (int)dictionary["seatId"];
			UnityEngine.Debug.LogError("==========DoRequestSeat=========" + num3);
			DK_GameInfo.getInstance().User.SeatIndex = num3;
		}
		if (!DK_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			DK_NetMngr.GetSingleton().mSceneBg = num2;
		}
		else
		{
			DK_SceneBgMngr.GetSingleton().SetScene(num2);
		}
		if (flag)
		{
			DK_GameInfo.getInstance().UIScene.EnterGame();
			return;
		}
		switch (num)
		{
		case 0:
			DK_TipManager.getInstance().ShowTip(DK_TipType.TableDeleted, 0, string.Empty);
			break;
		case 1:
			if (DK_GameInfo.getInstance().User.RoomId == 1)
			{
				DK_TipManager.getInstance().ShowTip(DK_TipType.SelectTable_CreditBelowRistrict, 0, string.Empty);
			}
			else
			{
				DK_TipManager.getInstance().ShowTip(DK_TipType.SelectTable_SendExpCoin, 0, string.Empty);
			}
			break;
		case 2:
			DK_TipManager.getInstance().ShowTip(DK_TipType.SelectSeat_NotEmpty, 0, string.Empty);
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
		DK_Seat[] array = new DK_Seat[length];
		int num2 = 0;
		for (int i = 0; i < length; i++)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2 = ((dictionary["Seat"] as Array).GetValue(i) as Dictionary<string, object>);
			array[i] = new DK_Seat();
			array[i].seatId = (int)dictionary2["id"];
			array[i].isFree = (bool)dictionary2["isFree"];
			array[i].gunValue = (int)dictionary2["gunValue"];
			if (!array[i].isFree)
			{
				num2++;
				Dictionary<string, object> dictionary3 = dictionary2["user"] as Dictionary<string, object>;
				array[i].user = new DK_User();
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
		DK_GameInfo.getInstance().updateOtherUsers(array, num);
		DK_GameInfo.getInstance().updateTableUserNumber(num, num2);
	}

	private void DoGameScore(int[] args)
	{
		Console.WriteLine("**********1*********DoGameScore**************************************");
		int num = args[0];
		Console.WriteLine("gameScore: " + num);
		Console.WriteLine("**********2*********DoGameScore**************************************");
		DK_GameInfo.getInstance().updateUser("gameScore", num);
	}

	private void DoNotFired(int[] args)
	{
		Console.WriteLine("**********1*********DoNotfired**************************************");
		int num = args[0];
		Console.WriteLine("gunId: " + num);
		int num2 = args[1];
		Console.WriteLine("seatId: " + num2);
		int num3 = args[2];
		Console.WriteLine("totalScore: " + num3);
		Console.WriteLine("**********2********DoNotfired**************************************");
	}

	private void DoFired(ArrayList args)
	{
		int num = int.Parse((string)args[0]);
		double num2 = Convert.ToDouble(args[1]);
		int num3 = int.Parse((string)args[2]);
		int num4 = int.Parse((string)args[3]);
		bool isLizi = Convert.ToBoolean(args[4]);
		int newScore = int.Parse((string)args[5]);
		if (DK_NetMngr.GetSingleton().IsGameSceneLoadOk && !DK_GameParameter.G_bTest && num3 != DK_GameMngr.GetSingleton().mPlayerSeatID)
		{
			DK_BulletPoolMngr.GetSingleton().LanchBullet(num3, (float)num2, num4, isLizi);
		}
		Console.WriteLine("**********2*********DoFired**************************************");
		if (DK_GameInfo.getInstance().currentState == DK_GameState.On_Game)
		{
			DK_GameInfo.getInstance().GameScene.UpdateWhenFired(num3, newScore, num4, (float)num2);
		}
	}

	private void DoRestBet(int[] args)
	{
		try
		{
			UnityEngine.Debug.LogError("牛魔王 或者 炸弹 倍率: " + JsonMapper.ToJson(args));
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
		}
		try
		{
			ZH2_GVars.niuMoWangBeiLv = 0;
			ZH2_GVars.niuMoWangBeiLv = int.Parse(args[0].ToString());
		}
		catch (Exception arg2)
		{
			UnityEngine.Debug.LogError("解析错误: " + arg2);
		}
	}

	private void DoGunHitFish(ArrayList args)
	{
		int num = int.Parse((string)args[0]);
		Console.WriteLine("gunid: " + num);
		int num2 = int.Parse((string)args[1]);
		Console.WriteLine("gunValue: " + num2);
		int num3 = int.Parse((string)args[2]);
		Console.WriteLine("seatid: " + num3);
		bool flag = Convert.ToBoolean(args[3]);
		Console.WriteLine("bCreateX2Gun: " + flag);
		bool flag2 = Convert.ToBoolean(args[4]);
		Console.WriteLine("bDeadKnife: " + flag2);
		int num4 = int.Parse((string)args[5]);
		UnityEngine.Debug.Log("fishDeadScore: " + num4);
		int num5 = int.Parse((string)args[6]);
		Console.WriteLine("totalScore: " + num5);
		if (DK_GameInfo.getInstance().currentState == DK_GameState.On_Game)
		{
			DK_GameInfo.getInstance().GameScene.UpdateWhenDead(num3, num5, num4, num2);
		}
		if (flag && num3 == DK_GameMngr.GetSingleton().mPlayerSeatID && DK_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			DK_BulletPoolMngr.GetSingleton().SetLizi();
		}
		string text = (string)args[7];
		string[] array = text.Split('|');
		DK_HitFish[] array2 = new DK_HitFish[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string[] array3 = array[i].Split('#');
			array2[i] = new DK_HitFish();
			array2[i].fishid = int.Parse(array3[0]);
			array2[i].fishtype = int.Parse(array3[1]);
			array2[i].fx = Convert.ToDouble(array3[2]);
			array2[i].fy = Convert.ToDouble(array3[3]);
			array2[i].bet = int.Parse(array3[4]);
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (!DK_GameParameter.G_bTest && DK_NetMngr.GetSingleton().IsGameSceneLoadOk && DK_FishPoolMngr.GetSingleton() != null && DK_EffectMngr.GetSingleton() != null)
			{
				Vector3 pos = Vector3.right * (float)array2[j].fx + Vector3.up * (float)array2[j].fy;
				DK_FishPoolMngr.GetSingleton().SetFishDie(array2[j].fishid, num2, array2[j].fishtype, array2[j].bet, num3, pos);
				if (flag && j == 0)
				{
					DK_EffectMngr.GetSingleton().PlayLiziCardFly(num3, pos);
				}
			}
		}
		if (!flag2)
		{
			return;
		}
		string text2 = (string)args[8];
		UnityEngine.Debug.LogError("strHitFish2：" + text2);
		string[] array4 = text2.Split('|');
		DK_HitFish[] array5 = new DK_HitFish[array4.Length];
		for (int k = 0; k < array4.Length; k++)
		{
			string[] array6 = array4[k].Split('#');
			array5[k] = new DK_HitFish();
			try
			{
				array5[k].fishid = int.Parse(array6[0]);
				array5[k].fishtype = int.Parse(array6[1]);
				array5[k].fx = Convert.ToDouble(array6[2]);
				array5[k].fy = Convert.ToDouble(array6[3]);
				array5[k].bet = int.Parse(array6[4]);
			}
			catch (Exception)
			{
			}
		}
		for (int l = 0; l < array4.Length; l++)
		{
			if (!DK_GameParameter.G_bTest && DK_FishPoolMngr.GetSingleton() != null && DK_EffectMngr.GetSingleton() != null)
			{
				Vector3 pos2 = Vector3.right * (float)array5[l].fx + Vector3.up * (float)array5[l].fy;
				DK_FishPoolMngr.GetSingleton().SetFishDie(array5[l].fishid, num2, array5[l].fishtype, array5[l].bet, num3, pos2);
			}
		}
	}

	private void DoNewFishGroup(int[] args)
	{
		int num = args[0];
		UnityEngine.Debug.LogError("鱼阵类型: " + num);
		DK_SceneBgMngr.GetSingleton().BigFishFormatReset();
		DK_Formation.GetSingleton().ShowFormation((DK_FORMATION)num);
		if (DK_GameInfo.getInstance().currentState == DK_GameState.On_Game)
		{
			DK_GameInfo.getInstance().GameScene.HideTip();
		}
	}

	private void DoNewFishGroup(int args)
	{
		UnityEngine.Debug.LogError("鱼阵类型: " + args);
		DK_SceneBgMngr.GetSingleton().BigFishFormatReset();
		DK_Formation.GetSingleton().ShowFormation((DK_FORMATION)args);
		if (DK_GameInfo.getInstance().currentState == DK_GameState.On_Game)
		{
			DK_GameInfo.getInstance().GameScene.HideTip();
		}
	}

	private void DoNewFish(string[] args)
	{
		int num = 0;
		int num2 = int.Parse(args[0]);
		int num3 = int.Parse(args[1]);
		int num4 = int.Parse(args[2]);
		int newFishPathType = int.Parse(args[3]);
		int[] array = null;
		if (args.Length == 5)
		{
			int num5 = int.Parse(args[4]);
			array = new int[1]
			{
				num5
			};
			num = 1;
		}
		else if (args.Length == 6)
		{
			int num6 = int.Parse(args[4]);
			int num7 = int.Parse(args[5]);
			array = new int[2]
			{
				num6,
				num7
			};
			num = 2;
		}
		DK_FISH_TYPE[] array2 = null;
		if (num > 0)
		{
			array2 = new DK_FISH_TYPE[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = (DK_FISH_TYPE)array[i];
			}
		}
		int num8 = num4;
		if (num8 <= 0)
		{
			return;
		}
		DK_FishPathType[] array3 = new DK_FishPathType[num8];
		for (int j = 0; j < num8; j++)
		{
			array3[j] = new DK_FishPathType();
			array3[j].fishId = num2 + j;
			array3[j].newFishPathType = newFishPathType;
		}
		if (num3 != 40 && DK_NetMngr.GetSingleton().IsGameSceneLoadOk && DK_FishPoolMngr.GetSingleton() != null && !DK_GameParameter.G_bTest)
		{
			if (num3 == 32 && array2 != null)
			{
				DK_FishPoolMngr.GetSingleton().CreateCoralReefsFish(array2[0], array3[0].newFishPathType, array3[0].fishId);
			}
			else if (num3 == 39 && array2 != null)
			{
				DK_FishPoolMngr.GetSingleton().CreateFish((DK_FISH_TYPE)num3, array3[0].newFishPathType, array3[0].fishId, num4, array2);
			}
			else
			{
				DK_FishPoolMngr.GetSingleton().CreateFish((DK_FISH_TYPE)num3, array3[0].newFishPathType, array3[0].fishId, num4);
			}
			if (DK_GameInfo.getInstance().CountTime)
			{
				DK_FishPoolMngr.GetSingleton().UnFixAllFish();
			}
		}
		if (DK_GameInfo.getInstance().currentState == DK_GameState.On_Game)
		{
			DK_GameInfo.getInstance().GameScene.HideTip();
		}
	}

	private void DoClearFish(int[] args, bool isBackGround)
	{
		Console.WriteLine("**********1*********DoClearfish**************************************");
		int num = args[0];
		Console.WriteLine("**********2*********DoClearfish**************************************");
		if (DK_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			if (!isBackGround)
			{
				DK_SceneBgMngr.GetSingleton().ChangeScene(num);
			}
			else
			{
				DK_SceneBgMngr.GetSingleton().SetScene(num);
			}
		}
	}

	private void DoForbitFired(object[] args)
	{
		Console.WriteLine("**********1*********DoForbitFired**************************************");
		Console.WriteLine("**********2*********DoForbitFired**************************************");
		if (DK_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			DK_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = false;
		}
	}

	private void DoSendNotice(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		UnityEngine.Debug.LogError("通知: " + jsonData.ToJson());
		if (All_NoticePanel.GetInstance() != null)
		{
			All_NoticePanel.GetInstance().AddTip(jsonData);
		}
	}

	private void DoScrollMessage(string[] args)
	{
		Console.WriteLine("**********1*********DoScrollMessage**************************************");
		string text = args[0];
		Console.WriteLine("message: " + text);
		Console.WriteLine("**********2*********DoScrollMessage**************************************");
		if (DK_GameInfo.getInstance().currentState == DK_GameState.On_Game)
		{
			DK_GameInfo.getInstance().GameScene.AddNotice(text);
		}
	}

	private void DoPlayerInfo(object[] args)
	{
		Console.WriteLine("**********1*********DoPlayerInfo**************************************");
		DK_User dK_User = new DK_User();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["seatId"];
		int honor = (int)dictionary["honor"];
		UnityEngine.Debug.LogError("=======DoPlayerInfo座位=======" + num);
		Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
		dK_User.id = (int)dictionary2["id"];
		dK_User.username = (string)dictionary2["username"];
		dK_User.nickname = (string)dictionary2["nickname"];
		dK_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
		dK_User.level = (int)dictionary2["level"];
		dK_User.gameGold = (int)dictionary2["gameGold"];
		dK_User.expeGold = (int)dictionary2["expeGold"];
		dK_User.photoId = (int)dictionary2["photoId"];
		dK_User.overflow = (int)dictionary2["overflow"];
		dK_User.gameScore = (int)dictionary2["gameScore"];
		dK_User.expeScore = (int)dictionary2["expeScore"];
		dK_User.type = (int)dictionary2["type"];
		DK_GameInfo.getInstance().getPersonInfo(new DK_UserInfo(dK_User), honor);
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
		if (DK_GameInfo.getInstance().currentState != DK_GameState.On_Game)
		{
		}
	}

	private void DoGameShutup(bool[] args)
	{
		Console.WriteLine("**********1*********DoGameShutup**************************************");
		bool flag = args[0];
		Console.WriteLine("forbidFlag: " + flag);
		Console.WriteLine("**********2*********DoGameShutup**************************************");
		DK_GameInfo.getInstance().IsGameShuUp = flag;
	}

	private void DoUserShutup(bool[] args)
	{
		Console.WriteLine("**********1*********DoUserShutup**************************************");
		bool flag = args[0];
		Console.WriteLine("forbidFlag: " + flag);
		Console.WriteLine("**********2*********DoUserShutup**************************************");
		DK_GameInfo.getInstance().IsUserShutUp = flag;
	}

	private void DoUserAward(int[] args)
	{
		Console.WriteLine("**********1*********DoUserAward**************************************");
		int num = args[0];
		Console.WriteLine("gold: " + num);
		Console.WriteLine("**********2*********DoUserAward**************************************");
		DK_GameInfo.getInstance().updateUser("gameCoin", num);
		DK_TipManager.getInstance().ShowTip(DK_TipType.GivingCoin, num, string.Empty);
	}

	private void DoGameGold(int[] args)
	{
		Console.WriteLine("**********1*********DoGameGold**************************************");
		int num = args[0];
		Console.WriteLine("gameGold: " + num);
		Console.WriteLine("**********2*********DoGameGold**************************************");
		DK_GameInfo.getInstance().updateUser("gameCoin", num);
	}

	private void DoExpeGold(int[] args)
	{
		Console.WriteLine("**********1*********DoExpeGold**************************************");
		int num = args[0];
		Console.WriteLine("expeGold: " + num);
		Console.WriteLine("**********2*********DoExpeGold**************************************");
		DK_GameInfo.getInstance().updateUser("testCoin", num);
	}

	private void DoAddExpeGoldAuto(bool[] args)
	{
		Console.WriteLine("**********1*********DoAddExpeGoldAuto**************************************");
		bool flag = args[0];
		Console.WriteLine("bOk: " + flag);
		Console.WriteLine("**********2*********DoAddExpeGoldAuto**************************************");
		DK_SoundManage.getInstance().playButtonMusic(DK_ButtonMusicType.getTestCoin);
		DK_TipManager.getInstance().ShowTip(DK_TipType.ApplyForExpCoin_Success, 0, string.Empty);
	}

	private void DoOverflow(object[] args)
	{
		Console.WriteLine("**********1*********DoOverflow**************************************");
		DK_TipManager.getInstance().ShowTip(DK_TipType.CoinOverFlow, 0, string.Empty);
	}

	private void DoQuitToLogin(int[] args)
	{
		Console.WriteLine("**********1*********DoQuitToLogin**************************************");
		int num = args[0];
		Console.WriteLine("type: " + num);
		Console.WriteLine("**********2*********DoQuitToLogin**************************************");
		switch (num)
		{
		case 1:
			DK_TipManager.getInstance().ShowTip(DK_TipType.ServerUpdate, 0, string.Empty);
			break;
		case 2:
			DK_TipManager.getInstance().ShowTip(DK_TipType.Game_UserIdFrozen, 0, string.Empty);
			break;
		case 3:
			DK_TipManager.getInstance().ShowTip(DK_TipType.UserIdDeleted, 0, string.Empty);
			break;
		case 4:
			DK_TipManager.getInstance().ShowTip(DK_TipType.UserIdRepeative, 0, string.Empty);
			DK_Sockets.GetSingleton().isReconnect = false;
			break;
		case 5:
			DK_TipManager.getInstance().ShowTip(DK_TipType.UserPwdChanged, 0, string.Empty);
			break;
		case 6:
			DK_TipManager.getInstance().ShowTip(DK_TipType.LoseTheServer, 0, string.Empty);
			break;
		}
	}

	private void DoQuitToRoom(int[] args)
	{
		Console.WriteLine("**********1*********DoQuitToRoom**************************************");
		int num = args[0];
		Console.WriteLine("type: " + num);
		Console.WriteLine("**********2*********DoQuitToRoom**************************************");
		switch (num)
		{
		case 1:
			DK_TipManager.getInstance().ShowTip(DK_TipType.TableDeleted, 0, string.Empty);
			break;
		case 2:
			DK_TipManager.getInstance().ShowTip(DK_TipType.TableConfigChanged, 0, string.Empty);
			break;
		default:
			DK_TipManager.getInstance().ShowTip(DK_TipType.LongTimeNoHandle, 0, string.Empty);
			break;
		}
	}

	private void DoQuitToDesk(object[] args)
	{
		DK_TipManager.getInstance().ShowTip(DK_TipType.QuitToDesk, 0, string.Empty);
	}

	private void HandleNetMsg_ApplyExchange(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		DK_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (DK_GameInfo.getInstance() != null && DK_GameInfo.getInstance().UIScene != null)
		{
			DK_GameInfo.getInstance().UIScene.UpdateUserInfo();
		}
		string ch = (!flag) ? "兑换被拒绝" : "兑换成功";
		string en = (!flag) ? "Failure of exchange" : "Successful exchange";
		DK_TipManager.getInstance().ShowTip(DK_TipType.Custom, 0, ZH2_GVars.ShowTip(ch, en, string.Empty));
	}

	private void HandleNetMsg_ApplyPay(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		DK_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (DK_GameInfo.getInstance() != null && DK_GameInfo.getInstance().UIScene != null)
		{
			DK_GameInfo.getInstance().UIScene.UpdateUserInfo();
		}
		string ch = (!flag) ? "充值被拒绝" : "充值成功";
		string en = (!flag) ? "Failure of TopUp" : "Successful TopUp";
		DK_TipManager.getInstance().ShowTip(DK_TipType.Custom, 0, ZH2_GVars.ShowTip(ch, en, string.Empty));
	}

	private void HandleNetMsg_changeScoreNotice(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		DK_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (DK_GameInfo.getInstance() != null && DK_GameInfo.getInstance().UIScene != null)
		{
			DK_GameInfo.getInstance().UIScene.UpdateUserInfo();
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
		DK_TipManager.getInstance().ShowTip(DK_TipType.Custom, 0, ZH2_GVars.ShowTip($"{arg}成功! 金币{num}", $"Successful {arg2}! GoldCoin {num}", string.Empty));
	}

	private void DoLockFish(ArrayList args)
	{
		UnityEngine.Debug.Log("**********1*********DoLockFish**************************************");
		bool flag = (bool)args[0];
		UnityEngine.Debug.Log("isLockFish: " + flag);
		int num = (int)args[1];
		Console.WriteLine("fishId: " + num);
		int num2 = (int)args[2];
		Console.WriteLine("seatId: " + num2);
		DK_FishPoolMngr.GetSingleton().LockFish(num, num2, flag);
	}

	private void DoUnLockFish(int[] args)
	{
		Console.WriteLine("**********1*********DoUnLockFish**************************************");
		int num = args[0];
		Console.WriteLine("fishId: " + num);
		int num2 = args[1];
		Console.WriteLine("seatId: " + num2);
		Console.WriteLine("**********2*********DoUnLockFish**************************************");
		DK_FishPoolMngr.GetSingleton().UnLockFish(num, num2);
	}

	private void DoUnLockScreen(object[] args)
	{
		Console.WriteLine("**********1*********unLockScreen**************************************");
		DK_FishPoolMngr.GetSingleton().UnFixAllFish();
		Console.WriteLine("**********1*********unLockScreen**************************************");
	}
}
