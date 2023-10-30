public class XLDT_UserInfo : XLDT_BaseItem
{
	private bool mIsExist;

	private int mSeatIndex;

	private int mGunValue;

	private string mAccount;

	private char mSex;

	private int mPhotoId;

	private int mCoinCount;

	private int mScoreCount;

	private int mTestCoinCount;

	private int mIsOverflow;

	private int mLevel;

	private int mType;

	public int Type
	{
		get
		{
			return mType;
		}
		set
		{
			mType = value;
		}
	}

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

	public int Icon
	{
		get
		{
			return mPhotoId;
		}
		set
		{
			mPhotoId = value;
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

	public XLDT_UserInfo()
	{
	}

	public XLDT_UserInfo(XLDT_User netUser)
	{
		if (netUser != null)
		{
			mId = netUser.id;
			mAccount = netUser.username;
			mName = netUser.nickname;
			mPhotoId = netUser.photoId;
			mCoinCount = netUser.gameGold;
			mTestCoinCount = netUser.expeGold;
			mSex = netUser.sex;
			mIsOverflow = netUser.overflow;
			mLevel = netUser.level;
			mScoreCount = ((netUser.gameScore > 0) ? netUser.gameScore : netUser.expeScore);
			mType = netUser.type;
			mSeatIndex = -1;
			mIsExist = true;
			mGunValue = -1;
			mRoomId = -1;
		}
	}

	public XLDT_UserInfo(int seatIndex, int photoId, string nickName)
	{
		mAccount = "gongjingtao";
		mName = nickName;
		mPhotoId = photoId;
		mSeatIndex = seatIndex;
		mIsExist = true;
	}

	public static XLDT_UserInfo createOtherUser(XLDT_Seat netSeat)
	{
		XLDT_UserInfo xLDT_UserInfo = new XLDT_UserInfo();
		xLDT_UserInfo.SeatIndex = netSeat.id;
		xLDT_UserInfo.Name = netSeat.userNickname;
		xLDT_UserInfo.mPhotoId = netSeat.photoId;
		xLDT_UserInfo.IsExist = !netSeat.isFree;
		xLDT_UserInfo.Id = netSeat.userId;
		return xLDT_UserInfo;
	}

	public static int compareBySeat(XLDT_UserInfo t1, XLDT_UserInfo t2)
	{
		return (t1.SeatIndex < t2.SeatIndex) ? (-1) : ((t1.SeatIndex > t2.SeatIndex) ? 1 : 0);
	}
}
