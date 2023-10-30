using JSYS_LL_GameCommon;

public class JSYS_LL_GameInfo
{
	private int mLanguage = -1;

	private string mIp = "106.52.101.52";

	private JSYS_LL_SettedInfo mSetted;

	private JSYS_LL_PersonInfo mUserInfo;

	private string mUserId;

	private string mPwd;

	private string mKey = string.Empty;

	private bool mIsSpecial;

	private JSYS_LL_LoadScene mLoadScene;

	public LoadType LoadStep;

	public int[] BeiLv = new int[15]
	{
		4,
		8,
		8,
		12,
		4,
		8,
		8,
		12,
		99,
		24,
		2,
		2,
		0,
		0,
		0
	};

	public int[] BetChip = new int[5];

	private static JSYS_LL_GameInfo mGameInfo;

	public static bool kaishi;

	public JSYS_LL_LoadScene LoadScene
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

	public JSYS_LL_PersonInfo UserInfo
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
				JSYS_LL_NetMngr.shouldBeBlocked = true;
			}
		}
	}

	public JSYS_LL_SettedInfo Setted
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

	public static JSYS_LL_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new JSYS_LL_GameInfo();
		}
		return mGameInfo;
	}

	public static void ClearGameInfo()
	{
		mGameInfo = null;
	}
}
