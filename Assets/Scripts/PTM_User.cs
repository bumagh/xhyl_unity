using System;
using System.Collections.Generic;
using UnityEngine;

public class PTM_User
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

	public static PTM_User CreateWithDic(Dictionary<string, object> data)
	{
		PTM_User pTM_User = new PTM_User();
		try
		{
			pTM_User.id = (int)data["id"];
			pTM_User.username = (string)data["username"];
			pTM_User.nickname = (string)data["nickname"];
			pTM_User.sex = (string)data["sex"];
			pTM_User.level = (int)data["level"];
			pTM_User.gameGold = (int)data["gameGold"];
			pTM_User.expeGold = (int)data["expeGold"];
			pTM_User.photoId = (int)data["photoId"];
			pTM_User.overflow = (int)data["overflow"];
			pTM_User.type = (int)data["type"];
			pTM_User.addGameGold = 0;
			return pTM_User;
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
			return pTM_User;
		}
	}
}
