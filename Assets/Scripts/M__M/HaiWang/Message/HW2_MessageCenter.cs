using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Message
{
	public class HW2_MessageCenter : MonoBehaviour
	{
		public delegate void HandleMessage(KeyValueInfo keyValueInfo);

		[HideInInspector]
		public static Dictionary<string, HandleMessage> messageList = new Dictionary<string, HandleMessage>();

		public static void RegisterHandle(string key, HandleMessage handle)
		{
			if (!messageList.ContainsKey(key))
			{
				messageList.Add(key, null);
			}
			Dictionary<string, HandleMessage> dictionary = new Dictionary<string, HandleMessage>();
			(dictionary = messageList)[key] = (HandleMessage)Delegate.Combine(dictionary[key], handle);
		}

		public static void UnRegisterHandle(string key, HandleMessage handle)
		{
			if (messageList.ContainsKey(key))
			{
				Dictionary<string, HandleMessage> dictionary = new Dictionary<string, HandleMessage>();
				(dictionary = messageList)[key] = (HandleMessage)Delegate.Remove(dictionary[key], handle);
			}
		}

		public static void SendMessage(string key, KeyValueInfo keyValueInfo)
		{
			if (messageList.TryGetValue(key, out HandleMessage value))
			{
				value?.Invoke(keyValueInfo);
			}
		}
	}
}
