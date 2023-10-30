using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STWM_DiceGameController2 : STWM_MB_Singleton<STWM_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	[SerializeField]
	private GameObject _goUIContainer;

	[SerializeField]
	private STWM_BetGoldAreaController betGoldAreaController;

	[SerializeField]
	private STWM_DicesController dicesController;

	[SerializeField]
	private STWM_DiceNPCsController npcsController;

	[SerializeField]
	private STWM_DiceTips diceTips;

	[SerializeField]
	private Text txtCredit;

	[SerializeField]
	private Text txtTotalBet;

	[SerializeField]
	private Text txtTotalWin;

	[SerializeField]
	private Button btnHalveWager;

	[SerializeField]
	private Button btnDoubleWager;

	[SerializeField]
	private Button btnReady;

	[SerializeField]
	private STWM_SpiAnim animEffGold;

	[SerializeField]
	private Sprite[] spiForBtnReady = new Sprite[2];

	[SerializeField]
	private Sprite[] spiForBtnReadyEn = new Sprite[2];

	[SerializeField]
	private Image imgForBtnReady;

	[SerializeField]
	private Image imgCurtain;

	private STWM_BetDiceType lastBetType;

	private STWM_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	private void Awake()
	{
		if (STWM_MB_Singleton<STWM_DiceGameController2>._instance == null)
		{
			STWM_MB_Singleton<STWM_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
	}

	public void PreInit()
	{
		if (_goUIContainer == null)
		{
			_goUIContainer = base.gameObject;
		}
	}

	public void InitGame(int credit, int bet)
	{
		if (STWM_MB_Singleton<STWM_NetManager>.GetInstance().useFake)
		{
			credit = 10000;
			bet = 450;
		}
		SetCredit(credit);
		SetTotalBet(bet);
		SetTotalWin(0);
		StartCoroutine(RoundStart(0.5f));
		diceTips.Hide();
		animEffGold.gameObject.SetActive(value: false);
		dicesController.Init();
		betGoldAreaController.Init();
		SetBtnReadyState("back");
		SetBtnReadyEnable(isEnable: false);
		SetBtnDoubleWagerEnable(isEnable: false);
		SetBtnHalveWagerEnable(isEnable: false);
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		isBtnHalveAndDoubleLock = false;
		STWM_LockManager.UnLock("ScoreBank", force: true);
		STWM_LockManager.UnLock("btn_options", force: true);
		STWM_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		STWM_MB_Singleton<STWM_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		overflowNum = STWM_GVars.desk.diceOverflow;
	}

	public void Show()
	{
		STWM_MB_Singleton<STWM_GameManager>.GetInstance().ChangeView("DiceGame");
		STWM_Utils.TrySetActive(_goUIContainer, active: true);
		STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (STWM_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		if (STWM_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		STWM_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		STWM_StateManager.GetInstance().ClearWork("MultipleInfo");
		STWM_SoundManager.Instance.StopDealerAudio();
		STWM_SoundManager.Instance.StopGuysAudio();
		STWM_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		STWM_LockManager.UnLock("ScoreBank", force: true);
		STWM_LockManager.UnLock("btn_options", force: true);
		STWM_MB_Singleton<STWM_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		if (totalWin != 0)
		{
			STWM_GVars.credit = credit + totalWin;
		}
		else
		{
			STWM_GVars.credit = credit + totalBet;
		}
		STWM_GVars.realCredit = STWM_GVars.credit;
		STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().SetKeepScore(0);
		if (onExitAction != null)
		{
			onExitAction();
		}
	}

	private IEnumerator RoundStart(float delay = 0f)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		UnityEngine.Debug.Log("_roundStart");
		yield return null;
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		SetBtnDoubleWagerEnable(isEnable: false);
		SetBtnHalveWagerEnable(isEnable: false);
		SetBtnReadyEnable(isEnable: false);
		SetBtnReadyState("back");
		yield return StartCoroutine(npcsController.Shake());
		yield return StartCoroutine(betGoldAreaController.AllBetImagesBlinkAni());
		UnityEngine.Debug.Log("_roundStart end");
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: true);
		if (!isBtnHalveAndDoubleLock)
		{
			SetBtnDoubleWagerEnable(isEnable: true);
			SetBtnHalveWagerEnable(isEnable: true);
			JudgeBtnDoubleAndHalveState();
		}
		SetBtnReadyEnable(isEnable: true);
		yield return StartCoroutine(npcsController.TouchMustache());
	}

	private IEnumerator RoundOpen(int diceA, int diceB, int totalWin, bool overflow)
	{
		UnityEngine.Debug.Log("_roundOpen");
		STWM_BetDiceType betType = CalcDiceType(diceA + diceB);
		STWM_DiceGameResultType resultType = (totalWin == 0) ? STWM_DiceGameResultType.Lose : (overflow ? STWM_DiceGameResultType.Overflow : STWM_DiceGameResultType.Win);
		int rate = CalcWinRate(lastBetType, diceA, diceB);
		UnityEngine.Debug.Log("resultType: " + resultType);
		lastResult = resultType;
		STWM_SoundManager.Instance.PlayDiceResultAudio(new int[2]
		{
			diceA,
			diceB
		});
		StartCoroutine(npcsController.Open(resultType));
		yield return new WaitForSeconds(0.2f);
		dicesController.SetSmallDices(diceA, diceB);
		yield return new WaitForSeconds(0.8f);
		dicesController.ShowBigDices(diceA, diceB, betType);
		STWM_DiceHistory._Instance.ShowPlates(betType);
		if (resultType == STWM_DiceGameResultType.Lose)
		{
			betGoldAreaController.SetSingleBetImage(betType);
			betGoldAreaController.SetGoldLayerEnable(isEnable: false);
			yield return new WaitForSeconds(0.5f);
			betGoldAreaController.SetSingleBetImage(betType, isEnable: false);
			STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "ScoreClearing");
			txtTotalBet.text = "0";
			SetTotalBet(0);
			yield return new WaitForSeconds(1.5f);
			ExitGame();
		}
		else
		{
			betGoldAreaController.SetSingleBetImage(betType);
			yield return new WaitForSeconds(0.5f);
			betGoldAreaController.SetSingleBetImage(betType, isEnable: false);
			betGoldAreaController.SetGoldLayerEnable(isEnable: false);
			yield return new WaitForSeconds(0.5f);
			betGoldAreaController.SetSingleBetImage(betType);
			betGoldAreaController.SetGoldLayerEnable(isEnable: true);
			yield return new WaitForSeconds(0.5f);
			betGoldAreaController.SetBetGoldEnable(isEnable: false);
			betGoldAreaController.SetBetWinGoldEnable(rate, isEnable: true);
			StartCoroutine(PlayMagicLight());
			StartCoroutine(STWM_Utils.ShiftReduceTextAni(0f, 0.05f, totalBet, delegate(int total, int diff)
			{
				txtTotalBet.text = (totalBet - total).ToString();
			}, delegate
			{
			}));
			int betNum = totalBet;
			yield return StartCoroutine(STWM_Utils.ShiftReduceTextAni(0.05f, 0.05f, totalBet, delegate(int total, int diff)
			{
				this.totalWin += rate * diff;
				txtTotalWin.text = this.totalWin.ToString();
				betNum += (rate - 1) * diff;
				betGoldAreaController.SetBetNum(betNum);
			}, delegate
			{
				STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "ScoreClearing");
				STWM_LockManager.UnLock("ScoreBank", force: true);
				STWM_LockManager.UnLock("btn_options", force: true);
				STWM_MB_Singleton<STWM_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
			}));
			if (resultType == STWM_DiceGameResultType.Win)
			{
				dicesController.HideBigDices();
				dicesController.HideSmallDices();
				diceTips.Show();
				diceTips.PlayNormal();
				SetBtnReadyEnable(isEnable: true);
				SetBtnReadyState("ready");
			}
			if (resultType == STWM_DiceGameResultType.Overflow)
			{
				diceTips.Show();
				diceTips.PlayOverflow();
				UnityEngine.Debug.Log("Particlas" + Time.time);
				betGoldAreaController.SetBetNumEnable(isEnable: false);
				SetBtnReadyEnable(isEnable: true);
				SetBtnReadyState("back");
				yield return new WaitForSeconds(20f);
				ExitGame();
			}
		}
		isBtnHalveAndDoubleLock = false;
		yield return null;
	}

	private IEnumerator PlayMagicLight()
	{
		animEffGold.gameObject.SetActive(value: true);
		animEffGold.PlayOnce(bActive: false);
		yield break;
	}

	public void OnBtnBetSmall_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetSmall");
		if (!STWM_GVars.tryLockOnePoint)
		{
			betGoldAreaController.ShowBet(STWM_BetDiceType.Small, totalBet);
			Send_MultipleInfo(STWM_BetDiceType.Small);
			BtnBetClickCommon();
			STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!STWM_GVars.tryLockOnePoint)
		{
			betGoldAreaController.ShowBet(STWM_BetDiceType.Middle, totalBet);
			Send_MultipleInfo(STWM_BetDiceType.Middle);
			BtnBetClickCommon();
			STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!STWM_GVars.tryLockOnePoint)
		{
			betGoldAreaController.ShowBet(STWM_BetDiceType.Big, totalBet);
			Send_MultipleInfo(STWM_BetDiceType.Big);
			BtnBetClickCommon();
			STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		UnityEngine.Debug.Log("HandleNoResponse_MultiInfo");
		if (!STWM_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			STWM_StateManager.GetInstance().CompleteWork("MultipleInfo");
			STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog((STWM_GVars.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		STWM_SoundManager.Instance.PlayClickAudio();
		STWM_SoundManager.bShouldStop = true;
		STWM_SoundManager.Instance.StopGuysAudio();
		STWM_SoundManager.Instance.StopDealerAudio();
		STWM_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		SetBtnDoubleWagerEnable(isEnable: false);
		SetBtnHalveWagerEnable(isEnable: false);
		SetBtnReadyEnable(isEnable: false);
		STWM_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!STWM_GVars.tryLockOnePoint)
		{
			STWM_SoundManager.Instance.PlayClickAudio();
			SetBtnDoubleWagerEnable(isEnable: false);
			SetBtnHalveWagerEnable(isEnable: false);
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				STWM_GVars.credit = credit;
				SetTotalBet(num);
			}
			else
			{
				UnityEngine.Debug.LogError("totalWin must be zero");
			}
		}
	}

	public void OnBtnDoubleWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnDoubleWager_Click");
		if (!STWM_GVars.tryLockOnePoint)
		{
			STWM_SoundManager.Instance.PlayClickAudio();
			SetBtnDoubleWagerEnable(isEnable: false);
			SetBtnHalveWagerEnable(isEnable: false);
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				STWM_GVars.credit = credit;
				SetTotalBet(num);
			}
			else
			{
				UnityEngine.Debug.LogError("totalWin must be zero");
			}
		}
	}

	public void OnBtnReady_Click()
	{
		UnityEngine.Debug.Log("OnBtnReady_Click");
		if (STWM_GVars.tryLockOnePoint)
		{
			return;
		}
		STWM_SoundManager.Instance.PlayClickAudio();
		if (STWM_LockManager.IsLocked("btn_ready"))
		{
			return;
		}
		if (strBtnReadyState == "back")
		{
			ExitGame();
		}
		else if (!(strBtnReadyState == "ready"))
		{
			UnityEngine.Debug.LogError("_btnReadyState: " + strBtnReadyState);
			if (lastResult == STWM_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			diceTips.Hide();
			betGoldAreaController.Init();
			StartCoroutine(RoundStart());
			STWM_LockManager.Lock("btn_ready");
			StartCoroutine(STWM_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				SetBtnDoubleWagerEnable(isEnable: true);
				SetBtnHalveWagerEnable(isEnable: true);
				JudgeBtnDoubleAndHalveState();
				STWM_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(STWM_BetDiceType type)
	{
		object[] args = new object[3]
		{
			STWM_GVars.desk.id,
			(int)type,
			totalBet
		};
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		STWM_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			STWM_GVars.desk.id
		};
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.Log("HandleNetMsg_MultipResult");
		STWM_StateManager.GetInstance().CompleteWork("MultipleInfo");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int[] array = (int[])dictionary["number"];
		int[] array2 = new int[2]
		{
			array[0],
			array[1]
		};
		int num = (int)dictionary["totalWin"];
		bool flag = (bool)dictionary["overflow"];
		bool flag2 = false;
		if (array2[0] == 7 || array2[1] == 7)
		{
			UnityEngine.Debug.Log("dice is 7");
		}
		UnityEngine.Debug.Log($"dices[0]:{array2[0]}, dices[1]:{array2[1]}");
		STWM_BetDiceType sTWM_BetDiceType = CalcDiceType(array2[0] + array2[1]);
		int num2;
		if (sTWM_BetDiceType != lastBetType)
		{
			num2 = 0;
			STWM_GVars.realCredit = STWM_GVars.credit;
		}
		else
		{
			num2 = CalcWinRate(sTWM_BetDiceType, array2[0], array2[1]) * totalBet;
			flag2 = (num2 >= overflowNum);
			STWM_GVars.realCredit = STWM_GVars.credit + num;
			STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().SetKeepScore(num);
		}
		StartCoroutine(RoundOpen(array2[0], array2[1], num2, flag));
		if (num != num2)
		{
			UnityEngine.Debug.LogError($"totalWin: {num}, clientWin: {num2}");
		}
		if (flag != flag2)
		{
			UnityEngine.Debug.LogError($"overflow: {flag}, clientOverflow: {flag2}, _overflowNum: {overflowNum}");
		}
	}

	private void PullCurtain()
	{
		if (!isCurtainOff)
		{
			imgCurtain.gameObject.SetActive(value: true);
			Color black = Color.black;
			black.a = 0f;
			imgCurtain.DOColor(black, 1f).OnStepComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = true;
			});
		}
		else if (STWM_GVars.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				STWM_Utils.TrySetActive(_goUIContainer, active: false);
				STWM_SoundManager.Instance.StopDealerAudio();
				STWM_SoundManager.Instance.StopGuysAudio();
				STWM_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (STWM_GVars.curView == "MajorGame")
		{
			STWM_Utils.TrySetActive(_goUIContainer, active: false);
			STWM_SoundManager.Instance.StopDealerAudio();
			STWM_SoundManager.Instance.StopGuysAudio();
			STWM_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().SetKeepScore(newBet);
		txtTotalBet.text = newBet.ToString();
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		STWM_GVars.credit = newCredit;
		txtCredit.text = newCredit.ToString();
	}

	private void SetTotalWin(int newWin)
	{
		totalWin = newWin;
		txtTotalWin.text = newWin.ToString();
	}

	private void SetBtnReadyEnable(bool isEnable)
	{
		btnReady.interactable = isEnable;
	}

	private void SetBtnHalveWagerEnable(bool isEnable)
	{
		btnHalveWager.interactable = isEnable;
	}

	private void SetBtnDoubleWagerEnable(bool isEnable)
	{
		btnDoubleWager.interactable = isEnable;
	}

	private void SetBtnReadyState(string state)
	{
		strBtnReadyState = state;
		if (state == "ready")
		{
			if (STWM_GVars.language == "zh")
			{
				imgForBtnReady.sprite = spiForBtnReady[0];
			}
			else
			{
				imgForBtnReady.sprite = spiForBtnReadyEn[0];
			}
		}
		else if (state == "back")
		{
			if (STWM_GVars.language == "zh")
			{
				imgForBtnReady.sprite = spiForBtnReady[1];
			}
			else
			{
				imgForBtnReady.sprite = spiForBtnReadyEn[1];
			}
		}
		imgForBtnReady.SetNativeSize();
	}

	private int CalcWinRate(STWM_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		STWM_BetDiceType sTWM_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (sTWM_BetDiceType == betType)
		{
			result = ((betType == STWM_BetDiceType.Middle) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private STWM_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum < 7) ? STWM_BetDiceType.Small : ((diceSum == 7) ? STWM_BetDiceType.Middle : STWM_BetDiceType.Big);
	}

	private void JudgeBtnDoubleAndHalveState()
	{
		if (!isBtnHalveAndDoubleLock)
		{
			int num = (totalBet > 0) ? totalBet : totalWin;
			UnityEngine.Debug.Log($"betNum: {num}, overflowNum: {overflowNum}");
			int num2 = credit + num;
			SetBtnDoubleWagerEnable(num * 2 <= num2 && num * 2 < overflowNum);
			SetBtnHalveWagerEnable(num > 1);
		}
	}

	private void OnSetScoreAndGoldAction()
	{
		if (_goUIContainer.activeInHierarchy)
		{
			SetCredit(STWM_GVars.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
