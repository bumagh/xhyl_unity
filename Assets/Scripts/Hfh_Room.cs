using System;
using System.Collections.Generic;
using UnityEngine;

public class Hfh_Room : IComparable
{
	public int roomId;

	public string Roomname;

	public int RoomType;

	public int MinBet;

	public int MaxBet;

	public int NeedCoin;

	public int PeopleNum;

	public int PeopleCount;

	public static Hfh_Room CreateWithDic(Dictionary<string, object> data)
	{
		Hfh_Room hfh_Room = new Hfh_Room();
		try
		{
			hfh_Room.roomId = (int)data["id"];
			hfh_Room.Roomname = data["RoomName"].ToString();
			hfh_Room.MinBet = (int)data["MinBet"];
			hfh_Room.MaxBet = (int)data["MaxBet"];
			hfh_Room.NeedCoin = (int)data["NeedCoin"];
			hfh_Room.PeopleNum = (int)data["PlayerCount"];
			hfh_Room.PeopleCount = (int)data["DeskNum"];
			hfh_Room.RoomType = (int)data["RoomType"];
			return hfh_Room;
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
			return hfh_Room;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is Hfh_Room)
		{
			Hfh_Room hfh_Room = obj as Hfh_Room;
			return roomId - hfh_Room.roomId;
		}
		return 0;
	}
}
