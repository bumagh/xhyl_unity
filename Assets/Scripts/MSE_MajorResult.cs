using System.Collections.Generic;

public class MSE_MajorResult
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

	public MSE_MajorResult Init(int lineNum, int singleStake, int totalStake, Dictionary<string, object> dic)
	{
		this.lineNum = lineNum;
		this.singleStake = singleStake;
		this.totalStake = totalStake;
		totalWin = (int)dic["totalWin"];
		Cinstance<MSE_Gcore>.Instance.Result = new List<int>();
		Cinstance<MSE_Gcore>.Instance.WinlineList = new List<int[][]>();
		Cinstance<MSE_Gcore>.Instance.Winlinenumlist = new List<int>();
		Cinstance<MSE_Gcore>.Instance.WinScoreList = new List<int>();
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
		int[,] array2 = gameContent;
		int length = array2.GetLength(0);
		int length2 = array2.GetLength(1);
		for (int k = 0; k < length; k++)
		{
			for (int l = 0; l < length2; l++)
			{
				int num = array2[k, l];
				Cinstance<MSE_Gcore>.Instance.Result.Add(num - 1);
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
		int[] array3 = dic["GameLineCount"] as int[];
		int[][][] array4 = dic["Line"] as int[][][];
		int[] array5 = dic["GameWin"] as int[];
		if (array4 != null && array4.Length > 0)
		{
			int[][][] array6 = array4;
			foreach (int[][] item in array6)
			{
				Cinstance<MSE_Gcore>.Instance.WinlineList.Add(item);
			}
		}
		for (int n = 0; n < array5.Length; n++)
		{
			if (array5[n] != 0)
			{
				Cinstance<MSE_Gcore>.Instance.Winlinenumlist.Add(array3[n]);
				Cinstance<MSE_Gcore>.Instance.WinScoreList.Add(array5[n]);
			}
		}
		int[] array7 = dic["GameType"] as int[];
		return this;
	}
}
