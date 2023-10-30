using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BCBM_BetScene : MonoBehaviour
{
	public static BCBM_BetScene publicBetScene;

	public Text allScoceText;

	[HideInInspector]
	public double[] displaylaue = new double[3];

	public int lastBetNum;

	private bool isLater;

	public bool isStartGame;

	[Header("计数显示组件")]
	public Text CounterText;

	private int SwitchVakue = 1000;

	public List<Button> SwitchBtn_List = new List<Button>();

	public List<Text> SwitchText_List = new List<Text>();

	private bool isAuto;

	[HideInInspector]
	public List<Text> BetGerenText_List = new List<Text>();

	public List<Button> BetBtn_List = new List<Button>();

	public List<Transform> chipPos_List = new List<Transform>();

	[HideInInspector]
	public int[] BetGerenValue = new int[8];

	[HideInInspector]
	public int[] TempBetGerenValue = new int[8];

	[HideInInspector]
	public int[] BetZongValue = new int[8];

	[HideInInspector]
	public int[] BetValue = new int[8];

	[HideInInspector]
	public int[] BetChips = new int[5];

	[Header("遮挡界面")]
	public Button OcclusionScene;

	private List<Image> Buttonanimators = new List<Image>();

	private Transform btn;

	public Button AutoBtn;

	public Button xuYaBtn;

	public Text txtTime;

	public Text userNameText;

	public List<Sprite> chipSprs = new List<Sprite>();

	public Transform chipStartPos_Self;

	public Transform chipStartPos_Other;

	public Transform chipPanPos;

	public GameObject chipPre;

	public BCBM_TNvLangCor tNvLangCor;

	public Transform lu_left;

	public Transform lu_Right;

	public Transform timePanel;

	public Transform timePanelUpPos;

	public Transform timePanelDownPos;

	private GameObject timeObj;

	private GameObject speedObj;

	private Image timeAndSpeedValue;

	public BCBM_AutoPanel bCBM_AutoPanel;

	public Transform tips;

	public Transform tuoGuan;

	public Image tipsIma;

	public Text tipsText;

	public Text tipsText2;

	public Sprite[] tipsSprites;

	private int chipId;

	private int enterBetIndex;

	private int[] beiLv = new int[8]
	{
		5,
		5,
		5,
		5,
		10,
		15,
		25,
		40
	};

	private Coroutine getWinNumShow;

	public Transform showWinNumTrn;

	private int lastBetNumTemp = -1;

	private Coroutine displayMethonCor;

	private bool isMinTime;

	private Coroutine waitSetValu;

	private Coroutine waitSetValuFull;

	public Transform speedTr;

	public Transform speedTrOld;

	public Coroutine coroutineEndSpeed;

	private void Awake()
	{
		publicBetScene = this;
		isAuto = false;
		btn = base.transform.Find("Button");
		BetGerenText_List = new List<Text>();
		Buttonanimators = new List<Image>();
		for (int i = 0; i < BetBtn_List.Count; i++)
		{
			int index2 = 0;
			index2 = i;
			BetBtn_List[index2].onClick.AddListener(delegate
			{
				BetButton(index2);
			});
			Image component = BetBtn_List[i].transform.Find("WinBg").GetComponent<Image>();
			if (component != null)
			{
				Buttonanimators.Add(component);
			}
			else
			{
				UnityEngine.Debug.LogError(BetBtn_List[i].name + " 找不到DoTween");
			}
			BetGerenText_List.Add(BetBtn_List[i].transform.Find("个人押注").GetComponent<Text>());
		}
		OcclusionScene.onClick.AddListener(OcclusionSceneOnClick);
		for (int j = 0; j < SwitchBtn_List.Count; j++)
		{
			int index = j;
			SwitchBtn_List[index].onClick.AddListener(delegate
			{
				SwitchButton(index);
			});
		}
		SwitchText_List = new List<Text>();
		for (int k = 0; k < SwitchBtn_List.Count; k++)
		{
			SwitchText_List.Add(SwitchBtn_List[k].transform.Find("Text").GetComponent<Text>());
		}
		timeObj = timePanel.Find("Time").gameObject;
		speedObj = timePanel.Find("Speed").gameObject;
		timeAndSpeedValue = timePanel.Find("Value").GetComponent<Image>();
	}

	private void OnEnable()
	{
		UnityEngine.Debug.LogError("==============Time_Dji: " + BCBM_link.Time_Dji);
		ResSetPanel();
		lastBetNum = 0;
		if (BCBM_link.Time_Dji <= 25)
		{
			SetTimePanel(isUp: false);
			ResSet();
			speedObj.SetActive(value: false);
			timeObj.SetActive(value: false);
			OcclusionScene.gameObject.SetActive(value: true);
		}
		else
		{
			SetTimePanel(isUp: true);
			speedObj.SetActive(value: false);
			timeObj.SetActive(value: true);
			OcclusionScene.gameObject.SetActive(value: false);
		}
		if (displayMethonCor != null)
		{
			StopCoroutine(displayMethonCor);
		}
		displayMethonCor = StartCoroutine(displayMethon());
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendResultList(BCBM_GameInfo.getInstance().UserInfo.TableId);
		userNameText.text = ZH2_GVars.GetBreviaryName(ZH2_GVars.nickname);
		SetLuRes();
		isMinTime = false;
		isStartGame = false;
		StartCoroutine(WaitTipText());
		SetChip();
	}

	public void SetBetChip(int[] betChip)
	{
		UnityEngine.Debug.LogError("====================================");
		BetChips = betChip;
		UnityEngine.Debug.LogError("==========BetChips: " + JsonMapper.ToJson(BetChips));
		SwitchButton(0);
		for (int i = 0; i < SwitchText_List.Count; i++)
		{
			SwitchText_List[i].text = GetChipName(BetChips[i]);
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

	private void SetLuRes()
	{
		lu_left.Find("Idel").gameObject.SetActive(value: true);
		lu_left.Find("Move").gameObject.SetActive(value: false);
		lu_Right.Find("Idel").gameObject.SetActive(value: true);
		lu_Right.Find("Move").gameObject.SetActive(value: false);
	}

	private void SetChip()
	{
		for (int i = 0; i < chipPos_List.Count; i++)
		{
			for (int j = 0; j < chipPos_List[i].childCount; j++)
			{
				UnityEngine.Object.Destroy(chipPos_List[i].GetChild(j).gameObject);
			}
		}
	}

	private void SetLuStart()
	{
		lu_left.Find("Idel").gameObject.SetActive(value: false);
		lu_left.Find("Move").gameObject.SetActive(value: true);
		lu_Right.Find("Idel").gameObject.SetActive(value: false);
		lu_Right.Find("Move").gameObject.SetActive(value: true);
	}

	public void ShowTips(int num)
	{
		tips.gameObject.SetActive(value: true);
		tipsIma.sprite = tipsSprites[num];
	}

	public void HidTips()
	{
		tips.gameObject.SetActive(value: false);
	}

	private IEnumerator WaitTipText()
	{
		int num = 0;
		while (true)
		{
			tipsText.text += ".";
			tipsText2.text += ".";
			num++;
			if (num > 3)
			{
				num = 0;
				tipsText.text = string.Empty;
				tipsText2.text = string.Empty;
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	private void SetTimePanel(bool isUp)
	{
		timePanel.DOLocalMove((!isUp) ? timePanelDownPos.localPosition : timePanelUpPos.localPosition, 0.5f);
		if (!isUp)
		{
			speedTr.DORotate(new Vector3(0f, 0f, 12f), 0.1f);
		}
	}

	private void OcclusionSceneOnClick()
	{
		All_GameMiniTipPanel.publicMiniTip.ShowTip("请在倒计时下注");
		UnityEngine.Debug.LogError("=====不允许下注=====");
	}

	public void _onClickCoinIn()
	{
		int num = 0;
		int num2 = (BCBM_GameInfo.getInstance().UserInfo.RoomId == 0) ? BCBM_GameInfo.getInstance().UserInfo.ExpCoinCount : BCBM_GameInfo.getInstance().UserInfo.CoinCount;
		if (num2 > 0)
		{
			BCBM_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinIn(100);
			if (BCBM_LuckyLion_SoundManager.GetSingleton() != null)
			{
				BCBM_LuckyLion_SoundManager.GetSingleton().playButtonSound(BCBM_LuckyLion_SoundManager.EUIBtnSoundType.CoinIn);
			}
		}
	}

	public void Auto_Game(Transform @object)
	{
		if (AutoBtn == null)
		{
			AutoBtn = @object.GetComponent<Button>();
		}
		if (!isAuto)
		{
			bCBM_AutoPanel.gameObject.SetActive(value: true);
		}
		else
		{
			AutoGame();
		}
	}

	public void AutoGame()
	{
		isAuto = !isAuto;
		if (isAuto)
		{
			AutoBtn.transform.GetChild(0).GetComponent<Text>().text = "清除";
		}
		else
		{
			AutoBtn.transform.GetChild(0).GetComponent<Text>().text = "自动";
		}
		tuoGuan.gameObject.SetActive(isAuto);
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SenAutoBet(isAuto ? 1 : 0, BCBM_GameInfo.getInstance().UserInfo.TableId);
	}

	public void ContinueBet()
	{
		UnityEngine.Debug.LogError("UserId: " + BCBM_GameInfo.getInstance().UserInfo.UserId);
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SenContinueBet(BCBM_GameInfo.getInstance().UserInfo.UserId, BCBM_GameInfo.getInstance().UserInfo.TableId);
	}

	public void CancelBet()
	{
		if (isAuto)
		{
			BCBM_NetMngr.GetSingleton().MyCreateSocket.SenAutoBet(0, BCBM_GameInfo.getInstance().UserInfo.TableId);
			isAuto = false;
			AutoBtn.transform.GetChild(0).gameObject.SetActive(value: false);
		}
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SenCancelBet(BCBM_GameInfo.getInstance().UserInfo.TableId);
	}

	public void SwitchButton(int index)
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

	public void BetButton(int btnIndex)
	{
		enterBetIndex = btnIndex;
		Vector2 vector = new Vector2(0f, 0f);
		vector = GetChipPos(btnIndex);
		UserBet userBet = new UserBet();
		userBet.chipId = chipId;
		userBet.x = vector.x;
		userBet.y = vector.y;
		userBet.space = btnIndex;
		userBet.userId = BCBM_GameInfo.getInstance().UserInfo.UserId;
		UserBet obj = userBet;
		string userBet2 = JsonUtility.ToJson(obj);
		BCBM_link.publiclink.BetMethon(userBet2, SwitchVakue, btnIndex);
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

	public void GetChip(JsonData jd)
	{
		if (jd == null || jd.Count <= 0)
		{
			UnityEngine.Debug.LogError("====jd为空====");
			return;
		}
		int num = (int)jd["userId"];
		bool isSelf = num == BCBM_GameInfo.getInstance().UserInfo.UserId;
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
		gameObject = UnityEngine.Object.Instantiate(chipPre, chipPos_List[btnIndex]);
		gameObject.transform.position = ((!isSelf) ? chipStartPos_Other.position : chipStartPos_Self.position);
		gameObject.transform.GetComponent<Image>().sprite = chipSprs[chipId];
		gameObject.name = ((!isSelf) ? ("1_" + chipId) : ("0_" + chipId));
		gameObject.transform.DOLocalMove(pos, 0.5f);
		BCBM_Audio.publicAudio.PlaychipClip();
	}

	public GameObject GetChip(Transform pos, int chipId)
	{
		GameObject gameObject = null;
		gameObject = UnityEngine.Object.Instantiate(chipPre, pos);
		gameObject.transform.position = pos.position;
		gameObject.transform.GetComponent<Image>().sprite = chipSprs[chipId];
		return gameObject;
	}

	public void Restart()
	{
		tNvLangCor.StartBet();
		timeAndSpeedValue.fillAmount = 0.6f;
		timeObj.SetActive(value: true);
		speedObj.SetActive(value: false);
		timeObj.transform.DOScale(Vector3.one, 1f).OnComplete(delegate
		{
			SetTimePanel(isUp: true);
		});
		BCBM_AnimationScene.publicAnimationScene.HidCar();
	}

	public void ShowChipOver(int winCarIndex)
	{
		SetTimePanel(isUp: false);
		StartCoroutine(WaitShowChipOver(winCarIndex));
		StartCoroutine(WaitTime());
	}

	private IEnumerator WaitTime()
	{
		while (true)
		{
			if (ZH2_GVars.moveTime < 0.7f)
			{
				ZH2_GVars.moveTime += 0.05f;
				if (ZH2_GVars.moveTime >= 0.7f)
				{
					break;
				}
			}
			yield return new WaitForSeconds(0.15f);
		}
		ZH2_GVars.moveTime = 0.7f;
		yield return new WaitForSeconds(0.15f);
		SetLuRes();
	}

	private IEnumerator WaitShowChipOver(int winCarIndex)
	{
		beiLv = new int[8]
		{
			5,
			5,
			5,
			5,
			10,
			15,
			25,
			40
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
				selfLose[i].DOLocalMove(chipPanPos.localPosition, 0.5f).OnComplete(delegate
				{
					UnityEngine.Object.Destroy(selfLose[i].gameObject);
				});
			}
		}
		int j;
		for (j = 0; j < otherLose.Count; j++)
		{
			if (otherLose[j] != null && chipPanPos != null)
			{
				otherLose[j].SetParent(chipPanPos);
				otherLose[j].DOLocalMove(chipPanPos.localPosition, 0.5f).OnComplete(delegate
				{
					UnityEngine.Object.Destroy(otherLose[j].gameObject);
				});
			}
		}
		for (int n = 0; n < selfWin.Count; n++)
		{
			int num = int.Parse(selfWin[n].name.Replace("0_", string.Empty));
			for (int num2 = 0; num2 < beiLv[winCarIndex] - 1; num2++)
			{
				GameObject chip = GetChip(chipPanPos, num);
				selfWin_Get.Add(chip.transform);
			}
		}
		for (int num3 = 0; num3 < otherWin.Count; num3++)
		{
			int num4 = int.Parse(otherWin[num3].name.Replace("1_", string.Empty));
			for (int num5 = 0; num5 < beiLv[winCarIndex]; num5++)
			{
				GameObject chip2 = GetChip(chipPanPos, num4);
				otherWin_Get.Add(chip2.transform);
			}
		}
		yield return new WaitForSeconds(1f);
		for (int num6 = 0; num6 < selfLose.Count; num6++)
		{
			if (selfLose[num6] != null && selfLose[num6].gameObject != null)
			{
				UnityEngine.Object.Destroy(selfLose[num6].gameObject);
			}
		}
		for (int num7 = 0; num7 < otherLose.Count; num7++)
		{
			if (otherLose[num7] != null && otherLose[num7].gameObject != null)
			{
				UnityEngine.Object.Destroy(otherLose[num7].gameObject);
			}
		}
		for (int num8 = 0; num8 < selfWin_Get.Count; num8++)
		{
			selfWin_Get[num8].SetParent(chipPos_List[winCarIndex]);
			selfWin_Get[num8].DOLocalMove(GetChipPos(winCarIndex), 0.5f);
		}
		for (int num9 = 0; num9 < otherWin_Get.Count; num9++)
		{
			otherWin_Get[num9].SetParent(chipPos_List[winCarIndex]);
			otherWin_Get[num9].DOLocalMove(GetChipPos(winCarIndex), 0.5f);
		}
		yield return new WaitForSeconds(1f);
		if (getWinNumShow != null)
		{
			StopCoroutine(getWinNumShow);
		}
		int addNum = (int)displaylaue[0];
		if (addNum > 0)
		{
			getWinNumShow = StartCoroutine(GetWinNumShow(addNum));
		}
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
		if (selfWin.Count > 0)
		{
			BCBM_Audio.publicAudio.PlaychipClip();
		}
		yield return new WaitForSeconds(0.6f);
		for (int num14 = 0; num14 < chipStartPos_Other.childCount; num14++)
		{
			UnityEngine.Object.Destroy(chipStartPos_Other.GetChild(num14).gameObject);
		}
		for (int num15 = 0; num15 < chipStartPos_Self.childCount; num15++)
		{
			UnityEngine.Object.Destroy(chipStartPos_Self.GetChild(num15).gameObject);
		}
	}

	private IEnumerator GetWinNumShow(int addNum)
	{
		showWinNumTrn.gameObject.SetActive(value: true);
		int tempNum = 0;
		Text addText = showWinNumTrn.Find("Text").GetComponent<Text>();
		float timeCor = 0f;
		while (true)
		{
			if (tempNum < addNum)
			{
				timeCor += 0.05f;
				tempNum = NumberChange(tempNum, tempNum, addNum, 0.1f);
				addText.text = "+" + tempNum;
				if (timeCor >= 2f)
				{
					tempNum = addNum;
				}
				if (tempNum >= addNum)
				{
					addText.text = "+" + addNum;
					UnityEngine.Debug.LogError("====加分停止===");
					break;
				}
				yield return new WaitForSeconds(0.05f);
				continue;
			}
			yield return new WaitForSeconds(0.1f);
			break;
		}
		addText.text = "+" + addNum;
		yield return new WaitForSeconds(1f);
		showWinNumTrn.gameObject.SetActive(value: false);
	}

	public int NumberChange(int initNum, int showNum, int targetNum, float v)
	{
		showNum += (int)((float)(targetNum - initNum) * Time.deltaTime / v);
		if (showNum >= targetNum)
		{
			showNum = targetNum;
		}
		return showNum;
	}

	public void CheckBet(int[] bets)
	{
	}

	public void PalyChip()
	{
	}

	private void UpdateTime()
	{
		int hour = DateTime.Now.Hour;
		int minute = DateTime.Now.Minute;
		string arg = (hour < 10) ? ("0" + hour) : hour.ToString();
		string arg2 = (minute < 10) ? ("0" + minute) : minute.ToString();
		txtTime.text = $"{BCBM_GameInfo.getInstance().UserInfo.RoomName}\n{arg}:{arg2}";
		if (lastBetNum > 0 && (double)lastBetNum <= displaylaue[1] && isStartGame)
		{
			xuYaBtn.interactable = true;
			xuYaBtn.transform.GetChild(0).GetComponent<Text>().DOFade(1f, 0.5f);
		}
		else
		{
			xuYaBtn.interactable = false;
			xuYaBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
		}
		if ((double)lastBetNum > displaylaue[1])
		{
			AutoBtn.interactable = false;
			AutoBtn.transform.GetChild(0).GetComponent<Text>().text = "自动";
			AutoBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
			isAuto = false;
		}
		if (lastBetNumTemp != lastBetNum)
		{
			lastBetNumTemp = lastBetNum;
			UnityEngine.Debug.LogError("=====上局自己总压====" + lastBetNumTemp);
		}
	}

	private IEnumerator displayMethon()
	{
		yield return new WaitForSeconds(0.5f);
		while (true)
		{
			UpdateTime();
			allScoceText.text = displaylaue[1].ToString();
			for (int i = 0; i < BetGerenText_List.Count; i++)
			{
				BetGerenText_List[i].text = $"{BetGerenValue[i]} / {BetZongValue[i]}";
			}
			if ((bool)OcclusionScene && !OcclusionScene.gameObject.activeInHierarchy)
			{
				if (isLater)
				{
					UnityEngine.Debug.LogError("======延迟一次======");
					yield return new WaitForSeconds(0.5f);
					isStartGame = true;
					isLater = false;
				}
				if (displaylaue[2] > 0.0)
				{
					if (AutoBtn != null && !AutoBtn.interactable)
					{
						AutoBtn.interactable = true;
					}
					AutoBtn.transform.GetChild(0).GetComponent<Text>().DOFade(1f, 0.5f);
				}
				else if (AutoBtn != null && AutoBtn.interactable)
				{
					AutoBtn.interactable = false;
					AutoBtn.transform.GetChild(0).GetComponent<Text>().DOFade(0.7f, 0.5f);
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	public IEnumerator ButtonAnimatorMethon(int Name)
	{
		Image image = Buttonanimators[Name];
		image.color = new Color(1f, 1f, 1f, 0f);
		image.gameObject.SetActive(value: true);
		for (int i = 0; i < 3; i++)
		{
			image.DOFade(1f, 0.5f);
			yield return new WaitForSeconds(0.5f);
			image.DOFade(0.25f, 0.5f);
			yield return new WaitForSeconds(0.5f);
		}
		image.DOFade(1f, 0.25f);
	}

	public void Init_ani()
	{
		foreach (Image buttonanimator in Buttonanimators)
		{
			if (buttonanimator != null)
			{
				buttonanimator.color = new Color(1f, 1f, 1f, 0f);
			}
			buttonanimator.gameObject.SetActive(value: false);
		}
	}

	public void TimeMethon(int Timelaue)
	{
		int num = BCBM_GameInfo.getInstance().BetTime + 17;
		if (Timelaue == num)
		{
			BCBM_Control.publicControl.NumList.Clear();
			Init_ani();
			CounterText.text = BCBM_GameInfo.getInstance().BetTime.ToString();
			displaylaue[0] = 0.0;
			BCBM_Control.publicControl.BetSceneMethon("向上");
			OcclusionScene.gameObject.SetActive(value: false);
			UnityEngine.Debug.LogError("===关闭遮挡1===");
			isLater = true;
			BCBM_Control.publicControl.waitScene.SetActive(value: false);
			return;
		}
		switch (Timelaue)
		{
		case -1:
			CounterText.text = string.Empty;
			return;
		case 2:
			CounterText.text = string.Empty;
			BCBM_Control.publicControl.BetSceneMethon("向上");
			BCBM_Control.publicControl.waitScene.SetActive(value: true);
			return;
		case 5:
			for (int i = 0; i < BetGerenValue.Length; i++)
			{
				BetGerenValue[i] = 0;
			}
			displaylaue[2] = 0.0;
			UnityEngine.Debug.LogError("===开始遮挡1===");
			OcclusionScene.gameObject.SetActive(value: true);
			BCBM_Control.publicControl.BoxRes();
			isLater = false;
			return;
		case 18:
		case 19:
		case 20:
		case 21:
			BCBM_Control.publicControl.BoxRes();
			isLater = false;
			break;
		case 17:
			BCBM_Control.publicControl.BetSceneMethon("向下");
			BCBM_AnimationScene.publicAnimationScene.HidCar();
			OcclusionScene.gameObject.SetActive(value: true);
			tNvLangCor.EndBet();
			SetLuStart();
			break;
		}
		if (Timelaue - 17 >= 0)
		{
			int num2 = Timelaue - 17;
			CounterText.text = num2.ToString();
			isStartGame = true;
			if (num2 > 3)
			{
				isMinTime = false;
				float num3 = (float)num2 / (float)BCBM_GameInfo.getInstance().BetTime;
				timeAndSpeedValue.fillAmount = num3 * 0.6f;
				return;
			}
			if (num2 > 0)
			{
				isMinTime = true;
				if (num2 >= 3)
				{
					if (waitSetValu != null)
					{
						StopCoroutine(waitSetValu);
					}
					waitSetValu = StartCoroutine(WaitSetValu());
				}
				return;
			}
			if (waitSetValu != null)
			{
				StopCoroutine(waitSetValu);
			}
			isMinTime = false;
			if (timeAndSpeedValue.fillAmount < 0.6f)
			{
				if (waitSetValuFull != null)
				{
					StopCoroutine(waitSetValuFull);
				}
				waitSetValuFull = StartCoroutine(WaitSetValuFull());
			}
			speedObj.SetActive(value: true);
			timeObj.SetActive(value: false);
			SetSpeed();
			return;
		}
		CounterText.text = string.Empty;
		if (timeAndSpeedValue.fillAmount < 0.6f)
		{
			if (waitSetValuFull != null)
			{
				StopCoroutine(waitSetValuFull);
			}
			waitSetValuFull = StartCoroutine(WaitSetValuFull());
		}
		isMinTime = false;
		isStartGame = false;
	}

	private IEnumerator WaitSetValu()
	{
		float tempVa = 0f;
		while (true)
		{
			tempVa += Time.deltaTime * 5.5f;
			if (tempVa >= 0.6f)
			{
				tempVa = 0f;
			}
			timeAndSpeedValue.fillAmount = tempVa;
			yield return new WaitForSeconds(0.1f);
		}
	}

	private IEnumerator WaitSetValuFull()
	{
		float tempVa2 = timeAndSpeedValue.fillAmount;
		while (true)
		{
			tempVa2 += Time.deltaTime * 10f;
			if (tempVa2 >= 0.6f)
			{
				break;
			}
			timeAndSpeedValue.fillAmount = tempVa2;
			yield return new WaitForSeconds(0.1f);
		}
		tempVa2 = 0.6f;
		timeAndSpeedValue.fillAmount = tempVa2;
	}

	private void SetSpeed()
	{
		speedTr.localRotation = speedTrOld.localRotation;
		speedTr.DORotate(new Vector3(0f, 0f, 12f), 0.5f).OnComplete(delegate
		{
			speedTr.DORotate(new Vector3(0f, 0f, -32f), 0.5f).OnComplete(delegate
			{
				speedTr.DORotate(new Vector3(0f, 0f, 12f), 0.5f).OnComplete(delegate
				{
					speedTr.DORotate(new Vector3(0f, 0f, -40f), 0.5f).OnComplete(delegate
					{
						speedTr.DORotate(new Vector3(0f, 0f, -168f), 0.5f).OnComplete(delegate
						{
							speedTr.DORotate(new Vector3(0f, 0f, -40f), 0.5f).OnComplete(delegate
							{
								speedTr.DORotate(new Vector3(0f, 0f, -224f), 1f);
							});
						});
					});
				});
			});
		});
	}

	public IEnumerator EndSpeed()
	{
		yield return new WaitForSeconds(6.5f);
		speedTr.DORotate(new Vector3(0f, 0f, -148f), 0.45f).OnComplete(delegate
		{
			speedTr.DORotate(new Vector3(0f, 0f, -74f), 0.45f).OnComplete(delegate
			{
				speedTr.DORotate(new Vector3(0f, 0f, 12f), 0.45f);
			});
		});
	}

	public static float NumberChange(float initNum, float showNum, float targetNum, float v)
	{
		showNum += (targetNum - initNum) * Time.deltaTime / v;
		if (showNum >= targetNum)
		{
			showNum = targetNum;
		}
		return showNum;
	}

	public void ResSetPanel()
	{
		for (int i = 0; i < displaylaue.Length; i++)
		{
			if (i != 1)
			{
				displaylaue[i] = 0.0;
			}
		}
		for (int j = 0; j < BetGerenValue.Length; j++)
		{
			BetGerenValue[j] = 0;
		}
		for (int k = 0; k < BetZongValue.Length; k++)
		{
			BetZongValue[k] = 0;
		}
	}

	public void ResSet()
	{
		if (BCBM_Control.publicControl != null)
		{
			BCBM_Control.publicControl.waitScene.SetActive(value: true);
			BCBM_Control.publicControl.BetSceneMethon("向下");
		}
		if (BCBM_AnimationScene.publicAnimationScene != null)
		{
			BCBM_AnimationScene.publicAnimationScene.HidCar();
		}
	}

	public void DeFenhgMethon(string Name)
	{
		switch (Name)
		{
		case "金鲨":
			displaylaue[0] = 91 * BetGerenValue[0];
			break;
		case "银鲨":
			displaylaue[0] = 24 * BetGerenValue[1];
			break;
		case "狮子":
			displaylaue[0] = 12 * BetGerenValue[2] + BetGerenValue[11] * 2;
			break;
		case "老鹰":
			displaylaue[0] = 12 * BetGerenValue[3] + BetGerenValue[10] * 2;
			break;
		case "熊猫":
			displaylaue[0] = 8 * BetGerenValue[4] + BetGerenValue[11] * 2;
			break;
		case "孔雀":
			displaylaue[0] = 8 * BetGerenValue[5] + BetGerenValue[10] * 2;
			break;
		case "猴子":
			displaylaue[0] = 8 * BetGerenValue[6] + BetGerenValue[11] * 2;
			break;
		case "鸽子":
			displaylaue[0] = 8 * BetGerenValue[7] + BetGerenValue[10] * 2;
			break;
		case "兔子":
			displaylaue[0] = 6 * BetGerenValue[8] + BetGerenValue[11] * 2;
			break;
		case "燕子":
			displaylaue[0] = 6 * BetGerenValue[9] + BetGerenValue[10] * 2;
			break;
		}
	}
}
