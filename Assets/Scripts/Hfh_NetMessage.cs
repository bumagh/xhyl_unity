using UnityEngine;

public class Hfh_NetMessage : MonoBehaviour
{
	public string method;

	public object[] args;

	public string jsonString;

	public int packetId;

	public int endIndex;

	public Hfh_NetMessage(string method, object[] args)
	{
		this.method = method;
		this.args = args;
	}
}
