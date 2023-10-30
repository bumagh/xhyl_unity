using UnityEngine;
using UnityEngine.UI;

public class STQM_DotsAnimCtrl : MonoBehaviour
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
		for (int i = 0; i < 10; i++)
		{
			imgDots[i].gameObject.SetActive(value: false);
		}
		imgCurDot.gameObject.SetActive(value: false);
		if (count > 10)
		{
			count = 10;
		}
		for (int j = 0; j < count; j++)
		{
			imgDots[j].transform.localPosition = Vector3.zero;
			int num = -(count - 1) / 2 * 30 + j * 30;
			imgDots[j].gameObject.SetActive(value: true);
			imgDots[j].transform.localPosition = Vector3.right * num;
			if (j == STQM_GameInfo.getInstance().TableIndex % 10)
			{
				imgCurDot.gameObject.SetActive(value: true);
				imgCurDot.localPosition = imgDots[j].transform.localPosition;
			}
		}
	}
}
