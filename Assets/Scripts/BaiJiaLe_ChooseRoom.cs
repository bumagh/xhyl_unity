using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaiJiaLe_ChooseRoom : MonoBehaviour
{
	public static BaiJiaLe_ChooseRoom instance;

	public Text NickName;

	public Text Gold;

	public Text Score;

	public Button ReturnBtn;

	public GameObject RoomContent;

	public GameObject MsgPanel;

	private Thread TimeThread;

	public int[] DJS;

	private void Awake()
	{
		instance = this;
		ReturnBtn = base.transform.Find("ReturnBtn").GetComponent<Button>();
		NickName = base.transform.Find("UserInfo/NickName/Text").GetComponent<Text>();
		Gold = base.transform.Find("UserInfo/Gold/Text").GetComponent<Text>();
		Score = base.transform.Find("UserInfo/Score/Text").GetComponent<Text>();
		RoomContent = base.transform.Find("ScrollView/Viewport/Content").gameObject;
		MsgPanel = base.transform.Find("MsgPanel").gameObject;
		BaiJiaLe_GameInfo.IsJoinDesk = false;
	}

	private void Start()
	{
		AddListener();
		TimeThread = new Thread(TimeThread_Run);
		TimeThread.Start();
		BaiJiaLe_GameInfo.getInstance().UserId = ZH2_GVars.username;
		BaiJiaLe_GameInfo.getInstance().Pwd = ZH2_GVars.pwd;
		BaiJiaLe_GameInfo.getInstance().IP = ZH2_GVars.IPAddress;
		UpdateLuDan();
		GetUserInfo();
		if (BaiJiaLe_NetMngr.GetSingleton().mIsGetIP)
		{
			BaiJiaLe_LoadPanel.instance.IsActive = true;
		}
		BaiJiaLe_NetMngr.GetSingleton().mIsGetIP = true;
	}

	private void Update()
	{
		for (int i = 0; i < DJS.Length; i++)
		{
			if (DJS[i] > 0)
			{
				RoomContent.transform.GetChild(i).Find("TimeType").GetComponent<Text>()
					.text = DJS[i].ToString();
				}
				else
				{
					RoomContent.transform.GetChild(i).Find("TimeType").GetComponent<Text>()
						.text = "结算中";
					}
				}
			}

			public void GetUserInfo()
			{
				NickName.text = BaiJiaLe_GameInfo.getInstance().NickName;
				Gold.text = BaiJiaLe_GameInfo.getInstance().GameGold;
				Score.text = BaiJiaLe_GameInfo.getInstance().GameScore;
			}

			private void AddListener()
			{
				ReturnBtn.onClick.AddListener(delegate
				{
					UnityEngine.Object.Destroy(BaiJiaLe_LuDanDate.instance.gameObject);
					ZH2_GVars.isStartedFromGame = true;
					SceneManager.LoadSceneAsync(0);
				});
				for (int i = 0; i < RoomContent.transform.childCount; i++)
				{
					GameObject Obj = RoomContent.transform.GetChild(i).gameObject;
					RoomContent.transform.GetChild(i).Find("EnterRoom").GetComponent<Button>()
						.onClick.AddListener(delegate
						{
							if (BaiJiaLe_GameInfo.getInstance().GameGold == "0")
							{
								MsgPanel.GetComponent<BaiJiaLe_WaitHide>().Show("金币不足，无法进入");
							}
							else
							{
								BaiJiaLe_GameInfo.getInstance().RoomID = Obj.name;
								BaiJiaLe_Sockets.GetSingleton().SendDeskInfo(int.Parse(Obj.name));
							}
						});
					}
				}

				public void UpdateLuDan()
				{
					DJS = new int[BaiJiaLe_GameInfo.getInstance().GameDeskList.Count];
					for (int i = 0; i < 4; i++)
					{
						if (i < BaiJiaLe_GameInfo.getInstance().GameDeskList.Count)
						{
							List<string> list = new List<string>();
							if (BaiJiaLe_GameInfo.getInstance().GameDeskList[i].ludan != null)
							{
								string ludan = BaiJiaLe_GameInfo.getInstance().GameDeskList[i].ludan;
								for (int j = 0; j < ludan.Length; j++)
								{
									list.Add(ludan[j].ToString());
								}
							}
							RoomContent.transform.GetChild(i).gameObject.SetActive(value: true);
							RoomContent.transform.GetChild(i).name = BaiJiaLe_GameInfo.getInstance().GameDeskList[i].id.ToString();
							RoomContent.transform.GetChild(i).GetComponent<BaiJiaLe_UpdateLuDan2>().ShowLuDan(list.ToArray());
							RoomContent.transform.GetChild(i).Find("RoomName").GetComponent<Text>()
								.text = BaiJiaLe_GameInfo.getInstance().GameDeskList[i].name;
								RoomContent.transform.GetChild(i).Find("XianHong").GetComponent<Text>()
									.text = BaiJiaLe_GameInfo.getInstance().GameDeskList[i].minBet + "-" + BaiJiaLe_GameInfo.getInstance().GameDeskList[i].maxBet;
									int num = 0;
									int num2 = 0;
									int num3 = 0;
									int num4 = 0;
									int num5 = 0;
									for (int k = 0; k < list.Count; k++)
									{
										switch (list[k])
										{
										case "0":
											num++;
											break;
										case "1":
											num2++;
											break;
										case "2":
											num3++;
											break;
										case "3":
											num++;
											num4++;
											break;
										case "4":
											num2++;
											num4++;
											break;
										case "5":
											num3++;
											num4++;
											break;
										case "6":
											num++;
											num5++;
											break;
										case "7":
											num2++;
											num5++;
											break;
										case "8":
											num3++;
											num5++;
											break;
										case "9":
											num++;
											num4++;
											num5++;
											break;
										case "a":
											num2++;
											num4++;
											num5++;
											break;
										case "b":
											num3++;
											num4++;
											num5++;
											break;
										}
									}
									RoomContent.transform.GetChild(i).Find("Inning").GetComponent<Text>()
										.text = "局数 " + list.Count;
										RoomContent.transform.GetChild(i).Find("Zhuang").GetComponent<Text>()
											.text = "庄 " + num;
											RoomContent.transform.GetChild(i).Find("Xian").GetComponent<Text>()
												.text = "闲 " + num2;
												RoomContent.transform.GetChild(i).Find("He").GetComponent<Text>()
													.text = "和 " + num3;
													RoomContent.transform.GetChild(i).Find("ZhuangDui").GetComponent<Text>()
														.text = "庄对 " + num4;
														RoomContent.transform.GetChild(i).Find("XianDui").GetComponent<Text>()
															.text = "闲对 " + num5;
															DJS[i] = BaiJiaLe_GameInfo.getInstance().GameDeskList[i].djs;
															if (BaiJiaLe_GameInfo.getInstance().GameDeskList[i].djs > 0)
															{
																RoomContent.transform.GetChild(i).Find("TimeType").GetComponent<Text>()
																	.text = BaiJiaLe_GameInfo.getInstance().GameDeskList[i].djs.ToString();
																}
																else
																{
																	RoomContent.transform.GetChild(i).Find("TimeType").GetComponent<Text>()
																		.text = "结算中";
																	}
																}
																else
																{
																	RoomContent.transform.GetChild(i).gameObject.SetActive(value: false);
																}
															}
														}

														private void TimeThread_Run()
														{
															while (true)
															{
																for (int i = 0; i < DJS.Length; i++)
																{
																	if (DJS[i] >= 0)
																	{
																		DJS[i]--;
																	}
																}
																Thread.Sleep(1000);
															}
														}
													}
