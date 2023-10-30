using BCBM_GameCommon;
using BCBM_UICommon;
using System;
using System.Collections;
using UnityEngine;

public class BCBM_HudManager : MonoBehaviour
{
	protected enum ENoticeState
	{
		NoticeFadeIn,
		NoticeMoveToLeft,
		NoticeFadeOut
	}

	private BCBM_GameInfo mGameInfo;

	protected BCBM_ChatPanel mChatPanel;

	protected BCBM_BetPanel mBetPanel;

	protected BCBM_PrizeResult mResult;

	protected BCBM_HistoryRecord mRecordPanel;

	protected UIPanel mHudPanel;

	protected BCBM_TableList mTableList;

	protected BCBM_SeatList mSeatList;

	protected Collider mBackgroundCol;

	protected Collider[] mUserItemCol = new Collider[8];

	protected GameObject mWaitTipObj;

	public Material mHudMaterial;

	public int mDiceResult;

	protected float mResultMatOffset = 0.0001f;

	protected EDiceState mRollState;

	protected float mRollTme;

	protected float mRollSpeed;

	protected float mTotalOffset;

	protected float mDistance;

	protected float mSpeedDownTotalTime;

	protected float mRollTotalTime = 15f;

	protected bool mIsJoinGame;

	protected int mMaxCD;

	protected float mCDTime;

	protected UILabel mGameCDText;

	protected float mCDApartTime = 1f;

	protected GameObject mCDTimeObj;

	protected bool mIsCDTimeVisable;

	protected UILabel mBonusText;

	protected System.Random mRandSeed;

	protected float mApartTime;

	protected bool mIsBonusStay = true;

	protected bool mIsBonusSpin;

	protected int mBonusNumber;

	protected bool mIsBonusAward;

	protected BCBM_TableInfo mCurrentTable;

	protected string mCurrentPosition = string.Empty;

	protected UILabel mSelfPosition;

	protected GameObject[] mAllUser = new GameObject[8];

	protected UISprite[] mAllUserIcon = new UISprite[8];

	protected UISprite[] mAllUserIconFrame = new UISprite[8];

	protected UILabel[] mAllUserNickname = new UILabel[8];

	protected bool[] mIsFree = new bool[8]
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

	protected GameObject mLuckyUserObj;

	protected UISprite mBlinkFrameSprite;

	protected Transform mLuckyType;

	protected UISprite mLuckyTypeSprite;

	protected bool mIsLuckyVisable;

	protected bool mIsLuckyBlink;

	protected float mBlinkTime;

	protected bool mIsBlinkActive;

	protected int mLuckyMacNum = 1;

	protected int mLuckyAwardType = 1;

	protected UILabel mGameCredit;

	protected UISprite mGameCoinTitle;

	protected UILabel mGameCoin;

	protected GameObject mNoticeObj;

	protected UILabel mNoticeMsgText;

	protected Transform mNoticeMsgTran;

	protected bool mIsNoticeVisable;

	protected Queue mNoticeMsgQueue = new Queue();

	protected float mNoticeTotalTime;

	protected float mNoticeScrollTime;

	protected ENoticeState mNoticeState;

	protected GameObject mCoinZeroTipObj;

	protected UISprite mCoinZeroTipSprite;

	protected GameObject mScoreZeroTipObj;

	protected bool mIsCoinZeroTipVisable;

	protected bool mIsScoreZeroTipVisable;

	protected float mCoinZeroTipTime;

	protected float mScoreZeroTipTime;

	protected bool mValidRecordRequest = true;

	protected bool mIsRecordPressed;

	private Collider BG;

	[SerializeField]
	private GameObject result;

	[SerializeField]
	private BCBM_ChatPanel PublicChatRecord;

	[SerializeField]
	private BCBM_OtherUserInfoPanel otherUserInfoPanel;

	public int TimeCD => mMaxCD;

	public int BonusNumber => mBonusNumber;

	public bool IsBonusAward
	{
		get
		{
			return mIsBonusAward;
		}
		set
		{
			mIsBonusAward = value;
		}
	}

	public int LuckyMacNum
	{
		set
		{
			if (value > 8 || value < 1)
			{
				BCBM_ErrorManager.GetSingleton().AddError("幸运奖衔接_分机号错误：" + value);
				value = 1;
			}
			mLuckyMacNum = value;
		}
	}

	public int LuckyAwardType
	{
		set
		{
			if (value > 2 || value < 0)
			{
				BCBM_ErrorManager.GetSingleton().AddError("幸运奖衔接_幸运奖错误：" + value);
				value = 0;
			}
			mLuckyAwardType = value;
		}
	}

	public int GameCredit
	{
		set
		{
			mGameCredit.text = ((value != 0) ? value.ToString() : ((BCBM_GameInfo.getInstance().Language == 0) ? "请取分" : "please key in"));
			if (value != 0 && mIsScoreZeroTipVisable)
			{
				mIsScoreZeroTipVisable = false;
				if ((bool)mScoreZeroTipObj)
				{
					mScoreZeroTipObj.SetActiveRecursively(state: false);
				}
			}
		}
	}

	public int GameCoin
	{
		set
		{
			mGameCoin.text = (mGameInfo.IsSpecial ? (value % 10000).ToString() : value.ToString());
			if (value != 0 && mIsCoinZeroTipVisable)
			{
				mIsCoinZeroTipVisable = false;
				if ((bool)mCoinZeroTipObj)
				{
					mCoinZeroTipObj.SetActiveRecursively(state: false);
				}
			}
		}
	}

	public bool IsRecordPressed
	{
		get
		{
			return mIsRecordPressed;
		}
		set
		{
			mIsRecordPressed = value;
		}
	}

	private void Start()
	{
		mRandSeed = new System.Random();
		mWaitTipObj = base.transform.Find("WaitTip").gameObject;
		mBackgroundCol = mWaitTipObj.transform.Find("Tip").GetComponent<Collider>();
		mHudMaterial.SetTextureOffset("_MainTex", new Vector2(0f, mResultMatOffset));
		mCDTimeObj = base.transform.Find("Time").gameObject;
		mGameCDText = mCDTimeObj.transform.Find("Number").GetComponent<UILabel>();
		mBonusText = base.transform.Find("Bonus").Find("Number").GetComponent<UILabel>();
		mBetPanel = GameObject.Find("BetPanel").GetComponent<BCBM_BetPanel>();
		mResult = GameObject.Find("ResultPanel").GetComponent<BCBM_PrizeResult>();
		mHudPanel = GameObject.Find("HudPanel").GetComponent<UIPanel>();
		mChatPanel = GameObject.Find("ChatPanel").GetComponent<BCBM_ChatPanel>();
		mRecordPanel = GameObject.Find("RecordPanel").GetComponent<BCBM_HistoryRecord>();
		mTableList = GameObject.Find("TableListPanel").GetComponent<BCBM_TableList>();
		mSeatList = GameObject.Find("SeatPanel").GetComponent<BCBM_SeatList>();
		mSelfPosition = base.transform.Find("SelfPosition").GetComponent<UILabel>();
		for (int i = 0; i < 8; i++)
		{
			mAllUser[i] = base.transform.Find("UserList").Find("UserItem" + i).gameObject;
			mAllUserIcon[i] = mAllUser[i].transform.Find("UserIcon").GetComponent<UISprite>();
			mAllUserNickname[i] = mAllUser[i].transform.Find("Nickname").GetComponent<UILabel>();
			mAllUserIconFrame[i] = base.transform.Find("UserList").Find("FrameList").Find("IconFrame" + i)
				.GetComponent<UISprite>();
			mUserItemCol[i] = mAllUser[i].transform.GetComponent<Collider>();
		}
		mGameCredit = base.transform.Find("GameScore").Find("CreditText").GetComponent<UILabel>();
		mGameCoin = base.transform.Find("GameScore").Find("CoinText").GetComponent<UILabel>();
		mGameCoinTitle = base.transform.Find("GameScore").Find("CoinTitle").GetComponent<UISprite>();
		mGameCoin.text = ((BCBM_GameInfo.getInstance().UserInfo.RoomId == 0) ? BCBM_GameInfo.getInstance().UserInfo.ExpCoinCount.ToString() : BCBM_GameInfo.getInstance().UserInfo.CoinCount.ToString());
		mGameCredit.text = ((BCBM_GameInfo.getInstance().UserInfo.GameScore == 0) ? "请取分" : BCBM_GameInfo.getInstance().UserInfo.GameScore.ToString());
		mLuckyUserObj = base.transform.Find("UserList").Find("LuckyUser").gameObject;
		mBlinkFrameSprite = mLuckyUserObj.transform.Find("BlinkFrame").GetComponent<UISprite>();
		mLuckyType = mLuckyUserObj.transform.Find("LuckyType");
		TweenAlpha.Begin(mLuckyType.gameObject, 0.2f, 1f);
		mLuckyTypeSprite = mLuckyType.GetComponent<UISprite>();
		mLuckyUserObj.SetActiveRecursively(state: false);
		mNoticeObj = base.transform.Find("NoticeMessage").gameObject;
		mNoticeMsgTran = base.transform.Find("NoticeMessage").transform.Find("Label");
		mNoticeMsgText = mNoticeMsgTran.GetComponent<UILabel>();
		mNoticeObj.transform.GetComponent<UIPanel>().enabled = false;
		mCoinZeroTipObj = base.transform.Find("GameScore").Find("CoinZeroTip").gameObject;
		mCoinZeroTipSprite = mCoinZeroTipObj.transform.Find("Background").GetComponent<UISprite>();
		mScoreZeroTipObj = base.transform.Find("GameScore").Find("ScoreZeroTip").gameObject;
		mCoinZeroTipObj.SetActiveRecursively(state: false);
		mScoreZeroTipObj.SetActiveRecursively(state: false);
		mHudPanel.enabled = false;
		_setColliderActive(bIsActive: false);
		mGameInfo = BCBM_GameInfo.getInstance();
		BG = base.transform.Find("BG").GetComponent<Collider>();
	}

	private void Update()
	{
		mGameCredit.text = ((BCBM_GameInfo.getInstance().UserInfo.GameScore == 0) ? "请取分" : BCBM_GameInfo.getInstance().UserInfo.GameScore.ToString());
		if (mHudPanel.enabled && (bool)mSelfPosition)
		{
			string text = (DateTime.Now.Hour < 10) ? ("0" + DateTime.Now.Hour) : DateTime.Now.Hour.ToString();
			string text2 = (DateTime.Now.Minute < 10) ? ("0" + DateTime.Now.Minute) : DateTime.Now.Minute.ToString();
			mSelfPosition.text = text + ":" + text2 + " " + mCurrentPosition;
		}
		if (mIsCoinZeroTipVisable)
		{
			mCoinZeroTipTime += Time.deltaTime;
			if (mCoinZeroTipTime >= 2f)
			{
				mIsCoinZeroTipVisable = false;
				mCoinZeroTipObj.SetActiveRecursively(state: false);
			}
		}
		if (mIsScoreZeroTipVisable)
		{
			mScoreZeroTipTime += Time.deltaTime;
			if (mScoreZeroTipTime >= 2f)
			{
				mIsScoreZeroTipVisable = false;
				mScoreZeroTipObj.SetActiveRecursively(state: false);
			}
		}
		_bonusAnimation();
		_cdTimeAnimation();
		_updateNoticeMessage();
		_rollDice();
		if (mIsLuckyVisable)
		{
			_luckyBlink();
		}
	}

	public void ShowHud()
	{
		BCBM_GameTipManager.GetSingleton().EndNetTiming();
		mTotalOffset = mResultMatOffset;
		mHudMaterial.SetTextureOffset("_MainTex", new Vector2(0f, mTotalOffset));
		mHudPanel.enabled = true;
		mNoticeObj.transform.GetComponent<UIPanel>().enabled = false;
		_setColliderActive(bIsActive: true);
		mGameCoinTitle.spriteName = ((BCBM_GameInfo.getInstance().UserInfo.RoomId != 0) ? ((BCBM_GameInfo.getInstance().Language == 0) ? "youXiBi" : "youXiBi_e") : ((BCBM_GameInfo.getInstance().Language == 0) ? "tiYanBi" : "tiYanBi_e"));
		mGameCoin.text = ((BCBM_GameInfo.getInstance().UserInfo.RoomId == 0) ? BCBM_GameInfo.getInstance().UserInfo.ExpCoinCount.ToString() : BCBM_GameInfo.getInstance().UserInfo.CoinCount.ToString());
		mGameCredit.text = ((BCBM_GameInfo.getInstance().UserInfo.GameScore != 0) ? BCBM_GameInfo.getInstance().UserInfo.GameScore.ToString() : ((BCBM_GameInfo.getInstance().Language == 0) ? "请取分" : "please key in"));
		mCDTimeObj.transform.localPosition = new Vector3(735f, 322f, 0f);
		BCBM_UITimesPrize.GetSingleton().Reset();
		mSeatList.HideSeatList();
		BCBM_LuckyLion_SoundManager.GetSingleton().StopGameHallMusic();
		if (BCBM_MyTest.TEST)
		{
			SetGameCD();
		}
		result.SetActive(value: true);
	}

	public void HideHud()
	{
		mHudPanel.enabled = false;
		_setColliderActive(bIsActive: false);
		ResetGame();
		result.SetActive(value: false);
		BCBM_LuckyLion_SoundManager.GetSingleton().PlayGameHallMusic();
	}

	protected void _setColliderActive(bool bIsActive)
	{
		mBackgroundCol.enabled = bIsActive;
		IEnumerator enumerator = mHudPanel.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform transform = (Transform)current;
				if ((bool)transform.GetComponent<Collider>())
				{
					transform.GetComponent<Collider>().enabled = bIsActive;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		IEnumerator enumerator2 = mHudPanel.transform.Find("GameScore").GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object current2 = enumerator2.Current;
				Transform transform2 = (Transform)current2;
				if ((bool)transform2.GetComponent<Collider>())
				{
					transform2.GetComponent<Collider>().enabled = bIsActive;
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		for (int i = 0; i < 8; i++)
		{
			mUserItemCol[i].enabled = bIsActive;
		}
	}

	public void SetDiceResult(EDiceResult EResult)
	{
		if (EResult >= EDiceResult.ZhuangJia && EResult <= EDiceResult.XianJia)
		{
			mDiceResult = (int)EResult;
			switch (EResult)
			{
			case EDiceResult.ZhuangJia:
				mResultMatOffset = 0.145f;
				break;
			case EDiceResult.He:
				mResultMatOffset = 0.86f;
				break;
			case EDiceResult.XianJia:
				mResultMatOffset = 0f;
				break;
			}
			if (mMaxCD > 0)
			{
				mMaxCD = 0;
				mIsCDTimeVisable = false;
				BCBM_MusicMngr.GetSingleton().PlayUISound(BCBM_MusicMngr.MUSIC_UI.UI_COUNTDONW1);
				TweenPosition.Begin(mCDTimeObj, 0.5f, new Vector3(735f, 322f, 0f));
			}
		}
	}

	public void StartRollDice(float fTime = 15f)
	{
		if (mIsJoinGame)
		{
			mRollState = EDiceState.SpeedUp;
			mRollSpeed = 0f;
			mRollTme = 0f;
			mTotalOffset = mResultMatOffset;
			mRollTotalTime = fTime - 2f;
			mIsBonusSpin = true;
			mIsBonusStay = false;
		}
	}

	public void SetGameCD(int nTime = 30, bool bIsJoinGame = false)
	{
		if (!mIsJoinGame)
		{
			mWaitTipObj.SetActiveRecursively(state: false);
			mIsJoinGame = true;
		}
		if (!mIsCDTimeVisable)
		{
			mIsCDTimeVisable = true;
			TweenPosition.Begin(mCDTimeObj, 0.5f, new Vector3(580f, 322f, 0f));
		}
		if (bIsJoinGame)
		{
			mMaxCD = nTime;
		}
		else
		{
			mMaxCD = mCurrentTable.MaxCD;
		}
		mCDTime = 0f;
		mGameCDText.text = mMaxCD.ToString();
		if (mIsLuckyVisable)
		{
			mIsLuckyVisable = false;
			mLuckyUserObj.SetActiveRecursively(state: false);
		}
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendResultList(BCBM_GameInfo.getInstance().UserInfo.TableId);
	}

	public void SetBonusNumber(int nNum, bool bIsBonusAward = false)
	{
		mIsBonusAward = bIsBonusAward;
		mBonusNumber = nNum;
	}

	public void StopBonusSpin()
	{
		mIsBonusSpin = false;
	}

	public void ShowLuckyMachineAnimation()
	{
		mIsLuckyVisable = true;
		mLuckyUserObj.SetActiveRecursively(state: true);
		mLuckyUserObj.transform.localPosition = ((mLuckyMacNum <= 4) ? new Vector3(-580f, 190f - (float)((mLuckyMacNum - 1) % 4) * 125f, 0f) : new Vector3(580f, 190f - (float)((mLuckyMacNum - 1) % 4) * 125f, 0f));
		mLuckyType.GetComponent<TweenAlpha>().alpha = 0f;
		TweenAlpha.Begin(mLuckyType.gameObject, 0.05f, 1f);
		mLuckyTypeSprite.spriteName = "luckyType" + mLuckyAwardType;
		mIsBlinkActive = true;
		mBlinkTime = 0f;
		mBlinkFrameSprite.spriteName = "IconFrameBlink0";
	}

	public void AddUser(int iSeatID, string strNickname, int iIconIndex = 1)
	{
		if (iSeatID > 7 && iSeatID < 0)
		{
			return;
		}
		if (strNickname.CompareTo(string.Empty) == 0)
		{
			mAllUserIconFrame[iSeatID].spriteName = "IconFrameOther";
			mAllUser[iSeatID].SetActiveRecursively(state: false);
			mIsFree[iSeatID] = true;
			return;
		}
		mAllUserNickname[iSeatID].text = strNickname;
		mAllUser[iSeatID].SetActiveRecursively(state: true);
		mAllUserIcon[iSeatID].spriteName = "personIcon" + iIconIndex;
		mIsFree[iSeatID] = false;
		if (iSeatID + 1 == BCBM_GameInfo.getInstance().UserInfo.SeatId)
		{
			mAllUserIconFrame[iSeatID].spriteName = "IconFrameSelf";
		}
		else
		{
			mAllUserIconFrame[iSeatID].spriteName = "IconFrameOther";
		}
	}

	public void ResetGame()
	{
		mIsCDTimeVisable = false;
		mMaxCD = 0;
		if (mIsLuckyVisable)
		{
			mIsLuckyVisable = false;
			mLuckyUserObj.SetActiveRecursively(state: false);
		}
		mWaitTipObj.SetActiveRecursively(state: true);
		mNoticeObj.transform.GetComponent<UIPanel>().enabled = false;
		mNoticeMsgQueue.Clear();
		mRecordPanel.ClearRecord();
		mIsNoticeVisable = false;
		mIsJoinGame = false;
		mRollState = EDiceState.Stay;
		mValidRecordRequest = true;
		mChatPanel.Restart();
		mBetPanel.Restart();
	}

	public void AddBroadcastMessage(string strMessage)
	{
		if (BCBM_AppUIMngr.GetSingleton().GetAppState == AppState.App_On_Game)
		{
			mNoticeMsgQueue.Enqueue(strMessage);
		}
	}

	protected void _updateNoticeMessage()
	{
		if (mNoticeMsgQueue.Count > 0 && !mIsNoticeVisable)
		{
			mNoticeObj.transform.GetComponent<UIPanel>().enabled = true;
			TweenAlpha.Begin(mNoticeObj, 0.5f, 1f);
			mNoticeMsgText.text = (string)mNoticeMsgQueue.Dequeue();
			mNoticeMsgTran.localPosition = new Vector3(300f, 0f, 0f);
			mNoticeState = ENoticeState.NoticeFadeIn;
			mNoticeTotalTime = 0f;
			mIsNoticeVisable = true;
		}
		if (!mIsNoticeVisable)
		{
			return;
		}
		if (mNoticeState == ENoticeState.NoticeFadeIn)
		{
			if (mNoticeTotalTime >= 0.5f)
			{
				float num = BCBM_MyUICommon.CalculateCharCount(mNoticeMsgText.text);
				Vector3 localScale = mNoticeMsgTran.localScale;
				float x = -900f - num * localScale.x / 2f;
				mNoticeScrollTime = 6f * (float)(BCBM_MyUICommon.CalculateCharCount(mNoticeMsgText.text) / 48 + 1);
				mNoticeTotalTime = 0f;
				mNoticeState = ENoticeState.NoticeMoveToLeft;
				TweenPosition.Begin(mNoticeMsgTran.gameObject, mNoticeScrollTime, new Vector3(x, 0f, 0f));
			}
		}
		else if (mNoticeState == ENoticeState.NoticeMoveToLeft)
		{
			if (mNoticeTotalTime >= mNoticeScrollTime)
			{
				mNoticeTotalTime = 0f;
				mNoticeState = ENoticeState.NoticeFadeOut;
				TweenAlpha.Begin(mNoticeObj, 0.5f, 0f);
			}
		}
		else if (mNoticeState == ENoticeState.NoticeFadeOut && mNoticeTotalTime >= 0.5f)
		{
			mIsNoticeVisable = false;
			mNoticeObj.transform.GetComponent<UIPanel>().enabled = false;
		}
		mNoticeTotalTime += Time.deltaTime;
	}

	protected void _luckyBlink()
	{
		if (mBlinkTime >= 0.2f)
		{
			if (mIsBlinkActive)
			{
				mBlinkFrameSprite.spriteName = "IconFrameBlink0";
			}
			else
			{
				mBlinkFrameSprite.spriteName = "IconFrameBlink1";
			}
			mBlinkTime = 0f;
			mIsBlinkActive = !mIsBlinkActive;
		}
		mBlinkTime += Time.deltaTime;
	}

	protected void _bonusAnimation()
	{
		if (mIsBonusStay)
		{
			return;
		}
		if (!mIsBonusSpin)
		{
			mIsBonusStay = true;
			mBonusText.text = mBonusNumber.ToString().PadLeft(5, '0');
			return;
		}
		mApartTime += Time.deltaTime;
		if (mApartTime >= 0.1f)
		{
			mApartTime = 0f;
			mBonusText.text = mRandSeed.Next(1000, 30000).ToString().PadLeft(5, '0');
		}
	}

	protected void _cdTimeAnimation()
	{
		if (mMaxCD <= 0)
		{
			return;
		}
		mCDTime += Time.deltaTime;
		if (mCDTime >= mCDApartTime)
		{
			mMaxCD -= (int)(mCDTime / mCDApartTime);
			mCDTime = 0f;
			mGameCDText.text = mMaxCD.ToString();
			if (mMaxCD <= 0)
			{
				mIsCDTimeVisable = false;
				BCBM_MusicMngr.GetSingleton().PlayUISound(BCBM_MusicMngr.MUSIC_UI.UI_COUNTDONW1);
				TweenPosition.Begin(mCDTimeObj, 0.5f, new Vector3(735f, 322f, 0f));
			}
			if (mMaxCD <= 3 && mMaxCD > 0)
			{
				BCBM_MusicMngr.GetSingleton().PlayUISound(BCBM_MusicMngr.MUSIC_UI.UI_COUNTDOWN0);
			}
		}
	}

	protected void _rollDice()
	{
		if (mRollState != 0)
		{
			mRollTme += Time.deltaTime;
			if (mRollState == EDiceState.SpeedUp)
			{
				_rollSpeedUp();
			}
			else if (mRollState == EDiceState.SpeedDown)
			{
				_rollToStop();
			}
			float num = Time.deltaTime * mRollSpeed;
			mTotalOffset = (mTotalOffset - num) % -1f;
			mHudMaterial.SetTextureOffset("_MainTex", new Vector2(0f, mTotalOffset));
		}
	}

	protected void _rollSpeedUp()
	{
		if (mRollSpeed <= 1f)
		{
			mRollSpeed += Time.deltaTime * 2f;
		}
		if (mRollTme >= mRollTotalTime / 3f)
		{
			mRollState = EDiceState.SpeedDown;
			mSpeedDownTotalTime = mRollTotalTime - mRollTme;
			if (mResultMatOffset > mTotalOffset)
			{
				mDistance = mResultMatOffset - mTotalOffset - 1f;
			}
			else
			{
				mDistance = mResultMatOffset - mTotalOffset;
			}
			mDistance -= 2f;
			mRollSpeed = -2f * mDistance / mSpeedDownTotalTime;
			_rollToStop();
		}
	}

	protected void _rollToStop()
	{
		if (mSpeedDownTotalTime <= 0f)
		{
			mRollSpeed = 0f;
			mTotalOffset = mResultMatOffset;
			mRollState = EDiceState.Stay;
			if (mDiceResult == 0)
			{
				BCBM_MusicMngr.GetSingleton().PlayUISound(BCBM_MusicMngr.MUSIC_UI.UI_ZHUANG);
			}
			else if (mDiceResult == 1)
			{
				BCBM_MusicMngr.GetSingleton().PlayUISound(BCBM_MusicMngr.MUSIC_UI.UI_HE);
			}
			else if (mDiceResult == 2)
			{
				BCBM_MusicMngr.GetSingleton().PlayUISound(BCBM_MusicMngr.MUSIC_UI.UI_XIAN);
			}
		}
		else
		{
			mRollSpeed = -2f * mDistance / mSpeedDownTotalTime;
			mSpeedDownTotalTime -= Time.deltaTime;
			mDistance += Mathf.Abs(Time.deltaTime * mRollSpeed);
		}
	}

	public void SetCurrentPosition(int room, BCBM_TableInfo table)
	{
		mCurrentTable = table;
		if (room == 1)
		{
			mCurrentPosition = ((BCBM_GameInfo.getInstance().Language == 0) ? (" 练习厅 \"" + table.TableName + "\"桌") : (" \"" + table.TableName + "\" table.Arena"));
		}
		else
		{
			mCurrentPosition = ((BCBM_GameInfo.getInstance().Language == 0) ? (" 竞技厅 \"" + table.TableName + "\"桌") : (" \"" + table.TableName + "\" table.Training"));
		}
		mCDApartTime = (float)(mCurrentTable.MaxCD - 1) / (float)mCurrentTable.MaxCD;
	}

	public void ShowScoreTip(bool bIsInHud = true)
	{
		mScoreZeroTipTime = 0f;
		mIsScoreZeroTipVisable = true;
		mScoreZeroTipObj.SetActiveRecursively(state: true);
		if (bIsInHud)
		{
			mScoreZeroTipObj.transform.localPosition = new Vector3(-305f, 30f, 0f);
		}
		else
		{
			mScoreZeroTipObj.transform.localPosition = new Vector3(200f, 30f, 0f);
		}
	}

	public void _onClickCoinIn()
	{
		int num = 0;
		int num2 = (BCBM_GameInfo.getInstance().UserInfo.RoomId == 0) ? BCBM_GameInfo.getInstance().UserInfo.ExpCoinCount : BCBM_GameInfo.getInstance().UserInfo.CoinCount;
		if (num2 <= 0)
		{
			if (BCBM_GameInfo.getInstance().UserInfo.RoomId == 0 && BCBM_GameInfo.getInstance().UserInfo.GameScore <= 0)
			{
				BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.SelectTable_SendExpCoin, string.Empty);
			}
			mCoinZeroTipTime = 0f;
			mIsCoinZeroTipVisable = true;
			mCoinZeroTipObj.SetActiveRecursively(state: true);
			if (BCBM_GameInfo.getInstance().Language == 0)
			{
				mCoinZeroTipSprite.spriteName = ((BCBM_GameInfo.getInstance().UserInfo.RoomId == 0) ? "tybbz" : "yxbbz");
			}
			else
			{
				mCoinZeroTipSprite.spriteName = ((BCBM_GameInfo.getInstance().UserInfo.RoomId == 0) ? "tybbz_e" : "yxbbz_e");
			}
			if (BCBM_LuckyLion_SoundManager.GetSingleton() != null)
			{
				BCBM_LuckyLion_SoundManager.GetSingleton().playButtonSound(BCBM_LuckyLion_SoundManager.EUIBtnSoundType.BetFail);
			}
		}
		else
		{
			num = ((num2 > mCurrentTable.CoinInSetting) ? mCurrentTable.CoinInSetting : num2);
			UnityEngine.Debug.Log(num);
			BCBM_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinIn(num);
			if (BCBM_LuckyLion_SoundManager.GetSingleton() != null)
			{
				BCBM_LuckyLion_SoundManager.GetSingleton().playButtonSound(BCBM_LuckyLion_SoundManager.EUIBtnSoundType.CoinIn);
			}
		}
	}

	public void _onClickCoinOut()
	{
		int num = 0;
		int gameScore = BCBM_GameInfo.getInstance().UserInfo.GameScore;
		if (gameScore < mCurrentTable.CreditPerCoin)
		{
			ShowScoreTip();
			if (BCBM_LuckyLion_SoundManager.GetSingleton() != null)
			{
				BCBM_LuckyLion_SoundManager.GetSingleton().playButtonSound(BCBM_LuckyLion_SoundManager.EUIBtnSoundType.BetFail);
			}
			return;
		}
		num = ((gameScore < mCurrentTable.CoinInSetting * mCurrentTable.CreditPerCoin) ? (gameScore / mCurrentTable.CreditPerCoin * mCurrentTable.CreditPerCoin) : (mCurrentTable.CoinInSetting * mCurrentTable.CreditPerCoin));
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinOut(num);
		if (BCBM_LuckyLion_SoundManager.GetSingleton() != null)
		{
			BCBM_LuckyLion_SoundManager.GetSingleton().playButtonSound(BCBM_LuckyLion_SoundManager.EUIBtnSoundType.CoinIn);
		}
	}

	public void OnClickExitGame()
	{
		BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.IsExitGame, string.Empty);
		if (BCBM_LuckyLion_SoundManager.GetSingleton() != null)
		{
			BCBM_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		}
	}

	public void _onClickUserItem(GameObject sender)
	{
		int num = int.Parse(sender.name.Substring(8)) + 1;
		if (BCBM_MyTest.TEST)
		{
			BCBM_ErrorManager.GetSingleton().AddError("点击玩家:" + num);
		}
		else if (num != BCBM_GameInfo.getInstance().UserInfo.SeatId)
		{
			mChatPanel.ChatSeatID = num;
			BCBM_NetMngr.GetSingleton().MyCreateSocket.SendPlayerInfo(mCurrentTable.GetUserKeyID(num - 1));
		}
	}

	public void _onPresskRecord()
	{
		mIsRecordPressed = true;
		mRecordPanel.ShowRecord();
	}

	public void _onReleaseBtn()
	{
		mIsRecordPressed = false;
		mRecordPanel.HideRecord();
	}

	public void OnClickChatRecord()
	{
		PublicChatRecord.ShowPublicChatWindow();
	}

	public void OnHidAllChat()
	{
		PublicChatRecord.HidePrivateChatWindow();
		PublicChatRecord.HidePublicChatWindow();
		otherUserInfoPanel.HideOtherInfo();
	}
}
