using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SHT_DiceGameController2 : SHT_MB_Singleton<SHT_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	private GameObject _goUIContainer;

	private SHT_BetGoldAreaController betGoldAreaController;

	private Image imgCurtain;

	private SHT_BetDiceType lastBetType;

	private SHT_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	public uint Gamelewinid;

	private void Init()
	{
		_goUIContainer = base.gameObject;
		betGoldAreaController = base.transform.Find("BetGoldArea").GetComponent<SHT_BetGoldAreaController>();
		imgCurtain = base.transform.parent.Find("ImgCurtain").GetComponent<Image>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		Init();
		if (SHT_MB_Singleton<SHT_DiceGameController2>._instance == null)
		{
			SHT_MB_Singleton<SHT_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		SHT_MB_Singleton<SHT_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
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
		SHT_LockManager.UnLock("ScoreBank", force: true);
		SHT_LockManager.UnLock("btn_options", force: true);
		SHT_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		overflowNum = SHT_GVars.desk.diceOverflow;
	}

	public void Show()
	{
		SHT_MB_Singleton<SHT_GameManager>.GetInstance().ChangeView("DiceGame");
		SHT_Utils.TrySetActive(_goUIContainer, active: true);
		SHT_MB_Singleton<SHT_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (SHT_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		SHT_Utils.TrySetActive(_goUIContainer, active: false);
		if (SHT_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		SHT_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		SHT_StateManager.GetInstance().ClearWork("MultipleInfo");
		SHT_SoundManager.Instance.StopGuysAudio();
		SHT_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		SHT_LockManager.UnLock("ScoreBank", force: true);
		SHT_LockManager.UnLock("btn_options", force: true);
		if (totalWin != 0)
		{
			SHT_GVars.credit = credit + totalWin;
		}
		else
		{
			SHT_GVars.credit = credit + totalBet;
		}
		SHT_GVars.realCredit = SHT_GVars.credit;
		SHT_MB_Singleton<SHT_ScoreBank>.GetInstance().SetKeepScore(0);
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
		if (!SHT_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(SHT_BetDiceType.GetScore);
			BtnBetClickCommon();
			SHT_MB_Singleton<SHT_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!SHT_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(SHT_BetDiceType.Half);
			BtnBetClickCommon();
			SHT_MB_Singleton<SHT_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!SHT_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(SHT_BetDiceType.Risk);
			BtnBetClickCommon();
			SHT_MB_Singleton<SHT_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		if (!SHT_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			UnityEngine.Debug.LogError("网络异常，请继续游戏");
			SHT_StateManager.GetInstance().CompleteWork("MultipleInfo");
			SHT_MB_Singleton<SHT_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			SHT_MB_Singleton<SHT_AlertDialog>.GetInstance().ShowDialog((SHT_GVars.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		SHT_SoundManager.Instance.PlayClickAudio();
		SHT_SoundManager.bShouldStop = true;
		SHT_SoundManager.Instance.StopGuysAudio();
		SHT_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		SHT_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!SHT_GVars.tryLockOnePoint)
		{
			SHT_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				SHT_GVars.credit = credit;
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
		if (!SHT_GVars.tryLockOnePoint)
		{
			SHT_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				SHT_GVars.credit = credit;
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
		if (SHT_GVars.tryLockOnePoint)
		{
			return;
		}
		SHT_SoundManager.Instance.PlayClickAudio();
		if (SHT_LockManager.IsLocked("btn_ready"))
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
			if (lastResult == SHT_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			StartCoroutine(RoundStart());
			SHT_LockManager.Lock("btn_ready");
			StartCoroutine(SHT_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				JudgeBtnDoubleAndHalveState();
				SHT_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(SHT_BetDiceType type)
	{
		object[] args = new object[2]
		{
			SHT_GVars.desk.id,
			(int)type
		};
		SHT_MB_Singleton<SHT_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		SHT_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			SHT_GVars.desk.id
		};
		SHT_MB_Singleton<SHT_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.LogError("MultipResult: " + JsonMapper.ToJson(args));
		SHT_StateManager.GetInstance().CompleteWork("MultipleInfo");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string s = dictionary["card"].ToString();
		Gamelewinid = uint.Parse(s);
		SHT_GamblePanel.ins.AllScroce = (int)dictionary["userScore"];
		SHT_GamblePanel.ins.Gamlewin = (int)dictionary["winScore"];
		SHT_GamblePanel.ins.isGamble = true;
		SHT_GamblePanel.ins.Showgamelegame(isshow: true, SHT_GamblePanel.ins.Gamlewin);
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
		else if (SHT_GVars.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				SHT_Utils.TrySetActive(_goUIContainer, active: false);
				SHT_SoundManager.Instance.StopGuysAudio();
				SHT_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (SHT_GVars.curView == "MajorGame")
		{
			SHT_Utils.TrySetActive(_goUIContainer, active: false);
			SHT_SoundManager.Instance.StopGuysAudio();
			SHT_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		SHT_MB_Singleton<SHT_ScoreBank>.GetInstance().SetKeepScore(newBet);
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		SHT_GVars.credit = newCredit;
	}

	private void SetTotalWin(int newWin)
	{
		totalWin = newWin;
	}

	private void SetBtnReadyState(string state)
	{
		strBtnReadyState = state;
	}

	private int CalcWinRate(SHT_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		SHT_BetDiceType sHT_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (sHT_BetDiceType == betType)
		{
			result = ((betType == SHT_BetDiceType.Half) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private SHT_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum >= 7) ? ((diceSum == 7) ? SHT_BetDiceType.Half : SHT_BetDiceType.Risk) : SHT_BetDiceType.GetScore;
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
			SetCredit(SHT_GVars.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
