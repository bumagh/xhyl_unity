using UnityEngine;

public class IOS_Agent : MonoBehaviour
{
	public static string g_msg = string.Empty;

	public static bool CanOpenUrl(string url)
	{
		return false;
	}

	public static string GetPassedUserName()
	{
		return GetPassedUserName(g_msg);
	}

	public static string GetPassedUserName(string para)
	{
		int num = para.IndexOf("+NAME=") + 6;
		int num2 = para.IndexOf("+PWD=");
		return para.Substring(num, num2 - num);
	}

	public static string GetPassedPwd()
	{
		return GetPassedPwd(g_msg);
	}

	public static string GetPassedPwd(string para)
	{
		int num = para.IndexOf("+PWD=") + 5;
		int num2 = para.IndexOf("+IP=");
		return para.Substring(num, num2 - num);
	}

	public static string GetPassedLan()
	{
		return GetPassedLan(g_msg);
	}

	public static string GetPassedLan(string para)
	{
		int startIndex = para.IndexOf("+LAN=") + 5;
		return para.Substring(startIndex);
	}
}
