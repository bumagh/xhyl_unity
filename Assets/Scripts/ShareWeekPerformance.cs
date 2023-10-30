using UnityEngine;
using UnityEngine.UI;

public class ShareWeekPerformance : MonoBehaviour
{
	private Text txtTotal;

	private Text txtTeam;

	private Text txtPersonal;

	private Text txtGains;

	private void Awake()
	{
		txtTotal = base.transform.Find("TxtTotal").GetComponent<Text>();
		txtTeam = base.transform.Find("TxtTeam").GetComponent<Text>();
		txtPersonal = base.transform.Find("TxtPersonal").GetComponent<Text>();
		txtGains = base.transform.Find("TxtGains").GetComponent<Text>();
	}

	public void Init(string[] strs)
	{
		txtTotal.text = strs[0];
		txtTeam.text = strs[1];
		txtPersonal.text = strs[2];
		txtGains.text = strs[3];
	}
}
