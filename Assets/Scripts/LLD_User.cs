using System;
using System.Collections.Generic;
using UnityEngine;

public class LLD_User
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

	public static LLD_User CreateWithDic(Dictionary<string, object> data)
	{
		LLD_User lLD_User = new LLD_User();
		try
		{
			lLD_User.id = (int)data["id"];
			lLD_User.username = (string)data["username"];
			lLD_User.nickname = (string)data["nickname"];
			lLD_User.sex = (string)data["sex"];
			lLD_User.level = (int)data["level"];
			lLD_User.gameGold = (int)data["gameGold"];
			lLD_User.expeGold = (int)data["expeGold"];
			lLD_User.photoId = (int)data["photoId"];
			lLD_User.overflow = (int)data["overflow"];
			lLD_User.type = (int)data["type"];
			lLD_User.addGameGold = 0;
			return lLD_User;
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
			return lLD_User;
		}
	}
}
