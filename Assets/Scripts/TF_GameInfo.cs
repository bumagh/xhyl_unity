using GameConfig;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TF_GameInfo
{
	private TF_LoadType mLoadStep;

	private int mLanguage = -1;

	private string mIp = string.Empty;

	private TF_GameState mGameState;

	private TF_UserInfo mUser;

	private TF_SettedInfo mSetted;

	private TF_TableInfo mTable;

	private List<TF_TableInfo> mTableList;

	private int mTableIndex;

	private string mKey = string.Empty;

	private bool mIsGameShutUp;

	private bool mIsUserShutUp;

	private string mUserId;

	private string mPwd;

	private bool mIsSpecial;

	private bool mNetShouldBlocked;

	private bool bCountTime;

	private List<TF_UserInfo> mUserList;

	private TF_LoadScene mLoadScene;

	private TF_UIScene mUIScene;

	private TF_GameScene mGameScene;

	private static TF_GameInfo mGameInfo;

	private string mStatusURL;

	public TF_LoadType LoadStep
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

	public TF_LoadScene LoadScene
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

	public TF_UIScene UIScene
	{
		get
		{
			return mUIScene;
		}
		set
		{
			mUIScene = value;
		}
	}

	public TF_GameScene GameScene
	{
		get
		{
			return mGameScene;
		}
		set
		{
			mGameScene = value;
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
				mLoadScene.Step = TF_LoadType.On_Login;
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
		}
	}

	public bool CountTime
	{
		get
		{
			return bCountTime;
		}
		set
		{
			bCountTime = value;
			mGameScene.StartFixScreenCountTime(bCountTime);
		}
	}

	public TF_GameState currentState
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

	public int TableIndex
	{
		get
		{
			return mTableIndex;
		}
		set
		{
			mTableIndex = value;
		}
	}

	public TF_SettedInfo Setted
	{
		get
		{
			return mSetted;
		}
		set
		{
			mSetted = value;
			mLoadScene.Step = TF_LoadType.On_PrepareLoadScene;
		}
	}

	public TF_UserInfo User
	{
		get
		{
			return mUser;
		}
		set
		{
			if (mUser != null)
			{
				value.RoomId = mUser.RoomId;
				value.SeatIndex = mUser.SeatIndex;
				value.GunValue = mUser.GunValue;
			}
			else
			{
				mLoadScene.Step = TF_LoadType.On_LoadUserSetting;
				mNetShouldBlocked = true;
			}
			mUser = value;
			if (mGameState == TF_GameState.On_Game)
			{
				mGameScene.UpdateUserInfo();
			}
			else if (mGameState != 0)
			{
				mUIScene.UpdateUserInfo();
			}
		}
	}

	public TF_TableInfo Table
	{
		get
		{
			return mTable;
		}
		set
		{
			mTable = value;
		}
	}

	public List<TF_UserInfo> UserList
	{
		get
		{
			return mUserList;
		}
		set
		{
			mUserList = value;
			if (mGameState == TF_GameState.On_Game)
			{
				mGameScene.UpdateTableUser();
			}
			else
			{
				mUIScene.UpdateTableUsers();
			}
		}
	}

	public List<TF_TableInfo> TableList
	{
		get
		{
			return mTableList;
		}
		set
		{
			mTableList = value;
			selectTable();
			if (mGameState == TF_GameState.On_Game)
			{
				mGameScene.UpdateTableConfig();
				return;
			}
			UnityEngine.Debug.Log("SetTableList");
			mUIScene.SetIndicator();
			mUIScene.UpdateTableList();
			mUIScene.UpdateTableConfig();
			mUIScene.UpdateUserInfo();
		}
	}

	public static TF_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new TF_GameInfo();
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
		if (mTable != null)
		{
			int i;
			for (i = 0; i < mTableList.Count && mTable.Id != TableList[i].Id; i++)
			{
			}
			mTableIndex = ((i != mTableList.Count) ? i : 0);
		}
		mTable = mTableList[mTableIndex];
	}

	public void changeTable(int direction)
	{
		if (direction == 0)
		{
			mTableIndex = ((mTableIndex != mTableList.Count - 1) ? (mTableIndex + 1) : 0);
		}
		else
		{
			mTableIndex = ((mTableIndex == 0) ? (mTableList.Count - 1) : (mTableIndex - 1));
		}
		mTable = mTableList[mTableIndex];
		mUIScene.UpdateTableConfig();
	}

	public void clearTableInfo()
	{
		mTableList.Clear();
		mTableList = null;
		mTable = null;
		mTableIndex = 0;
	}

	public void clearSeatInfo()
	{
		mUserList.Clear();
		mUserList = null;
	}

	public void createUser(TF_User netUser)
	{
		if (mUser != null)
		{
			mUser = null;
		}
		TF_UserInfo tF_UserInfo2 = User = new TF_UserInfo(netUser);
	}

	public void updateUser(string whichChanged, object ob)
	{
		try
		{
			if (whichChanged != null)
			{
				if (!(whichChanged == "gameCoin"))
				{
					if (!(whichChanged == "gameScore"))
					{
						if (whichChanged == "testCoin")
						{
							mUser.TestCoinCount = (int)Convert.ToDouble(ob);
						}
					}
					else
					{
						mUser.ScoreCount = (int)Convert.ToDouble(ob);
					}
				}
				else
				{
					mUser.CoinCount = (int)Convert.ToDouble(ob);
				}
			}
		}
		catch (Exception)
		{
		}
		if (mGameState == TF_GameState.On_Game)
		{
			mGameScene.UpdateUserInfo();
		}
		else if (mGameState == TF_GameState.On_SelectRoom || mGameState == TF_GameState.On_SelectTable)
		{
			mUIScene.UpdateUserInfo();
		}
	}

	public void updateTableList(TF_FishDesk[] netDesks)
	{
		List<TF_TableInfo> list = new List<TF_TableInfo>();
		TF_TableInfo tF_TableInfo = null;
		for (int i = 0; i < netDesks.Length; i++)
		{
			UnityEngine.Debug.Log(netDesks[i].name);
			tF_TableInfo = new TF_TableInfo(netDesks[i]);
			list.Add(tF_TableInfo);
		}
		mUser.RoomId = tF_TableInfo.RoomId;
		TableList = list;
		if (mGameState == TF_GameState.On_SelectRoom)
		{
			mUIScene.EnterRoom();
			getInstance().currentState = TF_GameState.On_SelectTable;
			mUIScene.UpdateUserInfo();
		}
	}

	public void updateCurUsers(TF_Seat[] netSeats)
	{
		List<TF_UserInfo> list = new List<TF_UserInfo>();
		for (int i = 0; i < netSeats.Length; i++)
		{
			TF_UserInfo item = TF_UserInfo.createOtherUser(netSeats[i]);
			list.Add(item);
		}
		list.Sort(TF_UserInfo.compareBySeat);
		UserList = list;
	}

	public void updateOtherUsers(TF_Seat[] netSeats, int deskId = -1)
	{
		if (deskId != -1)
		{
			for (int i = 0; i < TableList.Count; i++)
			{
				if (TableList[i].Id == deskId)
				{
					TableList[i].mNetSeats = netSeats;
				}
			}
		}
		List<TF_UserInfo> list = new List<TF_UserInfo>();
		for (int j = 0; j < netSeats.Length; j++)
		{
			TF_UserInfo item = TF_UserInfo.createOtherUser(netSeats[j]);
			list.Add(item);
		}
		list.Sort(TF_UserInfo.compareBySeat);
		if (getInstance().currentState == TF_GameState.On_SelectTable && !ZH2_GVars.isStartGame)
		{
			getInstance().User.SeatIndex = -1;
		}
		if (Table.Id != deskId || deskId == -1)
		{
			return;
		}
		for (int k = 0; k < UserList.Count; k++)
		{
			list[k].LockFish = UserList[k].LockFish;
			list[k].Lock = UserList[k].Lock;
			if (!list[k].IsExist && getInstance().currentState == TF_GameState.On_Game)
			{
				GameScene.areas[list[k].SeatIndex - 1].sptLock.sptLockCard.HideLockCard();
			}
		}
		UserList = list;
	}

	public void updateTableUserNumber(int tableId, int number)
	{
		int i;
		for (i = 0; i < mTableList.Count && mTableList[i].Id != tableId; i++)
		{
		}
		if (i == mTableList.Count)
		{
			return;
		}
		mTableList[i].PersonCount = number;
		if (mGameState != TF_GameState.On_Game)
		{
			if (mTableIndex == i)
			{
				mUIScene.UpdateTableConfig();
			}
			mUIScene.UpdateTableListItem(i);
		}
	}

	public void getPersonInfo(TF_UserInfo user, int honor)
	{
		if (mGameState == TF_GameState.On_Game)
		{
			mGameScene.ShowUserInfo(user, honor);
		}
		else
		{
			mUIScene.ShowPersonInfo(user, honor);
		}
	}
}
