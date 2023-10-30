using UnityEngine;

public class STTF_UserInfo : STTF_BaseItem
{
	private bool mIsExist;

	private int mSeatIndex;

	private int mGunValue;

	private bool bLock;

	private GameObject mLockFish;

	private string mAccount;

	private char mSex;

	private int mIcon;

	private int mCoinCount;

	private int mScoreCount;

	private int mTestCoinCount;

	private int mIsOverflow;

	private int mLevel;

	public char Sex
	{
		get
		{
			return mSex;
		}
		set
		{
			mSex = value;
		}
	}

	public bool Lock
	{
		get
		{
			return bLock;
		}
		set
		{
			bLock = value;
		}
	}

	public GameObject LockFish
	{
		get
		{
			return mLockFish;
		}
		set
		{
			mLockFish = value;
		}
	}

	public int Icon
	{
		get
		{
			return mIcon;
		}
		set
		{
			mIcon = value;
		}
	}

	public int CoinCount
	{
		get
		{
			return mCoinCount;
		}
		set
		{
			mCoinCount = value;
		}
	}

	public int ScoreCount
	{
		get
		{
			return mScoreCount;
		}
		set
		{
			mScoreCount = value;
		}
	}

	public int TestCoinCount
	{
		get
		{
			return mTestCoinCount;
		}
		set
		{
			mTestCoinCount = value;
		}
	}

	public int SeatIndex
	{
		get
		{
			return mSeatIndex;
		}
		set
		{
			mSeatIndex = value;
		}
	}

	public int IsOverFlow
	{
		get
		{
			return mIsOverflow;
		}
		set
		{
			mIsOverflow = value;
		}
	}

	public int GunValue
	{
		get
		{
			return mGunValue;
		}
		set
		{
			mGunValue = value;
		}
	}

	public bool IsExist
	{
		get
		{
			return mIsExist;
		}
		set
		{
			mIsExist = value;
		}
	}

	public int Level
	{
		get
		{
			return mLevel;
		}
		set
		{
			mLevel = value;
		}
	}

	public string UserAccount => mAccount;

	public STTF_UserInfo(STTF_User netUser)
	{
		if (netUser != null)
		{
			mId = netUser.id;
			mAccount = netUser.username;
			mName = netUser.nickname;
			mIcon = netUser.photoId;
			mCoinCount = netUser.gameGold;
			mTestCoinCount = netUser.expeGold;
			mSex = netUser.sex;
			mIsOverflow = netUser.overflow;
			mLevel = netUser.level;
			mScoreCount = ((netUser.gameScore > 0) ? netUser.gameScore : netUser.expeScore);
			mSeatIndex = -1;
			mIsExist = true;
			mGunValue = -1;
			mRoomId = -1;
		}
	}

	public STTF_UserInfo(int seatIndex, int photoId, string nickName)
	{
		mAccount = "gongjingtao";
		mName = nickName;
		mIcon = photoId;
		mSeatIndex = seatIndex;
		mIsExist = true;
	}

	public static STTF_UserInfo createOtherUser(STTF_Seat netSeat)
	{
		STTF_UserInfo sTTF_UserInfo = new STTF_UserInfo(netSeat.user);
		sTTF_UserInfo.mSeatIndex = netSeat.seatId;
		sTTF_UserInfo.mIsExist = !netSeat.isFree;
		sTTF_UserInfo.mGunValue = netSeat.gunValue;
		return sTTF_UserInfo;
	}

	public static int compareBySeat(STTF_UserInfo t1, STTF_UserInfo t2)
	{
		return (t1.SeatIndex < t2.SeatIndex) ? (-1) : ((t1.SeatIndex > t2.SeatIndex) ? 1 : 0);
	}
}
