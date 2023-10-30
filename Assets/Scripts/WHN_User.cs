using System;
using System.Collections.Generic;
using UnityEngine;

public class WHN_User
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

	public static WHN_User CreateWithDic(Dictionary<string, object> data)
	{
		WHN_User wHN_User = new WHN_User();
		try
		{
			wHN_User.id = (int)data["id"];
			wHN_User.username = (string)data["username"];
			wHN_User.nickname = (string)data["nickname"];
			wHN_User.sex = (string)data["sex"];
			wHN_User.level = (int)data["level"];
			wHN_User.gameGold = (int)data["gameGold"];
			wHN_User.expeGold = (int)data["expeGold"];
			wHN_User.photoId = (int)data["photoId"];
			wHN_User.overflow = (int)data["overflow"];
			wHN_User.type = (int)data["type"];
			wHN_User.addGameGold = 0;
			return wHN_User;
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
			return wHN_User;
		}
	}
}
