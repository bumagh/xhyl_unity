using UnityEngine;

public class JSYS_LL_PrizeResult : MonoBehaviour
{
	protected UIPanel mResultPanel;

	protected Collider mBackgroundCol;

	protected GameObject mJpPowerObj;

	protected UISprite mJpPower;

	protected GameObject mLuckyMac;

	protected UILabel mLuckyMachine;

	protected GameObject mBounsNum;

	protected GameObject mLocalBonus;

	protected GameObject mGlobalBonus;

	protected UILabel mBonusValue;

	protected JSYS_LL_PersonInfo mUserSelf;

	protected GameObject[] mNormalAnimalResult = new GameObject[12];

	protected GameObject[] mLuckyAnimalResult = new GameObject[12];

	protected UISprite[] mNormalResultIcon = new UISprite[12];

	protected UILabel[] mNormalPower = new UILabel[12];

	protected UISprite[] mLuckyResultIcon = new UISprite[12];

	protected UISprite[] mLuckyType = new UISprite[12];

	protected UILabel[] mLuckyPower = new UILabel[12];

	protected GameObject mZHXResultOBj;

	protected UISprite mZHXResult;

	protected UILabel mZHXPower;

	protected UILabel[] mAllUserWin = new UILabel[8];

	protected int[] mAllUserWinScores = new int[8];

	protected UISprite[] mAllUserIcon = new UISprite[8];

	protected UILabel[] mAllUserNickNameText = new UILabel[8];

	protected string[] mAllUserNickame = new string[8]
	{
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty,
		string.Empty
	};

	protected GameObject[] mUserItem = new GameObject[8];

	protected UILabel mCurrentWin;

	protected JSYS_LL_HudManager mHudManager;

	protected JSYS_LL_BetPanel mBet;

	protected JSYS_LL_HistoryRecord mRecordPanel;

	public int CurrentWin
	{
		set
		{
			mCurrentWin.text = value.ToString();
		}
	}

	private void Start()
	{
		mResultPanel = GetComponent<UIPanel>();
		mResultPanel.enabled = false;
		mBackgroundCol = base.transform.Find("Background").GetComponent<Collider>();
		mBackgroundCol.enabled = false;
		mUserSelf = JSYS_LL_GameInfo.getInstance().UserInfo;
		mLuckyMac = base.transform.Find("LuckyPrize").Find("LuckyMac").gameObject;
		mLuckyMachine = mLuckyMac.transform.Find("LuckyMacNum").GetComponent<UILabel>();
		mJpPowerObj = base.transform.Find("JpPower").gameObject;
		mJpPower = mJpPowerObj.transform.GetComponent<UISprite>();
		mBounsNum = base.transform.Find("BounsNum").Find("Number").gameObject;
		mLocalBonus = base.transform.Find("BounsNum").Find("LocalBonus").gameObject;
		mGlobalBonus = base.transform.Find("BounsNum").Find("GlobalBonus").gameObject;
		mBonusValue = mBounsNum.transform.GetComponent<UILabel>();
		mHudManager = GameObject.Find("HudPanel").GetComponent<JSYS_LL_HudManager>();
		mBet = GameObject.Find("BetPanel").GetComponent<JSYS_LL_BetPanel>();
		mRecordPanel = GameObject.Find("RecordPanel").GetComponent<JSYS_LL_HistoryRecord>();
		mZHXResultOBj = base.transform.Find("ZHXResult").gameObject;
		mZHXResult = mZHXResultOBj.transform.Find("ResultIcon").GetComponent<UISprite>();
		mZHXPower = mZHXResultOBj.transform.Find("PowerText").GetComponent<UILabel>();
		for (int i = 0; i < 12; i++)
		{
			mNormalAnimalResult[i] = base.transform.Find("NormalPrize").Find("PrizeAnimal" + i).gameObject;
			mNormalAnimalResult[i].SetActiveRecursively(state: false);
			mNormalResultIcon[i] = mNormalAnimalResult[i].transform.Find("ResultIcon").GetComponent<UISprite>();
			mNormalPower[i] = mNormalAnimalResult[i].transform.Find("Power").GetComponent<UILabel>();
			mLuckyAnimalResult[i] = base.transform.Find("LuckyPrize").Find("PrizeAnimal" + i).gameObject;
			mLuckyAnimalResult[i].SetActiveRecursively(state: false);
			mLuckyResultIcon[i] = mLuckyAnimalResult[i].transform.Find("ResultIcon").GetComponent<UISprite>();
			mLuckyType[i] = mLuckyAnimalResult[i].transform.Find("LuckyType").GetComponent<UISprite>();
			mLuckyPower[i] = mLuckyAnimalResult[i].transform.Find("Power").GetComponent<UILabel>();
		}
		mCurrentWin = base.transform.Find("CurrentWin").Find("WinScore").GetComponent<UILabel>();
		for (int j = 0; j < 8; j++)
		{
			mUserItem[j] = base.transform.Find("UserList").Find("UserItem" + (j + 1)).gameObject;
			mAllUserIcon[j] = mUserItem[j].transform.Find("PersonIcon").GetComponent<UISprite>();
			mAllUserNickNameText[j] = mUserItem[j].transform.Find("NickName").GetComponent<UILabel>();
			mAllUserWin[j] = mUserItem[j].transform.Find("WinNum").GetComponent<UILabel>();
		}
	}

	private void Update()
	{
	}

	public void SetUserList(string[] strNickname, int[] iIconId)
	{
		for (int i = 0; i < 8; i++)
		{
			if (i < strNickname.Length)
			{
				mAllUserNickame[i] = strNickname[i];
				mAllUserNickNameText[i].text = strNickname[i];
				if (strNickname[i].CompareTo(string.Empty) == 0)
				{
					mAllUserIcon[i].spriteName = "personIcon1";
				}
				else
				{
					mAllUserIcon[i].spriteName = "personIcon" + iIconId[i];
				}
			}
			else
			{
				mAllUserNickame[i] = string.Empty;
				mAllUserNickNameText[i].text = string.Empty;
			}
		}
	}

	public void SetAllUserWin(int[] nWinNum)
	{
		nWinNum.CopyTo(mAllUserWinScores, 0);
		for (int i = 0; i < 8; i++)
		{
			_updateScoreText(i, 0);
		}
	}

	public void ShowPrizeResult()
	{
		if (mHudManager.IsBonusAward)
		{
			mHudManager.IsBonusAward = false;
		}
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendResultList(mUserSelf.TableId);
		mResultPanel.enabled = true;
		mBackgroundCol.enabled = true;
		TweenAlpha.Begin(base.transform.gameObject, 0.5f, 1f);
		for (int i = 0; i < 8; i++)
		{
			if (mUserItem[i].activeSelf)
			{
				iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", mAllUserWinScores[i], "time", 1.8f, "onupdate", "_onScoreTweenUpdate" + i, "easetype", iTween.EaseType.easeInQuad));
				mAllUserWin[i].gameObject.transform.localScale = new Vector3(18f, 18f, 1f);
				iTween.ScaleTo(mAllUserWin[i].gameObject, iTween.Hash("scale", new Vector3(23f, 23f, 1f), "time", 0.5f, "delay", 1.8f, "easetype", iTween.EaseType.elastic));
			}
		}
	}

	protected void _updateScoreText(int iIndex, int iDestValue)
	{
		if (iDestValue >= 0)
		{
			mAllUserWin[iIndex].text = "+" + iDestValue;
		}
		else
		{
			mAllUserWin[iIndex].text = string.Empty + iDestValue;
		}
	}

	protected void _onScoreTweenUpdate0(int newValue)
	{
		_updateScoreText(0, newValue);
	}

	protected void _onScoreTweenUpdate1(int newValue)
	{
		_updateScoreText(1, newValue);
	}

	protected void _onScoreTweenUpdate2(int newValue)
	{
		_updateScoreText(2, newValue);
	}

	protected void _onScoreTweenUpdate3(int newValue)
	{
		_updateScoreText(3, newValue);
	}

	protected void _onScoreTweenUpdate4(int newValue)
	{
		_updateScoreText(4, newValue);
	}

	protected void _onScoreTweenUpdate5(int newValue)
	{
		_updateScoreText(5, newValue);
	}

	protected void _onScoreTweenUpdate6(int newValue)
	{
		_updateScoreText(6, newValue);
	}

	protected void _onScoreTweenUpdate7(int newValue)
	{
		_updateScoreText(7, newValue);
	}

	public void HidePrizeResult(bool bIsFadeOut = true)
	{
		mBackgroundCol.enabled = false;
		if (bIsFadeOut)
		{
			TweenAlpha.Begin(base.transform.gameObject, 0.5f, 0f);
		}
		else if ((bool)base.transform.GetComponent<TweenAlpha>())
		{
			base.transform.GetComponent<TweenAlpha>().alpha = 0f;
		}
	}

	public int SetAnimalResult(int[] iAnimalID, int nLength = 1, int nJpPower = 0)
	{
		if (nLength < 1 || nLength > 12)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:中奖结果错误length = " + nLength);
			return 1;
		}
		for (int i = 0; i < nLength; i++)
		{
			if (iAnimalID[i] > 11 || iAnimalID[i] < 0)
			{
				JSYS_LL_ErrorManager.GetSingleton().AddError("Error:中奖动物错误:" + iAnimalID[i]);
				return 2;
			}
		}
		_resetResult();
		float num = 250f;
		float num2 = 300f;
		int num3 = ((nLength + 1) % 4 == 0) ? ((nLength + 1) / 4) : ((nLength + 1) / 4 + 1);
		int num4 = (num3 != 1) ? (((nLength + 1) % num3 == 0) ? ((nLength + 1) / num3) : ((nLength + 1) / num3 + 1)) : (nLength + 1);
		float num5 = (nLength == 3) ? (2f / 3f) : (1f / (float)num3);
		Vector3 localPosition = mNormalAnimalResult[0].transform.localPosition;
		for (int j = 0; j < nLength + 1; j++)
		{
			if (j < num4 * (num3 - 1))
			{
				localPosition.x = -0.5f * num * num5 * (float)(num4 - 1) + num * num5 * (float)(j % num4);
			}
			else
			{
				localPosition.x = -0.5f * num * num5 * (float)(nLength + 1 - j / num4 * num4 - 1) + num * num5 * (float)(j % num4);
			}
			localPosition.y = 0.5f * num2 * num5 * (float)(num3 - 1) - num2 * num5 * (float)(j / num4);
			if (j < nLength)
			{
				if (!mHudManager.IsBonusAward)
				{
					mNormalResultIcon[j].spriteName = "animalIcon" + iAnimalID[j];
				}
				else
				{
					mNormalResultIcon[j].spriteName = "bonusAnimal" + iAnimalID[j];
				}
				mNormalPower[j].text = "X" + mBet.AnimalPower[iAnimalID[j]];
				mNormalAnimalResult[j].transform.localPosition = localPosition;
				mNormalAnimalResult[j].transform.localScale = new Vector3(num5, num5, num5);
				mNormalAnimalResult[j].SetActiveRecursively(state: true);
			}
			else
			{
				mZHXResultOBj.transform.localPosition = localPosition;
			}
		}
		if (mHudManager.IsBonusAward)
		{
			mGlobalBonus.SetActiveRecursively(state: true);
			mBounsNum.SetActiveRecursively(state: true);
			mBounsNum.transform.localPosition = new Vector3(30f, 12f, 0f);
			mBonusValue.text = mHudManager.BonusNumber.ToString().PadLeft(5, '0');
		}
		if (nJpPower >= 2 && nJpPower <= 5)
		{
			mJpPowerObj.SetActiveRecursively(state: true);
			mJpPower.spriteName = "JPX" + nJpPower;
			mJpPowerObj.transform.localPosition = new Vector3(-125f, -250f, 0f);
		}
		if (mHudManager.mDiceResult == 0)
		{
			mZHXResult.spriteName = "zhuang";
			mZHXPower.text = "X2";
		}
		else if (mHudManager.mDiceResult == 1)
		{
			mZHXResult.spriteName = "he";
			mZHXPower.text = "X8";
		}
		else if (mHudManager.mDiceResult == 2)
		{
			mZHXResult.spriteName = "xian";
			mZHXPower.text = "X2";
		}
		mZHXResultOBj.transform.localScale = new Vector3(num5, num5, num5);
		return 0;
	}

	public int SetLuckyJP(int iAnimalID, int iLuckyMachine, int iLuckyAnimalID, int nJPPower)
	{
		if (iAnimalID > 11 || iAnimalID < 0 || iLuckyAnimalID >= 12 || iLuckyAnimalID < 0)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:中奖动物错误" + iAnimalID);
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:中奖动物错误" + iLuckyAnimalID);
			return 1;
		}
		if (iLuckyMachine > 8 || iLuckyMachine < 1)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:中奖分机错误" + iLuckyMachine);
			return 2;
		}
		if (nJPPower > 5 || nJPPower < 2)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:闪电倍率错误" + nJPPower);
			return 3;
		}
		_resetResult();
		_showGlobalResult(iAnimalID, iLuckyMachine);
		mLuckyResultIcon[0].spriteName = "animalIcon" + iLuckyAnimalID;
		mLuckyPower[0].text = "X" + mBet.AnimalPower[iLuckyAnimalID];
		mLuckyType[0].spriteName = "luckyJP";
		mLuckyAnimalResult[0].transform.localPosition = new Vector3(0f, 0f, 0f);
		mLuckyAnimalResult[0].SetActiveRecursively(state: true);
		mJpPowerObj.SetActiveRecursively(state: true);
		mJpPower.spriteName = "JPX" + nJPPower;
		mJpPowerObj.transform.localPosition = new Vector3(0f, -220f, 0f);
		mHudManager.LuckyMacNum = iLuckyMachine;
		mHudManager.LuckyAwardType = 1;
		return 0;
	}

	public int SetLuckyTimes(int iAnimalID, int iLuckyMachine, int[] iLuckyAnimalID, int nLength)
	{
		if (iAnimalID > 11 || iAnimalID < 0)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:中奖动物错误" + iAnimalID);
			return 1;
		}
		if (iLuckyMachine > 8 || iLuckyMachine < 1)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:中奖动物错误" + iAnimalID);
			return 2;
		}
		if (nLength < 1 || nLength > 11)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:中奖结果错误length = " + nLength);
			return 3;
		}
		for (int i = 0; i < nLength; i++)
		{
			if (iLuckyAnimalID[i] > 11 || iLuckyAnimalID[i] < 0)
			{
				JSYS_LL_ErrorManager.GetSingleton().AddError("Error:中奖动物错误" + iLuckyAnimalID[i]);
				return 4;
			}
		}
		_resetResult();
		_showGlobalResult(iAnimalID, iLuckyMachine, nLength);
		float num = 160f;
		float num2 = 200f;
		int num3 = (nLength % 3 == 0) ? (nLength / 3) : (nLength / 3 + 1);
		int num4 = (num3 != 1) ? ((nLength % num3 == 0) ? (nLength / num3) : (nLength / num3 + 1)) : nLength;
		float num5 = (nLength == 3) ? (5f / 6f) : ((!(1f / (float)num3 + 0.333333343f <= 1f)) ? 1f : (1f / (float)num3 + 0.333333343f));
		Vector3 localPosition = new Vector3(0f, 0f, 0f);
		for (int j = 0; j < nLength; j++)
		{
			if (j < num4 * (num3 - 1))
			{
				localPosition.x = -0.5f * num * num5 * (float)(num4 - 1) + num * num5 * (float)(j % num4);
			}
			else
			{
				localPosition.x = -0.5f * num * num5 * (float)(nLength - j / num4 * num4 - 1) + num * num5 * (float)(j % num4);
			}
			localPosition.y = 0.5f * num2 * num5 * (float)(num3 - 1) - num2 * num5 * (float)(j / num4);
			mLuckyResultIcon[j].spriteName = "animalIcon" + iLuckyAnimalID[j];
			mLuckyPower[j].text = "X" + mBet.AnimalPower[iLuckyAnimalID[j]];
			mLuckyType[j].spriteName = "luckyTimes";
			mLuckyAnimalResult[j].SetActiveRecursively(state: true);
			mLuckyAnimalResult[j].transform.localPosition = localPosition;
			mLuckyAnimalResult[j].transform.localScale = new Vector3(num5, num5, num5);
		}
		mHudManager.LuckyMacNum = iLuckyMachine;
		mHudManager.LuckyAwardType = 2;
		return 0;
	}

	public int SetLuckyBonus(int iAnimalID, int iLuckyMachine, int iLuckyAnimalID)
	{
		if (iAnimalID > 11 || iAnimalID < 0 || iLuckyAnimalID >= 12 || iLuckyAnimalID < 0)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:中奖动物错误" + iAnimalID);
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:中奖动物错误" + iLuckyAnimalID);
			return 1;
		}
		if (iLuckyMachine > 8 || iLuckyMachine < 1)
		{
			JSYS_LL_ErrorManager.GetSingleton().AddError("Error:中奖分机错误" + iLuckyMachine);
			return 2;
		}
		_resetResult();
		_showGlobalResult(iAnimalID, iLuckyMachine);
		mLuckyResultIcon[0].spriteName = "bonusAnimal" + iLuckyAnimalID;
		mLuckyPower[0].text = "X" + mBet.AnimalPower[iLuckyAnimalID];
		mLuckyType[0].spriteName = "luckyBonus";
		mLuckyAnimalResult[0].transform.localPosition = new Vector3(0f, 0f, 0f);
		mLuckyAnimalResult[0].SetActiveRecursively(state: true);
		mLocalBonus.SetActiveRecursively(state: true);
		mBonusValue.text = mHudManager.BonusNumber.ToString().PadLeft(5, '0');
		mBounsNum.SetActiveRecursively(state: true);
		mBounsNum.transform.localPosition = new Vector3(0f, 12f, 0f);
		mHudManager.LuckyMacNum = iLuckyMachine;
		mHudManager.LuckyAwardType = 0;
		return 0;
	}

	protected void _resetResult()
	{
		mJpPowerObj.SetActiveRecursively(state: false);
		mLuckyMac.SetActiveRecursively(state: false);
		mBounsNum.SetActiveRecursively(state: false);
		mLocalBonus.transform.localPosition = new Vector3(-88f, 3f, 0f);
		mLocalBonus.SetActiveRecursively(state: false);
		mGlobalBonus.SetActiveRecursively(state: false);
		mZHXResultOBj.transform.localScale = new Vector3(1f, 1f, 1f);
		for (int i = 0; i < 12; i++)
		{
			mNormalAnimalResult[i].SetActiveRecursively(state: false);
			mNormalAnimalResult[i].transform.localScale = new Vector3(1f, 1f, 1f);
			mNormalAnimalResult[i].transform.localPosition = new Vector3(0f, 0f, 0f);
			mLuckyAnimalResult[i].SetActiveRecursively(state: false);
			mLuckyAnimalResult[i].transform.localScale = new Vector3(1f, 1f, 1f);
			mLuckyAnimalResult[i].transform.localPosition = new Vector3(0f, 0f, 0f);
		}
	}

	protected void _showGlobalResult(int iAnimalId, int iMacNum, int sum = 1)
	{
		mNormalResultIcon[0].spriteName = "animalIcon" + iAnimalId;
		mNormalPower[0].text = "X" + mBet.AnimalPower[iAnimalId];
		Vector3 localPosition = mNormalAnimalResult[0].transform.localPosition;
		localPosition.x = -300f;
		localPosition.y = 0f;
		mNormalAnimalResult[0].transform.localPosition = localPosition;
		mNormalAnimalResult[0].SetActiveRecursively(state: true);
		mLuckyMac.SetActiveRecursively(state: true);
		mLuckyMachine.text = iMacNum.ToString();
		mLuckyMac.transform.localPosition = new Vector3(0f, (float)((sum - 1) / 3) * 30f);
		if (mHudManager.mDiceResult == 0)
		{
			mZHXResult.spriteName = "zhuang";
			mZHXPower.text = "X2";
		}
		else if (mHudManager.mDiceResult == 1)
		{
			mZHXResult.spriteName = "he";
			mZHXPower.text = "X8";
		}
		else if (mHudManager.mDiceResult == 2)
		{
			mZHXResult.spriteName = "xian";
			mZHXPower.text = "X2";
		}
		localPosition = mZHXResultOBj.transform.localPosition;
		localPosition.x = 300f;
		localPosition.y = 0f;
		mZHXResultOBj.transform.localPosition = localPosition;
	}
}
