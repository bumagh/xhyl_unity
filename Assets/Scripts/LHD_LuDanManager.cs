using LitJson;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LHD_LuDanManager : MonoBehaviour
{
	private List<int> Result;

	public List<int> Ludan1;

	public List<int> Ludan2;

	private string[] tempLudan2;

	private string[] Ludan2_he;

	public List<int> Ludan3;

	private string[] tempLudan3;

	public List<int> Ludan4;

	private string[] tempLudan4;

	public List<int> Ludan5;

	private string[] tempLudan5;

	public int longNum;

	public int huNum;

	public int heNum;

	public Sprite[] 珠盘路Sprites;

	public Sprite[] 大路Sprites;

	public Sprite[] 大眼路Sprites;

	public Sprite[] 小眼路Sprites;

	public Sprite[] 曱甴路Sprites;

	public static LHD_LuDanManager instance;

	public int LuDan1_NowCount => Ludan1.Count;

	public int LuDan2_NowCount => Ludan2.Count;

	public int LuDan3_NowCount => Ludan3.Count;

	public int LuDan4_NowCount => Ludan4.Count;

	public int LuDan5_NowCount => Ludan5.Count;

	public void GetLuDanInfo(JsonData jd)
	{
		Result = new List<int>();
		Ludan1 = new List<int>();
		Ludan2 = new List<int>();
		Ludan3 = new List<int>();
		Ludan4 = new List<int>();
		Ludan5 = new List<int>();
		longNum = 0;
		huNum = 0;
		heNum = 0;
		int num = 0;
		for (int i = 0; i < jd.Count; i++)
		{
			num = int.Parse(jd[i].ToString());
			Result.Add(num);
			switch (num)
			{
			case 0:
				longNum++;
				break;
			case 2:
				huNum++;
				break;
			default:
				heNum++;
				break;
			}
		}
		for (int j = 0; j < Result.Count; j++)
		{
			Ludan1.Add(Result[j]);
			Ludan2.Add(Result[j]);
		}
		tempLudan2 = new string[253];
		Ludan2_he = new string[253];
		tempLudan3 = new string[253];
		tempLudan4 = new string[253];
		tempLudan5 = new string[253];
		UpdateLuDan();
		LHD_GameInfo.getInstance().updateLuDan?.Invoke();
	}

	private void UpdateLuDan()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < Result.Count; i++)
		{
			if (Result[i] == 0)
			{
				stringBuilder.Append(0);
			}
			else if (Result[i] == 2)
			{
				stringBuilder.Append(1);
			}
		}
		int num = 0;
		int j = 0;
		int[] array = new int[66];
		int num2 = 0;
		bool flag = false;
		for (int k = 0; k < stringBuilder.Length; k++)
		{
			if (k >= 1)
			{
				if ((stringBuilder[k] == '0' && stringBuilder[k - 1] == '0') || (stringBuilder[k] == '2' && stringBuilder[k - 1] == '2'))
				{
					if ((j != 0 && (j + 1) % 6 == 0) || tempLudan2[j + 1] != null)
					{
						if (j >= 216)
						{
							j = 222;
						}
						else
						{
							j += 6;
							flag = true;
						}
					}
					else if (j < 223)
					{
						j++;
					}
				}
				else if (flag)
				{
					for (int l = 0; l < 240; l += 6)
					{
						if (tempLudan2[l] == null)
						{
							j = l;
							break;
						}
					}
					flag = false;
				}
				else if (j >= 216)
				{
					j = 222;
				}
				else
				{
					for (j++; j % 6 != 0; j++)
					{
					}
				}
			}
			else if (IsHe(Result[0]))
			{
				num = 1;
				array[num2] = 0;
			}
			if (k > 0)
			{
				array[num2] = j;
			}
			switch (stringBuilder[k])
			{
			case '0':
				tempLudan2[j] = "Z";
				break;
			case '1':
				tempLudan2[j] = "X";
				break;
			}
			num2++;
		}
		if (Result.Count > 0)
		{
			int[,] array2 = Gethe();
			if (array2 != null)
			{
				for (int m = 1; m < array.Length; m++)
				{
					if (array[m] == 0)
					{
						array[m] = 222;
					}
				}
				if (IsHe(Result[0]))
				{
					Ludan2_he[0] = GetHeBitmap(array2[0, 1], 1);
					for (int n = 1; n < ((array2.Length == 2) ? 1 : (array2.Length / 2)); n++)
					{
						Ludan2_he[array[array2[n, 0] - 2]] = GetHeBitmap(array2[n, 1], 1);
					}
				}
				else
				{
					for (int num3 = 0; num3 < ((array2.Length == 2) ? 1 : (array2.Length / 2)); num3++)
					{
						Ludan2_he[array[(array2[num3, 0] != 0) ? (array2[num3, 0] - 1) : 0]] = GetHeBitmap(array2[num3, 1], 1);
					}
				}
			}
		}
		int[] array3 = new int[66];
		int num4 = 0;
		int num5 = 0;
		flag = false;
		StringBuilder stringBuilder2 = new StringBuilder();
		for (int num6 = 7; num6 < tempLudan2.Length; num6++)
		{
			if (tempLudan2[num6] == null || array3.Contains(num6))
			{
				continue;
			}
			if (num6 % 6 == 0)
			{
				int num7 = 0;
				int num8 = 0;
				for (int num9 = num6 - num6 % 6 - 12; num9 < num6 - num6 % 6 - 12 + 12; num9++)
				{
					if (tempLudan2[num9] != null)
					{
						if (num9 < num6 - num6 % 6 - 6)
						{
							num7++;
						}
						else
						{
							num8++;
						}
					}
				}
				if (num7 == num8)
				{
					stringBuilder2.Append(0);
				}
				else
				{
					stringBuilder2.Append(1);
				}
			}
			else if (tempLudan2[num6 - 6] != null)
			{
				stringBuilder2.Append(0);
			}
			else if (tempLudan2[num6 - 6] == null && tempLudan2[num6 - 7] != null)
			{
				stringBuilder2.Append(1);
			}
			else if (tempLudan2[num6 - 6] == null && tempLudan2[num6 - 7] == null)
			{
				stringBuilder2.Append(0);
			}
			if ((num6 + 1) % 6 == 0)
			{
				int num10 = 6;
				while (tempLudan2[num6] != null && tempLudan2[num6] == tempLudan2[num6 + num10])
				{
					stringBuilder2.Append(0);
					array3[num4] = num6 + num10;
					num4++;
					num10 += 6;
				}
			}
		}
		for (int num11 = 0; num11 < stringBuilder2.Length; num11++)
		{
			if (num11 != 0)
			{
				if (stringBuilder2[num11] != stringBuilder2[num11 - 1])
				{
					if (flag)
					{
						for (int num12 = 0; num12 < 240; num12 += 6)
						{
							if (tempLudan3[num12] == null)
							{
								num5 = num12;
								break;
							}
						}
						flag = false;
					}
					else
					{
						for (num5++; num5 % 6 != 0; num5++)
						{
						}
					}
				}
				else if ((num5 + 1) % 6 == 0 || tempLudan3[num5 + 1] != null)
				{
					num5 += 6;
					flag = true;
				}
				else
				{
					num5++;
				}
				if (num5 > 252)
				{
					num5 = 252;
				}
			}
			if (stringBuilder2[num11] == '0')
			{
				tempLudan3[num5] = "Z";
			}
			else
			{
				tempLudan3[num5] = "X";
			}
		}
		array3 = new int[66];
		num4 = 0;
		int num13 = 0;
		flag = false;
		StringBuilder stringBuilder3 = new StringBuilder();
		for (int num14 = 13; num14 < tempLudan2.Length; num14++)
		{
			if (tempLudan2[num14] == null || array3.Contains(num14))
			{
				continue;
			}
			if (num14 % 6 == 0)
			{
				int num15 = 0;
				int num16 = 0;
				for (int num17 = num14 - num14 % 6 - 18; num17 < num14 - num14 % 6 - 18 + 6; num17++)
				{
					if (tempLudan2[num17] != null)
					{
						num15++;
					}
				}
				for (int num17 = num14 - num14 % 6 - 6; num17 < num14 - num14 % 6 - 6 + 6; num17++)
				{
					if (tempLudan2[num17] != null)
					{
						num16++;
					}
				}
				if (num15 == num16)
				{
					stringBuilder3.Append(0);
				}
				else
				{
					stringBuilder3.Append(1);
				}
			}
			else if (tempLudan2[num14 - 12] != null)
			{
				stringBuilder3.Append(0);
			}
			else if (tempLudan2[num14 - 12] == null && tempLudan2[num14 - 13] != null)
			{
				stringBuilder3.Append(1);
			}
			else if (tempLudan2[num14 - 12] == null && tempLudan2[num14 - 13] == null)
			{
				stringBuilder3.Append(0);
			}
			if ((num14 + 1) % 6 == 0)
			{
				int num18 = 6;
				while (tempLudan2[num14] != null && tempLudan2[num14] == tempLudan2[num14 + num18])
				{
					stringBuilder3.Append(0);
					array3[num4] = num14 + num18;
					num4++;
					num18 += 6;
				}
			}
		}
		for (int num19 = 0; num19 < stringBuilder3.Length; num19++)
		{
			if (num19 != 0)
			{
				if (stringBuilder3[num19] != stringBuilder3[num19 - 1])
				{
					if (flag)
					{
						for (int num20 = 0; num20 < 240; num20 += 6)
						{
							if (tempLudan4[num20] == null)
							{
								num13 = num20;
								break;
							}
						}
						flag = false;
					}
					else
					{
						for (num13++; num13 % 6 != 0; num13++)
						{
						}
					}
				}
				else if ((num13 + 1) % 6 == 0 || tempLudan4[num13 + 1] != null)
				{
					num13 += 6;
					flag = true;
				}
				else
				{
					num13++;
				}
				if (num13 > 252)
				{
					num13 = 252;
				}
			}
			if (stringBuilder3[num19] == '0')
			{
				tempLudan4[num13] = "Z";
			}
			else
			{
				tempLudan4[num13] = "X";
			}
		}
		array3 = new int[66];
		num4 = 0;
		int num21 = 0;
		flag = false;
		StringBuilder stringBuilder4 = new StringBuilder();
		for (int num22 = 19; num22 < tempLudan2.Length; num22++)
		{
			if (tempLudan2[num22] == null || array3.Contains(num22))
			{
				continue;
			}
			if (num22 % 6 == 0)
			{
				int num23 = 0;
				int num24 = 0;
				for (int num25 = num22 - num22 % 6 - 24; num25 < num22 - num22 % 6 - 24 + 6; num25++)
				{
					if (tempLudan2[num25] != null)
					{
						num23++;
					}
				}
				for (int num25 = num22 - num22 % 6 - 6; num25 < num22 - num22 % 6 - 6 + 6; num25++)
				{
					if (tempLudan2[num25] != null)
					{
						num24++;
					}
				}
				if (num23 == num24)
				{
					stringBuilder4.Append(0);
				}
				else
				{
					stringBuilder4.Append(1);
				}
			}
			else if (tempLudan2[num22 - 18] != null)
			{
				stringBuilder4.Append(0);
			}
			else if (tempLudan2[num22 - 18] == null && tempLudan2[num22 - 19] != null)
			{
				stringBuilder4.Append(1);
			}
			else if (tempLudan2[num22 - 18] == null && tempLudan2[num22 - 19] == null)
			{
				stringBuilder4.Append(0);
			}
			if ((num22 + 1) % 6 == 0)
			{
				int num26 = 6;
				while (tempLudan2[num22] != null && tempLudan2[num22] == tempLudan2[num22 + num26])
				{
					stringBuilder4.Append(0);
					array3[num4] = num22 + num26;
					num4++;
					num26 += 6;
				}
			}
		}
		for (int num27 = 0; num27 < stringBuilder4.Length; num27++)
		{
			if (num27 != 0)
			{
				if (stringBuilder4[num27] != stringBuilder4[num27 - 1])
				{
					if (flag)
					{
						for (int num28 = 0; num28 < 253; num28 += 6)
						{
							if (tempLudan5[num28] == null)
							{
								num21 = num28;
								break;
							}
						}
						flag = false;
					}
					else
					{
						for (num21++; num21 % 6 != 0; num21++)
						{
						}
					}
				}
				else if ((num21 + 1) % 6 == 0 || tempLudan5[num21 + 1] != null)
				{
					num21 += 6;
					flag = true;
				}
				else
				{
					num21++;
				}
				if (num21 > 252)
				{
					num21 = 252;
				}
			}
			if (stringBuilder4[num27] == '0')
			{
				tempLudan5[num21] = "Z";
			}
			else
			{
				tempLudan5[num21] = "X";
			}
		}
		for (int num29 = 0; num29 < tempLudan3.Length; num29++)
		{
			if (tempLudan3[num29] != null)
			{
				if (tempLudan3[num29] == "Z")
				{
					Ludan3.Add(0);
				}
				else if (tempLudan3[num29] == "X")
				{
					Ludan3.Add(2);
				}
				else
				{
					UnityEngine.Debug.LogError("其他: " + tempLudan3[num29]);
				}
			}
		}
		for (int num30 = 0; num30 < tempLudan4.Length; num30++)
		{
			if (tempLudan4[num30] != null)
			{
				if (tempLudan4[num30] == "Z")
				{
					Ludan4.Add(0);
				}
				else if (tempLudan4[num30] == "X")
				{
					Ludan4.Add(2);
				}
				else
				{
					UnityEngine.Debug.LogError("其他: " + tempLudan4[num30]);
				}
			}
		}
		for (int num31 = 0; num31 < tempLudan5.Length; num31++)
		{
			if (tempLudan5[num31] != null)
			{
				if (tempLudan5[num31] == "Z")
				{
					Ludan5.Add(0);
				}
				else if (tempLudan5[num31] == "X")
				{
					Ludan5.Add(2);
				}
				else
				{
					UnityEngine.Debug.LogError("其他: " + tempLudan5[num31]);
				}
			}
		}
	}

	private bool IsHe(int result)
	{
		if (result == 1)
		{
			return true;
		}
		return false;
	}

	private int[,] Gethe()
	{
		int num = 0;
		if (IsHe(Result[0]))
		{
			num++;
		}
		for (int i = 1; i < Result.Count; i++)
		{
			if (IsHe(Result[i]) && !IsHe(Result[i - 1]))
			{
				num++;
			}
		}
		int[,] array = null;
		if (num != 0)
		{
			array = new int[num, 2];
		}
		int num2 = 0;
		int num3 = 0;
		num = 0;
		if (Result.Count >= 1 && IsHe(Result[0]))
		{
			num++;
			if (num < Result.Count)
			{
				while (IsHe(Result[num]))
				{
					num3++;
					num++;
					if (num >= Result.Count)
					{
						UnityEngine.Debug.LogError("===跳出===");
						break;
					}
				}
				array[num2, 0] = 0;
				array[num2, 1] = num;
				num = 0;
				num2++;
			}
		}
		for (int i = (num3 == 0) ? 1 : num3; i < Result.Count; i++)
		{
			if (!IsHe(Result[i]) || IsHe(Result[i - 1]))
			{
				continue;
			}
			num3++;
			num++;
			if (i + num < Result.Count)
			{
				while (IsHe(Result[i + num]))
				{
					num++;
					if (i + num >= Result.Count)
					{
						break;
					}
				}
			}
			array[num2, 0] = i - num3 + 1;
			array[num2, 1] = num;
			num3 += num - 1;
			num = 0;
			num2++;
		}
		return array;
	}

	private string GetHeBitmap(int count, int type)
	{
		return count.ToString();
	}

	public void Awake()
	{
		instance = this;
	}
}
