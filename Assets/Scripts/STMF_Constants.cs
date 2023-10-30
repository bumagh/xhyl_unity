using UnityEngine;

public class STMF_Constants : MonoBehaviour
{
	public const int PORT = 10011;

	public const int MSGLENTH = 4;

	public static int S_SvrID = 0;

	public static string IP = GetSvrIP();

	public static string VERSION_CODE = "9.0.1";

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
		return STMF_GameInfo.getInstance().IP;
	}
}
