using System.Collections;
using UnityEngine;

public class LKB_IOSGameStart : MonoBehaviour
{
	private string _loginParam = string.Empty;

	private string _version = string.Empty;

	private static LKB_IOSGameStart G_IOSGameStart;

	private string YouKeName = string.Empty;

	public string mLoginParam
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

	public string mVersion
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

	public static LKB_IOSGameStart GetSingleton()
	{
		return G_IOSGameStart;
	}

	private void Awake()
	{
		if (G_IOSGameStart != null)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
		else if (G_IOSGameStart == null)
		{
			G_IOSGameStart = this;
		}
	}

	public void StartGameHall(bool isCanLogin)
	{
		StopCoroutine("StartHall");
		StartCoroutine("StartHall", isCanLogin);
	}

	private IEnumerator StartHall(object typeValue)
	{
		yield return new WaitForFixedUpdate();
		bool isCanLogin = (bool)typeValue;
		string headStr = "XingLiHall://www.xingli.com?";
		string name = GetPassedUserName(mLoginParam);
		string pwd = GetPassedPwd(mLoginParam);
		if (pwd.CompareTo(string.Empty) == 0)
		{
			pwd = "123";
			name = YouKeName;
		}
		if (!isCanLogin)
		{
			pwd = "12";
			name = "aa";
		}
		UnityEngine.Debug.Log("IP =     " + GetPassedIp(mLoginParam));
		string str = headStr + "+NAME=" + name + "+PWD=" + pwd + "+IP=" + GetPassedIp(mLoginParam) + "+LAN=" + GetPassedLan(mLoginParam);
		Application.OpenURL(str);
		Application.Quit();
	}

	public string GetPassedUserName()
	{
		return GetPassedUserName(mLoginParam);
	}

	public string GetPassedUserName(string para)
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

	public string GetPassedPwd()
	{
		return GetPassedPwd(mLoginParam);
	}

	public string GetPassedPwd(string para)
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

	public string GetPassedIp()
	{
		return GetPassedIp(mLoginParam);
	}

	public string GetPassedIp(string para)
	{
		int num = para.IndexOf("+Domain=") + 8;
		int num2 = para.IndexOf("+NAME=");
		return para.Substring(num, num2 - num);
	}

	public string GetPassedLan()
	{
		return GetPassedLan(mLoginParam);
	}

	public string GetPassedLan(string para)
	{
		int startIndex = para.IndexOf("+LAN=") + 5;
		return para.Substring(startIndex);
	}

	public void GetLoginPara(string paraStr)
	{
	}

	public void GetPwd(string pwd)
	{
	}

	public void GetGameVersion()
	{
	}

	public void TellGameVersion(string version)
	{
		_version = version;
	}

	public void UpdateGameVesion(string downloadadress)
	{
		Application.OpenURL(downloadadress);
		Application.Quit();
	}
}
