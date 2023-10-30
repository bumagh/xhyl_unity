using LL_GameCommon;
using LL_UICommon;
using System;
using UnityEngine;

public class LL_SeatList : MonoBehaviour
{
	public UILabel userName;

	public UILabel coinCount;

	public UILabel testCoinCount;

	public UILabel roomName;

	protected Collider mBackgroundCol;

	protected Collider mBackBtnCol;

	protected Collider[] mSeatItemCol = new Collider[8];

	protected LL_HudManager mHudPanel;

	protected LL_TableList mTableList;

	protected LL_BetPanel mBetPanel;

	protected bool mIsButtonActiveFlg;

	protected GameObject[] mNicknameObj = new GameObject[8];

	protected GameObject[] mIconObj = new GameObject[8];

	protected UILabel[] mNicknameList = new UILabel[8];

	protected UISprite[] mIconList = new UISprite[8];

	protected bool[] mIsSeatFree = new bool[8]
	{
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true
	};

	protected Transform[] mUserList = new Transform[8];

	protected Transform mUserListTran;

	protected GameObject[] mSeatIdList = new GameObject[8];

	protected Transform mTableTran;

	protected bool mIsSeatListViable;

	protected float mStepTime;

	public ETableToSeatStep mListStep;

	private LL_GameInfo mGameInfo;

	private void Start()
	{
		userName = base.transform.Find("title/userName").GetComponent<UILabel>();
		coinCount = base.transform.Find("title/coin").GetComponent<UILabel>();
		testCoinCount = base.transform.Find("title/testCoin").GetComponent<UILabel>();
		roomName = base.transform.Find("title/name").GetComponent<UILabel>();
		mBackgroundCol = base.transform.Find("Background").GetComponent<Collider>();
		mBackBtnCol = base.transform.Find("title/backButton").GetComponent<Collider>();
		mHudPanel = GameObject.Find("HudPanel").GetComponent<LL_HudManager>();
		mTableList = GameObject.Find("TableListPanel").GetComponent<LL_TableList>();
		mBetPanel = GameObject.Find("BetPanel").GetComponent<LL_BetPanel>();
		base.transform.gameObject.AddComponent<TweenAlpha>();
		base.transform.GetComponent<TweenAlpha>().alpha = 1f;
		mTableTran = base.transform.Find("Table");
		mTableTran.gameObject.AddComponent<TweenScale>();
		mTableTran.gameObject.AddComponent<TweenPosition>();
		mUserListTran = base.transform.Find("SeatList");
		mUserListTran.gameObject.AddComponent<TweenPosition>();
		for (int i = 0; i < 8; i++)
		{
			mUserList[i] = mUserListTran.Find("SeatItem" + (i + 1));
			mNicknameObj[i] = mUserList[i].Find("PersonNickname").gameObject;
			mIconObj[i] = mUserList[i].Find("PersonIcon").gameObject;
			mNicknameList[i] = mUserList[i].Find("PersonNickname").GetComponent<UILabel>();
			mIconList[i] = mUserList[i].Find("PersonIcon").GetComponent<UISprite>();
			mSeatItemCol[i] = mUserList[i].GetComponent<Collider>();
			mUserList[i].gameObject.AddComponent<TweenScale>();
			mSeatIdList[i] = base.transform.Find("SeatId" + (i + 1)).gameObject;
			mSeatIdList[i].gameObject.AddComponent<TweenAlpha>();
			mSeatIdList[i].GetComponent<TweenAlpha>().alpha = 0f;
		}
		HideSeatList(3);
		mGameInfo = LL_GameInfo.getInstance();
	}

	public void updateUserInfo()
	{
		userName.text = mGameInfo.UserInfo.strName;
		coinCount.text = (mGameInfo.IsSpecial ? (mGameInfo.UserInfo.CoinCount % 10000).ToString() : mGameInfo.UserInfo.CoinCount.ToString());
		testCoinCount.text = (mGameInfo.IsSpecial ? (mGameInfo.UserInfo.ExpCoinCount % 10000).ToString() : mGameInfo.UserInfo.ExpCoinCount.ToString());
		if (mTableList.SelectedTable != null)
		{
			roomName.text = mTableList.SelectedTable.TableName;
		}
		else
		{
			roomName.text = string.Empty;
		}
	}

	private void Update()
	{
		if (mIsSeatListViable)
		{
			_doTableToSeat();
		}
		else
		{
			_doSeatToTable();
		}
	}

	public void ShowSeatList(int iMode = 0)
	{
		updateUserInfo();
		if (iMode == 0)
		{
			if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_Table)
			{
				return;
			}
			mIsButtonActiveFlg = false;
			mTableList.HideTableList(1);
		}
		else
		{
			base.transform.GetComponent<UIPanel>().enabled = true;
			base.transform.GetComponent<TweenAlpha>().alpha = 1f;
			if (iMode == 1)
			{
				if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_Table)
				{
					return;
				}
				mIsButtonActiveFlg = false;
				_setColliderActive(bIsActive: false);
				mListStep = ETableToSeatStep.SeatListScale;
				mStepTime = 0f;
				mUserListTran.localPosition = new Vector3(0f, 100f, 0f);
				for (int i = 0; i < 8; i++)
				{
					mUserList[i].localScale = new Vector3(0.001f, 0.001f, 1f);
					mSeatIdList[i].GetComponent<TweenAlpha>().alpha = 0f;
				}
				mTableTran.localPosition = new Vector3(0f, 50f, 0f);
				mTableTran.localScale = new Vector3(0.53f, 0.53f, 1f);
				TweenScale.Begin(mTableTran.gameObject, 0.5f, new Vector3(1f, 1f, 1f));
				TweenPosition.Begin(mTableTran.gameObject, 0.5f, new Vector3(0f, -260f, 0f));
			}
			else
			{
				base.transform.GetComponent<TweenAlpha>().alpha = 1f;
				base.transform.GetComponent<TweenAlpha>().enabled = false;
				mUserListTran.localPosition = new Vector3(0f, 150f, 0f);
				mUserListTran.GetComponent<TweenPosition>().enabled = false;
				for (int j = 0; j < 8; j++)
				{
					mUserList[j].localScale = new Vector3(1f, 1f, 1f);
					mUserList[j].GetComponent<TweenScale>().enabled = false;
				}
				for (int k = 0; k < 8; k++)
				{
					mSeatIdList[k].GetComponent<TweenAlpha>().alpha = 1f;
					mSeatIdList[k].GetComponent<TweenAlpha>().enabled = false;
				}
				mTableTran.localPosition = new Vector3(0f, -260f, 0f);
				mTableTran.localScale = new Vector3(1f, 1f, 1f);
				mTableTran.GetComponent<TweenPosition>().enabled = false;
				mTableTran.GetComponent<TweenScale>().enabled = false;
				mIsButtonActiveFlg = true;
				_setColliderActive(bIsActive: true);
				mListStep = ETableToSeatStep.NoneAnimation;
			}
		}
		mIsSeatListViable = true;
	}

	public void HideSeatList(int iMode = 0)
	{
		base.transform.GetComponent<TweenAlpha>().alpha = 1f;
		base.transform.GetComponent<TweenAlpha>().enabled = false;
		mUserListTran.localPosition = new Vector3(0f, 150f, 0f);
		mUserListTran.GetComponent<TweenPosition>().enabled = false;
		for (int i = 0; i < 8; i++)
		{
			mUserList[i].localScale = new Vector3(1f, 1f, 1f);
			mUserList[i].GetComponent<TweenScale>().enabled = false;
		}
		for (int j = 0; j < 8; j++)
		{
			mSeatIdList[j].GetComponent<TweenAlpha>().alpha = 1f;
			mSeatIdList[j].GetComponent<TweenAlpha>().enabled = false;
		}
		mTableTran.localPosition = new Vector3(0f, -260f, 0f);
		mTableTran.localScale = new Vector3(1f, 1f, 1f);
		mTableTran.GetComponent<TweenPosition>().enabled = false;
		mTableTran.GetComponent<TweenScale>().enabled = false;
		switch (iMode)
		{
		case 0:
			TweenAlpha.Begin(base.transform.gameObject, 0.5f, 0f);
			break;
		case 1:
			if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_TableList_Panel)
			{
				return;
			}
			mListStep = ETableToSeatStep.SeatIDScale;
			mStepTime = 0f;
			TweenPosition.Begin(mUserListTran.gameObject, 0.5f, new Vector3(0f, 100f, 0f));
			for (int k = 0; k < 8; k++)
			{
				TweenScale.Begin(mUserList[k].gameObject, 0.5f, new Vector3(0.01f, 0.01f, 1f));
				TweenAlpha.Begin(mSeatIdList[k], 0.3f, 0f);
			}
			break;
		case 2:
			if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_TableList_Panel)
			{
				return;
			}
			mListStep = ETableToSeatStep.NoneAnimation;
			base.transform.GetComponent<UIPanel>().enabled = false;
			break;
		default:
			mListStep = ETableToSeatStep.NoneAnimation;
			base.transform.GetComponent<UIPanel>().enabled = false;
			break;
		}
		mIsButtonActiveFlg = false;
		mIsSeatListViable = false;
		_setColliderActive(bIsActive: false);
	}

	public void AddPerson(int iSeatId, string strNickname, int iIconIndex = 1)
	{
		if (iSeatId > 8 || iSeatId < 1)
		{
			LL_ErrorManager.GetSingleton().AddError("Error:seatList座位序号错误" + iSeatId);
			return;
		}
		if (iIconIndex > 8 || iIconIndex < 0)
		{
			try
			{
				LL_ErrorManager.GetSingleton().AddError("Error:seatList玩家" + iSeatId + "头像错误" + iIconIndex);
			}
			catch (Exception)
			{
			}
			return;
		}
		if (strNickname.CompareTo(string.Empty) == 0)
		{
			mIsSeatFree[iSeatId - 1] = true;
			mNicknameObj[iSeatId - 1].SetActiveRecursively(state: false);
			mIconObj[iSeatId - 1].SetActiveRecursively(state: false);
			return;
		}
		mNicknameObj[iSeatId - 1].SetActiveRecursively(state: true);
		mIconObj[iSeatId - 1].SetActiveRecursively(state: true);
		mNicknameList[iSeatId - 1].text = strNickname;
		mIconList[iSeatId - 1].spriteName = "personIcon" + iIconIndex;
		if (mIsSeatFree[iSeatId - 1])
		{
			mIsSeatFree[iSeatId - 1] = false;
		}
	}

	protected void _doTableToSeat()
	{
		if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_Table || mListStep == ETableToSeatStep.NoneAnimation)
		{
			return;
		}
		mStepTime += Time.deltaTime;
		if (mListStep == ETableToSeatStep.SeatListScale)
		{
			if (mStepTime >= 0.5f)
			{
				mListStep = ETableToSeatStep.SeatIDScale;
				mStepTime = 0f;
				TweenPosition.Begin(mUserListTran.gameObject, 0.5f, new Vector3(0f, 150f, 0f));
				for (int i = 0; i < 8; i++)
				{
					TweenScale.Begin(mUserList[i].gameObject, 0.5f, new Vector3(1f, 1f, 1f));
					TweenAlpha.Begin(mSeatIdList[i], 0.3f, 1f);
				}
			}
		}
		else if (mListStep == ETableToSeatStep.SeatIDScale && mStepTime >= 0.7f)
		{
			mIsButtonActiveFlg = true;
			_setColliderActive(bIsActive: true);
			mListStep = ETableToSeatStep.NoneAnimation;
		}
	}

	protected void _doSeatToTable()
	{
		if (LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_TableList_Panel || mListStep == ETableToSeatStep.NoneAnimation)
		{
			return;
		}
		mStepTime += Time.deltaTime;
		if (mListStep == ETableToSeatStep.SeatListScale)
		{
			if (mStepTime >= 0.5f)
			{
				HideSeatList(2);
				mTableList.ShowTableList(1);
			}
		}
		else if (mListStep == ETableToSeatStep.SeatIDScale && mStepTime >= 0.5f)
		{
			mListStep = ETableToSeatStep.SeatListScale;
			mStepTime = 0f;
			TweenScale.Begin(mTableTran.gameObject, 0.5f, new Vector3(0.53f, 0.53f, 1f));
			TweenPosition.Begin(mTableTran.gameObject, 0.5f, new Vector3(0f, 50f, 0f));
		}
	}

	protected void _setColliderActive(bool bIsActive)
	{
		mBackgroundCol.enabled = bIsActive;
		mBackBtnCol.enabled = bIsActive;
		for (int i = 0; i < 8; i++)
		{
			mSeatItemCol[i].enabled = bIsActive;
		}
	}

	public void _onClickSeat(GameObject sender)
	{
		if (!mIsButtonActiveFlg || LL_AppUIMngr.GetSingleton().GetAppState() != AppState.App_On_Table)
		{
			return;
		}
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		int num = int.Parse(sender.name.Substring(8));
		int num2 = LL_GameInfo.getInstance().UserInfo.RoomId + 1;
		int num3 = (num2 != 1) ? LL_GameInfo.getInstance().UserInfo.CoinCount : LL_GameInfo.getInstance().UserInfo.ExpCoinCount;
		mBetPanel.ModifyChip(mTableList.SelectedTable);
		mHudPanel.SetCurrentPosition(num2, mTableList.SelectedTable);
		if (!LL_MyTest.TEST)
		{
			if (!mIsSeatFree[num - 1])
			{
				if (num != LL_GameInfo.getInstance().UserInfo.SeatId)
				{
					UnityEngine.Debug.LogError("点击头像");
				}
			}
			else if (LL_GameInfo.getInstance().UserInfo.SeatId == -1)
			{
				if (num3 >= mTableList.SelectedTable.Ristrict)
				{
					LL_GameInfo.getInstance().UserInfo.SeatId = num;
					LL_NetMngr.GetSingleton().MyCreateSocket.SendSelectSeat(LL_GameInfo.getInstance().UserInfo.TableId, num);
					LL_GameTipManager.GetSingleton().StartNetTiming();
					LL_MusicMngr.GetSingleton().SetGameMusicVolume(LL_GameInfo.getInstance().Setted.bIsGameVolum ? 0.5f : 0f);
				}
				else if (num2 == 1)
				{
					LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.SelectTable_SendExpCoin, string.Empty);
				}
				else
				{
					LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.SelectTable_CreditBelowRistrict, string.Empty);
				}
			}
		}
		else
		{
			LL_AppUIMngr.GetSingleton().SetAppState(AppState.App_On_Game);
			GameObject.Find("HudPanel").GetComponent<LL_HudManager>().ShowHud();
		}
	}

	public void OnClickBackToTable()
	{
		if (mIsButtonActiveFlg && LL_AppUIMngr.GetSingleton().GetAppState() == AppState.App_On_Table)
		{
			LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
			LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
			LL_GameInfo.getInstance().UserInfo.TableId = -1;
			LL_GameInfo.getInstance().UserInfo.SeatId = -1;
			HideSeatList(1);
		}
	}
}
