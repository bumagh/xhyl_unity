using System;
using System.Collections.Generic;
using UnityEngine;

public class DCDF_User
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

	public static DCDF_User CreateWithDic(Dictionary<string, object> data)
	{
		DCDF_User dCDF_User = new DCDF_User();
		try
		{
			dCDF_User.id = (int)data["id"];
			dCDF_User.username = (string)data["username"];
			dCDF_User.nickname = (string)data["nickname"];
			dCDF_User.sex = (string)data["sex"];
			dCDF_User.level = (int)data["level"];
			dCDF_User.gameGold = (int)data["gameGold"];
			dCDF_User.expeGold = (int)data["expeGold"];
			dCDF_User.photoId = (int)data["photoId"];
			dCDF_User.overflow = (int)data["overflow"];
			dCDF_User.type = (int)data["type"];
			dCDF_User.addGameGold = 0;
			return dCDF_User;
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
			return dCDF_User;
		}
	}
}
