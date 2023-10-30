using UnityEngine;
using UnityEngine.UI;

public class DP_TableItem : MonoBehaviour
{
	[HideInInspector]
	public Button btnTable;

	[HideInInspector]
	public Image imgTableIdBg;

	[HideInInspector]
	public Text txtTableId;

	[HideInInspector]
	public Text txtTableInfo;

	public void Init()
	{
		btnTable = base.transform.Find("BtnTable").GetComponent<Button>();
		imgTableIdBg = base.transform.Find("ImgTableNameBg").GetComponent<Image>();
		txtTableId = imgTableIdBg.transform.GetChild(0).GetComponent<Text>();
		txtTableInfo = base.transform.Find("ImgTableInfoBg/TxtTableInfo").GetComponent<Text>();
	}
}
