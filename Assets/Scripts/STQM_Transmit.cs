using GameCommon;
using GameConfig;
using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STQM_Transmit : MonoBehaviour
{
	private static STQM_Transmit _MyTransmit;

	private STQM_Sockets m_CreateSocket;

	public static STQM_Transmit GetSingleton()
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

	public void TransmitGetPoint(STQM_Sockets MyCreateSocket)
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
			if (STQM_GameInfo.getInstance().Key != string.Empty)
			{
				STQM_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(STQM_GameInfo.getInstance().UserId, STQM_GameInfo.getInstance().Pwd, (int)((STQM_GameInfo.getInstance().currentState < STQM_GameState.On_SelectRoom) ? STQM_GameState.On_SelectRoom : STQM_GameInfo.getInstance().currentState), string.Empty);
			}
			STQM_GameInfo.getInstance().Key = "sendServerTime";
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
		object[] args = table["args"] as object[];
		Console.WriteLine();
		if (text == "sendServerTime")
		{
			if (STQM_GameInfo.getInstance().Key != string.Empty)
			{
				STQM_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(STQM_GameInfo.getInstance().UserId, STQM_GameInfo.getInstance().Pwd, (int)STQM_GameInfo.getInstance().currentState, string.Empty);
			}
			STQM_GameInfo.getInstance().Key = "sendServerTime";
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
		if (STQM_Sockets.GetSingleton().isReconnect)
		{
			if (m_CreateSocket.GetRelineCount() < 5)
			{
				m_CreateSocket.CreateReceiveThread();
				return;
			}
			Console.WriteLine("30秒重连失败，网络断开，请重新登录网络大厅...");
			STQM_TipManager.getInstance().ShowTip(STQM_TipType.Net_ConnectionError, 0, string.Empty);
		}
	}

	private void DoCheckLogin(object[] args)
	{
		Console.WriteLine("**********1*********DoCheckLogin**************************************");
		STQM_User sTQM_User = new STQM_User();
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
			sTQM_User.id = (int)dictionary2["id"];
			ZH2_GVars.userId = sTQM_User.id;
			sTQM_User.username = (string)dictionary2["username"];
			sTQM_User.nickname = (string)dictionary2["nickname"];
			sTQM_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
			sTQM_User.level = (int)dictionary2["level"];
			sTQM_User.gameGold = (int)dictionary2["gameGold"];
			sTQM_User.expeGold = (int)dictionary2["expeGold"];
			sTQM_User.photoId = (int)dictionary2["photoId"];
			sTQM_User.overflow = (int)dictionary2["overflow"];
			sTQM_User.gameScore = (int)dictionary2["gameScore"];
			sTQM_User.expeScore = (int)dictionary2["expeScore"];
			sTQM_User.type = (int)dictionary2["type"];
			Console.WriteLine("user.id: " + sTQM_User.id);
			Console.WriteLine("user.username: " + sTQM_User.username);
			Console.WriteLine("user.nickname: " + sTQM_User.nickname);
			Console.WriteLine("user.sex: " + sTQM_User.sex);
			Console.WriteLine("user.level: " + sTQM_User.level);
			Console.WriteLine("user.gameGold: " + sTQM_User.gameGold);
			Console.WriteLine("user.expeGold: " + sTQM_User.expeGold);
			Console.WriteLine("user.photoId: " + sTQM_User.photoId);
			Console.WriteLine("user.overflow: " + sTQM_User.overflow);
			Console.WriteLine("user.gameScore: " + sTQM_User.gameScore);
			Console.WriteLine("user.expeScore: " + sTQM_User.expeScore);
			Console.WriteLine("user.type: " + sTQM_User.type);
			if (sTQM_User.overflow == 1)
			{
				STQM_TipManager.getInstance().ShowTip(STQM_TipType.CoinOverFlow, 0, string.Empty);
			}
			else
			{
				STQM_GameInfo.getInstance().IsSpecial = flag3;
				STQM_GameInfo.getInstance().createUser(sTQM_User);
				STQM_GameInfo.getInstance().IsGameShuUp = flag2;
			}
		}
		else
		{
			switch (num)
			{
			case 0:
				STQM_TipManager.getInstance().ShowTip(STQM_TipType.UserIdDeleted, 0, string.Empty);
				break;
			case 1:
				STQM_TipManager.getInstance().ShowTip(STQM_TipType.Game_UserIdFrozen, 0, string.Empty);
				break;
			default:
				STQM_TipManager.getInstance().ShowTip(STQM_TipType.ServerUpdate, 0, string.Empty);
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
		STQM_IOSGameStart.GetSingleton().UpdateGameVesion(text);
		Console.WriteLine("**********1*********DoCheckVersion**************************************");
	}

	private void DoNotUpdate(object[] args)
	{
		UnityEngine.Debug.Log("DoNotUpdate");
		STQM_NetMngr.GetSingleton().MyCreateSocket.SendPublicKey();
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
			STQM_FishDesk[] array = new STQM_FishDesk[length];
			int[] array2 = new int[length];
			for (int i = 0; i < length; i++)
			{
				array2[i] = 0;
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["deskInfo"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new STQM_FishDesk();
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
					array[i].seats[j] = new STQM_Seat();
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
			STQM_GameInfo.getInstance().updateTableList(array);
			STQM_GameInfo.getInstance().SetNoHall(isHavHall: true);
			for (int k = 0; k < length; k++)
			{
				STQM_GameInfo.getInstance().updateTableUserNumber(array[k].id, array2[k]);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("没有房间");
			STQM_GameInfo.getInstance().SetNoHall(isHavHall: false);
			STQM_GameInfo.getInstance().updateTableList(null);
		}
	}

	private STQM_FishDesk[] Test(STQM_FishDesk[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = 0; j < array.Length - 1; j++)
			{
				if (array[j].id > array[j + 1].id)
				{
					STQM_FishDesk sTQM_FishDesk = array[j];
					array[j] = array[j + 1];
					array[j + 1] = sTQM_FishDesk;
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
			if (STQM_GameInfo.getInstance() != null && STQM_GameInfo.getInstance().UIScene != null)
			{
				for (int i = 0; i < dictionary2.Count; i++)
				{
					for (int j = 0; j < STQM_GameInfo.getInstance().UIScene.selectBtnList.Count; j++)
					{
						if (i == STQM_GameInfo.getInstance().UIScene.selectBtnList[j].hallId)
						{
							STQM_GameInfo.getInstance().UIScene.selectBtnList[j].onlinePeople = dictionary2[i.ToString()].ToString();
							STQM_GameInfo.getInstance().UIScene.selectBtnList[j].UpdateText();
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
			STQM_FishDesk[] array = new STQM_FishDesk[length];
			int[] array2 = new int[length];
			for (int i = 0; i < length; i++)
			{
				array2[i] = 0;
				int num = 0;
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["deskInfo"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new STQM_FishDesk();
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
					array[i].seats[j] = new STQM_Seat();
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
					}
					STQM_GameInfo.getInstance().updateTableUserNumber(j, num);
				}
			}
			array = Test(array);
			STQM_GameInfo.getInstance().updateTableList(array);
			STQM_GameInfo.getInstance().SetNoHall(isHavHall: true);
			for (int k = 0; k < length; k++)
			{
				STQM_GameInfo.getInstance().updateTableUserNumber(array[k].id, array2[k]);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("没有房间");
			STQM_GameInfo.getInstance().SetNoHall(isHavHall: false);
			STQM_GameInfo.getInstance().updateTableList(null);
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
		STQM_GameInfo.getInstance().updateTableUserNumber(num, num2);
	}

	private void DoRequestSeat(object[] args)
	{
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
			int seatIndex = (int)dictionary["seatId"];
			STQM_GameInfo.getInstance().User.SeatIndex = seatIndex;
		}
		if (!STQM_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			STQM_NetMngr.GetSingleton().mSceneBg = num2;
		}
		else
		{
			STQM_SceneBgMngr.GetSingleton().SetScene(num2);
		}
		if (flag)
		{
			STQM_GameInfo.getInstance().UIScene.EnterGame();
			return;
		}
		switch (num)
		{
		case 0:
			STQM_TipManager.getInstance().ShowTip(STQM_TipType.TableDeleted, 0, string.Empty);
			break;
		case 1:
			if (STQM_GameInfo.getInstance().User.RoomId == 1)
			{
				STQM_TipManager.getInstance().ShowTip(STQM_TipType.SelectTable_CreditBelowRistrict, 0, string.Empty);
			}
			else
			{
				STQM_TipManager.getInstance().ShowTip(STQM_TipType.SelectTable_SendExpCoin, 0, string.Empty);
			}
			break;
		case 2:
			STQM_TipManager.getInstance().ShowTip(STQM_TipType.SelectSeat_NotEmpty, 0, string.Empty);
			break;
		}
	}

	private void DoUpdateDeskInfo(object[] args)
	{
		UnityEngine.Debug.Log("**********1*********DoUpdateDeskInfo**************************************");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["deskId"];
		int length = (dictionary["Seat"] as Array).Length;
		if (length > 0)
		{
			STQM_Seat[] array = new STQM_Seat[length];
			int num2 = 0;
			for (int i = 0; i < length; i++)
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["Seat"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new STQM_Seat();
				array[i].seatId = (int)dictionary2["id"];
				array[i].isFree = (bool)dictionary2["isFree"];
				array[i].gunValue = (int)dictionary2["gunValue"];
				if (!array[i].isFree)
				{
					num2++;
					Dictionary<string, object> dictionary3 = dictionary2["user"] as Dictionary<string, object>;
					array[i].user = new STQM_User();
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
			STQM_GameInfo.getInstance().updateOtherUsers(array, num);
			STQM_GameInfo.getInstance().updateTableUserNumber(num, num2);
		}
		UnityEngine.Debug.Log("**********2*********DoUpdateDeskInfo**************************************");
	}

	private void DoGameScore(int[] args)
	{
		Console.WriteLine("**********1*********DoGameScore**************************************");
		int num = args[0];
		Console.WriteLine("gameScore: " + num);
		Console.WriteLine("**********2*********DoGameScore**************************************");
		STQM_GameInfo.getInstance().updateUser("gameScore", num);
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
		if (STQM_NetMngr.GetSingleton().IsGameSceneLoadOk && !STQM_GameParameter.G_bTest && num3 != STQM_GameMngr.GetSingleton().mPlayerSeatID)
		{
			STQM_BulletPoolMngr.GetSingleton().LanchBullet(num3, (float)num2, num4, isLizi);
		}
		Console.WriteLine("**********2*********DoFired**************************************");
		if (STQM_GameInfo.getInstance().currentState == STQM_GameState.On_Game)
		{
			STQM_GameInfo.getInstance().GameScene.UpdateWhenFired(num3, newScore, num4, (float)num2);
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
		if (STQM_GameInfo.getInstance().currentState == STQM_GameState.On_Game)
		{
			STQM_GameInfo.getInstance().GameScene.UpdateWhenDead(num3, num5, num4, num2);
		}
		if (flag && num3 == STQM_GameMngr.GetSingleton().mPlayerSeatID && STQM_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			STQM_BulletPoolMngr.GetSingleton().SetLizi();
		}
		string text = (string)args[7];
		string[] array = text.Split('|');
		STQM_HitFish[] array2 = new STQM_HitFish[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string[] array3 = array[i].Split('#');
			array2[i] = new STQM_HitFish();
			array2[i].fishid = int.Parse(array3[0]);
			array2[i].fishtype = int.Parse(array3[1]);
			array2[i].fx = Convert.ToDouble(array3[2]);
			array2[i].fy = Convert.ToDouble(array3[3]);
			array2[i].bet = int.Parse(array3[4]);
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (!STQM_GameParameter.G_bTest && STQM_NetMngr.GetSingleton().IsGameSceneLoadOk && STQM_FishPoolMngr.GetSingleton() != null && STQM_EffectMngr.GetSingleton() != null)
			{
				Vector3 pos = Vector3.right * (float)array2[j].fx + Vector3.up * (float)array2[j].fy;
				STQM_FishPoolMngr.GetSingleton().SetFishDie(array2[j].fishid, num2, array2[j].fishtype, array2[j].bet, num3, pos);
				if (flag && j == 0)
				{
					STQM_EffectMngr.GetSingleton().PlayLiziCardFly(num3, pos);
				}
			}
		}
		if (flag2)
		{
			string text2 = (string)args[8];
			Console.WriteLine("strHitFish2：" + text2);
			string[] array4 = text2.Split('|');
			STQM_HitFish[] array5 = new STQM_HitFish[array4.Length];
			for (int k = 0; k < array4.Length; k++)
			{
				string[] array6 = array4[k].Split('#');
				array5[k] = new STQM_HitFish();
				array5[k].fishid = int.Parse(array6[0]);
				array5[k].fishtype = int.Parse(array6[1]);
				array5[k].fx = Convert.ToDouble(array6[2]);
				array5[k].fy = Convert.ToDouble(array6[3]);
				array5[k].bet = int.Parse(array6[4]);
			}
			for (int l = 0; l < array4.Length; l++)
			{
				if (!STQM_GameParameter.G_bTest && STQM_FishPoolMngr.GetSingleton() != null && STQM_EffectMngr.GetSingleton() != null)
				{
					Vector3 pos2 = Vector3.right * (float)array5[l].fx + Vector3.up * (float)array5[l].fy;
					STQM_FishPoolMngr.GetSingleton().SetFishDie(array5[l].fishid, num2, array5[l].fishtype, array5[l].bet, num3, pos2);
				}
			}
		}
		UnityEngine.Debug.Log("**********2*********DoGunHitfish**************************************");
	}

	private void DoNewFishGroup(int[] args)
	{
		Console.WriteLine("**********1*********DoNewfishGroup**************************************");
		int num = args[0];
		UnityEngine.Debug.Log("fishGroupType: " + num);
		Console.WriteLine("**********2********DoNewfishGroup**************************************");
		STQM_SceneBgMngr.GetSingleton().BigFishFormatReset();
		STQM_Formation.GetSingleton().ShowFormation((STQM_FORMATION)num);
		if (STQM_GameInfo.getInstance().currentState == STQM_GameState.On_Game)
		{
			STQM_GameInfo.getInstance().GameScene.HideTip();
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
		STQM_FISH_TYPE[] array2 = null;
		if (num > 0)
		{
			array2 = new STQM_FISH_TYPE[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = (STQM_FISH_TYPE)array[i];
			}
		}
		int num8 = num4;
		if (num8 <= 0)
		{
			return;
		}
		STQM_FishPathType[] array3 = new STQM_FishPathType[num8];
		for (int j = 0; j < num8; j++)
		{
			array3[j] = new STQM_FishPathType();
			array3[j].fishId = num2 + j;
			array3[j].newFishPathType = newFishPathType;
		}
		if (num3 != 32 && STQM_NetMngr.GetSingleton().IsGameSceneLoadOk && STQM_FishPoolMngr.GetSingleton() != null && !STQM_GameParameter.G_bTest)
		{
			if (num3 == 30 && array2 != null)
			{
				STQM_FishPoolMngr.GetSingleton().CreateCoralReefsFish(array2[0], array3[0].newFishPathType, array3[0].fishId);
			}
			else
			{
				STQM_FishPoolMngr.GetSingleton().CreateFish((STQM_FISH_TYPE)num3, array3[0].newFishPathType, array3[0].fishId, num4);
			}
			if (STQM_GameInfo.getInstance().CountTime)
			{
				STQM_FishPoolMngr.GetSingleton().UnFixAllFish();
			}
		}
		if (STQM_GameInfo.getInstance().currentState == STQM_GameState.On_Game)
		{
			STQM_GameInfo.getInstance().GameScene.HideTip();
		}
	}

	private void DoClearFish(int[] args, bool isBackGround)
	{
		Console.WriteLine("**********1*********DoClearfish**************************************");
		int num = args[0];
		Console.WriteLine("**********2*********DoClearfish**************************************");
		if (STQM_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			if (!isBackGround)
			{
				STQM_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = false;
				STQM_SceneBgMngr.GetSingleton().ChangeScene(num);
			}
			else
			{
				STQM_SceneBgMngr.GetSingleton().SetScene(num);
			}
		}
	}

	private void DoForbitFired(object[] args)
	{
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
		if (STQM_GameInfo.getInstance().currentState == STQM_GameState.On_Game)
		{
			STQM_GameInfo.getInstance().GameScene.AddNotice(text);
		}
	}

	private void DoPlayerInfo(object[] args)
	{
		Console.WriteLine("**********1*********DoPlayerInfo**************************************");
		STQM_User sTQM_User = new STQM_User();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["seatId"];
		int num2 = (int)dictionary["honor"];
		Console.WriteLine("seatid: " + num);
		Console.WriteLine("honor: " + num2);
		Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
		sTQM_User.id = (int)dictionary2["id"];
		sTQM_User.username = (string)dictionary2["username"];
		sTQM_User.nickname = (string)dictionary2["nickname"];
		sTQM_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
		sTQM_User.level = (int)dictionary2["level"];
		sTQM_User.gameGold = (int)dictionary2["gameGold"];
		sTQM_User.expeGold = (int)dictionary2["expeGold"];
		sTQM_User.photoId = (int)dictionary2["photoId"];
		sTQM_User.overflow = (int)dictionary2["overflow"];
		sTQM_User.gameScore = (int)dictionary2["gameScore"];
		sTQM_User.expeScore = (int)dictionary2["expeScore"];
		sTQM_User.type = (int)dictionary2["type"];
		Console.WriteLine("user.id: " + sTQM_User.id);
		Console.WriteLine("user.username: " + sTQM_User.username);
		Console.WriteLine("user.nickname: " + sTQM_User.nickname);
		Console.WriteLine("user.sex: " + sTQM_User.sex);
		Console.WriteLine("user.level: " + sTQM_User.level);
		Console.WriteLine("user.gameGold: " + sTQM_User.gameGold);
		Console.WriteLine("user.expeGold: " + sTQM_User.expeGold);
		Console.WriteLine("user.photoId: " + sTQM_User.photoId);
		Console.WriteLine("user.overflow: " + sTQM_User.overflow);
		Console.WriteLine("user.gameScore: " + sTQM_User.gameScore);
		Console.WriteLine("user.expeScore: " + sTQM_User.expeScore);
		Console.WriteLine();
		STQM_GameInfo.getInstance().getPersonInfo(new STQM_UserInfo(sTQM_User), num2);
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
		if (STQM_GameInfo.getInstance().currentState != STQM_GameState.On_Game)
		{
		}
	}

	private void DoGameShutup(bool[] args)
	{
		Console.WriteLine("**********1*********DoGameShutup**************************************");
		bool flag = args[0];
		Console.WriteLine("forbidFlag: " + flag);
		Console.WriteLine("**********2*********DoGameShutup**************************************");
		STQM_GameInfo.getInstance().IsGameShuUp = flag;
	}

	private void DoUserShutup(bool[] args)
	{
		Console.WriteLine("**********1*********DoUserShutup**************************************");
		bool flag = args[0];
		Console.WriteLine("forbidFlag: " + flag);
		Console.WriteLine("**********2*********DoUserShutup**************************************");
		STQM_GameInfo.getInstance().IsUserShutUp = flag;
	}

	private void DoUserAward(int[] args)
	{
		Console.WriteLine("**********1*********DoUserAward**************************************");
		int num = args[0];
		Console.WriteLine("gold: " + num);
		Console.WriteLine("**********2*********DoUserAward**************************************");
		STQM_GameInfo.getInstance().updateUser("gameCoin", num);
		STQM_TipManager.getInstance().ShowTip(STQM_TipType.GivingCoin, num, string.Empty);
	}

	private void DoGameGold(int[] args)
	{
		Console.WriteLine("**********1*********DoGameGold**************************************");
		int num = args[0];
		Console.WriteLine("gameGold: " + num);
		Console.WriteLine("**********2*********DoGameGold**************************************");
		STQM_GameInfo.getInstance().updateUser("gameCoin", num);
	}

	private void DoExpeGold(int[] args)
	{
		Console.WriteLine("**********1*********DoExpeGold**************************************");
		int num = args[0];
		Console.WriteLine("expeGold: " + num);
		Console.WriteLine("**********2*********DoExpeGold**************************************");
		STQM_GameInfo.getInstance().updateUser("testCoin", num);
	}

	private void DoAddExpeGoldAuto(bool[] args)
	{
		Console.WriteLine("**********1*********DoAddExpeGoldAuto**************************************");
		bool flag = args[0];
		Console.WriteLine("bOk: " + flag);
		Console.WriteLine("**********2*********DoAddExpeGoldAuto**************************************");
		STQM_SoundManage.getInstance().playButtonMusic(STQM_ButtonMusicType.getTestCoin);
		STQM_TipManager.getInstance().ShowTip(STQM_TipType.ApplyForExpCoin_Success, 0, string.Empty);
	}

	private void DoOverflow(object[] args)
	{
		Console.WriteLine("**********1*********DoOverflow**************************************");
		STQM_TipManager.getInstance().ShowTip(STQM_TipType.CoinOverFlow, 0, string.Empty);
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
			STQM_TipManager.getInstance().ShowTip(STQM_TipType.ServerUpdate, 0, string.Empty);
			break;
		case 2:
			STQM_TipManager.getInstance().ShowTip(STQM_TipType.Game_UserIdFrozen, 0, string.Empty);
			break;
		case 3:
			STQM_TipManager.getInstance().ShowTip(STQM_TipType.UserIdDeleted, 0, string.Empty);
			break;
		case 4:
			STQM_TipManager.getInstance().ShowTip(STQM_TipType.UserIdRepeative, 0, string.Empty);
			STQM_Sockets.GetSingleton().isReconnect = false;
			break;
		case 5:
			STQM_TipManager.getInstance().ShowTip(STQM_TipType.UserPwdChanged, 0, string.Empty);
			break;
		case 6:
			STQM_TipManager.getInstance().ShowTip(STQM_TipType.LoseTheServer, 0, string.Empty);
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
			STQM_TipManager.getInstance().ShowTip(STQM_TipType.TableDeleted, 0, string.Empty);
			break;
		case 2:
			STQM_TipManager.getInstance().ShowTip(STQM_TipType.TableConfigChanged, 0, string.Empty);
			break;
		default:
			STQM_TipManager.getInstance().ShowTip(STQM_TipType.LongTimeNoHandle, 0, string.Empty);
			break;
		}
	}

	private void DoQuitToDesk(object[] args)
	{
		STQM_TipManager.getInstance().ShowTip(STQM_TipType.QuitToDesk, 0, string.Empty);
	}

	private void HandleNetMsg_ApplyExchange(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		STQM_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (STQM_GameInfo.getInstance() != null && STQM_GameInfo.getInstance().UIScene != null)
		{
			STQM_GameInfo.getInstance().UIScene.UpdateUserInfo();
		}
		string ch = (!flag) ? "兑换被拒绝" : "兑换成功";
		string en = (!flag) ? "Failure of exchange" : "Successful exchange";
		STQM_TipManager.getInstance().ShowTip(STQM_TipType.Custom, 0, ZH2_GVars.ShowTip(ch, en, string.Empty));
	}

	private void HandleNetMsg_ApplyPay(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		STQM_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (STQM_GameInfo.getInstance() != null && STQM_GameInfo.getInstance().UIScene != null)
		{
			STQM_GameInfo.getInstance().UIScene.UpdateUserInfo();
		}
		string ch = (!flag) ? "充值被拒绝" : "充值成功";
		string en = (!flag) ? "Failure of TopUp" : "Successful TopUp";
		STQM_TipManager.getInstance().ShowTip(STQM_TipType.Custom, 0, ZH2_GVars.ShowTip(ch, en, string.Empty));
	}

	private void HandleNetMsg_changeScoreNotice(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		STQM_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (STQM_GameInfo.getInstance() != null && STQM_GameInfo.getInstance().UIScene != null)
		{
			STQM_GameInfo.getInstance().UIScene.UpdateUserInfo();
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
		STQM_TipManager.getInstance().ShowTip(STQM_TipType.Custom, 0, ZH2_GVars.ShowTip($"{arg}成功! 金币{num}", $"Successful {arg2}! GoldCoin {num}", string.Empty));
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
		STQM_FishPoolMngr.GetSingleton().LockFish(num, num2, flag);
	}

	private void DoUnLockFish(int[] args)
	{
		Console.WriteLine("**********1*********DoUnLockFish**************************************");
		int num = args[0];
		Console.WriteLine("fishId: " + num);
		int num2 = args[1];
		Console.WriteLine("seatId: " + num2);
		Console.WriteLine("**********2*********DoUnLockFish**************************************");
		STQM_FishPoolMngr.GetSingleton().UnLockFish(num, num2);
	}

	private void DoUnLockScreen(object[] args)
	{
		Console.WriteLine("**********1*********unLockScreen**************************************");
		STQM_FishPoolMngr.GetSingleton().UnFixAllFish();
		Console.WriteLine("**********1*********unLockScreen**************************************");
	}
}
