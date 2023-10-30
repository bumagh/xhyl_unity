using LL_GameCommon;
using UnityEngine;

public class LL_GameInfo
{
	private int mLanguage = -1;

	private string mIp = string.Empty;

	private LL_SettedInfo mSetted;

	private LL_PersonInfo mUserInfo;

	private string mUserId;

	private string mPwd;

	private string mKey = string.Empty;

	private bool mIsSpecial;

	private LL_LoadScene mLoadScene;

	public LoadType LoadStep;

	private static LL_GameInfo mGameInfo;

	public LL_LoadScene LoadScene
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

	public LL_PersonInfo UserInfo
	{
		get
		{
			return mUserInfo;
		}
		set
		{
			if (LoadStep != LoadType.On_Login)
			{
				mUserInfo.CoinCount = value.CoinCount;
				mUserInfo.ExpCoinCount = value.ExpCoinCount;
			}
			else
			{
				LoadStep = LoadType.On_LoadUserSetting;
				mUserInfo = value;
				LL_NetMngr.shouldBeBlocked = true;
			}
		}
	}

	public LL_SettedInfo Setted
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

	public static LL_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new LL_GameInfo();
		}
		return mGameInfo;
	}

	public void ClearGameInfo()
	{
		mKey = null;
		mGameInfo = null;
	}

	public void _setViewport()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		float g_fWidth = LL_ScreenParameter.G_fWidth;
		float g_fHeight = LL_ScreenParameter.G_fHeight;
		float x = 0f;
		float y = 0f;
		float num3 = 1f;
		float num4 = 1f;
		if (g_fWidth / g_fHeight > num / num2)
		{
			num3 = 1f;
			num4 = g_fHeight * num / g_fWidth / num2;
			y = (1f - num4) / 2f;
		}
		else if (g_fWidth / g_fHeight < num / num2)
		{
			num4 = 1f;
			num3 = g_fWidth * num2 / g_fHeight / num;
			x = (1f - num3) / 2f;
		}
		Camera camera = Camera.allCameras[0];
		camera.rect = new Rect(x, y, num3, num4);
	}
}
