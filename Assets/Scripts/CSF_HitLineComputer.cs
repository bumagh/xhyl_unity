using System.Collections.Generic;
using UnityEngine;

public class CSF_HitLineComputer
{
	public CSF_WinLineType winType;

	public int dragonNum;

	public int multiple;

	public int[] cells;

	public CSF_CellType cellType;

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

	public CSF_HitLineComputer(int lineIndex, int[] cells)
	{
		this.cells = cells;
		winType = CSF_WinLineType.None;
		dragonNum = 0;
		multiple = 0;
		cellType = CSF_CellType.None;
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
			winType = CSF_WinLineType.Full;
			rate = AwardRateMap[resultC][2];
		}
		else if (resultA == 3 && resultA2 <= 3)
		{
			winType = CSF_WinLineType.Left3;
			rate = AwardRateMap[resultC][0];
		}
		else if (resultA == 4 && resultA2 <= 4)
		{
			winType = CSF_WinLineType.Left4;
			rate = AwardRateMap[resultC][1];
		}
		else if (resultA < 3 && resultA2 == 3)
		{
			winType = CSF_WinLineType.Right3;
			rate = AwardRateMap[resultC2][0];
		}
		else if (resultA < 4 && resultA2 == 4)
		{
			winType = CSF_WinLineType.Right4;
			rate = AwardRateMap[resultC2][1];
		}
		else
		{
			winType = CSF_WinLineType.None;
			rate = 0;
		}
		if (rate != 0)
		{
			UnityEngine.Debug.Log("rate: " + rate);
		}
	}

	public CSF_WinLineResult[] Calculate2()
	{
		List<CSF_WinLineResult> list = new List<CSF_WinLineResult>();
		CSF_WinLineResult cSF_WinLineResult = new CSF_WinLineResult();
		if (CSF_CellManager.cellManager.gameLineCount[lineIndex] != 0)
		{
			if (CSF_CellManager.cellManager.gameLineLOrR[lineIndex] == 0)
			{
				switch (CSF_CellManager.cellManager.gameLineCount[lineIndex])
				{
				case 3:
					cSF_WinLineResult.winType = CSF_WinLineType.Left3;
					break;
				case 4:
					cSF_WinLineResult.winType = CSF_WinLineType.Left4;
					break;
				default:
					cSF_WinLineResult.winType = CSF_WinLineType.Full;
					break;
				}
			}
			else
			{
				switch (CSF_CellManager.cellManager.gameLineCount[lineIndex])
				{
				case 3:
					cSF_WinLineResult.winType = CSF_WinLineType.Right3;
					break;
				case 4:
					cSF_WinLineResult.winType = CSF_WinLineType.Right4;
					break;
				default:
					cSF_WinLineResult.winType = CSF_WinLineType.Full;
					break;
				}
			}
			cSF_WinLineResult.cellType = (CSF_CellType)CSF_CellManager.cellManager.gameType[lineIndex];
			cSF_WinLineResult.lineIndex = lineIndex;
			cSF_WinLineResult.rate = 0;
			list.Add(cSF_WinLineResult);
		}
		return list.ToArray();
	}

	public static int CalculateHitNum(CSF_WinLineType winlinetype)
	{
		switch (winlinetype)
		{
		case CSF_WinLineType.Left3:
		case CSF_WinLineType.Right3:
			return 3;
		case CSF_WinLineType.Left4:
		case CSF_WinLineType.Right4:
			return 4;
		case CSF_WinLineType.Full:
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
