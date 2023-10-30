using UnityEngine;

public class CRL_LineManager : MonoBehaviour
{
	private CRL_Line[] lines;

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		lines = new CRL_Line[9];
		for (int i = 0; i < 9; i++)
		{
			lines[i] = base.transform.Find("Line" + (i + 1)).GetComponent<CRL_Line>();
			lines[i].index = i;
			lines[i].gameObject.SetActive(value: false);
		}
	}

	public void ShowLine(int line, CRL_WinLineType lineType = CRL_WinLineType.None)
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
			lines[i].Show(CRL_WinLineType.None);
		}
	}

	public void HideAllLines()
	{
		CRL_Line[] array = lines;
		foreach (CRL_Line cRL_Line in array)
		{
			cRL_Line.Hide();
		}
	}
}
