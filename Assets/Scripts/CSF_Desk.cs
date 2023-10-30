using System;
using System.Collections.Generic;
using UnityEngine;

public class CSF_Desk : IComparable
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

	public static CSF_Desk CreateWithDic(Dictionary<string, object> data)
	{
		CSF_Desk cSF_Desk = new CSF_Desk();
		try
		{
			cSF_Desk.id = (int)data["id"];
			cSF_Desk.roomId = (int)data["roomId"];
			cSF_Desk.name = (string)data["name"];
			cSF_Desk.minGold = (int)data["minGold"];
			cSF_Desk.exchange = (int)data["exchange"];
			cSF_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			cSF_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			cSF_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			cSF_Desk.singlechangeScore = (int)data["singlechangeScore"];
			cSF_Desk.diceSwitch = (int)data["diceSwitch"];
			cSF_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			cSF_Desk.diceOverflow = (int)data["diceOverflow"];
			cSF_Desk.userId = (int)data["userId"];
			cSF_Desk.userPhotoId = (int)data["userPhotoId"];
			cSF_Desk.nickname = (string)data["nickname"];
			return cSF_Desk;
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
			return cSF_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is CSF_Desk)
		{
			CSF_Desk cSF_Desk = obj as CSF_Desk;
			return id - cSF_Desk.id;
		}
		return 0;
	}
}
