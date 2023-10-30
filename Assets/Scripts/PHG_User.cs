using System;
using System.Collections.Generic;
using UnityEngine;

public class PHG_User
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

	public static PHG_User CreateWithDic(Dictionary<string, object> data)
	{
		PHG_User pHG_User = new PHG_User();
		try
		{
			pHG_User.id = (int)data["id"];
			pHG_User.username = (string)data["username"];
			pHG_User.nickname = (string)data["nickname"];
			pHG_User.sex = (string)data["sex"];
			pHG_User.level = (int)data["level"];
			pHG_User.gameGold = (int)data["gameGold"];
			pHG_User.expeGold = (int)data["expeGold"];
			pHG_User.photoId = (int)data["photoId"];
			pHG_User.overflow = (int)data["overflow"];
			pHG_User.type = (int)data["type"];
			pHG_User.addGameGold = 0;
			return pHG_User;
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
			return pHG_User;
		}
	}
}
