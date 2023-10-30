using System.Collections.Generic;
using UnityEngine;

public class Dzb_CardType
{
	public enum CardType
	{
		e_ZA_PAI,
		e_YI_DUI,
		e_ER_DUI,
		e_SAN_TIAO,
		e_SHUNZI,
		e_TONGHUA,
		e_HULU,
		e_SI_TIAO,
		e_TONG_HUA_SHUN,
		e_TONG_HUA_DA_SHUN,
		e_Wu_Tiao
	}

	private enum CardType2
	{

	}

	private int[] myCards1;

	private int[] myCards2;

	private List<int[]> FiveKing = new List<int[]>();

	private List<int[]> FlushStraight = new List<int[]>();

	private List<int[]> Straight = new List<int[]>();

	private List<int[]> Flush = new List<int[]>();

	private List<int[]> FourKing = new List<int[]>();

	private List<int[]> BoatorFullHouse = new List<int[]>();

	private List<int[]> Tripleton = new List<int[]>();

	private List<int[]> TwoPair = new List<int[]>();

	private List<int[]> Pair = new List<int[]>();

	private int[] only = new int[1];

	private bool IsFlushStraight_Five;

	private bool IsStraight_Five;

	private bool IsFlush_Five;

	private bool IsFiveKing;

	private bool IsFlushStraight;

	private bool IsStraight;

	private bool IsFlush;

	private bool IsFourKing;

	private bool IsBoatorFullHouse;

	private bool IsTripleton;

	private bool IsTwoPair;

	private bool IsPair;

	private bool IsGetFive;

	public int[] CardsSort(int[] cards)
	{
		List<int> list = new List<int>();
		list.AddRange(cards);
		for (int i = 0; i < list.Count; i++)
		{
			for (int j = i + 1; j < list.Count; j++)
			{
				if (CardValueToNum(list[i]) > CardValueToNum(list[j]))
				{
					int value = list[i];
					list[i] = list[j];
					list[j] = value;
				}
			}
		}
		return list.ToArray();
	}

	public int[] CardsSort2(int[] cards)
	{
		List<int> list = new List<int>();
		list.AddRange(cards);
		for (int i = 0; i < list.Count; i++)
		{
			for (int j = i + 1; j < list.Count; j++)
			{
				if (CardValueToNum2(list[i]) > CardValueToNum2(list[j]))
				{
					int value = list[i];
					list[i] = list[j];
					list[j] = value;
				}
			}
		}
		return list.ToArray();
	}

	private int CardValueToNum(int value)
	{
		int num = 0;
		if (value < 52)
		{
			return value % 13;
		}
		return -1;
	}

	private int CardValueToNum2(int value)
	{
		int num = 0;
		if (value < 52)
		{
			if (value % 13 == 0)
			{
				return 13;
			}
			return value % 13;
		}
		return -1;
	}

	public string CardValueToString(int value)
	{
		string str = string.Empty;
		if (value == -1)
		{
			return "空";
		}
		if (value < 52)
		{
			switch (value / 13)
			{
			case 0:
				str += "黑桃";
				break;
			case 1:
				str += "红心";
				break;
			case 2:
				str += "梅花";
				break;
			case 3:
				str += "方块";
				break;
			}
			switch (value % 13)
			{
			case 0:
				return str + "A";
			case 10:
				return str + "J";
			case 11:
				return str + "Q";
			case 12:
				return str + "K";
			default:
				return str + (value % 13 + 1).ToString();
			}
		}
		return "王";
	}

	public int CardValueToColor(int value)
	{
		int num = 0;
		if (value < 52)
		{
			return value / 13;
		}
		return -1;
	}

	public int[] GetOnePair(int[] cards)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < cards.Length; i++)
		{
			int num = i + 1;
			for (; i < cards.Length; i++)
			{
				if (CardValueToNum(cards[i]) == CardValueToNum(cards[num]))
				{
					list.Add(cards[i]);
					list.Add(cards[num]);
					break;
				}
			}
		}
		return list.ToArray();
	}

	public int[] GetTwoPair(int[] cards)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < cards.Length; i++)
		{
			int num = i + 1;
			for (; i < cards.Length; i++)
			{
				if (CardValueToNum(cards[i]) == CardValueToNum(cards[num]))
				{
					list.Add(cards[i]);
					list.Add(cards[num]);
					break;
				}
			}
		}
		if (list.Count != 4)
		{
			list.Clear();
		}
		return list.ToArray();
	}

	public int[] GetTripleton(int[] cards)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < cards.Length; i++)
		{
			int num = i + 1;
			for (; i < cards.Length; i++)
			{
				if (CardValueToNum(cards[i]) != CardValueToNum(cards[num]))
				{
					continue;
				}
				for (int j = num + 1; j < cards.Length; j++)
				{
					if (CardValueToNum(cards[num]) == CardValueToNum(cards[j]))
					{
						list.Add(cards[i]);
						list.Add(cards[num]);
						list.Add(cards[j]);
						break;
					}
				}
				break;
			}
		}
		return list.ToArray();
	}

	public int[] GetBoatorFullHouse(int[] cards)
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		for (int i = 0; i < cards.Length; i++)
		{
			int num = i + 1;
			for (; i < cards.Length; i++)
			{
				if (CardValueToNum(cards[i]) != CardValueToNum(cards[num]))
				{
					continue;
				}
				for (int j = num + 1; j < cards.Length; j++)
				{
					if (CardValueToNum(cards[num]) == CardValueToNum(cards[j]))
					{
						list2.Add(cards[i]);
						list2.Add(cards[num]);
						list2.Add(cards[j]);
						break;
					}
				}
				break;
			}
		}
		if (list2.Count == 3)
		{
			for (int k = 0; k < cards.Length; k++)
			{
				if (CardValueToNum(cards[k]) == CardValueToNum(list2[0]))
				{
					continue;
				}
				int num2 = k + 1;
				for (; k < cards.Length; k++)
				{
					if (CardValueToNum(cards[k]) == CardValueToNum(cards[num2]))
					{
						list3.Add(cards[k]);
						list3.Add(cards[num2]);
						break;
					}
				}
			}
		}
		if (list2.Count == 3 && list3.Count == 2)
		{
			list.AddRange(list2);
			list.AddRange(list3);
		}
		return list.ToArray();
	}

	public int[] GetStraight(int[] cards)
	{
		List<int> list = new List<int>();
		return list.ToArray();
	}

	public int[] GetFlush(int[] cards)
	{
		List<int> list = new List<int>();
		int num = -1;
		for (int i = 0; i < cards.Length; i++)
		{
			if (CardValueToColor(cards[i]) == -1)
			{
				list.Add(cards[i]);
			}
			else if (CardValueToColor(cards[i]) != -1 && num == -1)
			{
				num = CardValueToColor(cards[i]);
				list.Add(cards[i]);
			}
			else if (num == CardValueToColor(cards[i]))
			{
				list.Add(cards[i]);
			}
		}
		if (list.Count < 4)
		{
			list.Clear();
		}
		return list.ToArray();
	}

	public int[] GetFourKing(int[] cards)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < cards.Length; i++)
		{
			int num = i + 1;
			for (; i < cards.Length; i++)
			{
				if (CardValueToNum(cards[i]) != -1 && CardValueToNum(cards[i]) != CardValueToNum(cards[num]))
				{
					continue;
				}
				for (int j = num + 1; j < cards.Length; j++)
				{
					if (CardValueToNum(cards[num]) != -1 && CardValueToNum(cards[num]) != CardValueToNum(cards[j]))
					{
						continue;
					}
					for (int k = j + 1; k < cards.Length; k++)
					{
						if (CardValueToNum(cards[j]) == -1 || CardValueToNum(cards[j]) == CardValueToNum(cards[k]))
						{
							list.Add(cards[i]);
							list.Add(cards[num]);
							list.Add(cards[j]);
							list.Add(cards[k]);
							break;
						}
					}
					break;
				}
				break;
			}
		}
		return list.ToArray();
	}

	public int[] GetStraightFlush(int knum, int[] cards)
	{
		List<int> list = new List<int>();
		int num = 0;
		for (int i = 0; i < cards.Length - 3 + num; i++)
		{
			if (CardValueToNum(cards[i]) != -1)
			{
				num++;
			}
		}
		return list.ToArray();
	}

	public int[] GetRoyalFlush(int knum, int[] cards)
	{
		List<int> list = new List<int>();
		int num = 1 + knum;
		for (int i = 0; i < cards.Length; i++)
		{
		}
		return list.ToArray();
	}

	public void AutoReserveCards(int[] cards)
	{
		FiveKing.Clear();
		FlushStraight.Clear();
		Straight.Clear();
		Flush.Clear();
		FourKing.Clear();
		BoatorFullHouse.Clear();
		Tripleton.Clear();
		TwoPair.Clear();
		Pair.Clear();
		IsFlushStraight_Five = false;
		IsStraight_Five = false;
		IsFlush_Five = false;
		IsFiveKing = false;
		IsFlushStraight = false;
		IsStraight = false;
		IsFlush = false;
		IsFourKing = false;
		IsBoatorFullHouse = false;
		IsTripleton = false;
		IsTwoPair = false;
		IsPair = false;
		IsGetFive = false;
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < cards.Length; i++)
		{
			if (cards[i] >= 52)
			{
				list.Add(cards[i]);
			}
			else
			{
				list2.Add(cards[i]);
			}
		}
		myCards1 = CardsSort(list2.ToArray());
		myCards2 = CardsSort2(list2.ToArray());
		only[0] = list2[0];
		for (int j = 1; j < list2.Count; j++)
		{
			if (CardValueToNum2(only[0]) < CardValueToNum2(list2[j]))
			{
				only[0] = list2[j];
			}
		}
		for (int k = 0; k < list2.Count; k++)
		{
			for (int l = k + 1; l < list2.Count; l++)
			{
				if (CardValueToNum(list2[k]) == CardValueToNum(list2[l]))
				{
					int[] item = new int[2]
					{
						list2[k],
						list2[l]
					};
					Pair.Add(item);
				}
			}
		}
		for (int m = 0; m < list2.Count; m++)
		{
			for (int n = m + 1; n < list2.Count; n++)
			{
				if (CardValueToNum(list2[m]) != CardValueToNum(list2[n]))
				{
					continue;
				}
				for (int num = n + 1; num < list2.Count; num++)
				{
					if (CardValueToNum(list2[n]) == CardValueToNum(list2[num]))
					{
						int[] item2 = new int[3]
						{
							list2[m],
							list2[n],
							list2[num]
						};
						Tripleton.Add(item2);
					}
				}
			}
		}
		for (int num2 = 0; num2 < list2.Count; num2++)
		{
			for (int num3 = num2 + 1; num3 < list2.Count; num3++)
			{
				if (CardValueToNum(list2[num2]) != CardValueToNum(list2[num3]))
				{
					continue;
				}
				for (int num4 = num3 + 1; num4 < list2.Count; num4++)
				{
					if (CardValueToNum(list2[num3]) == CardValueToNum(list2[num4]))
					{
						for (int num5 = num4 + 1; num5 < list2.Count; num5++)
						{
							if (CardValueToNum(list2[num4]) == CardValueToNum(list2[num5]))
							{
								int[] item3 = new int[4]
								{
									list2[num2],
									list2[num3],
									list2[num4],
									list2[num5]
								};
								FourKing.Add(item3);
							}
						}
						continue;
					}
					for (int num6 = num4 + 1; num6 < list2.Count; num6++)
					{
						if (CardValueToNum(list2[num4]) == CardValueToNum(list2[num6]))
						{
							int[] item4 = new int[4]
							{
								list2[num2],
								list2[num3],
								list2[num4],
								list2[num6]
							};
							TwoPair.Add(item4);
						}
					}
				}
			}
		}
		for (int num7 = 0; num7 < list2.Count; num7++)
		{
			for (int num8 = num7 + 1; num8 < list2.Count; num8++)
			{
				if (CardValueToNum(list2[num7]) != CardValueToNum(list2[num8]))
				{
					continue;
				}
				for (int num9 = num8 + 1; num9 < list2.Count; num9++)
				{
					if (CardValueToNum(list2[num8]) == CardValueToNum(list2[num9]))
					{
						for (int num10 = num9 + 1; num10 < list2.Count; num10++)
						{
							if (CardValueToNum(list2[num9]) == CardValueToNum(list2[num10]))
							{
								continue;
							}
							for (int num11 = num10 + 1; num11 < list2.Count; num11++)
							{
								if (CardValueToNum(list2[num10]) == CardValueToNum(list2[num11]))
								{
									int[] item5 = new int[5]
									{
										list2[num7],
										list2[num8],
										list2[num9],
										list2[num10],
										list2[num11]
									};
									BoatorFullHouse.Add(item5);
								}
							}
						}
						continue;
					}
					for (int num12 = num9 + 1; num12 < list2.Count; num12++)
					{
						if (CardValueToNum(list2[num9]) != CardValueToNum(list2[num12]))
						{
							continue;
						}
						for (int num13 = num12 + 1; num13 < list2.Count; num13++)
						{
							if (CardValueToNum(list2[num12]) == CardValueToNum(list2[num13]))
							{
								int[] item6 = new int[5]
								{
									list2[num7],
									list2[num8],
									list2[num9],
									list2[num12],
									list2[num13]
								};
								BoatorFullHouse.Add(item6);
							}
						}
					}
				}
			}
		}
		for (int num14 = 0; num14 < myCards1.Length; num14++)
		{
			for (int num15 = num14 + 1; num15 < myCards1.Length; num15++)
			{
				int num16 = CardValueToNum(myCards1[num15]) - CardValueToNum(myCards1[num14]);
				if (num16 < 1 || num16 > 2)
				{
					continue;
				}
				for (int num17 = num15 + 1; num17 < myCards1.Length; num17++)
				{
					int num18 = CardValueToNum(myCards1[num17]) - CardValueToNum(myCards1[num15]);
					if (num18 < 1 || num18 > 2 || num16 + num18 > 4)
					{
						continue;
					}
					for (int num19 = num17 + 1; num19 < myCards1.Length; num19++)
					{
						int num20 = CardValueToNum(myCards1[num19]) - CardValueToNum(myCards1[num17]);
						if (num20 < 1 || num20 > 2 || num16 + num18 + num20 > 4)
						{
							continue;
						}
						for (int num21 = num19 + 1; num21 < myCards1.Length; num21++)
						{
							int num22 = CardValueToNum(myCards1[num21]) - CardValueToNum(myCards1[num19]);
							if (num22 >= 1 && num22 <= 2 && num16 + num18 + num20 + num22 <= 4)
							{
								int[] item7 = new int[5]
								{
									myCards1[num14],
									myCards1[num15],
									myCards1[num17],
									myCards1[num19],
									myCards1[num21]
								};
								Straight.Add(item7);
								IsStraight_Five = true;
							}
						}
						if (list.Count == 1)
						{
							int[] array = new int[4]
							{
								-1,
								-1,
								-1,
								-1
							};
							array[0] = myCards1[num14];
							array[1] = myCards1[num15];
							array[2] = myCards1[num17];
							array[3] = myCards1[num19];
							Straight.Add(array);
							IsStraight_Five = true;
						}
						else if (list.Count == 0)
						{
							int[] array2 = new int[4]
							{
								-1,
								-1,
								-1,
								-1
							};
							array2[0] = myCards1[num14];
							array2[1] = myCards1[num15];
							array2[2] = myCards1[num17];
							array2[3] = myCards1[num19];
							Straight.Add(array2);
						}
					}
					if (list.Count == 2)
					{
						int[] array3 = new int[3]
						{
							-1,
							-1,
							-1
						};
						array3[0] = myCards1[num14];
						array3[1] = myCards1[num15];
						array3[2] = myCards1[num17];
						Straight.Add(array3);
						IsStraight_Five = true;
					}
					else if (list.Count == 1)
					{
						int[] array4 = new int[3]
						{
							-1,
							-1,
							-1
						};
						array4[0] = myCards1[num14];
						array4[1] = myCards1[num15];
						array4[2] = myCards1[num17];
						Straight.Add(array4);
					}
				}
				if (list.Count == 3)
				{
					int[] array5 = new int[2]
					{
						-1,
						-1
					};
					array5[0] = myCards1[num14];
					array5[1] = myCards1[num15];
					Straight.Add(array5);
					IsStraight_Five = true;
				}
				else if (list.Count == 2)
				{
					int[] array6 = new int[2]
					{
						-1,
						-1
					};
					array6[0] = myCards1[num14];
					array6[1] = myCards1[num15];
					Straight.Add(array6);
				}
			}
			if (list.Count == 3)
			{
				int[] array7 = new int[1]
				{
					-1
				};
				array7[0] = myCards1[num14];
				Straight.Add(array7);
			}
		}
		for (int num23 = 0; num23 < myCards2.Length; num23++)
		{
			for (int num24 = num23 + 1; num24 < myCards2.Length; num24++)
			{
				int num25 = CardValueToNum2(myCards2[num24]) - CardValueToNum2(myCards2[num23]);
				if (num25 < 1 || num25 > 2)
				{
					continue;
				}
				for (int num26 = num24 + 1; num26 < myCards2.Length; num26++)
				{
					int num27 = CardValueToNum2(myCards2[num26]) - CardValueToNum2(myCards2[num24]);
					if (num27 < 1 || num27 > 2 || num25 + num27 > 4)
					{
						continue;
					}
					for (int num28 = num26 + 1; num28 < myCards2.Length; num28++)
					{
						int num29 = CardValueToNum2(myCards2[num28]) - CardValueToNum2(myCards2[num26]);
						if (num29 < 1 || num29 > 2 || num25 + num27 + num29 > 4)
						{
							continue;
						}
						for (int num30 = num28 + 1; num30 < myCards2.Length; num30++)
						{
							int num31 = CardValueToNum2(myCards2[num30]) - CardValueToNum2(myCards2[num28]);
							if (num31 >= 1 && num31 <= 2 && num25 + num27 + num29 + num31 <= 4)
							{
								int[] item8 = new int[5]
								{
									myCards2[num23],
									myCards2[num24],
									myCards2[num26],
									myCards2[num28],
									myCards2[num30]
								};
								Straight.Add(item8);
								IsStraight_Five = true;
							}
						}
						if (list.Count == 1)
						{
							int[] array8 = new int[4]
							{
								-1,
								-1,
								-1,
								-1
							};
							array8[0] = myCards2[num23];
							array8[1] = myCards2[num24];
							array8[2] = myCards2[num26];
							array8[3] = myCards2[num28];
							Straight.Add(array8);
							IsStraight_Five = true;
						}
						else if (list.Count == 0)
						{
							int[] array9 = new int[4]
							{
								-1,
								-1,
								-1,
								-1
							};
							array9[0] = myCards2[num23];
							array9[1] = myCards2[num24];
							array9[2] = myCards2[num26];
							array9[3] = myCards2[num28];
							Straight.Add(array9);
						}
					}
					if (list.Count == 2)
					{
						int[] array10 = new int[3]
						{
							-1,
							-1,
							-1
						};
						array10[0] = myCards2[num23];
						array10[1] = myCards2[num24];
						array10[2] = myCards2[num26];
						Straight.Add(array10);
						IsStraight_Five = true;
					}
					else if (list.Count == 1)
					{
						int[] array11 = new int[3]
						{
							-1,
							-1,
							-1
						};
						array11[0] = myCards2[num23];
						array11[1] = myCards2[num24];
						array11[2] = myCards2[num26];
						Straight.Add(array11);
					}
				}
				if (list.Count == 3)
				{
					int[] array12 = new int[2]
					{
						-1,
						-1
					};
					array12[0] = myCards2[num23];
					array12[1] = myCards2[num24];
					Straight.Add(array12);
					IsStraight_Five = true;
				}
				else if (list.Count == 2)
				{
					int[] array13 = new int[2]
					{
						-1,
						-1
					};
					array13[0] = myCards2[num23];
					array13[1] = myCards2[num24];
					Straight.Add(array13);
				}
			}
			if (list.Count == 3)
			{
				int[] array14 = new int[1]
				{
					-1
				};
				array14[0] = myCards2[num23];
				Straight.Add(array14);
			}
		}
		for (int num32 = 0; num32 < list2.Count; num32++)
		{
			for (int num33 = num32 + 1; num33 < list2.Count; num33++)
			{
				if (CardValueToColor(list2[num32]) != CardValueToColor(list2[num33]))
				{
					continue;
				}
				for (int num34 = num33 + 1; num34 < list2.Count; num34++)
				{
					if (CardValueToColor(list2[num33]) != CardValueToColor(list2[num34]))
					{
						continue;
					}
					for (int num35 = num34 + 1; num35 < list2.Count; num35++)
					{
						if (CardValueToColor(list2[num34]) != CardValueToColor(list2[num35]))
						{
							continue;
						}
						for (int num36 = num35 + 1; num36 < list2.Count; num36++)
						{
							if (CardValueToColor(list2[num35]) == CardValueToColor(list2[num36]))
							{
								int[] array15 = new int[5]
								{
									-1,
									-1,
									-1,
									-1,
									-1
								};
								array15[0] = list2[num32];
								array15[1] = list2[num33];
								array15[2] = list2[num34];
								array15[3] = list2[num35];
								array15[4] = list2[num36];
								Flush.Add(array15);
								IsFlush_Five = true;
							}
						}
						if (list.Count == 1)
						{
							int[] array15 = new int[4]
							{
								-1,
								-1,
								-1,
								-1
							};
							array15[0] = list2[num32];
							array15[1] = list2[num33];
							array15[2] = list2[num34];
							array15[3] = list2[num35];
							Flush.Add(array15);
							IsFlush_Five = true;
						}
						else if (list.Count == 0)
						{
							int[] array15 = new int[4]
							{
								-1,
								-1,
								-1,
								-1
							};
							array15[0] = list2[num32];
							array15[1] = list2[num33];
							array15[2] = list2[num34];
							array15[3] = list2[num35];
							Flush.Add(array15);
						}
					}
					if (list.Count == 2)
					{
						int[] array15 = new int[3]
						{
							-1,
							-1,
							-1
						};
						array15[0] = list2[num32];
						array15[1] = list2[num33];
						array15[2] = list2[num34];
						Flush.Add(array15);
						IsFlush_Five = true;
					}
					else if (list.Count == 1)
					{
						int[] array15 = new int[3]
						{
							-1,
							-1,
							-1
						};
						array15[0] = list2[num32];
						array15[1] = list2[num33];
						array15[2] = list2[num34];
						Flush.Add(array15);
					}
				}
				if (list.Count == 3)
				{
					int[] array15 = new int[2]
					{
						-1,
						-1
					};
					array15[0] = list2[num32];
					array15[1] = list2[num33];
					Flush.Add(array15);
					IsFlush_Five = true;
				}
				else if (list.Count == 2)
				{
					int[] array15 = new int[2]
					{
						-1,
						-1
					};
					array15[0] = list2[num32];
					array15[1] = list2[num33];
					Flush.Add(array15);
				}
			}
			if (list.Count == 3)
			{
				int[] array15 = new int[1]
				{
					-1
				};
				array15[0] = list2[num32];
				Flush.Add(array15);
			}
		}
		for (int num37 = 0; num37 < myCards1.Length; num37++)
		{
			for (int num38 = num37 + 1; num38 < myCards1.Length; num38++)
			{
				int num39 = CardValueToNum(myCards1[num38]) - CardValueToNum(myCards1[num37]);
				if (num39 < 1 || num39 > 2 || CardValueToColor(list2[num37]) != CardValueToColor(list2[num38]))
				{
					continue;
				}
				for (int num40 = num38 + 1; num40 < myCards1.Length; num40++)
				{
					int num41 = CardValueToNum(myCards1[num40]) - CardValueToNum(myCards1[num38]);
					if (num41 < 1 || num41 > 2 || num39 + num41 > 4 || CardValueToColor(list2[num38]) != CardValueToColor(list2[num40]))
					{
						continue;
					}
					for (int num42 = num40 + 1; num42 < myCards1.Length; num42++)
					{
						int num43 = CardValueToNum(myCards1[num42]) - CardValueToNum(myCards1[num40]);
						if (num43 < 1 || num43 > 2 || num39 + num41 + num43 > 4 || CardValueToColor(list2[num40]) != CardValueToColor(list2[num42]))
						{
							continue;
						}
						for (int num44 = num42 + 1; num44 < myCards1.Length; num44++)
						{
							int num45 = CardValueToNum(myCards1[num44]) - CardValueToNum(myCards1[num42]);
							if (num45 >= 1 && num45 <= 2 && num39 + num41 + num43 + num45 <= 4 && CardValueToColor(list2[num42]) == CardValueToColor(list2[num44]))
							{
								int[] item9 = new int[5]
								{
									myCards1[num37],
									myCards1[num38],
									myCards1[num40],
									myCards1[num42],
									myCards1[num44]
								};
								FlushStraight.Add(item9);
								IsFlushStraight_Five = true;
							}
						}
						if (list.Count == 1)
						{
							int[] array16 = new int[4]
							{
								-1,
								-1,
								-1,
								-1
							};
							array16[0] = myCards1[num37];
							array16[1] = myCards1[num38];
							array16[2] = myCards1[num40];
							array16[3] = myCards1[num42];
							FlushStraight.Add(array16);
							IsFlushStraight_Five = true;
						}
						else if (list.Count == 0)
						{
							int[] array17 = new int[4]
							{
								-1,
								-1,
								-1,
								-1
							};
							array17[0] = myCards1[num37];
							array17[1] = myCards1[num38];
							array17[2] = myCards1[num40];
							array17[3] = myCards1[num42];
							FlushStraight.Add(array17);
						}
					}
					if (list.Count == 2)
					{
						int[] array18 = new int[3]
						{
							-1,
							-1,
							-1
						};
						array18[0] = myCards1[num37];
						array18[1] = myCards1[num38];
						array18[2] = myCards1[num40];
						FlushStraight.Add(array18);
						IsFlushStraight_Five = true;
					}
					else if (list.Count == 1)
					{
						int[] array19 = new int[3]
						{
							-1,
							-1,
							-1
						};
						array19[0] = myCards1[num37];
						array19[1] = myCards1[num38];
						array19[2] = myCards1[num40];
						FlushStraight.Add(array19);
					}
				}
				if (list.Count == 3)
				{
					int[] array20 = new int[2]
					{
						-1,
						-1
					};
					array20[0] = myCards1[num37];
					array20[1] = myCards1[num38];
					FlushStraight.Add(array20);
					IsFlushStraight_Five = true;
				}
				else if (list.Count == 2)
				{
					int[] array21 = new int[2]
					{
						-1,
						-1
					};
					array21[0] = myCards1[num37];
					array21[1] = myCards1[num38];
					FlushStraight.Add(array21);
				}
			}
			if (list.Count == 3)
			{
				int[] array22 = new int[1]
				{
					-1
				};
				array22[0] = myCards1[num37];
				FlushStraight.Add(array22);
			}
		}
		for (int num46 = 0; num46 < myCards2.Length; num46++)
		{
			for (int num47 = num46 + 1; num47 < myCards2.Length; num47++)
			{
				int num48 = CardValueToNum2(myCards2[num47]) - CardValueToNum2(myCards2[num46]);
				if (num48 < 1 || num48 > 2 || CardValueToColor(list2[num46]) != CardValueToColor(list2[num47]))
				{
					continue;
				}
				for (int num49 = num47 + 1; num49 < myCards2.Length; num49++)
				{
					int num50 = CardValueToNum2(myCards2[num49]) - CardValueToNum2(myCards2[num47]);
					if (num50 < 1 || num50 > 2 || num48 + num50 > 4 || CardValueToColor(list2[num47]) != CardValueToColor(list2[num49]))
					{
						continue;
					}
					for (int num51 = num49 + 1; num51 < myCards2.Length; num51++)
					{
						int num52 = CardValueToNum2(myCards2[num51]) - CardValueToNum2(myCards2[num49]);
						if (num52 < 1 || num52 > 2 || num48 + num50 + num52 > 4 || CardValueToColor(list2[num49]) != CardValueToColor(list2[num51]))
						{
							continue;
						}
						for (int num53 = num51 + 1; num53 < myCards2.Length; num53++)
						{
							int num54 = CardValueToNum2(myCards2[num53]) - CardValueToNum2(myCards2[num51]);
							if (num54 >= 1 && num54 <= 2 && num48 + num50 + num52 + num54 <= 4 && CardValueToColor(list2[num51]) == CardValueToColor(list2[num53]))
							{
								int[] item10 = new int[5]
								{
									myCards2[num46],
									myCards2[num47],
									myCards2[num49],
									myCards2[num51],
									myCards2[num53]
								};
								FlushStraight.Add(item10);
								IsFlushStraight_Five = true;
							}
						}
						if (list.Count == 1)
						{
							int[] array23 = new int[4]
							{
								-1,
								-1,
								-1,
								-1
							};
							array23[0] = myCards2[num46];
							array23[1] = myCards2[num47];
							array23[2] = myCards2[num49];
							array23[3] = myCards2[num51];
							FlushStraight.Add(array23);
							IsFlushStraight_Five = true;
						}
						else if (list.Count == 0)
						{
							int[] array24 = new int[4]
							{
								-1,
								-1,
								-1,
								-1
							};
							array24[0] = myCards2[num46];
							array24[1] = myCards2[num47];
							array24[2] = myCards2[num49];
							array24[3] = myCards2[num51];
							FlushStraight.Add(array24);
						}
					}
					if (list.Count == 2)
					{
						int[] array25 = new int[3]
						{
							-1,
							-1,
							-1
						};
						array25[0] = myCards2[num46];
						array25[1] = myCards2[num47];
						array25[2] = myCards2[num49];
						FlushStraight.Add(array25);
						IsFlushStraight_Five = true;
					}
					else if (list.Count == 1)
					{
						int[] array26 = new int[3]
						{
							-1,
							-1,
							-1
						};
						array26[0] = myCards2[num46];
						array26[1] = myCards2[num47];
						array26[2] = myCards2[num49];
						FlushStraight.Add(array26);
					}
				}
				if (list.Count == 3)
				{
					int[] array27 = new int[2]
					{
						-1,
						-1
					};
					array27[0] = myCards2[num46];
					array27[1] = myCards2[num47];
					FlushStraight.Add(array27);
					IsFlushStraight_Five = true;
				}
				else if (list.Count == 2)
				{
					int[] array28 = new int[2]
					{
						-1,
						-1
					};
					array28[0] = myCards2[num46];
					array28[1] = myCards2[num47];
					FlushStraight.Add(array28);
				}
			}
			if (list.Count == 3)
			{
				int[] array29 = new int[1]
				{
					-1
				};
				array29[0] = myCards2[num46];
				FlushStraight.Add(array29);
			}
		}
		string text = string.Empty;
		for (int num55 = 0; num55 < FiveKing.Count; num55++)
		{
			text += "[";
			for (int num56 = 0; num56 < FiveKing[num55].Length; num56++)
			{
				string text2 = text;
				text = text2 + FiveKing[num55][num56] + "(" + CardValueToString(FiveKing[num55][num56]) + "),";
			}
			text += "]";
		}
		UnityEngine.Debug.Log("五条：" + FiveKing.Count + "||" + text);
		text = string.Empty;
		for (int num57 = 0; num57 < FlushStraight.Count; num57++)
		{
			text += "[";
			for (int num58 = 0; num58 < FlushStraight[num57].Length; num58++)
			{
				string text2 = text;
				text = text2 + FlushStraight[num57][num58] + "(" + CardValueToString(FlushStraight[num57][num58]) + "),";
			}
			text += "]";
		}
		UnityEngine.Debug.Log("同花顺：" + FlushStraight.Count + "||" + text);
		text = string.Empty;
		for (int num59 = 0; num59 < Straight.Count; num59++)
		{
			text += "[";
			for (int num60 = 0; num60 < Straight[num59].Length; num60++)
			{
				string text2 = text;
				text = text2 + Straight[num59][num60] + "(" + CardValueToString(Straight[num59][num60]) + "),";
			}
			text += "]";
		}
		UnityEngine.Debug.Log("顺子：" + Straight.Count + "||" + text);
		text = string.Empty;
		for (int num61 = 0; num61 < Flush.Count; num61++)
		{
			text += "[";
			for (int num62 = 0; num62 < Flush[num61].Length; num62++)
			{
				string text2 = text;
				text = text2 + Flush[num61][num62] + "(" + CardValueToString(Flush[num61][num62]) + "),";
			}
			text += "]";
		}
		UnityEngine.Debug.Log("同花：" + Flush.Count + "||" + text);
		text = string.Empty;
		for (int num63 = 0; num63 < FourKing.Count; num63++)
		{
			text += "[";
			for (int num64 = 0; num64 < FourKing[num63].Length; num64++)
			{
				string text2 = text;
				text = text2 + FourKing[num63][num64] + "(" + CardValueToString(FourKing[num63][num64]) + "),";
			}
			text += "]";
		}
		UnityEngine.Debug.Log("四张：" + FourKing.Count + "||" + text);
		text = string.Empty;
		for (int num65 = 0; num65 < BoatorFullHouse.Count; num65++)
		{
			text += "[";
			for (int num66 = 0; num66 < BoatorFullHouse[num65].Length; num66++)
			{
				string text2 = text;
				text = text2 + BoatorFullHouse[num65][num66] + "(" + CardValueToString(BoatorFullHouse[num65][num66]) + "),";
			}
			text += "]";
		}
		UnityEngine.Debug.Log("葫芦：" + BoatorFullHouse.Count + "||" + text);
		text = string.Empty;
		for (int num67 = 0; num67 < Tripleton.Count; num67++)
		{
			text += "[";
			for (int num68 = 0; num68 < Tripleton[num67].Length; num68++)
			{
				string text2 = text;
				text = text2 + Tripleton[num67][num68] + "(" + CardValueToString(Tripleton[num67][num68]) + "),";
			}
			text += "]";
		}
		UnityEngine.Debug.Log("三张：" + Tripleton.Count + "||" + text);
		text = string.Empty;
		for (int num69 = 0; num69 < TwoPair.Count; num69++)
		{
			text += "[";
			for (int num70 = 0; num70 < TwoPair[num69].Length; num70++)
			{
				string text2 = text;
				text = text2 + TwoPair[num69][num70] + "(" + CardValueToString(TwoPair[num69][num70]) + "),";
			}
			text += "]";
		}
		UnityEngine.Debug.Log("两对：" + TwoPair.Count + "||" + text);
		text = string.Empty;
		for (int num71 = 0; num71 < Pair.Count; num71++)
		{
			text += "[";
			for (int num72 = 0; num72 < Pair[num71].Length; num72++)
			{
				string text2 = text;
				text = text2 + Pair[num71][num72] + "(" + CardValueToString(Pair[num71][num72]) + "),";
			}
			text += "]";
		}
		UnityEngine.Debug.Log("对子：" + Pair.Count + "||" + text);
		if (list.Count == 1)
		{
			if (FourKing.Count > 0)
			{
				IsFiveKing = true;
			}
			else if (Tripleton.Count > 0)
			{
				IsFourKing = true;
			}
			else if (Pair.Count > 0)
			{
				IsTripleton = true;
			}
			else
			{
				IsPair = true;
			}
		}
		else if (list.Count == 2)
		{
			if (Tripleton.Count > 0)
			{
				IsFiveKing = true;
			}
			else if (Pair.Count > 0)
			{
				IsFourKing = true;
			}
			else
			{
				IsTripleton = true;
				IsPair = true;
			}
		}
		else if (list.Count == 3)
		{
			if (Pair.Count > 0)
			{
				IsFiveKing = true;
			}
			else
			{
				IsFourKing = true;
				IsTripleton = true;
				IsPair = true;
			}
		}
		else
		{
			if (FourKing.Count > 0)
			{
				IsFourKing = true;
			}
			else if (Tripleton.Count > 0)
			{
				IsTripleton = true;
			}
			else if (Pair.Count > 0)
			{
				IsPair = true;
			}
			if (BoatorFullHouse.Count > 0)
			{
				IsBoatorFullHouse = true;
			}
			if (TwoPair.Count > 0)
			{
				IsTwoPair = true;
			}
		}
		for (int num73 = 0; num73 < FlushStraight.Count; num73++)
		{
			int num74 = 0;
			for (int num75 = 0; num75 < FlushStraight[num73].Length; num75++)
			{
				if (FlushStraight[num73][num75] != -1)
				{
					num74++;
				}
			}
			if (num74 + list.Count >= 4)
			{
				if (num74 + list.Count == 5)
				{
					IsGetFive = true;
				}
				IsFlushStraight = true;
			}
		}
		for (int num76 = 0; num76 < Flush.Count; num76++)
		{
			int num77 = 0;
			for (int num78 = 0; num78 < Flush[num76].Length; num78++)
			{
				if (Flush[num76][num78] != -1)
				{
					num77++;
				}
			}
			if (num77 + list.Count >= 4)
			{
				if (num77 + list.Count == 5)
				{
					IsGetFive = true;
				}
				IsFlush = true;
			}
		}
		for (int num79 = 0; num79 < Straight.Count; num79++)
		{
			int num80 = 0;
			for (int num81 = 0; num81 < Straight[num79].Length; num81++)
			{
				if (Straight[num79][num81] != -1)
				{
					num80++;
				}
			}
			if (num80 + list.Count >= 4)
			{
				if (num80 + list.Count == 5)
				{
					IsGetFive = true;
				}
				IsStraight = true;
			}
		}
	}

	public int[] GetHeldCards()
	{
		if (IsFiveKing)
		{
			UnityEngine.Debug.Log("五张");
			if (FiveKing.Count != 0)
			{
				return FiveKing[0];
			}
			if (FourKing.Count != 0)
			{
				return FourKing[0];
			}
			if (Tripleton.Count != 0)
			{
				return Tripleton[0];
			}
			if (Pair.Count != 0)
			{
				return Pair[0];
			}
		}
		if (IsFlushStraight_Five)
		{
			UnityEngine.Debug.Log("同花顺");
			return FlushStraight[0];
		}
		if (IsFlush_Five)
		{
			UnityEngine.Debug.Log("同花");
			return Flush[0];
		}
		if (IsStraight_Five)
		{
			UnityEngine.Debug.Log("顺子");
			return Straight[0];
		}
		if (IsFlushStraight)
		{
			UnityEngine.Debug.Log("同花顺");
			return FlushStraight[0];
		}
		if (IsFourKing)
		{
			UnityEngine.Debug.Log("四张");
			if (FourKing.Count != 0)
			{
				return FourKing[0];
			}
			if (Tripleton.Count != 0)
			{
				return Tripleton[0];
			}
			if (Pair.Count != 0)
			{
				return Pair[0];
			}
			return only;
		}
		if (IsBoatorFullHouse)
		{
			UnityEngine.Debug.Log("葫芦");
			return BoatorFullHouse[0];
		}
		if (IsFlush)
		{
			UnityEngine.Debug.Log("同花");
			return Flush[0];
		}
		if (IsStraight)
		{
			UnityEngine.Debug.Log("顺子");
			return Straight[0];
		}
		if (IsTripleton)
		{
			UnityEngine.Debug.Log("三张");
			if (Tripleton.Count != 0)
			{
				return Tripleton[0];
			}
			if (Pair.Count != 0)
			{
				return Pair[0];
			}
			return only;
		}
		if (IsTwoPair)
		{
			UnityEngine.Debug.Log("两对");
			return TwoPair[0];
		}
		if (IsPair)
		{
			UnityEngine.Debug.Log("对子");
			if (Pair.Count != 0)
			{
				return Pair[0];
			}
			return only;
		}
		return only;
	}
}
