using System;
using System.Collections.Generic;
using UnityEngine;

public class LKB_User
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

	public static LKB_User CreateWithDic(Dictionary<string, object> data)
	{
		LKB_User lKB_User = new LKB_User();
		try
		{
			lKB_User.id = (int)data["id"];
			lKB_User.username = (string)data["username"];
			lKB_User.nickname = (string)data["nickname"];
			lKB_User.sex = (string)data["sex"];
			lKB_User.level = (int)data["level"];
			lKB_User.gameGold = (int)data["gameGold"];
			lKB_User.expeGold = (int)data["expeGold"];
			lKB_User.photoId = (int)data["photoId"];
			lKB_User.overflow = (int)data["overflow"];
			lKB_User.type = (int)data["type"];
			lKB_User.addGameGold = 0;
			return lKB_User;
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
			return lKB_User;
		}
	}
}
