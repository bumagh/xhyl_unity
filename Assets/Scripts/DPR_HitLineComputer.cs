using System.Collections.Generic;

public class DPR_HitLineComputer
{
	public DPR_WinLineType winType;

	public int dragonNum;

	public int multiple;

	public int[] cells;

	public DPR_CellType cellType;

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

	public DPR_HitLineComputer(int lineIndex, int[] cells)
	{
		this.cells = cells;
		winType = DPR_WinLineType.None;
		dragonNum = 0;
		multiple = 0;
		cellType = DPR_CellType.None;
		this.lineIndex = lineIndex;
	}

	public DPR_WinLineResult[] Calculate2()
	{
		List<DPR_WinLineResult> list = new List<DPR_WinLineResult>();
		DPR_WinLineResult dPR_WinLineResult = new DPR_WinLineResult();
		if (DPR_CellManager.cellManager.GameWin[lineIndex] != 0)
		{
			switch (DPR_CellManager.cellManager.gameLineCount[lineIndex])
			{
			case 2:
				dPR_WinLineResult.winType = DPR_WinLineType.Left2;
				break;
			case 3:
				dPR_WinLineResult.winType = DPR_WinLineType.Left3;
				break;
			case 4:
				dPR_WinLineResult.winType = DPR_WinLineType.Left4;
				break;
			default:
				dPR_WinLineResult.winType = DPR_WinLineType.Full;
				break;
			}
			dPR_WinLineResult.cellType = (DPR_CellType)DPR_CellManager.cellManager.gameType[lineIndex];
			dPR_WinLineResult.lineIndex = lineIndex;
			dPR_WinLineResult.rate = 0;
			list.Add(dPR_WinLineResult);
		}
		return list.ToArray();
	}

	public static int CalculateHitNum(DPR_WinLineType winlinetype)
	{
		switch (winlinetype)
		{
		case DPR_WinLineType.Left2:
			return 2;
		case DPR_WinLineType.Left3:
			return 3;
		case DPR_WinLineType.Left4:
			return 4;
		case DPR_WinLineType.Full:
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
