using LitJson;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SpreadPanelController : MonoBehaviour
{
	[SerializeField]
	private Button[] btns = new Button[3];

	[SerializeField]
	private GameObject[] objChoose = new GameObject[3];

	[SerializeField]
	private GameObject[] objContents = new GameObject[3];

	[SerializeField]
	private GameObject objCommission;

	[SerializeField]
	private GameObject objSourceOfPerformance;

	[SerializeField]
	private GameObject objRecord;

	[SerializeField]
	private GameObject objPerformanceOfSevenDays;

	private int curIndex;

	private int year;

	private int month;

	private int day;

	private string downDir;

	private Image imgCode;

	private Button btnSaveCode;

	private Button btnCopyId;

	private Button btnCopyLink;

	private Button btnWeChat;

	private Button btnFriend;

	private Text txtId;

	private Text txtLink;

	private AndroidJavaClass jc;

	private AndroidJavaObject jo;

	private Text txtTodayPerformance;

	private Text txtYesterdayPerformance;

	private Text txtCurPerformance;

	private Text txtGVXX;

	private Text txtDSXX;

	private Text txtHistory;

	private float todayPerformance;

	private float yesterdayPerformance;

	private float curPerformance;

	private int GVXX;

	private int DSXX;

	private float historyPerformance;

	private Button btnSourceOfPerformance;

	private Button btnPerformanceOfSevenDays;

	private Button btnRecord;

	private Button btnGet;

	private Button btnAmmountTable;

	private Text txtDate;

	private Text txtMyId;

	private Button btnToday;

	private Button btnYesterday;

	private Button btnYesyesterday;

	[SerializeField]
	private Sprite[] spiButtons = new Sprite[2];

	private Transform tfContentSP;

	private Transform tfItemPS;

	private Text txtOhterId;

	private Text txtOtherGVXX;

	private Text txtOtherDSXX;

	private Text txtTeamPerformance;

	private Text txtUnderPerformance;

	private Text txtDevote;

	private Text txtOperate;

	private int dayIndex;

	private Button btnSearch;

	private Button btnReset;

	private InputField ifID;

	private Transform[] tfItemsPSD = new Transform[7];

	private Text[] txtDatesPSD = new Text[7];

	private Text[] txtTeamNew = new Text[7];

	private Text[] txtDSNew = new Text[7];

	private Text[] txtTeamPerformancePSD = new Text[7];

	private Text[] txtDSPerformancePSD = new Text[7];

	private Text[] txtDevotePSD = new Text[7];

	private Text[] txtOperatePSD = new Text[7];

	private Button[] btnResource = new Button[7];

	private string curId;

	private Transform tfItemRecord;

	private Text txtRecordTime;

	private Text txtAmmount;

	private Transform tfContentRecord;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		Transform transform = base.transform.Find("SVButton/Viewport/Content");
		for (int i = 0; i < 3; i++)
		{
			btns[i] = transform.GetChild(i).GetComponent<Button>();
			objChoose[i] = btns[i].transform.Find("Choose").gameObject;
			objContents[i] = base.transform.Find("Content" + i.ToString()).gameObject;
			int index = i;
			btns[index].onClick.AddListener(delegate
			{
				RechargeButtonToggle(index);
			});
		}
		objCommission = base.transform.Find("Commission").gameObject;
		objSourceOfPerformance = base.transform.Find("SourceOfPerformance").gameObject;
		objRecord = base.transform.Find("Record").gameObject;
		objPerformanceOfSevenDays = base.transform.Find("PerformanceOfSevenDays").gameObject;
		year = DateTime.Now.Year;
		month = DateTime.Now.Month;
		day = DateTime.Now.Day;
		downDir = Application.persistentDataPath;
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			downDir = Application.dataPath;
		}
		InitContent0();
		InitContent1();
		InitContent2();
		InitSP();
		InitPSD();
		InitRecord();
	}

	private void OnEnable()
	{
		curIndex = 0;
		for (int i = 0; i < 3; i++)
		{
			if (i == 0)
			{
				objChoose[i].SetActive(value: true);
				objContents[i].SetActive(value: true);
			}
			else
			{
				objChoose[i].SetActive(value: false);
				objContents[i].SetActive(value: false);
			}
		}
		objCommission.SetActive(value: false);
		objSourceOfPerformance.SetActive(value: false);
		objRecord.SetActive(value: false);
		objPerformanceOfSevenDays.SetActive(value: false);
	}

	private void RechargeButtonToggle(int i)
	{
		if (curIndex != i)
		{
			MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
			objChoose[curIndex].SetActive(value: false);
			objContents[curIndex].SetActive(value: false);
			curIndex = i;
			if (curIndex == 1)
			{
				ShowContent1();
			}
			objChoose[curIndex].SetActive(value: true);
			objContents[curIndex].SetActive(value: true);
		}
	}

	private void InitContent0()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		}
		imgCode = objContents[0].transform.Find("ImgCode").GetComponent<Image>();
		btnSaveCode = objContents[0].transform.Find("BtnSaveCode").GetComponent<Button>();
		btnCopyId = objContents[0].transform.Find("BtnCopyId").GetComponent<Button>();
		btnCopyLink = objContents[0].transform.Find("BtnCopyLink").GetComponent<Button>();
		btnWeChat = objContents[0].transform.Find("BtnWeChat").GetComponent<Button>();
		btnFriend = objContents[0].transform.Find("BtnFriend").GetComponent<Button>();
		txtId = objContents[0].transform.Find("TxtId").GetComponent<Text>();
		txtLink = objContents[0].transform.Find("TxtLink").GetComponent<Text>();
		txtId.text = "我的ID： " + ZH2_GVars.user.id.ToString();
		txtLink.text = ZH2_GVars.downloadStr;
		ShowQRCode(ZH2_GVars.downloadStr, imgCode);
		btnSaveCode.onClick.AddListener(ClickBtnSaveCode);
		btnCopyId.onClick.AddListener(delegate
		{
			ClickBtnCopy(txtId);
		});
		btnCopyLink.onClick.AddListener(delegate
		{
			ClickBtnCopy(txtLink);
		});
		btnWeChat.onClick.AddListener(ClickBtnWeChat);
		btnFriend.onClick.AddListener(ClickBtnFriend);
	}

	private void ShowQRCode(string str, Image QRimage)
	{
		QRCodeMaker.QRCodeCreate(str);
		QRimage.sprite = Sprite.Create(QRCodeMaker.encoded, new Rect(0f, 0f, 512f, 512f), new Vector2(0f, 0f));
	}

	private void ClickBtnSaveCode()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		Texture2D encoded = QRCodeMaker.encoded;
		DateTime now = DateTime.Now;
		string str = $"image{year}{month}{day}{now.Hour}{now.Minute}{now.Second}.png";
		File.WriteAllBytes(downDir + "/" + str, encoded.EncodeToPNG());
		MB_Singleton<AlertDialog>.GetInstance().ShowDialog("二维码已保存");
	}

	private void ClickBtnCopy(Text text)
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		string empty = string.Empty;
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			CopyTextToClipboard.instance.OnCopy(text.text);
		}
		else
		{
			TextEditor textEditor = new TextEditor();
			textEditor.text = text.text;
			TextEditor textEditor2 = textEditor;
			textEditor2.OnFocus();
			textEditor2.Copy();
		}
		All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("复制成功", "Copy successfully", string.Empty));
	}

	private void ClickBtnWeChat()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
	}

	private void ClickBtnFriend()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
	}

	private void InitContent1()
	{
		txtTodayPerformance = objContents[1].transform.Find("Today/Txt").GetComponent<Text>();
		txtYesterdayPerformance = objContents[1].transform.Find("Yesterday/Txt").GetComponent<Text>();
		txtCurPerformance = objContents[1].transform.Find("Current/Txt").GetComponent<Text>();
		txtGVXX = objContents[1].transform.Find("Today/TxtTeam").GetComponent<Text>();
		txtDSXX = objContents[1].transform.Find("Today/TxtPerson").GetComponent<Text>();
		txtHistory = objContents[1].transform.Find("Current/TxtTip").GetComponent<Text>();
		btnSourceOfPerformance = objContents[1].transform.Find("Today/TxtLook").GetComponent<Button>();
		btnPerformanceOfSevenDays = objContents[1].transform.Find("Yesterday/TxtLook").GetComponent<Button>();
		btnRecord = objContents[1].transform.Find("Current/TxtLook").GetComponent<Button>();
		btnGet = objContents[1].transform.Find("BtnGet").GetComponent<Button>();
		btnSourceOfPerformance.onClick.AddListener(ClickBtnSourceOfPerformance);
		btnPerformanceOfSevenDays.onClick.AddListener(ClickBtnPerformanceOfSevenDays);
		btnRecord.onClick.AddListener(ClickBtnRecord);
		btnGet.onClick.AddListener(ClickBtnGet);
	}

	private void ShowContent1()
	{
	}

	private IEnumerator GetContent1Data()
	{
		string url = ZH2_GVars.IPShortLink + "ag/index?user_id=" + ZH2_GVars.user.id;
		MonoBehaviour.print(url);
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			if (jsonData["code"].ToString().Trim() == "200")
			{
				JsonData jsonData2 = jsonData["data"];
				todayPerformance = float.Parse(jsonData2["dayYj"].ToString());
				yesterdayPerformance = float.Parse(jsonData2["yesterdayYj"].ToString());
				curPerformance = float.Parse(jsonData2["nowYj"].ToString());
				GVXX = Convert.ToInt32(jsonData2["teamSum"].ToString());
				DSXX = Convert.ToInt32(jsonData2["directly"].ToString());
				historyPerformance = float.Parse(jsonData2["historyYj"].ToString());
				txtTodayPerformance.text = $"{todayPerformance:N2}";
				txtYesterdayPerformance.text = $"{yesterdayPerformance:N2}";
				txtCurPerformance.text = $"{curPerformance:N2}";
				txtGVXX.text = $"团队人数：{GVXX}";
				txtDSXX.text = $"直属人数：{DSXX}";
				txtHistory.text = $"历史已领佣金：{historyPerformance:N2}";
			}
		}
	}

	private void ClickBtnSourceOfPerformance()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		objSourceOfPerformance.SetActive(value: true);
		ResetSP();
	}

	private void ClickBtnPerformanceOfSevenDays()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		objPerformanceOfSevenDays.SetActive(value: true);
		ResetPSD();
	}

	private void ClickBtnRecord()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		objRecord.SetActive(value: true);
		ShowRecord();
	}

	private void ClickBtnGet()
	{
	}

	private void InitContent2()
	{
		btnAmmountTable = objContents[2].transform.Find("BtnCommission").GetComponent<Button>();
		btnAmmountTable.onClick.AddListener(ClickBtnAmmountTable);
	}

	private void ClickBtnAmmountTable()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		objCommission.SetActive(value: true);
	}

	private void InitSP()
	{
		Transform transform = objSourceOfPerformance.transform;
		txtDate = transform.Find("TxtDate").GetComponent<Text>();
		txtDate.text = $"日期：{year}-{month:D2}-{day:D2}";
		txtMyId = transform.Find("TxtID").GetComponent<Text>();
		txtMyId.text = $"ID:{ZH2_GVars.user.id}";
		btnToday = transform.Find("BtnToday").GetComponent<Button>();
		btnToday.image.sprite = spiButtons[0];
		btnYesterday = transform.Find("BtnYesterday").GetComponent<Button>();
		btnYesterday.image.sprite = spiButtons[1];
		btnYesyesterday = transform.Find("BtnYesyesterday").GetComponent<Button>();
		btnYesyesterday.image.sprite = spiButtons[1];
		tfContentSP = transform.Find("ScrollView/Viewport/Content");
		tfItemPS = transform.Find("Item");
		txtOhterId = tfItemPS.Find("TxtId").GetComponent<Text>();
		txtOtherGVXX = tfItemPS.Find("TxtGVXX").GetComponent<Text>();
		txtOtherDSXX = tfItemPS.Find("TxtDSXX").GetComponent<Text>();
		txtTeamPerformance = tfItemPS.Find("TxtTeamPerformance").GetComponent<Text>();
		txtUnderPerformance = tfItemPS.Find("TxtDSPerformance").GetComponent<Text>();
		txtDevote = tfItemPS.Find("TxtDevote").GetComponent<Text>();
		txtOperate = tfItemPS.Find("TxtOperate").GetComponent<Text>();
		btnToday.onClick.AddListener(ClickBtnToday);
		btnYesterday.onClick.AddListener(ClickBtnYesterday);
		btnYesyesterday.onClick.AddListener(ClickBtnYesyesterday);
		dayIndex = 0;
		RefreshSP();
	}

	private void ClearContentSP()
	{
		for (int i = 0; i < tfContentSP.childCount; i++)
		{
			UnityEngine.Object.Destroy(tfContentSP.GetChild(i).gameObject);
		}
	}

	private void ClickBtnToday()
	{
		if (dayIndex != 0)
		{
			dayIndex = 0;
			btnToday.image.sprite = spiButtons[0];
			btnYesterday.image.sprite = spiButtons[1];
			btnYesyesterday.image.sprite = spiButtons[1];
			RefreshSP();
		}
	}

	private void ClickBtnYesterday()
	{
		if (dayIndex != 1)
		{
			dayIndex = 1;
			btnToday.image.sprite = spiButtons[1];
			btnYesterday.image.sprite = spiButtons[0];
			btnYesyesterday.image.sprite = spiButtons[1];
			RefreshSP();
		}
	}

	private void ClickBtnYesyesterday()
	{
		if (dayIndex != 2)
		{
			dayIndex = 2;
			btnToday.image.sprite = spiButtons[1];
			btnYesterday.image.sprite = spiButtons[1];
			btnYesyesterday.image.sprite = spiButtons[0];
			RefreshSP();
		}
	}

	private void ResetSP()
	{
		dayIndex = 0;
		btnToday.image.sprite = spiButtons[0];
		btnYesterday.image.sprite = spiButtons[1];
		btnYesyesterday.image.sprite = spiButtons[1];
		RefreshSP();
	}

	private void RefreshSP()
	{
		ClearContentSP();
	}

	private IEnumerator GetSPData()
	{
		string url = ZH2_GVars.IPShortLink + "ag/sp?user_id=" + ZH2_GVars.user.id;
		UnityWebRequest www = new UnityWebRequest(url);
		yield return www.SendWebRequest();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			if (!(jsonData["code"].ToString().Trim() == "200"))
			{
			}
		}
	}

	private void InitPSD()
	{
		btnSearch = objPerformanceOfSevenDays.transform.Find("BtnSearch").GetComponent<Button>();
		btnReset = objPerformanceOfSevenDays.transform.Find("BtnReset").GetComponent<Button>();
		ifID = objPerformanceOfSevenDays.transform.Find("InputField").GetComponent<InputField>();
		Transform transform = objPerformanceOfSevenDays.transform.Find("ScrollView/Viewport/Content");
		for (int i = 0; i < 7; i++)
		{
			tfItemsPSD[i] = transform.GetChild(i);
			txtDatesPSD[i] = tfItemsPSD[i].GetChild(0).GetComponent<Text>();
			txtDatesPSD[i].text = $"{year}-{month:D2}-{day - 1 - i:D2}";
			txtTeamNew[i] = tfItemsPSD[i].GetChild(1).GetComponent<Text>();
			txtDSNew[i] = tfItemsPSD[i].GetChild(2).GetComponent<Text>();
			txtTeamPerformancePSD[i] = tfItemsPSD[i].GetChild(3).GetComponent<Text>();
			txtDSPerformancePSD[i] = tfItemsPSD[i].GetChild(4).GetComponent<Text>();
			txtDevotePSD[i] = tfItemsPSD[i].GetChild(5).GetComponent<Text>();
			txtOperatePSD[i] = tfItemsPSD[i].GetChild(6).GetComponent<Text>();
			btnResource[i] = tfItemsPSD[i].GetChild(7).GetComponent<Button>();
		}
		btnSearch.onClick.AddListener(ClickBtnSearch);
		btnReset.onClick.AddListener(ClickBtnReset);
	}

	private void ClickBtnSearch()
	{
		curId = ifID.text;
	}

	private void ClickBtnReset()
	{
		ResetPSD();
	}

	private void ClickBtnResource(int index)
	{
		objPerformanceOfSevenDays.SetActive(value: false);
		txtDate.text = $"日期：{year}-{month:D2}-{day - 1 - index:D2}";
		btnToday.image.sprite = spiButtons[1];
		btnYesterday.image.sprite = spiButtons[(index != 0) ? 1 : 0];
		btnYesyesterday.image.sprite = spiButtons[(index != 1) ? 1 : 0];
		RefreshSP();
		objSourceOfPerformance.SetActive(value: true);
	}

	private void ResetPSD()
	{
		ifID.text = string.Empty;
		curId = ZH2_GVars.user.id.ToString();
	}

	private IEnumerator GetPSDData()
	{
		string url = string.Empty;
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			if (!(jsonData["code"].ToString().Trim() == "200"))
			{
			}
		}
	}

	private void InitRecord()
	{
		tfItemRecord = objRecord.transform.Find("Item");
		txtRecordTime = tfItemRecord.transform.Find("TxtTime").GetComponent<Text>();
		txtAmmount = tfItemRecord.transform.Find("TxtAmmount").GetComponent<Text>();
		tfContentRecord = objRecord.transform.Find("ScrollView/Viewport/Content");
	}

	private void ShowRecord()
	{
		ClearContentRecord();
	}

	private void ClearContentRecord()
	{
		for (int i = 0; i < tfContentRecord.childCount; i++)
		{
			UnityEngine.Object.Destroy(tfContentRecord.GetChild(i).gameObject);
		}
	}

	private IEnumerator GetRecordData()
	{
		string url = string.Empty;
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			if (!(jsonData["code"].ToString().Trim() == "200"))
			{
			}
		}
	}
}
