using BCBM_GameCommon;
using UnityEngine;

public class BCBM_Constants : MonoBehaviour
{
	public const int PORT = 10010;

	public const int MSGLENTH = 4;

	public static string IP = BCBM_Parameter.GetSingleton().GetSvrIP();

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
}
