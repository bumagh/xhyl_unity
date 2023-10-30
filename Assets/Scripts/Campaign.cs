using System;
using System.Collections.Generic;
using UnityEngine;

public class Campaign
{
	public int Id;

	public int type;

	public string content;

	public string content_en;

	public int awardGold;

	public int status;

	public int userSchedule;

	public int targetSchedule;

	public static Campaign CreateWithDic(Dictionary<string, object> data)
	{
		Campaign campaign = new Campaign();
		try
		{
			campaign.Id = (int)data["id"];
			campaign.type = (int)data["type"];
			campaign.content = (string)data["content"];
			campaign.content_en = (string)data["contentEn"];
			campaign.awardGold = (int)data["award"];
			campaign.status = (int)data["status"];
			campaign.userSchedule = (int)data["userSchedule"];
			campaign.targetSchedule = (int)data["targetSchedule"];
			return campaign;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Campaign excpetion: " + ex.Message);
			string text = string.Empty;
			foreach (string key in data.Keys)
			{
				text = text + key + ", ";
			}
			UnityEngine.Debug.Log("keys: " + text);
			return campaign;
		}
	}
}
