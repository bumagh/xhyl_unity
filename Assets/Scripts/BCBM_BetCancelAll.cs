using UnityEngine;

[SerializeField]
public class BCBM_BetCancelAll
{
	public string type;

	public string user_id;

	public string room_id;

	public string game_id;

	public BCBM_BetCancelAll(string _type, string roomid, string userid, string gameid)
	{
		type = _type;
		room_id = roomid;
		user_id = userid;
		game_id = gameid;
	}
}
