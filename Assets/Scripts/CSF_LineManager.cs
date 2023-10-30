using UnityEngine;

public class CSF_LineManager : MonoBehaviour
{
	private CSF_Line[] lines;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		lines = new CSF_Line[9];
		for (int i = 0; i < 9; i++)
		{
			lines[i] = base.transform.Find("Line" + (i + 1)).GetComponent<CSF_Line>();
			lines[i].index = i;
			lines[i].gameObject.SetActive(value: false);
		}
	}

	public void ShowLine(int line, CSF_WinLineType lineType = CSF_WinLineType.None)
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
			lines[i].Show(CSF_WinLineType.None);
		}
	}

	public void HideAllLines()
	{
		CSF_Line[] array = lines;
		foreach (CSF_Line cSF_Line in array)
		{
			cSF_Line.Hide();
		}
	}
}
