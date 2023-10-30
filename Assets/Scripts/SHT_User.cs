using System;
using System.Collections.Generic;
using UnityEngine;

public class SHT_User
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

	public static SHT_User CreateWithDic(Dictionary<string, object> data)
	{
		SHT_User sHT_User = new SHT_User();
		try
		{
			sHT_User.id = (int)data["id"];
			sHT_User.username = (string)data["username"];
			sHT_User.nickname = (string)data["nickname"];
			sHT_User.sex = (string)data["sex"];
			sHT_User.level = (int)data["level"];
			sHT_User.gameGold = (int)data["gameGold"];
			sHT_User.expeGold = (int)data["expeGold"];
			sHT_User.photoId = (int)data["photoId"];
			sHT_User.overflow = (int)data["overflow"];
			sHT_User.type = (int)data["type"];
			sHT_User.addGameGold = 0;
			return sHT_User;
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
			return sHT_User;
		}
	}
}
