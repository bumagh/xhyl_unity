using System.Collections.Generic;
using UnityEngine;

public class TalkData
{
	public static List<string> systemOperator = new List<string>();

	public static Dictionary<string, TalkPlayer> operatorInfos = new Dictionary<string, TalkPlayer>();

	public static Dictionary<TalkPlayer, List<TalkMsg>> talkMsg = new Dictionary<TalkPlayer, List<TalkMsg>>();

	public static bool haveNewMsg
	{
		get
		{
			foreach (KeyValuePair<TalkPlayer, List<TalkMsg>> item in talkMsg)
			{
				if (OperHaveNewMsg(item.Key))
				{
					return true;
				}
			}
			return false;
		}
	}

	public static void AddMsg(TalkPlayer oper, TalkMsg msg)
	{
		if (!talkMsg.ContainsKey(oper))
		{
			talkMsg.Add(oper, new List<TalkMsg>());
		}
		List<TalkMsg> list = talkMsg[oper];
		if (ListContain(list, msg))
		{
			return;
		}
		if (list.Count > 0)
		{
			int num = 0;
			while (true)
			{
				if (num < list.Count)
				{
					if (msg.timeTicks < list[num].timeTicks)
					{
						list.Insert(num, msg);
						return;
					}
					if (num == talkMsg[oper].Count - 1)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			list.Add(msg);
		}
		else
		{
			list.Add(msg);
		}
	}

	private static bool ListContain(List<TalkMsg> list, TalkMsg msg)
	{
		if (list.Contains(msg))
		{
			return true;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Equals(msg))
			{
				return true;
			}
		}
		return false;
	}

	public static void DeleteMsg(TalkPlayer oper, TalkMsg msg)
	{
		if (talkMsg.ContainsKey(oper))
		{
			if (talkMsg[oper].Contains(msg))
			{
				talkMsg[oper].Remove(msg);
			}
			SaveMsg(oper);
		}
	}

	public static void SavePlayerList()
	{
		if (operatorInfos.Count > 0)
		{
			string text = string.Empty;
			int num = 0;
			foreach (KeyValuePair<string, TalkPlayer> operatorInfo in operatorInfos)
			{
				text += operatorInfo.Value.ToString();
				if (num < operatorInfos.Count - 1)
				{
					text += ";";
				}
				num++;
			}
			PlayerPrefs.SetString(ZH2_GVars.user.id + "TalkPlayerList", text);
		}
	}

	public static void LoadPlayerList()
	{
		string @string = PlayerPrefs.GetString(ZH2_GVars.user.id + "TalkPlayerList");
		if (string.IsNullOrEmpty(@string))
		{
		}
	}

	public static void SaveMsg(TalkPlayer oper)
	{
		if (talkMsg.ContainsKey(oper))
		{
			List<TalkMsg> list = talkMsg[oper];
			string text = string.Empty;
			for (int i = 0; i < list.Count; i++)
			{
				text += list[i].ToString();
				if (i < list.Count - 1)
				{
					text += ";";
				}
			}
			PlayerPrefs.SetString(ZH2_GVars.user.id + "EMailTalkList" + oper.username, text);
		}
		SavePlayerList();
	}

	private static void LoadMsg(TalkPlayer oper)
	{
		if (!talkMsg.ContainsKey(oper))
		{
			talkMsg.Add(oper, new List<TalkMsg>());
		}
		List<TalkMsg> operMsgList = GetOperMsgList(oper);
		for (int i = 0; i < operMsgList.Count; i++)
		{
			AddMsg(oper, operMsgList[i]);
		}
	}

	private static List<TalkMsg> GetOperMsgList(TalkPlayer oper)
	{
		string @string = PlayerPrefs.GetString(ZH2_GVars.user.id + "EMailTalkList" + oper.username);
		return new List<TalkMsg>();
	}

	public static void ReadOperMsg(TalkPlayer oper)
	{
		bool flag = false;
		if (talkMsg.ContainsKey(oper))
		{
			List<TalkMsg> list = talkMsg[oper];
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].isRead)
				{
					list[i].isRead = true;
					flag = true;
				}
			}
		}
		if (flag)
		{
			SaveMsg(oper);
		}
	}

	public static bool OperHaveNewMsg(TalkPlayer oper)
	{
		if (talkMsg.ContainsKey(oper))
		{
			List<TalkMsg> list = talkMsg[oper];
			if (list.Count > 0 && !list[list.Count - 1].isRead)
			{
				return true;
			}
		}
		return false;
	}

	public static void AddOperator(TalkPlayer oper)
	{
		if (oper != null)
		{
			if (GetOperMsgList(oper).Count <= 0 && !systemOperator.Contains(oper.username))
			{
				operatorInfos.Remove(oper.username);
				talkMsg.Remove(oper);
			}
			else if (!operatorInfos.ContainsKey(oper.username))
			{
				operatorInfos.Add(oper.username, oper);
				LoadMsg(oper);
			}
			else
			{
				operatorInfos[oper.username].Copy(oper);
				LoadMsg(operatorInfos[oper.username]);
			}
		}
	}

	public static void ReorderPlayeList(TalkPlayer firstPlayer)
	{
		Dictionary<string, TalkPlayer> dictionary = new Dictionary<string, TalkPlayer>();
		dictionary.Add(firstPlayer.username, firstPlayer);
		foreach (KeyValuePair<string, TalkPlayer> operatorInfo in operatorInfos)
		{
			if (!dictionary.ContainsKey(operatorInfo.Key))
			{
				dictionary.Add(operatorInfo.Key, operatorInfo.Value);
			}
		}
		operatorInfos = dictionary;
	}

	public static void Clear()
	{
		operatorInfos.Clear();
		talkMsg.Clear();
	}
}
