using UnityEngine;

[SerializeField]
public class BCBM_OnBetDown
{
	public string type;

	public string user_id;

	public string room_id;

	public string coinDown;

	public string pokerCode;

	public BCBM_OnBetDown(string type, string user_id, string room_id, string coinDown, string pokerCode)
	{
		this.type = type;
		this.user_id = user_id;
		this.room_id = room_id;
		this.coinDown = coinDown;
		this.pokerCode = pokerCode;
	}
}
