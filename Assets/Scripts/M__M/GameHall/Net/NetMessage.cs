namespace M__M.GameHall.Net
{
	public class NetMessage
	{
		public string method;

		public object[] args;

		public string jsonString;

		public int packetId;

		public int endIndex;

		public NetMessage(string method, object[] args)
		{
			this.method = method;
			this.args = args;
		}
	}
}
