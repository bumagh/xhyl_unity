using UnityEngine;
using UnityEngine.UI;

public class DP_SeatItem : MonoBehaviour
{
	[HideInInspector]
	public Button btnSeat;

	[HideInInspector]
	public GameObject objPerson;

	[HideInInspector]
	public Transform tfPersonIconBg;

	[HideInInspector]
	public Image imgPersonIcon;

	[HideInInspector]
	public Text txtPersonName;

	[HideInInspector]
	public bool bSeated;

	[HideInInspector]
	public int index;

	private void Awake()
	{
		btnSeat = base.transform.GetComponent<Button>();
		objPerson = base.transform.GetChild(0).gameObject;
		tfPersonIconBg = objPerson.transform.GetChild(0);
		imgPersonIcon = objPerson.transform.GetChild(2).GetComponent<Image>();
		txtPersonName = objPerson.transform.GetChild(1).GetComponent<Text>();
	}
}
