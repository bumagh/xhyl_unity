using System;
using System.Collections.Generic;
using UnityEngine;

public class Dzb_Seat : IComparable
{
	public int id;

	public int roomId;

	public string name;

	public int minGold;

	public int exchange;

	public int onceExchangeValue;

	public int mainGameDiff;

	public int sumYaFen;

	public int sumDeFen;

	public int MinBet;

	public int MaxBet;

	public int OpenCardSpeed;

	public int YiDuiWinSpeed;

	public int ErDuiWinSpeed;

	public int SanTiaoWinSpeed;

	public int ShunZiWinSpeed;

	public int TongHuaWinSpeed;

	public int HuLuWinSpeed;

	public int SiTiaoWinSpeed;

	public int TongHuaShunWinSpeed;

	public int TongHuaDaShunWinSpeed;

	public int WuTiaoWinSpeed;

	public int YiDuiOdds;

	public int ErDuiOdds;

	public int SanTiaoOdds;

	public int ShunZiOdds;

	public int TongHuaOdds;

	public int HuLuOdds;

	public int SiTiaoOdds;

	public int TongHuaShunOdds;

	public int TongHuaDaShunOdds;

	public int WuTiaoOdds;

	public int LiuJiTime;

	public int LiuJiScore;

	public int ViewCard;

	public int HeartNum;

	public int ZuanShiNum;

	public int playerid;

	public string playername;

	public bool isKeepTable;

	public bool full;

	public int hour;

	public int minute;

	public int second;

	public static Dzb_Seat CreateWithDic(Dictionary<string, object> data)
	{
		Dzb_Seat dzb_Seat = new Dzb_Seat();
		try
		{
			if (data.ContainsKey("id"))
			{
				dzb_Seat.id = (int)data["id"];
			}
			if (data.ContainsKey("roomId"))
			{
				dzb_Seat.roomId = (int)data["roomId"];
			}
			if (data.ContainsKey("name"))
			{
				dzb_Seat.name = data["name"].ToString();
			}
			if (data.ContainsKey("minGold"))
			{
				dzb_Seat.minGold = (int)data["minGold"];
			}
			if (data.ContainsKey("exchange"))
			{
				dzb_Seat.exchange = (int)data["exchange"];
			}
			if (data.ContainsKey("onceExchangeValue"))
			{
				dzb_Seat.onceExchangeValue = (int)data["onceExchangeValue"];
			}
			if (data.ContainsKey("mainGameDiff"))
			{
				dzb_Seat.mainGameDiff = (int)data["mainGameDiff"];
			}
			if (data.ContainsKey("sumYaFen"))
			{
				dzb_Seat.sumYaFen = (int)data["sumYaFen"];
			}
			if (data.ContainsKey("sumDeFen"))
			{
				dzb_Seat.sumDeFen = (int)data["sumDeFen"];
			}
			if (data.ContainsKey("MinBet"))
			{
				dzb_Seat.MinBet = (int)data["MinBet"];
			}
			if (data.ContainsKey("MaxBet"))
			{
				dzb_Seat.MaxBet = (int)data["MaxBet"];
			}
			if (data.ContainsKey("OpenCardSpeed"))
			{
				dzb_Seat.OpenCardSpeed = (int)data["OpenCardSpeed"];
			}
			if (data.ContainsKey("YiDuiWinSpeed"))
			{
				dzb_Seat.YiDuiWinSpeed = (int)data["YiDuiWinSpeed"];
			}
			if (data.ContainsKey("ErDuiWinSpeed"))
			{
				dzb_Seat.ErDuiWinSpeed = (int)data["ErDuiWinSpeed"];
			}
			if (data.ContainsKey("SanTiaoWinSpeed"))
			{
				dzb_Seat.SanTiaoWinSpeed = (int)data["SanTiaoWinSpeed"];
			}
			if (data.ContainsKey("ShunZiWinSpeed"))
			{
				dzb_Seat.ShunZiWinSpeed = (int)data["ShunZiWinSpeed"];
			}
			if (data.ContainsKey("TongHuaWinSpeed"))
			{
				dzb_Seat.TongHuaWinSpeed = (int)data["TongHuaWinSpeed"];
			}
			if (data.ContainsKey("HuLuWinSpeed"))
			{
				dzb_Seat.HuLuWinSpeed = (int)data["HuLuWinSpeed"];
			}
			if (data.ContainsKey("SiTiaoWinSpeed"))
			{
				dzb_Seat.SiTiaoWinSpeed = (int)data["SiTiaoWinSpeed"];
			}
			if (data.ContainsKey("TongHuaShunWinSpeed"))
			{
				dzb_Seat.TongHuaShunWinSpeed = (int)data["TongHuaShunWinSpeed"];
			}
			if (data.ContainsKey("TongHuaDaShunWinSpeed"))
			{
				dzb_Seat.TongHuaDaShunWinSpeed = (int)data["TongHuaDaShunWinSpeed"];
			}
			if (data.ContainsKey("WuTiaoWinSpeed"))
			{
				dzb_Seat.WuTiaoWinSpeed = (int)data["WuTiaoWinSpeed"];
			}
			if (data.ContainsKey("YiDuiOdds"))
			{
				dzb_Seat.YiDuiOdds = (int)data["YiDuiOdds"];
			}
			if (data.ContainsKey("ErDuiOdds"))
			{
				dzb_Seat.ErDuiOdds = (int)data["ErDuiOdds"];
			}
			if (data.ContainsKey("SanTiaoOdds"))
			{
				dzb_Seat.SanTiaoOdds = (int)data["SanTiaoOdds"];
			}
			if (data.ContainsKey("ShunZiOdds"))
			{
				dzb_Seat.ShunZiOdds = (int)data["ShunZiOdds"];
			}
			if (data.ContainsKey("TongHuaOdds"))
			{
				dzb_Seat.TongHuaOdds = (int)data["TongHuaOdds"];
			}
			if (data.ContainsKey("HuLuOdds"))
			{
				dzb_Seat.HuLuOdds = (int)data["HuLuOdds"];
			}
			if (data.ContainsKey("SiTiaoOdds"))
			{
				dzb_Seat.SiTiaoOdds = (int)data["SiTiaoOdds"];
			}
			if (data.ContainsKey("TongHuaShunOdds"))
			{
				dzb_Seat.TongHuaShunOdds = (int)data["TongHuaShunOdds"];
			}
			if (data.ContainsKey("TongHuaDaShunOdds"))
			{
				dzb_Seat.TongHuaDaShunOdds = (int)data["TongHuaDaShunOdds"];
			}
			if (data.ContainsKey("WuTiaoOdds"))
			{
				dzb_Seat.WuTiaoOdds = (int)data["WuTiaoOdds"];
			}
			if (data.ContainsKey("LiuJiTime"))
			{
				dzb_Seat.LiuJiTime = (int)data["LiuJiTime"];
			}
			if (data.ContainsKey("LiuJiScore"))
			{
				dzb_Seat.LiuJiScore = (int)data["LiuJiScore"];
			}
			if (data.ContainsKey("ViewCard"))
			{
				dzb_Seat.ViewCard = (int)data["ViewCard"];
			}
			if (data.ContainsKey("HeartNum"))
			{
				dzb_Seat.HeartNum = (int)data["HeartNum"];
			}
			if (data.ContainsKey("ZuanShiNum"))
			{
				dzb_Seat.ZuanShiNum = (int)data["ZuanShiNum"];
			}
			Dictionary<string, object> dictionary = data["playerTableInfo"] as Dictionary<string, object>;
			if (dictionary.ContainsKey("id"))
			{
				dzb_Seat.playerid = (int)dictionary["id"];
			}
			if (dictionary.ContainsKey("name"))
			{
				dzb_Seat.playername = dictionary["name"].ToString();
			}
			if (dictionary.ContainsKey("isKeepTable"))
			{
				dzb_Seat.isKeepTable = (bool)dictionary["isKeepTable"];
			}
			if (dictionary.ContainsKey("full"))
			{
				dzb_Seat.full = (bool)dictionary["full"];
			}
			if (!dictionary.ContainsKey("ShengyuTime"))
			{
				return dzb_Seat;
			}
			if (!(dictionary["ShengyuTime"].ToString() != string.Empty))
			{
				return dzb_Seat;
			}
			string[] array = dictionary["ShengyuTime"].ToString().Split(',');
			dzb_Seat.hour = int.Parse(array[0]);
			dzb_Seat.minute = int.Parse(array[1]);
			dzb_Seat.second = int.Parse(array[2]);
			UnityEngine.Debug.Log(dzb_Seat.hour + ":" + dzb_Seat.minute + ":" + dzb_Seat.second);
			return dzb_Seat;
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
			return dzb_Seat;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is Dzb_Room)
		{
			Dzb_Seat dzb_Seat = obj as Dzb_Seat;
			return id - dzb_Seat.id;
		}
		return 0;
	}
}
