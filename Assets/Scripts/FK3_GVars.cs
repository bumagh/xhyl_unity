using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Player.Gun;
using M__M.HaiWang.UIDefine;
using System.Collections.Generic;
using UnityEngine;

public static class FK3_GVars
{
	public static string IPAddress = string.Empty;

	public static int IPPort = 30100;

	public static string versionURL = string.Empty;

	public static string versionCode = string.Empty;

	public static string language = string.Empty;

	public static string username = string.Empty;

	public static string pwd = string.Empty;

	public static bool lockReconnect;

	public static bool lockRelogin;

	public static FK3_Demo_UI_State m_curState = FK3_Demo_UI_State.StartupLoading;

	public static bool isShutChatGroup;

	public static bool isShutChatPrivate;

	public static bool isShutup;

	public static bool isUserShutup;

	public static float RandomTime;

	public static int[] NowGunSkin = new int[4];

	public static FK3_HW2_UserInfo user = new FK3_HW2_UserInfo();

	public static FK3_GameContext game = new FK3_GameContext();

	public static FK3_LobbyContext lobby = new FK3_LobbyContext();

	public static int hallId = 0;

	public static int roomId = 0;

	public static List<GameObject> dontDestroyOnLoadList = new List<GameObject>();

	public static FK3_AppConfig appConfig
	{
		get;
		private set;
	}

	public static bool IsHasTag(string tag)
	{
		return false;
	}

	public static void SetUserInfo(FK3_HW2_UserInfo _user)
	{
		user = _user;
	}

	public static void SetGameContext(FK3_GameContext _game)
	{
		game = _game;
	}

	public static void SetLobbyContext(FK3_LobbyContext _lobby)
	{
		lobby = _lobby;
	}

	public static void SetAppConfig(FK3_AppConfig _appConfig)
	{
		appConfig = _appConfig;
	}

	public static int GetCurSkinType(FK3_GunType m_curType)
	{
		if (m_curType == FK3_GunType.FK3_NormalGun)
		{
			return 0;
		}
		return 1;
	}
}
