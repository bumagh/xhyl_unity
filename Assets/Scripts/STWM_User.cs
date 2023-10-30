using System;
using System.Collections.Generic;
using UnityEngine;

public class STWM_User
{
	public string username;

	public string nickname;

	public string sex;

	public int id;

	public int level;

	public int gameGold;

	public int addGameGold;

	public int expeGold;

	public int photoId;

	public int overflow;

	public int type;

	public string promoterName;

	public static STWM_User CreateWithDic(Dictionary<string, object> data)
	{
		STWM_User sTWM_User = new STWM_User();
		try
		{
			sTWM_User.id = (int)data["id"];
			sTWM_User.username = (string)data["username"];
			sTWM_User.nickname = (string)data["nickname"];
			sTWM_User.sex = (string)data["sex"];
			sTWM_User.level = (int)data["level"];
			sTWM_User.gameGold = (int)data["gameGold"];
			sTWM_User.expeGold = (int)data["expeGold"];
			sTWM_User.photoId = (int)data["photoId"];
			sTWM_User.overflow = (int)data["overflow"];
			sTWM_User.type = (int)data["type"];
			sTWM_User.addGameGold = 0;
			return sTWM_User;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("User excpetion: " + ex.Message);
			string text = string.Empty;
			foreach (string key in data.Keys)
			{
				text = text + key + ", ";
			}
			UnityEngine.Debug.Log("keys: " + text);
			return sTWM_User;
		}
	}
}
