using System;
using System.Collections.Generic;
using UnityEngine;

public class SPA_User
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

	public static SPA_User CreateWithDic(Dictionary<string, object> data)
	{
		SPA_User sPA_User = new SPA_User();
		try
		{
			sPA_User.id = (int)data["id"];
			sPA_User.username = (string)data["username"];
			sPA_User.nickname = (string)data["nickname"];
			sPA_User.sex = (string)data["sex"];
			sPA_User.level = (int)data["level"];
			sPA_User.gameGold = (int)data["gameGold"];
			sPA_User.expeGold = (int)data["expeGold"];
			sPA_User.photoId = (int)data["photoId"];
			sPA_User.overflow = (int)data["overflow"];
			sPA_User.type = (int)data["type"];
			sPA_User.addGameGold = 0;
			return sPA_User;
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
			return sPA_User;
		}
	}
}
