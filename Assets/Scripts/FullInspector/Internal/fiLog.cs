using FullSerializer;
using System;
using System.Collections.Generic;

namespace FullInspector.Internal
{
	public static class fiLog
	{
		private static readonly List<string> _messages = new List<string>();

		public static void InsertAndClearMessagesTo(List<string> buffer)
		{
			lock (typeof(fiLog))
			{
				buffer.AddRange(_messages);
				_messages.Clear();
			}
		}

		public static void Blank()
		{
			if (fiSettings.EnableLogs)
			{
				lock (typeof(fiLog))
				{
					_messages.Add(string.Empty);
				}
			}
		}

		private static string GetTag(object tag)
		{
			if (tag == null)
			{
				return string.Empty;
			}
			if (tag is string)
			{
				return (string)tag;
			}
			if (tag is Type)
			{
				return "[" + ((Type)tag).CSharpName() + "]: ";
			}
			return "[" + tag.GetType().CSharpName() + "]: ";
		}

		public static void Log(object tag, string message)
		{
			if (fiSettings.EnableLogs)
			{
				string item = GetTag(tag) + message;
				lock (typeof(fiLog))
				{
					_messages.Add(item);
				}
			}
		}

		public static void Log(object tag, string format, object arg0)
		{
			if (fiSettings.EnableLogs)
			{
				string item = GetTag(tag) + string.Format(format, arg0);
				lock (typeof(fiLog))
				{
					_messages.Add(item);
				}
			}
		}

		public static void Log(object tag, string format, object arg0, object arg1)
		{
			if (fiSettings.EnableLogs)
			{
				string item = GetTag(tag) + string.Format(format, arg0, arg1);
				lock (typeof(fiLog))
				{
					_messages.Add(item);
				}
			}
		}

		public static void Log(object tag, string format, object arg0, object arg1, object arg2)
		{
			if (fiSettings.EnableLogs)
			{
				string item = GetTag(tag) + string.Format(format, arg0, arg1, arg2);
				lock (typeof(fiLog))
				{
					_messages.Add(item);
				}
			}
		}

		public static void Log(object tag, string format, params object[] args)
		{
			if (fiSettings.EnableLogs)
			{
				string item = GetTag(tag) + string.Format(format, args);
				lock (typeof(fiLog))
				{
					_messages.Add(item);
				}
			}
		}
	}
}
