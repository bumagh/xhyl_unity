using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AccountsPanelController : MonoBehaviour
{
	private Transform rightShow;

	private Transform recordShow;

	private List<Button> buttons = new List<Button>();

	private Text upNun;

	private Text upName;

	private Text middelNum;

	private Text middelName;

	private Button changeButton;

	private Button sureButton;

	private Button checkRecordButton;

	private Button recordButtonClose;

	private Button recordButtonLeft;

	private Button recordButtonRight;

	private InputField inputNumText;

	private Transform RecordPanel;

	public GameObject RecordPre;

	private List<GameObject> Record_List = new List<GameObject>();

	private GameObject loadAnim;

	private Text agTextMoney;

	private Text kyTextMoney;

	private Text dianZiTextMoney;

	private Text recordTextTag;

	private Text recordTextNum;

	private bool isChange;

	private int totalPage = 1;

	private int currentPage = 1;

	private string upStr = "AG真人";

	private string midStr = "我的钱包";

	private string upNumStr = "0.00";

	private string midNumStr = "0.00";

	private string duiHuanUrl = "http://dhyl.swccd88.xyz:81/Aeqw1FOd312s.php/index/duihuan/dh";

	private string duiHuanJiLu = "http://dhyl.swccd88.xyz:81/Aeqw1FOd312s.php/index/duihuan/duihuanRecord";

	private string CheckAmountUrl = "http://dhyl.swccd88.xyz:81/Aeqw1FOd312s.php/index/duihuan/userBalance";

	private void Awake()
	{
		duiHuanUrl = "http://" + ZH2_GVars.IPAddress + ":81/Aeqw1FOd312s.php/index/duihuan/dh";
		duiHuanJiLu = "http://" + ZH2_GVars.IPAddress + ":81/Aeqw1FOd312s.php/index/duihuan/duihuanRecord";
		CheckAmountUrl = "http://" + ZH2_GVars.IPAddress + ":81/Aeqw1FOd312s.php/index/duihuan/userBalance";
		rightShow = base.transform.Find("Rank_bg/RankGrid/Viewport/Grid");
		agTextMoney = rightShow.transform.Find("AG真人/text").GetComponent<Text>();
		kyTextMoney = rightShow.transform.Find("开元棋牌/text").GetComponent<Text>();
		dianZiTextMoney = rightShow.transform.Find("星力电子/text").GetComponent<Text>();
		upNun = base.transform.Find("Rank_bg/LeftPanel/up/num").GetComponent<Text>();
		upName = base.transform.Find("Rank_bg/LeftPanel/up/upName").GetComponent<Text>();
		middelNum = base.transform.Find("Rank_bg/LeftPanel/middel/num").GetComponent<Text>();
		middelName = base.transform.Find("Rank_bg/LeftPanel/middel/middelName").GetComponent<Text>();
		changeButton = base.transform.Find("Rank_bg/LeftPanel/changeButton").GetComponent<Button>();
		changeButton.onClick.AddListener(ButtonOnClick_Change);
		checkRecordButton = base.transform.Find("Rank_bg/RecordButton").GetComponent<Button>();
		checkRecordButton.onClick.AddListener(ButtonOnClick_Record);
		inputNumText = base.transform.Find("Rank_bg/LeftPanel/InputField").GetComponent<InputField>();
		sureButton = base.transform.Find("Rank_bg/LeftPanel/sureButton").GetComponent<Button>();
		sureButton.onClick.AddListener(ButtonOnClick_Sure);
		loadAnim = base.transform.Find("LoadAnim").gameObject;
		loadAnim.SetActive(value: false);
		RecordPanel = base.transform.Find("Rank_bg/RecordPanel");
		recordShow = RecordPanel.Find("Viewport/Grid");
		recordButtonClose = RecordPanel.Find("Close").GetComponent<Button>();
		recordButtonClose.onClick.AddListener(ButtonOnClick_Close);
		recordButtonLeft = RecordPanel.Find("ButtonLeft").GetComponent<Button>();
		recordButtonRight = RecordPanel.Find("ButtonRight").GetComponent<Button>();
		recordTextTag = RecordPanel.Find("TextTag").GetComponent<Text>();
		recordTextNum = RecordPanel.Find("TextNum").GetComponent<Text>();
		recordButtonLeft.onClick.AddListener(ButtonOnClick_L);
		recordButtonRight.onClick.AddListener(ButtonOnClick_R);
	}

	private void OnEnable()
	{
		base.transform.Find("Rank_bg").localScale = Vector3.zero;
		base.transform.Find("Rank_bg").DOScale(Vector3.one, 0.2f);
		upName.text = "AG真人";
		middelName.text = "我的钱包";
		inputNumText.text = string.Empty;
		RecordPanel.gameObject.SetActive(value: false);
		upNumStr = agTextMoney.text;
		UpdateMoney();
		recordTextTag.text = "01/01";
	}

	private void Start()
	{
		for (int i = 0; i < rightShow.childCount; i++)
		{
			buttons.Add(rightShow.GetChild(i).GetComponent<Button>());
			LevelButtonEvent component = buttons[i].GetComponent<LevelButtonEvent>();
			component.id = i;
			component.onLevelButtonOnClick += LevelButtonNum_onLevelButtonOnClick;
		}
	}

	private void UpdateMoney()
	{
		loadAnim.SetActive(value: true);
		StartCoroutine(GetPlatformAmount_AG(CheckAmountUrl));
		StartCoroutine(GetPlatformAmount_KY(CheckAmountUrl));
		StartCoroutine(GetPlatformAmount_DZ(CheckAmountUrl));
	}

	private IEnumerator GetPlatformAmount_AG(string url)
	{
		url = url + "?username=" + ZH2_GVars.AccountName + "&game=ag";
		UnityEngine.Debug.LogError("链接:" + url);
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			try
			{
				agTextMoney.text = jsonData["data"].ToString();
				if (upName.text == "AG真人" || middelName.text == "AG真人")
				{
					upNumStr = agTextMoney.text;
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
		}
	}

	private IEnumerator GetPlatformAmount_KY(string url)
	{
		url = url + "?username=" + ZH2_GVars.AccountName + "&game=ky";
		UnityEngine.Debug.LogError("链接:" + url);
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			try
			{
				kyTextMoney.text = jsonData["data"].ToString();
				if (upName.text == "开元棋牌" || middelName.text == "开元棋牌")
				{
					upNumStr = kyTextMoney.text;
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
		}
	}

	private IEnumerator GetPlatformAmount_DZ(string url)
	{
		url = url + "?username=" + ZH2_GVars.AccountName + "&game=jdb";
		UnityEngine.Debug.LogError("链接:" + url);
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			try
			{
				dianZiTextMoney.text = jsonData["data"].ToString();
				if (upName.text == "星力电子" || middelName.text == "星力电子")
				{
					upNumStr = dianZiTextMoney.text;
				}
				loadAnim.SetActive(value: false);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
		}
	}

	private IEnumerator GetRecordPanel(string url)
	{
		loadAnim.SetActive(value: true);
		url = url + "?username=" + ZH2_GVars.AccountName;
		UnityEngine.Debug.LogError("链接:" + url);
		Record_List = new List<GameObject>();
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			UnityEngine.Debug.LogError("转账记录: " + jsonData.ToJson());
			try
			{
				for (int i = 0; i < int.Parse(jsonData["count"].ToString()); i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(RecordPre);
					gameObject.transform.parent = recordShow;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.Find("text0").GetComponent<Text>().text = jsonData["data"][i]["money"].ToString();
					gameObject.transform.Find("text1").GetComponent<Text>().text = jsonData["data"][i]["plat_type"].ToString();
					gameObject.transform.Find("text2").GetComponent<Text>().text = jsonData["data"][i]["last_money"].ToString();
					gameObject.transform.Find("text3").GetComponent<Text>().text = jsonData["data"][i]["after_money"].ToString();
					gameObject.transform.Find("text4").GetComponent<Text>().text = jsonData["data"][i]["pay_time"].ToString();
					Record_List.Add(gameObject);
				}
				loadAnim.SetActive(value: false);
				currentPage = 1;
				RecordPaging();
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
		}
	}

	private void RecordPaging()
	{
		totalPage = Record_List.Count / 10 + 1;
		recordTextTag.text = currentPage.ToString("00") + "/" + totalPage.ToString("00");
		recordTextNum.text = (currentPage * 10 - 9).ToString("00") + "~" + (currentPage * 10).ToString("00") + "条";
		for (int i = 0; i < Record_List.Count; i++)
		{
			if (i >= currentPage * 10 || i < currentPage * 10 - 10)
			{
				Record_List[i].gameObject.SetActive(value: false);
			}
			else
			{
				Record_List[i].gameObject.SetActive(value: true);
			}
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < Record_List.Count; i++)
		{
			UnityEngine.Object.Destroy(Record_List[i].gameObject);
		}
	}

	private void ButtonOnClick_Change()
	{
		isChange = !isChange;
	}

	private void ButtonOnClick_Close()
	{
		for (int i = 0; i < Record_List.Count; i++)
		{
			UnityEngine.Object.Destroy(Record_List[i].gameObject);
		}
	}

	private void ButtonOnClick_Record()
	{
		StartCoroutine(GetRecordPanel(duiHuanJiLu));
		RecordPanel.gameObject.SetActive(value: true);
	}

	private void ButtonOnClick_L()
	{
		RecordPaging();
		currentPage++;
		if (currentPage > totalPage)
		{
			currentPage = 1;
		}
		RecordPaging();
	}

	private void ButtonOnClick_R()
	{
		RecordPaging();
		currentPage--;
		if (currentPage < 1)
		{
			currentPage = totalPage;
		}
		RecordPaging();
	}

	private void ButtonOnClick_Sure()
	{
		if (inputNumText.text == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please enter the transfer amount!" : ((ZH2_GVars.language_enum != 0) ? "กร\u0e38ณาพ\u0e34มพ\u0e4cเง\u0e34นโอน!" : "请输入转账金额!"));
			return;
		}
		if (float.Parse(inputNumText.text) <= 0f)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "The amount transferred is wrong!" : ((ZH2_GVars.language_enum != 0) ? "การโอนเง\u0e34นผ\u0e34ดพลาด!" : "转账金额有误!"));
			return;
		}
		if (upNun.text == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Unknown Error!" : ((ZH2_GVars.language_enum != 0) ? "ไม\u0e48ทราบข\u0e49อผ\u0e34ดพลาดโปรดต\u0e34ดต\u0e48อฝ\u0e48ายบร\u0e34การล\u0e39กค\u0e49า !" : "未知错误,请联系客服!"));
			return;
		}
		if (float.Parse(inputNumText.text) > float.Parse(upNun.text))
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "The transfer amount is greater than the game currency balance!" : ((ZH2_GVars.language_enum != 0) ? "เง\u0e34นโอนมากกว\u0e48า ยอดเง\u0e34นในเกม!" : "转账金额大于游戏币余额!"));
			return;
		}
		string type = string.Empty;
		string empty = string.Empty;
		if (upName.text != "我的钱包")
		{
			switch (upName.text)
			{
			case "AG真人":
				type = "ag";
				break;
			case "开元棋牌":
				type = "ky";
				break;
			case "星力电子":
				type = "jdb";
				break;
			}
			empty = "-" + inputNumText.text;
		}
		else
		{
			switch (middelName.text)
			{
			case "AG真人":
				type = "ag";
				break;
			case "开元棋牌":
				type = "ky";
				break;
			case "星力电子":
				type = "jdb";
				break;
			}
			empty = inputNumText.text;
		}
		loadAnim.SetActive(value: true);
		StartCoroutine(GetDuiHuan(duiHuanUrl, type, empty));
	}

	private IEnumerator GetDuiHuan(string url, string type, string money)
	{
		string text = url;
		url = text + "?username=" + ZH2_GVars.AccountName + "&game=" + type + "&money=" + money;
		UnityEngine.Debug.LogError("链接:" + url);
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			try
			{
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog(jsonData["message"].ToString());
				loadAnim.SetActive(value: false);
				UpdateMoney();
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("     " + arg);
				throw;
			}
		}
	}

	private void LevelButtonNum_onLevelButtonOnClick(int obj)
	{
		UnityEngine.Debug.Log("点击第 " + obj + " 个按钮");
		upStr = buttons[obj].name;
		upNumStr = buttons[obj].transform.GetChild(0).GetComponent<Text>().text;
		midNumStr = ZH2_GVars.user.gameGold.ToString("0.00");
	}

	public void OnBtnClick_Close()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		UnityEngine.Debug.Log(base.gameObject.name + ">>OnBtnClick_Close:");
		base.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		if (midNumStr != null)
		{
			midNumStr = ZH2_GVars.user.gameGold.ToString("0.00");
		}
		if (isChange)
		{
			upName.text = midStr;
			middelName.text = upStr;
			upNun.text = midNumStr;
			middelNum.text = upNumStr;
		}
		else
		{
			upName.text = upStr;
			middelName.text = midStr;
			upNun.text = upNumStr;
			middelNum.text = midNumStr;
		}
	}
}
