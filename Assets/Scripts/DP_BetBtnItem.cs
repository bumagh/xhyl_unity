using UnityEngine;
using UnityEngine.UI;

public class DP_BetBtnItem : MonoBehaviour
{
	[HideInInspector]
	public DP_LongPressOrClickEventTrigger btnBet;

	[HideInInspector]
	public Text txtPower;

	[HideInInspector]
	public Text txtPersonBet;

	[HideInInspector]
	public Text txtTotalBet;

	private void Awake()
	{
		btnBet = base.transform.Find("Bg").GetComponent<DP_LongPressOrClickEventTrigger>();
		txtPower = base.transform.Find("TxtPower").GetComponent<Text>();
		txtPersonBet = base.transform.Find("TxtBet").GetComponent<Text>();
		txtTotalBet = base.transform.Find("TxtTotalBet").GetComponent<Text>();
	}
}
