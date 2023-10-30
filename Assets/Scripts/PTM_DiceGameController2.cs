using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTM_DiceGameController2 : PTM_MB_Singleton<PTM_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	private GameObject _goUIContainer;

	private PTM_BetGoldAreaController betGoldAreaController;

	private Image imgCurtain;

	private PTM_BetDiceType lastBetType;

	private PTM_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	public long Gamelewinid;

	private void Init()
	{
		_goUIContainer = base.gameObject;
		betGoldAreaController = base.transform.Find("BetGoldArea").GetComponent<PTM_BetGoldAreaController>();
		imgCurtain = base.transform.parent.Find("ImgCurtain").GetComponent<Image>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		Init();
		if (PTM_MB_Singleton<PTM_DiceGameController2>._instance == null)
		{
			PTM_MB_Singleton<PTM_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
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
		PTM_LockManager.UnLock("ScoreBank", force: true);
		PTM_LockManager.UnLock("btn_options", force: true);
		PTM_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		PTM_MB_Singleton<PTM_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		overflowNum = PTM_GVars.desk.diceOverflow;
	}

	public void Show()
	{
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().ChangeView("DiceGame");
		PTM_Utils.TrySetActive(_goUIContainer, active: true);
		PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (PTM_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		PTM_Utils.TrySetActive(_goUIContainer, active: false);
		if (PTM_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		PTM_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		PTM_StateManager.GetInstance().ClearWork("MultipleInfo");
		PTM_SoundManager.Instance.StopDealerAudio();
		PTM_SoundManager.Instance.StopGuysAudio();
		PTM_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		PTM_LockManager.UnLock("ScoreBank", force: true);
		PTM_LockManager.UnLock("btn_options", force: true);
		PTM_MB_Singleton<PTM_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		if (totalWin != 0)
		{
			PTM_GVars.credit = credit + totalWin;
		}
		else
		{
			PTM_GVars.credit = credit + totalBet;
		}
		PTM_GVars.realCredit = PTM_GVars.credit;
		PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().SetKeepScore(0);
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
		if (!PTM_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(PTM_BetDiceType.GetScore);
			BtnBetClickCommon();
			PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!PTM_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(PTM_BetDiceType.Half);
			BtnBetClickCommon();
			PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!PTM_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(PTM_BetDiceType.Risk);
			BtnBetClickCommon();
			PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		if (!PTM_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			UnityEngine.Debug.LogError("网络异常，请继续游戏");
			PTM_StateManager.GetInstance().CompleteWork("MultipleInfo");
			PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			PTM_MB_Singleton<PTM_AlertDialog>.GetInstance().ShowDialog((PTM_GVars.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		PTM_SoundManager.Instance.PlayClickAudio();
		PTM_SoundManager.bShouldStop = true;
		PTM_SoundManager.Instance.StopGuysAudio();
		PTM_SoundManager.Instance.StopDealerAudio();
		PTM_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		PTM_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!PTM_GVars.tryLockOnePoint)
		{
			PTM_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				PTM_GVars.credit = credit;
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
		if (!PTM_GVars.tryLockOnePoint)
		{
			PTM_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				PTM_GVars.credit = credit;
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
		if (PTM_GVars.tryLockOnePoint)
		{
			return;
		}
		PTM_SoundManager.Instance.PlayClickAudio();
		if (PTM_LockManager.IsLocked("btn_ready"))
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
			if (lastResult == PTM_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			StartCoroutine(RoundStart());
			PTM_LockManager.Lock("btn_ready");
			StartCoroutine(PTM_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				JudgeBtnDoubleAndHalveState();
				PTM_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(PTM_BetDiceType type)
	{
		object[] args = new object[2]
		{
			PTM_GVars.desk.id,
			(int)type
		};
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		PTM_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			PTM_GVars.desk.id
		};
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.LogError("MultipResult: " + JsonMapper.ToJson(args));
		PTM_StateManager.GetInstance().CompleteWork("MultipleInfo");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string s = dictionary["card"].ToString();
		Gamelewinid = long.Parse(s);
		PTM_GamblePanel.ins.AllScroce = (int)dictionary["userScore"];
		PTM_GamblePanel.ins.Gamlewin = (int)dictionary["winScore"];
		PTM_GamblePanel.ins.isGamble = true;
		PTM_GamblePanel.ins.Showgamelegame(isshow: true, PTM_GamblePanel.ins.Gamlewin);
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
		else if (PTM_GVars.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				PTM_Utils.TrySetActive(_goUIContainer, active: false);
				PTM_SoundManager.Instance.StopDealerAudio();
				PTM_SoundManager.Instance.StopGuysAudio();
				PTM_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (PTM_GVars.curView == "MajorGame")
		{
			PTM_Utils.TrySetActive(_goUIContainer, active: false);
			PTM_SoundManager.Instance.StopDealerAudio();
			PTM_SoundManager.Instance.StopGuysAudio();
			PTM_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().SetKeepScore(newBet);
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		PTM_GVars.credit = newCredit;
	}

	private void SetTotalWin(int newWin)
	{
		totalWin = newWin;
	}

	private void SetBtnReadyState(string state)
	{
		strBtnReadyState = state;
	}

	private int CalcWinRate(PTM_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		PTM_BetDiceType pTM_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (pTM_BetDiceType == betType)
		{
			result = ((betType == PTM_BetDiceType.Half) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private PTM_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum >= 7) ? ((diceSum == 7) ? PTM_BetDiceType.Half : PTM_BetDiceType.Risk) : PTM_BetDiceType.GetScore;
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
			SetCredit(PTM_GVars.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
