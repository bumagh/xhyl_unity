using UnityEngine;

[SerializeField]
public class JSYS_BetCancelAll
{
	public string type;

	public string user_id;

	public string room_id;

	public string game_id;

	public JSYS_BetCancelAll(string _type, string roomid, string userid, string gameid)
	{
		type = _type;
		room_id = roomid;
		user_id = userid;
		game_id = gameid;
	}
}
