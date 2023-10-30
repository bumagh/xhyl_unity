using UnityEngine;
using UnityEngine.UI;

public class ShareCodeController : MonoBehaviour
{
	private Text[] txtTable;

	private Text txtInviteCode;

	private Text txtLink;

	private Image imgQRCode;

	private Button btnShareCode;

	private Button btnCopyLink;

	private AndroidJavaClass jc;

	private AndroidJavaObject jo;

	private void Awake()
	{
		InitComponent();
		AssignComponent();
		if (Application.platform == RuntimePlatform.Android)
		{
			jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		}
	}

	private void OnEnable()
	{
		AssignTable();
	}

	private void Start()
	{
		btnShareCode.onClick.AddListener(ClickBtnShareCode);
		btnCopyLink.onClick.AddListener(ClickBtnCopyLink);
	}

	private void InitComponent()
	{
		txtTable = new Text[6];
		for (int i = 0; i < 6; i++)
		{
			txtTable[i] = base.transform.Find("Table").GetChild(i).GetComponent<Text>();
		}
		txtInviteCode = base.transform.Find("InviteIDTxt").GetComponent<Text>();
		txtLink = base.transform.Find("LinkTxt/Text").GetComponent<Text>();
		imgQRCode = base.transform.Find("ImgQRCodeBG").GetComponent<Image>();
		btnShareCode = base.transform.Find("BtnShareCode").GetComponent<Button>();
		btnCopyLink = base.transform.Find("BtnCopy").GetComponent<Button>();
	}

	private void ClickBtnShareCode()
	{
	}

	private void ClickBtnCopyLink()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		string empty = string.Empty;
		if (Application.platform == RuntimePlatform.Android)
		{
			empty = jo.Call<string>("Docopy", new object[1]
			{
				txtLink.text
			});
		}
		else if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			TextEditor textEditor = new TextEditor();
			textEditor.text = txtLink.text;
			TextEditor textEditor2 = textEditor;
			textEditor2.OnFocus();
			textEditor2.Copy();
		}
		MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("已复制", "Copy Done", string.Empty));
	}

	private void AssignComponent()
	{
		string downloadStr = ZH2_GVars.downloadStr;
		downloadStr = "http://hyzx0308.swncd88.vip.qy30.top/haiyangzhixing/html/Hall.html";
		txtLink.text = downloadStr;
		QRCodeMaker.QRCodeCreate(downloadStr);
		imgQRCode.sprite = Sprite.Create(QRCodeMaker.encoded, new Rect(0f, 0f, 512f, 512f), new Vector2(0f, 0f));
		txtInviteCode.text = "276792";
	}

	private void AssignTable()
	{
	}
}
