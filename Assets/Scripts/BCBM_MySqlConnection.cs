using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class BCBM_MySqlConnection
{
	public const string ABDir = "AB";

	public const string touristPwd = "123456";

	public const string touristAccountPrefix_cn = "游客";

	public const string touristAccountPrefix_en = "tourist";

	public static string IPAddress = string.Empty;

	public static string IPAddress_Game = string.Empty;

	public static int IPPort = 0;

	public static string versionURL = string.Empty;

	public static string versionCode = string.Empty;

	public static string language = "zh";

	public static string username = string.Empty;

	public static string pwd = string.Empty;

	public static string nickname = string.Empty;

	public static string netVersion = "1.2.15";

	public static bool isTourist = false;

	public static bool isOverflow = false;

	public static int payMode = 1;

	public static bool useRaffleStore = false;

	public static int pwdSafeBox = 0;

	public static bool hasSetUpProtectQuestion = false;

	public static int gold = 0;

	public static int expeGold = 0;

	public static int raffle = 0;

	public static int savedGameGold = 0;

	public static int savedLottery = 0;

	public static Dictionary<int, BCBM_OwnShopProp> ownedProps = new Dictionary<int, BCBM_OwnShopProp>();

	public static bool isInternational = true;

	public static string lastChargeTime;

	public static int[] orders = new int[6]
	{
		1,
		14,
		7,
		16,
		0,
		5
	};

	public static string[] versions = new string[10]
	{
		"9.0.11",
		"9.0.2",
		"9.0.1",
		"9.0.1",
		"9.0.1",
		"9.0.1",
		"9.0.1",
		"9.0.1",
		"9.0.1",
		"9.0.1"
	};

	public static string[] QianpaoName = new string[2]
	{
		"千炮",
		"qianpao"
	};

	public static string[] WanpaoName = new string[2]
	{
		"万炮",
		"wanpao"
	};

	public static bool isShowHallName = true;

	public static User user;

	public static string downloadStr = string.Empty;

	public static int giftMode = 0;

	public static int shareMode = 2;

	public static int specialMark = 2;

	public static bool ScoreOverflow = false;

	public static string gamesJsonFilePath = "Assets/Resources/Saves/games.json";

	private static bool localDown = false;

	public static bool isStartedFromLuaGame = false;

	public static bool isStartedFromGame = false;

	public static bool lockSend = false;

	public static bool lockQuit = false;

	public static bool lockReconnect = false;

	public static bool lockRelogin = false;

	public static string AccountName
	{
		get
		{
			string str = string.Empty;
			if (isTourist)
			{
				str = ((language == "zh") ? "游客" : "tourist");
			}
			return str + username;
		}
	}

	public static string DownBaseUrl => "http://192.168.1.80:8080/";

	public static string DataPath
	{
		get
		{
			if (Application.isMobilePlatform)
			{
				return Application.persistentDataPath + "/";
			}
			return Path.GetFullPath(Path.Combine(Application.dataPath, "../Down/")).Replace('\\', '/');
		}
	}

	public static string WWWDataPath
	{
		get
		{
			if (Application.isEditor)
			{
				return "file://" + DataPath;
			}
			if (Application.isMobilePlatform || Application.isConsolePlatform)
			{
				return "file:///" + Application.persistentDataPath + "/";
			}
			return "file://" + DataPath;
		}
	}

	public static string PlatformDir => "Android";

	public static string GetOSDir()
	{
		return "Android";
	}
}
