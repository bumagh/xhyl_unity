using UnityEngine;

public class TF_UserInfo : TF_BaseItem
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

	public TF_UserInfo(TF_User netUser)
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
			mScoreCount = 0;
			mSeatIndex = -1;
			mIsExist = true;
			mGunValue = -1;
			mRoomId = -1;
		}
	}

	public TF_UserInfo(int seatIndex, int photoId, string nickName)
	{
		mAccount = "gongjingtao";
		mName = nickName;
		mIcon = photoId;
		mSeatIndex = seatIndex;
		mIsExist = true;
	}

	public static TF_UserInfo createOtherUser(TF_Seat netSeat)
	{
		TF_UserInfo tF_UserInfo = new TF_UserInfo(netSeat.user);
		tF_UserInfo.mSeatIndex = netSeat.seatId;
		tF_UserInfo.mIsExist = !netSeat.isFree;
		tF_UserInfo.mGunValue = netSeat.gunValue;
		return tF_UserInfo;
	}

	public static int compareBySeat(TF_UserInfo t1, TF_UserInfo t2)
	{
		return (t1.SeatIndex < t2.SeatIndex) ? (-1) : ((t1.SeatIndex > t2.SeatIndex) ? 1 : 0);
	}
}
