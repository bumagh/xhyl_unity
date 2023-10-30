using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSF_DiceGameController2 : CSF_MB_Singleton<CSF_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	private GameObject _goUIContainer;

	private CSF_BetGoldAreaController betGoldAreaController;

	private CSF_DicesController dicesController;

	private CSF_DiceNPCsController npcsController;

	private CSF_DiceTips diceTips;

	private Text txtCredit;

	private Text txtTotalBet;

	private Text txtTotalWin;

	private Button btnHalveWager;

	private Button btnDoubleWager;

	private Button btnReady;

	private CSF_SpiAnim animEffGold;

	private Image imgForBtnReady;

	private Image imgCurtain;

	private CSF_BetDiceType lastBetType;

	private CSF_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	private void Init()
	{
		_goUIContainer = base.gameObject;
		betGoldAreaController = base.transform.Find("BetGoldArea").GetComponent<CSF_BetGoldAreaController>();
		dicesController = base.transform.Find("TableBg").GetComponent<CSF_DicesController>();
		npcsController = base.transform.GetComponent<CSF_DiceNPCsController>();
		diceTips = base.transform.Find("ImgTip").GetComponent<CSF_DiceTips>();
		txtCredit = base.transform.Find("DownBg/TxtCredit").GetComponent<Text>();
		txtTotalBet = base.transform.Find("DownBg/TxtTotalBet").GetComponent<Text>();
		txtTotalWin = base.transform.Find("DownBg/TxtTotalWin").GetComponent<Text>();
		btnHalveWager = base.transform.Find("DownBg/BtnSub").GetComponent<Button>();
		btnDoubleWager = base.transform.Find("DownBg/BtnPlus").GetComponent<Button>();
		btnReady = base.transform.Find("DownBg/BtnExit").GetComponent<Button>();
		animEffGold = base.transform.Find("BetGoldArea/Bet/ImgEff").GetComponent<CSF_SpiAnim>();
		imgForBtnReady = base.transform.Find("DownBg/ImgBtnExit").GetComponent<Image>();
		imgCurtain = base.transform.parent.Find("ImgCurtain").GetComponent<Image>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		Init();
		if (CSF_MB_Singleton<CSF_DiceGameController2>._instance == null)
		{
			CSF_MB_Singleton<CSF_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		CSF_MB_Singleton<CSF_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
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
		CSF_LockManager.UnLock("ScoreBank", force: true);
		CSF_LockManager.UnLock("btn_options", force: true);
		CSF_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		CSF_MB_Singleton<CSF_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		overflowNum = CSF_MySqlConnection.desk.diceOverflow;
	}

	public void Show()
	{
		CSF_MB_Singleton<CSF_GameManager>.GetInstance().ChangeView("DiceGame");
		CSF_Utils.TrySetActive(_goUIContainer, active: true);
		CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (CSF_MySqlConnection.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		if (CSF_MySqlConnection.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		CSF_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		CSF_StateManager.GetInstance().ClearWork("MultipleInfo");
		CSF_SoundManager.Instance.StopDealerAudio();
		CSF_SoundManager.Instance.StopGuysAudio();
		CSF_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		CSF_LockManager.UnLock("ScoreBank", force: true);
		CSF_LockManager.UnLock("btn_options", force: true);
		CSF_MB_Singleton<CSF_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		if (totalWin != 0)
		{
			CSF_MySqlConnection.credit = credit + totalWin;
		}
		else
		{
			CSF_MySqlConnection.credit = credit + totalBet;
		}
		CSF_MySqlConnection.realCredit = CSF_MySqlConnection.credit;
		CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().SetKeepScore(0);
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
		CSF_BetDiceType betType = CalcDiceType(diceA + diceB);
		CSF_DiceGameResultType resultType = (totalWin == 0) ? CSF_DiceGameResultType.Lose : (overflow ? CSF_DiceGameResultType.Overflow : CSF_DiceGameResultType.Win);
		int rate = CalcWinRate(lastBetType, diceA, diceB);
		UnityEngine.Debug.Log("resultType: " + resultType);
		lastResult = resultType;
		CSF_SoundManager.Instance.PlayDiceResultAudio(new int[2]
		{
			diceA,
			diceB
		});
		StartCoroutine(npcsController.Open(resultType));
		yield return new WaitForSeconds(0.2f);
		dicesController.SetSmallDices(diceA, diceB);
		yield return new WaitForSeconds(0.8f);
		dicesController.ShowBigDices(diceA, diceB, betType);
		CSF_DiceHistory._Instance.ShowPlates(betType);
		if (resultType == CSF_DiceGameResultType.Lose)
		{
			betGoldAreaController.SetSingleBetImage(betType);
			betGoldAreaController.SetGoldLayerEnable(isEnable: false);
			yield return new WaitForSeconds(0.5f);
			betGoldAreaController.SetSingleBetImage(betType, isEnable: false);
			CSF_MB_Singleton<CSF_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "ScoreClearing");
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
			StartCoroutine(CSF_Utils.ShiftReduceTextAni(0f, 0.05f, totalBet, delegate(int total, int diff)
			{
				txtTotalBet.text = (totalBet - total).ToString();
			}, delegate
			{
			}));
			int betNum = totalBet;
			yield return StartCoroutine(CSF_Utils.ShiftReduceTextAni(0.05f, 0.05f, totalBet, delegate(int total, int diff)
			{
				this.totalWin += rate * diff;
				txtTotalWin.text = this.totalWin.ToString();
				betNum += (rate - 1) * diff;
				betGoldAreaController.SetBetNum(betNum);
			}, delegate
			{
				CSF_MB_Singleton<CSF_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "ScoreClearing");
				CSF_LockManager.UnLock("ScoreBank", force: true);
				CSF_LockManager.UnLock("btn_options", force: true);
				CSF_MB_Singleton<CSF_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
			}));
			if (resultType == CSF_DiceGameResultType.Win)
			{
				dicesController.HideBigDices();
				dicesController.HideSmallDices();
				diceTips.Show();
				diceTips.PlayNormal();
				SetBtnReadyEnable(isEnable: true);
				SetBtnReadyState("ready");
			}
			if (resultType == CSF_DiceGameResultType.Overflow)
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
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			betGoldAreaController.ShowBet(CSF_BetDiceType.Small, totalBet);
			Send_MultipleInfo(CSF_BetDiceType.Small);
			BtnBetClickCommon();
			CSF_MB_Singleton<CSF_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			betGoldAreaController.ShowBet(CSF_BetDiceType.Middle, totalBet);
			Send_MultipleInfo(CSF_BetDiceType.Middle);
			BtnBetClickCommon();
			CSF_MB_Singleton<CSF_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			betGoldAreaController.ShowBet(CSF_BetDiceType.Big, totalBet);
			Send_MultipleInfo(CSF_BetDiceType.Big);
			BtnBetClickCommon();
			CSF_MB_Singleton<CSF_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		if (!CSF_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			UnityEngine.Debug.LogError("网络异常，请继续游戏");
			CSF_StateManager.GetInstance().CompleteWork("MultipleInfo");
			CSF_MB_Singleton<CSF_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			CSF_MB_Singleton<CSF_AlertDialog>.GetInstance().ShowDialog((CSF_MySqlConnection.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		CSF_SoundManager.Instance.PlayClickAudio();
		CSF_SoundManager.bShouldStop = true;
		CSF_SoundManager.Instance.StopGuysAudio();
		CSF_SoundManager.Instance.StopDealerAudio();
		CSF_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		SetBtnDoubleWagerEnable(isEnable: false);
		SetBtnHalveWagerEnable(isEnable: false);
		SetBtnReadyEnable(isEnable: false);
		CSF_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			CSF_SoundManager.Instance.PlayClickAudio();
			SetBtnDoubleWagerEnable(isEnable: false);
			SetBtnHalveWagerEnable(isEnable: false);
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				CSF_MySqlConnection.credit = credit;
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
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			CSF_SoundManager.Instance.PlayClickAudio();
			SetBtnDoubleWagerEnable(isEnable: false);
			SetBtnHalveWagerEnable(isEnable: false);
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				CSF_MySqlConnection.credit = credit;
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
		if (CSF_MySqlConnection.tryLockOnePoint)
		{
			return;
		}
		CSF_SoundManager.Instance.PlayClickAudio();
		if (CSF_LockManager.IsLocked("btn_ready"))
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
			if (lastResult == CSF_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			diceTips.Hide();
			betGoldAreaController.Init();
			StartCoroutine(RoundStart());
			CSF_LockManager.Lock("btn_ready");
			StartCoroutine(CSF_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				SetBtnDoubleWagerEnable(isEnable: true);
				SetBtnHalveWagerEnable(isEnable: true);
				JudgeBtnDoubleAndHalveState();
				CSF_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(CSF_BetDiceType type)
	{
		object[] args = new object[3]
		{
			CSF_MySqlConnection.desk.id,
			(int)type,
			totalBet
		};
		CSF_MB_Singleton<CSF_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		CSF_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			CSF_MySqlConnection.desk.id
		};
		CSF_MB_Singleton<CSF_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.Log("HandleNetMsg_MultipResult");
		CSF_StateManager.GetInstance().CompleteWork("MultipleInfo");
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
		CSF_BetDiceType cSF_BetDiceType = CalcDiceType(array2[0] + array2[1]);
		int num2;
		if (cSF_BetDiceType != lastBetType)
		{
			num2 = 0;
			CSF_MySqlConnection.realCredit = CSF_MySqlConnection.credit;
		}
		else
		{
			num2 = CalcWinRate(cSF_BetDiceType, array2[0], array2[1]) * totalBet;
			flag2 = (num2 >= overflowNum);
			CSF_MySqlConnection.realCredit = CSF_MySqlConnection.credit + num;
			CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().SetKeepScore(num);
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
		else if (CSF_MySqlConnection.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				CSF_Utils.TrySetActive(_goUIContainer, active: false);
				CSF_SoundManager.Instance.StopDealerAudio();
				CSF_SoundManager.Instance.StopGuysAudio();
				CSF_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (CSF_MySqlConnection.curView == "MajorGame")
		{
			CSF_Utils.TrySetActive(_goUIContainer, active: false);
			CSF_SoundManager.Instance.StopDealerAudio();
			CSF_SoundManager.Instance.StopGuysAudio();
			CSF_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().SetKeepScore(newBet);
		txtTotalBet.text = newBet.ToString();
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		CSF_MySqlConnection.credit = newCredit;
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

	private int CalcWinRate(CSF_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		CSF_BetDiceType cSF_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (cSF_BetDiceType == betType)
		{
			result = ((betType == CSF_BetDiceType.Middle) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private CSF_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum < 7) ? CSF_BetDiceType.Small : ((diceSum == 7) ? CSF_BetDiceType.Middle : CSF_BetDiceType.Big);
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
			SetCredit(CSF_MySqlConnection.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
