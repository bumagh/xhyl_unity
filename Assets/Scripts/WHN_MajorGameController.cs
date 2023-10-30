using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WHN_MajorGameController : WHN_MB_Singleton<WHN_MajorGameController>
{
	private GameObject goUIContainer;

	private GameObject goContainer;

	private WHN_OptionsController mOptions;

	private WHN_BallManager ballManager;

	private WHN_RollManger RolM;

	private WHN_SymbolManger SymM;

	private WHN_LineGroup LineG;

	private Text textLineNum;

	private Text txtSingleStake;

	private Text txtLineNum;

	private Text freenGameText;

	private Text txtCredit;

	private Text txtWinScore;

	private Text txtDice;

	private Text txtWinBg;

	private int gameLineNum = 20;

	private int scatterid = 12;

	private Button btnStart;

	private Button btnDice;

	private Button btnAuto;

	private Button btnMinus;

	private Button btnPlus;

	private Button btnLineMinus;

	private Button btnLinePlus;

	private Button btnStop;

	private Button btnStopAuto;

	private Transform freenGameTitle;

	private GameObject bg;

	private Transform freenWin;

	private Text freenWinText;

	[HideInInspector]
	public Text tipBGText;

	public Action onBtnDice;

	public Action rollingFinishedAction;

	public Action allFinishedAction;

	private bool bGainScoreBtn;

	private bool bAuto;

	private bool isFreenStop;

	private int winScore;

	private int singleStake;

	private int totalStake;

	private int lineNum;

	private int maryTimes;

	private bool isOneFreen = true;

	private WHN_MajorResult lastResult;

	private List<Coroutine> listAni = new List<Coroutine>();

	private WHN_XTask _task_WinLineAni;

	private WHN_XTask _task_NormalReduceTextAni;

	private bool bGaming;

	private Coroutine coGridRolling;

	private bool canShowWinLines;

	private bool bReplaying;

	private int gameTimes;

	private int lastLineNum;

	private int lastSingleStake;

	private int language;

	private List<int> Lineshowwild;

	private Vector3 poslineg;

	private Vector3 possymg;

	private Transform RightPanel;

	private Transform LeftPanel;

	private Transform RightPanel_Panel;

	private Button bgBtn;

	private Button rightBtn;

	private Button helpBtn;

	private Button moveBtn;

	private Button micBtn;

	private Button blackBtn;

	private Text smallText;

	private Image leftPanelIco;

	private Button lp_Btn_Dec;

	private Button lp_Btn_Add;

	private int maxLpIco = 4;

	private Vector2 rightPanel_PanelOldPos;

	private Vector2 rightPanel_PanelTagPos;

	private Vector2 rightBtnOldPos;

	private Vector2 rightBtnTagPos;

	private Coroutine WaitStartCoroutine;

	private Tweener mTw;

	private int imaIndex;

	private string lpText = "8";

	private bool isFreenGame;

	private bool isOldAuto;

	private Coroutine StopAuto_Click;

	private Coroutine _coroutine;

	[HideInInspector]
	public bool isReStart;

	public List<int> Showchange1
	{
		get;
		set;
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		FinGame();
		possymg = SymM.transform.localPosition;
		poslineg = RolM.transform.localPosition;
		if (WHN_MB_Singleton<WHN_MajorGameController>._instance == null)
		{
			WHN_MB_Singleton<WHN_MajorGameController>.SetInstance(this);
			PreInit();
		}
		RolM.transform.localPosition = new Vector3(1000f, 1000f, 1000f);
		Sequence s = DOTween.Sequence();
		s.AppendInterval(0.1f);
		s.AppendCallback(Cinstance<WHN_Gcore>.Instance.Init);
		gameLineNum = Cinstance<WHN_Gcore>.Instance.lineConut;
		txtLineNum.text = gameLineNum.ToString();
	}

	private void Start()
	{
		Init();
	}

	private void OnEnable()
	{
		WHN_SoundManager.Instance.PlayMajorBGMAndio(isPlay: true);
		tipBGText.text = "点击开始按钮 , 开始游戏";
	}

	private void OnDisable()
	{
		WHN_SoundManager.Instance.PlayMajorBGMAndio(isPlay: false);
	}

	private void FinGame()
	{
		RightPanel = base.transform.Find("RightPanel");
		LeftPanel = base.transform.Find("LeftPanel");
		RightPanel_Panel = RightPanel.Find("Panel");
		bgBtn = RightPanel.Find("BgBtn").GetComponent<Button>();
		rightBtn = RightPanel.Find("RightBtn").GetComponent<Button>();
		helpBtn = RightPanel_Panel.Find("Help").GetComponent<Button>();
		moveBtn = RightPanel_Panel.Find("BtnMove").GetComponent<Button>();
		micBtn = RightPanel_Panel.Find("BtnMic").GetComponent<Button>();
		blackBtn = RightPanel_Panel.Find("BtnBlack").GetComponent<Button>();
		smallText = LeftPanel.Find<Text>("smallText");
		leftPanelIco = LeftPanel.Find<Image>("Ico");
		lp_Btn_Add = LeftPanel.Find<Button>("Btn_Add");
		lp_Btn_Dec = LeftPanel.Find<Button>("Btn_Dec");
		rightPanel_PanelOldPos = RightPanel_Panel.localPosition;
		rightBtnOldPos = rightBtn.transform.localPosition;
		rightPanel_PanelTagPos = new Vector2(900f, 65f);
		rightBtnTagPos = new Vector2(900f, 66f);
		RightPanel_Panel.localPosition = rightPanel_PanelTagPos;
		bgBtn.gameObject.SetActive(value: false);
		btnStart = base.transform.Find("Down/BtnGame").GetComponent<Button>();
		btnStop = base.transform.Find("Down/BtnGameStop").GetComponent<Button>();
		btnDice = base.transform.Find("Down/BtnDice").GetComponent<Button>();
		btnAuto = base.transform.Find("Down/BtnAuto").GetComponent<Button>();
		btnStopAuto = base.transform.Find("Down/BtnStopAuto").GetComponent<Button>();
		btnPlus = base.transform.Find("Down/BtnPlus/AddBtn").GetComponent<Button>();
		btnMinus = base.transform.Find("Down/BtnPlus/DecBtn").GetComponent<Button>();
		btnLineMinus = base.transform.Find("Down/BtnPlusLine/DecBtn").GetComponent<Button>();
		btnLinePlus = base.transform.Find("Down/BtnPlusLine/AddBtn").GetComponent<Button>();
		mOptions = base.transform.parent.Find("Options").GetComponent<WHN_OptionsController>();
		freenGameTitle = base.transform.Find("Down/FreenGameTitle");
		freenGameTitle.gameObject.SetActive(value: false);
		freenGameText = freenGameTitle.Find("Text").GetComponent<Text>();
		tipBGText = base.transform.Find("Down/tipBg/Text").GetComponent<Text>();
		freenWin = base.transform.Find("Down/WinFreen");
		freenWinText = freenWin.Find("Text").GetComponent<Text>();
		freenWin.gameObject.SetActive(value: false);
		bg = base.transform.Find("Bg0").gameObject;
		if (bg.transform.childCount > 0)
		{
			bg.transform.GetChild(0).gameObject.SetActive(value: false);
		}
		goUIContainer = base.gameObject;
		goContainer = base.gameObject;
		RolM = base.transform.Find<WHN_RollManger>("SymNRoll/mask/RollLineGroup");
		SymM = base.transform.Find<WHN_SymbolManger>("SymNRoll/Symbolgroup");
		LineG = base.transform.Find<WHN_LineGroup>("LineGroup");
		ballManager = base.transform.Find("LineBalls").GetComponent<WHN_BallManager>();
		textLineNum = base.transform.Find("Down/ImgScoreBg1/TxtYX").GetComponent<Text>();
		txtSingleStake = base.transform.Find("Down/BtnPlus/TxtDXYF").GetComponent<Text>();
		txtLineNum = base.transform.Find("Down/BtnPlusLine/TxtDXYF").GetComponent<Text>();
		txtCredit = base.transform.Find("Down/ImgScoreBg2/TxtZF/TxtZF").GetComponent<Text>();
		txtWinScore = base.transform.Find("Down/ImgScoreBg2/TxtDF/TxtDF").GetComponent<Text>();
		txtWinBg = base.transform.Find("Down/WinBg/Text").GetComponent<Text>();
		txtDice = base.transform.Find("Down/TxtDice").GetComponent<Text>();
		OnClickListener();
		SetStarAndStopState(isStar: true, isStop: false);
	}

	private void OnClickListener()
	{
		btnStart.onClick.AddListener(OnBtnStart_Click);
		btnAuto.onClick.AddListener(OnBtnAuto_Click);
		btnStop.onClick.AddListener(OnBtnStop_Click);
		btnStopAuto.onClick.AddListener(OnBtnStopAuto_Click);
		btnPlus.onClick.AddListener(delegate
		{
			OnBtnPlus_Click();
		});
		btnMinus.onClick.AddListener(delegate
		{
			OnBtnMinus_Click();
		});
		btnLinePlus.onClick.AddListener(delegate
		{
			OnBtnLinePlus_Click();
		});
		btnLineMinus.onClick.AddListener(delegate
		{
			OnBtnLineMinus_Click();
		});
		helpBtn.onClick.AddListener(mOptions.OnBtnRules_Click);
		blackBtn.onClick.AddListener(mOptions.OnBtnReturn_Click);
		rightBtn.onClick.AddListener(delegate
		{
			OnBtnRight_Click(isBg: false);
		});
		bgBtn.onClick.AddListener(delegate
		{
			OnBtnRight_Click(isBg: true);
		});
		lp_Btn_Dec.Addaction(delegate
		{
			LpBtnChane(isAdd: false);
		});
		lp_Btn_Add.Addaction(delegate
		{
			LpBtnChane(isAdd: true);
		});
		btnDice.onClick.AddListener(OnBtnDice_Click);
	}

	public void PreInit()
	{
		goUIContainer = base.gameObject;
		goContainer = base.gameObject;
		ballManager = base.transform.Find("LineBalls").GetComponent<WHN_BallManager>();
		ballManager.gameObject.SetActive(value: true);
		goContainer.SetActive(value: false);
	}

	public void Init()
	{
		rollingFinishedAction = Handle_RollingFinished;
		allFinishedAction = Handle_AllFinished;
		WHN_MB_Singleton<WHN_NetManager>.GetInstance().RegisterHandler("gameResult", HandleNetMsg_GameResult);
		WHN_MB_Singleton<WHN_NetManager>.GetInstance().RegisterHandler("maryResult", HandleNetMsg_MaryResult);
		WHN_MB_Singleton<WHN_NetManager>.GetInstance().RegisterHandler("deskInfo", HandleNetMsg_DeskInfo);
		WHN_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		language = ((WHN_GVars.language == "en") ? 1 : 0);
		MonoBehaviour.print(language);
		txtDice.text = ((language != 0) ? "Dice" : "比倍");
	}

	public void Show()
	{
		WHN_MB_Singleton<WHN_GameManager>.GetInstance().ChangeView("MajorGame");
		if (goUIContainer == null)
		{
			goUIContainer = base.gameObject;
		}
		if (goContainer == null)
		{
			goContainer = base.gameObject;
		}
		WHN_Utils.TrySetActive(goUIContainer, active: true);
		WHN_Utils.TrySetActive(goContainer, active: true);
		SetCredit(WHN_GVars.credit);
		StakeRebound();
		WHN_LockManager.UnLock("btn_options", force: true);
		WHN_MB_Singleton<WHN_OptionsController>.GetInstance().onItemBank = OnItemBankAction;
		WHN_MB_Singleton<WHN_OptionsController>.GetInstance().onItemSettings = OnSettingAction;
		WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
	}

	public void StartRollSym()
	{
		ResetAll();
		SymM.transform.localPosition = new Vector3(1000f, 1000f, 1000f);
		RolM.transform.localPosition = poslineg;
		RolM.RollAllline();
		SymM.RollAllsym();
	}

	public void AllStop()
	{
		UnityEngine.Debug.LogError("============全部停止============");
		float num = 0.6f;
		RolM.transform.localPosition = new Vector3(1000f, 1000f, 1000f);
		SymM.transform.localPosition = possymg;
		if (Cinstance<WHN_Gcore>.Instance.WinlineList.Count > 0)
		{
			num = (float)Cinstance<WHN_Gcore>.Instance.WinlineList.Count * 2f;
			SymM.Playani(Cinstance<WHN_Gcore>.Instance.WinSymlist);
			LineG.Showline(Cinstance<WHN_Gcore>.Instance.WinlineList);
			ballManager.Showline(Cinstance<WHN_Gcore>.Instance.WinlineList);
			JudgeBtnDice();
		}
		if (Cinstance<WHN_Gcore>.Instance.ScatterList.Count == 2)
		{
			SymM.PlayScatter(Cinstance<WHN_Gcore>.Instance.ScatterList);
		}
		int totalWin = lastResult.totalWin;
		UnityEngine.Debug.LogError("totalWin: " + totalWin);
		SetWinScore(totalWin);
		if (totalWin <= 0)
		{
			tipBGText.text = "再接再厉 , 祝君好运";
		}
		UnityEngine.Debug.LogError("等待时间: " + num);
		if (WaitStartCoroutine != null)
		{
			StopCoroutine(WaitStartCoroutine);
		}
		if (base.gameObject.activeInHierarchy)
		{
			WaitStartCoroutine = StartCoroutine(WaitStart(num));
		}
	}

	private IEnumerator WaitStart(float time)
	{
		yield return new WaitForSeconds(time);
		WinningLineAnimationControl();
	}

	public void StopRoll()
	{
		UnityEngine.Debug.LogError("StopRoll");
		Cinstance<WHN_Gcore>.Instance.IsRoll = false;
		RolM.StopAllLine();
	}

	public void ResetAll()
	{
		LineG.ResetLine();
		ballManager.ResetLine();
		SymM.Stopani();
	}

	private void ShowWild()
	{
	}

	private void CloseAllbat()
	{
	}

	private void ShowWild(int index, bool isshow)
	{
	}

	private void CloseAllWild()
	{
		for (int i = 0; i < 5; i++)
		{
			ShowWild(i, isshow: false);
		}
	}

	private void CloseAllchange()
	{
	}

	public void Hide()
	{
		UnityEngine.Debug.Log("MajorGameController Hide");
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
		WHN_Utils.TrySetActive(goUIContainer, active: false);
		WHN_Utils.TrySetActive(goContainer, active: false);
		WHN_SoundManager.Instance.StopMajorAudio();
	}

	public void ResetGame(bool isReconnect = false)
	{
		UnityEngine.Debug.LogError("重置游戏");
		ballManager.HideAllBall();
		OutGame(isReconnect);
		WHN_MB_Singleton<WHN_HUDController>.GetInstance().Hide();
		WHN_MB_Singleton<WHN_HUDController>.GetInstance().HideRules();
		WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().Hide();
		WHN_MB_Singleton<WHN_OptionsController>.GetInstance().Hide();
		WHN_MB_Singleton<WHN_SettingsController>.GetInstance().Hide();
		WHN_MB_Singleton<WHN_DiceGameController2>.GetInstance().Hide();
		WHN_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
	}

	public void InitGame(bool halfInit = false)
	{
		WHN_MB_Singleton<WHN_HUDController>.GetInstance().Show();
		WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().SetKeepScore(0);
		WHN_LockManager.UnLock("ScoreBank", force: true);
		WHN_LockManager.UnLock("btn_options", force: true);
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
		WHN_LockManager.UnLock("Delay_Overflow");
		bGaming = false;
		if (!halfInit)
		{
			SetLineNum(gameLineNum);
			SetSingleStake(WHN_GVars.desk.minSinglelineBet);
			SetTotalStake(gameLineNum * WHN_GVars.desk.minSinglelineBet);
			SetCredit(0, 0, check: true);
			bAuto = false;
			SetBtnAutoState("auto");
		}
		SetWinScore(0);
		JudgeBtnMinusPlus();
		JudgeBtnDice();
		canShowWinLines = false;
		bReplaying = false;
		ballManager.HideAllBall();
		lastResult = null;
		maryTimes = 0;
		UnityEngine.Debug.LogError("InitGame");
		SetBtnStartState(WHN_BtnState.start);
		UnityEngine.Debug.Log($"单线最小: {WHN_GVars.desk.minSinglelineBet}, 单线最大: {WHN_GVars.desk.maxSinglelineBet}, 单线压分距: {WHN_GVars.desk.singlechangeScore}, diceSwitch: {WHN_GVars.desk.diceSwitch}, diceDirectSwitch: {WHN_GVars.desk.diceDirectSwitch}");
		if (bAuto && winScore == 0)
		{
			StartGame();
		}
	}

	private void OutGame(bool isReconnect = false)
	{
		if (!isReconnect)
		{
			bAuto = false;
		}
		WHN_SoundManager.Instance.StopMaryBGM();
		WHN_SoundManager.Instance.PlayMajorBGMAndio(isPlay: false);
		freenGameTitle.gameObject.SetActive(value: false);
		if (bg.transform.childCount > 0)
		{
			bg.transform.GetChild(0).gameObject.SetActive(value: false);
		}
		isFreenGame = false;
		isOneFreen = true;
		freenWin.gameObject.SetActive(value: false);
		btnStart.interactable = true;
		btnStart.gameObject.SetActive(value: true);
		btnStop.interactable = true;
		btnStop.gameObject.SetActive(value: false);
		if (!isReconnect)
		{
			btnAuto.gameObject.SetActive(value: true);
		}
		else
		{
			btnAuto.gameObject.SetActive(value: false);
		}
		if (!isReconnect)
		{
			btnAuto.interactable = true;
		}
		else
		{
			btnAuto.interactable = false;
		}
		btnStopAuto.interactable = true;
		if (!isReconnect)
		{
			btnStopAuto.gameObject.SetActive(value: false);
		}
		else
		{
			btnStopAuto.gameObject.SetActive(value: true);
		}
		SetBtnPlusEnable(isEnable: true);
		OnBtnRight_Click(isBg: true);
		ResetAll();
		Cinstance<WHN_Gcore>.Instance.QuickStop();
		Cinstance<WHN_Gcore>.Instance.ClearList();
		Cinstance<WHN_Gcore>.Instance.Normallist = new List<int>();
		for (int i = 0; i < 15; i++)
		{
			Cinstance<WHN_Gcore>.Instance.Normallist.Add(UnityEngine.Random.Range(1, 10));
		}
		WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().SetGoldAndScore(0, 0);
	}

	public void ExitGame()
	{
		OutGame();
		WHN_MB_Singleton<WHN_HUDController>.GetInstance().Hide();
		WHN_SoundManager.Instance.StopNumberRollAudio();
		Send_LeaveDesk();
	}

	private void WinningLineAnimationControl()
	{
		UnityEngine.Debug.LogError("准备重置");
		Handle_AllFinished();
	}

	private void WinningLineAnimationControl(bool isReplay = false, bool isChange = false)
	{
		_task_WinLineAni = new WHN_XTask(WinLinesAni());
		WHN_XTask task_WinLineAni = _task_WinLineAni;
		task_WinLineAni.Finished = (WHN_XTask.FinishedHandler)Delegate.Combine(task_WinLineAni.Finished, (WHN_XTask.FinishedHandler)delegate(bool manual)
		{
			if (manual)
			{
				UnityEngine.Debug.LogError("准备重置");
				Handle_AllFinished();
			}
			JudgeBtnDice();
			_task_WinLineAni = null;
		});
	}

	private IEnumerator WinLinesAni()
	{
		yield return new WaitForSeconds(3.5f);
	}

	public void JudgeResetGame(int credit = 0)
	{
		UnityEngine.Debug.Log("JudgeResetGame");
		if (WHN_GVars.curView == "MajorGame")
		{
			HandleNoResponse_GameStart(credit);
			return;
		}
		if (WHN_GVars.curView == "DiceGame")
		{
			WHN_MB_Singleton<WHN_DiceGameController2>.GetInstance().HandleNoResponse_MultiInfo();
			SetCredit(credit);
		}
		UnityEngine.Debug.Log("no need reset game");
	}

	public void OnBtnRight_Click(bool isBg)
	{
		WHN_SoundManager.Instance.PlayClickAudio();
		if (!isBg)
		{
			mTw = rightBtn.transform.DOLocalMoveX(rightBtnTagPos.x, 0.25f);
			mTw.OnComplete(delegate
			{
				bgBtn.gameObject.SetActive(value: true);
				RightPanel_Panel.DOLocalMoveX(rightPanel_PanelOldPos.x, 0.25f);
			});
		}
		else
		{
			mTw = RightPanel_Panel.DOLocalMoveX(rightPanel_PanelTagPos.x, 0.25f);
			mTw.OnComplete(delegate
			{
				bgBtn.gameObject.SetActive(value: false);
				rightBtn.transform.DOLocalMoveX(rightBtnOldPos.x, 0.25f);
			});
		}
	}

	private void LpBtnChane(bool isAdd)
	{
		WHN_SoundManager.Instance.PlayClickAudio();
		UnityEngine.Debug.LogError("LpBtn isAdd: " + isAdd);
		if (isAdd)
		{
			imaIndex++;
			if (imaIndex > maxLpIco)
			{
				imaIndex = 0;
			}
		}
		else
		{
			imaIndex--;
			if (imaIndex < 0)
			{
				imaIndex = maxLpIco;
			}
		}
		leftPanelIco.sprite = WHN_MB_Singleton<WHN_GameManager>.GetInstance().Getanisprite($"Gold{imaIndex}");
		smallText.text = (8 + imaIndex * 4 + ((imaIndex == maxLpIco) ? 1 : 0)).ToString();
	}

	public void OnBtnStart_Click()
	{
		if (!WHN_GVars.tryLockOnePoint)
		{
			WHN_SoundManager.Instance.PlayClickAudio();
			if (bGainScoreBtn || winScore > 0)
			{
				GainScore();
			}
			btnStart.interactable = false;
			CloseAllWild();
			if (!bAuto)
			{
				StartGame();
			}
		}
	}

	public void OnBtnStop_Click()
	{
		UnityEngine.Debug.Log("OnBtnStop_Click");
		if (!WHN_GVars.tryLockOnePoint)
		{
			WHN_SoundManager.Instance.StopMaryAudio();
			if (_task_NormalReduceTextAni != null)
			{
				_task_NormalReduceTextAni.Stop();
				WHN_SoundManager.Instance.NumberRollEndAudio();
			}
			if (_task_WinLineAni != null)
			{
				_task_WinLineAni.Stop();
				WHN_SoundManager.Instance.StopMajorAudio();
			}
			StopRoll();
			btnStop.interactable = false;
			isReStart = false;
			SetBtnStartState(WHN_BtnState.reStart);
		}
	}

	private void GainScore()
	{
		WHN_LockManager.Lock("ScoreBank");
		WHN_LockManager.Lock("btnDice");
		int finalCredit = WHN_GVars.credit + winScore;
		bGainScoreBtn = false;
		_task_NormalReduceTextAni = new WHN_XTask(WHN_Utils.NormalReduceTextAni(0f, 0.05f, winScore, delegate(int total, int diff)
		{
			SetWinScore(winScore - diff);
			SetCredit(WHN_GVars.credit + diff);
		}, null));
		WHN_XTask task_NormalReduceTextAni = _task_NormalReduceTextAni;
		task_NormalReduceTextAni.Finished = (WHN_XTask.FinishedHandler)Delegate.Combine(task_NormalReduceTextAni.Finished, (WHN_XTask.FinishedHandler)delegate(bool manual)
		{
			if (manual)
			{
				SetWinScore(0);
				SetCredit(finalCredit, -1, check: true);
			}
			SetSomeBtnsEnable(isEnable: true);
			if (totalStake < WHN_GVars.credit)
			{
				StakeRebound();
			}
			JudgeBtnMinusPlus();
			WHN_LockManager.UnLock("ScoreBank", force: true);
			WHN_LockManager.UnLock("btnDice", force: true);
			_task_NormalReduceTextAni = null;
			canShowWinLines = false;
			StopFreenGame();
			if (WHN_MB_Singleton<WHN_NetManager>.GetInstance().isReady)
			{
				if (bAuto)
				{
					StartGame();
				}
			}
			else if (bAuto)
			{
				UnityEngine.Debug.Break();
				StartCoroutine(WHN_Utils.WaitCall(() => WHN_MB_Singleton<WHN_NetManager>.GetInstance().isReady, delegate
				{
					StartGame();
				}));
			}
		});
	}

	private void StopFreenGame()
	{
		if (isFreenStop)
		{
			if (!isOldAuto)
			{
				bAuto = false;
				btnStart.interactable = true;
			}
			if (bg.transform.childCount > 0)
			{
				bg.transform.GetChild(0).gameObject.SetActive(value: false);
			}
			WHN_SoundManager.Instance.StopMaryBGM();
			freenGameTitle.gameObject.SetActive(value: false);
			isFreenGame = false;
			isOneFreen = true;
			freenWinText.text = lastResult.freenWin.ToString();
			freenWin.gameObject.SetActive(value: true);
			StartCoroutine(WaitStopFreenGame());
		}
	}

	private IEnumerator WaitStopFreenGame()
	{
		yield return new WaitForSeconds(1.5f);
		freenWin.gameObject.SetActive(value: false);
	}

	private void StartGame()
	{
		UnityEngine.Debug.Log("开始游戏");
		int rightGold = WHN_ScoreBank.GetRightGold();
		if (WHN_GVars.credit < totalStake && freenGameTitle.gameObject != null && !freenGameTitle.gameObject.activeInHierarchy)
		{
			tipBGText.text = "分数不足,请取分";
			if (WHN_GVars.credit == 0 || (WHN_GVars.credit < WHN_GVars.desk.minSinglelineBet * gameLineNum && rightGold != 0))
			{
				WHN_SoundManager.Instance.PlayClickAudio();
				WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().Show();
				WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().InitBank();
				SetAndIfChangeThenPlayAutoAni(toAuto: false);
				return;
			}
			if (rightGold == 0 && WHN_GVars.credit < WHN_GVars.desk.minSinglelineBet * gameLineNum)
			{
			}
		}
		tipBGText.text = string.Empty;
		LowCreditMaxStake();
		SetBtnStartState(WHN_BtnState.disable);
		if (freenGameTitle.gameObject != null && freenGameTitle.gameObject.activeInHierarchy)
		{
			UnityEngine.Debug.LogError("免费,不扣钱");
			SetCredit(WHN_GVars.credit, WHN_GVars.credit);
		}
		else
		{
			UnityEngine.Debug.Log("扣:" + (WHN_GVars.credit - totalStake));
			SetCredit(WHN_GVars.credit - totalStake, WHN_GVars.credit - totalStake);
		}
		lastResult = null;
		Send_GameStart();
		listAni.Clear();
		WHN_SoundManager.Instance.PlayMajorRollAudio();
		SetSomeBtnsEnable(isEnable: false);
		WHN_LockManager.Lock("ScoreBank");
		canShowWinLines = false;
		bGaming = true;
		SetBtnPlusEnable(isEnable: false);
		WHN_LockManager.Lock("Delay_Overflow");
	}

	public void OnBtnMinus_Click()
	{
		if (!WHN_GVars.tryLockOnePoint)
		{
			WHN_SoundManager.Instance.PlayClickAudio();
			WHN_Desk desk = WHN_GVars.desk;
			if (totalStake >= desk.minSinglelineBet * gameLineNum)
			{
				int num = (singleStake == desk.minSinglelineBet) ? desk.maxSinglelineBet : (singleStake - desk.singlechangeScore);
				num = ((num < desk.minSinglelineBet) ? desk.minSinglelineBet : num);
				SetLineNum(gameLineNum);
				SetSingleStake(num);
				SetTotalStake(num * gameLineNum);
				UnityEngine.Debug.Log("newSingleStake: " + num);
			}
			else
			{
				UnityEngine.Debug.LogError($"BtnMinus should not be clicked. _totalStake: {totalStake}, minSinglelineBet: {desk.minSinglelineBet}*gameLineNum");
			}
		}
	}

	public void OnBtnPlus_Click()
	{
		if (!WHN_GVars.tryLockOnePoint)
		{
			WHN_SoundManager.Instance.PlayClickAudio();
			WHN_Desk desk = WHN_GVars.desk;
			if (totalStake >= desk.minSinglelineBet * gameLineNum)
			{
				int num = (singleStake == desk.maxSinglelineBet) ? desk.minSinglelineBet : (singleStake + desk.singlechangeScore);
				num = ((num > desk.maxSinglelineBet) ? desk.maxSinglelineBet : num);
				SetLineNum(gameLineNum);
				SetSingleStake(num);
				SetTotalStake(num * gameLineNum);
				UnityEngine.Debug.Log("单线压分: " + num);
			}
			else
			{
				UnityEngine.Debug.LogError($"BtnPlus should not be clicked. _totalStake: {totalStake}, minSinglelineBet: {desk.minSinglelineBet}*gameLineNum");
			}
		}
	}

	public void OnBtnLinePlus_Click()
	{
		WHN_SoundManager.Instance.PlayClickAudio();
		gameLineNum++;
		if (gameLineNum > Cinstance<WHN_Gcore>.Instance.lineConut)
		{
			gameLineNum = 1;
		}
		SetLineNum(gameLineNum);
		SetTotalStake(gameLineNum * singleStake);
		txtLineNum.text = gameLineNum.ToString();
	}

	public void OnBtnLineMinus_Click()
	{
		WHN_SoundManager.Instance.PlayClickAudio();
		gameLineNum--;
		if (gameLineNum <= 0)
		{
			gameLineNum = Cinstance<WHN_Gcore>.Instance.lineConut;
		}
		SetLineNum(gameLineNum);
		SetTotalStake(gameLineNum * singleStake);
		txtLineNum.text = gameLineNum.ToString();
	}

	public void OnBtnMaxStake_Click()
	{
		UnityEngine.Debug.Log("OnBtnMaxStake_Click");
		if (WHN_GVars.tryLockOnePoint)
		{
			return;
		}
		WHN_SoundManager.Instance.PlayClickAudio();
		WHN_Desk desk = WHN_GVars.desk;
		int num = lineNum;
		int num2 = singleStake;
		if (WHN_GVars.credit >= desk.maxSinglelineBet * gameLineNum)
		{
			num = gameLineNum;
			num2 = desk.maxSinglelineBet;
		}
		else if (WHN_GVars.credit >= desk.minSinglelineBet * gameLineNum)
		{
			num = gameLineNum;
			int origin = WHN_GVars.credit / gameLineNum;
			num2 = CalGearValue(desk.minSinglelineBet, desk.maxSinglelineBet, desk.singlechangeScore, origin);
		}
		else if (WHN_GVars.credit >= gameLineNum)
		{
			num = gameLineNum;
			num2 = WHN_GVars.credit / gameLineNum;
		}
		else
		{
			if (WHN_GVars.credit >= gameLineNum || WHN_GVars.credit <= 0)
			{
				return;
			}
			num = WHN_GVars.credit;
			num2 = 1;
		}
		SetLineNum(num);
		SetSingleStake(num2);
		SetTotalStake(num2 * num);
		JudgeBtnMinusPlus();
	}

	private void LowCreditMaxStake()
	{
		if (WHN_GVars.credit > singleStake * lineNum)
		{
			return;
		}
		WHN_Desk desk = WHN_GVars.desk;
		int num = lineNum;
		int num2 = singleStake;
		if (WHN_GVars.credit >= desk.minSinglelineBet * gameLineNum)
		{
			num = gameLineNum;
			int origin = WHN_GVars.credit / gameLineNum;
			num2 = CalGearValue(desk.minSinglelineBet, desk.maxSinglelineBet, desk.singlechangeScore, origin);
		}
		else if (WHN_GVars.credit >= gameLineNum)
		{
			num = gameLineNum;
			num2 = WHN_GVars.credit / gameLineNum;
		}
		else
		{
			if (WHN_GVars.credit >= gameLineNum || WHN_GVars.credit <= 0)
			{
				return;
			}
			num = WHN_GVars.credit;
			num2 = 1;
		}
		SetLineNum(num);
		SetSingleStake(num2);
		SetTotalStake(num2 * num);
	}

	public void OnBtnTestNet_Click()
	{
		UnityEngine.Debug.Log("OnBtnTestNet");
		if (!WHN_GVars.tryLockOnePoint)
		{
			WHN_MB_Singleton<WHN_LobbyViewController>.GetInstance().Send_EnterDesk(295);
		}
	}

	public void OnBtnDice_Click()
	{
		WHN_MB_Singleton<WHN_HUDController>.GetInstance().Hide();
		Cinstance<WHN_Gcore>.Instance.IsDic = true;
		UnityEngine.Debug.LogError("按下了比倍");
		if (WHN_GVars.tryLockOnePoint)
		{
			return;
		}
		WHN_SoundManager.Instance.StopMaryAudio();
		WHN_SoundManager.Instance.PlayClickAudio();
		WHN_SoundManager.Instance.StopMajorGameAwardAudio();
		int diceBet = 0;
		if (!WHN_LockManager.IsLocked("btnDice") && !bReplaying)
		{
			if (WHN_GVars.credit == 0 && winScore == 0)
			{
				WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().Show();
				WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().InitBank();
				SetAndIfChangeThenPlayAutoAni(toAuto: false);
				return;
			}
			SetBtnStartState(WHN_BtnState.start);
			diceBet = winScore;
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			canShowWinLines = false;
			WHN_MB_Singleton<WHN_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "OnBtnDice");
			WHN_MB_Singleton<WHN_GameRoot>.GetInstance().StartCoroutine(WHN_Utils.DelayCall(0.1f, delegate
			{
				WHN_MB_Singleton<WHN_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "OnBtnDice done");
				canShowWinLines = false;
				Hide();
				WHN_MB_Singleton<WHN_DiceGameController2>.GetInstance().Show();
				WHN_MB_Singleton<WHN_DiceGameController2>.GetInstance().Show();
				WHN_MB_Singleton<WHN_DiceGameController2>.GetInstance().InitGame(WHN_GVars.credit, diceBet);
				WHN_SoundManager.Instance.PlayDiceBGM();
				SetWinScore(0);
			}));
		}
	}

	public void OnBtnAuto_Click()
	{
		UnityEngine.Debug.Log("OnBtnAuto_Click");
		if (WHN_GVars.tryLockOnePoint)
		{
			return;
		}
		UnityEngine.Debug.Log("OnBtnAuto_Click1");
		WHN_SoundManager.Instance.PlayClickAudio();
		if (WHN_GVars.credit == 0 && winScore == 0)
		{
			WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().Show();
			WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().InitBank();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			return;
		}
		UnityEngine.Debug.Log("OnBtnAuto_Click2");
		SetAndIfChangeThenPlayAutoAni(toAuto: true);
		if (!bGaming)
		{
			UnityEngine.Debug.Log("OnBtnAuto_Click3");
			if (bAuto && winScore != 0 && _task_NormalReduceTextAni == null)
			{
				GainScore();
			}
			UnityEngine.Debug.Log("OnBtnAuto_Click4");
			if (bAuto && winScore == 0)
			{
				StartGame();
			}
		}
	}

	public void Auto_FreenGame()
	{
		WHN_SoundManager.Instance.PlayMaryBGM();
		isFreenGame = true;
		freenGameTitle.gameObject.SetActive(value: true);
		if (bg.transform.childCount > 0)
		{
			bg.transform.GetChild(0).gameObject.SetActive(value: true);
		}
		isOldAuto = bAuto;
		btnAuto.interactable = false;
		btnStart.interactable = false;
		UnityEngine.Debug.LogError("Auto_FreenGame");
		bAuto = true;
		if (winScore != 0 && _task_NormalReduceTextAni == null)
		{
			GainScore();
		}
		if (winScore == 0)
		{
			StartGame();
		}
	}

	public void OnBtnStopAuto_Click()
	{
		UnityEngine.Debug.Log("OnBtnStopAuto_Click");
		if (!WHN_GVars.tryLockOnePoint)
		{
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
			}
			WHN_SoundManager.Instance.StopMaryAudio();
			if (_task_WinLineAni != null)
			{
				_task_WinLineAni.Stop();
				WHN_SoundManager.Instance.StopMajorAudio();
			}
			StopRoll();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			btnStop.interactable = false;
			btnStart.interactable = false;
			btnAuto.interactable = false;
		}
	}

	public void OnBtnShowLines_Click()
	{
		UnityEngine.Debug.Log("OnBtnShowLines_Click");
		if (!WHN_GVars.tryLockOnePoint)
		{
			WHN_SoundManager.Instance.PlayClickAudio();
			if (canShowWinLines)
			{
				bReplaying = true;
				WinningLineAnimationControl(bReplaying);
				isReStart = false;
				SetBtnStartState(WHN_BtnState.stop);
				SetBtnDiceEnable(isEnable: false);
			}
		}
	}

	public void OnBtnShowLines_PointDown()
	{
		if (WHN_GVars.tryLockOnePoint)
		{
			UnityEngine.Debug.Log("OnBtnShowLines_PointDown");
		}
	}

	public void OnBtnShowLines_PointUp()
	{
	}

	public void Send_GameStart()
	{
		object[] args = new object[4]
		{
			WHN_GVars.desk.id,
			lineNum,
			singleStake,
			totalStake
		};
		WHN_SoundManager.Instance.StopMajorGameAwardAudio();
		WHN_MB_Singleton<WHN_NetManager>.GetInstance().Send("userService/gameStart", args);
		WHN_StateManager.GetInstance().RegisterWork("Send_GameStart");
		WHN_StateManager.GetInstance().RegisterWork("m_IsFree");
		gameTimes++;
		int num = gameTimes;
	}

	public void Send_LeaveDesk()
	{
		object[] args = new object[1]
		{
			WHN_GVars.desk.id
		};
		WHN_MB_Singleton<WHN_NetManager>.GetInstance().Send("userService/leaveDesk", args);
	}

	public void HandleNetMsg_GameResult(object[] args)
	{
		WHN_StateManager.GetInstance().CompleteWork("Send_GameStart");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		try
		{
			UnityEngine.Debug.LogError("开奖信息: " + JsonMapper.ToJson(dictionary));
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		lastResult = new WHN_MajorResult().Init(lineNum, singleStake, totalStake, dictionary);
		UnityEngine.Debug.LogError("开奖图标: " + JsonMapper.ToJson(Cinstance<WHN_Gcore>.Instance.Result));
		if (Cinstance<WHN_Gcore>.Instance.SendBet())
		{
			UnityEngine.Debug.LogError("开始转真轴");
		}
		isFreenStop = lastResult.isFreenGameStop;
		if (!lastResult.maryGame)
		{
			WHN_StateManager.GetInstance().CompleteWork("m_IsFree");
		}
		SetCredit(-1, lastResult.totalWin + WHN_GVars.realCredit);
		isReStart = false;
		SetBtnStartState(WHN_BtnState.stop);
	}

	public void HandleNoResponse_GameStart(int credit)
	{
		if (!WHN_StateManager.GetInstance().IsWorkCompleted("Send_GameStart"))
		{
			UnityEngine.Debug.Log("Send_GameStart  was not completed");
			WHN_StateManager.GetInstance().CompleteWork("Send_GameStart");
			WHN_StateManager.GetInstance().CompleteWork("m_IsFree");
			coGridRolling = null;
			ResetGame(isReconnect: true);
			InitGame(halfInit: true);
			if (bAuto)
			{
				bGaming = false;
			}
			if (!bAuto)
			{
				UnityEngine.Debug.LogError("网络存在波动，请继续游戏");
				WHN_MB_Singleton<WHN_AlertDialog>.GetInstance().ShowDialog((WHN_GVars.language == "zh") ? "网络存在波动,重连成功!,请继续游戏" : "Network error，please go on");
			}
			SetCredit(credit);
		}
		else
		{
			UnityEngine.Debug.Log("HandleNoResponse_GameStart ok");
		}
	}

	public void HandleNetMsg_MaryResult(object[] args)
	{
		UnityEngine.Debug.Log(WHN_LogHelper.NetHandle("HandleNetMsg_MaryResult"));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		WHN_StateManager.GetInstance().CompleteWork("MaryGame");
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
		StartCoroutine(WHN_Utils.WaitCall(() => lastResult != null, delegate
		{
			UnityEngine.Debug.Log($"wait: {Time.time - beginTime}s");
			WHN_MB_Singleton<WHN_MaryGameController>.GetInstance().PrepareGame(lastResult.maryCount, WHN_GVars.credit, totalBet, photoNumbers, photosArray, totalWins);
			SetCredit(-1, credit);
		}));
	}

	public void HandleNetMsg_DeskInfo(object[] args)
	{
		UnityEngine.Debug.Log(WHN_LogHelper.NetHandle("HandleNetMsg_DeskInfo"));
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
		UnityEngine.Debug.LogError("进入处理");
		lastLineNum = lineNum;
		lastSingleStake = singleStake;
		WinningLineAnimationControl(isReplay: false, isChange: false);
	}

	public bool JungeAuto()
	{
		return bAuto;
	}

	private void Handle_AllFinished()
	{
		ballManager.HideAllBall();
		bGaming = false;
		WHN_LockManager.UnLock("Delay_Overflow");
		SetSomeBtnsEnable(isEnable: true);
		JudgeBtnMinusPlus();
		canShowWinLines = false;
		WHN_LockManager.UnLock("ScoreBank", force: true);
		WHN_LockManager.UnLock("btn_options", force: true);
		if (WHN_GVars.credit == 0)
		{
			UnityEngine.Debug.Log("Lose: credit is zero");
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
		}
		UnityEngine.Debug.LogError("游戏准备就绪");
		SetBtnPlusEnable(isEnable: true);
		isReStart = true;
		SetBtnStartState(WHN_BtnState.start);
		if (bReplaying)
		{
			bReplaying = false;
			return;
		}
		if (lastResult.maryCount > 0 && lastResult.isFreenStart && isOneFreen)
		{
			lastResult.isFreenStart = false;
			isOneFreen = false;
			WHN_MB_Singleton<WHN_MaryMovieController>.GetInstance().Play();
			return;
		}
		JudgeBtnDice();
		if (WHN_LockManager.IsLocked("Overflow"))
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
		WHN_MB_Singleton<WHN_MaryGameController>.GetInstance().Hide();
		Show();
		WHN_MB_Singleton<WHN_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "enter mary");
		WHN_MB_Singleton<WHN_HUDController>.GetInstance().Show();
		SetWinScore(maryWin);
		JudgeBtnDice();
		bGaming = false;
		if (WHN_LockManager.IsLocked("Overflow"))
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
		string content = (WHN_GVars.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge";
		WHN_LockManager.Lock("Esc");
		WHN_LockManager.Lock("Quit");
		WHN_MB_Singleton<WHN_GameManager>.Get().PrepareQuitGame();
		WHN_MB_Singleton<WHN_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			WHN_MB_Singleton<WHN_GameManager>.GetInstance().QuitToHallGame();
		});
	}

	public void OnDiceExit()
	{
		SetWinScore(0);
		JudgeBtnDice();
		JudgeBtnMinusPlus();
		StakeRebound();
	}

	private void OnItemBankAction()
	{
		WHN_MB_Singleton<WHN_OptionsController>.GetInstance().Hide();
		WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().Show();
		WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().InitBank();
	}

	private void OnSettingAction()
	{
		WHN_MB_Singleton<WHN_OptionsController>.GetInstance().Hide();
		WHN_MB_Singleton<WHN_SettingsController>.GetInstance().Show();
	}

	private void OnSetScoreAndGoldAction()
	{
		if (goUIContainer.activeInHierarchy)
		{
			txtCredit.text = WHN_GVars.credit.ToString();
			if (WHN_GVars.credit == 0 && winScore == 0)
			{
				SetAndIfChangeThenPlayAutoAni(toAuto: false);
				return;
			}
			SetSomeBtnsEnable(isEnable: true);
			StakeRebound();
			JudgeBtnMinusPlus();
		}
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
	}

	public void SetCredit(int credit, int realCredit = -1, bool check = false)
	{
		if (credit >= 0)
		{
			WHN_GVars.credit = credit;
			txtCredit.text = credit.ToString();
		}
		if (realCredit >= 0)
		{
			WHN_GVars.realCredit = realCredit;
		}
		if (check && WHN_GVars.credit != WHN_GVars.realCredit)
		{
			UnityEngine.Debug.LogError($"credit: {WHN_GVars.credit}, realCredit: {WHN_GVars.realCredit}");
		}
	}

	private void SetWinScore(int winScore)
	{
		this.winScore = winScore;
		WHN_GVars.winScorce = winScore;
		txtWinScore.text = this.winScore.ToString();
		txtWinBg.text = this.winScore.ToString();
	}

	private void SetBtnDiceEnable(bool isEnable)
	{
		btnDice.gameObject.SetActive(isEnable);
	}

	private void SetBtnAutoEnable(bool isEnable)
	{
		btnAuto.interactable = isEnable;
	}

	private void SetBtnPlusEnable(bool isEnable)
	{
		btnPlus.interactable = isEnable;
		btnMinus.interactable = isEnable;
		btnLinePlus.interactable = isEnable;
		btnLineMinus.interactable = isEnable;
	}

	private void SetSomeBtnsEnable(bool isEnable)
	{
		if (!isEnable)
		{
		}
		if (isEnable)
		{
			JudgeBtnDice();
		}
		else
		{
			SetBtnDiceEnable(isEnable: false);
		}
	}

	public void SetBtnStartState(WHN_BtnState cSF_BtnState = WHN_BtnState.none)
	{
		bGainScoreBtn = false;
		if (_coroutine != null)
		{
			StopCoroutine(_coroutine);
		}
		_coroutine = StartCoroutine(WaitSetBtnState(cSF_BtnState));
	}

	private void Update()
	{
		if (bAuto && btnStopAuto.gameObject != null && !btnStopAuto.interactable && lastResult != null && !lastResult.maryGame && !freenGameTitle.gameObject.activeInHierarchy)
		{
			UnityEngine.Debug.LogError("强制");
			if (btnAuto.gameObject != null)
			{
				btnAuto.gameObject.SetActive(value: false);
			}
			if (btnAuto.gameObject != null)
			{
				btnAuto.interactable = false;
			}
			btnStopAuto.gameObject.SetActive(value: true);
			btnStopAuto.interactable = true;
		}
		if (!bAuto && btnAuto.gameObject != null && !btnAuto.interactable && lastResult != null && !lastResult.maryGame)
		{
			UnityEngine.Debug.LogError("强制");
			if (btnAuto.gameObject != null)
			{
				btnAuto.gameObject.SetActive(value: true);
			}
			if (btnAuto.gameObject != null)
			{
				btnAuto.interactable = true;
			}
			btnStopAuto.gameObject.SetActive(value: false);
			btnStopAuto.interactable = false;
		}
		if (btnStopAuto.gameObject != null && lastResult != null && lastResult.maryGame)
		{
			btnStopAuto.gameObject.SetActive(value: true);
			btnStopAuto.interactable = false;
		}
		if (freenGameText != null && lastResult != null && freenGameText.gameObject.activeInHierarchy)
		{
			freenGameText.text = lastResult.maryCount.ToString();
		}
		if (txtCredit != null && txtCredit.text == "0")
		{
			if (btnStopAuto != null && btnStopAuto.gameObject.activeInHierarchy)
			{
				btnStart.interactable = false;
			}
			else
			{
				btnStart.interactable = true;
			}
			btnStart.gameObject.SetActive(value: true);
			btnStop.interactable = false;
			btnStop.gameObject.SetActive(value: false);
		}
	}

	private IEnumerator WaitSetBtnState(WHN_BtnState cSF_BtnState = WHN_BtnState.none)
	{
		yield return new WaitForSeconds(0.1f);
		if (isFreenGame)
		{
			btnStart.interactable = false;
			btnStop.interactable = false;
			btnAuto.interactable = false;
			btnStopAuto.interactable = false;
			yield break;
		}
		switch (cSF_BtnState)
		{
		case WHN_BtnState.start:
			if (btnStopAuto != null && btnStopAuto.gameObject.activeInHierarchy)
			{
				btnStart.interactable = false;
			}
			else
			{
				btnStart.interactable = true;
			}
			btnStart.gameObject.SetActive(value: true);
			btnStop.interactable = false;
			btnStop.gameObject.SetActive(value: false);
			if (!bAuto)
			{
				btnAuto.gameObject.SetActive(value: true);
				btnAuto.interactable = true;
				btnStopAuto.gameObject.SetActive(value: false);
			}
			else
			{
				btnAuto.gameObject.SetActive(value: false);
				btnAuto.interactable = false;
				btnStopAuto.gameObject.SetActive(value: true);
				btnStopAuto.interactable = true;
			}
			break;
		case WHN_BtnState.stop:
			btnStart.interactable = false;
			btnStart.gameObject.SetActive(value: true);
			yield return new WaitForSeconds(0.35f);
			if (!isReStart)
			{
				btnStart.interactable = false;
				btnStart.gameObject.SetActive(value: false);
				btnStop.interactable = true;
				btnStop.gameObject.SetActive(value: true);
			}
			else
			{
				btnStart.interactable = true;
				btnStart.gameObject.SetActive(value: true);
				btnStop.interactable = false;
				btnStop.gameObject.SetActive(value: false);
			}
			yield return new WaitForSeconds(2.5f);
			if (!isReStart)
			{
				btnStart.interactable = false;
				btnStart.gameObject.SetActive(value: true);
				btnStop.interactable = false;
				btnStop.gameObject.SetActive(value: false);
			}
			else
			{
				UnityEngine.Debug.LogWarning("游戏重新开始");
			}
			break;
		case WHN_BtnState.disable:
			btnStart.interactable = false;
			btnStart.gameObject.SetActive(value: true);
			btnStop.interactable = false;
			btnStop.gameObject.SetActive(value: false);
			btnStopAuto.interactable = false;
			btnStopAuto.gameObject.SetActive(value: false);
			btnAuto.interactable = false;
			btnAuto.gameObject.SetActive(value: true);
			break;
		case WHN_BtnState.reStart:
			btnStop.interactable = false;
			btnStop.gameObject.SetActive(value: true);
			yield return new WaitForSeconds(0.35f);
			btnStart.interactable = false;
			btnStart.gameObject.SetActive(value: true);
			btnStop.interactable = false;
			btnStop.gameObject.SetActive(value: false);
			break;
		default:
			btnStart.interactable = true;
			btnStart.gameObject.SetActive(value: true);
			btnStop.interactable = false;
			btnStop.gameObject.SetActive(value: false);
			break;
		}
	}

	private void SetStarAndStopState(bool isStar, bool isStop)
	{
		btnStart.interactable = isStar;
		btnStop.interactable = isStop;
		btnStart.gameObject.SetActive(isStar);
		btnStop.gameObject.SetActive(isStop);
	}

	private void SetBtnAutoState(string state)
	{
		if (state == "auto")
		{
			btnAuto.gameObject.SetActive(value: true);
			btnStopAuto.gameObject.SetActive(value: false);
		}
		else if (state == "cancel")
		{
			btnAuto.gameObject.SetActive(value: false);
			btnStopAuto.gameObject.SetActive(value: true);
		}
	}

	private void AdjustBetNumber()
	{
		WHN_Desk desk = WHN_GVars.desk;
		UnityEngine.Debug.Log($"ZH2_GVars.credit: {WHN_GVars.credit}, _totalStake: {totalStake},_singleStake: {singleStake},minSinglelineBet: {desk.minSinglelineBet}");
		if (WHN_GVars.credit == 0)
		{
			WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().Show();
			WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().InitBank();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
		}
		else if (WHN_GVars.credit < gameLineNum)
		{
			SetLineNum(WHN_GVars.credit);
			SetSingleStake(1);
			SetTotalStake(WHN_GVars.credit);
		}
		else if (WHN_GVars.credit < desk.minSinglelineBet * gameLineNum)
		{
			int num = WHN_GVars.credit / gameLineNum;
			SetLineNum(gameLineNum);
			SetSingleStake(num);
			SetTotalStake(num * gameLineNum);
		}
		else if (singleStake < desk.minSinglelineBet)
		{
			int minSinglelineBet = desk.minSinglelineBet;
			SetLineNum(gameLineNum);
			SetSingleStake(minSinglelineBet);
			SetTotalStake(minSinglelineBet * gameLineNum);
		}
		else if (WHN_GVars.credit < lineNum * singleStake)
		{
			int num2 = WHN_GVars.credit / gameLineNum;
			SetLineNum(gameLineNum);
			SetSingleStake(num2);
			SetTotalStake(num2 * gameLineNum);
		}
		else if (lineNum < gameLineNum && WHN_GVars.credit >= gameLineNum)
		{
			SetLineNum(gameLineNum);
			SetSingleStake(singleStake);
			SetTotalStake(singleStake * gameLineNum);
		}
		UnityEngine.Debug.Log($"_lineNum: {lineNum}, _totalStake: {totalStake},_singleStake: {singleStake},minSinglelineBet: {desk.minSinglelineBet}");
	}

	private void JudgeBtnMinusPlus()
	{
		bool flag = singleStake * lineNum >= WHN_GVars.desk.minSinglelineBet * lineNum;
	}

	private void HandleCreditChange()
	{
		int num = int.Parse(txtCredit.text);
		if (num >= WHN_GVars.credit && num > WHN_GVars.credit)
		{
			int rightGold = WHN_ScoreBank.GetRightGold();
			if (WHN_GVars.credit < totalStake && rightGold > 0)
			{
				WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().Show();
			}
		}
	}

	private void StakeRebound()
	{
		WHN_Desk desk = WHN_GVars.desk;
		if (totalStake < desk.minSinglelineBet * gameLineNum)
		{
			if (WHN_GVars.credit >= desk.minSinglelineBet * gameLineNum)
			{
				int minSinglelineBet = desk.minSinglelineBet;
				SetLineNum(gameLineNum);
				SetSingleStake(minSinglelineBet);
				SetTotalStake(minSinglelineBet * gameLineNum);
			}
			else if (WHN_GVars.credit >= gameLineNum)
			{
				int num = WHN_GVars.credit / gameLineNum;
				SetLineNum(gameLineNum);
				SetSingleStake(num);
				SetTotalStake(num * gameLineNum);
			}
			else if (WHN_GVars.credit > 0)
			{
				SetLineNum(WHN_GVars.credit);
				SetSingleStake(1);
				SetTotalStake(WHN_GVars.credit);
			}
		}
	}

	private void JudgeBtnDice()
	{
		SetBtnDiceEnable(winScore != 0 && !lastResult.maryGame);
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
		bAuto = toAuto;
		if (bAuto)
		{
			SetBtnAutoState("cancel");
		}
		else
		{
			SetBtnAutoState("auto");
		}
	}
}
