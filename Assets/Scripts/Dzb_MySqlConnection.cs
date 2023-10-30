using System.Collections.Generic;

public class Dzb_MySqlConnection
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

	public static Dzb_User user;

	public static List<Dzb_Room> roomList = new List<Dzb_Room>();

	public static Dzb_Room room;

	public static Dzb_Desk desk;

	public static List<Dzb_Seat> seatList = new List<Dzb_Seat>();

	public static Dzb_Seat seat;

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
