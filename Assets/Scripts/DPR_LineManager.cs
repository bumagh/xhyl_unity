using UnityEngine;

public class DPR_LineManager : MonoBehaviour
{
	private DPR_Line[] lines;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		lines = new DPR_Line[9];
		for (int i = 0; i < 9; i++)
		{
			lines[i] = base.transform.Find("Line" + (i + 1)).GetComponent<DPR_Line>();
			lines[i].index = i;
			lines[i].gameObject.SetActive(value: false);
		}
	}

	public void ShowLine(int line, DPR_WinLineType lineType = DPR_WinLineType.None)
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
			lines[i].Show(DPR_WinLineType.None);
		}
	}

	public void HideAllLines()
	{
		DPR_Line[] array = lines;
		foreach (DPR_Line dPR_Line in array)
		{
			dPR_Line.Hide();
		}
	}
}
