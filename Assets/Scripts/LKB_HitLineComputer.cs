using System.Collections.Generic;
using UnityEngine;

public class LKB_HitLineComputer
{
	public LKB_WinLineType winType;

	public int dragonNum;

	public int multiple;

	public int[] cells;

	public LKB_CellType cellType;

	public int lineIndex;

	public int rate = 10;

	public static int[][] AwardRateMap = new int[12][]
	{
		new int[4],
		new int[4]
		{
			2,
			5,
			20,
			30
		},
		new int[4]
		{
			3,
			10,
			40,
			60
		},
		new int[4]
		{
			5,
			15,
			60,
			90
		},
		new int[4]
		{
			7,
			20,
			100,
			150
		},
		new int[4]
		{
			10,
			30,
			160,
			240
		},
		new int[4]
		{
			15,
			40,
			200,
			300
		},
		new int[4]
		{
			20,
			80,
			400,
			600
		},
		new int[4]
		{
			50,
			200,
			1000,
			1500
		},
		new int[4]
		{
			0,
			0,
			0,
			5000
		},
		new int[4]
		{
			0,
			0,
			0,
			5000
		},
		new int[4]
		{
			0,
			0,
			0,
			5000
		}
	};

	public static int[] MaryTimesMap = new int[4]
	{
		1,
		2,
		3,
		27
	};

	public LKB_HitLineComputer(int lineIndex, int[] cells)
	{
		this.cells = cells;
		winType = LKB_WinLineType.None;
		dragonNum = 0;
		multiple = 0;
		cellType = LKB_CellType.None;
		this.lineIndex = lineIndex;
	}

	public void Calculate()
	{
		int resultA = 0;
		int resultB = 0;
		int resultC = 0;
		int resultC2 = 0;
		int resultA2 = 0;
		HitLineByDirection(cells, 1, ref resultA, ref resultB, ref resultC);
		if (resultA == 5 || resultB >= 2)
		{
			UnityEngine.Debug.Log("no need reverse");
		}
		else
		{
			HitLineByDirection(cells, -1, ref resultA2, ref resultB, ref resultC2);
		}
		if (resultA >= 3 && resultA2 >= 3)
		{
			UnityEngine.Debug.Log("both hit");
		}
		dragonNum = resultB;
		if (resultA == 5)
		{
			winType = LKB_WinLineType.Full;
			rate = AwardRateMap[resultC][2];
		}
		else if (resultA == 3 && resultA2 <= 3)
		{
			winType = LKB_WinLineType.Left3;
			rate = AwardRateMap[resultC][0];
		}
		else if (resultA == 4 && resultA2 <= 4)
		{
			winType = LKB_WinLineType.Left4;
			rate = AwardRateMap[resultC][1];
		}
		else if (resultA < 3 && resultA2 == 3)
		{
			winType = LKB_WinLineType.Right3;
			rate = AwardRateMap[resultC2][0];
		}
		else if (resultA < 4 && resultA2 == 4)
		{
			winType = LKB_WinLineType.Right4;
			rate = AwardRateMap[resultC2][1];
		}
		else
		{
			winType = LKB_WinLineType.None;
			rate = 0;
		}
		if (rate != 0)
		{
			UnityEngine.Debug.Log("rate: " + rate);
		}
	}

	public LKB_WinLineResult[] Calculate2()
	{
		List<LKB_WinLineResult> list = new List<LKB_WinLineResult>();
		LKB_WinLineResult lKB_WinLineResult = new LKB_WinLineResult();
		if (LKB_CellManager.cellManager.gameLineCount[lineIndex] != 0)
		{
			if (LKB_CellManager.cellManager.gameLineLOrR[lineIndex] == 0)
			{
				switch (LKB_CellManager.cellManager.gameLineCount[lineIndex])
				{
				case 3:
					lKB_WinLineResult.winType = LKB_WinLineType.Left3;
					break;
				case 4:
					lKB_WinLineResult.winType = LKB_WinLineType.Left4;
					break;
				default:
					lKB_WinLineResult.winType = LKB_WinLineType.Full;
					break;
				}
			}
			else
			{
				switch (LKB_CellManager.cellManager.gameLineCount[lineIndex])
				{
				case 3:
					lKB_WinLineResult.winType = LKB_WinLineType.Right3;
					break;
				case 4:
					lKB_WinLineResult.winType = LKB_WinLineType.Right4;
					break;
				default:
					lKB_WinLineResult.winType = LKB_WinLineType.Full;
					break;
				}
			}
			lKB_WinLineResult.cellType = (LKB_CellType)LKB_CellManager.cellManager.gameType[lineIndex];
			lKB_WinLineResult.lineIndex = lineIndex;
			lKB_WinLineResult.rate = 0;
			list.Add(lKB_WinLineResult);
		}
		return list.ToArray();
	}

	public static int CalculateHitNum(LKB_WinLineType winlinetype)
	{
		switch (winlinetype)
		{
		case LKB_WinLineType.Left3:
		case LKB_WinLineType.Right3:
			return 3;
		case LKB_WinLineType.Left4:
		case LKB_WinLineType.Right4:
			return 4;
		case LKB_WinLineType.Full:
			return 5;
		default:
			return 0;
		}
	}

	private void HitLineByDirection(int[] line, int direction, ref int resultA, ref int resultB, ref int resultC)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int i = (direction <= 0) ? 4 : 0;
		int num4 = (direction > 0) ? 1 : (-1);
		int num5 = 9;
		for (; i > -1 && i < 5; i += num4)
		{
			int num6 = line[i];
			if (num6 != num5 && num2 != 0 && num2 != num6)
			{
				break;
			}
			num2 = ((num6 == num5) ? num2 : num6);
			num++;
			num3 = ((num2 == 0) ? (num3 + 1) : num3);
		}
		num2 = ((num3 == 5) ? num5 : num2);
		resultA = num;
		resultB = num3;
		resultC = num2;
	}
}
