using System;
using System.Collections.Generic;
using UnityEngine;

public class ESP_User
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

	public static ESP_User CreateWithDic(Dictionary<string, object> data)
	{
		ESP_User eSP_User = new ESP_User();
		try
		{
			eSP_User.id = (int)data["id"];
			eSP_User.username = (string)data["username"];
			eSP_User.nickname = (string)data["nickname"];
			eSP_User.sex = (string)data["sex"];
			eSP_User.level = (int)data["level"];
			eSP_User.gameGold = (int)data["gameGold"];
			eSP_User.expeGold = (int)data["expeGold"];
			eSP_User.photoId = (int)data["photoId"];
			eSP_User.overflow = (int)data["overflow"];
			eSP_User.type = (int)data["type"];
			eSP_User.addGameGold = 0;
			return eSP_User;
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
			return eSP_User;
		}
	}
}
