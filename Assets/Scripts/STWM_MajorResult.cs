using System.Collections.Generic;

public class STWM_MajorResult
{
	public int[,] gameContent;

	public int totalWin;

	public bool maryGame;

	public int maryTimes;

	public bool multipGame;

	public int specialAward;

	public int lineNum;

	public int singleStake;

	public int totalStake;

	public STWM_MajorResult Init(int lineNum, int singleStake, int totalStake, Dictionary<string, object> dic)
	{
		this.lineNum = lineNum;
		this.singleStake = singleStake;
		this.totalStake = totalStake;
		totalWin = (int)dic["totalWin"];
		if (STWM_MB_Singleton<STWM_NetManager>.GetInstance().useFake)
		{
			gameContent = (dic["gameContent"] as int[,]);
		}
		else
		{
			int[][] array = dic["gameContent"] as int[][];
			gameContent = new int[3, 5];
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					gameContent[i, j] = array[i][j];
				}
			}
			maryGame = (bool)dic["maryGame"];
			if (maryGame)
			{
				maryTimes = (int)dic["times"];
			}
			else
			{
				maryTimes = 0;
			}
			multipGame = (bool)dic["multipGame"];
			int num = (int)dic["specialAward"];
		}
		return this;
	}
}
