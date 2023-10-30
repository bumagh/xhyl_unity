using System;

namespace M__M.GameHall.Net
{
	[Serializable]
	public class FK3_FilterMessageItem
	{
		public bool use;

		public string method;

		public bool Filter(string method)
		{
			return !use || !method.Equals(this.method);
		}
	}
}
