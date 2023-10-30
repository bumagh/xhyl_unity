public class XLDT_TableInfo : XLDT_BaseItem
{
	private int mMinCarry;

	private int mMinGun;

	private int mMaxGun;

	private int mDeltaGun;

	private int mDeltaCoin;

	private int mScorePerCoin;

	private int mPersonCount;

	private int mRestrain;

	private int mMinBet;

	private int mBaseYaFen;

	private int mLockState;

	public XLDT_Seat[] mSeat;

	public int LockState
	{
		get
		{
			return mLockState;
		}
		set
		{
			mLockState = value;
		}
	}

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

	public int Restrain
	{
		get
		{
			return mRestrain;
		}
		set
		{
			mRestrain = value;
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

	public int BaseYaFen
	{
		get
		{
			return mBaseYaFen;
		}
		set
		{
			mBaseYaFen = value;
		}
	}

	public XLDT_TableInfo()
	{
		mSeat = new XLDT_Seat[8];
	}

	public XLDT_TableInfo(XLDT_CardDesk netDesk)
	{
		mSeat = new XLDT_Seat[8];
		mId = netDesk.id;
		mName = netDesk.name;
		mRoomId = netDesk.roomId;
		mScorePerCoin = netDesk.exchange;
		mMinCarry = netDesk.minGold;
		mDeltaCoin = netDesk.onceExchangeValue;
		mSeat = netDesk.seats;
		mRestrain = netDesk.gameXianHong;
		mMinBet = netDesk.minYaFen;
		mLockState = netDesk.state;
		mSeat = netDesk.seats;
		mPersonCount = 0;
		mBaseYaFen = netDesk.baseYaFen;
		for (int i = 0; i < mSeat.Length; i++)
		{
			if (!mSeat[i].isFree)
			{
				mPersonCount++;
			}
		}
	}

	public static int compareById(XLDT_TableInfo t1, XLDT_TableInfo t2)
	{
		return (t1.mId < t2.mId) ? (-1) : ((t1.mId > t2.mId) ? 1 : 0);
	}
}
