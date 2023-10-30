using STDT_GameConfig;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class XLDT_ShowScene : MonoBehaviour
{
	public GameObject objGameInfo;

	public XLDT_Book sptCard;

	public XLDT_AutoFlip sptFlipCard;

	public XLDT_CardSpis cards;

	public XLDT_SpiGirls girls;

	private Image imgGirl;

	private bool bShowGame;

	private Text[] txtLabs = new Text[100];

	private Text[] txtLabCai = new Text[100];

	private GameObject[] objSprites = new GameObject[100];

	private bool bShowTime;

	private int countTime;

	private int gameId;

	private int[] bonus;

	private Text txtCountTime;

	private Text txtBonus;

	private float time;

	private Text txtTime;

	private Text txtChit;

	private Text txtChit2;

	private GameObject[] objLang = new GameObject[2];

	private Text txtJu;

	private Text txtTurn;

	private XLDT_EAwardType awardType = XLDT_EAwardType.None;

	private bool bShowNotice;

	private Text txtNotice;

	private float fDs;

	private Color colJoker = new Color(40f / 51f, 0f, 1f);

	private int language;

	private Coroutine coBroadAwarding;

	private static XLDT_ShowScene _showScene;

	private int hour = -1;

	private int minute = -1;

	private int tempMinute = -100;

	public static XLDT_ShowScene getInstance()
	{
		return _showScene;
	}

	private void Awake()
	{
		if (_showScene == null)
		{
			_showScene = this;
		}
		language = XLDT_GameInfo.getInstance().Language;
		for (int i = 0; i < 2; i++)
		{
			objLang[i] = objGameInfo.transform.Find($"language{i}").gameObject;
			objLang[i].SetActive(i == language);
		}
		txtJu = objLang[language].transform.Find("ju/Label").GetComponent<Text>();
		txtJu.text = XLDT_GameInfo.getInstance().GamesId.ToString();
		txtTurn = objLang[language].transform.Find("turns/Label").GetComponent<Text>();
		txtCountTime = objGameInfo.transform.Find("counttime/Label").GetComponent<Text>();
		txtBonus = objGameInfo.transform.Find("bonus/Label").GetComponent<Text>();
		UnityEngine.Debug.Log(XLDT_GameInfo.getInstance().CurTable.Name);
		objGameInfo.transform.Find("other/name").GetComponent<Text>().text = XLDT_GameInfo.getInstance().CurTable.Name;
		txtTime = objGameInfo.transform.Find("other/time").GetComponent<Text>();
		Transform transform = objGameInfo.transform.Find("cardbg");
		txtChit = transform.Find("Label").GetComponent<Text>();
		txtChit2 = transform.Find("abnormal/Label2").GetComponent<Text>();
		imgGirl = transform.Find("Texture").GetComponent<Image>();
		txtNotice = base.transform.Find("Notice/ImgNotice/Text").GetComponent<Text>();
	}

	private void Start()
	{
		InitRecordObjs();
		bonus = new int[2];
		bonus[0] = 132311;
		bonus[1] = 132312;
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendResultList();
		XLDT_SoundManage.getInstance().IsGameMusic = XLDT_GameInfo.getInstance().Setted.bIsGameVolum;
		if (XLDT_GameInfo.getInstance().Setted.bIsGameVolum)
		{
			XLDT_SoundManage.getInstance().setGameBgMusic(isPlay: true);
		}
		UpdateTime();
		bShowGame = false;
	}

	private void OnEnable()
	{
	}

	private IEnumerator WaitStart()
	{
		UnityEngine.Debug.LogError("=====开始位置轮询=====");
		while (ZH2_GVars.isCanSenEnterGame)
		{
			yield return new WaitForSeconds(2.5f);
			try
			{
				WebSocket2.GetInstance().SenEnterGame(isEnterGame: true, ZH2_GVars.GameType.card_desk, XLDT_GameInfo.getInstance().CurTable.Id.ToString(), XLDT_GameInfo.getInstance().User.ScoreCount.ToString());
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}
	}

	private void Update()
	{
		if (bShowTime)
		{
			time += Time.deltaTime;
			if (time >= 1f)
			{
				time = 0f;
				countTime--;
				if (countTime < 0)
				{
					bShowTime = false;
				}
				else
				{
					if (countTime == 0)
					{
						XLDT_GameUIMngr.GetSingleton().mBetCtrl.BetEnable = false;
						XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.counttime0);
						XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.stopYZ);
					}
					else if (countTime <= 9)
					{
						XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.counttime);
					}
					if (countTime <= XLDT_GameInfo.getInstance().CountTime)
					{
						if (countTime == XLDT_GameInfo.getInstance().CountTime)
						{
							XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.startYZ);
						}
						txtCountTime.text = countTime.ToString();
						if (countTime != 0)
						{
							txtBonus.text = bonus[countTime - 1].ToString();
						}
					}
				}
			}
		}
		UpdateTime();
		UpdateShowNotice();
	}

	public void GameRestart(int turns, int nGameId, int[] bounus, int time, int girltype)
	{
		if (time >= 10)
		{
			bShowGame = true;
			XLDT_GameUIMngr.GetSingleton().objDark.SetActive(value: false);
			txtChit.gameObject.SetActive(value: false);
			txtChit2.transform.parent.gameObject.SetActive(value: false);
			ShowBeautifulGirl(show: false);
			countTime = time;
			UnityEngine.Debug.Log("GameRestart=" + time);
			bonus = new int[XLDT_GameInfo.getInstance().CountTime];
			UnityEngine.Debug.Log("GameRestart2=" + time);
			bonus = bounus;
			gameId = nGameId;
			txtTurn.text = turns.ToString();
			if (nGameId == 1)
			{
				for (int i = 0; i < 100; i++)
				{
					txtLabs[i].gameObject.SetActive(value: false);
					txtLabCai[i].gameObject.SetActive(value: false);
					objSprites[i].SetActive(value: false);
				}
			}
			if (XLDT_GameInfo.getInstance().CurAward.awardType == XLDT_EAwardType.Normal || XLDT_GameInfo.getInstance().CurAward.awardType == XLDT_EAwardType.Bonus)
			{
				girltype %= 29;
				StartCountTime(girltype);
			}
			else if (countTime == XLDT_GameInfo.getInstance().CountTime + 6)
			{
				ChangeCardTexture(XLDT_GameInfo.getInstance().CurAward.color, XLDT_GameInfo.getInstance().CurAward.num);
				StartYuJing();
				PlayYuJingEffect(show: true);
			}
			else
			{
				StartCountTime();
			}
		}
		else
		{
			bShowGame = false;
		}
	}

	protected void StartYuJing()
	{
		XLDT_SoundManage.getInstance().PlayPublicMusic(isPlay: true);
		if (XLDT_GameInfo.getInstance().CurAward.awardType == XLDT_EAwardType.ColorPublic)
		{
			sptFlipCard.FlipLeftPage_T();
		}
		PlayCoinEffect(show: false);
		ShowCard(show: false);
		StopCoroutine("YuJingEnd");
		StartCoroutine(YuJingEnd());
	}

	private IEnumerator YuJingEnd()
	{
		yield return new WaitForSeconds(4f);
		if (XLDT_GameInfo.getInstance().CurAward.awardType == XLDT_EAwardType.ColorPublic)
		{
		}
		yield return new WaitForSeconds(2f);
		PlayYuJingEffect(show: false);
		countTime -= 6;
		StartCountTime();
	}

	protected void StartCountTime(int girltype = 1)
	{
		bShowTime = true;
		time = 0f;
		txtChit.gameObject.SetActive(value: false);
		txtChit2.transform.parent.gameObject.SetActive(value: false);
		PlayCoinEffect(show: false);
		ShowCard(show: false);
		txtJu.text = gameId.ToString();
		if (countTime >= 0 && countTime <= XLDT_GameInfo.getInstance().CountTime)
		{
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.startYZ);
			if (countTime == 10)
			{
				XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.counttime);
			}
			if (countTime != 0)
			{
				txtBonus.text = bonus[countTime - 1].ToString();
			}
			txtCountTime.text = countTime.ToString();
		}
		else if (countTime > XLDT_GameInfo.getInstance().CountTime)
		{
			txtCountTime.text = "0";
		}
		if (XLDT_GameInfo.getInstance().CurAward.awardType != XLDT_EAwardType.ColorPublic && XLDT_GameInfo.getInstance().CurAward.awardType != XLDT_EAwardType.FlowerPublic)
		{
			ShowBeautifulGirl(show: true, girltype);
		}
	}

	public void StartPlayFanPai(XLDT_CurttenAward award)
	{
		MonoBehaviour.print("翻牌");
		if (!bShowGame)
		{
			XLDT_NetMain.GetSingleton().MyCreateSocket.SendResultList();
			return;
		}
		XLDT_GameUIMngr.GetSingleton().mBetCtrl.BetEnable = false;
		txtCountTime.text = "0";
		if (countTime > 0)
		{
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.counttime0);
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.stopYZ);
		}
		bShowTime = false;
		ShowBeautifulGirl(show: false);
		awardType = award.awardType;
		if (award.awardType == XLDT_EAwardType.Bonus)
		{
			txtBonus.text = award.bonus.ToString();
		}
		else
		{
			txtBonus.text = bonus[0].ToString();
		}
		ChangeCardTexture(award.color, award.num);
		ShowCard(show: true);
		sptFlipCard.FlipLeftPage();
		if (coBroadAwarding != null)
		{
			StopCoroutine("coRestrictReconnect");
		}
		coBroadAwarding = StartCoroutine(BroadAwarding(award));
	}

	protected void ChangeCardTexture(XLDT_ECardsColour color, int num)
	{
		int num2;
		switch (num)
		{
		case 14:
			num2 = 52;
			break;
		case 15:
			num2 = 53;
			break;
		default:
			num2 = (int)((num - 1) * 4 + color);
			break;
		}
		sptCard.bookPages[0] = cards.spiCrads[num2];
	}

	protected void ShowCard(bool show)
	{
		sptCard.gameObject.SetActive(show);
	}

	private IEnumerator BroadAwarding(XLDT_CurttenAward award)
	{
		yield return new WaitForSeconds(2f);
		if (XLDT_GameInfo.getInstance().Income.Count != 0)
		{
			for (int i = 0; i < XLDT_GameInfo.getInstance().Income.Count; i++)
			{
				XLDT_GameUIMngr.GetSingleton().mUserList.ShowUserWinScore(XLDT_GameInfo.getInstance().Income[i].seatId - 1, XLDT_GameInfo.getInstance().Income[i].score);
			}
		}
		if (award.num < 14 && award.num > 0)
		{
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.awardColor, (int)award.color);
			yield return new WaitForSeconds(1f);
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.awardNum, award.num - 1);
		}
		else if (award.num >= 14)
		{
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.awardNum, award.num - 1);
		}
		XLDT_NetMain.GetSingleton().MyCreateSocket.SendResultList();
		if (XLDT_GameInfo.getInstance().CurAward.num >= 14)
		{
			XLDT_GameUIMngr.GetSingleton().mBetCtrl.SetResultColor(XLDT_POKER_COLOR.PORKER_JOKER);
		}
		else
		{
			XLDT_GameUIMngr.GetSingleton().mBetCtrl.SetResultColor((XLDT_POKER_COLOR)XLDT_GameInfo.getInstance().CurAward.color);
		}
		if (awardType == XLDT_EAwardType.Bonus)
		{
			yield return new WaitForSeconds(6f);
			PlayCoinEffect(show: true);
			yield return new WaitForSeconds(10f);
		}
		else
		{
			yield return new WaitForSeconds(12f);
		}
		ShowCard(show: false);
		XLDT_GameInfo.getInstance().CurAward.awardType = XLDT_EAwardType.None;
		XLDT_SoundManage.getInstance().PlayPublicMusic(isPlay: false);
		PlayCoinEffect(show: false);
	}

	protected void PlayCoinEffect(bool show)
	{
		if (show)
		{
			XLDT_SoundManage.getInstance().playButtonMusic(XLDT_ButtonMusicType.bonus);
		}
	}

	protected void PlayYuJingEffect(bool show)
	{
		if (!show)
		{
		}
	}

	private void InitRecordObjs()
	{
		Transform transform = base.transform.Find("GameRecord/Records");
		for (int i = 0; i < 100; i++)
		{
			Transform child = transform.GetChild(i);
			txtLabs[i] = child.Find("Label").GetComponent<Text>();
			txtLabCai[i] = child.Find("LabelCai").GetComponent<Text>();
			objSprites[i] = child.Find("Sprite").gameObject;
		}
	}

	public void ShowRecordObj(XLDT_CardAlgorithmResult[] record)
	{
		if (record.Length > 100)
		{
			return;
		}
		for (int i = 0; i < record.Length; i++)
		{
			Text text = txtLabs[i];
			Text text2 = txtLabCai[i];
			GameObject gameObject = objSprites[i];
			if (record[i].awardType < 0 || record[i].awardType > 3)
			{
				text.text = "awardTypeerror";
			}
			if (i == record.Length - 1)
			{
				UnityEngine.Debug.Log("color=" + record[i].color);
				UnityEngine.Debug.Log("point=" + record[i].point);
			}
			if (record[i].color < 0 || record[i].color > 3)
			{
				text.text = "colorerror";
			}
			if (record[i].point < 0 || record[i].point > 15)
			{
				text.text = "pointerror";
			}
			string text3 = " ";
			switch (record[i].color)
			{
			case 0:
				text3 = ZH2_GVars.ShowTip("黑桃 ", "Spade ", string.Empty);
				text.color = Color.black;
				break;
			case 1:
				text3 = ZH2_GVars.ShowTip("红心 ", "Heart ", string.Empty);
				text.color = Color.red;
				break;
			case 2:
				text3 = ZH2_GVars.ShowTip("草花 ", "Clubs ", string.Empty);
				text.color = Color.black;
				break;
			case 3:
				text3 = ZH2_GVars.ShowTip("方片 ", "Diamo ", string.Empty);
				text.color = Color.red;
				break;
			}
			if (record[i].point > 0 && record[i].point < 14)
			{
				if (record[i].point == 11)
				{
					text.text = text3 + "J";
				}
				else if (record[i].point == 12)
				{
					text.text = text3 + "Q";
				}
				else if (record[i].point == 13)
				{
					text.text = text3 + "K";
				}
				else if (record[i].point == 1)
				{
					text.text = text3 + "A";
				}
				else
				{
					text.text = text3 + record[i].point;
				}
			}
			else if (record[i].point == 14)
			{
				text.text = ZH2_GVars.ShowTip("小王", "Joker", string.Empty);
				text.color = colJoker;
			}
			else if (record[i].point == 15)
			{
				text.text = ZH2_GVars.ShowTip("小王", "King", string.Empty);
				text.color = colJoker;
			}
			if (record[i].awardType == 3)
			{
				text2.text = text.text;
				text.gameObject.SetActive(value: false);
				text2.gameObject.SetActive(value: true);
				gameObject.SetActive(value: false);
			}
			else if (record[i].awardType == 1 || record[i].awardType == 2)
			{
				text.gameObject.SetActive(value: true);
				text2.gameObject.SetActive(value: false);
				gameObject.SetActive(value: true);
			}
			else
			{
				text.gameObject.SetActive(value: true);
				text2.gameObject.SetActive(value: false);
				gameObject.SetActive(value: false);
			}
		}
		for (int j = record.Length; j < 100; j++)
		{
			Text text4 = txtLabs[j];
			Text text5 = txtLabCai[j];
			GameObject gameObject2 = objSprites[j];
			if (text4.gameObject.activeSelf || text5.gameObject.activeSelf)
			{
				text4.gameObject.SetActive(value: false);
				text5.gameObject.SetActive(value: false);
				gameObject2.SetActive(value: false);
			}
		}
		if (awardType == XLDT_EAwardType.Bonus)
		{
			PlayCoinEffect(show: true);
		}
	}

	protected void ShowBeautifulGirl(bool show, int type = 1)
	{
		if ((bool)objGameInfo)
		{
			imgGirl.gameObject.SetActive(show);
			if (show)
			{
				imgGirl.sprite = girls.spiGirls[type];
				sptCard.ResetPage();
			}
		}
	}

	public void StartPrintChit(int type)
	{
		if (type >= 0 && type <= 4)
		{
			ShowBeautifulGirl(show: false);
			ShowCard(show: false);
			txtJu.text = XLDT_GameInfo.getInstance().GamesId.ToString();
			string text = string.Empty;
			switch (type)
			{
			case 0:
				text = ((XLDT_GameInfo.getInstance().Language == 0) ? "正在打印路单，请稍候……" : "The route list of first round is printing，please wait......");
				break;
			case 1:
				text = ((XLDT_GameInfo.getInstance().Language == 0) ? "路单已打印成功，祝您游戏愉快!" : "The route list has been printed successfully，and enjoy the game！");
				break;
			case 2:
				text = ((XLDT_GameInfo.getInstance().Language == 0) ? "游戏状态正常!" : " Normal condition");
				break;
			case 3:
				text = ((XLDT_GameInfo.getInstance().Language == 0) ? "本轮游戏结束，稍后将打印下一轮路单……" : "round is over，later will print the route list of next round......");
				break;
			case 4:
				text = ((XLDT_GameInfo.getInstance().Language == 0) ? "保箱异常，游戏暂停!" : "Printer error，the game has been stopped already！");
				break;
			}
			if (type == 2 || type == 4)
			{
				txtChit.gameObject.SetActive(value: false);
				txtChit2.transform.parent.gameObject.SetActive(value: true);
				txtChit2.text = text;
			}
			else
			{
				txtChit2.transform.parent.gameObject.SetActive(value: false);
				txtChit.gameObject.SetActive(value: true);
				txtChit.text = text;
			}
		}
	}

	public void updateSignal(string sLevel)
	{
	}

	public void registerSignalListen()
	{
	}

	public void unRegisterSinalListen()
	{
	}

	private void UpdateTime()
	{
		hour = DateTime.Now.Hour;
		minute = DateTime.Now.Minute;
		if (txtTime != null && tempMinute != minute)
		{
			tempMinute = minute;
			txtTime.text = string.Format("{0}:{1}", hour.ToString("00"), minute.ToString("00"));
		}
	}

	public void ShowNotice(string str)
	{
		Image component = txtNotice.transform.parent.GetComponent<Image>();
		component.gameObject.SetActive(value: true);
		txtNotice.text = str;
		Vector2 sizeDelta = component.rectTransform.sizeDelta;
		float num = sizeDelta.x / 2f;
		Vector2 sizeDelta2 = txtNotice.rectTransform.sizeDelta;
		float num2 = sizeDelta2.x / 2f;
		txtNotice.transform.localPosition = Vector3.right * (num + num2);
		bShowNotice = true;
		Vector3 localPosition = txtNotice.transform.localPosition;
		fDs = Mathf.Abs(localPosition.x);
	}

	protected void HideNotice()
	{
		bShowNotice = false;
		txtNotice.transform.parent.gameObject.SetActive(value: false);
	}

	protected void UpdateShowNotice()
	{
		if (bShowNotice)
		{
			txtNotice.transform.Translate((0f - Time.deltaTime) * 0.2f, 0f, 0f);
			Vector3 localPosition = txtNotice.transform.localPosition;
			if (Mathf.Abs(localPosition.x) > fDs)
			{
				HideNotice();
			}
		}
	}
}
