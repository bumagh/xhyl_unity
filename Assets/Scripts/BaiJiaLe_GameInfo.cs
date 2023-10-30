using com.miracle9.game.bean;
using com.miracle9.game.entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaiJiaLe_GameInfo
{
	private static BaiJiaLe_GameInfo mGameInfo;

	public BaiJiaLe_LoadScene mLoadScene;

	public int[] ZhuangCards = new int[3];

	public int[] XianCards = new int[3];

	public int ZhuangValue;

	public int XianValue;

	public int ZhuangNum;

	public int XianNum;

	private int port = 19927;

	private string mIp = string.Empty;

	private string mUserId = string.Empty;

	private string mPwd = string.Empty;

	private string mVersion = "1.2.15";

	private string mCode = "1.2.15";

	private bool mNetShouldBlocked;

	public static bool IsGetLoginInfo;

	public static bool IsConnect;

	public static bool IsLoginSuccess;

	public static bool IsBreak;

	public static bool IsJoinRoom;

	public static bool IsJoinDesk;

	public static int ErrorNum;

	public static float MusicVolume = 1f;

	public static float SoundVolume = 1f;

	public BaiJiaLe_User PlayerUser;

	public string NickName = string.Empty;

	public string GameGold = "0";

	public string GameScore = "0";

	public int Ratio;

	public List<BaiJiaLe_FreeBankerDesk> GameDeskList = new List<BaiJiaLe_FreeBankerDesk>();

	public List<BaiJiaLe_Seat> GameSeatList = new List<BaiJiaLe_Seat>();

	public BaiJiaLe_FreeBankerDesk GameDesk;

	public BaiJiaLe_FreeBankerDeskResult GameResult;

	public object[] GameWinResult;

	public List<BaiJiaLe_DeskSeatBet> DeskSeatBet = new List<BaiJiaLe_DeskSeatBet>();

	public List<int[]> PlayerChips = new List<int[]>();

	public List<string> GameLuDan = new List<string>();

	public int PlayerId;

	public string RoomID;

	public string SeatID;

	public int WaitTime;

	public int BetTime;

	public string IP
	{
		get
		{
			return mIp;
		}
		set
		{
			mIp = value;
		}
	}

	public int Port
	{
		get
		{
			return port;
		}
		set
		{
			port = value;
		}
	}

	public string UserId
	{
		get
		{
			return mUserId;
		}
		set
		{
			mUserId = value;
		}
	}

	public string Pwd
	{
		get
		{
			return mPwd;
		}
		set
		{
			mPwd = value;
		}
	}

	public string Version
	{
		get
		{
			return mVersion;
		}
		set
		{
			mVersion = value;
		}
	}

	public string Code
	{
		get
		{
			return mCode;
		}
		set
		{
			mCode = value;
		}
	}

	public bool NetShouldBlocked
	{
		get
		{
			return mNetShouldBlocked;
		}
		set
		{
			mNetShouldBlocked = value;
		}
	}

	public static BaiJiaLe_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new BaiJiaLe_GameInfo();
		}
		return mGameInfo;
	}

	public int GetCardNum(int value)
	{
		if (value % 13 < 10)
		{
			return value % 13;
		}
		return 0;
	}

	private IEnumerator Polling()
	{
		while (true)
		{
			if (IsJoinRoom && !IsJoinDesk)
			{
				BaiJiaLe_Sockets.GetSingleton().SendUpdateRoom();
			}
			else if (IsJoinDesk)
			{
				BaiJiaLe_Sockets.GetSingleton().SenddeskInfo1(int.Parse(getInstance().RoomID));
			}
			yield return new WaitForSeconds(2f);
		}
	}
}
