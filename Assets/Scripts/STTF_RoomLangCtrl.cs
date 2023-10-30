using UnityEngine;
using UnityEngine.UI;

public class STTF_RoomLangCtrl : MonoBehaviour
{
	public Sprite[] spiPersonInfoBg;

	private Image imgTitleBg;

	private Text txtTableList;

	private void Awake()
	{
		imgTitleBg = base.transform.Find("Title/ImgTitleBg").GetComponent<Image>();
		txtTableList = base.transform.Find("TableInfo/ListTable/ImgBg/TxtName").GetComponent<Text>();
	}

	private void Start()
	{
		int language = STTF_GameInfo.getInstance().Language;
		txtTableList.text = ((language != 0) ? "Table list" : "桌列表名称");
	}
}
