using UnityEngine;
using UnityEngine.UI;

public class DNTG_RoomLangCtrl : MonoBehaviour
{
	public Sprite[] spiPersonInfoBg;

	private Image imgTitleBg;

	private Image imgPersonInfoBg;

	private Text txtTableList;

	private void Awake()
	{
		imgTitleBg = base.transform.Find("Title/ImgTitleBg").GetComponent<Image>();
		imgPersonInfoBg = base.transform.Find("PersonInfoDialog/ImgDialog").GetComponent<Image>();
		txtTableList = base.transform.Find("TableInfo/ListTable/ImgBg/TxtName").GetComponent<Text>();
		Transform transform = base.transform.Find("TableInfo/Info");
	}

	private void Start()
	{
		int language = DNTG_GameInfo.getInstance().Language;
		imgPersonInfoBg.sprite = spiPersonInfoBg[language];
		txtTableList.text = ((language != 0) ? "Table list" : "桌列表名称");
	}
}
