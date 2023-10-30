using UnityEngine;

public class TF_IOSGameStart : MonoBehaviour
{
	private static string _loginParam = string.Empty;

	private static string _version = string.Empty;

	private static string YouKeName = string.Empty;

	public static string mLoginParam
	{
		get
		{
			return _loginParam;
		}
		set
		{
			_loginParam = value;
		}
	}

	public static string mVersion
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

	public static void DoCopy(string str)
	{
	}

	public static void StartGameHall(bool bStartFromGame)
	{
		string text = "XingLiHall://www.xingli.com?";
		string text2 = GetPassedUserName(mLoginParam);
		string text3 = GetPassedPwd(mLoginParam);
		if (!bStartFromGame)
		{
			text2 = string.Empty;
			text3 = string.Empty;
		}
		string url = text + "+NAME=" + text2 + "+PWD=" + text3 + "+IP=" + GetPassedIp(mLoginParam) + "+LAN=" + GetPassedLan(mLoginParam);
		Application.OpenURL(url);
		Application.Quit();
	}

	public static string GetPassedUserName()
	{
		return GetPassedUserName(mLoginParam);
	}

	public static string GetPassedUserName(string para)
	{
		int num = para.IndexOf("+NAME=") + 6;
		int num2 = para.IndexOf("+PWD=");
		int num3 = para.IndexOf("+PWD=") + 5;
		int num4 = para.IndexOf("+IP=");
		int num5 = para.IndexOf("+IP=") + 4;
		int num6 = para.IndexOf("+LAN=");
		int num7 = para.IndexOf("+LAN=") + 5;
		string text = YouKeName = para.Substring(num, num2 - num);
		string text2 = para.Substring(num3, num4 - num3);
		if (text2.CompareTo("123") == 0)
		{
			return "游客" + text;
		}
		return text;
	}

	public static string GetPassedPwd()
	{
		return GetPassedPwd(mLoginParam);
	}

	public static string GetPassedPwd(string para)
	{
		int num = para.IndexOf("+PWD=") + 5;
		int num2 = para.IndexOf("+IP=");
		string text = para.Substring(num, num2 - num);
		if (text.CompareTo("123") == 0)
		{
			text = string.Empty;
		}
		return text;
	}

	public static string GetPassedIp()
	{
		return GetPassedIp(mLoginParam);
	}

	public static string GetPassedIp(string para)
	{
		int num = para.IndexOf("+Domain=") + 8;
		int num2 = para.IndexOf("+NAME=");
		return para.Substring(num, num2 - num);
	}

	public static string GetPassedLan()
	{
		return GetPassedLan(mLoginParam);
	}

	public static string GetPassedLan(string para)
	{
		int startIndex = para.IndexOf("+LAN=") + 5;
		return para.Substring(startIndex);
	}

	public static void UpdateGameVesion(string downloadadress)
	{
	}
}
