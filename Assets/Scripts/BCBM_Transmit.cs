using BCBM_GameCommon;
using BCBM_UICommon;
using com.miracle9.game.entity;
using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BCBM_Transmit : MonoBehaviour
{
	private int[] nameani = new int[29]
	{
		0,
		1,
		2,
		3,
		4,
		5,
		6,
		7,
		8,
		9,
		10,
		11,
		12,
		13,
		14,
		15,
		16,
		17,
		18,
		19,
		20,
		21,
		22,
		23,
		24,
		25,
		26,
		27,
		28
	};

	private static BCBM_Transmit _MyTransmit;

	private BCBM_Sockets m_CreateSocket;

	private BCBM_DataEncrypt m_DataEncrypt;

	public GameObject script;

	public GameObject ui_Scene;

	public GameObject game_Scene;

	public GameObject gameManger;

	public BCBM_BetScene bCBM_BetScene;

	public static BCBM_Transmit GetSingleton()
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

	private void OnEnable()
	{
		if (BCBM_GameInfo.getInstance() != null)
		{
			BCBM_GameInfo.ClearGameInfo();
			UnityEngine.Debug.LogError("清除了键值");
		}
		else
		{
			UnityEngine.Debug.LogError("===mGameInfo为空===");
		}
	}

	public void TransmitGetPoint(BCBM_Sockets MyCreateSocket)
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
				UnityEngine.Debug.LogError("收到2: " + text + "  " + JsonFx.Json.JsonWriter.Serialize(array));
			}
			catch
			{
			}
		}
		if (BCBM_Parameter.G_Test)
		{
			BCBM_ErrorManager.GetSingleton().AddError(text);
		}
		if (text == "sendServerTime")
		{
			if (BCBM_GameInfo.getInstance().Key != string.Empty)
			{
				UnityEngine.Debug.Log("===========" + BCBM_GameInfo.getInstance().Key + "===========");
				if (!BCBM_NetMngr.isInLoading)
				{
					UnityEngine.Debug.LogError("!BCBM_NetMngr.isInLoading");
					switch (BCBM_AppUIMngr.GetSingleton().GetAppState)
					{
					case AppState.App_On_RoomList_Panel:
					case AppState.App_On_TableList_Panel:
					case AppState.App_On_Table:
						m_CreateSocket.SendUserLogin(BCBM_GameInfo.getInstance().UserId, BCBM_GameInfo.getInstance().Pwd, 1, string.Empty);
						break;
					case AppState.App_On_Game:
						m_CreateSocket.SendUserLogin(BCBM_GameInfo.getInstance().UserId, BCBM_GameInfo.getInstance().Pwd, 2, string.Empty);
						m_CreateSocket.SendEnterHall(BCBM_GameInfo.getInstance().UserInfo.HallId);
						break;
					}
				}
				else
				{
					m_CreateSocket.SendUserLogin(BCBM_GameInfo.getInstance().UserId, BCBM_GameInfo.getInstance().Pwd, 1, string.Empty);
				}
			}
			BCBM_GameInfo.getInstance().Key = "login";
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
			DoEnterRoom(array);
		}
		else if (text == "selectHall")
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
		else if (text == "betSpace")
		{
			DoBetSpace(array);
		}
		else if (text == "continueBetSpace" || text == "autoBet")
		{
			DoContinueBetSpace(array);
		}
		else if (text == "playerList")
		{
			DoPlayerList(array);
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
		else if (text == "scrollMessage")
		{
			DoScrollMessage(array);
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
				UnityEngine.Debug.LogError(text + "  No Message Type!");
			}
		}
	}

	public void SelectPostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] args = table["args"] as object[];
		if (BCBM_Parameter.G_Test)
		{
			BCBM_ErrorManager.GetSingleton().AddError(text);
		}
		if (text == "sendServerTime")
		{
			if (BCBM_GameInfo.getInstance().Key != string.Empty)
			{
				if (!BCBM_NetMngr.isInLoading)
				{
					switch (BCBM_AppUIMngr.GetSingleton().GetAppState)
					{
					case AppState.App_On_RoomList_Panel:
					case AppState.App_On_TableList_Panel:
					case AppState.App_On_Table:
						m_CreateSocket.SendUserLogin(BCBM_GameInfo.getInstance().UserId, BCBM_GameInfo.getInstance().Pwd, 1, string.Empty);
						break;
					case AppState.App_On_Game:
						m_CreateSocket.SendUserLogin(BCBM_GameInfo.getInstance().UserId, BCBM_GameInfo.getInstance().Pwd, 2, string.Empty);
						break;
					}
				}
				else
				{
					m_CreateSocket.SendUserLogin(BCBM_GameInfo.getInstance().UserId, BCBM_GameInfo.getInstance().Pwd, 1, string.Empty);
				}
			}
			UnityEngine.Debug.LogError("Key为空");
			BCBM_GameInfo.getInstance().Key = "login";
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
		int num = -1;
		if (!flag2)
		{
			num = (int)hashtable["messageStatus"];
		}
		if (flag2)
		{
			user = (com.miracle9.game.entity.User)hashtable["user"];
			ZH2_GVars.hallInfo2 = new JsonData();
			ZH2_GVars.hallInfo2 = jsonData["getAllDesk"];
			bool isSpecial = false;
			BCBM_PersonInfo bCBM_PersonInfo = new BCBM_PersonInfo();
			UserInfo.nikeName = user.nickname;
			user.quickCredit = user.gameGold;
			user.surname = UserInfo.nikeName;
			bCBM_PersonInfo.strId = user.username;
			bCBM_PersonInfo.strName = user.surname;
			bCBM_PersonInfo.UserId = user.id;
			ZH2_GVars.userId = user.id;
			if (user.sex == 30007)
			{
				bCBM_PersonInfo.IsMale = true;
			}
			else
			{
				bCBM_PersonInfo.IsMale = false;
			}
			bCBM_PersonInfo.IsGlobalFibbidChat = false;
			bCBM_PersonInfo.CoinCount = (int)user.quickCredit;
			bCBM_PersonInfo.ExpCoinCount = user.expeGold;
			bCBM_PersonInfo.IconIndex = user.PhotoId;
			bCBM_PersonInfo.IsOverFlow = false;
			BCBM_GameInfo.getInstance().IsSpecial = isSpecial;
			BCBM_GameInfo.getInstance().UserInfo = bCBM_PersonInfo;
			BCBM_UI.mUserInfo = bCBM_PersonInfo;
			BCBM_GameInfo.kaishi = true;
			return;
		}
		switch (num)
		{
		case 0:
			BCBM_MySqlConnection.isStartedFromGame = false;
			BCBM_GameInfo.ClearGameInfo();
			UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
			SceneManager.LoadScene(0);
			break;
		case 1:
			if (BCBM_NetMngr.isInLoading)
			{
				BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.IdIsFrozen, string.Empty);
			}
			else
			{
				BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIDFrozen, string.Empty);
			}
			break;
		default:
			if (BCBM_NetMngr.isInLoading)
			{
				BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.ServerStop, string.Empty);
			}
			else
			{
				BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.ServerUpdate, string.Empty);
			}
			break;
		}
	}

	private void DoNewGameGold(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.LogError("nGameGold:" + num);
		BCBM_GameInfo.getInstance().UserInfo.CoinCount = num;
	}

	private void DoNewExpeGold(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.LogError("nExpeGold:" + num);
		BCBM_GameInfo.getInstance().UserInfo.ExpCoinCount = num;
	}

	private void DoEnterRoom(object[] args)
	{
	}

	private void DoSelectHall(object[] args)
	{
	}

	private BCBM_Desk[] Test(BCBM_Desk[] array)
	{
		for (int i = 0; i < array.Length - 1; i++)
		{
			for (int j = 0; j < array.Length - 1; j++)
			{
				if (array[j].id > array[j + 1].id)
				{
					BCBM_Desk bCBM_Desk = array[j];
					array[j] = array[j + 1];
					array[j + 1] = bCBM_Desk;
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
		if (!(BCBM_AppUIMngr.GetSingleton() != null))
		{
			UnityEngine.Debug.Log("更新在线人数失败");
		}
	}

	private void DoUpdateRoomInfo(object[] args)
	{
	}

	private void DoUpdateHallInfo(object[] args)
	{
	}

	private void DoDeskOnlineNumber(object[] args)
	{
	}

	private void DoAddExpeGoldAuto(object[] args)
	{
		UnityEngine.Debug.LogError("体验币不足，系统已自动赠送!!!");
		BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.ApplyForExpCoin_Success, string.Empty);
	}

	private void DoDeskInfo(object[] args)
	{
		int length = (args[0] as Array).Length;
		if (length <= 0)
		{
			UnityEngine.Debug.LogError("该游戏桌子上没有座位!!!");
			BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.TableDeleted_Game, string.Empty);
		}
	}

	private void DoUpdateDeskInfo(object[] args)
	{
		ZH2_GVars.hallInfo2 = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
	}

	private void DoSelectSeat(object[] args)
	{
		int[] array = new int[5];
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		bool flag = (bool)hashtable["success"];
		array = (int[])hashtable["betChip"];
		UnityEngine.Debug.LogError("bIsEnterFlag:" + flag);
		if (flag)
		{
			UnityEngine.Debug.LogError("===进入游戏===");
			try
			{
				BCBM_AppUIMngr.GetSingleton().GetAppState = AppState.App_On_Game;
				script.SetActive(value: true);
				script.GetComponent<BCBM_UI>().OnEnable1();
				ui_Scene.SetActive(value: false);
				gameManger.SetActive(value: false);
				game_Scene.SetActive(value: true);
				BCBM_BetScene.publicBetScene.SetBetChip(array);
				BCBM_Audio.publicAudio.PlayBg(1, 0.6f);
				BCBM_BetScene.publicBetScene.ShowTips(0);
				JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(hashtable["betSpace"]));
				UnityEngine.Debug.LogError("押注结果: " + jsonData.ToJson());
				for (int i = 0; i < jsonData.Count; i++)
				{
					JsonData jd = jsonData[i];
					if (BCBM_BetScene.publicBetScene != null)
					{
						BCBM_BetScene.publicBetScene.GetChip(jd);
					}
					else
					{
						UnityEngine.Debug.LogError("=====BCBM_BetScene为空=====");
					}
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("===========" + arg);
			}
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
		array3 = (int[])hashtable["singleBets"];
		array4 = (int[])hashtable["totalBets"];
		BCBM_GameMngr.GetSingleton().Reset();
	}

	private void DoGameRestart(object[] args)
	{
		JsonData jsonData = new JsonData();
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		try
		{
			jsonData = JsonMapper.ToObject(JsonMapper.ToJson(hashtable["ratio"]));
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("解析倍率失败 " + arg);
		}
		int num = (int)hashtable["betTime"];
		BCBM_GameInfo.getInstance().BetTime = num;
		BCBM_link.Time_Dji = num + 17;
		UnityEngine.Debug.LogError("====压分时间: " + num + " ===总流程: " + BCBM_link.Time_Dji);
		BCBM_link.publiclink.parameter(BCBM_link.Time_Dji, 0, 0, 0);
		BCBM_BetScene.publicBetScene.Restart();
		for (int i = 0; i < BCBM_BetScene.publicBetScene.BetZongValue.Length; i++)
		{
			BCBM_BetScene.publicBetScene.BetZongValue[i] = 0;
			BCBM_BetScene.publicBetScene.BetGerenValue[i] = 0;
		}
		for (int j = 0; j < jsonData.Count; j++)
		{
			try
			{
				BCBM_BetScene.publicBetScene.BetValue[j] = int.Parse(jsonData[j].ToString());
			}
			catch (Exception arg2)
			{
				UnityEngine.Debug.LogError(j + " 倍率赋值失败 " + arg2);
			}
		}
		BCBM_BetScene.publicBetScene.displaylaue[2] = 0.0;
	}

	private void DoGameResult(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		UnityEngine.Debug.LogError("游戏结果: " + jsonData.ToJson());
		JsonData jsonData2 = jsonData["deskResult"];
		int num = (int)jsonData["addScore"];
		BCBM_BetScene.publicBetScene.displaylaue[0] = num;
		BCBM_link.moreInfo = -1;
		BCBM_link.animalInfo = (int)jsonData2["result"];
		BCBM_link.Time_Dji = 17;
		int num2 = (int)jsonData["space"];
		int num3 = (num2 < 0) ? (-1) : 0;
		num2 = Math.Abs(num2);
		UnityEngine.Debug.LogError("===游戏结果开始旋转===");
		BCBM_link.publiclink.parameter(BCBM_link.Time_Dji, BCBM_link.animalInfo, nameani[num2], num3);
		BCBM_link.name_ani = nameani[num2];
		BCBM_link.animal_ani = BCBM_link.animalInfo;
		BCBM_link.coloer_ani = num3;
	}

	private void DoDeskTotalBet(object[] args)
	{
		int[] array = (int[])args[0];
		for (int i = 0; i < array.Length; i++)
		{
			BCBM_BetScene.publicBetScene.BetZongValue[i] = array[i];
		}
	}

	private void DoCurrentBet(object[] args)
	{
		int[] array = (int[])args[0];
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			BCBM_BetScene.publicBetScene.BetGerenValue[i] = array[i];
			num += array[i];
		}
		BCBM_BetScene.publicBetScene.displaylaue[2] = num;
		BCBM_BetScene.publicBetScene.CheckBet(BCBM_BetScene.publicBetScene.BetGerenValue);
	}

	private void DoBetSpace(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args));
		UnityEngine.Debug.LogError("押注结果: " + jsonData.ToJson());
		for (int i = 0; i < jsonData.Count; i++)
		{
			JsonData jd = JsonMapper.ToObject(jsonData[i].ToString());
			BCBM_BetScene.publicBetScene.GetChip(jd);
		}
	}

	private void DoContinueBetSpace(object[] args)
	{
		UnityEngine.Debug.LogError("=======收到续压筹码======");
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args));
		for (int i = 0; i < jsonData.Count; i++)
		{
			JsonData jsonData2 = JsonMapper.ToObject(jsonData[i].ToString());
			for (int j = 0; j < jsonData2.Count; j++)
			{
				BCBM_BetScene.publicBetScene.GetChip(jsonData2[j]);
			}
		}
	}

	public void DoPlayerList(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args));
		for (int i = 0; i < jsonData.Count; i++)
		{
			JsonData jsonData2 = JsonMapper.ToObject(jsonData[i].ToString());
			BCBM_Control.publicControl.PlayerList(jsonData2);
		}
	}

	private void DoNewGameScore(object[] args)
	{
		int num = (int)args[0];
		if (BCBM_GameInfo.getInstance() != null)
		{
			BCBM_GameInfo.getInstance().UserInfo.GameScore = num;
		}
		if (BCBM_BetScene.publicBetScene != null)
		{
			BCBM_BetScene.publicBetScene.displaylaue[1] = num;
			return;
		}
		BCBM_BetScene.publicBetScene = bCBM_BetScene;
		BCBM_BetScene.publicBetScene.displaylaue[1] = num;
	}

	private void DoNewExpeScore(object[] args)
	{
		int gameScore = (int)args[0];
		BCBM_GameInfo.getInstance().UserInfo.GameScore = gameScore;
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
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		UnityEngine.Debug.LogError("结果列表: " + jsonData.ToJson());
		BCBM_Control.publicControl.ResetRecordingObj();
		int[] array = new int[jsonData.Count];
		for (int i = 0; i < jsonData.Count; i++)
		{
			array[i] = nameani[(int)jsonData[i]];
			BCBM_Control.publicControl.Recordingmethon(nameani[(int)jsonData[i]]);
		}
		BCBM_Control.publicControl.Recordingmethon(array);
	}

	private void DoSendNotice(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		UnityEngine.Debug.LogError("通知: " + jsonData.ToJson());
		if (All_NoticePanel.GetInstance() != null)
		{
			All_NoticePanel.GetInstance().AddTip(jsonData);
		}
		else
		{
			UnityEngine.Debug.LogError("====== BCBM_NoticePanel为空");
		}
	}

	private void DoOverflow(object[] args)
	{
		UnityEngine.Debug.LogError("爆机啦...");
		BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.CoinOverFlow, string.Empty);
	}

	private void DoQuitToLogin(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.LogError("nQuitType:" + num);
		if (BCBM_GameTipManager.GetSingleton() != null)
		{
			switch (num)
			{
			case 1:
				BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.ServerUpdate, string.Empty);
				break;
			case 2:
				BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.Game_UserIdFrozen, string.Empty);
				break;
			case 3:
				BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIdDeleted, string.Empty);
				break;
			case 4:
				BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIdRepeative, string.Empty);
				BCBM_Sockets.GetSingleton().isReconnect = false;
				break;
			case 5:
				BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserPwdChanged, string.Empty);
				break;
			case 6:
				BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.LoseTheServer, string.Empty);
				break;
			}
		}
		else
		{
			switch (num)
			{
			case 1:
				BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.ServerStop, string.Empty);
				break;
			case 2:
				BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.IdIsFrozen, string.Empty);
				break;
			case 3:
				BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.UserIdDeleted, string.Empty);
				break;
			case 4:
				BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.UserIdRepeative, string.Empty);
				break;
			case 5:
				BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.UserPwdChanged, string.Empty);
				break;
			case 6:
				BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.LoseTheServer, string.Empty);
				break;
			}
		}
	}

	private void DoQuitToRoom(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.LogError("nQuitType:" + num);
		switch (num)
		{
		case 1:
			BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.TableDeleted_Game, string.Empty);
			break;
		case 2:
			BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.TableParameterModified, string.Empty);
			break;
		case 3:
			BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.LongTimeNoOperate, string.Empty);
			break;
		}
	}

	private void DoQuitToDesk(object[] args)
	{
		BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.QuitToDesk, string.Empty);
	}

	private void DoGameShutup(object[] args)
	{
		bool flag = (bool)args[0];
		UnityEngine.Debug.LogError("bGameShutup:" + flag);
		BCBM_GameInfo.getInstance().UserInfo.IsGlobalFibbidChat = flag;
	}

	private void DoUserShutup(object[] args)
	{
		bool isSelfFibbidChat = (bool)args[0];
		try
		{
			BCBM_GameInfo.getInstance().UserInfo.IsSelfFibbidChat = isSelfFibbidChat;
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
	}

	private void DoUserAward(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.LogError("nGold:" + num);
		BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.GivingCoin, string.Empty);
	}

	private void HandleNetMsg_ApplyExchange(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		BCBM_GameInfo.getInstance().UserInfo.CoinCount = ZH2_GVars.user.gameGold;
		if (BCBM_AppUIMngr.GetSingleton() != null)
		{
			BCBM_AppUIMngr.GetSingleton().UpdateUserInfo();
		}
		string ch = (!flag) ? "兑换被拒绝" : "兑换成功";
		string en = (!flag) ? "Failure of exchange" : "Successful exchange";
		if (BCBM_GameTipManager.GetSingleton() != null)
		{
			BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.Custom, ZH2_GVars.ShowTip(ch, en, string.Empty));
		}
		else
		{
			BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.Custom, ZH2_GVars.ShowTip(ch, en, string.Empty));
		}
	}

	private void HandleNetMsg_ApplyPay(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		bool flag = (bool)jsonData["success"];
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		BCBM_GameInfo.getInstance().UserInfo.CoinCount = ZH2_GVars.user.gameGold;
		if (BCBM_AppUIMngr.GetSingleton() != null)
		{
			BCBM_AppUIMngr.GetSingleton().UpdateUserInfo();
		}
		string ch = (!flag) ? "充值被拒绝" : "充值成功";
		string en = (!flag) ? "Failure of TopUp" : "Successful TopUp";
		if (BCBM_GameTipManager.GetSingleton() != null)
		{
			BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.Custom, ZH2_GVars.ShowTip(ch, en, string.Empty));
		}
		else
		{
			BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.Custom, ZH2_GVars.ShowTip(ch, en, string.Empty));
		}
	}

	private void HandleNetMsg_changeScoreNotice(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		ZH2_GVars.user.gameGold = (int)jsonData["newGameGold"];
		BCBM_GameInfo.getInstance().UserInfo.CoinCount = ZH2_GVars.user.gameGold;
		if (BCBM_AppUIMngr.GetSingleton() != null)
		{
			BCBM_AppUIMngr.GetSingleton().UpdateUserInfo();
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
		if (BCBM_GameTipManager.GetSingleton() != null)
		{
			BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.Custom, ZH2_GVars.ShowTip($"{arg}成功! 金币{num}", $"Successful {arg2}! GoldCoin {num}", string.Empty));
		}
		else
		{
			BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.Custom, ZH2_GVars.ShowTip($"{arg}成功! 金币{num}", $"Successful {arg2}! GoldCoin {num}", string.Empty));
		}
	}

	private void DoScrollMessage(object[] args)
	{
		UnityEngine.Debug.LogError("======滚动公告: " + JsonMapper.ToJson(args));
	}

	private void DoCancelBet(object[] args)
	{
		BCBM_BetScene.publicBetScene.lastBetNum = 0;
	}

	private void DoNetDown(object[] args)
	{
		if (!BCBM_Sockets.GetSingleton().isReconnect)
		{
			return;
		}
		if (m_CreateSocket.GetRelineCount() < 5)
		{
			m_CreateSocket.CreateReceiveThread();
			return;
		}
		UnityEngine.Debug.LogError("重连失败，网络断开，请重新登录网络大厅...");
		if (BCBM_NetMngr.isInLoading)
		{
			if (BCBM_LoadTip.getInstance() != null)
			{
				BCBM_LoadTip.getInstance().showTip(BCBM_LoadTip.tipType.NetDown, string.Empty);
			}
		}
		else if (BCBM_GameTipManager.GetSingleton() != null)
		{
			BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.Net_ConnectionError, string.Empty);
		}
	}

	private void DoHeart(object[] args)
	{
		m_CreateSocket.SendHeart();
	}

	private void DoLogoffNotice(object[] args)
	{
		UnityEngine.Debug.LogError("此号码重复登录");
		BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.UserIdRepeative, string.Empty);
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
		BCBM_IOSGameStart.GetSingleton().UpdateGameVesion(text);
	}

	private void DoNotUpdate(object[] args)
	{
		BCBM_NetMngr.GetSingleton().MyCreateSocket.sendPublicKey();
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
