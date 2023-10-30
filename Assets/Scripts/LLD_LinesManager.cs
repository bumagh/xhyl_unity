using UnityEngine;

public class LLD_LinesManager : MonoBehaviour
{
	private LLD_Lines lines;

	public void ShowLine(int line, LLD_WinLineType lineType = LLD_WinLineType.None)
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<LLD_Lines>();
		}
		lines.imgLines[line].SetActive(value: true);
	}

	public void HideLine(int line)
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<LLD_Lines>();
		}
		lines.imgLines[line].SetActive(value: false);
	}

	public void HideAllLines()
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<LLD_Lines>();
		}
		for (int i = 0; i < base.transform.Find("Mask1").childCount; i++)
		{
			lines.imgLines[i].SetActive(value: false);
		}
	}
}
