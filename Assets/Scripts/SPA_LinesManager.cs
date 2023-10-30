using UnityEngine;

public class SPA_LinesManager : MonoBehaviour
{
	private SPA_Lines lines;

	public void ShowLine(int line, SPA_WinLineType lineType = SPA_WinLineType.None)
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<SPA_Lines>();
		}
		lines.imgLines[line].SetActive(value: true);
	}

	public void HideLine(int line)
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<SPA_Lines>();
		}
		lines.imgLines[line].SetActive(value: false);
	}

	public void HideAllLines()
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<SPA_Lines>();
		}
		for (int i = 0; i < base.transform.Find("Mask1").childCount; i++)
		{
			lines.imgLines[i].SetActive(value: false);
		}
	}
}
