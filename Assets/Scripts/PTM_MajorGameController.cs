using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTM_MajorGameController : PTM_MB_Singleton<PTM_MajorGameController>
{
	private GameObject goUIContainer;

	private GameObject goContainer;

	private PTM_OptionsController mOptions;

	private PTM_CellManager cellManager;

	private PTM_LinesManager lineManager;

	private PTM_BallManager ballManager;

	private Text textLineNum;

	private Text txtSingleStake;

	private Text freenGameText;

	private Text txtCredit;

	private Text txtWinScore;

	private Text txtDice;

	private int gameLineNum = 15;

	private Button btnStart;

	private Button btnDice;

	private Button btnAuto;

	private Button btnPlus;

	private Button btnHelp;

	private Button btnBlack;

	private Button btnStop;

	private Button btnStopAuto;

	private Transform freenGameTitle;

	private GameObject goWinScoreTip;

	private Transform freenWin;

	private Text freenWinText;

	private Text txtWinScoreTip;

	[SerializeField]
	private GameObject goBigWinTip;

	[SerializeField]
	private Text txtBigWinTip;

	public Action onBtnDice;

	private bool bGainScoreBtn;

	private bool bAuto;

	private bool isFreenStop;

	private int winScore;

	private int singleStake;

	private int totalStake;

	private int lineNum;

	private int maryTimes;

	private bool isOneFreen = true;

	private PTM_MajorResult lastResult;

	private List<Coroutine> listAni = new List<Coroutine>();

	private PTM_XTask _task_WinLineAni;

	private PTM_XTask _task_NormalReduceTextAni;

	private bool bGaming;

	private Coroutine coGridRolling;

	private bool canShowWinLines;

	private bool bReplaying;

	private int gameTimes;

	private int lastLineNum;

	private int lastSingleStake;

	private int language;

	private Transform WildGroup;

	private Transform Wildanigroup;

	private Transform GoldBatgroup;

	private List<Animator> Wildanilist;

	private List<Animator> GoldBatlist;

	private List<GameObject> WildLinelist;

	private List<int> Lineshowwild;

	private bool isFreenGame;

	private bool isOldAuto;

	private Coroutine StopAuto_Click;

	private Coroutine _coroutine;

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
		if (PTM_MB_Singleton<PTM_MajorGameController>._instance == null)
		{
			PTM_MB_Singleton<PTM_MajorGameController>.SetInstance(this);
			PreInit();
		}
	}

	private void OnEnable()
	{
		PTM_SoundManager.Instance.PlayMajorBGMAndio(isPlay: true);
	}

	private void OnDisable()
	{
		PTM_SoundManager.Instance.PlayMajorBGMAndio(isPlay: false);
	}

	private void FinGame()
	{
		WildGroup = base.transform.Find("WildGroup");
		Wildanigroup = base.transform.Find("WildaniGroup");
		GoldBatgroup = base.transform.Find("GoldBatGroup");
		WildLinelist = new List<GameObject>();
		Wildanilist = new List<Animator>();
		GoldBatlist = new List<Animator>();
		IEnumerator enumerator = WildGroup.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				WildLinelist.Add(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		IEnumerator enumerator2 = Wildanigroup.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				Transform transform2 = (Transform)enumerator2.Current;
				Wildanilist.Add(transform2.GetComponent<Animator>());
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		IEnumerator enumerator3 = GoldBatgroup.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				Transform transform3 = (Transform)enumerator3.Current;
				GoldBatlist.Add(transform3.GetComponent<Animator>());
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator3 as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		goWinScoreTip = base.transform.Find("Hint/ImgWinHit").gameObject;
		txtWinScoreTip = base.transform.Find("Hint/ImgWinHit/TxtHint").GetComponent<Text>();
		btnStart = base.transform.Find("Down/BtnGame").GetComponent<Button>();
		btnStop = base.transform.Find("Down/BtnGameStop").GetComponent<Button>();
		btnDice = base.transform.Find("Down/BtnDice").GetComponent<Button>();
		btnAuto = base.transform.Find("Down/BtnAuto").GetComponent<Button>();
		btnStopAuto = base.transform.Find("Down/BtnStopAuto").GetComponent<Button>();
		btnPlus = base.transform.Find("Down/BtnPlus").GetComponent<Button>();
		btnHelp = base.transform.Find("Down/BtnHelp").GetComponent<Button>();
		btnBlack = base.transform.Find("Down/BtnBlack").GetComponent<Button>();
		mOptions = base.transform.parent.Find("Options").GetComponent<PTM_OptionsController>();
		freenGameTitle = base.transform.Find("Down/FreenGameTitle");
		freenGameTitle.gameObject.SetActive(value: false);
		freenGameText = freenGameTitle.Find("Text").GetComponent<Text>();
		freenWin = base.transform.Find("Down/WinFreen");
		freenWinText = freenWin.Find("Text").GetComponent<Text>();
		freenWin.gameObject.SetActive(value: false);
		goUIContainer = base.gameObject;
		goContainer = base.gameObject;
		cellManager = base.transform.Find("Cells").GetComponent<PTM_CellManager>();
		lineManager = base.transform.Find("Lines").GetComponent<PTM_LinesManager>();
		ballManager = base.transform.Find("LineBalls").GetComponent<PTM_BallManager>();
		textLineNum = base.transform.Find("Down/ImgScoreBg1/TxtYX").GetComponent<Text>();
		txtSingleStake = base.transform.Find("Down/BtnPlus/TxtDXYF").GetComponent<Text>();
		txtCredit = base.transform.Find("Down/ImgScoreBg2/TxtZF").GetComponent<Text>();
		txtWinScore = base.transform.Find("Down/ImgScoreBg2/TxtDF").GetComponent<Text>();
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
		btnPlus.onClick.AddListener(OnBtnPlus_Click);
		btnHelp.onClick.AddListener(mOptions.OnBtnRules_Click);
		btnBlack.onClick.AddListener(mOptions.OnBtnReturn_Click);
		btnDice.onClick.AddListener(OnBtnDice_Click);
	}

	private void Start()
	{
		Init();
	}

	public void PreInit()
	{
		goUIContainer = base.gameObject;
		goContainer = base.gameObject;
		cellManager = base.transform.Find("Cells").GetComponent<PTM_CellManager>();
		lineManager = base.transform.Find("Lines").GetComponent<PTM_LinesManager>();
		ballManager = base.transform.Find("LineBalls").GetComponent<PTM_BallManager>();
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
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().RegisterHandler("gameResult", HandleNetMsg_GameResult);
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().RegisterHandler("maryResult", HandleNetMsg_MaryResult);
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().RegisterHandler("deskInfo", HandleNetMsg_DeskInfo);
		PTM_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		PTM_MB_Singleton<PTM_MaryGameController>.GetInstance().onMaryGameEnd = Handle_MaryEndGame;
		language = ((PTM_GVars.language == "en") ? 1 : 0);
		MonoBehaviour.print(language);
		txtDice.text = ((language != 0) ? "Dice" : "比倍");
	}

	public void Show()
	{
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().ChangeView("MajorGame");
		PTM_Utils.TrySetActive(goUIContainer, active: true);
		PTM_Utils.TrySetActive(goContainer, active: true);
		SetCredit(PTM_GVars.credit);
		StakeRebound();
		PTM_LockManager.UnLock("btn_options", force: true);
		PTM_MB_Singleton<PTM_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		PTM_MB_Singleton<PTM_OptionsController>.GetInstance().onItemBank = OnItemBankAction;
		PTM_MB_Singleton<PTM_OptionsController>.GetInstance().onItemSettings = OnSettingAction;
		PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
	}

	private void ShowWild()
	{
		List<int> list = new List<int>();
		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);
		List<int> list2 = list;
		Lineshowwild = new List<int>();
		for (int i = 0; i < 2; i++)
		{
			int index = UnityEngine.Random.Range(0, list2.Count);
			Lineshowwild.Add(list2[index]);
			list2.Remove(list2[index]);
		}
		Sequence s = DOTween.Sequence();
		for (int j = 0; j < Lineshowwild.Count; j++)
		{
			int index2 = Lineshowwild[j];
			GoldBatlist[index2].enabled = true;
			GoldBatlist[index2].gameObject.SetActive(value: true);
			GoldBatlist[index2].Play("GoldBat");
		}
		s.AppendInterval(1f);
		s.AppendCallback(delegate
		{
			for (int k = 0; k < Lineshowwild.Count; k++)
			{
				int index3 = Lineshowwild[k];
				ShowWild(index3, isshow: true);
			}
		});
		s.AppendInterval(1f);
		s.AppendCallback(CloseAllbat);
	}

	private void CloseAllbat()
	{
		for (int i = 0; i < 5; i++)
		{
			GoldBatlist[i].enabled = false;
			GoldBatlist[i].gameObject.SetActive(value: false);
		}
	}

	private void ShowWild(int index, bool isshow)
	{
		if (index < WildLinelist.Count)
		{
			WildLinelist[index].SetActive(isshow);
		}
	}

	private void CloseAllWild()
	{
		for (int i = 0; i < 5; i++)
		{
			ShowWild(i, isshow: false);
		}
	}

	private void PlayChange()
	{
		Showchange1 = cellManager.Changeline;
		UnityEngine.Debug.LogError("变图轴: " + JsonMapper.ToJson(Showchange1));
		for (int i = 0; i < Showchange1.Count; i++)
		{
			int index = Showchange1[i];
			Wildanilist[index].gameObject.SetActive(value: true);
			Wildanilist[index].Play("Wildani");
		}
		Sequence s = DOTween.Sequence();
		s.AppendInterval(1.2f);
		s.AppendCallback(cellManager.ChangeSym);
		s.AppendInterval(1.2f);
		UnityEngine.Debug.LogError("PlayChange");
		s.AppendCallback(CloseAllchange);
	}

	private void CloseAllchange()
	{
		for (int i = 0; i < 5; i++)
		{
			Wildanilist[i].gameObject.SetActive(value: false);
		}
		Showchange1.Clear();
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
		PTM_Utils.TrySetActive(goUIContainer, active: false);
		PTM_Utils.TrySetActive(goContainer, active: false);
		PTM_SoundManager.Instance.StopMajorAudio();
	}

	public void ResetGame()
	{
		UnityEngine.Debug.LogError("重置游戏");
		if (cellManager != null)
		{
			cellManager.ResetAllCells();
		}
		cellManager.SetAllCellsState();
		cellManager.HideAllCellBorders();
		ballManager.HideAllBall();
		lineManager.HideAllLines();
		OutGame();
		PTM_MB_Singleton<PTM_HUDController>.GetInstance().Hide();
		PTM_MB_Singleton<PTM_HUDController>.GetInstance().HideRules();
		PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().Hide();
		PTM_MB_Singleton<PTM_OptionsController>.GetInstance().Hide();
		PTM_MB_Singleton<PTM_SettingsController>.GetInstance().Hide();
		PTM_MB_Singleton<PTM_DiceGameController2>.GetInstance().Hide();
		PTM_MB_Singleton<PTM_MaryGameController>.GetInstance().Hide();
		PTM_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
	}

	public void InitGame(bool halfInit = false)
	{
		PTM_MB_Singleton<PTM_HUDController>.GetInstance().Show();
		PTM_MB_Singleton<PTM_HUDController>.GetInstance().ResetSprite();
		PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().SetKeepScore(0);
		PTM_LockManager.UnLock("ScoreBank", force: true);
		PTM_LockManager.UnLock("btn_options", force: true);
		PTM_MB_Singleton<PTM_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
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
		PTM_LockManager.UnLock("Delay_Overflow");
		bGaming = false;
		if (!halfInit)
		{
			SetLineNum(gameLineNum);
			SetSingleStake(PTM_GVars.desk.minSinglelineBet);
			SetTotalStake(gameLineNum * PTM_GVars.desk.minSinglelineBet);
			SetCredit(0, 0, check: true);
			bAuto = false;
			SetBtnAutoState("auto");
		}
		SetWinScore(0);
		JudgeBtnMinusPlus();
		JudgeBtnDice();
		SetBtnShowLinesEnable(isEnable: true);
		canShowWinLines = false;
		bReplaying = false;
		cellManager.SetAllCellsState();
		cellManager.HideAllCellBorders();
		ballManager.HideAllBall();
		lineManager.HideAllLines();
		lastResult = null;
		maryTimes = 0;
		UnityEngine.Debug.LogError("InitGame");
		SetBtnStartState(PTM_BtnState.start);
		HideBigWinTip();
		HideWinScoreTip();
		UnityEngine.Debug.Log($"单线最小: {PTM_GVars.desk.minSinglelineBet}, 单线最大: {PTM_GVars.desk.maxSinglelineBet}, 单线压分距: {PTM_GVars.desk.singlechangeScore}, diceSwitch: {PTM_GVars.desk.diceSwitch}, diceDirectSwitch: {PTM_GVars.desk.diceDirectSwitch}");
		if (bAuto && winScore == 0)
		{
			StartGame();
		}
	}

	private void OutGame()
	{
		bAuto = false;
		freenGameTitle.gameObject.SetActive(value: false);
		isFreenGame = false;
		isOneFreen = true;
		freenWin.gameObject.SetActive(value: false);
		btnStart.interactable = true;
		btnStart.gameObject.SetActive(value: false);
		btnStop.interactable = true;
		btnStop.gameObject.SetActive(value: false);
		btnAuto.gameObject.SetActive(value: true);
		btnAuto.interactable = true;
		btnStopAuto.interactable = true;
		btnStopAuto.gameObject.SetActive(value: false);
		SetBtnPlusEnable(isEnable: true);
	}

	public void ExitGame()
	{
		OutGame();
		PTM_MB_Singleton<PTM_HUDController>.GetInstance().Hide();
		PTM_SoundManager.Instance.StopNumberRollAudio();
		Send_LeaveDesk();
	}

	private void WinningLineAnimationControl(bool isReplay = false, bool isChange = false)
	{
		int num = cellManager.CalHitResult2(lastResult, isChange);
		SetWinScore(num);
		float delay = isReplay ? 0.1f : 0.1f;
		_task_WinLineAni = new PTM_XTask(cellManager.WinLinesAni(isReplay, delay, lastLineNum, lastSingleStake, lineManager, ballManager, delegate
		{
		}, delegate
		{
		}));
		PTM_XTask task_WinLineAni = _task_WinLineAni;
		task_WinLineAni.Finished = (PTM_XTask.FinishedHandler)Delegate.Combine(task_WinLineAni.Finished, (PTM_XTask.FinishedHandler)delegate(bool manual)
		{
			cellManager.StopShowAllHitCell();
			if (manual)
			{
				Handle_AllFinished();
			}
			if (!bAuto && !manual)
			{
				SetBtnShowLinesEnable(isEnable: true);
			}
			JudgeBtnDice();
			_task_WinLineAni = null;
		});
	}

	public void JudgeResetGame(int credit = 0)
	{
		UnityEngine.Debug.Log("JudgeResetGame");
		if (PTM_GVars.curView == "MajorGame")
		{
			HandleNoResponse_GameStart(credit);
			return;
		}
		if (PTM_GVars.curView == "DiceGame")
		{
			PTM_MB_Singleton<PTM_DiceGameController2>.GetInstance().HandleNoResponse_MultiInfo();
			SetCredit(credit);
		}
		UnityEngine.Debug.Log("no need reset game");
	}

	public void OnBtnStart_Click()
	{
		if (!PTM_GVars.tryLockOnePoint)
		{
			PTM_SoundManager.Instance.PlayClickAudio();
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
		if (!PTM_GVars.tryLockOnePoint)
		{
			PTM_SoundManager.Instance.StopMaryAudio();
			cellManager.celebrateCount = 4;
			if (_task_NormalReduceTextAni != null)
			{
				_task_NormalReduceTextAni.Stop();
				PTM_SoundManager.Instance.NumberRollEndAudio();
			}
			if (_task_WinLineAni != null)
			{
				_task_WinLineAni.Stop();
				PTM_SoundManager.Instance.StopMajorAudio();
			}
			if (!cellManager.forceStopRolling)
			{
				cellManager.forceStopRolling = true;
				PTM_SoundManager.Instance.PlayMajorRollEndAudio();
			}
			btnStop.interactable = false;
			isReStart = false;
			SetBtnStartState(PTM_BtnState.reStart);
		}
	}

	private void GainScore()
	{
		PTM_LockManager.Lock("ScoreBank");
		PTM_LockManager.Lock("btnDice");
		int finalCredit = PTM_GVars.credit + winScore;
		bGainScoreBtn = false;
		_task_NormalReduceTextAni = new PTM_XTask(PTM_Utils.NormalReduceTextAni(0f, 0.05f, winScore, delegate(int total, int diff)
		{
			SetWinScore(winScore - diff);
			SetBigWinTip(winScore - diff);
			SetCredit(PTM_GVars.credit + diff);
		}, null));
		PTM_XTask task_NormalReduceTextAni = _task_NormalReduceTextAni;
		task_NormalReduceTextAni.Finished = (PTM_XTask.FinishedHandler)Delegate.Combine(task_NormalReduceTextAni.Finished, (PTM_XTask.FinishedHandler)delegate(bool manual)
		{
			if (manual)
			{
				SetWinScore(0);
				SetCredit(finalCredit, -1, check: true);
			}
			SetSomeBtnsEnable(isEnable: true);
			if (totalStake < PTM_GVars.credit)
			{
				StakeRebound();
			}
			JudgeBtnMinusPlus();
			PTM_LockManager.UnLock("ScoreBank", force: true);
			PTM_LockManager.UnLock("btnDice", force: true);
			_task_NormalReduceTextAni = null;
			canShowWinLines = false;
			HideBigWinTip();
			SetBtnShowLinesEnable(isEnable: true);
			StopFreenGame();
			if (PTM_MB_Singleton<PTM_NetManager>.GetInstance().isReady)
			{
				if (bAuto)
				{
					StartGame();
				}
			}
			else if (bAuto)
			{
				UnityEngine.Debug.Break();
				StartCoroutine(PTM_Utils.WaitCall(() => PTM_MB_Singleton<PTM_NetManager>.GetInstance().isReady, delegate
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
		int rightGold = PTM_ScoreBank.GetRightGold();
		if (PTM_GVars.credit < totalStake && freenGameTitle.gameObject != null && !freenGameTitle.gameObject.activeInHierarchy)
		{
			if (PTM_GVars.credit == 0 || (PTM_GVars.credit < PTM_GVars.desk.minSinglelineBet * gameLineNum && rightGold != 0))
			{
				PTM_SoundManager.Instance.PlayClickAudio();
				PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().Show();
				PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().InitBank();
				SetAndIfChangeThenPlayAutoAni(toAuto: false);
				return;
			}
			if (rightGold == 0 && PTM_GVars.credit < PTM_GVars.desk.minSinglelineBet * gameLineNum)
			{
			}
		}
		LowCreditMaxStake();
		SetBtnStartState(PTM_BtnState.disable);
		if (freenGameTitle.gameObject != null && freenGameTitle.gameObject.activeInHierarchy)
		{
			UnityEngine.Debug.LogError("免费,不扣钱");
			SetCredit(PTM_GVars.credit, PTM_GVars.credit);
		}
		else
		{
			UnityEngine.Debug.Log("扣:" + (PTM_GVars.credit - totalStake));
			SetCredit(PTM_GVars.credit - totalStake, PTM_GVars.credit - totalStake);
		}
		coGridRolling = StartCoroutine(cellManager.GridRollingControl(delegate
		{
			coGridRolling = null;
		}));
		lastResult = null;
		Send_GameStart();
		listAni.Clear();
		PTM_SoundManager.Instance.PlayMajorRollAudio();
		SetSomeBtnsEnable(isEnable: false);
		PTM_LockManager.Lock("ScoreBank");
		SetBtnShowLinesEnable(isEnable: false);
		lineManager.HideAllLines();
		canShowWinLines = false;
		bGaming = true;
		SetBtnPlusEnable(isEnable: false);
		PTM_LockManager.Lock("Delay_Overflow");
	}

	public void OnBtnMinus_Click()
	{
		UnityEngine.Debug.Log("OnBtnMinus_Click");
		if (!PTM_GVars.tryLockOnePoint)
		{
			PTM_SoundManager.Instance.PlayClickAudio();
			PTM_Desk desk = PTM_GVars.desk;
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
		if (!PTM_GVars.tryLockOnePoint)
		{
			PTM_SoundManager.Instance.PlayClickAudio();
			PTM_Desk desk = PTM_GVars.desk;
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

	public void OnBtnMaxStake_Click()
	{
		UnityEngine.Debug.Log("OnBtnMaxStake_Click");
		if (PTM_GVars.tryLockOnePoint)
		{
			return;
		}
		PTM_SoundManager.Instance.PlayClickAudio();
		PTM_Desk desk = PTM_GVars.desk;
		int num = lineNum;
		int num2 = singleStake;
		if (PTM_GVars.credit >= desk.maxSinglelineBet * gameLineNum)
		{
			num = gameLineNum;
			num2 = desk.maxSinglelineBet;
		}
		else if (PTM_GVars.credit >= desk.minSinglelineBet * gameLineNum)
		{
			num = gameLineNum;
			int origin = PTM_GVars.credit / gameLineNum;
			num2 = CalGearValue(desk.minSinglelineBet, desk.maxSinglelineBet, desk.singlechangeScore, origin);
		}
		else if (PTM_GVars.credit >= gameLineNum)
		{
			num = gameLineNum;
			num2 = PTM_GVars.credit / gameLineNum;
		}
		else
		{
			if (PTM_GVars.credit >= gameLineNum || PTM_GVars.credit <= 0)
			{
				return;
			}
			num = PTM_GVars.credit;
			num2 = 1;
		}
		SetLineNum(num);
		SetSingleStake(num2);
		SetTotalStake(num2 * num);
		JudgeBtnMinusPlus();
	}

	private void LowCreditMaxStake()
	{
		if (PTM_GVars.credit > singleStake * lineNum)
		{
			return;
		}
		PTM_Desk desk = PTM_GVars.desk;
		int num = lineNum;
		int num2 = singleStake;
		if (PTM_GVars.credit >= desk.minSinglelineBet * gameLineNum)
		{
			num = gameLineNum;
			int origin = PTM_GVars.credit / gameLineNum;
			num2 = CalGearValue(desk.minSinglelineBet, desk.maxSinglelineBet, desk.singlechangeScore, origin);
		}
		else if (PTM_GVars.credit >= gameLineNum)
		{
			num = gameLineNum;
			num2 = PTM_GVars.credit / gameLineNum;
		}
		else
		{
			if (PTM_GVars.credit >= gameLineNum || PTM_GVars.credit <= 0)
			{
				return;
			}
			num = PTM_GVars.credit;
			num2 = 1;
		}
		SetLineNum(num);
		SetSingleStake(num2);
		SetTotalStake(num2 * num);
	}

	public void OnBtnTestNet_Click()
	{
		UnityEngine.Debug.Log("OnBtnTestNet");
		if (!PTM_GVars.tryLockOnePoint)
		{
			PTM_MB_Singleton<PTM_LobbyViewController>.GetInstance().Send_EnterDesk(295);
		}
	}

	public void OnBtnDice_Click()
	{
		UnityEngine.Debug.LogError("按下了比倍");
		if (PTM_GVars.tryLockOnePoint)
		{
			return;
		}
		PTM_SoundManager.Instance.StopMaryAudio();
		PTM_SoundManager.Instance.PlayClickAudio();
		int diceBet = 0;
		if (!PTM_LockManager.IsLocked("btnDice") && !bReplaying)
		{
			if (PTM_GVars.credit == 0 && winScore == 0)
			{
				PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().Show();
				PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().InitBank();
				SetAndIfChangeThenPlayAutoAni(toAuto: false);
				return;
			}
			SetBtnStartState(PTM_BtnState.start);
			diceBet = winScore;
			HideBigWinTip();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			canShowWinLines = false;
			PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "OnBtnDice");
			PTM_MB_Singleton<PTM_GameRoot>.GetInstance().StartCoroutine(PTM_Utils.DelayCall(0.1f, delegate
			{
				PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "OnBtnDice done");
				SetBtnShowLinesEnable(isEnable: false);
				lineManager.HideAllLines();
				canShowWinLines = false;
				Hide();
				PTM_MB_Singleton<PTM_DiceGameController2>.GetInstance().Show();
				PTM_MB_Singleton<PTM_DiceGameController2>.GetInstance().Show();
				PTM_MB_Singleton<PTM_DiceGameController2>.GetInstance().InitGame(PTM_GVars.credit, diceBet);
				PTM_SoundManager.Instance.PlayDiceBGM();
				SetWinScore(0);
			}));
		}
	}

	public void OnBtnAuto_Click()
	{
		UnityEngine.Debug.Log("OnBtnAuto_Click");
		if (PTM_GVars.tryLockOnePoint)
		{
			return;
		}
		PTM_SoundManager.Instance.PlayClickAudio();
		if (PTM_GVars.credit == 0 && winScore == 0)
		{
			PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().Show();
			PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().InitBank();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			return;
		}
		SetAndIfChangeThenPlayAutoAni(toAuto: true);
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

	public void Auto_FreenGame()
	{
		isFreenGame = true;
		freenGameTitle.gameObject.SetActive(value: true);
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
		if (!PTM_GVars.tryLockOnePoint)
		{
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
			}
			PTM_SoundManager.Instance.StopMaryAudio();
			if (_task_WinLineAni != null)
			{
				_task_WinLineAni.Stop();
				PTM_SoundManager.Instance.StopMajorAudio();
			}
			if (!cellManager.forceStopRolling)
			{
				cellManager.forceStopRolling = true;
				PTM_SoundManager.Instance.PlayMajorRollEndAudio();
			}
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			btnStop.interactable = false;
			btnStart.interactable = false;
			btnAuto.interactable = false;
		}
	}

	public void OnBtnShowLines_Click()
	{
		UnityEngine.Debug.Log("OnBtnShowLines_Click");
		if (!PTM_GVars.tryLockOnePoint)
		{
			PTM_SoundManager.Instance.PlayClickAudio();
			if (canShowWinLines)
			{
				bReplaying = true;
				WinningLineAnimationControl(bReplaying);
				SetBtnShowLinesEnable(isEnable: false);
				isReStart = false;
				SetBtnStartState(PTM_BtnState.stop);
				SetBtnDiceEnable(isEnable: false);
			}
		}
	}

	public void OnBtnShowLines_PointDown()
	{
		if (PTM_GVars.tryLockOnePoint)
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
			PTM_GVars.desk.id,
			lineNum,
			singleStake,
			totalStake
		};
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().Send("userService/gameStart", args);
		PTM_StateManager.GetInstance().RegisterWork("Send_GameStart");
		PTM_StateManager.GetInstance().RegisterWork("m_IsFree");
		gameTimes++;
		int num = gameTimes;
	}

	public void Send_LeaveDesk()
	{
		object[] args = new object[1]
		{
			PTM_GVars.desk.id
		};
		PTM_MB_Singleton<PTM_NetManager>.GetInstance().Send("userService/leaveDesk", args);
	}

	public void HandleNetMsg_GameResult(object[] args)
	{
		PTM_StateManager.GetInstance().CompleteWork("Send_GameStart");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		try
		{
			UnityEngine.Debug.LogError("开奖信息: " + JsonMapper.ToJson(dictionary));
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		lastResult = new PTM_MajorResult().Init(lineNum, singleStake, totalStake, dictionary, cellManager);
		isFreenStop = lastResult.isFreenGameStop;
		if (!lastResult.maryGame)
		{
			PTM_StateManager.GetInstance().CompleteWork("m_IsFree");
		}
		SetCredit(-1, lastResult.totalWin + PTM_GVars.realCredit);
		cellManager.UpdateCellGridData(lastResult.gameContent);
		cellManager.stopRolling = true;
		isReStart = false;
		SetBtnStartState(PTM_BtnState.stop);
	}

	public void HandleNoResponse_GameStart(int credit)
	{
		if (false || !PTM_StateManager.GetInstance().IsWorkCompleted("Send_GameStart"))
		{
			UnityEngine.Debug.Log("Send_GameStart or MaryGame was not completed");
			PTM_StateManager.GetInstance().CompleteWork("Send_GameStart");
			PTM_StateManager.GetInstance().CompleteWork("m_IsFree");
			if (coGridRolling != null)
			{
				StopCoroutine(coGridRolling);
				coGridRolling = null;
				ResetGame();
				InitGame(halfInit: true);
				if (bAuto)
				{
					bGaming = false;
				}
				if (!bAuto)
				{
					UnityEngine.Debug.LogError("网络存在波动，请继续游戏");
					PTM_MB_Singleton<PTM_AlertDialog>.GetInstance().ShowDialog((PTM_GVars.language == "zh") ? "网络存在波动,重连成功!,请继续游戏" : "Network error，please go on");
				}
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
		UnityEngine.Debug.Log(PTM_LogHelper.NetHandle("HandleNetMsg_MaryResult"));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		PTM_StateManager.GetInstance().CompleteWork("MaryGame");
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
		StartCoroutine(PTM_Utils.WaitCall(() => lastResult != null, delegate
		{
			UnityEngine.Debug.Log($"wait: {Time.time - beginTime}s");
			PTM_MB_Singleton<PTM_MaryGameController>.GetInstance().PrepareGame(lastResult.maryCount, PTM_GVars.credit, totalBet, photoNumbers, photosArray, totalWins);
			SetCredit(-1, credit);
		}));
	}

	public void HandleNetMsg_DeskInfo(object[] args)
	{
		UnityEngine.Debug.Log(PTM_LogHelper.NetHandle("HandleNetMsg_DeskInfo"));
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
		WinningLineAnimationControl();
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
		bGaming = false;
		PTM_LockManager.UnLock("Delay_Overflow");
		SetSomeBtnsEnable(isEnable: true);
		JudgeBtnMinusPlus();
		canShowWinLines = false;
		PTM_LockManager.UnLock("ScoreBank", force: true);
		PTM_LockManager.UnLock("btn_options", force: true);
		PTM_MB_Singleton<PTM_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		SetBtnShowLinesEnable(isEnable: true);
		if (PTM_GVars.credit == 0)
		{
			UnityEngine.Debug.Log("Lose: credit is zero");
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
		}
		UnityEngine.Debug.LogWarning("Handle_AllFinished");
		SetBtnPlusEnable(isEnable: true);
		isReStart = true;
		SetBtnStartState(PTM_BtnState.start);
		if (bReplaying)
		{
			bReplaying = false;
			return;
		}
		if (lastResult.maryCount > 0 && lastResult.isFreenStart && isOneFreen)
		{
			lastResult.isFreenStart = false;
			isOneFreen = false;
			PTM_MB_Singleton<PTM_MaryMovieController>.GetInstance().Play();
			return;
		}
		JudgeBtnDice();
		if (PTM_LockManager.IsLocked("Overflow"))
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
		PTM_MB_Singleton<PTM_MaryGameController>.GetInstance().Hide();
		Show();
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "enter mary");
		PTM_MB_Singleton<PTM_HUDController>.GetInstance().Show();
		SetWinScore(maryWin);
		ShowBigWinTip(maryWin);
		JudgeBtnDice();
		bGaming = false;
		if (PTM_LockManager.IsLocked("Overflow"))
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
		string content = (PTM_GVars.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge";
		PTM_LockManager.Lock("Esc");
		PTM_LockManager.Lock("Quit");
		PTM_MB_Singleton<PTM_GameManager>.Get().PrepareQuitGame();
		PTM_MB_Singleton<PTM_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			PTM_MB_Singleton<PTM_GameManager>.GetInstance().QuitToHallGame();
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
		PTM_MB_Singleton<PTM_OptionsController>.GetInstance().Hide();
		PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().Show();
		PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().InitBank();
	}

	private void OnSettingAction()
	{
		PTM_MB_Singleton<PTM_OptionsController>.GetInstance().Hide();
		PTM_MB_Singleton<PTM_SettingsController>.GetInstance().Show();
	}

	private void OnSetScoreAndGoldAction()
	{
		if (goUIContainer.activeInHierarchy)
		{
			txtCredit.text = PTM_GVars.credit.ToString();
			if (PTM_GVars.credit == 0 && winScore == 0)
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
			PTM_GVars.credit = credit;
			txtCredit.text = credit.ToString();
		}
		if (realCredit >= 0)
		{
			PTM_GVars.realCredit = realCredit;
		}
		if (check && PTM_GVars.credit != PTM_GVars.realCredit)
		{
			UnityEngine.Debug.LogError($"credit: {PTM_GVars.credit}, realCredit: {PTM_GVars.realCredit}");
		}
	}

	private void SetWinScore(int winScore)
	{
		this.winScore = winScore;
		PTM_GVars.winScorce = winScore;
		txtWinScore.text = this.winScore.ToString();
	}

	private void SetBtnDiceEnable(bool isEnable)
	{
		btnDice.gameObject.SetActive(isEnable);
	}

	private void SetBtnAutoEnable(bool isEnable)
	{
		btnAuto.interactable = isEnable;
	}

	private void SetBtnShowLinesEnable(bool isEnable)
	{
	}

	private void SetBtnPlusEnable(bool isEnable)
	{
		btnPlus.interactable = isEnable;
		btnHelp.interactable = isEnable;
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

	public void SetBtnStartState(PTM_BtnState cSF_BtnState = PTM_BtnState.none)
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

	private IEnumerator WaitSetBtnState(PTM_BtnState cSF_BtnState = PTM_BtnState.none)
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
		case PTM_BtnState.start:
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
		case PTM_BtnState.stop:
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
		case PTM_BtnState.disable:
			btnStart.interactable = false;
			btnStart.gameObject.SetActive(value: true);
			btnStop.interactable = false;
			btnStop.gameObject.SetActive(value: false);
			btnStopAuto.interactable = false;
			btnStopAuto.gameObject.SetActive(value: false);
			btnAuto.interactable = false;
			btnAuto.gameObject.SetActive(value: true);
			break;
		case PTM_BtnState.reStart:
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
		PTM_Desk desk = PTM_GVars.desk;
		UnityEngine.Debug.Log($"ZH2_GVars.credit: {PTM_GVars.credit}, _totalStake: {totalStake},_singleStake: {singleStake},minSinglelineBet: {desk.minSinglelineBet}");
		if (PTM_GVars.credit == 0)
		{
			PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().Show();
			PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().InitBank();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
		}
		else if (PTM_GVars.credit < gameLineNum)
		{
			SetLineNum(PTM_GVars.credit);
			SetSingleStake(1);
			SetTotalStake(PTM_GVars.credit);
		}
		else if (PTM_GVars.credit < desk.minSinglelineBet * gameLineNum)
		{
			int num = PTM_GVars.credit / gameLineNum;
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
		else if (PTM_GVars.credit < lineNum * singleStake)
		{
			int num2 = PTM_GVars.credit / gameLineNum;
			SetLineNum(gameLineNum);
			SetSingleStake(num2);
			SetTotalStake(num2 * gameLineNum);
		}
		else if (lineNum < gameLineNum && PTM_GVars.credit >= gameLineNum)
		{
			SetLineNum(gameLineNum);
			SetSingleStake(singleStake);
			SetTotalStake(singleStake * gameLineNum);
		}
		UnityEngine.Debug.Log($"_lineNum: {lineNum}, _totalStake: {totalStake},_singleStake: {singleStake},minSinglelineBet: {desk.minSinglelineBet}");
	}

	private void JudgeBtnMinusPlus()
	{
		bool flag = singleStake * lineNum >= PTM_GVars.desk.minSinglelineBet * lineNum;
	}

	private void HandleCreditChange()
	{
		int num = int.Parse(txtCredit.text);
		if (num >= PTM_GVars.credit && num > PTM_GVars.credit)
		{
			int rightGold = PTM_ScoreBank.GetRightGold();
			if (PTM_GVars.credit < totalStake && rightGold > 0)
			{
				PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().Show();
			}
		}
	}

	private void StakeRebound()
	{
		PTM_Desk desk = PTM_GVars.desk;
		if (totalStake < desk.minSinglelineBet * gameLineNum)
		{
			if (PTM_GVars.credit >= desk.minSinglelineBet * gameLineNum)
			{
				int minSinglelineBet = desk.minSinglelineBet;
				SetLineNum(gameLineNum);
				SetSingleStake(minSinglelineBet);
				SetTotalStake(minSinglelineBet * gameLineNum);
			}
			else if (PTM_GVars.credit >= gameLineNum)
			{
				int num = PTM_GVars.credit / gameLineNum;
				SetLineNum(gameLineNum);
				SetSingleStake(num);
				SetTotalStake(num * gameLineNum);
			}
			else if (PTM_GVars.credit > 0)
			{
				SetLineNum(PTM_GVars.credit);
				SetSingleStake(1);
				SetTotalStake(PTM_GVars.credit);
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
