using System;
using System.Text.RegularExpressions;
using UnityEngine;

public static class InputCheck
{
	public static bool CheckUserName(string UserName)
	{
		if (UserName.CompareTo(string.Empty) == 0)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Account must be 5-15 characters" : ((ZH2_GVars.language_enum != 0) ? "บ\u0e31ญช\u0e35เกมต\u0e49องม\u0e35ต\u0e31วละคร 5-15 ต\u0e31ว" : "游戏账号必须是5-15个字符"));
			return false;
		}
		if (_calculateCharCount(UserName) > 15 || _calculateCharCount(UserName) < 5)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Account must be 5-15 characters" : ((ZH2_GVars.language_enum != 0) ? "บ\u0e31ญช\u0e35เกมต\u0e49องม\u0e35ต\u0e31วละคร 5-15 ต\u0e31ว" : "游戏账号必须是5-15个字符"));
			return false;
		}
		for (int i = 0; i < UserName.Length; i++)
		{
			if (!_isDigits(UserName[i]) && !_isCharacter(UserName[i]) && !_isUnderline(UserName[i]))
			{
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "The User ID must use letters,numbers or underline" : ((ZH2_GVars.language_enum != 0) ? "หมายเลขบ\u0e31ญช\u0e35ของเกมต\u0e49องถ\u0e39กข\u0e35ดเส\u0e49นใต\u0e49 " : "游戏账号必须使用字母、数字或下划线"));
				return false;
			}
		}
		return true;
	}

	public static bool CheckPresentUserName(string UserName)
	{
		if (_calculateCharCount(UserName) > 15 || _calculateCharCount(UserName) < 1)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Account must be 5-15 characters" : ((ZH2_GVars.language_enum != 0) ? "บ\u0e31ญช\u0e35เกมต\u0e49องม\u0e35ต\u0e31วละคร 5-15 ต\u0e31ว" : "游戏账号必须是5-15个字符"));
			return false;
		}
		for (int i = 0; i < UserName.Length; i++)
		{
			if (!_isDigits(UserName[i]) && !_isCharacter(UserName[i]) && !_isUnderline(UserName[i]))
			{
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "The User ID must use letters,numbers or underline" : ((ZH2_GVars.language_enum != 0) ? "หมายเลขบ\u0e31ญช\u0e35ของเกมต\u0e49องถ\u0e39กข\u0e35ดเส\u0e49นใต\u0e49 " : "游戏账号必须使用字母、数字或下划线"));
				return false;
			}
		}
		return true;
	}

	public static bool CheckLoginUserName(string UserName)
	{
		if (UserName.CompareTo(string.Empty) == 0)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please enter your user ID" : ((ZH2_GVars.language_enum != 0) ? "ไม\u0e48ม\u0e35ช\u0e37\u0e48อผ\u0e39\u0e49ใช\u0e49ว\u0e48า ง  " : "用户名不可为空"));
			return false;
		}
		return true;
	}

	public static bool CheckPassWord(string PassWord)
	{
		if (PassWord == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please enter the password" : ((ZH2_GVars.language_enum != 0) ? "โปรดป\u0e49อนรห\u0e31สผ\u0e48านของเกม" : "请输入游戏密码"));
			return false;
		}
		if (_calculateCharCount(PassWord) > 16 || _calculateCharCount(PassWord) < 6)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Account must be 6-16 characters" : ((ZH2_GVars.language_enum != 0) ? "บ\u0e31ญช\u0e35เกมต\u0e49องม\u0e35ต\u0e31วละคร 6-16 ต\u0e31ว" : "游戏账号必须是6-16个字符"));
			return false;
		}
		for (int i = 0; i < PassWord.Length; i++)
		{
			if (!_isDigits(PassWord[i]) && !_isCharacter(PassWord[i]))
			{
				UnityEngine.Debug.Log("PassWord: " + PassWord);
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Password can only enter letters and numbers" : ((ZH2_GVars.language_enum != 0) ? "รห\u0e31สผ\u0e48านของเกมจะถ\u0e39กป\u0e49อนเป\u0e47นต\u0e31วอ\u0e31กษร และต\u0e31วเลขเท\u0e48าน\u0e31\u0e49น" : "游戏密码只能输入字母和数字"));
				return false;
			}
		}
		int num = 0;
		int num2 = 0;
		for (int j = 0; j < PassWord.Length; j++)
		{
			if (_isDigits(PassWord[j]))
			{
				num++;
			}
			if (_isCharacter(PassWord[j]))
			{
				num2++;
			}
			if (num == PassWord.Length)
			{
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "The password can not be all numbers" : ((ZH2_GVars.language_enum != 0) ? "รห\u0e31สผ\u0e48านไม\u0e48สามารถเป\u0e47นต\u0e31วเลขได\u0e49" : "密码不可全为数字"));
				return false;
			}
			if (num2 == PassWord.Length)
			{
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "The password can not be all letters" : ((ZH2_GVars.language_enum != 0) ? "รห\u0e31สผ\u0e48านไม\u0e48สมบ\u0e39รณ\u0e4c" : "密码不可全为字母"));
				return false;
			}
		}
		return true;
	}

	public static bool CheckReceivePassWord(string PassWord)
	{
		if (PassWord == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please enter the password" : ((ZH2_GVars.language_enum != 0) ? "รห\u0e31สผ\u0e48านว\u0e48า ง" : "密码为空"));
			return false;
		}
		if (_calculateCharCount(PassWord) > 16 || _calculateCharCount(PassWord) < 6)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Password malformed" : ((ZH2_GVars.language_enum != 0) ? "ร\u0e39ปแบบรห\u0e31สผ\u0e48านผ\u0e34ดพลาด" : "密码格式错误"));
			return false;
		}
		for (int i = 0; i < PassWord.Length; i++)
		{
			if (!_isDigits(PassWord[i]) && !_isCharacter(PassWord[i]))
			{
				UnityEngine.Debug.Log("PassWord: " + PassWord);
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Password malformed" : ((ZH2_GVars.language_enum != 0) ? "ร\u0e39ปแบบรห\u0e31สผ\u0e48านผ\u0e34ดพลาด" : "密码格式错误"));
				return false;
			}
		}
		int num = 0;
		int num2 = 0;
		for (int j = 0; j < PassWord.Length; j++)
		{
			if (_isDigits(PassWord[j]))
			{
				num++;
			}
			if (_isCharacter(PassWord[j]))
			{
				num2++;
			}
			if (num == PassWord.Length || num2 == PassWord.Length)
			{
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "The password cannot be composed of numbers all/nThe password cannot be composed of letters all" : ((ZH2_GVars.language_enum != 0) ? "รห\u0e31สผ\u0e48านไม\u0e48สามารถใช\u0e49ได\u0e49ท\u0e31\u0e49งต\u0e31วเลข และต\u0e31วอ\u0e31กษร" : "密码不可全为数字或字母"));
				return false;
			}
		}
		return true;
	}

	public static bool CheckLoginPassWord(string PassWord)
	{
		if (PassWord == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please enter the password" : ((ZH2_GVars.language_enum != 0) ? "โปรดป\u0e49อนรห\u0e31สผ\u0e48านของเกม" : "请输入游戏密码"));
			return false;
		}
		for (int i = 0; i < PassWord.Length; i++)
		{
			if (!_isDigits(PassWord[i]) && !_isCharacter(PassWord[i]))
			{
				UnityEngine.Debug.Log("PassWord: " + PassWord);
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "password error" : ((ZH2_GVars.language_enum != 0) ? "รห\u0e31สผ\u0e48านไม\u0e48ถ\u0e39กต\u0e49อง" : "密码错误"));
				return false;
			}
		}
		return true;
	}

	public static bool CheckChangeNickName(string NickName, Action a)
	{
		if (NickName == string.Empty)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("昵称不可为空", "Please enter your NickName", string.Empty));
			return false;
		}
		if (NickName.Length > 8 || NickName.Length < 1)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("请输入正确长度的昵称", "Please enter a NickName of the correct length", string.Empty));
			return false;
		}
		return true;
	}

	public static bool CheckNickName(string NickName)
	{
		return true;
	}

	public static bool CheckPhoneNumber(string PhoneNumber)
	{
		if (PhoneNumber.CompareTo(string.Empty) == 0)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please enter a phone number" : ((ZH2_GVars.language_enum != 0) ? "กร\u0e38ณาใส\u0e48หมายเลขโทรศ\u0e31พท\u0e4c" : "请输入电话号码"));
			return false;
		}
		if (_calculateCharCount(PhoneNumber) > 20 || _calculateCharCount(PhoneNumber) < 5)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please enter the correct phone number" : ((ZH2_GVars.language_enum != 0) ? "โปรดป\u0e49อนหมายเลข ท\u0e35\u0e48ถ\u0e39กต\u0e49อง" : "请输入正确的电话号码"));
			return false;
		}
		for (int i = 0; i < PhoneNumber.Length; i++)
		{
			if (!Regex.IsMatch(PhoneNumber, "^[a-zA-Z0-9\\u4e00-\\u9fa5]+$"))
			{
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "PhoneNumber must use letters,numbers or Chinese" : ((ZH2_GVars.language_enum != 0) ? "โทรศ\u0e31พท\u0e4cต\u0e49องใช\u0e49ต\u0e31วเลขต\u0e31วเลข" : "电话必须使用字母、数字、汉字"));
				return false;
			}
		}
		return true;
	}

	public static bool _isDigits(char c)
	{
		bool result = false;
		if (c >= '0' && c <= '9')
		{
			result = true;
		}
		return result;
	}

	public static bool _isCharacter(char c)
	{
		bool result = false;
		if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
		{
			result = true;
		}
		return result;
	}

	public static bool _isUnderline(char c)
	{
		bool result = false;
		if (c == '_')
		{
			result = true;
		}
		return result;
	}

	public static int _calculateCharCount(string str)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int i = 0; i < str.Length; i++)
		{
			if (char.IsDigit(str, i))
			{
				num++;
			}
			else if (char.IsWhiteSpace(str.Trim(), i))
			{
				num3++;
			}
			else if (char.ConvertToUtf32(str, i) >= Convert.ToInt32("4e00", 16) && char.ConvertToUtf32(str, i) <= Convert.ToInt32("9fff", 16))
			{
				num4++;
			}
			else if (char.IsLetter(str, i))
			{
				num2++;
			}
			else
			{
				num5++;
			}
		}
		return num + num2 + num3 + num4 * 2 + num5;
	}
}
