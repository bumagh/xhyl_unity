using DP_GameCommon;
using UnityEngine;

public class DP_GameInfo
{
	private int mLanguage = -1;

	private string mIp = string.Empty;

	private DP_TableInfo tableInfo;

	private DP_SettedInfo mSetted;

	private DP_PersonInfo mUserInfo;

	private string mUserId;

	private string mPwd;

	private string mKey = string.Empty;

	private bool mIsSpecial;

	public LoadType LoadStep;

	private AppState appState;

	private DP_SceneUi sceneUi;

	private DP_SceneGame sceneGame;

	private int mCoinCount;

	private int mGameScore;

	private int mExpCoinCount;

	private int mCurrentWin;

	public int betTime;

	public int pointerLocation;

	public int[] colors = new int[24];

	public int[] beilv = new int[12];

	private static DP_GameInfo mGameInfo;

	public AppState GetAppState
	{
		get
		{
			return appState;
		}
		set
		{
			appState = value;
		}
	}

	public DP_SceneUi SceneUi
	{
		get
		{
			return sceneUi;
		}
		set
		{
			sceneUi = value;
		}
	}

	public DP_SceneGame SceneGame
	{
		get
		{
			return sceneGame;
		}
		set
		{
			sceneGame = value;
		}
	}

	public int Language
	{
		get
		{
			if (mLanguage == -1)
			{
				mLanguage = 0;
			}
			return mLanguage;
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

	public DP_TableInfo TableInfo
	{
		get
		{
			return tableInfo;
		}
		set
		{
			tableInfo = value;
			UpdateTable();
		}
	}

	public DP_PersonInfo UserInfo
	{
		get
		{
			return mUserInfo;
		}
		set
		{
			if (LoadStep == LoadType.On_Login)
			{
				LoadStep = LoadType.On_LoadUserSetting;
				DP_NetMngr.shouldBeBlocked = true;
			}
			mUserInfo = value;
		}
	}

	public DP_SettedInfo Setted
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
				return;
			}
			mCoinCount = value;
			if (appState == AppState.App_On_Game && mUserInfo.RoomId == 1)
			{
				if (!sceneGame.sptHud.bInit)
				{
					sceneGame.sptHud.Init();
				}
				sceneGame.sptHud.txtCoin.text = mCoinCount.ToString();
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
			if (appState == AppState.App_On_Game)
			{
				if (!sceneGame.sptHud.bInit)
				{
					sceneGame.sptHud.Init();
				}
				sceneGame.sptHud.txtScore.text = mGameScore.ToString();
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
				DP_ErrorManager.GetSingleton().AddError("Error:体验币数错误:" + value);
			}
			else
			{
				mExpCoinCount = value;
			}
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
			if (value >= 0)
			{
				mCurrentWin = value;
			}
		}
	}

	public static DP_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new DP_GameInfo();
		}
		return mGameInfo;
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

	public void SetTableInfo(int iTableId, int[] iUserKeyID, string[] strNickname, int[] iIconId, int iRoomId = 1, int nLength = 8)
	{
		for (int i = 0; i < 8; i++)
		{
			if (i <= nLength - 1)
			{
				tableInfo.SetUserKeyID(i, iUserKeyID[i]);
				tableInfo.SetNick(i, strNickname[i]);
				tableInfo.SetIcon(i, iIconId[i]);
			}
			else
			{
				tableInfo.SetUserKeyID(i, 1);
				tableInfo.SetNick(i, string.Empty);
				tableInfo.SetIcon(i);
			}
		}
	}

	public void UpdateTable()
	{
		for (int i = 0; i < 8; i++)
		{
			if (sceneUi != null)
			{
				sceneUi.AddPerson(i + 1, tableInfo.Nickname()[i], tableInfo.IconIndex()[i]);
			}
			if (sceneGame != null)
			{
				sceneGame.sptHud.AddUser(i, tableInfo.Nickname()[i], tableInfo.IconIndex()[i]);
			}
		}
	}
}
