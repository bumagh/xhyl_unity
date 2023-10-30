using UnityEngine;

[SerializeField]
public class JSYS_PlayerBet
{
	public string type;

	public string user_id;

	public string room_id;

	public string game_id;

	public string num;

	public string id;

	public string sign;

	public JSYS_PlayerBet(string _type, string roomid, string userid, string gameid, string betnum, string betid, string betsign)
	{
		type = _type;
		room_id = roomid;
		user_id = userid;
		game_id = gameid;
		num = betnum;
		id = betid;
		sign = betsign;
	}
}
