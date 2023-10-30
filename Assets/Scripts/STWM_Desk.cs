using System;
using System.Collections.Generic;
using UnityEngine;

public class STWM_Desk : IComparable
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

	public static STWM_Desk CreateWithDic(Dictionary<string, object> data)
	{
		STWM_Desk sTWM_Desk = new STWM_Desk();
		try
		{
			sTWM_Desk.id = (int)data["id"];
			sTWM_Desk.roomId = (int)data["roomId"];
			sTWM_Desk.name = (string)data["name"];
			sTWM_Desk.minGold = (int)data["minGold"];
			sTWM_Desk.exchange = (int)data["exchange"];
			sTWM_Desk.onceExchangeValue = (int)data["onceExchangeValue"];
			sTWM_Desk.maxSinglelineBet = (int)data["maxSinglelineBet"];
			sTWM_Desk.minSinglelineBet = (int)data["minSinglelineBet"];
			sTWM_Desk.singlechangeScore = (int)data["singlechangeScore"];
			sTWM_Desk.diceSwitch = (int)data["diceSwitch"];
			sTWM_Desk.diceDirectSwitch = (int)data["diceDirectSwitch"];
			sTWM_Desk.diceOverflow = (int)data["diceOverflow"];
			sTWM_Desk.userId = (int)data["userId"];
			sTWM_Desk.userPhotoId = (int)data["userPhotoId"];
			sTWM_Desk.nickname = (string)data["nickname"];
			return sTWM_Desk;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Desk excpetion: " + ex.Message);
			return sTWM_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is STWM_Desk)
		{
			STWM_Desk sTWM_Desk = obj as STWM_Desk;
			return id - sTWM_Desk.id;
		}
		return 0;
	}
}
