using System;
using System.Collections.Generic;
using UnityEngine;

public class SPA_Desk : IComparable
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

	public static SPA_Desk CreateWithDic(Dictionary<string, object> data)
	{
		SPA_Desk sPA_Desk = new SPA_Desk();
		try
		{
			sPA_Desk.id = (int)data["id"];
			sPA_Desk.roomId = (int)data["roomId"];
			sPA_Desk.name = (string)data["name"];
			sPA_Desk.minGold = (int)data["minGold"];
			sPA_Desk.exchange = (int)data["exchange"];
			sPA_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			sPA_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			sPA_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			sPA_Desk.singlechangeScore = (int)data["singlechangeScore"];
			sPA_Desk.diceSwitch = (int)data["diceSwitch"];
			sPA_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			sPA_Desk.diceOverflow = (int)data["diceOverflow"];
			sPA_Desk.userId = (int)data["userId"];
			sPA_Desk.userPhotoId = (int)data["userPhotoId"];
			sPA_Desk.nickname = (string)data["nickname"];
			return sPA_Desk;
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
			return sPA_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is SPA_Desk)
		{
			SPA_Desk sPA_Desk = obj as SPA_Desk;
			return id - sPA_Desk.id;
		}
		return 0;
	}
}
