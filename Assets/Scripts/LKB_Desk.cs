using System;
using System.Collections.Generic;
using UnityEngine;

public class LKB_Desk : IComparable
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

	public static LKB_Desk CreateWithDic(Dictionary<string, object> data)
	{
		LKB_Desk lKB_Desk = new LKB_Desk();
		try
		{
			lKB_Desk.id = (int)data["id"];
			lKB_Desk.roomId = (int)data["roomId"];
			lKB_Desk.name = (string)data["name"];
			lKB_Desk.minGold = (int)data["minGold"];
			lKB_Desk.exchange = (int)data["exchange"];
			lKB_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			lKB_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			lKB_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			lKB_Desk.singlechangeScore = (int)data["singlechangeScore"];
			lKB_Desk.diceSwitch = (int)data["diceSwitch"];
			lKB_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			lKB_Desk.diceOverflow = (int)data["diceOverflow"];
			lKB_Desk.userId = (int)data["userId"];
			lKB_Desk.userPhotoId = (int)data["userPhotoId"];
			lKB_Desk.nickname = (string)data["nickname"];
			return lKB_Desk;
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
			return lKB_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is LKB_Desk)
		{
			LKB_Desk lKB_Desk = obj as LKB_Desk;
			return id - lKB_Desk.id;
		}
		return 0;
	}
}
