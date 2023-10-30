using System;
using System.Collections.Generic;
using System.Text;

namespace M__M.HaiWang.Misc
{
	public static class ExtendMethod
	{
		public static string JoinStrings<T>(this IEnumerable<T> source, Func<T, string> projection, string separator)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (T item in source)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(separator);
				}
				stringBuilder.Append(projection(item));
			}
			return stringBuilder.ToString();
		}

		public static string JoinStrings<T>(this IEnumerable<T> source, string separator = ",")
		{
			return source.JoinStrings((T t) => t.ToString(), separator);
		}

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

		public static string Repeat(this string str, int n)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < n; i++)
			{
				stringBuilder.Append(str);
			}
			return stringBuilder.ToString();
		}
	}
}
