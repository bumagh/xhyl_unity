using System;
using System.Collections.Generic;
using UnityEngine;

public class WHN_Desk : IComparable
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

	public static WHN_Desk CreateWithDic(Dictionary<string, object> data)
	{
		WHN_Desk wHN_Desk = new WHN_Desk();
		try
		{
			wHN_Desk.id = (int)data["id"];
			wHN_Desk.roomId = (int)data["roomId"];
			wHN_Desk.name = (string)data["name"];
			wHN_Desk.minGold = (int)data["minGold"];
			wHN_Desk.exchange = (int)data["exchange"];
			wHN_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			wHN_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			wHN_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			wHN_Desk.singlechangeScore = (int)data["singlechangeScore"];
			wHN_Desk.diceSwitch = (int)data["diceSwitch"];
			wHN_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			wHN_Desk.diceOverflow = (int)data["diceOverflow"];
			wHN_Desk.userId = (int)data["userId"];
			wHN_Desk.userPhotoId = (int)data["userPhotoId"];
			wHN_Desk.nickname = (string)data["nickname"];
			return wHN_Desk;
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
			return wHN_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is WHN_Desk)
		{
			WHN_Desk wHN_Desk = obj as WHN_Desk;
			return id - wHN_Desk.id;
		}
		return 0;
	}
}
