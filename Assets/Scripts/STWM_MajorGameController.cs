using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STWM_MajorGameController : STWM_MB_Singleton<STWM_MajorGameController>
{
	[SerializeField]
	private GameObject goUIContainer;

	[SerializeField]
	private GameObject goContainer;

	[SerializeField]
	private Sprite[] spiArrEn;

	[SerializeField]
	private STWM_CellManager cellManager;

	[SerializeField]
	private STWM_LinesManager lineManager;

	[SerializeField]
	private STWM_BallManager ballManager;

	[SerializeField]
	private Text textLineNum;

	[SerializeField]
	private Image imgBetLine;

	[SerializeField]
	private Text txtSingleStake;

	[SerializeField]
	private Image imgSingleBet;

	[SerializeField]
	private Text txtTotalStake;

	[SerializeField]
	private Image imgTotalBet;

	[SerializeField]
	private Text txtCredit;

	[SerializeField]
	private Image imgTotalScore;

	[SerializeField]
	private Text txtWinScore;

	[SerializeField]
	private Image imgWinScore;

	[SerializeField]
	private Text txtShowPath;

	[SerializeField]
	private Text txtMaxBet;

	[SerializeField]
	private Text txtDice;

	[SerializeField]
	private Text txtBtnAuto;

	[SerializeField]
	private Button btnStart;

	[SerializeField]
	private Button btnDice;

	[SerializeField]
	private Button btnAuto;

	[SerializeField]
	private Button btnShowLines;

	[SerializeField]
	private Button btnMinus;

	[SerializeField]
	private Button btnPlus;

	[SerializeField]
	private Button btnMaxStake;

	[SerializeField]
	private Image imgBtnStart;

	[SerializeField]
	private Sprite spiBtnStart;

	[SerializeField]
	private Sprite spiBtnStop;

	[SerializeField]
	private Sprite spiBtnGainScore;

	[SerializeField]
	private Sprite[] spiBetLine;

	[SerializeField]
	private Sprite[] spiSingleBet;

	[SerializeField]
	private Sprite[] spiTotalBet;

	[SerializeField]
	private Sprite[] spiTotalScore;

	[SerializeField]
	private Sprite[] spiWinScore;

	[SerializeField]
	private Transform tfAuto;

	[SerializeField]
	private GameObject goWinScoreTip;

	[SerializeField]
	private Text txtWinScoreTip;

	[SerializeField]
	private GameObject goBigWinTip;

	[SerializeField]
	private Text txtBigWinTip;

	[SerializeField]
	private STWM_NPCAnimator animFlag;

	[SerializeField]
	private STWM_NPCAnimator animDrum;

	[SerializeField]
	private STWM_MajorHints majorHints;

	public Action onBtnDice;

	private bool bGainScoreBtn;

	private bool bAuto;

	private int winScore;

	private int singleStake;

	private int totalStake;

	private int lineNum;

	private int maryTimes;

	private STWM_MajorResult lastResult;

	private List<Coroutine> listAni = new List<Coroutine>();

	private STWM_XTask _task_WinLineAni;

	private STWM_XTask _task_NormalReduceTextAni;

	private bool bGaming;

	private Coroutine coGridRolling;

	private bool canShowWinLines;

	private bool bReplaying;

	private int gameTimes;

	private int lastLineNum;

	private int lastSingleStake;

	private bool bStopBtn;

	private int language;

	private Transform All_PayPanelController;

	private void Awake()
	{
		if (STWM_MB_Singleton<STWM_MajorGameController>._instance == null)
		{
			STWM_MB_Singleton<STWM_MajorGameController>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		Init();
	}

	private void OnEnable()
	{
		GetCoinIn();
	}

	public void PreInit()
	{
		goUIContainer = base.gameObject;
		goContainer = base.gameObject;
		cellManager = base.transform.Find("Cells").GetComponent<STWM_CellManager>();
		lineManager = base.transform.Find("Lines").GetComponent<STWM_LinesManager>();
		ballManager = base.transform.Find("LineBalls").GetComponent<STWM_BallManager>();
		animFlag = base.transform.Find("Flag").GetComponent<STWM_NPCAnimator>();
		animDrum = base.transform.Find("Drum").GetComponent<STWM_NPCAnimator>();
		cellManager.gameObject.SetActive(value: true);
		lineManager.gameObject.SetActive(value: true);
		ballManager.gameObject.SetActive(value: true);
		cellManager.PreInit();
		goContainer.SetActive(value: false);
	}

	public void Init()
	{
		cellManager.rollingFinishedAction = Handle_RollingFinished;
		cellManager.allFinishedAction = Handle_AllFinished;
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("gameResult", HandleNetMsg_GameResult);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("maryResult", HandleNetMsg_MaryResult);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("deskInfo", HandleNetMsg_DeskInfo);
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().RegisterHandler("updateGoldAndScore", UpdateGoldAndScore);
		STWM_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().onMaryGameEnd = Handle_MaryEndGame;
		language = ((STWM_GVars.language == "en") ? 1 : 0);
		imgBetLine.sprite = spiBetLine[language];
		imgSingleBet.sprite = spiSingleBet[language];
		imgTotalBet.sprite = spiTotalBet[language];
		imgTotalScore.sprite = spiTotalScore[language];
		imgWinScore.sprite = spiWinScore[language];
		txtShowPath.text = ((language != 0) ? "ShowPath" : "路径显示");
		txtMaxBet.text = ((language != 0) ? "Max Bet" : "最大押分");
		txtDice.text = ((language != 0) ? "Dice" : "比倍");
	}

	public void Show()
	{
		STWM_MB_Singleton<STWM_GameManager>.GetInstance().ChangeView("MajorGame");
		STWM_Utils.TrySetActive(goUIContainer, active: true);
		STWM_Utils.TrySetActive(goContainer, active: true);
		SetCredit(STWM_GVars.credit);
		StakeRebound();
		STWM_LockManager.UnLock("btn_options", force: true);
		STWM_MB_Singleton<STWM_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		STWM_MB_Singleton<STWM_OptionsController>.GetInstance().onItemBank = OnItemBankAction;
		STWM_MB_Singleton<STWM_OptionsController>.GetInstance().onItemSettings = OnSettingAction;
		STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
	}

	public void GetCoinIn()
	{
		int goldNum = GetGoldNum();
		if (goldNum != 0)
		{
			Send_UserCoinIn(goldNum);
		}
	}

	public void Send_UserCoinIn(int coin)
	{
		UnityEngine.Debug.LogError("=======取分: " + coin);
		object[] args = new object[1]
		{
			coin
		};
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/userCoinIn", args);
	}

	public int GetGoldNum()
	{
		return (STWM_GVars.desk.roomId == 1) ? STWM_GVars.user.expeGold : STWM_GVars.user.gameGold;
	}

	public void Hide()
	{
		if (_task_WinLineAni != null)
		{
			_task_WinLineAni.Stop(doNothing: true);
		}
		if (_task_NormalReduceTextAni != null)
		{
			_task_NormalReduceTextAni.Stop(doNothing: true);
		}
		_task_WinLineAni = null;
		_task_NormalReduceTextAni = null;
		STWM_Utils.TrySetActive(goUIContainer, active: false);
		STWM_Utils.TrySetActive(goContainer, active: false);
		STWM_SoundManager.Instance.StopMajorAudio();
	}

	public void ResetGame()
	{
		if (cellManager != null)
		{
			cellManager.ResetAllCells();
		}
		cellManager.SetAllCellsState();
		cellManager.HideAllCellBorders();
		ballManager.HideAllBall();
		lineManager.HideAllLines();
		STWM_MB_Singleton<STWM_HUDController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_HUDController>.GetInstance().HideRules();
		STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_OptionsController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_SettingsController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_DiceGameController2>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().Hide();
		STWM_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
	}

	public void InitGame(bool halfInit = false)
	{
		STWM_MB_Singleton<STWM_HUDController>.GetInstance().Show();
		STWM_MB_Singleton<STWM_HUDController>.GetInstance().ResetSprite();
		STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().SetKeepScore(0);
		STWM_LockManager.UnLock("ScoreBank", force: true);
		STWM_LockManager.UnLock("btn_options", force: true);
		STWM_MB_Singleton<STWM_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		if (_task_WinLineAni != null)
		{
			_task_WinLineAni.Stop(doNothing: true);
		}
		if (_task_NormalReduceTextAni != null)
		{
			_task_NormalReduceTextAni.Stop(doNothing: true);
		}
		_task_WinLineAni = null;
		_task_NormalReduceTextAni = null;
		STWM_LockManager.UnLock("Delay_Overflow");
		bGaming = false;
		if (!halfInit)
		{
			SetLineNum(9);
			SetSingleStake(STWM_GVars.desk.minSinglelineBet);
			SetTotalStake(9 * STWM_GVars.desk.minSinglelineBet);
			SetCredit(0, 0, check: true);
			bAuto = false;
			SetBtnAutoState("auto");
		}
		SetWinScore(0);
		JudgeBtnMinusPlus();
		SetBtnMaxStakeEnable(isEnable: true);
		JudgeBtnDice();
		SetBtnShowLinesEnable(isEnable: true);
		canShowWinLines = false;
		bReplaying = false;
		majorHints.Show();
		majorHints.PlayHint(CalculateDice() ? "Normal_Dice" : "Normal");
		animDrum.Play("normal");
		animFlag.Play("normal");
		cellManager.SetAllCellsState();
		cellManager.HideAllCellBorders();
		ballManager.HideAllBall();
		lineManager.HideAllLines();
		lastResult = null;
		maryTimes = 0;
		if (STWM_GVars.credit != 0 || winScore == 0)
		{
		}
		SetBtnStartState("start");
		HideBigWinTip();
		HideWinScoreTip();
		if (bAuto && winScore == 0)
		{
			StartGame();
		}
	}

	public void ExitGame()
	{
		STWM_MB_Singleton<STWM_HUDController>.GetInstance().Hide();
		STWM_SoundManager.Instance.StopNumberRollAudio();
		Send_LeaveDesk();
	}

	private void WinningLineAnimationControl(bool isReplay = false)
	{
		int totalWin = cellManager.CalHitResult2(lastResult);
		float delay = isReplay ? 0.1f : 0.1f;
		_task_WinLineAni = new STWM_XTask(cellManager.WinLinesAni(isReplay, delay, lastLineNum, lastSingleStake, lineManager, ballManager, delegate(int win)
		{
			if (!isReplay)
			{
				SetWinScore(winScore + win);
			}
			majorHints.SetWinValue(win);
			majorHints.PlayHint("WinTip");
		}, delegate
		{
			animDrum.Play("celebrate");
			animFlag.Play("celebrate");
			majorHints.SetWinValue(totalWin);
			majorHints.PlayHint("WinTip");
		}));
		STWM_XTask task_WinLineAni = _task_WinLineAni;
		task_WinLineAni.Finished = (STWM_XTask.FinishedHandler)Delegate.Combine(task_WinLineAni.Finished, (STWM_XTask.FinishedHandler)delegate(bool manual)
		{
			cellManager.StopShowAllHitCell();
			if (manual)
			{
				if (!isReplay)
				{
					SetWinScore(totalWin);
				}
				Handle_AllFinished();
			}
			if (!bAuto && !manual)
			{
				SetBtnShowLinesEnable(isEnable: true);
			}
			JudgeBtnDice();
			if (totalWin > 0)
			{
				majorHints.SetWinValue(totalWin);
				majorHints.PlayHint(CalculateDice() ? "Win_Dice" : "Win");
			}
			else
			{
				majorHints.PlayHint(CalculateDice() ? "Normal_Dice" : "Normal");
			}
			_task_WinLineAni = null;
		});
	}

	public void JudgeResetGame(int credit = 0)
	{
		UnityEngine.Debug.Log("JudgeResetGame");
		if (STWM_GVars.curView == "MajorGame")
		{
			HandleNoResponse_GameStart(credit);
			return;
		}
		if (STWM_GVars.curView == "DiceGame")
		{
			STWM_MB_Singleton<STWM_DiceGameController2>.GetInstance().HandleNoResponse_MultiInfo();
			SetCredit(credit);
		}
		UnityEngine.Debug.Log("no need reset game");
	}

	public void OnBtnStart_Click()
	{
		if (STWM_GVars.tryLockOnePoint)
		{
			return;
		}
		STWM_SoundManager.Instance.StopMaryAudio();
		if (bStopBtn)
		{
			cellManager.celebrateCount = 4;
			if (_task_NormalReduceTextAni != null)
			{
				_task_NormalReduceTextAni.Stop();
				STWM_SoundManager.Instance.NumberRollEndAudio();
			}
			if (_task_WinLineAni != null)
			{
				_task_WinLineAni.Stop();
				STWM_SoundManager.Instance.StopMajorAudio();
			}
			if (!cellManager.forceStopRolling)
			{
				cellManager.forceStopRolling = true;
				STWM_SoundManager.Instance.PlayMajorRollEndAudio();
			}
		}
		else if (bGainScoreBtn)
		{
			STWM_SoundManager.Instance.PlayClickAudio();
			GainScore();
		}
		else
		{
			StartGame();
		}
	}

	private void GainScore()
	{
		STWM_LockManager.Lock("ScoreBank");
		STWM_LockManager.Lock("btnDice");
		int finalCredit = STWM_GVars.credit + winScore;
		bStopBtn = true;
		bGainScoreBtn = false;
		majorHints.PlayHint("GainScore");
		_task_NormalReduceTextAni = new STWM_XTask(STWM_Utils.NormalReduceTextAni(0f, 0.05f, winScore, delegate(int total, int diff)
		{
			SetWinScore(winScore - diff);
			SetBigWinTip(winScore - diff);
			SetCredit(STWM_GVars.credit + diff);
		}, null));
		STWM_XTask task_NormalReduceTextAni = _task_NormalReduceTextAni;
		task_NormalReduceTextAni.Finished = (STWM_XTask.FinishedHandler)Delegate.Combine(task_NormalReduceTextAni.Finished, (STWM_XTask.FinishedHandler)delegate(bool manual)
		{
			if (manual)
			{
				SetWinScore(0);
				SetCredit(finalCredit, -1, check: true);
			}
			SetSomeBtnsEnable(isEnable: true);
			if (totalStake < STWM_GVars.credit)
			{
				StakeRebound();
			}
			JudgeBtnMinusPlus();
			SetBtnStartState("start");
			STWM_LockManager.UnLock("ScoreBank", force: true);
			STWM_LockManager.UnLock("btnDice", force: true);
			_task_NormalReduceTextAni = null;
			canShowWinLines = false;
			majorHints.PlayHint(CalculateDice() ? "Normal_Dice" : "Normal");
			HideBigWinTip();
			SetBtnShowLinesEnable(isEnable: true);
			if (STWM_MB_Singleton<STWM_NetManager>.GetInstance().isReady)
			{
				if (bAuto)
				{
					StartGame();
				}
			}
			else if (bAuto)
			{
				UnityEngine.Debug.Break();
				StartCoroutine(STWM_Utils.WaitCall(() => STWM_MB_Singleton<STWM_NetManager>.GetInstance().isReady, delegate
				{
					StartGame();
				}));
			}
		});
	}

	private void StartGame()
	{
		int rightGold = STWM_ScoreBank.GetRightGold();
		if (STWM_GVars.credit < totalStake)
		{
			if (STWM_GVars.credit == 0 || (STWM_GVars.credit < STWM_GVars.desk.minSinglelineBet * 9 && rightGold != 0))
			{
				NoStart();
				SetAndIfChangeThenPlayAutoAni(toAuto: false);
				return;
			}
			if (rightGold == 0 && STWM_GVars.credit < STWM_GVars.desk.minSinglelineBet * 9)
			{
			}
		}
		LowCreditMaxStake();
		if (lineNum < 9 || singleStake < STWM_GVars.desk.minSinglelineBet || STWM_GVars.credit < STWM_GVars.desk.minSinglelineBet * 9)
		{
			UnityEngine.Debug.LogError("============");
			NoStart();
			return;
		}
		SetBtnStartState("disable");
		SetCredit(STWM_GVars.credit - totalStake, STWM_GVars.credit - totalStake);
		coGridRolling = StartCoroutine(cellManager.GridRollingControl(delegate
		{
			coGridRolling = null;
		}));
		majorHints.PlayHint("Rolling");
		lastResult = null;
		Send_GameStart();
		listAni.Clear();
		animDrum.Play("celebrate");
		animFlag.Play("celebrate");
		STWM_SoundManager.Instance.PlayMajorRollAudio();
		SetSomeBtnsEnable(isEnable: false);
		STWM_LockManager.Lock("ScoreBank");
		SetBtnShowLinesEnable(isEnable: false);
		lineManager.HideAllLines();
		canShowWinLines = false;
		bGaming = true;
		STWM_LockManager.Lock("Delay_Overflow");
	}

	public void OnBtnMinus_Click()
	{
		UnityEngine.Debug.Log("OnBtnMinus_Click");
		if (STWM_GVars.tryLockOnePoint)
		{
			return;
		}
		STWM_SoundManager.Instance.PlayClickAudio();
		animDrum.Play("hitonce");
		STWM_Desk desk = STWM_GVars.desk;
		if (totalStake >= desk.minSinglelineBet * 9)
		{
			int num = (singleStake == desk.minSinglelineBet) ? desk.maxSinglelineBet : (singleStake - desk.singlechangeScore);
			num = ((num < desk.minSinglelineBet) ? desk.minSinglelineBet : num);
			if (num < STWM_GVars.desk.minSinglelineBet)
			{
				num = STWM_GVars.desk.minSinglelineBet;
			}
			SetLineNum(9);
			SetSingleStake(num);
			SetTotalStake(num * 9);
			UnityEngine.Debug.Log("newSingleStake: " + num);
		}
		else
		{
			UnityEngine.Debug.LogError($"BtnMinus should not be clicked. _totalStake: {totalStake}, minSinglelineBet: {desk.minSinglelineBet}*9");
		}
	}

	public void OnBtnPlus_Click()
	{
		UnityEngine.Debug.Log("OnBtnPlus_Click");
		if (STWM_GVars.tryLockOnePoint)
		{
			return;
		}
		STWM_SoundManager.Instance.PlayClickAudio();
		animDrum.Play("hitonce");
		STWM_Desk desk = STWM_GVars.desk;
		if (totalStake >= desk.minSinglelineBet * 9)
		{
			int num = (singleStake == desk.maxSinglelineBet) ? desk.minSinglelineBet : (singleStake + desk.singlechangeScore);
			num = ((num > desk.maxSinglelineBet) ? desk.maxSinglelineBet : num);
			if (num < STWM_GVars.desk.minSinglelineBet)
			{
				num = STWM_GVars.desk.minSinglelineBet;
			}
			SetLineNum(9);
			SetSingleStake(num);
			SetTotalStake(num * 9);
			UnityEngine.Debug.Log("newSingleStake: " + num);
		}
		else
		{
			UnityEngine.Debug.LogError($"BtnPlus should not be clicked. _totalStake: {totalStake}, minSinglelineBet: {desk.minSinglelineBet}*9");
		}
	}

	public void OnBtnMaxStake_Click()
	{
		if (STWM_GVars.tryLockOnePoint)
		{
			return;
		}
		STWM_SoundManager.Instance.PlayClickAudio();
		STWM_Desk desk = STWM_GVars.desk;
		int num = lineNum;
		int num2 = singleStake;
		if (STWM_GVars.credit >= desk.maxSinglelineBet * 9)
		{
			num = 9;
			num2 = desk.maxSinglelineBet;
		}
		else if (STWM_GVars.credit >= desk.minSinglelineBet * 9)
		{
			num = 9;
			int origin = STWM_GVars.credit / 9;
			num2 = CalGearValue(desk.minSinglelineBet, desk.maxSinglelineBet, desk.singlechangeScore, origin);
		}
		else if (STWM_GVars.credit >= 9)
		{
			num = 9;
			num2 = STWM_GVars.credit / 9;
		}
		else
		{
			if (STWM_GVars.credit >= 9 || STWM_GVars.credit <= 0)
			{
				return;
			}
			num = STWM_GVars.credit;
			num2 = 1;
		}
		if (num < 9)
		{
			UnityEngine.Debug.LogError("==============不足9线============");
			NoStart();
			return;
		}
		if (num2 < STWM_GVars.desk.minSinglelineBet)
		{
			num2 = STWM_GVars.desk.minSinglelineBet;
		}
		SetLineNum(num);
		SetSingleStake(num2);
		SetTotalStake(num2 * num);
		JudgeBtnMinusPlus();
	}

	private void LowCreditMaxStake()
	{
		if (STWM_GVars.credit > singleStake * lineNum)
		{
			return;
		}
		STWM_Desk desk = STWM_GVars.desk;
		int num = lineNum;
		int num2 = singleStake;
		if (STWM_GVars.credit >= desk.minSinglelineBet * 9)
		{
			num = 9;
			int origin = STWM_GVars.credit / 9;
			num2 = CalGearValue(desk.minSinglelineBet, desk.maxSinglelineBet, desk.singlechangeScore, origin);
		}
		else if (STWM_GVars.credit >= 9)
		{
			num = 9;
			num2 = STWM_GVars.credit / 9;
		}
		else
		{
			if (STWM_GVars.credit >= 9 || STWM_GVars.credit <= 0)
			{
				return;
			}
			num = STWM_GVars.credit;
			num2 = 1;
		}
		if (STWM_GVars.credit < 9)
		{
			UnityEngine.Debug.LogError("==============不足9线============");
			NoStart();
			return;
		}
		if (num2 < STWM_GVars.desk.minSinglelineBet)
		{
			num2 = STWM_GVars.desk.minSinglelineBet;
		}
		SetLineNum(num);
		SetSingleStake(num2);
		SetTotalStake(num2 * num);
	}

	private void NoStart()
	{
		if (All_PayPanelController == null)
		{
			All_PayPanelController = base.transform.parent.Find("ALL_PayPanel");
		}
		string content = "金币不足,是否前往商城充值";
		STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: true, delegate
		{
			if (ZH2_GVars.OpenPlyBoxPanel != null)
			{
				ZH2_GVars.OpenPlyBoxPanel(ZH2_GVars.GameType_DJ.water_desk);
			}
		});
	}

	public void OnBtnTestNet_Click()
	{
		UnityEngine.Debug.Log("OnBtnTestNet");
		if (!STWM_GVars.tryLockOnePoint)
		{
			STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().Send_EnterDesk(295);
		}
	}

	public void OnBtnDice_Click()
	{
		UnityEngine.Debug.Log("OnBtnDice_Click");
		if (STWM_GVars.tryLockOnePoint)
		{
			return;
		}
		STWM_SoundManager.Instance.StopMaryAudio();
		STWM_SoundManager.Instance.PlayClickAudio();
		int diceBet = 0;
		if (STWM_LockManager.IsLocked("btnDice") || bReplaying)
		{
			return;
		}
		if (STWM_GVars.credit == 0 && winScore == 0)
		{
			STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().Show();
			STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().InitBank();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			return;
		}
		if (winScore == 0)
		{
			diceBet = ((STWM_GVars.credit <= totalStake) ? STWM_GVars.credit : totalStake);
			STWM_GVars.credit -= diceBet;
			SetCredit(STWM_GVars.credit);
		}
		else
		{
			int num = winScore;
			SetBtnStartState("start");
			majorHints.PlayHint(CalculateDice() ? "Normal_Dice" : "Normal");
			diceBet = winScore;
			HideBigWinTip();
		}
		SetAndIfChangeThenPlayAutoAni(toAuto: false);
		canShowWinLines = false;
		STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "OnBtnDice");
		STWM_MB_Singleton<STWM_GameRoot>.GetInstance().StartCoroutine(STWM_Utils.DelayCall(0.1f, delegate
		{
			STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "OnBtnDice done");
			SetBtnShowLinesEnable(isEnable: false);
			lineManager.HideAllLines();
			canShowWinLines = false;
			Hide();
			STWM_MB_Singleton<STWM_DiceGameController>.GetInstance().Show();
			STWM_MB_Singleton<STWM_DiceGameController2>.GetInstance().Show();
			STWM_MB_Singleton<STWM_DiceGameController2>.GetInstance().InitGame(STWM_GVars.credit, diceBet);
			STWM_SoundManager.Instance.PlayDiceBGM();
			SetWinScore(0);
		}));
	}

	public void OnBtnAuto_Click()
	{
		UnityEngine.Debug.Log("OnBtnShowLines_Click");
		if (STWM_GVars.tryLockOnePoint)
		{
			return;
		}
		STWM_SoundManager.Instance.PlayClickAudio();
		STWM_SoundManager.Instance.StopMaryAudio();
		if (bReplaying)
		{
			if (_task_WinLineAni != null)
			{
				_task_WinLineAni.Stop();
				STWM_SoundManager.Instance.StopMajorAudio();
			}
			if (!cellManager.forceStopRolling)
			{
				cellManager.forceStopRolling = true;
				STWM_SoundManager.Instance.PlayMajorRollEndAudio();
			}
			return;
		}
		if (STWM_GVars.credit == 0 && winScore == 0)
		{
			STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().Show();
			STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().InitBank();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			return;
		}
		SetAndIfChangeThenPlayAutoAni(!bAuto);
		if (!bGaming)
		{
			if (bAuto && winScore != 0 && _task_NormalReduceTextAni == null)
			{
				GainScore();
			}
			if (bAuto && winScore == 0)
			{
				StartGame();
			}
		}
	}

	private void PlayAutoAni()
	{
		if (tfAuto != null)
		{
			if (bAuto)
			{
				tfAuto.DOBlendableLocalMoveBy(Vector3.up * 50f, 0.5f);
			}
			else
			{
				tfAuto.DOBlendableLocalMoveBy(Vector3.down * 50f, 0.5f);
			}
		}
	}

	public void OnBtnShowLines_Click()
	{
		UnityEngine.Debug.Log("OnBtnShowLines_Click");
		if (!STWM_GVars.tryLockOnePoint)
		{
			STWM_SoundManager.Instance.PlayClickAudio();
			if (canShowWinLines)
			{
				bReplaying = true;
				WinningLineAnimationControl(bReplaying);
				SetBtnShowLinesEnable(isEnable: false);
				SetBtnStartState("stop");
				SetBtnDiceEnable(isEnable: false);
			}
		}
	}

	public void OnBtnShowLines_PointDown()
	{
		if (STWM_GVars.tryLockOnePoint)
		{
			UnityEngine.Debug.Log("OnBtnShowLines_PointDown");
		}
		else if (!canShowWinLines && btnShowLines.interactable)
		{
			lineManager.ShowLines(lineNum);
		}
	}

	public void OnBtnShowLines_PointUp()
	{
		if (!canShowWinLines && btnShowLines.interactable)
		{
			lineManager.HideAllLines();
		}
	}

	public void Send_GameStart()
	{
		object[] args = new object[4]
		{
			STWM_GVars.desk.id,
			lineNum,
			singleStake,
			totalStake
		};
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/gameStart", args);
		STWM_StateManager.GetInstance().RegisterWork("Send_GameStart");
		STWM_StateManager.GetInstance().RegisterWork("MaryGame");
		gameTimes++;
		int _gameTimes = gameTimes;
		StartCoroutine(STWM_Utils.DelayCall(10f, delegate
		{
			if (_gameTimes == gameTimes && STWM_MB_Singleton<STWM_NetManager>.GetInstance().isConnected && (!STWM_StateManager.GetInstance().IsWorkCompleted("Send_GameStart") || !STWM_StateManager.GetInstance().IsWorkCompleted("MaryGame")))
			{
				STWM_MB_Singleton<STWM_NetManager>.GetInstance().OnDestroy();
			}
		}));
	}

	public void Send_LeaveDesk()
	{
		object[] args = new object[1]
		{
			STWM_GVars.desk.id
		};
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/leaveDesk", args);
		Send_LeaveRoom();
	}

	public void Send_LeaveRoom()
	{
		object[] args = new object[1]
		{
			STWM_GVars.desk.roomId
		};
		STWM_MB_Singleton<STWM_NetManager>.GetInstance().Send("userService/leaveRoom", args);
	}

	public void HandleNetMsg_GameResult(object[] args)
	{
		STWM_StateManager.GetInstance().CompleteWork("Send_GameStart");
		Dictionary<string, object> dic = args[0] as Dictionary<string, object>;
		lastResult = new STWM_MajorResult().Init(lineNum, singleStake, totalStake, dic);
		if (!lastResult.maryGame)
		{
			STWM_StateManager.GetInstance().CompleteWork("MaryGame");
		}
		SetCredit(-1, lastResult.totalWin + STWM_GVars.realCredit);
		cellManager.UpdateCellGridData(lastResult.gameContent);
		cellManager.stopRolling = true;
		SetBtnStartState("stop");
	}

	public void HandleNoResponse_GameStart(int credit)
	{
		if (false || !STWM_StateManager.GetInstance().IsWorkCompleted("Send_GameStart") || !STWM_StateManager.GetInstance().IsWorkCompleted("MaryGame"))
		{
			UnityEngine.Debug.Log("Send_GameStart or MaryGame was not completed");
			STWM_StateManager.GetInstance().CompleteWork("Send_GameStart");
			STWM_StateManager.GetInstance().CompleteWork("MaryGame");
			if (coGridRolling != null)
			{
				StopCoroutine(coGridRolling);
				coGridRolling = null;
				ResetGame();
				InitGame(halfInit: true);
				if (!bAuto)
				{
					STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog((STWM_GVars.language == "zh") ? "网络异常，请继续游戏" : "Network error，please go on");
				}
			}
			SetCredit(credit);
		}
		else
		{
			UnityEngine.Debug.Log("HandleNoResponse_GameStart ok");
		}
	}

	public void UpdateCredit(int credit)
	{
		SetCredit(credit);
	}

	public void HandleNetMsg_MaryResult(object[] args)
	{
		UnityEngine.Debug.Log(STWM_LogHelper.NetHandle("HandleNetMsg_MaryResult"));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		STWM_StateManager.GetInstance().CompleteWork("MaryGame");
		int[] photoNumbers = (int[])dictionary["photoNumber"];
		int[][] photosArray = (int[][])dictionary["photos"];
		int credit = (int)dictionary["credit"];
		int totalBet = (int)dictionary["totalBet"];
		int[] totalWins = (int[])dictionary["totalWin"];
		float beginTime = Time.time;
		if (lastResult == null)
		{
			UnityEngine.Debug.Log("MaryResult is faster than GameResult");
		}
		StartCoroutine(STWM_Utils.WaitCall(() => lastResult != null, delegate
		{
			UnityEngine.Debug.Log($"wait: {Time.time - beginTime}s");
			STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().PrepareGame(lastResult.maryTimes, STWM_GVars.credit, totalBet, photoNumbers, photosArray, totalWins);
			SetCredit(-1, credit);
		}));
	}

	public void UpdateGoldAndScore(object[] args)
	{
		STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().HandleNetMsg_UpdateGoldAndScore(args);
	}

	public void HandleNetMsg_DeskInfo(object[] args)
	{
		UnityEngine.Debug.Log(STWM_LogHelper.NetHandle("HandleNetMsg_DeskInfo"));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["lines"];
		int num2 = (int)dictionary["lineScore"];
		int num3 = (int)dictionary["winScore"];
		int num4 = (int)dictionary["allBetScore"];
		int num5 = (int)dictionary["userId"];
		int num6 = (int)dictionary["userScore"];
		int num7 = (int)dictionary["userGold"];
	}

	public void Handle_RollingFinished()
	{
		lastLineNum = lineNum;
		lastSingleStake = singleStake;
		WinningLineAnimationControl();
		animDrum.Play("normal");
		animFlag.Play("normal");
	}

	public bool JungeAuto()
	{
		return bAuto;
	}

	private void Handle_AllFinished()
	{
		cellManager.SetAllCellsState();
		cellManager.HideAllCellBorders();
		ballManager.HideAllBall();
		lineManager.HideAllLines();
		animDrum.Play("normal");
		animFlag.Play("normal");
		bGaming = false;
		STWM_LockManager.UnLock("Delay_Overflow");
		if (winScore > 0)
		{
			SetBtnStartState("gainScore");
			majorHints.PlayHint(CalculateDice() ? "Win_Dice" : "Win");
			if (winScore >= singleStake * 20 && lastResult.maryTimes == 0)
			{
				ShowBigWinTip(winScore);
			}
			canShowWinLines = true;
			if (!bAuto)
			{
				SetBtnShowLinesEnable(isEnable: true);
			}
		}
		else
		{
			SetBtnStartState("start");
			SetSomeBtnsEnable(isEnable: true);
			JudgeBtnMinusPlus();
			canShowWinLines = false;
			majorHints.PlayHint(CalculateDice() ? "Normal_Dice" : "Normal");
			STWM_LockManager.UnLock("ScoreBank", force: true);
			STWM_LockManager.UnLock("btn_options", force: true);
			STWM_MB_Singleton<STWM_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
			SetBtnShowLinesEnable(isEnable: true);
			if (STWM_GVars.credit == 0)
			{
				UnityEngine.Debug.Log("Lose: credit is zero");
				SetAndIfChangeThenPlayAutoAni(toAuto: false);
			}
		}
		if (bReplaying)
		{
			bReplaying = false;
			return;
		}
		if (lastResult.maryTimes > 0)
		{
			bGaming = true;
			STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "enter mary");
			StartCoroutine(STWM_Utils.DelayCall(0.01f, delegate
			{
				STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().bEnterMary = true;
				STWM_MB_Singleton<STWM_MaryMovieController>.GetInstance().Play();
				STWM_MB_Singleton<STWM_HUDController>.GetInstance().Hide();
				STWM_MB_Singleton<STWM_MaryMovieController>.GetInstance().OnMiddle = delegate
				{
					Hide();
					STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().Show();
				};
				STWM_MB_Singleton<STWM_MaryMovieController>.GetInstance().OnEnd = delegate
				{
					STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().StartGame(winScore);
				};
			}));
			return;
		}
		JudgeBtnDice();
		if (STWM_LockManager.IsLocked("Overflow"))
		{
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			HandleOverflow();
		}
		if (bAuto)
		{
			GainScore();
		}
	}

	public void Handle_MaryEndGame(int maryWin)
	{
		STWM_MB_Singleton<STWM_MaryGameController>.GetInstance().Hide();
		Show();
		STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "enter mary");
		STWM_MB_Singleton<STWM_HUDController>.GetInstance().Show();
		SetWinScore(maryWin);
		ShowBigWinTip(maryWin);
		majorHints.SetWinValue(maryWin);
		JudgeBtnDice();
		bGaming = false;
		if (STWM_LockManager.IsLocked("Overflow"))
		{
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			HandleOverflow();
		}
		if (bAuto)
		{
			GainScore();
		}
	}

	private void HandleOverflow()
	{
		string content = (STWM_GVars.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge";
		STWM_LockManager.Lock("Esc");
		STWM_LockManager.Lock("Quit");
		STWM_MB_Singleton<STWM_GameManager>.Get().PrepareQuitGame();
		STWM_MB_Singleton<STWM_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			STWM_MB_Singleton<STWM_GameManager>.GetInstance().QuitToHallGame();
		});
	}

	public void OnDiceExit()
	{
		SetWinScore(0);
		JudgeBtnDice();
		JudgeBtnMinusPlus();
		StakeRebound();
		SetBtnShowLinesEnable(isEnable: true);
	}

	private void OnItemBankAction()
	{
		STWM_MB_Singleton<STWM_OptionsController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().Show();
		STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().InitBank();
	}

	private void OnSettingAction()
	{
		STWM_MB_Singleton<STWM_OptionsController>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_SettingsController>.GetInstance().Show();
	}

	private void OnSetScoreAndGoldAction()
	{
		if (!goUIContainer.activeInHierarchy)
		{
			return;
		}
		txtCredit.text = STWM_GVars.credit.ToString();
		if (STWM_GVars.credit == 0 && winScore == 0)
		{
			majorHints.PlayHint(CalculateDice() ? "Normal_Dice" : "Normal");
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			return;
		}
		SetSomeBtnsEnable(isEnable: true);
		if (winScore > 0)
		{
			majorHints.PlayHint(CalculateDice() ? "Win_Dice" : "Win");
		}
		else
		{
			majorHints.PlayHint(CalculateDice() ? "Normal_Dice" : "Normal");
		}
		StakeRebound();
		JudgeBtnMinusPlus();
	}

	private void SetLineNum(int lineNum)
	{
		this.lineNum = lineNum;
		textLineNum.text = this.lineNum.ToString();
	}

	private void SetSingleStake(int stake)
	{
		singleStake = stake;
		txtSingleStake.text = singleStake.ToString();
	}

	private void SetTotalStake(int totalStake)
	{
		this.totalStake = totalStake;
		txtTotalStake.text = this.totalStake.ToString();
	}

	private void SetCredit(int credit, int realCredit = -1, bool check = false)
	{
		if (credit >= 0)
		{
			STWM_GVars.credit = credit;
			txtCredit.text = credit.ToString();
		}
		if (realCredit >= 0)
		{
			STWM_GVars.realCredit = realCredit;
		}
		if (check && STWM_GVars.credit != STWM_GVars.realCredit)
		{
			UnityEngine.Debug.LogError($"credit: {STWM_GVars.credit}, realCredit: {STWM_GVars.realCredit}");
		}
	}

	private void SetWinScore(int winScore)
	{
		this.winScore = winScore;
		txtWinScore.text = this.winScore.ToString();
	}

	private void SetBtnDiceEnable(bool isEnable)
	{
		btnDice.interactable = isEnable;
	}

	private void SetBtnAutoEnable(bool isEnable)
	{
		btnAuto.interactable = isEnable;
	}

	private void SetBtnShowLinesEnable(bool isEnable)
	{
		btnShowLines.interactable = isEnable;
	}

	private void SetBtnMinusEnable(bool isEnable)
	{
		btnMinus.interactable = isEnable;
	}

	private void SetBtnPlusEnable(bool isEnable)
	{
		btnPlus.interactable = isEnable;
	}

	private void SetBtnMaxStakeEnable(bool isEnable)
	{
		btnMaxStake.interactable = isEnable;
	}

	private void SetSomeBtnsEnable(bool isEnable)
	{
		if (!isEnable)
		{
			SetBtnMinusEnable(isEnable);
			SetBtnPlusEnable(isEnable);
		}
		SetBtnMaxStakeEnable(isEnable);
		if (isEnable)
		{
			JudgeBtnDice();
		}
		else
		{
			SetBtnDiceEnable(isEnable: false);
		}
	}

	private void SetBtnStartState(string state)
	{
		bGainScoreBtn = false;
		bStopBtn = false;
		if (state == "start")
		{
			btnStart.interactable = true;
			if (language == 0)
			{
				imgBtnStart.sprite = spiBtnStart;
			}
			else
			{
				imgBtnStart.sprite = spiArrEn[0];
			}
		}
		else if (state == "stop")
		{
			btnStart.interactable = true;
			if (language == 0)
			{
				imgBtnStart.sprite = spiBtnStop;
			}
			else
			{
				imgBtnStart.sprite = spiArrEn[1];
			}
			bStopBtn = true;
		}
		else if (state == "gainScore")
		{
			btnStart.interactable = true;
			if (language == 0)
			{
				imgBtnStart.sprite = spiBtnGainScore;
			}
			else
			{
				imgBtnStart.sprite = spiArrEn[2];
			}
			bGainScoreBtn = true;
		}
		else if (state == "disable")
		{
			btnStart.interactable = false;
			if (language == 0)
			{
				imgBtnStart.sprite = spiBtnStart;
			}
			else
			{
				imgBtnStart.sprite = spiArrEn[0];
			}
		}
	}

	private void SetBtnAutoState(string state)
	{
		if (state == "auto")
		{
			txtBtnAuto.text = ((language != 0) ? "Auto" : "自动");
		}
		else if (state == "cancel")
		{
			txtBtnAuto.text = ((language != 0) ? "Cancel" : "取消自动");
		}
	}

	private void ShowWinScoreTip(int win)
	{
		goWinScoreTip.SetActive(value: true);
		txtWinScoreTip.text = win.ToString();
	}

	private void HideWinScoreTip()
	{
		goWinScoreTip.SetActive(value: false);
	}

	private void ShowBigWinTip(int win)
	{
		if (!goBigWinTip.activeSelf)
		{
			goBigWinTip.SetActive(value: true);
		}
		txtBigWinTip.text = win.ToString();
	}

	private void SetBigWinTip(int win)
	{
		txtBigWinTip.text = win.ToString();
	}

	private void HideBigWinTip()
	{
		goBigWinTip.SetActive(value: false);
	}

	private void AdjustBetNumber()
	{
		STWM_Desk desk = STWM_GVars.desk;
		UnityEngine.Debug.Log($"GVars.credit: {STWM_GVars.credit}, _totalStake: {totalStake},_singleStake: {singleStake},minSinglelineBet: {desk.minSinglelineBet}");
		if (STWM_GVars.credit == 0)
		{
			STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().Show();
			STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().InitBank();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
		}
		else
		{
			if (STWM_GVars.credit < 9)
			{
				UnityEngine.Debug.LogError("==============不足9线============");
				NoStart();
				return;
			}
			if (STWM_GVars.credit < desk.minSinglelineBet * 9)
			{
				int num = STWM_GVars.credit / 9;
				if (num < STWM_GVars.desk.minSinglelineBet)
				{
					num = STWM_GVars.desk.minSinglelineBet;
				}
				SetLineNum(9);
				SetSingleStake(num);
				SetTotalStake(num * 9);
			}
			else if (singleStake < desk.minSinglelineBet)
			{
				int minSinglelineBet = desk.minSinglelineBet;
				SetLineNum(9);
				SetSingleStake(minSinglelineBet);
				SetTotalStake(minSinglelineBet * 9);
			}
			else if (STWM_GVars.credit < lineNum * singleStake)
			{
				int num2 = STWM_GVars.credit / 9;
				if (num2 < STWM_GVars.desk.minSinglelineBet)
				{
					num2 = STWM_GVars.desk.minSinglelineBet;
				}
				SetLineNum(9);
				SetSingleStake(num2);
				SetTotalStake(num2 * 9);
			}
			else if (lineNum < 9 && STWM_GVars.credit >= 9)
			{
				SetLineNum(9);
				SetSingleStake(singleStake);
				SetTotalStake(singleStake * 9);
			}
		}
		UnityEngine.Debug.Log($"_lineNum: {lineNum}, _totalStake: {totalStake},_singleStake: {singleStake},minSinglelineBet: {desk.minSinglelineBet}");
	}

	private void AudgeBtnMinusPlusMax()
	{
		bool flag = STWM_GVars.credit >= STWM_GVars.desk.minSinglelineBet * 9;
		SetBtnMinusEnable(flag);
		SetBtnPlusEnable(flag);
		SetBtnMaxStakeEnable(flag);
	}

	private void JudgeBtnMinusPlus()
	{
		bool flag = singleStake * lineNum >= STWM_GVars.desk.minSinglelineBet * 9;
		SetBtnMinusEnable(flag);
		SetBtnPlusEnable(flag);
		SetBtnMaxStakeEnable(isEnable: true);
	}

	private void HandleCreditChange()
	{
		int num = int.Parse(txtCredit.text);
		if (num >= STWM_GVars.credit && num > STWM_GVars.credit)
		{
			int rightGold = STWM_ScoreBank.GetRightGold();
			if (STWM_GVars.credit < totalStake && rightGold > 0)
			{
				STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().Show();
			}
		}
	}

	private void StakeRebound()
	{
		STWM_Desk desk = STWM_GVars.desk;
		if (totalStake >= desk.minSinglelineBet * 9)
		{
			return;
		}
		if (STWM_GVars.credit >= desk.minSinglelineBet * 9)
		{
			int minSinglelineBet = desk.minSinglelineBet;
			SetLineNum(9);
			SetSingleStake(minSinglelineBet);
			SetTotalStake(minSinglelineBet * 9);
		}
		else if (STWM_GVars.credit >= 9)
		{
			int num = STWM_GVars.credit / 9;
			if (num < STWM_GVars.desk.minSinglelineBet)
			{
				num = STWM_GVars.desk.minSinglelineBet;
			}
			SetLineNum(9);
			SetSingleStake(num);
			SetTotalStake(num * 9);
		}
		else if (STWM_GVars.credit < 9)
		{
			UnityEngine.Debug.LogError("==============不足9线============");
		}
		else
		{
			SetLineNum(STWM_GVars.credit);
			SetSingleStake(STWM_GVars.desk.minSinglelineBet);
			SetTotalStake(STWM_GVars.credit);
		}
	}

	private void JudgeBtnDice()
	{
		if (CalculateDice())
		{
			SetBtnDiceEnable(isEnable: true);
		}
		else
		{
			SetBtnDiceEnable(isEnable: false);
		}
	}

	private bool CalculateDice()
	{
		return winScore < STWM_GVars.desk.diceOverflow && ((STWM_GVars.desk.diceSwitch == 1 && winScore != 0) || STWM_GVars.desk.diceDirectSwitch == 1);
	}

	private int CalGearValue(int min, int max, int step, int origin)
	{
		int num = min;
		if (step <= 0)
		{
			UnityEngine.Debug.LogError("step must be positive!");
			return num;
		}
		if (origin >= max)
		{
			num = max;
		}
		else if (origin >= min)
		{
			int i;
			for (i = min; i + step <= origin; i += step)
			{
			}
			num = i;
		}
		UnityEngine.Debug.Log($"_calGearValue> origin: {origin}, ret: {num}");
		return num;
	}

	private void SetAndIfChangeThenPlayAutoAni(bool toAuto)
	{
		if (toAuto == bAuto)
		{
			if (bAuto)
			{
				SetBtnAutoState("cancel");
			}
			else
			{
				SetBtnAutoState("auto");
			}
		}
		else if (bAuto)
		{
			bAuto = false;
			SetBtnAutoState("auto");
			PlayAutoAni();
		}
		else
		{
			bAuto = true;
			SetBtnAutoState("cancel");
			PlayAutoAni();
		}
	}
}
