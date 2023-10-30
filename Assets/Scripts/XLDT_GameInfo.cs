using STDT_GameConfig;
using System.Collections;
using System.Collections.Generic;

public class XLDT_GameInfo
{
	private XLDT_LoadType mLoadStep;

	private int mLanguage = -1;

	private string mIp = string.Empty;

	private XLDT_GameState mGameState;

	private XLDT_UserInfo mUser;

	private XLDT_SettedInfo mSetted;

	private XLDT_TableInfo mCurTable;

	private List<XLDT_TableInfo> mTableList = new List<XLDT_TableInfo>();

	private int mTotalTabNum;

	private string mKey = string.Empty;

	private bool mIsGameShutUp;

	private bool mIsUserShutUp;

	private string mUserId;

	private string mPwd;

	private bool mIsSpecial;

	private bool mNetShouldBlocked;

	private List<XLDT_UserInfo> mUserList;

	private int mCurTabIndex;

	private int mBetType = 1;

	private int mCountTime;

	private XLDT_LoadScene mLoadScene;

	private ArrayList mCoinList = new ArrayList();

	private int mGamesId;

	private List<XLDT_Income> mIncome = new List<XLDT_Income>();

	private XLDT_CurttenAward mCurAward = new XLDT_CurttenAward();

	private static XLDT_GameInfo mGameInfo;

	public XLDT_LoadType LoadStep
	{
		get
		{
			return mLoadStep;
		}
		set
		{
			mLoadStep = value;
		}
	}

	public XLDT_CurttenAward CurAward
	{
		get
		{
			return mCurAward;
		}
		set
		{
			mCurAward = value;
		}
	}

	public XLDT_LoadScene LoadScene
	{
		set
		{
			mLoadScene = value;
		}
	}

	public int Language
	{
		get
		{
			int num = (int)ZH2_GVars.language_enum;
			if (num < 0)
			{
				num = 0;
			}
			if (num > 1)
			{
				num = 1;
			}
			return num;
		}
		set
		{
			mLanguage = value;
		}
	}

	public int GamesId
	{
		get
		{
			return mGamesId;
		}
		set
		{
			mGamesId = value;
		}
	}

	public string IP
	{
		get
		{
			return mIp;
		}
		set
		{
			mIp = value;
		}
	}

	public string UserId
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

	public string Pwd
	{
		get
		{
			return mPwd;
		}
		set
		{
			mPwd = value;
		}
	}

	public List<XLDT_Income> Income
	{
		get
		{
			return mIncome;
		}
		set
		{
			mIncome = value;
		}
	}

	public bool IsSpecial
	{
		get
		{
			return mIsSpecial;
		}
		set
		{
			mIsSpecial = value;
		}
	}

	public bool NetShouldBlocked
	{
		get
		{
			return mNetShouldBlocked;
		}
		set
		{
			mNetShouldBlocked = value;
		}
	}

	public string Key
	{
		get
		{
			return mKey;
		}
		set
		{
			if (mKey == string.Empty)
			{
				mLoadScene.Step = XLDT_LoadType.On_Login;
			}
			mKey = value;
		}
	}

	public bool IsGameShuUp
	{
		get
		{
			return mIsGameShutUp;
		}
		set
		{
			mIsGameShutUp = value;
			if (mGameState != XLDT_GameState.On_Game)
			{
			}
		}
	}

	public bool IsUserShutUp
	{
		get
		{
			return mIsUserShutUp;
		}
		set
		{
			mIsUserShutUp = value;
			if (mGameState != XLDT_GameState.On_Game)
			{
			}
		}
	}

	public XLDT_GameState currentState
	{
		get
		{
			return mGameState;
		}
		set
		{
			mGameState = value;
		}
	}

	public int BetType
	{
		get
		{
			return mBetType;
		}
		set
		{
			mBetType = value;
		}
	}

	public int CountTime
	{
		get
		{
			return mCountTime;
		}
		set
		{
			mCountTime = value;
		}
	}

	public int TotalTabNum
	{
		get
		{
			return mTotalTabNum;
		}
		set
		{
			mTotalTabNum = value;
		}
	}

	public int CurTabIndex
	{
		get
		{
			return mCurTabIndex;
		}
		set
		{
			mCurTabIndex = value;
			if (mTableList.Count != 0)
			{
				mCurTable = mTableList[mCurTabIndex];
			}
		}
	}

	public ArrayList CoinList
	{
		get
		{
			return mCoinList;
		}
		set
		{
			mCoinList = value;
		}
	}

	public XLDT_TableInfo CurTable
	{
		get
		{
			if (mCurTabIndex == -1)
			{
				return null;
			}
			return mCurTable;
		}
	}

	public XLDT_SettedInfo Setted
	{
		get
		{
			return mSetted;
		}
		set
		{
			mSetted = value;
		}
	}

	public XLDT_UserInfo User
	{
		get
		{
			return mUser;
		}
		set
		{
			mUser = value;
			mLoadScene.Step = XLDT_LoadType.On_LoadUserSetting;
			if (mGameState != XLDT_GameState.On_Game && mGameState == XLDT_GameState.On_Loading)
			{
			}
		}
	}

	public XLDT_TableInfo Table
	{
		get
		{
			if (mCurTable == null)
			{
				mCurTable = new XLDT_TableInfo();
			}
			return mCurTable;
		}
		set
		{
			mCurTable = value;
		}
	}

	public List<XLDT_UserInfo> UserList
	{
		get
		{
			return mUserList;
		}
		set
		{
			mUserList = value;
			if (mGameState == XLDT_GameState.On_Game)
			{
				XLDT_GameUIMngr.GetSingleton().UpdateUserList(mUserList);
			}
		}
	}

	public List<XLDT_TableInfo> TableList
	{
		get
		{
			return mTableList;
		}
		set
		{
			mTableList = value;
		}
	}

	public static XLDT_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new XLDT_GameInfo();
		}
		return mGameInfo;
	}

	public void ClearGameInfo()
	{
		mKey = string.Empty;
		mGameInfo = null;
	}

	private void selectTable()
	{
	}

	public void changeTable(int direction)
	{
	}

	public void clearTableInfo()
	{
		mTableList.Clear();
		mTableList = null;
	}

	public void clearSeatInfo()
	{
		mUserList.Clear();
		mUserList = null;
	}

	public void createUser(XLDT_User netUser)
	{
		XLDT_UserInfo xLDT_UserInfo2 = User = new XLDT_UserInfo(netUser);
	}

	public void updateUser(string whichChanged, object ob)
	{
		if (whichChanged == null)
		{
			return;
		}
		if (!(whichChanged == "gameCoin"))
		{
			if (!(whichChanged == "gameScore"))
			{
				if (whichChanged == "testCoin")
				{
					mUser.TestCoinCount = (int)ob;
				}
			}
			else
			{
				mUser.ScoreCount = (int)ob;
			}
		}
		else
		{
			mUser.CoinCount = (int)ob;
		}
	}

	public void updateOtherUsers(XLDT_Seat[] netSeats)
	{
		List<XLDT_UserInfo> list = new List<XLDT_UserInfo>();
		for (int i = 0; i < netSeats.Length; i++)
		{
			XLDT_UserInfo item = XLDT_UserInfo.createOtherUser(netSeats[i]);
			list.Add(item);
		}
		list.Sort(XLDT_UserInfo.compareBySeat);
		UserList = list;
	}

	public void updateTableUserNumber(int tableId, int number)
	{
		int i;
		for (i = 0; i < mTableList.Count && mTableList[i].Id != tableId; i++)
		{
		}
		if (i != mTableList.Count)
		{
			mTableList[i].PersonCount = number;
			if (mGameState == XLDT_GameState.On_Game)
			{
			}
		}
	}

	public void getPersonInfo(XLDT_UserInfo user, int honor)
	{
	}
}
