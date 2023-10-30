using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivityInfo
{
	public int id;

	public string startDate;

	public string endDate;

	public int activityStatus;

	public static ActivityInfo CreateWithDic(Dictionary<string, object> data)
	{
		ActivityInfo activityInfo = new ActivityInfo();
		try
		{
			activityInfo.startDate = (string)data["startDate"];
			activityInfo.endDate = (string)data["endDate"];
			activityInfo.activityStatus = (int)data["activityStatus"];
			UnityEngine.Debug.LogError("活动状态: " + activityInfo.activityStatus);
			return activityInfo;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("ActivityInfo excpetion: " + ex.Message);
			string text = string.Empty;
			foreach (string key in data.Keys)
			{
				text = text + key + ", ";
			}
			UnityEngine.Debug.Log("keys: " + text);
			return activityInfo;
		}
	}
}
