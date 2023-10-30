using System.Collections.Generic;

public class PTM_MajorResult
{
	public int[,] gameContent;

	public int totalWin;

	public int freenWin;

	public bool maryGame;

	public bool isFreenGameStop;

	public int userScore;

	public int maryCount;

	public bool isFreenStart;

	public bool multipGame;

	public int specialAward;

	public int lineNum;

	public int singleStake;

	public int totalStake;

	public PTM_MajorResult Init(int lineNum, int singleStake, int totalStake, Dictionary<string, object> dic, PTM_CellManager cSF_CellManager)
	{
		this.lineNum = lineNum;
		this.singleStake = singleStake;
		this.totalStake = totalStake;
		totalWin = (int)dic["totalWin"];
		cSF_CellManager.lineResultsIndex = new List<int>();
		cSF_CellManager.gameLineCount = new List<int>();
		cSF_CellManager.gameType = new List<int>();
		cSF_CellManager.Changeline = new List<int>();
		cSF_CellManager.Winline = new List<int>();
		cSF_CellManager.GameWin = new List<int>();
		userScore = (int)dic["userScore"];
		isFreenGameStop = (bool)dic["m_IsFreeEnd"];
		if (isFreenGameStop)
		{
			freenWin = (int)dic["m_FreeWin"];
		}
		else
		{
			freenWin = 0;
		}
		int[][] array = dic["gameContent"] as int[][];
		gameContent = new int[3, 5];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				gameContent[i, j] = array[i][j];
			}
		}
		maryGame = (bool)dic["m_IsFree"];
		if (maryGame)
		{
			maryCount = (int)dic["m_FreeUsed"];
			isFreenStart = true;
		}
		else
		{
			maryCount = 0;
			isFreenStart = false;
		}
		int[] array2 = dic["GameLineCount"] as int[];
		for (int k = 0; k < array2.Length; k++)
		{
			cSF_CellManager.lineResultsIndex.Add(k);
			cSF_CellManager.gameLineCount.Add(array2[k]);
		}
		int[] array3 = dic["Line"] as int[];
		for (int l = 0; l < array3.Length; l++)
		{
			cSF_CellManager.Winline.Add(array3[l]);
		}
		int[] array4 = dic["GameWin"] as int[];
		for (int m = 0; m < array4.Length; m++)
		{
			cSF_CellManager.GameWin.Add(array4[m]);
		}
		int[] array5 = dic["GameType"] as int[];
		for (int n = 0; n < array5.Length; n++)
		{
			cSF_CellManager.gameType.Add(array5[n]);
		}
		return this;
	}
}
