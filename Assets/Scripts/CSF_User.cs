using System;
using System.Collections.Generic;
using UnityEngine;

public class CSF_User
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

	public static CSF_User CreateWithDic(Dictionary<string, object> data)
	{
		CSF_User cSF_User = new CSF_User();
		try
		{
			cSF_User.id = (int)data["id"];
			cSF_User.username = (string)data["username"];
			cSF_User.nickname = (string)data["nickname"];
			cSF_User.sex = (string)data["sex"];
			cSF_User.level = (int)data["level"];
			cSF_User.gameGold = (int)data["gameGold"];
			cSF_User.expeGold = (int)data["expeGold"];
			cSF_User.photoId = (int)data["photoId"];
			cSF_User.overflow = (int)data["overflow"];
			cSF_User.type = (int)data["type"];
			cSF_User.addGameGold = 0;
			return cSF_User;
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
			return cSF_User;
		}
	}
}
