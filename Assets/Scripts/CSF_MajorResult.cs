using System.Collections.Generic;

public class CSF_MajorResult
{
	public int[,] gameContent;

	public int[,] oldGameContent;

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

	public CSF_MajorResult Init(int lineNum, int singleStake, int totalStake, Dictionary<string, object> dic, CSF_CellManager cSF_CellManager, out bool isChange)
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
		isChange = (bool)dic["m_IsWild"];
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
		int[][] array;
		int[][] array2;
		if (isChange)
		{
			array = (dic["OldGameContent"] as int[][]);
			array2 = (dic["gameContent"] as int[][]);
		}
		else
		{
			array = (dic["gameContent"] as int[][]);
			array2 = (dic["gameContent"] as int[][]);
		}
		oldGameContent = new int[3, 5];
		gameContent = new int[3, 5];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				gameContent[i, j] = array[i][j];
				oldGameContent[i, j] = array2[i][j];
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
		if (isChange)
		{
			for (int k = 0; k < 3; k++)
			{
				for (int l = 0; l < 5; l++)
				{
					if (gameContent[k, l] != oldGameContent[k, l])
					{
						cSF_CellManager.Changeline.Add(l);
					}
				}
			}
		}
		int[] array3 = dic["GameLineCount"] as int[];
		for (int m = 0; m < array3.Length; m++)
		{
			cSF_CellManager.lineResultsIndex.Add(m);
			cSF_CellManager.gameLineCount.Add(array3[m]);
		}
		int[] array4 = dic["LR"] as int[];
		for (int n = 0; n < array4.Length; n++)
		{
			cSF_CellManager.gameLineLOrR.Add(array4[n]);
		}
		int[] array5 = dic["GameType"] as int[];
		for (int num = 0; num < array5.Length; num++)
		{
			cSF_CellManager.gameType.Add(array5[num]);
		}
		return this;
	}
}
