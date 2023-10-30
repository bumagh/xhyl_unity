using System.Collections;
using UnityEngine;

public class CSF_MaryCellManager : MonoBehaviour
{
	private CSF_Cell[] cells;

	private void Awake()
	{
		PreInit();
	}

	public void PreInit()
	{
		cells = new CSF_Cell[4];
		for (int i = 0; i < 4; i++)
		{
			cells[i] = base.transform.Find("Cell" + i.ToString()).GetComponent<CSF_Cell>();
		}
	}

	public void SetCells(int[] array, CSF_CellState cSF_CellState = CSF_CellState.normal)
	{
		for (int i = 0; i < 4; i++)
		{
			cells[i].PlayState(cSF_CellState, (CSF_CellType)array[i]);
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
				cells[i].PlayState(CSF_CellState.gray);
			}
			else if (isFlash)
			{
				cells[i].PlayState(CSF_CellState.flash);
				cells[i].ShowBorder("pink");
			}
			else
			{
				cells[i].PlayState(CSF_CellState.celebrate);
				cells[i].ShowBorder("color");
			}
		}
	}

	public void SetAllCellsState(CSF_CellState cSF_CellState = CSF_CellState.normal)
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
			StartCoroutine(cells[i].RolllingEnd((CSF_CellType)array[i]));
			yield return new WaitForSeconds(0.1f);
		}
		yield return null;
		UnityEngine.Debug.Log("MaryCellManager>: Rolling: end");
	}
}
