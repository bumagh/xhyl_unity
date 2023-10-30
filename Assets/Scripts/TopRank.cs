using System;
using System.Collections.Generic;
using UnityEngine;

public class TopRank
{
	public int userId;

	public string nickname;

	public int gold;

	public string awardName;

	public string datetime;

	public static TopRank CreateWithDic(Dictionary<string, object> data)
	{
		TopRank topRank = new TopRank();
		try
		{
			topRank.userId = (int)data["userId"];
			topRank.nickname = (string)data["nickname"];
			topRank.gold = (int)data["gold"];
			topRank.awardName = (string)data["awardName"];
			topRank.datetime = (string)data["datetime"];
			return topRank;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Mail excpetion: " + ex.Message);
			string text = string.Empty;
			foreach (string key in data.Keys)
			{
				text = text + key + ", ";
			}
			UnityEngine.Debug.Log("keys: " + text);
			return topRank;
		}
	}
}
