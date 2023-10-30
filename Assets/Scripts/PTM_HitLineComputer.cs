using System.Collections.Generic;

public class PTM_HitLineComputer
{
	public PTM_WinLineType winType;

	public int dragonNum;

	public int multiple;

	public int[] cells;

	public PTM_CellType cellType;

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

	public PTM_HitLineComputer(int lineIndex, int[] cells)
	{
		this.cells = cells;
		winType = PTM_WinLineType.None;
		dragonNum = 0;
		multiple = 0;
		cellType = PTM_CellType.None;
		this.lineIndex = lineIndex;
	}

	public PTM_WinLineResult[] Calculate2()
	{
		List<PTM_WinLineResult> list = new List<PTM_WinLineResult>();
		PTM_WinLineResult pTM_WinLineResult = new PTM_WinLineResult();
		if (PTM_CellManager.cellManager.GameWin[lineIndex] != 0)
		{
			switch (PTM_CellManager.cellManager.gameLineCount[lineIndex])
			{
			case 2:
				pTM_WinLineResult.winType = PTM_WinLineType.Left2;
				break;
			case 3:
				pTM_WinLineResult.winType = PTM_WinLineType.Left3;
				break;
			case 4:
				pTM_WinLineResult.winType = PTM_WinLineType.Left4;
				break;
			default:
				pTM_WinLineResult.winType = PTM_WinLineType.Full;
				break;
			}
			pTM_WinLineResult.cellType = (PTM_CellType)PTM_CellManager.cellManager.gameType[lineIndex];
			pTM_WinLineResult.lineIndex = lineIndex;
			pTM_WinLineResult.rate = 0;
			list.Add(pTM_WinLineResult);
		}
		return list.ToArray();
	}

	public static int CalculateHitNum(PTM_WinLineType winlinetype)
	{
		switch (winlinetype)
		{
		case PTM_WinLineType.Left2:
			return 2;
		case PTM_WinLineType.Left3:
			return 3;
		case PTM_WinLineType.Left4:
			return 4;
		case PTM_WinLineType.Full:
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
