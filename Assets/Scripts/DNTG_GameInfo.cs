using GameConfig;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DNTG_GameInfo
{
	private DNTG_LoadType mLoadStep;

	private int mLanguage = -1;

	private string mIp = string.Empty;

	private DNTG_GameState mGameState;

	private DNTG_UserInfo mUser;

	private DNTG_SettedInfo mSetted;

	private DNTG_TableInfo mTable;

	private List<DNTG_TableInfo> mTableList;

	private int mTableIndex;

	private string mKey = string.Empty;

	private bool mIsGameShutUp;

	private bool mIsUserShutUp;

	private string mUserId;

	private string mPwd;

	private bool mIsSpecial;

	private bool mNetShouldBlocked;

	private bool bCountTime;

	private List<DNTG_UserInfo> mUserList;

	private DNTG_LoadScene mLoadScene;

	private DNTG_UIScene mUIScene;

	private DNTG_GameScene mGameScene;

	private static DNTG_GameInfo mGameInfo;

	private string mStatusURL;

	public bool[] IsSuperShoot = new bool[4];

	public DNTG_LoadType LoadStep
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

	public DNTG_LoadScene LoadScene
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

	public DNTG_UIScene UIScene
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

	public DNTG_GameScene GameScene
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
				mLoadScene.Step = DNTG_LoadType.On_Login;
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

	public DNTG_GameState currentState
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

	public DNTG_SettedInfo Setted
	{
		get
		{
			return mSetted;
		}
		set
		{
			mSetted = value;
			mLoadScene.Step = DNTG_LoadType.On_PrepareLoadScene;
		}
	}

	public DNTG_UserInfo User
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
				mLoadScene.Step = DNTG_LoadType.On_LoadUserSetting;
				mNetShouldBlocked = true;
			}
			mUser = value;
			if (mGameState == DNTG_GameState.On_Game)
			{
				mGameScene.UpdateUserInfo();
			}
			else if (mGameState != 0)
			{
				mUIScene.UpdateUserInfo();
			}
		}
	}

	public DNTG_TableInfo Table
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

	public List<DNTG_UserInfo> UserList
	{
		get
		{
			return mUserList;
		}
		set
		{
			mUserList = value;
			if (mGameState == DNTG_GameState.On_Game)
			{
				mGameScene.UpdateTableUser();
			}
			else
			{
				mUIScene.UpdateTableUsers();
			}
		}
	}

	public List<DNTG_TableInfo> TableList
	{
		get
		{
			return mTableList;
		}
		set
		{
			mTableList = value;
			selectTable();
			if (mGameState == DNTG_GameState.On_Game)
			{
				mGameScene.UpdateTableConfig();
				return;
			}
			mUIScene.UpdateTableList();
			mUIScene.UpdateTableConfig();
			mUIScene.UpdateUserInfo();
		}
	}

	public static DNTG_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new DNTG_GameInfo();
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

	public void createUser(DNTG_User netUser)
	{
		if (mUser != null)
		{
			mUser = null;
		}
		DNTG_UserInfo dNTG_UserInfo2 = User = new DNTG_UserInfo(netUser);
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
		if (mGameState == DNTG_GameState.On_Game)
		{
			mGameScene.UpdateUserInfo();
		}
		else if (mGameState == DNTG_GameState.On_SelectRoom || mGameState == DNTG_GameState.On_SelectTable)
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

	public void updateTableList(DNTG_FishDesk[] netDesks)
	{
		List<DNTG_TableInfo> list = new List<DNTG_TableInfo>();
		DNTG_TableInfo dNTG_TableInfo = null;
		if ((bool)mUIScene)
		{
			mUIScene.InItTable(netDesks);
		}
		if (netDesks != null)
		{
			for (int i = 0; i < netDesks.Length; i++)
			{
				dNTG_TableInfo = new DNTG_TableInfo(netDesks[i]);
				list.Add(dNTG_TableInfo);
			}
			mUser.RoomId = dNTG_TableInfo.RoomId - 1;
			if (mUser.RoomId < 0)
			{
				mUser.RoomId = 0;
			}
			TableList = list;
			if (mGameState == DNTG_GameState.On_SelectRoom)
			{
				mUIScene.EnterRoom();
				getInstance().currentState = DNTG_GameState.On_SelectTable;
				mUIScene.UpdateUserInfo();
			}
		}
	}

	public void updateCurUsers(DNTG_Seat[] netSeats)
	{
		List<DNTG_UserInfo> list = new List<DNTG_UserInfo>();
		for (int i = 0; i < netSeats.Length; i++)
		{
			DNTG_UserInfo item = DNTG_UserInfo.createOtherUser(netSeats[i]);
			list.Add(item);
		}
		list.Sort(DNTG_UserInfo.compareBySeat);
		UserList = list;
	}

	public void updateOtherUsers(DNTG_Seat[] netSeats, int deskId = -1)
	{
		if (TableList == null)
		{
			return;
		}
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
		List<DNTG_UserInfo> list = new List<DNTG_UserInfo>();
		for (int j = 0; j < netSeats.Length; j++)
		{
			DNTG_UserInfo item = DNTG_UserInfo.createOtherUser(netSeats[j]);
			list.Add(item);
		}
		list.Sort(DNTG_UserInfo.compareBySeat);
		if (getInstance().currentState == DNTG_GameState.On_SelectTable && !ZH2_GVars.isStartGame)
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
			if (!list[k].IsExist && getInstance().currentState == DNTG_GameState.On_Game)
			{
				GameScene.areas[list[k].SeatIndex - 1].sptLock.sptLockCard.HideLockCard();
			}
		}
		UserList = list;
	}

	public void updateTableUserNumber(int tableId, int number)
	{
		int i = 0;
		if (mTableList == null)
		{
			return;
		}
		for (; i < mTableList.Count && mTableList[i].Id != tableId; i++)
		{
		}
		if (i == mTableList.Count)
		{
			return;
		}
		mTableList[i].PersonCount = number;
		if (mGameState != DNTG_GameState.On_Game)
		{
			if (mTableIndex == i)
			{
				mUIScene.UpdateTableConfig();
			}
			mUIScene.UpdateTableListItem(i);
		}
	}

	public void getPersonInfo(DNTG_UserInfo user, int honor)
	{
		if (mGameState == DNTG_GameState.On_Game)
		{
			mGameScene.ShowUserInfo(user, honor);
		}
	}
}
