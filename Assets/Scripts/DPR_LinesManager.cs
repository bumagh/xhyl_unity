using UnityEngine;

public class DPR_LinesManager : MonoBehaviour
{
	private DPR_Lines lines;

	public void ShowLine(int line, DPR_WinLineType lineType = DPR_WinLineType.None)
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<DPR_Lines>();
		}
		lines.imgLines[line].SetActive(value: true);
	}

	public void HideLine(int line)
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<DPR_Lines>();
		}
		lines.imgLines[line].SetActive(value: false);
	}

	public void HideAllLines()
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<DPR_Lines>();
		}
		for (int i = 0; i < base.transform.Find("Mask1").childCount; i++)
		{
			lines.imgLines[i].SetActive(value: false);
		}
	}
}
