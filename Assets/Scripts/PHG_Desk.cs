using System;
using System.Collections.Generic;
using UnityEngine;

public class PHG_Desk : IComparable
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

	public static PHG_Desk CreateWithDic(Dictionary<string, object> data)
	{
		PHG_Desk pHG_Desk = new PHG_Desk();
		try
		{
			pHG_Desk.id = (int)data["id"];
			pHG_Desk.roomId = (int)data["roomId"];
			pHG_Desk.name = (string)data["name"];
			pHG_Desk.minGold = (int)data["minGold"];
			pHG_Desk.exchange = (int)data["exchange"];
			pHG_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			pHG_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			pHG_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			pHG_Desk.singlechangeScore = (int)data["singlechangeScore"];
			pHG_Desk.diceSwitch = (int)data["diceSwitch"];
			pHG_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			pHG_Desk.diceOverflow = (int)data["diceOverflow"];
			pHG_Desk.userId = (int)data["userId"];
			pHG_Desk.userPhotoId = (int)data["userPhotoId"];
			pHG_Desk.nickname = (string)data["nickname"];
			return pHG_Desk;
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
			return pHG_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is PHG_Desk)
		{
			PHG_Desk pHG_Desk = obj as PHG_Desk;
			return id - pHG_Desk.id;
		}
		return 0;
	}
}
