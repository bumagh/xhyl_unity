using UnityEngine;

public class PTM_MaryCellManager : MonoBehaviour
{
	private PTM_Cell[] cells;

	private void Awake()
	{
		PreInit();
	}

	public void PreInit()
	{
		cells = new PTM_Cell[4];
		for (int i = 0; i < 4; i++)
		{
			cells[i] = base.transform.Find("Cell" + i.ToString()).GetComponent<PTM_Cell>();
		}
	}
}
