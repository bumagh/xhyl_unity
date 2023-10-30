using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPA_DiceGameController2 : SPA_MB_Singleton<SPA_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	private GameObject _goUIContainer;

	private SPA_BetGoldAreaController betGoldAreaController;

	private Image imgCurtain;

	private SPA_BetDiceType lastBetType;

	private SPA_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	public long Gamelewinid;

	private void Init()
	{
		_goUIContainer = base.gameObject;
		betGoldAreaController = base.transform.Find("BetGoldArea").GetComponent<SPA_BetGoldAreaController>();
		imgCurtain = base.transform.parent.Find("ImgCurtain").GetComponent<Image>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		Init();
		if (SPA_MB_Singleton<SPA_DiceGameController2>._instance == null)
		{
			SPA_MB_Singleton<SPA_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		SPA_MB_Singleton<SPA_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
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
		SPA_LockManager.UnLock("ScoreBank", force: true);
		SPA_LockManager.UnLock("btn_options", force: true);
		SPA_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		SPA_MB_Singleton<SPA_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		overflowNum = SPA_GVars.desk.diceOverflow;
	}

	public void Show()
	{
		SPA_MB_Singleton<SPA_GameManager>.GetInstance().ChangeView("DiceGame");
		SPA_Utils.TrySetActive(_goUIContainer, active: true);
		SPA_MB_Singleton<SPA_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (SPA_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		SPA_Utils.TrySetActive(_goUIContainer, active: false);
		if (SPA_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		SPA_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		SPA_StateManager.GetInstance().ClearWork("MultipleInfo");
		SPA_SoundManager.Instance.StopDealerAudio();
		SPA_SoundManager.Instance.StopGuysAudio();
		SPA_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		SPA_LockManager.UnLock("ScoreBank", force: true);
		SPA_LockManager.UnLock("btn_options", force: true);
		SPA_MB_Singleton<SPA_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		if (totalWin != 0)
		{
			SPA_GVars.credit = credit + totalWin;
		}
		else
		{
			SPA_GVars.credit = credit + totalBet;
		}
		SPA_GVars.realCredit = SPA_GVars.credit;
		SPA_MB_Singleton<SPA_ScoreBank>.GetInstance().SetKeepScore(0);
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
		if (!SPA_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(SPA_BetDiceType.GetScore);
			BtnBetClickCommon();
			SPA_MB_Singleton<SPA_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!SPA_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(SPA_BetDiceType.Half);
			BtnBetClickCommon();
			SPA_MB_Singleton<SPA_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!SPA_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(SPA_BetDiceType.Risk);
			BtnBetClickCommon();
			SPA_MB_Singleton<SPA_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		if (!SPA_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			UnityEngine.Debug.LogError("网络异常，请继续游戏");
			SPA_StateManager.GetInstance().CompleteWork("MultipleInfo");
			SPA_MB_Singleton<SPA_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			SPA_MB_Singleton<SPA_AlertDialog>.GetInstance().ShowDialog((SPA_GVars.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		SPA_SoundManager.Instance.PlayClickAudio();
		SPA_SoundManager.bShouldStop = true;
		SPA_SoundManager.Instance.StopGuysAudio();
		SPA_SoundManager.Instance.StopDealerAudio();
		SPA_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		SPA_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!SPA_GVars.tryLockOnePoint)
		{
			SPA_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				SPA_GVars.credit = credit;
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
		if (!SPA_GVars.tryLockOnePoint)
		{
			SPA_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				SPA_GVars.credit = credit;
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
		if (SPA_GVars.tryLockOnePoint)
		{
			return;
		}
		SPA_SoundManager.Instance.PlayClickAudio();
		if (SPA_LockManager.IsLocked("btn_ready"))
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
			if (lastResult == SPA_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			StartCoroutine(RoundStart());
			SPA_LockManager.Lock("btn_ready");
			StartCoroutine(SPA_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				JudgeBtnDoubleAndHalveState();
				SPA_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(SPA_BetDiceType type)
	{
		object[] args = new object[2]
		{
			SPA_GVars.desk.id,
			(int)type
		};
		SPA_MB_Singleton<SPA_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		SPA_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			SPA_GVars.desk.id
		};
		SPA_MB_Singleton<SPA_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.LogError("MultipResult: " + JsonMapper.ToJson(args));
		SPA_StateManager.GetInstance().CompleteWork("MultipleInfo");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string s = dictionary["card"].ToString();
		Gamelewinid = long.Parse(s);
		SPA_GamblePanel.ins.AllScroce = (int)dictionary["userScore"];
		SPA_GamblePanel.ins.Gamlewin = (int)dictionary["winScore"];
		SPA_GamblePanel.ins.isGamble = true;
		SPA_GamblePanel.ins.Showgamelegame(isshow: true, SPA_GamblePanel.ins.Gamlewin);
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
		else if (SPA_GVars.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				SPA_Utils.TrySetActive(_goUIContainer, active: false);
				SPA_SoundManager.Instance.StopDealerAudio();
				SPA_SoundManager.Instance.StopGuysAudio();
				SPA_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (SPA_GVars.curView == "MajorGame")
		{
			SPA_Utils.TrySetActive(_goUIContainer, active: false);
			SPA_SoundManager.Instance.StopDealerAudio();
			SPA_SoundManager.Instance.StopGuysAudio();
			SPA_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		SPA_MB_Singleton<SPA_ScoreBank>.GetInstance().SetKeepScore(newBet);
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		SPA_GVars.credit = newCredit;
	}

	private void SetTotalWin(int newWin)
	{
		totalWin = newWin;
	}

	private void SetBtnReadyState(string state)
	{
		strBtnReadyState = state;
	}

	private int CalcWinRate(SPA_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		SPA_BetDiceType sPA_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (sPA_BetDiceType == betType)
		{
			result = ((betType == SPA_BetDiceType.Half) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private SPA_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum >= 7) ? ((diceSum == 7) ? SPA_BetDiceType.Half : SPA_BetDiceType.Risk) : SPA_BetDiceType.GetScore;
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
			SetCredit(SPA_GVars.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
