using UnityEngine;

public class JSYS_LL_PersonInfo : MonoBehaviour
{
	private bool startgame;

	public JSYS_LL_RoomList mRoomList;

	public JSYS_LL_TableList mTableList;

	protected string mStrId = string.Empty;

	protected string mStrPassword = string.Empty;

	protected string mStrName = string.Empty;

	protected bool mIsMale = true;

	protected bool mIsOverFlow;

	protected bool mUserIdFrezon;

	protected bool mIsGlobalFibbidChat;

	protected bool mIsSelfFibbidChat;

	protected int mIconIndex = 1;

	protected int mCoinCount;

	protected int mGameScore;

	protected int mExpCoinCount;

	protected int mLevel = 1;

	protected int mCurrentWin;

	protected int mGameId;

	protected int mRoomId = -1;

	protected int mTableId = -1;

	protected int mUserId;

	protected int mSeatId = -1;

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

	public int UserId
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
			return ZH2_GVars.GetBreviaryName(mStrName);
		}
		set
		{
			mStrName = value;
			if ((bool)mRoomList)
			{
				mRoomList.updateUserInfo();
			}
			if ((bool)mTableList)
			{
				mTableList.updateUserInfo();
			}
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
			else
			{
				mIconIndex = 1;
			}
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
			if (value < 0)
			{
				UnityEngine.Debug.Log("Error:游戏币数错误");
				return;
			}
			mCoinCount = value;
			if ((bool)mRoomList)
			{
				mRoomList.updateUserInfo();
			}
			if ((bool)mTableList)
			{
				mTableList.updateUserInfo();
			}
		}
	}

	public int GameScore
	{
		get
		{
			return mGameScore;
		}
		set
		{
			mGameScore = value;
		}
	}

	public int ExpCoinCount
	{
		get
		{
			return mExpCoinCount;
		}
		set
		{
			if (value < 0)
			{
				JSYS_LL_ErrorManager.GetSingleton().AddError("Error:体验币数错误:" + value);
				return;
			}
			mExpCoinCount = value;
			if ((bool)mRoomList)
			{
				mRoomList.updateUserInfo();
			}
			if ((bool)mTableList)
			{
				mTableList.updateUserInfo();
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

	public int CurrentWin
	{
		get
		{
			return mCurrentWin;
		}
		set
		{
			if (value < 0)
			{
				UnityEngine.Debug.Log("Error:当局赢分错误");
			}
			else
			{
				mCurrentWin = value;
			}
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

	private void Awake()
	{
		if (JSYS_UI.mUserInfo != null)
		{
			JSYS_LL_GameInfo.getInstance().UserInfo = JSYS_UI.mUserInfo;
		}
	}

	private void Start()
	{
		JSYS_LL_PersonInfo userInfo = JSYS_LL_GameInfo.getInstance().UserInfo;
	}

	private void Update()
	{
		if (JSYS_LL_NetMngr.shouldBeBlocked)
		{
			JSYS_LL_NetMngr.shouldBeBlocked = false;
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
		CoinCount = 0;
		GameScore = 0;
		ExpCoinCount = 0;
		Level = 1;
		CurrentWin = 0;
		UserId = 0;
		GameId = 0;
		RoomId = -1;
		TableId = -1;
		SeatId = -1;
	}
}
