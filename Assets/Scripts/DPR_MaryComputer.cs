public class DPR_MaryComputer
{
	public const int HitThreeRate = 20;

	public const int HitFourRate = 500;

	public int tileRate;

	public int cellRate;

	public int totalRate;

	public int[] aniDic = new int[4];

	public DPR_MaryCellHitType cellHitType;

	public bool isTileWin;

	public int tileIndex;

	public DPR_CellType tileType;

	public int[] cells;

	public static readonly int[] TileRate = new int[10]
	{
		0,
		10,
		5,
		2,
		50,
		70,
		20,
		200,
		100,
		0
	};

	public DPR_MaryComputer(int tile, int[] cells)
	{
		tileType = (DPR_CellType)tile;
		this.cells = cells;
	}

	public void Calculate()
	{
		isTileWin = false;
		for (int i = 0; i < 4; i++)
		{
			if (cells[i] == (int)tileType)
			{
				isTileWin = true;
				aniDic[i] = 1;
			}
		}
		if (isTileWin)
		{
			tileRate = TileRate[(int)tileType];
		}
		int num = cells[0];
		int num2 = 1;
		for (int j = 1; j < 4 && num == cells[j]; j++)
		{
			num2++;
		}
		int num3 = cells[3];
		int num4 = 1;
		if (num2 < 2)
		{
			int num5 = 2;
			while (num5 >= 0 && num3 == cells[num5])
			{
				num4++;
				num5--;
			}
		}
		switch (num2)
		{
		case 4:
			cellRate = 500;
			cellHitType = DPR_MaryCellHitType.All;
			aniDic[0] = 1;
			aniDic[1] = 1;
			aniDic[2] = 1;
			aniDic[3] = 1;
			break;
		case 3:
			cellRate = 20;
			cellHitType = DPR_MaryCellHitType.Left3;
			aniDic[0] = 1;
			aniDic[1] = 1;
			aniDic[2] = 1;
			break;
		default:
			if (num4 == 3)
			{
				cellRate = 20;
				cellHitType = DPR_MaryCellHitType.Right3;
				aniDic[1] = 1;
				aniDic[2] = 1;
				aniDic[3] = 1;
			}
			else
			{
				cellRate = 0;
				cellHitType = DPR_MaryCellHitType.None;
			}
			break;
		}
		totalRate = cellRate + tileRate;
	}
}
