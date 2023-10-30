using STDT_GameConfig;
using UnityEngine;

public class XLDT_GameManager : MonoBehaviour
{
	private bool m_bHasLogin;

	[HideInInspector]
	public int mRoomId;

	public TextAsset asset;

	public XLDT_GameInfo _mGameInfo;

	public static XLDT_GameManager _mGameManager;

	public static XLDT_GameManager getInstance()
	{
		return _mGameManager;
	}

	private void Awake()
	{
		if (_mGameManager == null)
		{
			_mGameManager = this;
		}
		_mGameInfo = XLDT_GameInfo.getInstance();
		_mGameInfo.currentState = XLDT_GameState.On_Loading;
		_mGameInfo.NetShouldBlocked = false;
		XLDT_GameInfo.getInstance().Language = 0;
		if (XLDT_GameInfo.getInstance().Language == 0)
		{
			setLanguage(isCN: true);
		}
		else
		{
			setLanguage(isCN: false);
		}
	}

	public void setLanguage(bool isCN)
	{
		XLDT_Localization.LoadTxt(asset);
		if (isCN)
		{
			XLDT_Localization.language = 0;
		}
		else
		{
			XLDT_Localization.language = 1;
		}
		if (!XLDT_DanTiaoCommon.G_TEST)
		{
		}
	}

	public void SendLogin()
	{
		UnityEngine.Debug.Log("sss=" + XLDT_NetMain.GetSingleton().MyCreateSocket.GetSocketStartFlag());
		if (XLDT_NetMain.GetSingleton().MyCreateSocket.GetSocketStartFlag())
		{
			UnityEngine.Debug.Log("GetSocketStartFlag=" + XLDT_NetMain.GetSingleton().MyCreateSocket.GetSocketStartFlag());
			bool flag = XLDT_NetMain.GetSingleton().MyCreateSocket.SendUserLogin("xjtest", "test01", 1, string.Empty);
			UnityEngine.Debug.Log("sss=" + flag);
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && (_mGameInfo.currentState == XLDT_GameState.On_SelectRoom || _mGameInfo.currentState == XLDT_GameState.On_SelectSeat) && !XLDT_ShowUI.getInstance().m_bTableMove)
		{
			XLDT_GameInfo.getInstance().CoinList.Clear();
			if (_mGameInfo.currentState == XLDT_GameState.On_SelectRoom)
			{
				XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.IsExitApplication, 1);
			}
			else if (_mGameInfo.currentState == XLDT_GameState.On_SelectSeat)
			{
				XLDT_GameInfo.getInstance().currentState = XLDT_GameState.On_SelectRoom;
				XLDT_NetMain.GetSingleton().MyCreateSocket.SendLeaveRoom(mRoomId);
				_mGameInfo.User.RoomId = -1;
			}
		}
	}

	public void RecLoginMessage(bool succ, int type, XLDT_User user = null)
	{
		if (succ && !m_bHasLogin)
		{
			m_bHasLogin = true;
			if (user != null)
			{
				_mGameInfo.User = new XLDT_UserInfo(user);
			}
		}
		else if (!succ)
		{
			MonoBehaviour.print("!succ" + type);
			switch (type)
			{
			case 0:
				XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.UserIdPwdNotMatch);
				break;
			case 1:
				XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.Game_UserIdFrozen);
				break;
			case 2:
				XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.ServerUpdate);
				break;
			}
		}
		if (m_bHasLogin && succ && _mGameInfo.User.IsOverFlow == 1 && XLDT_GameInfo.getInstance().currentState != 0)
		{
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.CoinOverFlow);
		}
	}

	public void RecTableList(XLDT_CardDesk[] tablbe)
	{
		if (tablbe != null)
		{
			_mGameInfo.TableList.Clear();
			for (int i = 0; i < tablbe.Length; i++)
			{
				CreateATable(tablbe[i]);
			}
			XLDT_ShowUI.getInstance().EnterRoom(mRoomId);
			XLDT_ShowUI.getInstance().CreateTableAList(_mGameInfo.TableList.Count, 0);
		}
		else
		{
			_mGameInfo.TableList.Clear();
			_mGameInfo.CurTabIndex = -1;
			XLDT_ShowUI.getInstance().UnOpen(mRoomId);
		}
		_mGameInfo.User.RoomId = mRoomId;
	}

	protected void SetCurrentTabIndex(XLDT_CardDesk[] tablbe)
	{
		int num = 0;
		while (true)
		{
			if (num >= tablbe.Length)
			{
				return;
			}
			if (_mGameInfo.CurTable == null)
			{
				_mGameInfo.CurTabIndex = 0;
				if (_mGameInfo.currentState == XLDT_GameState.On_SelectSeat)
				{
					XLDT_ShowUI.getInstance().SetTableInfoVisible();
				}
				return;
			}
			if (_mGameInfo.CurTable.Id == tablbe[num].id)
			{
				break;
			}
			if (num == tablbe.Length - 1)
			{
				_mGameInfo.CurTabIndex = 0;
			}
			num++;
		}
		_mGameInfo.CurTabIndex = num;
	}

	public void RecUpdateRoomInfo(XLDT_CardDesk[] tablbe)
	{
		if (_mGameInfo.currentState == XLDT_GameState.On_Game || _mGameInfo.currentState == XLDT_GameState.On_EnterGame)
		{
			if (tablbe != null)
			{
				_mGameInfo.TableList.Clear();
				for (int i = 0; i < tablbe.Length; i++)
				{
					CreateATable(tablbe[i]);
				}
				SetCurrentTabIndex(tablbe);
				if (_mGameInfo.CurTabIndex >= _mGameInfo.TableList.Count)
				{
					_mGameInfo.CurTabIndex = 0;
				}
				_mGameInfo.TotalTabNum = _mGameInfo.TableList.Count;
			}
			else
			{
				_mGameInfo.TableList.Clear();
				_mGameInfo.CurTabIndex = -1;
				_mGameInfo.TotalTabNum = 0;
			}
		}
		else
		{
			if (_mGameInfo.currentState != XLDT_GameState.On_SelectSeat)
			{
				return;
			}
			if (tablbe != null)
			{
				_mGameInfo.TableList.Clear();
				for (int j = 0; j < tablbe.Length; j++)
				{
					CreateATable(tablbe[j]);
				}
				SetCurrentTabIndex(tablbe);
				if (_mGameInfo.CurTabIndex >= _mGameInfo.TableList.Count)
				{
					XLDT_ShowUI.getInstance().CreateTableAList(_mGameInfo.TableList.Count, 0);
				}
				else
				{
					XLDT_ShowUI.getInstance().CreateTableAList(_mGameInfo.TableList.Count, _mGameInfo.CurTabIndex);
				}
			}
			else
			{
				_mGameInfo.TableList.Clear();
				_mGameInfo.CurTabIndex = -1;
				XLDT_ShowUI.getInstance().UnOpen(mRoomId);
			}
		}
	}

	protected void CreateATable(XLDT_CardDesk tablbe)
	{
		XLDT_TableInfo item = new XLDT_TableInfo(tablbe);
		_mGameInfo.TableList.Add(item);
	}

	public void RecUpdatSeatInfo(int id, XLDT_Seat[] seat)
	{
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_SelectRoom)
		{
			return;
		}
		if (id == -1)
		{
			if (_mGameInfo.currentState == XLDT_GameState.On_SelectRoom || _mGameInfo.currentState == XLDT_GameState.On_SelectSeat)
			{
				XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.TableDeleted);
			}
			return;
		}
		for (int i = 0; i < _mGameInfo.TotalTabNum; i++)
		{
			if (_mGameInfo.TableList[i].Id != id)
			{
				continue;
			}
			_mGameInfo.TableList[i].PersonCount = 0;
			for (int j = 0; j < seat.Length; j++)
			{
				_mGameInfo.TableList[i].mSeat[j] = seat[j];
				if (!_mGameInfo.TableList[i].mSeat[j].isFree)
				{
					_mGameInfo.TableList[i].PersonCount++;
				}
			}
		}
	}

	public void RecExpCoin(int coin)
	{
		_mGameInfo.User.TestCoinCount = coin;
		if (_mGameInfo.currentState == XLDT_GameState.On_SelectSeat || _mGameInfo.currentState == XLDT_GameState.On_SelectRoom)
		{
			XLDT_ShowUI.getInstance().UpdateExpCoin();
		}
		else if (_mGameInfo.currentState != XLDT_GameState.On_Game)
		{
		}
	}

	public void RecEnterGame(bool canseat, int seatid, int type = 0)
	{
		if (canseat)
		{
			if (seatid == XLDT_GameInfo.getInstance().User.SeatIndex)
			{
				XLDT_GameInfo.getInstance().CoinList.Clear();
				XLDT_GameUIMngr.GetSingleton().EnterGame();
			}
			else if (seatid > 0 && seatid < 9)
			{
				XLDT_GameInfo.getInstance().User.SeatIndex = seatid;
				XLDT_GameInfo.getInstance().CoinList.Clear();
				XLDT_GameUIMngr.GetSingleton().EnterGame();
			}
			return;
		}
		switch (type)
		{
		case 0:
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.TableDeleted);
			break;
		case 1:
			if (mRoomId == 1)
			{
				XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.SelectTable_CreditBelowRistrict);
			}
			else
			{
				XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.SelectTable_SendExpCoin);
			}
			break;
		case 2:
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.SelectSeat_NotEmpty);
			break;
		}
		_mGameInfo.User.SeatIndex = -1;
	}

	public void RecUpdateLevel(int level)
	{
		_mGameInfo.User.Level = level;
		if (_mGameInfo.currentState != XLDT_GameState.On_SelectRoom && _mGameInfo.currentState != XLDT_GameState.On_SelectSeat)
		{
		}
	}

	public void RecUpdateExpCoin(bool ok)
	{
		if (ok && (_mGameInfo.currentState == XLDT_GameState.On_SelectRoom || _mGameInfo.currentState == XLDT_GameState.On_SelectSeat))
		{
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.ApplyForExpCoin_Success);
		}
	}

	public void RecUpdateCoin(int coin)
	{
		_mGameInfo.User.CoinCount = coin;
		if (XLDT_ShowUI.getInstance() != null)
		{
			XLDT_ShowUI.getInstance().UpdateGameCoin();
		}
	}

	public void RecOverflow()
	{
		_mGameInfo.User.IsOverFlow = 1;
		if (_mGameInfo.currentState == XLDT_GameState.On_SelectRoom || _mGameInfo.currentState == XLDT_GameState.On_SelectSeat)
		{
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.CoinOverFlow);
		}
	}

	public void RecQuitToTableList(int type)
	{
		if (_mGameInfo.currentState == XLDT_GameState.On_SelectSeat)
		{
			switch (type)
			{
			case 1:
				XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.TableDeleted);
				break;
			case 2:
				XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.TableConfigChanged);
				break;
			default:
				XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.LongTimeNoHandle);
				break;
			}
		}
	}

	public void RecQuitToHall(int type)
	{
		if (_mGameInfo.currentState >= XLDT_GameState.On_EnterGame)
		{
			return;
		}
		switch (type)
		{
		case 1:
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.ServerUpdate);
			break;
		case 2:
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.Game_UserIdFrozen);
			break;
		case 3:
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.UserIdDeleted);
			break;
		case 4:
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.UserIdRepeative);
			break;
		case 5:
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.UserPwdChanged);
			break;
		case 6:
			if (false)
			{
				bool flag = true;
				XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.LoseTheServer);
			}
			break;
		}
	}

	public void RecOtherPlayerInfo(int seatid, XLDT_User user, int[] honor)
	{
	}

	public void RecGivingCoin(int coin)
	{
		if (_mGameInfo.currentState == XLDT_GameState.On_SelectRoom || _mGameInfo.currentState == XLDT_GameState.On_SelectSeat)
		{
			XLDT_GameInfo.getInstance().CoinList.Add(coin);
			XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.GivingCoin);
			XLDT_TipManager.getInstance().txtTip.text = ((XLDT_GameInfo.getInstance().Language == 0) ? ("恭喜你获得客服赠送的 " + coin + " 游戏币") : ("Congratulations.You'v got " + coin + " complimentary coins"));
		}
	}

	public void RecNetDown()
	{
		if (_mGameInfo.currentState < XLDT_GameState.On_EnterGame)
		{
			if (XLDT_TipManager.getInstance() != null)
			{
				XLDT_TipManager.getInstance().ShowTip(XLDT_TipType.Net_ConnectionError);
			}
			else
			{
				UnityEngine.Debug.LogError("XLDT_TipManager为空");
			}
		}
	}

	public void RecNoticeInfo(string str)
	{
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_ShowScene.getInstance().ShowNotice(str);
		}
	}

	public void RecStartPrintChit(int type)
	{
		if (type >= 0 && type <= 4 && _mGameInfo.currentState >= XLDT_GameState.On_EnterGame)
		{
			XLDT_ShowScene.getInstance().StartPrintChit(type);
		}
	}

	public void RecGameRestart(int turns, int nGameId, int nClearCard, int color, int num, int[] bounus, int time, int girltype)
	{
		XLDT_GameInfo.getInstance().GamesId = nGameId;
		if (XLDT_GameInfo.getInstance().currentState < XLDT_GameState.On_EnterGame || turns < 0 || nGameId < 0 || time < 0 || girltype <= 0)
		{
			return;
		}
		_mGameInfo.CurAward.awardType = XLDT_EAwardType.Normal;
		if (nClearCard == 1 || nClearCard == 2)
		{
			if (num != 14 && num != 15)
			{
				_mGameInfo.CurAward.color = (XLDT_ECardsColour)color;
			}
			_mGameInfo.CurAward.num = num;
		}
		XLDT_ShowScene.getInstance().GameRestart(turns, nGameId, bounus, time, girltype);
	}

	public void RecRecord(XLDT_CardAlgorithmResult[] record)
	{
		if (XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_EnterGame || XLDT_GameInfo.getInstance().currentState == XLDT_GameState.On_Game)
		{
			XLDT_ShowScene.getInstance().ShowRecordObj(record);
		}
	}

	public void RecCurrentAward(XLDT_CardAlgorithmResult award)
	{
		if (award.awardType < 0 || award.awardType > 3 || award.color < 0 || award.color > 3 || award.point < 0 || award.point > 15)
		{
		}
		_mGameInfo.CurAward.awardType = (XLDT_EAwardType)award.awardType;
		_mGameInfo.CurAward.color = (XLDT_ECardsColour)award.color;
		_mGameInfo.CurAward.num = award.point;
		_mGameInfo.CurAward.bonus = award.jackpot;
		XLDT_ShowScene.getInstance().StartPlayFanPai(_mGameInfo.CurAward);
	}

	public void Write(string str)
	{
	}

	private void OnApplicationQuit()
	{
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendQuitGame();
	}
}
