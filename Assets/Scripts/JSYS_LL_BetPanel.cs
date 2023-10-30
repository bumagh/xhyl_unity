using UnityEngine;

public class JSYS_LL_BetPanel : MonoBehaviour
{
	protected JSYS_LL_HudManager mHudMgr = new JSYS_LL_HudManager();

	protected UIPanel mBetPanel;

	protected UISprite mBetPanelBtnSprite;

	protected Collider mBackgroundCol;

	protected bool mIsBetVisable;

	protected Transform mChipBtn;

	protected Collider[] mChipCol = new Collider[3];

	protected Collider[] mBetBtnCol = new Collider[15];

	protected Collider mAutoBetBtnCol;

	protected Collider mXuyaBtnCol;

	protected Collider mCancelBetBtnCol;

	protected UISprite mAutoBetBtnBG;

	protected UISprite mXuyaBtnBG;

	protected UISprite mCancelBetBtnBG;

	protected GameObject mBetFrameLightObj;

	protected bool mIsFrameLight;

	protected float mTotalTime;

	protected JSYS_LL_TableInfo mTableInfo;

	protected JSYS_LL_PersonInfo mUser;

	protected int mCurrentTableID;

	protected int mCurrentChip = 1;

	protected JSYS_LL_UICheckbox[] mChipOption = new JSYS_LL_UICheckbox[3];

	protected int[] mChipValue = new int[3]
	{
		1,
		5,
		10
	};

	protected UILabel[] mChipText = new UILabel[3];

	protected int[] mAnimalPower = new int[12];

	protected UILabel[] mPower = new UILabel[15];

	protected UILabel[] mPersonBetText = new UILabel[15];

	protected UILabel[] mTotalBetText = new UILabel[15];

	protected GameObject[] mBetItem = new GameObject[15];

	protected bool[] mIsBetZero = new bool[15]
	{
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true,
		true
	};

	protected int[] mCurPersonBet = new int[15];

	protected int[] mLastOnceBet = new int[15];

	protected int[] mLastValidPersonBet = new int[15];

	protected UILabel mAutoBtnLabel;

	protected bool mIsAutoBet;

	protected bool mIsDeskSettingChanged;

	protected bool mIsScorePoor;

	protected bool mIsAllzero = true;

	private bool[] bPressed = new bool[15];

	private float[] timer = new float[15];

	public int CurrentChip
	{
		get
		{
			return mCurrentChip;
		}
		set
		{
			mCurrentChip = value;
		}
	}

	public int[] AnimalPower => mAnimalPower;

	public bool IsAutoBet => mIsAutoBet;

	private void Awake()
	{
		mBetPanel = GetComponent<UIPanel>();
		mChipBtn = base.transform.Find("ChipBtn");
		mBackgroundCol = base.transform.Find("Background").GetComponent<Collider>();
		mAutoBetBtnCol = base.transform.Find("AutoBetBtn").GetComponent<Collider>();
		mXuyaBtnCol = base.transform.Find("XuyaBtn").GetComponent<Collider>();
		mCancelBetBtnCol = base.transform.Find("CancelBetBtn").GetComponent<Collider>();
		mAutoBetBtnBG = base.transform.Find("AutoBetBtn").Find("Background").GetComponent<UISprite>();
		mXuyaBtnBG = base.transform.Find("XuyaBtn").Find("Background").GetComponent<UISprite>();
		mCancelBetBtnBG = base.transform.Find("CancelBetBtn").Find("Background").GetComponent<UISprite>();
		mAutoBtnLabel = base.transform.Find("AutoBetBtn").Find("Label").GetComponent<UILabel>();
		mBetFrameLightObj = base.transform.Find("BetFrameLight").gameObject;
		mBetFrameLightObj.SetActiveRecursively(state: false);
		for (int i = 0; i < 15; i++)
		{
			bPressed[i] = false;
			timer[i] = 0f;
			mBetItem[i] = base.transform.Find("BetButton" + i).gameObject;
			mBetBtnCol[i] = mBetItem[i].transform.GetComponent<Collider>();
			mPersonBetText[i] = mBetItem[i].transform.Find("PersonBet").GetComponent<UILabel>();
			mTotalBetText[i] = mBetItem[i].transform.Find("TotalBet").GetComponent<UILabel>();
			mPower[i] = mBetItem[i].transform.Find("Power").GetComponent<UILabel>();
			if (i < 3)
			{
				mChipCol[i] = mChipBtn.Find("Chip" + i).GetComponent<Collider>();
				mChipOption[i] = mChipBtn.Find("Chip" + i).GetComponent<JSYS_LL_UICheckbox>();
				mChipText[i] = mChipBtn.Find("Chip" + i).Find("Label").GetComponent<UILabel>();
			}
		}
		mUser = JSYS_LL_GameInfo.getInstance().UserInfo;
		HideBetPanel();
		ClearAllBet();
		_setFuncBtnEnabled(bIsEnabled: false, 1);
		_setFuncBtnEnabled(bIsEnabled: false, 2);
	}

	private void Update()
	{
		if (mIsFrameLight)
		{
			mTotalTime += Time.deltaTime;
			if (mTotalTime >= 0.2f)
			{
				mIsFrameLight = false;
				mBetFrameLightObj.SetActiveRecursively(state: false);
			}
		}
		for (int i = 0; i < 15; i++)
		{
			if (bPressed[i])
			{
				timer[i] += Time.deltaTime;
				if (timer[i] >= 0.2f)
				{
					timer[i] = 0f;
					_onClickBetItem(mBetItem[i]);
				}
			}
		}
	}

	public void SetPanelVisable()
	{
		if (mIsBetVisable)
		{
			HideBetPanel();
		}
		else
		{
			ShowBetPanel();
		}
		if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
		{
			JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		}
	}

	public void ShowBetPanel(bool bIsMove = true)
	{
		mIsBetVisable = true;
		_setColliderActive(bIsActive: true);
		mBetPanelBtnSprite.spriteName = ((JSYS_LL_GameInfo.getInstance().Language == 0) ? "shouQi" : "shouQi_e");
		mBetPanelBtnSprite.MakePixelPerfect();
		mBetPanel.enabled = true;
		if (bIsMove)
		{
			TweenPosition.Begin(base.transform.gameObject, 0.5f, new Vector3(0f, 0f, -50f));
		}
		_onChangeChip();
	}

	public void HideBetPanel(bool bIsMove = true)
	{
		mIsBetVisable = false;
		_setColliderActive(bIsActive: false);
		if (bIsMove)
		{
			TweenPosition.Begin(base.transform.gameObject, 0.5f, new Vector3(0f, 730f, -50f));
			return;
		}
		mBetPanel.enabled = false;
		base.transform.localPosition = new Vector3(0f, 730f, -50f);
	}

	protected void _setColliderActive(bool bIsActive)
	{
		mBackgroundCol.enabled = bIsActive;
		for (int i = 0; i < 15; i++)
		{
			mBetBtnCol[i].enabled = bIsActive;
			if (i < 3)
			{
				mChipCol[i].enabled = bIsActive;
			}
		}
		if (bIsActive)
		{
			if (mIsAutoBet)
			{
				_setFuncBtnEnabled(bIsEnabled: false, 0);
				_setFuncBtnEnabled(bIsEnabled: false, 1);
				_setFuncBtnEnabled(bIsEnabled: true, 2);
				return;
			}
			_setFuncBtnEnabled(bIsEnabled: false, 0);
			_setFuncBtnEnabled(bIsEnabled: false, 1);
			_setFuncBtnEnabled(bIsEnabled: false, 2);
			int j;
			for (j = 0; j < 15 && mLastValidPersonBet[j] == 0; j++)
			{
			}
			if (mHudMgr.TimeCD > 0 && !mIsDeskSettingChanged)
			{
				if (mIsAllzero && j < 15)
				{
					_setFuncBtnEnabled(bIsEnabled: true, 1);
				}
				else if (!mIsAllzero)
				{
					_setFuncBtnEnabled(bIsEnabled: true, 0);
					_setFuncBtnEnabled(bIsEnabled: true, 2);
				}
			}
		}
		else
		{
			_setFuncBtnEnabled(bIsEnabled: false, 0);
			_setFuncBtnEnabled(bIsEnabled: false, 1);
			_setFuncBtnEnabled(bIsEnabled: false, 2);
		}
	}

	public int SetAnimalPower(int[] nPower, int nLength = 15)
	{
		if (nLength > 15 || nLength <= 0)
		{
			return 1;
		}
		for (int i = 0; i < nPower.Length; i++)
		{
			if (i < 12)
			{
				mAnimalPower[i] = nPower[i];
			}
			mPower[i].text = nPower[i].ToString();
		}
		return 0;
	}

	public int SetAnimalBet(int[] nNum, bool bIsTotal, bool isLastBet = true)
	{
		if (bIsTotal)
		{
			if (nNum.Length < 15)
			{
				for (int i = 0; i < nNum.Length; i++)
				{
					mTotalBetText[i].text = nNum[i].ToString();
				}
				for (int j = nNum.Length; j < 15; j++)
				{
					mTotalBetText[j].text = "0";
				}
			}
			else
			{
				for (int k = 0; k < 15; k++)
				{
					mTotalBetText[k].text = nNum[k].ToString();
				}
			}
		}
		else
		{
			if (nNum.Length < 15)
			{
				for (int l = 0; l < nNum.Length; l++)
				{
					mCurPersonBet[l] = nNum[l];
				}
				for (int m = nNum.Length; m < 15; m++)
				{
					mCurPersonBet[m] = 0;
				}
			}
			else
			{
				for (int n = 0; n < 15; n++)
				{
					mCurPersonBet[n] = nNum[n];
				}
			}
			for (int num = 0; num < 15; num++)
			{
				mPersonBetText[num].text = mCurPersonBet[num].ToString();
			}
			if (isLastBet)
			{
				int num2;
				for (num2 = 0; num2 < nNum.Length - 1 && nNum[num2] <= 0; num2++)
				{
				}
				if (nNum[nNum.Length - 1] <= 0)
				{
					num2++;
				}
				if (num2 < nNum.Length && num2 < 15)
				{
					if (nNum.Length < 15)
					{
						for (int num3 = 0; num3 < nNum.Length; num3++)
						{
							mLastValidPersonBet[num3] = nNum[num3];
						}
						for (int num4 = nNum.Length; num4 < 15; num4++)
						{
							mLastValidPersonBet[num4] = 0;
						}
					}
					else
					{
						for (int num5 = 0; num5 < 15; num5++)
						{
							mLastValidPersonBet[num5] = nNum[num5];
						}
					}
				}
			}
		}
		return 0;
	}

	public void ClearAllBet()
	{
		if (!mIsAutoBet)
		{
			_setFuncBtnEnabled(bIsEnabled: false, 2);
		}
		for (int i = 0; i < 15; i++)
		{
			mCurPersonBet[i] = 0;
			mLastOnceBet[i] = 0;
			mIsBetZero[i] = true;
			mPersonBetText[i].text = "0";
			mTotalBetText[i].text = "0";
		}
		mIsAllzero = true;
	}

	public void ModifyChip(JSYS_LL_TableInfo tableInfo)
	{
		mCurrentTableID = mUser.TableId;
		if (tableInfo != null)
		{
			mTableInfo = tableInfo;
			if (tableInfo.MaxBet == 50)
			{
				mChipValue[0] = 1;
				mChipValue[1] = 5;
				mChipValue[2] = 10;
			}
			else if (tableInfo.MaxBet >= 100 && tableInfo.MaxBet <= 500)
			{
				mChipValue[0] = 10;
				mChipValue[1] = 50;
				mChipValue[2] = 100;
			}
			else if (tableInfo.MaxBet >= 600 && tableInfo.MaxBet <= 1500)
			{
				mChipValue[0] = 50;
				mChipValue[1] = 100;
				mChipValue[2] = 500;
			}
		}
		for (int i = 0; i < 3; i++)
		{
			if ((bool)mChipText[i])
			{
				mChipText[i].text = mChipValue[i].ToString();
			}
		}
	}

	public void AutoBet()
	{
		if (mHudMgr.TimeCD <= 0)
		{
			return;
		}
		if (_isAllBetSuccess())
		{
			if (!JSYS_LL_MyTest.TEST)
			{
				for (int i = 0; i < 15; i++)
				{
					JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendUserBet(i, mLastOnceBet[i], mCurrentTableID);
				}
				mIsAllzero = false;
			}
		}
		else
		{
			mIsAutoBet = false;
			mAutoBtnLabel.text = ((JSYS_LL_GameInfo.getInstance().Language == 0) ? "自动" : "Auto");
		}
	}

	public void Restart()
	{
		mIsAutoBet = false;
		mAutoBtnLabel.text = ((JSYS_LL_GameInfo.getInstance().Language == 0) ? "自动" : "Auto");
		_setFuncBtnEnabled(bIsEnabled: true, 0);
		_setFuncBtnEnabled(bIsEnabled: true, 1);
		_setFuncBtnEnabled(bIsEnabled: false, 2);
		for (int i = 0; i < 15; i++)
		{
			mCurPersonBet[i] = 0;
			mLastOnceBet[i] = 0;
			mLastValidPersonBet[i] = 0;
			mIsBetZero[i] = true;
			mPersonBetText[i].text = "0";
			mTotalBetText[i].text = "0";
			if (i < 12)
			{
				mPower[i].text = "0";
			}
		}
		mIsDeskSettingChanged = false;
		mIsScorePoor = false;
		mIsAllzero = true;
		HideBetPanel(bIsMove: false);
	}

	public void _onChangeChip()
	{
		int num = 0;
		while (true)
		{
			if (num < 3)
			{
				if (mChipOption[num].isChecked)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		mCurrentChip = mChipValue[num];
	}

	public void _onClickBetItem(GameObject sender)
	{
		if (mHudMgr.TimeCD <= 0)
		{
			return;
		}
		int num = int.Parse(sender.name.Substring(9));
		if (num > 14 || num < 0)
		{
			return;
		}
		int gameScore = mUser.GameScore;
		int nMinBet = 0;
		int nMaxBet = 0;
		if (mTableInfo != null)
		{
			nMinBet = ((num > 11) ? mTableInfo.MinZHXBet : mTableInfo.MinBet);
			nMaxBet = _getMaxMinBet(num, isMax: true);
		}
		if (gameScore <= 0)
		{
			mHudMgr.ShowScoreTip(bIsInHud: false);
			if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
			{
				JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound(JSYS_LL_LuckyLion_SoundManager.EUIBtnSoundType.BetFail);
			}
			return;
		}
		mLastOnceBet[num] = _calculateOnceBet(ref mIsBetZero[num], gameScore, nMinBet, nMaxBet, num);
		if (mLastOnceBet[num] > 0)
		{
			mIsDeskSettingChanged = false;
			mIsScorePoor = false;
			mIsAllzero = false;
			if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
			{
				JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound(JSYS_LL_LuckyLion_SoundManager.EUIBtnSoundType.BetSuccess);
			}
			mBetFrameLightObj.SetActiveRecursively(state: true);
			mBetFrameLightObj.transform.localPosition = new Vector3(-390f + (float)(num / 3) * 195f, 165f - (float)(num % 3) * 150f, -5f);
			mIsFrameLight = true;
			mTotalTime = 0f;
			mCurPersonBet[num] += mLastOnceBet[num];
			mPersonBetText[num].text = mCurPersonBet[num].ToString();
			if (!IsAutoBet)
			{
				_setFuncBtnEnabled(bIsEnabled: true, 0);
				_setFuncBtnEnabled(bIsEnabled: true, 2);
			}
			_setFuncBtnEnabled(bIsEnabled: false, 1);
			if (!JSYS_LL_MyTest.TEST)
			{
				JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendUserBet(num, mLastOnceBet[num], mCurrentTableID);
			}
		}
		else if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
		{
			JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound(JSYS_LL_LuckyLion_SoundManager.EUIBtnSoundType.BetFail);
		}
	}

	public void _onClickCancelBtn()
	{
		if (mHudMgr.TimeCD > 0)
		{
			mIsAllzero = true;
			if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
			{
				JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
			}
			for (int i = 0; i < 15; i++)
			{
				mCurPersonBet[i] = 0;
				mIsBetZero[i] = true;
				mPersonBetText[i].text = "0";
			}
			if (!JSYS_LL_MyTest.TEST)
			{
				JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendCancelBet(mCurrentTableID);
			}
			_setFuncBtnEnabled(bIsEnabled: false, 0);
			_setFuncBtnEnabled(bIsEnabled: false, 2);
			int j;
			for (j = 0; j < 15 && mLastValidPersonBet[j] == 0; j++)
			{
			}
			if (j < 15)
			{
				_setFuncBtnEnabled(bIsEnabled: true, 1);
			}
		}
	}

	public void _onClickXuyaBtn()
	{
		if (mHudMgr.TimeCD <= 0)
		{
			return;
		}
		if (_isAllBetSuccess())
		{
			if (!JSYS_LL_MyTest.TEST)
			{
				for (int i = 0; i < 15; i++)
				{
					JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendUserBet(i, mLastOnceBet[i], mCurrentTableID);
				}
				mIsAllzero = false;
			}
			_setFuncBtnEnabled(bIsEnabled: false, 1);
			_setFuncBtnEnabled(bIsEnabled: true, 0);
			_setFuncBtnEnabled(bIsEnabled: true, 2);
			if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
			{
				JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
			}
		}
		else if (mIsScorePoor)
		{
			mHudMgr.ShowScoreTip(bIsInHud: false);
		}
		else
		{
			_setFuncBtnEnabled(bIsEnabled: false, 1);
		}
	}

	public void _onClickAutoBtn()
	{
		if (JSYS_LL_LuckyLion_SoundManager.GetSingleton() != null)
		{
			JSYS_LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		}
		if (!mIsAutoBet)
		{
			mIsAutoBet = true;
			mAutoBtnLabel.text = ((JSYS_LL_GameInfo.getInstance().Language == 0) ? "取消自动" : "Manual");
			_setFuncBtnEnabled(bIsEnabled: false, 0);
			_setFuncBtnEnabled(bIsEnabled: false, 1);
			return;
		}
		mIsAutoBet = false;
		mAutoBtnLabel.text = ((JSYS_LL_GameInfo.getInstance().Language == 0) ? "自动" : "Auto");
		if (mHudMgr.TimeCD > 0)
		{
			_setFuncBtnEnabled(bIsEnabled: true, 0);
		}
	}

	protected bool _isAllBetSuccess()
	{
		int num = 0;
		int gameScore = mUser.GameScore;
		if (gameScore <= 0)
		{
			mHudMgr.ShowScoreTip(bIsInHud: false);
		}
		for (int i = 0; i < 15; i++)
		{
			int num2 = _getMaxMinBet(i, isMax: true);
			int num3 = _getMaxMinBet(i, isMax: false);
			if (mLastValidPersonBet[i] != 0 && (mLastValidPersonBet[i] > num2 || mLastValidPersonBet[i] < num3))
			{
				mIsDeskSettingChanged = true;
				break;
			}
		}
		if (!mIsDeskSettingChanged)
		{
			for (int j = 0; j < 15; j++)
			{
				mLastOnceBet[j] = mLastValidPersonBet[j];
				mPersonBetText[j].text = mLastValidPersonBet[j].ToString();
				num += mLastOnceBet[j];
			}
			if (num > gameScore)
			{
				for (int k = 0; k < 15; k++)
				{
					mPersonBetText[k].text = "0";
				}
				mIsScorePoor = true;
			}
			else
			{
				for (int l = 0; l < 15; l++)
				{
					mCurPersonBet[l] += mLastOnceBet[l];
					if (mCurPersonBet[l] != 0)
					{
						mIsBetZero[l] = false;
					}
				}
				mIsScorePoor = false;
			}
		}
		return !mIsDeskSettingChanged && !mIsScorePoor;
	}

	protected int _calculateOnceBet(ref bool bIsStarted, int nUserScore, int nMinBet, int nMaxBet, int iLocateId)
	{
		int num;
		if (bIsStarted)
		{
			bIsStarted = false;
			num = ((nUserScore > nMinBet && mCurrentChip > nMinBet) ? mCurrentChip : ((nUserScore > nMinBet) ? nMinBet : nUserScore));
		}
		else
		{
			num = ((nUserScore < mCurrentChip) ? nUserScore : mCurrentChip);
			if (mCurPersonBet[iLocateId] == nMaxBet)
			{
				num = 0;
			}
			else if (mCurPersonBet[iLocateId] + num >= nMaxBet)
			{
				num = nMaxBet - mCurPersonBet[iLocateId];
			}
		}
		return num;
	}

	protected void _setFuncBtnEnabled(bool bIsEnabled, int iTypeId)
	{
		switch (iTypeId)
		{
		case 0:
			mCancelBetBtnCol.enabled = bIsEnabled;
			if (bIsEnabled)
			{
				mCancelBetBtnBG.color = new Color(1f, 1f, 1f);
			}
			else
			{
				mCancelBetBtnBG.color = new Color(0.5f, 0.5f, 0.5f);
			}
			break;
		case 1:
			mXuyaBtnCol.enabled = bIsEnabled;
			if (bIsEnabled)
			{
				mXuyaBtnBG.color = new Color(1f, 1f, 1f);
			}
			else
			{
				mXuyaBtnBG.color = new Color(0.5f, 0.5f, 0.5f);
			}
			break;
		case 2:
			mAutoBetBtnCol.enabled = bIsEnabled;
			if (bIsEnabled)
			{
				mAutoBetBtnBG.color = new Color(1f, 1f, 1f);
			}
			else
			{
				mAutoBetBtnBG.color = new Color(0.5f, 0.5f, 0.5f);
			}
			break;
		}
	}

	protected int _getMaxMinBet(int iLocate, bool isMax)
	{
		int result = 0;
		if (mTableInfo != null)
		{
			result = ((iLocate <= 11) ? (isMax ? mTableInfo.MaxBet : mTableInfo.MinBet) : ((iLocate != 12 && iLocate != 14) ? (isMax ? mTableInfo.MaxHBet : mTableInfo.MinZHXBet) : (isMax ? mTableInfo.MaxZXBet : mTableInfo.MinZHXBet)));
		}
		return result;
	}

	public void _onPress(GameObject go)
	{
		int num = int.Parse(go.name.Substring(9));
		if (num <= 14 && num >= 0)
		{
			bPressed[num] = true;
		}
	}

	public void _onRelease(GameObject go)
	{
		int num = int.Parse(go.name.Substring(9));
		if (num <= 14 && num >= 0)
		{
			bPressed[num] = false;
			timer[num] = 0f;
		}
	}
}
