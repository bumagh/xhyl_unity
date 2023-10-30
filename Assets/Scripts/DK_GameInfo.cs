using GameConfig;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DK_GameInfo
{
	private DK_LoadType mLoadStep;

	private int mLanguage = -1;

	private string mIp = string.Empty;

	private DK_GameState mGameState;

	private DK_UserInfo mUser;

	private DK_SettedInfo mSetted;

	private DK_TableInfo mTable;

	private List<DK_TableInfo> mTableList;

	private int mTableIndex;

	private string mKey = string.Empty;

	private bool mIsGameShutUp;

	private bool mIsUserShutUp;

	private string mUserId;

	private string mPwd;

	private bool mIsSpecial;

	private bool mNetShouldBlocked;

	private bool bCountTime;

	private List<DK_UserInfo> mUserList;

	private DK_LoadScene mLoadScene;

	private DK_UIScene mUIScene;

	private DK_GameScene mGameScene;

	private static DK_GameInfo mGameInfo;

	private string mStatusURL;

	public DK_LoadType LoadStep
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

	public DK_LoadScene LoadScene
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

	public DK_UIScene UIScene
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

	public DK_GameScene GameScene
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
				mLoadScene.Step = DK_LoadType.On_Login;
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

	public DK_GameState currentState
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

	public DK_SettedInfo Setted
	{
		get
		{
			return mSetted;
		}
		set
		{
			mSetted = value;
			mLoadScene.Step = DK_LoadType.On_PrepareLoadScene;
		}
	}

	public DK_UserInfo User
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
				UnityEngine.Debug.LogError("=======User座位=====" + value.SeatIndex);
				value.GunValue = mUser.GunValue;
			}
			else
			{
				mLoadScene.Step = DK_LoadType.On_LoadUserSetting;
				mNetShouldBlocked = true;
			}
			mUser = value;
			if (mGameState == DK_GameState.On_Game)
			{
				mGameScene.UpdateUserInfo();
			}
			else if (mGameState != 0)
			{
				mUIScene.UpdateUserInfo();
			}
		}
	}

	public DK_TableInfo Table
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

	public List<DK_UserInfo> UserList
	{
		get
		{
			return mUserList;
		}
		set
		{
			mUserList = value;
			if (mGameState == DK_GameState.On_Game)
			{
				mGameScene.UpdateTableUser();
			}
			else
			{
				mUIScene.UpdateTableUsers();
			}
		}
	}

	public List<DK_TableInfo> TableList
	{
		get
		{
			return mTableList;
		}
		set
		{
			mTableList = value;
			selectTable();
			if (mGameState == DK_GameState.On_Game)
			{
				mGameScene.UpdateTableConfig();
				return;
			}
			UnityEngine.Debug.Log("SetTableList");
			mUIScene.UpdateTableList();
			mUIScene.UpdateTableConfig();
			mUIScene.UpdateUserInfo();
		}
	}

	public static DK_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new DK_GameInfo();
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

	public void createUser(DK_User netUser)
	{
		DK_UserInfo dK_UserInfo2 = User = new DK_UserInfo(netUser);
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
		if (mGameState == DK_GameState.On_Game)
		{
			mGameScene.UpdateUserInfo();
		}
		else if (mGameState == DK_GameState.On_SelectRoom || mGameState == DK_GameState.On_SelectTable)
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

	public void updateTableList(DK_FishDesk[] netDesks)
	{
		List<DK_TableInfo> list = new List<DK_TableInfo>();
		DK_TableInfo dK_TableInfo = null;
		mUIScene.InItTable(netDesks);
		if (netDesks != null)
		{
			for (int i = 0; i < netDesks.Length; i++)
			{
				dK_TableInfo = new DK_TableInfo(netDesks[i]);
				list.Add(dK_TableInfo);
			}
			mUser.RoomId = dK_TableInfo.RoomId - 1;
			if (mUser.RoomId < 0)
			{
				mUser.RoomId = 0;
			}
			TableList = list;
			if (mGameState == DK_GameState.On_SelectRoom)
			{
				mUIScene.EnterRoom();
				getInstance().currentState = DK_GameState.On_SelectTable;
				mUIScene.UpdateUserInfo();
			}
		}
	}

	public void updateCurUsers(DK_Seat[] netSeats)
	{
		List<DK_UserInfo> list = new List<DK_UserInfo>();
		for (int i = 0; i < netSeats.Length; i++)
		{
			DK_UserInfo item = DK_UserInfo.createOtherUser(netSeats[i]);
			list.Add(item);
		}
		list.Sort(DK_UserInfo.compareBySeat);
		UserList = list;
	}

	public void updateOtherUsers(DK_Seat[] netSeats, int deskId = -1)
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
		List<DK_UserInfo> list = new List<DK_UserInfo>();
		for (int j = 0; j < netSeats.Length; j++)
		{
			DK_UserInfo item = DK_UserInfo.createOtherUser(netSeats[j]);
			list.Add(item);
		}
		list.Sort(DK_UserInfo.compareBySeat);
		if (getInstance().currentState == DK_GameState.On_SelectTable && !ZH2_GVars.isStartGame)
		{
			UnityEngine.Debug.LogError("===========重置座位 -1==========");
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
			if (!list[k].IsExist && getInstance().currentState == DK_GameState.On_Game)
			{
				GameScene.areas[list[k].SeatIndex - 1].sptLock.sptLockCard.HideLockCard();
			}
		}
		UserList = list;
	}

	public void updateTableUserNumber(int tableId, int number)
	{
		int num = 0;
		while (mTableList != null && num < mTableList.Count && mTableList[num] != null && mTableList[num].Id != tableId)
		{
			num++;
		}
		if (num == mTableList.Count)
		{
			return;
		}
		mTableList[num].PersonCount = number;
		if (mGameState != DK_GameState.On_Game)
		{
			if (mTableIndex == num)
			{
				mUIScene.UpdateTableConfig();
			}
			mUIScene.UpdateTableListItem(num);
		}
	}

	public void getPersonInfo(DK_UserInfo user, int honor)
	{
		if (mGameState == DK_GameState.On_Game)
		{
			mGameScene.ShowUserInfo(user, honor);
		}
		else
		{
			mUIScene.ShowPersonInfo(user, honor);
		}
	}
}
