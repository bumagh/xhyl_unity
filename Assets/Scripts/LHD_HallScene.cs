using LitJson;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LHD_HallScene : MonoBehaviour
{
	public Image Head;

	public Text NickName;

	public Text Coin;

	public Button Return;

	public GameObject Content;

	public GameObject ItemPrefab;

	public SkeletonGraphic tuNvLang;

	public Material material;

	public static LHD_HallScene instance;

	private JsonData jdRoom;

	private float _time;

	private Coroutine getLuDan;

	private Coroutine getTimeIE;

	private List<LHD_TimeInfo> tempTime;

	private Coroutine waitSetResultList;

	private int posx_DLu;

	private int posy_DLu;

	public Sprite[] luDanSprs;

	private List<int> reslut_DLu = new List<int>();

	private void Awake()
	{
		instance = this;
		Head = base.transform.Find("Head").GetComponent<Image>();
		NickName = base.transform.Find("NickName/Text").GetComponent<Text>();
		Coin = base.transform.Find("Coin/Text").GetComponent<Text>();
		Return = base.transform.Find("Return").GetComponent<Button>();
		ItemPrefab = base.transform.Find("Content/Item").gameObject;
		Content = base.transform.Find("ScrollView/Viewport/Content").gameObject;
		tuNvLang = base.transform.Find("HeGuan").GetComponent<SkeletonGraphic>();
	}

	private void OnEnable()
	{
		LHD_GameInfo lHD_GameInfo = LHD_GameInfo.getInstance();
		lHD_GameInfo.resultListCall = (Action<JsonData>)Delegate.Combine(lHD_GameInfo.resultListCall, new Action<JsonData>(ResultListCall));
		LHD_GameInfo lHD_GameInfo2 = LHD_GameInfo.getInstance();
		lHD_GameInfo2.upDateTime = (Action<JsonData>)Delegate.Combine(lHD_GameInfo2.upDateTime, new Action<JsonData>(UpDateTime));
		if (getLuDan != null)
		{
			StopCoroutine(getLuDan);
		}
		getLuDan = StartCoroutine(GetLuDanIE());
		if (getTimeIE != null)
		{
			StopCoroutine(getTimeIE);
		}
		getTimeIE = StartCoroutine(GetTimeIE());
		material = Resources.Load<Material>("SkeletonGraphicDefault");
		if (material != null && tuNvLang != null)
		{
			tuNvLang.material = material;
		}
	}

	private void OnDisable()
	{
		LHD_GameInfo lHD_GameInfo = LHD_GameInfo.getInstance();
		lHD_GameInfo.resultListCall = (Action<JsonData>)Delegate.Remove(lHD_GameInfo.resultListCall, new Action<JsonData>(ResultListCall));
		LHD_GameInfo lHD_GameInfo2 = LHD_GameInfo.getInstance();
		lHD_GameInfo2.upDateTime = (Action<JsonData>)Delegate.Remove(lHD_GameInfo2.upDateTime, new Action<JsonData>(UpDateTime));
	}

	private void Start()
	{
		Return.onClick.AddListener(delegate
		{
			DropOutButton();
		});
	}

	private void Update()
	{
		if (base.gameObject.activeInHierarchy && jdRoom != ZH2_GVars.hallInfo2)
		{
			jdRoom = ZH2_GVars.hallInfo2;
			UpdateRoomList(jdRoom);
			UpdateUserInfo();
		}
		if (_time >= 5f)
		{
			_time = 0f;
			UpdateUserInfo();
		}
		_time += Time.deltaTime;
	}

	private IEnumerator GetLuDanIE()
	{
		yield return new WaitForSeconds(0.5f);
		while (base.gameObject.activeInHierarchy)
		{
			GetLuDan();
			yield return new WaitForSeconds(10f);
		}
	}

	private IEnumerator GetTimeIE()
	{
		while (true)
		{
			GetTime();
			yield return new WaitForSeconds(1f);
		}
	}

	private void GetTime()
	{
		if (LHD_NetMngr.GetSingleton() != null)
		{
			LHD_NetMngr.GetSingleton().MyCreateSocket.SendTime();
		}
	}

	public void GetLuDan()
	{
		if (LHD_NetMngr.GetSingleton() != null)
		{
			LHD_NetMngr.GetSingleton().MyCreateSocket.SendLuDan();
		}
	}

	private void UpDateTime(JsonData jd)
	{
		tempTime = new List<LHD_TimeInfo>();
		for (int i = 0; i < jd.Count; i++)
		{
			LHD_TimeInfo lHD_TimeInfo = new LHD_TimeInfo();
			lHD_TimeInfo.id = int.Parse(jd[i]["deskId"].ToString());
			lHD_TimeInfo.time = int.Parse(jd[i]["countdown"].ToString());
			LHD_TimeInfo item = lHD_TimeInfo;
			tempTime.Add(item);
		}
		for (int j = 0; j < Content.transform.childCount; j++)
		{
			for (int k = 0; k < tempTime.Count; k++)
			{
				if (Content.transform.GetChild(j).name == tempTime[k].id.ToString())
				{
					Text component = Content.transform.GetChild(j).Find("State").GetComponent<Text>();
					if (tempTime[k].time > 0)
					{
						component.text = "下注中\t" + tempTime[k].time;
					}
					else
					{
						component.text = "发牌中\t" + 0;
					}
					break;
				}
			}
		}
	}

	private void UpdateRoomList(JsonData jd)
	{
		float width = ItemPrefab.GetComponent<RectTransform>().rect.width;
		float height = ItemPrefab.GetComponent<RectTransform>().rect.height;
		Content.GetComponent<GridLayoutGroup>().cellSize = Vector2.right * width + Vector2.up * height;
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
		if (Content.transform.childCount >= jd.Count)
		{
			for (int j = 0; j < Content.transform.childCount; j++)
			{
				if (j < LHD_GameInfo.getInstance().roomlist.Count)
				{
					Content.transform.GetChild(j).gameObject.SetActive(value: true);
					Content.transform.GetChild(j).name = LHD_GameInfo.getInstance().roomlist[j].id.ToString();
					Content.transform.GetChild(j).Find("roomName/Text").GetComponent<Text>()
						.text = LHD_GameInfo.getInstance().roomlist[j].name;
						Content.transform.GetChild(j).Find("min/Text").GetComponent<Text>()
							.text = "最小携带: " + LHD_GameInfo.getInstance().roomlist[j].minGold;
							Content.transform.GetChild(j).Find("xianhong/Text").GetComponent<Text>()
								.text = "限红: " + LHD_GameInfo.getInstance().roomlist[j].maxBet;
								Content.transform.GetChild(j).Find("State").GetComponent<Text>()
									.text = string.Empty;
									Content.transform.GetChild(j).Find("Count").GetComponent<Text>()
										.text = LHD_GameInfo.getInstance().roomlist[j].onlineNumber.ToString();
									}
									else
									{
										Content.transform.GetChild(j).name = "-100";
										Content.transform.GetChild(j).gameObject.SetActive(value: false);
									}
								}
								return;
							}
							for (int k = 0; k < LHD_GameInfo.getInstance().roomlist.Count; k++)
							{
								if (k < Content.transform.childCount)
								{
									Content.transform.GetChild(k).gameObject.SetActive(value: true);
									Content.transform.GetChild(k).name = LHD_GameInfo.getInstance().roomlist[k].id.ToString();
									Content.transform.GetChild(k).Find("roomName/Text").GetComponent<Text>()
										.text = LHD_GameInfo.getInstance().roomlist[k].name;
										Content.transform.GetChild(k).Find("min/Text").GetComponent<Text>()
											.text = "最小携带: " + LHD_GameInfo.getInstance().roomlist[k].minGold;
											Content.transform.GetChild(k).Find("xianhong/Text").GetComponent<Text>()
												.text = "限红: " + LHD_GameInfo.getInstance().roomlist[k].maxBet;
												Content.transform.GetChild(k).Find("State").GetComponent<Text>()
													.text = string.Empty;
													Content.transform.GetChild(k).Find("Count").GetComponent<Text>()
														.text = LHD_GameInfo.getInstance().roomlist[k].onlineNumber.ToString();
													}
													else
													{
														GameObject obj = UnityEngine.Object.Instantiate(ItemPrefab, Content.transform);
														obj.SetActive(value: true);
														obj.name = LHD_GameInfo.getInstance().roomlist[k].id.ToString();
														obj.transform.Find("roomName/Text").GetComponent<Text>().text = LHD_GameInfo.getInstance().roomlist[k].name;
														obj.transform.Find("min/Text").GetComponent<Text>().text = "最小携带: " + LHD_GameInfo.getInstance().roomlist[k].minGold;
														obj.transform.Find("xianhong/Text").GetComponent<Text>().text = "限红: " + LHD_GameInfo.getInstance().roomlist[k].maxBet;
														obj.transform.Find("State").GetComponent<Text>().text = string.Empty;
														obj.transform.Find("Count").GetComponent<Text>().text = LHD_GameInfo.getInstance().roomlist[k].onlineNumber.ToString();
														obj.transform.Find("JoinBtn").GetComponent<Button>().onClick.AddListener(delegate
														{
															UnityEngine.Debug.Log("进入" + obj.name);
															for (int l = 0; l < LHD_GameInfo.getInstance().roomlist.Count; l++)
															{
																if (obj.name == LHD_GameInfo.getInstance().roomlist[l].id.ToString())
																{
																	LHD_GameInfo.getInstance().roominfo = LHD_GameInfo.getInstance().roomlist[l];
																	break;
																}
															}
															for (int m = 0; m < tempTime.Count; m++)
															{
																if (obj.name == tempTime[m].id.ToString())
																{
																	LHD_GameInfo.getInstance().roominfo.havTime = tempTime[m].time;
																	UnityEngine.Debug.LogError("设置当前剩余时间: " + LHD_GameInfo.getInstance().roominfo.havTime);
																	break;
																}
															}
															LHD_NetMngr.GetSingleton().MyCreateSocket.SendSelectSeat(LHD_GameInfo.getInstance().roominfo.id, 0);
														});
													}
												}
											}

											private void ResultListCall(JsonData jd)
											{
												if (waitSetResultList != null)
												{
													StopCoroutine(waitSetResultList);
												}
												waitSetResultList = StartCoroutine(WaitSetResultList(jd));
											}

											private IEnumerator WaitSetResultList(JsonData jd)
											{
												for (int j = 0; j < jd.Count; j++)
												{
													for (int i = 0; i < Content.transform.childCount; i++)
													{
														if (Content.transform.GetChild(i).name == jd[j]["deskId"].ToString())
														{
															SetLuDan(Content.transform.GetChild(i).Find("LuDan/Content"), jd[j]["recordList"]);
															yield return new WaitForSeconds(0.35f);
															break;
														}
													}
												}
											}

											private void SetLuDan(Transform luDanTr, JsonData jd)
											{
												for (int i = 0; i < luDanTr.childCount; i++)
												{
													luDanTr.GetChild(i).gameObject.SetActive(value: false);
												}
												posx_DLu = 0;
												posy_DLu = 0;
												List<int> list = new List<int>();
												for (int j = 0; j < jd.Count; j++)
												{
													list.Add((int)jd[j]);
												}
												reslut_DLu = new List<int>();
												for (int k = 0; k < list.Count; k++)
												{
													AddLuDan_DLu(list[k], luDanTr);
												}
											}

											private void AddLuDan_DLu(int num, Transform luDanTr)
											{
												reslut_DLu.Add(num);
												int count = reslut_DLu.Count;
												if (count >= 2 && reslut_DLu[count - 1] == 1)
												{
													switch (reslut_DLu[count - 2])
													{
													case 0:
														reslut_DLu[count - 2] = -1;
														break;
													case 2:
														reslut_DLu[count - 2] = -2;
														break;
													}
												}
												if (count >= 2 && reslut_DLu[count - 1] == 1 && reslut_DLu[count - 2] == 1)
												{
													reslut_DLu.Remove(num);
													return;
												}
												count = reslut_DLu.Count;
												if (count < 2)
												{
													posy_DLu = 0;
													posx_DLu = 0;
												}
												else if ((count >= 2 && reslut_DLu[count - 2] >= 0 && (reslut_DLu[count - 1] == reslut_DLu[count - 2] || (count >= 3 && IsHe(reslut_DLu[count - 2]) && reslut_DLu[count - 3] == reslut_DLu[count - 1]) || (count >= 3 && reslut_DLu[count - 1] == 2 && reslut_DLu[count - 3] == -2) || (count >= 3 && reslut_DLu[count - 1] == 0 && reslut_DLu[count - 3] == -1))) || (count >= 3 && reslut_DLu[count - 1] == 1 && reslut_DLu[count - 3] == 2 && reslut_DLu[count - 2] == -2) || (count >= 3 && reslut_DLu[count - 1] == 1 && reslut_DLu[count - 3] == 1 && reslut_DLu[count - 2] == -1))
												{
													posy_DLu++;
													if (posy_DLu >= 6)
													{
														posy_DLu = 0;
														posx_DLu++;
													}
												}
												else if ((count >= 2 && reslut_DLu[count - 2] < 0) || (count >= 3 && reslut_DLu[count - 2] < 0 && (reslut_DLu[count - 3] == reslut_DLu[count - 1] || (reslut_DLu[count - 3] == -1 && reslut_DLu[count - 1] == 0) || (reslut_DLu[count - 3] == -2 && reslut_DLu[count - 1] == 2) || (reslut_DLu[count - 3] == 1 && reslut_DLu[count - 1] == 1))))
												{
													UnityEngine.Debug.LogError("跳过: " + reslut_DLu[count - 1]);
												}
												else if (count == 2 && IsHe(reslut_DLu[count - 2]))
												{
													posy_DLu++;
													if (posy_DLu >= 6)
													{
														posy_DLu = 0;
														posx_DLu++;
													}
												}
												else
												{
													posy_DLu = 0;
													posx_DLu++;
													if (posx_DLu >= 30)
													{
														List<int> list = new List<int>();
														int num2 = -100;
														if (reslut_DLu.Count >= 1)
														{
															num2 = reslut_DLu[0];
														}
														UnityEngine.Debug.LogError("当前第一位: " + num2);
														UnityEngine.Debug.LogError("前: " + JsonMapper.ToJson(reslut_DLu));
														for (int i = 0; i < reslut_DLu.Count; i++)
														{
															if (num2 == -100)
															{
																break;
															}
															if (num2 == reslut_DLu[i] || IsHe(reslut_DLu[i]) || (num2 == -2 && reslut_DLu[i] != 0) || (num2 == -1 && reslut_DLu[i] != 2))
															{
																UnityEngine.Debug.LogError("删除: " + reslut_DLu[i]);
																reslut_DLu[i] = -1000000;
																continue;
															}
															UnityEngine.Debug.LogError("==当前到: " + reslut_DLu[i] + "  跳出==");
															break;
														}
														for (int j = 0; j < reslut_DLu.Count; j++)
														{
															if (reslut_DLu[j] != -1000000)
															{
																list.Add(reslut_DLu[j]);
															}
														}
														reslut_DLu = new List<int>();
														UnityEngine.Debug.LogError("完成后: " + JsonMapper.ToJson(list));
														posy_DLu = 0;
														posx_DLu = 0;
														for (int k = 0; k < luDanTr.childCount; k++)
														{
															luDanTr.GetChild(k).gameObject.SetActive(value: false);
														}
														for (int l = 0; l < list.Count; l++)
														{
															AddLuDan_DLu(list[l], luDanTr);
														}
														return;
													}
												}
												int num3 = posx_DLu + posy_DLu * 30;
												if (num3 < luDanTr.childCount)
												{
													int latsIndex = 0;
													num = reslut_DLu[count - 1];
													try
													{
														latsIndex = ((count <= 1 || reslut_DLu.Count < 1) ? num : reslut_DLu[count - 2]);
													}
													catch (Exception arg)
													{
														UnityEngine.Debug.LogError("e: " + arg);
													}
													luDanTr.GetChild(num3).GetComponent<Image>().sprite = GetSprite_DLu(latsIndex, num);
													luDanTr.GetChild(num3).gameObject.SetActive(value: true);
												}
												else
												{
													UnityEngine.Debug.LogError("====未处理: " + num3);
												}
											}

											private bool IsHe(int num)
											{
												if (num == 1 || num < 0)
												{
													return true;
												}
												return false;
											}

											private Sprite GetSprite_DLu(int latsIndex, int index)
											{
												int num = 0;
												switch (index)
												{
												case 0:
													num = 0;
													break;
												case 2:
													num = 1;
													break;
												default:
													num = ((latsIndex != 0 && latsIndex != -1) ? 3 : 2);
													break;
												}
												return luDanSprs[num];
											}

											private void UpdateUserInfo()
											{
												NickName.text = ZH2_GVars.GetBreviaryName(LHD_GameInfo.getInstance().userinfo.nickname);
												int gameScore = LHD_GameInfo.getInstance().GameScore;
												int coinCount = LHD_GameInfo.getInstance().CoinCount;
												Coin.text = ((gameScore <= 0) ? coinCount.ToString() : gameScore.ToString());
											}

											public void DropOutButton()
											{
												UnityEngine.Debug.LogError("=======4");
												LHD_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
												ZH2_GVars.isStartedFromGame = true;
												GameObject gameObject = GameObject.Find("netMngr");
												GameObject gameObject2 = GameObject.Find("GameMngr");
												if (gameObject != null)
												{
													UnityEngine.Object.Destroy(gameObject);
												}
												else
												{
													UnityEngine.Debug.LogError("====netMngr===为空");
												}
												if (gameObject2 != null)
												{
													UnityEngine.Object.Destroy(gameObject2);
												}
												else
												{
													UnityEngine.Debug.LogError("====gameMngr===为空");
												}
												AssetBundleManager.GetInstance().UnloadAB("NLHD");
												SceneManager.LoadScene("MainScene");
											}
										}
