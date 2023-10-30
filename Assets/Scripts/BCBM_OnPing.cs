using UnityEngine;

[SerializeField]
public class BCBM_OnPing
{
	public string type;

	public string user_id;

	public string room_id;

	public string game_id;

	public BCBM_OnPing(string type, string user_id, string room_id, string game_id)
	{
		this.type = type;
		this.user_id = user_id;
		this.room_id = room_id;
		this.game_id = game_id;
	}
}
