using System;
using System.Collections.Generic;
using UnityEngine;

public class Dzb_User
{
	public string username;

	public string nickname;

	public string sex;

	public int id;

	public int level;

	public int gameGold;

	public int addGameGold;

	public int expeGold;

	public int gameScore;

	public int expeScore;

	public int photoId;

	public int overflow;

	public int type;

	public string promoterName;

	public static Dzb_User CreateWithDic(Dictionary<string, object> data)
	{
		Dzb_User dzb_User = new Dzb_User();
		try
		{
			dzb_User.id = (int)data["id"];
			dzb_User.username = (string)data["username"];
			dzb_User.nickname = (string)data["nickname"];
			dzb_User.sex = (string)data["sex"];
			dzb_User.level = (int)data["level"];
			dzb_User.gameGold = (int)data["gameGold"];
			dzb_User.expeGold = (int)data["expeGold"];
			dzb_User.gameScore = (int)data["gameScore"];
			dzb_User.expeScore = (int)data["expeScore"];
			dzb_User.photoId = (int)data["photoId"];
			dzb_User.overflow = (int)data["overflow"];
			dzb_User.type = (int)data["type"];
			dzb_User.addGameGold = 0;
			return dzb_User;
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
			return dzb_User;
		}
	}
}
