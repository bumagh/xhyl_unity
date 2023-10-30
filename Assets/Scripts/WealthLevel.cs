using System;
using System.Collections.Generic;
using UnityEngine;

public class WealthLevel : MonoBehaviour
{
	public int userId;

	public string nickname;

	public int photoId;

	public int gameGoldOrLevel;

	public static WealthLevel CreateWithDic(Dictionary<string, object> data)
	{
		WealthLevel wealthLevel = new WealthLevel();
		try
		{
			wealthLevel.userId = (int)data["userId"];
			wealthLevel.nickname = (string)data["nickname"];
			wealthLevel.photoId = (int)data["photoId"] - 1;
			wealthLevel.gameGoldOrLevel = (int)data["gameGoldOrLevel"];
			return wealthLevel;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("WealthLevel excpetion: " + ex.Message);
			string text = string.Empty;
			foreach (string key in data.Keys)
			{
				text = text + key + ", ";
			}
			UnityEngine.Debug.Log("keys: " + text);
			return wealthLevel;
		}
	}
}
