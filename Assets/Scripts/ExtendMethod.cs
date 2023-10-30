using System.Collections.Generic;

public static class ExtendMethod
{
	public static bool GetBoolValue(this Dictionary<string, object> dic, string key, bool defaultValue = false)
	{
		if (dic.ContainsKey(key))
		{
			return (bool)dic[key];
		}
		return defaultValue;
	}

	public static T GetValue<T>(this Dictionary<string, object> dic, string key, T defaultValue)
	{
		T result = defaultValue;
		if (dic.ContainsKey(key))
		{
			result = (T)dic[key];
		}
		return result;
	}

	public static string Repeat(this string str, int n)
	{
		string text = string.Empty;
		for (int i = 0; i < n; i++)
		{
			text += str;
		}
		return text;
	}
}
