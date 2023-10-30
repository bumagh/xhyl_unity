public class BZJX_TableInfo : BZJX_BaseItem
{
	private int mMinCarry;

	private int mMinGun;

	private int mMaxGun;

	private int mDeltaGun;

	private int mDeltaCoin;

	private int mScorePerCoin;

	private int mPersonCount;

	public BZJX_Seat[] mNetSeats;

	public int MinCarry
	{
		get
		{
			return mMinCarry;
		}
		set
		{
			mMinCarry = value;
		}
	}

	public int MinGun
	{
		get
		{
			return mMinGun;
		}
		set
		{
			mMinGun = value;
		}
	}

	public int MaxGun
	{
		get
		{
			return mMaxGun;
		}
		set
		{
			mMaxGun = value;
		}
	}

	public int ScorePerCoin
	{
		get
		{
			return mScorePerCoin;
		}
		set
		{
			mScorePerCoin = value;
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

	public int DeltaGun
	{
		get
		{
			return mDeltaGun;
		}
		set
		{
			mDeltaGun = value;
		}
	}

	public int DeltaCoin
	{
		get
		{
			return mDeltaCoin;
		}
		set
		{
			mDeltaCoin = value;
		}
	}

	public BZJX_TableInfo()
	{
	}

	public BZJX_TableInfo(BZJX_FishDesk netDesk)
	{
		mId = netDesk.id;
		mName = netDesk.name;
		mRoomId = netDesk.roomId;
		mScorePerCoin = netDesk.exchange;
		mMinCarry = netDesk.minGold;
		mMinGun = netDesk.minGunValue;
		mMaxGun = netDesk.maxGunValue;
		mDeltaGun = netDesk.addstepGunValue;
		mDeltaCoin = netDesk.onceExchangeValue;
		mNetSeats = netDesk.seats;
	}

	public static int compareById(BZJX_TableInfo t1, BZJX_TableInfo t2)
	{
		return (t1.mId < t2.mId) ? (-1) : ((t1.mId > t2.mId) ? 1 : 0);
	}
}
