using com.miracle9.game.bean;
using com.miracle9.game.entity;
using DP_GameCommon;
using DP_UICommon;
using System;
using System.Collections;
using UnityEngine;

public class DP_Transmit : MonoBehaviour
{
	private static DP_Transmit _MyTransmit;

	private DP_Sockets m_CreateSocket;

	private DP_DataEncrypt m_DataEncrypt;

	public static DP_Transmit GetSingleton()
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

	public void TransmitGetPoint(DP_Sockets MyCreateSocket)
	{
		m_CreateSocket = MyCreateSocket;
	}

	public void PostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] args = table["args"] as object[];
		if (LL_Parameter.G_Test)
		{
			UnityEngine.Debug.LogError("aaaaa");
			DP_ErrorManager.GetSingleton().AddError(text);
		}
		if (text == "sendServerTime")
		{
			if (DP_GameInfo.getInstance().Key != string.Empty)
			{
				UnityEngine.Debug.LogError(DP_GameInfo.getInstance().Key);
				if (!DP_NetMngr.isInLoading)
				{
					switch (DP_GameInfo.getInstance().GetAppState)
					{
					case AppState.App_On_RoomList_Panel:
						UnityEngine.Debug.LogError(1);
						m_CreateSocket.SendUserLogin(DP_GameInfo.getInstance().UserId, DP_GameInfo.getInstance().Pwd, 1, string.Empty);
						break;
					case AppState.App_On_TableList_Panel:
						m_CreateSocket.SendUserLogin(DP_GameInfo.getInstance().UserId, DP_GameInfo.getInstance().Pwd, 2, string.Empty);
						break;
					case AppState.App_On_Table:
						m_CreateSocket.SendUserLogin(DP_GameInfo.getInstance().UserId, DP_GameInfo.getInstance().Pwd, 3, string.Empty);
						break;
					case AppState.App_On_Game:
						m_CreateSocket.SendUserLogin(DP_GameInfo.getInstance().UserId, DP_GameInfo.getInstance().Pwd, 4, string.Empty);
						break;
					}
				}
				else
				{
					m_CreateSocket.SendUserLogin(DP_GameInfo.getInstance().UserId, DP_GameInfo.getInstance().Pwd, 1, string.Empty);
				}
			}
			DP_GameInfo.getInstance().Key = "login";
		}
		else if (text == "checkVersion")
		{
			DoCheckVersion(args);
		}
		else if (text == "notUpdate")
		{
			DoNotUpdate(args);
		}
		else if (text == "userLogin")
		{
			DoUserLogin(args);
		}
		else if (text == "newGameGold")
		{
			DoNewGameGold(args);
		}
		else if (text == "newExpeGold")
		{
			DoNewExpeGold(args);
		}
		else if (text == "overflow")
		{
			DoOverflow(args);
		}
		else if (text == "quitToLogin")
		{
			DoQuitToLogin(args);
		}
		else if (text == "quitToRoom")
		{
			DoQuitToRoom(args);
		}
		else if (text == "enterRoom")
		{
			DoEnterRoom(args);
		}
		else if (text == "updateRoomInfo")
		{
			DoUpdateRoomInfo(args);
		}
		else if (text == "deskOnlineNumber")
		{
			DoDeskOnlineNumber(args);
		}
		else if (text == "addExpeGoldAuto")
		{
			DoAddExpeGoldAuto(args);
		}
		else if (text == "deskInfo")
		{
			DoDeskInfo(args);
		}
		else if (text == "updateDeskInfo")
		{
			DoUpdateDeskInfo(args);
		}
		else if (text == "selectSeat")
		{
			DoSelectSeat(args);
		}
		else if (text == "updateGame")
		{
			DoUpdateGame(args);
		}
		else if (text == "gameRestart")
		{
			DoGameRestart(args);
		}
		else if (text == "gameResult")
		{
			DoGameResult(args);
		}
		else if (text == "deskTotalBet")
		{
			DoDeskTotalBet(args);
		}
		else if (text == "currentBet")
		{
			DoCurrentBet(args);
		}
		else if (text == "playerInfo")
		{
			DoPlayerInfo(args);
		}
		else if (text == "sendChat")
		{
			DoSendChat(args);
		}
		else if (text == "newGameScore")
		{
			DoNewGameScore(args);
		}
		else if (text == "newExpeScore")
		{
			DoNewExpeScore(args);
		}
		else if (text == "resultList")
		{
			DoResultList(args);
		}
		else if (text == "sendNotice")
		{
			DoSendNotice(args);
		}
		else if (text == "gameShutup")
		{
			DoGameShutup(args);
		}
		else if (text == "userShutup")
		{
			DoUserShutup(args);
		}
		else if (text == "userAward")
		{
			DoUserAward(args);
		}
		else if (!(text == "heart"))
		{
			if (text == "NetThread/NetDown")
			{
				DoNetDown(args);
			}
			else
			{
				UnityEngine.Debug.Log("No Message Type!");
			}
		}
	}

	public void SelectPostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] args = table["args"] as object[];
		if (LL_Parameter.G_Test)
		{
			DP_ErrorManager.GetSingleton().AddError(text);
		}
		if (text == "sendServerTime")
		{
			if (DP_GameInfo.getInstance().Key != string.Empty)
			{
				if (!DP_NetMngr.isInLoading)
				{
					switch (DP_GameInfo.getInstance().GetAppState)
					{
					case AppState.App_On_RoomList_Panel:
						m_CreateSocket.SendUserLogin(DP_GameInfo.getInstance().UserId, DP_GameInfo.getInstance().Pwd, 1, string.Empty);
						break;
					case AppState.App_On_TableList_Panel:
						m_CreateSocket.SendUserLogin(DP_GameInfo.getInstance().UserId, DP_GameInfo.getInstance().Pwd, 2, string.Empty);
						break;
					case AppState.App_On_Table:
						m_CreateSocket.SendUserLogin(DP_GameInfo.getInstance().UserId, DP_GameInfo.getInstance().Pwd, 3, string.Empty);
						break;
					case AppState.App_On_Game:
						m_CreateSocket.SendUserLogin(DP_GameInfo.getInstance().UserId, DP_GameInfo.getInstance().Pwd, 4, string.Empty);
						break;
					}
				}
				else
				{
					m_CreateSocket.SendUserLogin(DP_GameInfo.getInstance().UserId, DP_GameInfo.getInstance().Pwd, 1, string.Empty);
				}
			}
			DP_GameInfo.getInstance().Key = "login";
		}
		else
		{
			if (text == "userLogin")
			{
				return;
			}
			if (text == "checkVersion")
			{
				DoCheckVersion(args);
			}
			else if (text == "notUpdate")
			{
				DoNotUpdate(args);
			}
			else if (text == "newGameGold")
			{
				DoNewGameGold(args);
			}
			else if (text == "newExpeGold")
			{
				DoNewExpeGold(args);
			}
			else if (text == "overflow")
			{
				DoOverflow(args);
			}
			else if (text == "quitToLogin")
			{
				DoQuitToLogin(args);
			}
			else if (text == "quitToRoom")
			{
				DoQuitToRoom(args);
			}
			else
			{
				if (text == "enterRoom")
				{
					return;
				}
				if (text == "updateRoomInfo")
				{
					DoUpdateRoomInfo(args);
				}
				else if (text == "deskOnlineNumber")
				{
					DoDeskOnlineNumber(args);
				}
				else if (text == "addExpeGoldAuto")
				{
					DoAddExpeGoldAuto(args);
				}
				else if (text == "deskInfo")
				{
					DoDeskInfo(args);
				}
				else if (text == "updateDeskInfo")
				{
					DoUpdateDeskInfo(args);
				}
				else if (text == "selectSeat")
				{
					DoSelectSeat(args);
				}
				else if (text == "updateGame")
				{
					DoUpdateGame(args);
				}
				else
				{
					if (text == "gameRestart" || text == "gameResult" || text == "deskTotalBet")
					{
						return;
					}
					if (text == "currentBet")
					{
						DoCurrentBet(args);
					}
					else
					{
						if (text == "playerInfo" || text == "sendChat")
						{
							return;
						}
						if (text == "newGameScore")
						{
							DoNewGameScore(args);
						}
						else if (text == "newExpeScore")
						{
							DoNewExpeScore(args);
						}
						else
						{
							if (text == "resultList" || text == "sendNotice")
							{
								return;
							}
							if (text == "gameShutup")
							{
								DoGameShutup(args);
							}
							else if (text == "userShutup")
							{
								DoUserShutup(args);
							}
							else if (text == "userAward")
							{
								DoUserAward(args);
							}
							else if (!(text == "heart"))
							{
								if (text == "NetThread/NetDown")
								{
									DoNetDown(args);
								}
								else
								{
									UnityEngine.Debug.Log("No Message Type!");
								}
							}
						}
					}
				}
			}
		}
	}

	private void DoUserLogin(object[] args)
	{
		MonoBehaviour.print("DoUserLogin");
		bool flag = false;
		com.miracle9.game.entity.User user = new com.miracle9.game.entity.User();
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		bool flag2 = (bool)hashtable["isLogin"];
		int num = (int)hashtable["messageStatus"];
		UnityEngine.Debug.Log("bIsLoginFlag: " + flag2);
		UnityEngine.Debug.Log("nMessageStatus: " + num);
		UnityEngine.Debug.Log("bIsShutupFlag: " + flag);
		if (flag2)
		{
			UnityEngine.Debug.Log("MyUser.username: " + user.username);
			UnityEngine.Debug.Log("MyUser.nickname: " + user.nickname);
			UnityEngine.Debug.Log("MyUser.sex: " + user.sex);
			UnityEngine.Debug.Log("MyUser.level: " + user.level);
			UnityEngine.Debug.Log("MyUser.gameGold: " + user.gameGold);
			UnityEngine.Debug.Log("MyUser.expeGold: " + user.expeGold);
			UnityEngine.Debug.Log("MyUser.Overflow: " + user.overflow);
			UnityEngine.Debug.Log("MyUser.photoId:" + user.photoId);
			UnityEngine.Debug.Log("MyUser.gameScore:" + user.gameScore);
			UnityEngine.Debug.Log("MyUser.expeScore:" + user.expeScore);
			flag = (bool)hashtable["isShutup"];
			user = (hashtable["user"] as com.miracle9.game.entity.User);
			bool isSpecial = (bool)hashtable["special"];
			DP_PersonInfo dP_PersonInfo = new DP_PersonInfo();
			dP_PersonInfo.strId = user.username;
			dP_PersonInfo.strName = user.nickname;
			MonoBehaviour.print(dP_PersonInfo.strId + "," + dP_PersonInfo.strName);
			if (user.sex == 30007)
			{
				dP_PersonInfo.IsMale = true;
			}
			else
			{
				dP_PersonInfo.IsMale = false;
			}
			dP_PersonInfo.Level = user.level;
			dP_PersonInfo.IsGlobalFibbidChat = flag;
			DP_GameInfo.getInstance().CoinCount = user.gameGold;
			DP_GameInfo.getInstance().ExpCoinCount = user.expeGold;
			dP_PersonInfo.IconIndex = user.photoId;
			if (user.overflow == 1)
			{
				DP_TipManager.GetSingleton().ShowTip(EGameTipType.CoinOverFlow);
				dP_PersonInfo.IsOverFlow = true;
			}
			else
			{
				dP_PersonInfo.IsOverFlow = false;
				DP_GameInfo.getInstance().IsSpecial = isSpecial;
				DP_GameInfo.getInstance().UserInfo = dP_PersonInfo;
			}
		}
		else
		{
			switch (num)
			{
			case 0:
				DP_TipManager.GetSingleton().ShowTip(EGameTipType.UserIdDeleted);
				break;
			case 1:
				DP_TipManager.GetSingleton().ShowTip(EGameTipType.UserIDFrozen);
				break;
			default:
				DP_TipManager.GetSingleton().ShowTip(EGameTipType.ServerUpdate);
				break;
			}
		}
	}

	private void DoNewGameGold(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.Log("nGameGold:" + num);
		DP_GameInfo.getInstance().CoinCount = num;
	}

	private void DoNewExpeGold(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.Log("nExpeGold:" + num);
		DP_GameInfo.getInstance().ExpCoinCount = num;
	}

	private void DoEnterRoom(object[] args)
	{
		MonoBehaviour.print("EnterRoom");
		int length = (args[0] as Array).Length;
		MonoBehaviour.print("桌台总数" + length);
		if (length > 0)
		{
			DreamlandDesk[] array = new DreamlandDesk[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = (DreamlandDesk)(args[0] as Array).GetValue(i);
			}
			if (!DP_GameInfo.getInstance().UserInfo.IsOverFlow)
			{
				DP_GameInfo.getInstance().GetAppState = AppState.App_On_TableList_Panel;
				DP_GameInfo.getInstance().SceneUi.gameObject.SetActive(value: true);
				DP_GameInfo.getInstance().SceneUi.InitTableList(array, array[0].roomId);
			}
		}
		else
		{
			DP_TipManager.GetSingleton().ShowTip(EGameTipType.RoomEmpty);
			DP_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(1);
		}
	}

	private void DoUpdateRoomInfo(object[] args)
	{
		int length = (args[0] as Array).Length;
		if (length > 0)
		{
			DreamlandDesk[] array = new DreamlandDesk[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = (DreamlandDesk)(args[0] as Array).GetValue(i);
			}
			DP_GameInfo.getInstance().SceneUi.InitTableList(array, array[0].roomId);
		}
		else
		{
			UnityEngine.Debug.Log(" 此房间没有游戏桌子！！！");
		}
	}

	private void DoDeskOnlineNumber(object[] args)
	{
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		int num = (int)hashtable["deskId"];
		int num2 = (int)hashtable["onlineNumber"];
		UnityEngine.Debug.Log("nDeskId:" + num);
		UnityEngine.Debug.Log("nOnline:" + num2);
	}

	private void DoAddExpeGoldAuto(object[] args)
	{
		UnityEngine.Debug.Log("体验币不足，系统已自动赠送!!!");
		DP_TipManager.GetSingleton().ShowTip(EGameTipType.ApplyForExpCoin_Success);
	}

	private void DoDeskInfo(object[] args)
	{
		MonoBehaviour.print("DeskInfo");
		int length = (args[0] as Array).Length;
		if (length > 0)
		{
			Seat[] array = new Seat[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = (Seat)(args[0] as Array).GetValue(i);
			}
			string[] array2 = new string[length];
			int[] array3 = new int[length];
			int[] array4 = new int[length];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = string.Empty;
				array3[j] = 1;
				array4[j] = 1;
			}
			for (int k = 0; k < length; k++)
			{
				UnityEngine.Debug.LogError("id:" + array[k].id);
				UnityEngine.Debug.LogError("isFree:" + array[k].isFree);
				UnityEngine.Debug.LogError("userId:" + array[k].userId);
				UnityEngine.Debug.LogError("userNickname:" + array[k].userNickname);
				UnityEngine.Debug.LogError("userSex:" + array[k].userSex);
				UnityEngine.Debug.LogError("PhotoID:" + array[k].photoId);
				if (!array[k].isFree)
				{
					array2[array[k].id - 1] = array[k].userNickname;
					array4[array[k].id - 1] = array[k].photoId;
					array3[array[k].id - 1] = array[k].userId;
				}
			}
			int tableId = DP_GameInfo.getInstance().UserInfo.TableId;
			DP_GameInfo.getInstance().SetTableInfo(tableId, array3, array2, array4);
			DP_GameInfo.getInstance().GetAppState = AppState.App_On_Table;
			DP_GameInfo.getInstance().SceneUi.EnterDesk();
		}
		else
		{
			UnityEngine.Debug.Log("该游戏桌子上没有座位!!!");
		}
	}

	private void DoUpdateDeskInfo(object[] args)
	{
		int length = (args[0] as Array).Length;
		if (length > 0)
		{
			Seat[] array = new Seat[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = (Seat)(args[0] as Array).GetValue(i);
			}
			string[] array2 = new string[length];
			int[] array3 = new int[length];
			int[] array4 = new int[length];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = string.Empty;
				array3[j] = 1;
				array4[j] = 1;
			}
			for (int k = 0; k < length; k++)
			{
				UnityEngine.Debug.LogError("id:" + array[k].id);
				UnityEngine.Debug.LogError("isFree:" + array[k].isFree);
				UnityEngine.Debug.LogError("userId:" + array[k].userId);
				UnityEngine.Debug.LogError("userNickname:" + array[k].userNickname);
				UnityEngine.Debug.LogError("userSex:" + array[k].userSex);
				UnityEngine.Debug.LogError("PhotoID:" + array[k].photoId);
				if (!array[k].isFree)
				{
					array2[array[k].id - 1] = array[k].userNickname;
					array4[array[k].id - 1] = array[k].photoId;
					array3[array[k].id - 1] = array[k].userId;
				}
			}
			int tableId = DP_GameInfo.getInstance().UserInfo.TableId;
			DP_GameInfo.getInstance().SetTableInfo(tableId, array3, array2, array4);
			DP_GameInfo.getInstance().UpdateTable();
		}
		else
		{
			UnityEngine.Debug.Log("该游戏桌子上没有座位!!!");
		}
	}

	private void DoSelectSeat(object[] args)
	{
		MonoBehaviour.print("EnterGame");
		if (DP_GameInfo.getInstance().GetAppState == AppState.App_On_Table)
		{
			Hashtable hashtable = new Hashtable();
			hashtable = (args[0] as Hashtable);
			bool flag = (bool)hashtable["success"];
			UnityEngine.Debug.Log("bIsEnterFlag:" + flag);
			if (flag)
			{
				DP_GameInfo.getInstance().betTime = (int)hashtable["betTime"];
				int num = (int)hashtable["zxh"];
				DP_GameInfo.getInstance().pointerLocation = (int)hashtable["pointerLocation"];
				int num2 = (int)hashtable["awardType"];
				DP_GameInfo.getInstance().colors = (int[])hashtable["colors"];
				DP_GameInfo.getInstance().beilv = (int[])hashtable["beilv"];
				DP_GameData.waitTime = (int)hashtable["awardTime"];
				DP_GameInfo.getInstance().SceneUi.EnterGame();
			}
		}
	}

	private void DoUpdateGame(object[] args)
	{
		int[] array = new int[24];
		int[] array2 = new int[12];
		int[] array3 = new int[12];
		int[] array4 = new int[12];
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		int num = (int)hashtable["betTime"];
		int num2 = (int)hashtable["zxh"];
		int num3 = (int)hashtable["pointerLocation"];
		int num4 = (int)hashtable["awardType"];
		array = (int[])hashtable["colors"];
		array2 = (int[])hashtable["beilv"];
		array3 = (int[])hashtable["singleBets"];
		array4 = (int[])hashtable["totalBets"];
		UnityEngine.Debug.Log("nBetTime:" + num);
		UnityEngine.Debug.Log("nZXH:" + num2);
		UnityEngine.Debug.Log("nPointerLocation:" + num3);
		UnityEngine.Debug.Log("nAwardType:" + num4);
		UnityEngine.Debug.Log("nColors:");
		for (int i = 0; i < 24; i++)
		{
			Console.Write(" " + array[i]);
		}
		UnityEngine.Debug.Log("nBeiLvs:");
		for (int j = 0; j < 12; j++)
		{
			Console.Write(" " + array2[j]);
		}
		UnityEngine.Debug.Log("nSingleBet:");
		for (int k = 0; k < 12; k++)
		{
			Console.Write(" " + array3[k]);
		}
		UnityEngine.Debug.Log("nTotalBet:");
		for (int l = 0; l < 12; l++)
		{
			Console.Write(" " + array4[l]);
		}
		DP_GameInfo.getInstance().SceneGame.sptBet.SetAnimalPower(array2);
		DP_GameInfo.getInstance().SceneGame.sptBet.SetAnimalBet(array4, bIsTotal: true);
		DP_GameInfo.getInstance().SceneGame.sptBet.SetAnimalBet(array3, bIsTotal: false);
		DP_GameCtrl.GetSingleton().Reset();
		DP_GameCtrl.GetSingleton().animalColorCtrl.SetColorIndexs(array);
		DP_GameCtrl.GetSingleton().animalColorCtrl.SetPointToIndex(num3);
		UnityEngine.Debug.Log("gameinfoUpdate");
	}

	private void DoGameRestart(object[] args)
	{
		if (DP_GameInfo.getInstance().GetAppState == AppState.App_On_Game)
		{
			int[] array = new int[24];
			int[] array2 = new int[12];
			Hashtable hashtable = new Hashtable();
			hashtable = (args[0] as Hashtable);
			int num = (int)hashtable["zxh"];
			int num2 = (int)hashtable["pointerLocation"];
			int num3 = (int)hashtable["awardType"];
			array = (int[])hashtable["colors"];
			array2 = (int[])hashtable["beilv"];
			UnityEngine.Debug.Log("nZXH:" + num);
			UnityEngine.Debug.Log("nPointerLocation:" + num2);
			UnityEngine.Debug.Log("nAwardType:" + num3);
			Console.Write("nColor:");
			for (int i = 0; i < 24; i++)
			{
				Console.Write(" " + array[i]);
			}
			Console.Write("nBeiLv:");
			for (int j = 0; j < 12; j++)
			{
				Console.Write(" " + array2[j]);
			}
			DP_GameData.bet = 0;
			DP_GameInfo.getInstance().SceneGame.sptHud.SetGameCD(29);
			DP_GameInfo.getInstance().SceneGame.sptBet.SetAnimalPower(array2);
			DP_GameInfo.getInstance().SceneGame.sptBet.ClearAllBet();
			if (DP_GameInfo.getInstance().SceneGame.sptBet.bAuto)
			{
				DP_GameInfo.getInstance().SceneGame.sptBet.AutoBet();
			}
			if (!DP_GameInfo.getInstance().SceneGame.sptBet.bShowBet)
			{
				DP_GameInfo.getInstance().SceneGame.ClickBtnBet();
			}
			DP_MusicMngr.GetSingleton().Reset();
			DP_GameCtrl.GetSingleton().Reset();
			DP_GameCtrl.GetSingleton().animalColorCtrl.SetColorIndexs(array);
		}
	}

	private void DoGameResult(object[] args)
	{
		if (DP_GameInfo.getInstance().GetAppState != AppState.App_On_Game)
		{
			return;
		}
		DreamlandDeskResult dreamlandDeskResult = new DreamlandDeskResult();
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		int[] array = (int[])hashtable["currentBets"];
		int[] array2 = (int[])hashtable["colors"];
		Income[] array3 = (Income[])hashtable["incomeInfo"];
		int num = (int)hashtable["addScore"];
		int blance = num - DP_GameData.bet;
		dreamlandDeskResult = (DreamlandDeskResult)hashtable["deskResult"];
		DP_GameData.times = dreamlandDeskResult.lightningBeilv;
		int num2 = array.Length;
		int num3 = array2.Length;
		int num4 = array3.Length;
		DP_GameInfo.getInstance().SceneGame.sptBet.SetAnimalBet(array, bIsTotal: false);
		if (DP_GameInfo.getInstance().SceneGame.sptBet.bShowBet)
		{
			DP_GameInfo.getInstance().SceneGame.ClickBtnBet();
		}
		int[] array4 = new int[8];
		for (int i = 0; i < array3.Length; i++)
		{
			array4[array3[i].seatId - 1] = array3[i].score;
		}
		DP_GameInfo.getInstance().SceneGame.sptResult.SetScoreAndBlance(num, blance);
		DP_GameInfo.getInstance().CurrentWin = num;
		DP_GameInfo.getInstance().SceneGame.sptHud.SetBonusNumber(dreamlandDeskResult.awardGold);
		if (DP_GameData.times > 0)
		{
			DP_GameInfo.getInstance().SceneGame.sptHud.ShowObjDouble(bShow: true);
			DP_GameInfo.getInstance().SceneGame.sptHud.PlayDoubleAnim(bPlay: true);
		}
		if (dreamlandDeskResult.type == 0)
		{
			DP_GameInfo.getInstance().SceneGame.sptResult.SetAnimalResult(dreamlandDeskResult.animal);
		}
		else if (dreamlandDeskResult.type == 1)
		{
			if (dreamlandDeskResult.globalType == 0)
			{
				int[] array5 = new int[1]
				{
					dreamlandDeskResult.animal
				};
				DP_GameInfo.getInstance().SceneGame.sptHud.SetBonusNumber(dreamlandDeskResult.awardGold, bIsBonusAward: true);
				DP_GameInfo.getInstance().SceneGame.sptResult.SetAnimalResult(dreamlandDeskResult.animal);
			}
			if (dreamlandDeskResult.globalType == 1)
			{
				int[] array6 = new int[1]
				{
					dreamlandDeskResult.animal
				};
				DP_GameInfo.getInstance().SceneGame.sptResult.SetAnimalResult(dreamlandDeskResult.animal);
			}
			else if (dreamlandDeskResult.globalType != 2 && dreamlandDeskResult.globalType != 3 && dreamlandDeskResult.globalType != 4)
			{
			}
		}
		else if (dreamlandDeskResult.type == 2)
		{
			if (dreamlandDeskResult.luckType == 0)
			{
				DP_GameInfo.getInstance().SceneGame.sptHud.SetBonusNumber(dreamlandDeskResult.awardGold, bIsBonusAward: true);
				DP_GameInfo.getInstance().SceneGame.sptResult.SetBonus(dreamlandDeskResult.luckNum);
			}
			else if (dreamlandDeskResult.luckType != 1)
			{
			}
			if (dreamlandDeskResult.luckType != 2)
			{
			}
		}
		if (dreamlandDeskResult.type == 0)
		{
			UnityEngine.Debug.Log("---------------NormalPrize--Go!!---------------------");
			int animal = dreamlandDeskResult.animal;
			AnimalType aType = _ConvertIntAnimal(animal);
			DP_GameCtrl.GetSingleton().GoOneAnimal(aType, array2[0], 24f, 15f);
		}
		else if (dreamlandDeskResult.type == 1)
		{
			if (dreamlandDeskResult.luckType == 0)
			{
				int[] array7 = new int[2];
				int[] array8 = new int[2];
				array7[0] = (int)_ConvertIntAnimal(dreamlandDeskResult.animal);
				array7[1] = (int)_ConvertIntAnimal(dreamlandDeskResult.luckAnimal);
				DP_GameCtrl.GetSingleton().GoBonus((AnimalType)array7[0], dreamlandDeskResult.awardGold, dreamlandDeskResult.luckNum, array2[0]);
			}
		}
		else if (dreamlandDeskResult.songDengCount <= 0)
		{
			int animal2 = dreamlandDeskResult.animal;
			AnimalType aType2 = (animal2 / 3 != 0) ? ((animal2 / 3 == 1) ? AnimalType.Panda : ((animal2 / 3 != 2) ? AnimalType.Rabbit : AnimalType.Monkey)) : AnimalType.Lion;
			DP_GameCtrl.GetSingleton().GoOneAnimal(aType2, array2[0]);
		}
	}

	private void DoDeskTotalBet(object[] args)
	{
		int[] array = (int[])args[0];
		int num = array.Length;
		UnityEngine.Debug.Log("deskTotalBet：");
		for (int i = 0; i < num; i++)
		{
			Console.Write(" " + array[i]);
		}
		DP_GameInfo.getInstance().SceneGame.sptBet.SetAnimalBet(array, bIsTotal: true);
	}

	private void DoCurrentBet(object[] args)
	{
		int[] array = (int[])args[0];
		int num = array.Length;
		DP_GameInfo.getInstance().SceneGame.sptBet.SetAnimalBet(array, bIsTotal: false, isLastBet: false);
	}

	private void DoNewGameScore(object[] args)
	{
		int gameScore = (int)args[0];
		UnityEngine.Debug.Log("gameScore:" + args[0].ToString());
		DP_GameInfo.getInstance().GameScore = gameScore;
	}

	private void DoNewExpeScore(object[] args)
	{
		int gameScore = int.Parse(args[0].ToString());
		DP_GameInfo.getInstance().GameScore = gameScore;
	}

	private void DoPlayerInfo(object[] args)
	{
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		int num = (int)hashtable["seatId"];
		string str = (string)hashtable["nickname"];
		int num2 = (int)hashtable["userGameScore"];
		int num3 = (int)hashtable["userLevel"];
		UnityEngine.Debug.Log("nSeatId:" + num);
		UnityEngine.Debug.Log("strNickName:" + str);
		UnityEngine.Debug.Log("nUserGameScore:" + num2);
		UnityEngine.Debug.Log("nUserLeave:" + num3);
		if (DP_GameInfo.getInstance().GetAppState != AppState.App_On_Game && DP_GameInfo.getInstance().GetAppState != AppState.App_On_Table)
		{
		}
	}

	private void DoSendChat(object[] args)
	{
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		int num = (int)hashtable["chatType"];
		int num2 = (int)hashtable["senderSeatId"];
		string str = (string)hashtable["chatMessage"];
		UnityEngine.Debug.Log("nChatType:" + num);
		UnityEngine.Debug.Log("nSenderSeatId:" + num2);
		UnityEngine.Debug.Log("strChatMessage:" + str);
	}

	private void DoResultList(object[] args)
	{
		DPDeskRecord[] array = (DPDeskRecord[])(args[0] as Array);
		DP_GameInfo.getInstance().SceneGame.sptHud.animalRecord.AnimalRecordAnim(array);
		DP_GameInfo.getInstance().SceneGame.sptRecord.SetRecord(array);
	}

	private void DoSendNotice(object[] args)
	{
		string text = (string)args[0];
		UnityEngine.Debug.Log("strMessage:" + text);
		if (DP_GameInfo.getInstance().GetAppState == AppState.App_On_Game)
		{
			DP_GameInfo.getInstance().SceneGame.sptHud.AddNotice(text);
		}
	}

	private void DoOverflow(object[] args)
	{
		UnityEngine.Debug.Log("爆机啦...");
		DP_TipManager.GetSingleton().ShowTip(EGameTipType.CoinOverFlow);
	}

	private void DoQuitToLogin(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.Log("nQuitType:" + num);
		switch (num)
		{
		case 1:
			DP_TipManager.GetSingleton().ShowTip(EGameTipType.ServerUpdate);
			break;
		case 2:
			DP_TipManager.GetSingleton().ShowTip(EGameTipType.Game_UserIdFrozen);
			break;
		case 3:
			DP_TipManager.GetSingleton().ShowTip(EGameTipType.UserIdDeleted);
			break;
		case 4:
			DP_TipManager.GetSingleton().ShowTip(EGameTipType.UserIdRepeative);
			DP_Sockets.GetSingleton().isReconnect = false;
			break;
		case 5:
			DP_TipManager.GetSingleton().ShowTip(EGameTipType.UserPwdChanged);
			break;
		case 6:
			DP_TipManager.GetSingleton().ShowTip(EGameTipType.LoseTheServer);
			break;
		}
	}

	private void DoQuitToRoom(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.Log("nQuitType:" + num);
		switch (num)
		{
		case 1:
			DP_TipManager.GetSingleton().ShowTip(EGameTipType.TableDeleted_Game);
			break;
		case 2:
			DP_TipManager.GetSingleton().ShowTip(EGameTipType.TableParameterModified);
			break;
		case 3:
			DP_TipManager.GetSingleton().ShowTip(EGameTipType.LongTimeNoOperate);
			break;
		}
	}

	private void DoGameShutup(object[] args)
	{
		bool flag = (bool)args[0];
		UnityEngine.Debug.Log("bGameShutup:" + flag);
		DP_GameInfo.getInstance().UserInfo.IsGlobalFibbidChat = flag;
	}

	private void DoUserShutup(object[] args)
	{
		bool flag = (bool)args[0];
		UnityEngine.Debug.Log("bUserShutup:" + flag);
		DP_GameInfo.getInstance().UserInfo.IsSelfFibbidChat = flag;
	}

	private void DoUserAward(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.Log("nGold:" + num);
		DP_TipManager.GetSingleton().ShowTip(EGameTipType.GivingCoin);
	}

	private void DoNetDown(object[] args)
	{
		if (!DP_Sockets.GetSingleton().isReconnect)
		{
			return;
		}
		if (m_CreateSocket.GetRelineCount() < 5)
		{
			m_CreateSocket.CreateReceiveThread();
			return;
		}
		UnityEngine.Debug.LogError("30秒重连失败，网络断开，请重新登录网络大厅...");
		if (DP_GameInfo.getInstance().GetAppState != AppState.APP_NET_ERROR)
		{
			DP_GameInfo.getInstance().GetAppState = AppState.APP_NET_ERROR;
			DP_TipManager.GetSingleton().ShowTip(EGameTipType.Net_ConnectionError);
		}
	}

	private void DoHeart(object[] args)
	{
		m_CreateSocket.SendHeart();
	}

	private void DoLogoffNotice(object[] args)
	{
		UnityEngine.Debug.Log("此号码重复登录");
		DP_TipManager.GetSingleton().ShowTip(EGameTipType.UserIdRepeative);
	}

	private AnimalType _ConvertIntAnimal(int newTyp)
	{
		if (newTyp / 3 == 0)
		{
			return AnimalType.Lion;
		}
		if (newTyp / 3 == 1)
		{
			return AnimalType.Panda;
		}
		if (newTyp / 3 == 2)
		{
			return AnimalType.Monkey;
		}
		return AnimalType.Rabbit;
	}

	private void DoCheckVersion(object[] args)
	{
		UnityEngine.Debug.Log("**********1*********DoCheckVersion**************************************");
		string str = (string)args[0];
		UnityEngine.Debug.Log("haveNewVersionIDFlag: " + str);
		UnityEngine.Debug.Log("**********1*********DoCheckVersion**************************************");
	}

	private void DoNotUpdate(object[] args)
	{
		UnityEngine.Debug.Log("DoNotUpdate");
		DP_NetMngr.GetSingleton().MyCreateSocket.sendPublicKey();
	}

	private ArrayList _ConvertStringSongDeng(string moreInfo)
	{
		ArrayList arrayList = new ArrayList();
		string text = string.Empty;
		for (int i = 0; i < moreInfo.Length; i++)
		{
			if (moreInfo[i] != ',')
			{
				text += moreInfo[i];
			}
			else
			{
				int newTyp = Convert.ToInt32(text);
				arrayList.Add(_ConvertIntAnimal(newTyp));
				text = string.Empty;
			}
			if (i == moreInfo.Length - 1)
			{
				int newTyp2 = Convert.ToInt32(text);
				arrayList.Add(_ConvertIntAnimal(newTyp2));
			}
		}
		return arrayList;
	}

	private ArrayList _ConvertStringSongDengforUI(string moreInfo)
	{
		ArrayList arrayList = new ArrayList();
		string text = string.Empty;
		for (int i = 0; i < moreInfo.Length; i++)
		{
			if (moreInfo[i] != ',')
			{
				text += moreInfo[i];
			}
			else
			{
				int num = Convert.ToInt32(text);
				arrayList.Add(num);
				text = string.Empty;
			}
			if (i == moreInfo.Length - 1)
			{
				int num2 = Convert.ToInt32(text);
				arrayList.Add(num2);
			}
		}
		return arrayList;
	}
}
