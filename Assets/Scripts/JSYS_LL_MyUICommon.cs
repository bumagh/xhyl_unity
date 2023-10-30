using System;

public sealed class JSYS_LL_MyUICommon
{
	public static string[] gLeverName = new string[16]
	{
		"新手",
		"学徒",
		"新秀",
		"新贵",
		"高手",
		"达人",
		"精英",
		"专家",
		"大师",
		"宗师",
		"盟主",
		"传奇",
		"赌王",
		"赌圣",
		"赌神",
		"至尊"
	};

	public static int CalculateCharCount(string str)
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
			else if (char.IsWhiteSpace(str, i))
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

	public static int CalculateWhiteSpace(string str)
	{
		int num = 0;
		for (int i = 0; i < str.Length; i++)
		{
			if (char.IsWhiteSpace(str[i]))
			{
				num++;
			}
		}
		return num;
	}
}
