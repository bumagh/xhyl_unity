using UnityEngine;

public class BaiJiaLe_Constants : MonoBehaviour
{
	public const int PORT = 10010;

	public const int MSGLENTH = 4;

	public static string IP = GetSvrIP();

	public static string VERSION_CODE = "9.0.2";

	private static string _version;

	public static string Version
	{
		get
		{
			return _version;
		}
		set
		{
			_version = value;
		}
	}

	public static string GetSvrIP()
	{
		return BaiJiaLe_GameInfo.getInstance().IP;
	}
}
