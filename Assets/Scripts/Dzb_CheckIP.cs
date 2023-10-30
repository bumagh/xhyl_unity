using System.Net;

public class Dzb_CheckIP
{
	public static string[] IP_whileList = new string[1]
	{
		"192.168.3.232"
	};

	public static string[] domainName_whileList = new string[1]
	{
		"nasara.cn"
	};

	public static bool CheckInput(string str)
	{
		str = str.Trim();
		bool result = false;
		if (IPAddress.TryParse(str, out IPAddress _))
		{
			for (int i = 0; i < IP_whileList.Length; i++)
			{
				if (str == IP_whileList[i])
				{
					result = true;
				}
			}
		}
		else
		{
			for (int j = 0; j < domainName_whileList.Length; j++)
			{
				if (str.EndsWith(domainName_whileList[j]))
				{
					result = true;
				}
			}
		}
		return result;
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
