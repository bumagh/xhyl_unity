using System;
using System.Collections.Generic;
using UnityEngine;

public class Dzb_Room : IComparable
{
	public int roomId;

	public string Roomname;

	public int RoomType;

	public int MinBet;

	public int MaxBet;

	public int NeedCoin;

	public int PeopleNum;

	public int PeopleCount;

	public static Dzb_Room CreateWithDic(Dictionary<string, object> data)
	{
		Dzb_Room dzb_Room = new Dzb_Room();
		try
		{
			dzb_Room.roomId = (int)data["id"];
			dzb_Room.Roomname = data["RoomName"].ToString();
			dzb_Room.MinBet = (int)data["MinBet"];
			dzb_Room.MaxBet = (int)data["MaxBet"];
			dzb_Room.NeedCoin = (int)data["NeedCoin"];
			dzb_Room.PeopleNum = (int)data["PlayerCount"];
			dzb_Room.PeopleCount = (int)data["DeskNum"];
			dzb_Room.RoomType = (int)data["RoomType"];
			return dzb_Room;
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
			return dzb_Room;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is Dzb_Room)
		{
			Dzb_Room dzb_Room = obj as Dzb_Room;
			return roomId - dzb_Room.roomId;
		}
		return 0;
	}
}
