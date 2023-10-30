using System.Collections.Generic;

public static class DCDF_ExtendMethod
{
	public static bool GetBoolValue0(this Dictionary<string, object> dic, string key, bool defaultValue = false)
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

	public static int[] Multiply(this int[] array, int x)
	{
		int num = array.Length;
		int[] array2 = new int[num];
		for (int i = 0; i < num; i++)
		{
			array2[i] = array[i] * x;
		}
		return array2;
	}

	public static int[] Divide(this int[] array, int x)
	{
		x = ((x == 0) ? 1 : x);
		int num = array.Length;
		int[] array2 = new int[num];
		for (int i = 0; i < num; i++)
		{
			array2[i] = array[i] / x;
		}
		return array2;
	}

	public static string Repeat0(this string str, int n)
	{
		string text = string.Empty;
		for (int i = 0; i < n; i++)
		{
			text += str;
		}
		return text;
	}
}
