public class LLD_NetMessage
{
	public string method;

	public object[] args;

	public string jsonString;

	public int packetId;

	public int endIndex;

	public LLD_NetMessage(string method, object[] args)
	{
		this.method = method;
		this.args = args;
	}
}
