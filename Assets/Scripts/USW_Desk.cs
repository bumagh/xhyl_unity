using System;
using System.Collections.Generic;
using UnityEngine;

public class USW_Desk : IComparable
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

	public static USW_Desk CreateWithDic(Dictionary<string, object> data)
	{
		USW_Desk uSW_Desk = new USW_Desk();
		try
		{
			uSW_Desk.id = (int)data["id"];
			uSW_Desk.roomId = (int)data["roomId"];
			uSW_Desk.name = (string)data["name"];
			uSW_Desk.minGold = (int)data["minGold"];
			uSW_Desk.exchange = (int)data["exchange"];
			uSW_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			uSW_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			uSW_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			uSW_Desk.singlechangeScore = (int)data["singlechangeScore"];
			uSW_Desk.diceSwitch = (int)data["diceSwitch"];
			uSW_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			uSW_Desk.diceOverflow = (int)data["diceOverflow"];
			uSW_Desk.userId = (int)data["userId"];
			uSW_Desk.userPhotoId = (int)data["userPhotoId"];
			uSW_Desk.nickname = (string)data["nickname"];
			return uSW_Desk;
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
			return uSW_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is USW_Desk)
		{
			USW_Desk uSW_Desk = obj as USW_Desk;
			return id - uSW_Desk.id;
		}
		return 0;
	}
}
