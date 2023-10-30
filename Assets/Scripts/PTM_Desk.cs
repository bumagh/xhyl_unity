using System;
using System.Collections.Generic;
using UnityEngine;

public class PTM_Desk : IComparable
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

	public static PTM_Desk CreateWithDic(Dictionary<string, object> data)
	{
		PTM_Desk pTM_Desk = new PTM_Desk();
		try
		{
			pTM_Desk.id = (int)data["id"];
			pTM_Desk.roomId = (int)data["roomId"];
			pTM_Desk.name = (string)data["name"];
			pTM_Desk.minGold = (int)data["minGold"];
			pTM_Desk.exchange = (int)data["exchange"];
			pTM_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			pTM_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			pTM_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			pTM_Desk.singlechangeScore = (int)data["singlechangeScore"];
			pTM_Desk.diceSwitch = (int)data["diceSwitch"];
			pTM_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			pTM_Desk.diceOverflow = (int)data["diceOverflow"];
			pTM_Desk.userId = (int)data["userId"];
			pTM_Desk.userPhotoId = (int)data["userPhotoId"];
			pTM_Desk.nickname = (string)data["nickname"];
			return pTM_Desk;
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
			return pTM_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is PTM_Desk)
		{
			PTM_Desk pTM_Desk = obj as PTM_Desk;
			return id - pTM_Desk.id;
		}
		return 0;
	}
}
