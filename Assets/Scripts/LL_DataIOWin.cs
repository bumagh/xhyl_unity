// using Microsoft.Win32;
// using System.Diagnostics;
// using UnityEngine;
//
// public class LL_DataIOWin
// {
// 	private string mRsgName;
//
// 	private RegistryKey mRsg;
//
// 	private static LL_DataIOWin g_Singleton;
//
// 	private string mUserId;
//
// 	private string mPwd;
//
// 	private string mIP;
//
// 	private bool mIsStartFromeGame;
//
// 	private bool mIsStartFromHall;
//
// 	private int mLanguage;
//
// 	private int mLastStartTime;
//
// 	public LL_DataIOWin()
// 	{
// 		mRsgName = "SOFTWARE\\XingLiData";
// 		mRsg = Registry.LocalMachine.OpenSubKey(mRsgName);
// 		if (mRsg == null)
// 		{
// 			FirstCreateRsg(mRsgName);
// 		}
// 		mRsg = Registry.LocalMachine.OpenSubKey(mRsgName, writable: true);
// 	}
//
// 	public static LL_DataIOWin GetSingleton()
// 	{
// 		if (g_Singleton == null)
// 		{
// 			g_Singleton = new LL_DataIOWin();
// 		}
// 		return g_Singleton;
// 	}
//
// 	private void FirstCreateRsg(string rsgName)
// 	{
// 		RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(rsgName);
// 		registryKey = Registry.LocalMachine.OpenSubKey(rsgName, writable: true);
// 		registryKey.SetValue("userId", string.Empty);
// 		registryKey.SetValue("pwd", string.Empty);
// 		registryKey.SetValue("HallStartFromGame", "false");
// 		registryKey.SetValue("GameStartFromHall", "false");
// 		registryKey.SetValue("IP", string.Empty);
// 		registryKey.SetValue("LANGUAGE", "0");
// 		registryKey.SetValue("StartTime", "201410300922");
// 		mRsg = registryKey;
// 	}
//
// 	public string GetUserID()
// 	{
// 		mUserId = _readData("userId");
// 		return mUserId;
// 	}
//
// 	public string GetUserPwd()
// 	{
// 		mPwd = _readData("pwd");
// 		return mPwd;
// 	}
//
// 	public string GetHallIP()
// 	{
// 		mIP = _readData("IP");
// 		return mIP;
// 	}
//
// 	public int GetLanguage()
// 	{
// 		mLanguage = int.Parse(_readData("LANGUAGE"));
// 		if (mLanguage != 1 && mLanguage != 0)
// 		{
// 			return 0;
// 		}
// 		return mLanguage;
// 	}
//
// 	public bool IsStartFromHall()
// 	{
// 		mIsStartFromHall = bool.Parse(_readData("GameStartFromHall"));
// 		_writeData("HallStartFromGame", "false");
// 		_writeData("GameStartFromHall", "false");
// 		return mIsStartFromHall;
// 	}
//
// 	public bool IsStartFromGame()
// 	{
// 		mIsStartFromeGame = bool.Parse(_readData("HallStartFromGame"));
// 		_writeData("HallStartFromGame", "false");
// 		_writeData("GameStartFromHall", "false");
// 		return mIsStartFromeGame;
// 	}
//
// 	public int GetStartTime()
// 	{
// 		mLastStartTime = int.Parse(_readData("StartTime"));
// 		return mLastStartTime;
// 	}
//
// 	public void WriteUserInfo(string strName, string strPwd, bool isHall, string IP = "")
// 	{
// 		_writeData("userId", strName);
// 		_writeData("pwd", strPwd);
// 		if (isHall && IP != string.Empty)
// 		{
// 			_writeData("IP", IP);
// 		}
// 		if (isHall)
// 		{
// 			_writeData("HallStartFromGame", "false");
// 			_writeData("GameStartFromHall", "true");
// 		}
// 		else
// 		{
// 			_writeData("HallStartFromGame", "true");
// 			_writeData("GameStartFromHall", "false");
// 		}
// 	}
//
// 	protected string _readData(string strName)
// 	{
// 		string result = string.Empty;
// 		if (mRsg != null)
// 		{
// 			result = (string)mRsg.GetValue(strName);
// 		}
// 		else
// 		{
// 			UnityEngine.Debug.Log("Rsg load failed.");
// 		}
// 		return result;
// 	}
//
// 	protected int _writeData(string strName, string strValue)
// 	{
// 		if (mRsg != null)
// 		{
// 			mRsg.SetValue(strName, strValue);
// 			return 0;
// 		}
// 		UnityEngine.Debug.Log("Rsg load failed.");
// 		return 1;
// 	}
//
// 	public void StartPCExe(string gamePath)
// 	{
// 		Process.Start(gamePath);
// 	}
// }
