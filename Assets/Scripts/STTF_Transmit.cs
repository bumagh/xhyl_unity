using GameCommon;
using GameConfig;
using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STTF_Transmit : MonoBehaviour
{
	private static STTF_Transmit _MyTransmit;

	private STTF_Sockets m_CreateSocket;

	public static STTF_Transmit GetSingleton()
	{
		return _MyTransmit;
	}

	private void Awake()
	{
		if (_MyTransmit == null)
		{
			UnityEngine.Debug.Log("_MyTransmit");
			_MyTransmit = this;
		}
	}

	public void TransmitGetPoint(STTF_Sockets MyCreateSocket)
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
					UnityEngine.Debug.Log(text + "  " + empty);
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.Log("打印报错：" + arg);
			}
		}
		if (text == "sendServerTime")
		{
			if (STTF_GameInfo.getInstance().Key != string.Empty)
			{
				STTF_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(STTF_GameInfo.getInstance().UserId, STTF_GameInfo.getInstance().Pwd, (int)((STTF_GameInfo.getInstance().currentState < STTF_GameState.On_SelectRoom) ? STTF_GameState.On_SelectRoom : STTF_GameInfo.getInstance().currentState), string.Empty);
			}
			STTF_GameInfo.getInstance().Key = "sendServerTime";
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
		Console.WriteLine();
		if (text == "sendServerTime")
		{
			if (STTF_GameInfo.getInstance().Key != string.Empty)
			{
				STTF_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(STTF_GameInfo.getInstance().UserId, STTF_GameInfo.getInstance().Pwd, (int)STTF_GameInfo.getInstance().currentState, string.Empty);
			}
			STTF_GameInfo.getInstance().Key = "sendServerTime";
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
		if (STTF_Sockets.GetSingleton().isReconnect)
		{
			if (m_CreateSocket.GetRelineCount() < 5)
			{
				m_CreateSocket.CreateReceiveThread();
				return;
			}
			Console.WriteLine("30秒重连失败，网络断开，请重新登录网络大厅...");
			STTF_TipManager.getInstance().ShowTip(STTF_TipType.Net_ConnectionError, 0, string.Empty);
		}
	}

	private void DoCheckLogin(object[] args)
	{
		STTF_User sTTF_User = new STTF_User();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["isLogin"];
		int num = (int)dictionary["messageStatus"];
		if (flag)
		{
			bool isGameShuUp = (bool)dictionary["isShutup"];
			bool isSpecial = (bool)dictionary["special"];
			ZH2_GVars.hallInfo = new Dictionary<string, object>();
			ZH2_GVars.hallInfo = (dictionary["hallInfo"] as Dictionary<string, object>);
			Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
			sTTF_User.id = (int)dictionary2["id"];
			ZH2_GVars.userId = sTTF_User.id;
			sTTF_User.username = (string)dictionary2["username"];
			sTTF_User.nickname = (string)dictionary2["nickname"];
			sTTF_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
			sTTF_User.level = (int)dictionary2["level"];
			sTTF_User.gameGold = (int)dictionary2["gameGold"];
			sTTF_User.expeGold = (int)dictionary2["expeGold"];
			sTTF_User.photoId = (int)dictionary2["photoId"];
			sTTF_User.overflow = (int)dictionary2["overflow"];
			sTTF_User.gameScore = (int)dictionary2["gameScore"];
			sTTF_User.expeScore = (int)dictionary2["expeScore"];
			sTTF_User.type = (int)dictionary2["type"];
			if (sTTF_User.overflow == 1)
			{
				STTF_TipManager.getInstance().ShowTip(STTF_TipType.CoinOverFlow, 0, string.Empty);
			}
			else
			{
				STTF_GameInfo.getInstance().IsSpecial = isSpecial;
				STTF_GameInfo.getInstance().createUser(sTTF_User);
				STTF_GameInfo.getInstance().IsGameShuUp = isGameShuUp;
			}
		}
		else
		{
			switch (num)
			{
			case 0:
				STTF_TipManager.getInstance().ShowTip(STTF_TipType.UserIdDeleted, 0, string.Empty);
				break;
			case 1:
				STTF_TipManager.getInstance().ShowTip(STTF_TipType.Game_UserIdFrozen, 0, string.Empty);
				break;
			default:
				STTF_TipManager.getInstance().ShowTip(STTF_TipType.ServerUpdate, 0, string.Empty);
				break;
			}
		}
		Console.WriteLine("**********2*********DoCheckLogin**************************************");
	}

	private void DoCheckVersion(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			UnityEngine.Debug.Log($"Key：{item.Key},Value：{item.Value.ToString()}");
		}
		string text = (string)args[0];
		Console.WriteLine("haveNewVersionIDFlag: " + text);
		STTF_IOSGameStart.GetSingleton().UpdateGameVesion(text);
		Console.WriteLine("**********1*********DoCheckVersion**************************************");
	}

	private void DoNotUpdate(object[] args)
	{
		UnityEngine.Debug.Log("DoNotUpdate");
		STTF_NetMngr.GetSingleton().MyCreateSocket.SendPublicKey();
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
			STTF_FishDesk[] array = new STTF_FishDesk[length];
			int[] array2 = new int[length];
			for (int i = 0; i < length; i++)
			{
				array2[i] = 0;
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["deskInfo"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new STTF_FishDesk();
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
					array[i].seats[j] = new STTF_Seat();
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
			STTF_GameInfo.getInstance().updateTableList(array);
			STTF_GameInfo.getInstance().SetNoHall(isHavHall: true);
			for (int k = 0; k < length; k++)
			{
				STTF_GameInfo.getInstance().updateTableUserNumber(array[k].id, array2[k]);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("没有房间");
			STTF_GameInfo.getInstance().SetNoHall(isHavHall: false);
			STTF_GameInfo.getInstance().updateTableList(null);
		}
	}

	private STTF_FishDesk[] Test(STTF_FishDesk[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = 0; j < array.Length - 1; j++)
			{
				if (array[j].id > array[j + 1].id)
				{
					STTF_FishDesk sTTF_FishDesk = array[j];
					array[j] = array[j + 1];
					array[j + 1] = sTTF_FishDesk;
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
			if (STTF_GameInfo.getInstance() != null && STTF_GameInfo.getInstance().UIScene != null)
			{
				for (int i = 0; i < dictionary2.Count; i++)
				{
					for (int j = 0; j < STTF_GameInfo.getInstance().UIScene.selectBtnList.Count; j++)
					{
						if (i == STTF_GameInfo.getInstance().UIScene.selectBtnList[j].hallId)
						{
							STTF_GameInfo.getInstance().UIScene.selectBtnList[j].onlinePeople = dictionary2[i.ToString()].ToString();
							STTF_GameInfo.getInstance().UIScene.selectBtnList[j].UpdateText();
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
			STTF_FishDesk[] array = new STTF_FishDesk[length];
			int[] array2 = new int[length];
			for (int i = 0; i < length; i++)
			{
				array2[i] = 0;
				int num = 0;
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["deskInfo"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new STTF_FishDesk();
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
					array[i].seats[j] = new STTF_Seat();
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
					STTF_GameInfo.getInstance().updateTableUserNumber(j, num);
				}
			}
			array = Test(array);
			STTF_GameInfo.getInstance().updateTableList(array);
			STTF_GameInfo.getInstance().SetNoHall(isHavHall: true);
			for (int k = 0; k < length; k++)
			{
				STTF_GameInfo.getInstance().updateTableUserNumber(array[k].id, array2[k]);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("没有房间");
			STTF_GameInfo.getInstance().SetNoHall(isHavHall: false);
			STTF_GameInfo.getInstance().updateTableList(null);
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
		STTF_GameInfo.getInstance().updateTableUserNumber(num, num2);
	}

	private void DoRequestSeat(object[] args)
	{
		Console.WriteLine("**********1*********DoRequestSeat**************************************");
		int num = 0;
		int mSceneBg = 0;
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
			mSceneBg = (int)dictionary["bgId"];
			int seatIndex = (int)dictionary["seatId"];
			STTF_GameInfo.getInstance().User.SeatIndex = seatIndex;
		}
		if (!STTF_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			STTF_NetMngr.GetSingleton().mSceneBg = mSceneBg;
		}
		else
		{
			STTF_SceneBgMngr.GetSingleton().SetScene();
		}
		if (flag)
		{
			STTF_GameInfo.getInstance().UIScene.EnterGame();
			return;
		}
		switch (num)
		{
		case 0:
			STTF_TipManager.getInstance().ShowTip(STTF_TipType.TableDeleted, 0, string.Empty);
			break;
		case 1:
			if (STTF_GameInfo.getInstance().User.RoomId == 1)
			{
				STTF_TipManager.getInstance().ShowTip(STTF_TipType.SelectTable_CreditBelowRistrict, 0, string.Empty);
			}
			else
			{
				STTF_TipManager.getInstance().ShowTip(STTF_TipType.SelectTable_SendExpCoin, 0, string.Empty);
			}
			break;
		case 2:
			STTF_TipManager.getInstance().ShowTip(STTF_TipType.SelectSeat_NotEmpty, 0, string.Empty);
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
		STTF_Seat[] array = new STTF_Seat[length];
		int num2 = 0;
		for (int i = 0; i < length; i++)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2 = ((dictionary["Seat"] as Array).GetValue(i) as Dictionary<string, object>);
			array[i] = new STTF_Seat();
			array[i].seatId = (int)dictionary2["id"];
			array[i].isFree = (bool)dictionary2["isFree"];
			array[i].gunValue = (int)dictionary2["gunValue"];
			if (!array[i].isFree)
			{
				num2++;
				Dictionary<string, object> dictionary3 = dictionary2["user"] as Dictionary<string, object>;
				array[i].user = new STTF_User();
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
		STTF_GameInfo.getInstance().updateOtherUsers(array, num);
		STTF_GameInfo.getInstance().updateTableUserNumber(num, num2);
	}

	private void DoGameScore(int[] args)
	{
		Console.WriteLine("**********1*********DoGameScore**************************************");
		int num = args[0];
		Console.WriteLine("gameScore: " + num);
		Console.WriteLine("**********2*********DoGameScore**************************************");
		STTF_GameInfo.getInstance().updateUser("gameScore", num);
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
		if (STTF_NetMngr.GetSingleton().IsGameSceneLoadOk && !STTF_GameParameter.G_bTest && num3 != STTF_GameMngr.GetSingleton().mPlayerSeatID)
		{
			STTF_BulletPoolMngr.GetSingleton().LanchBullet(num3, (float)num2, num4, isLizi);
		}
		if (STTF_GameInfo.getInstance().currentState == STTF_GameState.On_Game)
		{
			STTF_GameInfo.getInstance().GameScene.UpdateWhenFired(num3, newScore, num4, (float)num2);
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
		if (STTF_GameInfo.getInstance().currentState == STTF_GameState.On_Game)
		{
			STTF_GameInfo.getInstance().GameScene.UpdateWhenDead(num3, newScore, gunScore, num2);
		}
		if (flag && num3 == STTF_GameMngr.GetSingleton().mPlayerSeatID && STTF_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			STTF_BulletPoolMngr.GetSingleton().SetLizi();
		}
		string text = (string)args[7];
		string[] array = text.Split('|');
		STTF_HitFish[] array2 = new STTF_HitFish[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string[] array3 = array[i].Split('#');
			array2[i] = new STTF_HitFish();
			array2[i].fishid = int.Parse(array3[0]);
			array2[i].fishtype = int.Parse(array3[1]);
			array2[i].fx = Convert.ToDouble(array3[2]);
			array2[i].fy = Convert.ToDouble(array3[3]);
			array2[i].bet = int.Parse(array3[4]);
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (!STTF_GameParameter.G_bTest && STTF_NetMngr.GetSingleton().IsGameSceneLoadOk && STTF_FishPoolMngr.GetSingleton() != null && STTF_EffectMngr.GetSingleton() != null)
			{
				Vector3 pos = Vector3.right * (float)array2[j].fx + Vector3.up * (float)array2[j].fy;
				STTF_FishPoolMngr.GetSingleton().SetFishDie(array2[j].fishid, num2, array2[j].fishtype, array2[j].bet, num3, pos);
				if (flag && j == 0)
				{
					STTF_EffectMngr.GetSingleton().PlayLiziCardFly(num3, pos);
				}
			}
		}
		if (!flag2)
		{
			return;
		}
		string text2 = (string)args[8];
		UnityEngine.Debug.Log("strHitFish2：" + text2);
		if (!(text2 != string.Empty) || text2.Length < 1)
		{
			return;
		}
		string[] array4 = text2.Split('|');
		STTF_HitFish[] array5 = new STTF_HitFish[array4.Length];
		for (int k = 0; k < array4.Length; k++)
		{
			string[] array6 = array4[k].Split('#');
			array5[k] = new STTF_HitFish();
			array5[k].fishid = int.Parse(array6[0]);
			array5[k].fishtype = int.Parse(array6[1]);
			array5[k].fx = Convert.ToDouble(array6[2]);
			array5[k].fy = Convert.ToDouble(array6[3]);
			array5[k].bet = int.Parse(array6[4]);
		}
		for (int l = 0; l < array4.Length; l++)
		{
			if (!STTF_GameParameter.G_bTest && STTF_FishPoolMngr.GetSingleton() != null && STTF_EffectMngr.GetSingleton() != null)
			{
				Vector3 pos2 = Vector3.right * (float)array5[l].fx + Vector3.up * (float)array5[l].fy;
				STTF_FishPoolMngr.GetSingleton().SetFishDie(array5[l].fishid, num2, array5[l].fishtype, array5[l].bet, num3, pos2);
			}
		}
	}

	private void DoNewFishGroup(int[] args)
	{
		Console.WriteLine("**********1*********DoNewfishGroup**************************************");
		int num = args[0];
		UnityEngine.Debug.Log("fishGroupType: " + num);
		Console.WriteLine("**********2********DoNewfishGroup**************************************");
		STTF_SceneBgMngr.GetSingleton().BigFishFormatReset();
		STTF_Formation.GetSingleton().ShowFormation((STTF_FORMATION)num);
		if (STTF_GameInfo.getInstance().currentState == STTF_GameState.On_Game)
		{
			STTF_GameInfo.getInstance().GameScene.HideTip();
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
		STTF_FISH_TYPE[] array2 = null;
		if (num > 0)
		{
			array2 = new STTF_FISH_TYPE[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = (STTF_FISH_TYPE)array[i];
			}
		}
		int num8 = num4;
		if (num8 <= 0)
		{
			return;
		}
		STTF_FishPathType[] array3 = new STTF_FishPathType[num8];
		for (int j = 0; j < num8; j++)
		{
			array3[j] = new STTF_FishPathType();
			array3[j].fishId = num2 + j;
			array3[j].newFishPathType = newFishPathType;
		}
		if (num3 != 40 && STTF_NetMngr.GetSingleton().IsGameSceneLoadOk && STTF_FishPoolMngr.GetSingleton() != null && !STTF_GameParameter.G_bTest)
		{
			if (num3 == 32 && array2 != null)
			{
				STTF_FishPoolMngr.GetSingleton().CreateCoralReefsFish(array2[0], array3[0].newFishPathType, array3[0].fishId);
			}
			else
			{
				STTF_FishPoolMngr.GetSingleton().CreateFish((STTF_FISH_TYPE)num3, array3[0].newFishPathType, array3[0].fishId, num4);
			}
			if (STTF_GameInfo.getInstance().CountTime)
			{
				STTF_FishPoolMngr.GetSingleton().UnFixAllFish();
			}
		}
		if (STTF_GameInfo.getInstance().currentState == STTF_GameState.On_Game)
		{
			STTF_GameInfo.getInstance().GameScene.HideTip();
		}
	}

	private void DoClearFish(object[] args)
	{
		Console.WriteLine("**********1*********DoClearfish**************************************");
		if (STTF_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			STTF_SceneBgMngr.GetSingleton().ChangeScene();
		}
		Console.WriteLine("**********2*********DoClearfish**************************************");
	}

	private void DoForbitFired(object[] args)
	{
		Console.WriteLine("**********1*********DoForbitFired**************************************");
		Console.WriteLine("**********2*********DoForbitFired**************************************");
		if (STTF_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			STTF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = false;
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
		if (STTF_GameInfo.getInstance().currentState == STTF_GameState.On_Game)
		{
			STTF_GameInfo.getInstance().GameScene.AddNotice(text);
		}
	}

	private void DoPlayerInfo(object[] args)
	{
		Console.WriteLine("**********1*********DoPlayerInfo**************************************");
		STTF_User sTTF_User = new STTF_User();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["seatId"];
		int num2 = (int)dictionary["honor"];
		Console.WriteLine("seatid: " + num);
		Console.WriteLine("honor: " + num2);
		Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
		sTTF_User.id = (int)dictionary2["id"];
		sTTF_User.username = (string)dictionary2["username"];
		sTTF_User.nickname = (string)dictionary2["nickname"];
		sTTF_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
		sTTF_User.level = (int)dictionary2["level"];
		sTTF_User.gameGold = (int)dictionary2["gameGold"];
		sTTF_User.expeGold = (int)dictionary2["expeGold"];
		sTTF_User.photoId = (int)dictionary2["photoId"];
		sTTF_User.overflow = (int)dictionary2["overflow"];
		sTTF_User.gameScore = (int)dictionary2["gameScore"];
		sTTF_User.expeScore = (int)dictionary2["expeScore"];
		sTTF_User.type = (int)dictionary2["type"];
		Console.WriteLine("user.id: " + sTTF_User.id);
		Console.WriteLine("user.username: " + sTTF_User.username);
		Console.WriteLine("user.nickname: " + sTTF_User.nickname);
		Console.WriteLine("user.sex: " + sTTF_User.sex);
		Console.WriteLine("user.level: " + sTTF_User.level);
		Console.WriteLine("user.gameGold: " + sTTF_User.gameGold);
		Console.WriteLine("user.expeGold: " + sTTF_User.expeGold);
		Console.WriteLine("user.photoId: " + sTTF_User.photoId);
		Console.WriteLine("user.overflow: " + sTTF_User.overflow);
		Console.WriteLine("user.gameScore: " + sTTF_User.gameScore);
		Console.WriteLine("user.expeScore: " + sTTF_User.expeScore);
		Console.WriteLine();
		STTF_GameInfo.getInstance().getPersonInfo(new STTF_UserInfo(sTTF_User), num2);
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
		if (STTF_GameInfo.getInstance().currentState != STTF_GameState.On_Game)
		{
		}
	}

	private void DoGameShutup(bool[] args)
	{
		Console.WriteLine("**********1*********DoGameShutup**************************************");
		bool flag = args[0];
		Console.WriteLine("forbidFlag: " + flag);
		Console.WriteLine("**********2*********DoGameShutup**************************************");
		STTF_GameInfo.getInstance().IsGameShuUp = flag;
	}

	private void DoUserShutup(bool[] args)
	{
		Console.WriteLine("**********1*********DoUserShutup**************************************");
		bool flag = args[0];
		Console.WriteLine("forbidFlag: " + flag);
		Console.WriteLine("**********2*********DoUserShutup**************************************");
		STTF_GameInfo.getInstance().IsUserShutUp = flag;
	}

	private void DoUserAward(int[] args)
	{
		Console.WriteLine("**********1*********DoUserAward**************************************");
		int num = args[0];
		Console.WriteLine("gold: " + num);
		Console.WriteLine("**********2*********DoUserAward**************************************");
		STTF_GameInfo.getInstance().updateUser("gameCoin", num);
		STTF_TipManager.getInstance().ShowTip(STTF_TipType.GivingCoin, num, string.Empty);
	}

	private void DoGameGold(int[] args)
	{
		Console.WriteLine("**********1*********DoGameGold**************************************");
		int num = args[0];
		Console.WriteLine("gameGold: " + num);
		Console.WriteLine("**********2*********DoGameGold**************************************");
		STTF_GameInfo.getInstance().updateUser("gameCoin", num);
	}

	private void DoExpeGold(int[] args)
	{
		Console.WriteLine("**********1*********DoExpeGold**************************************");
		int num = args[0];
		Console.WriteLine("expeGold: " + num);
		Console.WriteLine("**********2*********DoExpeGold**************************************");
		STTF_GameInfo.getInstance().updateUser("testCoin", num);
	}

	private void DoAddExpeGoldAuto(bool[] args)
	{
		Console.WriteLine("**********1*********DoAddExpeGoldAuto**************************************");
		bool flag = args[0];
		Console.WriteLine("bOk: " + flag);
		Console.WriteLine("**********2*********DoAddExpeGoldAuto**************************************");
		STTF_SoundManage.getInstance().playButtonMusic(STTF_ButtonMusicType.getTestCoin);
		STTF_TipManager.getInstance().ShowTip(STTF_TipType.ApplyForExpCoin_Success, 0, string.Empty);
	}

	private void DoOverflow(object[] args)
	{
		Console.WriteLine("**********1*********DoOverflow**************************************");
		STTF_TipManager.getInstance().ShowTip(STTF_TipType.CoinOverFlow, 0, string.Empty);
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
			STTF_TipManager.getInstance().ShowTip(STTF_TipType.ServerUpdate, 0, string.Empty);
			break;
		case 2:
			STTF_TipManager.getInstance().ShowTip(STTF_TipType.Game_UserIdFrozen, 0, string.Empty);
			break;
		case 3:
			STTF_TipManager.getInstance().ShowTip(STTF_TipType.UserIdDeleted, 0, string.Empty);
			break;
		case 4:
			STTF_TipManager.getInstance().ShowTip(STTF_TipType.UserIdRepeative, 0, string.Empty);
			STTF_Sockets.GetSingleton().isReconnect = false;
			break;
		case 5:
			STTF_TipManager.getInstance().ShowTip(STTF_TipType.UserPwdChanged, 0, string.Empty);
			break;
		case 6:
			STTF_TipManager.getInstance().ShowTip(STTF_TipType.LoseTheServer, 0, string.Empty);
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
			STTF_TipManager.getInstance().ShowTip(STTF_TipType.TableDeleted, 0, string.Empty);
			break;
		case 2:
			STTF_TipManager.getInstance().ShowTip(STTF_TipType.TableConfigChanged, 0, string.Empty);
			break;
		default:
			STTF_TipManager.getInstance().ShowTip(STTF_TipType.LongTimeNoHandle, 0, string.Empty);
			break;
		}
	}

	private void DoQuitToDesk(object[] args)
	{
		STTF_TipManager.getInstance().ShowTip(STTF_TipType.QuitToDesk, 0, string.Empty);
	}

	private void HandleNetMsg_ApplyExchange(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		STTF_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (STTF_GameInfo.getInstance() != null && STTF_GameInfo.getInstance().UIScene != null)
		{
			STTF_GameInfo.getInstance().UIScene.UpdateUserInfo();
		}
		string ch = (!flag) ? "兑换被拒绝" : "兑换成功";
		string en = (!flag) ? "Failure of exchange" : "Successful exchange";
		STTF_TipManager.getInstance().ShowTip(STTF_TipType.Custom, 0, ZH2_GVars.ShowTip(ch, en, string.Empty));
	}

	private void HandleNetMsg_ApplyPay(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		STTF_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (STTF_GameInfo.getInstance() != null && STTF_GameInfo.getInstance().UIScene != null)
		{
			STTF_GameInfo.getInstance().UIScene.UpdateUserInfo();
		}
		string ch = (!flag) ? "充值被拒绝" : "充值成功";
		string en = (!flag) ? "Failure of TopUp" : "Successful TopUp";
		STTF_TipManager.getInstance().ShowTip(STTF_TipType.Custom, 0, ZH2_GVars.ShowTip(ch, en, string.Empty));
	}

	private void HandleNetMsg_changeScoreNotice(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		STTF_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (STTF_GameInfo.getInstance() != null && STTF_GameInfo.getInstance().UIScene != null)
		{
			STTF_GameInfo.getInstance().UIScene.UpdateUserInfo();
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
		STTF_TipManager.getInstance().ShowTip(STTF_TipType.Custom, 0, ZH2_GVars.ShowTip($"{arg}成功! 金币{num}", $"Successful {arg2}! GoldCoin {num}", string.Empty));
	}

	private void DoLockFish(ArrayList args)
	{
		bool locking = (bool)args[0];
		int num = (int)args[1];
		Console.WriteLine("fishId: " + num);
		int num2 = (int)args[2];
		Console.WriteLine("seatId: " + num2);
		STTF_FishPoolMngr.GetSingleton().LockFish(num, num2, locking);
	}

	private void DoUnLockFish(int[] args)
	{
		Console.WriteLine("**********1*********DoUnLockFish**************************************");
		int num = args[0];
		Console.WriteLine("fishId: " + num);
		int num2 = args[1];
		Console.WriteLine("seatId: " + num2);
		Console.WriteLine("**********2*********DoUnLockFish**************************************");
		STTF_FishPoolMngr.GetSingleton().UnLockFish(num, num2);
	}

	private void DoUnLockScreen(object[] args)
	{
		Console.WriteLine("**********1*********unLockScreen**************************************");
		STTF_FishPoolMngr.GetSingleton().UnFixAllFish();
		Console.WriteLine("**********1*********unLockScreen**************************************");
	}
}
