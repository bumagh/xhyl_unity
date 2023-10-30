using GameConfig;
using System;
using System.Collections.Generic;
using UnityEngine;

public class STQM_GameInfo
{
	private STQM_LoadType mLoadStep;

	private int mLanguage = -1;

	private string mIp = string.Empty;

	private STQM_GameState mGameState;

	private STQM_UserInfo mUser;

	private STQM_SettedInfo mSetted;

	private STQM_TableInfo mTable;

	private List<STQM_TableInfo> mTableList;

	private int mTableIndex;

	private string mKey = string.Empty;

	private bool mIsGameShutUp;

	private bool mIsUserShutUp;

	private string mUserId;

	private string mPwd;

	private bool mIsSpecial;

	private bool mNetShouldBlocked;

	private bool bCountTime;

	private List<STQM_UserInfo> mUserList;

	private STQM_LoadScene mLoadScene;

	private STQM_UIScene mUIScene;

	private STQM_GameScene mGameScene;

	private static STQM_GameInfo mGameInfo;

	private string mStatusURL;

	public STQM_LoadType LoadStep
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

	public STQM_LoadScene LoadScene
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

	public STQM_UIScene UIScene
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

	public STQM_GameScene GameScene
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
				mLoadScene.Step = STQM_LoadType.On_Login;
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

	public STQM_GameState currentState
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

	public STQM_SettedInfo Setted
	{
		get
		{
			return mSetted;
		}
		set
		{
			mSetted = value;
			mLoadScene.Step = STQM_LoadType.On_PrepareLoadScene;
		}
	}

	public STQM_UserInfo User
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
				mLoadScene.Step = STQM_LoadType.On_LoadUserSetting;
				mNetShouldBlocked = true;
			}
			mUser = value;
			if (mGameState == STQM_GameState.On_Game)
			{
				mGameScene.UpdateUserInfo();
			}
			else if (mGameState != 0)
			{
				mUIScene.UpdateUserInfo();
			}
		}
	}

	public STQM_TableInfo Table
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

	public List<STQM_UserInfo> UserList
	{
		get
		{
			return mUserList;
		}
		set
		{
			mUserList = value;
			if (mGameState == STQM_GameState.On_Game)
			{
				mGameScene.UpdateTableUser();
			}
			else
			{
				mUIScene.UpdateTableUsers();
			}
		}
	}

	public List<STQM_TableInfo> TableList
	{
		get
		{
			return mTableList;
		}
		set
		{
			mTableList = value;
			selectTable();
			if (mGameState == STQM_GameState.On_Game)
			{
				mGameScene.UpdateTableConfig();
				return;
			}
			mUIScene.UpdateTableList();
			mUIScene.UpdateTableConfig();
			mUIScene.UpdateUserInfo();
		}
	}

	public static STQM_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new STQM_GameInfo();
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

	public void createUser(STQM_User netUser)
	{
		STQM_UserInfo sTQM_UserInfo2 = User = new STQM_UserInfo(netUser);
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
		if (mGameState == STQM_GameState.On_Game)
		{
			mGameScene.UpdateUserInfo();
		}
		else if (mGameState == STQM_GameState.On_SelectRoom || mGameState == STQM_GameState.On_SelectTable)
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

	public void updateTableList(STQM_FishDesk[] netDesks)
	{
		List<STQM_TableInfo> list = new List<STQM_TableInfo>();
		STQM_TableInfo sTQM_TableInfo = null;
		mUIScene.InItTable(netDesks);
		if (netDesks != null)
		{
			for (int i = 0; i < netDesks.Length; i++)
			{
				sTQM_TableInfo = new STQM_TableInfo(netDesks[i]);
				list.Add(sTQM_TableInfo);
			}
			mUser.RoomId = sTQM_TableInfo.RoomId - 1;
			TableList = list;
			if (mGameState == STQM_GameState.On_SelectRoom)
			{
				mUIScene.EnterRoom();
				getInstance().currentState = STQM_GameState.On_SelectTable;
				mUIScene.UpdateUserInfo();
			}
		}
	}

	public void updateCurUsers(STQM_Seat[] netSeats)
	{
		List<STQM_UserInfo> list = new List<STQM_UserInfo>();
		for (int i = 0; i < netSeats.Length; i++)
		{
			STQM_UserInfo item = STQM_UserInfo.createOtherUser(netSeats[i]);
			list.Add(item);
		}
		list.Sort(STQM_UserInfo.compareBySeat);
		UserList = list;
	}

	public void updateOtherUsers(STQM_Seat[] netSeats, int deskId = -1)
	{
		if (deskId != -1 && TableList != null)
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
		List<STQM_UserInfo> list = new List<STQM_UserInfo>();
		for (int j = 0; j < netSeats.Length; j++)
		{
			STQM_UserInfo item = STQM_UserInfo.createOtherUser(netSeats[j]);
			list.Add(item);
		}
		list.Sort(STQM_UserInfo.compareBySeat);
		if (getInstance().currentState == STQM_GameState.On_SelectTable && !ZH2_GVars.isStartGame)
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
			if (!list[k].IsExist && getInstance().currentState == STQM_GameState.On_Game)
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
		if (mGameState != STQM_GameState.On_Game)
		{
			if (mTableIndex == i)
			{
				mUIScene.UpdateTableConfig();
			}
			mUIScene.UpdateTableListItem(i);
		}
	}

	public void getPersonInfo(STQM_UserInfo user, int honor)
	{
		if (mGameState == STQM_GameState.On_Game)
		{
			mGameScene.ShowUserInfo(user, honor);
		}
		else
		{
			mUIScene.ShowPersonInfo(user, honor);
		}
	}
}
