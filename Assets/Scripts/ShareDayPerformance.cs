using UnityEngine;
using UnityEngine.UI;

public class ShareDayPerformance : MonoBehaviour
{
	private Text txtTime;

	private Text txtDayTotal;

	private Text txtDayTeam;

	private Text txtDayPersonal;

	private void Awake()
	{
		txtTime = base.transform.Find("TxtTime").GetComponent<Text>();
		txtDayTotal = base.transform.Find("TxtDayTotal").GetComponent<Text>();
		txtDayTeam = base.transform.Find("TxtDayTeam").GetComponent<Text>();
		txtDayPersonal = base.transform.Find("TxtDayPersonal").GetComponent<Text>();
	}

	public void Init(string[] strs)
	{
		txtTime.text = strs[0];
		txtDayTotal.text = strs[1];
		txtDayTeam.text = strs[2];
		txtDayPersonal.text = strs[3];
	}
}
