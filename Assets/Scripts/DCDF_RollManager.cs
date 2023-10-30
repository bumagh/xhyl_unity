using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DCDF_RollManager : MonoBehaviour
{
	public DCDF_Roll[] sptRolls = new DCDF_Roll[5];

	private bool bCanStart;

	private bool bSpeedUp;

	private bool bOnHook;

	private Button btnSpeedUp;

	private Image imgSpeedUpEff;

	private Button btnSpeedUpCancel;

	private Image imgSpeedUpCancelEff;

	private Button btnOnHook;

	private Image imgOnHookEff;

	private Button btnOnHookCancel;

	private Image imgOnHookCancelEff;

	private Button btnStart;

	private Image imgStartEff;

	[SerializeField]
	private Sprite[] spiStarts;

	private Text txtPower;

	[SerializeField]
	private DCDF_CaishenAnimCtrl caishenAnimCtrl;

	private float userGold;

	private float curBet;

	private float curWin;

	private int freeTimes;

	private Text txtUserGold;

	private Text txtCurBet;

	private Button btnSubBet;

	private Button btnMulBet;

	private Text txtCurWin;

	private Color colDef = new Color(0.8f, 0.7843f, 0.8588f, 1f);

	private Color colGold = new Color(0.9333f, 0.8f, 0.3451f, 1f);

	private Button btnBack;

	private int[,] result = new int[5, 3];

	private List<int[]> allResultLists = new List<int[]>();

	private List<DCDF_ImageAnim[]> allElementLists = new List<DCDF_ImageAnim[]>();

	private List<List<DCDF_ImageAnim>> listWins = new List<List<DCDF_ImageAnim>>();

	private List<DCDF_ImageAnim> winElements = new List<DCDF_ImageAnim>();

	private List<int> listWinElements = new List<int>();

	private bool bPlayingWinAnim;

	private DCDF_SquareEffCtrl[] allSquareEffs = new DCDF_SquareEffCtrl[15];

	private List<List<DCDF_SquareEffCtrl>> listWinEffs = new List<List<DCDF_SquareEffCtrl>>();

	private List<DCDF_SquareEffCtrl> winEffs = new List<DCDF_SquareEffCtrl>();

	[SerializeField]
	private DCDF_CaishendaoCtrl caishendaoCtrl;

	private float[] prizePoolAmounts = new float[4]
	{
		10f,
		30f,
		100f,
		4000f
	};

	private Text[] txtPrizePool = new Text[4];

	private GameObject objBg;

	private GameObject objRotateLightEff;

	private GameObject objFreeGamePanel;

	private bool bTriggerFreeGame;

	private Button[] BtnQius = new Button[3];

	private int[] freeCounts = new int[3]
	{
		15,
		10,
		5
	};

	private GameObject objSurplus;

	private Text txtSurplus;

	private GameObject objCountDown;

	private Text txtCountDown;

	private GameObject objTreasureBox;

	private bool bTriggerTreasureBox;

	private DCDF_ImageAnim[] boxAnims = new DCDF_ImageAnim[12];

	private Button[] btnOpens = new Button[12];

	private GameObject[] objOpenBoxEffs = new GameObject[12];

	private bool[] bOpend = new bool[12];

	private Transform tfInside;

	private List<DCDF_ImageAnim> listInsideAnim6;

	private List<DCDF_ImageAnim> listInsideAnim7;

	private List<DCDF_ImageAnim> listInsideAnim8;

	private List<DCDF_ImageAnim> listInsideAnim9;

	private List<DCDF_ImageAnim> listOpenAnim6;

	private List<DCDF_ImageAnim> listOpenAnim7;

	private List<DCDF_ImageAnim> listOpenAnim8;

	private List<DCDF_ImageAnim> listOpenAnim9;

	private GameObject objZhuanfanle;

	private Text txtZhuanfanle;

	private GameObject objCaishendao;

	private Text txtCaishendao;

	private Image imgPrizeType;

	[SerializeField]
	private Sprite[] spiPrizes = new Sprite[4];

	private int prizeType;

	private int[][] testInt;

	private void Start()
	{
		bCanStart = true;
		bSpeedUp = false;
		InitComponent();
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().RegisterHandler("gameResult", HandleNetMsg_GameStart);
		testInt = new int[5][]
		{
			new int[3]
			{
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10)
			},
			new int[3]
			{
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10)
			},
			new int[3]
			{
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10)
			},
			new int[3]
			{
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10)
			},
			new int[3]
			{
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10)
			}
		};
	}

	private void InitComponent()
	{
		btnSpeedUp = base.transform.Find("BtnSpeedUp").GetComponent<Button>();
		imgSpeedUpEff = btnSpeedUp.transform.Find("Image").GetComponent<Image>();
		btnSpeedUpCancel = base.transform.Find("BtnSpeedUpCancel").GetComponent<Button>();
		imgSpeedUpCancelEff = btnSpeedUpCancel.transform.Find("Image").GetComponent<Image>();
		btnOnHook = base.transform.Find("BtnOnHook").GetComponent<Button>();
		imgOnHookEff = btnOnHook.transform.Find("Image").GetComponent<Image>();
		btnOnHookCancel = base.transform.Find("BtnOnHookCancel").GetComponent<Button>();
		imgOnHookCancelEff = btnOnHookCancel.transform.Find("Image").GetComponent<Image>();
		btnStart = base.transform.Find("BtnStart").GetComponent<Button>();
		imgStartEff = btnStart.transform.Find("Image").GetComponent<Image>();
		txtPower = base.transform.Find("Cell/TxtPower").GetComponent<Text>();
		btnBack = base.transform.Find("BtnBack").GetComponent<Button>();
		btnBack.onClick.AddListener(ClickBtnBack);
		btnSpeedUp.onClick.AddListener(ClickBtnSpeedUp);
		btnSpeedUpCancel.onClick.AddListener(ClickBtnSpeedUpCancel);
		btnOnHook.onClick.AddListener(ClickBtnOnHook);
		btnOnHookCancel.onClick.AddListener(ClickBtnOnHookCancel);
		btnStart.onClick.AddListener(ClickBtnStart);
		txtUserGold = base.transform.Find("TxtCoin").GetComponent<Text>();
		userGold = 1200f;
		txtUserGold.text = userGold.ToString();
		txtCurBet = base.transform.Find("TxtBet").GetComponent<Text>();
		curBet = 50f;
		txtCurBet.text = curBet.ToString();
		txtCurWin = base.transform.Find("TxtWin").GetComponent<Text>();
		curWin = 0f;
		txtCurWin.text = "0.00";
		txtCurWin.color = colDef;
		btnSubBet = base.transform.Find("BtnSub").GetComponent<Button>();
		btnSubBet.onClick.AddListener(ClickBtnSubBet);
		btnMulBet = base.transform.Find("BtnMul").GetComponent<Button>();
		btnMulBet.onClick.AddListener(ClickBtnMulBet);
		freeTimes = 0;
		Transform transform = base.transform.Find("Cell/Mask");
		for (int i = 0; i < 15; i++)
		{
			allSquareEffs[i] = transform.Find($"Square{i}").GetComponent<DCDF_SquareEffCtrl>();
		}
		for (int j = 0; j < 4; j++)
		{
			Transform transform2 = base.transform.Find((j + 1).ToString());
			txtPrizePool[j] = transform2.Find("Text").GetComponent<Text>();
			prizePoolAmounts[j] = prizePoolAmounts[j];
			txtPrizePool[j].text = prizePoolAmounts[j].ToString();
		}
		Transform transform3 = GameObject.Find("CanvasFront").transform;
		objBg = transform3.Find("Bg").gameObject;
		objFreeGamePanel = transform3.Find("FreeGame").gameObject;
		bTriggerFreeGame = false;
		objSurplus = base.transform.Find("Surplus").gameObject;
		txtSurplus = objSurplus.transform.Find("TxtNum").GetComponent<Text>();
		objSurplus.SetActive(value: false);
		objCountDown = transform3.Find("DownTimeBg").gameObject;
		objCountDown.SetActive(value: false);
		txtCountDown = objCountDown.transform.Find("TxtTime").GetComponent<Text>();
		objTreasureBox = transform3.Find("TreasureBox").gameObject;
		objTreasureBox.SetActive(value: false);
		bTriggerTreasureBox = false;
		objZhuanfanle = transform3.Find("Zhuanfanle").gameObject;
		objZhuanfanle.SetActive(value: false);
		txtZhuanfanle = objZhuanfanle.transform.Find("Text").GetComponent<Text>();
		txtZhuanfanle.text = "0.00";
		objCaishendao = transform3.Find("Caishendao").gameObject;
		objCaishendao.SetActive(value: false);
		txtCaishendao = objCaishendao.transform.Find("Text").GetComponent<Text>();
		imgPrizeType = objCaishendao.transform.Find("ImgPrizeType").GetComponent<Image>();
		tfInside = objTreasureBox.transform.Find("Inside");
		listInsideAnim6 = new List<DCDF_ImageAnim>();
		listInsideAnim7 = new List<DCDF_ImageAnim>();
		listInsideAnim8 = new List<DCDF_ImageAnim>();
		listInsideAnim9 = new List<DCDF_ImageAnim>();
		listOpenAnim6 = new List<DCDF_ImageAnim>();
		listOpenAnim7 = new List<DCDF_ImageAnim>();
		listOpenAnim8 = new List<DCDF_ImageAnim>();
		listOpenAnim9 = new List<DCDF_ImageAnim>();
		for (int k = 0; k < 12; k++)
		{
			bOpend[k] = false;
			DCDF_ImageAnim component = tfInside.GetChild(k).GetComponent<DCDF_ImageAnim>();
			component.gameObject.SetActive(value: false);
			if (k / 3 == 0)
			{
				listInsideAnim6.Add(component);
			}
			else if (k / 3 == 1)
			{
				listInsideAnim7.Add(component);
			}
			else if (k / 3 == 2)
			{
				listInsideAnim8.Add(component);
			}
			else
			{
				listInsideAnim9.Add(component);
			}
			Transform transform4 = objTreasureBox.transform.Find((k + 1).ToString());
			boxAnims[k] = transform4.Find("ImgBox").GetComponent<DCDF_ImageAnim>();
			btnOpens[k] = transform4.Find("BtnOpen").GetComponent<Button>();
			int index = k;
			btnOpens[index].onClick.AddListener(delegate
			{
				ClickBtnOpen(index);
			});
		}
		for (int l = 0; l < 3; l++)
		{
			BtnQius[l] = objFreeGamePanel.transform.Find($"{l.ToString()}/Btn").GetComponent<Button>();
			int index2 = l;
			BtnQius[index2].onClick.AddListener(delegate
			{
				ClickBtnQiu(index2);
			});
		}
	}

	private void ClickBtnStart()
	{
		if (bCanStart)
		{
			DCDF_SoundManager.Instance.PlayClickAudio();
			imgStartEff.color = Color.white;
			imgStartEff.DOFade(0f, 1f);
			Send_GameStart();
		}
	}

	private void StartRoll()
	{
		bCanStart = false;
		btnStart.interactable = false;
		btnStart.image.sprite = spiStarts[1];
		if (freeTimes >= 1)
		{
			objSurplus.SetActive(value: true);
			txtSurplus.text = freeTimes.ToString();
			freeTimes--;
		}
		else
		{
			if (!(userGold >= curBet))
			{
				return;
			}
			userGold -= curBet;
			txtUserGold.text = userGold.ToString();
			curWin = 0f;
			txtCurWin.text = "0.00";
		}
		StopWinAnim();
		if (bSpeedUp)
		{
			DCDF_SoundManager.Instance.PlayNormalRollAudio();
		}
		else
		{
			DCDF_SoundManager.Instance.PlaySpeedUpRollAudio();
		}
		for (int i = 0; i < 4; i++)
		{
			if (bSpeedUp)
			{
				StartCoroutine(sptRolls[i].SpeedUpRoll());
			}
			else
			{
				StartCoroutine(sptRolls[i].NormalRoll());
			}
		}
		StartCoroutine("ShowResult");
	}

	private void ClickBtnSpeedUp()
	{
		DCDF_SoundManager.Instance.PlayClickAudio();
		bSpeedUp = true;
		btnSpeedUp.gameObject.SetActive(value: false);
		btnSpeedUpCancel.gameObject.SetActive(value: true);
		imgSpeedUpCancelEff.color = Color.white;
		imgSpeedUpCancelEff.DOFade(0f, 1f);
	}

	private void ClickBtnSpeedUpCancel()
	{
		DCDF_SoundManager.Instance.PlayClickAudio();
		bSpeedUp = false;
		btnSpeedUpCancel.gameObject.SetActive(value: false);
		btnSpeedUp.gameObject.SetActive(value: true);
		imgSpeedUpEff.color = Color.white;
		imgSpeedUpEff.DOFade(0f, 1f);
	}

	private void ClickBtnOnHook()
	{
		DCDF_SoundManager.Instance.PlayClickAudio();
		bOnHook = true;
		btnStart.interactable = false;
		btnOnHook.gameObject.SetActive(value: false);
		btnOnHookCancel.gameObject.SetActive(value: true);
		imgOnHookCancelEff.color = Color.white;
		imgOnHookCancelEff.DOFade(0f, 1f);
		if (bCanStart)
		{
			Send_GameStart();
		}
	}

	private void ClickBtnOnHookCancel()
	{
		DCDF_SoundManager.Instance.PlayClickAudio();
		bOnHook = false;
		btnStart.interactable = true;
		btnOnHookCancel.gameObject.SetActive(value: false);
		btnOnHook.gameObject.SetActive(value: true);
		imgOnHookEff.color = Color.white;
		imgOnHookEff.DOFade(0f, 1f);
		if (bCanStart)
		{
			StopCoroutine("ShowResult");
			if (winElements.Count > 0)
			{
				bPlayingWinAnim = true;
				StartCoroutine("PlayWinListsAnim");
			}
		}
	}

	private void ClickBtnBack()
	{
		if (bCanStart)
		{
			DCDF_SoundManager.Instance.PlayClickAudio();
			DCDF_SoundManager.Instance.StopMajorAudio();
			DCDF_SoundManager.Instance.StopBGM();
			StopAllCoroutines();
			base.gameObject.SetActive(value: false);
			DCDF_MySqlConnection.curView = "LoadingView";
			DCDF_SoundManager.Instance.PlayClickAudio();
			DCDF_MB_Singleton<DCDF_GameManager>.GetInstance().QuitToHallGame();
		}
	}

	private void ClickBtnSubBet()
	{
		if (!(curBet <= DCDF_MySqlConnection.desk.minBet))
		{
		}
	}

	private void ClickBtnMulBet()
	{
	}

	public void InitGame()
	{
		for (int i = 0; i < 5; i++)
		{
			sptRolls[i].Init();
		}
	}

	private void Send_GameStart()
	{
		UnityEngine.Debug.LogError("点击开始");
		object[] args = new object[3]
		{
			0,
			curBet,
			freeTimes
		};
		DCDF_MB_Singleton<DCDF_NetManager>.GetInstance().Send("userService/gameStart", args);
	}

	private void HandleNetMsg_GameStart(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		testInt = new int[5][]
		{
			new int[3]
			{
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10)
			},
			new int[3]
			{
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10)
			},
			new int[3]
			{
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10)
			},
			new int[3]
			{
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10)
			},
			new int[3]
			{
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10),
				UnityEngine.Random.Range(0, 10)
			}
		};
		int[][] array = testInt;
		UnityEngine.Debug.LogError("开奖结果: " + JsonMapper.ToJson(array));
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				result[i, j] = array[i][j];
			}
		}
		for (int k = 0; k < 5; k++)
		{
			if (bSpeedUp)
			{
				sptRolls[k].SpeedUpResetElements(array[k], freeTimes >= 1);
			}
			else
			{
				sptRolls[k].NormalResetElements(array[k], freeTimes >= 1);
			}
		}
		StartRoll();
	}

	private void UpdateRollElementsResult(int[,] grid)
	{
		result = grid;
	}

	private void GetResultIndexs()
	{
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				result[i, j] = Convert.ToInt32(sptRolls[i].tfScroll.GetChild(j).name);
			}
		}
	}

	private bool TriggerTreasureBox()
	{
		for (int i = 0; i < 15; i++)
		{
			if (result[i / 3, i % 3] != 11)
			{
				return false;
			}
		}
		return true;
	}

	private void UpdatePrizePool()
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 15; j++)
			{
				int num = result[j / 3, j % 3];
				if (num == i + 6)
				{
					prizePoolAmounts[i] += 0.01f;
					txtPrizePool[i].text = prizePoolAmounts[i].ToString();
					break;
				}
			}
		}
	}

	private void GetLists()
	{
		allResultLists.Clear();
		allElementLists.Clear();
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					for (int l = 0; l < 3; l++)
					{
						for (int m = 0; m < 3; m++)
						{
							int[] item = new int[5]
							{
								result[0, i],
								result[1, j],
								result[2, k],
								result[3, l],
								result[4, m]
							};
							allResultLists.Add(item);
							DCDF_ImageAnim[] item2 = new DCDF_ImageAnim[5]
							{
								sptRolls[0].tfScroll.GetChild(i).GetComponentInChildren<DCDF_ImageAnim>(),
								sptRolls[1].tfScroll.GetChild(j).GetComponentInChildren<DCDF_ImageAnim>(),
								sptRolls[2].tfScroll.GetChild(k).GetComponentInChildren<DCDF_ImageAnim>(),
								sptRolls[3].tfScroll.GetChild(l).GetComponentInChildren<DCDF_ImageAnim>(),
								sptRolls[4].tfScroll.GetChild(m).GetComponentInChildren<DCDF_ImageAnim>()
							};
							allElementLists.Add(item2);
						}
					}
				}
			}
		}
	}

	private void GetWinLists()
	{
		listWins.Clear();
		winElements.Clear();
		listWinElements.Clear();
		for (int i = 0; i < allResultLists.Count; i++)
		{
			int num = 1;
			int num2 = 1;
			int num3 = allResultLists[i][0];
			while (num < 5)
			{
				if (Equal(num3, allResultLists[i][num]))
				{
					if (num3 == 0 && allResultLists[i][num] != 0)
					{
						num3 = allResultLists[i][num];
					}
					num2++;
					num++;
					if (num != 5)
					{
						continue;
					}
					List<DCDF_ImageAnim> list = new List<DCDF_ImageAnim>();
					for (int j = 0; j < 5; j++)
					{
						list.Add(allElementLists[i][j]);
						if (!winElements.Contains(allElementLists[i][j]))
						{
							winElements.Add(allElementLists[i][j]);
						}
					}
					if (!Contains(listWins, list) && num3 != 0)
					{
						listWins.Add(list);
						listWinElements.Add(num3);
					}
					continue;
				}
				if (num2 < 3)
				{
					break;
				}
				List<DCDF_ImageAnim> list2 = new List<DCDF_ImageAnim>();
				for (int k = 0; k < num2; k++)
				{
					list2.Add(allElementLists[i][k]);
					if (!winElements.Contains(allElementLists[i][k]))
					{
						winElements.Add(allElementLists[i][k]);
					}
				}
				if (!Contains(listWins, list2) && num3 != 0)
				{
					listWins.Add(list2);
					listWinElements.Add(num3);
				}
				break;
			}
		}
		List<List<DCDF_ImageAnim>> list3 = new List<List<DCDF_ImageAnim>>();
		List<List<DCDF_ImageAnim>> list4 = new List<List<DCDF_ImageAnim>>();
		List<List<DCDF_ImageAnim>> list5 = new List<List<DCDF_ImageAnim>>();
		for (int l = 0; l < listWins.Count; l++)
		{
			if (listWins[l].Count == 3)
			{
				list3.Add(listWins[l]);
			}
			else if (listWins[l].Count == 4)
			{
				list4.Add(listWins[l]);
			}
			else if (listWins[l].Count == 5)
			{
				list5.Add(listWins[l]);
			}
		}
		for (int m = 0; m < list3.Count; m++)
		{
			for (int n = 0; n < list4.Count; n++)
			{
				if (ContainsAll(list4[n], list3[m]))
				{
					listWinElements.RemoveAt(listWins.IndexOf(list3[m]));
					listWins.Remove(list3[m]);
					list3.RemoveAt(m);
					m--;
					break;
				}
			}
		}
		for (int num4 = 0; num4 < list3.Count; num4++)
		{
			for (int num5 = 0; num5 < list5.Count; num5++)
			{
				if (ContainsAll(list5[num5], list3[num4]))
				{
					listWinElements.RemoveAt(listWins.IndexOf(list3[num4]));
					listWins.Remove(list3[num4]);
					list3.RemoveAt(num4);
					num4--;
					break;
				}
			}
		}
		for (int num6 = 0; num6 < list4.Count; num6++)
		{
			for (int num7 = 0; num7 < list5.Count; num7++)
			{
				if (ContainsAll(list5[num7], list4[num6]))
				{
					listWinElements.RemoveAt(listWins.IndexOf(list4[num6]));
					listWins.Remove(list4[num6]);
					list4.RemoveAt(num6);
					num6--;
					break;
				}
			}
		}
	}

	private void GetWinEffLists()
	{
		listWinEffs.Clear();
		winEffs.Clear();
		for (int i = 0; i < listWins.Count; i++)
		{
			List<DCDF_SquareEffCtrl> list = new List<DCDF_SquareEffCtrl>();
			for (int j = 0; j < listWins[i].Count; j++)
			{
				list.Add(GetWinEff(listWins[i][j]));
			}
			listWinEffs.Add(list);
		}
		for (int k = 0; k < winElements.Count; k++)
		{
			winEffs.Add(GetWinEff(winElements[k]));
		}
	}

	private DCDF_SquareEffCtrl GetWinEff(DCDF_ImageAnim imageAnim)
	{
		DCDF_SquareEffCtrl dCDF_SquareEffCtrl = null;
		int indexRoll = imageAnim.transform.GetComponentInParent<DCDF_Roll>().indexRoll;
		int num = 0;
		num = ((!imageAnim.transform.parent.name.Equals("ScrollRoll")) ? imageAnim.transform.parent.GetSiblingIndex() : imageAnim.transform.GetSiblingIndex());
		return allSquareEffs[indexRoll * 3 + num];
	}

	private bool Equal(int a, int b)
	{
		if (a == b || a == 0 || b == 0)
		{
			return true;
		}
		return false;
	}

	private bool ContainsAll(List<DCDF_ImageAnim> list1, List<DCDF_ImageAnim> list2)
	{
		for (int i = 0; i < list2.Count; i++)
		{
			if (!list1.Contains(list2[i]))
			{
				return false;
			}
		}
		return true;
	}

	private bool Contains(List<List<DCDF_ImageAnim>> list1, List<DCDF_ImageAnim> list2)
	{
		for (int i = 0; i < list1.Count; i++)
		{
			for (int j = 0; j < list2.Count && list1[i].Contains(list2[j]); j++)
			{
				if (list1[i].Contains(list2[j]) && j == list2.Count - 1)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void CalculateCurWin()
	{
		for (int i = 0; i < listWins.Count; i++)
		{
			int count = listWins[i].Count;
			float power = GetPower(listWinElements[i], count);
			curWin += power * curBet;
		}
	}

	private void CheckIfTriggerFreeGame()
	{
		if (listWinElements.Contains(10))
		{
			bTriggerFreeGame = true;
		}
		else
		{
			bTriggerFreeGame = false;
		}
	}

	private void PlayWinElementsAnim()
	{
		txtPower.gameObject.SetActive(value: false);
		for (int i = 0; i < winElements.Count; i++)
		{
			winElements[i].Play();
			winEffs[i].Play();
		}
	}

	private void StopWinAnim()
	{
		bPlayingWinAnim = false;
		txtPower.gameObject.SetActive(value: false);
		StopCoroutine("PlayWinListsAnim");
		StopCoroutine("ShowResult");
		for (int i = 0; i < winElements.Count; i++)
		{
			winElements[i].Reset();
		}
		for (int j = 0; j < 15; j++)
		{
			allSquareEffs[j].Stop();
		}
	}

	private void PlayWinElementsAnim(int index)
	{
		int count = listWins[index].Count;
		for (int i = 0; i < count; i++)
		{
			listWins[index][i].Play();
			listWinEffs[index][i].Play();
		}
		float power = GetPower(listWinElements[index], count);
		if (!(power <= 0f))
		{
			txtPower.transform.position = listWins[index][count - 1].transform.position + Vector3.up * 1f + Vector3.right * 0.6f;
			txtPower.text = (power * curBet).ToString("#0.00");
			txtPower.gameObject.SetActive(value: true);
		}
	}

	private IEnumerator PlayWinListsAnim()
	{
		int index2 = 0;
		int count = listWins.Count + 1;
		while (bPlayingWinAnim)
		{
			if (count > 2)
			{
				if (index2 == 0)
				{
					PlayWinElementsAnim();
				}
				else
				{
					PlayWinElementsAnim(index2 - 1);
				}
				yield return new WaitForSeconds(1.05f);
				for (int i = 0; i < winElements.Count; i++)
				{
					winElements[i].Reset();
					winEffs[i].Stop();
				}
				index2++;
				index2 %= count;
			}
			else
			{
				PlayWinElementsAnim(0);
				yield return new WaitForSeconds(1.05f);
			}
		}
	}

	private IEnumerator ShowResult()
	{
		if (bSpeedUp)
		{
			yield return sptRolls[4].SpeedUpRoll();
			DCDF_SoundManager.Instance.PlayRollEndAudio();
		}
		else
		{
			yield return sptRolls[4].NormalRoll();
		}
		GetResultIndexs();
		bTriggerTreasureBox = TriggerTreasureBox();
		if (bTriggerTreasureBox)
		{
			for (int i = 0; i < 15; i++)
			{
				allSquareEffs[i].Play();
			}
			yield return new WaitForSeconds(0.5f);
			objBg.SetActive(value: true);
			caishendaoCtrl.Move();
			yield return new WaitForSeconds(4.5f);
			ShowTreasureBox();
			yield break;
		}
		UpdatePrizePool();
		GetLists();
		GetWinLists();
		GetWinEffLists();
		CalculateCurWin();
		CheckIfTriggerFreeGame();
		yield return new WaitForSeconds(0.5f);
		if (winElements.Count > 0)
		{
			DCDF_SoundManager.Instance.PlayWinAudio();
			caishenAnimCtrl.PlayWin();
			txtCurWin.color = colGold;
			txtCurWin.text = curWin.ToString();
			txtCurWin.DOColor(colDef, 0.5f);
			if (bTriggerFreeGame)
			{
				PlayWinElementsAnim();
				yield return new WaitForSeconds(1f);
				ShowFreeGamePanel();
				yield break;
			}
			if (curWin >= 10f && freeTimes <= 0)
			{
				userGold += curWin;
				txtUserGold.text = userGold.ToString();
				bPlayingWinAnim = true;
				StartCoroutine("PlayWinListsAnim");
				yield return new WaitForSeconds(1f);
				ShowZhuanfanle();
				yield break;
			}
			if (!bOnHook && freeTimes <= 0)
			{
				objSurplus.SetActive(value: false);
				bPlayingWinAnim = true;
				StartCoroutine("PlayWinListsAnim");
				bCanStart = true;
				btnStart.interactable = true;
				btnStart.image.sprite = spiStarts[0];
				yield break;
			}
			PlayWinElementsAnim();
			yield return new WaitForSeconds(1f);
			if (freeTimes > 0 || userGold >= curBet)
			{
				Send_GameStart();
				yield break;
			}
			bCanStart = true;
			btnStart.interactable = true;
			btnStart.image.sprite = spiStarts[0];
		}
		else
		{
			if (freeTimes <= 0)
			{
				objSurplus.SetActive(value: false);
				curWin = 0f;
				txtCurWin.text = "0.00";
			}
			DCDF_SoundManager.Instance.PlayLoseAudio();
			caishenAnimCtrl.PlayFail();
			if ((bOnHook && userGold >= curBet) || freeTimes > 0)
			{
				yield return new WaitForSeconds(0.5f);
				Send_GameStart();
			}
			else
			{
				bCanStart = true;
				btnStart.interactable = true;
				btnStart.image.sprite = spiStarts[0];
			}
		}
	}

	private float GetPower(int type, int count)
	{
		switch (type)
		{
		case 1:
		case 2:
			switch (count)
			{
			case 3:
				return 0.2f;
			case 4:
				return 0.6f;
			case 5:
				return 3f;
			default:
				return 0f;
			}
		case 3:
			switch (count)
			{
			case 3:
				return 0.2f;
			case 4:
				return 0.8f;
			case 5:
				return 4f;
			default:
				return 0f;
			}
		case 4:
		case 5:
			switch (count)
			{
			case 3:
				return 0.2f;
			case 4:
				return 1.2f;
			case 5:
				return 4f;
			default:
				return 0f;
			}
		case 6:
			switch (count)
			{
			case 3:
				return 0.6f;
			case 4:
				return 2f;
			case 5:
				return 8f;
			default:
				return 0f;
			}
		case 7:
		case 8:
			switch (count)
			{
			case 3:
				return 0.6f;
			case 4:
				return 4f;
			case 5:
				return 10f;
			default:
				return 0f;
			}
		case 9:
			switch (count)
			{
			case 3:
				return 1f;
			case 4:
				return 6f;
			case 5:
				return 12f;
			default:
				return 0f;
			}
		default:
			return 0f;
		}
	}

	private void ShowFreeGamePanel()
	{
		objBg.SetActive(value: true);
		objFreeGamePanel.SetActive(value: true);
		StartCoroutine("CountDown");
	}

	private void ClickBtnQiu(int index)
	{
		DCDF_SoundManager.Instance.PlayClickAudio();
		freeTimes = freeCounts[index];
		objFreeGamePanel.SetActive(value: false);
		StopCoroutine("CountDown");
		objCountDown.SetActive(value: false);
		objBg.SetActive(value: false);
		StartCoroutine("FreeRoll");
	}

	private IEnumerator FreeRoll()
	{
		yield return new WaitForSeconds(1f);
		Send_GameStart();
	}

	private void ShowTreasureBox()
	{
		objTreasureBox.SetActive(value: false);
		objTreasureBox.transform.localScale = Vector3.one;
		objCountDown.transform.SetParent(objTreasureBox.transform);
		objTreasureBox.transform.localScale = Vector3.zero;
		objTreasureBox.transform.DOScale(1f, 0.5f).OnComplete(delegate
		{
			StartCoroutine("CountDown");
		});
	}

	private void ClickBtnOpen(int index)
	{
		if ((prizeType < 6 || prizeType > 9) && !bOpend[index])
		{
			DCDF_ImageAnim dCDF_ImageAnim = null;
			int num = UnityEngine.Random.Range(6, 10);
			if (num == 6)
			{
				dCDF_ImageAnim = listInsideAnim6[0];
				listInsideAnim6.RemoveAt(0);
				listOpenAnim6.Add(dCDF_ImageAnim);
			}
			if (num == 7)
			{
				dCDF_ImageAnim = listInsideAnim7[0];
				listInsideAnim7.RemoveAt(0);
				listOpenAnim7.Add(dCDF_ImageAnim);
			}
			if (num == 8)
			{
				dCDF_ImageAnim = listInsideAnim8[0];
				listInsideAnim8.RemoveAt(0);
				listOpenAnim8.Add(dCDF_ImageAnim);
			}
			if (num == 9)
			{
				dCDF_ImageAnim = listInsideAnim9[0];
				listInsideAnim9.RemoveAt(0);
				listOpenAnim9.Add(dCDF_ImageAnim);
			}
			dCDF_ImageAnim.transform.SetParent(btnOpens[index].transform.parent);
			dCDF_ImageAnim.transform.localPosition = Vector3.right * -5f + Vector3.up * -10f;
			GetPrizeType();
			StartCoroutine(OpenBox(index, dCDF_ImageAnim));
		}
	}

	private IEnumerator OpenBox(int index, DCDF_ImageAnim elementAnim)
	{
		DCDF_SoundManager.Instance.PlayOpenBoxAudio();
		boxAnims[index].Play();
		yield return new WaitForSeconds(0.3f);
		elementAnim.gameObject.SetActive(value: true);
		if (prizeType == 0)
		{
			yield break;
		}
		if (prizeType == 6)
		{
			for (int i = 0; i < listOpenAnim6.Count; i++)
			{
				listOpenAnim6[i].Play();
			}
		}
		if (prizeType == 7)
		{
			for (int j = 0; j < listOpenAnim7.Count; j++)
			{
				listOpenAnim7[j].Play();
			}
		}
		if (prizeType == 8)
		{
			for (int k = 0; k < listOpenAnim8.Count; k++)
			{
				listOpenAnim8[k].Play();
			}
		}
		if (prizeType == 9)
		{
			for (int l = 0; l < listOpenAnim9.Count; l++)
			{
				listOpenAnim9[l].Play();
			}
		}
		curWin = prizePoolAmounts[prizeType - 6];
		userGold += curWin;
		txtUserGold.text = userGold.ToString();
		txtCurWin.text = curWin.ToString();
		txtCurWin.color = colGold;
		txtCurWin.DOColor(colDef, 0.5f);
		yield return new WaitForSeconds(1f);
		HideTreasureBox();
		ShowCaishendao();
	}

	private void GetPrizeType()
	{
		if (listOpenAnim6.Count >= 3)
		{
			prizeType = 6;
		}
		if (listOpenAnim7.Count >= 3)
		{
			prizeType = 7;
		}
		if (listOpenAnim8.Count >= 3)
		{
			prizeType = 8;
		}
		if (listOpenAnim9.Count >= 3)
		{
			prizeType = 9;
		}
	}

	private void HideTreasureBox()
	{
		objCountDown.transform.SetParent(objTreasureBox.transform.parent);
		objCountDown.SetActive(value: false);
		objTreasureBox.SetActive(value: false);
		prizeType = 0;
		for (int i = 0; i < 12; i++)
		{
			if (bOpend[i])
			{
				bOpend[i] = false;
				boxAnims[i].Reset();
			}
		}
		while (listOpenAnim6.Count > 0)
		{
			DCDF_ImageAnim dCDF_ImageAnim = listOpenAnim6[0];
			dCDF_ImageAnim.transform.SetParent(tfInside);
			listOpenAnim6.RemoveAt(0);
			listInsideAnim6.Add(dCDF_ImageAnim);
			dCDF_ImageAnim.gameObject.SetActive(value: false);
		}
		while (listOpenAnim7.Count > 0)
		{
			DCDF_ImageAnim dCDF_ImageAnim = listOpenAnim7[0];
			dCDF_ImageAnim.transform.SetParent(tfInside);
			listOpenAnim7.RemoveAt(0);
			listInsideAnim7.Add(dCDF_ImageAnim);
			dCDF_ImageAnim.gameObject.SetActive(value: false);
		}
		while (listOpenAnim8.Count > 0)
		{
			DCDF_ImageAnim dCDF_ImageAnim = listOpenAnim8[0];
			dCDF_ImageAnim.transform.SetParent(tfInside);
			listOpenAnim8.RemoveAt(0);
			listInsideAnim8.Add(dCDF_ImageAnim);
			dCDF_ImageAnim.gameObject.SetActive(value: false);
		}
		while (listOpenAnim9.Count > 0)
		{
			DCDF_ImageAnim dCDF_ImageAnim = listOpenAnim9[0];
			dCDF_ImageAnim.transform.SetParent(tfInside);
			listOpenAnim9.RemoveAt(0);
			listInsideAnim9.Add(dCDF_ImageAnim);
			dCDF_ImageAnim.gameObject.SetActive(value: false);
		}
	}

	private IEnumerator CountDown()
	{
		objCountDown.SetActive(value: true);
		for (int countDown = 20; countDown >= 0; countDown--)
		{
			txtCountDown.text = countDown.ToString() + "s";
			yield return new WaitForSeconds(1f);
		}
		if (bTriggerFreeGame)
		{
			freeTimes = freeCounts[0];
			objFreeGamePanel.SetActive(value: false);
			objCountDown.SetActive(value: false);
			StartCoroutine("FreeRoll");
		}
	}

	private void ShowZhuanfanle()
	{
		objBg.SetActive(value: true);
		txtZhuanfanle.text = "0.00";
		objZhuanfanle.transform.localScale = Vector3.zero;
		objZhuanfanle.transform.DOScale(1f, 1f).OnComplete(delegate
		{
			StartCoroutine(WinScoreAnim(txtZhuanfanle));
		});
		objZhuanfanle.transform.DOScale(0f, 1f).SetDelay(3f).OnComplete(delegate
		{
			objBg.SetActive(value: false);
			if (bOnHook && userGold >= curBet)
			{
				Send_GameStart();
			}
			else
			{
				bCanStart = true;
				btnStart.interactable = true;
				btnStart.image.sprite = spiStarts[0];
			}
		});
	}

	private void ShowCaishendao()
	{
		imgPrizeType.sprite = spiPrizes[prizeType - 6];
		imgPrizeType.SetNativeSize();
		txtCaishendao.text = "0.00";
		objCaishendao.transform.localScale = Vector3.zero;
		objCaishendao.transform.DOScale(1f, 1f).OnComplete(delegate
		{
			StartCoroutine(WinScoreAnim(txtCaishendao));
		});
		objCaishendao.transform.DOScale(0f, 1f).SetDelay(3f).OnComplete(delegate
		{
			objBg.SetActive(value: false);
			if (bOnHook && userGold >= curBet)
			{
				Send_GameStart();
			}
			else
			{
				bCanStart = true;
				btnStart.interactable = true;
				btnStart.image.sprite = spiStarts[0];
			}
		});
	}

	private IEnumerator WinScoreAnim(Text txt)
	{
		float value = 0f;
		DOTween.To(() => value, delegate(float x)
		{
			value = x;
		}, curWin, 3f).OnUpdate(delegate
		{
			txt.text = value.ToString("#0.00");
		});
		DCDF_SoundManager.Instance.PlayGoldIncreaseAudio();
		yield return new WaitForSeconds(1.5f);
		DCDF_SoundManager.Instance.StopGoldIncreaseAudio();
		yield return new WaitForSeconds(0.5f);
	}
}
