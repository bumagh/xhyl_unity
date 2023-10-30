using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSF_MajorGameController : CSF_MB_Singleton<CSF_MajorGameController>
{
	private GameObject goUIContainer;

	private GameObject goContainer;

	private CSF_OptionsController mOptions;

	private CSF_CellManager cellManager;

	private CSF_LinesManager lineManager;

	private CSF_BallManager ballManager;

	private Text textLineNum;

	private Text txtSingleStake;

	private Text freenGameText;

	private Text txtCredit;

	private Text txtWinScore;

	private Text txtDice;

	private Button btnStart;

	private Button btnAuto;

	private Button btnPlus;

	private Button btnHelp;

	private Button btnBlack;

	private Button btnStop;

	private Button btnStopAuto;

	private Transform tfAuto;

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

	private bool isChange;

	private bool isFreenStop;

	private int winScore;

	private int singleStake;

	private int totalStake;

	private int lineNum;

	private int maryTimes;

	private bool isOneFreen = true;

	private CSF_MajorResult lastResult;

	private List<Coroutine> listAni = new List<Coroutine>();

	private CSF_XTask _task_WinLineAni;

	private CSF_XTask _task_NormalReduceTextAni;

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

	private bool isReStart;

	public List<int> Showchange1
	{
		get;
		set;
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		FinGame();
		if (CSF_MB_Singleton<CSF_MajorGameController>._instance == null)
		{
			CSF_MB_Singleton<CSF_MajorGameController>.SetInstance(this);
			PreInit();
		}
	}

	private void OnEnable()
	{
		CSF_SoundManager.Instance.PlayMajorBGMAndio(isPlay: true);
	}

	private void OnDisable()
	{
		CSF_SoundManager.Instance.PlayMajorBGMAndio(isPlay: false);
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
		tfAuto = base.transform.Find("Down/ImgAuto");
		goWinScoreTip = base.transform.Find("Hint/ImgWinHit").gameObject;
		txtWinScoreTip = base.transform.Find("Hint/ImgWinHit/TxtHint").GetComponent<Text>();
		btnStart = base.transform.Find("Down/BtnGame").GetComponent<Button>();
		btnStop = base.transform.Find("Down/BtnGameStop").GetComponent<Button>();
		btnAuto = base.transform.Find("Down/BtnAuto").GetComponent<Button>();
		btnStopAuto = base.transform.Find("Down/BtnStopAuto").GetComponent<Button>();
		btnPlus = base.transform.Find("Down/BtnPlus").GetComponent<Button>();
		btnHelp = base.transform.Find("Down/BtnHelp").GetComponent<Button>();
		btnBlack = base.transform.Find("Down/BtnBlack").GetComponent<Button>();
		mOptions = base.transform.parent.Find("Options").GetComponent<CSF_OptionsController>();
		freenGameTitle = base.transform.Find("Down/FreenGameTitle");
		freenGameTitle.gameObject.SetActive(value: false);
		freenGameText = freenGameTitle.Find("Text").GetComponent<Text>();
		freenWin = base.transform.Find("Down/WinFreen");
		freenWinText = freenWin.Find("Text").GetComponent<Text>();
		freenWin.gameObject.SetActive(value: false);
		goUIContainer = base.gameObject;
		goContainer = base.gameObject;
		cellManager = base.transform.Find("Cells").GetComponent<CSF_CellManager>();
		lineManager = base.transform.Find("Lines").GetComponent<CSF_LinesManager>();
		ballManager = base.transform.Find("LineBalls").GetComponent<CSF_BallManager>();
		textLineNum = base.transform.Find("Down/ImgScoreBg1/TxtYX").GetComponent<Text>();
		txtSingleStake = base.transform.Find("Down/BtnPlus/TxtDXYF").GetComponent<Text>();
		txtCredit = base.transform.Find("Down/ImgScoreBg2/TxtZF").GetComponent<Text>();
		txtWinScore = base.transform.Find("Down/ImgScoreBg2/TxtDF").GetComponent<Text>();
		txtDice = base.transform.Find("Down/TxtDice").GetComponent<Text>();
		OnClickListener();
		SetStarAndStopState(isStar: true, isStop: false);
		tfAuto.gameObject.SetActive(value: false);
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
	}

	private void Start()
	{
		Init();
	}

	public void PreInit()
	{
		goUIContainer = base.gameObject;
		goContainer = base.gameObject;
		cellManager = base.transform.Find("Cells").GetComponent<CSF_CellManager>();
		lineManager = base.transform.Find("Lines").GetComponent<CSF_LinesManager>();
		ballManager = base.transform.Find("LineBalls").GetComponent<CSF_BallManager>();
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
		CSF_MB_Singleton<CSF_NetManager>.GetInstance().RegisterHandler("gameResult", HandleNetMsg_GameResult);
		CSF_MB_Singleton<CSF_NetManager>.GetInstance().RegisterHandler("maryResult", HandleNetMsg_MaryResult);
		CSF_MB_Singleton<CSF_NetManager>.GetInstance().RegisterHandler("deskInfo", HandleNetMsg_DeskInfo);
		CSF_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		CSF_MB_Singleton<CSF_MaryGameController>.GetInstance().onMaryGameEnd = Handle_MaryEndGame;
		language = ((CSF_MySqlConnection.language == "en") ? 1 : 0);
		MonoBehaviour.print(language);
		txtDice.text = ((language != 0) ? "Dice" : "比倍");
	}

	public void Show()
	{
		CSF_MB_Singleton<CSF_GameManager>.GetInstance().ChangeView("MajorGame");
		CSF_Utils.TrySetActive(goUIContainer, active: true);
		CSF_Utils.TrySetActive(goContainer, active: true);
		SetCredit(CSF_MySqlConnection.credit);
		StakeRebound();
		CSF_LockManager.UnLock("btn_options", force: true);
		CSF_MB_Singleton<CSF_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		CSF_MB_Singleton<CSF_OptionsController>.GetInstance().onItemBank = OnItemBankAction;
		CSF_MB_Singleton<CSF_OptionsController>.GetInstance().onItemSettings = OnSettingAction;
		CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
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
		CSF_Utils.TrySetActive(goUIContainer, active: false);
		CSF_Utils.TrySetActive(goContainer, active: false);
		CSF_SoundManager.Instance.StopMajorAudio();
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
		CSF_MB_Singleton<CSF_HUDController>.GetInstance().Hide();
		CSF_MB_Singleton<CSF_HUDController>.GetInstance().HideRules();
		CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().Hide();
		CSF_MB_Singleton<CSF_OptionsController>.GetInstance().Hide();
		CSF_MB_Singleton<CSF_SettingsController>.GetInstance().Hide();
		CSF_MB_Singleton<CSF_DiceGameController2>.GetInstance().Hide();
		CSF_MB_Singleton<CSF_MaryGameController>.GetInstance().Hide();
		CSF_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
	}

	public void InitGame(bool halfInit = false)
	{
		CSF_MB_Singleton<CSF_HUDController>.GetInstance().Show();
		CSF_MB_Singleton<CSF_HUDController>.GetInstance().ResetSprite();
		CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().SetKeepScore(0);
		CSF_LockManager.UnLock("ScoreBank", force: true);
		CSF_LockManager.UnLock("btn_options", force: true);
		CSF_MB_Singleton<CSF_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
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
		CSF_LockManager.UnLock("Delay_Overflow");
		bGaming = false;
		if (!halfInit)
		{
			SetLineNum(9);
			SetSingleStake(CSF_MySqlConnection.desk.minSinglelineBet);
			SetTotalStake(9 * CSF_MySqlConnection.desk.minSinglelineBet);
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
		SetBtnStartState(CSF_BtnState.start);
		HideBigWinTip();
		HideWinScoreTip();
		UnityEngine.Debug.Log($"单线最小: {CSF_MySqlConnection.desk.minSinglelineBet}, 单线最大: {CSF_MySqlConnection.desk.maxSinglelineBet}, 单线压分距: {CSF_MySqlConnection.desk.singlechangeScore}, diceSwitch: {CSF_MySqlConnection.desk.diceSwitch}, diceDirectSwitch: {CSF_MySqlConnection.desk.diceDirectSwitch}");
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
		btnStop.interactable = true;
		btnStop.gameObject.SetActive(value: false);
		btnAuto.interactable = true;
		btnStopAuto.interactable = true;
		btnStopAuto.gameObject.SetActive(value: false);
		SetBtnPlusEnable(isEnable: true);
	}

	public void ExitGame()
	{
		OutGame();
		CSF_MB_Singleton<CSF_HUDController>.GetInstance().Hide();
		CSF_SoundManager.Instance.StopNumberRollAudio();
		Send_LeaveDesk();
	}

	private void WinningLineAnimationControl(bool isReplay = false, bool isChange = false)
	{
		int num = cellManager.CalHitResult2(lastResult, isChange);
		SetWinScore(num);
		float delay = isReplay ? 0.1f : 0.1f;
		_task_WinLineAni = new CSF_XTask(cellManager.WinLinesAni(isReplay, delay, lastLineNum, lastSingleStake, lineManager, ballManager, delegate
		{
			if (isReplay)
			{
			}
		}, delegate
		{
		}));
		CSF_XTask task_WinLineAni = _task_WinLineAni;
		task_WinLineAni.Finished = (CSF_XTask.FinishedHandler)Delegate.Combine(task_WinLineAni.Finished, (CSF_XTask.FinishedHandler)delegate(bool manual)
		{
			cellManager.StopShowAllHitCell();
			if (manual)
			{
				if (!isReplay)
				{
				}
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
		if (CSF_MySqlConnection.curView == "MajorGame")
		{
			HandleNoResponse_GameStart(credit);
			return;
		}
		if (CSF_MySqlConnection.curView == "DiceGame")
		{
			CSF_MB_Singleton<CSF_DiceGameController2>.GetInstance().HandleNoResponse_MultiInfo();
			SetCredit(credit);
		}
		UnityEngine.Debug.Log("no need reset game");
	}

	public void OnBtnStart_Click()
	{
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			CSF_SoundManager.Instance.PlayClickAudio();
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
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			CSF_SoundManager.Instance.StopMaryAudio();
			cellManager.celebrateCount = 4;
			if (_task_NormalReduceTextAni != null)
			{
				_task_NormalReduceTextAni.Stop();
				CSF_SoundManager.Instance.NumberRollEndAudio();
			}
			if (_task_WinLineAni != null)
			{
				_task_WinLineAni.Stop();
				CSF_SoundManager.Instance.StopMajorAudio();
			}
			if (!cellManager.forceStopRolling)
			{
				cellManager.forceStopRolling = true;
				CSF_SoundManager.Instance.PlayMajorRollEndAudio();
			}
			btnStop.interactable = false;
			isReStart = false;
			SetBtnStartState(CSF_BtnState.reStart);
		}
	}

	private void GainScore()
	{
		CSF_LockManager.Lock("ScoreBank");
		CSF_LockManager.Lock("btnDice");
		int finalCredit = CSF_MySqlConnection.credit + winScore;
		bGainScoreBtn = false;
		_task_NormalReduceTextAni = new CSF_XTask(CSF_Utils.NormalReduceTextAni(0f, 0.05f, winScore, delegate(int total, int diff)
		{
			SetWinScore(winScore - diff);
			SetBigWinTip(winScore - diff);
			SetCredit(CSF_MySqlConnection.credit + diff);
		}, null));
		CSF_XTask task_NormalReduceTextAni = _task_NormalReduceTextAni;
		task_NormalReduceTextAni.Finished = (CSF_XTask.FinishedHandler)Delegate.Combine(task_NormalReduceTextAni.Finished, (CSF_XTask.FinishedHandler)delegate(bool manual)
		{
			if (manual)
			{
				SetWinScore(0);
				SetCredit(finalCredit, -1, check: true);
			}
			SetSomeBtnsEnable(isEnable: true);
			if (totalStake < CSF_MySqlConnection.credit)
			{
				StakeRebound();
			}
			JudgeBtnMinusPlus();
			CSF_LockManager.UnLock("ScoreBank", force: true);
			CSF_LockManager.UnLock("btnDice", force: true);
			_task_NormalReduceTextAni = null;
			canShowWinLines = false;
			HideBigWinTip();
			SetBtnShowLinesEnable(isEnable: true);
			StopFreenGame();
			if (CSF_MB_Singleton<CSF_NetManager>.GetInstance().isReady)
			{
				if (bAuto)
				{
					StartGame();
				}
			}
			else if (bAuto)
			{
				UnityEngine.Debug.Break();
				StartCoroutine(CSF_Utils.WaitCall(() => CSF_MB_Singleton<CSF_NetManager>.GetInstance().isReady, delegate
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
		int rightGold = CSF_ScoreBank.GetRightGold();
		if (CSF_MySqlConnection.credit < totalStake && freenGameTitle.gameObject != null && !freenGameTitle.gameObject.activeInHierarchy)
		{
			if (CSF_MySqlConnection.credit == 0 || (CSF_MySqlConnection.credit < CSF_MySqlConnection.desk.minSinglelineBet * 9 && rightGold != 0))
			{
				CSF_SoundManager.Instance.PlayClickAudio();
				CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().Show();
				CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().InitBank();
				SetAndIfChangeThenPlayAutoAni(toAuto: false);
				return;
			}
			if (rightGold == 0 && CSF_MySqlConnection.credit < CSF_MySqlConnection.desk.minSinglelineBet * 9)
			{
			}
		}
		LowCreditMaxStake();
		SetBtnStartState(CSF_BtnState.disable);
		if (freenGameTitle.gameObject != null && freenGameTitle.gameObject.activeInHierarchy)
		{
			UnityEngine.Debug.LogError("免费,不扣钱");
			SetCredit(CSF_MySqlConnection.credit, CSF_MySqlConnection.credit);
		}
		else
		{
			UnityEngine.Debug.Log("扣:" + (CSF_MySqlConnection.credit - totalStake));
			SetCredit(CSF_MySqlConnection.credit - totalStake, CSF_MySqlConnection.credit - totalStake);
		}
		coGridRolling = StartCoroutine(cellManager.GridRollingControl(delegate
		{
			coGridRolling = null;
		}));
		lastResult = null;
		Send_GameStart();
		listAni.Clear();
		CSF_SoundManager.Instance.PlayMajorRollAudio();
		SetSomeBtnsEnable(isEnable: false);
		CSF_LockManager.Lock("ScoreBank");
		SetBtnShowLinesEnable(isEnable: false);
		lineManager.HideAllLines();
		canShowWinLines = false;
		bGaming = true;
		SetBtnPlusEnable(isEnable: false);
		CSF_LockManager.Lock("Delay_Overflow");
	}

	public void OnBtnMinus_Click()
	{
		UnityEngine.Debug.Log("OnBtnMinus_Click");
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			CSF_SoundManager.Instance.PlayClickAudio();
			CSF_Desk desk = CSF_MySqlConnection.desk;
			if (totalStake >= desk.minSinglelineBet * 9)
			{
				int num = (singleStake == desk.minSinglelineBet) ? desk.maxSinglelineBet : (singleStake - desk.singlechangeScore);
				num = ((num < desk.minSinglelineBet) ? desk.minSinglelineBet : num);
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
	}

	public void OnBtnPlus_Click()
	{
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			CSF_SoundManager.Instance.PlayClickAudio();
			CSF_Desk desk = CSF_MySqlConnection.desk;
			if (totalStake >= desk.minSinglelineBet * 9)
			{
				int num = (singleStake == desk.maxSinglelineBet) ? desk.minSinglelineBet : (singleStake + desk.singlechangeScore);
				num = ((num > desk.maxSinglelineBet) ? desk.maxSinglelineBet : num);
				SetLineNum(9);
				SetSingleStake(num);
				SetTotalStake(num * 9);
				UnityEngine.Debug.Log("单线压分: " + num);
			}
			else
			{
				UnityEngine.Debug.LogError($"BtnPlus should not be clicked. _totalStake: {totalStake}, minSinglelineBet: {desk.minSinglelineBet}*9");
			}
		}
	}

	public void OnBtnMaxStake_Click()
	{
		UnityEngine.Debug.Log("OnBtnMaxStake_Click");
		if (CSF_MySqlConnection.tryLockOnePoint)
		{
			return;
		}
		CSF_SoundManager.Instance.PlayClickAudio();
		CSF_Desk desk = CSF_MySqlConnection.desk;
		int num = lineNum;
		int num2 = singleStake;
		if (CSF_MySqlConnection.credit >= desk.maxSinglelineBet * 9)
		{
			num = 9;
			num2 = desk.maxSinglelineBet;
		}
		else if (CSF_MySqlConnection.credit >= desk.minSinglelineBet * 9)
		{
			num = 9;
			int origin = CSF_MySqlConnection.credit / 9;
			num2 = CalGearValue(desk.minSinglelineBet, desk.maxSinglelineBet, desk.singlechangeScore, origin);
		}
		else if (CSF_MySqlConnection.credit >= 9)
		{
			num = 9;
			num2 = CSF_MySqlConnection.credit / 9;
		}
		else
		{
			if (CSF_MySqlConnection.credit >= 9 || CSF_MySqlConnection.credit <= 0)
			{
				return;
			}
			num = CSF_MySqlConnection.credit;
			num2 = 1;
		}
		SetLineNum(num);
		SetSingleStake(num2);
		SetTotalStake(num2 * num);
		JudgeBtnMinusPlus();
	}

	private void LowCreditMaxStake()
	{
		if (CSF_MySqlConnection.credit > singleStake * lineNum)
		{
			return;
		}
		CSF_Desk desk = CSF_MySqlConnection.desk;
		int num = lineNum;
		int num2 = singleStake;
		if (CSF_MySqlConnection.credit >= desk.minSinglelineBet * 9)
		{
			num = 9;
			int origin = CSF_MySqlConnection.credit / 9;
			num2 = CalGearValue(desk.minSinglelineBet, desk.maxSinglelineBet, desk.singlechangeScore, origin);
		}
		else if (CSF_MySqlConnection.credit >= 9)
		{
			num = 9;
			num2 = CSF_MySqlConnection.credit / 9;
		}
		else
		{
			if (CSF_MySqlConnection.credit >= 9 || CSF_MySqlConnection.credit <= 0)
			{
				return;
			}
			num = CSF_MySqlConnection.credit;
			num2 = 1;
		}
		SetLineNum(num);
		SetSingleStake(num2);
		SetTotalStake(num2 * num);
	}

	public void OnBtnTestNet_Click()
	{
		UnityEngine.Debug.Log("OnBtnTestNet");
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			CSF_MB_Singleton<CSF_LobbyViewController>.GetInstance().Send_EnterDesk(295);
		}
	}

	public void OnBtnDice_Click()
	{
		UnityEngine.Debug.Log("OnBtnDice_Click");
		if (CSF_MySqlConnection.tryLockOnePoint)
		{
			return;
		}
		CSF_SoundManager.Instance.StopMaryAudio();
		CSF_SoundManager.Instance.PlayClickAudio();
		int diceBet = 0;
		if (CSF_LockManager.IsLocked("btnDice") || bReplaying)
		{
			return;
		}
		if (CSF_MySqlConnection.credit == 0 && winScore == 0)
		{
			CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().Show();
			CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().InitBank();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			return;
		}
		if (winScore == 0)
		{
			diceBet = ((CSF_MySqlConnection.credit <= totalStake) ? CSF_MySqlConnection.credit : totalStake);
			if (lastResult.freenWin > 0)
			{
				UnityEngine.Debug.Log("免费,不扣钱");
			}
			else
			{
				CSF_MySqlConnection.credit -= diceBet;
			}
			SetCredit(CSF_MySqlConnection.credit);
		}
		else
		{
			int num = winScore;
			SetBtnStartState(CSF_BtnState.start);
			diceBet = winScore;
			HideBigWinTip();
		}
		SetAndIfChangeThenPlayAutoAni(toAuto: false);
		canShowWinLines = false;
		CSF_MB_Singleton<CSF_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "OnBtnDice");
		CSF_MB_Singleton<CSF_GameRoot>.GetInstance().StartCoroutine(CSF_Utils.DelayCall(0.1f, delegate
		{
			CSF_MB_Singleton<CSF_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "OnBtnDice done");
			SetBtnShowLinesEnable(isEnable: false);
			lineManager.HideAllLines();
			canShowWinLines = false;
			Hide();
			CSF_MB_Singleton<CSF_DiceGameController>.GetInstance().Show();
			CSF_MB_Singleton<CSF_DiceGameController2>.GetInstance().Show();
			CSF_MB_Singleton<CSF_DiceGameController2>.GetInstance().InitGame(CSF_MySqlConnection.credit, diceBet);
			CSF_SoundManager.Instance.PlayDiceBGM();
			SetWinScore(0);
		}));
	}

	public void OnBtnAuto_Click()
	{
		UnityEngine.Debug.Log("OnBtnAuto_Click");
		if (CSF_MySqlConnection.tryLockOnePoint)
		{
			return;
		}
		CSF_SoundManager.Instance.PlayClickAudio();
		if (CSF_MySqlConnection.credit == 0 && winScore == 0)
		{
			CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().Show();
			CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().InitBank();
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
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
			}
			CSF_SoundManager.Instance.StopMaryAudio();
			if (_task_WinLineAni != null)
			{
				_task_WinLineAni.Stop();
				CSF_SoundManager.Instance.StopMajorAudio();
			}
			if (!cellManager.forceStopRolling)
			{
				cellManager.forceStopRolling = true;
				CSF_SoundManager.Instance.PlayMajorRollEndAudio();
			}
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			btnStop.interactable = false;
			btnStart.interactable = false;
			btnAuto.interactable = false;
		}
	}

	private void PlayAutoAni()
	{
		if (tfAuto != null)
		{
			if (bAuto)
			{
				tfAuto.gameObject.SetActive(value: true);
				tfAuto.DOLocalMoveY(-242f, 0.5f);
			}
			else
			{
				tfAuto.gameObject.SetActive(value: false);
				tfAuto.DOLocalMoveY(-292f, 0.5f);
			}
		}
	}

	public void OnBtnShowLines_Click()
	{
		UnityEngine.Debug.Log("OnBtnShowLines_Click");
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			CSF_SoundManager.Instance.PlayClickAudio();
			if (canShowWinLines)
			{
				bReplaying = true;
				WinningLineAnimationControl(bReplaying);
				SetBtnShowLinesEnable(isEnable: false);
				isReStart = false;
				SetBtnStartState(CSF_BtnState.stop);
				SetBtnDiceEnable(isEnable: false);
			}
		}
	}

	public void OnBtnShowLines_PointDown()
	{
		if (CSF_MySqlConnection.tryLockOnePoint)
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
			CSF_MySqlConnection.desk.id,
			lineNum,
			singleStake,
			totalStake
		};
		CSF_MB_Singleton<CSF_NetManager>.GetInstance().Send("userService/gameStart", args);
		CSF_StateManager.GetInstance().RegisterWork("Send_GameStart");
		CSF_StateManager.GetInstance().RegisterWork("m_IsFree");
		gameTimes++;
		int num = gameTimes;
	}

	public void Send_LeaveDesk()
	{
		object[] args = new object[1]
		{
			CSF_MySqlConnection.desk.id
		};
		CSF_MB_Singleton<CSF_NetManager>.GetInstance().Send("userService/leaveDesk", args);
	}

	public void HandleNetMsg_GameResult(object[] args)
	{
		CSF_StateManager.GetInstance().CompleteWork("Send_GameStart");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		try
		{
			UnityEngine.Debug.LogError("开奖信息: " + JsonMapper.ToJson(dictionary));
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		lastResult = new CSF_MajorResult().Init(lineNum, singleStake, totalStake, dictionary, cellManager, out isChange);
		isFreenStop = lastResult.isFreenGameStop;
		if (!lastResult.maryGame)
		{
			CSF_StateManager.GetInstance().CompleteWork("m_IsFree");
		}
		SetCredit(-1, lastResult.totalWin + CSF_MySqlConnection.realCredit);
		cellManager.UpdateCellGridData(lastResult.gameContent, lastResult.oldGameContent);
		cellManager.stopRolling = true;
		isReStart = false;
		SetBtnStartState(CSF_BtnState.stop);
	}

	public void HandleNoResponse_GameStart(int credit)
	{
		if (false || !CSF_StateManager.GetInstance().IsWorkCompleted("Send_GameStart"))
		{
			UnityEngine.Debug.Log("Send_GameStart or MaryGame was not completed");
			CSF_StateManager.GetInstance().CompleteWork("Send_GameStart");
			CSF_StateManager.GetInstance().CompleteWork("m_IsFree");
			if (coGridRolling != null)
			{
				StopCoroutine(coGridRolling);
				coGridRolling = null;
				ResetGame();
				InitGame(halfInit: true);
				if (!bAuto)
				{
					UnityEngine.Debug.LogError("网络存在波动，请继续游戏");
					CSF_MB_Singleton<CSF_AlertDialog>.GetInstance().ShowDialog((CSF_MySqlConnection.language == "zh") ? "网络存在波动,重连成功!,请继续游戏" : "Network error，please go on");
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
		UnityEngine.Debug.Log(CSF_LogHelper.NetHandle("HandleNetMsg_MaryResult"));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		CSF_StateManager.GetInstance().CompleteWork("MaryGame");
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
		StartCoroutine(CSF_Utils.WaitCall(() => lastResult != null, delegate
		{
			UnityEngine.Debug.Log($"wait: {Time.time - beginTime}s");
			CSF_MB_Singleton<CSF_MaryGameController>.GetInstance().PrepareGame(lastResult.maryCount, CSF_MySqlConnection.credit, totalBet, photoNumbers, photosArray, totalWins);
			SetCredit(-1, credit);
		}));
	}

	public void HandleNetMsg_DeskInfo(object[] args)
	{
		UnityEngine.Debug.Log(CSF_LogHelper.NetHandle("HandleNetMsg_DeskInfo"));
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
		bGaming = true;
		lastLineNum = lineNum;
		lastSingleStake = singleStake;
		if (!isChange)
		{
			WinningLineAnimationControl();
			return;
		}
		CSF_SoundManager.Instance.PlayPeopleFullAwardAudio(isChange: true);
		PlayChange();
		StartCoroutine(WaitChange());
	}

	private IEnumerator WaitChange()
	{
		yield return new WaitForSeconds(2.5f);
		WinningLineAnimationControl(isReplay: false, isChange: true);
	}

	public bool JungeAuto()
	{
		return bAuto;
	}

	private void Handle_AllFinished()
	{
		cellManager.SetAllCellsState(CSF_CellState.normal, isChange: true);
		cellManager.HideAllCellBorders();
		ballManager.HideAllBall();
		lineManager.HideAllLines();
		bGaming = false;
		CSF_LockManager.UnLock("Delay_Overflow");
		SetSomeBtnsEnable(isEnable: true);
		JudgeBtnMinusPlus();
		canShowWinLines = false;
		CSF_LockManager.UnLock("ScoreBank", force: true);
		CSF_LockManager.UnLock("btn_options", force: true);
		CSF_MB_Singleton<CSF_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		SetBtnShowLinesEnable(isEnable: true);
		if (CSF_MySqlConnection.credit == 0)
		{
			UnityEngine.Debug.Log("Lose: credit is zero");
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
		}
		UnityEngine.Debug.LogWarning("Handle_AllFinished");
		SetBtnPlusEnable(isEnable: true);
		isReStart = true;
		SetBtnStartState(CSF_BtnState.start);
		if (bReplaying)
		{
			bReplaying = false;
			return;
		}
		if (lastResult.maryCount > 0 && lastResult.isFreenStart && isOneFreen)
		{
			lastResult.isFreenStart = false;
			isOneFreen = false;
			CSF_MB_Singleton<CSF_MaryMovieController>.GetInstance().Play();
			return;
		}
		JudgeBtnDice();
		if (CSF_LockManager.IsLocked("Overflow"))
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
		CSF_MB_Singleton<CSF_MaryGameController>.GetInstance().Hide();
		Show();
		CSF_MB_Singleton<CSF_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "enter mary");
		CSF_MB_Singleton<CSF_HUDController>.GetInstance().Show();
		SetWinScore(maryWin);
		ShowBigWinTip(maryWin);
		JudgeBtnDice();
		bGaming = false;
		if (CSF_LockManager.IsLocked("Overflow"))
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
		string content = (CSF_MySqlConnection.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge";
		CSF_LockManager.Lock("Esc");
		CSF_LockManager.Lock("Quit");
		CSF_MB_Singleton<CSF_GameManager>.Get().PrepareQuitGame();
		CSF_MB_Singleton<CSF_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			CSF_MB_Singleton<CSF_GameManager>.GetInstance().QuitToHallGame();
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
		CSF_MB_Singleton<CSF_OptionsController>.GetInstance().Hide();
		CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().Show();
		CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().InitBank();
	}

	private void OnSettingAction()
	{
		CSF_MB_Singleton<CSF_OptionsController>.GetInstance().Hide();
		CSF_MB_Singleton<CSF_SettingsController>.GetInstance().Show();
	}

	private void OnSetScoreAndGoldAction()
	{
		if (goUIContainer.activeInHierarchy)
		{
			txtCredit.text = CSF_MySqlConnection.credit.ToString();
			if (CSF_MySqlConnection.credit == 0 && winScore == 0)
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

	private void SetCredit(int credit, int realCredit = -1, bool check = false)
	{
		if (credit >= 0)
		{
			CSF_MySqlConnection.credit = credit;
			txtCredit.text = credit.ToString();
		}
		if (realCredit >= 0)
		{
			CSF_MySqlConnection.realCredit = realCredit;
		}
		if (check && CSF_MySqlConnection.credit != CSF_MySqlConnection.realCredit)
		{
			UnityEngine.Debug.LogError($"credit: {CSF_MySqlConnection.credit}, realCredit: {CSF_MySqlConnection.realCredit}");
		}
	}

	private void SetWinScore(int winScore)
	{
		this.winScore = winScore;
		txtWinScore.text = this.winScore.ToString();
	}

	private void SetBtnDiceEnable(bool isEnable)
	{
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

	public void SetBtnStartState(CSF_BtnState cSF_BtnState = CSF_BtnState.none)
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

	private IEnumerator WaitSetBtnState(CSF_BtnState cSF_BtnState = CSF_BtnState.none)
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
		case CSF_BtnState.start:
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
		case CSF_BtnState.stop:
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
		case CSF_BtnState.disable:
			btnStart.interactable = false;
			btnStart.gameObject.SetActive(value: true);
			btnStop.interactable = false;
			btnStop.gameObject.SetActive(value: false);
			btnStopAuto.interactable = false;
			btnStopAuto.gameObject.SetActive(value: false);
			btnAuto.interactable = false;
			btnAuto.gameObject.SetActive(value: true);
			break;
		case CSF_BtnState.reStart:
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
		CSF_Desk desk = CSF_MySqlConnection.desk;
		UnityEngine.Debug.Log($"ZH2_GVars.credit: {CSF_MySqlConnection.credit}, _totalStake: {totalStake},_singleStake: {singleStake},minSinglelineBet: {desk.minSinglelineBet}");
		if (CSF_MySqlConnection.credit == 0)
		{
			CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().Show();
			CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().InitBank();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
		}
		else if (CSF_MySqlConnection.credit < 9)
		{
			SetLineNum(CSF_MySqlConnection.credit);
			SetSingleStake(1);
			SetTotalStake(CSF_MySqlConnection.credit);
		}
		else if (CSF_MySqlConnection.credit < desk.minSinglelineBet * 9)
		{
			int num = CSF_MySqlConnection.credit / 9;
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
		else if (CSF_MySqlConnection.credit < lineNum * singleStake)
		{
			int num2 = CSF_MySqlConnection.credit / 9;
			SetLineNum(9);
			SetSingleStake(num2);
			SetTotalStake(num2 * 9);
		}
		else if (lineNum < 9 && CSF_MySqlConnection.credit >= 9)
		{
			SetLineNum(9);
			SetSingleStake(singleStake);
			SetTotalStake(singleStake * 9);
		}
		UnityEngine.Debug.Log($"_lineNum: {lineNum}, _totalStake: {totalStake},_singleStake: {singleStake},minSinglelineBet: {desk.minSinglelineBet}");
	}

	private void JudgeBtnMinusPlus()
	{
		bool flag = singleStake * lineNum >= CSF_MySqlConnection.desk.minSinglelineBet * 9;
	}

	private void HandleCreditChange()
	{
		int num = int.Parse(txtCredit.text);
		if (num >= CSF_MySqlConnection.credit && num > CSF_MySqlConnection.credit)
		{
			int rightGold = CSF_ScoreBank.GetRightGold();
			if (CSF_MySqlConnection.credit < totalStake && rightGold > 0)
			{
				CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().Show();
			}
		}
	}

	private void StakeRebound()
	{
		CSF_Desk desk = CSF_MySqlConnection.desk;
		if (totalStake < desk.minSinglelineBet * 9)
		{
			if (CSF_MySqlConnection.credit >= desk.minSinglelineBet * 9)
			{
				int minSinglelineBet = desk.minSinglelineBet;
				SetLineNum(9);
				SetSingleStake(minSinglelineBet);
				SetTotalStake(minSinglelineBet * 9);
			}
			else if (CSF_MySqlConnection.credit >= 9)
			{
				int num = CSF_MySqlConnection.credit / 9;
				SetLineNum(9);
				SetSingleStake(num);
				SetTotalStake(num * 9);
			}
			else if (CSF_MySqlConnection.credit > 0)
			{
				SetLineNum(CSF_MySqlConnection.credit);
				SetSingleStake(1);
				SetTotalStake(CSF_MySqlConnection.credit);
			}
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
		return winScore < CSF_MySqlConnection.desk.diceOverflow && ((CSF_MySqlConnection.desk.diceSwitch == 1 && winScore != 0) || CSF_MySqlConnection.desk.diceDirectSwitch == 1);
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
			PlayAutoAni();
		}
		else
		{
			SetBtnAutoState("auto");
			PlayAutoAni();
		}
	}
}
