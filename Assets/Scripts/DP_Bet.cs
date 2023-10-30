using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DP_Bet : MonoBehaviour
{
	[HideInInspector]
	public Image imgBetFrameLight;

	[HideInInspector]
	public DP_BtnAnimCtrl btnCancelBet;

	[HideInInspector]
	public DP_BtnAnimCtrl btnXuyaBet;

	[HideInInspector]
	public DP_BtnAnimCtrl btnAuto;

	[HideInInspector]
	public Text txtBtnAuto;

	[HideInInspector]
	public Button[] btnChips = new Button[3];

	private Transform tfCurChip;

	[HideInInspector]
	public DP_BetBtnItem[] betBtnItems = new DP_BetBtnItem[12];

	[HideInInspector]
	public bool bAuto;

	private int curChipValue = 10;

	private int curChipIndex;

	private int[] mChipValue = new int[3]
	{
		10,
		50,
		100
	};

	[HideInInspector]
	public int[] curPersonBet = new int[12];

	[HideInInspector]
	public int[] lastPersonBet = new int[12];

	[HideInInspector]
	public int[] totalBet = new int[12];

	[HideInInspector]
	public bool[] bBetZero = new bool[12]
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
		true
	};

	[HideInInspector]
	public int[] lastValidPersonBet = new int[12];

	private bool bAllBetZero;

	private bool bDeskSettingChanged;

	private bool bScorePoor;

	private DP_TableInfo mTableInfo;

	private DP_GameInfo mGameInfo;

	[SerializeField]
	private GameObject objScoreTip;

	[HideInInspector]
	public bool bShowBet;

	public void Init()
	{
		imgBetFrameLight = base.transform.Find("BetFrameLight").GetComponent<Image>();
		btnCancelBet = base.transform.Find("BtnCancel").GetComponent<DP_BtnAnimCtrl>();
		btnXuyaBet = base.transform.Find("BtnXuya").GetComponent<DP_BtnAnimCtrl>();
		btnAuto = base.transform.Find("BtnAuto").GetComponent<DP_BtnAnimCtrl>();
		txtBtnAuto = btnAuto.transform.GetChild(0).GetChild(0).GetComponent<Text>();
		Transform transform = base.transform.Find("ChipBtn");
		tfCurChip = transform.Find("CurChip");
		curChipValue = mChipValue[curChipIndex];
		for (int i = 0; i < 3; i++)
		{
			btnChips[i] = transform.GetChild(i).GetComponent<Button>();
			int index2 = i;
			btnChips[index2].onClick.AddListener(delegate
			{
				ClickBtnChip(index2);
			});
		}
		for (int j = 0; j < 12; j++)
		{
			betBtnItems[j] = base.transform.Find($"BtnBetItem{j}").GetComponent<DP_BetBtnItem>();
			int index = j;
			betBtnItems[index].btnBet.onClick.AddListener(delegate
			{
				ClickBtnBetItem(index);
			});
			betBtnItems[index].btnBet.onLongPress.AddListener(delegate
			{
				ClickBtnBetItem(index);
			});
		}
		mGameInfo = DP_GameInfo.getInstance();
		mTableInfo = mGameInfo.TableInfo;
		btnCancelBet.btn.onClick.AddListener(ClickBtnCancel);
		btnXuyaBet.btn.onClick.AddListener(ClickBtnXuya);
		btnAuto.btn.onClick.AddListener(ClickBtnAuto);
	}

	public void ShowBet(bool bShow)
	{
		bShowBet = bShow;
		base.gameObject.SetActive(bShow);
	}

	public int SetAnimalPower(int[] nPower, int nLength = 12)
	{
		if (nLength > 12 || nLength <= 0)
		{
			UnityEngine.Debug.Log("Error:倍率错误");
			return 1;
		}
		for (int i = 0; i < nPower.Length; i++)
		{
			if (i < 12)
			{
				DP_GameData.power[i] = nPower[i];
			}
			betBtnItems[i].txtPower.text = nPower[i].ToString();
		}
		return 0;
	}

	public int SetAnimalBet(int[] nNum, bool bIsTotal, bool isLastBet = true)
	{
		if (bIsTotal)
		{
			if (nNum.Length < 12)
			{
				for (int i = 0; i < nNum.Length; i++)
				{
					betBtnItems[i].txtTotalBet.text = nNum[i].ToString();
				}
				for (int j = nNum.Length; j < 12; j++)
				{
					betBtnItems[j].txtTotalBet.text = "0";
				}
			}
			else
			{
				for (int k = 0; k < 12; k++)
				{
					betBtnItems[k].txtTotalBet.text = nNum[k].ToString();
				}
			}
		}
		else
		{
			if (nNum.Length < 12)
			{
				for (int l = 0; l < nNum.Length; l++)
				{
					curPersonBet[l] = nNum[l];
				}
				for (int m = nNum.Length; m < 12; m++)
				{
					curPersonBet[m] = 0;
				}
			}
			else
			{
				for (int n = 0; n < 12; n++)
				{
					curPersonBet[n] = nNum[n];
				}
			}
			for (int num = 0; num < 12; num++)
			{
				betBtnItems[num].txtPersonBet.text = curPersonBet[num].ToString();
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
				if (num2 < nNum.Length && num2 < 12)
				{
					if (nNum.Length < 12)
					{
						for (int num3 = 0; num3 < nNum.Length; num3++)
						{
							lastValidPersonBet[num3] = nNum[num3];
						}
						for (int num4 = nNum.Length; num4 < 12; num4++)
						{
							lastValidPersonBet[num4] = 0;
						}
					}
					else
					{
						for (int num5 = 0; num5 < 12; num5++)
						{
							lastValidPersonBet[num5] = nNum[num5];
						}
					}
				}
			}
		}
		return 0;
	}

	public void ClearAllBet()
	{
		if (!bAuto)
		{
			SetFuncBtnEnabled(bIsEnabled: false, 2);
		}
		for (int i = 0; i < 12; i++)
		{
			curPersonBet[i] = 0;
			lastPersonBet[i] = 0;
			bBetZero[i] = true;
			betBtnItems[i].txtPersonBet.text = "0";
			betBtnItems[i].txtTotalBet.text = "0";
		}
		bAllBetZero = true;
	}

	public void AutoBet()
	{
		if (IsAllBetSuccess())
		{
			for (int i = 0; i < 15; i++)
			{
				DP_NetMngr.GetSingleton().MyCreateSocket.SendUserBet(i, lastPersonBet[i], mTableInfo.TableServerID);
			}
			bAllBetZero = false;
		}
		else
		{
			bAuto = false;
			txtBtnAuto.text = ((DP_GameInfo.getInstance().Language == 0) ? "自动" : "Auto");
		}
	}

	public void Restart()
	{
		bAuto = false;
		txtBtnAuto.text = ((DP_GameInfo.getInstance().Language == 0) ? "自动" : "Auto");
		SetFuncBtnEnabled(bIsEnabled: true, 0);
		SetFuncBtnEnabled(bIsEnabled: true, 1);
		SetFuncBtnEnabled(bIsEnabled: false, 2);
		for (int i = 0; i < 15; i++)
		{
			curPersonBet[i] = 0;
			lastPersonBet[i] = 0;
			lastValidPersonBet[i] = 0;
			bBetZero[i] = true;
			betBtnItems[i].txtPersonBet.text = "0";
			betBtnItems[i].txtTotalBet.text = "0";
			if (i < 12)
			{
				betBtnItems[i].txtPower.text = "0";
			}
		}
		bDeskSettingChanged = false;
		bScorePoor = false;
		bAllBetZero = true;
	}

	public void ClickBtnChip(int index)
	{
		if (index != curChipIndex)
		{
			DP_SoundManager.GetSingleton().playButtonSound();
			curChipIndex = index;
			curChipValue = mChipValue[index];
			tfCurChip.transform.localPosition = btnChips[index].transform.localPosition;
		}
	}

	public void ClickBtnBetItem(int index)
	{
		if (DP_GameData.timeCD <= 0)
		{
			return;
		}
		if (index > 14 || index < 0)
		{
			UnityEngine.Debug.Log("押注对象错误：betItem" + index);
			return;
		}
		int gameScore = mGameInfo.GameScore;
		int nMinBet = 0;
		int nMaxBet = 0;
		if (mTableInfo != null)
		{
			nMinBet = ((index > 11) ? mTableInfo.MinZHXBet : mTableInfo.MinBet);
			nMaxBet = GetMaxMinBet(index, isMax: true);
		}
		if (gameScore <= 0)
		{
			ShowScoreTip();
			DP_SoundManager.GetSingleton().playButtonSound(DP_SoundManager.EUIBtnSoundType.BetFail);
			return;
		}
		lastPersonBet[index] = CalculateOnceBet(ref bBetZero[index], gameScore, nMinBet, nMaxBet, index);
		if (lastPersonBet[index] > 0)
		{
			bDeskSettingChanged = false;
			bScorePoor = false;
			bAllBetZero = false;
			DP_SoundManager.GetSingleton().playButtonSound(DP_SoundManager.EUIBtnSoundType.BetSuccess);
			imgBetFrameLight.color = Color.white;
			imgBetFrameLight.transform.localPosition = betBtnItems[index].transform.localPosition;
			Color col = Color.white;
			col.a = 0f;
			imgBetFrameLight.transform.DOScale(1f, 0.2f).OnComplete(delegate
			{
				imgBetFrameLight.color = col;
			});
			curPersonBet[index] += lastPersonBet[index];
			betBtnItems[index].txtPersonBet.text = curPersonBet[index].ToString();
			if (!bAuto)
			{
				SetFuncBtnEnabled(bIsEnabled: true, 0);
				SetFuncBtnEnabled(bIsEnabled: true, 2);
			}
			SetFuncBtnEnabled(bIsEnabled: false, 1);
			DP_GameData.bet += lastPersonBet[index];
			DP_NetMngr.GetSingleton().MyCreateSocket.SendUserBet(index, lastPersonBet[index], mTableInfo.TableServerID);
		}
		else
		{
			DP_SoundManager.GetSingleton().playButtonSound(DP_SoundManager.EUIBtnSoundType.BetFail);
		}
	}

	public void ClickBtnCancel()
	{
		if (DP_GameData.timeCD > 0)
		{
			bAllBetZero = true;
			DP_SoundManager.GetSingleton().playButtonSound();
			for (int i = 0; i < 12; i++)
			{
				curPersonBet[i] = 0;
				bBetZero[i] = true;
				betBtnItems[i].txtPersonBet.text = "0";
			}
			DP_NetMngr.GetSingleton().MyCreateSocket.SendCancelBet(mTableInfo.TableServerID);
			DP_GameData.bet = 0;
			SetFuncBtnEnabled(bIsEnabled: false, 0);
			SetFuncBtnEnabled(bIsEnabled: false, 2);
			int j;
			for (j = 0; j < 12 && lastValidPersonBet[j] == 0; j++)
			{
			}
			if (j < 12)
			{
				SetFuncBtnEnabled(bIsEnabled: true, 1);
			}
		}
	}

	public void ClickBtnXuya()
	{
		if (DP_GameData.timeCD <= 0)
		{
			return;
		}
		if (IsAllBetSuccess())
		{
			for (int i = 0; i < 12; i++)
			{
				DP_NetMngr.GetSingleton().MyCreateSocket.SendUserBet(i, lastPersonBet[i], mTableInfo.TableServerID);
				DP_GameData.bet += lastPersonBet[i];
			}
			bAllBetZero = false;
			SetFuncBtnEnabled(bIsEnabled: false, 1);
			SetFuncBtnEnabled(bIsEnabled: true, 0);
			SetFuncBtnEnabled(bIsEnabled: true, 2);
			DP_SoundManager.GetSingleton().playButtonSound();
		}
		else if (bScorePoor)
		{
			ShowScoreTip();
		}
		else
		{
			SetFuncBtnEnabled(bIsEnabled: false, 1);
		}
	}

	public void ClickBtnAuto()
	{
		DP_SoundManager.GetSingleton().playButtonSound();
		if (!bAuto)
		{
			bAuto = true;
			txtBtnAuto.text = ((DP_GameInfo.getInstance().Language == 0) ? "取消自动" : "Manual");
			SetFuncBtnEnabled(bIsEnabled: false, 0);
			SetFuncBtnEnabled(bIsEnabled: false, 1);
			return;
		}
		bAuto = false;
		txtBtnAuto.text = ((DP_GameInfo.getInstance().Language == 0) ? "自动" : "Auto");
		if (DP_GameData.timeCD > 0)
		{
			SetFuncBtnEnabled(bIsEnabled: true, 0);
		}
	}

	private bool IsAllBetSuccess()
	{
		int num = 0;
		int gameScore = mGameInfo.GameScore;
		if (gameScore <= 0)
		{
			ShowScoreTip();
		}
		for (int i = 0; i < 15; i++)
		{
			int maxMinBet = GetMaxMinBet(i, isMax: true);
			int maxMinBet2 = GetMaxMinBet(i, isMax: false);
			if (lastValidPersonBet[i] != 0 && (lastValidPersonBet[i] > maxMinBet || lastValidPersonBet[i] < maxMinBet2))
			{
				bDeskSettingChanged = true;
				break;
			}
		}
		if (!bDeskSettingChanged)
		{
			for (int j = 0; j < 12; j++)
			{
				lastPersonBet[j] = lastValidPersonBet[j];
				betBtnItems[j].txtPersonBet.text = lastValidPersonBet[j].ToString();
				num += lastPersonBet[j];
			}
			if (num > gameScore)
			{
				for (int k = 0; k < 12; k++)
				{
					betBtnItems[k].txtPersonBet.text = "0";
				}
				bScorePoor = true;
			}
			else
			{
				for (int l = 0; l < 12; l++)
				{
					curPersonBet[l] += lastPersonBet[l];
					if (curPersonBet[l] != 0)
					{
						bBetZero[l] = false;
					}
				}
				bScorePoor = false;
			}
		}
		return !bDeskSettingChanged && !bScorePoor;
	}

	private int CalculateOnceBet(ref bool bIsStarted, int nUserScore, int nMinBet, int nMaxBet, int iLocateId)
	{
		int num;
		if (bIsStarted)
		{
			bIsStarted = false;
			num = ((nUserScore > nMinBet && curChipValue > nMinBet) ? curChipValue : ((nUserScore > nMinBet) ? nMinBet : nUserScore));
		}
		else
		{
			num = ((nUserScore < curChipValue) ? nUserScore : curChipValue);
			if (curPersonBet[iLocateId] == nMaxBet)
			{
				num = 0;
			}
			else if (curPersonBet[iLocateId] + num >= nMaxBet)
			{
				num = nMaxBet - curPersonBet[iLocateId];
			}
		}
		return num;
	}

	private void SetFuncBtnEnabled(bool bIsEnabled, int iTypeId)
	{
		switch (iTypeId)
		{
		case 0:
			btnCancelBet.SetBtnActive(bIsEnabled);
			break;
		case 1:
			btnXuyaBet.SetBtnActive(bIsEnabled);
			break;
		case 2:
			btnAuto.SetBtnActive(bIsEnabled);
			break;
		}
	}

	private int GetMaxMinBet(int iLocate, bool isMax)
	{
		int result = 0;
		if (mTableInfo != null)
		{
			result = ((iLocate <= 11) ? (isMax ? mTableInfo.MaxBet : mTableInfo.MinBet) : ((iLocate != 12 && iLocate != 14) ? (isMax ? mTableInfo.MaxHBet : mTableInfo.MinZHXBet) : (isMax ? mTableInfo.MaxZXBet : mTableInfo.MinZHXBet)));
		}
		return result;
	}

	public void ShowScoreTip()
	{
		objScoreTip.SetActive(value: true);
		objScoreTip.transform.DOKill();
		objScoreTip.transform.DOScale(1f, 2f).OnComplete(delegate
		{
			objScoreTip.SetActive(value: false);
		});
	}
}
