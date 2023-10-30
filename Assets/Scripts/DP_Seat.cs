using UnityEngine;

public class DP_Seat : MonoBehaviour
{
	[HideInInspector]
	public Transform tfSeat;

	[HideInInspector]
	public GameObject objTableIds;

	[HideInInspector]
	public DP_SeatItem[] seatItems;

	private Vector3 vec = new Vector3(0.54f, 0.68f, 1f);

	private void Awake()
	{
		tfSeat = base.transform.GetChild(0);
		objTableIds = base.transform.GetChild(1).gameObject;
		seatItems = new DP_SeatItem[8];
		Transform child = base.transform.GetChild(2);
		for (int i = 0; i < 8; i++)
		{
			seatItems[i] = child.GetChild(i).GetComponent<DP_SeatItem>();
			seatItems[i].index = i;
		}
	}

	public void Init()
	{
		tfSeat.localPosition = Vector3.up * 15f;
		tfSeat.localScale = vec;
		objTableIds.SetActive(value: false);
		for (int i = 0; i < 8; i++)
		{
			seatItems[i].tfPersonIconBg.localScale = Vector3.zero;
			seatItems[i].txtPersonName.gameObject.SetActive(value: false);
			seatItems[i].imgPersonIcon.gameObject.SetActive(value: false);
		}
		base.gameObject.SetActive(value: false);
	}
}
