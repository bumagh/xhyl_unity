using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STWM_CellManager : MonoBehaviour
{
	private STWM_Cell[,] cells;

	private int[,] grid;

	private int[,] hitGrid;

	public bool stopRolling;

	public bool forceStopRolling;

	public Action rollingFinishedAction;

	public Action allFinishedAction;

	private bool bPreInit;

	private STWM_OverallComputer overall;

	private STWM_HitLineComputer[] winLines;

	private int winCount;

	private int overallWin;

	private int totalWin;

	private List<STWM_WinLineResult> lineResults;

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
			2,
			2,
			2,
			1
		},
		{
			1,
			0,
			0,
			0,
			1
		}
	};

	private void Awake()
	{
		PreInit();
	}

	private void Start()
	{
		SetGridRandom(ref grid);
		SetCells(grid);
	}

	public void PreInit()
	{
		if (bPreInit)
		{
			return;
		}
		bPreInit = true;
		cells = new STWM_Cell[3, 5];
		grid = new int[3, 5];
		hitGrid = new int[3, 5];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				cells[i, j] = base.transform.Find($"Cell{i}_{j}").GetComponent<STWM_Cell>();
			}
		}
	}

	public void HideAllCellBorders()
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				cells[i, j].HideBorder();
			}
		}
	}

	public void UpdateCellGridData(int[,] grid)
	{
		this.grid = grid;
	}

	public void SetAllCellsState(string state = "normal")
	{
		SetCells(grid, state);
	}

	public void ResetAllCells()
	{
		if (cells == null)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				cells[i, j].ResetCell();
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
				cells[j, i].RolllingStart();
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
					yield return StartCoroutine(cells[row2, col2].RolllingEnd((STWM_CellType)grid[row2, col2]));
					break;
				}
				StartCoroutine(cells[row2, col2].RolllingEnd((STWM_CellType)grid[row2, col2]));
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
		rollingFinishedAction();
		endAction?.Invoke();
	}

	public int CalHitResult2(STWM_MajorResult majorResult)
	{
		int lineNum = majorResult.lineNum;
		int singleStake = majorResult.singleStake;
		int num = 0;
		int num2 = 0;
		num = 0;
		overall = new STWM_OverallComputer(majorResult.gameContent);
		if (lineNum == 9)
		{
			overall.Calculate();
		}
		overallWin = overall.rate * lineNum * singleStake;
		num += overallWin;
		winLines = new STWM_HitLineComputer[lineNum];
		winCount = 0;
		ResetHitGrid();
		lineResults = new List<STWM_WinLineResult>();
		List<int[]> list = new List<int[]>();
		if (overall.rate > 0 && !overall.isWeaponOrHuman)
		{
			for (int i = 0; i < lineNum; i++)
			{
				STWM_WinLineResult sTWM_WinLineResult = new STWM_WinLineResult();
				int[] betLine = GetBetLine(majorResult.gameContent, i);
				STWM_HitLineComputer sTWM_HitLineComputer = new STWM_HitLineComputer(i, betLine);
				STWM_WinLineResult[] array = sTWM_HitLineComputer.Calculate2();
				sTWM_WinLineResult.winType = STWM_WinLineType.Full;
				sTWM_WinLineResult.lineIndex = i;
				sTWM_WinLineResult.cellType = overall.cellType;
				sTWM_WinLineResult.rate = overall.GetSingleLineRate();
				if (array.Length == 1)
				{
					sTWM_WinLineResult.dragonNum = array[0].dragonNum;
					sTWM_WinLineResult.maryTimes = array[0].maryTimes;
				}
				lineResults.Add(sTWM_WinLineResult);
				SetHitGridLine(i, STWM_WinLineType.Full);
				num2 += sTWM_WinLineResult.maryTimes;
				winCount++;
				list.Add(betLine);
				num += sTWM_WinLineResult.rate * singleStake;
			}
		}
		else
		{
			for (int j = 0; j < lineNum; j++)
			{
				int[] betLine2 = GetBetLine(majorResult.gameContent, j);
				STWM_HitLineComputer sTWM_HitLineComputer2 = new STWM_HitLineComputer(j, betLine2);
				list.Add(betLine2);
				STWM_WinLineResult[] array2 = sTWM_HitLineComputer2.Calculate2();
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
		if (num != majorResult.totalWin || num2 != majorResult.maryTimes)
		{
			string text = string.Format("[lineNum]: {3}, [singleStake]: {2},[totalWin]> server: {0}, client: {1}", majorResult.totalWin, num, singleStake, lineNum);
		}
		totalWin = num;
		totalWin = majorResult.totalWin;
		return totalWin;
	}

	private void PlayFullAwardAudio()
	{
		UnityEngine.Debug.Log("PlayFullAwardAudio:" + overall.overallType);
		STWM_SoundManager.Instance.PlayMajorGameAwardAudio(GetOverallType());
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

	public IEnumerator WinLinesAni(bool isReplay, float delay, int lineNum, int singleStake, STWM_LinesManager lineManager, STWM_BallManager ballManager, Action<int> changeScoreHandler, Action celebrateAction)
	{
		yield return new WaitForSeconds(delay);
		if (overall.rate > 0)
		{
			STWM_SoundManager.Instance.PlayDrawLineAudio(0);
			for (int j = 0; j < 5; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					cells[k, j].PlayState("flash");
					cells[k, j].ShowBorder("pink");
				}
			}
			changeScoreHandler(overallWin);
			yield return new WaitForSeconds(0.5f);
			HideAllCellBorders();
		}
		if (winCount > 0 || overall.rate != 0)
		{
			for (int l = 0; l < 5; l++)
			{
				for (int m = 0; m < 3; m++)
				{
					cells[m, l].PlayState("gray");
				}
			}
		}
		int lineAudioIndex = 0;
		int maxRate = 0;
		STWM_CellType celltype = STWM_CellType.None;
		STWM_WinLineType winlinetype = STWM_WinLineType.None;
		for (int i = 0; i < lineResults.Count; i++)
		{
			if (lineResults[i].winType != 0)
			{
				int lineIndex = lineResults[i].lineIndex;
				if (lineAudioIndex > 3)
				{
					lineAudioIndex = 0;
				}
				STWM_SoundManager.Instance.PlayDrawLineAudio(lineAudioIndex);
				lineAudioIndex++;
				ShowSingleLineBegin(lineIndex, lineResults[i].winType);
				lineManager.ShowLine(lineIndex, lineResults[i].winType);
				ballManager.ShowBrightBall(lineIndex);
				changeScoreHandler(singleStake * lineResults[i].rate);
				yield return new WaitForSeconds(0.5f);
				ShowSingleLineEnd(lineIndex);
				lineManager.HideLine(lineIndex);
				ballManager.HideBrightBall(lineIndex);
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
			celebrateAction?.Invoke();
			if (maxRate != 0 && overall.rate == 0)
			{
				STWM_SoundManager.Instance.PlayMajorGameAwardAudio(celltype, STWM_HitLineComputer.CalculateHitNum(winlinetype));
			}
			else if (overall.rate != 0)
			{
				PlayFullAwardAudio();
			}
			yield return StartCoroutine(WinCellsCelebrateAni(overall.rate != 0));
		}
		allFinishedAction();
	}

	private void SetCells(int[,] grid, string state = "normal")
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				cells[i, j].PlayState(state, (STWM_CellType)grid[i, j]);
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

	private int[,] SetGridSame(ref int[,] grid, STWM_CellType type = STWM_CellType.Dragon)
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

	private void ShowSingleLineBegin(int index, STWM_WinLineType lineType)
	{
		int num = 0;
		int num2 = 4;
		string state = "green";
		switch (lineType)
		{
		case STWM_WinLineType.None:
			UnityEngine.Debug.LogError("should not be none");
			return;
		case STWM_WinLineType.Left3:
			num2 = 2;
			state = "green";
			break;
		case STWM_WinLineType.Left4:
			num2 = 3;
			state = "blue";
			break;
		case STWM_WinLineType.Right3:
			num = 2;
			state = "green";
			break;
		case STWM_WinLineType.Right4:
			num = 1;
			state = "blue";
			break;
		case STWM_WinLineType.Full:
			num = 0;
			num2 = 4;
			state = "pink";
			break;
		}
		for (int i = num; i <= num2; i++)
		{
			cells[LineMap[index, i], i].ShowBorder(state);
			cells[LineMap[index, i], i].PlayState("flash");
		}
	}

	private void ShowAllHitCell()
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (hitGrid[i, j] >= 3 || overall.rate > 0)
				{
					if (overall.rate > 0)
					{
						cells[i, j].ShowBorder("color");
					}
					else
					{
						cells[i, j].ShowBorder(GetBorderColor(hitGrid[i, j]));
					}
					cells[i, j].PlayState("celebrate");
				}
				else
				{
					cells[i, j].HideBorder();
					cells[i, j].PlayState("gray");
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
			cells[LineMap[index, i], i].HideBorder();
			cells[LineMap[index, i], i].PlayState("gray");
		}
	}

	public IEnumerator WinCellsCelebrateAni(bool isOverall)
	{
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
			coShowAllHitCell = StartCoroutine(STWM_Utils.DelayCall(0.5f, delegate
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
			coShowAllHitCell = StartCoroutine(STWM_Utils.DelayCall(0.5f, delegate
			{
				if (celebrateCount < num)
				{
					ShowAllHitCell();
				}
				coShowAllHitCell = null;
			}));
			UnityEngine.Debug.Log(STWM_LogHelper.Aqua("eventName: {0}, _celebrateCount: {1}", eventName, celebrateCount));
		}
	}

	public void StopShowAllHitCell()
	{
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

	private void SetHitGridLine(int lineIndex, STWM_WinLineType winType)
	{
		int num = 0;
		int num2 = 0;
		int a = 3;
		switch (winType)
		{
		case STWM_WinLineType.Left3:
			num = 0;
			num2 = 3;
			a = 3;
			break;
		case STWM_WinLineType.Left4:
			num = 0;
			num2 = 4;
			a = 4;
			break;
		case STWM_WinLineType.Right3:
			num = 2;
			num2 = 5;
			a = 3;
			break;
		case STWM_WinLineType.Right4:
			num = 1;
			num2 = 5;
			a = 4;
			break;
		case STWM_WinLineType.Full:
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
