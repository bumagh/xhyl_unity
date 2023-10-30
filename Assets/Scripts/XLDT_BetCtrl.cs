using LitJson;
using STDT_GameConfig;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class XLDT_BetCtrl : MonoBehaviour
{
	private static int S_nMaxItemNum = 5;

	protected XLDT_BetItem[] mCurBetArr = new XLDT_BetItem[S_nMaxItemNum];

	protected XLDT_BetItem[] mLastBetArr = new XLDT_BetItem[S_nMaxItemNum];

	protected XLDT_BetItem[] mLastValidBetArr = new XLDT_BetItem[S_nMaxItemNum];

	protected XLDT_BetItem[] mMingPaiBetArr = new XLDT_BetItem[S_nMaxItemNum];

	protected XLDT_TableInfo mTableInfo;

	protected int mMaxAllowedBet;

	protected XLDT_BET_PRIZE_TYPE mPrizeType;

	protected XLDT_BET_PRIZE_TYPE mLastPrizeType;

	protected XLDT_POKER_COLOR mResultColor;

	protected int mXianHong;

	protected Button mClearBtn;

	protected Button mRebetBtn;

	protected Button mSwitchPowerBtn;

	protected Button mAutoBtn;

	protected Button[] mBtnBetItem = new Button[S_nMaxItemNum];

	protected GameObject mBetEnableObj;

	protected GameObject mBetTip;

	protected Text txtCurScore;

	protected Text txtTotalScore;

	protected Text[] mMyBetNum = new Text[S_nMaxItemNum];

	protected Text[] mTotalBetNum = new Text[S_nMaxItemNum];

	protected Text[] mBetCount = new Text[S_nMaxItemNum];

	protected Text[] mBetPower = new Text[S_nMaxItemNum];

	private XLDT_TipAnim sptTA;

	private Text mTxtSwitchBtn;

	protected int mBetPowerType;

	private static float[][] mPowerArray = new float[4][]
	{
		new float[5]
		{
			3.8f,
			3.8f,
			4f,
			4f,
			20f
		},
		new float[5]
		{
			2f,
			4f,
			6f,
			12f,
			24f
		},
		new float[5]
		{
			2f,
			4f,
			8f,
			8f,
			24f
		},
		new float[5]
		{
			3f,
			3f,
			6f,
			6f,
			24f
		}
	};

	protected int[] mChipValue = new int[3]
	{
		5,
		10,
		100
	};

	protected int mCurrentChip = 5;

	protected int mChipIndex;

	protected int mCurScore;

	protected int mTotalScore;

	protected bool mBetEnable;

	protected bool mIsAutoBet;

	protected int mAutoCount;

	private bool bDingFen;

	private bool bXuYaBnEnbled = true;

	public Sprite[] spiAutoBtn;

	public Sprite[] spiBtnRebet;

	public Sprite[] spiBtnClear;

	public Sprite[] spiScore;

	public Sprite[] spiTotalScore;

	private XLDT_BtnLongPress[] btnLongPress = new XLDT_BtnLongPress[S_nMaxItemNum];

	private Color tempColor = new Color(25f / 32f, 25f / 32f, 25f / 32f);

	public XLDT_TableInfo TableInfo
	{
		get
		{
			return mTableInfo;
		}
		set
		{
			mTableInfo = value;
			mXianHong = mTableInfo.Restrain;
			UpdateChip(mTableInfo);
		}
	}

	public int CurScore
	{
		get
		{
			return mCurScore;
		}
		set
		{
			mCurScore = value;
			if (txtCurScore == null)
			{
				txtCurScore = base.transform.Find("CurScore/Txt").GetComponent<Text>();
			}
			txtCurScore.text = XLDT_DanTiaoCommon.ChangeNumber(mCurScore);
		}
	}

	public int TotalScore
	{
		get
		{
			return mTotalScore;
		}
		set
		{
			mTotalScore = value;
			if (txtTotalScore == null)
			{
				txtTotalScore = base.transform.Find("TotalScore/Txt").GetComponent<Text>();
			}
			txtTotalScore.text = XLDT_DanTiaoCommon.ChangeNumber(value);
		}
	}

	public int CurrentChip
	{
		get
		{
			return mCurrentChip;
		}
		set
		{
			mCurrentChip = value;
			if (!mTxtSwitchBtn)
			{
				mTxtSwitchBtn = mSwitchPowerBtn.transform.Find("Text").GetComponent<Text>();
			}
			mTxtSwitchBtn.text = XLDT_Localization.Get("PanelBetSwitch") + mCurrentChip.ToString();
		}
	}

	public bool BetEnable
	{
		get
		{
			return mBetEnable;
		}
		set
		{
			mBetEnable = value;
			mBetEnableObj.SetActive(!value);
			SetFuncBtnEnable(value);
		}
	}

	public bool IsAutoBet
	{
		get
		{
			return mIsAutoBet;
		}
		set
		{
			mIsAutoBet = value;
			if (mIsAutoBet)
			{
				mAutoBtn.image.sprite = spiAutoBtn[XLDT_Localization.language * 2 + 1];
			}
			else
			{
				mAutoBtn.image.sprite = spiAutoBtn[XLDT_Localization.language * 2];
			}
			if (!mIsAutoBet)
			{
				mAutoCount = 0;
			}
		}
	}

	public bool IsAutoMoreThan3 => mAutoCount >= 2;

	private void Awake()
	{
		for (int i = 0; i < mCurBetArr.Length; i++)
		{
			mCurBetArr[i] = new XLDT_BetItem();
			mLastBetArr[i] = new XLDT_BetItem();
			mLastValidBetArr[i] = new XLDT_BetItem();
			mMingPaiBetArr[i] = new XLDT_BetItem();
		}
		mBetEnableObj = base.transform.Find("BtnBetEnable").gameObject;
		mBetTip = base.transform.Find("BetTip").gameObject;
		sptTA = mBetTip.GetComponent<XLDT_TipAnim>();
		mBetEnableObj.GetComponent<Button>().onClick.AddListener(OnEnableBtnClick);
		mClearBtn = base.transform.Find("BtnClear").GetComponent<Button>();
		mRebetBtn = base.transform.Find("BtnRebet").GetComponent<Button>();
		mSwitchPowerBtn = base.transform.Find("BtnSwitchBet").GetComponent<Button>();
		mAutoBtn = base.transform.Find("BtnAuto").GetComponent<Button>();
		mTxtSwitchBtn = mSwitchPowerBtn.transform.Find("Text").GetComponent<Text>();
		mClearBtn.onClick.AddListener(delegate
		{
			OnFuncBtnsClick(mClearBtn.gameObject);
		});
		mRebetBtn.onClick.AddListener(delegate
		{
			OnFuncBtnsClick(mRebetBtn.gameObject);
		});
		mSwitchPowerBtn.onClick.AddListener(delegate
		{
			OnFuncBtnsClick(mSwitchPowerBtn.gameObject);
		});
		mAutoBtn.onClick.AddListener(delegate
		{
			OnFuncBtnsClick(mAutoBtn.gameObject);
		});
		for (int j = 0; j < S_nMaxItemNum; j++)
		{
			mBtnBetItem[j] = base.transform.Find($"BtnBetItem{j}").GetComponent<Button>();
			btnLongPress[j] = mBtnBetItem[j].GetComponent<XLDT_BtnLongPress>();
			mMyBetNum[j] = mBtnBetItem[j].transform.Find("TxtMyBet").GetComponent<Text>();
			mTotalBetNum[j] = mBtnBetItem[j].transform.Find("TxtTotalBet").GetComponent<Text>();
			mBetCount[j] = mBtnBetItem[j].transform.Find("TxtBetCount").GetComponent<Text>();
			mBetPower[j] = mBtnBetItem[j].transform.Find("TxtPowerType").GetComponent<Text>();
			SetBtnValue((XLDT_POKER_COLOR)j, 0, 0, 0);
			int index = j;
			mBtnBetItem[j].onClick.AddListener(delegate
			{
				OnBetBtnClick(index);
			});
			btnLongPress[j].action = delegate
			{
				OnBetBtnClick(index);
			};
		}
		SetBetPower(0);
	}

	private IEnumerator Start()
	{
		SetLanguage();
		CurrentChip = mChipValue[0];
		SetFuncBtnEnable(isEnable: false);
		SetBetPower(XLDT_GameInfo.getInstance().BetType - 1);
		yield return new WaitForEndOfFrame();
		TableInfo = XLDT_GameInfo.getInstance().CurTable;
	}

	private void SetLanguage()
	{
		int language = XLDT_Localization.language;
		mClearBtn.image.sprite = spiBtnClear[language];
		mRebetBtn.image.sprite = spiBtnRebet[language];
		base.transform.Find("CurScore/Img").GetComponent<Image>().sprite = spiScore[language];
		base.transform.Find("CurScore/Img").GetComponent<Image>().SetNativeSize();
		base.transform.Find("TotalScore/Img").GetComponent<Image>().sprite = spiTotalScore[language];
		base.transform.Find("TotalScore/Img").GetComponent<Image>().SetNativeSize();
		for (int i = 0; i < mBtnBetItem.Length; i++)
		{
			mBtnBetItem[i].transform.Find("TxtMyBetTitle").GetComponent<Text>().text = ((language != 0) ? "My" : "我");
			mBtnBetItem[i].transform.Find("TxtTotalTitle").GetComponent<Text>().text = ((language != 0) ? "Total" : "总");
		}
	}

	public void Restart(XLDT_BET_PRIZE_TYPE prizeTyp = XLDT_BET_PRIZE_TYPE.PRIZE_NORMAL)
	{
		BetEnable = true;
		SetPrizeType(prizeTyp);
		XLDT_GameUIMngr.GetSingleton().mBetCtrl.CurScore = 0;
		SetBetPower(XLDT_GameInfo.getInstance().BetType - 1);
		for (int i = 0; i < mLastBetArr.Length; i++)
		{
			mLastBetArr[i].SetValue(mCurBetArr[i].fPower, mCurBetArr[i].nCount, mCurBetArr[i].nMy, mCurBetArr[i].nTotal);
		}
		if (_isCurBetValid())
		{
			if (mLastPrizeType != XLDT_BET_PRIZE_TYPE.PRIZE_NORMAL)
			{
				for (int j = 0; j < mMingPaiBetArr.Length; j++)
				{
					mMingPaiBetArr[j].nMy = mCurBetArr[j].nMy;
				}
				if (IsAutoBet)
				{
					bool flag = false;
					for (int k = 0; k < mLastValidBetArr.Length; k++)
					{
						if (mMingPaiBetArr[k].nMy != 0 && mMingPaiBetArr[k].nMy != mLastValidBetArr[k].nMy)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						for (int l = 0; l < mLastValidBetArr.Length; l++)
						{
							mLastValidBetArr[l].nMy = mMingPaiBetArr[l].nMy;
						}
					}
				}
				else
				{
					for (int m = 0; m < mLastValidBetArr.Length; m++)
					{
						mLastValidBetArr[m].SetValue(mCurBetArr[m].fPower, mCurBetArr[m].nCount, mCurBetArr[m].nMy, mCurBetArr[m].nTotal);
					}
				}
			}
			else
			{
				for (int n = 0; n < mLastValidBetArr.Length; n++)
				{
					mLastValidBetArr[n].SetValue(mCurBetArr[n].fPower, mCurBetArr[n].nCount, mCurBetArr[n].nMy, mCurBetArr[n].nTotal);
				}
			}
		}
		mMaxAllowedBet = _getMyLastTotalBet();
		TotalScore = XLDT_GameInfo.getInstance().User.ScoreCount;
		if (!IsAutoBet)
		{
			SetBetBtnMyArray(new int[5]);
			SetBetBtnTotalArray(new int[5]);
		}
		else
		{
			mAutoCount++;
		}
		SetFuncBtnEnable(isEnable: true);
		bDingFen = false;
	}

	private void SetFuncBtnEnable(bool isEnable)
	{
		if (isEnable)
		{
			if (IsAutoBet)
			{
				_btnEnable(mClearBtn, isEnable: false);
				_btnEnable(mAutoBtn, isEnable: true);
				_btnEnable(mRebetBtn, isEnable: false);
				return;
			}
			_btnEnable(mClearBtn, isEnable: false);
			_btnEnable(mRebetBtn, isEnable: false);
			_btnEnable(mAutoBtn, isEnable: false);
			if (_isLastBetValid() && !bDingFen && bXuYaBnEnbled)
			{
				_btnEnable(mRebetBtn, isEnable: true);
			}
		}
		else
		{
			_btnEnable(mClearBtn, isEnable: false);
			_btnEnable(mAutoBtn, isEnable: false);
			_btnEnable(mRebetBtn, isEnable: false);
		}
	}

	public void OnFuncBtnsClick(GameObject go)
	{
		switch (go.name)
		{
		case "BtnSwitchBet":
			mChipIndex = (mChipIndex + 1) % mChipValue.Length;
			CurrentChip = mChipValue[mChipIndex];
			break;
		case "BtnClear":
			ClearAllMyBet();
			break;
		case "BtnRebet":
			RebetBtnPress(1);
			break;
		case "BtnAuto":
			AuToBetBtnPress();
			break;
		}
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
	}

	public void ErrorResetAll()
	{
		for (int i = 0; i < mLastValidBetArr.Length; i++)
		{
			mLastValidBetArr[i].Reset();
			mLastBetArr[i].Reset();
			mMingPaiBetArr[i].Reset();
		}
		if (IsAutoBet)
		{
			IsAutoBet = false;
		}
		ClearAllMyBet(isTellSvr: false);
	}

	private void ClearAllMyBet(bool isTellSvr = true)
	{
		_btnEnable(mClearBtn, isEnable: false);
		_btnEnable(mAutoBtn, isEnable: false);
		if (_isLastBetValid())
		{
			_btnEnable(mRebetBtn, isEnable: true);
		}
		if (isTellSvr)
		{
			XLDT_NetMain.GetSingleton().MyCreateSocket.SendCancelBet(XLDT_GameInfo.getInstance().CurTable.Id);
		}
	}

	private void RebetBtnPress(int type)
	{
		XLDT_BetItem[] array = new XLDT_BetItem[S_nMaxItemNum];
		bool flag = true;
		for (int i = 0; i < array.Length; i++)
		{
			if (mCurBetArr[i].nMy != 0)
			{
				flag = false;
			}
		}
		if (mPrizeType == XLDT_BET_PRIZE_TYPE.PRIZE_NORMAL)
		{
			if (!flag)
			{
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = new XLDT_BetItem();
					array[j].nMy = mCurBetArr[j].nMy;
				}
			}
			else
			{
				for (int k = 0; k < array.Length; k++)
				{
					array[k] = new XLDT_BetItem();
					array[k].nMy = mLastValidBetArr[k].nMy;
				}
			}
		}
		else
		{
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = new XLDT_BetItem();
				array[l].nMy = 0;
			}
			if (!flag)
			{
				switch (mPrizeType)
				{
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_COLOR_RED:
					array[1].nMy = mCurBetArr[1].nMy;
					array[3].nMy = mCurBetArr[3].nMy;
					break;
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_COLOR_BLACK:
					array[0].nMy = mCurBetArr[0].nMy;
					array[2].nMy = mCurBetArr[2].nMy;
					break;
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_SPADE:
					array[0].nMy = mCurBetArr[0].nMy;
					break;
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_HEART:
					array[1].nMy = mCurBetArr[1].nMy;
					break;
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_CLUB:
					array[2].nMy = mCurBetArr[2].nMy;
					break;
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_DIAMOND:
					array[3].nMy = mCurBetArr[3].nMy;
					break;
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_PORKER_JOKER:
					array[4].nMy = mCurBetArr[4].nMy;
					break;
				}
			}
			else
			{
				switch (mPrizeType)
				{
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_COLOR_RED:
					array[1].nMy = mLastBetArr[1].nMy;
					array[3].nMy = mLastBetArr[3].nMy;
					break;
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_COLOR_BLACK:
					array[0].nMy = mLastBetArr[0].nMy;
					array[2].nMy = mLastBetArr[2].nMy;
					break;
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_SPADE:
					array[0].nMy = mLastBetArr[0].nMy;
					break;
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_HEART:
					array[1].nMy = mLastBetArr[1].nMy;
					break;
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_CLUB:
					array[2].nMy = mLastBetArr[2].nMy;
					break;
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_DIAMOND:
					array[3].nMy = mLastBetArr[3].nMy;
					break;
				case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_PORKER_JOKER:
					array[4].nMy = mLastBetArr[4].nMy;
					break;
				}
			}
		}
		int num = _getTotalBet(array);
		if (!XLDT_GameUIMngr.GetSingleton().IsUserScoreEnough(num))
		{
			SetBetTip(XLDT_Localization.Get("PanelBetTipPleaseCoinIn"));
			ClearAllMyBet();
			if (num > 0)
			{
				_btnEnable(mRebetBtn, isEnable: true);
			}
			else
			{
				_btnEnable(mRebetBtn, isEnable: false);
			}
			_btnEnable(mAutoBtn, isEnable: false);
			_btnEnable(mClearBtn, isEnable: false);
			IsAutoBet = false;
		}
		else
		{
			for (int m = 0; m < mLastBetArr.Length; m++)
			{
			}
			SendRebet(array, type);
			if (type == 1)
			{
				_btnEnable(mRebetBtn, isEnable: false);
			}
		}
	}

	private void AuToBetBtnPress()
	{
		IsAutoBet = !IsAutoBet;
		if (IsAutoBet)
		{
			_btnEnable(mClearBtn, isEnable: false);
		}
		else if (!bDingFen)
		{
			_btnEnable(mClearBtn, isEnable: true);
		}
	}

	private void OnBetBtnClick(int index)
	{
		if (!bDingFen && bXuYaBnEnbled)
		{
			bXuYaBnEnbled = true;
		}
		bool flag = false;
		int scoreCount = XLDT_GameInfo.getInstance().User.ScoreCount;
		int num = 0;
		if (mCurBetArr[index].nMy < XLDT_GameInfo.getInstance().CurTable.MinBet)
		{
			num = XLDT_GameInfo.getInstance().CurTable.MinBet;
		}
		num = ((num > CurrentChip) ? num : CurrentChip);
		int num2 = (scoreCount < num) ? scoreCount : num;
		int num3 = 99999;
		if (mCurBetArr[index].nMy + num2 > num3)
		{
			num2 = num3 - mCurBetArr[index].nMy;
		}
		if (scoreCount == 0)
		{
			TotalScore = scoreCount;
			SetBetTip(XLDT_Localization.Get("PanelBetTipPleaseCoinIn"));
			XLDT_GameUIMngr.GetSingleton().ShowMoneyInOutDlg(isShow: true);
			return;
		}
		switch (mPrizeType)
		{
		case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_COLOR_RED:
			if ((index == 1 && (_isLastBet(XLDT_POKER_COLOR.POKER_HEART) || _isLastBet(XLDT_POKER_COLOR.POKER_DIAMOND))) || (index == 3 && (_isLastBet(XLDT_POKER_COLOR.POKER_HEART) || _isLastBet(XLDT_POKER_COLOR.POKER_DIAMOND))))
			{
				flag = true;
			}
			break;
		case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_COLOR_BLACK:
			if ((index == 0 && (_isLastBet(XLDT_POKER_COLOR.POKER_SPADE) || _isLastBet(XLDT_POKER_COLOR.POKER_CLUB))) || (index == 2 && (_isLastBet(XLDT_POKER_COLOR.POKER_SPADE) || _isLastBet(XLDT_POKER_COLOR.POKER_CLUB))))
			{
				flag = true;
			}
			break;
		case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_SPADE:
			if (index == 0 && _isLastBet((XLDT_POKER_COLOR)index))
			{
				flag = true;
			}
			break;
		case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_HEART:
			if (index == 1 && _isLastBet((XLDT_POKER_COLOR)index))
			{
				flag = true;
			}
			break;
		case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_CLUB:
			if (index == 2 && _isLastBet((XLDT_POKER_COLOR)index))
			{
				flag = true;
			}
			break;
		case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_POKER_DIAMOND:
			if (index == 3 && _isLastBet((XLDT_POKER_COLOR)index))
			{
				flag = true;
			}
			break;
		case XLDT_BET_PRIZE_TYPE.KNOWN_PRIZE_PORKER_JOKER:
			if (index == 4 && _isLastBet((XLDT_POKER_COLOR)index))
			{
				flag = true;
			}
			break;
		case XLDT_BET_PRIZE_TYPE.PRIZE_NORMAL:
			flag = true;
			break;
		}
		if (flag)
		{
			if (mPrizeType == XLDT_BET_PRIZE_TYPE.PRIZE_NORMAL)
			{
				XLDT_NetMain.GetSingleton().MyCreateSocket.SendUserBet(index, num2, XLDT_GameInfo.getInstance().User.SeatIndex);
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
			}
			else if (!_isExceedXianHong(num2, mCurBetArr[index].nMy, mPowerArray[mBetPowerType][index]))
			{
				XLDT_NetMain.GetSingleton().MyCreateSocket.SendUserBet(index, num2, XLDT_GameInfo.getInstance().User.SeatIndex);
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.common);
			}
			else
			{
				SetBetTip(XLDT_Localization.Get("PanelBetTipXianHong"));
			}
		}
		else
		{
			SetBetTip(XLDT_Localization.Get("PanelBetTipKnownPrize"));
		}
		TotalScore = XLDT_GameInfo.getInstance().User.ScoreCount;
	}

	private void OnEnableBtnClick()
	{
		SetBetTip(XLDT_Localization.Get("PanelBetTipWaitForBegin"));
	}

	public void SetBetTip(string words)
	{
		if (words == "请取分" || words == "Please Coin In")
		{
			XLDT_GameUIMngr.GetSingleton().ShowMoneyInOutDlg();
			return;
		}
		sptTA.Play(words);
		XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.error);
	}

	public void SetBtnValue(XLDT_POKER_COLOR pokerColor, int count, int my, int total)
	{
		mCurBetArr[(int)pokerColor].nCount = count;
		mCurBetArr[(int)pokerColor].nMy = my;
		mCurBetArr[(int)pokerColor].nTotal = total;
		mBetCount[(int)pokerColor].text = count.ToString();
		mMyBetNum[(int)pokerColor].text = my.ToString();
		mTotalBetNum[(int)pokerColor].text = total.ToString();
	}

	public void SetBetBtnMyArray(int[] my)
	{
		bool flag = true;
		for (int i = 0; i < my.Length; i++)
		{
			SetBetBtnMy((XLDT_POKER_COLOR)i, my[i]);
			if (my[i] != 0)
			{
				flag = false;
			}
		}
		if (!flag)
		{
			return;
		}
		IsAutoBet = false;
		if (BetEnable)
		{
			_btnEnable(mClearBtn, isEnable: false);
			_btnEnable(mAutoBtn, isEnable: false);
			if (_isLastBetValid())
			{
				_btnEnable(mRebetBtn, isEnable: true);
			}
		}
	}

	public void SetBetBtnTotalArray(int[] total)
	{
		for (int i = 0; i < total.Length; i++)
		{
			SetBetBtnTotal((XLDT_POKER_COLOR)i, total[i]);
		}
	}

	public static string LogArray<T>(T[] arr)
	{
		string text = "[";
		for (int i = 0; i < arr.Length; i++)
		{
			text = text + arr[i].ToString() + ((i == arr.Length - 1) ? "]" : ", ");
		}
		return text;
	}

	public void SetBetBtnCountArray(int[] count)
	{
		for (int i = 0; i < count.Length; i++)
		{
			SetBetBtnCount((XLDT_POKER_COLOR)i, count[i]);
		}
	}

	public void SetCancelYaFenBtnEnable(int type)
	{
		switch (type)
		{
		case 0:
			SetBetTip(XLDT_Localization.Get("PanelBetTipXianHong"));
			break;
		case 1:
			bDingFen = true;
			bXuYaBnEnbled = false;
			_btnEnable(mClearBtn, isEnable: false);
			SetBetTip(XLDT_Localization.Get("Dingfen"));
			break;
		}
	}

	public void SetBetBtnMy(XLDT_POKER_COLOR pokerColor, int my, bool isServer = true)
	{
		mCurBetArr[(int)pokerColor].nMy = my;
		mMyBetNum[(int)pokerColor].text = my.ToString();
		if (isServer && mCurBetArr[(int)pokerColor].nMy > 0 && !IsAutoBet)
		{
			_btnEnable(mAutoBtn, isEnable: true);
			if (!bDingFen)
			{
				_btnEnable(mClearBtn, isEnable: true);
			}
			_btnEnable(mRebetBtn, isEnable: false);
			if (!BetEnable)
			{
				SetFuncBtnEnable(isEnable: false);
			}
		}
	}

	public void SetBetBtnTotal(XLDT_POKER_COLOR pokerColor, int total)
	{
		mCurBetArr[(int)pokerColor].nTotal = total;
		mTotalBetNum[(int)pokerColor].text = total.ToString();
	}

	public void SetBetBtnCount(XLDT_POKER_COLOR pokerColor, int count)
	{
		mCurBetArr[(int)pokerColor].nCount = count;
		mBetCount[(int)pokerColor].text = count.ToString();
	}

	public void StatisRecord(XLDT_CardAlgorithmResult[] record)
	{
		int[] array = new int[5];
		for (int i = 0; i < record.Length; i++)
		{
			int color = record[i].color;
			int point = record[i].point;
			if (record[i].point < 14)
			{
				if (color < 5 && color >= 0)
				{
					array[color]++;
				}
			}
			else if (record[i].point == 14 || record[i].point == 15)
			{
				array[4]++;
			}
		}
		SetBetBtnCountArray(array);
	}

	public void SetBetPower(int nType)
	{
		mBetPowerType = nType;
		if (nType > S_nMaxItemNum - 1 || nType < 0)
		{
		}
		for (int i = 0; i < mBetPower.Length; i++)
		{
			mBetPower[i].text = mPowerArray[nType][i].ToString();
		}
	}

	public void SetChip(int[] chips)
	{
		UnityEngine.Debug.LogError("===============SetChip===========");
		UnityEngine.Debug.LogError("========== " + JsonMapper.ToJson(chips));
		mChipValue = chips;
		mChipIndex = 0;
		CurrentChip = mChipValue[0];
	}

	public void UpdateChip(XLDT_TableInfo table)
	{
		int baseYaFen = table.BaseYaFen;
		UnityEngine.Debug.LogError("==============baseYaFen: " + baseYaFen);
	}

	public void SetPrizeType(XLDT_BET_PRIZE_TYPE prize)
	{
		mLastPrizeType = mPrizeType;
		mPrizeType = prize;
	}

	public void SetResultColor(XLDT_POKER_COLOR color)
	{
		mResultColor = color;
		StopCoroutine("ShineResultColor");
		for (int i = 0; i < mBtnBetItem.Length; i++)
		{
			ColorBlock colors = mBtnBetItem[i].colors;
			colors.normalColor = Color.white;
			mBtnBetItem[i].colors = colors;
		}
		StartCoroutine("ShineResultColor", color);
	}

	public void SendRebet(XLDT_BetItem[] bet, int type)
	{
		XLDT_UserBets xLDT_UserBets = new XLDT_UserBets();
		xLDT_UserBets.nUserBets = new int[5];
		for (int i = 0; i < bet.Length; i++)
		{
			xLDT_UserBets.nUserBets[i] = bet[i].nMy;
		}
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendContinueBet(XLDT_GameInfo.getInstance().CurTable.Id, xLDT_UserBets, type);
	}

	private IEnumerator ShineResultColor(object color)
	{
		XLDT_POKER_COLOR col = (XLDT_POKER_COLOR)(int)color;
		Button btn = mBtnBetItem[(int)col];
		ColorBlock cb = btn.colors;
		for (int i = 0; i < 20; i++)
		{
			cb.normalColor = tempColor;
			btn.colors = cb;
			yield return new WaitForSeconds(0.25f);
			cb.normalColor = Color.white;
			btn.colors = cb;
			yield return new WaitForSeconds(0.25f);
		}
		cb.normalColor = Color.white;
		btn.colors = cb;
	}

	private void _btnEnable(Button btn, bool isEnable)
	{
		btn.interactable = isEnable;
	}

	private int _getMyLastTotalBet()
	{
		return _getTotalBet(mLastBetArr);
	}

	private int _getMyCurTotalBet()
	{
		return _getTotalBet(mCurBetArr);
	}

	private int _getMyLastValidTotalBet()
	{
		return _getTotalBet(mLastValidBetArr);
	}

	private int _getTotalBet(XLDT_BetItem[] betArr)
	{
		int num = 0;
		for (int i = 0; i < betArr.Length; i++)
		{
			num += betArr[i].nMy;
		}
		return num;
	}

	private bool _isLastBet(XLDT_POKER_COLOR color)
	{
		return mLastBetArr[(int)color].nMy > 0;
	}

	private bool _isExceedMaxBetNum(int nBet)
	{
		return nBet + _getMyCurTotalBet() > mMaxAllowedBet;
	}

	private bool _isExceedXianHong(int nBet, int curBet, float nPower)
	{
		float num = (float)(nBet + curBet) * nPower - ((float)_getMyCurTotalBet() + (float)nBet);
		return num > (float)mXianHong;
	}

	private bool _isCurBetValid()
	{
		bool result = false;
		for (int i = 0; i < mCurBetArr.Length; i++)
		{
			if (mCurBetArr[i].nMy > 0)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private bool _isLastBetValid()
	{
		bool result = false;
		for (int i = 0; i < mLastValidBetArr.Length; i++)
		{
			if (mLastValidBetArr[i].nMy > 0)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private void Update()
	{
	}

	public void SendContinueBet()
	{
		if (IsAutoBet)
		{
			RebetBtnPress(0);
		}
	}
}
