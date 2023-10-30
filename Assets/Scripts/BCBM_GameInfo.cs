using BCBM_GameCommon;

public class BCBM_GameInfo
{
	private int mLanguage = -1;

	private string mIp = "106.52.101.52";

	private BCBM_SettedInfo mSetted;

	private BCBM_PersonInfo mUserInfo;

	private string mUserId;

	private string mPwd;

	private string mKey = string.Empty;

	private bool mIsSpecial;

	private BCBM_LoadScene mLoadScene;

	public LoadType LoadStep;

	public int BetTime = 30;

	private static BCBM_GameInfo mGameInfo;

	public static bool kaishi;

	public BCBM_LoadScene LoadScene
	{
		get
		{
			return mLoadScene;
		}
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
				LoadStep = LoadType.On_Login;
			}
			mKey = value;
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

	public BCBM_PersonInfo UserInfo
	{
		get
		{
			return mUserInfo;
		}
		set
		{
			if (LoadStep != LoadType.On_Login)
			{
				mUserInfo = value;
				mUserInfo.CoinCount = value.CoinCount;
				mUserInfo.ExpCoinCount = value.ExpCoinCount;
			}
			else
			{
				LoadStep = LoadType.On_LoadUserSetting;
				mUserInfo = value;
				BCBM_NetMngr.shouldBeBlocked = true;
			}
		}
	}

	public BCBM_SettedInfo Setted
	{
		get
		{
			return mSetted;
		}
		set
		{
			mSetted = value;
			LoadStep = LoadType.On_PrepareLoadScene;
		}
	}

	public static BCBM_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new BCBM_GameInfo();
		}
		return mGameInfo;
	}

	public static void ClearGameInfo()
	{
		getInstance().mKey = string.Empty;
		mGameInfo = null;
	}
}
