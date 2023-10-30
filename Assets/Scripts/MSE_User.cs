using System;
using System.Collections.Generic;
using UnityEngine;

public class MSE_User
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

	public static MSE_User CreateWithDic(Dictionary<string, object> data)
	{
		MSE_User mSE_User = new MSE_User();
		try
		{
			mSE_User.id = (int)data["id"];
			mSE_User.username = (string)data["username"];
			mSE_User.nickname = (string)data["nickname"];
			mSE_User.sex = (string)data["sex"];
			mSE_User.level = (int)data["level"];
			mSE_User.gameGold = (int)data["gameGold"];
			mSE_User.expeGold = (int)data["expeGold"];
			mSE_User.photoId = (int)data["photoId"];
			mSE_User.overflow = (int)data["overflow"];
			mSE_User.type = (int)data["type"];
			mSE_User.addGameGold = 0;
			return mSE_User;
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
			return mSE_User;
		}
	}
}
