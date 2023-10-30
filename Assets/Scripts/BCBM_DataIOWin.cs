using System.Diagnostics;
using UnityEngine;

public class BCBM_DataIOWin
{
	private string mRsgName;

	private static BCBM_DataIOWin g_Singleton;

	private string mUserId;

	private string mPwd;

	private string mIP;

	private bool mIsStartFromeGame;

	private bool mIsStartFromHall;

	private int mLanguage;

	private int mLastStartTime;

	public static BCBM_DataIOWin GetSingleton()
	{
		if (g_Singleton == null)
		{
			g_Singleton = new BCBM_DataIOWin();
		}
		return g_Singleton;
	}

	private void FirstCreateRsg(string rsgName)
	{
	}

	public string GetUserID()
	{
		mUserId = _readData("userId");
		return mUserId;
	}

	public string GetUserPwd()
	{
		mPwd = _readData("pwd");
		return mPwd;
	}

	public string GetHallIP()
	{
		mIP = _readData("IP");
		return mIP;
	}

	public int GetLanguage()
	{
		mLanguage = int.Parse(_readData("LANGUAGE"));
		if (mLanguage != 1 && mLanguage != 0)
		{
			return 0;
		}
		return mLanguage;
	}

	public bool IsStartFromHall()
	{
		mIsStartFromHall = bool.Parse(_readData("GameStartFromHall"));
		_writeData("HallStartFromGame", "false");
		_writeData("GameStartFromHall", "false");
		return mIsStartFromHall;
	}

	public bool IsStartFromGame()
	{
		mIsStartFromeGame = bool.Parse(_readData("HallStartFromGame"));
		_writeData("HallStartFromGame", "false");
		_writeData("GameStartFromHall", "false");
		return mIsStartFromeGame;
	}

	public int GetStartTime()
	{
		mLastStartTime = int.Parse(_readData("StartTime"));
		return mLastStartTime;
	}

	public void WriteUserInfo(string strName, string strPwd, bool isHall, string IP = "")
	{
		_writeData("userId", strName);
		_writeData("pwd", strPwd);
		if (isHall && IP != string.Empty)
		{
			_writeData("IP", IP);
		}
		if (isHall)
		{
			_writeData("HallStartFromGame", "false");
			_writeData("GameStartFromHall", "true");
		}
		else
		{
			_writeData("HallStartFromGame", "true");
			_writeData("GameStartFromHall", "false");
		}
	}

	protected string _readData(string strName)
	{
		return string.Empty;
	}

	protected int _writeData(string strName, string strValue)
	{
		UnityEngine.Debug.Log("Rsg load failed.");
		return 1;
	}

	public void StartPCExe(string gamePath)
	{
		Process.Start(gamePath);
	}
}
