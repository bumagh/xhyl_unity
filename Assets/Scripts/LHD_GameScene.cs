using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LHD_GameScene : MonoBehaviour
{
	public Sprite[] CardSprites;

	public Animator LongAnime;

	public Animator HuAnime;

	public Text RoomName;

	public Text TimeText;

	public Image Head;

	public Text NickName;

	public Text Coin;

	public LongPressBtn LongClick;

	public LongPressBtn HuClick;

	public LongPressBtn HeClick;

	public Image LongClickIma;

	public Image HuClickIma;

	public Image HeClickIma;

	public List<Transform> chipPos_List;

	public Transform chipPanPos;

	public GameObject StartPos;

	public GameObject EndPos;

	public GameObject LongHuVSAnime;

	public Text Timer;

	public Button PlayerCountBtn;

	public Text PlayerCount;

	public Transform PlayerListPanel;

	public Transform ResultCount;

	public Text ResultLongText;

	public Text ResultHuText;

	public Text ResultHeText;

	public Text ResultAllText;

	public Text CountBet;

	public Text LongScore;

	public Text HuScore;

	public Text HeScore;

	public Text Inning;

	public Text Count1;

	public Text Count2;

	public GameObject LongWin;

	public GameObject HuWin;

	public GameObject HeWin;

	public Image LongCard;

	public Image HuCard;

	public GameObject BetHint;

	public GameObject Hint;

	public LongPressBtn ContinuedBtn;

	public Button AutoBtn;

	public Button LogBtn;

	public Transform LuDanBtns;

	public List<Button> LuDanBtn_List = new List<Button>();

	public Transform LuDanPanels;

	public List<GameObject> LuDanPanel_List = new List<GameObject>();

	public Button MoreBtn;

	public GameObject MorePanel;

	public Button ReturnBtn;

	public Button SetUpBtn;

	public Button RuleBtn;

	public Button RecordsBtn;

	public Button SafeBoxBtn;

	public Button PayBtn;

	public GameObject SetUpPanel;

	public GameObject RulePanel;

	public GameObject SafeBoxPanel;

	public GameObject SafeBoxPwdPanel;

	public GameObject PayPanel;

	public LHD_JettonManager Jettons;

	public Transform recordsPanel;

	public Transform SwitchBtns;

	public List<Button> SwitchBtn_List = new List<Button>();

	private int[] BetChips = new int[6]
	{
		500,
		1000,
		2000,
		5000,
		10000,
		20000
	};

	public int SwitchVakue = 100;

	public int chipId;

	private Sequence seq_reslut;

	public int[] BetGerenValue = new int[3];

	public int[] BetZongValue = new int[3];

	public static LHD_GameScene instance;

	private bool isContinuedBtn;

	private bool isAuto;

	private int hour = -1;

	private int minute = -1;

	private int tempMinute = -100;

	private string[] textVuale;

	public Transform chipStartPos_Self;

	public Transform chipStartPos_Other;

	public Sprite[] JettonSprites;

	private int[] beiLv = new int[3]
	{
		2,
		8,
		2
	};

	private bool IsClickDown;

	private bool IsClickUp;

	private float time;

	private Color black = new Color(0f, 0.3f, 0f, 1f);

	private Color red = Color.red;

	private float interval;

	private float countdownfl;

	private int countdown;

	private int showCardNum;

	public bool IsCanBet => countdown > 0;

	private void Awake()
	{
		instance = this;
		LongAnime = base.transform.Find("LongAnim").GetComponent<Animator>();
		HuAnime = base.transform.Find("HuAnim").GetComponent<Animator>();
		RoomName = base.transform.Find("RoomInfo/RoomName").GetComponent<Text>();
		TimeText = base.transform.Find("RoomInfo/Time").GetComponent<Text>();
		Head = base.transform.Find("UserInfo/Head").GetComponent<Image>();
		NickName = base.transform.Find("UserInfo/NickName").GetComponent<Text>();
		Coin = base.transform.Find("UserInfo/Coin").GetComponent<Text>();
		Timer = base.transform.Find("Table/Timer/Text").GetComponent<Text>();
		LongClick = base.transform.Find("Table/Long/Btn").GetComponent<LongPressBtn>();
		HuClick = base.transform.Find("Table/Hu/Btn").GetComponent<LongPressBtn>();
		HeClick = base.transform.Find("Table/He/Btn").GetComponent<LongPressBtn>();
		LongClickIma = base.transform.Find("Table/Long/Image").GetComponent<Image>();
		HeClickIma = base.transform.Find("Table/He/Image").GetComponent<Image>();
		HuClickIma = base.transform.Find("Table/Hu/Image").GetComponent<Image>();
		LongHuVSAnime = base.transform.Find("LongHuVSAnime").gameObject;
		recordsPanel = base.transform.Find("RecordsPanel");
		SwitchBtns = base.transform.Find("BetBtus");
		SwitchBtn_List = new List<Button>();
		for (int i = 0; i < SwitchBtns.childCount; i++)
		{
			SwitchBtn_List.Add(SwitchBtns.GetChild(i).GetComponent<Button>());
		}
		for (int j = 0; j < SwitchBtn_List.Count; j++)
		{
			int index2 = j;
			SwitchBtn_List[j].onClick.AddListener(delegate
			{
				OnClickSwitchBtn(index2);
			});
		}
		LuDanBtns = base.transform.Find("LuDanBtns");
		for (int k = 0; k < LuDanBtns.childCount; k++)
		{
			LuDanBtn_List.Add(LuDanBtns.GetChild(k).GetComponent<Button>());
		}
		for (int l = 0; l < LuDanBtn_List.Count; l++)
		{
			int index = l;
			LuDanBtn_List[l].onClick.AddListener(delegate
			{
				OnClickLuDanBtn(index);
			});
		}
		LuDanPanels = base.transform.Find("LuDanPanels");
		for (int m = 0; m < LuDanPanels.childCount; m++)
		{
			LuDanPanel_List.Add(LuDanPanels.GetChild(m).gameObject);
		}
		ContinuedBtn = base.transform.Find("ContinuedBtn").GetComponent<LongPressBtn>();
		AutoBtn = base.transform.Find("AutoBtn").GetComponent<Button>();
		LogBtn = base.transform.Find("LogBtn").GetComponent<Button>();
		MoreBtn = base.transform.Find("MoreBtn").GetComponent<Button>();
		MorePanel = base.transform.Find("MorePanel").gameObject;
		ReturnBtn = base.transform.Find("MorePanel/ReturnBtn").GetComponent<Button>();
		SetUpBtn = base.transform.Find("MorePanel/SetUpBtn").GetComponent<Button>();
		RuleBtn = base.transform.Find("MorePanel/RuleBtn").GetComponent<Button>();
		RecordsBtn = base.transform.Find("MorePanel/RecordsBtn").GetComponent<Button>();
		SafeBoxBtn = base.transform.Find("MorePanel/SafeBoxBtn").GetComponent<Button>();
		PayBtn = base.transform.Find("MorePanel/PayBtn").GetComponent<Button>();
		SetUpPanel = base.transform.Find("SetUpPanel").gameObject;
		RulePanel = base.transform.Find("HelpPanel").gameObject;
		PlayerListPanel = base.transform.Find("PlayerPanel");
		PlayerCountBtn = base.transform.Find("PlayerCount").GetComponent<Button>();
		PlayerCount = PlayerCountBtn.transform.Find("Text").GetComponent<Text>();
		ResultCount = base.transform.Find("ResultCount");
		ResultLongText = ResultCount.Find("Long").GetComponent<Text>();
		ResultHuText = ResultCount.Find("Hu").GetComponent<Text>();
		ResultHeText = ResultCount.Find("He").GetComponent<Text>();
		ResultAllText = ResultCount.Find("Count").GetComponent<Text>();
		CountBet = base.transform.Find("CountBet").GetComponent<Text>();
		LongScore = base.transform.Find("LongScore").GetComponent<Text>();
		HuScore = base.transform.Find("HuScore").GetComponent<Text>();
		HeScore = base.transform.Find("HeScore").GetComponent<Text>();
		LongCard = base.transform.Find("LongCard").GetComponent<Image>();
		HuCard = base.transform.Find("HuCard").GetComponent<Image>();
		Inning = base.transform.Find("Inning").GetComponent<Text>();
		Count1 = base.transform.Find("Count1").GetComponent<Text>();
		Count2 = base.transform.Find("Count2").GetComponent<Text>();
		LongWin = base.transform.Find("LongWin").gameObject;
		HuWin = base.transform.Find("HuWin").gameObject;
		HeWin = base.transform.Find("HeWin").gameObject;
		SetDoFid();
		Jettons = base.transform.Find("Jettons").GetComponent<LHD_JettonManager>();
		BetHint = base.transform.Find("BetHint").gameObject;
		Hint = base.transform.Find("Hint").gameObject;
		Inittween();
	}

	private void Inittween()
	{
		seq_reslut = DOTween.Sequence();
		seq_reslut.Pause();
		seq_reslut.SetAutoKill(autoKillOnCompletion: false);
		seq_reslut.AppendCallback(delegate
		{
			ShowBetHit(isShow: true, isStart: false);
		});
		seq_reslut.AppendInterval(3f);
		seq_reslut.AppendCallback(ShowCard);
		seq_reslut.AppendInterval(0.5f);
		seq_reslut.AppendCallback(ShowCard);
		seq_reslut.AppendInterval(1f);
		seq_reslut.AppendCallback(ShowWin);
		seq_reslut.AppendInterval(0.5f);
		seq_reslut.AppendCallback(ShowChipPlay);
	}

	private void OnEnable()
	{
		AddAction();
		countdownfl = 0f;
		ResetWinTag();
		BetHint.transform.localScale = Vector3.zero;
		BetHint.SetActive(value: true);
		LongHuVSAnime.SetActive(value: false);
		if (RoomName != null)
		{
			RoomName.text = LHD_GameInfo.getInstance().roominfo.name;
		}
		if (NickName != null)
		{
			NickName.text = ZH2_GVars.GetBreviaryName(LHD_GameInfo.getInstance().userinfo.nickname);
		}
		ShowHint(isShow: false);
		MorePanel.SetActive(value: false);
		recordsPanel.gameObject.SetActive(value: false);
		OnClickSwitchBtn(0);
		OnClickLuDanBtn(0);
		SetResultText();
		countdown = 0;
		countdown = LHD_GameInfo.getInstance().roominfo.havTime;
		LHD_GameInfo.getInstance().updateRoomList?.Invoke(ZH2_GVars.hallInfo2);
		UnityEngine.Debug.LogError("剩余押注时间: " + countdown);
		if (countdown < 1)
		{
			Timer.text = countdown.ToString("00");
		}
		UpDateScore(LHD_GameInfo.getInstance().GameScore);
	}

	private void OnDisable()
	{
		ReAction();
		if (Timer != null)
		{
			Timer.text = string.Empty;
		}
	}

	private void AddAction()
	{
		LHD_GameInfo lHD_GameInfo = LHD_GameInfo.getInstance();
		lHD_GameInfo.upDateScore = (Action<int>)Delegate.Combine(lHD_GameInfo.upDateScore, new Action<int>(UpDateScore));
		LHD_GameInfo lHD_GameInfo2 = LHD_GameInfo.getInstance();
		lHD_GameInfo2.resultListCall = (Action<JsonData>)Delegate.Combine(lHD_GameInfo2.resultListCall, new Action<JsonData>(ResultListCall));
		LHD_GameInfo lHD_GameInfo3 = LHD_GameInfo.getInstance();
		lHD_GameInfo3.updateRoomList = (Action<JsonData>)Delegate.Combine(lHD_GameInfo3.updateRoomList, new Action<JsonData>(UpdateRoomList));
		ZH2_GVars.saveScore = (Action)Delegate.Combine(ZH2_GVars.saveScore, new Action(SaveScore));
		ZH2_GVars.closeSafeBox = (Action)Delegate.Combine(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
	}

	private void ReAction()
	{
		LHD_GameInfo lHD_GameInfo = LHD_GameInfo.getInstance();
		lHD_GameInfo.upDateScore = (Action<int>)Delegate.Remove(lHD_GameInfo.upDateScore, new Action<int>(UpDateScore));
		LHD_GameInfo lHD_GameInfo2 = LHD_GameInfo.getInstance();
		lHD_GameInfo2.resultListCall = (Action<JsonData>)Delegate.Remove(lHD_GameInfo2.resultListCall, new Action<JsonData>(ResultListCall));
		LHD_GameInfo lHD_GameInfo3 = LHD_GameInfo.getInstance();
		lHD_GameInfo3.updateRoomList = (Action<JsonData>)Delegate.Remove(lHD_GameInfo3.updateRoomList, new Action<JsonData>(UpdateRoomList));
		ZH2_GVars.saveScore = (Action)Delegate.Remove(ZH2_GVars.saveScore, new Action(SaveScore));
		ZH2_GVars.closeSafeBox = (Action)Delegate.Remove(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
	}

	private void SaveScore()
	{
		LHD_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinOut();
	}

	private void CloseSafeBox()
	{
		LHD_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinIn();
	}

	private void UpdateRoomList(JsonData jd)
	{
		int id = LHD_GameInfo.getInstance().roominfo.id;
		LHD_GameInfo.getInstance().roomlist.Clear();
		for (int i = 0; i < jd.Count; i++)
		{
			LHD_RoomInfo lHD_RoomInfo = new LHD_RoomInfo();
			lHD_RoomInfo.id = int.Parse(jd[(i + 1).ToString()]["id"].ToString());
			lHD_RoomInfo.roomId = int.Parse(jd[(i + 1).ToString()]["roomId"].ToString());
			lHD_RoomInfo.name = jd[(i + 1).ToString()]["name"].ToString();
			lHD_RoomInfo.orderBy = int.Parse(jd[(i + 1).ToString()]["orderBy"].ToString());
			lHD_RoomInfo.minGold = int.Parse(jd[(i + 1).ToString()]["minGold"].ToString());
			lHD_RoomInfo.minBet = int.Parse(jd[(i + 1).ToString()]["minBet"].ToString());
			lHD_RoomInfo.maxBet = int.Parse(jd[(i + 1).ToString()]["maxBet"].ToString());
			lHD_RoomInfo.tieMaxBet = int.Parse(jd[(i + 1).ToString()]["tieMaxBet"].ToString());
			lHD_RoomInfo.exchange = int.Parse(jd[(i + 1).ToString()]["exchange"].ToString());
			lHD_RoomInfo.onceExchangeValue = int.Parse(jd[(i + 1).ToString()]["onceExchangeValue"].ToString());
			lHD_RoomInfo.betTime = int.Parse(jd[(i + 1).ToString()]["betTime"].ToString());
			lHD_RoomInfo.onlineNumber = int.Parse(jd[(i + 1).ToString()]["onlineNumber"].ToString());
			LHD_RoomInfo item = lHD_RoomInfo;
			LHD_GameInfo.getInstance().roomlist.Add(item);
		}
		for (int j = 0; j < LHD_GameInfo.getInstance().roomlist.Count; j++)
		{
			if (LHD_GameInfo.getInstance().roomlist[j].id == id)
			{
				LHD_GameInfo.getInstance().roominfo = LHD_GameInfo.getInstance().roomlist[j];
				break;
			}
		}
		SetPlayerCount();
	}

	private void SetPlayerCount()
	{
		UnityEngine.Debug.LogError("===当前房: " + JsonMapper.ToJson(LHD_GameInfo.getInstance().roominfo));
		PlayerCount.text = LHD_GameInfo.getInstance().roominfo.onlineNumber.ToString("00");
	}

	private void Start()
	{
		LongClick.onClick.AddListener(delegate
		{
			if (IsCanBet)
			{
				OnClickBet(0);
			}
		});
		HeClick.onClick.AddListener(delegate
		{
			if (IsCanBet)
			{
				OnClickBet(1);
			}
		});
		HuClick.onClick.AddListener(delegate
		{
			if (IsCanBet)
			{
				OnClickBet(2);
			}
		});
		ContinuedBtn.onClick.AddListener(delegate
		{
			OnClickContinuedBtnLong();
		});
		LongClick.onClickDown.AddListener(delegate
		{
			OnClickBet(0);
		});
		HeClick.onClickDown.AddListener(delegate
		{
			OnClickBet(1);
		});
		HuClick.onClickDown.AddListener(delegate
		{
			OnClickBet(2);
		});
		PlayerCountBtn.onClick.AddListener(delegate
		{
			PlayerListPanel.gameObject.SetActive(value: true);
		});
		ContinuedBtn.onClickDown.AddListener(delegate
		{
			OnClickContinuedBtnDown();
		});
		ContinuedBtn.onClickUp.AddListener(delegate
		{
			OnClickContinuedBtnUp();
		});
		AutoBtn.onClick.AddListener(delegate
		{
			isAuto = false;
			AutoBtn.gameObject.SetActive(value: false);
		});
		LogBtn.onClick.AddListener(delegate
		{
			recordsPanel.gameObject.SetActive(value: true);
		});
		MoreBtn.onClick.AddListener(delegate
		{
			if (MorePanel.activeSelf)
			{
				MorePanel.SetActive(value: false);
			}
			else
			{
				MorePanel.SetActive(value: true);
			}
		});
		ReturnBtn.onClick.AddListener(delegate
		{
			LHD_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(LHD_GameInfo.getInstance().roominfo.id, 0);
			LHD_UIManager.instance.ShowHall();
		});
		SetUpBtn.onClick.AddListener(delegate
		{
			SetUpPanel.SetActive(value: true);
		});
		RuleBtn.onClick.AddListener(delegate
		{
			RulePanel.SetActive(value: true);
		});
		RecordsBtn.onClick.AddListener(delegate
		{
			recordsPanel.gameObject.SetActive(value: true);
		});
		SafeBoxBtn.onClick.AddListener(delegate
		{
			if (ZH2_GVars.OpenCheckSafeBoxPwdPanel != null)
			{
				ZH2_GVars.OpenCheckSafeBoxPwdPanel(ZH2_GVars.GameType_DJ.lhd_desk);
			}
		});
		PayBtn.onClick.AddListener(delegate
		{
			if (ZH2_GVars.OpenPlyBoxPanel != null)
			{
				ZH2_GVars.OpenPlyBoxPanel(ZH2_GVars.GameType_DJ.lhd_desk);
			}
		});
	}

	private void Update()
	{
		UpdateTime();
	}

	public void GetLuDan()
	{
		if (LHD_NetMngr.GetSingleton() != null)
		{
			LHD_NetMngr.GetSingleton().MyCreateSocket.SendLuDan();
		}
	}

	private void UpdateTime()
	{
		hour = DateTime.Now.Hour;
		minute = DateTime.Now.Minute;
		if (TimeText != null && tempMinute != minute)
		{
			tempMinute = minute;
			TimeText.text = string.Format("{0}:{1}", hour.ToString("00"), minute.ToString("00"));
		}
	}

	private void UpDateScore(int score)
	{
		if (Coin != null)
		{
			Coin.text = score.ToString();
		}
	}

	private void ResultListCall(JsonData jd)
	{
		JsonData jd2 = null;
		for (int i = 0; i < jd.Count; i++)
		{
			if (jd[i]["deskId"].ToString() == LHD_GameInfo.getInstance().roominfo.id.ToString())
			{
				jd2 = jd[i]["recordList"];
				break;
			}
		}
		LHD_LuDanManager.instance.GetLuDanInfo(jd2);
		SetResultText();
	}

	public void SetBetChip(int[] betChip)
	{
		textVuale = new string[betChip.Length];
		BetChips = betChip;
		OnClickSwitchBtn(0);
		for (int i = 0; i < SwitchBtn_List.Count; i++)
		{
			Text component = SwitchBtn_List[i].transform.Find("Text").GetComponent<Text>();
			component.text = GetChipName(BetChips[i]);
			textVuale[i] = component.text;
		}
	}

	private string GetChipName(int num)
	{
		string empty = string.Empty;
		if (num.ToString().Length <= 3)
		{
			return num.ToString();
		}
		if (num.ToString().Length == 4)
		{
			num /= 1000;
			return num + "千";
		}
		num /= 10000;
		return num + "万";
	}

	private void OnClickSwitchBtn(int index)
	{
		chipId = index;
		SwitchVakue = BetChips[index];
		for (int i = 0; i < SwitchBtn_List.Count; i++)
		{
			SwitchBtn_List[i].transform.localScale = Vector3.one;
			SwitchBtn_List[i].transform.Find("Image").gameObject.SetActive(value: false);
		}
		SwitchBtn_List[index].transform.localScale = Vector3.one * 1.15f;
		SwitchBtn_List[index].transform.Find("Image").gameObject.SetActive(value: true);
	}

	private void OnClickLuDanBtn(int index)
	{
		for (int i = 0; i < LuDanBtn_List.Count; i++)
		{
			LuDanBtn_List[i].transform.Find("Image").gameObject.SetActive(value: false);
		}
		LuDanBtn_List[index].transform.Find("Image").gameObject.SetActive(value: true);
		for (int j = 0; j < LuDanPanel_List.Count; j++)
		{
			LuDanPanel_List[j].SetActive(value: false);
		}
		LuDanPanel_List[index].SetActive(value: true);
		GetLuDan();
	}

	private void SetResultText()
	{
		ResultLongText.text = LHD_LuDanManager.instance.longNum.ToString("00");
		ResultHuText.text = LHD_LuDanManager.instance.huNum.ToString("00");
		ResultHeText.text = LHD_LuDanManager.instance.heNum.ToString("00");
		int num = LHD_LuDanManager.instance.longNum + LHD_LuDanManager.instance.huNum + LHD_LuDanManager.instance.heNum;
		ResultAllText.text = num.ToString("00");
	}

	private void OnClickBet(int index)
	{
		if (!IsCanBet)
		{
			UnityEngine.Debug.LogError("====停止下注====");
			All_GameMiniTipPanel.publicMiniTip.ShowTip("请在倒计时下注");
			return;
		}
		Vector2 vector = new Vector2(0f, 0f);
		vector = GetChipPos(index);
		LHD_UserBet lHD_UserBet = new LHD_UserBet();
		lHD_UserBet.chipId = chipId;
		lHD_UserBet.x = vector.x;
		lHD_UserBet.y = vector.y;
		lHD_UserBet.space = index;
		lHD_UserBet.userId = ZH2_GVars.userId;
		LHD_UserBet obj = lHD_UserBet;
		string betInfo = JsonUtility.ToJson(obj);
		LHD_NetMngr.GetSingleton().MyCreateSocket.SendUserBet(betInfo, index, SwitchVakue, LHD_GameInfo.getInstance().roominfo.id);
	}

	private void OnClickContinuedBtnDown()
	{
		UnityEngine.Debug.LogError("======按下======");
		isContinuedBtn = true;
		LHD_NetMngr.GetSingleton().MyCreateSocket.SenContinueBet(ZH2_GVars.userId, LHD_GameInfo.getInstance().roominfo.id);
	}

	private void OnClickContinuedBtnUp()
	{
		UnityEngine.Debug.LogError("======抬起======");
		isContinuedBtn = false;
	}

	private void OnClickContinuedBtnLong()
	{
		if (!isAuto)
		{
			if (!isAuto)
			{
				isAuto = true;
			}
			AutoBtn.gameObject.SetActive(value: true);
			UnityEngine.Debug.LogError("======长按======");
		}
	}

	public void GetChip(JsonData jd)
	{
		if (jd == null || jd.Count <= 0)
		{
			UnityEngine.Debug.LogError("====jd为空====");
			return;
		}
		int num = (int)jd["userId"];
		bool isSelf = num == ZH2_GVars.userId;
		float x = float.Parse(jd["x"].ToString());
		float y = float.Parse(jd["y"].ToString());
		int btnIndex = (int)jd["space"];
		int num2 = (int)jd["chipId"];
		Vector2 pos = new Vector2(x, y);
		GetChipObj(pos, btnIndex, isSelf, num2);
	}

	public void GetChipObj(Vector2 pos, int btnIndex, bool isSelf, int chipId)
	{
		GameObject gameObject = null;
		gameObject = Jettons.GetUseJetton(chipPos_List[btnIndex]);
		if (gameObject == null)
		{
			UnityEngine.Debug.LogError("====筹码创建失败====");
			return;
		}
		gameObject.SetActive(value: true);
		gameObject.transform.position = ((!isSelf) ? chipStartPos_Other.position : chipStartPos_Self.position);
		gameObject.transform.GetComponent<Image>().sprite = JettonSprites[chipId];
		gameObject.transform.Find("Text").GetComponent<Text>().text = textVuale[chipId];
		gameObject.name = ((!isSelf) ? ("1_" + chipId) : ("0_" + chipId));
		gameObject.transform.DOLocalMove(pos, 0.5f);
	}

	private Vector2 GetChipPos(int btnIndex)
	{
		Vector2 result = new Vector2(0f, 0f);
		Vector3 position = chipPos_List[btnIndex].position;
		float x = position.x + UnityEngine.Random.Range(-120f, 120f);
		Vector3 position2 = chipPos_List[btnIndex].position;
		result = new Vector2(x, position2.y + UnityEngine.Random.Range(-70f, 70f));
		return result;
	}

	public void ShowChipOver(int winCarIndex)
	{
		StartCoroutine(WaitShowChipOver(winCarIndex));
	}

	private IEnumerator WaitShowChipOver(int winCarIndex)
	{
		beiLv = new int[3]
		{
			2,
			8,
			2
		};
		yield return new WaitForSeconds(1f);
		List<Transform> selfWin = new List<Transform>();
		List<Transform> selfLose = new List<Transform>();
		List<Transform> otherWin = new List<Transform>();
		List<Transform> otherLose = new List<Transform>();
		List<Transform> selfWin_Get = new List<Transform>();
		List<Transform> otherWin_Get = new List<Transform>();
		for (int k = 0; k < chipPos_List.Count; k++)
		{
			if (k == winCarIndex)
			{
				for (int l = 0; l < chipPos_List[k].childCount; l++)
				{
					if (chipPos_List[k].GetChild(l).name.StartsWith("0"))
					{
						selfWin.Add(chipPos_List[k].GetChild(l));
					}
					else
					{
						otherWin.Add(chipPos_List[k].GetChild(l));
					}
				}
				continue;
			}
			for (int m = 0; m < chipPos_List[k].childCount; m++)
			{
				if (chipPos_List[k].GetChild(m).name.StartsWith("0"))
				{
					selfLose.Add(chipPos_List[k].GetChild(m));
				}
				else
				{
					otherLose.Add(chipPos_List[k].GetChild(m));
				}
			}
		}
		yield return new WaitForSeconds(1f);
		if (chipPanPos == null)
		{
			UnityEngine.Debug.LogError("========chipPanPos为空");
		}
		int i;
		for (i = 0; i < selfLose.Count; i++)
		{
			if (selfLose[i] != null && chipPanPos != null)
			{
				selfLose[i].SetParent(chipPanPos);
				selfLose[i].DOLocalMove(chipPanPos.localPosition, 1.25f).OnComplete(delegate
				{
					Jettons.ReMoveUseJetton(selfLose[i].gameObject);
				});
			}
		}
		int j;
		for (j = 0; j < otherLose.Count; j++)
		{
			if (otherLose[j] != null && chipPanPos != null)
			{
				otherLose[j].SetParent(chipPanPos);
				otherLose[j].DOLocalMove(chipPanPos.localPosition, 1.25f).OnComplete(delegate
				{
					Jettons.ReMoveUseJetton(otherLose[j].gameObject);
				});
			}
		}
		for (int n = 0; n < selfWin.Count; n++)
		{
			int num = int.Parse(selfWin[n].name.Replace("0_", string.Empty));
			for (int num2 = 0; num2 < beiLv[winCarIndex] - 1; num2++)
			{
				GameObject useJetton = Jettons.GetUseJetton(chipPanPos);
				useJetton.transform.GetComponent<Image>().sprite = JettonSprites[num];
				useJetton.transform.Find("Text").GetComponent<Text>().text = textVuale[num];
				useJetton.transform.localPosition = chipPanPos.localPosition;
				selfWin_Get.Add(useJetton.transform);
			}
		}
		for (int num3 = 0; num3 < otherWin.Count; num3++)
		{
			int num4 = int.Parse(otherWin[num3].name.Replace("1_", string.Empty));
			for (int num5 = 0; num5 < beiLv[winCarIndex]; num5++)
			{
				GameObject useJetton2 = Jettons.GetUseJetton(chipPanPos);
				useJetton2.transform.GetComponent<Image>().sprite = JettonSprites[num4];
				useJetton2.transform.Find("Text").GetComponent<Text>().text = textVuale[num4];
				useJetton2.transform.localPosition = chipPanPos.localPosition;
				otherWin_Get.Add(useJetton2.transform);
			}
		}
		yield return new WaitForSeconds(1.3f);
		for (int num6 = 0; num6 < selfLose.Count; num6++)
		{
			if (selfLose[num6] != null && selfLose[num6].gameObject != null)
			{
				Jettons.ReMoveUseJetton(selfLose[num6].gameObject);
			}
		}
		for (int num7 = 0; num7 < otherLose.Count; num7++)
		{
			if (otherLose[num7] != null && otherLose[num7].gameObject != null)
			{
				Jettons.ReMoveUseJetton(otherLose[num7].gameObject);
			}
		}
		yield return new WaitForSeconds(0.2f);
		for (int num8 = 0; num8 < selfWin_Get.Count; num8++)
		{
			selfWin_Get[num8].SetParent(chipPos_List[winCarIndex]);
			selfWin_Get[num8].DOLocalMove(GetChipPos(winCarIndex), 0.65f);
		}
		for (int num9 = 0; num9 < otherWin_Get.Count; num9++)
		{
			otherWin_Get[num9].SetParent(chipPos_List[winCarIndex]);
			otherWin_Get[num9].DOLocalMove(GetChipPos(winCarIndex), 0.65f);
		}
		yield return new WaitForSeconds(1.2f);
		for (int num10 = 0; num10 < selfWin.Count; num10++)
		{
			selfWin[num10].SetParent(chipStartPos_Self);
			selfWin[num10].DOLocalMove(chipStartPos_Self.position, 0.5f);
		}
		for (int num11 = 0; num11 < selfWin_Get.Count; num11++)
		{
			selfWin_Get[num11].SetParent(chipStartPos_Self);
			selfWin_Get[num11].DOLocalMove(chipStartPos_Self.position, 0.5f);
		}
		for (int num12 = 0; num12 < otherWin.Count; num12++)
		{
			otherWin[num12].SetParent(chipStartPos_Other);
			otherWin[num12].DOLocalMove(chipStartPos_Other.position, 0.5f);
		}
		for (int num13 = 0; num13 < otherWin_Get.Count; num13++)
		{
			otherWin_Get[num13].SetParent(chipStartPos_Other);
			otherWin_Get[num13].DOLocalMove(chipStartPos_Other.position, 0.5f);
		}
		yield return new WaitForSeconds(0.6f);
		for (int num14 = 0; num14 < chipStartPos_Other.childCount; num14++)
		{
			Jettons.ReMoveUseJetton(chipStartPos_Other.GetChild(num14).gameObject);
		}
		for (int num15 = 0; num15 < chipStartPos_Self.childCount; num15++)
		{
			Jettons.ReMoveUseJetton(chipStartPos_Self.GetChild(num15).gameObject);
		}
	}

	private IEnumerator OnPressClick()
	{
		while (true)
		{
			IsClickDown = false;
		}
	}

	private void FixedUpdate()
	{
		if (countdown <= 0)
		{
			return;
		}
		countdownfl += Time.fixedDeltaTime;
		if (!(countdownfl >= 1f))
		{
			return;
		}
		countdownfl = 0f;
		countdown--;
		if (countdown <= 0)
		{
			countdown = 0;
			Timer.text = countdown.ToString("00");
			return;
		}
		Timer.text = countdown.ToString("00");
		if (countdown <= 3)
		{
			Timer.color = red;
		}
		else
		{
			Timer.color = black;
		}
	}

	public void ShowHint(bool isShow, int state = 0)
	{
		if (!isShow)
		{
			Hint.SetActive(value: false);
			return;
		}
		Hint.SetActive(value: true);
		for (int i = 0; i < Hint.transform.childCount; i++)
		{
			Hint.transform.GetChild(i).gameObject.SetActive(value: false);
		}
		switch (state)
		{
		case 0:
			Hint.transform.Find("Deal").gameObject.SetActive(value: true);
			break;
		case 1:
			Hint.transform.Find("Bet").gameObject.SetActive(value: true);
			break;
		case 2:
			Hint.transform.Find("Payout").gameObject.SetActive(value: true);
			break;
		case 3:
			Hint.transform.Find("Open").gameObject.SetActive(value: true);
			break;
		case 4:
			Hint.transform.Find("Shuffle").gameObject.SetActive(value: false);
			break;
		}
	}

	public void UpdateLuDan()
	{
	}

	public void UpdateJetton()
	{
	}

	public void GameInfo(int inningNum)
	{
		Inning.text = $"第{inningNum}局";
		Count1.text = (inningNum * 2).ToString();
		Count2.text = (416 - inningNum * 2).ToString();
	}

	public void SetAllBet(int bet)
	{
		CountBet.text = $"本局下注 \n {bet}/999999";
	}

	public void Restart(int inningNum, int betTime)
	{
		Jettons.RecycleJetton();
		SetAllBet(0);
		LongScore.text = "0/0";
		HuScore.text = "0/0";
		HeScore.text = "0/0";
		ResetWinTag();
		LongHuVSAnime.SetActive(value: true);
		LongCard.sprite = CardSprites[0];
		HuCard.sprite = CardSprites[0];
		LongClickIma.gameObject.SetActive(value: false);
		HeClickIma.gameObject.SetActive(value: false);
		HuClickIma.gameObject.SetActive(value: false);
		ShowBetHit();
		LHD_AudioManger.instance.PlayAudio(LHD_AudioManger.AudioType.StartBet);
		ShowHint(isShow: true, 1);
		countdown = betTime;
		countdownfl = 0f;
		GameInfo(inningNum);
		if (isAuto)
		{
			OnClickContinuedBtnDown();
		}
		GetLuDan();
	}

	private void ResetWinTag()
	{
		LongWin.SetActive(value: false);
		HuWin.SetActive(value: false);
		HeWin.SetActive(value: false);
	}

	private void ShowBetHit(bool isShow = true, bool isStart = true)
	{
		BetHint.transform.DOScale(Vector3.one * (isShow ? 1 : 0), 0.5f).OnComplete(delegate
		{
			if (isShow)
			{
				BetHint.transform.DOScale(Vector3.one, 1f).OnComplete(delegate
				{
					BetHint.transform.DOScale(Vector3.zero, 0.5f);
				});
			}
		});
		BetHint.transform.GetChild(0).gameObject.SetActive(isStart);
		BetHint.transform.GetChild(1).gameObject.SetActive(!isStart);
	}

	public void Result()
	{
		ShowHint(isShow: true, 3);
		seq_reslut.Restart();
	}

	private void ShowCard()
	{
		if (showCardNum == 0)
		{
			Sequence s = DOTween.Sequence();
			s.Append(LongCard.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), 0.4f));
			s.AppendCallback(delegate
			{
				LongCard.sprite = Getsprite();
			});
			s.Append(LongCard.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.4f));
		}
		else
		{
			Sequence s2 = DOTween.Sequence();
			s2.Append(HuCard.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), 0.4f));
			s2.AppendCallback(delegate
			{
				HuCard.sprite = Getsprite();
			});
			s2.Append(HuCard.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.4f));
		}
		LHD_AudioManger.instance.PlayAudio(LHD_AudioManger.AudioType.CardShow);
	}

	private void ShowWin()
	{
		LongWin.SetActive(value: false);
		HuWin.SetActive(value: false);
		HeWin.SetActive(value: false);
		switch (LHD_GameInfo.getInstance().winType)
		{
		case LHD_GameInfo.WinType.dragonPoker:
			LongWin.SetActive(value: true);
			LongAnime.SetTrigger("Fire");
			LongClickIma.gameObject.SetActive(value: true);
			break;
		case LHD_GameInfo.WinType.peace:
			HeWin.SetActive(value: true);
			LongAnime.SetTrigger("Fire");
			HuAnime.SetTrigger("Fire");
			HeClickIma.gameObject.SetActive(value: true);
			break;
		case LHD_GameInfo.WinType.tigerPoker:
			HuWin.SetActive(value: true);
			HuAnime.SetTrigger("Fire");
			HuClickIma.gameObject.SetActive(value: true);
			break;
		}
		ShowHint(isShow: true, 2);
		GetLuDan();
	}

	private void SetDoFid()
	{
		Image component = LongWin.transform.Find("LongWin").GetComponent<Image>();
		SetFade(component);
		Image component2 = HeWin.transform.Find("HeWin").GetComponent<Image>();
		SetFade(component2);
		Image component3 = HuWin.transform.Find("HuWin").GetComponent<Image>();
		SetFade(component3);
		SetFade(LongClickIma);
		SetFade(HeClickIma);
		SetFade(HuClickIma);
	}

	private void SetFade(Image image)
	{
		image.DOFade(0.2f, 1f).SetLoops(-1).SetEase(Ease.InOutCubic);
	}

	private void ShowChipPlay()
	{
		switch (LHD_GameInfo.getInstance().winType)
		{
		case LHD_GameInfo.WinType.dragonPoker:
			LHD_AudioManger.instance.PlayAudio(LHD_AudioManger.AudioType.LongWin);
			break;
		case LHD_GameInfo.WinType.peace:
			LHD_AudioManger.instance.PlayAudio(LHD_AudioManger.AudioType.HeWin);
			break;
		case LHD_GameInfo.WinType.tigerPoker:
			LHD_AudioManger.instance.PlayAudio(LHD_AudioManger.AudioType.HuWin);
			break;
		}
		ShowChipOver((int)LHD_GameInfo.getInstance().winType);
	}

	private Sprite Getsprite()
	{
		Sprite result = null;
		if (showCardNum == 1)
		{
			int tigerPoker = LHD_GameInfo.getInstance().tigerPoker;
			if (tigerPoker < CardSprites.Length)
			{
				result = CardSprites[tigerPoker];
			}
		}
		else
		{
			int dragonPoker = LHD_GameInfo.getInstance().dragonPoker;
			if (dragonPoker < CardSprites.Length)
			{
				result = CardSprites[dragonPoker];
			}
		}
		showCardNum = (showCardNum + 1) % 2;
		return result;
	}

	public void ShowTotalBet()
	{
		LongScore.text = BetGerenValue[0] + "/" + BetZongValue[0];
		HeScore.text = BetGerenValue[1] + "/" + BetZongValue[1];
		HuScore.text = BetGerenValue[2] + "/" + BetZongValue[2];
	}

	public void SpaceBet(int id, int BetNum)
	{
		switch (id)
		{
		}
	}
}
