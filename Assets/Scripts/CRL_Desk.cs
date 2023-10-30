using System;
using System.Collections.Generic;
using UnityEngine;

public class CRL_Desk : IComparable
{
	public int id;

	public int roomId;

	public string name;

	public int minGold;

	public int exchange;

	public int onceExchangeValue;

	public int maxSinglelineBet;

	public int minSinglelineBet;

	public int singlechangeScore;

	public int diceSwitch;

	public int diceDirectSwitch;

	public int diceOverflow;

	public int userId;

	public int userPhotoId;

	public string nickname;

	public static CRL_Desk CreateWithDic(Dictionary<string, object> data)
	{
		CRL_Desk cRL_Desk = new CRL_Desk();
		try
		{
			cRL_Desk.id = (int)data["id"];
			cRL_Desk.roomId = (int)data["roomId"];
			cRL_Desk.name = (string)data["name"];
			cRL_Desk.minGold = (int)data["minGold"];
			cRL_Desk.exchange = (int)data["exchange"];
			cRL_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			cRL_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			cRL_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			cRL_Desk.singlechangeScore = (int)data["singlechangeScore"];
			cRL_Desk.diceSwitch = (int)data["diceSwitch"];
			cRL_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			cRL_Desk.diceOverflow = (int)data["diceOverflow"];
			cRL_Desk.userId = (int)data["userId"];
			cRL_Desk.userPhotoId = (int)data["userPhotoId"];
			cRL_Desk.nickname = (string)data["nickname"];
			return cRL_Desk;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Desk excpetion: " + ex.Message);
			string text = string.Empty;
			foreach (string key in data.Keys)
			{
				text = text + key + ", ";
			}
			UnityEngine.Debug.Log("keys: " + text);
			return cRL_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is CRL_Desk)
		{
			CRL_Desk cRL_Desk = obj as CRL_Desk;
			return id - cRL_Desk.id;
		}
		return 0;
	}
}
