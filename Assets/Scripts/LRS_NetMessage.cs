public class LRS_NetMessage
{
	public string method;

	public object[] args;

	public string jsonString;

	public int packetId;

	public int endIndex;

	public LRS_NetMessage(string method, object[] args)
	{
		this.method = method;
		this.args = args;
	}
}
