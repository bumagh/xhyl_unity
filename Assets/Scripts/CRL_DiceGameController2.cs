using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CRL_DiceGameController2 : CRL_MB_Singleton<CRL_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	private GameObject _goUIContainer;

	private CRL_BetGoldAreaController betGoldAreaController;

	private Image imgCurtain;

	private CRL_BetDiceType lastBetType;

	private CRL_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	public uint Gamelewinid;

	private void Init()
	{
		_goUIContainer = base.gameObject;
		betGoldAreaController = base.transform.Find("BetGoldArea").GetComponent<CRL_BetGoldAreaController>();
		imgCurtain = base.transform.parent.Find("ImgCurtain").GetComponent<Image>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		Init();
		if (CRL_MB_Singleton<CRL_DiceGameController2>._instance == null)
		{
			CRL_MB_Singleton<CRL_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		CRL_MB_Singleton<CRL_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
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
		CRL_LockManager.UnLock("ScoreBank", force: true);
		CRL_LockManager.UnLock("btn_options", force: true);
		CRL_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		CRL_MB_Singleton<CRL_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		overflowNum = CRL_MySqlConnection.desk.diceOverflow;
	}

	public void Show()
	{
		CRL_MB_Singleton<CRL_GameManager>.GetInstance().ChangeView("DiceGame");
		CRL_Utils.TrySetActive(_goUIContainer, active: true);
		CRL_MB_Singleton<CRL_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (CRL_MySqlConnection.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		CRL_Utils.TrySetActive(_goUIContainer, active: false);
		if (CRL_MySqlConnection.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		CRL_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		CRL_StateManager.GetInstance().ClearWork("MultipleInfo");
		CRL_SoundManager.Instance.StopDealerAudio();
		CRL_SoundManager.Instance.StopGuysAudio();
		CRL_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		CRL_LockManager.UnLock("ScoreBank", force: true);
		CRL_LockManager.UnLock("btn_options", force: true);
		CRL_MB_Singleton<CRL_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		if (totalWin != 0)
		{
			CRL_MySqlConnection.credit = credit + totalWin;
		}
		else
		{
			CRL_MySqlConnection.credit = credit + totalBet;
		}
		CRL_MySqlConnection.realCredit = CRL_MySqlConnection.credit;
		CRL_MB_Singleton<CRL_ScoreBank>.GetInstance().SetKeepScore(0);
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
		if (!CRL_MySqlConnection.tryLockOnePoint)
		{
			Send_MultipleInfo(CRL_BetDiceType.GetScore);
			BtnBetClickCommon();
			CRL_MB_Singleton<CRL_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!CRL_MySqlConnection.tryLockOnePoint)
		{
			Send_MultipleInfo(CRL_BetDiceType.Half);
			BtnBetClickCommon();
			CRL_MB_Singleton<CRL_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!CRL_MySqlConnection.tryLockOnePoint)
		{
			Send_MultipleInfo(CRL_BetDiceType.Risk);
			BtnBetClickCommon();
			CRL_MB_Singleton<CRL_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		if (!CRL_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			UnityEngine.Debug.LogError("网络异常，请继续游戏");
			CRL_StateManager.GetInstance().CompleteWork("MultipleInfo");
			CRL_MB_Singleton<CRL_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			CRL_MB_Singleton<CRL_AlertDialog>.GetInstance().ShowDialog((CRL_MySqlConnection.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		CRL_SoundManager.Instance.PlayClickAudio();
		CRL_SoundManager.bShouldStop = true;
		CRL_SoundManager.Instance.StopGuysAudio();
		CRL_SoundManager.Instance.StopDealerAudio();
		CRL_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		CRL_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!CRL_MySqlConnection.tryLockOnePoint)
		{
			CRL_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				CRL_MySqlConnection.credit = credit;
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
		if (!CRL_MySqlConnection.tryLockOnePoint)
		{
			CRL_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				CRL_MySqlConnection.credit = credit;
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
		if (CRL_MySqlConnection.tryLockOnePoint)
		{
			return;
		}
		CRL_SoundManager.Instance.PlayClickAudio();
		if (CRL_LockManager.IsLocked("btn_ready"))
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
			if (lastResult == CRL_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			StartCoroutine(RoundStart());
			CRL_LockManager.Lock("btn_ready");
			StartCoroutine(CRL_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				JudgeBtnDoubleAndHalveState();
				CRL_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(CRL_BetDiceType type)
	{
		object[] args = new object[2]
		{
			CRL_MySqlConnection.desk.id,
			(int)type
		};
		CRL_MB_Singleton<CRL_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		CRL_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			CRL_MySqlConnection.desk.id
		};
		CRL_MB_Singleton<CRL_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.LogError("MultipResult: " + JsonMapper.ToJson(args));
		CRL_StateManager.GetInstance().CompleteWork("MultipleInfo");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string s = dictionary["card"].ToString();
		Gamelewinid = uint.Parse(s);
		CRL_GamblePanel.ins.AllScroce = (int)dictionary["userScore"];
		CRL_GamblePanel.ins.Gamlewin = (int)dictionary["winScore"];
		CRL_GamblePanel.ins.isGamble = true;
		CRL_GamblePanel.ins.Showgamelegame(isshow: true, CRL_GamblePanel.ins.Gamlewin);
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
		else if (CRL_MySqlConnection.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				CRL_Utils.TrySetActive(_goUIContainer, active: false);
				CRL_SoundManager.Instance.StopDealerAudio();
				CRL_SoundManager.Instance.StopGuysAudio();
				CRL_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (CRL_MySqlConnection.curView == "MajorGame")
		{
			CRL_Utils.TrySetActive(_goUIContainer, active: false);
			CRL_SoundManager.Instance.StopDealerAudio();
			CRL_SoundManager.Instance.StopGuysAudio();
			CRL_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		CRL_MB_Singleton<CRL_ScoreBank>.GetInstance().SetKeepScore(newBet);
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		CRL_MySqlConnection.credit = newCredit;
	}

	private void SetTotalWin(int newWin)
	{
		totalWin = newWin;
	}

	private void SetBtnReadyState(string state)
	{
		strBtnReadyState = state;
	}

	private int CalcWinRate(CRL_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		CRL_BetDiceType cRL_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (cRL_BetDiceType == betType)
		{
			result = ((betType == CRL_BetDiceType.Half) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private CRL_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum >= 7) ? ((diceSum == 7) ? CRL_BetDiceType.Half : CRL_BetDiceType.Risk) : CRL_BetDiceType.GetScore;
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
			SetCredit(CRL_MySqlConnection.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
