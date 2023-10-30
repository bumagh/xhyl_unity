using UnityEngine;

public class SPA_LineManager : MonoBehaviour
{
	private SPA_Line[] lines;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		lines = new SPA_Line[9];
		for (int i = 0; i < 9; i++)
		{
			lines[i] = base.transform.Find("Line" + (i + 1)).GetComponent<SPA_Line>();
			lines[i].index = i;
			lines[i].gameObject.SetActive(value: false);
		}
	}

	public void ShowLine(int line, SPA_WinLineType lineType = SPA_WinLineType.None)
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
			lines[i].Show(SPA_WinLineType.None);
		}
	}

	public void HideAllLines()
	{
		SPA_Line[] array = lines;
		foreach (SPA_Line sPA_Line in array)
		{
			sPA_Line.Hide();
		}
	}
}
