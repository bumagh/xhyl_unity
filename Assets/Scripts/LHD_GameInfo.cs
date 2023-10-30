using LHD_GameCommon;
using LitJson;
using System;
using System.Collections.Generic;

public class LHD_GameInfo
{
	public enum WinType
	{
		dragonPoker,
		peace,
		tigerPoker
	}

	public int Language;

	public string IP = "43.154.92.102";

	public string UserId = "ding888";

	public string Pwd = "ding888";

	public string Key;

	public Action<JsonData> resultListCall;

	public Action updateLuDan;

	public Action<JsonData> upDatePlayerList;

	public Action<JsonData> upDateTime;

	public Action getPlayerList;

	private static LHD_GameInfo mGameInfo;

	public LHD_LoadScene LoadScene;

	public LHD_HallScene HallScene;

	public LHD_GameScene GameScene;

	public LoadType LoadStep;

	public AppState GetAppState = AppState.App_On_RoomList_Panel;

	public int CoinCount;

	public int ExpCoinCount;

	private int gameScore;

	public LHD_UserInfo userinfo = new LHD_UserInfo();

	public List<LHD_RoomInfo> roomlist = new List<LHD_RoomInfo>();

	public LHD_RoomInfo roominfo = new LHD_RoomInfo();

	public static string LuDan;

	public Action<int> upDateScore;

	public Action<JsonData> updateRoomList;

	public int tigerPoker;

	public int dragonPoker;

	public WinType winType;

	public int GameScore
	{
		get
		{
			return gameScore;
		}
		set
		{
			gameScore = value;
			if (upDateScore != null)
			{
				upDateScore(gameScore);
			}
		}
	}

	public static LHD_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new LHD_GameInfo();
		}
		return mGameInfo;
	}

	public void ClearGameInfo()
	{
		getInstance().Key = string.Empty;
		mGameInfo = null;
	}
}
