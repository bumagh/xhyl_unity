using System.Collections.Generic;

public class Hfh_GVars
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

	public static Hfh_User user;

	public static List<Hfh_Room> roomList = new List<Hfh_Room>();

	public static Hfh_Room room;

	public static List<Hfh_Seat> seatList = new List<Hfh_Seat>();

	public static Hfh_Seat seat;

	public static int credit;

	public static int realCredit;

	public static int goldType;

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
