public class DPR_OverallComputer
{
	public const int AllHumanRate = 50;

	public const int AllWeaponRate = 15;

	private int[,] _matrix;

	public string overallType = "none";

	public int rate;

	public bool isWeaponOrHuman;

	public DPR_CellType cellType;

	public DPR_OverallComputer(int[,] matrix)
	{
		_matrix = matrix;
	}

	public void Calculate()
	{
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
			result = DPR_HitLineComputer.AwardRateMap[(int)cellType][2];
		}
		return result;
	}
}
