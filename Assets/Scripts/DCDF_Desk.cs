using System;
using System.Collections.Generic;
using UnityEngine;

public class DCDF_Desk : IComparable
{
	public int id;

	public string name;

	public int minGold;

	public float bet;

	public float minBet;

	public float maxBet;

	public float deltaBet;

	public float[] prizePool;

	public static DCDF_Desk CreateWithDic(Dictionary<string, object> data)
	{
		DCDF_Desk dCDF_Desk = new DCDF_Desk();
		try
		{
			dCDF_Desk.id = (int)data["id"];
			dCDF_Desk.name = (string)data["name"];
			dCDF_Desk.minGold = (int)data["minGold"];
			dCDF_Desk.bet = (float)data["bet"];
			dCDF_Desk.minBet = (float)data["minBet"];
			dCDF_Desk.maxBet = (float)data["maxBet"];
			dCDF_Desk.deltaBet = (float)data["deltaBet"];
			dCDF_Desk.prizePool = (float[])data["prizePool"];
			return dCDF_Desk;
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
			return dCDF_Desk;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is DCDF_Desk)
		{
			DCDF_Desk dCDF_Desk = obj as DCDF_Desk;
			return id - dCDF_Desk.id;
		}
		return 0;
	}
}
