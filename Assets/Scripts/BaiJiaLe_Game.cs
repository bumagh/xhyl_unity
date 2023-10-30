using com.miracle9.game.bean;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaiJiaLe_Game : MonoBehaviour
{
	public static BaiJiaLe_Game instance;

	public GameObject ZhuangText;

	public GameObject XianText;

	public GameObject HeText;

	public GameObject ZhuangDuiText;

	public GameObject XianDuiText;

	public GameObject CountDown;

	public GameObject XianCount;

	public GameObject ZhuangCount;

	public GameObject XianHongMin;

	public GameObject XianHongMax;

	public Text ZhuangRate;

	public GameObject[] BetNums;

	public GameObject[] BetChips;

	public GameObject ZhuangWinChips;

	public GameObject ShowZhuangWin;

	public GameObject ShowXianWin;

	public GameObject ShowHeWin;

	public GameObject ShowWinPanel_Zhuang;

	public GameObject ShowWinPanel_Xian;

	public GameObject ShowWinPanel_He;

	public GameObject ShowWinPanel_ZhuangDui;

	public GameObject ShowWinPanel_XianDui;

	public BaiJiaLe_DealerAnime DealerAnime;

	public GameObject ChipPool;

	public GameObject[] BetChipPosition;

	public GameObject[] WinChipPosition;

	public List<GameObject[]> PlayerChipsPosition;

	public GameObject[] RecyclePosition;

	public GameObject[] PlayerPos;

	public Camera MainCamera;

	public GameObject canvas;

	private GameObject[] Players;

	private Button ReturnBtn;

	private Button PlayerListBtn;

	private Button ContinueBtn;

	private Button DmodeBtn;

	private Button FmodeBtn;

	private Button AudioBtn;

	private Button RuleBtn;

	private Button MYBtn;

	private Button LuDanBtn;

	private Button[] Chips;

	private Text NickName;

	private Text Score;

	private GameObject WaitPanel;

	private GameObject LuDanPanel;

	private GameObject GetScorePanel;

	private GameObject NextPanel;

	private GameObject PlayerListPanel;

	private GameObject MsgPanel;

	private int BetNumber = 5;

	public bool BetLock = true;

	public bool ShowPanel;

	public int betTime;

	private Thread TimeThread;

	public int MyselfID = -1;

	public int FirstID = -1;

	public int SecondID = -1;

	public int ThirdID = -1;

	public int FourthID = -1;

	public int LuckyID = -1;

	public int MyselfSeat = -1;

	public int FirstSeat = -1;

	public int SecondSeat = -1;

	public int ThirdSeat = -1;

	public int FourthSeat = -1;

	public int LuckySeat = -1;

	private int InningNum;

	private int ZhuangNum;

	private int XianNum;

	private int HeNum;

	private int ZhuangDuiNum;

	private int XianDuiNum;

	private int[,] NowSeatChips = new int[7, 5];

	private int[,] NewSeatChips = new int[7, 5];

	public GameObject PrefabChip;

	public List<GameObject> LostChips = new List<GameObject>();

	public List<float> LostValues = new List<float>();

	public List<GameObject> WinChips = new List<GameObject>();

	public List<float> WinValues = new List<float>();

	public GameObject[] CameraPos;

	private int CameraType;

	private bool CameraLock;

	private float moveValue;

	public void Awake()
	{
		instance = this;
		BaiJiaLe_GameInfo.IsJoinDesk = true;
	}

	private void Start()
	{
		Camera[] allCameras = Camera.allCameras;
		foreach (Camera camera in allCameras)
		{
			camera.aspect = 1.77777779f;
		}
		Players = new GameObject[canvas.transform.Find("Players").childCount];
		for (int j = 0; j < Players.Length; j++)
		{
			Players[j] = canvas.transform.Find("Players").GetChild(j).gameObject;
		}
		ReturnBtn = canvas.transform.Find("ReturnBtn").GetComponent<Button>();
		PlayerListBtn = canvas.transform.Find("Players/PlayerList/PlayerListBtn").GetComponent<Button>();
		ContinueBtn = canvas.transform.Find("ContinueBtn").GetComponent<Button>();
		DmodeBtn = canvas.transform.Find("DModeBtn").GetComponent<Button>();
		FmodeBtn = canvas.transform.Find("FModeBtn").GetComponent<Button>();
		AudioBtn = canvas.transform.Find("AudioBtn").GetComponent<Button>();
		RuleBtn = canvas.transform.Find("RuleBtn").GetComponent<Button>();
		MYBtn = canvas.transform.Find("MYBtn").GetComponent<Button>();
		LuDanBtn = canvas.transform.Find("LuDanInfo").GetComponent<Button>();
		Chips = new Button[5];
		for (int k = 0; k < 5; k++)
		{
			Chips[k] = canvas.transform.Find("UserInfo/Chip" + k).GetComponent<Button>();
		}
		NickName = canvas.transform.Find("UserInfo/User/NickName").GetComponent<Text>();
		Score = canvas.transform.Find("UserInfo/User/Score").GetComponent<Text>();
		for (int l = 0; l < canvas.transform.Find("Click").transform.childCount; l++)
		{
			canvas.transform.Find("Click").transform.GetChild(l).GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
		}
		WaitPanel = canvas.transform.Find("Panel/WaitPanel").gameObject;
		LuDanPanel = canvas.transform.Find("Panel/LuDanPanel").gameObject;
		GetScorePanel = canvas.transform.Find("Panel/GetScorePanel").gameObject;
		NextPanel = canvas.transform.Find("Panel/NextPanel").gameObject;
		PlayerListPanel = canvas.transform.Find("Panel/PlayerListPanel").gameObject;
		MsgPanel = canvas.transform.Find("Panel/MsgPanel").gameObject;
		AddListener();
		ResetGame();
		ZhuangRate.text = string.Empty;
		BaiJiaLe_Sockets.GetSingleton().SendCheckBindname(int.Parse(BaiJiaLe_GameInfo.getInstance().RoomID), "0");
		for (int m = 0; m < BaiJiaLe_GameInfo.getInstance().GameDeskList.Count; m++)
		{
			if (BaiJiaLe_GameInfo.getInstance().RoomID == BaiJiaLe_GameInfo.getInstance().GameDeskList[m].id.ToString())
			{
				BaiJiaLe_GameInfo.getInstance().GameDesk = BaiJiaLe_GameInfo.getInstance().GameDeskList[m];
				BaiJiaLe_GameInfo.getInstance().Ratio = BaiJiaLe_GameInfo.getInstance().GameDeskList[m].onceExchangeValue;
				LuDanPanel.transform.Find("BG/RoomName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameDeskList[m].name;
				LuDanPanel.transform.Find("BG/XianHong").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameDeskList[m].minBet + "-" + BaiJiaLe_GameInfo.getInstance().GameDeskList[m].maxBet;
				XianHongMin.GetComponent<TextMesh>().text = BaiJiaLe_GameInfo.getInstance().GameDeskList[m].minBet.ToString();
				XianHongMax.GetComponent<TextMesh>().text = BaiJiaLe_GameInfo.getInstance().GameDeskList[m].maxBet.ToString();
				break;
			}
		}
		if (int.Parse(BaiJiaLe_GameInfo.getInstance().GameScore) <= 0)
		{
			ShowPanel = true;
			GetScorePanel.SetActive(value: true);
		}
		betTime = BaiJiaLe_GameInfo.getInstance().BetTime;
		if (betTime >= 0)
		{
			DealerAnime.ShowCards();
			MainCamera.transform.position = CameraPos[0].transform.position;
			MainCamera.transform.rotation = CameraPos[0].transform.rotation;
			BetLock = false;
		}
		else
		{
			BetLock = true;
			Players[0].SetActive(value: true);
			NextPanel.SetActive(value: true);
			CameraType = 1;
			MainCamera.transform.position = CameraPos[2].transform.position;
			MainCamera.transform.rotation = CameraPos[2].transform.rotation;
			CameraLock = false;
		}
		UpdateLuDan();
		UpdateUserInfo();
		if (BaiJiaLe_GameInfo.getInstance().GameSeatList.Count > 0)
		{
			UpdatePlayerList();
		}
		TimeThread = new Thread(TimeRun);
		TimeThread.Start();
		StartCoroutine(GameProcess());
		StartCoroutine(CameraMove_IE());
	}

	private void AddListener()
	{
		ReturnBtn.onClick.AddListener(delegate
		{
			BaiJiaLe_Sockets.GetSingleton().SendLeaveSeat(int.Parse(BaiJiaLe_GameInfo.getInstance().RoomID), int.Parse(BaiJiaLe_GameInfo.getInstance().SeatID));
			SceneManager.LoadSceneAsync("BaiJiaLe_ChooseRoom");
		});
		for (int i = 0; i < canvas.transform.Find("Click").transform.childCount; i++)
		{
			string name = canvas.transform.Find("Click").transform.GetChild(i).name;
			canvas.transform.Find("Click").transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate
			{
				UnityEngine.Debug.Log(name);
				UnityEngine.Debug.Log(BaiJiaLe_GameInfo.getInstance().GameScore);
				UnityEngine.Debug.Log(BetNumber);
				if (float.Parse(BaiJiaLe_GameInfo.getInstance().GameScore) >= (float)BetNumber)
				{
					if (!BetLock && !ShowPanel)
					{
						if (name != null)
						{
							if (!(name == "XianDui"))
							{
								if (!(name == "ZhuangDui"))
								{
									if (!(name == "Xian"))
									{
										if (!(name == "Zhuang"))
										{
											if (name == "He")
											{
												BaiJiaLe_Sockets.GetSingleton().SendUserBet(2, BetNumber, int.Parse(BaiJiaLe_GameInfo.getInstance().RoomID));
											}
										}
										else
										{
											BaiJiaLe_Sockets.GetSingleton().SendUserBet(0, BetNumber, int.Parse(BaiJiaLe_GameInfo.getInstance().RoomID));
										}
									}
									else
									{
										BaiJiaLe_Sockets.GetSingleton().SendUserBet(1, BetNumber, int.Parse(BaiJiaLe_GameInfo.getInstance().RoomID));
									}
								}
								else
								{
									BaiJiaLe_Sockets.GetSingleton().SendUserBet(3, BetNumber, int.Parse(BaiJiaLe_GameInfo.getInstance().RoomID));
								}
							}
							else
							{
								BaiJiaLe_Sockets.GetSingleton().SendUserBet(4, BetNumber, int.Parse(BaiJiaLe_GameInfo.getInstance().RoomID));
							}
						}
					}
					else
					{
						MsgPanel.GetComponent<BaiJiaLe_WaitHide>().Show("停止下注");
					}
				}
				else
				{
					ShowPanel = true;
					GetScorePanel.SetActive(value: true);
				}
			});
		}
		Chips[0].onClick.AddListener(delegate
		{
			for (int n = 0; n < 5; n++)
			{
				Chips[n].transform.Find("Image").gameObject.SetActive(value: false);
			}
			Chips[0].transform.Find("Image").gameObject.SetActive(value: true);
			BetNumber = 5;
		});
		Chips[1].onClick.AddListener(delegate
		{
			for (int m = 0; m < 5; m++)
			{
				Chips[m].transform.Find("Image").gameObject.SetActive(value: false);
			}
			Chips[1].transform.Find("Image").gameObject.SetActive(value: true);
			BetNumber = 50;
		});
		Chips[2].onClick.AddListener(delegate
		{
			for (int l = 0; l < 5; l++)
			{
				Chips[l].transform.Find("Image").gameObject.SetActive(value: false);
			}
			Chips[2].transform.Find("Image").gameObject.SetActive(value: true);
			BetNumber = 100;
		});
		Chips[3].onClick.AddListener(delegate
		{
			for (int k = 0; k < 5; k++)
			{
				Chips[k].transform.Find("Image").gameObject.SetActive(value: false);
			}
			Chips[3].transform.Find("Image").gameObject.SetActive(value: true);
			BetNumber = 500;
		});
		Chips[4].onClick.AddListener(delegate
		{
			for (int j = 0; j < 5; j++)
			{
				Chips[j].transform.Find("Image").gameObject.SetActive(value: false);
			}
			Chips[4].transform.Find("Image").gameObject.SetActive(value: true);
			BetNumber = 1000;
		});
		MYBtn.onClick.AddListener(delegate
		{
			if (MYBtn.transform.Find("Text").GetComponent<Text>().text == "非免佣")
			{
				MYBtn.transform.Find("Text").GetComponent<Text>().text = "免佣";
				BaiJiaLe_Sockets.GetSingleton().SendCheckBindname(int.Parse(BaiJiaLe_GameInfo.getInstance().RoomID), "1");
			}
			else if (MYBtn.transform.Find("Text").GetComponent<Text>().text == "免佣")
			{
				MYBtn.transform.Find("Text").GetComponent<Text>().text = "非免佣";
				BaiJiaLe_Sockets.GetSingleton().SendCheckBindname(int.Parse(BaiJiaLe_GameInfo.getInstance().RoomID), "0");
			}
		});
		ContinueBtn.onClick.AddListener(delegate
		{
			BaiJiaLe_Sockets.GetSingleton().SendContinueBet(int.Parse(BaiJiaLe_GameInfo.getInstance().RoomID));
		});
		DmodeBtn.onClick.AddListener(delegate
		{
			DmodeBtn.gameObject.SetActive(value: false);
			FmodeBtn.gameObject.SetActive(value: true);
		});
		FmodeBtn.onClick.AddListener(delegate
		{
			DmodeBtn.gameObject.SetActive(value: true);
			FmodeBtn.gameObject.SetActive(value: false);
		});
		AudioBtn.onClick.AddListener(delegate
		{
		});
		RuleBtn.onClick.AddListener(delegate
		{
		});
		LuDanBtn.onClick.AddListener(delegate
		{
			LuDanPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		});
		LuDanPanel.transform.Find("BG/Close").GetComponent<Button>().onClick.AddListener(delegate
		{
			LuDanPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.one * 1000f;
		});
		canvas.transform.Find("UserInfo/User").GetComponent<Button>().onClick.AddListener(delegate
		{
			ShowPanel = true;
			GetScorePanel.SetActive(value: true);
		});
		GetScorePanel.transform.Find("Close").GetComponent<Button>().onClick.AddListener(delegate
		{
			ShowPanel = false;
			GetScorePanel.SetActive(value: false);
		});
		GetScorePanel.transform.Find("ShangFen").GetComponent<Button>().onClick.AddListener(delegate
		{
			if (int.Parse(BaiJiaLe_GameInfo.getInstance().GameGold) > 10)
			{
				BaiJiaLe_Sockets.GetSingleton().SendUserCoinIn(10);
			}
			else if (int.Parse(BaiJiaLe_GameInfo.getInstance().GameGold) > 0)
			{
				BaiJiaLe_Sockets.GetSingleton().SendUserCoinIn(1);
			}
		});
		GetScorePanel.transform.Find("XiaFen").GetComponent<Button>().onClick.AddListener(delegate
		{
			if (int.Parse(BaiJiaLe_GameInfo.getInstance().GameScore) >= BaiJiaLe_GameInfo.getInstance().Ratio * 10)
			{
				BaiJiaLe_Sockets.GetSingleton().SendUserCoinOut(BaiJiaLe_GameInfo.getInstance().Ratio * 10);
			}
			else if (int.Parse(BaiJiaLe_GameInfo.getInstance().GameScore) >= BaiJiaLe_GameInfo.getInstance().Ratio)
			{
				BaiJiaLe_Sockets.GetSingleton().SendUserCoinOut(BaiJiaLe_GameInfo.getInstance().Ratio);
			}
		});
		PlayerListBtn.onClick.AddListener(delegate
		{
			PlayerListPanel.SetActive(value: true);
			UpdatePlayerListPanel();
		});
		PlayerListPanel.transform.Find("Close").GetComponent<Button>().onClick.AddListener(delegate
		{
			PlayerListPanel.SetActive(value: false);
		});
	}

	private void ResetGame()
	{
		XianCount.SetActive(value: false);
		ZhuangCount.SetActive(value: false);
		for (int i = 0; i < BetNums.Length; i++)
		{
			for (int j = 0; j < BetNums[i].transform.childCount; j++)
			{
				BetNums[i].transform.GetChild(j).gameObject.SetActive(value: false);
			}
		}
		for (int k = 0; k < 6; k++)
		{
			Players[k].SetActive(value: false);
		}
		for (int l = 0; l < Players.Length; l++)
		{
			Players[l].transform.Find("WinText").gameObject.SetActive(value: false);
			Players[l].transform.Find("LostText").gameObject.SetActive(value: false);
		}
		ShowXianWin.SetActive(value: false);
		ShowZhuangWin.SetActive(value: false);
		ShowHeWin.SetActive(value: false);
	}

	public void PlayerQuit(int seat)
	{
		int num = 0;
		while (true)
		{
			if (num < BaiJiaLe_GameInfo.getInstance().GameSeatList.Count)
			{
				if (BaiJiaLe_GameInfo.getInstance().GameSeatList[num].id == seat)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		if (BaiJiaLe_GameInfo.getInstance().GameSeatList[num].userId == BaiJiaLe_GameInfo.getInstance().PlayerId)
		{
			SceneManager.LoadSceneAsync("BaiJiaLe_ChooseRoom");
		}
		BaiJiaLe_GameInfo.getInstance().GameSeatList.Remove(BaiJiaLe_GameInfo.getInstance().GameSeatList[num]);
	}

	public void UpdatePlayerList()
	{
		for (int i = 0; i < BaiJiaLe_GameInfo.getInstance().GameSeatList.Count; i++)
		{
			if (BaiJiaLe_GameInfo.getInstance().GameSeatList[i].userId == BaiJiaLe_GameInfo.getInstance().PlayerId)
			{
				BaiJiaLe_GameInfo.getInstance().SeatID = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].id.ToString();
				Players[0].transform.Find("NickName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].userNickname;
				Players[0].transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].gamescore.ToString();
				MyselfID = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].userId;
				MyselfSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].id;
				break;
			}
		}
		for (int j = 0; j < BaiJiaLe_GameInfo.getInstance().GameSeatList.Count; j++)
		{
			if (MyselfID != BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId && FirstID != BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId && SecondID != BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId && ThirdID != BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId && FourthID != BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId && LuckyID != BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId)
			{
				if (FirstID == -1)
				{
					Players[1].transform.Find("NickName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userNickname;
					Players[1].transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].gamescore.ToString();
					FirstID = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId;
					FirstSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].id;
				}
				else if (SecondID == -1)
				{
					Players[2].transform.Find("NickName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userNickname;
					Players[2].transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].gamescore.ToString();
					SecondID = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId;
					SecondSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].id;
				}
				else if (ThirdID == -1)
				{
					Players[3].transform.Find("NickName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userNickname;
					Players[3].transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].gamescore.ToString();
					ThirdID = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId;
					ThirdSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].id;
				}
				else if (FourthID == -1)
				{
					Players[4].transform.Find("NickName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userNickname;
					Players[4].transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].gamescore.ToString();
					FourthID = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId;
					FourthSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].id;
				}
				else if (LuckyID == -1)
				{
					Players[5].transform.Find("NickName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userNickname;
					Players[5].transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].gamescore.ToString();
					LuckyID = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId;
					LuckySeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].id;
				}
			}
		}
	}

	public void UpdatePlayerList1()
	{
		if (betTime < 0)
		{
			return;
		}
		for (int i = 0; i < BaiJiaLe_GameInfo.getInstance().GameSeatList.Count; i++)
		{
			if (BaiJiaLe_GameInfo.getInstance().GameSeatList[i].userId == BaiJiaLe_GameInfo.getInstance().PlayerId)
			{
				BaiJiaLe_GameInfo.getInstance().SeatID = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].id.ToString();
				Players[0].transform.Find("NickName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].userNickname;
				Players[0].transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].gamescore.ToString();
				MyselfID = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].userId;
				MyselfSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].id;
				break;
			}
		}
		FirstSeat = -1;
		SecondSeat = -1;
		ThirdSeat = -1;
		FourthSeat = -1;
		LuckySeat = -1;
		for (int j = 0; j < BaiJiaLe_GameInfo.getInstance().GameSeatList.Count; j++)
		{
			if (FirstID == BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId)
			{
				FirstSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].id;
			}
			else if (SecondID == BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId)
			{
				SecondSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].id;
			}
			else if (ThirdID == BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId)
			{
				ThirdSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].id;
			}
			else if (FourthID == BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId)
			{
				FourthSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].id;
			}
			else if (LuckyID == BaiJiaLe_GameInfo.getInstance().GameSeatList[j].userId)
			{
				LuckySeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[j].id;
			}
		}
		if (FirstSeat == -1)
		{
			FirstID = -1;
			FirstSeat = -1;
		}
		else if (SecondSeat == -1)
		{
			SecondID = -1;
			SecondSeat = -1;
		}
		else if (ThirdSeat == -1)
		{
			ThirdID = -1;
			ThirdSeat = -1;
		}
		else if (FourthSeat == -1)
		{
			FourthID = -1;
			FourthSeat = -1;
		}
		else if (LuckySeat == -1)
		{
			LuckyID = -1;
			LuckySeat = -1;
		}
		for (int k = 0; k < BaiJiaLe_GameInfo.getInstance().GameSeatList.Count; k++)
		{
			if (MyselfID != BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userId && FirstID != BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userId && SecondID != BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userId && ThirdID != BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userId && FourthID != BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userId && LuckyID != BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userId)
			{
				if (FirstID == -1)
				{
					Players[1].transform.Find("NickName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userNickname;
					Players[1].transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].gamescore.ToString();
					FirstID = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userId;
					FirstSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].id;
				}
				else if (SecondID == -1)
				{
					Players[2].transform.Find("NickName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userNickname;
					Players[2].transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].gamescore.ToString();
					SecondID = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userId;
					SecondSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].id;
				}
				else if (ThirdID == -1)
				{
					Players[3].transform.Find("NickName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userNickname;
					Players[3].transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].gamescore.ToString();
					ThirdID = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userId;
					ThirdSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].id;
				}
				else if (FourthID == -1)
				{
					Players[4].transform.Find("NickName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userNickname;
					Players[4].transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].gamescore.ToString();
					FourthID = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userId;
					FourthSeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].id;
				}
				else if (LuckyID == -1)
				{
					Players[5].transform.Find("NickName").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userNickname;
					Players[5].transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].gamescore.ToString();
					LuckyID = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].userId;
					LuckySeat = BaiJiaLe_GameInfo.getInstance().GameSeatList[k].id;
				}
			}
		}
	}

	public void UpdatePlayerListPanel()
	{
		GameObject gameObject = PlayerListPanel.transform.Find("Scroll View/Viewport/Content").gameObject;
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			if (i < BaiJiaLe_GameInfo.getInstance().GameSeatList.Count)
			{
				gameObject.transform.GetChild(i).gameObject.SetActive(value: true);
				gameObject.transform.GetChild(i).Find("Text").GetComponent<Text>()
					.text = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].userNickname;
					gameObject.transform.GetChild(i).Find("Score/Text").GetComponent<Text>()
						.text = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].gamescore.ToString();
						gameObject.transform.GetChild(i).Find("Bet").GetComponent<Text>()
							.text = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].totalbet.ToString();
							gameObject.transform.GetChild(i).Find("Win").GetComponent<Text>()
								.text = BaiJiaLe_GameInfo.getInstance().GameSeatList[i].totalwin.ToString();
							}
							else
							{
								gameObject.transform.GetChild(i).gameObject.SetActive(value: false);
							}
						}
					}

					public void UpdateUserInfo()
					{
						NickName.text = BaiJiaLe_GameInfo.getInstance().NickName;
						Score.text = BaiJiaLe_GameInfo.getInstance().GameScore;
						GetScorePanel.transform.Find("Gold/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameGold;
						GetScorePanel.transform.Find("Score/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().GameScore;
					}

					private void UpdateGameInfo()
					{
						LuDanBtn.transform.Find("ZhuangCount").GetComponent<Text>().text = ZhuangNum.ToString();
						LuDanBtn.transform.Find("XianCount").GetComponent<Text>().text = XianNum.ToString();
						LuDanBtn.transform.Find("HeCount").GetComponent<Text>().text = HeNum.ToString();
						LuDanPanel.transform.Find("BG/Zhuang").GetComponent<Text>().text = "庄  " + ZhuangNum;
						LuDanPanel.transform.Find("BG/Xian").GetComponent<Text>().text = "闲  " + XianNum;
						LuDanPanel.transform.Find("BG/ZhuangDui").GetComponent<Text>().text = "庄对 " + ZhuangDuiNum;
						LuDanPanel.transform.Find("BG/XianDui").GetComponent<Text>().text = "闲对 " + XianDuiNum;
						LuDanPanel.transform.Find("BG/He").GetComponent<Text>().text = "和  " + HeNum;
						LuDanPanel.transform.Find("BG/Inning").GetComponent<Text>().text = "局数 " + InningNum;
					}

					private void UpdateLuDan()
					{
						for (int i = 0; i < BaiJiaLe_GameInfo.getInstance().GameDeskList.Count; i++)
						{
							if (!(BaiJiaLe_GameInfo.getInstance().RoomID == BaiJiaLe_GameInfo.getInstance().GameDeskList[i].id.ToString()))
							{
								continue;
							}
							BaiJiaLe_GameInfo.getInstance().GameLuDan.Clear();
							if (BaiJiaLe_GameInfo.getInstance().GameDeskList[i].ludan != null)
							{
								string ludan = BaiJiaLe_GameInfo.getInstance().GameDeskList[i].ludan;
								for (int j = 0; j < ludan.Length; j++)
								{
									char c = ludan[j];
									BaiJiaLe_GameInfo.getInstance().GameLuDan.Add(c.ToString());
								}
							}
							LuDanBtn.GetComponent<BaiJiaLe_UpdateLuDan>().ShowLuDan(BaiJiaLe_GameInfo.getInstance().GameLuDan.ToArray());
							LuDanPanel.GetComponent<BaiJiaLe_UpdateLuDan2>().ShowLuDan(BaiJiaLe_GameInfo.getInstance().GameLuDan.ToArray());
							break;
						}
						for (int k = 0; k < BaiJiaLe_GameInfo.getInstance().GameLuDan.Count; k++)
						{
							switch (BaiJiaLe_GameInfo.getInstance().GameLuDan[k])
							{
							case "0":
								ZhuangNum++;
								break;
							case "1":
								XianNum++;
								break;
							case "2":
								HeNum++;
								break;
							case "3":
								ZhuangNum++;
								ZhuangDuiNum++;
								break;
							case "4":
								XianNum++;
								ZhuangDuiNum++;
								break;
							case "5":
								HeNum++;
								ZhuangDuiNum++;
								break;
							case "6":
								ZhuangNum++;
								XianDuiNum++;
								break;
							case "7":
								XianNum++;
								XianDuiNum++;
								break;
							case "8":
								HeNum++;
								XianDuiNum++;
								break;
							case "9":
								ZhuangNum++;
								ZhuangDuiNum++;
								XianDuiNum++;
								break;
							case "a":
								XianNum++;
								ZhuangDuiNum++;
								XianDuiNum++;
								break;
							case "b":
								HeNum++;
								ZhuangDuiNum++;
								XianDuiNum++;
								break;
							default:
								ZhuangDuiNum++;
								XianDuiNum++;
								break;
							}
						}
						InningNum = BaiJiaLe_GameInfo.getInstance().GameLuDan.Count;
						UpdateGameInfo();
					}

					public void SetZhuangRate(float rate)
					{
						ZhuangRate.GetComponent<Text>().text = "1:" + rate.ToString();
					}

					public void GameRestart(int chang, double[] beilv)
					{
						BaiJiaLe_GameInfo.getInstance().PlayerChips.Clear();
						if (chang == 0)
						{
							BaiJiaLe_GameInfo.getInstance().GameLuDan.Clear();
						}
						ZhuangRate.text = "1:" + (beilv[0] - 1.0);
						if (MYBtn.transform.Find("Text").GetComponent<Text>().text == "免佣")
						{
							ZhuangRate.text += " 庄6赔1.5";
						}
						betTime = BaiJiaLe_GameInfo.getInstance().BetTime;
						NextPanel.SetActive(value: false);
						DealerAnime.PlayDealCards();
						moveValue = 0f;
						CameraType = 2;
						CameraLock = true;
						BetLock = false;
						ZhuangCount.SetActive(value: false);
						XianCount.SetActive(value: false);
						DealerAnime.RecycleCard();
						ShowZhuangWin.SetActive(value: false);
						ShowXianWin.SetActive(value: false);
						ShowHeWin.SetActive(value: false);
						ShowWinPanel_Zhuang.SetActive(value: false);
						ShowWinPanel_ZhuangDui.SetActive(value: false);
						ShowWinPanel_Xian.SetActive(value: false);
						ShowWinPanel_XianDui.SetActive(value: false);
						ShowWinPanel_He.SetActive(value: false);
						ZhuangText.GetComponent<TextMesh>().text = "0";
						XianText.GetComponent<TextMesh>().text = "0";
						HeText.GetComponent<TextMesh>().text = "0";
						ZhuangDuiText.GetComponent<TextMesh>().text = "0";
						XianDuiText.GetComponent<TextMesh>().text = "0";
						for (int i = 0; i < BetChips.Length; i++)
						{
							for (int j = 0; j < BetChips[i].transform.childCount; j++)
							{
								int num = 0;
								while (num < BetChips[i].transform.GetChild(j).childCount)
								{
									BetChips[i].transform.GetChild(j).GetChild(0).gameObject.SetActive(value: false);
									BetChips[i].transform.GetChild(j).GetChild(0).position = ChipPool.transform.position;
									BetChips[i].transform.GetChild(j).GetChild(0).SetParent(ChipPool.transform);
								}
							}
						}
						for (int k = 0; k < ZhuangWinChips.transform.childCount; k++)
						{
							int num2 = 0;
							while (num2 < ZhuangWinChips.transform.GetChild(k).childCount)
							{
								ZhuangWinChips.transform.GetChild(k).GetChild(0).gameObject.SetActive(value: false);
								ZhuangWinChips.transform.GetChild(k).GetChild(0).position = ChipPool.transform.position;
								ZhuangWinChips.transform.GetChild(k).GetChild(0).SetParent(ChipPool.transform);
							}
						}
						for (int l = 0; l < BetNums.Length; l++)
						{
							BetNums[l].transform.Find("Z").gameObject.SetActive(value: false);
							BetNums[l].transform.Find("X").gameObject.SetActive(value: false);
							BetNums[l].transform.Find("H").gameObject.SetActive(value: false);
						}
						for (int m = 0; m < Players.Length; m++)
						{
							Players[m].transform.Find("WinText").gameObject.SetActive(value: false);
							Players[m].transform.Find("LostText").gameObject.SetActive(value: false);
						}
						NowSeatChips = new int[7, 5];
						NewSeatChips = new int[7, 5];
						LuDanBtn.GetComponent<BaiJiaLe_UpdateLuDan>().ShowLuDan(BaiJiaLe_GameInfo.getInstance().GameLuDan.ToArray());
						LuDanPanel.GetComponent<BaiJiaLe_UpdateLuDan2>().ShowLuDan(BaiJiaLe_GameInfo.getInstance().GameLuDan.ToArray());
						UpdateGameInfo();
					}

					public void GetCurrentBet(int z, int x, int h, int zd, int xd)
					{
						ZhuangText.GetComponent<TextMesh>().text = z.ToString();
						XianText.GetComponent<TextMesh>().text = x.ToString();
						HeText.GetComponent<TextMesh>().text = h.ToString();
						ZhuangDuiText.GetComponent<TextMesh>().text = zd.ToString();
						XianDuiText.GetComponent<TextMesh>().text = xd.ToString();
					}

					public void UpdatePlayerBet()
					{
						int num = 0;
						int num2 = 0;
						int num3 = 0;
						int num4 = 0;
						int num5 = 0;
						for (int i = 0; i < BaiJiaLe_GameInfo.getInstance().PlayerChips.Count; i++)
						{
							if (MyselfID == BaiJiaLe_GameInfo.getInstance().PlayerChips[i][0])
							{
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1] > 0)
								{
									BetNums[0].transform.Find("Z").gameObject.SetActive(value: true);
									BetNums[0].transform.Find("Z/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1].ToString();
								}
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2] > 0)
								{
									BetNums[0].transform.Find("X").gameObject.SetActive(value: true);
									BetNums[0].transform.Find("X/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2].ToString();
								}
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3] > 0)
								{
									BetNums[0].transform.Find("H").gameObject.SetActive(value: true);
									BetNums[0].transform.Find("H/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3].ToString();
								}
								for (int j = 0; j < 5; j++)
								{
									NewSeatChips[0, j] = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][j + 1];
								}
								continue;
							}
							if (FirstID == BaiJiaLe_GameInfo.getInstance().PlayerChips[i][0])
							{
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1] > 0)
								{
									BetNums[1].transform.Find("Z").gameObject.SetActive(value: true);
									BetNums[1].transform.Find("Z/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1].ToString();
								}
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2] > 0)
								{
									BetNums[1].transform.Find("X").gameObject.SetActive(value: true);
									BetNums[1].transform.Find("X/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2].ToString();
								}
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3] > 0)
								{
									BetNums[1].transform.Find("H").gameObject.SetActive(value: true);
									BetNums[1].transform.Find("H/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3].ToString();
								}
								for (int k = 0; k < 5; k++)
								{
									NewSeatChips[1, k] = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][k + 1];
								}
								continue;
							}
							if (SecondID == BaiJiaLe_GameInfo.getInstance().PlayerChips[i][0])
							{
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1] > 0)
								{
									BetNums[2].transform.Find("Z").gameObject.SetActive(value: true);
									BetNums[2].transform.Find("Z/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1].ToString();
								}
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2] > 0)
								{
									BetNums[2].transform.Find("X").gameObject.SetActive(value: true);
									BetNums[2].transform.Find("X/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2].ToString();
								}
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3] > 0)
								{
									BetNums[2].transform.Find("H").gameObject.SetActive(value: true);
									BetNums[2].transform.Find("H/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3].ToString();
								}
								for (int l = 0; l < 5; l++)
								{
									NewSeatChips[2, l] = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][l + 1];
								}
								continue;
							}
							if (ThirdID == BaiJiaLe_GameInfo.getInstance().PlayerChips[i][0])
							{
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1] > 0)
								{
									BetNums[3].transform.Find("Z").gameObject.SetActive(value: true);
									BetNums[3].transform.Find("Z/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1].ToString();
								}
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2] > 0)
								{
									BetNums[3].transform.Find("X").gameObject.SetActive(value: true);
									BetNums[3].transform.Find("X/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2].ToString();
								}
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3] > 0)
								{
									BetNums[3].transform.Find("H").gameObject.SetActive(value: true);
									BetNums[3].transform.Find("H/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3].ToString();
								}
								for (int m = 0; m < 5; m++)
								{
									NewSeatChips[3, m] = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][m + 1];
								}
								continue;
							}
							if (FourthID == BaiJiaLe_GameInfo.getInstance().PlayerChips[i][0])
							{
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1] > 0)
								{
									BetNums[4].transform.Find("Z").gameObject.SetActive(value: true);
									BetNums[4].transform.Find("Z/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1].ToString();
								}
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2] > 0)
								{
									BetNums[4].transform.Find("X").gameObject.SetActive(value: true);
									BetNums[4].transform.Find("X/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2].ToString();
								}
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3] > 0)
								{
									BetNums[4].transform.Find("H").gameObject.SetActive(value: true);
									BetNums[4].transform.Find("H/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3].ToString();
								}
								for (int n = 0; n < 5; n++)
								{
									NewSeatChips[4, n] = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][n + 1];
								}
								continue;
							}
							if (LuckyID == BaiJiaLe_GameInfo.getInstance().PlayerChips[i][0])
							{
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1] > 0)
								{
									BetNums[5].transform.Find("Z").gameObject.SetActive(value: true);
									BetNums[5].transform.Find("Z/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1].ToString();
								}
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2] > 0)
								{
									BetNums[5].transform.Find("X").gameObject.SetActive(value: true);
									BetNums[5].transform.Find("X/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2].ToString();
								}
								if (BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3] > 0)
								{
									BetNums[5].transform.Find("H").gameObject.SetActive(value: true);
									BetNums[5].transform.Find("H/Text").GetComponent<Text>().text = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3].ToString();
								}
								for (int num6 = 0; num6 < 5; num6++)
								{
									NewSeatChips[5, num6] = BaiJiaLe_GameInfo.getInstance().PlayerChips[i][num6 + 1];
								}
								continue;
							}
							num += BaiJiaLe_GameInfo.getInstance().PlayerChips[i][1];
							num2 += BaiJiaLe_GameInfo.getInstance().PlayerChips[i][2];
							num3 += BaiJiaLe_GameInfo.getInstance().PlayerChips[i][3];
							num4 += BaiJiaLe_GameInfo.getInstance().PlayerChips[i][4];
							num5 += BaiJiaLe_GameInfo.getInstance().PlayerChips[i][5];
							if (num > 0)
							{
								BetNums[6].transform.Find("Z").gameObject.SetActive(value: true);
								BetNums[6].transform.Find("Z/Text").GetComponent<Text>().text = num.ToString();
							}
							if (num2 > 0)
							{
								BetNums[6].transform.Find("X").gameObject.SetActive(value: true);
								BetNums[6].transform.Find("X/Text").GetComponent<Text>().text = num2.ToString();
							}
							if (num3 > 0)
							{
								BetNums[6].transform.Find("H").gameObject.SetActive(value: true);
								BetNums[6].transform.Find("H/Text").GetComponent<Text>().text = num3.ToString();
							}
							NewSeatChips[6, 0] = num;
							NewSeatChips[6, 1] = num2;
							NewSeatChips[6, 2] = num3;
							NewSeatChips[6, 3] = num4;
							NewSeatChips[6, 4] = num5;
						}
						for (int num7 = 0; num7 < 7; num7++)
						{
							for (int num8 = 0; num8 < 5; num8++)
							{
								int value = NewSeatChips[num7, num8] - NowSeatChips[num7, num8];
								int[] array = ValueToChips(value);
								for (int num9 = 0; num9 < array.Length; num9++)
								{
									AddChip(array[num9], num7, num8);
								}
								NowSeatChips[num7, num8] = NewSeatChips[num7, num8];
							}
						}
					}

					private int[] ValueToChips(int _value)
					{
						List<int> list = new List<int>();
						if (_value >= 1000)
						{
							int num = _value - 1000;
							list.Add(1000);
						}
						else if (_value >= 500)
						{
							int num = _value - 500;
							list.Add(500);
						}
						else if (_value >= 100)
						{
							int num = _value - 100;
							list.Add(100);
						}
						else if (_value >= 50)
						{
							int num = _value - 50;
							list.Add(50);
						}
						else if (_value >= 5)
						{
							int num = _value - 5;
							list.Add(5);
						}
						else
						{
							int num = 0;
						}
						return list.ToArray();
					}

					private void AddChip(int num, int seat, int type)
					{
						GameObject gameObject = (ChipPool.transform.childCount >= 1) ? ChipPool.transform.GetChild(0).gameObject : UnityEngine.Object.Instantiate(PrefabChip, ChipPool.transform);
						gameObject.SetActive(value: true);
						gameObject.name = num.ToString();
						gameObject.transform.SetParent(BetChips[seat].transform.Find("bet" + type));
						gameObject.transform.position = BetChips[seat].transform.Find("bet" + type).position + Vector3.up * BetChips[seat].transform.Find("bet" + type).childCount * 0.01f;
						switch (num)
						{
						case 5:
							gameObject.GetComponent<MeshRenderer>().material = BaiJiaLe_LuDanDate.instance.ChipMaterial[0];
							break;
						case 50:
							gameObject.GetComponent<MeshRenderer>().material = BaiJiaLe_LuDanDate.instance.ChipMaterial[1];
							break;
						case 100:
							gameObject.GetComponent<MeshRenderer>().material = BaiJiaLe_LuDanDate.instance.ChipMaterial[2];
							break;
						case 500:
							gameObject.GetComponent<MeshRenderer>().material = BaiJiaLe_LuDanDate.instance.ChipMaterial[3];
							break;
						case 1000:
							gameObject.GetComponent<MeshRenderer>().material = BaiJiaLe_LuDanDate.instance.ChipMaterial[4];
							break;
						default:
							gameObject.GetComponent<MeshRenderer>().material = BaiJiaLe_LuDanDate.instance.ChipMaterial[0];
							break;
						}
					}

					private GameObject GetChip(int num, int seat, int type)
					{
						GameObject gameObject = (ChipPool.transform.childCount >= 1) ? ChipPool.transform.GetChild(0).gameObject : UnityEngine.Object.Instantiate(PrefabChip, ChipPool.transform);
						gameObject.SetActive(value: true);
						gameObject.name = num.ToString();
						gameObject.transform.SetParent(BetChips[seat].transform.Find("win" + type));
						switch (num)
						{
						case 5:
							gameObject.GetComponent<MeshRenderer>().material = BaiJiaLe_LuDanDate.instance.ChipMaterial[0];
							break;
						case 50:
							gameObject.GetComponent<MeshRenderer>().material = BaiJiaLe_LuDanDate.instance.ChipMaterial[1];
							break;
						case 100:
							gameObject.GetComponent<MeshRenderer>().material = BaiJiaLe_LuDanDate.instance.ChipMaterial[2];
							break;
						case 500:
							gameObject.GetComponent<MeshRenderer>().material = BaiJiaLe_LuDanDate.instance.ChipMaterial[3];
							break;
						case 1000:
							gameObject.GetComponent<MeshRenderer>().material = BaiJiaLe_LuDanDate.instance.ChipMaterial[4];
							break;
						default:
							gameObject.GetComponent<MeshRenderer>().material = BaiJiaLe_LuDanDate.instance.ChipMaterial[0];
							break;
						}
						return gameObject;
					}

					public void GameResult()
					{
						bool zz = false;
						bool xz = false;
						BetLock = true;
						string[] array = BaiJiaLe_GameInfo.getInstance().GameResult.pai.Split('-');
						UnityEngine.Debug.Log(BaiJiaLe_GameInfo.getInstance().GameResult.pai);
						BaiJiaLe_GameInfo.getInstance().ZhuangCards[0] = int.Parse(array[0]);
						BaiJiaLe_GameInfo.getInstance().ZhuangCards[1] = int.Parse(array[1]);
						BaiJiaLe_GameInfo.getInstance().ZhuangCards[2] = int.Parse(array[2]);
						BaiJiaLe_GameInfo.getInstance().XianCards[0] = int.Parse(array[3]);
						BaiJiaLe_GameInfo.getInstance().XianCards[1] = int.Parse(array[4]);
						BaiJiaLe_GameInfo.getInstance().XianCards[2] = int.Parse(array[5]);
						BaiJiaLe_GameInfo.getInstance().ZhuangNum = int.Parse(array[6]);
						BaiJiaLe_GameInfo.getInstance().XianNum = int.Parse(array[7]);
						if (BaiJiaLe_GameInfo.getInstance().ZhuangCards[2] > 0)
						{
							zz = true;
							UnityEngine.Debug.Log(BaiJiaLe_GameInfo.getInstance().ZhuangCards[2]);
						}
						if (BaiJiaLe_GameInfo.getInstance().XianCards[2] > 0)
						{
							xz = true;
							UnityEngine.Debug.Log(BaiJiaLe_GameInfo.getInstance().XianCards[2]);
						}
						switch (BaiJiaLe_GameInfo.getInstance().GameResult.ludan)
						{
						case 0:
						case 3:
						case 6:
						case 9:
							DealerAnime.PlayOpenCards(zz, xz, 0);
							break;
						case 1:
						case 4:
						case 7:
						case 10:
							DealerAnime.PlayOpenCards(zz, xz, 1);
							break;
						case 2:
						case 5:
						case 8:
						case 11:
							DealerAnime.PlayOpenCards(zz, xz, 2);
							break;
						}
						if (BaiJiaLe_GameInfo.getInstance().GameLuDan.Count == 72)
						{
							BaiJiaLe_GameInfo.getInstance().GameLuDan.Clear();
							ZhuangNum = 0;
							XianNum = 0;
							HeNum = 0;
							ZhuangDuiNum = 0;
							XianDuiNum = 0;
						}
						if (BaiJiaLe_GameInfo.getInstance().GameResult.ludan == 10)
						{
							BaiJiaLe_GameInfo.getInstance().GameLuDan.Add("a");
						}
						else if (BaiJiaLe_GameInfo.getInstance().GameResult.ludan == 11)
						{
							BaiJiaLe_GameInfo.getInstance().GameLuDan.Add("b");
						}
						else
						{
							BaiJiaLe_GameInfo.getInstance().GameLuDan.Add(BaiJiaLe_GameInfo.getInstance().GameResult.ludan.ToString());
						}
						UnityEngine.Debug.Log("第" + BaiJiaLe_GameInfo.getInstance().GameLuDan.Count);
						switch (BaiJiaLe_GameInfo.getInstance().GameResult.ludan.ToString())
						{
						case "0":
							ZhuangNum++;
							break;
						case "1":
							XianNum++;
							break;
						case "2":
							HeNum++;
							break;
						case "3":
							ZhuangNum++;
							ZhuangDuiNum++;
							break;
						case "4":
							XianNum++;
							ZhuangDuiNum++;
							break;
						case "5":
							HeNum++;
							ZhuangDuiNum++;
							break;
						case "6":
							ZhuangNum++;
							XianDuiNum++;
							break;
						case "7":
							XianNum++;
							XianDuiNum++;
							break;
						case "8":
							HeNum++;
							XianDuiNum++;
							break;
						case "9":
							ZhuangNum++;
							ZhuangDuiNum++;
							XianDuiNum++;
							break;
						case "10":
							XianNum++;
							ZhuangDuiNum++;
							XianDuiNum++;
							break;
						case "11":
							HeNum++;
							ZhuangDuiNum++;
							XianDuiNum++;
							break;
						default:
							ZhuangDuiNum++;
							XianDuiNum++;
							break;
						}
						moveValue = 0f;
						CameraType = 0;
						CameraLock = true;
					}

					public void ShowWin()
					{
						switch (BaiJiaLe_GameInfo.getInstance().GameResult.ludan)
						{
						case 0:
						case 3:
						case 6:
						case 9:
							ShowZhuangWin.SetActive(value: true);
							ShowWinPanel_Zhuang.SetActive(value: true);
							break;
						case 1:
						case 4:
						case 7:
						case 10:
							ShowXianWin.SetActive(value: true);
							ShowWinPanel_Xian.SetActive(value: true);
							break;
						case 2:
						case 5:
						case 8:
						case 11:
							ShowHeWin.SetActive(value: true);
							ShowWinPanel_He.SetActive(value: true);
							break;
						}
						switch (BaiJiaLe_GameInfo.getInstance().GameResult.ludan)
						{
						case 3:
						case 4:
						case 5:
							ShowWinPanel_ZhuangDui.SetActive(value: true);
							break;
						case 6:
						case 7:
						case 8:
							ShowWinPanel_XianDui.SetActive(value: true);
							break;
						case 9:
						case 10:
						case 11:
							ShowWinPanel_ZhuangDui.SetActive(value: true);
							ShowWinPanel_XianDui.SetActive(value: true);
							break;
						}
						LuDanBtn.GetComponent<BaiJiaLe_UpdateLuDan>().ShowLuDan(BaiJiaLe_GameInfo.getInstance().GameLuDan.ToArray());
						LuDanPanel.GetComponent<BaiJiaLe_UpdateLuDan2>().ShowLuDan(BaiJiaLe_GameInfo.getInstance().GameLuDan.ToArray());
						UpdateGameInfo();
					}

					public void RecyclingChip()
					{
						StartCoroutine(RecycleChip_IE());
					}

					private IEnumerator RecycleChip_IE()
					{
						for (int i = 0; i < BetNums.Length; i++)
						{
							BetNums[i].transform.Find("Z").gameObject.SetActive(value: false);
							BetNums[i].transform.Find("X").gameObject.SetActive(value: false);
							BetNums[i].transform.Find("H").gameObject.SetActive(value: false);
						}
						LostChips.Clear();
						LostValues.Clear();
						WinChips.Clear();
						WinValues.Clear();
						moveValue = 0f;
						CameraType = 1;
						CameraLock = true;
						switch (BaiJiaLe_GameInfo.getInstance().GameResult.ludan)
						{
						case 0:
							for (int num14 = 0; num14 < BetChips.Length; num14++)
							{
								for (int num15 = 0; num15 < BetChips[num14].transform.childCount; num15++)
								{
									if (BetChips[num14].transform.GetChild(num15).name == "bet1" || BetChips[num14].transform.GetChild(num15).name == "bet2" || BetChips[num14].transform.GetChild(num15).name == "bet3" || BetChips[num14].transform.GetChild(num15).name == "bet4")
									{
										for (int num16 = 0; num16 < BetChips[num14].transform.GetChild(num15).childCount; num16++)
										{
											LostChips.Add(BetChips[num14].transform.GetChild(num15).GetChild(num16).gameObject);
											LostValues.Add(0f);
										}
									}
								}
							}
							for (int num17 = 0; num17 < BetChips.Length; num17++)
							{
								for (int num18 = 0; num18 < BetChips[num17].transform.childCount; num18++)
								{
									if (BetChips[num17].transform.GetChild(num18).name == "bet0")
									{
										for (int num19 = 0; num19 < BetChips[num17].transform.GetChild(num18).childCount; num19++)
										{
											WinChips.Add(GetChip(int.Parse(BetChips[num17].transform.GetChild(num18).GetChild(num19).name), num17, 0));
											WinValues.Add(0f);
										}
									}
								}
							}
							break;
						case 1:
							for (int num29 = 0; num29 < BetChips.Length; num29++)
							{
								for (int num30 = 0; num30 < BetChips[num29].transform.childCount; num30++)
								{
									if (BetChips[num29].transform.GetChild(num30).name == "bet0" || BetChips[num29].transform.GetChild(num30).name == "bet2" || BetChips[num29].transform.GetChild(num30).name == "bet3" || BetChips[num29].transform.GetChild(num30).name == "bet4")
									{
										for (int num31 = 0; num31 < BetChips[num29].transform.GetChild(num30).childCount; num31++)
										{
											LostChips.Add(BetChips[num29].transform.GetChild(num30).GetChild(num31).gameObject);
											LostValues.Add(0f);
										}
									}
								}
							}
							for (int num32 = 0; num32 < BetChips.Length; num32++)
							{
								for (int num33 = 0; num33 < BetChips[num32].transform.childCount; num33++)
								{
									if (BetChips[num32].transform.GetChild(num33).name == "bet1")
									{
										for (int num34 = 0; num34 < BetChips[num32].transform.GetChild(num33).childCount; num34++)
										{
											WinChips.Add(GetChip(int.Parse(BetChips[num32].transform.GetChild(num33).GetChild(num34).name), num32, 1));
											WinValues.Add(0f);
										}
									}
								}
							}
							break;
						case 2:
							for (int m = 0; m < BetChips.Length; m++)
							{
								for (int n = 0; n < BetChips[m].transform.childCount; n++)
								{
									if (BetChips[m].transform.GetChild(n).name == "bet3" || BetChips[m].transform.GetChild(n).name == "bet4")
									{
										for (int num = 0; num < BetChips[m].transform.GetChild(n).childCount; num++)
										{
											LostChips.Add(BetChips[m].transform.GetChild(n).GetChild(num).gameObject);
											LostValues.Add(0f);
										}
									}
								}
							}
							for (int num2 = 0; num2 < BetChips.Length; num2++)
							{
								for (int num3 = 0; num3 < BetChips[num2].transform.childCount; num3++)
								{
									if (BetChips[num2].transform.GetChild(num3).name == "bet2")
									{
										for (int num4 = 0; num4 < BetChips[num2].transform.GetChild(num3).childCount; num4++)
										{
											WinChips.Add(GetChip(int.Parse(BetChips[num2].transform.GetChild(num3).GetChild(num4).name), num2, 2));
											WinValues.Add(0f);
										}
									}
								}
							}
							break;
						case 3:
							for (int num23 = 0; num23 < BetChips.Length; num23++)
							{
								for (int num24 = 0; num24 < BetChips[num23].transform.childCount; num24++)
								{
									if (BetChips[num23].transform.GetChild(num24).name == "bet1" || BetChips[num23].transform.GetChild(num24).name == "bet2" || BetChips[num23].transform.GetChild(num24).name == "bet4")
									{
										for (int num25 = 0; num25 < BetChips[num23].transform.GetChild(num24).childCount; num25++)
										{
											LostChips.Add(BetChips[num23].transform.GetChild(num24).GetChild(num25).gameObject);
											LostValues.Add(0f);
										}
									}
								}
							}
							break;
						case 4:
							for (int num8 = 0; num8 < BetChips.Length; num8++)
							{
								for (int num9 = 0; num9 < BetChips[num8].transform.childCount; num9++)
								{
									if (BetChips[num8].transform.GetChild(num9).name == "bet0" || BetChips[num8].transform.GetChild(num9).name == "bet2" || BetChips[num8].transform.GetChild(num9).name == "bet4")
									{
										for (int num10 = 0; num10 < BetChips[num8].transform.GetChild(num9).childCount; num10++)
										{
											LostChips.Add(BetChips[num8].transform.GetChild(num9).GetChild(num10).gameObject);
											LostValues.Add(0f);
										}
									}
								}
							}
							break;
						case 5:
							for (int num35 = 0; num35 < BetChips.Length; num35++)
							{
								for (int num36 = 0; num36 < BetChips[num35].transform.childCount; num36++)
								{
									if (BetChips[num35].transform.GetChild(num36).name == "bet4")
									{
										for (int num37 = 0; num37 < BetChips[num35].transform.GetChild(num36).childCount; num37++)
										{
											LostChips.Add(BetChips[num35].transform.GetChild(num36).GetChild(num37).gameObject);
											LostValues.Add(0f);
										}
									}
								}
							}
							break;
						case 6:
							for (int num26 = 0; num26 < BetChips.Length; num26++)
							{
								for (int num27 = 0; num27 < BetChips[num26].transform.childCount; num27++)
								{
									if (BetChips[num26].transform.GetChild(num27).name == "bet1" || BetChips[num26].transform.GetChild(num27).name == "bet2" || BetChips[num26].transform.GetChild(num27).name == "bet3")
									{
										for (int num28 = 0; num28 < BetChips[num26].transform.GetChild(num27).childCount; num28++)
										{
											LostChips.Add(BetChips[num26].transform.GetChild(num27).GetChild(num28).gameObject);
											LostValues.Add(0f);
										}
									}
								}
							}
							break;
						case 7:
							for (int num20 = 0; num20 < BetChips.Length; num20++)
							{
								for (int num21 = 0; num21 < BetChips[num20].transform.childCount; num21++)
								{
									if (BetChips[num20].transform.GetChild(num21).name == "bet0" || BetChips[num20].transform.GetChild(num21).name == "bet2" || BetChips[num20].transform.GetChild(num21).name == "bet3")
									{
										for (int num22 = 0; num22 < BetChips[num20].transform.GetChild(num21).childCount; num22++)
										{
											LostChips.Add(BetChips[num20].transform.GetChild(num21).GetChild(num22).gameObject);
											LostValues.Add(0f);
										}
									}
								}
							}
							break;
						case 8:
							for (int num11 = 0; num11 < BetChips.Length; num11++)
							{
								for (int num12 = 0; num12 < BetChips[num11].transform.childCount; num12++)
								{
									if (BetChips[num11].transform.GetChild(num12).name == "bet3")
									{
										for (int num13 = 0; num13 < BetChips[num11].transform.GetChild(num12).childCount; num13++)
										{
											LostChips.Add(BetChips[num11].transform.GetChild(num12).GetChild(num13).gameObject);
											LostValues.Add(0f);
										}
									}
								}
							}
							break;
						case 9:
							for (int num5 = 0; num5 < BetChips.Length; num5++)
							{
								for (int num6 = 0; num6 < BetChips[num5].transform.childCount; num6++)
								{
									if (BetChips[num5].transform.GetChild(num6).name == "bet1" || BetChips[num5].transform.GetChild(num6).name == "bet2")
									{
										for (int num7 = 0; num7 < BetChips[num5].transform.GetChild(num6).childCount; num7++)
										{
											LostChips.Add(BetChips[num5].transform.GetChild(num6).GetChild(num7).gameObject);
											LostValues.Add(0f);
										}
									}
								}
							}
							break;
						case 10:
							for (int j = 0; j < BetChips.Length; j++)
							{
								for (int k = 0; k < BetChips[j].transform.childCount; k++)
								{
									if (BetChips[j].transform.GetChild(k).name == "bet0" || BetChips[j].transform.GetChild(k).name == "bet2")
									{
										for (int l = 0; l < BetChips[j].transform.GetChild(k).childCount; l++)
										{
											LostChips.Add(BetChips[j].transform.GetChild(k).GetChild(l).gameObject);
											LostValues.Add(0f);
										}
									}
								}
							}
							break;
						}
						Vector3[] lostPos = new Vector3[LostChips.Count];
						for (int num38 = 0; num38 < LostChips.Count; num38++)
						{
							lostPos[num38] = LostChips[num38].transform.position;
						}
						if (LostValues.Count == 0)
						{
							yield return new WaitForSeconds(3f);
						}
						while (LostValues.Count > 0)
						{
							if (LostValues[LostValues.Count - 1] > 1f)
							{
								UnityEngine.Debug.Log("需要停止了");
								break;
							}
							GameObject target = null;
							for (int num39 = 0; num39 < LostChips.Count; num39++)
							{
								switch (LostChips[num39].transform.parent.parent.name)
								{
								case "BetChips0":
									target = ZhuangWinChips.transform.GetChild(0).gameObject;
									LostChips[num39].transform.position = Vector3.Lerp(lostPos[num39], target.transform.position + target.transform.childCount * Vector3.up * 0.01f, LostValues[num39]) + Vector3.up * 2f * Mathf.Sin(LostValues[num39] * (float)Math.PI);
									break;
								case "BetChips1":
									target = ZhuangWinChips.transform.GetChild(1).gameObject;
									LostChips[num39].transform.position = Vector3.Lerp(lostPos[num39], target.transform.position + target.transform.childCount * Vector3.up * 0.01f, LostValues[num39]) + Vector3.up * 2f * Mathf.Sin(LostValues[num39] * (float)Math.PI);
									break;
								case "BetChips2":
									target = ZhuangWinChips.transform.GetChild(2).gameObject;
									LostChips[num39].transform.position = Vector3.Lerp(lostPos[num39], target.transform.position + target.transform.childCount * Vector3.up * 0.01f, LostValues[num39]) + Vector3.up * 2f * Mathf.Sin(LostValues[num39] * (float)Math.PI);
									break;
								case "BetChips3":
									target = ZhuangWinChips.transform.GetChild(3).gameObject;
									LostChips[num39].transform.position = Vector3.Lerp(lostPos[num39], target.transform.position + target.transform.childCount * Vector3.up * 0.01f, LostValues[num39]) + Vector3.up * 2f * Mathf.Sin(LostValues[num39] * (float)Math.PI);
									break;
								case "BetChips4":
									target = ZhuangWinChips.transform.GetChild(4).gameObject;
									LostChips[num39].transform.position = Vector3.Lerp(lostPos[num39], target.transform.position + target.transform.childCount * Vector3.up * 0.01f, LostValues[num39]) + Vector3.up * 2f * Mathf.Sin(LostValues[num39] * (float)Math.PI);
									break;
								case "BetChips5":
									target = ZhuangWinChips.transform.GetChild(5).gameObject;
									LostChips[num39].transform.position = Vector3.Lerp(lostPos[num39], target.transform.position + target.transform.childCount * Vector3.up * 0.01f, LostValues[num39]) + Vector3.up * 2f * Mathf.Sin(LostValues[num39] * (float)Math.PI);
									break;
								case "BetChips6":
									target = ZhuangWinChips.transform.GetChild(6).gameObject;
									LostChips[num39].transform.position = Vector3.Lerp(lostPos[num39], target.transform.position + target.transform.childCount * Vector3.up * 0.01f, LostValues[num39]) + Vector3.up * 2f * Mathf.Sin(LostValues[num39] * (float)Math.PI);
									break;
								}
								if (LostValues[num39] > 1f && target != null)
								{
									LostChips[num39].transform.SetParent(target.transform);
									LostChips[num39].transform.position = target.transform.position + target.transform.childCount * Vector3.up * 0.01f;
								}
								if (LostValues[num39] <= 1f)
								{
									if (num39 == 0)
									{
										List<float> lostValues;
										int index;
										(lostValues = LostValues)[index = num39] = lostValues[index] + 0.04f;
									}
									else if (LostValues[num39 - 1] > 0.08f)
									{
										List<float> lostValues;
										int index2;
										(lostValues = LostValues)[index2 = num39] = lostValues[index2] + 0.04f;
									}
								}
							}
							yield return new WaitForSeconds(0.02f);
						}
						if (WinValues.Count == 0)
						{
							yield return new WaitForSeconds(3f);
						}
						while (WinValues.Count > 0)
						{
							if (WinValues[WinValues.Count - 1] > 1f)
							{
								UnityEngine.Debug.Log("需要停止了");
								break;
							}
							for (int num40 = 0; num40 < WinChips.Count; num40++)
							{
								switch (WinChips[num40].transform.parent.parent.name)
								{
								case "BetChips0":
								{
									GameObject target2 = WinChips[num40].transform.parent.gameObject;
									WinChips[num40].transform.position = Vector3.Lerp(ZhuangWinChips.transform.position, target2.transform.position + num40 * Vector3.up * 0.01f, WinValues[num40]) + Vector3.up * 2f * Mathf.Sin(WinValues[num40] * (float)Math.PI);
									break;
								}
								case "BetChips1":
									WinChips[num40].transform.position = Vector3.Lerp(ZhuangWinChips.transform.position, WinChips[num40].transform.parent.position, WinValues[num40]) + Vector3.up * 2f * Mathf.Sin(WinValues[num40] * (float)Math.PI);
									break;
								case "BetChips2":
									WinChips[num40].transform.position = Vector3.Lerp(ZhuangWinChips.transform.position, WinChips[num40].transform.parent.position, WinValues[num40]) + Vector3.up * 2f * Mathf.Sin(WinValues[num40] * (float)Math.PI);
									break;
								case "BetChips3":
									WinChips[num40].transform.position = Vector3.Lerp(ZhuangWinChips.transform.position, WinChips[num40].transform.parent.position, WinValues[num40]) + Vector3.up * 2f * Mathf.Sin(WinValues[num40] * (float)Math.PI);
									break;
								case "BetChips4":
									WinChips[num40].transform.position = Vector3.Lerp(ZhuangWinChips.transform.position, WinChips[num40].transform.parent.position, WinValues[num40]) + Vector3.up * 2f * Mathf.Sin(WinValues[num40] * (float)Math.PI);
									break;
								case "BetChips5":
									WinChips[num40].transform.position = Vector3.Lerp(ZhuangWinChips.transform.position, WinChips[num40].transform.parent.position, WinValues[num40]) + Vector3.up * 2f * Mathf.Sin(WinValues[num40] * (float)Math.PI);
									break;
								case "BetChips6":
									WinChips[num40].transform.position = Vector3.Lerp(ZhuangWinChips.transform.position, WinChips[num40].transform.parent.position, WinValues[num40]) + Vector3.up * 2f * Mathf.Sin(WinValues[num40] * (float)Math.PI);
									break;
								}
								if (WinValues[num40] <= 1f)
								{
									if (num40 == 0)
									{
										List<float> lostValues;
										int index3;
										(lostValues = WinValues)[index3 = num40] = lostValues[index3] + 0.04f;
									}
									else if (WinValues[num40 - 1] > 0.08f)
									{
										List<float> lostValues;
										int index4;
										(lostValues = WinValues)[index4 = num40] = lostValues[index4] + 0.04f;
									}
								}
							}
							yield return new WaitForSeconds(0.02f);
						}
						float movevalue = 0f;
						while (WinValues.Count > 0 && !(movevalue > 1f))
						{
							for (int num41 = 0; num41 < 7; num41++)
							{
								switch (BaiJiaLe_GameInfo.getInstance().GameResult.ludan)
								{
								case 0:
									for (int num84 = 0; num84 < BetChips[num41].transform.Find("win0").childCount; num84++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win0").gameObject;
										BetChips[num41].transform.Find("win0").GetChild(num84).position = Vector3.Lerp(target45.transform.position + num84 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip0").position + num84 * Vector3.up * 0.01f, movevalue);
									}
									for (int num85 = 0; num85 < BetChips[num41].transform.Find("bet0").childCount; num85++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet0").gameObject;
										BetChips[num41].transform.Find("bet0").GetChild(num85).position = Vector3.Lerp(target45.transform.position + num85 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip1").position + num85 * Vector3.up * 0.01f, movevalue);
									}
									break;
								case 1:
									for (int num64 = 0; num64 < BetChips[num41].transform.Find("win1").childCount; num64++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win1").gameObject;
										BetChips[num41].transform.Find("win1").GetChild(num64).position = Vector3.Lerp(target45.transform.position + num64 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip0").position + num64 * Vector3.up * 0.01f, movevalue);
									}
									for (int num65 = 0; num65 < BetChips[num41].transform.Find("bet1").childCount; num65++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet1").gameObject;
										BetChips[num41].transform.Find("bet1").GetChild(num65).position = Vector3.Lerp(target45.transform.position + num65 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip1").position + num65 * Vector3.up * 0.01f, movevalue);
									}
									break;
								case 2:
									for (int num78 = 0; num78 < BetChips[num41].transform.Find("win2").childCount; num78++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win2").gameObject;
										BetChips[num41].transform.Find("win2").GetChild(num78).position = Vector3.Lerp(target45.transform.position + num78 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip0").position + num78 * Vector3.up * 0.01f, movevalue);
									}
									for (int num79 = 0; num79 < BetChips[num41].transform.Find("bet2").childCount; num79++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet2").gameObject;
										BetChips[num41].transform.Find("bet2").GetChild(num79).position = Vector3.Lerp(target45.transform.position + num79 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip1").position + num79 * Vector3.up * 0.01f, movevalue);
									}
									break;
								case 3:
									for (int num86 = 0; num86 < BetChips[num41].transform.Find("win0").childCount; num86++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win0").gameObject;
										BetChips[num41].transform.Find("win0").GetChild(num86).position = Vector3.Lerp(target45.transform.position + num86 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip0").position + num86 * Vector3.up * 0.01f, movevalue);
									}
									for (int num87 = 0; num87 < BetChips[num41].transform.Find("bet0").childCount; num87++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet0").gameObject;
										BetChips[num41].transform.Find("bet0").GetChild(num87).position = Vector3.Lerp(target45.transform.position + num87 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip1").position + num87 * Vector3.up * 0.01f, movevalue);
									}
									for (int num88 = 0; num88 < BetChips[num41].transform.Find("win3").childCount; num88++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win3").gameObject;
										BetChips[num41].transform.Find("win3").GetChild(num88).position = Vector3.Lerp(target45.transform.position + num88 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip2").position + num88 * Vector3.up * 0.01f, movevalue);
									}
									for (int num89 = 0; num89 < BetChips[num41].transform.Find("bet3").childCount; num89++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet3").gameObject;
										BetChips[num41].transform.Find("bet3").GetChild(num89).position = Vector3.Lerp(target45.transform.position + num89 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip3").position + num89 * Vector3.up * 0.01f, movevalue);
									}
									break;
								case 4:
									for (int num54 = 0; num54 < BetChips[num41].transform.Find("win1").childCount; num54++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win1").gameObject;
										BetChips[num41].transform.Find("win1").GetChild(num54).position = Vector3.Lerp(target45.transform.position + num54 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip0").position + num54 * Vector3.up * 0.01f, movevalue);
									}
									for (int num55 = 0; num55 < BetChips[num41].transform.Find("bet1").childCount; num55++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet1").gameObject;
										BetChips[num41].transform.Find("bet1").GetChild(num55).position = Vector3.Lerp(target45.transform.position + num55 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip1").position + num55 * Vector3.up * 0.01f, movevalue);
									}
									for (int num56 = 0; num56 < BetChips[num41].transform.Find("win3").childCount; num56++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win3").gameObject;
										BetChips[num41].transform.Find("win3").GetChild(num56).position = Vector3.Lerp(target45.transform.position + num56 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip2").position + num56 * Vector3.up * 0.01f, movevalue);
									}
									for (int num57 = 0; num57 < BetChips[num41].transform.Find("bet3").childCount; num57++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet3").gameObject;
										BetChips[num41].transform.Find("bet3").GetChild(num57).position = Vector3.Lerp(target45.transform.position + num57 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip3").position + num57 * Vector3.up * 0.01f, movevalue);
									}
									break;
								case 5:
									for (int num80 = 0; num80 < BetChips[num41].transform.Find("win2").childCount; num80++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win2").gameObject;
										BetChips[num41].transform.Find("win2").GetChild(num80).position = Vector3.Lerp(target45.transform.position + num80 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip0").position + num80 * Vector3.up * 0.01f, movevalue);
									}
									for (int num81 = 0; num81 < BetChips[num41].transform.Find("bet2").childCount; num81++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet2").gameObject;
										BetChips[num41].transform.Find("bet2").GetChild(num81).position = Vector3.Lerp(target45.transform.position + num81 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip1").position + num81 * Vector3.up * 0.01f, movevalue);
									}
									for (int num82 = 0; num82 < BetChips[num41].transform.Find("win3").childCount; num82++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win3").gameObject;
										BetChips[num41].transform.Find("win3").GetChild(num82).position = Vector3.Lerp(target45.transform.position + num82 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip2").position + num82 * Vector3.up * 0.01f, movevalue);
									}
									for (int num83 = 0; num83 < BetChips[num41].transform.Find("bet3").childCount; num83++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet3").gameObject;
										BetChips[num41].transform.Find("bet3").GetChild(num83).position = Vector3.Lerp(target45.transform.position + num83 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip3").position + num83 * Vector3.up * 0.01f, movevalue);
									}
									break;
								case 6:
									for (int num74 = 0; num74 < BetChips[num41].transform.Find("win0").childCount; num74++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win0").gameObject;
										BetChips[num41].transform.Find("win0").GetChild(num74).position = Vector3.Lerp(target45.transform.position + num74 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip0").position + num74 * Vector3.up * 0.01f, movevalue);
									}
									for (int num75 = 0; num75 < BetChips[num41].transform.Find("bet0").childCount; num75++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet0").gameObject;
										BetChips[num41].transform.Find("bet0").GetChild(num75).position = Vector3.Lerp(target45.transform.position + num75 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip1").position + num75 * Vector3.up * 0.01f, movevalue);
									}
									for (int num76 = 0; num76 < BetChips[num41].transform.Find("win4").childCount; num76++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win4").gameObject;
										BetChips[num41].transform.Find("win4").GetChild(num76).position = Vector3.Lerp(target45.transform.position + num76 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip2").position + num76 * Vector3.up * 0.01f, movevalue);
									}
									for (int num77 = 0; num77 < BetChips[num41].transform.Find("bet4").childCount; num77++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet4").gameObject;
										BetChips[num41].transform.Find("bet4").GetChild(num77).position = Vector3.Lerp(target45.transform.position + num77 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip3").position + num77 * Vector3.up * 0.01f, movevalue);
									}
									break;
								case 7:
									for (int num66 = 0; num66 < BetChips[num41].transform.Find("win1").childCount; num66++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win1").gameObject;
										BetChips[num41].transform.Find("win1").GetChild(num66).position = Vector3.Lerp(target45.transform.position + num66 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip0").position + num66 * Vector3.up * 0.01f, movevalue);
									}
									for (int num67 = 0; num67 < BetChips[num41].transform.Find("bet1").childCount; num67++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet1").gameObject;
										BetChips[num41].transform.Find("bet1").GetChild(num67).position = Vector3.Lerp(target45.transform.position + num67 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip1").position + num67 * Vector3.up * 0.01f, movevalue);
									}
									for (int num68 = 0; num68 < BetChips[num41].transform.Find("win4").childCount; num68++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win4").gameObject;
										BetChips[num41].transform.Find("win4").GetChild(num68).position = Vector3.Lerp(target45.transform.position + num68 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip2").position + num68 * Vector3.up * 0.01f, movevalue);
									}
									for (int num69 = 0; num69 < BetChips[num41].transform.Find("bet4").childCount; num69++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet4").gameObject;
										BetChips[num41].transform.Find("bet4").GetChild(num69).position = Vector3.Lerp(target45.transform.position + num69 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip3").position + num69 * Vector3.up * 0.01f, movevalue);
									}
									break;
								case 8:
									for (int num70 = 0; num70 < BetChips[num41].transform.Find("win2").childCount; num70++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win2").gameObject;
										BetChips[num41].transform.Find("win2").GetChild(num70).position = Vector3.Lerp(target45.transform.position + num70 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip0").position + num70 * Vector3.up * 0.01f, movevalue);
									}
									for (int num71 = 0; num71 < BetChips[num41].transform.Find("bet2").childCount; num71++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet2").gameObject;
										BetChips[num41].transform.Find("bet2").GetChild(num71).position = Vector3.Lerp(target45.transform.position + num71 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip1").position + num71 * Vector3.up * 0.01f, movevalue);
									}
									for (int num72 = 0; num72 < BetChips[num41].transform.Find("win4").childCount; num72++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win4").gameObject;
										BetChips[num41].transform.Find("win4").GetChild(num72).position = Vector3.Lerp(target45.transform.position + num72 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip2").position + num72 * Vector3.up * 0.01f, movevalue);
									}
									for (int num73 = 0; num73 < BetChips[num41].transform.Find("bet4").childCount; num73++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet4").gameObject;
										BetChips[num41].transform.Find("bet4").GetChild(num73).position = Vector3.Lerp(target45.transform.position + num73 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip3").position + num73 * Vector3.up * 0.01f, movevalue);
									}
									break;
								case 9:
									for (int num58 = 0; num58 < BetChips[num41].transform.Find("win0").childCount; num58++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win0").gameObject;
										BetChips[num41].transform.Find("win0").GetChild(num58).position = Vector3.Lerp(target45.transform.position + num58 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip0").position + num58 * Vector3.up * 0.01f, movevalue);
									}
									for (int num59 = 0; num59 < BetChips[num41].transform.Find("bet0").childCount; num59++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet0").gameObject;
										BetChips[num41].transform.Find("bet0").GetChild(num59).position = Vector3.Lerp(target45.transform.position + num59 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip1").position + num59 * Vector3.up * 0.01f, movevalue);
									}
									for (int num60 = 0; num60 < BetChips[num41].transform.Find("win3").childCount; num60++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win3").gameObject;
										BetChips[num41].transform.Find("win3").GetChild(num60).position = Vector3.Lerp(target45.transform.position + num60 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip2").position + num60 * Vector3.up * 0.01f, movevalue);
									}
									for (int num61 = 0; num61 < BetChips[num41].transform.Find("bet3").childCount; num61++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet3").gameObject;
										BetChips[num41].transform.Find("bet3").GetChild(num61).position = Vector3.Lerp(target45.transform.position + num61 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip3").position + num61 * Vector3.up * 0.01f, movevalue);
									}
									for (int num62 = 0; num62 < BetChips[num41].transform.Find("win4").childCount; num62++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win4").gameObject;
										BetChips[num41].transform.Find("win4").GetChild(num62).position = Vector3.Lerp(target45.transform.position + num62 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip4").position + num62 * Vector3.up * 0.01f, movevalue);
									}
									for (int num63 = 0; num63 < BetChips[num41].transform.Find("bet4").childCount; num63++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet4").gameObject;
										BetChips[num41].transform.Find("bet4").GetChild(num63).position = Vector3.Lerp(target45.transform.position + num63 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip5").position + num63 * Vector3.up * 0.01f, movevalue);
									}
									break;
								case 10:
									for (int num48 = 0; num48 < BetChips[num41].transform.Find("win1").childCount; num48++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win1").gameObject;
										BetChips[num41].transform.Find("win1").GetChild(num48).position = Vector3.Lerp(target45.transform.position + num48 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip0").position + num48 * Vector3.up * 0.01f, movevalue);
									}
									for (int num49 = 0; num49 < BetChips[num41].transform.Find("bet1").childCount; num49++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet1").gameObject;
										BetChips[num41].transform.Find("bet1").GetChild(num49).position = Vector3.Lerp(target45.transform.position + num49 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip1").position + num49 * Vector3.up * 0.01f, movevalue);
									}
									for (int num50 = 0; num50 < BetChips[num41].transform.Find("win3").childCount; num50++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win3").gameObject;
										BetChips[num41].transform.Find("win3").GetChild(num50).position = Vector3.Lerp(target45.transform.position + num50 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip2").position + num50 * Vector3.up * 0.01f, movevalue);
									}
									for (int num51 = 0; num51 < BetChips[num41].transform.Find("bet3").childCount; num51++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet3").gameObject;
										BetChips[num41].transform.Find("bet3").GetChild(num51).position = Vector3.Lerp(target45.transform.position + num51 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip3").position + num51 * Vector3.up * 0.01f, movevalue);
									}
									for (int num52 = 0; num52 < BetChips[num41].transform.Find("win4").childCount; num52++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win4").gameObject;
										BetChips[num41].transform.Find("win4").GetChild(num52).position = Vector3.Lerp(target45.transform.position + num52 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip4").position + num52 * Vector3.up * 0.01f, movevalue);
									}
									for (int num53 = 0; num53 < BetChips[num41].transform.Find("bet4").childCount; num53++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet4").gameObject;
										BetChips[num41].transform.Find("bet4").GetChild(num53).position = Vector3.Lerp(target45.transform.position + num53 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip5").position + num53 * Vector3.up * 0.01f, movevalue);
									}
									break;
								case 11:
									for (int num42 = 0; num42 < BetChips[num41].transform.Find("win2").childCount; num42++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win2").gameObject;
										BetChips[num41].transform.Find("win2").GetChild(num42).position = Vector3.Lerp(target45.transform.position + num42 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip0").position + num42 * Vector3.up * 0.01f, movevalue);
									}
									for (int num43 = 0; num43 < BetChips[num41].transform.Find("bet2").childCount; num43++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet2").gameObject;
										BetChips[num41].transform.Find("bet2").GetChild(num43).position = Vector3.Lerp(target45.transform.position + num43 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip1").position + num43 * Vector3.up * 0.01f, movevalue);
									}
									for (int num44 = 0; num44 < BetChips[num41].transform.Find("win3").childCount; num44++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win3").gameObject;
										BetChips[num41].transform.Find("win3").GetChild(num44).position = Vector3.Lerp(target45.transform.position + num44 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip2").position + num44 * Vector3.up * 0.01f, movevalue);
									}
									for (int num45 = 0; num45 < BetChips[num41].transform.Find("bet3").childCount; num45++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet3").gameObject;
										BetChips[num41].transform.Find("bet3").GetChild(num45).position = Vector3.Lerp(target45.transform.position + num45 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip3").position + num45 * Vector3.up * 0.01f, movevalue);
									}
									for (int num46 = 0; num46 < BetChips[num41].transform.Find("win4").childCount; num46++)
									{
										GameObject target45 = BetChips[num41].transform.Find("win4").gameObject;
										BetChips[num41].transform.Find("win4").GetChild(num46).position = Vector3.Lerp(target45.transform.position + num46 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip4").position + num46 * Vector3.up * 0.01f, movevalue);
									}
									for (int num47 = 0; num47 < BetChips[num41].transform.Find("bet4").childCount; num47++)
									{
										GameObject target45 = BetChips[num41].transform.Find("bet4").gameObject;
										BetChips[num41].transform.Find("bet4").GetChild(num47).position = Vector3.Lerp(target45.transform.position + num47 * Vector3.up * 0.01f, BetChips[num41].transform.Find("pchip5").position + num47 * Vector3.up * 0.01f, movevalue);
									}
									break;
								}
							}
							movevalue += 0.04f;
							yield return new WaitForSeconds(0.02f);
						}
						int OtherScore = 0;
						if (BaiJiaLe_GameInfo.getInstance().GameWinResult.Length > 0)
						{
							for (int num90 = 0; num90 < BaiJiaLe_GameInfo.getInstance().GameWinResult.Length; num90++)
							{
								BaiJiaLe_Income baiJiaLe_Income = new BaiJiaLe_Income();
								Dictionary<string, object> dictionary = BaiJiaLe_GameInfo.getInstance().GameWinResult[num90] as Dictionary<string, object>;
								baiJiaLe_Income.seatId = (int)dictionary["seatId"];
								baiJiaLe_Income.score = (int)dictionary["score"];
								if (MyselfSeat == baiJiaLe_Income.seatId)
								{
									if (baiJiaLe_Income.score > 0)
									{
										Players[0].transform.Find("WinText").gameObject.SetActive(value: true);
										Players[0].transform.Find("WinText").GetComponent<Text>().text = "+" + baiJiaLe_Income.score.ToString();
									}
									else if (baiJiaLe_Income.score < 0)
									{
										Players[0].transform.Find("LostText").gameObject.SetActive(value: true);
										Players[0].transform.Find("LostText").GetComponent<Text>().text = baiJiaLe_Income.score.ToString();
									}
								}
								else if (FirstSeat == baiJiaLe_Income.seatId)
								{
									if (baiJiaLe_Income.score > 0)
									{
										Players[1].transform.Find("WinText").gameObject.SetActive(value: true);
										Players[1].transform.Find("WinText").GetComponent<Text>().text = "+" + baiJiaLe_Income.score.ToString();
									}
									else if (baiJiaLe_Income.score < 0)
									{
										Players[1].transform.Find("LostText").gameObject.SetActive(value: true);
										Players[1].transform.Find("LostText").GetComponent<Text>().text = baiJiaLe_Income.score.ToString();
									}
								}
								else if (SecondSeat == baiJiaLe_Income.seatId)
								{
									if (baiJiaLe_Income.score > 0)
									{
										Players[2].transform.Find("WinText").gameObject.SetActive(value: true);
										Players[2].transform.Find("WinText").GetComponent<Text>().text = "+" + baiJiaLe_Income.score.ToString();
									}
									else if (baiJiaLe_Income.score < 0)
									{
										Players[2].transform.Find("LostText").gameObject.SetActive(value: true);
										Players[2].transform.Find("LostText").GetComponent<Text>().text = baiJiaLe_Income.score.ToString();
									}
								}
								else if (ThirdSeat == baiJiaLe_Income.seatId)
								{
									if (baiJiaLe_Income.score > 0)
									{
										Players[3].transform.Find("WinText").gameObject.SetActive(value: true);
										Players[3].transform.Find("WinText").GetComponent<Text>().text = "+" + baiJiaLe_Income.score.ToString();
									}
									else if (baiJiaLe_Income.score < 0)
									{
										Players[3].transform.Find("LostText").gameObject.SetActive(value: true);
										Players[3].transform.Find("LostText").GetComponent<Text>().text = baiJiaLe_Income.score.ToString();
									}
								}
								else if (FourthSeat == baiJiaLe_Income.seatId)
								{
									if (baiJiaLe_Income.score > 0)
									{
										Players[4].transform.Find("WinText").gameObject.SetActive(value: true);
										Players[4].transform.Find("WinText").GetComponent<Text>().text = "+" + baiJiaLe_Income.score.ToString();
									}
									else if (baiJiaLe_Income.score < 0)
									{
										Players[4].transform.Find("LostText").gameObject.SetActive(value: true);
										Players[4].transform.Find("LostText").GetComponent<Text>().text = baiJiaLe_Income.score.ToString();
									}
								}
								else if (LuckySeat == baiJiaLe_Income.seatId)
								{
									if (baiJiaLe_Income.score > 0)
									{
										Players[5].transform.Find("WinText").gameObject.SetActive(value: true);
										Players[5].transform.Find("WinText").GetComponent<Text>().text = "+" + baiJiaLe_Income.score.ToString();
									}
									else if (baiJiaLe_Income.score < 0)
									{
										Players[5].transform.Find("LostText").gameObject.SetActive(value: true);
										Players[5].transform.Find("LostText").GetComponent<Text>().text = baiJiaLe_Income.score.ToString();
									}
								}
								else
								{
									OtherScore += baiJiaLe_Income.score;
								}
							}
							if (OtherScore > 0)
							{
								Players[6].transform.Find("WinText").gameObject.SetActive(value: true);
								Players[6].transform.Find("WinText").GetComponent<Text>().text = "+" + OtherScore;
							}
							else if (OtherScore < 0)
							{
								Players[6].transform.Find("LostText").gameObject.SetActive(value: true);
								Players[6].transform.Find("LostText").GetComponent<Text>().text = OtherScore.ToString();
							}
						}
						DealerAnime.GetComponent<Animator>().SetBool("IsRecycle", value: true);
						ShowZhuangWin.SetActive(value: false);
						ShowHeWin.SetActive(value: false);
						ShowXianWin.SetActive(value: false);
						ShowWinPanel_Zhuang.SetActive(value: false);
						ShowWinPanel_Xian.SetActive(value: false);
						ShowWinPanel_He.SetActive(value: false);
						ShowWinPanel_ZhuangDui.SetActive(value: false);
						ShowWinPanel_XianDui.SetActive(value: false);
						ZhuangCount.SetActive(value: false);
						XianCount.SetActive(value: false);
					}

					public void ShowZhuangCount()
					{
						ZhuangCount.SetActive(value: true);
						ZhuangCount.GetComponent<MeshRenderer>().material.mainTexture = BaiJiaLe_LuDanDate.instance.Numbers[BaiJiaLe_GameInfo.getInstance().ZhuangValue];
					}

					public void ShowXianCount()
					{
						XianCount.SetActive(value: true);
						XianCount.GetComponent<MeshRenderer>().material.mainTexture = BaiJiaLe_LuDanDate.instance.Numbers[BaiJiaLe_GameInfo.getInstance().XianValue];
					}

					private IEnumerator CameraMove_IE()
					{
						while (true)
						{
							if (CameraLock)
							{
								if (CameraType == 0)
								{
									MainCamera.transform.position = Vector3.Lerp(CameraPos[0].transform.position, CameraPos[1].transform.position, moveValue);
									MainCamera.transform.rotation = Quaternion.Lerp(CameraPos[0].transform.rotation, CameraPos[1].transform.rotation, moveValue);
									MainCamera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(5f, 5.5f, moveValue);
									moveValue += 0.08f;
								}
								else if (CameraType == 1)
								{
									MainCamera.transform.position = Vector3.Lerp(CameraPos[1].transform.position, CameraPos[2].transform.position, moveValue);
									MainCamera.transform.rotation = Quaternion.Lerp(CameraPos[1].transform.rotation, CameraPos[2].transform.rotation, moveValue);
									MainCamera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(5.5f, 5f, moveValue);
									moveValue += 0.04f;
								}
								else if (CameraType == 2)
								{
									MainCamera.transform.position = Vector3.Lerp(CameraPos[2].transform.position, CameraPos[0].transform.position, moveValue);
									MainCamera.transform.rotation = Quaternion.Lerp(CameraPos[2].transform.rotation, CameraPos[0].transform.rotation, moveValue);
									moveValue += 0.1f;
								}
								if (moveValue >= 1f)
								{
									switch (CameraType)
									{
									case 0:
										for (int k = 0; k < Players.Length; k++)
										{
											Players[k].SetActive(value: false);
										}
										break;
									case 1:
										if (MyselfID != -1)
										{
											Players[0].SetActive(value: true);
										}
										if (FirstID != -1)
										{
											Players[1].SetActive(value: true);
										}
										if (SecondID != -1)
										{
											Players[2].SetActive(value: true);
										}
										if (ThirdID != -1)
										{
											Players[3].SetActive(value: true);
										}
										if (FourthID != -1)
										{
											Players[4].SetActive(value: true);
										}
										if (LuckyID != -1)
										{
											Players[5].SetActive(value: true);
										}
										Players[6].SetActive(value: true);
										for (int j = 0; j < Players.Length; j++)
										{
											Players[j].transform.position = PlayerPos[1].transform.GetChild(j).position;
										}
										break;
									case 2:
										Players[0].SetActive(value: false);
										if (FirstID != -1)
										{
											Players[1].SetActive(value: true);
										}
										if (SecondID != -1)
										{
											Players[2].SetActive(value: true);
										}
										if (ThirdID != -1)
										{
											Players[3].SetActive(value: true);
										}
										if (FourthID != -1)
										{
											Players[4].SetActive(value: true);
										}
										if (LuckyID != -1)
										{
											Players[5].SetActive(value: true);
										}
										Players[6].SetActive(value: true);
										for (int i = 0; i < Players.Length; i++)
										{
											Players[i].transform.position = PlayerPos[0].transform.GetChild(i).position;
										}
										break;
									}
									CameraLock = false;
								}
							}
							yield return new WaitForSeconds(0.02f);
						}
					}

					private void TimeRun()
					{
						while (true)
						{
							Thread.Sleep(1000);
							if (betTime >= 0)
							{
								betTime--;
							}
						}
					}

					private IEnumerator GameProcess()
					{
						while (true)
						{
							if (betTime >= 0)
							{
								if (!CountDown.activeSelf)
								{
									CountDown.SetActive(value: true);
								}
								CountDown.GetComponent<TextMesh>().text = betTime.ToString();
								if (betTime == 0)
								{
									BetLock = true;
								}
							}
							else if (CountDown.activeSelf)
							{
								CountDown.SetActive(value: false);
								BetLock = true;
							}
							yield return new WaitForSeconds(0.1f);
						}
					}

					public void OnApplicationPause(bool pause)
					{
						if (!pause)
						{
							SceneManager.LoadSceneAsync("BaiJiaLe_Game");
						}
					}
				}
