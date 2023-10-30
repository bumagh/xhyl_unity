using UnityEngine;
using UnityEngine.UI;

public class DP_PrizeAnimalItem : MonoBehaviour
{
	[HideInInspector]
	public Image imgAnimalIcon;

	[HideInInspector]
	public Image imgLuckyType;

	[HideInInspector]
	public Text txtPower;

	private void Awake()
	{
		imgAnimalIcon = base.transform.Find("ResultIcon").GetComponent<Image>();
		imgLuckyType = base.transform.Find("LuckyType").GetComponent<Image>();
		txtPower = base.transform.Find("Power").GetComponent<Text>();
	}
}
