using System.Collections.Generic;
using UnityEngine;

public class STWM_HitLineComputer
{
	public STWM_WinLineType winType;

	public int dragonNum;

	public int multiple;

	public int[] cells;

	public STWM_CellType cellType;

	public int lineIndex;

	public int rate = 10;

	public static int[][] AwardRateMap = new int[10][]
	{
		new int[4],
		new int[4]
		{
			5,
			15,
			60,
			90
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
			2,
			5,
			20,
			30
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
			7,
			20,
			100,
			150
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
			20,
			80,
			400,
			600
		},
		new int[4]
		{
			0,
			0,
			2000,
			3000
		}
	};

	public static int[] MaryTimesMap = new int[4]
	{
		1,
		2,
		3,
		27
	};

	public STWM_HitLineComputer(int lineIndex, int[] cells)
	{
		this.cells = cells;
		winType = STWM_WinLineType.None;
		dragonNum = 0;
		multiple = 0;
		cellType = STWM_CellType.None;
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
			winType = STWM_WinLineType.Full;
			rate = AwardRateMap[resultC][2];
		}
		else if (resultA == 3 && resultA2 <= 3)
		{
			winType = STWM_WinLineType.Left3;
			rate = AwardRateMap[resultC][0];
		}
		else if (resultA == 4 && resultA2 <= 4)
		{
			winType = STWM_WinLineType.Left4;
			rate = AwardRateMap[resultC][1];
		}
		else if (resultA < 3 && resultA2 == 3)
		{
			winType = STWM_WinLineType.Right3;
			rate = AwardRateMap[resultC2][0];
		}
		else if (resultA < 4 && resultA2 == 4)
		{
			winType = STWM_WinLineType.Right4;
			rate = AwardRateMap[resultC2][1];
		}
		else
		{
			winType = STWM_WinLineType.None;
			rate = 0;
		}
		if (rate != 0)
		{
			UnityEngine.Debug.Log("rate: " + rate);
		}
	}

	public STWM_WinLineResult[] Calculate2()
	{
		int resultA = 0;
		int resultB = 0;
		int resultC = 0;
		int resultC2 = 0;
		int resultB2 = 0;
		int resultA2 = 0;
		HitLineByDirection(cells, 1, ref resultA, ref resultB, ref resultC);
		if (resultA == 5 && resultB >= 2)
		{
			UnityEngine.Debug.Log("no need reverse");
		}
		else
		{
			HitLineByDirection(cells, -1, ref resultA2, ref resultB2, ref resultC2);
		}
		List<STWM_WinLineResult> list = new List<STWM_WinLineResult>();
		if (resultA >= 3)
		{
			STWM_WinLineResult sTWM_WinLineResult = new STWM_WinLineResult();
			STWM_WinLineResult sTWM_WinLineResult2 = sTWM_WinLineResult;
			int num;
			switch (resultA)
			{
			default:
				num = 5;
				break;
			case 4:
				num = 2;
				break;
			case 3:
				num = 1;
				break;
			}
			sTWM_WinLineResult2.winType = (STWM_WinLineType)num;
			sTWM_WinLineResult.cellType = (STWM_CellType)resultC;
			sTWM_WinLineResult.lineIndex = lineIndex;
			sTWM_WinLineResult.rate = AwardRateMap[resultC][resultA - 3];
			if (resultB >= 3)
			{
				sTWM_WinLineResult.dragonNum = resultB;
				sTWM_WinLineResult.maryTimes = MaryTimesMap[resultB - 3];
			}
			else if (resultB2 >= 3)
			{
				sTWM_WinLineResult.dragonNum = resultB2;
				sTWM_WinLineResult.maryTimes = MaryTimesMap[resultB2 - 3];
			}
			list.Add(sTWM_WinLineResult);
		}
		if (resultA2 >= 3 && resultA != 5)
		{
			STWM_WinLineResult sTWM_WinLineResult3 = new STWM_WinLineResult();
			STWM_WinLineResult sTWM_WinLineResult4 = sTWM_WinLineResult3;
			int num2;
			switch (resultA2)
			{
			default:
				num2 = 5;
				break;
			case 4:
				num2 = 4;
				break;
			case 3:
				num2 = 3;
				break;
			}
			sTWM_WinLineResult4.winType = (STWM_WinLineType)num2;
			sTWM_WinLineResult3.cellType = (STWM_CellType)resultC2;
			sTWM_WinLineResult3.lineIndex = lineIndex;
			sTWM_WinLineResult3.rate = AwardRateMap[resultC2][resultA2 - 3];
			if (resultB2 >= 3)
			{
				sTWM_WinLineResult3.dragonNum = resultB2;
				sTWM_WinLineResult3.maryTimes = MaryTimesMap[resultB2 - 3];
			}
			list.Add(sTWM_WinLineResult3);
		}
		return list.ToArray();
	}

	public static int CalculateHitNum(STWM_WinLineType winlinetype)
	{
		switch (winlinetype)
		{
		case STWM_WinLineType.Left3:
		case STWM_WinLineType.Right3:
			return 3;
		case STWM_WinLineType.Left4:
		case STWM_WinLineType.Right4:
			return 4;
		case STWM_WinLineType.Full:
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
