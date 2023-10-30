using System;
using System.Collections.Generic;
using UnityEngine;

public class LLD_Desk : IComparable
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

	public static LLD_Desk CreateWithDic(Dictionary<string, object> data)
	{
		LLD_Desk lLD_Desk = new LLD_Desk();
		try
		{
			lLD_Desk.id = (int)data["id"];
			lLD_Desk.roomId = (int)data["roomId"];
			lLD_Desk.name = (string)data["name"];
			lLD_Desk.minGold = (int)data["minGold"];
			lLD_Desk.exchange = (int)data["exchange"];
			lLD_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			lLD_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			lLD_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			lLD_Desk.singlechangeScore = (int)data["singlechangeScore"];
			lLD_Desk.diceSwitch = (int)data["diceSwitch"];
			lLD_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			lLD_Desk.diceOverflow = (int)data["diceOverflow"];
			lLD_Desk.userId = (int)data["userId"];
			lLD_Desk.userPhotoId = (int)data["userPhotoId"];
			lLD_Desk.nickname = (string)data["nickname"];
			return lLD_Desk;
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
			return lLD_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is LLD_Desk)
		{
			LLD_Desk lLD_Desk = obj as LLD_Desk;
			return id - lLD_Desk.id;
		}
		return 0;
	}
}
