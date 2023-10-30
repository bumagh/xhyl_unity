using System;
using System.Collections.Generic;
using UnityEngine;

public class LRS_User
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

	public static LRS_User CreateWithDic(Dictionary<string, object> data)
	{
		LRS_User lRS_User = new LRS_User();
		try
		{
			lRS_User.id = (int)data["id"];
			lRS_User.username = (string)data["username"];
			lRS_User.nickname = (string)data["nickname"];
			lRS_User.sex = (string)data["sex"];
			lRS_User.level = (int)data["level"];
			lRS_User.gameGold = (int)data["gameGold"];
			lRS_User.expeGold = (int)data["expeGold"];
			lRS_User.photoId = (int)data["photoId"];
			lRS_User.overflow = (int)data["overflow"];
			lRS_User.type = (int)data["type"];
			lRS_User.addGameGold = 0;
			return lRS_User;
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
			return lRS_User;
		}
	}
}
