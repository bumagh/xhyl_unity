using System;
using System.Collections.Generic;
using UnityEngine;

public class Hfh_User
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

	public static Hfh_User CreateWithDic(Dictionary<string, object> data)
	{
		Hfh_User hfh_User = new Hfh_User();
		try
		{
			hfh_User.id = (int)data["id"];
			hfh_User.username = (string)data["username"];
			hfh_User.nickname = (string)data["nickname"];
			hfh_User.sex = (string)data["sex"];
			hfh_User.level = (int)data["level"];
			hfh_User.gameGold = (int)data["gameGold"];
			hfh_User.expeGold = (int)data["expeGold"];
			hfh_User.gameScore = (int)data["gameScore"];
			hfh_User.expeScore = (int)data["expeScore"];
			hfh_User.photoId = (int)data["photoId"];
			hfh_User.overflow = (int)data["overflow"];
			hfh_User.type = (int)data["type"];
			hfh_User.addGameGold = 0;
			return hfh_User;
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
			return hfh_User;
		}
	}
}
