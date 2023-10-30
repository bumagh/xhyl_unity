using com.miracle9.game.entity;
using JsonFx.Json;
using LHD_GameCommon;
using LitJson;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LHD_Transmit : MonoBehaviour
{
	private static LHD_Transmit _MyTransmit;

	private LHD_Sockets m_CreateSocket;

	public static LHD_Transmit GetSingleton()
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
		if (LHD_GameInfo.getInstance() != null)
		{
			LHD_GameInfo.getInstance().ClearGameInfo();
			UnityEngine.Debug.LogError("清除了键值");
		}
		else
		{
			UnityEngine.Debug.LogError("===mGameInfo为空===");
		}
	}

	public void TransmitGetPoint(LHD_Sockets MyCreateSocket)
	{
		m_CreateSocket = MyCreateSocket;
	}

	public void PostMsgControl(Hashtable table)
	{
		string text = table["method"].ToString();
		object[] array = table["args"] as object[];
		if (Application.platform == RuntimePlatform.WindowsEditor && text != "NetThread/NetDown" && text != "newGameScore" && text != "deskStatus")
		{
			UnityEngine.Debug.LogError("收到2: " + text + "  " + JsonFx.Json.JsonWriter.Serialize(array));
		}
		if (text == "sendServerTime")
		{
			if (LHD_GameInfo.getInstance().Key != string.Empty)
			{
				if (!LHD_NetMngr.isInLoading)
				{
					switch (LHD_GameInfo.getInstance().GetAppState)
					{
					case AppState.App_On_RoomList_Panel:
					case AppState.App_On_TableList_Panel:
					case AppState.App_On_Table:
						m_CreateSocket.SendUserLogin(LHD_GameInfo.getInstance().UserId, LHD_GameInfo.getInstance().Pwd, 1, string.Empty);
						break;
					case AppState.App_On_Game:
						m_CreateSocket.SendUserLogin(LHD_GameInfo.getInstance().UserId, LHD_GameInfo.getInstance().Pwd, 2, string.Empty);
						break;
					}
				}
				else
				{
					m_CreateSocket.SendUserLogin(LHD_GameInfo.getInstance().UserId, LHD_GameInfo.getInstance().Pwd, 1, string.Empty);
				}
			}
			LHD_GameInfo.getInstance().Key = "login";
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
		else if (text == "newGameScore")
		{
			DoNewGameScore(array);
		}
		else if (text == "selectSeat")
		{
			DoSelectSeat(array);
		}
		else if (text == "gameRestart")
		{
			DogameRestart(array);
		}
		else if (text == "gameResult")
		{
			DoGameResult(array);
		}
		else if (text == "deskTotalBet")
		{
			DoDeskTotalBet(array);
		}
		else if (text == "betSpace")
		{
			DoBetSpace(array);
		}
		else if (!(text == string.Empty) && !(text == "quitToLogin"))
		{
			if (text == "NetThread/NetDown")
			{
				DoNetDown(array);
			}
			else if (text == "resultList")
			{
				DoResultList(array);
			}
			else if (text == "updateDeskInfo")
			{
				DoUpdateDeskInfo(array);
			}
			else if (text == "currentBet")
			{
				DoCurrentBet(array);
			}
			else if (text == "continueBet")
			{
				DoContinueBet(array);
			}
			else if (text == "continueBetSpace")
			{
				DoContinueBetSpace(array);
			}
			else if (text == "playerList")
			{
				DoPlayerList(array);
			}
			else if (text == "deskStatus")
			{
				DoDeskStatus(array);
			}
			else
			{
				UnityEngine.Debug.LogError("其他接口: " + text);
			}
		}
	}

	public void SelectPostMsgControl(Hashtable table)
	{
		string str = table["method"].ToString();
		object[] value = table["args"] as object[];
		UnityEngine.Debug.LogError("收到2: " + str + "  " + JsonFx.Json.JsonWriter.Serialize(value));
	}

	private void DoCheckVersion(object[] args)
	{
		string str = (string)args[0];
		UnityEngine.Debug.LogError("检测版本: " + str);
	}

	private void DoNotUpdate(object[] args)
	{
		LHD_NetMngr.GetSingleton().MyCreateSocket.sendPublicKey();
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
			LHD_GameInfo.getInstance().userinfo.nickname = user.nickname;
			LHD_GameInfo.getInstance().userinfo.IconIndex = user.photoId;
			LHD_GameInfo.getInstance().userinfo.sex = user.sex;
			LHD_GameInfo.getInstance().GameScore = user.gameGold;
			LHD_GameInfo.getInstance().LoadStep = LoadType.On_Hall;
			ZH2_GVars.userId = user.id;
			return;
		}
		switch (num)
		{
		case 0:
			UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
			SceneManager.LoadScene(0);
			break;
		case 1:
			LHD_GameTipManager.GetSingleton().ShowTip(LHD_EGameTipType.UserIDFrozen, string.Empty);
			break;
		default:
			LHD_GameTipManager.GetSingleton().ShowTip(LHD_EGameTipType.ServerUpdate, string.Empty);
			break;
		}
	}

	private void DoResultList(object[] args)
	{
		JsonData obj = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		LHD_GameInfo.getInstance().resultListCall?.Invoke(obj);
	}

	private void DoUpdateDeskInfo(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		ZH2_GVars.hallInfo2 = new JsonData();
		ZH2_GVars.hallInfo2 = jsonData;
		LHD_GameInfo.getInstance().updateRoomList?.Invoke(jsonData);
	}

	private void DoNewGameGold(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.LogError("nGameGold:" + num);
		LHD_GameInfo.getInstance().CoinCount = num;
	}

	private void DoNewExpeGold(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.LogError("nExpeGold:" + num);
		LHD_GameInfo.getInstance().ExpCoinCount = num;
	}

	private void DoNewGameScore(object[] args)
	{
		int gameScore = (int)args[0];
		LHD_GameInfo.getInstance().GameScore = gameScore;
	}

	private void DoSelectSeat(object[] args)
	{
		int[] array = new int[6];
		Hashtable hashtable = new Hashtable();
		hashtable = (args[0] as Hashtable);
		bool flag = (bool)hashtable["success"];
		UnityEngine.Debug.LogError("bIsEnterFlag:" + flag);
		if (flag)
		{
			UnityEngine.Debug.LogError("进入游戏");
			LHD_GameInfo.getInstance().GetAppState = AppState.App_On_Game;
			array = (int[])hashtable["betChip"];
			LHD_UIManager.instance.ShowGame();
			LHD_GameScene.instance.SetBetChip(array);
		}
	}

	private void DogameRestart(object[] args)
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
		int betTime = (int)hashtable["betTime"];
		int inningNum = (int)hashtable["innings"];
		for (int i = 0; i < jsonData.Count; i++)
		{
		}
		LHD_GameScene.instance.Restart(inningNum, betTime);
		LHD_GameInfo.getInstance().getPlayerList?.Invoke();
	}

	private void DoGameResult(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		JsonData jsonData2 = jsonData["deskResult"];
		int num = (int)jsonData["addScore"];
		int tigerPoker = (int)jsonData["poker"]["tigerPoker"];
		int dragonPoker = (int)jsonData["poker"]["dragonPoker"];
		int winType = (int)jsonData["result"];
		LHD_GameInfo.getInstance().winType = (LHD_GameInfo.WinType)winType;
		LHD_GameInfo.getInstance().tigerPoker = tigerPoker;
		LHD_GameInfo.getInstance().dragonPoker = dragonPoker;
		LHD_GameScene.instance.Result();
	}

	private void DoCurrentBet(object[] args)
	{
		int[] array = (int[])args[0];
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			LHD_GameScene.instance.BetGerenValue[i] = array[i];
			num += array[i];
		}
		LHD_GameScene.instance.SetAllBet(num);
		LHD_GameScene.instance.ShowTotalBet();
	}

	private void DoContinueBet(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		UnityEngine.Debug.LogError(jsonData.ToJson());
		if (!(bool)jsonData["continueBet"])
		{
			string tips = jsonData["message"].ToString();
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
		}
	}

	private void DoContinueBetSpace(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args));
		for (int i = 0; i < jsonData.Count; i++)
		{
			JsonData jsonData2 = JsonMapper.ToObject(jsonData[i].ToString());
			for (int j = 0; j < jsonData2.Count; j++)
			{
				LHD_GameScene.instance.GetChip(jsonData2[j]);
			}
		}
		LHD_GameInfo.getInstance().getPlayerList?.Invoke();
	}

	private void DoPlayerList(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args));
		for (int i = 0; i < jsonData.Count; i++)
		{
			JsonData obj = JsonMapper.ToObject(jsonData[i].ToString());
			LHD_GameInfo.getInstance().upDatePlayerList?.Invoke(obj);
		}
	}

	private void DoDeskStatus(object[] args)
	{
		JsonData obj = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args[0]));
		LHD_GameInfo.getInstance().upDateTime?.Invoke(obj);
	}

	private void DoDeskTotalBet(object[] args)
	{
		int[] array = (int[])args[0];
		for (int i = 0; i < array.Length; i++)
		{
			LHD_GameScene.instance.BetZongValue[i] = array[i];
		}
		LHD_GameScene.instance.ShowTotalBet();
	}

	private void DoBetSpace(object[] args)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(args));
		UnityEngine.Debug.LogError("押注结果: " + jsonData.ToJson());
		for (int i = 0; i < jsonData.Count; i++)
		{
			JsonData jd = JsonMapper.ToObject(jsonData[i].ToString());
			LHD_GameScene.instance.GetChip(jd);
		}
		LHD_AudioManger.instance.PlayAudio(LHD_AudioManger.AudioType.EnterBet);
		LHD_GameInfo.getInstance().getPlayerList?.Invoke();
	}

	private void DoQuitToLogin(object[] args)
	{
		int num = (int)args[0];
		UnityEngine.Debug.LogError("退出登录:" + num);
	}

	private void DoNetDown(object[] args)
	{
		if (!LHD_Sockets.GetSingleton().isReconnect)
		{
			return;
		}
		if (m_CreateSocket.GetRelineCount() < 5)
		{
			m_CreateSocket.CreateReceiveThread();
			return;
		}
		UnityEngine.Debug.LogError("重连失败，网络断开，请重新登录网络大厅...");
		if (LHD_GameTipManager.GetSingleton() != null)
		{
			LHD_GameTipManager.GetSingleton().ShowTip(LHD_EGameTipType.Net_ConnectionError, string.Empty);
		}
	}
}
