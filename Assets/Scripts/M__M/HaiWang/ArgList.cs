using System.Collections.Generic;

namespace M__M.HaiWang
{
	public class ArgList
	{
		private List<object> args;

		private int curPos;

		private ArgList(params object[] args)
		{
			this.args = new List<object>(args.Length);
			this.args.AddRange(args);
		}

		public static ArgList Create(params object[] args)
		{
			return new ArgList(args);
		}

		public T GetAt<T>(int idx)
		{
			if (idx < 0 || idx >= args.Count)
			{
				return default(T);
			}
			return (T)args[idx];
		}

		public T Next<T>()
		{
			return GetAt<T>(curPos++);
		}

		public void Rewind()
		{
			curPos = 0;
		}

		public bool Finish()
		{
			return curPos >= args.Count;
		}
	}
}
