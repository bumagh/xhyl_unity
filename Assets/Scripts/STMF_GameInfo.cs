using GameConfig;
using System;
using System.Collections.Generic;
using UnityEngine;

public class STMF_GameInfo
{
	private STMF_LoadType mLoadStep;

	private int mLanguage = -1;

	private string mIp = string.Empty;

	private STMF_GameState mGameState;

	private STMF_UserInfo mUser;

	private STMF_SettedInfo mSetted;

	private STMF_TableInfo mTable;

	private List<STMF_TableInfo> mTableList;

	private int mTableIndex;

	private string mKey = string.Empty;

	private bool mIsGameShutUp;

	private bool mIsUserShutUp;

	private string mUserId;

	private string mPwd;

	private bool mIsSpecial;

	private bool mNetShouldBlocked;

	private List<STMF_UserInfo> mUserList;

	private STMF_LoadScene mLoadScene;

	private STMF_UIScene mUIScene;

	private STMF_GameScene mGameScene;

	private static STMF_GameInfo mGameInfo;

	private string mStatusURL;

	public STMF_LoadType LoadStep
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

	public STMF_LoadScene LoadScene
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

	public STMF_UIScene UIScene
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

	public STMF_GameScene GameScene
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
				mLoadScene.Step = STMF_LoadType.On_Login;
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

	public STMF_GameState currentState
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

	public STMF_SettedInfo Setted
	{
		get
		{
			return mSetted;
		}
		set
		{
			mSetted = value;
			mLoadScene.Step = STMF_LoadType.On_PrepareLoadScene;
		}
	}

	public STMF_UserInfo User
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
				mLoadScene.Step = STMF_LoadType.On_LoadUserSetting;
				mNetShouldBlocked = true;
			}
			mUser = value;
			if (mGameState == STMF_GameState.On_Game)
			{
				mGameScene.UpdateUserInfo();
			}
			else if (mGameState != 0)
			{
				mUIScene.UpdateUserInfo();
			}
		}
	}

	public STMF_TableInfo Table
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

	public List<STMF_UserInfo> UserList
	{
		get
		{
			return mUserList;
		}
		set
		{
			mUserList = value;
			if (mGameState == STMF_GameState.On_Game)
			{
				mGameScene.UpdateTableUser();
			}
			else
			{
				mUIScene.UpdateTableUsers();
			}
		}
	}

	public List<STMF_TableInfo> TableList
	{
		get
		{
			return mTableList;
		}
		set
		{
			mTableList = value;
			selectTable();
			if (mGameState == STMF_GameState.On_Game)
			{
				mGameScene.UpdateTableConfig();
				return;
			}
			mUIScene.SetIndicator();
			mUIScene.UpdateTableList();
			mUIScene.UpdateTableConfig();
			mUIScene.UpdateUserInfo();
		}
	}

	public static STMF_GameInfo getInstance()
	{
		if (mGameInfo == null)
		{
			mGameInfo = new STMF_GameInfo();
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

	public void createUser(STMF_User netUser)
	{
		STMF_UserInfo sTMF_UserInfo2 = User = new STMF_UserInfo(netUser);
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
		if (mGameState == STMF_GameState.On_Game)
		{
			mGameScene.UpdateUserInfo();
		}
		else if (mGameState == STMF_GameState.On_SelectRoom || mGameState == STMF_GameState.On_SelectSeat || mGameState == STMF_GameState.On_SelectTable)
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

	public void updateTableList(STMF_FishDesk[] netDesks)
	{
		List<STMF_TableInfo> list = new List<STMF_TableInfo>();
		STMF_TableInfo sTMF_TableInfo = null;
		if ((bool)mUIScene)
		{
			mUIScene.InItTable(netDesks);
		}
		if (netDesks != null)
		{
			for (int i = 0; i < netDesks.Length; i++)
			{
				sTMF_TableInfo = new STMF_TableInfo(netDesks[i]);
				list.Add(sTMF_TableInfo);
			}
			mUser.RoomId = sTMF_TableInfo.RoomId - 1;
			if (mUser.RoomId < 0)
			{
				mUser.RoomId = 0;
			}
			TableList = list;
			if (mGameState == STMF_GameState.On_SelectRoom)
			{
				mUIScene.EnterRoom();
				getInstance().currentState = STMF_GameState.On_SelectTable;
				mUIScene.UpdateUserInfo();
			}
		}
	}

	public void updateCurUsers(STMF_Seat[] netSeats)
	{
		List<STMF_UserInfo> list = new List<STMF_UserInfo>();
		for (int i = 0; i < netSeats.Length; i++)
		{
			STMF_UserInfo item = STMF_UserInfo.createOtherUser(netSeats[i]);
			list.Add(item);
		}
		list.Sort(STMF_UserInfo.compareBySeat);
		UserList = list;
	}

	public void updateOtherUsers(STMF_Seat[] netSeats, bool isEnter)
	{
		if (isEnter)
		{
			mUIScene.InSeat(netSeats);
		}
		List<STMF_UserInfo> list = new List<STMF_UserInfo>();
		for (int i = 0; i < netSeats.Length; i++)
		{
			STMF_UserInfo item = STMF_UserInfo.createOtherUser(netSeats[i]);
			list.Add(item);
		}
		list.Sort(STMF_UserInfo.compareBySeat);
		UserList = list;
	}

	public void updateOtherUsers(STMF_Seat[] netSeats, int deskId = -1)
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
		List<STMF_UserInfo> list = new List<STMF_UserInfo>();
		for (int j = 0; j < netSeats.Length; j++)
		{
			STMF_UserInfo item = STMF_UserInfo.createOtherUser(netSeats[j]);
			list.Add(item);
		}
		list.Sort(STMF_UserInfo.compareBySeat);
		if (getInstance().currentState == STMF_GameState.On_SelectTable && !ZH2_GVars.isStartGame)
		{
			getInstance().User.SeatIndex = -1;
		}
		if (Table.Id == deskId && deskId != -1)
		{
			UserList = list;
		}
	}

	public void updateTableUserNumber(int tableId, int number)
	{
		if (mTableList == null)
		{
			return;
		}
		int i;
		for (i = 0; i < mTableList.Count && mTableList[i].Id != tableId; i++)
		{
		}
		if (i == mTableList.Count)
		{
			return;
		}
		mTableList[i].PersonCount = number;
		if (mGameState != STMF_GameState.On_Game)
		{
			if (mTableIndex == i)
			{
				mUIScene.UpdateTableConfig();
			}
			mUIScene.UpdateTableListItem(i);
		}
	}

	public void UpdateTableUserNumber(STMF_Seat[] netSeats, int tableId, int number)
	{
		try
		{
			if ((bool)mUIScene)
			{
				mUIScene.UpdateSeat(netSeats, tableId);
			}
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		if (mTableList == null)
		{
			return;
		}
		int i;
		for (i = 0; i < mTableList.Count && mTableList[i].Id != tableId; i++)
		{
		}
		if (i == mTableList.Count)
		{
			return;
		}
		mTableList[i].PersonCount = number;
		if (mGameState != STMF_GameState.On_Game)
		{
			if (mTableIndex == i)
			{
				mUIScene.UpdateTableConfig();
			}
			mUIScene.UpdateTableListItem(i);
		}
	}

	public void getPersonInfo(STMF_UserInfo user, int honor)
	{
		if (mGameState == STMF_GameState.On_Game)
		{
			mGameScene.ShowUserInfo(user, honor);
		}
		else
		{
			mUIScene.ShowPersonInfo(user, honor);
		}
	}
}
