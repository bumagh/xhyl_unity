using System;
using System.Collections.Generic;
using UnityEngine;

public class Dzb_Desk : IComparable
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

	public static Dzb_Desk CreateWithDic(Dictionary<string, object> data)
	{
		Dzb_Desk dzb_Desk = new Dzb_Desk();
		try
		{
			dzb_Desk.id = (int)data["id"];
			dzb_Desk.roomId = (int)data["roomId"];
			dzb_Desk.name = (string)data["name"];
			dzb_Desk.minGold = (int)data["minGold"];
			dzb_Desk.exchange = (int)data["exchange"];
			dzb_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			dzb_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			dzb_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			dzb_Desk.singlechangeScore = (int)data["singlechangeScore"];
			dzb_Desk.diceSwitch = (int)data["diceSwitch"];
			dzb_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			dzb_Desk.diceOverflow = (int)data["diceOverflow"];
			dzb_Desk.userId = (int)data["userId"];
			dzb_Desk.userPhotoId = (int)data["userPhotoId"];
			dzb_Desk.nickname = (string)data["nickname"];
			return dzb_Desk;
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
			return dzb_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is Dzb_Desk)
		{
			Dzb_Desk dzb_Desk = obj as Dzb_Desk;
			return id - dzb_Desk.id;
		}
		return 0;
	}
}
