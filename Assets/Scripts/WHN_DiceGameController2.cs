using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WHN_DiceGameController2 : WHN_MB_Singleton<WHN_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	private GameObject _goUIContainer;

	private WHN_BetGoldAreaController betGoldAreaController;

	private Image imgCurtain;

	private WHN_BetDiceType lastBetType;

	private WHN_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	public uint Gamelewinid;

	private void Init()
	{
		_goUIContainer = base.gameObject;
		betGoldAreaController = base.transform.Find("BetGoldArea").GetComponent<WHN_BetGoldAreaController>();
		imgCurtain = base.transform.parent.Find("ImgCurtain").GetComponent<Image>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		Init();
		if (WHN_MB_Singleton<WHN_DiceGameController2>._instance == null)
		{
			WHN_MB_Singleton<WHN_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		WHN_MB_Singleton<WHN_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
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
		SetBtnReadyState("back");
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		isBtnHalveAndDoubleLock = false;
		WHN_LockManager.UnLock("ScoreBank", force: true);
		WHN_LockManager.UnLock("btn_options", force: true);
		WHN_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		overflowNum = WHN_GVars.desk.diceOverflow;
	}

	public void Show()
	{
		WHN_MB_Singleton<WHN_GameManager>.GetInstance().ChangeView("DiceGame");
		WHN_Utils.TrySetActive(_goUIContainer, active: true);
		WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (WHN_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		WHN_Utils.TrySetActive(_goUIContainer, active: false);
		if (WHN_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		WHN_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		WHN_StateManager.GetInstance().ClearWork("MultipleInfo");
		WHN_SoundManager.Instance.StopGuysAudio();
		WHN_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		WHN_LockManager.UnLock("ScoreBank", force: true);
		WHN_LockManager.UnLock("btn_options", force: true);
		if (totalWin != 0)
		{
			WHN_GVars.credit = credit + totalWin;
		}
		else
		{
			WHN_GVars.credit = credit + totalBet;
		}
		WHN_GVars.realCredit = WHN_GVars.credit;
		WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().SetKeepScore(0);
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
		SetBtnReadyState("back");
		UnityEngine.Debug.Log("_roundStart end");
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: true);
		if (!isBtnHalveAndDoubleLock)
		{
			JudgeBtnDoubleAndHalveState();
		}
	}

	public void OnBtnBetSmall_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetSmall");
		if (!WHN_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(WHN_BetDiceType.GetScore);
			BtnBetClickCommon();
			WHN_MB_Singleton<WHN_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!WHN_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(WHN_BetDiceType.Half);
			BtnBetClickCommon();
			WHN_MB_Singleton<WHN_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!WHN_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(WHN_BetDiceType.Risk);
			BtnBetClickCommon();
			WHN_MB_Singleton<WHN_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		if (!WHN_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			UnityEngine.Debug.LogError("网络异常，请继续游戏");
			WHN_StateManager.GetInstance().CompleteWork("MultipleInfo");
			WHN_MB_Singleton<WHN_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			WHN_MB_Singleton<WHN_AlertDialog>.GetInstance().ShowDialog((WHN_GVars.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		WHN_SoundManager.Instance.PlayClickAudio();
		WHN_SoundManager.bShouldStop = true;
		WHN_SoundManager.Instance.StopGuysAudio();
		WHN_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		WHN_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!WHN_GVars.tryLockOnePoint)
		{
			WHN_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				WHN_GVars.credit = credit;
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
		if (!WHN_GVars.tryLockOnePoint)
		{
			WHN_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				WHN_GVars.credit = credit;
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
		if (WHN_GVars.tryLockOnePoint)
		{
			return;
		}
		WHN_SoundManager.Instance.PlayClickAudio();
		if (WHN_LockManager.IsLocked("btn_ready"))
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
			if (lastResult == WHN_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			StartCoroutine(RoundStart());
			WHN_LockManager.Lock("btn_ready");
			StartCoroutine(WHN_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				JudgeBtnDoubleAndHalveState();
				WHN_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(WHN_BetDiceType type)
	{
		object[] args = new object[2]
		{
			WHN_GVars.desk.id,
			(int)type
		};
		WHN_MB_Singleton<WHN_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		WHN_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			WHN_GVars.desk.id
		};
		WHN_MB_Singleton<WHN_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.LogError("MultipResult: " + JsonMapper.ToJson(args));
		WHN_StateManager.GetInstance().CompleteWork("MultipleInfo");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string s = dictionary["card"].ToString();
		Gamelewinid = uint.Parse(s);
		WHN_GamblePanel.ins.AllScroce = (int)dictionary["userScore"];
		WHN_GamblePanel.ins.Gamlewin = (int)dictionary["winScore"];
		WHN_GamblePanel.ins.isGamble = true;
		WHN_GamblePanel.ins.Showgamelegame(isshow: true, WHN_GamblePanel.ins.Gamlewin);
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
		else if (WHN_GVars.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				WHN_Utils.TrySetActive(_goUIContainer, active: false);
				WHN_SoundManager.Instance.StopGuysAudio();
				WHN_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (WHN_GVars.curView == "MajorGame")
		{
			WHN_Utils.TrySetActive(_goUIContainer, active: false);
			WHN_SoundManager.Instance.StopGuysAudio();
			WHN_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().SetKeepScore(newBet);
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		WHN_GVars.credit = newCredit;
	}

	private void SetTotalWin(int newWin)
	{
		totalWin = newWin;
	}

	private void SetBtnReadyState(string state)
	{
		strBtnReadyState = state;
	}

	private int CalcWinRate(WHN_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		WHN_BetDiceType wHN_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (wHN_BetDiceType == betType)
		{
			result = ((betType == WHN_BetDiceType.Half) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private WHN_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum >= 7) ? ((diceSum == 7) ? WHN_BetDiceType.Half : WHN_BetDiceType.Risk) : WHN_BetDiceType.GetScore;
	}

	private void JudgeBtnDoubleAndHalveState()
	{
		if (!isBtnHalveAndDoubleLock)
		{
			int num = (totalBet > 0) ? totalBet : totalWin;
			UnityEngine.Debug.Log($"betNum: {num}, overflowNum: {overflowNum}");
			int num2 = credit + num;
		}
	}

	private void OnSetScoreAndGoldAction()
	{
		if (_goUIContainer.activeInHierarchy)
		{
			SetCredit(WHN_GVars.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
