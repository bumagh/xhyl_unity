using System;
using System.Collections.Generic;
using UnityEngine;

public class DPR_Desk : IComparable
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

	public static DPR_Desk CreateWithDic(Dictionary<string, object> data)
	{
		DPR_Desk dPR_Desk = new DPR_Desk();
		try
		{
			dPR_Desk.id = (int)data["id"];
			dPR_Desk.roomId = (int)data["roomId"];
			dPR_Desk.name = (string)data["name"];
			dPR_Desk.minGold = (int)data["minGold"];
			dPR_Desk.exchange = (int)data["exchange"];
			dPR_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			dPR_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			dPR_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			dPR_Desk.singlechangeScore = (int)data["singlechangeScore"];
			dPR_Desk.diceSwitch = (int)data["diceSwitch"];
			dPR_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			dPR_Desk.diceOverflow = (int)data["diceOverflow"];
			dPR_Desk.userId = (int)data["userId"];
			dPR_Desk.userPhotoId = (int)data["userPhotoId"];
			dPR_Desk.nickname = (string)data["nickname"];
			return dPR_Desk;
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
			return dPR_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is DPR_Desk)
		{
			DPR_Desk dPR_Desk = obj as DPR_Desk;
			return id - dPR_Desk.id;
		}
		return 0;
	}
}
