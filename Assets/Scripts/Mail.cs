using System;
using System.Collections.Generic;
using UnityEngine;

public class Mail
{
	public int mailId;

	public string mailName;

	public string mailSendPeople;

	public string mailTime;

	public string mailContent;

	public int mailIsReadStatus;

	public int gainStatus;

	public static Mail CreateWithDic(Dictionary<string, object> data)
	{
		Mail mail = new Mail();
		try
		{
			mail.mailName = (string)data["title"];
			mail.mailContent = (string)data["content"];
			mail.mailTime = (string)data["datetime"];
			mail.mailSendPeople = (string)data["sender"];
			mail.mailIsReadStatus = (int)data["status"];
			mail.mailId = (int)data["id"];
			mail.gainStatus = (int)data["gainStatus"];
			return mail;
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
			return mail;
		}
	}
}
