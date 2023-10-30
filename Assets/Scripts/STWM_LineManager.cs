using UnityEngine;

public class STWM_LineManager : MonoBehaviour
{
	private STWM_Line[] lines;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		lines = new STWM_Line[9];
		for (int i = 0; i < 9; i++)
		{
			lines[i] = base.transform.Find("Line" + (i + 1)).GetComponent<STWM_Line>();
			lines[i].index = i;
			lines[i].gameObject.SetActive(value: false);
		}
	}

	public void ShowLine(int line, STWM_WinLineType lineType = STWM_WinLineType.None)
	{
		lines[line].Show(lineType);
	}

	public void HideLine(int line)
	{
		lines[line].Hide();
	}

	public void ShowLines(int total = 9)
	{
		for (int i = 0; i < total; i++)
		{
			lines[i].Show(STWM_WinLineType.None);
		}
	}

	public void HideAllLines()
	{
		STWM_Line[] array = lines;
		foreach (STWM_Line sTWM_Line in array)
		{
			sTWM_Line.Hide();
		}
	}
}
