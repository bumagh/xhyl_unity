using UnityEngine;

public class CRL_LinesManager : MonoBehaviour
{
	private CRL_Lines lines;

	public void ShowLine(int line, CRL_WinLineType lineType = CRL_WinLineType.None)
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<CRL_Lines>();
		}
		lines.imgLines[line].SetActive(value: true);
	}

	public void HideLine(int line)
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<CRL_Lines>();
		}
		lines.imgLines[line].SetActive(value: false);
	}

	public void HideAllLines()
	{
		if (lines == null)
		{
			lines = base.transform.Find("Mask1").GetComponent<CRL_Lines>();
		}
		for (int i = 0; i < base.transform.Find("Mask1").childCount; i++)
		{
			lines.imgLines[i].SetActive(value: false);
		}
	}
}
