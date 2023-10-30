using System.Collections.Generic;
using UnityEngine;

public static class I2Localization
{
	private static string[] _languages;

	private static HashSet<string> _languageSet;

	private static string _defaultLanguage;

	private static string _curLanguage;

	static I2Localization()
	{
		_languages = new string[3]
		{
			"0",
			"1",
			"2"
		};
		_languageSet = new HashSet<string>();
		string[] languages = _languages;
		foreach (string item in languages)
		{
			_languageSet.Add(item);
		}
		_defaultLanguage = "0";
		_curLanguage = _defaultLanguage;
	}

	public static bool ChangeLanguage(string language)
	{
		bool result = false;
		string[] languages = _languages;
		foreach (string a in languages)
		{
			if (a == language)
			{
				result = true;
				_curLanguage = language;
			}
		}
		return result;
	}

	public static string[] GetAllLanguages()
	{
		return _languages;
	}

	public static string GetCurLanguage()
	{
		return _curLanguage;
	}

	public static void SetCurLanguage(string lang)
	{
		if (_languageSet.Contains(lang))
		{
			_curLanguage = lang;
		}
		else
		{
			UnityEngine.Debug.LogError($"unknown language: [{lang}]");
		}
	}
}
