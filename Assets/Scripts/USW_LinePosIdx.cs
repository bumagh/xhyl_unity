using System.Collections.Generic;
using UnityEngine;

public class USW_LinePosIdx : MonoBehaviour
{
	public static string[] Lineposlist = new string[50]
	{
		"(1)000001111100000",
		"(2)111110000000000",
		"(3)000000000011111",
		"(4)100010101000100",
		"(5)001000101010001",
		"(6)110110010000000",
		"(7)000000010011011",
		"(8)011101000100000",
		"(9)000001000101110",
		"(10)010001010100010",
		"(11)000101010101000",
		"(12)100010111000000",
		"(13)000000111010001",
		"(14)101010101000000",
		"(15)000000101010101",
		"(16)001001101100000",
		"(17)000001101100100",
		"(18)110110000000100",
		"(19)001000000011011",
		"(20)100010000001110",
		"(21)011100000010001",
		"(22)010101000100100",
		"(23)001001000101010",
		"(24)101010000001010",
		"(25)010100000010101",
		"(26)100100010001001",
		"(27)010010010010010",
		"(28)010001001000101",
		"(29)100010010001010",
		"(30)001100100110000",
		"(31)100000111000001",
		"(32)000010111010000",
		"(33)100000101000101",
		"(34)001010101010000",
		"(35)111100000100000",
		"(36)000000000111110",
		"(37)101000101000001",
		"(38)000010101010100",
		"(39)010001010100010",
		"(40)000101010101000",
		"(41)001111100000000",
		"(42)000001100000111",
		"(43)011001001000001",
		"(44)000011001001100",
		"(45)010001010000011",
		"(46)000111010001000",
		"(47)001100100110000",
		"(48)100000100100110",
		"(49)110000010100010",
		"(50)000100010111000"
	};

	public static List<int> Getwinlinepos(int lineidx, int conut, bool left = true)
	{
		List<int> list = new List<int>();
		int num = 0;
		string text = Lineposlist[lineidx].Replace($"({lineidx + 1:0})", string.Empty);
		if (left)
		{
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					int num2 = i + j * 5;
					if (text[num2] == '1')
					{
						list.Add(num2);
						num++;
						if (num >= conut)
						{
							return list;
						}
					}
				}
			}
		}
		else
		{
			for (int num3 = 4; num3 >= 0; num3--)
			{
				for (int k = 0; k < 3; k++)
				{
					int num4 = num3 + k * 5;
					if (text[num4] == '1')
					{
						list.Add(num4);
						num++;
						if (num >= conut)
						{
							return list;
						}
					}
				}
			}
		}
		return list;
	}
}
