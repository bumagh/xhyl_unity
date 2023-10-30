using System.Collections.Generic;

public class LKB_MajorResult
{
	public int[,] gameContent;

	public int totalWin;

	public int userScore;

	public bool isFreenStart;

	public bool multipGame;

	public int specialAward;

	public int lineNum;

	public int singleStake;

	public int totalStake;

	public LKB_MajorResult Init(int lineNum, int singleStake, int totalStake, Dictionary<string, object> dic, LKB_CellManager cSF_CellManager)
	{
		this.lineNum = lineNum;
		this.singleStake = singleStake;
		this.totalStake = totalStake;
		totalWin = (int)dic["totalWin"];
		cSF_CellManager.lineResultsIndex = new List<int>();
		cSF_CellManager.gameLineCount = new List<int>();
		cSF_CellManager.gameLineLOrR = new List<int>();
		cSF_CellManager.gameType = new List<int>();
		cSF_CellManager.Changeline = new List<int>();
		userScore = (int)dic["userScore"];
		int[][] array = dic["gameContent"] as int[][];
		gameContent = new int[3, 5];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				gameContent[i, j] = array[i][j];
			}
		}
		isFreenStart = false;
		int[] array2 = dic["GameLineCount"] as int[];
		for (int k = 0; k < array2.Length; k++)
		{
			cSF_CellManager.lineResultsIndex.Add(k);
			cSF_CellManager.gameLineCount.Add(array2[k]);
		}
		int[] array3 = dic["LR"] as int[];
		for (int l = 0; l < array3.Length; l++)
		{
			cSF_CellManager.gameLineLOrR.Add(array3[l]);
		}
		int[] array4 = dic["GameType"] as int[];
		for (int m = 0; m < array4.Length; m++)
		{
			cSF_CellManager.gameType.Add(array4[m]);
		}
		return this;
	}
}
