using System;
using System.Collections.Generic;
using UnityEngine;

public class LRS_Desk : IComparable
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

	public static LRS_Desk CreateWithDic(Dictionary<string, object> data)
	{
		LRS_Desk lRS_Desk = new LRS_Desk();
		try
		{
			lRS_Desk.id = (int)data["id"];
			lRS_Desk.roomId = (int)data["roomId"];
			lRS_Desk.name = (string)data["name"];
			lRS_Desk.minGold = (int)data["minGold"];
			lRS_Desk.exchange = (int)data["exchange"];
			lRS_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			lRS_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			lRS_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			lRS_Desk.singlechangeScore = (int)data["singlechangeScore"];
			lRS_Desk.diceSwitch = (int)data["diceSwitch"];
			lRS_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			lRS_Desk.diceOverflow = (int)data["diceOverflow"];
			lRS_Desk.userId = (int)data["userId"];
			lRS_Desk.userPhotoId = (int)data["userPhotoId"];
			lRS_Desk.nickname = (string)data["nickname"];
			return lRS_Desk;
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
			return lRS_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is LRS_Desk)
		{
			LRS_Desk lRS_Desk = obj as LRS_Desk;
			return id - lRS_Desk.id;
		}
		return 0;
	}
}
