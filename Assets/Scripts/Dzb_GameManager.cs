using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dzb_GameManager : Dzb_Singleton<Dzb_GameManager>
{
	public Sprite CardBack;

	public Sprite[] CardSprites;

	public Sprite PCardBack;

	public Sprite[] PCardSprites;

	private Button SideBarBtn;

	private Button BetBtn;

	private Button AwardsBtn;

	private Button ActivityBtn;

	private Button QueryBtn;

	private EventTrigger StartBtn;

	private Button CancelBtn;

	private Button ScoreBtn;

	private Button SetBtn;

	private GameObject SideBar;

	private Button PickUpBtn;

	private Button TurntableBtn;

	private Button StopBtn;

	private Button ReturnBtn;

	private GameObject WaitAnime;

	private GameObject WaitAnime1;

	private GameObject WaitAnime2;

	private GameObject CardsPanel;

	private Text CountPickUp;

	private Text Score;

	private Text Bet;

	private Text GameType;

	private Text SeatID;

	private GameObject[] TextGroup;

	private Text[] ScoreGroup;

	private GameObject Heart;

	private GameObject Diamond;

	private Text HintText;

	private Text BetHint;

	private Button ClickPanel;

	private GameObject SetPanel;

	private GameObject PickUpPanel;

	private GameObject AwardsPanel;

	private GameObject ActivityPanel;

	private GameObject QueryPanel;

	private GameObject TurntablePanel;

	private GameObject StopPanel;

	private GameObject BaoXiang;

	public Dzb_LoopScrollView _loopScrollView;

	public GameObject _Content;

	private Coroutine _coDrawCard;

	private Coroutine _coShowEffect;

	private Coroutine _coScoreEffect;

	private Coroutine _coAutoGame;

	public int BetValue;

	public int PickUpNum;

	public int PickUpScore;

	public int LockNum;

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

	private int[] RateGroup = new int[10];

	public float FlickerTime;

	public int HeartNum;

	public int ZuanShiNum;

	public int WinPos = -1;

	public float WinTime;

	public int DrawSpeed = 5;

	private bool IsSwitch;

	private int SwitchNum;

	private bool ISDrawStop;

	private bool IsFrist = true;

	private void Awake()
	{
		Dzb_Singleton<Dzb_GameManager>.SetInstance(this);
		SideBarBtn = base.transform.Find("SideBarBtn").GetComponent<Button>();
		WaitAnime1 = base.transform.Find("WaitAnime1").gameObject;
		WaitAnime2 = base.transform.Find("WaitAnime2").gameObject;
		BetBtn = base.transform.Find("BetBtn").GetComponent<Button>();
		StartBtn = base.transform.Find("StartBtn").GetComponent<EventTrigger>();
		CancelBtn = base.transform.Find("CancelBtn").GetComponent<Button>();
		ScoreBtn = base.transform.Find("ScoreBtn").GetComponent<Button>();
		SetBtn = base.transform.Find("SetBtn").GetComponent<Button>();
		AwardsBtn = base.transform.Find("AwardsBtn").GetComponent<Button>();
		ActivityBtn = base.transform.Find("ActivityBtn").GetComponent<Button>();
		QueryBtn = base.transform.Find("QueryBtn").GetComponent<Button>();
		SideBar = base.transform.Find("SideBar").gameObject;
		PickUpBtn = SideBar.transform.Find("PickUpBtn").GetComponent<Button>();
		TurntableBtn = SideBar.transform.Find("TurntableBtn").GetComponent<Button>();
		StopBtn = SideBar.transform.Find("StopBtn").GetComponent<Button>();
		ReturnBtn = SideBar.transform.Find("ReturnBtn").GetComponent<Button>();
		CardsPanel = base.transform.Find("CardsPanel").gameObject;
		CountPickUp = base.transform.Find("GameInfo/CountPickUp").GetComponent<Text>();
		Score = base.transform.Find("GameInfo/Score").GetComponent<Text>();
		Bet = base.transform.Find("GameInfo/Bet").GetComponent<Text>();
		GameType = base.transform.Find("GameInfo/GameType").GetComponent<Text>();
		SeatID = base.transform.Find("GameInfo/SeatID").GetComponent<Text>();
		TextGroup = new GameObject[10];
		for (int i = 0; i < 10; i++)
		{
			TextGroup[i] = base.transform.Find("GameInfo/TextGroup").GetChild(i).gameObject;
		}
		ScoreGroup = new Text[10];
		for (int j = 0; j < 10; j++)
		{
			ScoreGroup[j] = base.transform.Find("GameInfo/ScoreGroup").GetChild(j).GetComponent<Text>();
		}
		Heart = base.transform.Find("GameInfo/Heart").gameObject;
		Diamond = base.transform.Find("GameInfo/Diamond").gameObject;
		HintText = base.transform.Find("HintText").GetComponent<Text>();
		BetHint = base.transform.Find("BetHint").GetComponent<Text>();
		ClickPanel = base.transform.Find("ClickPanel").GetComponent<Button>();
		SetPanel = base.transform.Find("SetPanel").gameObject;
		PickUpPanel = base.transform.Find("PickUpPanel").gameObject;
		AwardsPanel = base.transform.Find("AwardsPanel").gameObject;
		ActivityPanel = base.transform.Find("ActivityPanel").gameObject;
		QueryPanel = base.transform.Find("QueryPanel").gameObject;
		TurntablePanel = base.transform.Find("TurntablePanel").gameObject;
		StopPanel = base.transform.Find("StopPanel").gameObject;
		BaoXiang = base.transform.Find("BaoXiang").gameObject;
		_loopScrollView = TurntablePanel.transform.Find("ScrollView").GetComponent<Dzb_LoopScrollView>();
		_Content = TurntablePanel.transform.Find("ScrollView/Viewport/Content").gameObject;
	}

	private void Start()
	{
		Init();
		AddListener();
		StartCoroutine(FlickerEffect());
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			ClearCards();
			ResetCards();
			MyCards = new int[5]
			{
				0,
				1,
				15,
				52,
				17
			};
			for (int i = 0; i < CardsPanel.transform.childCount; i++)
			{
				CardsPanel.transform.GetChild(i).GetComponent<Dzb_Poker>().PokerNum = MyCards[i];
			}
			if (_coDrawCard != null)
			{
				StopCoroutine(_coDrawCard);
				_coDrawCard = null;
			}
			_coDrawCard = StartCoroutine(DrawCard_IE());
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.X))
		{
			MyCards = new int[5]
			{
				52,
				1,
				3,
				4,
				52
			};
			for (int j = 0; j < CardsPanel.transform.childCount; j++)
			{
				CardsPanel.transform.GetChild(j).GetComponent<Dzb_Poker>().PokerNum = MyCards[j];
			}
			if (_coDrawCard != null)
			{
				StopCoroutine(_coDrawCard);
				_coDrawCard = null;
			}
			_coDrawCard = StartCoroutine(DrawCard_IE());
		}
		if (!Input.GetKeyDown(KeyCode.Z))
		{
			return;
		}
		Dzb_CardType dzb_CardType = new Dzb_CardType();
		dzb_CardType.AutoReserveCards(MyCards);
		int[] heldCards = dzb_CardType.GetHeldCards();
		for (int k = 0; k < CardsPanel.transform.childCount; k++)
		{
			for (int l = 0; l < heldCards.Length; l++)
			{
				if (heldCards[l] == CardsPanel.transform.GetChild(k).GetComponent<Dzb_Poker>().PokerNum)
				{
					CardsPanel.transform.GetChild(k).GetComponent<Dzb_Poker>().SetHeld();
				}
			}
		}
		for (int m = 0; m < CardsPanel.transform.childCount; m++)
		{
			if (CardsPanel.transform.GetChild(m).GetComponent<Dzb_Poker>().PokerNum >= 52)
			{
				CardsPanel.transform.GetChild(m).GetComponent<Dzb_Poker>().SetHeld();
			}
		}
	}

	private void Init()
	{
		Dzb_MySqlConnection.curView = "DzbGame";
		SideBarBtn.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		PickUpBtn.interactable = true;
		TurntableBtn.interactable = true;
		StopBtn.interactable = true;
		BetBtn.interactable = true;
		ReturnBtn.interactable = true;
		StartBtn.gameObject.SetActive(value: true);
		CancelBtn.gameObject.SetActive(value: false);
		ScoreBtn.gameObject.SetActive(value: false);
		CardsPanel.SetActive(value: false);
		for (int i = 0; i < 10; i++)
		{
			TextGroup[i].SetActive(value: true);
			ScoreGroup[i].gameObject.SetActive(value: true);
		}
		GameType.text = Dzb_MySqlConnection.room.Roomname;
		SeatID.text = Dzb_MySqlConnection.seat.id.ToString();
		if (Dzb_MySqlConnection.seat.ViewCard == 0)
		{
			WaitAnime = WaitAnime1;
		}
		else if (Dzb_MySqlConnection.seat.ViewCard == 1)
		{
			WaitAnime = WaitAnime2;
		}
		WaitAnime.SetActive(value: true);
		for (int j = 0; j < CardsPanel.transform.childCount; j++)
		{
			if (Dzb_MySqlConnection.seat.ViewCard == 0)
			{
				CardsPanel.transform.GetChild(j).GetComponent<Dzb_Poker>()._cardType = Dzb_Poker.ImageType.Cards;
			}
			else if (Dzb_MySqlConnection.seat.ViewCard == 1)
			{
				CardsPanel.transform.GetChild(j).GetComponent<Dzb_Poker>()._cardType = Dzb_Poker.ImageType.Fruit;
			}
		}
		IsOncCards = false;
		IsGetOneCards = false;
		IsTwoCards = false;
		IsGetTwoCards = false;
		IsDraw = false;
		IsWin = false;
		WinPos = -1;
		IsgetWinScore = false;
		Bet.text = "0";
		BetValue = Dzb_MySqlConnection.room.MinBet;
		if (Dzb_MySqlConnection.room.RoomType == 4)
		{
			Score.text = Dzb_MySqlConnection.user.expeScore.ToString();
		}
		else
		{
			Score.text = Dzb_MySqlConnection.user.gameScore.ToString();
		}
		CountPickUp.text = PickUpNum.ToString();
		HintText.gameObject.SetActive(value: false);
		BetHint.gameObject.SetActive(value: true);
		BetHint.text = "PLAY " + Dzb_MySqlConnection.room.MinBet + " TO " + Dzb_MySqlConnection.room.MaxBet;
		HintText.text = "<color=\"#45D042FF\">押 注</color> 或 <color=\"#E7E65AFF\">开 始</color>";
		ClickPanel.gameObject.SetActive(value: false);
		SideBar.GetComponent<RectTransform>().anchoredPosition = Vector2.left * SideBar.GetComponent<RectTransform>().rect.width;
		RateGroup[9] = Dzb_MySqlConnection.seat.YiDuiOdds;
		RateGroup[8] = Dzb_MySqlConnection.seat.ErDuiOdds;
		RateGroup[7] = Dzb_MySqlConnection.seat.SanTiaoOdds;
		RateGroup[6] = Dzb_MySqlConnection.seat.ShunZiOdds;
		RateGroup[5] = Dzb_MySqlConnection.seat.TongHuaOdds;
		RateGroup[4] = Dzb_MySqlConnection.seat.HuLuOdds;
		RateGroup[3] = Dzb_MySqlConnection.seat.SiTiaoOdds;
		RateGroup[2] = Dzb_MySqlConnection.seat.TongHuaShunOdds;
		RateGroup[1] = Dzb_MySqlConnection.seat.TongHuaDaShunOdds;
		RateGroup[0] = Dzb_MySqlConnection.seat.WuTiaoOdds;
		StopPanel.transform.Find("0/Image/Text").GetComponent<Text>().text = "2小时（消耗" + 2 * Dzb_MySqlConnection.seat.LiuJiScore / Dzb_MySqlConnection.seat.LiuJiTime + "分）";
		StopPanel.transform.Find("1/Image/Text").GetComponent<Text>().text = "4小时（消耗" + 4 * Dzb_MySqlConnection.seat.LiuJiScore / Dzb_MySqlConnection.seat.LiuJiTime + "分）";
		StopPanel.transform.Find("2/Image/Text").GetComponent<Text>().text = "6小时（消耗" + 6 * Dzb_MySqlConnection.seat.LiuJiScore / Dzb_MySqlConnection.seat.LiuJiTime + "分）";
		StopPanel.transform.Find("3/Image/Text").GetComponent<Text>().text = "8小时（消耗" + 8 * Dzb_MySqlConnection.seat.LiuJiScore / Dzb_MySqlConnection.seat.LiuJiTime + "分）";
		ResetInfo();
		UpdateScoreGroup(1);
		UpdateHeartAndDiamond(Dzb_MySqlConnection.seat.HeartNum, Dzb_MySqlConnection.seat.ZuanShiNum, 0);
		DrawSpeed = Dzb_MySqlConnection.seat.OpenCardSpeed;
		SetPanel.SetActive(value: false);
		PickUpPanel.SetActive(value: false);
	}

	private void AddListener()
	{
		SideBarBtn.onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			SwitchNum = 0;
			StartCoroutine(SideBarSwitch());
		});
		BetBtn.onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			if (!IsStart)
			{
				WaitAnime.SetActive(value: false);
				CardsPanel.SetActive(value: true);
				IsStart = true;
				Bet.text = BetValue.ToString();
			}
			else
			{
				BetValue += 50;
				if (BetValue > Dzb_MySqlConnection.room.MaxBet)
				{
					BetValue = Dzb_MySqlConnection.room.MinBet;
				}
				Bet.text = BetValue.ToString();
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
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			StartBtn.gameObject.SetActive(value: true);
			CancelBtn.gameObject.SetActive(value: false);
			IsAuto = false;
		});
		ScoreBtn.onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			ScoreBtn.interactable = false;
			if (_coScoreEffect != null)
			{
				StopCoroutine(_coScoreEffect);
				_coScoreEffect = null;
			}
			_coScoreEffect = StartCoroutine(ScoreEffect());
		});
		SetBtn.onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			SetPanel.SetActive(value: true);
		});
		AwardsBtn.onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			Dzb_SendMsgManager.Send_getDaJiangBan();
		});
		ActivityBtn.onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
		});
		QueryBtn.onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			Dzb_SendMsgManager.Send_getDeskHistory(Dzb_MySqlConnection.seat.id);
		});
		PickUpBtn.onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			PickUpPanel.SetActive(value: true);
			if (Dzb_MySqlConnection.room.RoomType == 4)
			{
				PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeScore.ToString();
				PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeGold.ToString();
			}
			else
			{
				PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameScore.ToString();
				PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameGold.ToString();
			}
		});
		TurntableBtn.onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			Dzb_SendMsgManager.Send_GetRoomInfo(Dzb_MySqlConnection.seat.roomId);
		});
		StopBtn.onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			StopPanel.SetActive(value: true);
		});
		ReturnBtn.onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			Dzb_Singleton<Dzb_AlertDialog>.GetInstance().ShowDialog("是否退出座位？", showOkCancel: true, delegate
			{
				if (Dzb_Singleton<Dzb_GameInfo>.GetInstance()._IsTest)
				{
					SceneManager.LoadSceneAsync("DzbHall");
				}
				else
				{
					Dzb_SendMsgManager.Send_LeaveDesk(Dzb_Singleton<Dzb_GameInfo>.GetInstance().DeskId);
				}
			});
		});
		ClickPanel.onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			SwitchNum = 1;
			StartCoroutine(SideBarSwitch());
		});
		SetPanel.transform.Find("ClosePanel").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			SetPanel.SetActive(value: false);
		});
		SetPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			SetPanel.SetActive(value: false);
		});
		PickUpPanel.transform.Find("ClosePanel").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			PickUpPanel.SetActive(value: false);
		});
		PickUpPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			PickUpPanel.SetActive(value: false);
		});
		PickUpPanel.transform.Find("CFBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			Dzb_SendMsgManager.Send_UserCoinOut(Dzb_MySqlConnection.seat.onceExchangeValue * Dzb_MySqlConnection.seat.exchange);
		});
		PickUpPanel.transform.Find("QFBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			Dzb_SendMsgManager.Send_UserCoinIn(Dzb_MySqlConnection.seat.onceExchangeValue);
		});
		StopPanel.transform.Find("ClosePanel").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			StopPanel.SetActive(value: false);
		});
		StopPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			StopPanel.SetActive(value: false);
		});
		StopPanel.transform.Find("0").GetComponent<Toggle>().onValueChanged.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			LockNum = 0;
		});
		StopPanel.transform.Find("1").GetComponent<Toggle>().onValueChanged.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			LockNum = 1;
		});
		StopPanel.transform.Find("2").GetComponent<Toggle>().onValueChanged.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			LockNum = 2;
		});
		StopPanel.transform.Find("3").GetComponent<Toggle>().onValueChanged.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			LockNum = 3;
		});
		StopPanel.transform.Find("LockBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			if (Dzb_MySqlConnection.room.RoomType == 4)
			{
				if (Dzb_MySqlConnection.user.expeScore < (LockNum + 1) * 2 * (Dzb_MySqlConnection.seat.LiuJiScore / Dzb_MySqlConnection.seat.LiuJiTime) || Dzb_MySqlConnection.user.expeScore == 0)
				{
					PickUpPanel.SetActive(value: true);
					PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeScore.ToString();
					PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeGold.ToString();
					return;
				}
			}
			else if (Dzb_MySqlConnection.user.gameScore < (LockNum + 1) * 2 * (Dzb_MySqlConnection.seat.LiuJiScore / Dzb_MySqlConnection.seat.LiuJiTime) || Dzb_MySqlConnection.user.gameScore == 0)
			{
				PickUpPanel.SetActive(value: true);
				PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameScore.ToString();
				PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameGold.ToString();
				return;
			}
			Dzb_SendMsgManager.Send_LogoutKeep(Dzb_MySqlConnection.seat.id, LockNum);
		});
		TurntablePanel.transform.Find("ClosePanel").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			TurntablePanel.SetActive(value: false);
		});
		TurntablePanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			TurntablePanel.SetActive(value: false);
		});
		AwardsPanel.transform.Find("ClosePanel").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			AwardsPanel.SetActive(value: false);
		});
		AwardsPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			AwardsPanel.SetActive(value: false);
		});
		QueryPanel.transform.Find("ClosePanel").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			QueryPanel.SetActive(value: false);
		});
		QueryPanel.transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			QueryPanel.SetActive(value: false);
		});
		BaoXiang.transform.Find("ClosePanel").GetComponent<Button>().onClick.AddListener(delegate
		{
			Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
			BaoXiang.SetActive(value: false);
		});
	}

	public void UpdateScore()
	{
		if (Dzb_MySqlConnection.room.RoomType == 4)
		{
			Score.text = Dzb_MySqlConnection.user.expeScore.ToString();
			PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeScore.ToString();
			PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeGold.ToString();
			if (PickUpScore != Dzb_MySqlConnection.user.expeScore)
			{
				PickUpNum += Dzb_MySqlConnection.user.expeScore - PickUpScore;
				PickUpScore = Dzb_MySqlConnection.user.expeScore;
			}
			CountPickUp.text = PickUpNum.ToString();
		}
		else
		{
			Score.text = Dzb_MySqlConnection.user.gameScore.ToString();
			PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameScore.ToString();
			PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameGold.ToString();
			if (PickUpScore != Dzb_MySqlConnection.user.gameScore)
			{
				PickUpNum += Dzb_MySqlConnection.user.gameScore - PickUpScore;
				PickUpScore = Dzb_MySqlConnection.user.gameScore;
			}
			CountPickUp.text = PickUpNum.ToString();
		}
	}

	public void ShowScore(int score)
	{
		Score.text = score.ToString();
	}

	public void UpdateScoreGroup(int value)
	{
		for (int i = 0; i < 10; i++)
		{
			ScoreGroup[i].text = (RateGroup[i] * value).ToString();
		}
	}

	public void UpdateHeartAndDiamond(int heart, int zhuanshi, int box)
	{
		for (int i = 0; i < Heart.transform.childCount; i++)
		{
			if (i < heart)
			{
				Heart.transform.GetChild(i).GetChild(0).gameObject.SetActive(value: true);
			}
			else
			{
				Heart.transform.GetChild(i).GetChild(0).gameObject.SetActive(value: false);
			}
		}
		for (int j = 0; j < Diamond.transform.childCount; j++)
		{
			if (j < zhuanshi)
			{
				Diamond.transform.GetChild(j).GetChild(0).gameObject.SetActive(value: true);
			}
			else
			{
				Diamond.transform.GetChild(j).GetChild(0).gameObject.SetActive(value: false);
			}
		}
		if (box > 0)
		{
			BaoXiang.SetActive(value: true);
			BaoXiang.transform.Find("Score/Text").GetComponent<Text>().text = "获得" + box + string.Empty;
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

	public void UpdateAwards(object[] jiangs)
	{
		AwardsPanel.SetActive(value: true);
		GameObject gameObject = AwardsPanel.transform.Find("Scroll View/Viewport/Content").gameObject;
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			if (i < jiangs.Length)
			{
				Dictionary<string, object> dictionary = jiangs[i] as Dictionary<string, object>;
				gameObject.transform.GetChild(i).gameObject.SetActive(value: true);
				gameObject.transform.GetChild(i).Find("Game").GetComponent<Text>()
					.text = dictionary["game"].ToString();
					gameObject.transform.GetChild(i).Find("Name").GetComponent<Text>()
						.text = dictionary["username"].ToString();
						string empty = string.Empty;
						gameObject.transform.GetChild(i).Find("Num").GetComponent<Text>()
							.text = empty + dictionary["desk"].ToString() + "号机";
							gameObject.transform.GetChild(i).Find("Jiang").GetComponent<Text>()
								.text = dictionary["JiangXiang"].ToString();
								gameObject.transform.GetChild(i).Find("Score").GetComponent<Text>()
									.text = dictionary["winScore"].ToString();
								}
								else
								{
									gameObject.transform.GetChild(i).gameObject.SetActive(value: false);
								}
							}
						}

						public void UpdateQuery(int[] historyscore)
						{
							QueryPanel.SetActive(value: true);
							try
							{
								QueryPanel.transform.Find("BG/ScoreGroup").GetChild(0).GetComponent<Text>()
									.text = historyscore[10].ToString();
									QueryPanel.transform.Find("BG/ScoreGroup").GetChild(1).GetComponent<Text>()
										.text = historyscore[9].ToString();
										QueryPanel.transform.Find("BG/ScoreGroup").GetChild(2).GetComponent<Text>()
											.text = historyscore[8].ToString();
											QueryPanel.transform.Find("BG/ScoreGroup").GetChild(3).GetComponent<Text>()
												.text = historyscore[7].ToString();
												QueryPanel.transform.Find("BG/ScoreGroup").GetChild(4).GetComponent<Text>()
													.text = historyscore[6].ToString();
													QueryPanel.transform.Find("BG/ScoreGroup").GetChild(5).GetComponent<Text>()
														.text = historyscore[5].ToString();
														QueryPanel.transform.Find("BG/ScoreGroup").GetChild(6).GetComponent<Text>()
															.text = historyscore[4].ToString();
															QueryPanel.transform.Find("BG/ScoreGroup").GetChild(7).GetComponent<Text>()
																.text = historyscore[3].ToString();
																QueryPanel.transform.Find("BG/ScoreGroup").GetChild(8).GetComponent<Text>()
																	.text = historyscore[2].ToString();
																	QueryPanel.transform.Find("BG/ScoreGroup").GetChild(9).GetComponent<Text>()
																		.text = historyscore[1].ToString();
																		QueryPanel.transform.Find("BG/ScoreGroup").GetChild(10).GetComponent<Text>()
																			.text = (historyscore[11] + historyscore[12]).ToString();
																			QueryPanel.transform.Find("BG/ScoreGroup").GetChild(11).GetComponent<Text>()
																				.text = historyscore[11].ToString();
																				QueryPanel.transform.Find("BG/ScoreGroup").GetChild(12).GetComponent<Text>()
																					.text = historyscore[12].ToString();
																				}
																				catch (Exception)
																				{
																					for (int i = 0; i < 13; i++)
																					{
																						QueryPanel.transform.Find("BG/ScoreGroup").GetChild(0).GetComponent<Text>()
																							.text = "0";
																						}
																					}
																				}

																				private IEnumerator FlickerEffect()
																				{
																					bool IsShow = false;
																					while (true)
																					{
																						if (IsDraw)
																						{
																							if (FlickerTime >= 0.5f)
																							{
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
																							FlickerTime = 0f;
																							BetHint.gameObject.SetActive(!BetHint.gameObject.activeSelf);
																							if (HintText.gameObject.activeSelf)
																							{
																								HintText.gameObject.SetActive(value: false);
																							}
																						}
																						if (!IsWin && (IsGetOneCards || IsGetTwoCards))
																						{
																							if (WinPos >= 0 && WinTime >= 0.5f)
																							{
																								WinTime = 0f;
																								TextGroup[WinPos].SetActive(IsShow);
																								ScoreGroup[WinPos].gameObject.SetActive(IsShow);
																								IsShow = !IsShow;
																							}
																							FlickerTime += 0.02f;
																							WinTime = FlickerTime;
																						}
																						else if (WinPos != -1)
																						{
																							TextGroup[WinPos].SetActive(value: true);
																							ScoreGroup[WinPos].gameObject.SetActive(value: true);
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
																					Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
																					if (!IsStartBtnClick)
																					{
																						return;
																					}
																					IsStartBtnClick = false;
																					if (!IsOncCards)
																					{
																						if (Dzb_MySqlConnection.room.RoomType == 4)
																						{
																							if (Dzb_MySqlConnection.user.expeScore < BetValue || Dzb_MySqlConnection.user.expeScore == 0)
																							{
																								PickUpPanel.SetActive(value: true);
																								PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeScore.ToString();
																								PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeGold.ToString();
																								return;
																							}
																						}
																						else if (Dzb_MySqlConnection.user.gameScore < BetValue || Dzb_MySqlConnection.user.gameScore == 0)
																						{
																							PickUpPanel.SetActive(value: true);
																							PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameScore.ToString();
																							PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameGold.ToString();
																							return;
																						}
																					}
																					if (!IsStart)
																					{
																						WaitAnime.SetActive(value: false);
																						CardsPanel.SetActive(value: true);
																						IsStart = true;
																						Bet.text = BetValue.ToString();
																					}
																					if (!IsAuto)
																					{
																						if (!IsOncCards)
																						{
																							IsOncCards = true;
																							StartBtn.GetComponent<Button>().interactable = false;
																							StartBtn.enabled = false;
																							Dzb_SendMsgManager.Send_OneCard(Dzb_MySqlConnection.seat.id, BetValue);
																						}
																						else if (IsGetOneCards && !IsTwoCards)
																						{
																							IsTwoCards = true;
																							StartBtn.GetComponent<Button>().interactable = false;
																							StartBtn.enabled = false;
																							Dzb_SendMsgManager.Send_TwiceCard(Dzb_MySqlConnection.seat.id, GetHeldCards());
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
																							if (Dzb_MySqlConnection.room.RoomType == 4)
																							{
																								if (Dzb_MySqlConnection.user.expeScore < BetValue || Dzb_MySqlConnection.user.expeScore == 0)
																								{
																									PickUpPanel.SetActive(value: true);
																									PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeScore.ToString();
																									PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeGold.ToString();
																									IsStartBtnClick = false;
																									yield break;
																								}
																							}
																							else if (Dzb_MySqlConnection.user.gameScore < BetValue || Dzb_MySqlConnection.user.gameScore == 0)
																							{
																								PickUpPanel.SetActive(value: true);
																								PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameScore.ToString();
																								PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameGold.ToString();
																								IsStartBtnClick = false;
																								yield break;
																							}
																							if (!IsAuto)
																							{
																								WaitAnime.SetActive(value: false);
																								CardsPanel.SetActive(value: true);
																								IsAuto = true;
																								Bet.text = BetValue.ToString();
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
																					if (IsAuto)
																					{
																						while (IsAuto)
																						{
																							if (!IsOncCards)
																							{
																								if (Dzb_MySqlConnection.room.RoomType == 4)
																								{
																									if (Dzb_MySqlConnection.user.expeScore < BetValue || Dzb_MySqlConnection.user.expeScore == 0)
																									{
																										PickUpPanel.SetActive(value: true);
																										PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeScore.ToString();
																										PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeGold.ToString();
																										IsAuto = false;
																									}
																								}
																								else if (Dzb_MySqlConnection.user.gameScore < BetValue || Dzb_MySqlConnection.user.gameScore == 0)
																								{
																									PickUpPanel.SetActive(value: true);
																									PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameScore.ToString();
																									PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameGold.ToString();
																									IsAuto = false;
																								}
																							}
																							if (!IsAuto)
																							{
																								StartBtn.gameObject.SetActive(value: true);
																								CancelBtn.gameObject.SetActive(value: false);
																								IsStartBtnClick = false;
																								yield break;
																							}
																							if (!IsOncCards)
																							{
																								IsOncCards = true;
																								StartBtn.GetComponent<Button>().interactable = false;
																								StartBtn.enabled = false;
																								Dzb_SendMsgManager.Send_OneCard(Dzb_MySqlConnection.seat.id, BetValue);
																							}
																							else if (IsGetOneCards && !IsTwoCards)
																							{
																								yield return new WaitForSeconds(GetDrawWaitTime() + 1f);
																								if (ISDrawStop)
																								{
																									do
																									{
																										yield return new WaitForSeconds(0.02f);
																									}
																									while (ISDrawStop);
																								}
																								if (!IsAuto)
																								{
																									IsStartBtnClick = false;
																									yield break;
																								}
																								IsTwoCards = true;
																								StartBtn.GetComponent<Button>().interactable = false;
																								StartBtn.enabled = false;
																								Dzb_SendMsgManager.Send_TwiceCard(Dzb_MySqlConnection.seat.id, GetHeldCards());
																							}
																							else if (IsGetTwoCards && IsgetWinScore)
																							{
																								yield return new WaitForSeconds(GetDrawWaitTime() + 1f);
																								if (ISDrawStop)
																								{
																									do
																									{
																										yield return new WaitForSeconds(0.02f);
																									}
																									while (ISDrawStop);
																								}
																								if (!IsAuto)
																								{
																									IsStartBtnClick = false;
																									yield break;
																								}
																								IsgetWinScore = false;
																								UnityEngine.Debug.Log("自动得分");
																								if (userWin > 0)
																								{
																									ScoreBtn.interactable = false;
																									ScoreBtn.gameObject.SetActive(value: true);
																									if (_coScoreEffect != null)
																									{
																										StopCoroutine(_coScoreEffect);
																										_coScoreEffect = null;
																									}
																									_coScoreEffect = StartCoroutine(ScoreEffect());
																								}
																							}
																							else if (userWin <= 0)
																							{
																							}
																							yield return new WaitForSeconds(0.02f);
																						}
																					}
																					IsStartBtnClick = false;
																				}

																				private IEnumerator NewAutoStartGame()
																				{
																					while (IsStartBtnClick)
																					{
																						if (StartBtnTime >= 2f)
																						{
																							if (Dzb_MySqlConnection.room.RoomType == 4)
																							{
																								if (Dzb_MySqlConnection.user.expeScore < BetValue || Dzb_MySqlConnection.user.expeScore == 0)
																								{
																									PickUpPanel.SetActive(value: true);
																									PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeScore.ToString();
																									PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeGold.ToString();
																									IsStartBtnClick = false;
																									yield break;
																								}
																							}
																							else if (Dzb_MySqlConnection.user.gameScore < BetValue || Dzb_MySqlConnection.user.gameScore == 0)
																							{
																								PickUpPanel.SetActive(value: true);
																								PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameScore.ToString();
																								PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameGold.ToString();
																								IsStartBtnClick = false;
																								yield break;
																							}
																							if (!IsAuto)
																							{
																								WaitAnime.SetActive(value: false);
																								CardsPanel.SetActive(value: true);
																								IsAuto = true;
																								Bet.text = BetValue.ToString();
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
																					while (true)
																					{
																						if (!IsOncCards)
																						{
																							if (Dzb_MySqlConnection.room.RoomType == 4)
																							{
																								if (Dzb_MySqlConnection.user.expeScore < BetValue || Dzb_MySqlConnection.user.expeScore == 0)
																								{
																									PickUpPanel.SetActive(value: true);
																									PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeScore.ToString();
																									PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.expeGold.ToString();
																									IsAuto = false;
																								}
																							}
																							else if (Dzb_MySqlConnection.user.gameScore < BetValue || Dzb_MySqlConnection.user.gameScore == 0)
																							{
																								PickUpPanel.SetActive(value: true);
																								PickUpPanel.transform.Find("ScoreText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameScore.ToString();
																								PickUpPanel.transform.Find("DiamondText/Text").GetComponent<Text>().text = Dzb_MySqlConnection.user.gameGold.ToString();
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
																								Dzb_SendMsgManager.Send_OneCard(Dzb_MySqlConnection.seat.id, BetValue);
																							}
																							else if (IsGetOneCards)
																							{
																								if (!CardsPanel.transform.GetChild(drawPos).GetComponent<Dzb_Poker>().IsHeld)
																								{
																									if (!CardsPanel.transform.GetChild(drawPos).GetComponent<Dzb_Poker>().IsPlayAudio)
																									{
																										CardsPanel.transform.GetChild(drawPos).GetComponent<Dzb_Poker>().IsPlayAudio = true;
																										Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound_Hold1(drawPos);
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
																											CardsPanel.transform.GetChild(drawPos).GetComponent<Dzb_Poker>().UpdateCard();
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
																						Dzb_CardType cardtype = new Dzb_CardType();
																						cardtype.AutoReserveCards(cardtype.CardsSort2(MyCards));
																						int[] Gcards = cardtype.GetHeldCards();
																						for (int i = 0; i < CardsPanel.transform.childCount; i++)
																						{
																							for (int j = 0; j < Gcards.Length; j++)
																							{
																								if (Gcards[j] == CardsPanel.transform.GetChild(i).GetComponent<Dzb_Poker>().PokerNum)
																								{
																									CardsPanel.transform.GetChild(i).GetComponent<Dzb_Poker>().SetHeld();
																								}
																							}
																						}
																						for (int k = 0; k < CardsPanel.transform.childCount; k++)
																						{
																							if (CardsPanel.transform.GetChild(k).GetComponent<Dzb_Poker>().PokerNum >= 52)
																							{
																								CardsPanel.transform.GetChild(k).GetComponent<Dzb_Poker>().SetHeld();
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
																								Dzb_SendMsgManager.Send_TwiceCard(Dzb_MySqlConnection.seat.id, GetHeldCards());
																							}
																							else if (IsGetTwoCards)
																							{
																								UnityEngine.Debug.Log("第二手翻牌中");
																								if (!CardsPanel.transform.GetChild(drawPos).GetComponent<Dzb_Poker>().IsHeld)
																								{
																									if (!CardsPanel.transform.GetChild(drawPos).GetComponent<Dzb_Poker>().IsPlayAudio)
																									{
																										CardsPanel.transform.GetChild(drawPos).GetComponent<Dzb_Poker>().IsPlayAudio = true;
																										Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound_Hold1(drawPos);
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
																											CardsPanel.transform.GetChild(drawPos).GetComponent<Dzb_Poker>().UpdateCard();
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
																								Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("fail");
																								yield return new WaitForSeconds(1f);
																								ResetCards();
																								IsOncCards = false;
																								IsGetOneCards = false;
																								IsTwoCards = false;
																								IsGetTwoCards = false;
																								IsDraw = false;
																								ResetInfo();
																								UpdateScoreGroup(1);
																								HintText.text = "<color=\"#45D042FF\">押 注</color> 或 <color=\"#E7E65AFF\">开 始</color>";
																								StartBtn.transform.GetChild(1).gameObject.SetActive(value: true);
																								StartBtn.GetComponent<Button>().interactable = true;
																								StartBtn.enabled = true;
																								PickUpBtn.interactable = true;
																								TurntableBtn.interactable = true;
																								StopBtn.interactable = true;
																								BetBtn.interactable = true;
																								ReturnBtn.interactable = true;
																							}
																							else
																							{
																								Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("suc");
																								StartBtn.gameObject.SetActive(value: false);
																								ScoreBtn.gameObject.SetActive(value: true);
																								IsgetWinScore = true;
																								HintText.text = "<color=\"#45D042FF\">得  分</color>";
																							}
																							yield break;
																						}
																						IsWin = true;
																						if (userWin > 0)
																						{
																							Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("suc");
																							yield return new WaitForSeconds(1f);
																							Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound_Score();
																							while (userWin > 0)
																							{
																								ScoreGroup[WinPos].text = userWin.ToString();
																								if (Dzb_MySqlConnection.room.RoomType == 4)
																								{
																									Score.text = (Dzb_MySqlConnection.user.expeScore - userWin).ToString();
																								}
																								else
																								{
																									Score.text = (Dzb_MySqlConnection.user.gameScore - userWin).ToString();
																								}
																								userWin -= ((userWin <= userVariate) ? (userVariate = userWin) : userVariate);
																								yield return new WaitForSeconds(0.02f);
																							}
																							Dzb_Singleton<Dzb_AudioManager>.GetInstance().StopSound_Score();
																							ScoreGroup[WinPos].text = userWin.ToString();
																							if (Dzb_MySqlConnection.room.RoomType == 4)
																							{
																								Score.text = (Dzb_MySqlConnection.user.expeScore - userWin).ToString();
																							}
																							else
																							{
																								Score.text = (Dzb_MySqlConnection.user.gameScore - userWin).ToString();
																							}
																						}
																						else
																						{
																							Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("fail");
																							yield return new WaitForSeconds(1f);
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
																						ResetInfo();
																						UpdateScoreGroup(1);
																						HintText.text = "<color=\"#45D042FF\">押 注</color> 或 <color=\"#E7E65AFF\">开 始</color>";
																						StartBtn.gameObject.SetActive(value: true);
																						ScoreBtn.gameObject.SetActive(value: false);
																						ScoreBtn.interactable = true;
																						StartBtn.GetComponent<Button>().interactable = true;
																						StartBtn.enabled = true;
																						BetBtn.interactable = true;
																						PickUpBtn.interactable = true;
																						TurntableBtn.interactable = true;
																						StopBtn.interactable = true;
																						ReturnBtn.interactable = true;
																						StartBtn.transform.GetChild(1).gameObject.SetActive(value: true);
																						if (!IsAuto)
																						{
																							break;
																						}
																						yield return new WaitForSeconds(0.5f);
																					}
																					UnityEngine.Debug.Log("跳出来了？");
																					IsStartBtnClick = false;
																				}

																				public void DrawCards()
																				{
																					PickUpBtn.interactable = false;
																					TurntableBtn.interactable = false;
																					StopBtn.interactable = false;
																					BetBtn.interactable = false;
																					ReturnBtn.interactable = false;
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
																						CardsPanel.transform.GetChild(i).GetComponent<Dzb_Poker>().PokerNum = MyCards[i];
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
																					Dzb_CardType cardtype = new Dzb_CardType();
																					cardtype.AutoReserveCards(cardtype.CardsSort2(MyCards));
																					int[] Gcards = cardtype.GetHeldCards();
																					for (int j = 0; j < CardsPanel.transform.childCount; j++)
																					{
																						for (int k = 0; k < Gcards.Length; k++)
																						{
																							if (Gcards[k] == CardsPanel.transform.GetChild(j).GetComponent<Dzb_Poker>().PokerNum)
																							{
																								CardsPanel.transform.GetChild(j).GetComponent<Dzb_Poker>().SetHeld();
																							}
																						}
																					}
																					for (int l = 0; l < CardsPanel.transform.childCount; l++)
																					{
																						if (CardsPanel.transform.GetChild(l).GetComponent<Dzb_Poker>().PokerNum >= 52)
																						{
																							CardsPanel.transform.GetChild(l).GetComponent<Dzb_Poker>().SetHeld();
																						}
																					}
																					HeldEnabled();
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
																						if (!CardsPanel.transform.GetChild(i).GetComponent<Dzb_Poker>().IsHeld)
																						{
																							CardsPanel.transform.GetChild(i).GetComponent<Dzb_Poker>().Reset();
																						}
																					}
																					for (int j = 0; j < CardsPanel.transform.childCount; j++)
																					{
																						CardsPanel.transform.GetChild(j).GetComponent<Dzb_Poker>().PokerNum = MyCards[j];
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
																						Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("fail");
																						yield return new WaitForSeconds(1f);
																						ResetCards();
																						IsOncCards = false;
																						IsGetOneCards = false;
																						IsTwoCards = false;
																						IsGetTwoCards = false;
																						IsDraw = false;
																						ResetInfo();
																						UpdateScoreGroup(1);
																						HintText.text = "<color=\"#45D042FF\">押 注</color> 或 <color=\"#E7E65AFF\">开 始</color>";
																						StartBtn.transform.GetChild(1).gameObject.SetActive(value: true);
																						StartBtn.GetComponent<Button>().interactable = true;
																						StartBtn.enabled = true;
																						PickUpBtn.interactable = true;
																						TurntableBtn.interactable = true;
																						StopBtn.interactable = true;
																						BetBtn.interactable = true;
																						ReturnBtn.interactable = true;
																					}
																					else
																					{
																						Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("suc");
																						StartBtn.gameObject.SetActive(value: false);
																						ScoreBtn.gameObject.SetActive(value: true);
																						IsgetWinScore = true;
																						HintText.text = "<color=\"#45D042FF\">得  分</color>";
																					}
																				}

																				public float GetDrawWaitTime()
																				{
																					float num = 0f;
																					for (int i = 0; i < CardsPanel.transform.childCount; i++)
																					{
																						if (!CardsPanel.transform.GetChild(i).GetComponent<Dzb_Poker>().IsHeld)
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
																						if (!CardsPanel.transform.GetChild(drawPos).GetComponent<Dzb_Poker>().IsHeld)
																						{
																							if (!CardsPanel.transform.GetChild(drawPos).GetComponent<Dzb_Poker>().IsPlayAudio)
																							{
																								CardsPanel.transform.GetChild(drawPos).GetComponent<Dzb_Poker>().IsPlayAudio = true;
																								Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound_Hold1(drawPos);
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
																									CardsPanel.transform.GetChild(drawPos).GetComponent<Dzb_Poker>().UpdateCard();
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
																				}

																				public void ResetCards()
																				{
																					for (int i = 0; i < CardsPanel.transform.childCount; i++)
																					{
																						CardsPanel.transform.GetChild(i).GetComponent<Dzb_Poker>().Reset();
																					}
																				}

																				public void ClearCards()
																				{
																					for (int i = 0; i < CardsPanel.transform.childCount; i++)
																					{
																						CardsPanel.transform.GetChild(i).GetComponent<Dzb_Poker>().Clear();
																					}
																				}

																				public void ResetInfo()
																				{
																					for (int i = 0; i < TextGroup.Length; i++)
																					{
																						TextGroup[i].gameObject.SetActive(value: true);
																					}
																					for (int j = 0; j < ScoreGroup.Length; j++)
																					{
																						ScoreGroup[j].gameObject.SetActive(value: true);
																					}
																				}

																				public void HeldEnabled()
																				{
																					UnityEngine.Debug.Log("开启");
																					for (int i = 0; i < CardsPanel.transform.childCount; i++)
																					{
																						CardsPanel.transform.GetChild(i).GetComponent<Dzb_Poker>().HeldEnabled();
																					}
																				}

																				public void HeldDisable()
																				{
																					UnityEngine.Debug.Log("关闭");
																					for (int i = 0; i < CardsPanel.transform.childCount; i++)
																					{
																						CardsPanel.transform.GetChild(i).GetComponent<Dzb_Poker>().HeldDisable();
																					}
																				}

																				public int[] GetHeldCards()
																				{
																					int[] array = new int[CardsPanel.transform.childCount];
																					for (int i = 0; i < CardsPanel.transform.childCount; i++)
																					{
																						if (CardsPanel.transform.GetChild(i).GetComponent<Dzb_Poker>().IsHeld)
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
																					UnityEngine.Debug.Log(WinPos);
																					switch (type)
																					{
																					case 1:
																						WinPos = 9;
																						userVariate = Dzb_MySqlConnection.seat.YiDuiWinSpeed;
																						break;
																					case 2:
																						WinPos = 8;
																						userVariate = Dzb_MySqlConnection.seat.ErDuiWinSpeed;
																						break;
																					case 3:
																						WinPos = 7;
																						userVariate = Dzb_MySqlConnection.seat.SanTiaoWinSpeed;
																						break;
																					case 4:
																						WinPos = 6;
																						userVariate = Dzb_MySqlConnection.seat.ShunZiWinSpeed;
																						break;
																					case 5:
																						WinPos = 5;
																						userVariate = Dzb_MySqlConnection.seat.TongHuaWinSpeed;
																						break;
																					case 6:
																						WinPos = 4;
																						userVariate = Dzb_MySqlConnection.seat.HuLuWinSpeed;
																						break;
																					case 7:
																						WinPos = 3;
																						userVariate = Dzb_MySqlConnection.seat.SiTiaoWinSpeed;
																						break;
																					case 8:
																						WinPos = 2;
																						userVariate = Dzb_MySqlConnection.seat.TongHuaShunWinSpeed;
																						break;
																					case 9:
																						WinPos = 1;
																						userVariate = Dzb_MySqlConnection.seat.TongHuaDaShunWinSpeed;
																						break;
																					case 10:
																						WinPos = 0;
																						userVariate = Dzb_MySqlConnection.seat.WuTiaoWinSpeed;
																						break;
																					default:
																						WinPos = -1;
																						break;
																					}
																					for (int i = 0; i < TextGroup.Length; i++)
																					{
																						TextGroup[i].SetActive(value: true);
																						ScoreGroup[i].gameObject.SetActive(value: true);
																					}
																				}

																				private IEnumerator ScoreEffect()
																				{
																					IsWin = true;
																					UnityEngine.Debug.Log("得分：" + userVariate);
																					Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound_Score();
																					while (userWin > 0)
																					{
																						ScoreGroup[WinPos].text = userWin.ToString();
																						if (Dzb_MySqlConnection.room.RoomType == 4)
																						{
																							Score.text = (Dzb_MySqlConnection.user.expeScore - userWin).ToString();
																						}
																						else
																						{
																							Score.text = (Dzb_MySqlConnection.user.gameScore - userWin).ToString();
																						}
																						userWin -= ((userWin <= userVariate) ? (userVariate = userWin) : userVariate);
																						yield return new WaitForSeconds(0.02f);
																					}
																					Dzb_Singleton<Dzb_AudioManager>.GetInstance().StopSound_Score();
																					ScoreGroup[WinPos].text = userWin.ToString();
																					if (Dzb_MySqlConnection.room.RoomType == 4)
																					{
																						Score.text = (Dzb_MySqlConnection.user.expeScore - userWin).ToString();
																					}
																					else
																					{
																						Score.text = (Dzb_MySqlConnection.user.gameScore - userWin).ToString();
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
																					ResetInfo();
																					UpdateScoreGroup(1);
																					HintText.text = "<color=\"#45D042FF\">押 注</color> 或 <color=\"#E7E65AFF\">开 始</color>";
																					StartBtn.gameObject.SetActive(value: true);
																					ScoreBtn.gameObject.SetActive(value: false);
																					ScoreBtn.interactable = true;
																					StartBtn.GetComponent<Button>().interactable = true;
																					StartBtn.enabled = true;
																					BetBtn.interactable = true;
																					PickUpBtn.interactable = true;
																					TurntableBtn.interactable = true;
																					StopBtn.interactable = true;
																					ReturnBtn.interactable = true;
																					StartBtn.transform.GetChild(1).gameObject.SetActive(value: true);
																				}

																				private void InitRoomCell()
																				{
																					_loopScrollView.SetcacheCount = 2;
																					_loopScrollView.Init(0, UpdateCell);
																					if (Dzb_MySqlConnection.seatList.Count % 9 > 0)
																					{
																						_loopScrollView.UpdateList(Dzb_MySqlConnection.seatList.Count / 9 + 1);
																					}
																					else
																					{
																						_loopScrollView.UpdateList(Dzb_MySqlConnection.seatList.Count / 9);
																					}
																					for (int i = 0; i < _Content.transform.childCount; i++)
																					{
																						for (int j = 0; j < _Content.transform.GetChild(i).childCount; j++)
																						{
																							GameObject obj = _Content.transform.GetChild(i).GetChild(j).gameObject;
																							_Content.transform.GetChild(i).GetChild(j).GetComponent<Button>()
																								.onClick.AddListener(delegate
																								{
																									Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
																									Dzb_Singleton<Dzb_GameInfo>.GetInstance().DeskId = int.Parse(obj.name);
																									for (int k = 0; k < Dzb_MySqlConnection.seatList.Count; k++)
																									{
																										if (Dzb_MySqlConnection.seatList[k].id == Dzb_Singleton<Dzb_GameInfo>.GetInstance().DeskId)
																										{
																											Dzb_MySqlConnection.seat = Dzb_MySqlConnection.seatList[k];
																											break;
																										}
																									}
																									Dzb_SendMsgManager.Send_EnterDesk(Dzb_Singleton<Dzb_GameInfo>.GetInstance().DeskId, 1);
																									Dzb_Singleton<Dzb_AlertDialogText>.GetInstance().ShowDialogText("转台中...请稍后...", 9999f);
																								});
																							}
																						}
																					}

																					public void UpdateSeatCell()
																					{
																						TurntablePanel.SetActive(value: true);
																						if (IsFrist)
																						{
																							InitRoomCell();
																							IsFrist = false;
																						}
																						else if (Dzb_MySqlConnection.seatList.Count % 9 > 0)
																						{
																							_loopScrollView.UpdateList(Dzb_MySqlConnection.seatList.Count / 9 + 1);
																						}
																						else
																						{
																							_loopScrollView.UpdateList(Dzb_MySqlConnection.seatList.Count / 9);
																						}
																					}

																					private void UpdateCell(GameObject obj, int num)
																					{
																						obj.name = num.ToString();
																						for (int i = 0; i < obj.transform.childCount; i++)
																						{
																							if (num * 9 + i < Dzb_MySqlConnection.seatList.Count)
																							{
																								obj.transform.GetChild(i).name = Dzb_MySqlConnection.seatList[num * 9 + i].id.ToString();
																								obj.transform.GetChild(i).Find("Text").GetComponent<Text>()
																									.text = Dzb_MySqlConnection.seatList[num * 9 + i].id.ToString();
																									if (Dzb_MySqlConnection.seatList[num * 9 + i].isKeepTable)
																									{
																										if (Dzb_MySqlConnection.seatList[num * 9 + i].playerid == Dzb_MySqlConnection.user.id)
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
																										obj.transform.GetChild(i).Find("Time").GetComponent<Dzb_Timer>()
																											.Activate(Dzb_MySqlConnection.seatList[num * 9 + i]);
																									}
																									else
																									{
																										obj.transform.GetChild(i).Find("MyLock").gameObject.SetActive(value: false);
																										obj.transform.GetChild(i).Find("Lock").gameObject.SetActive(value: false);
																										obj.transform.GetChild(i).Find("Time").gameObject.SetActive(value: false);
																										if (Dzb_MySqlConnection.seatList[num * 9 + i].playerid != -1)
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
																					}
