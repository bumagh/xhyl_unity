using com.miracle9.game.entity;
using JSYS_LL_GameCommon;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JSYS_LL_TableList : MonoBehaviour
{
	public static JSYS_LL_TableList _LL_TableList;

	public Text userName;

	public Text coinCount;

	public Text testCoinCount;

	protected JSYS_LL_PersonInfo mUserInfo;

	protected ArrayList mTableInfoList = new ArrayList();

	protected int mCurrentRoomId = -1;

	protected int mCurrentTableId;

	protected float mAnimationTime = -1f;

	protected int mAnimationStep = -1;

	protected float mStepTime;

	protected bool mIsDraging;

	private JSYS_LL_GameInfo mGameInfo;

	public static GoldSharkDesk[] deskListstatic;

	private void Awake()
	{
		_LL_TableList = this;
		UIEventListener.VectorDelegate vectorDelegate = _dragTable;
		UIEventListener.BoolDelegate boolDelegate = _releaseFromBg;
		mUserInfo = JSYS_LL_GameInfo.getInstance().UserInfo;
		mGameInfo = JSYS_LL_GameInfo.getInstance();
	}

	public void updateUserInfo()
	{
		userName.text = mGameInfo.UserInfo.strName;
		coinCount.text = (mGameInfo.IsSpecial ? (mGameInfo.UserInfo.CoinCount % 10000).ToString() : mGameInfo.UserInfo.CoinCount.ToString());
		testCoinCount.text = (mGameInfo.IsSpecial ? (mGameInfo.UserInfo.ExpCoinCount % 10000).ToString() : mGameInfo.UserInfo.ExpCoinCount.ToString());
	}

	private void Update()
	{
		_doSelectingTable();
	}

	public void InitTableList(GoldSharkDesk[] deskList, int iRoomId = 1)
	{
		int num = 0;
		for (int i = 0; i < deskList.Length; i++)
		{
			if (num < 1 && deskList[i].onlineNumber < 8)
			{
				_clickTable(deskList[i].id);
				num++;
				break;
			}
		}
		deskListstatic = deskList;
		mCurrentRoomId = iRoomId;
		mUserInfo.RoomId = iRoomId - 1;
		mTableInfoList.Clear();
		mCurrentTableId = 0;
		for (int j = 0; j < deskList.Length; j++)
		{
			JSYS_LL_TableInfo jSYS_LL_TableInfo = new JSYS_LL_TableInfo();
			jSYS_LL_TableInfo.RoomId = deskList[j].roomId;
			jSYS_LL_TableInfo.TableServerID = deskList[j].id;
			if (JSYS_LL_AppUIMngr.GetSingleton().GetAppState == AppState.App_On_Game || JSYS_LL_AppUIMngr.GetSingleton().GetAppState == AppState.App_On_Table)
			{
				mCurrentTableId = j;
			}
			jSYS_LL_TableInfo.TableName = deskList[j].name;
			jSYS_LL_TableInfo.MinBet = deskList[j].minBet;
			jSYS_LL_TableInfo.MaxBet = deskList[j].maxBet;
			jSYS_LL_TableInfo.Ristrict = deskList[j].minGold;
			jSYS_LL_TableInfo.CreditPerCoin = deskList[j].exchange;
			jSYS_LL_TableInfo.MinZHXBet = deskList[j].min_zxh;
			jSYS_LL_TableInfo.MaxCD = deskList[j].betTime;
			jSYS_LL_TableInfo.MaxZXBet = deskList[j].max_zx;
			jSYS_LL_TableInfo.MaxHBet = deskList[j].max_h;
			jSYS_LL_TableInfo.PersonCount = deskList[j].onlineNumber;
			jSYS_LL_TableInfo.CoinInSetting = deskList[j].onceExchangeValue;
			jSYS_LL_TableInfo.IsAutoKick = deskList[j].autoKick;
			mTableInfoList.Add(jSYS_LL_TableInfo);
		}
		_updateRistrictText();
	}

	public void ShowTableList(int iMode = 0)
	{
		updateUserInfo();
	}

	protected void _doSelectingTable()
	{
		if ((JSYS_LL_AppUIMngr.GetSingleton() != null && JSYS_LL_AppUIMngr.GetSingleton().GetAppState != AppState.App_On_TableList_Panel) || mAnimationStep < 0)
		{
			return;
		}
		if (mAnimationTime >= 0.2f)
		{
			if (mAnimationStep == 0)
			{
				mAnimationStep = 1;
			}
			else if (mAnimationStep == 1)
			{
				mAnimationStep = 2;
			}
			else if (mAnimationStep == 2)
			{
				mAnimationStep = -1;
				_updateRistrictText();
			}
			mAnimationTime = 0f;
		}
		mAnimationTime += Time.deltaTime;
	}

	protected void _updateRistrictText()
	{
		int num = (mTableInfoList.Count > 10) ? Mathf.FloorToInt(10f / (float)mTableInfoList.Count * (float)mCurrentTableId) : mCurrentTableId;
	}

	public void _clickTable(int num)
	{
		JSYS_LL_GameInfo.getInstance().UserInfo.TableId = num;
	}

	public void OnClickBackToRoom()
	{
		int roomId = JSYS_LL_GameInfo.getInstance().UserInfo.RoomId;
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(roomId);
		JSYS_LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
		JSYS_LL_GameInfo.getInstance().UserInfo.RoomId = -1;
		JSYS_LL_GameInfo.getInstance().UserInfo.TableId = -1;
	}

	protected void _releaseFromBg(GameObject go, bool isPress)
	{
		if (!isPress)
		{
			mIsDraging = false;
		}
	}

	protected void _dragTable(GameObject go, Vector2 delt)
	{
		if (mIsDraging)
		{
			return;
		}
		if (delt.x < 0f)
		{
			mCurrentTableId++;
			if (mCurrentTableId >= mTableInfoList.Count)
			{
				mCurrentTableId = 0;
			}
		}
		else
		{
			mCurrentTableId--;
			if (mCurrentTableId < 0)
			{
				mCurrentTableId = mTableInfoList.Count - 1;
			}
		}
		mAnimationTime = 0f;
		mAnimationStep = 0;
		mIsDraging = true;
	}
}
