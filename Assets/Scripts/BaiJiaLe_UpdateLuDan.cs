using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BaiJiaLe_UpdateLuDan : MonoBehaviour
{
	[Serializable]
	public class BaiJiaLeLuDan
	{
		private string[] Result;

		public string[] Ludan1;

		public int LuDan1_NowCount;

		public string[] Ludan2;

		public string[] Ludan2_he;

		public int LuDan2_NowCount;

		public string[] Ludan3;

		public int LuDan3_NowCount;

		public string[] Ludan4;

		public int LuDan4_NowCount;

		public string[] Ludan5;

		public int LuDan5_NowCount;

		public BaiJiaLeLuDan(string[] result)
		{
			Result = result;
			Ludan1 = new string[253];
			Ludan2 = new string[253];
			Ludan2_he = new string[253];
			Ludan3 = new string[253];
			Ludan4 = new string[253];
			Ludan5 = new string[253];
			UpdateLuDan();
		}

		private void UpdateLuDan()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < Result.Length; i++)
			{
				switch (Result[i])
				{
				case "0":
				case "3":
				case "6":
				case "9":
					stringBuilder.Append(0);
					break;
				case "1":
				case "4":
				case "7":
				case "a":
					stringBuilder.Append(1);
					break;
				}
			}
			int num = 0;
			int j = 0;
			int[] array = new int[84];
			int num2 = 0;
			bool flag = false;
			for (int k = 0; k < stringBuilder.Length; k++)
			{
				if (k >= 1)
				{
					if ((stringBuilder[k] == '0' && stringBuilder[k - 1] == '0') || (stringBuilder[k] == '1' && stringBuilder[k - 1] == '1'))
					{
						if ((j != 0 && (j + 1) % 6 == 0) || Ludan2[j + 1] != null)
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
							if (Ludan2[l] == null)
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
					Ludan2[j] = "Z";
					break;
				case '1':
					Ludan2[j] = "X";
					break;
				}
				num2++;
			}
			if (Result.Length > 0)
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
			for (int num6 = 7; num6 < Ludan2.Length; num6++)
			{
				if (Ludan2[num6] == null || array3.Contains(num6))
				{
					continue;
				}
				if (num6 % 6 == 0)
				{
					int num7 = 0;
					int num8 = 0;
					for (int num9 = num6 - num6 % 6 - 12; num9 < num6 - num6 % 6 - 12 + 12; num9++)
					{
						if (Ludan2[num9] != null)
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
				else if (Ludan2[num6 - 6] != null)
				{
					stringBuilder2.Append(0);
				}
				else if (Ludan2[num6 - 6] == null && Ludan2[num6 - 7] != null)
				{
					stringBuilder2.Append(1);
				}
				else if (Ludan2[num6 - 6] == null && Ludan2[num6 - 7] == null)
				{
					stringBuilder2.Append(0);
				}
				if ((num6 + 1) % 6 == 0)
				{
					int num10 = 6;
					while (Ludan2[num6] != null && Ludan2[num6] == Ludan2[num6 + num10])
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
								if (Ludan3[num12] == null)
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
					else if ((num5 + 1) % 6 == 0 || Ludan3[num5 + 1] != null)
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
					Ludan3[num5] = "Z";
				}
				else
				{
					Ludan3[num5] = "X";
				}
			}
			array3 = new int[66];
			num4 = 0;
			int num13 = 0;
			flag = false;
			StringBuilder stringBuilder3 = new StringBuilder();
			for (int num14 = 13; num14 < Ludan2.Length; num14++)
			{
				if (Ludan2[num14] == null || array3.Contains(num14))
				{
					continue;
				}
				if (num14 % 6 == 0)
				{
					int num15 = 0;
					int num16 = 0;
					for (int num17 = num14 - num14 % 6 - 18; num17 < num14 - num14 % 6 - 18 + 6; num17++)
					{
						if (Ludan2[num17] != null)
						{
							num15++;
						}
					}
					for (int num17 = num14 - num14 % 6 - 6; num17 < num14 - num14 % 6 - 6 + 6; num17++)
					{
						if (Ludan2[num17] != null)
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
				else if (Ludan2[num14 - 12] != null)
				{
					stringBuilder3.Append(0);
				}
				else if (Ludan2[num14 - 12] == null && Ludan2[num14 - 13] != null)
				{
					stringBuilder3.Append(1);
				}
				else if (Ludan2[num14 - 12] == null && Ludan2[num14 - 13] == null)
				{
					stringBuilder3.Append(0);
				}
				if ((num14 + 1) % 6 == 0)
				{
					int num18 = 6;
					while (Ludan2[num14] != null && Ludan2[num14] == Ludan2[num14 + num18])
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
								if (Ludan4[num20] == null)
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
					else if ((num13 + 1) % 6 == 0 || Ludan4[num13 + 1] != null)
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
					Ludan4[num13] = "Z";
				}
				else
				{
					Ludan4[num13] = "X";
				}
			}
			array3 = new int[66];
			num4 = 0;
			int num21 = 0;
			flag = false;
			StringBuilder stringBuilder4 = new StringBuilder();
			for (int num22 = 19; num22 < Ludan2.Length; num22++)
			{
				if (Ludan2[num22] == null || array3.Contains(num22))
				{
					continue;
				}
				if (num22 % 6 == 0)
				{
					int num23 = 0;
					int num24 = 0;
					for (int num25 = num22 - num22 % 6 - 24; num25 < num22 - num22 % 6 - 24 + 6; num25++)
					{
						if (Ludan2[num25] != null)
						{
							num23++;
						}
					}
					for (int num25 = num22 - num22 % 6 - 6; num25 < num22 - num22 % 6 - 6 + 6; num25++)
					{
						if (Ludan2[num25] != null)
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
				else if (Ludan2[num22 - 18] != null)
				{
					stringBuilder4.Append(0);
				}
				else if (Ludan2[num22 - 18] == null && Ludan2[num22 - 19] != null)
				{
					stringBuilder4.Append(1);
				}
				else if (Ludan2[num22 - 18] == null && Ludan2[num22 - 19] == null)
				{
					stringBuilder4.Append(0);
				}
				if ((num22 + 1) % 6 == 0)
				{
					int num26 = 6;
					while (Ludan2[num22] != null && Ludan2[num22] == Ludan2[num22 + num26])
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
								if (Ludan5[num28] == null)
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
					else if ((num21 + 1) % 6 == 0 || Ludan5[num21 + 1] != null)
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
					Ludan5[num21] = "Z";
				}
				else
				{
					Ludan5[num21] = "X";
				}
			}
			for (int num29 = 0; num29 < Result.Length; num29++)
			{
				switch (Result[num29])
				{
				case "a":
					Ludan1[num29] = "10";
					break;
				case "b":
					Ludan1[num29] = "11";
					break;
				default:
					Ludan1[num29] = Result[num29].ToString();
					break;
				}
			}
			for (int num30 = 0; num30 < Ludan1.Length; num30++)
			{
				if (Ludan1[num30] != null)
				{
					LuDan1_NowCount = num30 + 1;
				}
			}
			for (int num31 = 0; num31 < Ludan2.Length; num31++)
			{
				if (Ludan2[num31] != null)
				{
					LuDan2_NowCount = num31 + 1;
				}
			}
			for (int num32 = 0; num32 < Ludan3.Length; num32++)
			{
				if (Ludan3[num32] != null)
				{
					LuDan3_NowCount = num32 + 1;
				}
			}
			for (int num33 = 0; num33 < Ludan4.Length; num33++)
			{
				if (Ludan4[num33] != null)
				{
					LuDan4_NowCount = num33 + 1;
				}
			}
			for (int num34 = 0; num34 < Ludan5.Length; num34++)
			{
				if (Ludan5[num34] != null)
				{
					LuDan5_NowCount = num34 + 1;
				}
			}
		}

		private bool IsHe(string result)
		{
			if (result != null && (result == "2" || result == "5" || result == "8" || result == "b"))
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
			for (int i = 1; i < Result.Length; i++)
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
			if (IsHe(Result[0]))
			{
				num++;
				if (num < Result.Length)
				{
					for (; IsHe(Result[num]); num++)
					{
						num3++;
					}
					array[num2, 0] = 0;
					array[num2, 1] = num;
					num = 0;
					num2++;
				}
			}
			for (int i = (num3 == 0) ? 1 : num3; i < Result.Length; i++)
			{
				if (!IsHe(Result[i]) || IsHe(Result[i - 1]))
				{
					continue;
				}
				num3++;
				num++;
				if (i + num < Result.Length)
				{
					while (IsHe(Result[i + num]))
					{
						num++;
						if (i + num >= Result.Length)
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
	}

	public string[] NowResults;

	public int lastNum;

	public GameObject LuDanObject1;

	public GameObject LuDanObject2;

	public BaiJiaLeLuDan testNowLuDan;

	private void Start()
	{
	}

	public void ShowLuDan(string[] nowLudan)
	{
		UnityEngine.Debug.Log("刷新路单");
		NowResults = nowLudan;
		BaiJiaLeLuDan baiJiaLeLuDan = testNowLuDan = new BaiJiaLeLuDan(NowResults);
		if (LuDanObject1.transform.childCount > baiJiaLeLuDan.LuDan1_NowCount)
		{
			for (int i = 0; i < LuDanObject1.transform.childCount; i++)
			{
				if (baiJiaLeLuDan.Ludan1[i] != null)
				{
					LuDanObject1.transform.GetChild(i).GetChild(0).gameObject.SetActive(value: true);
					LuDanObject1.transform.GetChild(i).GetChild(0).GetComponent<Image>()
						.sprite = BaiJiaLe_LuDanDate.instance.LuDanSprite1[int.Parse(baiJiaLeLuDan.Ludan1[i])];
					}
					else
					{
						LuDanObject1.transform.GetChild(i).GetChild(0).gameObject.SetActive(value: false);
					}
				}
			}
			else
			{
				int num = (int)(Mathf.Ceil((float)baiJiaLeLuDan.LuDan1_NowCount / 6f) - Mathf.Ceil((float)LuDanObject1.transform.childCount / 6f)) * 6;
				for (int j = 0; j < LuDanObject1.transform.childCount; j++)
				{
					if (baiJiaLeLuDan.Ludan1[num] != null)
					{
						LuDanObject1.transform.GetChild(j).GetChild(0).gameObject.SetActive(value: true);
						LuDanObject1.transform.GetChild(j).GetChild(0).GetComponent<Image>()
							.sprite = BaiJiaLe_LuDanDate.instance.LuDanSprite1[int.Parse(baiJiaLeLuDan.Ludan1[num])];
						}
						else
						{
							LuDanObject1.transform.GetChild(j).GetChild(0).gameObject.SetActive(value: false);
						}
						num++;
					}
				}
				if (LuDanObject2.transform.childCount > baiJiaLeLuDan.LuDan2_NowCount)
				{
					int num2 = 0;
					for (int k = 0; k < LuDanObject2.transform.childCount; k++)
					{
						if (baiJiaLeLuDan.Ludan2[k] != null)
						{
							LuDanObject2.transform.GetChild(k).GetChild(0).gameObject.SetActive(value: true);
							if (baiJiaLeLuDan.Ludan2[k] == "Z")
							{
								LuDanObject2.transform.GetChild(k).GetChild(0).GetComponent<Image>()
									.sprite = BaiJiaLe_LuDanDate.instance.LuDanSprite2[0];
								}
								else if (baiJiaLeLuDan.Ludan2[k] == "X")
								{
									LuDanObject2.transform.GetChild(k).GetChild(0).GetComponent<Image>()
										.sprite = BaiJiaLe_LuDanDate.instance.LuDanSprite2[1];
									}
									else
									{
										LuDanObject2.transform.GetChild(k).GetChild(0).gameObject.SetActive(value: false);
									}
								}
								else
								{
									LuDanObject2.transform.GetChild(k).GetChild(0).gameObject.SetActive(value: false);
								}
								if (baiJiaLeLuDan.Ludan2_he[k] != null)
								{
									LuDanObject2.transform.GetChild(k).GetChild(1).gameObject.SetActive(value: true);
									LuDanObject2.transform.GetChild(k).GetChild(1).GetComponent<Text>()
										.text = baiJiaLeLuDan.Ludan2_he[k].ToString();
									}
									else
									{
										LuDanObject2.transform.GetChild(k).GetChild(1).gameObject.SetActive(value: false);
									}
								}
								return;
							}
							int num3 = (int)(Mathf.Ceil((float)baiJiaLeLuDan.LuDan2_NowCount / 6f) - Mathf.Ceil((float)LuDanObject2.transform.childCount / 6f)) * 6;
							int num4 = 0;
							for (int l = 0; l < LuDanObject2.transform.childCount; l++)
							{
								if (baiJiaLeLuDan.Ludan2[num3] != null)
								{
									LuDanObject2.transform.GetChild(l).GetChild(0).gameObject.SetActive(value: true);
									if (baiJiaLeLuDan.Ludan2[num3] == "Z")
									{
										LuDanObject2.transform.GetChild(l).GetChild(0).GetComponent<Image>()
											.sprite = BaiJiaLe_LuDanDate.instance.LuDanSprite2[0];
										}
										else if (baiJiaLeLuDan.Ludan2[num3] == "X")
										{
											LuDanObject2.transform.GetChild(l).GetChild(0).GetComponent<Image>()
												.sprite = BaiJiaLe_LuDanDate.instance.LuDanSprite2[1];
											}
											else
											{
												LuDanObject2.transform.GetChild(l).GetChild(0).gameObject.SetActive(value: false);
											}
										}
										else
										{
											LuDanObject2.transform.GetChild(l).GetChild(0).gameObject.SetActive(value: false);
										}
										if (baiJiaLeLuDan.Ludan2_he[num3] != null)
										{
											LuDanObject2.transform.GetChild(l).GetChild(1).gameObject.SetActive(value: true);
											LuDanObject2.transform.GetChild(l).GetChild(1).GetComponent<Text>()
												.text = baiJiaLeLuDan.Ludan2_he[num3].ToString();
											}
											else
											{
												LuDanObject2.transform.GetChild(l).GetChild(1).gameObject.SetActive(value: false);
											}
											lastNum = num3;
											num3++;
										}
									}
								}
