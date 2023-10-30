using UnityEngine;

public class LHD_Constants : MonoBehaviour
{
	public const int PORT = 10010;

	public const int MSGLENTH = 4;

	public static string IP = string.Empty;

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
