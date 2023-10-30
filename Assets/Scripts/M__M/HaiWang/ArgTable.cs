using System.Collections.Generic;

namespace M__M.HaiWang
{
	public class ArgTable
	{
		private Dictionary<string, object> args;

		public Dictionary<string, object>.ValueCollection Values => args.Values;

		private ArgTable()
		{
			args = new Dictionary<string, object>();
		}

		public static ArgTable Create()
		{
			return new ArgTable();
		}

		public ArgTable Add(string key, object value)
		{
			if (args.ContainsKey(key))
			{
				args[key] = value;
			}
			else
			{
				args.Add(key, value);
			}
			return this;
		}

		public T Get<T>(string key)
		{
			if (args.ContainsKey(key))
			{
				return (T)args[key];
			}
			return default(T);
		}
	}
}
