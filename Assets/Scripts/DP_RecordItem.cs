using UnityEngine;
using UnityEngine.UI;

public class DP_RecordItem : MonoBehaviour
{
	[HideInInspector]
	public Image imgIcon;

	[HideInInspector]
	public Image imgZHXResult;

	[HideInInspector]
	public Image imgLuckyType;

	[HideInInspector]
	public Text txtLuckyMac;

	private void Awake()
	{
		imgIcon = base.transform.Find("Icon").GetComponent<Image>();
		imgZHXResult = base.transform.Find("ZHXResult").GetComponent<Image>();
		imgLuckyType = base.transform.Find("LuckyType").GetComponent<Image>();
		txtLuckyMac = base.transform.Find("LuckyMac").GetComponent<Text>();
	}
}
