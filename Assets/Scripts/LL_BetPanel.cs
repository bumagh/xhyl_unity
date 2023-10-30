using LitJson;
using UnityEngine;

public class LL_BetPanel : MonoBehaviour
{
	protected LL_HudManager mHudMgr;

	protected UIPanel mBetPanel;

	protected UISprite mBetPanelBtnSprite;

	protected Collider mBackgroundCol;

	protected bool mIsBetVisable;

	protected Transform mChipBtn;

	protected Collider[] mChipCol = new Collider[4];

	protected Collider[] mBetBtnCol = new Collider[15];

	protected Collider mAutoBetBtnCol;

	protected Collider mXuyaBtnCol;

	protected Collider mCancelBetBtnCol;

	protected UISprite mAutoBetBtnBG;

	protected UI2DSprite mXuyaBtnBG;

	protected UI2DSprite mCancelBetBtnBG;

	protected GameObject mBetFrameLightObj;

	protected bool mIsFrameLight;

	protected float mTotalTime;

	protected LL_TableInfo mTableInfo;

	protected LL_PersonInfo mUser;

	protected int mCurrentTableID;

	protected int mCurrentChip = 1;

	protected LL_UICheckbox[] mChipOption = new LL_UICheckbox[4];

	protected int[] mChipValue = new int[4]
	{
		1000,
		3000,
		5000,
		10000
	};

	protected UILabel[] mChipText = new UILabel[4];

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

	protected UILabel mCancelBtnLabel;

	protected UILabel mXuyaBtnLabel;

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
		mHudMgr = GameObject.Find("HudPanel").GetComponent<LL_HudManager>();
		mBetPanel = GetComponent<UIPanel>();
		mChipBtn = base.transform.Find("ChipBtn");
		mBackgroundCol = base.transform.Find("Background").GetComponent<Collider>();
		mAutoBetBtnCol = base.transform.Find("AutoBetBtn").GetComponent<Collider>();
		mXuyaBtnCol = base.transform.Find("XuyaBtn").GetComponent<Collider>();
		mCancelBetBtnCol = base.transform.Find("CancelBetBtn").GetComponent<Collider>();
		mAutoBetBtnBG = base.transform.Find("AutoBetBtn").Find("Background").GetComponent<UISprite>();
		mXuyaBtnBG = base.transform.Find("XuyaBtn").Find("Background").GetComponent<UI2DSprite>();
		mCancelBetBtnBG = base.transform.Find("CancelBetBtn").Find("Background").GetComponent<UI2DSprite>();
		mAutoBtnLabel = base.transform.Find("AutoBetBtn").Find("Label").GetComponent<UILabel>();
		mCancelBtnLabel = base.transform.Find("CancelBetBtn").Find("Label").GetComponent<UILabel>();
		mXuyaBtnLabel = base.transform.Find("XuyaBtn").Find("Label").GetComponent<UILabel>();
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
			if (i < 4)
			{
				mChipCol[i] = mChipBtn.Find("Chip" + i).GetComponent<Collider>();
				mChipOption[i] = mChipBtn.Find("Chip" + i).GetComponent<LL_UICheckbox>();
				mChipText[i] = mChipBtn.Find("Chip" + i).Find("Label").GetComponent<UILabel>();
			}
		}
		mBetPanelBtnSprite = GameObject.Find("HudPanel").transform.Find("BetPanelBtn").Find("Title").GetComponent<UISprite>();
		mUser = LL_GameInfo.getInstance().UserInfo;
		HideBetPanel();
		ClearAllBet();
		_setFuncBtnEnabled(bIsEnabled: false, 1);
		_setFuncBtnEnabled(bIsEnabled: false, 2);
	}

	private void OnEnable()
	{
		SetEN();
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
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
	}

	public void ShowBetPanel(bool bIsMove = true)
	{
		mIsBetVisable = true;
		_setColliderActive(bIsActive: true);
		mBetPanelBtnSprite.spriteName = ((LL_GameInfo.getInstance().Language == 0) ? "shouQi" : "shouQi_e");
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
		mBetPanelBtnSprite.spriteName = ((LL_GameInfo.getInstance().Language == 0) ? "bet" : "bet_e");
		mBetPanelBtnSprite.MakePixelPerfect();
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
			if (i < 4)
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
			UnityEngine.Debug.Log("Error:倍率错误");
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
		UnityEngine.Debug.LogError("SetAnimalBet: " + JsonMapper.ToJson(nNum) + " bIsTotal: " + bIsTotal + " isLastBet: " + isLastBet);
		if (bIsTotal)
		{
			if (nNum.Length < 15)
			{
				for (int i = 0; i < nNum.Length; i++)
				{
					mTotalBetText[i].text = nNum[i].ToString();
				}
			}
			else
			{
				for (int j = 0; j < 15; j++)
				{
					mTotalBetText[j].text = nNum[j].ToString();
				}
			}
		}
		else
		{
			if (nNum.Length < 15)
			{
				for (int k = 0; k < nNum.Length; k++)
				{
					mCurPersonBet[k] = nNum[k];
				}
			}
			else
			{
				for (int l = 0; l < 15; l++)
				{
					mCurPersonBet[l] = nNum[l];
				}
			}
			for (int m = 0; m < 15; m++)
			{
				mPersonBetText[m].text = mCurPersonBet[m].ToString();
			}
			if (isLastBet)
			{
				int n;
				for (n = 0; n < nNum.Length - 1 && nNum[n] <= 0; n++)
				{
				}
				if (nNum[nNum.Length - 1] <= 0)
				{
					n++;
				}
				if (n < nNum.Length && n < 15)
				{
					if (nNum.Length < 15)
					{
						for (int num = 0; num < nNum.Length; num++)
						{
							mLastValidPersonBet[num] = nNum[num];
						}
						for (int num2 = nNum.Length; num2 < 15; num2++)
						{
							mLastValidPersonBet[num2] = 0;
						}
					}
					else
					{
						for (int num3 = 0; num3 < 15; num3++)
						{
							mLastValidPersonBet[num3] = nNum[num3];
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

	public void ModifyChip(LL_TableInfo tableInfo)
	{
		mCurrentTableID = mUser.TableId;
		mTableInfo = tableInfo;
		for (int i = 0; i < 4; i++)
		{
			if ((bool)mChipText[i])
			{
				mChipText[i].text = mChipValue[i].ToString();
			}
		}
	}

	public void SetChip(int[] betChips)
	{
		mChipValue = betChips;
		for (int i = 0; i < 4; i++)
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
			if (!LL_MyTest.TEST)
			{
				for (int i = 0; i < 15; i++)
				{
					LL_NetMngr.GetSingleton().MyCreateSocket.SendUserBet(i, mLastOnceBet[i], mCurrentTableID);
				}
				mIsAllzero = false;
			}
		}
		else
		{
			mIsAutoBet = false;
			mAutoBtnLabel.text = ((LL_GameInfo.getInstance().Language == 0) ? "自动" : "Auto");
		}
	}

	public void Restart()
	{
		mIsAutoBet = false;
		SetEN();
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

	private void SetEN()
	{
		mAutoBtnLabel.text = ((LL_GameInfo.getInstance().Language == 0) ? "自动" : "Auto");
		mCancelBtnLabel.text = ((LL_GameInfo.getInstance().Language == 0) ? "取消压分" : "Cancel");
		mXuyaBtnLabel.text = ((LL_GameInfo.getInstance().Language == 0) ? "续压" : "Contin");
	}

	public void _onChangeChip()
	{
		int num = 0;
		while (true)
		{
			if (num < 4)
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
			UnityEngine.Debug.LogError("mHudMgr.TimeCD <= 0");
			return;
		}
		int num = int.Parse(sender.name.Substring(9));
		if (num > 14 || num < 0)
		{
			UnityEngine.Debug.Log("押注对象错误：betItem" + num);
			return;
		}
		int gameScore = mUser.GameScore;
		mCurrentTableID = mUser.TableId;
		int nMinBet = 0;
		int num2 = 0;
		if (mTableInfo == null && LL_AppUIMngr.GetSingleton() != null)
		{
			UnityEngine.Debug.LogError(1);
			if (LL_AppUIMngr.GetSingleton().mTableList != null)
			{
				UnityEngine.Debug.LogError(2);
				if (LL_AppUIMngr.GetSingleton().mTableList.mSelectTable != null)
				{
					UnityEngine.Debug.LogError(3);
					mTableInfo = LL_AppUIMngr.GetSingleton().mTableList.mSelectTable;
				}
			}
		}
		if (mTableInfo != LL_AppUIMngr.GetSingleton().mTableList.mSelectTable)
		{
			mTableInfo = LL_AppUIMngr.GetSingleton().mTableList.mSelectTable;
			UnityEngine.Debug.LogError("==========强制更新桌子信息========");
		}
		if (mTableInfo != null)
		{
			nMinBet = ((num > 11) ? mTableInfo.MinZHXBet : mTableInfo.MinBet);
		}
		else
		{
			UnityEngine.Debug.LogError("mTableInfo = null");
		}
		num2 = _getMaxMinBet(num, isMax: true);
		if (gameScore <= 0)
		{
			UnityEngine.Debug.LogError("gameScore <= 0");
			mHudMgr.ShowScoreTip(bIsInHud: false);
			LL_LuckyLion_SoundManager.GetSingleton().playButtonSound(LL_LuckyLion_SoundManager.EUIBtnSoundType.BetFail);
			return;
		}
		mLastOnceBet[num] = _calculateOnceBet(ref mIsBetZero[num], gameScore, nMinBet, num2, num);
		if (mLastOnceBet[num] > 0)
		{
			mIsDeskSettingChanged = false;
			mIsScorePoor = false;
			mIsAllzero = false;
			LL_LuckyLion_SoundManager.GetSingleton().playButtonSound(LL_LuckyLion_SoundManager.EUIBtnSoundType.BetSuccess);
			mBetFrameLightObj.SetActiveRecursively(state: true);
			mBetFrameLightObj.transform.position = sender.transform.position;
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
			if (!LL_MyTest.TEST)
			{
				UnityEngine.Debug.LogError(sender.name + " 押注 " + num + "  " + mLastOnceBet[num] + "  " + mCurrentTableID);
				LL_NetMngr.GetSingleton().MyCreateSocket.SendUserBet(num, mLastOnceBet[num], mCurrentTableID);
			}
			else
			{
				UnityEngine.Debug.LogError("不允许押注 " + LL_MyTest.TEST);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("this.mLastOnceBet[num] <= 0: " + mLastOnceBet[num]);
			LL_LuckyLion_SoundManager.GetSingleton().playButtonSound(LL_LuckyLion_SoundManager.EUIBtnSoundType.BetFail);
		}
	}

	public void _onClickCancelBtn()
	{
		if (mHudMgr.TimeCD > 0)
		{
			mIsAllzero = true;
			LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
			for (int i = 0; i < 15; i++)
			{
				mCurPersonBet[i] = 0;
				mIsBetZero[i] = true;
				mPersonBetText[i].text = "0";
			}
			if (!LL_MyTest.TEST)
			{
				LL_NetMngr.GetSingleton().MyCreateSocket.SendCancelBet(mCurrentTableID);
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
			if (!LL_MyTest.TEST)
			{
				for (int i = 0; i < 15; i++)
				{
					LL_NetMngr.GetSingleton().MyCreateSocket.SendUserBet(i, mLastOnceBet[i], mCurrentTableID);
				}
				mIsAllzero = false;
			}
			_setFuncBtnEnabled(bIsEnabled: false, 1);
			_setFuncBtnEnabled(bIsEnabled: true, 0);
			_setFuncBtnEnabled(bIsEnabled: true, 2);
			LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
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
		LL_LuckyLion_SoundManager.GetSingleton().playButtonSound();
		if (!mIsAutoBet)
		{
			mIsAutoBet = true;
			mAutoBtnLabel.text = ((LL_GameInfo.getInstance().Language == 0) ? "取消自动" : "Manual");
			_setFuncBtnEnabled(bIsEnabled: false, 0);
			_setFuncBtnEnabled(bIsEnabled: false, 1);
			return;
		}
		mIsAutoBet = false;
		mAutoBtnLabel.text = ((LL_GameInfo.getInstance().Language == 0) ? "自动" : "Auto");
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
