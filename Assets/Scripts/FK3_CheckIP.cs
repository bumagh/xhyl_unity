using System.Net;

public class FK3_CheckIP
{
	public static string[] IP_whileList = new string[6]
	{
		"192.168.3.73",
		"l.swccd88.xyz",
		".swccd88.xyz",
		"swccd88.xyz",
		"192.168.3.15",
		"114.117.251.114"
	};

	public static string[] domainName_whileList = new string[3]
	{
		"l.swccd88.xyz",
		".swccd88.xyz",
		"swccd88.xyz"
	};

	public static bool CheckInput(string str)
	{
		return true;
	}

	public static string DoGetHostAddresses(string hostname)
	{
		if (IPAddress.TryParse(hostname, out IPAddress address))
		{
			return address.ToString();
		}
		return Dns.GetHostEntry(hostname).AddressList[0].ToString();
	}
}
