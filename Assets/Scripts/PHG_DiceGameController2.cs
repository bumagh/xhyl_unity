using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PHG_DiceGameController2 : PHG_MB_Singleton<PHG_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	private GameObject _goUIContainer;

	private PHG_BetGoldAreaController betGoldAreaController;

	private Image imgCurtain;

	private PHG_BetDiceType lastBetType;

	private PHG_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	public uint Gamelewinid;

	private void Init()
	{
		_goUIContainer = base.gameObject;
		betGoldAreaController = base.transform.Find("BetGoldArea").GetComponent<PHG_BetGoldAreaController>();
		imgCurtain = base.transform.parent.Find("ImgCurtain").GetComponent<Image>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		Init();
		if (PHG_MB_Singleton<PHG_DiceGameController2>._instance == null)
		{
			PHG_MB_Singleton<PHG_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		PHG_MB_Singleton<PHG_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
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
		PHG_LockManager.UnLock("ScoreBank", force: true);
		PHG_LockManager.UnLock("btn_options", force: true);
		PHG_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		overflowNum = PHG_GVars.desk.diceOverflow;
	}

	public void Show()
	{
		PHG_MB_Singleton<PHG_GameManager>.GetInstance().ChangeView("DiceGame");
		PHG_Utils.TrySetActive(_goUIContainer, active: true);
		PHG_MB_Singleton<PHG_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (PHG_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		PHG_Utils.TrySetActive(_goUIContainer, active: false);
		if (PHG_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		PHG_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		PHG_StateManager.GetInstance().ClearWork("MultipleInfo");
		PHG_SoundManager.Instance.StopGuysAudio();
		PHG_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		PHG_LockManager.UnLock("ScoreBank", force: true);
		PHG_LockManager.UnLock("btn_options", force: true);
		if (totalWin != 0)
		{
			PHG_GVars.credit = credit + totalWin;
		}
		else
		{
			PHG_GVars.credit = credit + totalBet;
		}
		PHG_GVars.realCredit = PHG_GVars.credit;
		PHG_MB_Singleton<PHG_ScoreBank>.GetInstance().SetKeepScore(0);
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
		if (!PHG_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(PHG_BetDiceType.GetScore);
			BtnBetClickCommon();
			PHG_MB_Singleton<PHG_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!PHG_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(PHG_BetDiceType.Half);
			BtnBetClickCommon();
			PHG_MB_Singleton<PHG_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!PHG_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(PHG_BetDiceType.Risk);
			BtnBetClickCommon();
			PHG_MB_Singleton<PHG_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		if (!PHG_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			UnityEngine.Debug.LogError("网络异常，请继续游戏");
			PHG_StateManager.GetInstance().CompleteWork("MultipleInfo");
			PHG_MB_Singleton<PHG_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			PHG_MB_Singleton<PHG_AlertDialog>.GetInstance().ShowDialog((PHG_GVars.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		PHG_SoundManager.Instance.PlayClickAudio();
		PHG_SoundManager.bShouldStop = true;
		PHG_SoundManager.Instance.StopGuysAudio();
		PHG_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		PHG_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!PHG_GVars.tryLockOnePoint)
		{
			PHG_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				PHG_GVars.credit = credit;
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
		if (!PHG_GVars.tryLockOnePoint)
		{
			PHG_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				PHG_GVars.credit = credit;
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
		if (PHG_GVars.tryLockOnePoint)
		{
			return;
		}
		PHG_SoundManager.Instance.PlayClickAudio();
		if (PHG_LockManager.IsLocked("btn_ready"))
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
			if (lastResult == PHG_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			StartCoroutine(RoundStart());
			PHG_LockManager.Lock("btn_ready");
			StartCoroutine(PHG_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				JudgeBtnDoubleAndHalveState();
				PHG_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(PHG_BetDiceType type)
	{
		object[] args = new object[2]
		{
			PHG_GVars.desk.id,
			(int)type
		};
		PHG_MB_Singleton<PHG_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		PHG_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			PHG_GVars.desk.id
		};
		PHG_MB_Singleton<PHG_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.LogError("MultipResult: " + JsonMapper.ToJson(args));
		PHG_StateManager.GetInstance().CompleteWork("MultipleInfo");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string s = dictionary["card"].ToString();
		Gamelewinid = uint.Parse(s);
		PHG_GamblePanel.ins.AllScroce = (int)dictionary["userScore"];
		PHG_GamblePanel.ins.Gamlewin = (int)dictionary["winScore"];
		PHG_GamblePanel.ins.isGamble = true;
		PHG_GamblePanel.ins.Showgamelegame(isshow: true, PHG_GamblePanel.ins.Gamlewin);
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
		else if (PHG_GVars.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				PHG_Utils.TrySetActive(_goUIContainer, active: false);
				PHG_SoundManager.Instance.StopGuysAudio();
				PHG_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (PHG_GVars.curView == "MajorGame")
		{
			PHG_Utils.TrySetActive(_goUIContainer, active: false);
			PHG_SoundManager.Instance.StopGuysAudio();
			PHG_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		PHG_MB_Singleton<PHG_ScoreBank>.GetInstance().SetKeepScore(newBet);
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		PHG_GVars.credit = newCredit;
	}

	private void SetTotalWin(int newWin)
	{
		totalWin = newWin;
	}

	private void SetBtnReadyState(string state)
	{
		strBtnReadyState = state;
	}

	private int CalcWinRate(PHG_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		PHG_BetDiceType pHG_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (pHG_BetDiceType == betType)
		{
			result = ((betType == PHG_BetDiceType.Half) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private PHG_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum >= 7) ? ((diceSum == 7) ? PHG_BetDiceType.Half : PHG_BetDiceType.Risk) : PHG_BetDiceType.GetScore;
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
			SetCredit(PHG_GVars.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
