using System;

namespace M__M.GameHall.Net
{
	[Serializable]
	public class FilterMessageItem
	{
		public bool use;

		public string method;

		public bool Filter(string method)
		{
			return !use || !method.Equals(this.method);
		}
	}
}
