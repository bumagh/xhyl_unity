using UnityEngine;

public class PTM_LinesManager : MonoBehaviour
{
	private PTM_Lines lines;

	public void ShowLine(int line, PTM_WinLineType lineType = PTM_WinLineType.None)
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<PTM_Lines>();
		}
		lines.imgLines[line].SetActive(value: true);
	}

	public void HideLine(int line)
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<PTM_Lines>();
		}
		lines.imgLines[line].SetActive(value: false);
	}

	public void HideAllLines()
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<PTM_Lines>();
		}
		for (int i = 0; i < base.transform.Find("Mask1").childCount; i++)
		{
			lines.imgLines[i].SetActive(value: false);
		}
	}
}
