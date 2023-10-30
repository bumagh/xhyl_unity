using System;
using System.Collections.Generic;
using UnityEngine;

public class SHT_Desk : IComparable
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

	public static SHT_Desk CreateWithDic(Dictionary<string, object> data)
	{
		SHT_Desk sHT_Desk = new SHT_Desk();
		try
		{
			sHT_Desk.id = (int)data["id"];
			sHT_Desk.roomId = (int)data["roomId"];
			sHT_Desk.name = (string)data["name"];
			sHT_Desk.minGold = (int)data["minGold"];
			sHT_Desk.exchange = (int)data["exchange"];
			sHT_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			sHT_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			sHT_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			sHT_Desk.singlechangeScore = (int)data["singlechangeScore"];
			sHT_Desk.diceSwitch = (int)data["diceSwitch"];
			sHT_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			sHT_Desk.diceOverflow = (int)data["diceOverflow"];
			sHT_Desk.userId = (int)data["userId"];
			sHT_Desk.userPhotoId = (int)data["userPhotoId"];
			sHT_Desk.nickname = (string)data["nickname"];
			return sHT_Desk;
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
			return sHT_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is SHT_Desk)
		{
			SHT_Desk sHT_Desk = obj as SHT_Desk;
			return id - sHT_Desk.id;
		}
		return 0;
	}
}
