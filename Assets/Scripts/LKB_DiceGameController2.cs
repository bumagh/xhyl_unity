using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LKB_DiceGameController2 : LKB_MB_Singleton<LKB_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	private GameObject _goUIContainer;

	private LKB_BetGoldAreaController betGoldAreaController;

	private LKB_DicesController dicesController;

	private LKB_DiceNPCsController npcsController;

	private LKB_DiceTips diceTips;

	private Text txtCredit;

	private Text txtTotalBet;

	private Text txtTotalWin;

	private Button btnHalveWager;

	private Button btnDoubleWager;

	private Button btnReady;

	private LKB_SpiAnim animEffGold;

	private Image imgForBtnReady;

	private Image imgCurtain;

	private LKB_BetDiceType lastBetType;

	private LKB_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	private void Init()
	{
		_goUIContainer = base.gameObject;
		betGoldAreaController = base.transform.Find("BetGoldArea").GetComponent<LKB_BetGoldAreaController>();
		dicesController = base.transform.Find("TableBg").GetComponent<LKB_DicesController>();
		npcsController = base.transform.GetComponent<LKB_DiceNPCsController>();
		diceTips = base.transform.Find("ImgTip").GetComponent<LKB_DiceTips>();
		txtCredit = base.transform.Find("DownBg/TxtCredit").GetComponent<Text>();
		txtTotalBet = base.transform.Find("DownBg/TxtTotalBet").GetComponent<Text>();
		txtTotalWin = base.transform.Find("DownBg/TxtTotalWin").GetComponent<Text>();
		btnHalveWager = base.transform.Find("DownBg/BtnSub").GetComponent<Button>();
		btnDoubleWager = base.transform.Find("DownBg/BtnPlus").GetComponent<Button>();
		btnReady = base.transform.Find("DownBg/BtnExit").GetComponent<Button>();
		animEffGold = base.transform.Find("BetGoldArea/Bet/ImgEff").GetComponent<LKB_SpiAnim>();
		imgForBtnReady = base.transform.Find("DownBg/ImgBtnExit").GetComponent<Image>();
		imgCurtain = base.transform.parent.Find("ImgCurtain").GetComponent<Image>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		Init();
		if (LKB_MB_Singleton<LKB_DiceGameController2>._instance == null)
		{
			LKB_MB_Singleton<LKB_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		LKB_MB_Singleton<LKB_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
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
		LKB_LockManager.UnLock("ScoreBank", force: true);
		LKB_LockManager.UnLock("btn_options", force: true);
		LKB_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		LKB_MB_Singleton<LKB_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		overflowNum = LKB_GVars.desk.diceOverflow;
	}

	public void Show()
	{
		LKB_MB_Singleton<LKB_GameManager>.GetInstance().ChangeView("DiceGame");
		LKB_Utils.TrySetActive(_goUIContainer, active: true);
		LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (LKB_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		if (LKB_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		LKB_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		LKB_StateManager.GetInstance().ClearWork("MultipleInfo");
		LKB_SoundManager.Instance.StopDealerAudio();
		LKB_SoundManager.Instance.StopGuysAudio();
		LKB_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		LKB_LockManager.UnLock("ScoreBank", force: true);
		LKB_LockManager.UnLock("btn_options", force: true);
		LKB_MB_Singleton<LKB_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		if (totalWin != 0)
		{
			LKB_GVars.credit = credit + totalWin;
		}
		else
		{
			LKB_GVars.credit = credit + totalBet;
		}
		LKB_GVars.realCredit = LKB_GVars.credit;
		LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().SetKeepScore(0);
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
		LKB_BetDiceType betType = CalcDiceType(diceA + diceB);
		LKB_DiceGameResultType resultType = (totalWin == 0) ? LKB_DiceGameResultType.Lose : (overflow ? LKB_DiceGameResultType.Overflow : LKB_DiceGameResultType.Win);
		int rate = CalcWinRate(lastBetType, diceA, diceB);
		UnityEngine.Debug.Log("resultType: " + resultType);
		lastResult = resultType;
		LKB_SoundManager.Instance.PlayDiceResultAudio(new int[2]
		{
			diceA,
			diceB
		});
		StartCoroutine(npcsController.Open(resultType));
		yield return new WaitForSeconds(0.2f);
		dicesController.SetSmallDices(diceA, diceB);
		yield return new WaitForSeconds(0.8f);
		dicesController.ShowBigDices(diceA, diceB, betType);
		LKB_DiceHistory._Instance.ShowPlates(betType);
		if (resultType == LKB_DiceGameResultType.Lose)
		{
			betGoldAreaController.SetSingleBetImage(betType);
			betGoldAreaController.SetGoldLayerEnable(isEnable: false);
			yield return new WaitForSeconds(0.5f);
			betGoldAreaController.SetSingleBetImage(betType, isEnable: false);
			LKB_MB_Singleton<LKB_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "ScoreClearing");
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
			StartCoroutine(LKB_Utils.ShiftReduceTextAni(0f, 0.05f, totalBet, delegate(int total, int diff)
			{
				txtTotalBet.text = (totalBet - total).ToString();
			}, delegate
			{
			}));
			int betNum = totalBet;
			yield return StartCoroutine(LKB_Utils.ShiftReduceTextAni(0.05f, 0.05f, totalBet, delegate(int total, int diff)
			{
				this.totalWin += rate * diff;
				txtTotalWin.text = this.totalWin.ToString();
				betNum += (rate - 1) * diff;
				betGoldAreaController.SetBetNum(betNum);
			}, delegate
			{
				LKB_MB_Singleton<LKB_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "ScoreClearing");
				LKB_LockManager.UnLock("ScoreBank", force: true);
				LKB_LockManager.UnLock("btn_options", force: true);
				LKB_MB_Singleton<LKB_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
			}));
			if (resultType == LKB_DiceGameResultType.Win)
			{
				dicesController.HideBigDices();
				dicesController.HideSmallDices();
				diceTips.Show();
				diceTips.PlayNormal();
				SetBtnReadyEnable(isEnable: true);
				SetBtnReadyState("ready");
			}
			if (resultType == LKB_DiceGameResultType.Overflow)
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
		if (!LKB_GVars.tryLockOnePoint)
		{
			betGoldAreaController.ShowBet(LKB_BetDiceType.Small, totalBet);
			Send_MultipleInfo(LKB_BetDiceType.Small);
			BtnBetClickCommon();
			LKB_MB_Singleton<LKB_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!LKB_GVars.tryLockOnePoint)
		{
			betGoldAreaController.ShowBet(LKB_BetDiceType.Middle, totalBet);
			Send_MultipleInfo(LKB_BetDiceType.Middle);
			BtnBetClickCommon();
			LKB_MB_Singleton<LKB_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!LKB_GVars.tryLockOnePoint)
		{
			betGoldAreaController.ShowBet(LKB_BetDiceType.Big, totalBet);
			Send_MultipleInfo(LKB_BetDiceType.Big);
			BtnBetClickCommon();
			LKB_MB_Singleton<LKB_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		if (!LKB_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			UnityEngine.Debug.LogError("网络异常，请继续游戏");
			LKB_StateManager.GetInstance().CompleteWork("MultipleInfo");
			LKB_MB_Singleton<LKB_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			LKB_MB_Singleton<LKB_AlertDialog>.GetInstance().ShowDialog((LKB_GVars.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		LKB_SoundManager.Instance.PlayClickAudio();
		LKB_SoundManager.bShouldStop = true;
		LKB_SoundManager.Instance.StopGuysAudio();
		LKB_SoundManager.Instance.StopDealerAudio();
		LKB_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		SetBtnDoubleWagerEnable(isEnable: false);
		SetBtnHalveWagerEnable(isEnable: false);
		SetBtnReadyEnable(isEnable: false);
		LKB_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!LKB_GVars.tryLockOnePoint)
		{
			LKB_SoundManager.Instance.PlayClickAudio();
			SetBtnDoubleWagerEnable(isEnable: false);
			SetBtnHalveWagerEnable(isEnable: false);
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				LKB_GVars.credit = credit;
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
		if (!LKB_GVars.tryLockOnePoint)
		{
			LKB_SoundManager.Instance.PlayClickAudio();
			SetBtnDoubleWagerEnable(isEnable: false);
			SetBtnHalveWagerEnable(isEnable: false);
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				LKB_GVars.credit = credit;
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
		if (LKB_GVars.tryLockOnePoint)
		{
			return;
		}
		LKB_SoundManager.Instance.PlayClickAudio();
		if (LKB_LockManager.IsLocked("btn_ready"))
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
			if (lastResult == LKB_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			diceTips.Hide();
			betGoldAreaController.Init();
			StartCoroutine(RoundStart());
			LKB_LockManager.Lock("btn_ready");
			StartCoroutine(LKB_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				SetBtnDoubleWagerEnable(isEnable: true);
				SetBtnHalveWagerEnable(isEnable: true);
				JudgeBtnDoubleAndHalveState();
				LKB_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(LKB_BetDiceType type)
	{
		object[] args = new object[3]
		{
			LKB_GVars.desk.id,
			(int)type,
			totalBet
		};
		LKB_MB_Singleton<LKB_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		LKB_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			LKB_GVars.desk.id
		};
		LKB_MB_Singleton<LKB_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.Log("HandleNetMsg_MultipResult");
		LKB_StateManager.GetInstance().CompleteWork("MultipleInfo");
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
		LKB_BetDiceType lKB_BetDiceType = CalcDiceType(array2[0] + array2[1]);
		int num2;
		if (lKB_BetDiceType != lastBetType)
		{
			num2 = 0;
			LKB_GVars.realCredit = LKB_GVars.credit;
		}
		else
		{
			num2 = CalcWinRate(lKB_BetDiceType, array2[0], array2[1]) * totalBet;
			flag2 = (num2 >= overflowNum);
			LKB_GVars.realCredit = LKB_GVars.credit + num;
			LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().SetKeepScore(num);
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
		else if (LKB_GVars.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				LKB_Utils.TrySetActive(_goUIContainer, active: false);
				LKB_SoundManager.Instance.StopDealerAudio();
				LKB_SoundManager.Instance.StopGuysAudio();
				LKB_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (LKB_GVars.curView == "MajorGame")
		{
			LKB_Utils.TrySetActive(_goUIContainer, active: false);
			LKB_SoundManager.Instance.StopDealerAudio();
			LKB_SoundManager.Instance.StopGuysAudio();
			LKB_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().SetKeepScore(newBet);
		txtTotalBet.text = newBet.ToString();
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		LKB_GVars.credit = newCredit;
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
	}

	private int CalcWinRate(LKB_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		LKB_BetDiceType lKB_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (lKB_BetDiceType == betType)
		{
			result = ((betType == LKB_BetDiceType.Middle) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private LKB_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum < 7) ? LKB_BetDiceType.Small : ((diceSum == 7) ? LKB_BetDiceType.Middle : LKB_BetDiceType.Big);
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
			SetCredit(LKB_GVars.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
