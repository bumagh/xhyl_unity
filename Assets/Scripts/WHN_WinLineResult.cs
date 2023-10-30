public class WHN_WinLineResult
{
	public WHN_WinLineType winType;

	public int lineIndex;

	public WHN_CellType cellType;

	public int rate;

	public int dragonNum;

	public int maryTimes;

	public override string ToString()
	{
		return $"lineIndex: {lineIndex}, winType: {winType}, cellType: {cellType}, rate: {rate}, dragonNum: {dragonNum}, maryTimes: {maryTimes}";
	}
}
