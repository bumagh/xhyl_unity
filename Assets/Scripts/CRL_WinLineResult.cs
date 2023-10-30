public class CRL_WinLineResult
{
	public CRL_WinLineType winType;

	public int lineIndex;

	public CRL_CellType cellType;

	public int rate;

	public int dragonNum;

	public int maryTimes;

	public override string ToString()
	{
		return $"lineIndex: {lineIndex}, winType: {winType}, cellType: {cellType}, rate: {rate}, dragonNum: {dragonNum}, maryTimes: {maryTimes}";
	}
}
