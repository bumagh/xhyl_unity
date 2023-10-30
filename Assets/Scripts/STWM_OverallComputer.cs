public class STWM_OverallComputer
{
	public const int AllHumanRate = 50;

	public const int AllWeaponRate = 15;

	private int[,] _matrix;

	public string overallType = "none";

	public int rate;

	public bool isWeaponOrHuman;

	public STWM_CellType cellType;

	public STWM_OverallComputer(int[,] matrix)
	{
		_matrix = matrix;
	}

	public void Calculate()
	{
		int[] array = new int[10];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				array[_matrix[i, j]]++;
			}
		}
		if (array[9] == 15)
		{
			overallType = "overall_dragon";
			rate = STWM_HitLineComputer.AwardRateMap[9][3];
			cellType = STWM_CellType.Dragon;
			return;
		}
		for (int k = 1; k < 9; k++)
		{
			if (array[k] + array[9] == 15)
			{
				STWM_CellType sTWM_CellType = (STWM_CellType)k;
				overallType = "overall_" + sTWM_CellType.ToString();
				cellType = (STWM_CellType)k;
				rate = STWM_HitLineComputer.AwardRateMap[k][3];
				return;
			}
		}
		if (array[1] + array[2] + array[3] + array[9] == 15)
		{
			overallType = "overall_weapon";
			rate = 15;
			isWeaponOrHuman = true;
		}
		else if (array[4] + array[5] + array[6] + array[9] == 15)
		{
			overallType = "overall_human";
			rate = 50;
			isWeaponOrHuman = true;
		}
	}

	public override string ToString()
	{
		return $"rate: {rate}, overallType: {overallType}";
	}

	public int GetSingleLineRate()
	{
		int result = 0;
		if (cellType != 0)
		{
			result = STWM_HitLineComputer.AwardRateMap[(int)cellType][2];
		}
		return result;
	}
}
