using System;
using System.Collections.Generic;
using UnityEngine;

public class User
{
	public string username;

	public string nickname;

	public string phone;

	public string password;

	public string sex;

	public string card;

	public string question;

	public string answer;

	public string registDate;

	public string loginDate;

	public int status;

	public int overflow;

	public int gameGold;

	public int lottery;

	public int expeGold;

	public double levelScore;

	public int gameScore;

	public int expeScore;

	public int level = 1;

	public int photoId = 1;

	public int type;

	public int payMoney;

	public int id;

	public int gameid;

	public int security;

	public int promoterId;

	public string promoterUsername;

	public int safeBox;

	public int boxGameGold;

	public string accountBankName = string.Empty;

	public string bankName = string.Empty;

	public string BankCard = string.Empty;

	public string BankUserName = string.Empty;

	public static User CreateWithDic(Dictionary<string, object> data)
	{
		User user = new User();
		try
		{
			user.username = (string)data["username"];
			user.nickname = (string)data["nickname"];
			user.phone = TryGetValue(data, "phone", string.Empty);
			user.photoId = (int)data["photoId"] - 1;
			if (user.photoId < 0 || user.photoId >= 10)
			{
				user.photoId = 0;
			}
			user.sex = (string)data["sex"];
			user.level = (int)data["level"];
			user.gameGold = (int)data["gameGold"];
			user.lottery = (int)data["lottery"];
			user.card = (string)data["card"];
			user.expeGold = (int)data["expeGold"];
			user.overflow = (int)data["overflow"];
			user.type = (int)data["type"];
			user.question = (string)data["question"];
			user.security = (int)data["security"];
			user.safeBox = (int)data["safeBox"];
			user.id = (int)data["id"];
			user.gameid = (int)data["gameId"];
			user.promoterId = (int)data["promoterId"];
			user.promoterUsername = (string)data["promoterName"];
			user.boxGameGold = (int)data["boxGameGold"];
			user.accountBankName = (string)data["accountBankName"];
			user.bankName = (string)data["bankName"];
			user.BankCard = (string)data["card"];
			user.BankUserName = (string)data["name"];
			return user;
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
			return user;
		}
	}

	private static T TryGetValue<T>(Dictionary<string, object> data, string key, T defValue)
	{
		if (data.ContainsKey(key))
		{
			return (T)data[key];
		}
		return defValue;
	}
}
