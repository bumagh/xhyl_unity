using DG.Tweening;
using SuperScrollView;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpreadPanelNewController : MonoBehaviour
{
	private List<Button> buttons = new List<Button>();

	private Transform leftShow;

	public List<GameObject> content = new List<GameObject>();

	public Sprite[] icos;

	public Sprite[] timePanelIco;

	private Transform contentRightShow;

	private Button btnCopyLink;

	private Button btnFenXiang;

	private Button content1BtnRight;

	private Button content1BtnLeft;

	private Button content1BtnShow;

	private Button buttoncCeate;

	private Button buttonCaoZuo;

	private Button fenRunBtn;

	private Button beiZhuBtn;

	private Button shanChuBtn;

	private Text txtId;

	private Text txtTJId;

	private Text txtLink;

	private Image imgCode;

	private Image content1Image;

	private GameObject caoZuo;

	private GameObject fenRunPanel;

	private GameObject beiZhuPanel;

	private Text beiZhuTxt;

	private InputField beiZhuInput;

	private Button beiZhuBtnSure;

	private Button content2BtnLeft;

	private Button content2BtnRight;

	private GameObject content2Cnt0;

	private GameObject content2Cnt1;

	private Button content5BtnLeft;

	private Button content5BtnRight;

	private GameObject content5Cnt0;

	private GameObject content5Cnt1;

	private Button content6BtnLeft;

	private Button content6BtnRight;

	private Transform TimePanel;

	private Button timePanelBtnSuer;

	private AndroidJavaObject jo;

	private AndroidJavaClass jc;

	private Button Content3BtnStart;

	private Button Content3BtnEnd;

	private Button Content4BtnStart;

	private Button Content4BtnEnd;

	private Button Content5BtnStart;

	private Button Content5BtnEnd;

	private Button Content6BtnStart;

	private Button Content6BtnEnd;

	private Text Content3TxtStart;

	private Text Content3TxtEnd;

	private Text Content4TxtStart;

	private Text Content4TxtEnd;

	private Text Content5TxtStart;

	private Text Content5TxtEnd;

	private Text Content6TxtStart;

	private Text Content6TxtEnd;

	private List<Text> ContentTxtStart = new List<Text>();

	private List<Text> ContentTxtEnd = new List<Text>();

	private int timeSelectNum;

	private bool isTimeSelectEnd;

	private bool isRight;

	private SpinDatePickerDemoScript timePanelData;

	private void Awake()
	{
		leftShow = base.transform.Find("SVButton/Viewport/Content");
		contentRightShow = base.transform.Find("Content0/RankGrid/Viewport/Grid");
		txtLink = contentRightShow.Find("LinkPanel/TxtLink").GetComponent<Text>();
		beiZhuTxt = contentRightShow.Find("LinkPanel/TxtBZ").GetComponent<Text>();
		btnCopyLink = contentRightShow.Find("LinkPanel/BtnCopyLink").GetComponent<Button>();
		btnCopyLink.onClick.AddListener(delegate
		{
			ClickBtnCopy(txtLink);
		});
		btnFenXiang = contentRightShow.Find("LinkPanel/ButtonFenXiang").GetComponent<Button>();
		buttonCaoZuo = contentRightShow.Find("LinkPanel/ButtonCaoZuo").GetComponent<Button>();
		btnFenXiang.onClick.AddListener(FenXiangOnClick);
		buttonCaoZuo.onClick.AddListener(delegate
		{
			CaoZuoOnClick(0);
		});
		txtId = base.transform.Find("Content0/TxtId").GetComponent<Text>();
		txtTJId = base.transform.Find("Content0/TxtTJId").GetComponent<Text>();
		imgCode = base.transform.Find("Content0/Code/ImgCode").GetComponent<Image>();
		buttoncCeate = base.transform.Find("Content0/ButtoncCeate").GetComponent<Button>();
		buttoncCeate.onClick.AddListener(BtnCaeateOnClick);
		caoZuo = base.transform.Find("Content0/CaoZuo").gameObject;
		fenRunBtn = caoZuo.transform.Find("bg/FenRun").GetComponent<Button>();
		beiZhuBtn = caoZuo.transform.Find("bg/BeiZhu").GetComponent<Button>();
		shanChuBtn = caoZuo.transform.Find("bg/ShanChu").GetComponent<Button>();
		fenRunBtn.onClick.AddListener(delegate
		{
			CaoZuoOnClick(1);
		});
		beiZhuBtn.onClick.AddListener(delegate
		{
			CaoZuoOnClick(2);
		});
		shanChuBtn.onClick.AddListener(delegate
		{
			CaoZuoOnClick(3);
		});
		fenRunPanel = base.transform.Find("Content0/FenRunPanel").gameObject;
		beiZhuPanel = base.transform.Find("Content0/BeiZhuPanel").gameObject;
		beiZhuInput = beiZhuPanel.transform.Find("bg/InputField").GetComponent<InputField>();
		beiZhuBtnSure = beiZhuPanel.transform.Find("bg/Sure").GetComponent<Button>();
		beiZhuBtnSure.onClick.AddListener(BeiZhuBtnOnClick);
		content1Image = base.transform.Find("Content1/Image").GetComponent<Image>();
		content1BtnRight = content1Image.transform.Find("ButtonRight").GetComponent<Button>();
		content1BtnLeft = content1Image.transform.Find("ButtonLeft").GetComponent<Button>();
		content1BtnShow = content1Image.transform.Find("BtnCommission").GetComponent<Button>();
		content1BtnLeft.onClick.AddListener(LeftBtnOnClick);
		content1BtnRight.onClick.AddListener(LeftBtnOnClick);
		content2BtnLeft = base.transform.Find("Content2/Button0").GetComponent<Button>();
		content2BtnRight = base.transform.Find("Content2/Button1").GetComponent<Button>();
		content2Cnt0 = base.transform.Find("Content2/Content0").gameObject;
		content2Cnt1 = base.transform.Find("Content2/Content1").gameObject;
		content2BtnLeft.onClick.AddListener(delegate
		{
			Content2OnClick(0);
		});
		content2BtnRight.onClick.AddListener(delegate
		{
			Content2OnClick(1);
		});
		content5BtnLeft = base.transform.Find("Content5/Button0").GetComponent<Button>();
		content5BtnRight = base.transform.Find("Content5/Button1").GetComponent<Button>();
		content5Cnt0 = base.transform.Find("Content5/Content0").gameObject;
		content5Cnt1 = base.transform.Find("Content5/Content1").gameObject;
		content5BtnLeft.onClick.AddListener(delegate
		{
			Content5OnClick(0);
		});
		content5BtnRight.onClick.AddListener(delegate
		{
			Content5OnClick(1);
		});
		content6BtnLeft = base.transform.Find("Content6/Button0").GetComponent<Button>();
		content6BtnRight = base.transform.Find("Content6/Button1").GetComponent<Button>();
		content6BtnLeft.onClick.AddListener(delegate
		{
			Content6OnClick(0);
		});
		content6BtnRight.onClick.AddListener(delegate
		{
			Content6OnClick(1);
		});
		TimePanel = base.transform.Find("TimePanel");
		timePanelBtnSuer = TimePanel.Find("Sure").GetComponent<Button>();
		timePanelBtnSuer.onClick.AddListener(TimePanelBtnSuer);
		TimePanel.GetComponent<Image>().sprite = timePanelIco[0];
		InitTimeBtnAndTxt();
		content1Image.sprite = icos[0];
		content1BtnLeft.enabled = false;
		content1BtnRight.enabled = true;
		content1BtnShow.enabled = false;
		caoZuo.SetActive(value: false);
		fenRunPanel.SetActive(value: false);
		beiZhuPanel.SetActive(value: false);
		txtId.text = ZH2_GVars.user.id.ToString();
		txtLink.text = ZH2_GVars.downloadStr;
		txtTJId.text = "88888";
		if (Application.platform == RuntimePlatform.Android)
		{
			jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		}
	}

	private void Start()
	{
		for (int i = 0; i < leftShow.childCount; i++)
		{
			buttons.Add(leftShow.GetChild(i).GetComponent<Button>());
			LevelButtonEvent component = buttons[i].GetComponent<LevelButtonEvent>();
			component.id = i;
			component.onLevelButtonOnClick += LevelButtonNum_ButtonOnClick;
		}
		for (int j = 0; j < content.Count; j++)
		{
			content[j].SetActive(value: false);
		}
		content[0].SetActive(value: true);
		content2BtnLeft.transform.GetChild(0).gameObject.SetActive(value: true);
		content2BtnRight.transform.GetChild(0).gameObject.SetActive(value: false);
		content2Cnt0.SetActive(value: true);
		content2Cnt1.SetActive(value: false);
		content5BtnLeft.transform.GetChild(0).gameObject.SetActive(value: true);
		content5BtnRight.transform.GetChild(0).gameObject.SetActive(value: false);
		content5Cnt0.SetActive(value: true);
		content5Cnt1.SetActive(value: false);
		content6BtnLeft.transform.GetChild(0).gameObject.SetActive(value: true);
		content6BtnRight.transform.GetChild(0).gameObject.SetActive(value: false);
	}

	private void InitTimeBtnAndTxt()
	{
		Content3BtnStart = base.transform.Find("Content3/ButtonStrat").GetComponent<Button>();
		Content3BtnEnd = base.transform.Find("Content3/ButtonEnd").GetComponent<Button>();
		Content4BtnStart = base.transform.Find("Content4/ButtonStrat").GetComponent<Button>();
		Content4BtnEnd = base.transform.Find("Content4/ButtonEnd").GetComponent<Button>();
		Content5BtnStart = base.transform.Find("Content5/ButtonStrat").GetComponent<Button>();
		Content5BtnEnd = base.transform.Find("Content5/ButtonEnd").GetComponent<Button>();
		Content6BtnStart = base.transform.Find("Content6/ButtonStrat").GetComponent<Button>();
		Content6BtnEnd = base.transform.Find("Content6/ButtonEnd").GetComponent<Button>();
		Content3TxtStart = Content3BtnStart.transform.GetChild(0).GetComponent<Text>();
		Content3TxtEnd = Content3BtnEnd.transform.GetChild(0).GetComponent<Text>();
		Content4TxtStart = Content4BtnStart.transform.GetChild(0).GetComponent<Text>();
		Content4TxtEnd = Content4BtnEnd.transform.GetChild(0).GetComponent<Text>();
		Content5TxtStart = Content5BtnStart.transform.GetChild(0).GetComponent<Text>();
		Content5TxtEnd = Content5BtnEnd.transform.GetChild(0).GetComponent<Text>();
		Content6TxtStart = Content6BtnStart.transform.GetChild(0).GetComponent<Text>();
		Content6TxtEnd = Content6BtnEnd.transform.GetChild(0).GetComponent<Text>();
		ContentTxtStart.Add(Content3TxtStart);
		ContentTxtStart.Add(Content4TxtStart);
		ContentTxtStart.Add(Content5TxtStart);
		ContentTxtStart.Add(Content6TxtStart);
		ContentTxtEnd.Add(Content3TxtEnd);
		ContentTxtEnd.Add(Content4TxtEnd);
		ContentTxtEnd.Add(Content5TxtEnd);
		ContentTxtEnd.Add(Content6TxtEnd);
		Content3BtnStart.onClick.AddListener(delegate
		{
			ContentBtnClick(3, isEnd: false);
		});
		Content4BtnStart.onClick.AddListener(delegate
		{
			ContentBtnClick(4, isEnd: false);
		});
		Content5BtnStart.onClick.AddListener(delegate
		{
			ContentBtnClick(5, isEnd: false);
		});
		Content6BtnStart.onClick.AddListener(delegate
		{
			ContentBtnClick(6, isEnd: false);
		});
		Content3BtnEnd.onClick.AddListener(delegate
		{
			ContentBtnClick(3, isEnd: true);
		});
		Content4BtnEnd.onClick.AddListener(delegate
		{
			ContentBtnClick(4, isEnd: true);
		});
		Content5BtnEnd.onClick.AddListener(delegate
		{
			ContentBtnClick(5, isEnd: true);
		});
		Content6BtnEnd.onClick.AddListener(delegate
		{
			ContentBtnClick(6, isEnd: true);
		});
		for (int i = 0; i < ContentTxtEnd.Count; i++)
		{
			ContentTxtEnd[i].text = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
			ContentTxtStart[i].text = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
		}
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

	private void LeftBtnOnClick()
	{
		isRight = !isRight;
		content1Image.sprite = icos[isRight ? 1 : 0];
		content1BtnLeft.enabled = isRight;
		content1BtnShow.enabled = isRight;
		content1BtnRight.enabled = !isRight;
	}

	private void BeiZhuBtnOnClick()
	{
		if (beiZhuInput.text == string.Empty || beiZhuInput.text.Length > 7)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please check the number of notes!" : ((ZH2_GVars.language_enum != 0) ? "โปรดตรวจสอบหมายเลขบ\u0e31นท\u0e36ก" : "请检查备注字数"));
			return;
		}
		beiZhuTxt.text = beiZhuInput.text;
		beiZhuPanel.SetActive(value: false);
	}

	private void BtnCaeateOnClick()
	{
		MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Your own bonus has been set, please contact your superior" : ((ZH2_GVars.language_enum != 0) ? "โปรดต\u0e34ดต\u0e48อผ\u0e39\u0e49บ\u0e31งค\u0e31บบ\u0e31ญชา" : "自身分润未被设定,请联系上级"));
	}

	private void FenXiangOnClick()
	{
		imgCode.transform.parent.gameObject.SetActive(value: true);
		imgCode.transform.localScale = Vector3.zero;
		imgCode.transform.DOScale(Vector3.one, 0.35f);
		imgCode.transform.DOLocalRotate(new Vector3(0f, 0f, 360f), 0.5f, RotateMode.WorldAxisAdd);
		ShowQRCode(ZH2_GVars.downloadStr, imgCode);
	}

	private void Content2OnClick(int num)
	{
		switch (num)
		{
		case 0:
			content2BtnLeft.transform.GetChild(0).gameObject.SetActive(value: true);
			content2BtnRight.transform.GetChild(0).gameObject.SetActive(value: false);
			content2Cnt0.SetActive(value: true);
			content2Cnt1.SetActive(value: false);
			break;
		case 1:
			content2BtnLeft.transform.GetChild(0).gameObject.SetActive(value: false);
			content2BtnRight.transform.GetChild(0).gameObject.SetActive(value: true);
			content2Cnt0.SetActive(value: false);
			content2Cnt1.SetActive(value: true);
			break;
		}
	}

	private void Content5OnClick(int num)
	{
		switch (num)
		{
		case 0:
			content5BtnLeft.transform.GetChild(0).gameObject.SetActive(value: true);
			content5BtnRight.transform.GetChild(0).gameObject.SetActive(value: false);
			content5Cnt0.SetActive(value: true);
			content5Cnt1.SetActive(value: false);
			break;
		case 1:
			content5BtnLeft.transform.GetChild(0).gameObject.SetActive(value: false);
			content5BtnRight.transform.GetChild(0).gameObject.SetActive(value: true);
			content5Cnt0.SetActive(value: false);
			content5Cnt1.SetActive(value: true);
			break;
		}
	}

	private void Content6OnClick(int num)
	{
		switch (num)
		{
		case 0:
			content6BtnLeft.transform.GetChild(0).gameObject.SetActive(value: true);
			content6BtnRight.transform.GetChild(0).gameObject.SetActive(value: false);
			break;
		case 1:
			content6BtnLeft.transform.GetChild(0).gameObject.SetActive(value: false);
			content6BtnRight.transform.GetChild(0).gameObject.SetActive(value: true);
			break;
		}
	}

	private void ContentBtnClick(int num, bool isEnd)
	{
		timeSelectNum = num - 3;
		isTimeSelectEnd = isEnd;
		TimePanel.gameObject.SetActive(value: true);
		if (isEnd)
		{
			TimePanel.GetComponent<Image>().sprite = timePanelIco[1];
		}
		else
		{
			TimePanel.GetComponent<Image>().sprite = timePanelIco[0];
		}
	}

	private void TimePanelBtnSuer()
	{
		TimePanel.gameObject.SetActive(value: false);
		timePanelData = TimePanel.GetComponent<SpinDatePickerDemoScript>();
		if (isTimeSelectEnd)
		{
			ContentTxtEnd[timeSelectNum].text = timePanelData.Year + "-" + timePanelData.Month + "-" + timePanelData.Day;
		}
		else
		{
			ContentTxtStart[timeSelectNum].text = timePanelData.Year + "-" + timePanelData.Month + "-" + timePanelData.Day;
		}
	}

	private void CaoZuoOnClick(int num)
	{
		switch (num)
		{
		case 0:
			caoZuo.SetActive(value: true);
			caoZuo.transform.Find("bg").localScale = Vector3.zero;
			caoZuo.transform.Find("bg").DOScale(Vector3.one, 0.35f);
			break;
		case 1:
			caoZuo.SetActive(value: false);
			fenRunPanel.SetActive(value: true);
			break;
		case 2:
			caoZuo.SetActive(value: false);
			beiZhuPanel.SetActive(value: true);
			break;
		case 3:
			caoZuo.SetActive(value: false);
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Do you want to delete this link" : ((ZH2_GVars.language_enum != 0) ? "ค\u0e38ณต\u0e49องการจะลบ ท\u0e35\u0e48อย\u0e39\u0e48เช\u0e37\u0e48อมโยงน\u0e35\u0e49 หร\u0e37อไม\u0e48?" : "是否要删除此条链接"), showOkCancel: true, DeleteLink);
			break;
		}
	}

	private void DeleteLink()
	{
		UnityEngine.Debug.LogError("删除");
		if (contentRightShow.childCount > 0)
		{
			UnityEngine.Object.Destroy(contentRightShow.GetChild(0).gameObject);
		}
	}

	private void ShowQRCode(string str, Image QRimage)
	{
		QRCodeMaker.QRCodeCreate(str);
		QRimage.sprite = Sprite.Create(QRCodeMaker.encoded, new Rect(0f, 0f, 512f, 512f), new Vector2(0f, 0f));
	}

	private void LevelButtonNum_ButtonOnClick(int obj)
	{
		UnityEngine.Debug.Log("点击左第 " + obj + " 个按钮");
		for (int i = 0; i < buttons.Count; i++)
		{
			buttons[i].transform.GetChild(0).gameObject.SetActive(value: false);
		}
		buttons[obj].transform.GetChild(0).gameObject.SetActive(value: true);
		for (int j = 0; j < content.Count; j++)
		{
			content[j].SetActive(value: false);
		}
		content[obj].SetActive(value: true);
	}

	private void Update()
	{
	}
}
