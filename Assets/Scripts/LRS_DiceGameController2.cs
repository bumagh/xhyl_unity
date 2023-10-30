using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LRS_DiceGameController2 : LRS_MB_Singleton<LRS_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	private GameObject _goUIContainer;

	private LRS_BetGoldAreaController betGoldAreaController;

	private Image imgCurtain;

	private LRS_BetDiceType lastBetType;

	private LRS_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	public uint Gamelewinid;

	private void Init()
	{
		_goUIContainer = base.gameObject;
		betGoldAreaController = base.transform.Find("BetGoldArea").GetComponent<LRS_BetGoldAreaController>();
		imgCurtain = base.transform.parent.Find("ImgCurtain").GetComponent<Image>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		Init();
		if (LRS_MB_Singleton<LRS_DiceGameController2>._instance == null)
		{
			LRS_MB_Singleton<LRS_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
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
		LRS_LockManager.UnLock("ScoreBank", force: true);
		LRS_LockManager.UnLock("btn_options", force: true);
		LRS_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		overflowNum = LRS_GVars.desk.diceOverflow;
	}

	public void Show()
	{
		LRS_MB_Singleton<LRS_GameManager>.GetInstance().ChangeView("DiceGame");
		LRS_Utils.TrySetActive(_goUIContainer, active: true);
		LRS_MB_Singleton<LRS_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (LRS_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		LRS_Utils.TrySetActive(_goUIContainer, active: false);
		if (LRS_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		LRS_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		LRS_StateManager.GetInstance().ClearWork("MultipleInfo");
		LRS_SoundManager.Instance.StopGuysAudio();
		LRS_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		LRS_LockManager.UnLock("ScoreBank", force: true);
		LRS_LockManager.UnLock("btn_options", force: true);
		if (totalWin != 0)
		{
			LRS_GVars.credit = credit + totalWin;
		}
		else
		{
			LRS_GVars.credit = credit + totalBet;
		}
		LRS_GVars.realCredit = LRS_GVars.credit;
		LRS_MB_Singleton<LRS_ScoreBank>.GetInstance().SetKeepScore(0);
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
		if (!LRS_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(LRS_BetDiceType.GetScore);
			BtnBetClickCommon();
			LRS_MB_Singleton<LRS_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!LRS_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(LRS_BetDiceType.Half);
			BtnBetClickCommon();
			LRS_MB_Singleton<LRS_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!LRS_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(LRS_BetDiceType.Risk);
			BtnBetClickCommon();
			LRS_MB_Singleton<LRS_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		if (!LRS_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			UnityEngine.Debug.LogError("网络异常，请继续游戏");
			LRS_StateManager.GetInstance().CompleteWork("MultipleInfo");
			LRS_MB_Singleton<LRS_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			LRS_MB_Singleton<LRS_AlertDialog>.GetInstance().ShowDialog((LRS_GVars.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		LRS_SoundManager.Instance.PlayClickAudio();
		LRS_SoundManager.bShouldStop = true;
		LRS_SoundManager.Instance.StopGuysAudio();
		LRS_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		LRS_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!LRS_GVars.tryLockOnePoint)
		{
			LRS_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				LRS_GVars.credit = credit;
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
		if (!LRS_GVars.tryLockOnePoint)
		{
			LRS_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				LRS_GVars.credit = credit;
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
		if (LRS_GVars.tryLockOnePoint)
		{
			return;
		}
		LRS_SoundManager.Instance.PlayClickAudio();
		if (LRS_LockManager.IsLocked("btn_ready"))
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
			if (lastResult == LRS_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			StartCoroutine(RoundStart());
			LRS_LockManager.Lock("btn_ready");
			StartCoroutine(LRS_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				JudgeBtnDoubleAndHalveState();
				LRS_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(LRS_BetDiceType type)
	{
		object[] args = new object[2]
		{
			LRS_GVars.desk.id,
			(int)type
		};
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		LRS_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			LRS_GVars.desk.id
		};
		LRS_MB_Singleton<LRS_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.LogError("MultipResult: " + JsonMapper.ToJson(args));
		LRS_StateManager.GetInstance().CompleteWork("MultipleInfo");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string s = dictionary["card"].ToString();
		Gamelewinid = uint.Parse(s);
		LRS_GamblePanel.ins.AllScroce = (int)dictionary["userScore"];
		LRS_GamblePanel.ins.Gamlewin = (int)dictionary["winScore"];
		LRS_GamblePanel.ins.isGamble = true;
		LRS_GamblePanel.ins.Showgamelegame(isshow: true, LRS_GamblePanel.ins.Gamlewin);
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
		else if (LRS_GVars.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				LRS_Utils.TrySetActive(_goUIContainer, active: false);
				LRS_SoundManager.Instance.StopGuysAudio();
				LRS_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (LRS_GVars.curView == "MajorGame")
		{
			LRS_Utils.TrySetActive(_goUIContainer, active: false);
			LRS_SoundManager.Instance.StopGuysAudio();
			LRS_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		LRS_MB_Singleton<LRS_ScoreBank>.GetInstance().SetKeepScore(newBet);
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		LRS_GVars.credit = newCredit;
	}

	private void SetTotalWin(int newWin)
	{
		totalWin = newWin;
	}

	private void SetBtnReadyState(string state)
	{
		strBtnReadyState = state;
	}

	private int CalcWinRate(LRS_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		LRS_BetDiceType lRS_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (lRS_BetDiceType == betType)
		{
			result = ((betType == LRS_BetDiceType.Half) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private LRS_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum >= 7) ? ((diceSum == 7) ? LRS_BetDiceType.Half : LRS_BetDiceType.Risk) : LRS_BetDiceType.GetScore;
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
			SetCredit(LRS_GVars.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
