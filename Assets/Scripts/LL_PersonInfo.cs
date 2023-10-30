using UnityEngine;

public class LL_PersonInfo : MonoBehaviour
{
	protected LL_HudManager mGameHud;

	protected LL_BetPanel mBet;

	protected LL_PrizeResult mResult;

	protected LL_HudManager mHudPanel;

	protected LL_SeatList mSeatList;

	protected LL_ChatPanel mChatPanel;

	protected LL_RoomList mRoomList;

	protected LL_TableList mTableList;

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
			if ((bool)mRoomList)
			{
				mRoomList.updateUserInfo();
			}
			if ((bool)mTableList)
			{
				mTableList.updateUserInfo();
			}
			if ((bool)mSeatList)
			{
				mSeatList.updateUserInfo();
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
			if ((bool)mChatPanel)
			{
				if (!value && !mIsSelfFibbidChat)
				{
					mChatPanel.IsForbidChat(bIsForbidChat: false);
				}
				else
				{
					mChatPanel.IsForbidChat(bIsForbidChat: true);
				}
			}
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
			if ((bool)mChatPanel)
			{
				if (!value && !mIsGlobalFibbidChat)
				{
					mChatPanel.IsForbidChat(bIsForbidChat: false);
				}
				else
				{
					mChatPanel.IsForbidChat(bIsForbidChat: true);
				}
			}
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
			if (value <= 4 && value >= 1)
			{
				mIconIndex = value;
				return;
			}
			UnityEngine.Debug.Log("玩家头像错误: " + value);
			value = 1;
			mIconIndex = value;
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
			if ((bool)mSeatList)
			{
				mSeatList.updateUserInfo();
			}
			if ((bool)mGameHud && mRoomId == 1)
			{
				mGameHud.GameCoin = mCoinCount;
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
			if ((bool)mGameHud)
			{
				mGameHud.GameCredit = value;
			}
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
				LL_ErrorManager.GetSingleton().AddError("Error:体验币数错误:" + value);
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
			if ((bool)mSeatList)
			{
				mSeatList.updateUserInfo();
			}
			if ((bool)mGameHud && mRoomId == 0)
			{
				mGameHud.GameCoin = mExpCoinCount;
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
				return;
			}
			mCurrentWin = value;
			if ((bool)mResult)
			{
				mResult.CurrentWin = value;
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

	private void Start()
	{
		LL_PersonInfo userInfo = LL_GameInfo.getInstance().UserInfo;
		userInfo.mGameHud = GameObject.Find("HudPanel").GetComponent<LL_HudManager>();
		userInfo.mBet = GameObject.Find("BetPanel").GetComponent<LL_BetPanel>();
		userInfo.mResult = GameObject.Find("ResultPanel").GetComponent<LL_PrizeResult>();
		userInfo.mHudPanel = GameObject.Find("HudPanel").GetComponent<LL_HudManager>();
		userInfo.mSeatList = GameObject.Find("SeatPanel").GetComponent<LL_SeatList>();
		userInfo.mChatPanel = GameObject.Find("ChatPanel").GetComponent<LL_ChatPanel>();
		userInfo.mRoomList = LL_AppUIMngr.GetSingleton().mRoomList;
		userInfo.mTableList = GameObject.Find("TableListPanel").GetComponent<LL_TableList>();
	}

	private void Update()
	{
		if (LL_NetMngr.shouldBeBlocked)
		{
			LL_NetMngr.shouldBeBlocked = false;
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
		GameId = 0;
		RoomId = -1;
		TableId = -1;
		SeatId = -1;
	}
}
