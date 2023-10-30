using System;
using System.Collections.Generic;
using UnityEngine;

public class MSE_Desk : IComparable
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

	public static MSE_Desk CreateWithDic(Dictionary<string, object> data)
	{
		MSE_Desk mSE_Desk = new MSE_Desk();
		try
		{
			mSE_Desk.id = (int)data["id"];
			mSE_Desk.roomId = (int)data["roomId"];
			mSE_Desk.name = (string)data["name"];
			mSE_Desk.minGold = (int)data["minGold"];
			mSE_Desk.exchange = (int)data["exchange"];
			mSE_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			mSE_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			mSE_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			mSE_Desk.singlechangeScore = (int)data["singlechangeScore"];
			mSE_Desk.diceSwitch = (int)data["diceSwitch"];
			mSE_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			mSE_Desk.diceOverflow = (int)data["diceOverflow"];
			mSE_Desk.userId = (int)data["userId"];
			mSE_Desk.userPhotoId = (int)data["userPhotoId"];
			mSE_Desk.nickname = (string)data["nickname"];
			return mSE_Desk;
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
			return mSE_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is MSE_Desk)
		{
			MSE_Desk mSE_Desk = obj as MSE_Desk;
			return id - mSE_Desk.id;
		}
		return 0;
	}
}
