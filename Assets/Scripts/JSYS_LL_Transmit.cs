using com.miracle9.game.bean;
using com.miracle9.game.entity;
using JsonFx.Json;
using JSYS_LL_GameCommon;
using JSYS_LL_UICommon;
using LitJson;
using System;
using System.Collections;
using UnityEngine;

public class JSYS_LL_Transmit : MonoBehaviour
{
	private string[] nameani = new string[12]
	{
		"兔子",
		"猴子",
		"熊猫",
		"狮子",
		"燕子",
		"鸽子",
		"孔雀",
		"老鹰",
		"金鲨",
		"银鲨",
		"飞禽",
		"走兽"
	};

	private static JSYS_LL_Transmit _MyTransmit;

	private JSYS_LL_Sockets m_CreateSocket;

	private JSYS_LL_DataEncrypt m_DataEncrypt;

	public static JSYS_LL_Transmit GetSingleton()
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

	public void TransmitGetPoint(JSYS_LL_Sockets MyCreateSocket)
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
				if (text != "newGameGold" && text != "gameGold" && text != "updateDeskInfo" && text != "updateHallInfo")
				{
					UnityEngine.Debug.LogError("收到2: " + text + "  " + JsonFx.Json.JsonWriter.Serialize(array));
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError(text + " 错误: " + arg);
			}
		}
		if (text == "sendServerTime")
		{
			LL_GameInfo.getInstance().Key = string.Empty;
			if (JSYS_LL_GameInfo.getInstance().Key != string.Empty)
			{
				UnityEngine.Debug.Log(LL_GameInfo.getInstance().Key);
				if (!JSYS_LL_NetMngr.isInLoading)
				{
					switch (JSYS_LL_AppUIMngr.GetSingleton().GetAppState)
					{
					case AppState.App_On_RoomList_Panel:
						UnityEngine.Debug.Log(1);
						m_CreateSocket.SendUserLogin(JSYS_LL_GameInfo.getInstance().UserId, JSYS_LL_GameInfo.getInstance().Pwd, 1, string.Empty);
						break;
					case AppState.App_On_TableList_Panel:
						UnityEngine.Debug.Log(2);
						m_CreateSocket.SendUserLogin(JSYS_LL_GameInfo.getInstance().UserId, JSYS_LL_GameInfo.getInstance().Pwd, 2, string.Empty);
						break;
					case AppState.App_On_Table:
						UnityEngine.Debug.Log(3);
						m_CreateSocket.SendUserLogin(JSYS_LL_GameInfo.getInstance().UserId, JSYS_LL_GameInfo.getInstance().Pwd, 3, string.Empty);
						break;
					case AppState.App_On_Game:
						UnityEngine.Debug.Log(4);
						m_CreateSocket.SendUserLogin(JSYS_LL_GameInfo.getInstance().UserId, JSYS_LL_GameInfo.getInstance().Pwd, 4, string.Empty);
						break;
					}
				}
				else
				{
					m_CreateSocket.SendUserLogin(JSYS_LL_GameInfo.getInstance().UserId, JSYS_LL_GameInfo.getInstance().Pwd, 1, string.Empty);
				}
			}
			UnityEngine.Debug.LogError("Key为空");
			JSYS_LL_GameInfo.getInstance().Key = "login";
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
		else if (text == "newGameGold" || text == "gameGold")
		{
			DoNewGameGold(array);
		}
		else if (text == "newExpeGold" || text == "expeGold")
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
		else if (text == "enterRoom")
		{
			DoEnterRoom(array);
		}
		else if (text == "selectHall" || text == "updateHallInfo")
		{
			DoSelectHall(array);
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
		else if (text == "cancelBet" || text == "continueBet")
		{
			DoCancelBet(array);
		}
		else if (!(text == "heart"))
		{
			if (text == "NetThread/NetDown")
			{
				DoNetDown(array);
			}
			else
			{
				UnityEngine.Debug.LogError("No Message Type! " + text);
			}
		}
	}

	public void SelectPostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] args = table["args"] as object[];
		if (JSYS_LL_Parameter.G_Test)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError(text);
		}
		if (text == "sendServerTime")
		{
			if (JSYS_LL_GameInfo.getInstance().Key != string.Empty)
			{
				UnityEngine.Debug.LogError(JSYS_LL_GameInfo.getInstance().Key);
				if (!JSYS_LL_NetMngr.isInLoading)
				{
					switch (JSYS_LL_AppUIMngr.GetSingleton().GetAppState)
					{
					case AppState.App_On_RoomList_Panel:
						UnityEngine.Debug.LogError(1);
						m_CreateSocket.SendUserLogin(JSYS_LL_GameInfo.getInstance().UserId, JSYS_LL_GameInfo.getInstance().Pwd, 1, string.Empty);
						break;
					case AppState.App_On_TableList_Panel:
						UnityEngine.Debug.LogError(2);
						m_CreateSocket.SendUserLogin(JSYS_LL_GameInfo.getInstance().UserId, JSYS_LL_GameInfo.getInstance().Pwd, 2, string.Empty);
						break;
					case AppState.App_On_Table:
						UnityEngine.Debug.LogError(3);
						m_CreateSocket.SendUserLogin(JSYS_LL_GameInfo.getInstance().UserId, JSYS_LL_GameInfo.getInstance().Pwd, 3, string.Empty);
						break;
					case AppState.App_On_Game:
						UnityEngine.Debug.LogError(4);
						m_CreateSocket.SendUserLogin(JSYS_LL_GameInfo.getInstance().UserId, JSYS_LL_GameInfo.getInstance().Pwd, 4, string.Empty);
						break;
					}
				}
				else
				{
					UnityEngine.Debug.LogError(5);
					m_CreateSocket.SendUserLogin(JSYS_LL_GameInfo.getInstance().UserId, JSYS_LL_GameInfo.getInstance().Pwd, 1, string.Empty);
				}
			}
			UnityEngine.Debug.LogError("Key为空");
			JSYS_LL_GameInfo.getInstance().Key = "login";
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
									UnityEngine.Debug.LogError("No Message Type!");
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
		UnityEngine.Debug.LogError("bIsLoginFlag: " + flag2);
		if (flag2)
		{
			flag = (bool)hashtable["isShutup"];
			user = (hashtable["user"] as com.miracle9.game.entity.User);
			ZH2_GVars.hallInfo2 = new JsonData();
			ZH2_GVars.hallInfo2 = jsonData["hallInfo"];
			bool isSpecial = (bool)hashtable["special"];
			JSYS_LL_PersonInfo jSYS_LL_PersonInfo = new JSYS_LL_PersonInfo();
			UserInfo.nikeName = user.nickname;
			user.quickCredit = user.gameGold;
			user.surname = UserInfo.nikeName;
			jSYS_LL_PersonInfo.strId = user.username;
			jSYS_LL_PersonInfo.strName = user.surname;
			jSYS_LL_PersonInfo.UserId = user.id;
			ZH2_GVars.userId = user.id;
			if (user.sex == 30007)
			{
				jSYS_LL_PersonInfo.IsMale = true;
			}
			else
			{
				jSYS_LL_PersonInfo.IsMale = false;
			}
			jSYS_LL_PersonInfo.IsGlobalFibbidChat = flag;
			jSYS_LL_PersonInfo.CoinCount = (int)user.quickCredit;
			jSYS_LL_PersonInfo.ExpCoinCount = user.expeGold;
			jSYS_LL_PersonInfo.IconIndex = user.PhotoId;
			jSYS_LL_PersonInfo.IsOverFlow = false;
			JSYS_LL_GameInfo.getInstance().IsSpecial = isSpecial;
			JSYS_LL_GameInfo.getInstance().UserInfo = jSYS_LL_PersonInfo;
			JSYS_UI.mUserInfo = jSYS_LL_PersonInfo;
			JSYS_LL_GameInfo.kaishi = true;
			return;
		}
		switch (num)
		{
		case 0:
		{
			JSYS_GVars.isStartedFromGame = false;
			JSYS_LL_GameInfo.ClearGameInfo();
			UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
			UnityEngine.Debug.LogError("这里被我注销了");
			AsyncOperation asyncOperation = Application.LoadLevelAsync(0);
			asyncOperation.allowSceneActivation = true;
			break;
		}
		case 1:
			if (JSYS_LL_NetMngr.isInLoading)
			{
				JSYS_LL_LoadTip.getInstance().showTip(JSYS_LL_LoadTip.tipType.IdIsFrozen);
			}
			else
			{
				JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIDFrozen, string.Empty);
			}
			break;
		default:
			if (JSYS_LL_NetMngr.isInLoading)
			{
				JSYS_LL_LoadTip.getInstance().showTip(JSYS_LL_LoadTip.tipType.ServerStop);
			}
			else
			{
				JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.ServerUpdate, string.Empty);
			}
			break;
		}
	}

	private void DoNewGameGold(object[] args)
	{
		int coinCount = (int)args[0];
		JSYS_LL_GameInfo.getInstance().UserInfo.CoinCount = coinCount;
	}

	private void DoNewExpeGold(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.LogError("nExpeGold:" + num);
		JSYS_LL_GameInfo.getInstance().UserInfo.ExpCoinCount = num;
	}

	private void DoEnterRoom(object[] args)
	{
	}

	private void DoSelectHall(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		JsonData jsonData2 = jsonData["deskInfo"];
		UpdateOnline(jsonData);
		int count = jsonData2.Count;
		if (count > 0)
		{
			GoldSharkDesk[] array = new GoldSharkDesk[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = new GoldSharkDesk();
				array[i].id = (int)jsonData2[i]["id"];
				array[i].roomId = (int)jsonData2[i]["roomId"];
				array[i].name = (string)jsonData2[i]["name"];
				array[i].autoKick = (int)jsonData2[i]["autoKick"];
				array[i].minGold = (int)jsonData2[i]["minGold"];
				array[i].exchange = (int)jsonData2[i]["exchange"];
				array[i].onceExchangeValue = (int)jsonData2[i]["onceExchangeValue"];
				array[i].orderBy = (int)jsonData2[i]["orderBy"];
				array[i].siteType = (int)jsonData2[i]["siteType"];
				array[i].min_zxh = (int)jsonData2[i]["min_zxh"];
				array[i].minBet = (int)jsonData2[i]["minBet"];
				array[i].beilvType = (int)jsonData2[i]["beilvType"];
				array[i].pourWater = (int)jsonData2[i]["pourWater"];
				array[i].beilvModel = (int)jsonData2[i]["beilvModel"];
				array[i].onlineNumber = (int)jsonData2[i]["onlineNumber"];
				array[i].zxhDiff = (int)jsonData2[i]["zxhDiff"];
				array[i].betTime = (int)jsonData2[i]["betTime"];
				array[i].animalDiff = (int)jsonData2[i]["animalDiff"];
				array[i].maxBet = (int)jsonData2[i]["maxBet"];
				array[i].max_h = (int)jsonData2[i]["max_h"];
				array[i].drawWater = (int)jsonData2[i]["drawWater"];
				array[i].max_zx = (int)jsonData2[i]["max_zx"];
			}
			JSYS_LL_AppUIMngr.GetSingleton().InItTable(array, array[0].roomId);
			if (JSYS_LL_AppUIMngr.GetSingleton().GetAppState == AppState.App_On_RoomList_Panel && !JSYS_LL_GameInfo.getInstance().UserInfo.IsOverFlow)
			{
				JSYS_LL_AppUIMngr.GetSingleton().GetAppState = AppState.App_On_TableList_Panel;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("该房间没有桌子");
			JSYS_LL_AppUIMngr.GetSingleton().InItTable(null, 0);
			JSYS_LL_AppUIMngr.GetSingleton().SetNoHall(isHavHall: false);
		}
	}

	private void UpdateOnline(JsonData dictionary)
	{
		if (dictionary == null)
		{
			UnityEngine.Debug.LogError("dictionary为空");
			return;
		}
		int count = dictionary["onlineNumber"].Count;
		if (JSYS_LL_AppUIMngr.GetSingleton() != null)
		{
			for (int i = 0; i < JSYS_LL_AppUIMngr.GetSingleton().selectBtnList.Count; i++)
			{
				JSYS_LL_AppUIMngr.GetSingleton().selectBtnList[i].onlinePeople = dictionary["onlineNumber"][i.ToString()].ToString();
				JSYS_LL_AppUIMngr.GetSingleton().selectBtnList[i].UpdateText();
			}
		}
		else
		{
			UnityEngine.Debug.Log("更新在线人数失败");
		}
	}

	private void DoUpdateRoomInfo(object[] args)
	{
	}

	private void DoDeskOnlineNumber(object[] args)
	{
	}

	private void DoAddExpeGoldAuto(object[] args)
	{
		UnityEngine.Debug.LogError("体验币不足，系统已自动赠送!!!");
		JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.ApplyForExpCoin_Success, string.Empty);
	}

	private void DoDeskInfo(object[] args)
	{
		int length = (args[0] as Array).Length;
		if (length > 0)
		{
			Seat[] array = new Seat[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = (Seat)(args[0] as Array).GetValue(i);
			}
			JSYS_LL_AppUIMngr.GetSingleton().InSeat(array);
		}
		else
		{
			UnityEngine.Debug.LogError("该游戏桌子上没有座位!!!");
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.SelectSeat_NotEmpty, string.Empty);
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
				if (!array[k].isFree)
				{
					array2[array[k].id - 1] = array[k].userNickname;
					array4[array[k].id - 1] = array[k].photoId;
					array3[array[k].id - 1] = array[k].userId;
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("该游戏桌子上没有座位!!!");
		}
	}

	private void DoSelectSeat(object[] args)
	{
		int[] array = new int[5];
		int[] array2 = new int[15];
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		bool flag = (bool)hashtable["success"];
		UnityEngine.Debug.LogError("bIsEnterFlag:" + flag);
		array2 = (int[])hashtable["beilv"];
		array = (int[])hashtable["betChip"];
		JSYS_LL_GameInfo.getInstance().BeiLv = array2;
		JSYS_LL_GameInfo.getInstance().BetChip = array;
		if (flag)
		{
			UnityEngine.Debug.LogError("进入游戏");
			UnityEngine.SceneManagement.SceneManager.LoadScene("JSYS_Game");
		}
	}

	private void DoUpdateGame(object[] args)
	{
		int[] array = new int[24];
		int[] array2 = new int[15];
		int[] array3 = new int[15];
		int[] array4 = new int[15];
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		int num = (int)hashtable["betTime"];
		int num2 = (int)hashtable["zxh"];
		int num3 = (int)hashtable["pointerLocation"];
		int num4 = (int)hashtable["awardType"];
		array = (int[])hashtable["colors"];
		array2 = (int[])hashtable["beilv"];
		JSYS_LL_GameInfo.getInstance().BeiLv = array2;
		array3 = (int[])hashtable["singleBets"];
		array4 = (int[])hashtable["totalBets"];
		JSYS_LL_GameMngr.GetSingleton().Reset();
	}

	private void DoGameRestart(object[] args)
	{
		int[] array = new int[15];
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		array = (int[])hashtable["beilv"];
		JSYS_LL_GameInfo.getInstance().BeiLv = array;
		JSYS_link.Time_Dji = 47;
		JSYS_link.publiclink.parameter(JSYS_link.Time_Dji, "燕子");
		for (int i = 0; i < JSYS_BetScene.publicBetScene.BetZongValue.Length; i++)
		{
			JSYS_BetScene.publicBetScene.BetZongValue[i] = 0;
			JSYS_BetScene.publicBetScene.BetGerenValue[i] = 0;
		}
		JSYS_BetScene.publicBetScene.displaylaue[2] = 0.0;
	}

	private void DoGameResult(object[] args)
	{
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		GoldSharkDeskResult goldSharkDeskResult = new GoldSharkDeskResult();
		goldSharkDeskResult = (GoldSharkDeskResult)hashtable["deskResult"];
		int num = (int)hashtable["addScore"];
		JSYS_BetScene.publicBetScene.displaylaue[0] = num;
		if (goldSharkDeskResult.moreInfo == string.Empty)
		{
			JSYS_link.moreInfo = goldSharkDeskResult.moreInfo;
		}
		else
		{
			JSYS_link.moreInfo = nameani[int.Parse(goldSharkDeskResult.moreInfo)];
		}
		JSYS_link.Time_Dji = 17;
		JSYS_link.publiclink.parameter(JSYS_link.Time_Dji, nameani[goldSharkDeskResult.animal]);
		JSYS_link.name_ani = nameani[goldSharkDeskResult.animal];
	}

	private void DoDeskTotalBet(object[] args)
	{
		int[] array = (int[])args[0];
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			Console.Write(" " + array[i]);
			JSYS_BetScene.publicBetScene.BetZongValue[i] = array[i];
		}
	}

	private void DoCurrentBet(object[] args)
	{
		int[] array = (int[])args[0];
		int num = array.Length;
		UnityEngine.Debug.LogError("currentBet：");
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			Console.Write(" " + array[i]);
			JSYS_BetScene.publicBetScene.BetGerenValue[i] = array[i];
			num2 += array[i];
		}
		JSYS_BetScene.publicBetScene.displaylaue[2] = num2;
	}

	private void DoNewGameScore(object[] args)
	{
		int num = (int)args[0];
		JSYS_LL_GameInfo.getInstance().UserInfo.GameScore = num;
		if (JSYS_BetScene.publicBetScene != null)
		{
			JSYS_BetScene.publicBetScene.displaylaue[1] = num;
		}
		else
		{
			StartCoroutine(enumerator(num));
		}
	}

	private void DoNewExpeScore(object[] args)
	{
		int num = (int)args[0];
		JSYS_LL_GameInfo.getInstance().UserInfo.GameScore = num;
		if (JSYS_BetScene.publicBetScene != null)
		{
			JSYS_BetScene.publicBetScene.displaylaue[1] = num;
		}
		else
		{
			StartCoroutine(enumerator(num));
		}
	}

	private IEnumerator enumerator(int num)
	{
		yield return new WaitForSeconds(0.5f);
		if (JSYS_BetScene.publicBetScene != null)
		{
			JSYS_BetScene.publicBetScene.displaylaue[1] = num;
		}
	}

	private void DoPlayerInfo(object[] args)
	{
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		int num = (int)hashtable["seatId"];
		string text = (string)hashtable["surname"];
		int num2 = (int)hashtable["userGameScore"];
		int num3 = (int)hashtable["userLevel"];
	}

	private void DoSendChat(object[] args)
	{
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		int num = (int)hashtable["chatType"];
		int num2 = (int)hashtable["senderSeatId"];
		string str = (string)hashtable["chatMessage"];
		UnityEngine.Debug.LogError("nChatType:" + num);
		UnityEngine.Debug.LogError("nSenderSeatId:" + num2);
		UnityEngine.Debug.LogError("strChatMessage:" + str);
	}

	private void DoResultList(object[] args)
	{
		DeskRecord[] array = (DeskRecord[])(args[0] as Array);
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			JSYS_Control.publicControl.Recordingmethon(nameani[array[num - 1 - i].animalType]);
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
		UnityEngine.Debug.LogError("爆机啦...");
		JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.CoinOverFlow, string.Empty);
	}

	private void DoQuitToLogin(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.LogError("nQuitType:" + num);
		switch (num)
		{
		case 1:
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.ServerUpdate, string.Empty);
			break;
		case 2:
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.Game_UserIdFrozen, string.Empty);
			break;
		case 3:
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIdDeleted, string.Empty);
			break;
		case 4:
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIdRepeative, string.Empty);
			JSYS_LL_Sockets.GetSingleton().isReconnect = false;
			break;
		case 5:
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserPwdChanged, string.Empty);
			break;
		case 6:
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.LoseTheServer, string.Empty);
			break;
		}
	}

	private void DoQuitToRoom(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.LogError("nQuitType:" + num);
		switch (num)
		{
		case 1:
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.TableDeleted_Game, string.Empty);
			break;
		case 2:
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.TableParameterModified, string.Empty);
			break;
		case 3:
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.LongTimeNoOperate, string.Empty);
			break;
		}
	}

	private void DoGameShutup(object[] args)
	{
		bool flag = (bool)args[0];
		UnityEngine.Debug.LogError("bGameShutup:" + flag);
		JSYS_LL_GameInfo.getInstance().UserInfo.IsGlobalFibbidChat = flag;
	}

	private void DoUserShutup(object[] args)
	{
		bool flag = (bool)args[0];
		UnityEngine.Debug.LogError("bUserShutup:" + flag);
		JSYS_LL_GameInfo.getInstance().UserInfo.IsSelfFibbidChat = flag;
	}

	private void DoUserAward(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.LogError("nGold:" + num);
		JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.GivingCoin, string.Empty);
	}

	private void DoCancelBet(object[] args)
	{
		JsonData jsonData = new JsonData();
		try
		{
			jsonData = JsonMapper.ToObject(JsonMapper.ToJson(args));
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.Custom, (string)jsonData[0]["message"]);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.Custom, "操作失败,请检查状态");
		}
	}

	private void DoNetDown(object[] args)
	{
		if (!JSYS_LL_Sockets.GetSingleton().isReconnect)
		{
			return;
		}
		if (m_CreateSocket.GetRelineCount() < 15)
		{
			m_CreateSocket.CreateReceiveThread();
			return;
		}
		UnityEngine.Debug.LogError("30秒重连失败，网络断开，请重新登录网络大厅...");
		if (JSYS_LL_NetMngr.isInLoading)
		{
			if (JSYS_LL_LoadTip.getInstance().currentType == JSYS_LL_LoadTip.tipType.None)
			{
				JSYS_LL_LoadTip.getInstance().showTip(JSYS_LL_LoadTip.tipType.NetDown);
			}
		}
		else if (JSYS_LL_AppUIMngr.GetSingleton().GetAppState != AppState.APP_NET_ERROR)
		{
			JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.Net_ConnectionError, string.Empty);
		}
	}

	private void DoHeart(object[] args)
	{
		m_CreateSocket.SendHeart();
	}

	private void DoLogoffNotice(object[] args)
	{
		UnityEngine.Debug.LogError("此号码重复登录");
		JSYS_LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIdRepeative, string.Empty);
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
		string text = (string)args[0];
		UnityEngine.Debug.LogError("haveNewVersionIDFlag: " + text);
		JSYS_LL_IOSGameStart.GetSingleton().UpdateGameVesion(text);
	}

	private void DoNotUpdate(object[] args)
	{
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.sendPublicKey();
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
