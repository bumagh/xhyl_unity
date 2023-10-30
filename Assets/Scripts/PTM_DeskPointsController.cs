using UnityEngine;
using UnityEngine.UI;

public class PTM_DeskPointsController : MonoBehaviour
{
	public static PTM_DeskPointsController _Instance;

	private Image[] arrPoints;

	private Color colMark = Color.white;

	private Color colPoint = Color.white;

	private void Awake()
	{
		_Instance = this;
		colPoint.a = 0.5f;
		arrPoints = new Image[10];
		for (int i = 0; i < 10; i++)
		{
			arrPoints[i] = base.transform.GetChild(i).GetComponent<Image>();
		}
	}

	public void ShowDeskPoint2(int deskNum, int deskIndex)
	{
		int num = deskIndex / 10;
		int num2 = deskIndex % 10;
		int num3 = (deskNum - num * 10 > 10) ? 10 : (deskNum - num * 10);
		for (int i = 0; i < num3; i++)
		{
			arrPoints[i].transform.localPosition = Vector3.right * ((float)i - (float)(num3 - 1) / 2f) * 40f;
			arrPoints[i].gameObject.SetActive(value: true);
			arrPoints[i].color = ((i != num2) ? colPoint : colMark);
		}
		for (int j = num3; j < 10; j++)
		{
			if (arrPoints[j].gameObject.activeSelf)
			{
				arrPoints[j].gameObject.SetActive(value: false);
			}
		}
	}
}
