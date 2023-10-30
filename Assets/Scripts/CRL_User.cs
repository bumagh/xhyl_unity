using System;
using System.Collections.Generic;
using UnityEngine;

public class CRL_User
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

	public static CRL_User CreateWithDic(Dictionary<string, object> data)
	{
		CRL_User cRL_User = new CRL_User();
		try
		{
			cRL_User.id = (int)data["id"];
			cRL_User.username = (string)data["username"];
			cRL_User.nickname = (string)data["nickname"];
			cRL_User.sex = (string)data["sex"];
			cRL_User.level = (int)data["level"];
			cRL_User.gameGold = (int)data["gameGold"];
			cRL_User.expeGold = (int)data["expeGold"];
			cRL_User.photoId = (int)data["photoId"];
			cRL_User.overflow = (int)data["overflow"];
			cRL_User.type = (int)data["type"];
			cRL_User.addGameGold = 0;
			return cRL_User;
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
			return cRL_User;
		}
	}
}
