using System.Collections.Generic;

public class LRS_GVars
{
	public static bool isInit = false;

	public static string IPAddress = string.Empty;

	public static int IPPort = 0;

	public static string versionURL = string.Empty;

	public static string versionCode = string.Empty;

	public static string language = string.Empty;

	public static string username = string.Empty;

	public static string pwd = string.Empty;

	public static bool isSpecial;

	public static string curView = string.Empty;

	public static LRS_User user;

	public static LRS_Desk desk;

	public static int credit;

	public static int winScorce;

	public static int realCredit;

	public static int goldType;

	public static float RandomTime;

	public static Dictionary<string, bool> btnLockDic = new Dictionary<string, bool>();

	public static bool lockOnePoint = false;

	public static string Version = "1.2.15";

	public static bool tryLockOnePoint
	{
		get
		{
			if (!lockOnePoint)
			{
				lockOnePoint = true;
				return false;
			}
			return true;
		}
	}
}
