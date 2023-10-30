using GameConfig;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BZJX_GameInfo
{
	private BZJX_LoadType mLoadStep;

	private int mLanguage = -1;

	private string mIp = string.Empty;

	private BZJX_GameState mGameState;

	private BZJX_UserInfo mUser;

	private BZJX_SettedInfo mSetted;

	private BZJX_TableInfo mTable;

	private List<BZJX_TableInfo> mTableList;

	private int mTableIndex;

	private string mKey = string.Empty;

	private bool mIsGameShutUp;

	private bool mIsUserShutUp;

	private string mUserId;

	private string mPwd;

	private bool mIsSpecial;

	private bool mNetShouldBlocked;

	private bool bCountTime;

	private List<BZJX_UserInfo> mUserList;

	private BZJX_LoadScene mLoadScene;

	private BZJX_UIScene mUIScene;

	private BZJX_GameScene mGameScene;

	private static BZJX_GameInfo mGameInfo;

	private string mStatusURL;

	public BZJX_LoadType LoadStep
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

	public BZJX_LoadScene LoadScene
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

	public BZJX_UIScene UIScene
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

	public BZJX_GameScene GameScene
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
				mLoadScene.Step = BZJX_LoadType.On_Login;
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

	public BZJX_GameState currentState
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

	public BZJX_SettedInfo Setted
	{
		get
		{
			return mSetted;
		}
		set
		{
			mSetted = value;
			mLoadScene.Step = BZJX_LoadType.On_PrepareLoadScene;
		}
	}

	public BZJX_UserInfo User
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
				mLoadScene.Step = BZJX_LoadType.On_LoadUserSetting;
				mNetShouldBlocked = true;
			}
			mUser = value;
			if (mGameState == BZJX_GameState.On_Game)
			{
				mGameScene.UpdateUserInfo();
			}
			else if (mGameState != 0)
			{
				mUIScene.UpdateUserInfo();
			}
		}
	}

	public BZJX_TableInfo Table
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

	public List<BZJX_UserInfo> UserList
	{
		get
		{
			return mUserList;
		}
		set
		{
			mUserList = value;
			if (mGameState == BZJX_GameState.On_Game)
			{
				mGameScene.UpdateTableUser();
			}
			else
			{
				mUIScene.UpdateTableUsers();
			}
		}
	}

	public List<BZJX_TableInfo> TableList
	{
		get
		{
			return mTableList;
		}
		set
		{
			mTableList = value;
			selectTable();
			if (mGameState == BZJX_GameState.On_Game)
			{
				mGameScene.UpdateTableConfig();
				return;
			}
			mUIScene.UpdateTableList();
			mUIScene.UpdateTableConfig();
			mUIScene.UpdateUserInfo();
		}
	}

	public static BZJX_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new BZJX_GameInfo();
		}
		return mGameInfo;
	}

	public static void ClearGameInfo()
	{
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

	public void SelectTable(int deskId)
	{
		for (int i = 0; i < mTableList.Count; i++)
		{
			if (mTableList[i].Id == deskId)
			{
				mTable = mTableList[i];
			}
		}
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

	public void createUser(BZJX_User netUser)
	{
		if (mUser != null)
		{
			mUser = null;
		}
		BZJX_UserInfo bZJX_UserInfo2 = User = new BZJX_UserInfo(netUser);
	}

	public void updateUser(string whichChanged, object ob)
	{
		if (whichChanged != null)
		{
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
		if (mGameState == BZJX_GameState.On_Game)
		{
			mGameScene.UpdateUserInfo();
		}
		else if (mGameState == BZJX_GameState.On_SelectRoom || mGameState == BZJX_GameState.On_SelectTable)
		{
			mUIScene.UpdateUserInfo();
		}
	}

	public void SetNoHall(bool isHavHall)
	{
		if ((bool)mUIScene)
		{
			mUIScene.SetNoHall(isHavHall);
		}
	}

	public void updateTableList(BZJX_FishDesk[] netDesks)
	{
		List<BZJX_TableInfo> list = new List<BZJX_TableInfo>();
		BZJX_TableInfo bZJX_TableInfo = null;
		if ((bool)mUIScene)
		{
			mUIScene.InItTable(netDesks);
		}
		if (netDesks != null)
		{
			for (int i = 0; i < netDesks.Length; i++)
			{
				bZJX_TableInfo = new BZJX_TableInfo(netDesks[i]);
				list.Add(bZJX_TableInfo);
			}
			mUser.RoomId = bZJX_TableInfo.RoomId - 1;
			if (mUser.RoomId < 0)
			{
				mUser.RoomId = 0;
			}
			TableList = list;
			if (mGameState == BZJX_GameState.On_SelectRoom)
			{
				mUIScene.EnterRoom();
				getInstance().currentState = BZJX_GameState.On_SelectTable;
				mUIScene.UpdateUserInfo();
			}
		}
	}

	public void updateCurUsers(BZJX_Seat[] netSeats)
	{
		List<BZJX_UserInfo> list = new List<BZJX_UserInfo>();
		for (int i = 0; i < netSeats.Length; i++)
		{
			BZJX_UserInfo item = BZJX_UserInfo.createOtherUser(netSeats[i]);
			list.Add(item);
		}
		list.Sort(BZJX_UserInfo.compareBySeat);
		UserList = list;
	}

	public void updateOtherUsers(BZJX_Seat[] netSeats, int deskId = -1)
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
		try
		{
			if ((bool)mUIScene)
			{
				mUIScene.InSeat(netSeats);
			}
			if ((bool)mUIScene)
			{
				mUIScene.UpdateSeat(netSeats, deskId);
			}
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		List<BZJX_UserInfo> list = new List<BZJX_UserInfo>();
		for (int j = 0; j < netSeats.Length; j++)
		{
			BZJX_UserInfo item = BZJX_UserInfo.createOtherUser(netSeats[j]);
			list.Add(item);
		}
		list.Sort(BZJX_UserInfo.compareBySeat);
		if (getInstance().currentState == BZJX_GameState.On_SelectTable && !ZH2_GVars.isStartGame)
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
			if (!list[k].IsExist && getInstance().currentState == BZJX_GameState.On_Game)
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
		if (mGameState != BZJX_GameState.On_Game)
		{
			if (mTableIndex == i)
			{
				mUIScene.UpdateTableConfig();
			}
			mUIScene.UpdateTableListItem(i);
		}
	}

	public void getPersonInfo(BZJX_UserInfo user, int honor)
	{
		if (mGameState == BZJX_GameState.On_Game)
		{
			mGameScene.ShowUserInfo(user, honor);
		}
	}
}
