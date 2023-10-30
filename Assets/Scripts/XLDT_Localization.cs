using System.Collections.Generic;
using UnityEngine;

public static class XLDT_Localization
{
	private static Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();

	private static int mLanguage;

	public static int language
	{
		get
		{
			return mLanguage;
		}
		set
		{
			if (mLanguage != value)
			{
				mLanguage = value;
			}
		}
	}

	public static void LoadTxt(TextAsset asset)
	{
		string[] array = asset.text.Split('\n');
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(',');
			string key = array2[0];
			string[] value = new string[2]
			{
				array2[1],
				array2[2]
			};
			if (!mDictionary.ContainsKey(key))
			{
				mDictionary.Add(key, value);
			}
		}
	}

	public static string Get(string str)
	{
		string[] value = new string[2];
		if (mDictionary.TryGetValue(str, out value))
		{
			return value[mLanguage];
		}
		return null;
	}
}
