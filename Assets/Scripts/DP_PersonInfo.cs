public class DP_PersonInfo
{
	private string mStrId = string.Empty;

	private string mStrPassword = string.Empty;

	private string mStrName = string.Empty;

	private bool mIsMale = true;

	private bool mIsOverFlow;

	private bool mUserIdFrezon;

	private bool mIsGlobalFibbidChat;

	private bool mIsSelfFibbidChat;

	private int mIconIndex = 1;

	private int mLevel = 1;

	private int mGameId;

	private int mRoomId = -1;

	private int mTableId = -1;

	private int mSeatId = -1;

	public string strId
	{
		get
		{
			return mStrId;
		}
		set
		{
			mStrId = value;
		}
	}

	public string strPassword
	{
		get
		{
			return mStrPassword;
		}
		set
		{
			mStrPassword = value;
		}
	}

	public string strName
	{
		get
		{
			return mStrName;
		}
		set
		{
			mStrName = value;
		}
	}

	public bool IsMale
	{
		get
		{
			return mIsMale;
		}
		set
		{
			mIsMale = value;
		}
	}

	public bool IsOverFlow
	{
		get
		{
			return mIsOverFlow;
		}
		set
		{
			mIsOverFlow = value;
		}
	}

	public bool UserIdFrezon
	{
		get
		{
			return mUserIdFrezon;
		}
		set
		{
			mUserIdFrezon = value;
		}
	}

	public bool IsGlobalFibbidChat
	{
		get
		{
			return mIsGlobalFibbidChat;
		}
		set
		{
			mIsGlobalFibbidChat = value;
		}
	}

	public bool IsSelfFibbidChat
	{
		get
		{
			return mIsSelfFibbidChat;
		}
		set
		{
			mIsSelfFibbidChat = value;
		}
	}

	public int IconIndex
	{
		get
		{
			return mIconIndex;
		}
		set
		{
			if (value <= 8 && value >= 1)
			{
				mIconIndex = value;
			}
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

	public int GameId
	{
		get
		{
			return mGameId;
		}
		set
		{
			mGameId = value;
		}
	}

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

	public int TableId
	{
		get
		{
			return mTableId;
		}
		set
		{
			mTableId = value;
		}
	}

	public int SeatId
	{
		get
		{
			return mSeatId;
		}
		set
		{
			mSeatId = value;
		}
	}

	public void ResetUserInfo()
	{
		strId = string.Empty;
		strName = string.Empty;
		IsMale = true;
		IsOverFlow = false;
		UserIdFrezon = false;
		IsGlobalFibbidChat = false;
		IsSelfFibbidChat = false;
		IconIndex = 1;
		Level = 1;
		GameId = 0;
		RoomId = -1;
		TableId = -1;
		SeatId = -1;
	}
}
