using UnityEngine;
using UnityEngine.UI;

public class DP_SeatItems : MonoBehaviour
{
	[HideInInspector]
	public Button btnSeat;

	[HideInInspector]
	public Text txtSeatId;

	[HideInInspector]
	public Image imgPerson;

	[HideInInspector]
	public GameObject objNoPerson;

	[HideInInspector]
	public bool bSeated;

	public void Init()
	{
		btnSeat = base.transform.Find("BtnSeat").GetComponent<Button>();
		txtSeatId = base.transform.Find("ImgSeatIdBg/TxtSeatId").GetComponent<Text>();
		imgPerson = base.transform.Find("ImgPerson").GetComponent<Image>();
		objNoPerson = base.transform.Find("NoPerson").gameObject;
	}
}
