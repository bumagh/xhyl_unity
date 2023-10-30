using UnityEngine;

public class LKB_LineManager : MonoBehaviour
{
	private LKB_Line[] lines;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		lines = new LKB_Line[9];
		for (int i = 0; i < 9; i++)
		{
			lines[i] = base.transform.Find("Line" + (i + 1)).GetComponent<LKB_Line>();
			lines[i].index = i;
			lines[i].gameObject.SetActive(value: false);
		}
	}

	public void ShowLine(int line, LKB_WinLineType lineType = LKB_WinLineType.None)
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
			lines[i].Show(LKB_WinLineType.None);
		}
	}

	public void HideAllLines()
	{
		LKB_Line[] array = lines;
		foreach (LKB_Line lKB_Line in array)
		{
			lKB_Line.Hide();
		}
	}
}
