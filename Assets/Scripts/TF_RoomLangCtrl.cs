using UnityEngine;
using UnityEngine.UI;

public class TF_RoomLangCtrl : MonoBehaviour
{
	public Sprite[] spiTitleBg;

	public Sprite[] spiBtnTrain;

	public Sprite[] spiBtnArena;

	public Sprite[] spiPersonInfoBg;

	private Image imgTitleBg;

	private Image imgBtnTrain;

	private Image imgBtnArena;

	private Image imgPersonInfoBg;

	private Text txtTableList;

	private void Awake()
	{
		imgTitleBg = base.transform.Find("Title/ImgTitleBg").GetComponent<Image>();
		imgBtnTrain = base.transform.Find("Rooms/BtnTrain").GetComponent<Image>();
		imgBtnArena = base.transform.Find("Rooms/BtnArena").GetComponent<Image>();
		imgPersonInfoBg = base.transform.Find("PersonInfoDialog/ImgDialog").GetComponent<Image>();
		txtTableList = base.transform.Find("TableInfo/ListTable/ImgBg/TxtName").GetComponent<Text>();
		Transform transform = base.transform.Find("TableInfo/Info");
	}

	private void Start()
	{
		int language = TF_GameInfo.getInstance().Language;
		imgTitleBg.sprite = spiTitleBg[language];
		imgBtnTrain.sprite = spiBtnTrain[language];
		imgBtnArena.sprite = spiBtnArena[language];
		imgPersonInfoBg.sprite = spiPersonInfoBg[language];
		txtTableList.text = ((language != 0) ? "Table list" : "桌列表名称");
	}
}
