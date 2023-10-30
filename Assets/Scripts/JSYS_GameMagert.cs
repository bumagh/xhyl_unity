using System;
using System.Collections;
using System.Collections.Generic;
using com.QH.QPGame.Lobby.Surfaces;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JSYS_GameMagert : MonoBehaviour
{
	public delegate void cheakpoket();

	private delegate void cheakupdatemoney();

	private delegate void isfinishiAndRushView();

	private delegate void onetime_date(string opendate);

	private static JSYS_GameMagert _instance;

	public List<Text> uesrinscoercout;

	public List<Text> userinscoercoutright;

	public List<Text> userscorein;

	public List<Text> uiallintext;

	public List<Button> buttonlist;

	public List<Text> titlettextlist;

	public List<Button> canellist;

	public List<Image> imagechange;

	public Text statetext;

	public Text coundowntext;

	public GameObject pokerlist;

	public GameObject pokerlist2;

	public GameObject poketGOtoinit;

	public GameObject userGo;

	public GameObject error_left;

	public GameObject error_right;

	public Button cnealGO;

	public Button cnealGO2;

	public Text AllScroceText;

	public Text WinScroceInText;

	public int waittimeui;

	public GameObject errorGo;

	public Button quit;

	public string URL;

	public List<GameObject> buttonmask;

	public InputField inputtext;

	public GameObject winpanel;

	public GameObject winpanel2;

	public Text gonggao;

	public JSYS_MessageBoxPopup messg;

	public static bool swithonislistchange;

	public countdowdata nowdata;

	public JSYS_playstate ChangeState;

	public int severcount;

	public Toggle fullwindowGO;

	public Slider audiosource;

	public Toggle voicetoggle;

	public GameObject changeimageGO;

	public Button canelconfirmGO;

	public GameObject useropenGO;

	public GameObject canelhint;

	public Text canelhinttext;

	public GameObject timeover;

	public Button rushpanel;

	public Text limittext;

	public Text lessdown;

	public Text thistimecoutText;

	public int count;

	public string value_;

	private bool fristopen;

	private JSYS_playstate NowState;

	private int countsize;

	private bool isfrist;

	private int[] listrange;

	private int tempc;

	private int[] poketcount;

	private int[] poketcount2;

	private int winCodeforSever;

	private int winCodeforUi;

	private double allScroceForSever;

	private double allScroceForUi;

	protected int[] historyuserin;

	protected int historyallin;

	private List<poketstate> severpokethuase;

	private List<poketstate> severpokethuase2;

	private int[] betinlist;

	private bool openinnexttime;

	private List<int> severpoketcode;

	private IEnumerator counwtIDE;

	private List<int> buttonIDcode;

	private List<string> buttonrate;

	private int gotohallint;

	private bool isPause;

	private bool isFocus;

	private bool isopendatafromsever = true;

	private int erroropendatacout;

	public static bool iscomeback;

	public static bool isupdatetime;

	private bool isopennextdata;

	private Vector3 startcont;

	private Vector3 endcont;

	private bool isfullopenwindows;

	private bool fristuserin;

	private bool isbetincheak;

	private bool istimeover;

	public int gonggaocount;

	private int countouit;

	private int rushin;

	private bool isfristopennumber = true;

	private string textwindata = string.Empty;

	private int updatenowcount;

	private int updatelimitcount = 3;

	private int updateusercoutnowcount = 101;

	private int updateusercoutlimitcount = 100;

	private bool isopenanmimation = true;

	private static bool fixeddatestringswith;

	private bool isopendatenocheakswith;

	public Button tuiShuiShowButton;

	public Button tuiShuiButton;

	public Text tuiShuiRate;

	public Text tuiShuiRateNum;

	public GameObject TuiShuiPanel;

	private Vector3 vecFir;

	public Text servicesPanelText;

	public List<Text> rateList;

	private bool isFirstRefreashRate;

	public GameObject grid;

	public Transform gridNewPar;

	public List<GameObject> gridNew;

	private int aGrid;

	public ScrollRect[] gridScrool;

	public GameObject uiGoPanel;

	private bool agr;

	private bool bgr;

	private bool isChangeKaiJiang;

	private bool isChangeXiaZhu;

	public Text mainText;

	private GameObject goLeft;

	private GameObject goRight;

	public int Count
	{
		get
		{
			return count;
		}
	}

	public string value
	{
		get
		{
			return value_;
		}
	}

	public static JSYS_GameMagert Instance
	{
		get
		{
			return _instance;
		}
	}

	public string Textwindata
	{
		get
		{
			return textwindata;
		}
		set
		{
			textwindata = value;
		}
	}

	public event cheakpoket cheak;

	private event cheakupdatemoney cheakindang;

	private event isfinishiAndRushView rushviewevent;

	private event onetime_date openinfrist;

	private void Awake()
	{
		_instance = this;
	}

	private void OnEnable()
	{
		isPause = false;
		isFocus = false;
	}

	private void init()
	{
		mainText.text = "切换 " + JSYS_LoginInfo.Instance().mylogindata.roomcount;
		vecFir = (gonggao.rectTransform.localPosition += new Vector3(gonggao.preferredWidth, 0f, 0f));
		ChangeState = JSYS_playstate.betready;
		isfrist = true;
		listrange = new int[4] { 10, 50, 100, 500 };
		poketcount = new int[5];
		poketcount2 = new int[4];
		tempc = 0;
		betinlist = new int[5];
		gonggaocount = 0;
		JSYS_LoginInfo.Instance();
		servicesPanelText.text = JSYS_LoginInfo.Instance().mylogindata.servicesInfo;
		aGrid = 0;
		fristopen = true;
		fristuserin = true;
		isbetincheak = true;
		openinnexttime = false;
		isFirstRefreashRate = true;
		bgr = true;
		isChangeKaiJiang = false;
		isChangeXiaZhu = false;
		JSYS_LoginInfo.Instance().mylogindata.isOpenError = true;
		countouit = 0;
		rushin = 0;
		JSYS_LoginInfo.Instance().mylogindata.coindown = int.Parse(JSYS_LoginInfo.Instance().mylogindata.roomcount);
		historyuserin = new int[5];
		swithonislistchange = true;
		severpoketcode = new List<int>();
		severpokethuase = new List<poketstate>();
		severpokethuase2 = new List<poketstate>();
		nowdata = new countdowdata();
		buttonrate = new List<string>();
		buttonIDcode = new List<int>();
		erroropendatacout = 0;
		titlettextlist[0].text = JSYS_LoginInfo.Instance().mylogindata.username;
		titlettextlist[1].text = "0";
		titlettextlist[2].text = "0";
		titlettextlist[3].text = "0";
		titlettextlist[4].text = "0";
		titlettextlist[5].text = "0";
		limittext.text = JSYS_LoginInfo.Instance().mylogindata.roomlitmit;
		lessdown.text = JSYS_LoginInfo.Instance().mylogindata.roomcount;
		gotohallint = 0;
		startcont = gonggao.rectTransform.localPosition;
		endcont = startcont;
		isfullopenwindows = false;
		fullwindowGO.onValueChanged.AddListener(fullwindowcontorl);
		voicetoggle.onValueChanged.AddListener(closeclickvoice);
		audiosource.onValueChanged.AddListener(backmusicchange);
		cheak += cheakpoketpoolisture;
		cheakindang += cheakifmoney;
		tuiShuiShowButton.onClick.AddListener(OnShowTuiShui);
		tuiShuiButton.onClick.AddListener(OnTuiShui);
	}

	private void Start()
	{
		init();
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		if ((Screen.width == 2048 && Screen.height == 1536) || (Screen.width == 1536 && Screen.height == 2048))
		{
			uiGoPanel.transform.localScale = new Vector3(0.81f, 1f, 1f);
		}
		Debug.Log(DateTime.Now.Hour);
		getpoint(Convert.ToDouble(JSYS_LoginInfo.Instance().mylogindata.ALLScroce));
		setUserUIinfo();
		addlistenrt();
		StartCoroutine(polling());
		StartCoroutine(gonggaoanmation());
	}

	private void getpoint(double temp)
	{
		allScroceForSever = temp;
		allScroceForUi = allScroceForSever;
		setallsroce(allScroceForUi);
	}

	private void changetextmoney(double temp)
	{
		allScroceForUi = temp;
		setallsroce(allScroceForUi);
	}

	private void changecountall(List<poketstate> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] > poketstate.Diamond)
			{
				poketcount[(int)list[i] % 4]++;
				poketcount[4]++;
			}
			else
			{
				poketcount[(int)list[i]]++;
			}
		}
	}

	private void changecountallright(List<poketstate> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			poketcount2[(int)list[i] % 4]++;
		}
	}

	private void changecountall(poketstate state)
	{
		if (state > poketstate.Diamond)
		{
			poketcount[(int)state % 4]++;
			poketcount[4]++;
		}
		else
		{
			poketcount[(int)state]++;
		}
	}

	private void changecountallright(poketstate state)
	{
		poketcount2[(int)state % 4]++;
	}

	private void opendataUpdate(string text)
	{
		if (text != string.Empty)
		{
			updatenowcount++;
			if (updatenowcount >= updatelimitcount || !(textwindata != string.Empty))
			{
				Textwindata = text;
				updatenowcount = 0;
			}
		}
	}

	private IEnumerator cheakwillfixed(string URL)
	{
		UnityWebRequest www = UnityWebRequest.Get(URL);
		yield return www.Send();
		opendataUpdate(www.downloadHandler.text);
		JsonData jd = getdataforjson(www.downloadHandler.text);
		if (www.error == null && NowState != JSYS_playstate.betclearing && !isopendatenocheakswith && jd["code"].ToString() == "200")
		{
			if (jd["ArrList"].Count < countsize)
			{
				startclosepoket(0);
				yield return StartCoroutine(initpoketlist(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.winlistAPI, 1));
			}
			yield return new WaitForSeconds(1f);
			if (NowState != JSYS_playstate.betclearing && jd["ArrList"].Count > countsize)
			{
				startclosepoket(0);
				yield return StartCoroutine(initpoketlist(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.winlistAPI, 1));
			}
		}
	}

	private IEnumerator gonggaoanmation()
	{
		while (true)
		{
			gonggao.rectTransform.localPosition -= new Vector3(2f, 0f, 0f);
			yield return new WaitForSeconds(0.05f);
			if (gonggao.rectTransform.localPosition.x <= vecFir.x - gonggao.preferredWidth / 2f - 486.5f - 5f)
			{
				gonggao.rectTransform.localPosition = vecFir + new Vector3(gonggao.preferredWidth / 2f, 0f, 0f);
				gonggao.transform.localScale = Vector3.zero;
				endcont = startcont;
				gonggaocount++;
			}
		}
	}

	private IEnumerator polling()
	{
		while (true)
		{
			if (DateTime.Now.Hour < 4)
			{
				if (titlettextlist[3].text.Equals("48"))
				{
					isopendatenocheakswith = true;
				}
				else
				{
					isopendatenocheakswith = false;
				}
				changeimageGO.transform.GetChild(0).gameObject.SetActive(false);
				changeimageGO.transform.GetChild(6).gameObject.SetActive(true);
			}
			else
			{
				isopendatenocheakswith = false;
				changeimageGO.transform.GetChild(0).gameObject.SetActive(true);
				changeimageGO.transform.GetChild(6).gameObject.SetActive(false);
			}
			try
			{
				StartCoroutine(getdatainitformsever(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.gameinfoPollingAPI + "user_id=" + JSYS_LoginInfo.Instance().mylogindata.user_id + "&room_id=" + JSYS_LoginInfo.Instance().mylogindata.room_id + "&unionuid=" + JSYS_LoginInfo.Instance().mylogindata.token));
				StartCoroutine(JSYS_LoginInfo.Instance().wwwinstance.sendcoutdown(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.counwtAPI));
				if (!isfrist)
				{
					StartCoroutine(cheakwillfixed(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.winlistAPI));
				}
				StartCoroutine(GetAnnouncementInfo(JSYS_LoginInfo.Instance().mylogindata.URL + "api/notice?user_id=" + JSYS_LoginInfo.Instance().mylogindata.user_id + "&game_id=" + JSYS_LoginInfo.Instance().mylogindata.choosegame));
				if (updateusercoutnowcount < updateusercoutlimitcount)
				{
					updateusercoutnowcount++;
				}
				else
				{
					StartCoroutine(rewww(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.usercoininhistoryAPI + "plate=1&user_id=" + JSYS_LoginInfo.Instance().mylogindata.user_id + "&room_id=" + JSYS_LoginInfo.Instance().mylogindata.room_id, true));
					StartCoroutine(rewww(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.usercoininhistoryAPI + "plate=2&user_id=" + JSYS_LoginInfo.Instance().mylogindata.user_id + "&room_id=" + JSYS_LoginInfo.Instance().mylogindata.room_id, false));
					updateusercoutnowcount = 0;
				}
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
			}
			yield return new WaitForSeconds(2f);
			if (bgr)
			{
				gridScrool[0].verticalNormalizedPosition = 0f;
				gridScrool[1].verticalNormalizedPosition = 0f;
				bgr = false;
			}
		}
	}

	private IEnumerator rewww(string url, bool leftorright)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();
		if (www.error == null)
		{
			useropenGO.GetComponent<JSYS_outtextstlye>().addtexttoui(www.downloadHandler.text, leftorright);
		}
		else
		{
			StartCoroutine(rewww(url, leftorright));
		}
	}

	private IEnumerator getdatainitformsever(string url)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			if (jsonData["code"].ToString() == "200")
			{
				gotohallint = 0;
				double num = Convert.ToDouble(jsonData["Userinfo"]["quick_credit"].ToString());
				JSYS_LoginInfo.Instance().mylogindata.ALLScroce = num.ToString();
				if (isFirstRefreashRate)
				{
					for (int i = 0; i < rateList.Count; i++)
					{
						rateList[i].text = "x" + jsonData["Oddlist"][i]["rate"].ToString();
						if (i == 8)
						{
							isFirstRefreashRate = false;
						}
					}
				}
				if (isopenanmimation && num != allScroceForSever)
				{
					getpoint(num);
				}
				for (int j = 0; j < jsonData["Oddlist"].Count; j++)
				{
					uiallintext[j].text = jsonData["Oddlist"][j]["dnum"].ToString();
				}
				for (int k = 0; k < jsonData["Oddlist"].Count; k++)
				{
					if (buttonIDcode.Count > 9 && buttonrate.Count > 9)
					{
						break;
					}
					buttonIDcode.Add(Convert.ToInt32(jsonData["Oddlist"][k]["id"].ToString()));
					buttonrate.Add(jsonData["Oddlist"][k]["rate"].ToString());
				}
				if (fristuserin)
				{
					rushin++;
					for (int l = 0; l < jsonData["Oddlist"].Count; l++)
					{
						userscorein[l].text = jsonData["Oddlist"][l]["user_dnum"].ToString();
					}
					if (rushin >= 2)
					{
						fristuserin = false;
					}
				}
				if (openinnexttime)
				{
					for (int m = 0; m < jsonData["Oddlist"].Count; m++)
					{
						userscorein[m].text = jsonData["Oddlist"][m]["user_dnum"].ToString();
					}
					openinnexttime = false;
				}
				if (jsonData["BetTotal"].ToString() != string.Empty)
				{
					WinScroceInText.text = "总压分：" + jsonData["BetTotal"].ToString();
				}
				yield break;
			}
			messg.Show("异常", jsonData["msg"].ToString(), JSYS_ButtonStyle.Confirm, delegate(JSYS_MessageBoxResult call)
			{
				switch (call)
				{
				case JSYS_MessageBoxResult.Cancel:
				{
					JSYS_LoginInfo.Instance().cleanmylogindata();
					AsyncOperation asyncOperation6 = Application.LoadLevelAsync(0);
					asyncOperation6.allowSceneActivation = true;
					break;
				}
				case JSYS_MessageBoxResult.Confirm:
				{
					JSYS_LoginInfo.Instance().cleanmylogindata();
					AsyncOperation asyncOperation5 = Application.LoadLevelAsync(0);
					asyncOperation5.allowSceneActivation = true;
					break;
				}
				case JSYS_MessageBoxResult.Timeout:
				{
					JSYS_LoginInfo.Instance().cleanmylogindata();
					AsyncOperation asyncOperation4 = Application.LoadLevelAsync(0);
					asyncOperation4.allowSceneActivation = true;
					break;
				}
				}
			}, 0f);
			yield break;
		}
		gotohallint++;
		if (gotohallint <= 3)
		{
			yield break;
		}
		gotohallint = 0;
		messg.Show("异常", "网络异常，即将退出游戏", JSYS_ButtonStyle.Confirm, delegate(JSYS_MessageBoxResult call)
		{
			switch (call)
			{
			case JSYS_MessageBoxResult.Cancel:
			{
				JSYS_LoginInfo.Instance().cleanmylogindata();
				AsyncOperation asyncOperation3 = Application.LoadLevelAsync(0);
				asyncOperation3.allowSceneActivation = true;
				break;
			}
			case JSYS_MessageBoxResult.Confirm:
			{
				JSYS_LoginInfo.Instance().cleanmylogindata();
				AsyncOperation asyncOperation2 = Application.LoadLevelAsync(0);
				asyncOperation2.allowSceneActivation = true;
				break;
			}
			case JSYS_MessageBoxResult.Timeout:
			{
				JSYS_LoginInfo.Instance().cleanmylogindata();
				AsyncOperation asyncOperation = Application.LoadLevelAsync(0);
				asyncOperation.allowSceneActivation = true;
				break;
			}
			}
		});
	}

	private void FixedUpdate()
	{
		if (agr)
		{
			gridScrool[0].verticalNormalizedPosition = 0f;
			gridScrool[1].verticalNormalizedPosition = 0f;
			agr = false;
		}
		if (NowState != ChangeState)
		{
			NowState = ChangeState;
			changeState(NowState);
		}
		if (JSYS_LoginInfo.Instance().wwwinstance.listformwwwinfo.Count <= 0)
		{
			return;
		}
		if (JSYS_LoginInfo.Instance().wwwinstance.listformwwwinfo.Count == 1)
		{
			if (swithonislistchange)
			{
				getinfoandsetstate(JSYS_LoginInfo.Instance().wwwinstance.listformwwwinfo.Peek());
				swithonislistchange = false;
			}
		}
		else if (JSYS_LoginInfo.Instance().wwwinstance.listformwwwinfo.Count > 1)
		{
			JSYS_wwwinfo jSYS_wwwinfo = JSYS_LoginInfo.Instance().wwwinstance.listformwwwinfo.Dequeue();
			swithonislistchange = true;
		}
	}

	private void getinfoandsetstate(JSYS_wwwinfo info)
	{
		switch (info.statetype)
		{
		case 0:
			getdatatojson(info.textinfo);
			changestate(JSYS_playstate.betin);
			break;
		case 1:
			getdatatojson(info.textinfo);
			changestate(JSYS_playstate.betover);
			break;
		case 2:
			getdatatojson(info.textinfo);
			changestate(JSYS_playstate.betclearing);
			break;
		}
	}

	private void getdatatojson(string temp)
	{
		Debug.Log(temp);
		JsonData jsonData = JsonMapper.ToObject(temp);
		if (!(jsonData["code"].ToString() == "200"))
		{
			return;
		}
		nowdata.is_open = (int)jsonData["info"]["is_open"];
		if (!isopendatafromsever && nowdata.periods != null && !nowdata.periods.Equals(jsonData["info"]["dates"].ToString()))
		{
			isopennextdata = true;
		}
		nowdata.periods = jsonData["info"]["dates"].ToString();
		if ((nowdata.is_open == 0 || nowdata.is_open == 1) && nowdata.couwttime != 0 && nowdata.couwttime != (int)jsonData["info"]["WinnerCountdown"])
		{
			isupdatetime = true;
		}
		nowdata.couwttime = (int)jsonData["info"]["WinnerCountdown"];
		nowdata.last_periods = jsonData["info"]["last_dates"].ToString();
		nowdata.last_winnumber = jsonData["info"]["last_winnumber"].ToString();
		nowdata.TotalCount = jsonData["info"]["TotalCount"].ToString();
		nowdata.RemainData = jsonData["info"]["RemainData"].ToString();
		if (isopennextdata)
		{
			if (nowdata.is_open == 1)
			{
				this.cheak();
			}
			for (int i = 0; i < userscorein.Count; i++)
			{
				userscorein[i].text = "0";
				openinnexttime = true;
			}
			if (nowdata.is_open != 2)
			{
				isopennextdata = false;
			}
		}
		if (nowdata.is_open == 2)
		{
			nowdata.wintype1 = changeTYPEtoint(jsonData["info"]["win_text1"].ToString());
			nowdata.wintype2 = changeTYPEtoint(jsonData["info"]["win_text2"].ToString());
			nowdata.win_number = jsonData["info"]["win_number"].ToString();
			Debug.Log(temp + "is_open = 2");
			nowdata.EWinTime = (int)jsonData["info"]["EWinTime"];
		}
		else
		{
			nowdata.wintype1 = poketstate.isnull;
			nowdata.wintype2 = poketstate.isnull;
			nowdata.win_number = string.Empty;
			nowdata.EWinTime = 0;
		}
		if (isfristopennumber)
		{
			if (nowdata.win_number != string.Empty)
			{
				JSYS_NumSpriteControl.Instances.SetImage(changestringtointlist(nowdata.win_number));
			}
			else
			{
				JSYS_NumSpriteControl.Instances.SetImage(changestringtointlist(nowdata.last_winnumber));
			}
			isfristopennumber = false;
		}
		titlettextlist[1].text = nowdata.last_periods;
		titlettextlist[2].text = nowdata.TotalCount;
		titlettextlist[3].text = nowdata.RemainData;
		titlettextlist[4].text = nowdata.periods;
	}

	private void changeState(JSYS_playstate state)
	{
		switch (state)
		{
		case JSYS_playstate.betnull:
			break;
		case JSYS_playstate.betready:
			betready();
			break;
		case JSYS_playstate.betin:
			betin();
			break;
		case JSYS_playstate.betover:
			betover();
			break;
		case JSYS_playstate.betopen:
			betwaitopen();
			break;
		case JSYS_playstate.betclearing:
			betcleaning();
			break;
		case JSYS_playstate.bettime:
			bettime();
			break;
		}
	}

	private void betready()
	{
		if (isfrist)
		{
			startclosepoket(0);
			isfrist = false;
			StartCoroutine(initpoketlist(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.winlistAPI, 1));
		}
		else
		{
			startclosepoket(countsize);
		}
		thistimecoutText.text = "本局总得分：0";
		statetext.text = "准备中";
		cleanallinanduserin();
		cleanhistorydata();
	}

	private void betin()
	{
		if (!isChangeXiaZhu)
		{
			JSYS_Audiomanger._instenc.ChangeBGMusic();
			isChangeXiaZhu = !isChangeXiaZhu;
			if (isChangeKaiJiang)
			{
				isChangeKaiJiang = false;
			}
		}
		if (counwtIDE != null)
		{
			StopCoroutine(counwtIDE);
		}
		thistimecoutText.text = "本局总得分：0";
		statetext.text = "请下注";
		statetext.color = Color.red;
		if (!fristuserin && !isbetincheak)
		{
			cleanallinanduserin();
			cleanhistorydata();
		}
		else
		{
			isbetincheak = false;
		}
		if (fristopen)
		{
			fristopen = false;
		}
		if (istimeover)
		{
		}
		isopenanmimation = true;
		if (nowdata.couwttime > 0)
		{
			waittimeui = nowdata.couwttime + 1;
		}
		counwtIDE = betincountdown();
		StartCoroutine(counwtIDE);
	}

	private void changefromdataislivergame()
	{
		StartCoroutine(initpoketlist(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.winlistAPI, 0));
	}

	private void erroroutgame()
	{
		erroropendatacout++;
		if (erroropendatacout <= 4)
		{
			return;
		}
		erroropendatacout = 0;
		messg.Show("异常", "您由于在游戏期间离开次数过多，请退出房间后再进入", JSYS_ButtonStyle.Confirm, delegate(JSYS_MessageBoxResult call)
		{
			switch (call)
			{
			case JSYS_MessageBoxResult.Cancel:
				tohall();
				break;
			case JSYS_MessageBoxResult.Confirm:
				tohall();
				break;
			case JSYS_MessageBoxResult.Timeout:
				tohall();
				break;
			}
		}, 0f);
	}

	private void betover()
	{
		if (fristopen)
		{
			if (nowdata.couwttime > 0)
			{
				waittimeui = nowdata.couwttime + 1;
			}
			counwtIDE = betincountdown();
			StartCoroutine(counwtIDE);
			fristopen = false;
		}
		thistimecoutText.text = "本局总得分：0";
		JSYS_NumSpriteControl.Instances.StartImage();
		isopendatafromsever = true;
		isopenanmimation = false;
		statetext.text = "已封盘";
		statetext.color = Color.green;
		if (!isChangeKaiJiang)
		{
			JSYS_Audiomanger._instenc.ChangeBGMusic_KaiJiang();
			isChangeKaiJiang = !isChangeKaiJiang;
			if (isChangeXiaZhu)
			{
				isChangeXiaZhu = false;
			}
		}
	}

	private void betwaitopen()
	{
		isbetincheak = false;
		if (counwtIDE != null)
		{
			StopCoroutine(counwtIDE);
		}
		isopendatafromsever = true;
		if (isopennextdata)
		{
			isopennextdata = false;
		}
		if (fristopen)
		{
		}
		statetext.text = "当前状态：等待开奖中";
	}

	private void betcleaning()
	{
		isbetincheak = false;
		if (counwtIDE != null)
		{
			StopCoroutine(counwtIDE);
		}
		statetext.text = "开奖中";
		if (nowdata.EWinTime > 0)
		{
			waittimeui = nowdata.EWinTime + 1;
		}
		counwtIDE = betincountdown();
		StartCoroutine(counwtIDE);
		if (!isopennextdata)
		{
			isopendatafromsever = true;
		}
		try
		{
			isopenanmimation = false;
			if (!isopendatafromsever)
			{
				StartCoroutine(opentolist(1));
			}
			else
			{
				StartCoroutine(opentolist(0));
			}
		}
		catch (Exception)
		{
		}
	}

	private void bettime()
	{
		statetext.text = "当前状态：休息";
	}

	private IEnumerator GetAnnouncementInfo(string URL)
	{
		UnityWebRequest temp = UnityWebRequest.Get(URL);
		yield return temp.Send();
		JsonData jd = getdataforjson(temp.downloadHandler.text);
		if (jd["code"].ToString() == "200")
		{
			if (gonggaocount >= jd["List"].Count)
			{
				gonggaocount = 0;
			}
			if (gonggao.text != string.Empty && !gonggao.text.Equals(jd["List"][gonggaocount]["site"].ToString()))
			{
				gonggao.text = jd["List"][gonggaocount]["site"].ToString();
				gonggao.rectTransform.localPosition = vecFir + new Vector3(gonggao.preferredWidth / 2f, 0f, 0f);
				gonggao.transform.localScale = Vector3.one;
			}
		}
		else
		{
			gonggao.text = "暂无公告";
		}
	}

	private JsonData getdataforjson(string text)
	{
		return JsonMapper.ToObject(text);
	}

	private IEnumerator opentolist(int toggle)
	{
		switch (toggle)
		{
		case 1:
			yield return StartCoroutine(initpoketlist(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.winlistAPI, 0));
			JSYS_NumSpriteControl.Instances.StopImage(changestringtointlist(nowdata.win_number));
			isopendatafromsever = true;
			break;
		case 0:
			if (isfullopenwindows)
			{
				yield return StartCoroutine(change());
			}
			yield return new WaitForSeconds(3f);
			yield return starsetwinnumber(changestringtointlist(nowdata.win_number));
			JSYS_NumSpriteControl.Instances.SetImage(changestringtointlist(nowdata.win_number));
			yield return new WaitForSeconds(3f);
			if (nowdata.wintype1 >= poketstate.spdaesJR)
			{
				StartCoroutine(buttomwinanimation((int)nowdata.wintype1 % 4));
				StartCoroutine(buttomwinanimation(4));
			}
			else
			{
				StartCoroutine(buttomwinanimation((int)nowdata.wintype1));
			}
			StartCoroutine(buttomwinanimation((int)nowdata.wintype2 % 4 + 5));
			isopendatafromsever = true;
			yield return addpoket();
			showpoketcount(poketcount);
			showpoketcountright(poketcount2);
			StartCoroutine(getwindataandopenpanel(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.wingetforinterfroAPI + "user_id=" + JSYS_LoginInfo.Instance().mylogindata.user_id + "&periods=" + nowdata.periods));
			break;
		}
		updatenowcount = 4;
		updateusercoutnowcount = 101;
		isopennextdata = false;
		yield return new WaitForSeconds(1f);
		cleanallinanduserin();
	}

	private IEnumerator change()
	{
		yield return new WaitForSeconds(5f);
	}

	private IEnumerator getwindataandopenpanel(string url)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();
		JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
		Debug.Log(www.downloadHandler.text);
		if (www.error == null)
		{
			if (!jd["periods"].ToString().Equals(nowdata.periods))
			{
				yield break;
			}
			if (jd["code"].ToString() == "200")
			{
				if (Convert.ToDouble(jd["WinTotal1"].ToString()) > 0.0)
				{
					winpanel.transform.GetChild(0).GetChild(1).GetComponent<Text>()
						.text = "恭喜你本局赢取了：" + jd["WinTotal1"].ToString() + "分";
					winpanel.SetActive(true);
				}
				if (Convert.ToDouble(jd["WinTotal2"].ToString()) > 0.0)
				{
					winpanel2.transform.GetChild(0).GetChild(1).GetComponent<Text>()
						.text = "恭喜你本局赢取了：" + jd["WinTotal2"].ToString() + "分";
					winpanel2.SetActive(true);
				}
				if (Convert.ToDouble(jd["WinTotal"].ToString()) > 0.0)
				{
					thistimecoutText.text = "本局总得分：" + jd["WinTotal"].ToString();
				}
				else
				{
					thistimecoutText.text = "本局总得分：0";
				}
			}
		}
		else
		{
			StartCoroutine(getwindataandopenpanel(url));
		}
		yield return new WaitForSeconds(1f);
		isopenanmimation = true;
	}

	private IEnumerator colsewindows(GameObject go)
	{
		yield return new WaitForSeconds(5f);
		if (go != null)
		{
			go.SetActive(false);
		}
	}

	private IEnumerator gamealive(string URL)
	{
		UnityWebRequest www = UnityWebRequest.Get(URL);
		yield return www.Send();
		JsonData jd = getdataforjson(www.downloadHandler.text);
		if (!(jd["code"].ToString() == "200"))
		{
			tohall();
		}
	}

	private void cheakpoketpoolisture()
	{
		StartCoroutine(initpoketlist(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.winlistAPI, 0));
	}

	private IEnumerator addpoket()
	{
		if (countsize == severcount)
		{
			startclosepoket(0);
			for (int i = 0; i < poketcount.Length; i++)
			{
				poketcount[i] = 0;
				poketcount2[i] = 0;
			}
		}
		poketActivetolist(nowdata.wintype1, countsize, pokerlist);
		poketActivetolist(nowdata.wintype2, countsize, pokerlist2);
		countsize++;
		if (nowdata.wintype1 >= poketstate.spdaesJR)
		{
			poketcount[(int)nowdata.wintype1 % 4]++;
			poketcount[4]++;
		}
		else
		{
			poketcount[(int)nowdata.wintype1]++;
		}
		if (nowdata.wintype2 < poketstate.HeartsJR)
		{
			Debug.Log((int)nowdata.wintype2);
			Debug.Log(poketcount2.Length);
			try
			{
				poketcount2[(int)nowdata.wintype2]++;
			}
			catch (Exception)
			{
				throw;
			}
		}
		yield return null;
	}

	private IEnumerator betincountdown()
	{
		do
		{
			if (isupdatetime)
			{
				waittimeui = nowdata.couwttime - 1;
				changefromdataislivergame();
				isupdatetime = false;
			}
			yield return new WaitForSeconds(1f);
			coundowntext.text = waittimeui--.ToString();
		}
		while (waittimeui > 0);
	}

	private IEnumerator waitsecnd(JSYS_playstate state)
	{
		yield return new WaitForSeconds(1f);
		ChangeState = state;
	}

	private void changestate(JSYS_playstate state)
	{
		ChangeState = state;
	}

	private void startclosepoket(int countpoket)
	{
		int num = pokerlist.transform.childCount - 1;
		while (num > -1 && num != countpoket - 1)
		{
			pokerlist.transform.GetChild(num).gameObject.SetActive(false);
			num--;
		}
		int num2 = pokerlist2.transform.childCount - 1;
		while (num2 > -1 && num2 != countpoket - 1)
		{
			pokerlist2.transform.GetChild(num2).gameObject.SetActive(false);
			num2--;
		}
		for (int i = 0; i < aGrid; i++)
		{
			gridNew[i].SetActive(false);
			gridNew[i].transform.SetParent(gridNewPar);
		}
		Debug.Log(aGrid);
		aGrid = countpoket;
		countsize = countpoket;
	}

	private void poketActivetolist(poketstate poketstate, int size, GameObject poketlistGO)
	{
		if (size >= 48)
		{
			gridNew[aGrid].gameObject.SetActive(true);
			gridNew[aGrid].transform.SetParent(poketlistGO.transform);
			agr = true;
			aGrid++;
		}
		GameObject gameObject = poketGOtoinit.transform.GetChild(0).GetChild((int)poketstate).gameObject;
		gameObject.SetActive(true);
		GameObject gameObject2 = ((poketstate != 0 && poketstate != poketstate.Culb) ? poketGOtoinit.transform.GetChild(1).GetChild(1).gameObject : poketGOtoinit.transform.GetChild(1).GetChild(0).gameObject);
		if (gameObject2 != null)
		{
			gameObject2.SetActive(true);
		}
		poketlistGO.transform.GetChild(size).GetChild(0).GetComponent<Image>()
			.sprite = gameObject.GetComponent<Image>().sprite;
		poketlistGO.transform.GetChild(size).gameObject.SetActive(true);
		closepoketGO();
	}

	private void closepoketGO()
	{
		Transform child = poketGOtoinit.transform.GetChild(0);
		child.GetChild(0).gameObject.SetActive(false);
		child.GetChild(1).gameObject.SetActive(false);
		child.GetChild(2).gameObject.SetActive(false);
		child.GetChild(3).gameObject.SetActive(false);
		child.GetChild(4).gameObject.SetActive(false);
		child = poketGOtoinit.transform.GetChild(1);
		child.GetChild(0).gameObject.SetActive(false);
		child.GetChild(1).gameObject.SetActive(false);
		child.GetChild(2).gameObject.SetActive(false);
	}

	private void cleanallin()
	{
	}

	private void rangerom()
	{
		tempc = 0;
		int num = UnityEngine.Random.Range(0, 4);
		int index = UnityEngine.Random.Range(0, 5);
		tempc += listrange[num];
		if (uiallintext[index].text.Equals(string.Empty))
		{
			uiallintext[index].text = tempc.ToString();
		}
		else
		{
			uiallintext[index].text = (Convert.ToInt16(uiallintext[index].text) + tempc).ToString();
		}
	}

	private void chagerom(int value, int round)
	{
		if (uiallintext[round].text.Equals(string.Empty))
		{
			uiallintext[round].text = value.ToString();
		}
		else
		{
			uiallintext[round].text = (Convert.ToInt16(uiallintext[round].text) + value).ToString();
		}
	}

	private void showpoketcount(int[] list)
	{
		for (int i = 0; i < list.Length; i++)
		{
			uesrinscoercout[i].text = list[i].ToString();
		}
	}

	private void showpoketcountright(int[] list)
	{
		for (int i = 0; i < list.Length; i++)
		{
			userinscoercoutright[i].text = list[i].ToString();
		}
	}

	private void senderror(string value)
	{
		if (JSYS_LoginInfo.Instance().mylogindata.isOpenError)
		{
			if (goLeft != null)
			{
				UnityEngine.Object.Destroy(goLeft);
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(errorGo, error_left.transform, true);
			gameObject.transform.localPosition = Vector3.zero;
			goLeft = gameObject;
			goLeft.transform.GetChild(0).GetComponent<Text>().text = value;
			goLeft.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goLeft.transform.GetChild(0).GetComponent<Text>().preferredWidth + 10f, 100f);
		}
	}

	private void senderror_right(string value)
	{
		if (JSYS_LoginInfo.Instance().mylogindata.isOpenError)
		{
			if (goRight != null)
			{
				UnityEngine.Object.Destroy(goRight);
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(errorGo, error_right.transform, true);
			gameObject.transform.localPosition = Vector3.zero;
			goRight = gameObject;
			goRight.transform.GetChild(0).GetComponent<Text>().text = value;
			goRight.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(goRight.transform.GetChild(0).GetComponent<Text>().preferredWidth + 10f, 100f);
		}
	}

	private void addlistenrt()
	{
		buttonlist[0].onClick.AddListener(SPclick);
		buttonlist[1].onClick.AddListener(HTclick);
		buttonlist[2].onClick.AddListener(CBclick);
		buttonlist[3].onClick.AddListener(DMclick);
		buttonlist[4].onClick.AddListener(JKclick);
		buttonlist[5].onClick.AddListener(SPRclick);
		buttonlist[6].onClick.AddListener(HTRclick);
		buttonlist[7].onClick.AddListener(CBRclick);
		buttonlist[8].onClick.AddListener(DMRclick);
		cnealGO.onClick.AddListener(delegate
		{
			canelallin(JSYS_LoginInfo.Instance().mylogindata.user_id, nowdata.periods, 1);
		});
		cnealGO.onClick.AddListener(JSYS_Audiomanger._instenc.clickvoice);
		cnealGO2.onClick.AddListener(delegate
		{
			canelallin(JSYS_LoginInfo.Instance().mylogindata.user_id, nowdata.periods, 2);
		});
		cnealGO2.onClick.AddListener(JSYS_Audiomanger._instenc.clickvoice);
		quit.onClick.AddListener(tohall);
		quit.onClick.AddListener(JSYS_Audiomanger._instenc.clickvoice);
	}

	private void setUserUIinfo()
	{
		setallsroce(allScroceForUi);
	}

	private void cleanallinanduserin()
	{
		for (int i = 0; i < uiallintext.Count; i++)
		{
			uiallintext[i].text = "0";
			userscorein[i].text = "0";
		}
		historyallin = 0;
	}

	private void setallsroce(double value)
	{
		AllScroceText.text = value.ToString();
	}

	private void changeallsroce(int value)
	{
		allScroceForUi += value;
		allScroceForUi = Mathf.Clamp((float)allScroceForUi, 0f, 1E+08f);
		setallsroce(allScroceForUi);
	}

	private void returnciontologininfo(int value)
	{
		JSYS_LoginInfo.Instance().mylogindata.coindown = value;
	}

	private void btnaddclick(int value, int id, string huase)
	{
		if (NowState != JSYS_playstate.betin)
		{
			if (value <= 4)
			{
				senderror("未到下注时间");
			}
			else
			{
				senderror_right("未到下注时间");
			}
			return;
		}
		if (allScroceForUi < (double)int.Parse(JSYS_LoginInfo.Instance().mylogindata.roomcount))
		{
			changeallsroce(0);
			if (value <= 4)
			{
				senderror("余额不足或未满足最低下分要求");
			}
			else
			{
				senderror_right("余额不足或未满足最低下分要求");
			}
			return;
		}
		buttonmask[value].SetActive(true);
		try
		{
			if (allScroceForUi < 100.0 && JSYS_LoginInfo.Instance().mylogindata.coindown == 100)
			{
				StartCoroutine(betincoin(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.betinAPI + "user_id=" + JSYS_LoginInfo.Instance().mylogindata.user_id + "&num=" + allScroceForUi + "&room_id=" + JSYS_LoginInfo.Instance().mylogindata.room_id + "&drop_content=" + huase + "&id=" + id, value, allScroceForUi));
			}
			else
			{
				StartCoroutine(betincoin(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.betinAPI + "user_id=" + JSYS_LoginInfo.Instance().mylogindata.user_id + "&num=" + JSYS_LoginInfo.Instance().mylogindata.coindown + "&room_id=" + JSYS_LoginInfo.Instance().mylogindata.room_id + "&drop_content=" + huase + "&id=" + id, value, JSYS_LoginInfo.Instance().mylogindata.coindown));
			}
		}
		catch (Exception)
		{
		}
	}

	private void ConfirmToSubmit()
	{
		if (betinlist[0] <= 0 && betinlist[1] <= 0 && betinlist[2] <= 0 && betinlist[3] <= 0 && betinlist[4] <= 0)
		{
		}
	}

	private void CanelBetIn()
	{
		if (betinlist[0] > 0 || betinlist[1] > 0 || betinlist[2] > 0 || betinlist[3] > 0 || betinlist[4] > 0)
		{
			for (int i = 0; i < betinlist.Length; i++)
			{
				SubCoinToText(i, betinlist[i]);
			}
			changetextmoney(allScroceForSever);
			BetListClean();
		}
	}

	private void AddCoinToText(int TextSqueacn, int AddValue)
	{
		userscorein[TextSqueacn].text = (Convert.ToInt32(userscorein[TextSqueacn].text) + AddValue).ToString();
	}

	private void SubCoinToText(int TextSqueacn, int AddValue)
	{
		userscorein[TextSqueacn].text = (Convert.ToInt32(userscorein[TextSqueacn].text) - AddValue).ToString();
	}

	private void canelallin(string user_id, string ratedate, int plate)
	{
		string text = string.Empty;
		if (NowState != JSYS_playstate.betin)
		{
			if (plate == 1)
			{
				senderror("已超过了下注时间无法取消");
			}
			else
			{
				senderror_right("已超过了下注时间无法取消");
			}
			return;
		}
		switch (plate)
		{
		case 1:
			buttonmask[9].SetActive(true);
			text = "1";
			break;
		case 2:
			buttonmask[10].SetActive(true);
			text = "2";
			break;
		}
		try
		{
			StartCoroutine(canelbetin(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.caneldownAPI + "user_id=" + user_id + "&drop_date=" + ratedate + "&room_id=" + JSYS_LoginInfo.Instance().mylogindata.room_id + "&plate=" + text, plate));
		}
		catch (Exception)
		{
		}
	}

	private IEnumerator canelbetin(string URL, int plate)
	{
		UnityWebRequest www = UnityWebRequest.Get(URL);
		yield return www.Send();
		JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
		if (www.error == null)
		{
			if (jd["code"].ToString().Equals("200"))
			{
				for (int i = 0; i < jd["DnumList"].Count; i++)
				{
					int index = changeTYPEtointforcanel(jd["DnumList"][i]["num"].ToString());
					userscorein[index].text = jd["DnumList"][i]["user_dnum"].ToString();
				}
			}
		}
		else
		{
			StartCoroutine(canelbetin(URL, plate));
		}
		switch (plate)
		{
		case 1:
			buttonmask[9].SetActive(false);
			break;
		case 2:
			buttonmask[10].SetActive(false);
			break;
		}
	}

	private IEnumerator betincoin(string URL, int value, double invalue)
	{
		UnityWebRequest TEMP = UnityWebRequest.Get(URL);
		yield return TEMP.Send();
		if (TEMP.error != null)
		{
			yield break;
		}
		JsonData jsonData = JsonMapper.ToObject(TEMP.downloadHandler.text);
		if (jsonData["code"].ToString() == "200")
		{
			changeallsroce(-(int)invalue);
			if (userscorein[value].text.Equals(string.Empty))
			{
				userscorein[value].text = JSYS_LoginInfo.Instance().mylogindata.coindown.ToString();
			}
			else
			{
				userscorein[value].text = ((double)Convert.ToInt16(userscorein[value].text) + invalue).ToString();
			}
			if (uiallintext[value].text.Equals(string.Empty))
			{
				uiallintext[value].text = JSYS_LoginInfo.Instance().mylogindata.coindown.ToString();
			}
			else
			{
				uiallintext[value].text = ((double)Convert.ToInt16(uiallintext[value].text) + invalue).ToString();
			}
		}
		else if (value <= 4)
		{
			senderror(jsonData["msg"].ToString());
		}
		else
		{
			senderror_right(jsonData["msg"].ToString());
		}
		buttonmask[value].SetActive(false);
	}

	private void cheakifmoney()
	{
		if (allScroceForUi < 0.0)
		{
			allScroceForUi = 0.0;
			setallsroce(allScroceForUi);
		}
	}

	private void SPclick()
	{
		btnaddclick(0, buttonIDcode[0], "A");
		buttonlist[0].GetComponent<AudioSource>().Play();
	}

	private void HTclick()
	{
		btnaddclick(1, buttonIDcode[1], "B");
		buttonlist[1].GetComponent<AudioSource>().Play();
	}

	private void CBclick()
	{
		btnaddclick(2, buttonIDcode[2], "C");
		buttonlist[2].GetComponent<AudioSource>().Play();
	}

	private void DMclick()
	{
		btnaddclick(3, buttonIDcode[3], "D");
		buttonlist[3].GetComponent<AudioSource>().Play();
	}

	private void JKclick()
	{
		btnaddclick(4, buttonIDcode[4], "E");
		buttonlist[4].GetComponent<AudioSource>().Play();
	}

	private void SPRclick()
	{
		btnaddclick(5, buttonIDcode[5], "F");
		buttonlist[5].GetComponent<AudioSource>().Play();
	}

	private void HTRclick()
	{
		btnaddclick(6, buttonIDcode[6], "G");
		buttonlist[6].GetComponent<AudioSource>().Play();
	}

	private void CBRclick()
	{
		btnaddclick(7, buttonIDcode[7], "H");
		buttonlist[7].GetComponent<AudioSource>().Play();
	}

	private void DMRclick()
	{
		btnaddclick(8, buttonIDcode[8], "I");
		buttonlist[8].GetComponent<AudioSource>().Play();
	}

	public void changecointo100()
	{
		JSYS_Audiomanger._instenc.clickvoice();
		returnciontologininfo(100);
	}

	public void changecointo10()
	{
		JSYS_Audiomanger._instenc.clickvoice();
		returnciontologininfo(int.Parse(lessdown.text));
	}

	private void historyuserinlist(int coin, int round)
	{
		historyuserin[round] += coin;
		historyallin += coin;
	}

	private void cleanhistorydata()
	{
		for (int i = 0; i < uiallintext.Count; i++)
		{
			userscorein[i].text = "0";
		}
	}

	public void tohall()
	{
		StartCoroutine(loadloginscene());
	}

	private IEnumerator loadloginscene()
	{
		AsyncOperation temp = SceneManager.LoadSceneAsync(1);
		yield return temp;
		if (!temp.isDone)
		{
		}
	}

	private IEnumerator gooutroom(string url)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();
		if (www.error == null)
		{
			yield return loadloginscene();
		}
		else
		{
			senderror("返回大厅错误，请重试");
		}
	}

	private IEnumerator initpoketlist(string URL, int toggle)
	{
		lock (base.gameObject)
		{
			UnityWebRequest temp = UnityWebRequest.Get(URL);
			yield return temp.Send();
			JsonData jd = JsonMapper.ToObject(temp.downloadHandler.text);
			severpoketcode.Clear();
			severpokethuase.Clear();
			severpokethuase2.Clear();
			if (temp.error == null && temp.isDone)
			{
				if (temp.downloadHandler.text != string.Empty)
				{
					if (jd["code"].ToString() == "200" && jd["ArrList"].Count > 0)
					{
						for (int i = 0; i < jd["ArrList"].Count; i++)
						{
							severpokethuase.Add(changeTYPEtoint(jd["ArrList"][i]["win_text1"].ToString()));
							severpokethuase2.Add(changeTYPEtoint(jd["ArrList"][i]["win_text2"].ToString()));
						}
					}
				}
				else
				{
					messg.Show("异常", "无法获取以往期数，请重新进入", JSYS_ButtonStyle.Confirm, delegate(JSYS_MessageBoxResult call)
					{
						switch (call)
						{
						case JSYS_MessageBoxResult.Cancel:
							tohall();
							break;
						case JSYS_MessageBoxResult.Confirm:
							tohall();
							break;
						case JSYS_MessageBoxResult.Timeout:
							tohall();
							break;
						}
					}, 0f);
				}
			}
			else
			{
				StartCoroutine(initpoketlist(URL, toggle));
			}
			switch (toggle)
			{
			case 1:
			{
				for (int l = 0; l < poketcount.Length; l++)
				{
					poketcount[l] = 0;
				}
				for (int m = 0; m < poketcount2.Length; m++)
				{
					poketcount2[m] = 0;
				}
				changecountall(severpokethuase);
				showpoketcount(poketcount);
				changecountallright(severpokethuase2);
				showpoketcountright(poketcount2);
				for (int n = 0; n < severpokethuase.Count; n++)
				{
					poketActivetolist(severpokethuase[n], countsize, pokerlist);
					poketActivetolist(severpokethuase2[n], countsize, pokerlist2);
					countsize++;
				}
				agr = true;
				break;
			}
			case 0:
			{
				int num = ((severpokethuase.Count - countsize > 0) ? countsize : 0);
				if (num <= 0)
				{
					break;
				}
				for (int j = num; j < severpokethuase.Count; j++)
				{
					if (countsize == severcount)
					{
						startclosepoket(0);
						for (int k = 0; k < poketcount.Length; k++)
						{
							poketcount[k] = 0;
							poketcount2[k] = 0;
						}
					}
					poketActivetolist(severpokethuase[j], countsize, pokerlist);
					poketActivetolist(severpokethuase2[j], countsize, pokerlist2);
					changecountall(severpokethuase[j]);
					changecountallright(severpokethuase2[j]);
					countsize++;
				}
				erroroutgame();
				showpoketcount(poketcount);
				showpoketcountright(poketcount2);
				break;
			}
			}
		}
	}

	private IEnumerator buttomwinanimation(int ButtomSequence)
	{
		for (int i = 0; i < 5; i++)
		{
			imagechange[ButtomSequence].GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 1f);
			yield return new WaitForSeconds(0.2f);
			imagechange[ButtomSequence].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
			yield return new WaitForSeconds(0.2f);
		}
	}

	private IEnumerator starsetwinnumber(int[] winnumber)
	{
		for (int i = 0; i < winnumber.Length; i++)
		{
			JSYS_NumSpriteControl.Instances.StopImage(winnumber[i]);
			yield return new WaitForSeconds(0.5f);
		}
	}

	private void betlistadd(int sequence, int value)
	{
		if (betinlist != null)
		{
			betinlist[sequence] += value;
		}
	}

	private void BetListClean()
	{
		for (int i = 0; i < betinlist.Length; i++)
		{
			betinlist[i] = 0;
		}
	}

	private void refrshview()
	{
	}

	private void fullwindowcontorl(bool ison)
	{
		if (ison)
		{
			isfullopenwindows = true;
		}
		else
		{
			isfullopenwindows = false;
		}
	}

	private void backmusicchange(float value)
	{
		JSYS_Audiomanger._instenc.GetComponent<AudioSource>().volume = value;
	}

	private void closeclickvoice(bool isclose)
	{
		if (!isclose)
		{
		}
	}

	public int[] changestringtointlist(string temp)
	{
		string[] array = temp.Split(',');
		int[] array2 = new int[10];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = int.Parse(array[i]);
		}
		return array2;
	}

	public poketstate changeTYPEtoint(string temp)
	{
		if (temp.Equals("A1"))
		{
			return poketstate.spdaes;
		}
		if (temp.Equals("B1"))
		{
			return poketstate.Hearts;
		}
		if (temp.Equals("C1"))
		{
			return poketstate.Culb;
		}
		if (temp.Equals("D1"))
		{
			return poketstate.Diamond;
		}
		if (temp.Equals("A2"))
		{
			return poketstate.spdaesJR;
		}
		if (temp.Equals("B2"))
		{
			return poketstate.HeartsJR;
		}
		if (temp.Equals("C2"))
		{
			return poketstate.CulbJR;
		}
		if (temp.Equals("D2"))
		{
			return poketstate.DiamondJR;
		}
		return poketstate.spdaes;
	}

	private int changeTYPEtointforcanel(string temp)
	{
		if (temp.Equals("A"))
		{
			return 0;
		}
		if (temp.Equals("B"))
		{
			return 1;
		}
		if (temp.Equals("C"))
		{
			return 2;
		}
		if (temp.Equals("D"))
		{
			return 3;
		}
		if (temp.Equals("E"))
		{
			return 4;
		}
		if (temp.Equals("F"))
		{
			return 5;
		}
		if (temp.Equals("G"))
		{
			return 6;
		}
		if (temp.Equals("H"))
		{
			return 7;
		}
		if (temp.Equals("I"))
		{
			return 8;
		}
		return 0;
	}

	private void OnApplicationPause()
	{
		if (isPause)
		{
			isFocus = true;
		}
		isPause = true;
	}

	private void OnApplicationFocus()
	{
		if (isPause)
		{
			isFocus = true;
		}
		if (isFocus)
		{
			isopendatafromsever = false;
			iscomeback = true;
			isPause = false;
			isFocus = false;
		}
	}

	private void OnDestroy()
	{
		JSYS_LoginInfo.Instance().wwwinstance.listformwwwinfo.Clear();
		JSYS_LoginInfo.Instance().mylogindata.room_id = 0;
		cheak -= cheakpoketpoolisture;
	}

	private void OnShowTuiShui()
	{
		StartCoroutine(ShowTuiShui(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.userCut + "user_id=" + JSYS_LoginInfo.Instance().mylogindata.user_id));
	}

	private IEnumerator ShowTuiShui(string url)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			if (jsonData["code"].ToString() == "200")
			{
				tuiShuiRate.text = jsonData["Proportion"].ToString() + "‰";
				tuiShuiRateNum.text = jsonData["UserCut"].ToString();
				TuiShuiPanel.SetActive(true);
			}
		}
	}

	private void OnTuiShui()
	{
		Debug.Log(Convert.ToDouble(tuiShuiRateNum.text.ToString()));
		if (Convert.ToDouble(tuiShuiRateNum.text.ToString()) >= 100.0)
		{
			StartCoroutine(TuiShui(JSYS_LoginInfo.Instance().mylogindata.URL + "api/" + JSYS_LoginInfo.Instance().mylogindata.userCutSend + "user_id=" + JSYS_LoginInfo.Instance().mylogindata.user_id));
		}
		else
		{
			senderror("退水金额未达到最低标准");
		}
	}

	private IEnumerator TuiShui(string url)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			if (jsonData["code"].ToString() == "200")
			{
				tuiShuiRateNum.text = "0";
			}
			senderror("退水成功！");
		}
	}
}
