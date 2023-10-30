using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESP_DiceGameController2 : ESP_MB_Singleton<ESP_DiceGameController2>
{
	private int credit;

	private int totalBet;

	private int totalWin;

	private int overflowNum = 100000;

	private GameObject _goUIContainer;

	private ESP_BetGoldAreaController betGoldAreaController;

	private Image imgCurtain;

	private ESP_BetDiceType lastBetType;

	private ESP_DiceGameResultType lastResult;

	private bool isBtnHalveAndDoubleLock;

	private bool isCurtainOff;

	public Action onExitAction;

	private string strBtnReadyState = "ready";

	public uint Gamelewinid;

	private void Init()
	{
		_goUIContainer = base.gameObject;
		betGoldAreaController = base.transform.Find("BetGoldArea").GetComponent<ESP_BetGoldAreaController>();
		imgCurtain = base.transform.parent.Find("ImgCurtain").GetComponent<Image>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		Init();
		if (ESP_MB_Singleton<ESP_DiceGameController2>._instance == null)
		{
			ESP_MB_Singleton<ESP_DiceGameController2>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().RegisterHandler("multipResult", HandleNetMsg_MultipResult);
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
		ESP_LockManager.UnLock("ScoreBank", force: true);
		ESP_LockManager.UnLock("btn_options", force: true);
		ESP_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		overflowNum = ESP_MySqlConnection.desk.diceOverflow;
	}

	public void Show()
	{
		ESP_MB_Singleton<ESP_GameManager>.GetInstance().ChangeView("DiceGame");
		ESP_Utils.TrySetActive(_goUIContainer, active: true);
		ESP_MB_Singleton<ESP_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
		if (ESP_MySqlConnection.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void Hide()
	{
		ESP_Utils.TrySetActive(_goUIContainer, active: false);
		if (ESP_MySqlConnection.curView == "DiceGame")
		{
			PullCurtain();
		}
	}

	public void ExitGame()
	{
		UnityEngine.Debug.Log("ExitGame");
		ESP_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		ESP_StateManager.GetInstance().ClearWork("MultipleInfo");
		ESP_SoundManager.Instance.StopGuysAudio();
		ESP_SoundManager.Instance.StopDiceBGM();
		Send_ExitMultipleInfo();
		ESP_LockManager.UnLock("ScoreBank", force: true);
		ESP_LockManager.UnLock("btn_options", force: true);
		if (totalWin != 0)
		{
			ESP_MySqlConnection.credit = credit + totalWin;
		}
		else
		{
			ESP_MySqlConnection.credit = credit + totalBet;
		}
		ESP_MySqlConnection.realCredit = ESP_MySqlConnection.credit;
		ESP_MB_Singleton<ESP_ScoreBank>.GetInstance().SetKeepScore(0);
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
		if (!ESP_MySqlConnection.tryLockOnePoint)
		{
			Send_MultipleInfo(ESP_BetDiceType.GetScore);
			BtnBetClickCommon();
			ESP_MB_Singleton<ESP_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetMiddle_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetMiddle");
		if (!ESP_MySqlConnection.tryLockOnePoint)
		{
			Send_MultipleInfo(ESP_BetDiceType.Half);
			BtnBetClickCommon();
			ESP_MB_Singleton<ESP_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void OnBtnBetBig_Click()
	{
		UnityEngine.Debug.Log("OnBtnBetBig");
		if (!ESP_MySqlConnection.tryLockOnePoint)
		{
			Send_MultipleInfo(ESP_BetDiceType.Risk);
			BtnBetClickCommon();
			ESP_MB_Singleton<ESP_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "DiceLottery");
		}
	}

	public void HandleNoResponse_MultiInfo()
	{
		if (!ESP_StateManager.GetInstance().IsWorkCompleted("MultipleInfo"))
		{
			UnityEngine.Debug.LogError("网络异常，请继续游戏");
			ESP_StateManager.GetInstance().CompleteWork("MultipleInfo");
			ESP_MB_Singleton<ESP_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "MultiInfo> reconnect success");
			ESP_MB_Singleton<ESP_AlertDialog>.GetInstance().ShowDialog((ESP_MySqlConnection.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
			ExitGame();
		}
	}

	private void BtnBetClickCommon()
	{
		ESP_SoundManager.Instance.PlayClickAudio();
		ESP_SoundManager.bShouldStop = true;
		ESP_SoundManager.Instance.StopGuysAudio();
		ESP_SoundManager.Instance.StopDiceBGM();
		betGoldAreaController.SetBetGoldButtonsEnable(isEnable: false);
		ESP_LockManager.Lock("ScoreBank");
	}

	public void OnBtnHalveWager_Click()
	{
		UnityEngine.Debug.Log("OnBtnHalveWager_Click");
		if (!ESP_MySqlConnection.tryLockOnePoint)
		{
			ESP_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = (totalBet + 1) / 2;
				int num2 = credit;
				int num3 = totalBet;
				SetCredit(credit + totalBet - num);
				ESP_MySqlConnection.credit = credit;
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
		if (!ESP_MySqlConnection.tryLockOnePoint)
		{
			ESP_SoundManager.Instance.PlayClickAudio();
			isBtnHalveAndDoubleLock = true;
			if (totalWin == 0)
			{
				int num = totalBet * 2;
				int num2 = totalBet;
				SetCredit(credit + totalBet - num);
				ESP_MySqlConnection.credit = credit;
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
		if (ESP_MySqlConnection.tryLockOnePoint)
		{
			return;
		}
		ESP_SoundManager.Instance.PlayClickAudio();
		if (ESP_LockManager.IsLocked("btn_ready"))
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
			if (lastResult == ESP_DiceGameResultType.Overflow)
			{
				ExitGame();
			}
		}
		else if (totalWin != 0)
		{
			StartCoroutine(RoundStart());
			ESP_LockManager.Lock("btn_ready");
			StartCoroutine(ESP_Utils.ShiftReduceTextAni(0f, 0.1f, totalWin, delegate(int total, int diff)
			{
				SetTotalWin(totalWin - diff);
				SetTotalBet(total);
			}, delegate
			{
				SetBtnReadyState("back");
				JudgeBtnDoubleAndHalveState();
				ESP_LockManager.UnLock("btn_ready");
			}));
		}
		else
		{
			UnityEngine.Debug.LogError("totalWin cannot be zero here");
			ExitGame();
		}
	}

	public void Send_MultipleInfo(ESP_BetDiceType type)
	{
		object[] args = new object[2]
		{
			ESP_MySqlConnection.desk.id,
			(int)type
		};
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().Send("userService/multipleInfo", args);
		lastBetType = type;
		ESP_StateManager.GetInstance().RegisterWork("MultipleInfo");
	}

	public void Send_ExitMultipleInfo()
	{
		object[] args = new object[1]
		{
			ESP_MySqlConnection.desk.id
		};
		ESP_MB_Singleton<ESP_NetManager>.GetInstance().Send("userService/exitMultipleInfo", args);
	}

	public void HandleNetMsg_MultipResult(object[] args)
	{
		UnityEngine.Debug.LogError("MultipResult: " + JsonMapper.ToJson(args));
		ESP_StateManager.GetInstance().CompleteWork("MultipleInfo");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string s = dictionary["card"].ToString();
		Gamelewinid = uint.Parse(s);
		ESP_GamblePanel.ins.AllScroce = (int)dictionary["userScore"];
		ESP_GamblePanel.ins.Gamlewin = (int)dictionary["winScore"];
		ESP_GamblePanel.ins.isGamble = true;
		ESP_GamblePanel.ins.Showgamelegame(isshow: true, ESP_GamblePanel.ins.Gamlewin);
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
		else if (ESP_MySqlConnection.curView == "DiceGame")
		{
			imgCurtain.gameObject.SetActive(value: true);
			imgCurtain.DOColor(Color.black, 0.8f).OnComplete(delegate
			{
				imgCurtain.gameObject.SetActive(value: false);
				isCurtainOff = false;
				ESP_Utils.TrySetActive(_goUIContainer, active: false);
				ESP_SoundManager.Instance.StopGuysAudio();
				ESP_SoundManager.Instance.StopDiceBGM();
			});
		}
		else if (ESP_MySqlConnection.curView == "MajorGame")
		{
			ESP_Utils.TrySetActive(_goUIContainer, active: false);
			ESP_SoundManager.Instance.StopGuysAudio();
			ESP_SoundManager.Instance.StopDiceBGM();
		}
	}

	private void SetTotalBet(int newBet)
	{
		totalBet = newBet;
		ESP_MB_Singleton<ESP_ScoreBank>.GetInstance().SetKeepScore(newBet);
	}

	private void SetCredit(int newCredit)
	{
		credit = newCredit;
		ESP_MySqlConnection.credit = newCredit;
	}

	private void SetTotalWin(int newWin)
	{
		totalWin = newWin;
	}

	private void SetBtnReadyState(string state)
	{
		strBtnReadyState = state;
	}

	private int CalcWinRate(ESP_BetDiceType betType, int diceA, int diceB)
	{
		bool flag = diceA == diceB;
		int diceSum = diceA + diceB;
		ESP_BetDiceType eSP_BetDiceType = CalcDiceType(diceSum);
		int result = 0;
		if (eSP_BetDiceType == betType)
		{
			result = ((betType == ESP_BetDiceType.Half) ? 6 : (flag ? 4 : 2));
		}
		return result;
	}

	private ESP_BetDiceType CalcDiceType(int diceSum)
	{
		return (diceSum >= 7) ? ((diceSum == 7) ? ESP_BetDiceType.Half : ESP_BetDiceType.Risk) : ESP_BetDiceType.GetScore;
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
			SetCredit(ESP_MySqlConnection.credit);
			JudgeBtnDoubleAndHalveState();
		}
	}
}
