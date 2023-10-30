using M__M.HaiWang.GameDefine;
using M__M.HaiWang.UIDefine;
using System.Collections.Generic;
using UnityEngine;

public static class HW2_GVars
{
	public static string IPAddress = string.Empty;

	public static int IPPort = 10100;

	public static string versionURL = string.Empty;

	public static string versionCode = string.Empty;

	public static string language = string.Empty;

	public static string username = string.Empty;

	public static string pwd = string.Empty;

	public static bool lockReconnect;

	public static bool lockRelogin;

	public static Demo_UI_State m_curState = Demo_UI_State.StartupLoading;

	public static bool isShutChatGroup;

	public static bool isShutChatPrivate;

	public static bool isShutup;

	public static bool isUserShutup;

	public static float RandomTime;

	public static HW2_UserInfo user = new HW2_UserInfo();

	public static GameContext game = new GameContext();

	public static LobbyContext lobby = new LobbyContext();

	public static int selectRoomId = 1;

	public static List<GameObject> dontDestroyOnLoadList = new List<GameObject>();

	public static AppConfig appConfig
	{
		get;
		private set;
	}

	public static bool IsHasTag(string tag)
	{
		return false;
	}

	public static void SetUserInfo(HW2_UserInfo _user)
	{
		user = _user;
	}

	public static void SetGameContext(GameContext _game)
	{
		game = _game;
	}

	public static void SetLobbyContext(LobbyContext _lobby)
	{
		lobby = _lobby;
	}

	public static void SetAppConfig(AppConfig _appConfig)
	{
		appConfig = _appConfig;
	}
}
