using System;
using System.Collections.Generic;
using UnityEngine;

public class ESP_Desk : IComparable
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

	public static ESP_Desk CreateWithDic(Dictionary<string, object> data)
	{
		ESP_Desk eSP_Desk = new ESP_Desk();
		try
		{
			eSP_Desk.id = (int)data["id"];
			eSP_Desk.roomId = (int)data["roomId"];
			eSP_Desk.name = (string)data["name"];
			eSP_Desk.minGold = (int)data["minGold"];
			eSP_Desk.exchange = (int)data["exchange"];
			eSP_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			eSP_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			eSP_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			eSP_Desk.singlechangeScore = (int)data["singlechangeScore"];
			eSP_Desk.diceSwitch = (int)data["diceSwitch"];
			eSP_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			eSP_Desk.diceOverflow = (int)data["diceOverflow"];
			eSP_Desk.userId = (int)data["userId"];
			eSP_Desk.userPhotoId = (int)data["userPhotoId"];
			eSP_Desk.nickname = (string)data["nickname"];
			return eSP_Desk;
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
			return eSP_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is ESP_Desk)
		{
			ESP_Desk eSP_Desk = obj as ESP_Desk;
			return id - eSP_Desk.id;
		}
		return 0;
	}
}
