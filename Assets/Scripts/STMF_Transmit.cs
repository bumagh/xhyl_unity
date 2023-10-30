using GameCommon;
using GameConfig;
using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class STMF_Transmit : MonoBehaviour
{
	private static STMF_Transmit _MyTransmit;

	private STMF_Sockets m_CreateSocket;

	private int tempNum;

	public static STMF_Transmit GetSingleton()
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

	public void TransmitGetPoint(STMF_Sockets MyCreateSocket)
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
				if (text != "newFish" && text != "gameScore" && text != "scrollMessage" && text != "roomInfo" && text != "userShutup" && text != "fired" && text != "lockFish" && text != "newfish" && text != "gunHitfish")
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
			if (STMF_GameInfo.getInstance().Key != string.Empty)
			{
				STMF_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(STMF_GameInfo.getInstance().UserId, STMF_GameInfo.getInstance().Pwd, (int)((STMF_GameInfo.getInstance().currentState >= STMF_GameState.On_SelectRoom) ? STMF_GameInfo.getInstance().currentState : STMF_GameState.On_Loading), string.Empty);
			}
			STMF_GameInfo.getInstance().Key = "sendServerTime";
		}
		else if (text == "checkVersion")
		{
			DoCheckVersion(array);
		}
		else if (text == "notUpdate")
		{
			DoNotUpdate(array);
		}
		else if (text == "checklogin")
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
		else if (text == "enterDesk")
		{
			DoEnterDesk(array);
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
		else if (text == "notfired")
		{
			DoNotfired(array);
		}
		else if (text == "fired")
		{
			DoFired(array);
		}
		else if (text == "gunHitfish")
		{
			DoGunHitfish(table["args"] as string[]);
		}
		else if (text == "newfishGroup")
		{
			DoNewfishGroup(table["args"] as int[]);
		}
		else if (text == "newfish")
		{
			DoNewfish(table["args"] as string[]);
		}
		else if (text == "forbitFired")
		{
			DoForbitFired(array);
		}
		else if (text == "clearfish")
		{
			DoClearfish(table["args"] as int[], isBackGround: false);
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
			else if (text == "quitToRoom")
			{
				DoQuitToRoom(table["args"] as int[]);
			}
			else if (text == "NetThread/NetDown")
			{
				DoNetDown(array);
			}
			else
			{
				Console.WriteLine("recv No Message Type!");
			}
		}
	}

	public void SelectPostMsgControl(Hashtable table)
	{
		string a = table["method"].ToString();
		object[] args = table["args"] as object[];
		Console.WriteLine();
		if (a == "sendServerTime")
		{
			if (STMF_GameInfo.getInstance().Key != string.Empty)
			{
				STMF_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(STMF_GameInfo.getInstance().UserId, STMF_GameInfo.getInstance().Pwd, (int)STMF_GameInfo.getInstance().currentState, string.Empty);
			}
			STMF_GameInfo.getInstance().Key = "sendServerTime";
		}
		else if (a == "checkVersion")
		{
			DoCheckVersion(args);
		}
		else if (a == "notUpdate")
		{
			DoNotUpdate(args);
		}
		else if (a == "checklogin")
		{
			DoCheckLogin(args);
		}
		else if (a == "roomInfo")
		{
			DoRoomInfo(args);
		}
		else if (a == "enterDesk")
		{
			DoEnterDesk(args);
		}
		else if (a == "updateRoomInfo")
		{
			DoUpdateRoomInfo(args);
		}
		else if (a == "updateHallInfo")
		{
			DoUpdateHallInfo(args);
		}
		else if (a == "deskOnlineNumber")
		{
			DoDeskOnlineNumber(args);
		}
		else if (a == "requestSeat")
		{
			DoRequestSeat(args);
		}
		else if (a == "updateDeskInfo")
		{
			DoUpdateDeskInfo(args);
		}
		else if (a == "gameScore")
		{
			DoGameScore(table["args"] as int[]);
		}
		else
		{
			if (a == "notfired" || a == "fired" || a == "gunHitfish" || a == "newfishGroup" || a == "newfish" || a == "forbitFired")
			{
				return;
			}
			if (a == "clearfish")
			{
				DoClearfish(table["args"] as int[], isBackGround: true);
			}
			else if (a == "scrollMessage")
			{
				DoScrollMessage(table["args"] as string[]);
			}
			else if (a == "playerInfo")
			{
				DoPlayerInfo(args);
			}
			else if (a == "sendChat")
			{
				DoSendChat(args);
			}
			else if (a == "gameShutup")
			{
				DoGameShutup(table["args"] as bool[]);
			}
			else if (a == "userShutup")
			{
				DoUserShutup(table["args"] as bool[]);
			}
			else if (a == "userAward")
			{
				DoUserAward(table["args"] as int[]);
			}
			else if (!(a == "heart"))
			{
				if (a == "gameGold")
				{
					DoGameGold(table["args"] as int[]);
				}
				else if (a == "expeGold")
				{
					DoExpeGold(table["args"] as int[]);
				}
				else if (a == "addExpeGoldAuto")
				{
					DoAddExpeGoldAuto(table["args"] as bool[]);
				}
				else if (a == "overflow")
				{
					DoOverflow(args);
				}
				else if (a == "quitToLogin")
				{
					DoQuitToLogin(table["args"] as int[]);
				}
				else if (a == "quitToRoom")
				{
					DoQuitToRoom(table["args"] as int[]);
				}
				else if (a == "NetThread/NetDown")
				{
					DoNetDown(args);
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
		if (STMF_Sockets.GetSingleton().isReconnect)
		{
			if (m_CreateSocket.GetRelineCount() < 5)
			{
				m_CreateSocket.CreateReceiveThread();
				return;
			}
			Console.WriteLine("30秒重连失败，网络断开，请重新登录网络大厅...");
			STMF_TipManager.getInstance().ShowTip(STMF_TipType.Net_ConnectionError, 0, string.Empty);
		}
	}

	private void DoCheckLogin(object[] args)
	{
		Console.WriteLine("**********1*********DoCheckLogin**************************************");
		STMF_User sTMF_User = new STMF_User();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["isLogin"];
		Console.WriteLine("isLogin: " + flag);
		int num = (int)dictionary["messageStatus"];
		Console.WriteLine("messageStatus: " + num);

        JsonData jsonData = new JsonData();
        jsonData = JsonMapper.ToObject(JsonMapper.ToJson(dictionary));
        Debug.LogError("获取DoCheckLogin:  " + jsonData.ToJson());


        if (flag)
		{
			bool flag2 = (bool)dictionary["isShutup"];
			bool flag3 = (bool)dictionary["special"];
			Console.WriteLine("isShutup: " + flag2);
			Console.WriteLine("special: " + flag3);
			ZH2_GVars.hallInfo = new Dictionary<string, object>();
			ZH2_GVars.hallInfo = (dictionary["hallInfo"] as Dictionary<string, object>);
			Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
			sTMF_User.id = (int)dictionary2["id"];
			ZH2_GVars.userId = sTMF_User.id;
			sTMF_User.username = (string)dictionary2["username"];
			sTMF_User.nickname = (string)dictionary2["nickname"];
			sTMF_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
			sTMF_User.level = (int)dictionary2["level"];
			sTMF_User.gameGold = (int)dictionary2["gameGold"];
			sTMF_User.expeGold = (int)dictionary2["expeGold"];
			sTMF_User.photoId = (int)dictionary2["photoId"];
			sTMF_User.overflow = (int)dictionary2["overflow"];
			sTMF_User.gameScore = (int)dictionary2["gameScore"];
			sTMF_User.expeScore = (int)dictionary2["expeScore"];
			sTMF_User.shutupStatus = (int)dictionary2["shutupStatus"];
			Console.WriteLine("user.id: " + sTMF_User.id);
			Console.WriteLine("user.username: " + sTMF_User.username);
			Console.WriteLine("user.nickname: " + sTMF_User.nickname);
			Console.WriteLine("user.sex: " + sTMF_User.sex);
			Console.WriteLine("user.level: " + sTMF_User.level);
			Console.WriteLine("user.gameGold: " + sTMF_User.gameGold);
			Console.WriteLine("user.expeGold: " + sTMF_User.expeGold);
			Console.WriteLine("user.photoId: " + sTMF_User.photoId);
			Console.WriteLine("user.overflow: " + sTMF_User.overflow);
			Console.WriteLine("user.gameScore: " + sTMF_User.gameScore);
			Console.WriteLine("user.expeScore: " + sTMF_User.expeScore);
			Console.WriteLine("user.shutupStatus: " + sTMF_User.shutupStatus);
			if (sTMF_User.overflow == 1)
			{
				STMF_TipManager.getInstance().ShowTip(STMF_TipType.CoinOverFlow, 0, string.Empty);
			}
			else
			{
				STMF_GameInfo.getInstance().IsSpecial = flag3;
				STMF_GameInfo.getInstance().createUser(sTMF_User);
				STMF_GameInfo.getInstance().IsGameShuUp = flag2;
				if (sTMF_User.shutupStatus == 0)
				{
					STMF_GameInfo.getInstance().IsUserShutUp = false;
				}
				else
				{
					STMF_GameInfo.getInstance().IsUserShutUp = true;
				}
			}
		}
		else
		{
			switch (num)
			{
			case 0:
				STMF_TipManager.getInstance().BackToHall();
				break;
			case 1:
				STMF_TipManager.getInstance().ShowTip(STMF_TipType.Game_UserIdFrozen, 0, string.Empty);
				break;
			default:
				STMF_TipManager.getInstance().ShowTip(STMF_TipType.ServerUpdate, 0, string.Empty);
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
		STMF_IOSGameStart.GetSingleton().UpdateGameVesion(text);
		Console.WriteLine("**********1*********DoCheckVersion**************************************");
	}

	private void DoNotUpdate(object[] args)
	{
		UnityEngine.Debug.Log("DoNotUpdate");
		STMF_NetMngr.GetSingleton().MyCreateSocket.SendPublicKey();
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
			STMF_FishDesk[] array = new STMF_FishDesk[length];
			int[] array2 = new int[length];
			for (int i = 0; i < length; i++)
			{
				array2[i] = 0;
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["deskInfo"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new STMF_FishDesk();
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
					array[i].seats[j] = new STMF_Seat();
					array[i].seats[j].seatId = (int)dictionary3["seatId"];
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
					}
				}
			}
			array = Test(array);
			STMF_GameInfo.getInstance().updateTableList(array);
			STMF_GameInfo.getInstance().SetNoHall(isHavHall: true);
			for (int k = 0; k < length; k++)
			{
				STMF_GameInfo.getInstance().updateTableUserNumber(array[k].id, array2[k]);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("没有房间");
			STMF_GameInfo.getInstance().SetNoHall(isHavHall: false);
			STMF_GameInfo.getInstance().updateTableList(null);
		}
	}

	private STMF_FishDesk[] Test(STMF_FishDesk[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = 0; j < array.Length - 1; j++)
			{
				if (array[j].id > array[j + 1].id)
				{
					STMF_FishDesk sTMF_FishDesk = array[j];
					array[j] = array[j + 1];
					array[j + 1] = sTMF_FishDesk;
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
			if (STMF_GameInfo.getInstance() != null && STMF_GameInfo.getInstance().UIScene != null)
			{
				for (int i = 0; i < dictionary2.Count; i++)
				{
					for (int j = 0; j < STMF_GameInfo.getInstance().UIScene.selectBtnList.Count; j++)
					{
						if (i == STMF_GameInfo.getInstance().UIScene.selectBtnList[j].hallId)
						{
							STMF_GameInfo.getInstance().UIScene.selectBtnList[j].onlinePeople = dictionary2[i.ToString()].ToString();
							STMF_GameInfo.getInstance().UIScene.selectBtnList[j].UpdateText();
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

	private void DoEnterDesk(object[] args)
	{
		int length = (args[0] as Array).Length;
		if (length <= 0)
		{
			return;
		}
		STMF_Seat[] array = new STMF_Seat[length];
		for (int i = 0; i < length; i++)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary = ((args[0] as Array).GetValue(i) as Dictionary<string, object>);
			array[i] = new STMF_Seat();
			array[i].seatId = (int)dictionary["seatId"];
			array[i].isFree = (bool)dictionary["isFree"];
			array[i].gunValue = (int)dictionary["gunValue"];
			if (!array[i].isFree)
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = (dictionary["user"] as Dictionary<string, object>);
				array[i].user = new STMF_User();
				array[i].user.id = (int)dictionary2["id"];
				array[i].user.username = (string)dictionary2["username"];
				array[i].user.nickname = (string)dictionary2["nickname"];
				array[i].user.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
				array[i].user.level = (int)dictionary2["level"];
				array[i].user.gameGold = (int)dictionary2["gameGold"];
				array[i].user.expeGold = (int)dictionary2["expeGold"];
				array[i].user.photoId = (int)dictionary2["photoId"];
				array[i].user.overflow = (int)dictionary2["overflow"];
				array[i].user.gameScore = (int)dictionary2["gameScore"];
				array[i].user.expeScore = (int)dictionary2["expeScore"];
			}
		}
		STMF_GameInfo.getInstance().updateOtherUsers(array, isEnter: true);
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
			STMF_FishDesk[] array = new STMF_FishDesk[length];
			int[] array2 = new int[length];
			for (int i = 0; i < length; i++)
			{
				array2[i] = 0;
				int num = 0;
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["deskInfo"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new STMF_FishDesk();
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
					array[i].seats[j] = new STMF_Seat();
					array[i].seats[j].seatId = (int)dictionary3["seatId"];
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
					STMF_GameInfo.getInstance().updateTableUserNumber(j, num);
				}
			}
			array = Test(array);
			STMF_GameInfo.getInstance().updateTableList(array);
			STMF_GameInfo.getInstance().SetNoHall(isHavHall: true);
			for (int k = 0; k < length; k++)
			{
				STMF_GameInfo.getInstance().updateTableUserNumber(array[k].id, array2[k]);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("没有房间");
			STMF_GameInfo.getInstance().SetNoHall(isHavHall: false);
			STMF_GameInfo.getInstance().updateTableList(null);
		}
	}

	private void DoDeskOnlineNumber(object[] args)
	{
	}

	private void DoRequestSeat(object[] args)
	{
		Console.WriteLine("**********1*********DoRequestSeat**************************************");
		int num = 0;
		int num2 = 0;
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = false;
		try
		{
			flag = (bool)dictionary["bCanSeat"];
		}
		catch (Exception)
		{
		}
		try
		{
			flag = (bool)dictionary["success"];
		}
		catch (Exception)
		{
		}
		Console.WriteLine("bCanSeat:" + flag);
		if (!flag)
		{
			num = (int)dictionary["messageStatus"];
			Console.WriteLine("messageStatus:" + num);
		}
		else
		{
			num2 = (int)dictionary["bgid"];
			try
			{
				int seatIndex = (int)dictionary["seatId"];
				STMF_GameInfo.getInstance().User.SeatIndex = seatIndex;
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
		}
		Console.WriteLine("**********2*********DoRequestSeat**************************************");
		if (!STMF_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			STMF_NetMngr.GetSingleton().mSceneBg = num2;
		}
		else
		{
			STMF_SceneBgMngr.GetSingleton().SetScene(num2);
		}
		if (flag)
		{
			STMF_GameInfo.getInstance().UIScene.EnterGame();
			return;
		}
		switch (num)
		{
		case 0:
			STMF_TipManager.getInstance().ShowTip(STMF_TipType.TableDeleted, 0, string.Empty);
			break;
		case 1:
			if (STMF_GameInfo.getInstance().User.RoomId == 1)
			{
				STMF_TipManager.getInstance().ShowTip(STMF_TipType.SelectTable_CreditBelowRistrict, 0, string.Empty);
			}
			else
			{
				STMF_TipManager.getInstance().ShowTip(STMF_TipType.SelectTable_SendExpCoin, 0, string.Empty);
			}
			break;
		case 2:
			STMF_TipManager.getInstance().ShowTip(STMF_TipType.SelectSeat_NotEmpty, 0, string.Empty);
			break;
		}
	}

	private void DoUpdateDeskInfo(object[] args)
	{
		Console.WriteLine("**********1*********DoUpdateDeskInfo**************************************");
		int length = (args[0] as Array).Length;
		if (length > 0)
		{
			STMF_Seat[] array = new STMF_Seat[length];
			for (int i = 0; i < length; i++)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary = ((args[0] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new STMF_Seat();
				array[i].seatId = (int)dictionary["seatId"];
				array[i].isFree = (bool)dictionary["isFree"];
				array[i].gunValue = (int)dictionary["gunValue"];
				if (!array[i].isFree)
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					dictionary2 = (dictionary["user"] as Dictionary<string, object>);
					array[i].user = new STMF_User();
					array[i].user.id = (int)dictionary2["id"];
					array[i].user.username = (string)dictionary2["username"];
					array[i].user.nickname = (string)dictionary2["nickname"];
					array[i].user.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
					array[i].user.level = (int)dictionary2["level"];
					array[i].user.gameGold = (int)dictionary2["gameGold"];
					array[i].user.expeGold = (int)dictionary2["expeGold"];
					array[i].user.photoId = (int)dictionary2["photoId"];
					array[i].user.overflow = (int)dictionary2["overflow"];
					array[i].user.gameScore = (int)dictionary2["gameScore"];
					array[i].user.expeScore = (int)dictionary2["expeScore"];
				}
			}
			STMF_GameInfo.getInstance().updateOtherUsers(array, isEnter: false);
		}
		Console.WriteLine("**********2*********DoUpdateDeskInfo**************************************");
	}

	private void DoGameScore(int[] args)
	{
		Console.WriteLine("**********1*********DoGameScore**************************************");
		int num = args[0];
		Console.WriteLine("gameScore: " + num);
		Console.WriteLine("**********2*********DoGameScore**************************************");
		STMF_GameInfo.getInstance().updateUser("gameScore", num);
	}

	private void DoNotfired(object[] args)
	{
		Console.WriteLine("**********1*********DoNotfired**************************************");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["gunid"];
		Console.WriteLine("gunid: " + num);
		int num2 = (int)dictionary["seatid"];
		Console.WriteLine("seatid: " + num2);
		int num3 = (int)dictionary["totalScore"];
		Console.WriteLine("totalScore: " + num3);
		Console.WriteLine("**********2********DoNotfired**************************************");
	}

	private void DoFired(object[] args)
	{
		Console.WriteLine("**********1*********DoFired**************************************");
		int num = int.Parse((string)args[0]);
		double num2 = Convert.ToDouble(args[1]);
		int num3 = int.Parse((string)args[2]);
		int num4 = int.Parse((string)args[3]);
		bool isLizi = Convert.ToBoolean(args[4]);
		int newScore = int.Parse((string)args[5]);
		if (STMF_NetMngr.GetSingleton().IsGameSceneLoadOk && !STMF_GameParameter.G_bTest && num3 != STMF_GameMngr.GetSingleton().mPlayerSeatID)
		{
			STMF_BulletPoolMngr.GetSingleton().LanchBullet(num3, (float)num2, num4, isLizi);
		}
		Console.WriteLine("**********2*********DoFired**************************************");
		if (STMF_GameInfo.getInstance().currentState == STMF_GameState.On_Game)
		{
			STMF_GameInfo.getInstance().GameScene.UpdateWhenFired(num3, newScore, num4, (float)num2);
		}
	}

	private void DoGunHitfish(string[] args)
	{
		int num = int.Parse(args[0]);
		Console.WriteLine("gunid: " + num);
		int num2 = int.Parse(args[1]);
		Console.WriteLine("gunValue: " + num2);
		int num3 = int.Parse(args[2]);
		Console.WriteLine("seatid: " + num3);
		bool flag = Convert.ToBoolean(args[3]);
		Console.WriteLine("bCreateX2Gun: " + flag);
		bool flag2 = Convert.ToBoolean(args[4]);
		Console.WriteLine("bDeadKnife: " + flag2);
		int num4 = int.Parse(args[5]);
		Console.WriteLine("fishDeadScore: " + num4);
		int num5 = int.Parse(args[6]);
		Console.WriteLine("totalScore: " + num5);
		if (STMF_GameInfo.getInstance().currentState == STMF_GameState.On_Game)
		{
			STMF_GameInfo.getInstance().GameScene.UpdateWhenDead(num3, num5, num4, num2);
		}
		if (flag && num3 == STMF_GameMngr.GetSingleton().mPlayerSeatID && STMF_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			STMF_BulletPoolMngr.GetSingleton().SetLizi();
		}
		string text = args[7];
		string[] array = text.Split('|');
		STMF_HitFish[] array2 = new STMF_HitFish[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string[] array3 = array[i].Split('#');
			array2[i] = new STMF_HitFish();
			array2[i].fishid = int.Parse(array3[0]);
			array2[i].fishtype = int.Parse(array3[1]);
			array2[i].fx = Convert.ToDouble(array3[2]);
			array2[i].fy = Convert.ToDouble(array3[3]);
			array2[i].bet = int.Parse(array3[4]);
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array2[j] == null)
			{
			}
			if (!STMF_GameParameter.G_bTest && STMF_NetMngr.GetSingleton().IsGameSceneLoadOk && STMF_FishPoolMngr.GetSingleton() != null && STMF_EffectMngr.GetSingleton() != null)
			{
				Vector3 pos = Vector3.right * (float)array2[j].fx + Vector3.up * (float)array2[j].fy;
				STMF_FishPoolMngr.GetSingleton().SetFishDie(array2[j].fishid, num2, array2[j].fishtype, array2[j].bet, num3, pos);
				if (flag && j == 0)
				{
					STMF_EffectMngr.GetSingleton().PlayLiziCardFly(num3, pos);
				}
			}
		}
		if (flag2)
		{
			string text2 = args[8];
			Console.WriteLine("strHitFish2：" + text2);
			string[] array4 = text2.Split('|');
			STMF_HitFish[] array5 = new STMF_HitFish[array4.Length];
			for (int k = 0; k < array4.Length; k++)
			{
				string[] array6 = array4[k].Split('#');
				array5[k] = new STMF_HitFish();
				array5[k].fishid = int.Parse(array6[0]);
				array5[k].fishtype = int.Parse(array6[1]);
				array5[k].fx = Convert.ToDouble(array6[2]);
				array5[k].fy = Convert.ToDouble(array6[3]);
				array5[k].bet = int.Parse(array6[4]);
			}
			for (int l = 0; l < array4.Length; l++)
			{
				Console.WriteLine("hitfish2  fishid:" + array5[l].fishid);
				Console.WriteLine("hitfish2  fishtype:" + array5[l].fishtype);
				Console.WriteLine("hitfish2  fx:" + array5[l].fx);
				Console.WriteLine("hitfish2  fy:" + array5[l].fy);
				Console.WriteLine("hitfish1  bet:" + array5[l].bet);
				if (!STMF_GameParameter.G_bTest && STMF_FishPoolMngr.GetSingleton() != null && STMF_EffectMngr.GetSingleton() != null)
				{
					Vector3 pos2 = Vector3.right * (float)array5[l].fx + Vector3.up * (float)array5[l].fy;
					STMF_FishPoolMngr.GetSingleton().SetFishDie(array5[l].fishid, num2, array5[l].fishtype, array5[l].bet, num3, pos2);
				}
			}
		}
		Console.WriteLine("**********2*********DoGunHitfish**************************************");
	}

	private void DoNewfishGroup(int[] args)
	{
		Console.WriteLine("**********1*********DoNewfishGroup**************************************");
		int num = args[0];
		Console.WriteLine("fishGroupType: " + num);
		Console.WriteLine("**********2********DoNewfishGroup**************************************");
		STMF_SceneBgMngr.GetSingleton().BigFishFormatReset();
		STMF_Formation.GetSingleton().ShowFormation((STMF_FORMATION)num);
		if (STMF_GameInfo.getInstance().currentState == STMF_GameState.On_Game)
		{
			STMF_GameInfo.getInstance().GameScene.HideTip();
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.F2))
		{
			if (tempNum > 4)
			{
				tempNum = 0;
			}
			STMF_Formation.GetSingleton().ShowFormation((STMF_FORMATION)tempNum);
			tempNum++;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.F3))
		{
			STMF_Formation.GetSingleton().ShowFormation(STMF_FORMATION.Formation_YaoQianShuL);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.F4))
		{
			STMF_Formation.GetSingleton().ShowFormation(STMF_FORMATION.Formation_YaoQianShuR);
		}
	}

	private void DoNewfish(string[] args)
	{
		Console.WriteLine("**********1*********DoNewfish**************************************");
		int realFishType = 0;
		int num = int.Parse(args[0]);
		Console.WriteLine("fishtype: " + num);
		int num2 = int.Parse(args[1]);
		Console.WriteLine("newFishCount: " + num2);
		if (num == 32)
		{
			realFishType = int.Parse(args[3]);
		}
		STMF_FishPathType[] array = new STMF_FishPathType[num2];
		string text = args[2];
		string[] array2 = text.Split('|');
		for (int i = 0; i < array2.Length; i++)
		{
			string[] array3 = array2[i].Split('#');
			array[i] = new STMF_FishPathType();
			array[i].fishid = int.Parse(array3[0]);
			array[i].newFishPathType = int.Parse(array3[1]);
		}
		for (int j = 0; j < array.Length; j++)
		{
			Console.WriteLine("fishid:" + array[j].fishid);
			Console.WriteLine("newFishPathType:" + array[j].newFishPathType);
		}
		if (num != 36 && STMF_NetMngr.GetSingleton().IsGameSceneLoadOk && STMF_FishPoolMngr.GetSingleton() != null && !STMF_GameParameter.G_bTest)
		{
			if (num == 32)
			{
				STMF_FishPoolMngr.GetSingleton().CreateCoralReefsFish((STMF_FISH_TYPE)realFishType, array[0].newFishPathType, array[0].fishid);
			}
			else
			{
				STMF_FishPoolMngr.GetSingleton().CreateFish((STMF_FISH_TYPE)num, array[0].newFishPathType, array[0].fishid, num2);
			}
		}
		if (STMF_GameInfo.getInstance().currentState == STMF_GameState.On_Game)
		{
			STMF_GameInfo.getInstance().GameScene.HideTip();
		}
	}

	private void DoClearfish(int[] args, bool isBackGround)
	{
		UnityEngine.Debug.LogError("清除鱼群: " + JsonMapper.ToJson(args) + " isBackGround: " + isBackGround);
		int num = args[0];
		if (STMF_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			if (!isBackGround)
			{
				STMF_SceneBgMngr.GetSingleton().ChangeScene(num);
			}
			else
			{
				STMF_SceneBgMngr.GetSingleton().SetScene(num);
			}
		}
	}

	private void DoForbitFired(object[] args)
	{
		Console.WriteLine("**********1*********DoForbitFired**************************************");
		Console.WriteLine("**********2*********DoForbitFired**************************************");
		if (STMF_NetMngr.GetSingleton().IsGameSceneLoadOk)
		{
			STMF_BulletPoolMngr.GetSingleton().mIsFireEnableBySvr = false;
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
		if (STMF_GameInfo.getInstance().currentState == STMF_GameState.On_Game)
		{
			STMF_GameInfo.getInstance().GameScene.AddNotice(text);
		}
	}

	private void DoPlayerInfo(object[] args)
	{
		Console.WriteLine("**********1*********DoPlayerInfo**************************************");
		STMF_User sTMF_User = new STMF_User();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["seatid"];
		int num2 = (int)dictionary["honor"];
		Console.WriteLine("seatid: " + num);
		Console.WriteLine("honor: " + num2);
		Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
		sTMF_User.id = (int)dictionary2["id"];
		sTMF_User.username = (string)dictionary2["username"];
		sTMF_User.nickname = (string)dictionary2["nickname"];
		sTMF_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
		sTMF_User.level = (int)dictionary2["level"];
		sTMF_User.gameGold = (int)dictionary2["gameGold"];
		sTMF_User.expeGold = (int)dictionary2["expeGold"];
		sTMF_User.photoId = (int)dictionary2["photoId"];
		sTMF_User.overflow = (int)dictionary2["overflow"];
		sTMF_User.gameScore = (int)dictionary2["gameScore"];
		sTMF_User.expeScore = (int)dictionary2["expeScore"];
		Console.WriteLine("user.id: " + sTMF_User.id);
		Console.WriteLine("user.username: " + sTMF_User.username);
		Console.WriteLine("user.nickname: " + sTMF_User.nickname);
		Console.WriteLine("user.sex: " + sTMF_User.sex);
		Console.WriteLine("user.level: " + sTMF_User.level);
		Console.WriteLine("user.gameGold: " + sTMF_User.gameGold);
		Console.WriteLine("user.expeGold: " + sTMF_User.expeGold);
		Console.WriteLine("user.photoId: " + sTMF_User.photoId);
		Console.WriteLine("user.overflow: " + sTMF_User.overflow);
		Console.WriteLine("user.gameScore: " + sTMF_User.gameScore);
		Console.WriteLine("user.expeScore: " + sTMF_User.expeScore);
		Console.WriteLine();
		STMF_GameInfo.getInstance().getPersonInfo(new STMF_UserInfo(sTMF_User), num2);
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
		if (STMF_GameInfo.getInstance().currentState != STMF_GameState.On_Game)
		{
		}
	}

	private void DoGameShutup(bool[] args)
	{
		Console.WriteLine("**********1*********DoGameShutup**************************************");
		bool flag = args[0];
		Console.WriteLine("forbidFlag: " + flag);
		Console.WriteLine("**********2*********DoGameShutup**************************************");
		STMF_GameInfo.getInstance().IsGameShuUp = flag;
	}

	private void DoUserShutup(bool[] args)
	{
		Console.WriteLine("**********1*********DoUserShutup**************************************");
		bool flag = args[0];
		Console.WriteLine("forbidFlag: " + flag);
		Console.WriteLine("**********2*********DoUserShutup**************************************");
		STMF_GameInfo.getInstance().IsUserShutUp = flag;
	}

	private void DoUserAward(int[] args)
	{
		Console.WriteLine("**********1*********DoUserAward**************************************");
		int num = args[0];
		Console.WriteLine("gold: " + num);
		Console.WriteLine("**********2*********DoUserAward**************************************");
		STMF_GameInfo.getInstance().updateUser("gameCoin", num);
		STMF_TipManager.getInstance().ShowTip(STMF_TipType.GivingCoin, num, string.Empty);
	}

	private void DoGameGold(int[] args)
	{
		Console.WriteLine("**********1*********DoGameGold**************************************");
		int num = args[0];
		Console.WriteLine("gameGold: " + num);
		Console.WriteLine("**********2*********DoGameGold**************************************");
		STMF_GameInfo.getInstance().updateUser("gameCoin", num);
	}

	private void DoExpeGold(int[] args)
	{
		Console.WriteLine("**********1*********DoExpeGold**************************************");
		int num = args[0];
		Console.WriteLine("expeGold: " + num);
		Console.WriteLine("**********2*********DoExpeGold**************************************");
		STMF_GameInfo.getInstance().updateUser("testCoin", num);
	}

	private void DoAddExpeGoldAuto(bool[] args)
	{
		Console.WriteLine("**********1*********DoAddExpeGoldAuto**************************************");
		bool flag = args[0];
		Console.WriteLine("bOk: " + flag);
		Console.WriteLine("**********2*********DoAddExpeGoldAuto**************************************");
		STMF_SoundManage.getInstance().playButtonMusic(STMF_ButtonMusicType.getTestCoin);
		STMF_TipManager.getInstance().ShowTip(STMF_TipType.ApplyForExpCoin_Success, 0, string.Empty);
	}

	private void DoOverflow(object[] args)
	{
		Console.WriteLine("**********1*********DoOverflow**************************************");
		Console.WriteLine("**********2*********DoOverflow**************************************");
		STMF_TipManager.getInstance().ShowTip(STMF_TipType.CoinOverFlow, 0, string.Empty);
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
			STMF_TipManager.getInstance().ShowTip(STMF_TipType.ServerUpdate, 0, string.Empty);
			break;
		case 2:
			STMF_TipManager.getInstance().ShowTip(STMF_TipType.Game_UserIdFrozen, 0, string.Empty);
			break;
		case 3:
			STMF_TipManager.getInstance().ShowTip(STMF_TipType.UserIdDeleted, 0, string.Empty);
			break;
		case 4:
			STMF_TipManager.getInstance().ShowTip(STMF_TipType.UserIdRepeative, 0, string.Empty);
			STMF_Sockets.GetSingleton().isReconnect = false;
			break;
		case 5:
			STMF_TipManager.getInstance().ShowTip(STMF_TipType.UserPwdChanged, 0, string.Empty);
			break;
		case 6:
			STMF_TipManager.getInstance().ShowTip(STMF_TipType.LoseTheServer, 0, string.Empty);
			break;
		}
	}

	private void DoQuitToDesk(object[] args)
	{
		STMF_TipManager.getInstance().ShowTip(STMF_TipType.QuitToDesk, 0, string.Empty);
	}

	private void HandleNetMsg_ApplyExchange(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		STMF_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (STMF_GameInfo.getInstance() != null && STMF_GameInfo.getInstance().UIScene != null)
		{
			STMF_GameInfo.getInstance().UIScene.UpdateUserInfo();
		}
		string ch = (!flag) ? "兑换被拒绝" : "兑换成功";
		string en = (!flag) ? "Failure of exchange" : "Successful exchange";
		STMF_TipManager.getInstance().ShowTip(STMF_TipType.Custom, 0, ZH2_GVars.ShowTip(ch, en, string.Empty));
	}

	private void HandleNetMsg_ApplyPay(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		STMF_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (STMF_GameInfo.getInstance() != null && STMF_GameInfo.getInstance().UIScene != null)
		{
			STMF_GameInfo.getInstance().UIScene.UpdateUserInfo();
		}
		string ch = (!flag) ? "充值被拒绝" : "充值成功";
		string en = (!flag) ? "Failure of TopUp" : "Successful TopUp";
		STMF_TipManager.getInstance().ShowTip(STMF_TipType.Custom, 0, ZH2_GVars.ShowTip(ch, en, string.Empty));
	}

	private void HandleNetMsg_changeScoreNotice(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		STMF_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (STMF_GameInfo.getInstance() != null && STMF_GameInfo.getInstance().UIScene != null)
		{
			STMF_GameInfo.getInstance().UIScene.UpdateUserInfo();
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
		STMF_TipManager.getInstance().ShowTip(STMF_TipType.Custom, 0, ZH2_GVars.ShowTip($"{arg}成功! 金币{num}", $"Successful {arg2}! GoldCoin {num}", string.Empty));
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
			STMF_TipManager.getInstance().ShowTip(STMF_TipType.TableDeleted, 0, string.Empty);
			break;
		case 2:
			STMF_TipManager.getInstance().ShowTip(STMF_TipType.TableConfigChanged, 0, string.Empty);
			break;
		default:
			STMF_TipManager.getInstance().ShowTip(STMF_TipType.LongTimeNoHandle, 0, string.Empty);
			break;
		}
	}
}
