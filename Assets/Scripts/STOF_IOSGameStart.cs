using UnityEngine;

public static class STOF_IOSGameStart
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
		string text4 = text + "+NAME=" + text2 + "+PWD=" + text3 + "+IP=" + GetPassedIp(mLoginParam) + "+LAN=" + GetPassedLan(mLoginParam);
		DoCopy(text4);
		Application.OpenURL(text4);
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
		return para.Substring(num, num2 - num);
	}

	public static string GetPassedPwd()
	{
		return GetPassedPwd(mLoginParam);
	}

	public static string GetPassedPwd(string para)
	{
		int num = para.IndexOf("+PWD=") + 5;
		int num2 = para.IndexOf("+IP=");
		return para.Substring(num, num2 - num);
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
