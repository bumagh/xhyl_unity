using com.miracle9.game.bean;
using com.miracle9.game.entity;
using DG.Tweening;
using JsonFx.Json;
using LitJson;
using LL_GameCommon;
using LL_UICommon;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LL_Transmit : MonoBehaviour
{
	private static LL_Transmit _MyTransmit;

	private LL_Sockets m_CreateSocket;

	private LL_DataEncrypt m_DataEncrypt;

	public static LL_Transmit GetSingleton()
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

	public void TransmitGetPoint(LL_Sockets MyCreateSocket)
	{
		m_CreateSocket = MyCreateSocket;
	}

	public void PostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] array = table["args"] as object[];
		try
		{
			if (text != "newGameGold")
			{
				UnityEngine.Debug.LogError("收到2: " + text + "  " + JsonFx.Json.JsonWriter.Serialize(array));
			}
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError(text + " 错误: " + arg);
		}
		if (text == "sendServerTime")
		{
			if (LL_GameInfo.getInstance().Key != string.Empty)
			{
				if (!LL_NetMngr.isInLoading)
				{
					switch (LL_AppUIMngr.GetSingleton().GetAppState())
					{
					case AppState.App_On_RoomList_Panel:
						m_CreateSocket.SendUserLogin(LL_GameInfo.getInstance().UserId, LL_GameInfo.getInstance().Pwd, 1, string.Empty);
						break;
					case AppState.App_On_TableList_Panel:
						m_CreateSocket.SendUserLogin(LL_GameInfo.getInstance().UserId, LL_GameInfo.getInstance().Pwd, 2, string.Empty);
						break;
					case AppState.App_On_Table:
						m_CreateSocket.SendUserLogin(LL_GameInfo.getInstance().UserId, LL_GameInfo.getInstance().Pwd, 3, string.Empty);
						break;
					case AppState.App_On_Game:
						m_CreateSocket.SendUserLogin(LL_GameInfo.getInstance().UserId, LL_GameInfo.getInstance().Pwd, 4, string.Empty);
						break;
					}
				}
				else
				{
					m_CreateSocket.SendUserLogin(LL_GameInfo.getInstance().UserId, LL_GameInfo.getInstance().Pwd, 1, string.Empty);
				}
			}
			LL_GameInfo.getInstance().Key = "login";
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
		else if (text == "newGameGold")
		{
			DoNewGameGold(array);
		}
		else if (text == "newExpeGold")
		{
			DoNewExpeGold(array);
		}
		else if (text == "overflow")
		{
			DoOverflow(array);
		}
		else if (text == "quitToLogin")
		{
			DoQuitToLogin(array);
		}
		else if (text == "quitToRoom")
		{
			DoQuitToRoom(array);
		}
		else if (text == "quitToDesk")
		{
			DoQuitToDesk(array);
		}
		else if (text == "enterRoom")
		{
			DoEnterRoomNew(array);
		}
		else if (text == "selectHall")
		{
			DoSelectHall(array);
		}
		else
		{
			if (text == "seatInfo")
			{
				return;
			}
			if (text == "updateRoomInfo")
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
			else if (text == "addExpeGoldAuto")
			{
				DoAddExpeGoldAuto(array);
			}
			else if (text == "deskInfo")
			{
				DoDeskInfo(array);
			}
			else if (text == "updateDeskInfo")
			{
				DoUpdateDeskInfo(array);
			}
			else if (text == "selectSeat")
			{
				DoSelectSeat(array);
			}
			else if (text == "updateGame")
			{
				DoUpdateGame(array);
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
				DoNewGameScore(array);
			}
			else if (text == "newExpeScore")
			{
				DoNewExpeScore(array);
			}
			else if (text == "resultList")
			{
				DoResultList(array);
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
			else if (text == "sendNotice")
			{
				DoSendNotice(array);
			}
			else if (text == "gameShutup")
			{
				DoGameShutup(array);
			}
			else if (text == "userShutup")
			{
				DoUserShutup(array);
			}
			else if (text == "userAward")
			{
				DoUserAward(array);
			}
			else if (!(text == "heart"))
			{
				if (text == "NetThread/NetDown")
				{
					DoNetDown(array);
				}
				else
				{
					Console.WriteLine("No Message Type!");
				}
			}
		}
	}

	public void SelectPostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] args = table["args"] as object[];
		if (LL_Parameter.G_Test)
		{
			LL_ErrorManager.GetSingleton().AddError(text);
		}
		if (text == "sendServerTime")
		{
			if (LL_GameInfo.getInstance().Key != string.Empty)
			{
				if (!LL_NetMngr.isInLoading)
				{
					switch (LL_AppUIMngr.GetSingleton().GetAppState())
					{
					case AppState.App_On_RoomList_Panel:
						m_CreateSocket.SendUserLogin(LL_GameInfo.getInstance().UserId, LL_GameInfo.getInstance().Pwd, 1, string.Empty);
						break;
					case AppState.App_On_TableList_Panel:
						m_CreateSocket.SendUserLogin(LL_GameInfo.getInstance().UserId, LL_GameInfo.getInstance().Pwd, 2, string.Empty);
						break;
					case AppState.App_On_Table:
						m_CreateSocket.SendUserLogin(LL_GameInfo.getInstance().UserId, LL_GameInfo.getInstance().Pwd, 3, string.Empty);
						break;
					case AppState.App_On_Game:
						m_CreateSocket.SendUserLogin(LL_GameInfo.getInstance().UserId, LL_GameInfo.getInstance().Pwd, 4, string.Empty);
						break;
					}
				}
				else
				{
					m_CreateSocket.SendUserLogin(LL_GameInfo.getInstance().UserId, LL_GameInfo.getInstance().Pwd, 1, string.Empty);
				}
			}
			LL_GameInfo.getInstance().Key = "login";
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
									Console.WriteLine("No Message Type!");
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
		bool flag = false;
		com.miracle9.game.entity.User user = new com.miracle9.game.entity.User();
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		bool flag2 = (bool)hashtable["isLogin"];
		int num = (int)hashtable["messageStatus"];
		UnityEngine.Debug.LogError("====登录状态====" + num + " 登录成功: " + flag2);
		if (flag2)
		{
			flag = (bool)hashtable["isShutup"];
			user = (hashtable["user"] as com.miracle9.game.entity.User);
			ZH2_GVars.hallInfo2 = new JsonData();
			ZH2_GVars.hallInfo2 = jsonData["hallInfo"];
			bool isSpecial = (bool)hashtable["special"];
			LL_PersonInfo component = LL_GameInfo.getInstance().LoadScene.UIRoot.GetComponent<LL_PersonInfo>();
			if (user.sex == 30007)
			{
				component.IsMale = true;
			}
			else
			{
				component.IsMale = false;
			}
			component.Level = user.level;
			component.IsGlobalFibbidChat = flag;
			component.CoinCount = user.gameGold;
			component.ExpCoinCount = user.expeGold;
			component.IconIndex = user.photoId;
			ZH2_GVars.userId = user.id;
			if (user.overflow == 1)
			{
				if (LL_NetMngr.isInLoading)
				{
					UnityEngine.Debug.LogError(LL_LoadTip.tipType.OverFlow);
					LL_LoadTip.getInstance().showTip(LL_LoadTip.tipType.OverFlow, string.Empty);
				}
				else
				{
					LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.CoinOverFlow, string.Empty);
				}
				component.IsOverFlow = true;
				ZH2_GVars.isStartedFromGame = true;
				UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
				AssetBundleManager.GetInstance().UnloadAB("LuckyLion");
				SceneManager.LoadScene("MainScene");
			}
			else
			{
				component.IsOverFlow = false;
				LL_GameInfo.getInstance().IsSpecial = isSpecial;
				LL_GameInfo.getInstance().UserInfo = component;
			}
			return;
		}
		switch (num)
		{
		case 0:
			try
			{
				LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIdDeleted, string.Empty);
			}
			catch (Exception message)
			{
				LL_LoadTip.getInstance().showTip(LL_LoadTip.tipType.UserIdDeleted, string.Empty);
				UnityEngine.Debug.LogError(message);
			}
			break;
		case 1:
			if (LL_NetMngr.isInLoading)
			{
				LL_LoadTip.getInstance().showTip(LL_LoadTip.tipType.IdIsFrozen, string.Empty);
			}
			else
			{
				LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIDFrozen, string.Empty);
			}
			break;
		default:
			if (LL_NetMngr.isInLoading)
			{
				LL_LoadTip.getInstance().showTip(LL_LoadTip.tipType.ServerStop, string.Empty);
			}
			else
			{
				LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.ServerUpdate, string.Empty);
			}
			break;
		}
	}

	private void DoNewGameGold(object[] args)
	{
		int num = (int)args[0];
		Console.WriteLine("nGameGold:" + num);
		LL_GameInfo.getInstance().UserInfo.CoinCount = num;
	}

	private void DoNewExpeGold(object[] args)
	{
		int num = (int)args[0];
		Console.WriteLine("nExpeGold:" + num);
		LL_GameInfo.getInstance().UserInfo.ExpCoinCount = num;
	}

	private void DoEnterRoomNew(object[] args)
	{
	}

	private void DoSelectHall(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		UpdateOnline(jsonData);
		int count = jsonData["deskInfo"].Count;
		if (count > 0)
		{
			LL_Desk[] array = new LL_Desk[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = new LL_Desk();
				array[i].id = (int)jsonData["deskInfo"][i]["id"];
				array[i].roomId = (int)jsonData["deskInfo"][i]["roomId"];
				array[i].name = (string)jsonData["deskInfo"][i]["name"];
				array[i].autoKick = (int)jsonData["deskInfo"][i]["autoKick"];
				array[i].minGold = (int)jsonData["deskInfo"][i]["minGold"];
				array[i].exchange = (int)jsonData["deskInfo"][i]["exchange"];
				array[i].onceExchangeValue = (int)jsonData["deskInfo"][i]["onceExchangeValue"];
				array[i].orderBy = (int)jsonData["deskInfo"][i]["orderBy"];
				array[i].siteType = (int)jsonData["deskInfo"][i]["siteType"];
				array[i].min_zxh = (int)jsonData["deskInfo"][i]["min_zxh"];
				array[i].minBet = (int)jsonData["deskInfo"][i]["minBet"];
				array[i].beilvType = (int)jsonData["deskInfo"][i]["beilvType"];
				array[i].pourWater = (int)jsonData["deskInfo"][i]["pourWater"];
				array[i].beilvModel = (int)jsonData["deskInfo"][i]["beilvModel"];
				array[i].onlineNumber = (int)jsonData["deskInfo"][i]["onlineNumber"];
				array[i].zxhDiff = (int)jsonData["deskInfo"][i]["zxhDiff"];
				array[i].betTime = (int)jsonData["deskInfo"][i]["betTime"];
				array[i].animalDiff = (int)jsonData["deskInfo"][i]["animalDiff"];
				array[i].maxBet = (int)jsonData["deskInfo"][i]["maxBet"];
				array[i].max_h = (int)jsonData["deskInfo"][i]["max_h"];
				array[i].drawWater = (int)jsonData["deskInfo"][i]["drawWater"];
				array[i].max_zx = (int)jsonData["deskInfo"][i]["max_zx"];
				try
				{
					array[i].seats = new Seat[8];
					for (int j = 0; j < 8; j++)
					{
						array[i].seats[j] = new Seat();
						array[i].seats[j].id = (int)jsonData["deskInfo"][i]["seats"][j]["id"];
						array[i].seats[j].isFree = (bool)jsonData["deskInfo"][i]["seats"][j]["isFree"];
						array[i].seats[j].photoId = (int)jsonData["deskInfo"][i]["seats"][j]["photoId"];
						array[i].seats[j].userId = (int)jsonData["deskInfo"][i]["seats"][j]["userId"];
						array[i].seats[j].userNickname = (string)jsonData["deskInfo"][i]["seats"][j]["userNickname"];
						array[i].seats[j].userSex = '0';
					}
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
				}
			}
			array = Test(array);
			LL_AppUIMngr.GetSingleton().mTableList.InitTableList(array, array[0].roomId);
			LL_AppUIMngr.GetSingleton().InItTable(array, array[0].roomId);
			LL_AppUIMngr.GetSingleton().SetNoHall(isHavHall: true);
			if (LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_RoomList_Panel && !LL_GameInfo.getInstance().UserInfo.IsOverFlow)
			{
				LL_AppUIMngr.GetSingleton().SetAppState(AppState.App_On_TableList_Panel);
				LL_AppUIMngr.GetSingleton().mTableList.ShowTableList();
			}
		}
		else
		{
			UnityEngine.Debug.LogError("该房间没有桌子");
			LL_AppUIMngr.GetSingleton().SetNoHall(isHavHall: false);
			LL_AppUIMngr.GetSingleton().mTableList.InitTableList(null, 0);
			LL_AppUIMngr.GetSingleton().InItTable(null, 0);
		}
	}

	private LL_Desk[] Test(LL_Desk[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = 0; j < array.Length - 1; j++)
			{
				if (array[j].id > array[j + 1].id)
				{
					LL_Desk lL_Desk = array[j];
					array[j] = array[j + 1];
					array[j + 1] = lL_Desk;
				}
			}
		}
		return array;
	}

	private void UpdateOnline(JsonData dictionary)
	{
		if (dictionary == null)
		{
			UnityEngine.Debug.LogError("dictionary为空");
			return;
		}
		JsonData jsonData = dictionary["onlineNumber"];
		if (LL_AppUIMngr.GetSingleton() != null)
		{
			for (int i = 0; i < jsonData.Count; i++)
			{
				for (int j = 0; j < LL_AppUIMngr.GetSingleton().selectBtnList.Count; j++)
				{
					if (i == LL_AppUIMngr.GetSingleton().selectBtnList[j].hallId)
					{
						LL_AppUIMngr.GetSingleton().selectBtnList[j].onlinePeople = jsonData[i.ToString()].ToString();
						LL_AppUIMngr.GetSingleton().selectBtnList[j].UpdateText();
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

	private void DoSeatInfo(object[] args)
	{
		int length = (args[0] as Array).Length;
		if (length > 0)
		{
			Seat[] array = new Seat[length];
		}
	}

	private void DoUpdateRoomInfo(object[] args)
	{
	}

	private void DoUpdateHallInfo(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		UpdateOnline(jsonData);
		int count = jsonData["deskInfo"].Count;
		if (count > 0)
		{
			LL_Desk[] array = new LL_Desk[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = new LL_Desk();
				array[i].id = (int)jsonData["deskInfo"][i]["id"];
				array[i].roomId = (int)jsonData["deskInfo"][i]["roomId"];
				array[i].name = (string)jsonData["deskInfo"][i]["name"];
				array[i].autoKick = (int)jsonData["deskInfo"][i]["autoKick"];
				array[i].minGold = (int)jsonData["deskInfo"][i]["minGold"];
				array[i].exchange = (int)jsonData["deskInfo"][i]["exchange"];
				array[i].onceExchangeValue = (int)jsonData["deskInfo"][i]["onceExchangeValue"];
				array[i].orderBy = (int)jsonData["deskInfo"][i]["orderBy"];
				array[i].siteType = (int)jsonData["deskInfo"][i]["siteType"];
				array[i].min_zxh = (int)jsonData["deskInfo"][i]["min_zxh"];
				array[i].minBet = (int)jsonData["deskInfo"][i]["minBet"];
				array[i].beilvType = (int)jsonData["deskInfo"][i]["beilvType"];
				array[i].pourWater = (int)jsonData["deskInfo"][i]["pourWater"];
				array[i].beilvModel = (int)jsonData["deskInfo"][i]["beilvModel"];
				array[i].onlineNumber = (int)jsonData["deskInfo"][i]["onlineNumber"];
				array[i].zxhDiff = (int)jsonData["deskInfo"][i]["zxhDiff"];
				array[i].betTime = (int)jsonData["deskInfo"][i]["betTime"];
				array[i].animalDiff = (int)jsonData["deskInfo"][i]["animalDiff"];
				array[i].maxBet = (int)jsonData["deskInfo"][i]["maxBet"];
				array[i].max_h = (int)jsonData["deskInfo"][i]["max_h"];
				array[i].drawWater = (int)jsonData["deskInfo"][i]["drawWater"];
				array[i].max_zx = (int)jsonData["deskInfo"][i]["max_zx"];
				try
				{
					array[i].seats = new Seat[8];
					for (int j = 0; j < 8; j++)
					{
						array[i].seats[j] = new Seat();
						array[i].seats[j].id = (int)jsonData["deskInfo"][i]["seats"][j]["id"];
						array[i].seats[j].isFree = (bool)jsonData["deskInfo"][i]["seats"][j]["isFree"];
						array[i].seats[j].photoId = (int)jsonData["deskInfo"][i]["seats"][j]["photoId"];
						array[i].seats[j].userId = (int)jsonData["deskInfo"][i]["seats"][j]["userId"];
						array[i].seats[j].userNickname = (string)jsonData["deskInfo"][i]["seats"][j]["userNickname"];
						array[i].seats[j].userSex = '0';
					}
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
				}
			}
			array = Test(array);
			LL_AppUIMngr.GetSingleton().mTableList.InitTableList(array, array[0].roomId);
			LL_AppUIMngr.GetSingleton().InItTable(array, array[0].roomId);
			LL_AppUIMngr.GetSingleton().SetNoHall(isHavHall: true);
			if (LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_RoomList_Panel && !LL_GameInfo.getInstance().UserInfo.IsOverFlow)
			{
				LL_AppUIMngr.GetSingleton().SetAppState(AppState.App_On_TableList_Panel);
				LL_AppUIMngr.GetSingleton().mTableList.ShowTableList();
			}
		}
		else
		{
			UnityEngine.Debug.LogError("该房间没有桌子");
			LL_AppUIMngr.GetSingleton().SetNoHall(isHavHall: false);
			LL_AppUIMngr.GetSingleton().mTableList.InitTableList(null, 0);
			LL_AppUIMngr.GetSingleton().InItTable(null, 0);
		}
	}

	private void DoDeskOnlineNumber(object[] args)
	{
	}

	private void DoAddExpeGoldAuto(object[] args)
	{
		Console.WriteLine("体验币不足，系统已自动赠送!!!");
		LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.ApplyForExpCoin_Success, string.Empty);
	}

	private void DoDeskInfo(object[] args)
	{
		MonoBehaviour.print("DeskInfo: " + JsonMapper.ToJson(args));
		int length = (args[0] as Array).Length;
		if (length > 0)
		{
			Seat[] array = new Seat[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = (Seat)(args[0] as Array).GetValue(i);
			}
			LL_AppUIMngr.GetSingleton().InSeat(array);
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
				if (!array[k].isFree)
				{
					array2[array[k].id - 1] = array[k].userNickname;
					array4[array[k].id - 1] = ((array[k].photoId <= 0 || array[k].photoId > 4) ? 1 : array[k].photoId);
					array3[array[k].id - 1] = array[k].userId;
				}
			}
			if (LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_TableList_Panel)
			{
				int tableId = LL_GameInfo.getInstance().UserInfo.TableId;
				LL_AppUIMngr.GetSingleton().mTableList.SetTableInfo(tableId, array3, array2, array4);
				LL_AppUIMngr.GetSingleton().SetAppState(AppState.App_On_Table);
				LL_AppUIMngr.GetSingleton().mSeatList.ShowSeatList();
			}
		}
		else
		{
			Console.WriteLine("该游戏桌子上没有座位!!!");
		}
	}

	private void DoUpdateDeskInfo(object[] args)
	{
		MonoBehaviour.print("UpdateDeskInfo: " + JsonMapper.ToJson(args));
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
				if (!array[k].isFree)
				{
					array2[array[k].id - 1] = array[k].userNickname;
					array4[array[k].id - 1] = ((array[k].photoId <= 0 || array[k].photoId > 4) ? 1 : array[k].photoId);
					array3[array[k].id - 1] = array[k].userId;
				}
			}
			int tableId = LL_GameInfo.getInstance().UserInfo.TableId;
			LL_AppUIMngr.GetSingleton().mTableList.SetTableInfo(tableId, array3, array2, array4);
		}
		else
		{
			Console.WriteLine("该游戏桌子上没有座位!!!");
		}
	}

	private void DoSelectSeat(object[] args)
	{
		MonoBehaviour.print("DoSelectSeat: " + JsonMapper.ToJson(args));
		if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_Table)
		{
			return;
		}
		int num = 0;
		int ponterToLight = 0;
		int[] arry = new int[24];
		int[] nPower = new int[15];
		int[] array = new int[4];
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		bool flag = false;
		try
		{
			flag = (bool)hashtable["success"];
		}
		catch (Exception)
		{
		}
		try
		{
			flag = (bool)hashtable["bCanSeat"];
		}
		catch (Exception)
		{
		}
		Console.WriteLine("bIsEnterFlag:" + flag);
		if (flag)
		{
			num = (int)hashtable["betTime"];
			int num2 = (int)hashtable["zxh"];
			ponterToLight = (int)hashtable["pointerLocation"];
			int num3 = (int)hashtable["awardType"];
			arry = (int[])hashtable["colors"];
			nPower = (int[])hashtable["beilv"];
			array = (int[])hashtable["betChip"];
			LL_AppUIMngr.GetSingleton().u_Camera.SetActive(value: false);
			LL_AppUIMngr.GetSingleton().u_Canvas.SetActive(value: false);
			LL_AppUIMngr.GetSingleton().EnterGame();
			LL_AppUIMngr.GetSingleton().mBetPanel.SetChip(array);
		}
		if (flag)
		{
			LL_AppUIMngr.GetSingleton().SetAppState(AppState.App_On_Game);
			if (num >= 10)
			{
				LL_AppUIMngr.GetSingleton().mHudManager.SetGameCD(num, bIsJoinGame: true);
				LL_AppUIMngr.GetSingleton().mBetPanel.ShowBetPanel();
				LL_AppUIMngr.GetSingleton().mBetPanel.SetAnimalPower(nPower);
				LL_AppUIMngr.GetSingleton().mBetPanel.ClearAllBet();
				LL_MusicMngr.GetSingleton().Reset();
				LL_GameMngr.GetSingleton().Reset();
				LL_GameMngr.GetSingleton().m_LightPointer.UpdateLightColor(arry);
			}
			LL_GameMngr.GetSingleton().m_LightPointer.SetPonterToLight(ponterToLight);
		}
		if (flag)
		{
			return;
		}
		try
		{
			num = (int)hashtable["messageStatus"];
		}
		catch (Exception message)
		{
			UnityEngine.Debug.Log(message);
		}
		Console.WriteLine("messageStatus:" + num);
		switch (num)
		{
		case 0:
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.SelectSeat_NotEmpty, string.Empty);
			break;
		case 1:
		{
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.SelectTable_CreditBelowRistrict, string.Empty);
			GameObject tip = Resources.Load<GameObject>("Tip_LL");
			if (tip != null)
			{
				tip = UnityEngine.Object.Instantiate(tip, GameObject.Find("Canvas").transform);
				tip.transform.DOLocalMoveY(100f, 1f).OnComplete(delegate
				{
					UnityEngine.Object.Destroy(tip);
				});
			}
			else
			{
				UnityEngine.Debug.LogError("tip为空");
			}
			break;
		}
		case 2:
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.SelectSeat_NotEmpty, string.Empty);
			break;
		}
	}

	private void DoUpdateGame(object[] args)
	{
		MonoBehaviour.print("DoUpdateGame: " + JsonMapper.ToJson(args));
		int[] array = new int[24];
		int[] array2 = new int[15];
		int[] array3 = new int[15];
		int[] array4 = new int[15];
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		int num = (int)hashtable["betTime"];
		int diceResult = (int)hashtable["zxh"];
		int ponterToLight = (int)hashtable["pointerLocation"];
		int num2 = (int)hashtable["awardType"];
		array = (int[])hashtable["colors"];
		array2 = (int[])hashtable["beilv"];
		array3 = (int[])hashtable["singleBets"];
		array4 = (int[])hashtable["totalBets"];
		LL_AppUIMngr.GetSingleton().mBetPanel.SetAnimalPower(array2);
		LL_AppUIMngr.GetSingleton().mHudManager.SetDiceResult((EDiceResult)diceResult);
		LL_AppUIMngr.GetSingleton().mBetPanel.SetAnimalBet(array4, bIsTotal: true);
		LL_AppUIMngr.GetSingleton().mBetPanel.SetAnimalBet(array3, bIsTotal: false);
		LL_GameMngr.GetSingleton().Reset();
		LL_GameMngr.GetSingleton().m_LightPointer.SetPonterToLight(ponterToLight);
		LL_GameMngr.GetSingleton().m_LightPointer.SetLightsColorImmediate(array);
	}

	private void DoGameRestart(object[] args)
	{
		if (LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_Game)
		{
			int[] array = new int[24];
			int[] array2 = new int[15];
			Hashtable hashtable = new Hashtable();
			hashtable = (args[0] as Hashtable);
			int num = (int)hashtable["zxh"];
			int ponterToLight = (int)hashtable["pointerLocation"];
			int num2 = (int)hashtable["awardType"];
			array = (int[])hashtable["colors"];
			array2 = (int[])hashtable["beilv"];
			LL_AppUIMngr.GetSingleton().mHudManager.SetGameCD(29);
			LL_AppUIMngr.GetSingleton().mBetPanel.SetAnimalPower(array2);
			LL_AppUIMngr.GetSingleton().mBetPanel.ClearAllBet();
			if (LL_AppUIMngr.GetSingleton().mBetPanel.IsAutoBet)
			{
				LL_AppUIMngr.GetSingleton().mBetPanel.AutoBet();
			}
			LL_AppUIMngr.GetSingleton().mBetPanel.ShowBetPanel();
			LL_MusicMngr.GetSingleton().Reset();
			LL_GameMngr.GetSingleton().Reset();
			LL_GameMngr.GetSingleton().m_LightPointer.SetPonterToLight(ponterToLight);
			LL_GameMngr.GetSingleton().m_LightPointer.UpdateLightColor(array);
		}
	}

	private void DoGameResult(object[] args)
	{
		if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_Game)
		{
			return;
		}
		DeskResult deskResult = new DeskResult();
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		int[] array = (int[])hashtable["currentBets"];
		int[] array2 = (int[])hashtable["colors"];
		Income[] array3 = (Income[])hashtable["incomeInfo"];
		int currentWin = (int)hashtable["addScore"];
		deskResult = (DeskResult)hashtable["deskResult"];
		int num = array.Length;
		int num2 = array2.Length;
		int num3 = array3.Length;
		Console.Write("nCurrentBets:");
		for (int i = 0; i < num; i++)
		{
			Console.Write(" " + array[i]);
		}
		Console.WriteLine();
		Console.Write("nColors:");
		for (int j = 0; j < num2; j++)
		{
			Console.Write(" " + array2[j]);
		}
		Console.WriteLine();
		Console.Write("InComeInFo:");
		for (int k = 0; k < num3; k++)
		{
			Console.Write(" " + array3[k].seatId + "," + array3[k].score);
		}
		LL_AppUIMngr.GetSingleton().mHudManager.SetDiceResult((EDiceResult)deskResult.zxh);
		LL_AppUIMngr.GetSingleton().mBetPanel.SetAnimalBet(array, bIsTotal: false);
		LL_AppUIMngr.GetSingleton().mBetPanel.HideBetPanel();
		int[] array4 = new int[8];
		for (int l = 0; l < array3.Length; l++)
		{
			array4[array3[l].seatId - 1] = array3[l].score;
		}
		LL_AppUIMngr.GetSingleton().mPrizeResult.SetAllUserWin(array4);
		LL_GameInfo.getInstance().UserInfo.CurrentWin = currentWin;
		LL_AppUIMngr.GetSingleton().mHudManager.SetBonusNumber(deskResult.awardGold);
		if (deskResult.type == 0)
		{
			int[] iAnimalID = new int[1]
			{
				deskResult.animal
			};
			LL_AppUIMngr.GetSingleton().mPrizeResult.SetAnimalResult(iAnimalID);
		}
		else if (deskResult.type == 1)
		{
			if (deskResult.globalType == 0)
			{
				int[] iAnimalID2 = new int[1]
				{
					deskResult.animal
				};
				LL_AppUIMngr.GetSingleton().mHudManager.SetBonusNumber(deskResult.awardGold, bIsBonusAward: true);
				LL_AppUIMngr.GetSingleton().mPrizeResult.SetAnimalResult(iAnimalID2);
			}
			if (deskResult.globalType == 1)
			{
				int[] iAnimalID3 = new int[1]
				{
					deskResult.animal
				};
				LL_AppUIMngr.GetSingleton().mPrizeResult.SetAnimalResult(iAnimalID3, 1, deskResult.lightningBeilv);
			}
			else if (deskResult.globalType == 2)
			{
				ArrayList arrayList = _ConvertStringSongDengforUI(deskResult.moreInfo);
				int[] array5 = new int[arrayList.Count];
				for (int m = 0; m < array5.Length; m++)
				{
					array5[m] = (int)arrayList[m];
				}
				LL_AppUIMngr.GetSingleton().mPrizeResult.SetAnimalResult(array5, array5.Length);
			}
			else if (deskResult.globalType == 3)
			{
				int[] array6 = new int[3];
				for (int n = 0; n < array6.Length; n++)
				{
					array6[n] = deskResult.animal * 3 + n;
				}
				LL_AppUIMngr.GetSingleton().mPrizeResult.SetAnimalResult(array6, array6.Length);
			}
			else if (deskResult.globalType == 4)
			{
				int color = deskResult.color;
				int[] array7 = new int[4];
				for (int num4 = 0; num4 < array7.Length; num4++)
				{
					array7[num4] = num4 * 3 + color;
				}
				LL_AppUIMngr.GetSingleton().mPrizeResult.SetAnimalResult(array7, array7.Length);
			}
		}
		else if (deskResult.type == 2)
		{
			if (deskResult.luckType == 0)
			{
				LL_AppUIMngr.GetSingleton().mHudManager.SetBonusNumber(deskResult.awardGold, bIsBonusAward: true);
				LL_AppUIMngr.GetSingleton().mPrizeResult.SetLuckyBonus(deskResult.animal, deskResult.luckNum, deskResult.luckAnimal);
			}
			else if (deskResult.luckType == 1)
			{
				LL_AppUIMngr.GetSingleton().mPrizeResult.SetLuckyJP(deskResult.animal, deskResult.luckNum, deskResult.luckAnimal, deskResult.lightningBeilv);
			}
			if (deskResult.luckType == 2)
			{
				ArrayList arrayList2 = _ConvertStringSongDengforUI(deskResult.moreInfo);
				int[] array8 = new int[arrayList2.Count];
				for (int num5 = 0; num5 < array8.Length; num5++)
				{
					array8[num5] = (int)arrayList2[num5];
				}
				LL_AppUIMngr.GetSingleton().mPrizeResult.SetLuckyTimes(deskResult.animal, deskResult.luckNum, array8, array8.Length);
			}
		}
		if (deskResult.type == 0)
		{
			UnityEngine.Debug.Log("---------------NormalPrize--Go!!---------------------");
			int animal = deskResult.animal;
			AnimalType animal2 = _ConvertIntAnimal(animal);
			LL_GameMngr.GetSingleton().GoOneAnimal(animal2, array2[0], 24f, 15f);
			return;
		}
		if (deskResult.type == 1)
		{
			if (deskResult.globalType == 0)
			{
				UnityEngine.Debug.Log("---------------CaiJin--Go!!---------------------");
				AnimalType animal3 = _ConvertIntAnimal(deskResult.animal);
				LL_GameMngr.GetSingleton().GoAllCaiJin(animal3, array2[0], 19f, 12f);
				return;
			}
			if (deskResult.globalType == 1)
			{
				UnityEngine.Debug.Log("---------------AllLightning--Go!!---------------------");
				AnimalType animal4 = _ConvertIntAnimal(deskResult.animal);
				LL_GameMngr.GetSingleton().GoAllShanDian(deskResult.lightningBeilv, animal4, array2[0], 19f, 15f);
				return;
			}
			if (deskResult.globalType == 2)
			{
				UnityEngine.Debug.Log("---------------SongDeng--Go!!---------------------");
				ArrayList arrayList3 = _ConvertStringSongDeng(deskResult.moreInfo);
				int[] array9 = new int[arrayList3.Count];
				int[] array10 = new int[arrayList3.Count];
				for (int num6 = 0; num6 < arrayList3.Count; num6++)
				{
					array9[num6] = (int)arrayList3[num6];
					array10[num6] = array2[num6];
				}
				LL_GameMngr.GetSingleton().GoAllSongDeng(array9, array2);
				return;
			}
			if (deskResult.globalType == 3)
			{
				UnityEngine.Debug.Log("---------------DaSanYuan--Go!!---------------------");
				AnimalType animal5 = (deskResult.animal == 0) ? AnimalType.Lion : ((deskResult.animal == 1) ? AnimalType.Panda : ((deskResult.animal == 2) ? AnimalType.Monkey : AnimalType.Rabbit));
				LL_GameMngr.GetSingleton().GoAllDaSanYuan(animal5, array2[0], 19f, 12f);
				return;
			}
			if (deskResult.globalType == 4)
			{
				UnityEngine.Debug.Log("---------------DaSiXi--Go!!---------------------");
				LL_GameMngr.GetSingleton().GoAllDaSiXi(array2[0]);
				return;
			}
		}
		else if (deskResult.type == 2)
		{
			if (deskResult.luckType == 0)
			{
				int[] array11 = new int[2];
				int[] array12 = new int[2];
				array11[0] = (int)_ConvertIntAnimal(deskResult.animal);
				array11[1] = (int)_ConvertIntAnimal(deskResult.luckAnimal);
				LL_GameMngr.GetSingleton().GoLuckyBonus(array11, deskResult.awardGold, deskResult.luckNum, array2);
				return;
			}
			if (deskResult.luckType == 1)
			{
				int[] array13 = new int[2];
				int[] array14 = new int[2];
				array13[0] = (int)_ConvertIntAnimal(deskResult.animal);
				array13[1] = (int)_ConvertIntAnimal(deskResult.luckAnimal);
				LL_GameMngr.GetSingleton().GoLuckyLightning(array13, deskResult.lightningBeilv, deskResult.luckNum, array2);
				return;
			}
			ArrayList arrayList4 = _ConvertStringSongDeng(deskResult.moreInfo);
			int[] array15 = new int[arrayList4.Count + 1];
			int[] array16 = new int[arrayList4.Count + 1];
			array15[0] = (int)_ConvertIntAnimal(deskResult.animal);
			for (int num7 = 0; num7 < arrayList4.Count + 1; num7++)
			{
				if (num7 != 0)
				{
					array15[num7] = (int)arrayList4[num7 - 1];
				}
				array16[num7] = array2[num7];
			}
			LL_GameMngr.GetSingleton().GoLuckySongDeng(array15, deskResult.luckNum, array2, 14f);
			return;
		}
		if (deskResult.songDengCount > 0)
		{
			LL_GameMngr.GetSingleton().GoAnimalPrize(AnimalType.Lion, array2[0]);
			return;
		}
		int animal6 = deskResult.animal;
		AnimalType animal7 = (animal6 / 3 == 0) ? AnimalType.Lion : ((animal6 / 3 == 1) ? AnimalType.Panda : ((animal6 / 3 == 2) ? AnimalType.Monkey : AnimalType.Rabbit));
		LL_GameMngr.GetSingleton().GoOneAnimal(animal7, array2[0]);
	}

	private void DoDeskTotalBet(object[] args)
	{
		int[] array = (int[])args[0];
		int num = array.Length;
		LL_AppUIMngr.GetSingleton().mBetPanel.SetAnimalBet(array, bIsTotal: true);
	}

	private void DoCurrentBet(object[] args)
	{
		int[] array = (int[])args[0];
		int num = array.Length;
		LL_AppUIMngr.GetSingleton().mBetPanel.SetAnimalBet(array, bIsTotal: false, isLastBet: false);
	}

	private void DoNewGameScore(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.Log("gameScore:" + num);
		LL_GameInfo.getInstance().UserInfo.GameScore = num;
	}

	private void DoNewExpeScore(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.Log("nExpeScore:" + num);
		LL_GameInfo.getInstance().UserInfo.GameScore = num;
	}

	private void DoPlayerInfo(object[] args)
	{
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		int num = (int)hashtable["seatId"];
		string text = (string)hashtable["nickname"];
		int num2 = (int)hashtable["userGameScore"];
		int num3 = (int)hashtable["userLevel"];
		Console.WriteLine("nSeatId:" + num);
		Console.WriteLine("strNickName:" + text);
		Console.WriteLine("nUserGameScore:" + num2);
		Console.WriteLine("nUserLeave:" + num3);
		if (LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_Game)
		{
			LL_AppUIMngr.GetSingleton().mOtherUserInfoPanel.ShowOtherInfo(text, num2, num3, bIsNeedPrivateChat: true);
		}
		else if (LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_Table)
		{
			LL_AppUIMngr.GetSingleton().mOtherUserInfoPanel.ShowOtherInfo(text, num2, num3, bIsNeedPrivateChat: false);
		}
	}

	private void DoSendChat(object[] args)
	{
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		int num = (int)hashtable["chatType"];
		int num2 = (int)hashtable["senderSeatId"];
		string text = (string)hashtable["chatMessage"];
		Console.WriteLine("nChatType:" + num);
		Console.WriteLine("nSenderSeatId:" + num2);
		Console.WriteLine("strChatMessage:" + text);
		LL_AppUIMngr.GetSingleton().mChatPanel.AddChatMessage(num, num2, text);
	}

	private void DoResultList(object[] args)
	{
		DeskRecord[] result = (DeskRecord[])(args[0] as Array);
		LL_AppUIMngr.GetSingleton().mHistoryRecord.ResetRecord(result);
		LL_AppUIMngr.GetSingleton().mHistoryRecord.ShowRecord();
	}

	private void HandleNetMsg_ApplyExchange(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		LL_GameInfo.getInstance().UserInfo.CoinCount = ZH2_GVars.user.gameGold;
		if (LL_AppUIMngr.GetSingleton() != null && LL_AppUIMngr.GetSingleton().mRoomList != null)
		{
			LL_AppUIMngr.GetSingleton().mRoomList.updateUserInfo();
		}
		string ch = (!flag) ? "兑换被拒绝" : "兑换成功";
		string en = (!flag) ? "Failure of exchange" : "Successful exchange";
		if (LL_GameTipManager.GetSingleton() != null)
		{
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.Custom, ZH2_GVars.ShowTip(ch, en, string.Empty));
		}
		else
		{
			LL_LoadTip.getInstance().showTip(LL_LoadTip.tipType.Custom, ZH2_GVars.ShowTip(ch, en, string.Empty));
		}
	}

	private void HandleNetMsg_ApplyPay(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		LL_GameInfo.getInstance().UserInfo.CoinCount = ZH2_GVars.user.gameGold;
		if (LL_AppUIMngr.GetSingleton() != null && LL_AppUIMngr.GetSingleton().mRoomList != null)
		{
			LL_AppUIMngr.GetSingleton().mRoomList.updateUserInfo();
		}
		string ch = (!flag) ? "充值被拒绝" : "充值成功";
		string en = (!flag) ? "Failure of TopUp" : "Successful TopUp";
		if (LL_GameTipManager.GetSingleton() != null)
		{
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.Custom, ZH2_GVars.ShowTip(ch, en, string.Empty));
		}
		else
		{
			LL_LoadTip.getInstance().showTip(LL_LoadTip.tipType.Custom, ZH2_GVars.ShowTip(ch, en, string.Empty));
		}
	}

	private void HandleNetMsg_changeScoreNotice(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		LL_GameInfo.getInstance().UserInfo.CoinCount = ZH2_GVars.user.gameGold;
		if (LL_AppUIMngr.GetSingleton() != null && LL_AppUIMngr.GetSingleton().mRoomList != null)
		{
			LL_AppUIMngr.GetSingleton().mRoomList.updateUserInfo();
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
		if (LL_GameTipManager.GetSingleton() != null)
		{
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.Custom, ZH2_GVars.ShowTip($"{arg}成功! 金币{num}", $"Successful {arg2}! GoldCoin {num}", string.Empty));
		}
		else
		{
			LL_LoadTip.getInstance().showTip(LL_LoadTip.tipType.Custom, ZH2_GVars.ShowTip($"{arg}成功! 金币{num}", $"Successful {arg2}! GoldCoin {num}", string.Empty));
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

	private void DoOverflow(object[] args)
	{
		Console.WriteLine("爆机啦...");
		LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.CoinOverFlow, string.Empty);
	}

	private void DoQuitToLogin(object[] args)
	{
		int num = (int)args[0];
		Console.WriteLine("nQuitType:" + num);
		switch (num)
		{
		case 1:
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.ServerUpdate, string.Empty);
			break;
		case 2:
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.Game_UserIdFrozen, string.Empty);
			break;
		case 3:
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIdDeleted, string.Empty);
			break;
		case 4:
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIdRepeative, string.Empty);
			LL_Sockets.GetSingleton().isReconnect = false;
			break;
		case 5:
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserPwdChanged, string.Empty);
			break;
		case 6:
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.LoseTheServer, string.Empty);
			break;
		}
	}

	private void DoQuitToRoom(object[] args)
	{
		int num = (int)args[0];
		Console.WriteLine("nQuitType:" + num);
		switch (num)
		{
		case 1:
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.TableDeleted_Game, string.Empty);
			break;
		case 2:
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.TableParameterModified, string.Empty);
			break;
		case 3:
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.LongTimeNoOperate, string.Empty);
			break;
		}
	}

	private void DoQuitToDesk(object[] args)
	{
		LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.QuitToDesk, string.Empty);
	}

	private void DoGameShutup(object[] args)
	{
		bool flag = (bool)args[0];
		Console.WriteLine("bGameShutup:" + flag);
		LL_GameInfo.getInstance().UserInfo.IsGlobalFibbidChat = flag;
	}

	private void DoUserShutup(object[] args)
	{
		bool flag = (bool)args[0];
		Console.WriteLine("bUserShutup:" + flag);
		LL_GameInfo.getInstance().UserInfo.IsSelfFibbidChat = flag;
	}

	private void DoUserAward(object[] args)
	{
		int num = (int)args[0];
		Console.WriteLine("nGold:" + num);
		LL_GameTipManager.GetSingleton().AwardNum = num;
		LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.GivingCoin, string.Empty);
	}

	private void DoNetDown(object[] args)
	{
		if (!LL_Sockets.GetSingleton().isReconnect)
		{
			return;
		}
		if (m_CreateSocket.GetRelineCount() < 3)
		{
			m_CreateSocket.CreateReceiveThread();
			return;
		}
		UnityEngine.Debug.LogError("30秒重连失败，网络断开，请重新登录网络大厅...");
		if (LL_NetMngr.isInLoading)
		{
			if (LL_LoadTip.getInstance().currentType == LL_LoadTip.tipType.None)
			{
				LL_LoadTip.getInstance().showTip(LL_LoadTip.tipType.NetDown, string.Empty);
			}
		}
		else if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.APP_NET_ERROR)
		{
			LL_AppUIMngr.GetSingleton().SetAppState(AppState.APP_NET_ERROR);
			LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.Net_ConnectionError, string.Empty);
		}
	}

	private void DoHeart(object[] args)
	{
		m_CreateSocket.SendHeart();
	}

	private void DoLogoffNotice(object[] args)
	{
		Console.WriteLine("此号码重复登录");
		LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIdRepeative, string.Empty);
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
		Console.WriteLine("**********1*********DoCheckVersion**************************************");
		string text = (string)args[0];
		Console.WriteLine("haveNewVersionIDFlag: " + text);
		LL_IOSGameStart.GetSingleton().UpdateGameVesion(text);
		Console.WriteLine("**********1*********DoCheckVersion**************************************");
	}

	private void DoNotUpdate(object[] args)
	{
        Debug.LogError("DoNotUpdate");
		LL_NetMngr.GetSingleton().MyCreateSocket.sendPublicKey();
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
