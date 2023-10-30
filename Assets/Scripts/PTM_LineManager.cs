using UnityEngine;

public class PTM_LineManager : MonoBehaviour
{
	private PTM_Line[] lines;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		lines = new PTM_Line[9];
		for (int i = 0; i < 9; i++)
		{
			lines[i] = base.transform.Find("Line" + (i + 1)).GetComponent<PTM_Line>();
			lines[i].index = i;
			lines[i].gameObject.SetActive(value: false);
		}
	}

	public void ShowLine(int line, PTM_WinLineType lineType = PTM_WinLineType.None)
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
			lines[i].Show(PTM_WinLineType.None);
		}
	}

	public void HideAllLines()
	{
		PTM_Line[] array = lines;
		foreach (PTM_Line pTM_Line in array)
		{
			pTM_Line.Hide();
		}
	}
}
