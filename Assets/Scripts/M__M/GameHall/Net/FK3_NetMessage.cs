namespace M__M.GameHall.Net
{
	public class FK3_NetMessage
	{
		public string method;

		public object[] args;

		public string jsonString;

		public int packetId;

		public int endIndex;

		public FK3_NetMessage(string method, object[] args)
		{
			this.method = method;
			this.args = args;
		}
	}
}
