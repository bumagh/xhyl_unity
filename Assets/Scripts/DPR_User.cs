using System;
using System.Collections.Generic;
using UnityEngine;

public class DPR_User
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

	public static DPR_User CreateWithDic(Dictionary<string, object> data)
	{
		DPR_User dPR_User = new DPR_User();
		try
		{
			dPR_User.id = (int)data["id"];
			dPR_User.username = (string)data["username"];
			dPR_User.nickname = (string)data["nickname"];
			dPR_User.sex = (string)data["sex"];
			dPR_User.level = (int)data["level"];
			dPR_User.gameGold = (int)data["gameGold"];
			dPR_User.expeGold = (int)data["expeGold"];
			dPR_User.photoId = (int)data["photoId"];
			dPR_User.overflow = (int)data["overflow"];
			dPR_User.type = (int)data["type"];
			dPR_User.addGameGold = 0;
			return dPR_User;
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
			return dPR_User;
		}
	}
}
