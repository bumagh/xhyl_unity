using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LLD_DiceGameController2 : LLD_MB_Singleton<LLD_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	private GameObject _goUIContainer;

	private LLD_BetGoldAreaController betGoldAreaController;

	private Image imgCurtain;

	private LLD_BetDiceType lastBetType;

	private LLD_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	public uint Gamelewinid;

	private void Init()
	{
		_goUIContainer = base.gameObject;
		betGoldAreaController = base.transform.Find("BetGoldArea").GetComponent<LLD_BetGoldAreaController>();
		imgCurtain = base.transform.parent.Find("ImgCurtain").GetComponent<Image>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		Init();
		if (LLD_MB_Singleton<LLD_DiceGameController2>._instance == null)
		{
			LLD_MB_Singleton<LLD_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		LLD_MB_Singleton<LLD_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
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
		LLD_LockManager.UnLock("ScoreBank", force: true);
		LLD_LockManager.UnLock("btn_options", force: true);
		LLD_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		LLD_MB_Singleton<LLD_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		overflowNum = LLD_GVars.desk.diceOverflow;
	}

	public void Show()
	{
		LLD_MB_Singleton<LLD_GameManager>.GetInstance().ChangeView("DiceGame");
		LLD_Utils.TrySetActive(_goUIContainer, active: true);
		LLD_MB_Singleton<LLD_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (LLD_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		LLD_Utils.TrySetActive(_goUIContainer, active: false);
		if (LLD_GVars.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		LLD_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		LLD_StateManager.GetInstance().ClearWork("MultipleInfo");
		LLD_SoundManager.Instance.StopDealerAudio();
		LLD_SoundManager.Instance.StopGuysAudio();
		LLD_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		LLD_LockManager.UnLock("ScoreBank", force: true);
		LLD_LockManager.UnLock("btn_options", force: true);
		LLD_MB_Singleton<LLD_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		if (totalWin != 0)
		{
			LLD_GVars.credit = credit + totalWin;
		}
		else
		{
			LLD_GVars.credit = credit + totalBet;
		}
		LLD_GVars.realCredit = LLD_GVars.credit;
		LLD_MB_Singleton<LLD_ScoreBank>.GetInstance().SetKeepScore(0);
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
		if (!LLD_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(LLD_BetDiceType.GetScore);
			BtnBetClickCommon();
			LLD_MB_Singleton<LLD_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!LLD_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(LLD_BetDiceType.Half);
			BtnBetClickCommon();
			LLD_MB_Singleton<LLD_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!LLD_GVars.tryLockOnePoint)
		{
			Send_MultipleInfo(LLD_BetDiceType.Risk);
			BtnBetClickCommon();
			LLD_MB_Singleton<LLD_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		if (!LLD_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			UnityEngine.Debug.LogError("网络异常，请继续游戏");
			LLD_StateManager.GetInstance().CompleteWork("MultipleInfo");
			LLD_MB_Singleton<LLD_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			LLD_MB_Singleton<LLD_AlertDialog>.GetInstance().ShowDialog((LLD_GVars.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		LLD_SoundManager.Instance.PlayClickAudio();
		LLD_SoundManager.bShouldStop = true;
		LLD_SoundManager.Instance.StopGuysAudio();
		LLD_SoundManager.Instance.StopDealerAudio();
		LLD_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		LLD_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!LLD_GVars.tryLockOnePoint)
		{
			LLD_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				LLD_GVars.credit = credit;
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
		if (!LLD_GVars.tryLockOnePoint)
		{
			LLD_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				LLD_GVars.credit = credit;
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
		if (LLD_GVars.tryLockOnePoint)
		{
			return;
		}
		LLD_SoundManager.Instance.PlayClickAudio();
		if (LLD_LockManager.IsLocked("btn_ready"))
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
			if (lastResult == LLD_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			StartCoroutine(RoundStart());
			LLD_LockManager.Lock("btn_ready");
			StartCoroutine(LLD_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				JudgeBtnDoubleAndHalveState();
				LLD_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(LLD_BetDiceType type)
	{
		object[] args = new object[2]
		{
			LLD_GVars.desk.id,
			(int)type
		};
		LLD_MB_Singleton<LLD_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		LLD_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			LLD_GVars.desk.id
		};
		LLD_MB_Singleton<LLD_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.LogError("MultipResult: " + JsonMapper.ToJson(args));
		LLD_StateManager.GetInstance().CompleteWork("MultipleInfo");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string s = dictionary["card"].ToString();
		Gamelewinid = uint.Parse(s);
		LLD_GamblePanel.ins.AllScroce = (int)dictionary["userScore"];
		LLD_GamblePanel.ins.Gamlewin = (int)dictionary["winScore"];
		LLD_GamblePanel.ins.isGamble = true;
		LLD_GamblePanel.ins.Showgamelegame(isshow: true, LLD_GamblePanel.ins.Gamlewin);
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
		else if (LLD_GVars.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				LLD_Utils.TrySetActive(_goUIContainer, active: false);
				LLD_SoundManager.Instance.StopDealerAudio();
				LLD_SoundManager.Instance.StopGuysAudio();
				LLD_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (LLD_GVars.curView == "MajorGame")
		{
			LLD_Utils.TrySetActive(_goUIContainer, active: false);
			LLD_SoundManager.Instance.StopDealerAudio();
			LLD_SoundManager.Instance.StopGuysAudio();
			LLD_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		LLD_MB_Singleton<LLD_ScoreBank>.GetInstance().SetKeepScore(newBet);
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		LLD_GVars.credit = newCredit;
	}

	private void SetTotalWin(int newWin)
	{
		totalWin = newWin;
	}

	private void SetBtnReadyState(string state)
	{
		strBtnReadyState = state;
	}

	private int CalcWinRate(LLD_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		LLD_BetDiceType lLD_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (lLD_BetDiceType == betType)
		{
			result = ((betType == LLD_BetDiceType.Half) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private LLD_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum >= 7) ? ((diceSum == 7) ? LLD_BetDiceType.Half : LLD_BetDiceType.Risk) : LLD_BetDiceType.GetScore;
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
			SetCredit(LLD_GVars.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
