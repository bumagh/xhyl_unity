using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hfh_GameManager : Hfh_Singleton<Hfh_GameManager>
{
	public Sprite CardBack;

	public Sprite[] CardSprites;

	public Sprite[] ResultSprites;

	private Button SideBarBtn;

	private GameObject SideBar;

	private GameObject SideBar2;

	private Button PickUpBtn;

	private Button StopBtn;

	private Button QueryBtn;

	private Button ReturnBtn;

	private Button PickUpBtn2;

	private Button BanBiBtn;

	private Button BiBeiBtn;

	private Button ShuangBiBtn;

	private Button SetBtn;

	private EventTrigger StartBtn;

	private Button CancelBtn;

	private Button ScoreBtn;

	private Button BetBtn;

	private GameObject WaitAnime;

	private GameObject GameInfo;

	private GameObject CardsPanel;

	private Text CreditText;

	private Text BetText;

	private Text WinText;

	private GameObject BiBeiInfo;

	private Text BB_CreditText;

	private Text BB_BetText;

	private Text BB_WinText;

	private Text BB_BiBeiBaoJi;

	private Text BB_BaoJiGold;

	private Text BB_GuoGuanSongGold;

	private Text BB_GuoGuanMax;

	private GameObject BiBeiPanel;

	private Button BB_da;

	private Button BB_xiao;

	private Image[] BB_LastCard;

	private Image BB_Card;

	private Image BB_Result;

	private Text HintText;

	private Text BetHint;

	private GameObject[] TextGroup;

	private Text[] ScoreGroup;

	private GameObject[] SparkletGroup;

	private GameObject Sparklet;

	private Button ClickPanel;

	private GameObject StopPanel;

	private GameObject PickUpPanel;

	private GameObject QueryPanel;

	private GameObject SetPanel;

	private GameObject BaoXiang;

	private Coroutine _coDrawCard;

	private Coroutine _coShowEffect;

	private Coroutine _coBiBeiResult;

	private Coroutine _coScoreEffect;

	private Coroutine _coAutoGame;

	public int BetValue;

	public int PickUpNum;

	public int PickUpScore;

	public int LockNum = 1;

	public bool IsStart;

	public bool IsDraw;

	public bool IsWin;

	public bool IsAuto;

	public bool IsgetWinScore;

	public bool IsOncCards;

	public bool IsTwoCards;

	public bool IsGetOneCards;

	public bool IsGetTwoCards;

	public bool IsStartBtnClick;

	private float StartBtnTime;

	public int[] MyCards = new int[5];

	public int CardTypeNum;

	public int userWin;

	public int userVariate;

	private int[] RateGroup = new int[12];

	public float FlickerTime;

	public int HeartNum;

	public int ZuanShiNum;

	public int WinPos = -1;

	public float WinTime;

	public int DrawSpeed = 5;

	public bool IsEndMultiple;

	private object[] historyCard;

	private object[] historyMultiple;

	private Dictionary<string, object> historyReward;

	public int Page;

	private Button CardRecordsBtn;

	private Button GameRecordsBtn;

	private Button JiangRecordsBtn;

	private Button TurntableBtn;

	private GameObject CardRecordsPanel;

	private GameObject GameRecordsPanel;

	private GameObject JiangRecordsPanel;

	private GameObject TurntablePanel;

	public Hfh_LoopScrollView _loopScrollView;

	public GameObject _Content;

	private bool IsFrist = true;

	private bool IsSwitch;

	private int SwitchNum;

	private bool ISDrawStop;

	private void Awake()
	{
		Hfh_Singleton<Hfh_GameManager>.SetInstance(this);
		SideBarBtn = base.transform.Find("SideBarBtn").GetComponent<Button>();
		SideBar = base.transform.Find("SideBar").gameObject;
		SideBar2 = base.transform.Find("SideBar2").gameObject;
		GameInfo = base.transform.Find("GameInfo").gameObject;
		CreditText = base.transform.Find("GameInfo/Credit").GetComponent<Text>();
		BetText = base.transform.Find("GameInfo/Bet").GetComponent<Text>();
		WinText = base.transform.Find("GameInfo/Win").GetComponent<Text>();
		BiBeiInfo = base.transform.Find("BiBeiInfo").gameObject;
		BB_CreditText = base.transform.Find("BiBeiInfo/Credit").GetComponent<Text>();
		BB_BetText = base.transform.Find("BiBeiInfo/Bet").GetComponent<Text>();
		BB_WinText = base.transform.Find("BiBeiInfo/Win").GetComponent<Text>();
		BB_BiBeiBaoJi = base.transform.Find("BiBeiInfo/BiBeiBaoJi").GetComponent<Text>();
		BB_BaoJiGold = base.transform.Find("BiBeiInfo/BaoJiGold").GetComponent<Text>();
		BB_GuoGuanSongGold = base.transform.Find("BiBeiInfo/GuoGuanSongGold").GetComponent<Text>();
		BB_GuoGuanMax = base.transform.Find("BiBeiInfo/GuoGuanMax").GetComponent<Text>();
		BB_LastCard = new Image[base.transform.Find("BiBei/LastCard").childCount];
		for (int i = 0; i < BB_LastCard.Length; i++)
		{
			BB_LastCard[i] = base.transform.Find("BiBei/LastCard").GetChild(i).GetComponent<Image>();
		}
		StartBtn = base.transform.Find("StartBtn").GetComponent<EventTrigger>();
		CancelBtn = base.transform.Find("CancelBtn").GetComponent<Button>();
		ScoreBtn = base.transform.Find("ScoreBtn").GetComponent<Button>();
		BetBtn = base.transform.Find("BetBtn").GetComponent<Button>();
		WaitAnime = base.transform.Find("WaitAnime").gameObject;
		CardsPanel = base.transform.Find("Cards").gameObject;
		BiBeiPanel = base.transform.Find("BiBei").gameObject;
		BB_da = base.transform.Find("BiBei/da").GetComponent<Button>();
		BB_xiao = base.transform.Find("BiBei/xiao").GetComponent<Button>();
		BB_Card = base.transform.Find("BiBei/Card").GetComponent<Image>();
		BB_Result = base.transform.Find("BiBei/Result").GetComponent<Image>();
		PickUpBtn = base.transform.Find("SideBar/PickUpBtn").GetComponent<Button>();
		StopBtn = base.transform.Find("SideBar/StopBtn").GetComponent<Button>();
		QueryBtn = base.transform.Find("SideBar/QueryBtn").GetComponent<Button>();
		ReturnBtn = base.transform.Find("SideBar/ReturnBtn").GetComponent<Button>();
		PickUpBtn2 = base.transform.Find("SideBar2/PickUpBtn2").GetComponent<Button>();
		BanBiBtn = base.transform.Find("SideBar2/BanBiBtn").GetComponent<Button>();
		BiBeiBtn = base.transform.Find("SideBar2/BiBeiBtn").GetComponent<Button>();
		ShuangBiBtn = base.transform.Find("SideBar2/ShuangBiBtn").GetComponent<Button>();
		ClickPanel = base.transform.Find("ClickPanel").GetComponent<Button>();
		StopPanel = base.transform.Find("StopPanel").gameObject;
		PickUpPanel = base.transform.Find("PickUpPanel").gameObject;
		QueryPanel = base.transform.Find("QueryPanel").gameObject;
		SetPanel = base.transform.Find("SetPanel").gameObject;
		BaoXiang = base.transform.Find("BaoXiang").gameObject;
		TextGroup = new GameObject[12];
		for (int j = 0; j < 12; j++)
		{
			TextGroup[j] = base.transform.Find("GameInfo/TextGroup/" + j).gameObject;
		}
		ScoreGroup = new Text[12];
		for (int k = 0; k < 12; k++)
		{
			ScoreGroup[k] = base.transform.Find("GameInfo/ScoreGroup/" + k).GetComponent<Text>();
		}
		SparkletGroup = new GameObject[12];
		for (int l = 0; l < 12; l++)
		{
			SparkletGroup[l] = base.transform.Find("GameInfo/SparkletGroup/" + l).gameObject;
		}
		Sparklet = base.transform.Find("GameInfo/Sparklet/").gameObject;
		HintText = base.transform.Find("HintText").GetComponent<Text>();
		BetHint = base.transform.Find("BetHint").GetComponent<Text>();
		CardRecordsBtn = QueryPanel.transform.Find("CardRecordsBtn").GetComponent<Button>();
		GameRecordsBtn = QueryPanel.transform.Find("GameRecordsBtn").GetComponent<Button>();
		JiangRecordsBtn = QueryPanel.transform.Find("JiangRecordsBtn").GetComponent<Button>();
		TurntableBtn = QueryPanel.transform.Find("TurntableBtn").GetComponent<Button>();
		CardRecordsPanel = QueryPanel.transform.Find("CardRecordsPanel").gameObject;
		GameRecordsPanel = QueryPanel.transform.Find("GameRecordsPanel").gameObject;
		JiangRecordsPanel = QueryPanel.transform.Find("JiangRecordsPanel").gameObject;
		TurntablePanel = QueryPanel.transform.Find("TurntablePanel").gameObject;
		_loopScrollView = TurntablePanel.transform.Find("ScrollView").GetComponent<Hfh_LoopScrollView>();
		_Content = TurntablePanel.transform.Find("ScrollView/Viewport/Content").gameObject;
	}

	private void Start()
	{
		Init();
		AddListener();
		StartCoroutine(FlickerEffect());
	}

	private void Init()
	{
		SideBarBtn.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		StartBtn.gameObject.SetActive(value: true);
		CancelBtn.gameObject.SetActive(value: false);
		ScoreBtn.gameObject.SetActive(value: false);
		WaitAnime.SetActive(value: true);
		BiBeiInfo.SetActive(value: false);
		GameInfo.SetActive(value: true);
		CardsPanel.SetActive(value: false);
		BiBeiPanel.SetActive(value: false);
		for (int i = 0; i < 12; i++)
		{
			TextGroup[i].SetActive(value: true);
			ScoreGroup[i].gameObject.SetActive(value: true);
		}
		Sparklet.SetActive(value: false);
		SideBarBtn.gameObject.SetActive(value: true);
		SideBar.SetActive(value: true);
		SideBar2.SetActive(value: false);
		SideBar.GetComponent<RectTransform>().anchoredPosition = Vector2.left * SideBar.GetComponent<RectTransform>().rect.width;
		UpdateScoreGroup(1);
		for (int j = 0; j < 12; j++)
		{
			TextGroup[j].SetActive(value: true);
			ScoreGroup[j].gameObject.SetActive(value: true);
		}
		IsOncCards = false;
		IsGetOneCards = false;
		IsTwoCards = false;
		IsGetTwoCards = false;
		IsDraw = false;
		IsWin = false;
		WinPos = -1;
		IsgetWinScore = false;
		BetValue = Hfh_GVars.room.MinBet;
		BetText.text = "0";
		WinText.text = "0";
		BB_BetText.text = "0";
		BB_WinText.text = "0";
		if (Hfh_GVars.room.RoomType == 4)
		{
			CreditText.text = Hfh_GVars.user.expeScore.ToString();
			BB_CreditText.text = Hfh_GVars.user.expeScore.ToString();
		}
		else
		{
			CreditText.text = Hfh_GVars.user.gameScore.ToString();
			BB_CreditText.text = Hfh_GVars.user.gameScore.ToString();
		}
		BB_BiBeiBaoJi.text = Hfh_GVars.seat.BiBeiBaoJi.ToString();
		BB_BaoJiGold.text = Hfh_GVars.seat.BaoJiGold.ToString();
		BB_GuoGuanSongGold.text = Hfh_GVars.seat.GuoGuanSongGold.ToString();
		BB_GuoGuanMax.text = Hfh_GVars.seat.GuoGuanMax.ToString();
		HintText.gameObject.SetActive(value: false);
		BetHint.gameObject.SetActive(value: true);
		BetHint.text = "PLAY " + Hfh_GVars.room.MinBet + " TO " + Hfh_GVars.room.MaxBet;
		HintText.text = "<color=\"#45D042FF\">押 注</color> 或 <color=\"#E7E65AFF\">开 始</color>";
		ClickPanel.gameObject.SetActive(value: false);
		SideBar.GetComponent<RectTransform>().anchoredPosition = Vector2.left * SideBar.GetComponent<RectTransform>().rect.width;
		RateGroup[11] = Hfh_GVars.seat.YiDuiOdds;
		RateGroup[10] = Hfh_GVars.seat.ErDuiOdds;
		RateGroup[9] = Hfh_GVars.seat.SanTiaoOdds;
		RateGroup[8] = Hfh_GVars.seat.ShunZiOdds;
		RateGroup[7] = Hfh_GVars.seat.TongHuaOdds;
		RateGroup[6] = Hfh_GVars.seat.HuLuOdds;
		RateGroup[5] = Hfh_GVars.seat.XiaoSiMeiOdds;
		RateGroup[4] = Hfh_GVars.seat.DaSiMeiOdds;
		RateGroup[3] = Hfh_GVars.seat.TongHuaShunOdds;
		RateGroup[2] = Hfh_GVars.seat.WuTiaoOdds;
		RateGroup[1] = Hfh_GVars.seat.TongHuaDaShunOdds;
		RateGroup[0] = Hfh_GVars.seat.WuGuiOdds;
		StopPanel.transform.Find("0/Image/Text").GetComponent<Text>().text = "2小时（消耗" + 2 * Hfh_GVars.seat.LiuJiScore / Hfh_GVars.seat.LiuJiTime + "分）";
		StopPanel.transform.Find("1/Image/Text").GetComponent<Text>().text = "4小时（消耗" + 4 * Hfh_GVars.seat.LiuJiScore / Hfh_GVars.seat.LiuJiTime + "分）";
		StopPanel.transform.Find("2/Image/Text").GetComponent<Text>().text = "6小时（消耗" + 6 * Hfh_GVars.seat.LiuJiScore / Hfh_GVars.seat.LiuJiTime + "分）";
		StopPanel.transform.Find("3/Image/Text").GetComponent<Text>().text = "8小时（消耗" + 8 * Hfh_GVars.seat.LiuJiScore / Hfh_GVars.seat.LiuJiTime + "分）";
		UpdateScoreGroup(1);
		DrawSpeed = Hfh_GVars.seat.OpenCardSpeed;
		SetPanel.SetActive(value: false);
		PickUpPanel.SetActive(value: false);
	}

	private void AddListener()
	{
		SideBarBtn.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			SwitchNum = 0;
			StartCoroutine(SideBarSwitch());
		});
		BetBtn.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			if (!IsStart)
			{
				WaitAnime.SetActive(value: false);
				CardsPanel.SetActive(value: true);
				IsStart = true;
				BetText.text = BetValue.ToString();
			}
			else
			{
				BetValue += 50;
				if (BetValue > Hfh_GVars.room.MaxBet)
				{
					BetValue = Hfh_GVars.room.MinBet;
				}
				BetText.text = BetValue.ToString();
			}
			UpdateScoreGroup(BetValue);
		});
		UnityAction<BaseEventData> call = StartBtnPointerDown;
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener(call);
		StartBtn.triggers.Add(entry);
		UnityAction<BaseEventData> call2 = StartBtnPointerUp;
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.PointerUp;
		entry2.callback.AddListener(call2);
		StartBtn.triggers.Add(entry2);
		UnityAction<BaseEventData> call3 = StartBtnPointerExit;
		EventTrigger.Entry entry3 = new EventTrigger.Entry();
		entry3.eventID = EventTriggerType.PointerExit;
		entry3.callback.AddListener(call3);
		StartBtn.triggers.Add(entry3);
		CancelBtn.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			StartBtn.gameObject.SetActive(value: true);
			CancelBtn.gameObject.SetActive(value: false);
			IsAuto = false;
		});
		ScoreBtn.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			ScoreBtn.interactable = false;
			Hfh_SendMsgManager.Send_EndMultiple(Hfh_GVars.seat.id);
		});
		QueryBtn.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			Hfh_SendMsgManager.Send_getDeskHistory(Hfh_GVars.seat.id);
		});
		PickUpBtn.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			PickUpPanel.SetActive(value: true);
			if (Hfh_GVars.room.RoomType == 4)
			{
				PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeScore.ToString();
				PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeGold.ToString();
			}
			else
			{
				PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameScore.ToString();
				PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameGold.ToString();
			}
		});
		PickUpBtn2.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			PickUpPanel.SetActive(value: true);
			if (Hfh_GVars.room.RoomType == 4)
			{
				PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeScore.ToString();
				PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeGold.ToString();
			}
			else
			{
				PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameScore.ToString();
				PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameGold.ToString();
			}
		});
		StopBtn.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			StopPanel.SetActive(value: true);
		});
		ReturnBtn.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			Hfh_Singleton<Hfh_AlertDialog>.GetInstance().ShowDialog("是否退出座位？", showOkCancel: true, delegate
			{
				Hfh_SendMsgManager.Send_LeaveDesk(Hfh_Singleton<Hfh_GameInfo>.GetInstance().DeskId);
			});
		});
		ClickPanel.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			SwitchNum = 1;
			StartCoroutine(SideBarSwitch());
		});
		BanBiBtn.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			ScoreBtn.interactable = false;
			Hfh_SendMsgManager.Send_startMultiple(Hfh_GVars.seat.id, 0);
		});
		BiBeiBtn.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			ScoreBtn.interactable = false;
			Hfh_SendMsgManager.Send_startMultiple(Hfh_GVars.seat.id, 1);
		});
		ShuangBiBtn.onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			ScoreBtn.interactable = false;
			Hfh_SendMsgManager.Send_startMultiple(Hfh_GVars.seat.id, 2);
			HintText.text = "<color=\"#3366ff\">比 大</color> 或 <color=\"#3366ff\">比 小</color>";
		});
		PickUpPanel.transform.Find("ClosePanel").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			PickUpPanel.SetActive(value: false);
		});
		PickUpPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			PickUpPanel.SetActive(value: false);
		});
		PickUpPanel.transform.Find("CFBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			Hfh_SendMsgManager.Send_UserCoinOut(Hfh_GVars.seat.onceExchangeValue * Hfh_GVars.seat.exchange);
		});
		PickUpPanel.transform.Find("QFBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			Hfh_SendMsgManager.Send_UserCoinIn(Hfh_GVars.seat.onceExchangeValue);
		});
		SetPanel.transform.Find("ClosePanel").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			SetPanel.SetActive(value: false);
		});
		SetPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			SetPanel.SetActive(value: false);
		});
		StopPanel.transform.Find("ClosePanel").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			StopPanel.SetActive(value: false);
		});
		StopPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			StopPanel.SetActive(value: false);
		});
		StopPanel.transform.Find("0").GetComponent<Toggle>().onValueChanged.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			LockNum = 0;
		});
		StopPanel.transform.Find("1").GetComponent<Toggle>().onValueChanged.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			LockNum = 1;
		});
		StopPanel.transform.Find("2").GetComponent<Toggle>().onValueChanged.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			LockNum = 2;
		});
		StopPanel.transform.Find("3").GetComponent<Toggle>().onValueChanged.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			LockNum = 3;
		});
		StopPanel.transform.Find("LockBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			if (Hfh_GVars.room.RoomType == 4)
			{
				if (Hfh_GVars.user.expeScore < (LockNum + 1) * 2 * (Hfh_GVars.seat.LiuJiScore / Hfh_GVars.seat.LiuJiTime) || Hfh_GVars.user.expeScore == 0)
				{
					PickUpPanel.SetActive(value: true);
					PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeScore.ToString();
					PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeGold.ToString();
					return;
				}
			}
			else if (Hfh_GVars.user.gameScore < (LockNum + 1) * 2 * (Hfh_GVars.seat.LiuJiScore / Hfh_GVars.seat.LiuJiTime) || Hfh_GVars.user.gameScore == 0)
			{
				PickUpPanel.SetActive(value: true);
				PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameScore.ToString();
				PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameGold.ToString();
				return;
			}
			Hfh_SendMsgManager.Send_LogoutKeep(Hfh_GVars.seat.id, LockNum);
		});
		QueryPanel.transform.Find("ClosePanel").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			QueryPanel.SetActive(value: false);
		});
		QueryPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			QueryPanel.SetActive(value: false);
		});
		BaoXiang.transform.Find("ClosePanel").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			BaoXiang.SetActive(value: false);
		});
		BB_da.GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			Hfh_SendMsgManager.Send_multipleInfo(Hfh_GVars.seat.id, 0);
		});
		BB_xiao.GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			Hfh_SendMsgManager.Send_multipleInfo(Hfh_GVars.seat.id, 1);
		});
		CardRecordsBtn.GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			CardRecordsBtn.interactable = false;
			CardRecordsPanel.SetActive(value: true);
			GameRecordsBtn.interactable = true;
			GameRecordsPanel.SetActive(value: false);
			JiangRecordsBtn.interactable = true;
			JiangRecordsPanel.SetActive(value: false);
			TurntableBtn.interactable = true;
			TurntablePanel.SetActive(value: false);
			Page = 0;
			UpdateHistoryCard();
		});
		CardRecordsPanel.transform.Find("Left").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			if (Page > 0)
			{
				Page--;
			}
			if (Page >= 0)
			{
				UpdateHistoryCard();
			}
		});
		CardRecordsPanel.transform.Find("Right").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			if (Page < historyCard.Length)
			{
				Page++;
			}
			if (Page < historyCard.Length)
			{
				UpdateHistoryCard();
			}
		});
		GameRecordsBtn.GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			CardRecordsBtn.interactable = true;
			CardRecordsPanel.SetActive(value: false);
			GameRecordsBtn.interactable = false;
			GameRecordsPanel.SetActive(value: true);
			JiangRecordsBtn.interactable = true;
			JiangRecordsPanel.SetActive(value: false);
			TurntableBtn.interactable = true;
			TurntablePanel.SetActive(value: false);
			Page = 0;
			UpdateHistoryMultiple();
		});
		GameRecordsPanel.transform.Find("Left").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			if (Page > 0)
			{
				Page--;
			}
			if (Page >= 0)
			{
				UpdateHistoryMultiple();
			}
		});
		GameRecordsPanel.transform.Find("Right").GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			if (Page < historyMultiple.Length)
			{
				Page++;
			}
			UpdateHistoryMultiple();
		});
		JiangRecordsBtn.GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			CardRecordsBtn.interactable = true;
			CardRecordsPanel.SetActive(value: false);
			GameRecordsBtn.interactable = true;
			GameRecordsPanel.SetActive(value: false);
			JiangRecordsBtn.interactable = false;
			JiangRecordsPanel.SetActive(value: true);
			TurntableBtn.interactable = true;
			TurntablePanel.SetActive(value: false);
			Page = 0;
			if (Page < historyCard.Length)
			{
				UpdateHistoryReward();
			}
		});
		TurntableBtn.GetComponent<Button>().onClick.AddListener(delegate
		{
			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
			CardRecordsBtn.interactable = true;
			CardRecordsPanel.SetActive(value: false);
			GameRecordsBtn.interactable = true;
			GameRecordsPanel.SetActive(value: false);
			JiangRecordsBtn.interactable = true;
			JiangRecordsPanel.SetActive(value: false);
			TurntableBtn.interactable = false;
			TurntablePanel.SetActive(value: true);
			Hfh_SendMsgManager.Send_GetRoomInfo(Hfh_GVars.seat.roomId);
			Page = 0;
		});
	}

	private void UpdateGameInfo()
	{
		if (Hfh_GVars.room.RoomType == 4)
		{
			CreditText.text = Hfh_GVars.user.expeScore.ToString();
			BB_CreditText.text = Hfh_GVars.user.expeScore.ToString();
		}
		else
		{
			CreditText.text = Hfh_GVars.user.gameScore.ToString();
			BB_CreditText.text = Hfh_GVars.user.gameScore.ToString();
		}
		BetText.text = BetValue.ToString();
		WinText.text = userWin.ToString();
	}

	public void UpdateScore()
	{
		if (Hfh_GVars.room.RoomType == 4)
		{
			CreditText.text = Hfh_GVars.user.expeScore.ToString();
			PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeScore.ToString();
			PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeGold.ToString();
		}
		else
		{
			CreditText.text = Hfh_GVars.user.gameScore.ToString();
			PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameScore.ToString();
			PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameGold.ToString();
		}
	}

	public void ShowScore(int score)
	{
		CreditText.text = score.ToString();
	}

	public void UpdateScoreGroup(int value)
	{
		for (int i = 0; i < 12; i++)
		{
			ScoreGroup[i].text = (RateGroup[i] * value).ToString();
		}
	}

	public void ShowAwards(int Awards)
	{
		if (Awards > 0)
		{
			BaoXiang.SetActive(value: true);
			BaoXiang.transform.Find("Score/Text").GetComponent<Text>().text = "获得" + Awards + string.Empty;
		}
	}

	public void UpdateQuery(Dictionary<string, object> dictionary)
	{
		QueryPanel.SetActive(value: true);
		historyCard = (dictionary["historyCard"] as object[]);
		historyMultiple = (dictionary["historyMultiple"] as object[]);
		historyReward = (dictionary["historyreward"] as Dictionary<string, object>);
		Page = 0;
		CardRecordsBtn.interactable = false;
		CardRecordsPanel.SetActive(value: true);
		GameRecordsBtn.interactable = true;
		GameRecordsPanel.SetActive(value: false);
		JiangRecordsBtn.interactable = true;
		JiangRecordsPanel.SetActive(value: false);
		TurntableBtn.interactable = true;
		TurntablePanel.SetActive(value: false);
		UpdateHistoryCard();
	}

	public void UpdateHistoryCard()
	{
		if (historyCard.Length == 0)
		{
			CardRecordsPanel.transform.Find("oneCards").gameObject.SetActive(value: false);
			CardRecordsPanel.transform.Find("twoCards").gameObject.SetActive(value: false);
			CardRecordsPanel.transform.Find("oneCards/Credit/Text").GetComponent<Text>().text = string.Empty;
			CardRecordsPanel.transform.Find("oneCards/Bet/Text").GetComponent<Text>().text = string.Empty;
			CardRecordsPanel.transform.Find("oneCards/Win/Text").GetComponent<Text>().text = string.Empty;
			return;
		}
		if (historyCard[Page] == null)
		{
			CardRecordsPanel.transform.Find("oneCards").gameObject.SetActive(value: false);
			CardRecordsPanel.transform.Find("twoCards").gameObject.SetActive(value: false);
			CardRecordsPanel.transform.Find("Credit/Text").GetComponent<Text>().text = string.Empty;
			CardRecordsPanel.transform.Find("Bet/Text").GetComponent<Text>().text = string.Empty;
			CardRecordsPanel.transform.Find("Win/Text").GetComponent<Text>().text = string.Empty;
			return;
		}
		Dictionary<string, object> dictionary = historyCard[Page] as Dictionary<string, object>;
		int[] array = (int[])dictionary["once_card"];
		int[] array2 = (int[])dictionary["two_card"];
		for (int i = 0; i < array.Length; i++)
		{
			CardRecordsPanel.transform.Find("oneCards/" + i).GetComponent<Image>().sprite = CardSprites[array[i]];
		}
		for (int j = 0; j < array.Length; j++)
		{
			CardRecordsPanel.transform.Find("twoCards/" + j).GetComponent<Image>().sprite = CardSprites[array[j]];
		}
		CardRecordsPanel.transform.Find("Credit/Text").GetComponent<Text>().text = dictionary["userScore"].ToString();
		CardRecordsPanel.transform.Find("Bet/Text").GetComponent<Text>().text = dictionary["betScore"].ToString();
		CardRecordsPanel.transform.Find("Win/Text").GetComponent<Text>().text = dictionary["winScore"].ToString();
		CardRecordsPanel.transform.Find("PageText").GetComponent<Text>().text = "上" + (Page + 1) + "笔";
	}

	public void UpdateHistoryMultiple()
	{
		if (historyMultiple.Length == 0)
		{
			GameRecordsPanel.transform.Find("ScrollView").gameObject.SetActive(value: false);
			return;
		}
		if (historyMultiple[Page] == null)
		{
			GameRecordsPanel.transform.Find("ScrollView").gameObject.SetActive(value: false);
			return;
		}
		object[] array = (historyMultiple[Page] as Dictionary<string, object>)["historyMultipleList"] as object[];
		GameObject gameObject = GameRecordsPanel.transform.Find("ScrollView/Viewport/Content").gameObject;
		GameObject gameObject2 = GameRecordsPanel.transform.Find("Item").gameObject;
		GameRecordsPanel.transform.Find("ScrollView").gameObject.SetActive(value: true);
		if (array.Length < 4)
		{
			GameRecordsPanel.transform.Find("ScrollView").GetComponent<ScrollRect>().vertical = false;
		}
		if (gameObject.transform.childCount > array.Length)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				if (i < array.Length)
				{
					Dictionary<string, object> dictionary = array[i] as Dictionary<string, object>;
					gameObject.transform.GetChild(i).gameObject.SetActive(value: true);
					for (int j = 0; j < gameObject.transform.GetChild(i).Find("Type").childCount; j++)
					{
						gameObject.transform.GetChild(i).Find("Type").GetChild(j)
							.gameObject.SetActive(value: false);
						}
						gameObject.transform.GetChild(i).Find("Type").GetChild((int)dictionary["mode"])
							.gameObject.SetActive(value: true);
							gameObject.transform.GetChild(i).Find("Bet").GetComponent<Text>()
								.text = dictionary["bet"].ToString();
								for (int k = 0; k < gameObject.transform.GetChild(i).Find("Size").childCount; k++)
								{
									gameObject.transform.GetChild(i).Find("Size").GetChild(k)
										.gameObject.SetActive(value: false);
									}
									gameObject.transform.GetChild(i).Find("Size").GetChild((int)dictionary["BigOrSmall"])
										.gameObject.SetActive(value: true);
										gameObject.transform.GetChild(i).Find("Count").GetComponent<Text>()
											.text = (i + 1).ToString();
										}
										else
										{
											gameObject.transform.GetChild(i).gameObject.SetActive(value: false);
										}
									}
								}
								else
								{
									for (int l = 0; l < array.Length; l++)
									{
										Dictionary<string, object> dictionary2 = array[l] as Dictionary<string, object>;
										if (l < gameObject.transform.childCount)
										{
											gameObject.transform.GetChild(l).gameObject.SetActive(value: true);
											for (int m = 0; m < gameObject.transform.GetChild(l).Find("Type").childCount; m++)
											{
												gameObject.transform.GetChild(l).Find("Type").GetChild(m)
													.gameObject.SetActive(value: false);
												}
												gameObject.transform.GetChild(l).Find("Type").GetChild((int)dictionary2["mode"])
													.gameObject.SetActive(value: true);
													gameObject.transform.GetChild(l).Find("Bet").GetComponent<Text>()
														.text = dictionary2["bet"].ToString();
														for (int n = 0; n < gameObject.transform.GetChild(l).Find("Size").childCount; n++)
														{
															gameObject.transform.GetChild(l).Find("Size").GetChild(n)
																.gameObject.SetActive(value: false);
															}
															gameObject.transform.GetChild(l).Find("Size").GetChild((int)dictionary2["BigOrSmall"])
																.gameObject.SetActive(value: true);
																gameObject.transform.GetChild(l).Find("Count").GetComponent<Text>()
																	.text = (l + 1).ToString();
																	continue;
																}
																GameObject gameObject3 = Object.Instantiate(gameObject2, gameObject.transform);
																gameObject3.SetActive(value: true);
																for (int num = 0; num < gameObject3.transform.Find("Type").childCount; num++)
																{
																	gameObject3.transform.Find("Type").GetChild(num).gameObject.SetActive(value: false);
																}
																gameObject3.transform.Find("Type").GetChild((int)dictionary2["mode"]).gameObject.SetActive(value: true);
																gameObject3.transform.Find("Bet").GetComponent<Text>().text = dictionary2["bet"].ToString();
																for (int num2 = 0; num2 < gameObject3.transform.Find("Size").childCount; num2++)
																{
																	gameObject3.transform.Find("Size").GetChild(num2).gameObject.SetActive(value: false);
																}
																gameObject3.transform.Find("Size").GetChild((int)dictionary2["BigOrSmall"]).gameObject.SetActive(value: true);
																gameObject3.transform.Find("Count").GetComponent<Text>().text = (l + 1).ToString();
															}
														}
														GameRecordsPanel.transform.Find("PageText").GetComponent<Text>().text = "上" + (Page + 1) + "笔";
													}

													public void UpdateHistoryReward()
													{
														JiangRecordsPanel.transform.Find("Wugui").GetComponent<Text>().text = historyReward["Wugui"].ToString();
														JiangRecordsPanel.transform.Find("TongHuaDaShun").GetComponent<Text>().text = historyReward["TongHuaDaShun"].ToString();
														JiangRecordsPanel.transform.Find("WuTiao").GetComponent<Text>().text = historyReward["WuTiao"].ToString();
														JiangRecordsPanel.transform.Find("TongHuaShun").GetComponent<Text>().text = historyReward["TongHuaShun"].ToString();
														JiangRecordsPanel.transform.Find("DaSiMei").GetComponent<Text>().text = historyReward["DaSiMei"].ToString();
														JiangRecordsPanel.transform.Find("XiaoSiMei").GetComponent<Text>().text = historyReward["XiaoSiMei"].ToString();
														int[] array = (int[])historyReward["ZZDaSiMeiBeiLvCount"];
														for (int i = 0; i < JiangRecordsPanel.transform.Find("ZZDaSiMeiBeiLvCount").childCount; i++)
														{
															JiangRecordsPanel.transform.Find("ZZDaSiMeiBeiLvCount").GetChild(i).GetComponent<Text>()
																.text = array[i].ToString();
															}
															int[] array2 = (int[])historyReward["WuMeiBeiLvCount"];
															for (int j = 0; j < JiangRecordsPanel.transform.Find("WuMeiBeiLvCount").childCount; j++)
															{
																JiangRecordsPanel.transform.Find("WuMeiBeiLvCount").GetChild(j).GetComponent<Text>()
																	.text = array2[j].ToString();
																}
															}

															private void InitRoomCell()
															{
																_loopScrollView.SetcacheCount = 2;
																_loopScrollView.Init(0, UpdateCell);
																if (Hfh_GVars.seatList.Count % 9 > 0)
																{
																	_loopScrollView.UpdateList(Hfh_GVars.seatList.Count / 9 + 1);
																}
																else
																{
																	_loopScrollView.UpdateList(Hfh_GVars.seatList.Count / 9);
																}
																for (int i = 0; i < _Content.transform.childCount; i++)
																{
																	for (int j = 0; j < _Content.transform.GetChild(i).childCount; j++)
																	{
																		GameObject obj = _Content.transform.GetChild(i).GetChild(j).gameObject;
																		_Content.transform.GetChild(i).GetChild(j).GetComponent<Button>()
																			.onClick.AddListener(delegate
																			{
																				Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
																				Hfh_Singleton<Hfh_GameInfo>.GetInstance().DeskId = int.Parse(obj.name);
																				for (int k = 0; k < Hfh_GVars.seatList.Count; k++)
																				{
																					if (Hfh_GVars.seatList[k].id == Hfh_Singleton<Hfh_GameInfo>.GetInstance().DeskId)
																					{
																						Hfh_GVars.seat = Hfh_GVars.seatList[k];
																						break;
																					}
																				}
																				Hfh_SendMsgManager.Send_EnterDesk(Hfh_Singleton<Hfh_GameInfo>.GetInstance().DeskId, 1);
																			});
																		}
																	}
																}

																public void UpdateSeatCell()
																{
																	if (IsFrist)
																	{
																		InitRoomCell();
																		IsFrist = false;
																	}
																	else if (Hfh_GVars.seatList.Count % 9 > 0)
																	{
																		_loopScrollView.UpdateList(Hfh_GVars.seatList.Count / 9 + 1);
																	}
																	else
																	{
																		_loopScrollView.UpdateList(Hfh_GVars.seatList.Count / 9);
																	}
																}

																private void UpdateCell(GameObject obj, int num)
																{
																	obj.name = num.ToString();
																	for (int i = 0; i < obj.transform.childCount; i++)
																	{
																		if (num * 9 + i < Hfh_GVars.seatList.Count)
																		{
																			obj.transform.GetChild(i).name = Hfh_GVars.seatList[num * 9 + i].id.ToString();
																			obj.transform.GetChild(i).Find("Text").GetComponent<Text>()
																				.text = Hfh_GVars.seatList[num * 9 + i].id.ToString();
																				if (Hfh_GVars.seatList[num * 9 + i].isKeepTable)
																				{
																					if (Hfh_GVars.seatList[num * 9 + i].playerid == Hfh_GVars.user.id)
																					{
																						obj.transform.GetChild(i).Find("MyLock").gameObject.SetActive(value: true);
																						obj.transform.GetChild(i).Find("Lock").gameObject.SetActive(value: false);
																					}
																					else
																					{
																						obj.transform.GetChild(i).Find("MyLock").gameObject.SetActive(value: false);
																						obj.transform.GetChild(i).Find("Lock").gameObject.SetActive(value: true);
																					}
																					obj.transform.GetChild(i).Find("Image").gameObject.SetActive(value: false);
																					obj.transform.GetChild(i).Find("Time").gameObject.SetActive(value: true);
																					obj.transform.GetChild(i).Find("Time").GetComponent<Hfh_Timer>()
																						.Activate(Hfh_GVars.seatList[num * 9 + i]);
																				}
																				else
																				{
																					obj.transform.GetChild(i).Find("MyLock").gameObject.SetActive(value: false);
																					obj.transform.GetChild(i).Find("Lock").gameObject.SetActive(value: false);
																					obj.transform.GetChild(i).Find("Time").gameObject.SetActive(value: false);
																					if (Hfh_GVars.seatList[num * 9 + i].playerid != -1)
																					{
																						obj.transform.GetChild(i).Find("Image").gameObject.SetActive(value: true);
																					}
																					else
																					{
																						obj.transform.GetChild(i).Find("Image").gameObject.SetActive(value: false);
																					}
																				}
																				obj.transform.GetChild(i).gameObject.SetActive(value: true);
																			}
																			else
																			{
																				obj.transform.GetChild(i).gameObject.SetActive(value: false);
																			}
																		}
																	}

																	public void ShowBiBeiPanel(int[] historyCard)
																	{
																		CardsPanel.SetActive(value: false);
																		GameInfo.SetActive(value: false);
																		BiBeiPanel.SetActive(value: true);
																		BiBeiInfo.SetActive(value: true);
																		BanBiBtn.interactable = false;
																		BiBeiBtn.interactable = false;
																		ShuangBiBtn.interactable = false;
																		BB_da.enabled = true;
																		BB_xiao.enabled = true;
																		BB_da.interactable = true;
																		BB_xiao.interactable = true;
																		HintText.text = "<color=\"#3366ff\">比 大</color> 或 <color=\"#3366ff\">比 小</color>";
																		BiBeiPanel.transform.Find("Card").GetComponent<Image>().sprite = CardBack;
																		BiBeiPanel.transform.Find("Result").gameObject.SetActive(value: false);
																		BB_BetText.text = userWin.ToString();
																		BB_WinText.text = userWin.ToString();
																		for (int i = 0; i < BB_LastCard.Length; i++)
																		{
																			if (i < historyCard.Length)
																			{
																				if (historyCard[i] > -1)
																				{
																					BB_LastCard[i].gameObject.SetActive(value: true);
																					BB_LastCard[i].sprite = CardSprites[historyCard[i]];
																				}
																				else
																				{
																					BB_LastCard[i].gameObject.SetActive(value: false);
																				}
																			}
																			else
																			{
																				BB_LastCard[i].gameObject.SetActive(value: false);
																			}
																		}
																	}

																	public void BiBeiResult(int BigOrSmall, int card)
																	{
																		if (_coBiBeiResult != null)
																		{
																			StopCoroutine(_coBiBeiResult);
																			_coBiBeiResult = null;
																		}
																		_coBiBeiResult = StartCoroutine(BiBeiResult_IE(BigOrSmall, card));
																	}

																	public void EndMultiple()
																	{
																		IsEndMultiple = true;
																		SideBarBtn.gameObject.SetActive(value: true);
																		SideBar.SetActive(value: true);
																		SideBar2.SetActive(value: false);
																		if (!IsAuto)
																		{
																			if (_coScoreEffect != null)
																			{
																				StopCoroutine(_coScoreEffect);
																				_coScoreEffect = null;
																			}
																			_coScoreEffect = StartCoroutine(ScoreEffect());
																		}
																	}

																	private IEnumerator FlickerEffect()
																	{
																		while (true)
																		{
																			if (IsDraw)
																			{
																				if (FlickerTime >= 0.5f)
																				{
																					UnityEngine.Debug.Log("这个？1");
																					FlickerTime = 0f;
																					HintText.gameObject.SetActive(!HintText.gameObject.activeSelf);
																					if (BetHint.gameObject.activeSelf)
																					{
																						BetHint.gameObject.SetActive(value: false);
																					}
																				}
																			}
																			else if (FlickerTime >= 0.5f)
																			{
																				UnityEngine.Debug.Log("这个？2");
																				FlickerTime = 0f;
																				BetHint.gameObject.SetActive(!BetHint.gameObject.activeSelf);
																				if (HintText.gameObject.activeSelf)
																				{
																					HintText.gameObject.SetActive(value: false);
																				}
																			}
																			if (IsgetWinScore && userWin > 0)
																			{
																				if (WinPos >= 0 && !Sparklet.activeSelf)
																				{
																					Sparklet.SetActive(value: true);
																					Sparklet.transform.position = SparkletGroup[WinPos].transform.position;
																					Sparklet.transform.GetChild(0).GetComponent<Text>().text = TextGroup[WinPos].GetComponent<Text>().text;
																					Sparklet.transform.GetChild(0).GetComponent<Text>().color = TextGroup[WinPos].GetComponent<Text>().color;
																					Sparklet.transform.GetChild(1).GetComponent<Text>().text = ScoreGroup[WinPos].text;
																					Sparklet.transform.GetChild(1).GetComponent<Text>().color = ScoreGroup[WinPos].color;
																				}
																			}
																			else if (WinPos != -1)
																			{
																				Sparklet.SetActive(value: false);
																			}
																			yield return new WaitForSeconds(0.02f);
																		}
																	}

																	private IEnumerator SideBarSwitch()
																	{
																		if (IsSwitch)
																		{
																			yield break;
																		}
																		IsSwitch = true;
																		float _value = 0f;
																		while (true)
																		{
																			if (SwitchNum == 1)
																			{
																				if (_value <= 1.1f)
																				{
																					SideBar.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(Vector2.zero, SideBar.GetComponent<RectTransform>().rect.width * Vector2.left, _value);
																				}
																				else
																				{
																					SideBarBtn.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(SideBarBtn.GetComponent<RectTransform>().rect.width * Vector2.left, Vector2.zero, _value - 1f);
																				}
																			}
																			else if (_value <= 1.1f)
																			{
																				SideBarBtn.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(Vector2.zero, SideBarBtn.GetComponent<RectTransform>().rect.width * Vector2.left, _value);
																			}
																			else
																			{
																				SideBar.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(SideBar.GetComponent<RectTransform>().rect.width * Vector2.left, Vector2.zero, _value - 1f);
																			}
																			if (_value > 2f)
																			{
																				break;
																			}
																			_value += 0.1f;
																			yield return new WaitForSeconds(0.02f);
																		}
																		if (SwitchNum == 1)
																		{
																			SideBar.GetComponent<RectTransform>().anchoredPosition = SideBar.GetComponent<RectTransform>().rect.width * Vector2.left;
																			SideBarBtn.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
																			ClickPanel.gameObject.SetActive(value: false);
																		}
																		else
																		{
																			SideBarBtn.GetComponent<RectTransform>().anchoredPosition = SideBarBtn.GetComponent<RectTransform>().rect.width * Vector2.left;
																			SideBar.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
																			ClickPanel.gameObject.SetActive(value: true);
																		}
																		IsSwitch = false;
																	}

																	public void StartBtnPointerDown(BaseEventData data)
																	{
																		if (!IsAuto && !IsOncCards)
																		{
																			if (IsStartBtnClick)
																			{
																				return;
																			}
																			IsStartBtnClick = true;
																			StartBtnTime = 0f;
																			if (!IsAuto)
																			{
																				if (_coAutoGame != null)
																				{
																					StopCoroutine(_coAutoGame);
																					_coAutoGame = null;
																				}
																				_coAutoGame = StartCoroutine(NewAutoStartGame());
																			}
																		}
																		else
																		{
																			IsStartBtnClick = true;
																		}
																	}

																	public void StartBtnPointerUp(BaseEventData data)
																	{
																		Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
																		if (!IsStartBtnClick)
																		{
																			return;
																		}
																		IsStartBtnClick = false;
																		if (!IsOncCards)
																		{
																			if (Hfh_GVars.room.RoomType == 4)
																			{
																				if (Hfh_GVars.user.expeScore < BetValue || Hfh_GVars.user.expeScore == 0)
																				{
																					PickUpPanel.SetActive(value: true);
																					PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeScore.ToString();
																					PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeGold.ToString();
																					return;
																				}
																			}
																			else if (Hfh_GVars.user.gameScore < BetValue || Hfh_GVars.user.gameScore == 0)
																			{
																				PickUpPanel.SetActive(value: true);
																				PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameScore.ToString();
																				PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameGold.ToString();
																				return;
																			}
																		}
																		if (!IsStart)
																		{
																			WaitAnime.SetActive(value: false);
																			CardsPanel.SetActive(value: true);
																			IsStart = true;
																			BetText.text = BetValue.ToString();
																		}
																		if (!IsAuto)
																		{
																			if (!IsOncCards)
																			{
																				IsOncCards = true;
																				StartBtn.GetComponent<Button>().interactable = false;
																				StartBtn.enabled = false;
																				Hfh_SendMsgManager.Send_OneCard(Hfh_GVars.seat.id, BetValue);
																			}
																			else if (IsGetOneCards && !IsTwoCards)
																			{
																				IsTwoCards = true;
																				StartBtn.GetComponent<Button>().interactable = false;
																				StartBtn.enabled = false;
																				Hfh_SendMsgManager.Send_TwiceCard(Hfh_GVars.seat.id, GetHeldCards());
																			}
																		}
																	}

																	public void StartBtnPointerExit(BaseEventData data)
																	{
																		IsStartBtnClick = false;
																	}

																	private IEnumerator AutoStartGame()
																	{
																		while (IsStartBtnClick)
																		{
																			if (StartBtnTime >= 2f)
																			{
																				if (Hfh_GVars.room.RoomType == 4)
																				{
																					if (Hfh_GVars.user.expeScore < BetValue || Hfh_GVars.user.expeScore == 0)
																					{
																						PickUpPanel.SetActive(value: true);
																						PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeScore.ToString();
																						PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeGold.ToString();
																						IsStartBtnClick = false;
																						yield break;
																					}
																				}
																				else if (Hfh_GVars.user.gameScore < BetValue || Hfh_GVars.user.gameScore == 0)
																				{
																					PickUpPanel.SetActive(value: true);
																					PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameScore.ToString();
																					PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameGold.ToString();
																					IsStartBtnClick = false;
																					yield break;
																				}
																				if (!IsAuto)
																				{
																					WaitAnime.SetActive(value: false);
																					CardsPanel.SetActive(value: true);
																					IsAuto = true;
																					BetText.text = BetValue.ToString();
																				}
																				StartBtn.gameObject.SetActive(value: false);
																				CancelBtn.gameObject.SetActive(value: true);
																				break;
																			}
																			yield return new WaitForSeconds(0.02f);
																			StartBtnTime += 0.04f;
																		}
																		IsStartBtnClick = false;
																	}

																	private IEnumerator NewAutoStartGame()
																	{
																		while (IsStartBtnClick)
																		{
																			if (StartBtnTime >= 2f)
																			{
																				if (Hfh_GVars.room.RoomType == 4)
																				{
																					if (Hfh_GVars.user.expeScore < BetValue || Hfh_GVars.user.expeScore == 0)
																					{
																						PickUpPanel.SetActive(value: true);
																						PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeScore.ToString();
																						PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeGold.ToString();
																						IsStartBtnClick = false;
																						yield break;
																					}
																				}
																				else if (Hfh_GVars.user.gameScore < BetValue || Hfh_GVars.user.gameScore == 0)
																				{
																					PickUpPanel.SetActive(value: true);
																					PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameScore.ToString();
																					PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.gameGold.ToString();
																					IsStartBtnClick = false;
																					yield break;
																				}
																				if (!IsAuto)
																				{
																					WaitAnime.SetActive(value: false);
																					CardsPanel.SetActive(value: true);
																					IsAuto = true;
																					BetText.text = BetValue.ToString();
																				}
																				StartBtn.GetComponent<Button>().interactable = false;
																				StartBtn.enabled = false;
																				StartBtn.gameObject.SetActive(value: false);
																				CancelBtn.gameObject.SetActive(value: true);
																				break;
																			}
																			yield return new WaitForSeconds(0.02f);
																			StartBtnTime += 0.04f;
																		}
																		do
																		{
																			if (!IsOncCards)
																			{
																				if (Hfh_GVars.room.RoomType == 4)
																				{
																					if (Hfh_GVars.user.expeScore < BetValue || Hfh_GVars.user.expeScore == 0)
																					{
																						PickUpPanel.SetActive(value: true);
																						PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeScore.ToString();
																						PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeGold.ToString();
																						IsAuto = false;
																					}
																				}
																				else if (Hfh_GVars.user.gameScore < BetValue || Hfh_GVars.user.gameScore == 0)
																				{
																					PickUpPanel.SetActive(value: true);
																					PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeScore.ToString();
																					PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Hfh_GVars.user.expeGold.ToString();
																					IsAuto = false;
																				}
																			}
																			if (!IsAuto)
																			{
																				StartBtn.gameObject.SetActive(value: true);
																				CancelBtn.gameObject.SetActive(value: false);
																				yield break;
																			}
																			int drawPos = 0;
																			float _value = 0f;
																			while (true)
																			{
																				if (!IsOncCards)
																				{
																					UnityEngine.Debug.Log("自动第一手");
																					IsOncCards = true;
																					drawPos = 0;
																					_value = 0f;
																					Hfh_SendMsgManager.Send_OneCard(Hfh_GVars.seat.id, BetValue);
																				}
																				else if (IsGetOneCards)
																				{
																					if (!CardsPanel.transform.GetChild(drawPos).GetComponent<Hfh_Poker>().IsHold)
																					{
																						if (!CardsPanel.transform.GetChild(drawPos).GetComponent<Hfh_Poker>().IsPlayAudio)
																						{
																							CardsPanel.transform.GetChild(drawPos).GetComponent<Hfh_Poker>().IsPlayAudio = true;
																							Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound_Hold();
																						}
																						if (_value < 1f)
																						{
																							CardsPanel.transform.GetChild(drawPos).localScale = Vector3.Lerp(Vector3.one, Vector3.up + Vector3.forward, _value);
																						}
																						else if (_value > 1f)
																						{
																							CardsPanel.transform.GetChild(drawPos).localScale = Vector3.Lerp(Vector3.up + Vector3.forward, Vector3.one, _value - 1f);
																						}
																						if (_value >= 2f)
																						{
																							_value = 0f;
																							if (drawPos == CardsPanel.transform.childCount - 1)
																							{
																								break;
																							}
																							drawPos++;
																						}
																						if (_value < 1f)
																						{
																							_value += 0.02f * (float)DrawSpeed;
																							if (_value >= 1f)
																							{
																								CardsPanel.transform.GetChild(drawPos).GetComponent<Hfh_Poker>().UpdateCard();
																							}
																						}
																						else
																						{
																							_value += 0.02f * (float)DrawSpeed;
																						}
																						yield return new WaitForSeconds(0.02f);
																					}
																					else
																					{
																						_value = 0f;
																						if (drawPos == CardsPanel.transform.childCount - 1)
																						{
																							break;
																						}
																						drawPos++;
																					}
																				}
																				else
																				{
																					yield return new WaitForSeconds(0.02f);
																				}
																			}
																			Hfh_CardType cardtype = new Hfh_CardType();
																			cardtype.AutoReserveCards(cardtype.CardsSort2(MyCards));
																			int[] Gcards = cardtype.GetHeldCards();
																			for (int i = 0; i < CardsPanel.transform.childCount; i++)
																			{
																				for (int j = 0; j < Gcards.Length; j++)
																				{
																					if (Gcards[j] == CardsPanel.transform.GetChild(i).GetComponent<Hfh_Poker>().PokerNum)
																					{
																						CardsPanel.transform.GetChild(i).GetComponent<Hfh_Poker>().SetHeld();
																					}
																				}
																			}
																			for (int k = 0; k < CardsPanel.transform.childCount; k++)
																			{
																				if (CardsPanel.transform.GetChild(k).GetComponent<Hfh_Poker>().PokerNum >= 52)
																				{
																					CardsPanel.transform.GetChild(k).GetComponent<Hfh_Poker>().SetHeld();
																				}
																			}
																			HeldEnabled();
																			yield return new WaitForSeconds(1f);
																			if (!IsAuto)
																			{
																				StartBtn.enabled = true;
																				StartBtn.GetComponent<Button>().interactable = true;
																				yield break;
																			}
																			while (true)
																			{
																				if (!IsTwoCards)
																				{
																					UnityEngine.Debug.Log("自动第二手");
																					IsTwoCards = true;
																					drawPos = 0;
																					_value = 0f;
																					Hfh_SendMsgManager.Send_TwiceCard(Hfh_GVars.seat.id, GetHeldCards());
																				}
																				else if (IsGetTwoCards)
																				{
																					UnityEngine.Debug.Log("第二手翻牌中");
																					if (!CardsPanel.transform.GetChild(drawPos).GetComponent<Hfh_Poker>().IsHold)
																					{
																						if (!CardsPanel.transform.GetChild(drawPos).GetComponent<Hfh_Poker>().IsPlayAudio)
																						{
																							CardsPanel.transform.GetChild(drawPos).GetComponent<Hfh_Poker>().IsPlayAudio = true;
																							Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound_Hold();
																						}
																						if (_value < 1f)
																						{
																							CardsPanel.transform.GetChild(drawPos).localScale = Vector3.Lerp(Vector3.one, Vector3.up + Vector3.forward, _value);
																						}
																						else if (_value > 1f)
																						{
																							CardsPanel.transform.GetChild(drawPos).localScale = Vector3.Lerp(Vector3.up + Vector3.forward, Vector3.one, _value - 1f);
																						}
																						if (_value >= 2f)
																						{
																							_value = 0f;
																							if (drawPos == CardsPanel.transform.childCount - 1)
																							{
																								break;
																							}
																							drawPos++;
																						}
																						if (_value < 1f)
																						{
																							_value += 0.02f * (float)DrawSpeed;
																							if (_value >= 1f)
																							{
																								CardsPanel.transform.GetChild(drawPos).GetComponent<Hfh_Poker>().UpdateCard();
																							}
																						}
																						else
																						{
																							_value += 0.02f * (float)DrawSpeed;
																						}
																						yield return new WaitForSeconds(0.02f);
																					}
																					else
																					{
																						_value = 0f;
																						if (drawPos == CardsPanel.transform.childCount - 1)
																						{
																							break;
																						}
																						drawPos++;
																					}
																				}
																				else
																				{
																					yield return new WaitForSeconds(0.02f);
																				}
																			}
																			if (!IsAuto)
																			{
																				ClearCards();
																				if (userWin == 0)
																				{
																					Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("fail");
																					yield return new WaitForSeconds(1f);
																					ResetCards();
																					IsOncCards = false;
																					IsGetOneCards = false;
																					IsTwoCards = false;
																					IsGetTwoCards = false;
																					IsDraw = false;
																					UpdateScoreGroup(1);
																					HintText.text = "<color=\"#45D042FF\">押 注</color> 或 <color=\"#E7E65AFF\">开 始</color>";
																					StartBtn.transform.GetChild(1).gameObject.SetActive(value: true);
																					StartBtn.GetComponent<Button>().interactable = true;
																					StartBtn.enabled = true;
																					PickUpBtn.interactable = true;
																					StopBtn.interactable = true;
																					BetBtn.interactable = true;
																					ReturnBtn.interactable = true;
																					QueryBtn.interactable = true;
																				}
																				else
																				{
																					Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlayVoice(WinPos);
																					Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("suc");
																					StartBtn.gameObject.SetActive(value: false);
																					ScoreBtn.gameObject.SetActive(value: true);
																					IsgetWinScore = true;
																					HintText.text = "<color=\"#45D042FF\">得  分</color>";
																				}
																				yield break;
																			}
																			ClearCards();
																			if (userWin > 0)
																			{
																				Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlayVoice(WinPos);
																				Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("suc");
																				IsgetWinScore = true;
																				yield return new WaitForSeconds(1f);
																			}
																			else
																			{
																				Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("fail");
																				yield return new WaitForSeconds(1f);
																			}
																			if (userWin > 0 && IsgetWinScore)
																			{
																				IsEndMultiple = false;
																				Hfh_SendMsgManager.Send_EndMultiple(Hfh_GVars.seat.id);
																				while (!IsEndMultiple)
																				{
																					UnityEngine.Debug.Log("陷入奇怪的循环了？");
																					yield return new WaitForSeconds(0.02f);
																				}
																			}
																			IsWin = true;
																			if (userWin > 0)
																			{
																				Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound_Score();
																				while (userWin > 0)
																				{
																					ScoreGroup[WinPos].text = userWin.ToString();
																					WinText.text = userWin.ToString();
																					Sparklet.transform.GetChild(1).GetComponent<Text>().text = userWin.ToString();
																					if (Hfh_GVars.room.RoomType == 4)
																					{
																						CreditText.text = (Hfh_GVars.user.expeScore - userWin).ToString();
																					}
																					else
																					{
																						CreditText.text = (Hfh_GVars.user.gameScore - userWin).ToString();
																					}
																					userWin -= ((userWin <= userVariate) ? (userVariate = userWin) : userVariate);
																					yield return new WaitForSeconds(0.02f);
																				}
																				Hfh_Singleton<Hfh_AudioManager>.GetInstance().StopSound_Score();
																				ScoreGroup[WinPos].text = userWin.ToString();
																				WinText.text = userWin.ToString();
																				Sparklet.transform.GetChild(1).GetComponent<Text>().text = userWin.ToString();
																				if (Hfh_GVars.room.RoomType == 4)
																				{
																					CreditText.text = (Hfh_GVars.user.expeScore - userWin).ToString();
																				}
																				else
																				{
																					CreditText.text = (Hfh_GVars.user.gameScore - userWin).ToString();
																				}
																			}
																			yield return new WaitForSeconds(0.5f);
																			ClearCards();
																			ResetCards();
																			IsOncCards = false;
																			IsGetOneCards = false;
																			IsTwoCards = false;
																			IsGetTwoCards = false;
																			IsDraw = false;
																			IsWin = false;
																			WinPos = -1;
																			IsgetWinScore = false;
																			UpdateScoreGroup(1);
																			HintText.text = "<color=\"#45D042FF\">押 注</color> 或 <color=\"#E7E65AFF\">开 始</color>";
																			StartBtn.gameObject.SetActive(value: true);
																			ScoreBtn.gameObject.SetActive(value: false);
																			ScoreBtn.interactable = true;
																			StartBtn.GetComponent<Button>().interactable = true;
																			StartBtn.enabled = true;
																			BetBtn.interactable = true;
																			PickUpBtn.interactable = true;
																			StopBtn.interactable = true;
																			ReturnBtn.interactable = true;
																			QueryBtn.interactable = true;
																			StartBtn.transform.GetChild(1).gameObject.SetActive(value: true);
																		}
																		while (IsAuto);
																		UnityEngine.Debug.Log("跳出来了？");
																		IsStartBtnClick = false;
																	}

																	public void DrawCards()
																	{
																		PickUpBtn.interactable = false;
																		PickUpBtn2.interactable = false;
																		StopBtn.interactable = false;
																		BetBtn.interactable = false;
																		ReturnBtn.interactable = false;
																		QueryBtn.interactable = false;
																		IsDraw = true;
																		StartBtn.transform.GetChild(1).gameObject.SetActive(value: false);
																		StartCoroutine(DrawaCards_IE());
																		UpdateScoreGroup(BetValue);
																		BetHint.gameObject.SetActive(value: false);
																		HintText.gameObject.SetActive(value: true);
																		HintText.text = "<color=\"#45D042FF\">保 留</color> 或 <color=\"#E7E65AFF\">开 牌</color>";
																	}

																	private IEnumerator DrawaCards_IE()
																	{
																		yield return null;
																		for (int i = 0; i < CardsPanel.transform.childCount; i++)
																		{
																			CardsPanel.transform.GetChild(i).GetComponent<Hfh_Poker>().PokerNum = MyCards[i];
																		}
																		if (IsAuto)
																		{
																			yield break;
																		}
																		if (_coDrawCard != null)
																		{
																			StopCoroutine(_coDrawCard);
																			_coDrawCard = null;
																		}
																		_coDrawCard = StartCoroutine(DrawCard_IE());
																		yield return new WaitForSeconds(GetDrawWaitTime());
																		if (ISDrawStop)
																		{
																			do
																			{
																				yield return new WaitForSeconds(0.02f);
																			}
																			while (ISDrawStop);
																		}
																		Hfh_CardType cardtype = new Hfh_CardType();
																		cardtype.AutoReserveCards(cardtype.CardsSort2(MyCards));
																		int[] Gcards = cardtype.GetHeldCards();
																		for (int j = 0; j < CardsPanel.transform.childCount; j++)
																		{
																			for (int k = 0; k < Gcards.Length; k++)
																			{
																				if (Gcards[k] == CardsPanel.transform.GetChild(j).GetComponent<Hfh_Poker>().PokerNum)
																				{
																					CardsPanel.transform.GetChild(j).GetComponent<Hfh_Poker>().SetHeld();
																				}
																			}
																		}
																		for (int l = 0; l < CardsPanel.transform.childCount; l++)
																		{
																			if (CardsPanel.transform.GetChild(l).GetComponent<Hfh_Poker>().PokerNum >= 52)
																			{
																				CardsPanel.transform.GetChild(l).GetComponent<Hfh_Poker>().SetHeld();
																			}
																		}
																		HeldEnabled();
																		yield return new WaitForSeconds(0.5f);
																	}

																	public void DrawCards_Twice()
																	{
																		HeldDisable();
																		StartCoroutine(DrawCards_IE_Twice());
																		UpdateScoreGroup(BetValue);
																	}

																	private IEnumerator DrawCards_IE_Twice()
																	{
																		for (int i = 0; i < CardsPanel.transform.childCount; i++)
																		{
																			if (!CardsPanel.transform.GetChild(i).GetComponent<Hfh_Poker>().IsHold)
																			{
																				CardsPanel.transform.GetChild(i).GetComponent<Hfh_Poker>().Reset();
																			}
																		}
																		for (int j = 0; j < CardsPanel.transform.childCount; j++)
																		{
																			CardsPanel.transform.GetChild(j).GetComponent<Hfh_Poker>().PokerNum = MyCards[j];
																		}
																		if (IsAuto)
																		{
																			yield break;
																		}
																		if (_coDrawCard != null)
																		{
																			StopCoroutine(_coDrawCard);
																			_coDrawCard = null;
																		}
																		_coDrawCard = StartCoroutine(DrawCard_IE());
																		yield return new WaitForSeconds(GetDrawWaitTime());
																		if (ISDrawStop)
																		{
																			do
																			{
																				yield return new WaitForSeconds(0.02f);
																			}
																			while (ISDrawStop);
																		}
																		ClearCards();
																		if (userWin == 0)
																		{
																			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("fail");
																			yield return new WaitForSeconds(1f);
																			ResetCards();
																			IsOncCards = false;
																			IsGetOneCards = false;
																			IsTwoCards = false;
																			IsGetTwoCards = false;
																			IsDraw = false;
																			UpdateScoreGroup(1);
																			HintText.text = "<color=\"#45D042FF\">押 注</color> 或 <color=\"#E7E65AFF\">开 始</color>";
																			StartBtn.GetComponent<Button>().interactable = true;
																			StartBtn.enabled = true;
																			PickUpBtn.interactable = true;
																			PickUpBtn2.interactable = true;
																			StopBtn.interactable = true;
																			BetBtn.interactable = true;
																			ReturnBtn.interactable = true;
																			QueryBtn.interactable = true;
																			StartBtn.transform.GetChild(1).gameObject.SetActive(value: true);
																			SideBarBtn.gameObject.SetActive(value: true);
																			SideBar.SetActive(value: true);
																			SideBar2.SetActive(value: false);
																		}
																		else
																		{
																			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlayVoice(WinPos);
																			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("suc");
																			StartBtn.gameObject.SetActive(value: false);
																			ScoreBtn.gameObject.SetActive(value: true);
																			SideBarBtn.gameObject.SetActive(value: false);
																			SideBar.SetActive(value: false);
																			SideBar2.SetActive(value: true);
																			HintText.text = "<color=\"#45D042FF\">得  分</color>或 <color=\"#E7E65AFF\">比 倍</color>";
																		}
																	}

																	public float GetDrawWaitTime()
																	{
																		float num = 0f;
																		for (int i = 0; i < CardsPanel.transform.childCount; i++)
																		{
																			if (!CardsPanel.transform.GetChild(i).GetComponent<Hfh_Poker>().IsHold)
																			{
																				num += 1f;
																			}
																		}
																		return 1.4f / (float)DrawSpeed * num;
																	}

																	private IEnumerator DrawCard_IE()
																	{
																		ISDrawStop = true;
																		int drawPos = 0;
																		float _value = 0f;
																		while (true)
																		{
																			if (!CardsPanel.transform.GetChild(drawPos).GetComponent<Hfh_Poker>().IsHold)
																			{
																				if (!CardsPanel.transform.GetChild(drawPos).GetComponent<Hfh_Poker>().IsPlayAudio)
																				{
																					CardsPanel.transform.GetChild(drawPos).GetComponent<Hfh_Poker>().IsPlayAudio = true;
																					Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound_Hold();
																				}
																				if (_value < 1f)
																				{
																					CardsPanel.transform.GetChild(drawPos).localScale = Vector3.Lerp(Vector3.one, Vector3.up + Vector3.forward, _value);
																				}
																				else if (_value > 1f)
																				{
																					CardsPanel.transform.GetChild(drawPos).localScale = Vector3.Lerp(Vector3.up + Vector3.forward, Vector3.one, _value - 1f);
																				}
																				if (_value >= 2f)
																				{
																					_value = 0f;
																					if (drawPos == CardsPanel.transform.childCount - 1)
																					{
																						break;
																					}
																					drawPos++;
																				}
																				if (_value < 1f)
																				{
																					_value += 0.02f * (float)DrawSpeed;
																					if (_value >= 1f)
																					{
																						CardsPanel.transform.GetChild(drawPos).GetComponent<Hfh_Poker>().UpdateCard();
																					}
																				}
																				else
																				{
																					_value += 0.02f * (float)DrawSpeed;
																				}
																				yield return new WaitForSeconds(0.02f);
																			}
																			else
																			{
																				_value = 0f;
																				if (drawPos == CardsPanel.transform.childCount - 1)
																				{
																					break;
																				}
																				drawPos++;
																			}
																		}
																		ISDrawStop = false;
																		if (IsOncCards && !IsTwoCards)
																		{
																			StartBtn.GetComponent<Button>().interactable = true;
																			StartBtn.enabled = true;
																		}
																		else if (IsTwoCards)
																		{
																			IsgetWinScore = true;
																		}
																	}

																	public void ResetCards()
																	{
																		for (int i = 0; i < CardsPanel.transform.childCount; i++)
																		{
																			CardsPanel.transform.GetChild(i).GetComponent<Hfh_Poker>().Reset();
																		}
																	}

																	public void ClearCards()
																	{
																		for (int i = 0; i < CardsPanel.transform.childCount; i++)
																		{
																			CardsPanel.transform.GetChild(i).GetComponent<Hfh_Poker>().Clear();
																		}
																	}

																	public void HeldEnabled()
																	{
																		UnityEngine.Debug.Log("开启");
																		for (int i = 0; i < CardsPanel.transform.childCount; i++)
																		{
																			CardsPanel.transform.GetChild(i).GetComponent<Hfh_Poker>().HeldEnabled();
																		}
																	}

																	public void HeldDisable()
																	{
																		UnityEngine.Debug.Log("关闭");
																		for (int i = 0; i < CardsPanel.transform.childCount; i++)
																		{
																			CardsPanel.transform.GetChild(i).GetComponent<Hfh_Poker>().HeldDisable();
																		}
																	}

																	public int[] GetHeldCards()
																	{
																		int[] array = new int[CardsPanel.transform.childCount];
																		for (int i = 0; i < CardsPanel.transform.childCount; i++)
																		{
																			if (CardsPanel.transform.GetChild(i).GetComponent<Hfh_Poker>().IsHold)
																			{
																				array[i] = 1;
																			}
																			else
																			{
																				array[i] = 0;
																			}
																		}
																		return array;
																	}

																	public void ShowEffect(int type)
																	{
																		switch (type)
																		{
																		case 1:
																			WinPos = 11;
																			userVariate = Hfh_GVars.seat.YiDuiWinSpeed;
																			break;
																		case 2:
																			WinPos = 10;
																			userVariate = Hfh_GVars.seat.ErDuiWinSpeed;
																			break;
																		case 3:
																			WinPos = 9;
																			userVariate = Hfh_GVars.seat.SanTiaoWinSpeed;
																			break;
																		case 4:
																			WinPos = 8;
																			userVariate = Hfh_GVars.seat.ShunZiWinSpeed;
																			break;
																		case 5:
																			WinPos = 7;
																			userVariate = Hfh_GVars.seat.TongHuaWinSpeed;
																			break;
																		case 6:
																			WinPos = 6;
																			userVariate = Hfh_GVars.seat.HuLuWinSpeed;
																			break;
																		case 7:
																			WinPos = 5;
																			userVariate = Hfh_GVars.seat.XiaoSiMeiWinSpeed;
																			break;
																		case 8:
																			WinPos = 4;
																			userVariate = Hfh_GVars.seat.DaSiMeiWinSpeed;
																			break;
																		case 9:
																			WinPos = 3;
																			userVariate = Hfh_GVars.seat.TongHuaShunWinSpeed;
																			break;
																		case 10:
																			WinPos = 2;
																			userVariate = Hfh_GVars.seat.WuTiaoWinSpeed;
																			break;
																		case 11:
																			WinPos = 1;
																			userVariate = Hfh_GVars.seat.TongHuaDaShunWinSpeed;
																			break;
																		case 12:
																			WinPos = 0;
																			userVariate = Hfh_GVars.seat.WuGuiWinSpeed;
																			break;
																		default:
																			WinPos = -1;
																			userVariate = 1;
																			break;
																		}
																	}

																	private IEnumerator BiBeiResult_IE(int BigOrSmall, int card)
																	{
																		BB_Card.sprite = CardSprites[card];
																		BB_Result.gameObject.SetActive(value: true);
																		if (userWin > 0)
																		{
																			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("bwin");
																			BB_Result.sprite = ResultSprites[0];
																		}
																		else
																		{
																			Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("fail");
																			BB_Result.sprite = ResultSprites[1];
																		}
																		if (BigOrSmall == 0)
																		{
																			BB_da.interactable = false;
																			BB_xiao.enabled = false;
																		}
																		else
																		{
																			BB_da.enabled = false;
																			BB_xiao.interactable = false;
																		}
																		BB_WinText.text = userWin.ToString();
																		BanBiBtn.interactable = true;
																		BiBeiBtn.interactable = true;
																		ShuangBiBtn.interactable = true;
																		UpdateGameInfo();
																		if (userWin == 0)
																		{
																			yield return new WaitForSeconds(3f);
																			BiBeiInfo.SetActive(value: false);
																			GameInfo.SetActive(value: true);
																			BiBeiPanel.SetActive(value: false);
																			CardsPanel.SetActive(value: true);
																			ResetCards();
																			IsOncCards = false;
																			IsGetOneCards = false;
																			IsTwoCards = false;
																			IsGetTwoCards = false;
																			IsDraw = false;
																			IsWin = false;
																			WinPos = -1;
																			IsgetWinScore = false;
																			UpdateScoreGroup(1);
																			HintText.text = "<color=\"#45D042FF\">押 注</color> 或 <color=\"#E7E65AFF\">开 始</color>";
																			StartBtn.gameObject.SetActive(value: true);
																			ScoreBtn.gameObject.SetActive(value: false);
																			ScoreBtn.interactable = true;
																			StartBtn.GetComponent<Button>().interactable = true;
																			StartBtn.enabled = true;
																			BetBtn.interactable = true;
																			PickUpBtn.interactable = true;
																			PickUpBtn2.interactable = true;
																			StopBtn.interactable = true;
																			ReturnBtn.interactable = true;
																			QueryBtn.interactable = true;
																			SideBarBtn.gameObject.SetActive(value: true);
																			SideBar.SetActive(value: true);
																			SideBar2.SetActive(value: false);
																			StartBtn.transform.GetChild(1).gameObject.SetActive(value: true);
																		}
																		else
																		{
																			ScoreBtn.interactable = true;
																		}
																	}

																	private IEnumerator ScoreEffect()
																	{
																		IsWin = true;
																		UnityEngine.Debug.Log("得分：" + userVariate);
																		Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound_Score();
																		while (userWin > 0)
																		{
																			ScoreGroup[WinPos].text = userWin.ToString();
																			WinText.text = userWin.ToString();
																			BB_WinText.text = userWin.ToString();
																			Sparklet.transform.GetChild(1).GetComponent<Text>().text = userWin.ToString();
																			if (Hfh_GVars.room.RoomType == 4)
																			{
																				CreditText.text = (Hfh_GVars.user.expeScore - userWin).ToString();
																				BB_CreditText.text = (Hfh_GVars.user.expeScore - userWin).ToString();
																			}
																			else
																			{
																				CreditText.text = (Hfh_GVars.user.gameScore - userWin).ToString();
																				BB_CreditText.text = (Hfh_GVars.user.gameScore - userWin).ToString();
																			}
																			userWin -= ((userWin <= userVariate) ? (userVariate = userWin) : userVariate);
																			yield return new WaitForSeconds(0.02f);
																		}
																		Hfh_Singleton<Hfh_AudioManager>.GetInstance().StopSound_Score();
																		ScoreGroup[WinPos].text = userWin.ToString();
																		WinText.text = userWin.ToString();
																		BB_WinText.text = userWin.ToString();
																		Sparklet.transform.GetChild(1).GetComponent<Text>().text = userWin.ToString();
																		if (Hfh_GVars.room.RoomType == 4)
																		{
																			CreditText.text = (Hfh_GVars.user.expeScore - userWin).ToString();
																			BB_CreditText.text = (Hfh_GVars.user.expeScore - userWin).ToString();
																		}
																		else
																		{
																			CreditText.text = (Hfh_GVars.user.gameScore - userWin).ToString();
																			BB_CreditText.text = (Hfh_GVars.user.gameScore - userWin).ToString();
																		}
																		yield return new WaitForSeconds(0.5f);
																		ResetCards();
																		IsOncCards = false;
																		IsGetOneCards = false;
																		IsTwoCards = false;
																		IsGetTwoCards = false;
																		IsDraw = false;
																		IsWin = false;
																		WinPos = -1;
																		IsgetWinScore = false;
																		UpdateScoreGroup(1);
																		HintText.text = "<color=\"#45D042FF\">押 注</color> 或 <color=\"#E7E65AFF\">开 始</color>";
																		GameInfo.SetActive(value: true);
																		BiBeiInfo.SetActive(value: false);
																		BiBeiPanel.SetActive(value: false);
																		CardsPanel.SetActive(value: true);
																		StartBtn.gameObject.SetActive(value: true);
																		ScoreBtn.gameObject.SetActive(value: false);
																		ScoreBtn.interactable = true;
																		StartBtn.GetComponent<Button>().interactable = true;
																		StartBtn.enabled = true;
																		BetBtn.interactable = true;
																		PickUpBtn.interactable = true;
																		PickUpBtn2.interactable = true;
																		StopBtn.interactable = true;
																		ReturnBtn.interactable = true;
																		QueryBtn.interactable = true;
																		SideBarBtn.gameObject.SetActive(value: true);
																		SideBar.SetActive(value: true);
																		SideBar2.SetActive(value: false);
																		StartBtn.transform.GetChild(1).gameObject.SetActive(value: true);
																	}
																}
