using UnityEngine;

[SerializeField]
public class BCBM_OnWinning
{
	public string type;

	public string user_id;

	public string room_id;

	public string game_id;

	public string[] fishType;

	public string[] fishRate;

	public string[] FishValue;

	public string roomType;

	public string BulletValue;

	public string BulletType;

	public string typeInfo;

	public string Bomb;

	public BCBM_OnWinning(string type, string user_id, string room_id, string game_id, string[] fishType, string[] fishRate, string[] FishValue, string roomType, string BulletValue, string BulletType, string typeInfo, string Bomb)
	{
		this.type = type;
		this.user_id = user_id;
		this.room_id = room_id;
		this.game_id = game_id;
		this.fishType = fishType;
		this.fishRate = fishRate;
		this.FishValue = FishValue;
		this.roomType = roomType;
		this.BulletValue = BulletValue;
		this.BulletType = BulletType;
		this.typeInfo = typeInfo;
		this.Bomb = Bomb;
	}
}
