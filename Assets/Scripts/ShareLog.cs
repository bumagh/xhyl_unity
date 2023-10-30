using System;
using System.Collections.Generic;
using UnityEngine;

public class ShareLog : MonoBehaviour
{
	public int UserId;

	public string Username;

	public string bindingName;

	public string bindTime;

	public int countGameGold;

	public int weeklyRecharge;

	public int weeklyExpiry;

	public int wardGameGold;

	public static ShareLog CreateWithDic(Dictionary<string, object> data)
	{
		ShareLog shareLog = new ShareLog();
		try
		{
			shareLog.UserId = (int)data["userId"];
			shareLog.Username = (string)data["username"];
			shareLog.bindingName = (string)data["bindingName"];
			shareLog.bindTime = (string)data["bindingTime"];
			shareLog.countGameGold = (int)data["weekendGameGold"];
			if (data.ContainsKey("weeklyRecharge"))
			{
				shareLog.weeklyRecharge = (int)data["weeklyRecharge"];
			}
			if (data.ContainsKey("weeklyExpiry"))
			{
				shareLog.weeklyExpiry = (int)data["weeklyExpiry"];
			}
			shareLog.wardGameGold = (int)data["weekendAwardGold"];
			return shareLog;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("ShareLog excpetion: " + ex.Message);
			string text = string.Empty;
			foreach (string key in data.Keys)
			{
				text = text + key + ", ";
			}
			UnityEngine.Debug.Log("keys: " + text);
			return shareLog;
		}
	}
}
