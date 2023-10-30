using System.Net;

public class CheckIP
{
	public static string[] IP_whileList = new string[10]
	{
		"192.168.3.73",
		"l.swccd88.xyz",
		".swccd88.xyz",
		"swccd88.xyz",
		"192.168.3.15",
		"1.14.150.58",
		"csxl.swccd88.xyz",
		"114.117.251.114",
		"192.168.3.113",
		"43.142.73.43"
	};

	public static string[] domainName_whileList = new string[8]
	{
		"l.swccd88.xyz",
		".swccd88.xyz",
		"swccd88.xyz",
		"1.14.150.58",
		"csxl.swccd88.xyz",
		"114.117.251.114",
		"192.168.3.113",
		"43.142.73.43"
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
