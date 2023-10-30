using JsonFx.Json;
using LitJson;
using STDT_GameConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XLDT_Transmit : MonoBehaviour
{
	private static XLDT_Transmit _MyTransmit;

	private XLDT_Sockets m_CreateSocket;

	public static XLDT_Transmit GetSingleton()
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

	public void TransmitGetPoint(XLDT_Sockets MyCreateSocket)
	{
		m_CreateSocket = MyCreateSocket;
	}

	public void PostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] array = table["args"] as object[];
		XLDT_GameManager.getInstance().Write(text.ToString());
		if (text == "sendServerTime")
		{
			XLDT_GameManager.getInstance().Write("sendServerTime");
			if (XLDT_GameInfo.getInstance().Key != string.Empty)
			{
				if (XLDT_GameInfo.getInstance().currentState < XLDT_GameState.On_EnterGame)
				{
					XLDT_NetMain.GetSingleton().MyCreateSocket.SendUserLogin(XLDT_GameInfo.getInstance().UserId, XLDT_GameInfo.getInstance().Pwd, (int)((XLDT_GameInfo.getInstance().currentState < XLDT_GameState.On_SelectRoom) ? XLDT_GameState.On_SelectRoom : XLDT_GameInfo.getInstance().currentState), "9.0.1");
				}
				else
				{
					XLDT_NetMain.GetSingleton().MyCreateSocket.SendUserLogin(XLDT_GameInfo.getInstance().UserId, XLDT_GameInfo.getInstance().Pwd, 3, "9.0.1");
				}
			}
			XLDT_GameInfo.getInstance().Key = "sendServerTime";
		}
		else if (text == "checkVersion")
		{
			DoCheckVersion(array);
		}
		else if (text == "notUpdate")
		{
			DoNotUpdate(array);
		}
		else if (text == "userLogin")
		{
			DoUserLogin(array);
		}
		else if (text == "enterRoom")
		{
			DoEnterRoom(array);
		}
		else if (text == "selectHall")
		{
			DoSelectHallInfo(array);
		}
		else if (text == "addExpeGoldAuto")
		{
			DoAddExpeGoldAuto(table["args"] as bool[]);
		}
		else if (text == "selectSeat")
		{
			DoSelectSeat(array);
		}
		else if (text == "updateDeskInfo")
		{
			DoUpdateDeskInfo(array);
		}
		else if (text == "gameRestart")
		{
			DoGameRestart(array);
		}
		else if (text == "gameResult")
		{
			DoGameResult(array);
		}
		else if (text == "deskTotalBet")
		{
			DoDeskTotalBet(array);
		}
		else if (text == "currentBet")
		{
			DoCurrentBet(array);
		}
		else if (text == "continueBet")
		{
			DoContinueBet(array);
		}
		else if (text == "playerInfo")
		{
			DoPlayerInfo(array);
		}
		else if (text == "sendChat")
		{
			DoSendChat(array);
		}
		else if (text == "newGameScore")
		{
			DoNewGameScore(table["args"] as int[]);
		}
		else if (text == "newExpeScore")
		{
			DoNewExpeScore(table["args"] as int[]);
		}
		else if (text == "newGameGold")
		{
			DoNewGameGold(table["args"] as int[]);
		}
		else if (text == "newExpeGold")
		{
			DoNewExpeGold(table["args"] as int[]);
		}
		else if (text == "sendNotice")
		{
			DoSendNotice(array);
		}
		else if (text == "resultList")
		{
			DoResultList(array);
		}
		else if (text == "overflow")
		{
			DoOverflow(array);
		}
		else if (text == "quitToLogin")
		{
			int[] args = table["args"] as int[];
			DoQuitToLogin(args);
		}
		else if (text == "quitToRoom")
		{
			DoQuitToRoom(array);
		}
		else if (text == "quitToDesk")
		{
			DoQuitToDesk(array);
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
		else if (text == "updateRoomCommonInfo")
		{
			DoUpdateRoomCommonInfo(array);
		}
		else if (text == "gameShutup")
		{
			DoGameShutup(array);
		}
		else if (text == "userShutup")
		{
			MonoBehaviour.print(table["args"]);
			DoUserShutup(table["args"] as bool[]);
		}
		else if (text == "userAward")
		{
			DoUserAward(array);
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
		else if (text == "printStart")
		{
			DoPrintStart(array);
		}
		else if (text == "printOver")
		{
			DoPrintOver(array);
		}
		else if (text == "printClearError")
		{
			DoPrintClearError(array);
		}
		else if (text == "checkRecord")
		{
			DoCheckRecord(array);
		}
		else if (text == "abnormal")
		{
			DoAbnormal(array);
		}
		else if (text == "pleaseWait")
		{
			DoPleaseWait2(array);
		}
		else if (text == "leaveSeat")
		{
			DoLeaveSeat(table["args"] as bool[]);
		}
	}

	public void SelectPostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] array = table["args"] as object[];
		if (text == "pleaseWait")
		{
			int agrs = (int)table["args"];
			DoPleaseWait2(agrs);
		}
		XLDT_GameManager.getInstance().Write(text.ToString());
		if (text == "sendServerTime")
		{
			if (XLDT_GameInfo.getInstance().Key != string.Empty)
			{
				XLDT_NetMain.GetSingleton().MyCreateSocket.SendUserLogin(XLDT_GameInfo.getInstance().UserId, XLDT_GameInfo.getInstance().Pwd, (int)((XLDT_GameInfo.getInstance().currentState < XLDT_GameState.On_SelectRoom) ? XLDT_GameState.On_SelectRoom : XLDT_GameInfo.getInstance().currentState), "9.0.1");
			}
			XLDT_GameInfo.getInstance().Key = "sendServerTime";
		}
		else if (text == "checkVersion")
		{
			DoCheckVersion(array);
		}
		else if (text == "notUpdate")
		{
			DoNotUpdate(array);
		}
		else if (text == "userLogin")
		{
			DoUserLogin(array);
		}
		else if (text == "enterRoom")
		{
			DoEnterRoom(array);
		}
		else if (text == "addExpeGoldAuto")
		{
			DoAddExpeGoldAuto(table["args"] as bool[]);
		}
		else if (text == "selectSeat")
		{
			DoSelectSeat(array);
		}
		else if (text == "updateDeskInfo")
		{
			DoUpdateDeskInfo(array);
		}
		else
		{
			if (text == "gameRestart" || text == "gameResult")
			{
				return;
			}
			if (text == "deskTotalBet")
			{
				DoDeskTotalBet(array);
			}
			else if (text == "currentBet")
			{
				DoCurrentBet(array);
			}
			else if (text == "continueBet")
			{
				DoContinueBet(array);
			}
			else
			{
				if (text == "playerInfo" || text == "sendChat")
				{
					return;
				}
				if (text == "newGameScore")
				{
					DoNewGameScore(table["args"] as int[]);
				}
				else if (text == "newExpeScore")
				{
					DoNewExpeScore(table["args"] as int[]);
				}
				else if (text == "newGameGold")
				{
					DoNewGameGold(table["args"] as int[]);
				}
				else if (text == "newExpeGold")
				{
					DoNewExpeGold(table["args"] as int[]);
				}
				else if (!(text == "sendNotice"))
				{
					if (text == "resultList")
					{
						DoResultList(array);
					}
					else if (text == "overflow")
					{
						DoOverflow(array);
					}
					else if (text == "quitToLogin")
					{
						int[] args = table["args"] as int[];
						DoQuitToLogin(args);
					}
					else if (text == "quitToRoom")
					{
						DoQuitToRoom(array);
					}
					else if (text == "updateRoomInfo")
					{
						DoUpdateRoomInfo(array);
					}
					else if (text == "updateRoomCommonInfo")
					{
						DoUpdateRoomCommonInfo(array);
					}
					else if (text == "gameShutup")
					{
						DoGameShutup(array);
					}
					else if (text == "userShutup")
					{
						DoUserShutup(table["args"] as bool[]);
					}
					else if (text == "userAward")
					{
						DoUserAward(array);
					}
					else if (text == "NetThread/NetDown")
					{
						DoNetDown(array);
					}
					else if (text == "printStart")
					{
						DoPrintStart(array);
					}
					else if (text == "printOver")
					{
						DoPrintOver(array);
					}
					else if (text == "printClearError")
					{
						DoPrintClearError(array);
					}
					else if (text == "checkRecord")
					{
						DoCheckRecord(array);
					}
					else if (text == "abnormal")
					{
						DoAbnormal(array);
					}
					else if (text == "pleaseWait")
					{
						DoPleaseWait2(array);
					}
					else if (text == "leaveSeat")
					{
						DoLeaveSeat(table["args"] as bool[]);
					}
				}
			}
		}
	}

	private void DoNetDown(object[] args)
	{
		if (!XLDT_Sockets.GetSingleton().isReconnect)
		{
			return;
		}
		if (m_CreateSocket.GetRelineCount() < 15)
		{
			m_CreateSocket.CreateReceiveThread();
			return;
		}
		Console.WriteLine("30秒重连失败，网络断开，请重新登录网络大厅...");
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.Net_ConnectionError, 0, string.Empty);
		}
		XLDT_GameManager.getInstance().RecNetDown();
	}

	private void DoUserLogin(object[] args)
	{
		UnityEngine.Debug.LogError("DoCheckLogin: " + JsonMapper.ToJson(args));
		XLDT_User xLDT_User = new XLDT_User();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["isLogin"];
		int num = (int)dictionary["messageStatus"];
		Console.WriteLine("isLogin: " + flag);
		Console.WriteLine("messageStatus: " + num);
		if (flag)
		{
			MonoBehaviour.print("isLogin");
			bool flag2 = (bool)dictionary["isShutup"];
			bool flag3 = (bool)dictionary["special"];
			Console.WriteLine("isShutup: " + flag2);
			Console.WriteLine("special: " + flag3);
			ZH2_GVars.hallInfo = new Dictionary<string, object>();
			ZH2_GVars.hallInfo = (dictionary["hallInfo"] as Dictionary<string, object>);
			Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
			xLDT_User.id = (int)dictionary2["id"];
			ZH2_GVars.userId = xLDT_User.id;
			xLDT_User.username = (string)dictionary2["username"];
			xLDT_User.nickname = (string)dictionary2["nickname"];
			xLDT_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
			xLDT_User.password = (string)dictionary2["password"];
			xLDT_User.phone = (string)dictionary2["phone"];
			xLDT_User.card = (string)dictionary2["card"];
			xLDT_User.answer = (string)dictionary2["answer"];
			xLDT_User.registDate = (string)dictionary2["registDate"];
			xLDT_User.loginDate = (string)dictionary2["loginDate"];
			xLDT_User.status = (int)dictionary2["status"];
			xLDT_User.overflow = (int)dictionary2["overflow"];
			xLDT_User.gameGold = (int)dictionary2["gameGold"];
			xLDT_User.expeGold = (int)dictionary2["expeGold"];
			xLDT_User.levelScore = (double)dictionary2["levelScore"];
			xLDT_User.gameScore = (int)dictionary2["gameScore"];
			xLDT_User.expeScore = (int)dictionary2["expeScore"];
			xLDT_User.level = (int)dictionary2["level"];
			xLDT_User.photoId = (int)dictionary2["photoId"];
			xLDT_User.lastDeskId = (int)dictionary2["lastDeskId"];
			xLDT_User.shutupStatus = (int)dictionary2["shutupStatus"];
			xLDT_User.lastGame = (int)dictionary2["lastGame"];
			xLDT_User.type = (int)dictionary2["type"];
			xLDT_User.borrow = (int)dictionary2["borrow"];
			xLDT_User.displayStatus = (int)dictionary2["displayStatus"];
			Console.WriteLine("user.id:" + xLDT_User.id);
			Console.WriteLine("user.username:" + xLDT_User.username);
			Console.WriteLine("user.nickname:" + xLDT_User.nickname);
			Console.WriteLine("user.name:" + xLDT_User.name);
			Console.WriteLine("user.sex:" + xLDT_User.sex);
			Console.WriteLine("user.password:" + xLDT_User.password);
			Console.WriteLine("user.phone:" + xLDT_User.phone);
			Console.WriteLine("user.card:" + xLDT_User.card);
			Console.WriteLine("user.question:" + xLDT_User.question);
			Console.WriteLine("user.answer:" + xLDT_User.answer);
			Console.WriteLine("user.registDate:" + xLDT_User.registDate);
			Console.WriteLine("user.loginDate:" + xLDT_User.loginDate);
			Console.WriteLine("user.status:" + xLDT_User.status);
			Console.WriteLine("user.overflow:" + xLDT_User.overflow);
			Console.WriteLine("user.gameGold:" + xLDT_User.gameGold);
			Console.WriteLine("user.expeGold:" + xLDT_User.expeGold);
			Console.WriteLine("user.levelScore:" + xLDT_User.levelScore);
			Console.WriteLine("user.gameScore:" + xLDT_User.gameScore);
			Console.WriteLine("user.expeScore:" + xLDT_User.expeScore);
			Console.WriteLine("user.level:" + xLDT_User.level);
			Console.WriteLine("user.photoId:" + xLDT_User.photoId);
			Console.WriteLine("user.lastDeskId:" + xLDT_User.lastDeskId);
			Console.WriteLine("user.shutupStatus:" + xLDT_User.shutupStatus);
			Console.WriteLine("user.lastGame:" + xLDT_User.lastGame);
			Console.WriteLine("user.type:" + xLDT_User.type);
			Console.WriteLine("user.borrow:" + xLDT_User.borrow);
			Console.WriteLine("user.displayStatus:" + xLDT_User.displayStatus);
			XLDT_GameManager.getInstance().RecLoginMessage(flag, 0, xLDT_User);
			XLDT_GameInfo.getInstance().IsGameShuUp = flag2;
			if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
			{
				XLDT_GameInfo.getInstance().User.CoinCount = xLDT_User.gameGold;
				XLDT_GameInfo.getInstance().User.ScoreCount = xLDT_User.gameScore;
				XLDT_GameUIMngr.GetSingleton().UserScore = xLDT_User.gameGold;
			}
		}
		else
		{
			MonoBehaviour.print("RecLogin");
			XLDT_GameManager.getInstance().RecLoginMessage(flag, num);
		}
		Console.WriteLine("**********2*********DoUserLogin**************************************");
	}

	private void DoEnterRoom(object[] args)
	{
	}

	private void DoSelectHallInfo(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		UpdateOnline(dictionary);
		int countTime = (int)dictionary["betTime"];
		int betType = (int)dictionary["beilvType"];
		int length = (dictionary["Desk"] as Array).Length;
		if (length > 0)
		{
			XLDT_ShowUI.getInstance().SetNoHall(isHavHall: true);
			XLDT_CardDesk[] array = new XLDT_CardDesk[length];
			for (int i = 0; i < length; i++)
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["Desk"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new XLDT_CardDesk();
				array[i].id = (int)dictionary2["id"];
				array[i].roomId = (int)dictionary2["roomId"];
				array[i].name = (string)dictionary2["name"];
				array[i].autoKick = (int)dictionary2["autoKick"];
				array[i].minGold = (int)dictionary2["minGold"];
				array[i].gameXianHong = (int)dictionary2["gameXianHong"];
				array[i].exchange = (int)dictionary2["exchange"];
				array[i].minYaFen = (int)dictionary2["minYaFen"];
				array[i].wheelLocal = (int)dictionary2["wheelLocal"];
				array[i].dayLocal = (int)dictionary2["dayLocal"];
				array[i].onceExchangeValue = (int)dictionary2["onceExchangeValue"];
				array[i].state = (int)dictionary2["state"];
				array[i].baseYaFen = (int)dictionary2["baseYaFen"];
				array[i].orderBy = (int)dictionary2["orderBy"];
				array[i].sumYaFen = Convert.ToInt64(dictionary2["sumYaFen"]);
				array[i].sumDeFen = Convert.ToInt64(dictionary2["sumDeFen"]);
				for (int j = 0; j < 8; j++)
				{
					Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
					dictionary3 = ((dictionary2["seats"] as Array).GetValue(j) as Dictionary<string, object>);
					array[i].seats[j] = new XLDT_Seat();
					array[i].seats[j].id = (int)dictionary3["id"];
					array[i].seats[j].isFree = (bool)dictionary3["isFree"];
					array[i].seats[j].userNickname = (string)dictionary3["userNickname"];
					array[i].seats[j].photoId = (int)dictionary3["photoId"];
					array[i].seats[j].userId = (int)dictionary3["userId"];
				}
			}
			array = Test(array);
			XLDT_GameManager.getInstance().RecTableList(array);
			XLDT_ShowUI.getInstance().InItTable(array);
		}
		else
		{
			UnityEngine.Debug.LogError("没有房间");
			XLDT_ShowUI.getInstance().SetNoHall(isHavHall: false);
			XLDT_ShowUI.getInstance().UnOpen(XLDT_GameInfo.getInstance().User.RoomId);
			XLDT_ShowUI.getInstance().InItTable(null);
		}
		XLDT_GameInfo.getInstance().CountTime = countTime;
		XLDT_GameInfo.getInstance().BetType = betType;
	}

	private XLDT_CardDesk[] Test(XLDT_CardDesk[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = 0; j < array.Length - 1; j++)
			{
				if (array[j].id > array[j + 1].id)
				{
					XLDT_CardDesk xLDT_CardDesk = array[j];
					array[j] = array[j + 1];
					array[j + 1] = xLDT_CardDesk;
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
			if (XLDT_ShowUI.getInstance() != null)
			{
				for (int i = 0; i < dictionary2.Count; i++)
				{
					for (int j = 0; j < XLDT_ShowUI.getInstance().selectBtnList.Count; j++)
					{
						if (i == XLDT_ShowUI.getInstance().selectBtnList[j].hallId)
						{
							XLDT_ShowUI.getInstance().selectBtnList[j].onlinePeople = dictionary2[i.ToString()].ToString();
							XLDT_ShowUI.getInstance().selectBtnList[j].UpdateText();
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

	private void DoAddExpeGoldAuto(bool[] args)
	{
		bool flag = args[0];
		Console.WriteLine("bOk: " + flag);
		XLDT_GameManager.getInstance().RecUpdateExpCoin(flag);
	}

	private void DoSelectSeat(object[] args)
	{
		int seatid = 0;
		int[] array = new int[5];
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["success"];
		Console.WriteLine("bCanSeat:" + flag);
		if (!flag)
		{
			int type = (int)dictionary["messageStatus"];
			XLDT_GameManager.getInstance().RecEnterGame(flag, seatid, type);
		}
		else
		{
			seatid = (int)dictionary["seatId"];
			XLDT_GameManager.getInstance().RecEnterGame(flag, seatid);
			try
			{
				array = (int[])dictionary["betChip"];
				XLDT_GameUIMngr.GetSingleton().mBetCtrl.SetChip(array);
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
		}
	}

	private void DoUpdateDeskInfo(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["deskId"];
		Console.WriteLine("nDeskId:" + num);
		int length = (dictionary["Seat"] as Array).Length;
		if (length > 0)
		{
			XLDT_Seat[] array = new XLDT_Seat[length];
			for (int i = 0; i < length; i++)
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["Seat"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new XLDT_Seat();
				array[i].id = (int)dictionary2["id"];
				array[i].isFree = (bool)dictionary2["isFree"];
				array[i].userNickname = (string)dictionary2["userNickname"];
				array[i].photoId = (int)dictionary2["photoId"];
				array[i].userId = (int)dictionary2["userId"];
				Console.WriteLine("id:" + array[i].id);
				Console.WriteLine("isFree:" + array[i].isFree);
				Console.WriteLine("userNickname:" + array[i].userNickname);
				Console.WriteLine("photoId:" + array[i].photoId);
				Console.WriteLine("userId:" + array[i].userId);
			}
			XLDT_GameManager.getInstance().RecUpdatSeatInfo(num, array);
			XLDT_ShowUI.getInstance().UpdateSeat(array, num);
			if (num == XLDT_GameInfo.getInstance().CurTable.Id)
			{
				XLDT_GameInfo.getInstance().updateOtherUsers(array);
			}
		}
		else
		{
			Console.WriteLine("该游戏桌子上没有座位!!!");
			XLDT_GameManager.getInstance().RecUpdatSeatInfo(-1, null);
		}
	}

	private void DoGameRestart(object[] args)
	{
		XLDT_GameManager.getInstance().Write("DoGameRestart");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["roundId"];
		int num2 = (int)dictionary["gameId"];
		int num3 = (int)dictionary["clearCard"];
		int num4 = (int)dictionary["clearCardSuit"];
		int num5 = (int)dictionary["clearCardPoint"];
		int num6 = (int)dictionary["betTime"];
		int num7 = (int)dictionary["laddyId"];
		XLDT_GameManager.getInstance().Write("girltype=" + num7);
		int length = (dictionary["awardGold"] as Array).Length;
		int[] array = new int[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = (int)(dictionary["awardGold"] as Array).GetValue(i);
		}
		Console.WriteLine("nRoundId：" + num);
		Console.WriteLine("nGameId：" + num2);
		Console.WriteLine("nClearCard：" + num3);
		Console.WriteLine("nClearCardSuit：" + num4);
		Console.WriteLine("nClearCardPoint：" + num5);
		Console.WriteLine("nBetTime:" + num6);
		Console.WriteLine("laddyId:" + num7);
		Console.Write("awardGold：");
		if (length > 0)
		{
			for (int j = 0; j < length; j++)
			{
				Console.Write(" " + array[j]);
			}
		}
		XLDT_GameManager.getInstance().RecGameRestart(num, num2, num3, num4, num5, array, num6, num7);
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUIMngr.GetSingleton().Restart(num3, num4, num5);
		}
	}

	private void DoGameResult(object[] args)
	{
		UnityEngine.Debug.Log("**********1*********DoGameResult**************************************");
		XLDT_CardAlgorithmResult xLDT_CardAlgorithmResult = new XLDT_CardAlgorithmResult();
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["addScore"];
		Console.WriteLine("addScore" + num);
		int length = (dictionary["currentBets"] as Array).Length;
		if (length > 0)
		{
			int[] array = new int[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = (int)(dictionary["currentBets"] as Array).GetValue(i);
				Console.WriteLine("currentBets:" + array[i]);
			}
			if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
			{
				XLDT_GameUIMngr.GetSingleton().mBetCtrl.SetBetBtnMyArray(array);
			}
		}
		int length2 = (dictionary["incomeInfo"] as Array).Length;
		XLDT_GameInfo.getInstance().Income.Clear();
		if (length2 > 0)
		{
			XLDT_Income[] array2 = new XLDT_Income[length2];
			for (int j = 0; j < length2; j++)
			{
				array2[j] = new XLDT_Income();
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["incomeInfo"] as Array).GetValue(j) as Dictionary<string, object>);
				array2[j].seatId = (int)dictionary2["seatId"];
				array2[j].score = (int)dictionary2["score"];
				XLDT_GameInfo.getInstance().Income.Add(array2[j]);
			}
		}
		Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
		dictionary3 = (dictionary["cardAlgorithmResulr"] as Dictionary<string, object>);
		xLDT_CardAlgorithmResult.awardType = (int)dictionary3["awardType"];
		xLDT_CardAlgorithmResult.awardType = 0;
		xLDT_CardAlgorithmResult.color = (int)dictionary3["color"];
		xLDT_CardAlgorithmResult.point = (int)dictionary3["point"];
		xLDT_CardAlgorithmResult.jackpot = (int)dictionary3["jackpot"];
		XLDT_GameManager.getInstance().RecCurrentAward(xLDT_CardAlgorithmResult);
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUIMngr.GetSingleton().mBetCtrl.CurScore = num;
			XLDT_GameUIMngr.GetSingleton().mBetCtrl.SendContinueBet();
		}
	}

	private void DoDeskTotalBet(object[] args)
	{
		int length = (args[0] as Array).Length;
		if (length > 0)
		{
			int[] array = new int[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = (int)(args[0] as Array).GetValue(i);
			}
			if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
			{
				XLDT_GameUIMngr.GetSingleton().mBetCtrl.SetBetBtnTotalArray(array);
			}
		}
	}

	private void DoCurrentBet(object[] args)
	{
		int length = (args[0] as Array).Length;
		Console.WriteLine("CurrentBet: ");
		if (length > 0)
		{
			int[] array = new int[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = (int)(args[0] as Array).GetValue(i);
				Console.Write(" " + array[i]);
			}
			if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
			{
				XLDT_GameUIMngr.GetSingleton().mBetCtrl.SetBetBtnMyArray(array);
			}
		}
	}

	private void DoPlayerInfo(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		XLDT_User xLDT_User = new XLDT_User();
		int num = (int)dictionary["seatid"];
		Console.WriteLine("seatId:" + num);
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		dictionary2 = (dictionary["user"] as Dictionary<string, object>);
		xLDT_User.id = (int)dictionary2["id"];
		xLDT_User.username = (string)dictionary2["username"];
		xLDT_User.nickname = (string)dictionary2["nickname"];
		xLDT_User.sex = ((string)dictionary2["sex"]).ToCharArray()[0];
		xLDT_User.password = (string)dictionary2["password"];
		xLDT_User.phone = (string)dictionary2["phone"];
		xLDT_User.card = (string)dictionary2["card"];
		xLDT_User.question = (string)dictionary2["question"];
		xLDT_User.answer = (string)dictionary2["answer"];
		xLDT_User.registDate = (string)dictionary2["registDate"];
		xLDT_User.loginDate = (string)dictionary2["loginDate"];
		xLDT_User.status = (int)dictionary2["status"];
		xLDT_User.overflow = (int)dictionary2["overflow"];
		xLDT_User.gameGold = (int)dictionary2["gameGold"];
		xLDT_User.expeGold = (int)dictionary2["expeGold"];
		xLDT_User.levelScore = (double)dictionary2["levelScore"];
		xLDT_User.gameScore = (int)dictionary2["gameScore"];
		xLDT_User.expeScore = (int)dictionary2["expeScore"];
		xLDT_User.level = (int)dictionary2["level"];
		xLDT_User.photoId = (int)dictionary2["photoId"];
		xLDT_User.lastDeskId = (int)dictionary2["lastDeskId"];
		xLDT_User.shutupStatus = (int)dictionary2["shutupStatus"];
		xLDT_User.lastGame = (int)dictionary2["lastGame"];
		xLDT_User.type = (int)dictionary2["type"];
		xLDT_User.borrow = (int)dictionary2["borrow"];
		xLDT_User.displayStatus = (int)dictionary2["displayStatus"];
		int length = (dictionary["honor"] as Array).Length;
		int[] array = new int[length];
		string text = string.Empty;
		string[] array2 = new string[3]
		{
			"总:No.",
			" 周:No.",
			" 日:No."
		};
		for (int i = 0; i < length; i++)
		{
			array[i] = (int)(dictionary["honor"] as Array).GetValue(i);
			Console.WriteLine("排行榜" + i + ": " + array[i]);
			if (array[i] != -1)
			{
				string text2 = text;
				text = text2 + array2[i] + array[i] + " ";
			}
		}
		xLDT_User.level--;
		XLDT_GameManager.getInstance().RecOtherPlayerInfo(num, xLDT_User, array);
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUIMngr.GetSingleton().ShowPersonInfoDlg(xLDT_User, array);
		}
	}

	private void DoSendChat(object[] args)
	{
	}

	private void DoNewGameScore(int[] args)
	{
		int num = args[0];
		UnityEngine.Debug.LogError("当前状态: " + XLDT_GameInfo.getInstance().currentState + "   " + num);
		XLDT_GameInfo.getInstance().User.ScoreCount = num;
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUIMngr.GetSingleton().UserScore = num;
		}
		else if (XLDT_GameUIMngr.GetSingleton() != null)
		{
			XLDT_GameUIMngr.GetSingleton().UserScore = num;
		}
	}

	private void DoNewExpeScore(int[] args)
	{
		int num = args[0];
		Console.WriteLine("nExpeScore:" + num);
		XLDT_GameInfo.getInstance().User.ScoreCount = num;
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUIMngr.GetSingleton().UserScore = num;
		}
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.getTestCoin);
	}

	private void DoNewGameGold(int[] args)
	{
		int num = args[0];
		XLDT_GameInfo.getInstance().User.CoinCount = num;
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUIMngr.GetSingleton().UserCoin = num;
		}
		XLDT_GameManager.getInstance().RecUpdateCoin(num);
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.getCoin);
	}

	private void DoNewExpeGold(int[] args)
	{
		int num = args[0];
		XLDT_GameInfo.getInstance().User.TestCoinCount = num;
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUIMngr.GetSingleton().UserCoin = num;
		}
		XLDT_GameManager.getInstance().RecExpCoin(num);
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

	private void DoResultList(object[] args)
	{
		int length = (args[0] as Array).Length;
		MonoBehaviour.print("nDeskRecordCount: " + length);
		if (length > 0)
		{
			XLDT_CardAlgorithmResult[] array = new XLDT_CardAlgorithmResult[length];
			for (int i = 0; i < length; i++)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary = ((args[0] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new XLDT_CardAlgorithmResult();
				array[i].awardType = (int)dictionary["awardType"];
				array[i].awardType = 0;
				array[i].color = (int)dictionary["color"];
				array[i].point = (int)dictionary["point"];
				array[i].jackpot = (int)dictionary["jackpot"];
			}
			XLDT_GameManager.getInstance().RecRecord(array);
			if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
			{
				XLDT_GameUIMngr.GetSingleton().mBetCtrl.StatisRecord(array);
			}
		}
		Console.WriteLine("**********2*********DoResultList**************************************");
	}

	private void DoOverflow(object[] args)
	{
		XLDT_GameManager.getInstance().RecOverflow();
		Console.WriteLine("**********1*********DoOverflow**************************************");
		Console.WriteLine("**********2*********DoOverflow**************************************");
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.CoinOverFlow, 0, string.Empty);
		}
	}

	private void DoQuitToLogin(int[] args)
	{
		Console.WriteLine("**********1*********DoQuitToLogin**************************************");
		int type = args[0];
		XLDT_Sockets.GetSingleton().isReconnect = false;
		UnityEngine.Debug.Log("DoQuitToLogin1 type: " + type);
		XLDT_DelayCall.G_DelayCall.StartDelay(2f, delegate
		{
			XLDT_GameManager.getInstance().RecQuitToHall(type);
		});
		UnityEngine.Debug.Log("GameManager.getInstance(): " + XLDT_GameManager.getInstance());
		UnityEngine.Debug.Log("GameManager");
		UnityEngine.Debug.Log("DoQuitToLogin2 type: " + type);
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
		switch (type)
		{
		case 1:
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.ServerUpdate, 0, string.Empty);
			break;
		case 2:
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.Game_UserIdFrozen, 0, string.Empty);
			break;
		case 3:
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.UserIdDeleted, 0, string.Empty);
			break;
		case 4:
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.UserIdRepeative, 0, string.Empty);
			break;
		case 5:
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.UserPwdChanged, 0, string.Empty);
			break;
		case 6:
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.LoseTheServer, 0, string.Empty);
			break;
		}
	}

	private void DoQuitToRoom(object[] args)
	{
		Console.WriteLine("**********1*********DoQuitToRoom**************************************");
		int num = (int)args[0];
		Console.WriteLine("type:" + num);
		XLDT_GameManager.getInstance().RecQuitToTableList(num);
		Console.WriteLine("**********2*********DoQuitToRoom**************************************");
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			switch (num)
			{
			case 1:
				XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.TableDeleted, 0, string.Empty);
				break;
			case 2:
				XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.TableConfigChanged, 0, string.Empty);
				break;
			default:
				XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.LongTimeNoHandle, 0, string.Empty);
				break;
			}
		}
	}

	private void DoQuitToDesk(object[] args)
	{
		XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.QuitToDesk, 0, string.Empty);
	}

	private void DoUpdateRoomInfo(object[] args)
	{
	}

	private void DoUpdateHallInfo(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		UpdateOnline(dictionary);
		int countTime = (int)dictionary["betTime"];
		int num = (int)dictionary["beilvType"];
		int length = (dictionary["Desk"] as Array).Length;
		XLDT_GameInfo.getInstance().CountTime = countTime;
		XLDT_GameInfo.getInstance().BetType = num;
		XLDT_GameManager.getInstance().Write("DoUpdateRoomInfo=" + length);
		if (length > 0)
		{
			XLDT_ShowUI.getInstance().SetNoHall(isHavHall: true);
			XLDT_CardDesk[] array = new XLDT_CardDesk[length];
			for (int i = 0; i < length; i++)
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2 = ((dictionary["Desk"] as Array).GetValue(i) as Dictionary<string, object>);
				array[i] = new XLDT_CardDesk();
				array[i].id = (int)dictionary2["id"];
				array[i].roomId = (int)dictionary2["roomId"];
				array[i].name = (string)dictionary2["name"];
				array[i].autoKick = (int)dictionary2["autoKick"];
				array[i].minGold = (int)dictionary2["minGold"];
				array[i].gameXianHong = (int)dictionary2["gameXianHong"];
				array[i].exchange = (int)dictionary2["exchange"];
				array[i].minYaFen = (int)dictionary2["minYaFen"];
				array[i].wheelLocal = (int)dictionary2["wheelLocal"];
				array[i].dayLocal = (int)dictionary2["dayLocal"];
				array[i].onceExchangeValue = (int)dictionary2["onceExchangeValue"];
				array[i].state = (int)dictionary2["state"];
				array[i].baseYaFen = (int)dictionary2["baseYaFen"];
				array[i].orderBy = (int)dictionary2["orderBy"];
				array[i].sumYaFen = Convert.ToInt64(dictionary2["sumYaFen"]);
				array[i].sumDeFen = Convert.ToInt64(dictionary2["sumDeFen"]);
				for (int j = 0; j < 8; j++)
				{
					Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
					dictionary3 = ((dictionary2["seats"] as Array).GetValue(j) as Dictionary<string, object>);
					array[i].seats[j] = new XLDT_Seat();
					array[i].seats[j].id = (int)dictionary3["id"];
					array[i].seats[j].isFree = (bool)dictionary3["isFree"];
					array[i].seats[j].userNickname = (string)dictionary3["userNickname"];
					array[i].seats[j].photoId = (int)dictionary3["photoId"];
					array[i].seats[j].userId = (int)dictionary3["userId"];
				}
			}
			array = Test(array);
			XLDT_GameManager.getInstance().RecUpdateRoomInfo(array);
			XLDT_ShowUI.getInstance().InItTable(array);
		}
		else
		{
			UnityEngine.Debug.LogError("没有房间");
			XLDT_ShowUI.getInstance().SetNoHall(isHavHall: false);
			XLDT_GameManager.getInstance().RecUpdateRoomInfo(null);
			XLDT_ShowUI.getInstance().InItTable(null);
		}
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUIMngr.GetSingleton().mBetCtrl.SetBetPower(num - 1);
		}
	}

	private void DoUpdateRoomCommonInfo(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int countTime = (int)dictionary["beiTime"];
		int num = (int)dictionary["beilvType"];
		XLDT_GameInfo.getInstance().CountTime = countTime;
		XLDT_GameInfo.getInstance().BetType = num;
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUIMngr.GetSingleton().mBetCtrl.SetBetPower(num - 1);
		}
	}

	private void DoGameShutup(object[] args)
	{
		Console.WriteLine("**********1*********DoGameShutup**************************************");
		bool flag = (bool)args[0];
		Console.WriteLine("forbidFlag:" + flag);
		Console.WriteLine("**********2*********DoGameShutup**************************************");
		XLDT_GameInfo.getInstance().IsGameShuUp = flag;
	}

	private void DoUserShutup(bool[] args)
	{
		Console.WriteLine("**********1*********DoUserShutup**************************************");
		bool flag = args[0];
		Console.WriteLine("forbidFlag: " + flag);
		Console.WriteLine("**********2*********DoUserShutup**************************************");
		XLDT_GameInfo.getInstance().IsUserShutUp = flag;
	}

	private void DoUserAward(object[] args)
	{
		Console.WriteLine("**********1*********DoUserAward**************************************");
		int num = (int)args[0];
		Console.WriteLine("gold:" + num);
		XLDT_GameManager.getInstance().RecGivingCoin(num);
		Console.WriteLine("**********2*********DoUserAward**************************************");
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.GivingCoin, num, string.Empty);
		}
	}

	private void HandleNetMsg_ApplyExchange(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		XLDT_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (XLDT_ShowUI.getInstance() != null)
		{
			XLDT_ShowUI.getInstance().UpdateGameCoin();
		}
		string ch = (!flag) ? "兑换被拒绝" : "兑换成功";
		string en = (!flag) ? "Failure of exchange" : "Successful exchange";
		if (XLDT_GameUITipMngr.getInstance() != null)
		{
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.Custom, 0, ZH2_GVars.ShowTip(ch, en, string.Empty));
		}
	}

	private void HandleNetMsg_ApplyPay(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		XLDT_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (XLDT_ShowUI.getInstance() != null)
		{
			XLDT_ShowUI.getInstance().UpdateGameCoin();
		}
		string ch = (!flag) ? "充值被拒绝" : "充值成功";
		string en = (!flag) ? "Failure of TopUp" : "Successful TopUp";
		if (XLDT_GameUITipMngr.getInstance() != null)
		{
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.Custom, 0, ZH2_GVars.ShowTip(ch, en, string.Empty));
		}
	}

	private void HandleNetMsg_changeScoreNotice(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		XLDT_GameInfo.getInstance().User.CoinCount = ZH2_GVars.user.gameGold;
		if (XLDT_ShowUI.getInstance() != null)
		{
			XLDT_ShowUI.getInstance().UpdateGameCoin();
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
		if (XLDT_GameUITipMngr.getInstance() != null)
		{
			XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.Custom, 0, ZH2_GVars.ShowTip($"{arg}成功! 金币{num}", $"Successful {arg2}! GoldCoin {num}", string.Empty));
		}
	}

	private void DoContinueBet(object[] args)
	{
		Console.WriteLine("**********1*********DoContinueBet**************************************");
		bool flag = (bool)args[0];
		Console.WriteLine("continueFlag: " + flag);
		if (!flag && XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_GameUIMngr.GetSingleton().mBetCtrl.ErrorResetAll();
		}
		Console.WriteLine("**********2*********DoContinueBet**************************************");
	}

	private void DoHeart(object[] args)
	{
		m_CreateSocket.SendHeart();
	}

	private void DoPrintStart(object[] args)
	{
		Console.WriteLine("**********1*********DoPrintStart**************************************");
		Console.WriteLine("正在打印路单，请稍候。。。");
		Console.WriteLine("**********2*********DoPrintStart**************************************");
		XLDT_GameManager.getInstance().RecStartPrintChit(0);
	}

	private void DoPrintOver(object[] args)
	{
		Console.WriteLine("**********1*********DoPrintOver**************************************");
		Console.WriteLine("路单已打印成功，祝您游戏愉快 ！");
		Console.WriteLine("**********2*********DoPrintOver**************************************");
		XLDT_GameManager.getInstance().RecStartPrintChit(1);
	}

	private void DoPrintClearError(object[] args)
	{
		Console.WriteLine("**********1*********DoPrintClearError**************************************");
		Console.WriteLine("游戏状态正常 ！");
		Console.WriteLine("**********2*********DoPrintClearError**************************************");
		XLDT_GameManager.getInstance().RecStartPrintChit(2);
	}

	private void DoCheckRecord(object[] args)
	{
		Console.WriteLine("**********1*********DoCheckRecord**************************************");
		Console.WriteLine("进入对单状态 ！");
		Console.WriteLine("**********2*********DoCheckRecord**************************************");
		XLDT_GameManager.getInstance().RecStartPrintChit(3);
	}

	private void DoAbnormal(object[] args)
	{
		Console.WriteLine("**********1*********DoAbnormal**************************************");
		Console.WriteLine("保箱异常，游戏暂停 ！");
		Console.WriteLine("**********2*********DoAbnormal**************************************");
		XLDT_GameManager.getInstance().RecStartPrintChit(4);
	}

	private void DoCheckVersion(object[] args)
	{
	}

	private void DoNotUpdate(object[] args)
	{
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendPublicKey();
	}

	private void DoPleaseWait(object[] agrs)
	{
		int cancelYaFenBtnEnable = (int)agrs[0];
		XLDT_GameUIMngr.GetSingleton().SetCancelYaFenBtnEnable(cancelYaFenBtnEnable);
	}

	private void DoPleaseWait2(object[] agrs)
	{
		int cancelYaFenBtnEnable = 0;
		try
		{
			cancelYaFenBtnEnable = int.Parse(agrs[0].ToString());
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("Es: " + arg);
		}
		XLDT_GameUIMngr.GetSingleton().SetCancelYaFenBtnEnable(cancelYaFenBtnEnable);
	}

	private void DoPleaseWait2(int agrs)
	{
		XLDT_GameUIMngr.GetSingleton().SetCancelYaFenBtnEnable(agrs);
	}

	private void DoLeaveSeat(bool[] args)
	{
		Console.WriteLine("**********1*********DoLeaveSeat**************************************");
		bool flag = args[0];
		Console.WriteLine("bLeaveSeat: " + flag);
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			if (flag)
			{
				XLDT_GameUITipMngr.getInstance().BackToSelectSeat();
			}
			else
			{
				XLDT_GameUITipMngr.getInstance().ShowTip(XLDT_TipType.WaitForGameOver, 2, string.Empty);
			}
		}
		Console.WriteLine("**********2*********DoLeaveSeat**************************************");
	}
}
