using System;
using System.Collections.Generic;
using UnityEngine;

public class Hfh_Seat : IComparable
{
	public int id;

	public int roomId;

	public string name;

	public int minGold;

	public int exchange;

	public int onceExchangeValue;

	public int mainGameDiff;

	public int MinBet;

	public int MaxBet;

	public int OpenCardSpeed;

	public int YiDuiWinSpeed;

	public int ErDuiWinSpeed;

	public int SanTiaoWinSpeed;

	public int ShunZiWinSpeed;

	public int TongHuaWinSpeed;

	public int HuLuWinSpeed;

	public int XiaoSiMeiWinSpeed;

	public int DaSiMeiWinSpeed;

	public int TongHuaShunWinSpeed;

	public int TongHuaDaShunWinSpeed;

	public int WuTiaoWinSpeed;

	public int WuGuiWinSpeed;

	public int YiDuiOdds;

	public int ErDuiOdds;

	public int SanTiaoOdds;

	public int ShunZiOdds;

	public int TongHuaOdds;

	public int HuLuOdds;

	public int XiaoSiMeiOdds;

	public int DaSiMeiOdds;

	public int TongHuaShunOdds;

	public int WuTiaoOdds;

	public int TongHuaDaShunOdds;

	public int WuGuiOdds;

	public int LiuJiTime;

	public int LiuJiScore;

	public int BiBeiBaoJi;

	public int BaoJiGold;

	public int GuoGuanSongGold;

	public int GuoGuanMax;

	public int playerid;

	public string playername;

	public bool isKeepTable;

	public bool full;

	public int hour;

	public int minute;

	public int second;

	public static Hfh_Seat CreateWithDic(Dictionary<string, object> data)
	{
		Hfh_Seat hfh_Seat = new Hfh_Seat();
		try
		{
			if (data.ContainsKey("id"))
			{
				hfh_Seat.id = (int)data["id"];
			}
			if (data.ContainsKey("roomId"))
			{
				hfh_Seat.roomId = (int)data["roomId"];
			}
			if (data.ContainsKey("name"))
			{
				hfh_Seat.name = data["name"].ToString();
			}
			if (data.ContainsKey("minGold"))
			{
				hfh_Seat.minGold = (int)data["minGold"];
			}
			if (data.ContainsKey("exchange"))
			{
				hfh_Seat.exchange = (int)data["exchange"];
			}
			if (data.ContainsKey("onceExchangeValue"))
			{
				hfh_Seat.onceExchangeValue = (int)data["onceExchangeValue"];
			}
			if (data.ContainsKey("mainGameDiff"))
			{
				hfh_Seat.mainGameDiff = (int)data["mainGameDiff"];
			}
			if (data.ContainsKey("MinBet"))
			{
				hfh_Seat.MinBet = (int)data["MinBet"];
			}
			if (data.ContainsKey("MaxBet"))
			{
				hfh_Seat.MaxBet = (int)data["MaxBet"];
			}
			if (data.ContainsKey("OpenCardSpeed"))
			{
				hfh_Seat.OpenCardSpeed = (int)data["OpenCardSpeed"];
			}
			if (data.ContainsKey("YiDuiWinSpeed"))
			{
				hfh_Seat.YiDuiWinSpeed = (int)data["YiDuiWinSpeed"];
			}
			if (data.ContainsKey("ErDuiWinSpeed"))
			{
				hfh_Seat.ErDuiWinSpeed = (int)data["ErDuiWinSpeed"];
			}
			if (data.ContainsKey("SanTiaoWinSpeed"))
			{
				hfh_Seat.SanTiaoWinSpeed = (int)data["SanTiaoWinSpeed"];
			}
			if (data.ContainsKey("ShunZiWinSpeed"))
			{
				hfh_Seat.ShunZiWinSpeed = (int)data["ShunZiWinSpeed"];
			}
			if (data.ContainsKey("TongHuaWinSpeed"))
			{
				hfh_Seat.TongHuaWinSpeed = (int)data["TongHuaWinSpeed"];
			}
			if (data.ContainsKey("HuLuWinSpeed"))
			{
				hfh_Seat.HuLuWinSpeed = (int)data["HuLuWinSpeed"];
			}
			if (data.ContainsKey("XiaoSiMeiWinSpeed"))
			{
				hfh_Seat.XiaoSiMeiWinSpeed = (int)data["XiaoSiMeiWinSpeed"];
			}
			if (data.ContainsKey("DaSiMeiWinSpeed"))
			{
				hfh_Seat.DaSiMeiWinSpeed = (int)data["DaSiMeiWinSpeed"];
			}
			if (data.ContainsKey("TongHuaShunWinSpeed"))
			{
				hfh_Seat.TongHuaShunWinSpeed = (int)data["TongHuaShunWinSpeed"];
			}
			if (data.ContainsKey("WuTiaoWinSpeed"))
			{
				hfh_Seat.WuTiaoWinSpeed = (int)data["WuTiaoWinSpeed"];
			}
			if (data.ContainsKey("TongHuaDaShunWinSpeed"))
			{
				hfh_Seat.TongHuaDaShunWinSpeed = (int)data["TongHuaDaShunWinSpeed"];
			}
			if (data.ContainsKey("WuGuiWinSpeed"))
			{
				hfh_Seat.WuGuiWinSpeed = (int)data["WuGuiWinSpeed"];
			}
			if (data.ContainsKey("YiDuiOdds"))
			{
				hfh_Seat.YiDuiOdds = (int)data["YiDuiOdds"];
			}
			if (data.ContainsKey("ErDuiOdds"))
			{
				hfh_Seat.ErDuiOdds = (int)data["ErDuiOdds"];
			}
			if (data.ContainsKey("SanTiaoOdds"))
			{
				hfh_Seat.SanTiaoOdds = (int)data["SanTiaoOdds"];
			}
			if (data.ContainsKey("ShunZiOdds"))
			{
				hfh_Seat.ShunZiOdds = (int)data["ShunZiOdds"];
			}
			if (data.ContainsKey("TongHuaOdds"))
			{
				hfh_Seat.TongHuaOdds = (int)data["TongHuaOdds"];
			}
			if (data.ContainsKey("HuLuOdds"))
			{
				hfh_Seat.HuLuOdds = (int)data["HuLuOdds"];
			}
			if (data.ContainsKey("XiaoSiMeiOdds"))
			{
				hfh_Seat.XiaoSiMeiOdds = (int)data["XiaoSiMeiOdds"];
			}
			if (data.ContainsKey("DaSiMeiOdds"))
			{
				hfh_Seat.DaSiMeiOdds = (int)data["DaSiMeiOdds"];
			}
			if (data.ContainsKey("TongHuaShunOdds"))
			{
				hfh_Seat.TongHuaShunOdds = (int)data["TongHuaShunOdds"];
			}
			if (data.ContainsKey("WuTiaoOdds"))
			{
				hfh_Seat.WuTiaoOdds = (int)data["WuTiaoOdds"];
			}
			if (data.ContainsKey("TongHuaDaShunOdds"))
			{
				hfh_Seat.TongHuaDaShunOdds = (int)data["TongHuaDaShunOdds"];
			}
			if (data.ContainsKey("WuGuiOdds"))
			{
				hfh_Seat.WuGuiOdds = (int)data["WuGuiOdds"];
			}
			if (data.ContainsKey("LiuJiTime"))
			{
				hfh_Seat.LiuJiTime = (int)data["LiuJiTime"];
			}
			if (data.ContainsKey("LiuJiScore"))
			{
				hfh_Seat.LiuJiScore = (int)data["LiuJiScore"];
			}
			if (data.ContainsKey("BiBeiBaoJi"))
			{
				hfh_Seat.BiBeiBaoJi = (int)data["BiBeiBaoJi"];
			}
			if (data.ContainsKey("BaoJiGold"))
			{
				hfh_Seat.BaoJiGold = (int)data["BaoJiGold"];
			}
			if (data.ContainsKey("GuoGuanSongGold"))
			{
				hfh_Seat.GuoGuanSongGold = (int)data["GuoGuanSongGold"];
			}
			if (data.ContainsKey("GuoGuanMax"))
			{
				hfh_Seat.GuoGuanMax = (int)data["GuoGuanMax"];
			}
			Dictionary<string, object> dictionary = data["playerTableInfo"] as Dictionary<string, object>;
			if (dictionary.ContainsKey("id"))
			{
				hfh_Seat.playerid = (int)dictionary["id"];
			}
			if (dictionary.ContainsKey("name"))
			{
				hfh_Seat.playername = dictionary["name"].ToString();
			}
			if (dictionary.ContainsKey("isKeepTable"))
			{
				hfh_Seat.isKeepTable = (bool)dictionary["isKeepTable"];
			}
			if (dictionary.ContainsKey("full"))
			{
				hfh_Seat.full = (bool)dictionary["full"];
			}
			if (!dictionary.ContainsKey("ShengyuTime"))
			{
				return hfh_Seat;
			}
			if (!(dictionary["ShengyuTime"].ToString() != string.Empty))
			{
				return hfh_Seat;
			}
			string[] array = dictionary["ShengyuTime"].ToString().Split(',');
			hfh_Seat.hour = int.Parse(array[0]);
			hfh_Seat.minute = int.Parse(array[1]);
			hfh_Seat.second = int.Parse(array[2]);
			UnityEngine.Debug.Log(hfh_Seat.hour + ":" + hfh_Seat.minute + ":" + hfh_Seat.second);
			return hfh_Seat;
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
			return hfh_Seat;
		}
	}

	public int CompareTo(object obj)
	{
		if (obj is Hfh_Room)
		{
			Hfh_Seat hfh_Seat = obj as Hfh_Seat;
			return id - hfh_Seat.id;
		}
		return 0;
	}
}
