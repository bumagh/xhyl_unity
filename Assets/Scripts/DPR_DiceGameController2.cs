using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPR_DiceGameController2 : DPR_MB_Singleton<DPR_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	private GameObject _goUIContainer;

	private DPR_BetGoldAreaController betGoldAreaController;

	private Image imgCurtain;

	private DPR_BetDiceType lastBetType;

	private DPR_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	public uint Gamelewinid;

	private void Init()
	{
		_goUIContainer = base.gameObject;
		betGoldAreaController = base.transform.Find("BetGoldArea").GetComponent<DPR_BetGoldAreaController>();
		imgCurtain = base.transform.parent.Find("ImgCurtain").GetComponent<Image>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		Init();
		if (DPR_MB_Singleton<DPR_DiceGameController2>._instance == null)
		{
			DPR_MB_Singleton<DPR_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		DPR_MB_Singleton<DPR_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
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
		DPR_LockManager.UnLock("ScoreBank", force: true);
		DPR_LockManager.UnLock("btn_options", force: true);
		DPR_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		DPR_MB_Singleton<DPR_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		overflowNum = DPR_MySqlConnection.desk.diceOverflow;
	}

	public void Show()
	{
		DPR_MB_Singleton<DPR_GameManager>.GetInstance().ChangeView("DiceGame");
		DPR_Utils.TrySetActive(_goUIContainer, active: true);
		DPR_MB_Singleton<DPR_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (DPR_MySqlConnection.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		DPR_Utils.TrySetActive(_goUIContainer, active: false);
		if (DPR_MySqlConnection.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		DPR_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		DPR_StateManager.GetInstance().ClearWork("MultipleInfo");
		DPR_SoundManager.Instance.StopDealerAudio();
		DPR_SoundManager.Instance.StopGuysAudio();
		DPR_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		DPR_LockManager.UnLock("ScoreBank", force: true);
		DPR_LockManager.UnLock("btn_options", force: true);
		DPR_MB_Singleton<DPR_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		if (totalWin != 0)
		{
			DPR_MySqlConnection.credit = credit + totalWin;
		}
		else
		{
			DPR_MySqlConnection.credit = credit + totalBet;
		}
		DPR_MySqlConnection.realCredit = DPR_MySqlConnection.credit;
		DPR_MB_Singleton<DPR_ScoreBank>.GetInstance().SetKeepScore(0);
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
		if (!DPR_MySqlConnection.tryLockOnePoint)
		{
			Send_MultipleInfo(DPR_BetDiceType.GetScore);
			BtnBetClickCommon();
			DPR_MB_Singleton<DPR_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!DPR_MySqlConnection.tryLockOnePoint)
		{
			Send_MultipleInfo(DPR_BetDiceType.Half);
			BtnBetClickCommon();
			DPR_MB_Singleton<DPR_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!DPR_MySqlConnection.tryLockOnePoint)
		{
			Send_MultipleInfo(DPR_BetDiceType.Risk);
			BtnBetClickCommon();
			DPR_MB_Singleton<DPR_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		if (!DPR_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			UnityEngine.Debug.LogError("网络异常，请继续游戏");
			DPR_StateManager.GetInstance().CompleteWork("MultipleInfo");
			DPR_MB_Singleton<DPR_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			DPR_MB_Singleton<DPR_AlertDialog>.GetInstance().ShowDialog((DPR_MySqlConnection.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		DPR_SoundManager.Instance.PlayClickAudio();
		DPR_SoundManager.bShouldStop = true;
		DPR_SoundManager.Instance.StopGuysAudio();
		DPR_SoundManager.Instance.StopDealerAudio();
		DPR_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		DPR_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!DPR_MySqlConnection.tryLockOnePoint)
		{
			DPR_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				DPR_MySqlConnection.credit = credit;
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
		if (!DPR_MySqlConnection.tryLockOnePoint)
		{
			DPR_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				DPR_MySqlConnection.credit = credit;
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
		if (DPR_MySqlConnection.tryLockOnePoint)
		{
			return;
		}
		DPR_SoundManager.Instance.PlayClickAudio();
		if (DPR_LockManager.IsLocked("btn_ready"))
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
			if (lastResult == DPR_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			StartCoroutine(RoundStart());
			DPR_LockManager.Lock("btn_ready");
			StartCoroutine(DPR_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				JudgeBtnDoubleAndHalveState();
				DPR_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(DPR_BetDiceType type)
	{
		object[] args = new object[2]
		{
			DPR_MySqlConnection.desk.id,
			(int)type
		};
		DPR_MB_Singleton<DPR_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		DPR_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			DPR_MySqlConnection.desk.id
		};
		DPR_MB_Singleton<DPR_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.LogError("MultipResult: " + JsonMapper.ToJson(args));
		DPR_StateManager.GetInstance().CompleteWork("MultipleInfo");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string s = dictionary["card"].ToString();
		Gamelewinid = uint.Parse(s);
		DPR_GamblePanel.ins.AllScroce = (int)dictionary["userScore"];
		DPR_GamblePanel.ins.Gamlewin = (int)dictionary["winScore"];
		DPR_GamblePanel.ins.isGamble = true;
		DPR_GamblePanel.ins.Showgamelegame(isshow: true, DPR_GamblePanel.ins.Gamlewin);
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
		else if (DPR_MySqlConnection.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				DPR_Utils.TrySetActive(_goUIContainer, active: false);
				DPR_SoundManager.Instance.StopDealerAudio();
				DPR_SoundManager.Instance.StopGuysAudio();
				DPR_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (DPR_MySqlConnection.curView == "MajorGame")
		{
			DPR_Utils.TrySetActive(_goUIContainer, active: false);
			DPR_SoundManager.Instance.StopDealerAudio();
			DPR_SoundManager.Instance.StopGuysAudio();
			DPR_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		DPR_MB_Singleton<DPR_ScoreBank>.GetInstance().SetKeepScore(newBet);
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		DPR_MySqlConnection.credit = newCredit;
	}

	private void SetTotalWin(int newWin)
	{
		totalWin = newWin;
	}

	private void SetBtnReadyState(string state)
	{
		strBtnReadyState = state;
	}

	private int CalcWinRate(DPR_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		DPR_BetDiceType dPR_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (dPR_BetDiceType == betType)
		{
			result = ((betType == DPR_BetDiceType.Half) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private DPR_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum >= 7) ? ((diceSum == 7) ? DPR_BetDiceType.Half : DPR_BetDiceType.Risk) : DPR_BetDiceType.GetScore;
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
			SetCredit(DPR_MySqlConnection.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
