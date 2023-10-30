using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LKB_MajorGameController : LKB_MB_Singleton<LKB_MajorGameController>
{
	private GameObject goUIContainer;

	private GameObject goContainer;

	private LKB_OptionsController mOptions;

	private LKB_CellManager cellManager;

	private LKB_LinesManager lineManager;

	private LKB_BallManager ballManager;

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

	private Transform freenGameTitle;

	private Transform freenWin;

	private Text freenWinText;

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

	private LKB_MajorResult lastResult;

	private List<Coroutine> listAni = new List<Coroutine>();

	private LKB_XTask _task_WinLineAni;

	private LKB_XTask _task_NormalReduceTextAni;

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
		if (LKB_MB_Singleton<LKB_MajorGameController>._instance == null)
		{
			LKB_MB_Singleton<LKB_MajorGameController>.SetInstance(this);
			PreInit();
		}
	}

	private void OnEnable()
	{
		LKB_SoundManager.Instance.PlayMajorBGMAndio(isPlay: true);
	}

	private void OnDisable()
	{
		LKB_SoundManager.Instance.PlayMajorBGMAndio(isPlay: false);
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
		btnStart = base.transform.Find("Down/BtnGame").GetComponent<Button>();
		btnStop = base.transform.Find("Down/BtnGameStop").GetComponent<Button>();
		btnAuto = base.transform.Find("Down/BtnAuto").GetComponent<Button>();
		btnStopAuto = base.transform.Find("Down/BtnStopAuto").GetComponent<Button>();
		btnPlus = base.transform.Find("Down/BtnPlus").GetComponent<Button>();
		btnHelp = base.transform.Find("Down/BtnHelp").GetComponent<Button>();
		btnBlack = base.transform.Find("Down/BtnBlack").GetComponent<Button>();
		mOptions = base.transform.parent.Find("Options").GetComponent<LKB_OptionsController>();
		freenGameTitle = base.transform.Find("Down/FreenGameTitle");
		freenGameTitle.gameObject.SetActive(value: false);
		freenGameText = freenGameTitle.Find("Text").GetComponent<Text>();
		freenWin = base.transform.Find("Down/WinFreen");
		freenWinText = freenWin.Find("Text").GetComponent<Text>();
		freenWin.gameObject.SetActive(value: false);
		goUIContainer = base.gameObject;
		goContainer = base.gameObject;
		cellManager = base.transform.Find("Cells").GetComponent<LKB_CellManager>();
		lineManager = base.transform.Find("Lines").GetComponent<LKB_LinesManager>();
		ballManager = base.transform.Find("LineBalls").GetComponent<LKB_BallManager>();
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
	}

	private void Start()
	{
		Init();
	}

	public void PreInit()
	{
		goUIContainer = base.gameObject;
		goContainer = base.gameObject;
		cellManager = base.transform.Find("Cells").GetComponent<LKB_CellManager>();
		lineManager = base.transform.Find("Lines").GetComponent<LKB_LinesManager>();
		ballManager = base.transform.Find("LineBalls").GetComponent<LKB_BallManager>();
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
		LKB_MB_Singleton<LKB_NetManager>.GetInstance().RegisterHandler("gameResult", HandleNetMsg_GameResult);
		LKB_MB_Singleton<LKB_NetManager>.GetInstance().RegisterHandler("maryResult", HandleNetMsg_MaryResult);
		LKB_MB_Singleton<LKB_NetManager>.GetInstance().RegisterHandler("deskInfo", HandleNetMsg_DeskInfo);
		LKB_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
		language = ((LKB_GVars.language == "en") ? 1 : 0);
		MonoBehaviour.print(language);
		txtDice.text = ((language != 0) ? "Dice" : "比倍");
	}

	public void Show()
	{
		LKB_MB_Singleton<LKB_GameManager>.GetInstance().ChangeView("MajorGame");
		LKB_Utils.TrySetActive(goUIContainer, active: true);
		LKB_Utils.TrySetActive(goContainer, active: true);
		SetCredit(LKB_GVars.credit);
		StakeRebound();
		LKB_LockManager.UnLock("btn_options", force: true);
		LKB_MB_Singleton<LKB_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		LKB_MB_Singleton<LKB_OptionsController>.GetInstance().onItemBank = OnItemBankAction;
		LKB_MB_Singleton<LKB_OptionsController>.GetInstance().onItemSettings = OnSettingAction;
		LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().onSetScoreAndGold = OnSetScoreAndGoldAction;
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
		LKB_Utils.TrySetActive(goUIContainer, active: false);
		LKB_Utils.TrySetActive(goContainer, active: false);
		LKB_SoundManager.Instance.StopMajorAudio();
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
		LKB_MB_Singleton<LKB_HUDController>.GetInstance().Hide();
		LKB_MB_Singleton<LKB_HUDController>.GetInstance().HideRules();
		LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().Hide();
		LKB_MB_Singleton<LKB_OptionsController>.GetInstance().Hide();
		LKB_MB_Singleton<LKB_SettingsController>.GetInstance().Hide();
		LKB_LockManager.UnLock("Deposit_Or_Withdraw", force: true);
	}

	public void InitGame(bool halfInit = false)
	{
		LKB_MB_Singleton<LKB_HUDController>.GetInstance().Show();
		LKB_MB_Singleton<LKB_HUDController>.GetInstance().ResetSprite();
		LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().SetKeepScore(0);
		LKB_LockManager.UnLock("ScoreBank", force: true);
		LKB_LockManager.UnLock("btn_options", force: true);
		LKB_MB_Singleton<LKB_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
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
		LKB_LockManager.UnLock("Delay_Overflow");
		bGaming = false;
		if (!halfInit)
		{
			SetLineNum(9);
			SetSingleStake(LKB_GVars.desk.minSinglelineBet);
			SetTotalStake(9 * LKB_GVars.desk.minSinglelineBet);
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
		SetBtnStartState(LKB_BtnState.start);
		HideBigWinTip();
		HideWinScoreTip();
		UnityEngine.Debug.Log($"单线最小: {LKB_GVars.desk.minSinglelineBet}, 单线最大: {LKB_GVars.desk.maxSinglelineBet}, 单线压分距: {LKB_GVars.desk.singlechangeScore}, diceSwitch: {LKB_GVars.desk.diceSwitch}, diceDirectSwitch: {LKB_GVars.desk.diceDirectSwitch}");
		if (bAuto && winScore == 0)
		{
			StartGame();
		}
	}

	public void ExitGame()
	{
		LKB_MB_Singleton<LKB_HUDController>.GetInstance().Hide();
		LKB_SoundManager.Instance.StopNumberRollAudio();
		Send_LeaveDesk();
	}

	private void WinningLineAnimationControl(bool isReplay = false)
	{
		int num = cellManager.CalHitResult2(lastResult);
		SetWinScore(num);
		float delay = isReplay ? 0.1f : 0.1f;
		_task_WinLineAni = new LKB_XTask(cellManager.WinLinesAni(isReplay, delay, lastLineNum, lastSingleStake, lineManager, ballManager, delegate
		{
		}, delegate
		{
		}));
		LKB_XTask task_WinLineAni = _task_WinLineAni;
		task_WinLineAni.Finished = (LKB_XTask.FinishedHandler)Delegate.Combine(task_WinLineAni.Finished, (LKB_XTask.FinishedHandler)delegate(bool manual)
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
		UnityEngine.Debug.LogError("-----------JudgeResetGame-----------" + LKB_GVars.curView);
		if (LKB_GVars.curView == "MajorGame")
		{
			HandleNoResponse_GameStart(credit);
			return;
		}
		if (LKB_GVars.curView == "DiceGame")
		{
			LKB_MB_Singleton<LKB_DiceGameController2>.GetInstance().HandleNoResponse_MultiInfo();
			SetCredit(credit);
		}
		UnityEngine.Debug.Log("no need reset game");
	}

	public void OnBtnStart_Click()
	{
		if (!LKB_GVars.tryLockOnePoint)
		{
			LKB_SoundManager.Instance.PlayClickAudio();
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
		if (!LKB_GVars.tryLockOnePoint)
		{
			LKB_SoundManager.Instance.StopMaryAudio();
			cellManager.celebrateCount = 4;
			if (_task_NormalReduceTextAni != null)
			{
				_task_NormalReduceTextAni.Stop();
				LKB_SoundManager.Instance.NumberRollEndAudio();
			}
			if (_task_WinLineAni != null)
			{
				_task_WinLineAni.Stop();
				LKB_SoundManager.Instance.StopMajorAudio();
			}
			if (!cellManager.forceStopRolling)
			{
				cellManager.forceStopRolling = true;
				LKB_SoundManager.Instance.PlayMajorRollEndAudio();
			}
			btnStop.interactable = false;
			isReStart = false;
			SetBtnStartState(LKB_BtnState.reStart);
		}
	}

	private void GainScore()
	{
		LKB_LockManager.Lock("ScoreBank");
		LKB_LockManager.Lock("btnDice");
		int finalCredit = LKB_GVars.credit + winScore;
		bGainScoreBtn = false;
		_task_NormalReduceTextAni = new LKB_XTask(LKB_Utils.NormalReduceTextAni(0f, 0.05f, winScore, delegate(int total, int diff)
		{
			SetWinScore(winScore - diff);
			SetBigWinTip(winScore - diff);
			SetCredit(LKB_GVars.credit + diff);
		}, null));
		LKB_XTask task_NormalReduceTextAni = _task_NormalReduceTextAni;
		task_NormalReduceTextAni.Finished = (LKB_XTask.FinishedHandler)Delegate.Combine(task_NormalReduceTextAni.Finished, (LKB_XTask.FinishedHandler)delegate(bool manual)
		{
			if (manual)
			{
				SetWinScore(0);
				SetCredit(finalCredit, -1, check: true);
			}
			SetSomeBtnsEnable(isEnable: true);
			if (totalStake < LKB_GVars.credit)
			{
				StakeRebound();
			}
			JudgeBtnMinusPlus();
			LKB_LockManager.UnLock("ScoreBank", force: true);
			LKB_LockManager.UnLock("btnDice", force: true);
			_task_NormalReduceTextAni = null;
			canShowWinLines = false;
			HideBigWinTip();
			SetBtnShowLinesEnable(isEnable: true);
			StopFreenGame();
			if (LKB_MB_Singleton<LKB_NetManager>.GetInstance().isReady)
			{
				if (bAuto)
				{
					StartGame();
				}
			}
			else if (bAuto)
			{
				UnityEngine.Debug.Break();
				StartCoroutine(LKB_Utils.WaitCall(() => LKB_MB_Singleton<LKB_NetManager>.GetInstance().isReady, delegate
				{
					StartGame();
				}));
			}
		});
	}

	private void StopFreenGame()
	{
	}

	private IEnumerator WaitStopFreenGame()
	{
		yield return new WaitForSeconds(1.5f);
		freenWin.gameObject.SetActive(value: false);
		btnStart.interactable = true;
		btnStop.interactable = true;
		btnStop.gameObject.SetActive(value: false);
		btnAuto.interactable = true;
		btnStopAuto.interactable = true;
		btnStopAuto.gameObject.SetActive(value: false);
	}

	private void StartGame()
	{
		UnityEngine.Debug.Log("开始游戏");
		int rightGold = LKB_ScoreBank.GetRightGold();
		if (LKB_GVars.credit < totalStake && freenGameTitle.gameObject != null && !freenGameTitle.gameObject.activeInHierarchy)
		{
			if (LKB_GVars.credit == 0 || (LKB_GVars.credit < LKB_GVars.desk.minSinglelineBet * 9 && rightGold != 0))
			{
				LKB_SoundManager.Instance.PlayClickAudio();
				LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().Show();
				LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().InitBank();
				SetAndIfChangeThenPlayAutoAni(toAuto: false);
				return;
			}
			if (rightGold == 0 && LKB_GVars.credit < LKB_GVars.desk.minSinglelineBet * 9)
			{
			}
		}
		LowCreditMaxStake();
		SetBtnStartState(LKB_BtnState.disable);
		if (freenGameTitle.gameObject != null && freenGameTitle.gameObject.activeInHierarchy)
		{
			UnityEngine.Debug.LogError("免费,不扣钱");
			SetCredit(LKB_GVars.credit, LKB_GVars.credit);
		}
		else
		{
			UnityEngine.Debug.Log("扣:" + (LKB_GVars.credit - totalStake));
			SetCredit(LKB_GVars.credit - totalStake, LKB_GVars.credit - totalStake);
		}
		coGridRolling = StartCoroutine(cellManager.GridRollingControl(delegate
		{
			coGridRolling = null;
		}));
		lastResult = null;
		Send_GameStart();
		listAni.Clear();
		LKB_SoundManager.Instance.PlayMajorRollAudio();
		SetSomeBtnsEnable(isEnable: false);
		LKB_LockManager.Lock("ScoreBank");
		SetBtnShowLinesEnable(isEnable: false);
		lineManager.HideAllLines();
		canShowWinLines = false;
		bGaming = true;
		SetBtnPlusEnable(isEnable: false);
		LKB_LockManager.Lock("Delay_Overflow");
	}

	public void OnBtnMinus_Click()
	{
		UnityEngine.Debug.Log("OnBtnMinus_Click");
		if (!LKB_GVars.tryLockOnePoint)
		{
			LKB_SoundManager.Instance.PlayClickAudio();
			LKB_Desk desk = LKB_GVars.desk;
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
		if (!LKB_GVars.tryLockOnePoint)
		{
			LKB_SoundManager.Instance.PlayClickAudio();
			LKB_Desk desk = LKB_GVars.desk;
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
		if (LKB_GVars.tryLockOnePoint)
		{
			return;
		}
		LKB_SoundManager.Instance.PlayClickAudio();
		LKB_Desk desk = LKB_GVars.desk;
		int num = lineNum;
		int num2 = singleStake;
		if (LKB_GVars.credit >= desk.maxSinglelineBet * 9)
		{
			num = 9;
			num2 = desk.maxSinglelineBet;
		}
		else if (LKB_GVars.credit >= desk.minSinglelineBet * 9)
		{
			num = 9;
			int origin = LKB_GVars.credit / 9;
			num2 = CalGearValue(desk.minSinglelineBet, desk.maxSinglelineBet, desk.singlechangeScore, origin);
		}
		else if (LKB_GVars.credit >= 9)
		{
			num = 9;
			num2 = LKB_GVars.credit / 9;
		}
		else
		{
			if (LKB_GVars.credit >= 9 || LKB_GVars.credit <= 0)
			{
				return;
			}
			num = LKB_GVars.credit;
			num2 = 1;
		}
		SetLineNum(num);
		SetSingleStake(num2);
		SetTotalStake(num2 * num);
		JudgeBtnMinusPlus();
	}

	private void LowCreditMaxStake()
	{
		if (LKB_GVars.credit > singleStake * lineNum)
		{
			return;
		}
		LKB_Desk desk = LKB_GVars.desk;
		int num = lineNum;
		int num2 = singleStake;
		if (LKB_GVars.credit >= desk.minSinglelineBet * 9)
		{
			num = 9;
			int origin = LKB_GVars.credit / 9;
			num2 = CalGearValue(desk.minSinglelineBet, desk.maxSinglelineBet, desk.singlechangeScore, origin);
		}
		else if (LKB_GVars.credit >= 9)
		{
			num = 9;
			num2 = LKB_GVars.credit / 9;
		}
		else
		{
			if (LKB_GVars.credit >= 9 || LKB_GVars.credit <= 0)
			{
				return;
			}
			num = LKB_GVars.credit;
			num2 = 1;
		}
		SetLineNum(num);
		SetSingleStake(num2);
		SetTotalStake(num2 * num);
	}

	public void OnBtnTestNet_Click()
	{
		UnityEngine.Debug.Log("OnBtnTestNet");
		if (!LKB_GVars.tryLockOnePoint)
		{
			LKB_MB_Singleton<LKB_LobbyViewController>.GetInstance().Send_EnterDesk(295);
		}
	}

	public void OnBtnDice_Click()
	{
		UnityEngine.Debug.Log("OnBtnDice_Click");
		if (LKB_GVars.tryLockOnePoint)
		{
			return;
		}
		LKB_SoundManager.Instance.StopMaryAudio();
		LKB_SoundManager.Instance.PlayClickAudio();
		int diceBet = 0;
		if (LKB_LockManager.IsLocked("btnDice") || bReplaying)
		{
			return;
		}
		if (LKB_GVars.credit == 0 && winScore == 0)
		{
			LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().Show();
			LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().InitBank();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			return;
		}
		if (winScore == 0)
		{
			diceBet = ((LKB_GVars.credit <= totalStake) ? LKB_GVars.credit : totalStake);
			LKB_GVars.credit -= diceBet;
			SetCredit(LKB_GVars.credit);
		}
		else
		{
			int num = winScore;
			SetBtnStartState(LKB_BtnState.start);
			diceBet = winScore;
			HideBigWinTip();
		}
		SetAndIfChangeThenPlayAutoAni(toAuto: false);
		canShowWinLines = false;
		LKB_MB_Singleton<LKB_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "OnBtnDice");
		LKB_MB_Singleton<LKB_GameRoot>.GetInstance().StartCoroutine(LKB_Utils.DelayCall(0.1f, delegate
		{
			LKB_MB_Singleton<LKB_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "OnBtnDice done");
			SetBtnShowLinesEnable(isEnable: false);
			lineManager.HideAllLines();
			canShowWinLines = false;
			Hide();
			LKB_MB_Singleton<LKB_DiceGameController>.GetInstance().Show();
			LKB_MB_Singleton<LKB_DiceGameController2>.GetInstance().Show();
			LKB_MB_Singleton<LKB_DiceGameController2>.GetInstance().InitGame(LKB_GVars.credit, diceBet);
			LKB_SoundManager.Instance.PlayDiceBGM();
			SetWinScore(0);
		}));
	}

	public void OnBtnAuto_Click()
	{
		UnityEngine.Debug.Log("OnBtnAuto_Click");
		if (LKB_GVars.tryLockOnePoint)
		{
			return;
		}
		LKB_SoundManager.Instance.PlayClickAudio();
		if (LKB_GVars.credit == 0 && winScore == 0)
		{
			LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().Show();
			LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().InitBank();
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
	}

	public void OnBtnStopAuto_Click()
	{
		UnityEngine.Debug.Log("OnBtnStopAuto_Click");
		if (!LKB_GVars.tryLockOnePoint)
		{
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
			}
			LKB_SoundManager.Instance.StopMaryAudio();
			if (_task_WinLineAni != null)
			{
				_task_WinLineAni.Stop();
				LKB_SoundManager.Instance.StopMajorAudio();
			}
			if (!cellManager.forceStopRolling)
			{
				cellManager.forceStopRolling = true;
				LKB_SoundManager.Instance.PlayMajorRollEndAudio();
			}
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
			btnStop.interactable = false;
			btnStart.interactable = false;
			btnAuto.interactable = false;
		}
	}

	private void PlayAutoAni()
	{
	}

	public void OnBtnShowLines_Click()
	{
		UnityEngine.Debug.Log("OnBtnShowLines_Click");
		if (!LKB_GVars.tryLockOnePoint)
		{
			LKB_SoundManager.Instance.PlayClickAudio();
			if (canShowWinLines)
			{
				bReplaying = true;
				WinningLineAnimationControl(bReplaying);
				SetBtnShowLinesEnable(isEnable: false);
				isReStart = false;
				SetBtnStartState(LKB_BtnState.stop);
				SetBtnDiceEnable(isEnable: false);
			}
		}
	}

	public void OnBtnShowLines_PointDown()
	{
		if (LKB_GVars.tryLockOnePoint)
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
			LKB_GVars.desk.id,
			lineNum,
			singleStake,
			totalStake
		};
		LKB_MB_Singleton<LKB_NetManager>.GetInstance().Send("userService/gameStart", args);
		LKB_StateManager.GetInstance().RegisterWork("Send_GameStart");
		LKB_StateManager.GetInstance().RegisterWork("m_IsFree");
		gameTimes++;
		int num = gameTimes;
	}

	public void Send_LeaveDesk()
	{
		object[] args = new object[1]
		{
			LKB_GVars.desk.id
		};
		LKB_MB_Singleton<LKB_NetManager>.GetInstance().Send("userService/leaveDesk", args);
	}

	public void HandleNetMsg_GameResult(object[] args)
	{
		LKB_StateManager.GetInstance().CompleteWork("Send_GameStart");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		UnityEngine.Debug.LogError("开奖信息: " + JsonMapper.ToJson(dictionary));
		lastResult = new LKB_MajorResult().Init(lineNum, singleStake, totalStake, dictionary, cellManager);
		SetCredit(-1, lastResult.totalWin + LKB_GVars.realCredit);
		cellManager.UpdateCellGridData(lastResult.gameContent);
		cellManager.stopRolling = true;
		isReStart = false;
		SetBtnStartState(LKB_BtnState.stop);
	}

	public void HandleNoResponse_GameStart(int credit)
	{
		if (!LKB_StateManager.GetInstance().IsWorkCompleted("Send_GameStart"))
		{
			LKB_StateManager.GetInstance().CompleteWork("Send_GameStart");
			LKB_StateManager.GetInstance().CompleteWork("m_IsFree");
			if (coGridRolling != null)
			{
				StopCoroutine(coGridRolling);
				coGridRolling = null;
				ResetGame();
				InitGame(halfInit: true);
				if (!bAuto)
				{
					UnityEngine.Debug.LogError("网络存在波动，请继续游戏");
					LKB_MB_Singleton<LKB_AlertDialog>.GetInstance().ShowDialog((LKB_GVars.language == "zh") ? "网络存在波动,重连成功!,请继续游戏" : "Network error，please go on");
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
		UnityEngine.Debug.Log(LKB_LogHelper.NetHandle("HandleNetMsg_MaryResult"));
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		LKB_StateManager.GetInstance().CompleteWork("m_IsFree");
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
		StartCoroutine(LKB_Utils.WaitCall(() => lastResult != null, delegate
		{
			UnityEngine.Debug.LogError($"wait: {Time.time - beginTime}s");
			LKB_MB_Singleton<LKB_MaryGameController>.GetInstance().PrepareGame(0, LKB_GVars.credit, totalBet, photoNumbers, photosArray, totalWins);
			SetCredit(-1, credit);
		}));
	}

	public void HandleNetMsg_DeskInfo(object[] args)
	{
		UnityEngine.Debug.Log(LKB_LogHelper.NetHandle("HandleNetMsg_DeskInfo"));
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
		LKB_LockManager.UnLock("Delay_Overflow");
		SetSomeBtnsEnable(isEnable: true);
		JudgeBtnMinusPlus();
		canShowWinLines = false;
		LKB_LockManager.UnLock("ScoreBank", force: true);
		LKB_LockManager.UnLock("btn_options", force: true);
		LKB_MB_Singleton<LKB_HUDController>.GetInstance().SetBtnOptionsEnable(isEnable: true);
		SetBtnShowLinesEnable(isEnable: true);
		if (LKB_GVars.credit == 0)
		{
			UnityEngine.Debug.Log("Lose: credit is zero");
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
		}
		UnityEngine.Debug.LogWarning("Handle_AllFinished");
		SetBtnPlusEnable(isEnable: true);
		isReStart = true;
		SetBtnStartState(LKB_BtnState.start);
		if (bReplaying)
		{
			bReplaying = false;
			return;
		}
		if (lastResult.isFreenStart && isOneFreen)
		{
			lastResult.isFreenStart = false;
			isOneFreen = false;
			LKB_MB_Singleton<LKB_MaryMovieController>.GetInstance().Play();
			return;
		}
		JudgeBtnDice();
		if (LKB_LockManager.IsLocked("Overflow"))
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
		LKB_MB_Singleton<LKB_MaryGameController>.GetInstance().Hide();
		Show();
		LKB_MB_Singleton<LKB_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "enter mary");
		LKB_MB_Singleton<LKB_HUDController>.GetInstance().Show();
		SetWinScore(maryWin);
		ShowBigWinTip(maryWin);
		JudgeBtnDice();
		bGaming = false;
		if (LKB_LockManager.IsLocked("Overflow"))
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
		string content = (LKB_GVars.language == "zh") ? "您的账户已爆机，请兑奖" : "Your account is blasting, please excharge";
		LKB_LockManager.Lock("Esc");
		LKB_LockManager.Lock("Quit");
		LKB_MB_Singleton<LKB_GameManager>.Get().PrepareQuitGame();
		LKB_MB_Singleton<LKB_AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			LKB_MB_Singleton<LKB_GameManager>.GetInstance().QuitToHallGame();
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
		LKB_MB_Singleton<LKB_OptionsController>.GetInstance().Hide();
		LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().Show();
		LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().InitBank();
	}

	private void OnSettingAction()
	{
		LKB_MB_Singleton<LKB_OptionsController>.GetInstance().Hide();
		LKB_MB_Singleton<LKB_SettingsController>.GetInstance().Show();
	}

	private void OnSetScoreAndGoldAction()
	{
		if (goUIContainer.activeInHierarchy)
		{
			txtCredit.text = LKB_GVars.credit.ToString();
			if (LKB_GVars.credit == 0 && winScore == 0)
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
			LKB_GVars.credit = credit;
			txtCredit.text = credit.ToString();
		}
		if (realCredit >= 0)
		{
			LKB_GVars.realCredit = realCredit;
		}
		if (check && LKB_GVars.credit != LKB_GVars.realCredit)
		{
			UnityEngine.Debug.LogError($"credit: {LKB_GVars.credit}, realCredit: {LKB_GVars.realCredit}");
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

	public void SetBtnStartState(LKB_BtnState LKB_BtnState = LKB_BtnState.none)
	{
		bGainScoreBtn = false;
		if (_coroutine != null)
		{
			StopCoroutine(_coroutine);
		}
		_coroutine = StartCoroutine(WaitSetBtnState(LKB_BtnState));
	}

	private void Update()
	{
		if (bAuto && btnStopAuto.gameObject != null && !btnStopAuto.gameObject.activeInHierarchy)
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

	private IEnumerator WaitSetBtnState(LKB_BtnState LKB_BtnState = LKB_BtnState.none)
	{
		yield return new WaitForSeconds(0.1f);
		if (isFreenGame)
		{
			btnStart.interactable = false;
			btnStop.interactable = false;
			btnStop.gameObject.SetActive(value: false);
			btnAuto.interactable = false;
			btnStopAuto.interactable = false;
			btnStopAuto.gameObject.SetActive(value: false);
			yield break;
		}
		switch (LKB_BtnState)
		{
		case LKB_BtnState.start:
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
		case LKB_BtnState.stop:
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
		case LKB_BtnState.disable:
			btnStart.interactable = false;
			btnStart.gameObject.SetActive(value: true);
			btnStop.interactable = false;
			btnStop.gameObject.SetActive(value: false);
			btnStopAuto.interactable = false;
			btnStopAuto.gameObject.SetActive(value: false);
			btnAuto.interactable = false;
			btnAuto.gameObject.SetActive(value: true);
			break;
		case LKB_BtnState.reStart:
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
	}

	private void HideWinScoreTip()
	{
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
		LKB_Desk desk = LKB_GVars.desk;
		UnityEngine.Debug.Log($"ZH2_GVars.credit: {LKB_GVars.credit}, _totalStake: {totalStake},_singleStake: {singleStake},minSinglelineBet: {desk.minSinglelineBet}");
		if (LKB_GVars.credit == 0)
		{
			LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().Show();
			LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().InitBank();
			SetAndIfChangeThenPlayAutoAni(toAuto: false);
		}
		else if (LKB_GVars.credit < 9)
		{
			SetLineNum(LKB_GVars.credit);
			SetSingleStake(1);
			SetTotalStake(LKB_GVars.credit);
		}
		else if (LKB_GVars.credit < desk.minSinglelineBet * 9)
		{
			int num = LKB_GVars.credit / 9;
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
		else if (LKB_GVars.credit < lineNum * singleStake)
		{
			int num2 = LKB_GVars.credit / 9;
			SetLineNum(9);
			SetSingleStake(num2);
			SetTotalStake(num2 * 9);
		}
		else if (lineNum < 9 && LKB_GVars.credit >= 9)
		{
			SetLineNum(9);
			SetSingleStake(singleStake);
			SetTotalStake(singleStake * 9);
		}
		UnityEngine.Debug.Log($"_lineNum: {lineNum}, _totalStake: {totalStake},_singleStake: {singleStake},minSinglelineBet: {desk.minSinglelineBet}");
	}

	private void JudgeBtnMinusPlus()
	{
		bool flag = singleStake * lineNum >= LKB_GVars.desk.minSinglelineBet * 9;
	}

	private void HandleCreditChange()
	{
		int num = int.Parse(txtCredit.text);
		if (num >= LKB_GVars.credit && num > LKB_GVars.credit)
		{
			int rightGold = LKB_ScoreBank.GetRightGold();
			if (LKB_GVars.credit < totalStake && rightGold > 0)
			{
				LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().Show();
			}
		}
	}

	private void StakeRebound()
	{
		LKB_Desk desk = LKB_GVars.desk;
		if (totalStake < desk.minSinglelineBet * 9)
		{
			if (LKB_GVars.credit >= desk.minSinglelineBet * 9)
			{
				int minSinglelineBet = desk.minSinglelineBet;
				SetLineNum(9);
				SetSingleStake(minSinglelineBet);
				SetTotalStake(minSinglelineBet * 9);
			}
			else if (LKB_GVars.credit >= 9)
			{
				int num = LKB_GVars.credit / 9;
				SetLineNum(9);
				SetSingleStake(num);
				SetTotalStake(num * 9);
			}
			else if (LKB_GVars.credit > 0)
			{
				SetLineNum(LKB_GVars.credit);
				SetSingleStake(1);
				SetTotalStake(LKB_GVars.credit);
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
		return winScore < LKB_GVars.desk.diceOverflow && ((LKB_GVars.desk.diceSwitch == 1 && winScore != 0) || LKB_GVars.desk.diceDirectSwitch == 1);
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
