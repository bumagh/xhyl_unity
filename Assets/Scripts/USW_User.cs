using System;
using System.Collections.Generic;
using UnityEngine;

public class USW_User
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

	public static USW_User CreateWithDic(Dictionary<string, object> data)
	{
		USW_User uSW_User = new USW_User();
		try
		{
			uSW_User.id = (int)data["id"];
			uSW_User.username = (string)data["username"];
			uSW_User.nickname = (string)data["nickname"];
			uSW_User.sex = (string)data["sex"];
			uSW_User.level = (int)data["level"];
			uSW_User.gameGold = (int)data["gameGold"];
			uSW_User.expeGold = (int)data["expeGold"];
			uSW_User.photoId = (int)data["photoId"];
			uSW_User.overflow = (int)data["overflow"];
			uSW_User.type = (int)data["type"];
			uSW_User.addGameGold = 0;
			return uSW_User;
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
			return uSW_User;
		}
	}
}
