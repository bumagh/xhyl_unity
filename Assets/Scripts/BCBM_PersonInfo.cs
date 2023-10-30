using System;
using UnityEngine;

public class BCBM_PersonInfo : MonoBehaviour
{
	private bool startgame;

	public BCBM_SeatList mSeatList;

	public BCBM_TableList mTableList;

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

	protected int mUserId;

	protected int mRoomId = -1;

	protected string mRoomName = string.Empty;

	protected int mHallId = -1;

	protected int mTableId = -1;

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
			if (BCBM_AppUIMngr.GetSingleton() != null)
			{
				BCBM_AppUIMngr.GetSingleton().UpdateUserInfo();
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
			if (BCBM_AppUIMngr.GetSingleton() != null)
			{
				BCBM_AppUIMngr.GetSingleton().UpdateUserInfo();
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
				try
				{
					BCBM_ErrorManager.GetSingleton().AddError("Error:体验币数错误:" + value);
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
				}
				return;
			}
			mExpCoinCount = value;
			if (BCBM_AppUIMngr.GetSingleton() != null)
			{
				BCBM_AppUIMngr.GetSingleton().UpdateUserInfo();
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

	public string RoomName
	{
		get
		{
			return mRoomName;
		}
		set
		{
			mRoomName = value;
		}
	}

	public int HallId
	{
		get
		{
			return mHallId;
		}
		set
		{
			mHallId = value;
			if (BCBM_AppUIMngr.GetSingleton() != null)
			{
				BCBM_AppUIMngr.GetSingleton().UpdateUserInfo();
			}
			else
			{
				UnityEngine.Debug.LogError("=======HallId==BCBM_AppUIMngr======为空");
			}
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
			BCBM_GameInfo.getInstance().UserInfo = BCBM_UI.mUserInfo;
		}
	}

	private void Start()
	{
		BCBM_PersonInfo userInfo = BCBM_GameInfo.getInstance().UserInfo;
	}

	private void Update()
	{
		if (BCBM_NetMngr.shouldBeBlocked)
		{
			BCBM_NetMngr.shouldBeBlocked = false;
		}
	}

	public void ResetUserInfo()
	{
		UnityEngine.Debug.LogError("===========ResetUserInfo=======");
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
		GameId = 0;
		UserId = 0;
		RoomId = -1;
		TableId = -1;
		SeatId = -1;
	}
}
