using UnityEngine;

public class DP_TableInfo
{
	protected int mRoomId = -1;

	protected int mTableServerID;

	protected int mPersonCount;

	protected string mTableName = string.Empty;

	protected int mRistrict;

	protected int mMinBet = 50;

	protected int mMaxBet = 500;

	protected int mMaxZXBet = 500;

	protected int mMaxHBet = 500;

	protected int mMinZHXBet = 100;

	protected int mCreditPerCoin = 50;

	protected int mCoinInSetting = 500;

	protected int mMaxCDTime = 30;

	protected int mIsAutoKick;

	protected int[] mUserKeyID = new int[8]
	{
		1,
		1,
		1,
		1,
		1,
		1,
		1,
		1
	};

	public string[] mNickname = new string[8]
	{
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty
	};

	public int[] mIconIndex = new int[8]
	{
		1,
		1,
		1,
		1,
		1,
		1,
		1,
		1
	};

	public int RoomId
	{
		get
		{
			return mRoomId;
		}
		set
		{
			mRoomId = value;
		}
	}

	public int TableServerID
	{
		get
		{
			return mTableServerID;
		}
		set
		{
			mTableServerID = value;
		}
	}

	public int PersonCount
	{
		get
		{
			return mPersonCount;
		}
		set
		{
			mPersonCount = value;
		}
	}

	public string TableName
	{
		get
		{
			return mTableName;
		}
		set
		{
			mTableName = value;
		}
	}

	public int Ristrict
	{
		get
		{
			return mRistrict;
		}
		set
		{
			mRistrict = value;
		}
	}

	public int MinBet
	{
		get
		{
			return mMinBet;
		}
		set
		{
			mMinBet = value;
		}
	}

	public int MaxBet
	{
		get
		{
			return mMaxBet;
		}
		set
		{
			mMaxBet = value;
		}
	}

	public int MaxZXBet
	{
		get
		{
			return mMaxZXBet;
		}
		set
		{
			mMaxZXBet = value;
		}
	}

	public int MaxHBet
	{
		get
		{
			return mMaxHBet;
		}
		set
		{
			mMaxHBet = value;
		}
	}

	public int MinZHXBet
	{
		get
		{
			return mMinZHXBet;
		}
		set
		{
			mMinZHXBet = value;
		}
	}

	public int CreditPerCoin
	{
		get
		{
			return mCreditPerCoin;
		}
		set
		{
			mCreditPerCoin = value;
		}
	}

	public int CoinInSetting
	{
		get
		{
			return mCoinInSetting;
		}
		set
		{
			mCoinInSetting = value;
		}
	}

	public int MaxCD
	{
		get
		{
			return mMaxCDTime;
		}
		set
		{
			mMaxCDTime = value;
		}
	}

	public int IsAutoKick
	{
		get
		{
			return mIsAutoKick;
		}
		set
		{
			mIsAutoKick = value;
		}
	}

	public void SetUserKeyID(int iIndex, int ID)
	{
		if (iIndex < 8 && iIndex >= 0)
		{
			mUserKeyID[iIndex] = ID;
		}
		else
		{
			UnityEngine.Debug.Log("Error:座位号错误");
		}
	}

	public int GetUserKeyID(int iIndex)
	{
		int result = 1;
		if (iIndex < 8 && iIndex >= 0)
		{
			result = mUserKeyID[iIndex];
		}
		else
		{
			UnityEngine.Debug.Log("Error:座位号错误");
		}
		return result;
	}

	public void SetNick(int iIndex, string str)
	{
		if (iIndex < 8 && iIndex >= 0)
		{
			mNickname[iIndex] = str;
		}
		else
		{
			UnityEngine.Debug.Log("Error:座位号错误");
		}
	}

	public string[] Nickname()
	{
		return mNickname;
	}

	public void SetIcon(int iIndex, int iIconId = 1)
	{
		if (iIndex > 7 || iIndex < 0)
		{
			UnityEngine.Debug.Log("Error:座位号错误");
		}
		else if (iIconId > 8 || iIconId < 1)
		{
			UnityEngine.Debug.Log("Error:玩家头像错误");
		}
		else
		{
			mIconIndex[iIndex] = iIconId;
		}
	}

	public int[] IconIndex()
	{
		return mIconIndex;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
