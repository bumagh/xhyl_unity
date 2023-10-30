using UnityEngine;
using UnityEngine.UI;

public class STMF_RoomLangCtrl : MonoBehaviour
{
	public Sprite[] spiPersonInfoBg;

	private Text txtTableList;

	private void Awake()
	{
		txtTableList = base.transform.Find("TableInfo/ListTable/ImgBg/TxtName").GetComponent<Text>();
	}

	private void Start()
	{
		int language = STMF_GameInfo.getInstance().Language;
		txtTableList.text = ((language != 0) ? "Table list" : "桌列表名称");
	}
}
