using System.Collections;
using UnityEngine;

public class DPR_MaryCellManager : MonoBehaviour
{
	private DPR_Cell[] cells;

	private void Awake()
	{
		PreInit();
	}

	public void PreInit()
	{
		cells = new DPR_Cell[4];
		for (int i = 0; i < 4; i++)
		{
			cells[i] = base.transform.Find("Cell" + i.ToString()).GetComponent<DPR_Cell>();
		}
	}

	public void SetCells(int[] array, DPR_CellState cSF_CellState = DPR_CellState.normal)
	{
		for (int i = 0; i < 4; i++)
		{
			cells[i].PlayState(cSF_CellState, (DPR_CellType)array[i]);
		}
	}

	public void ResetCells()
	{
		for (int i = 0; i < 4; i++)
		{
			cells[i].ResetCell();
		}
	}

	public void SetCellsHitState(int[] array, bool isFlash = false)
	{
		for (int i = 0; i < 4; i++)
		{
			if (array[i] == 0)
			{
				cells[i].PlayState(DPR_CellState.gray);
			}
			else if (isFlash)
			{
				cells[i].PlayState(DPR_CellState.flash);
				cells[i].ShowBorder("pink");
			}
			else
			{
				cells[i].PlayState(DPR_CellState.celebrate);
				cells[i].ShowBorder("color");
			}
		}
	}

	public void SetAllCellsState(DPR_CellState cSF_CellState = DPR_CellState.normal)
	{
		for (int i = 0; i < 4; i++)
		{
			cells[i].PlayState(cSF_CellState);
		}
	}

	public void ResetAllCells()
	{
		if (cells != null)
		{
			for (int i = 0; i < 4; i++)
			{
				cells[i].ResetCell();
			}
		}
	}

	public void HideAllBorders()
	{
		for (int i = 0; i < 4; i++)
		{
			cells[i].HideBorder();
		}
	}

	public IEnumerator Rolling(int[] array)
	{
		UnityEngine.Debug.Log("MaryCellManager>: Rolling");
		for (int j = 0; j < 4; j++)
		{
			cells[j].RolllingStart();
		}
		yield return new WaitForSeconds(1.8f);
		for (int i = 0; i < 4; i++)
		{
			StartCoroutine(cells[i].RolllingEnd((DPR_CellType)array[i]));
			yield return new WaitForSeconds(0.1f);
		}
		yield return null;
		UnityEngine.Debug.Log("MaryCellManager>: Rolling: end");
	}
}
