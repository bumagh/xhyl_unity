using UnityEngine;

public class LLD_LineManager : MonoBehaviour
{
	private LLD_Line[] lines;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		lines = new LLD_Line[9];
		for (int i = 0; i < 9; i++)
		{
			lines[i] = base.transform.Find("Line" + (i + 1)).GetComponent<LLD_Line>();
			lines[i].index = i;
			lines[i].gameObject.SetActive(value: false);
		}
	}

	public void ShowLine(int line, LLD_WinLineType lineType = LLD_WinLineType.None)
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
			lines[i].Show(LLD_WinLineType.None);
		}
	}

	public void HideAllLines()
	{
		LLD_Line[] array = lines;
		foreach (LLD_Line lLD_Line in array)
		{
			lLD_Line.Hide();
		}
	}
}
