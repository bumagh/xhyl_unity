using UnityEngine;
using UnityEngine.UI;

public class STMF_DotsAnimCtrl : MonoBehaviour
{
	private Image[] imgDots;

	private int chooseIndex;

	private Transform imgCurDot;

	private void Awake()
	{
		imgDots = new Image[10];
		for (int i = 0; i < 10; i++)
		{
			imgDots[i] = base.transform.GetChild(i).GetComponent<Image>();
		}
		imgCurDot = base.transform.GetChild(10);
	}

	public void ChooseTable(int index)
	{
		if (chooseIndex != index)
		{
			chooseIndex = index;
			imgCurDot.localPosition = imgDots[chooseIndex].transform.localPosition;
		}
	}

	public void ShowDots(int count)
	{
	}
}
