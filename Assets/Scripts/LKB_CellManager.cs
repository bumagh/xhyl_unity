using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LKB_CellManager : MonoBehaviour
{
	public static LKB_CellManager cellManager;

	public STWM_SpiCell[] spiCells;

	private int[,] grid;

	private int[,] hitGrid;

	[HideInInspector]
	public bool stopRolling;

	[HideInInspector]
	public bool forceStopRolling;

	public Action rollingFinishedAction;

	public Action allFinishedAction;

	private bool bPreInit;

	private LKB_OverallComputer overall;

	private LKB_HitLineComputer[] winLines;

	private int winCount;

	private int overallWin;

	private int totalWin;

	private List<LKB_WinLineResult> lineResults;

	[HideInInspector]
	public List<int> lineResultsIndex;

	[HideInInspector]
	public List<int> gameLineCount;

	[HideInInspector]
	public List<int> gameLineLOrR;

	[HideInInspector]
	public List<int> gameType;

	[HideInInspector]
	public List<int> Changeline;

	[HideInInspector]
	public int celebrateCount;

	private int num;

	private Coroutine coShowAllHitCell;

	public static int[,] LineMap = new int[9, 5]
	{
		{
			1,
			1,
			1,
			1,
			1
		},
		{
			0,
			0,
			0,
			0,
			0
		},
		{
			2,
			2,
			2,
			2,
			2
		},
		{
			0,
			1,
			2,
			1,
			0
		},
		{
			2,
			1,
			0,
			1,
			2
		},
		{
			0,
			0,
			1,
			0,
			0
		},
		{
			2,
			2,
			1,
			2,
			2
		},
		{
			1,
			0,
			0,
			0,
			1
		},
		{
			1,
			2,
			2,
			2,
			1
		}
	};

	public LKB_Cell[,] Cells
	{
		get;
		set;
	}

	private void Awake()
	{
		cellManager = this;
		PreInit();
	}

	private void Start()
	{
		SetGridRandom(ref grid);
		SetCells(grid);
	}

	public void ChangeSym()
	{
		List<int> showchange = LKB_MB_Singleton<LKB_MajorGameController>.GetInstance().Showchange1;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (IsObjectInTheArray(showchange, j))
				{
					Cells[i, j].PlayState(LKB_CellState.normal, LKB_CellType.LanTongZi);
				}
			}
		}
	}

	public static bool IsObjectInTheArray(List<int> sits, int name)
	{
		for (int i = 0; i < sits.Count; i++)
		{
			if (name.Equals(sits[i]))
			{
				return true;
			}
		}
		return false;
	}

	public void PreInit()
	{
		if (bPreInit)
		{
			return;
		}
		bPreInit = true;
		Cells = new LKB_Cell[3, 5];
		grid = new int[3, 5];
		hitGrid = new int[3, 5];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				Cells[i, j] = base.transform.Find($"Cell{i}_{j}").GetComponent<LKB_Cell>();
			}
		}
	}

	public void HideAllCellBorders()
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				Cells[i, j].HideBorder();
			}
		}
	}

	public void UpdateCellGridData(int[,] grid)
	{
		this.grid = grid;
	}

	public void SetAllCellsState(LKB_CellState cSF_CellState = LKB_CellState.normal)
	{
		SetCells(grid, cSF_CellState);
	}

	public void ResetAllCells()
	{
		if (Cells == null)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				Cells[i, j].ResetCell();
			}
		}
	}

	public IEnumerator GridRollingControl(Action endAction)
	{
		stopRolling = false;
		forceStopRolling = false;
		float time = Time.time;
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				Cells[j, i].RolllingStart();
			}
		}
		yield return StartCoroutine(WaitForStopRolling(1.2f, 2.14748365E+09f));
		if (forceStopRolling)
		{
			yield return new WaitForSeconds(0.2f);
		}
		for (int col2 = 0; col2 < 5; col2++)
		{
			for (int row2 = 0; row2 < 3; row2++)
			{
				if (col2 == 4 && row2 == 2)
				{
					yield return StartCoroutine(Cells[row2, col2].RolllingEnd((LKB_CellType)grid[row2, col2]));
					break;
				}
				StartCoroutine(Cells[row2, col2].RolllingEnd((LKB_CellType)grid[row2, col2]));
				if (!forceStopRolling)
				{
					yield return new WaitForSeconds(0.08f);
				}
			}
			if ((!forceStopRolling) ? true : false)
			{
				yield return new WaitForSeconds(0.12f);
			}
		}
		UnityEngine.Debug.Log("play line hitting animation");
		rollingFinishedAction();
		endAction?.Invoke();
	}

	public int CalHitResult2(LKB_MajorResult majorResult)
	{
		int lineNum = majorResult.lineNum;
		int singleStake = majorResult.singleStake;
		int num = 0;
		int num2 = 0;
		num = 0;
		overall = new LKB_OverallComputer(majorResult.gameContent);
		overall.Calculate();
		overallWin = overall.rate * lineNum * singleStake;
		UnityEngine.Debug.Log($"[overall] type: {overall.overallType}, rate: {overall.rate}");
		num += overallWin;
		winLines = new LKB_HitLineComputer[lineNum];
		winCount = 0;
		ResetHitGrid();
		lineResults = new List<LKB_WinLineResult>();
		List<int[]> list = new List<int[]>();
		bool flag = overall.rate > 0 && !overall.isWeaponOrHuman;
		UnityEngine.Debug.LogError("flag: " + flag);
		if (flag)
		{
			for (int i = 0; i < lineNum; i++)
			{
				LKB_WinLineResult lKB_WinLineResult = new LKB_WinLineResult();
				int[] betLine = GetBetLine(majorResult.gameContent, i);
				LKB_HitLineComputer lKB_HitLineComputer = new LKB_HitLineComputer(i, betLine);
				LKB_WinLineResult[] array = lKB_HitLineComputer.Calculate2();
				lKB_WinLineResult.winType = LKB_WinLineType.Full;
				lKB_WinLineResult.lineIndex = i;
				lKB_WinLineResult.cellType = overall.cellType;
				lKB_WinLineResult.rate = overall.GetSingleLineRate();
				if (array.Length == 1)
				{
					lKB_WinLineResult.dragonNum = array[0].dragonNum;
					lKB_WinLineResult.maryTimes = array[0].maryTimes;
				}
				lineResults.Add(lKB_WinLineResult);
				SetHitGridLine(i, LKB_WinLineType.Full);
				num2 += lKB_WinLineResult.maryTimes;
				winCount++;
				list.Add(betLine);
				num += lKB_WinLineResult.rate * singleStake;
			}
		}
		else
		{
			for (int j = 0; j < lineNum; j++)
			{
				int[] betLine2 = GetBetLine(majorResult.gameContent, j);
				LKB_HitLineComputer lKB_HitLineComputer2 = new LKB_HitLineComputer(j, betLine2);
				list.Add(betLine2);
				LKB_WinLineResult[] array2 = lKB_HitLineComputer2.Calculate2();
				if (array2.Length == 2)
				{
					if (array2[0].rate >= array2[1].rate)
					{
						lineResults.Add(array2[0]);
						lineResults.Add(array2[1]);
					}
					else
					{
						lineResults.Add(array2[1]);
						lineResults.Add(array2[0]);
					}
				}
				else if (array2.Length == 1)
				{
					lineResults.Add(array2[0]);
				}
				for (int k = 0; k < array2.Length; k++)
				{
					SetHitGridLine(j, array2[k].winType);
					winCount += ((array2[k].winType != 0) ? 1 : 0);
					int num3 = singleStake * array2[k].rate;
					num += num3;
					num2 += array2[k].maryTimes;
				}
			}
		}
		if (num != majorResult.totalWin)
		{
			string str = string.Format("[lineNum]: {3}, [singleStake]: {2},[totalWin]> server: {0}, client: {1}", majorResult.totalWin, num, singleStake, lineNum);
			UnityEngine.Debug.LogError("消息: " + str);
		}
		else
		{
			UnityEngine.Debug.Log(" overall: " + overall.overallType);
		}
		totalWin = num;
		totalWin = majorResult.totalWin;
		return totalWin;
	}

	private void PlayFullAwardAudio()
	{
		UnityEngine.Debug.Log("PlayFullAwardAudio:" + overall.overallType);
		LKB_SoundManager.Instance.PlayMajorGameAwardAudio(GetOverallType());
	}

	private string GetOverallType()
	{
		if (overall.overallType == "none")
		{
			return null;
		}
		string[] array = overall.overallType.Split('_');
		return array[1];
	}

	public IEnumerator WinLinesAni(bool isReplay, float delay, int lineNum, int singleStake, LKB_LinesManager lineManager, LKB_BallManager ballManager, Action<int> changeScoreHandler, Action celebrateAction)
	{
		yield return new WaitForSeconds(delay);
		if (overall.rate > 0)
		{
			UnityEngine.Debug.LogError("overall.rate: " + overall.rate);
			LKB_SoundManager.Instance.PlayDrawLineAudio(0);
			for (int j = 0; j < 5; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					Cells[k, j].PlayState(LKB_CellState.flash);
					Cells[k, j].ShowBorder("pink");
				}
			}
			changeScoreHandler(overallWin);
			yield return new WaitForSeconds(0.5f);
			HideAllCellBorders();
		}
		if (winCount > 0 || overall.rate != 0)
		{
			UnityEngine.Debug.LogError("赢线: " + winCount);
			LKB_SoundManager.Instance.PlayMajorGameAwardAudio(winCount - 1);
			for (int l = 0; l < 5; l++)
			{
				for (int m = 0; m < 3; m++)
				{
					Cells[m, l].PlayState(LKB_CellState.gray);
				}
			}
		}
		int lineAudioIndex = 0;
		int maxRate = 0;
		LKB_CellType celltype = LKB_CellType.None;
		LKB_WinLineType winlinetype = LKB_WinLineType.None;
		for (int i = 0; i < lineResults.Count; i++)
		{
			if (lineResults[i].winType != 0)
			{
				UnityEngine.Debug.LogError("赢线方向及个数: " + lineResults[i].winType + "  " + (lineResults[i].lineIndex + 1) + " 号线赢线");
				int lineIndex = lineResults[i].lineIndex;
				LKB_SoundManager.Instance.PlayDrawLineAudio(lineAudioIndex);
				lineAudioIndex++;
				ShowSingleLineBegin(lineIndex, lineResults[i].winType);
				lineManager.ShowLine(lineIndex, lineResults[i].winType);
				ballManager.ShowBrightBall(lineIndex);
				changeScoreHandler(singleStake * lineResults[i].rate);
				yield return new WaitForSeconds(0.5f);
				ShowSingleLineEnd(lineIndex);
				lineManager.HideLine(lineIndex);
				StopCoroutine("WaitHideBrightBall");
				StartCoroutine(WaitHideBrightBall(ballManager, lineIndex));
				yield return null;
			}
			if (lineResults[i].rate > maxRate)
			{
				maxRate = lineResults[i].rate;
			}
			if (maxRate == lineResults[i].rate)
			{
				celltype = lineResults[i].cellType;
				winlinetype = lineResults[i].winType;
			}
		}
		if (!isReplay && (winCount > 0 || overall.rate != 0))
		{
			UnityEngine.Debug.LogError("_overall.rate: " + overall.rate);
			celebrateAction?.Invoke();
			UnityEngine.Debug.LogError("中奖类型: " + celltype + " 中奖个数: " + LKB_HitLineComputer.CalculateHitNum(winlinetype));
			yield return StartCoroutine(WinCellsCelebrateAni(overall.rate != 0));
		}
		allFinishedAction();
	}

	private IEnumerator WaitHideBrightBall(LKB_BallManager ballManager, int index)
	{
		yield return new WaitForSeconds(3f);
		ballManager.HideBrightBall(index);
	}

	private void SetCells(int[,] grid, LKB_CellState cSF_CellState = LKB_CellState.normal)
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				Cells[i, j].PlayState(cSF_CellState, (LKB_CellType)grid[i, j]);
			}
		}
	}

	private int[,] SetGridRandom(ref int[,] grid)
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				grid[i, j] = UnityEngine.Random.Range(1, 9);
			}
		}
		return grid;
	}

	private int[,] SetGridSame(ref int[,] grid, LKB_CellType type = LKB_CellType.LvTongZi)
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				grid[i, j] = (int)type;
			}
		}
		return grid;
	}

	private IEnumerator WaitForStopRolling(float minTime, float timeOut)
	{
		float timer = 0f;
		while (timer < timeOut)
		{
			timer += Time.deltaTime;
			if ((stopRolling && timer > minTime) || (forceStopRolling && stopRolling))
			{
				yield break;
			}
			yield return null;
		}
		if (!stopRolling)
		{
			UnityEngine.Debug.Log("time out");
		}
	}

	private void ShowSingleLineBegin(int index, LKB_WinLineType lineType)
	{
		UnityEngine.Debug.Log("显示框,播放动画");
		int num = 0;
		int num2 = 4;
		string state = "green";
		switch (lineType)
		{
		case LKB_WinLineType.None:
			UnityEngine.Debug.LogError("should not be none");
			return;
		case LKB_WinLineType.Left3:
			num2 = 2;
			state = "green";
			break;
		case LKB_WinLineType.Left4:
			num2 = 3;
			state = "blue";
			break;
		case LKB_WinLineType.Right3:
			num = 2;
			state = "green";
			break;
		case LKB_WinLineType.Right4:
			num = 1;
			state = "blue";
			break;
		case LKB_WinLineType.Full:
			num = 0;
			num2 = 4;
			state = "pink";
			break;
		}
		for (int i = num; i <= num2; i++)
		{
			Cells[LineMap[index, i], i].ShowBorder(state);
			Cells[LineMap[index, i], i].PlayState(LKB_CellState.flash);
		}
	}

	private void ShowAllHitCell()
	{
		UnityEngine.Debug.Log("_showAllHitCell");
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (hitGrid[i, j] >= 3 || overall.rate > 0)
				{
					if (overall.rate > 0)
					{
						Cells[i, j].ShowBorder("color");
					}
					else
					{
						Cells[i, j].ShowBorder(GetBorderColor(hitGrid[i, j]));
					}
					Cells[i, j].PlayState(LKB_CellState.celebrate);
				}
				else
				{
					Cells[i, j].HideBorder();
					Cells[i, j].PlayState(LKB_CellState.gray);
				}
			}
		}
	}

	private string GetBorderColor(int hitNum)
	{
		string result = "green";
		switch (hitNum)
		{
		case 3:
			result = "green";
			break;
		case 4:
			result = "blue";
			break;
		case 5:
			result = "pink";
			break;
		}
		return result;
	}

	private void ShowSingleLineEnd(int index)
	{
		for (int i = 0; i < 5; i++)
		{
			Cells[LineMap[index, i], i].HideBorder();
			Cells[LineMap[index, i], i].PlayState(LKB_CellState.gray);
		}
	}

	public IEnumerator WinCellsCelebrateAni(bool isOverall)
	{
		UnityEngine.Debug.Log("_winCellsCelebrateAni  isOverall:" + isOverall);
		celebrateCount = 0;
		ShowAllHitCell();
		if (isOverall)
		{
			if (GetOverallType() == "weapon" || GetOverallType() == "human")
			{
				num = 3;
			}
			else
			{
				num = 4;
			}
			yield return StartCoroutine(WaitForCelebrate(num));
		}
		else
		{
			yield return new WaitForSeconds(3f);
		}
	}

	private IEnumerator WaitForCelebrate(int num)
	{
		while (celebrateCount < num)
		{
			MonoBehaviour.print(celebrateCount + "-----------------------------" + Time.time);
			yield return new WaitForSeconds(3f);
			coShowAllHitCell = StartCoroutine(LKB_Utils.DelayCall(0.5f, delegate
			{
				if (celebrateCount < num)
				{
					ShowAllHitCell();
				}
				coShowAllHitCell = null;
			}));
			celebrateCount++;
		}
		yield return null;
	}

	public void OnOverallPlayed(string eventName)
	{
		if (overall.rate != 0)
		{
			celebrateCount++;
			coShowAllHitCell = StartCoroutine(LKB_Utils.DelayCall(0.5f, delegate
			{
				if (celebrateCount < num)
				{
					ShowAllHitCell();
				}
				coShowAllHitCell = null;
			}));
			UnityEngine.Debug.Log(LKB_LogHelper.Aqua("eventName: {0}, _celebrateCount: {1}", eventName, celebrateCount));
		}
	}

	public void StopShowAllHitCell()
	{
		UnityEngine.Debug.Log("停止协程");
		if (coShowAllHitCell != null)
		{
			StopCoroutine(coShowAllHitCell);
		}
	}

	private void ResetHitGrid()
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				hitGrid[i, j] = 0;
			}
		}
	}

	private void SetHitGridLine(int lineIndex, LKB_WinLineType winType)
	{
		int num = 0;
		int num2 = 0;
		int a = 3;
		switch (winType)
		{
		case LKB_WinLineType.Left3:
			num = 0;
			num2 = 3;
			a = 3;
			break;
		case LKB_WinLineType.Left4:
			num = 0;
			num2 = 4;
			a = 4;
			break;
		case LKB_WinLineType.Right3:
			num = 2;
			num2 = 5;
			a = 3;
			break;
		case LKB_WinLineType.Right4:
			num = 1;
			num2 = 5;
			a = 4;
			break;
		case LKB_WinLineType.Full:
			num = 0;
			num2 = 5;
			a = 5;
			break;
		}
		for (int i = num; i < num2; i++)
		{
			hitGrid[LineMap[lineIndex, i], i] = Mathf.Max(a, hitGrid[LineMap[lineIndex, i], i]);
		}
	}

	public static int[] GetBetLine(int[,] matrix, int lineIndex)
	{
		int[] array = new int[5];
		for (int i = 0; i < 5; i++)
		{
			array[i] = matrix[LineMap[lineIndex, i], i];
		}
		return array;
	}
}
