using UnityEngine;

public class LRS_BallManager : MonoBehaviour
{
	private GameObject[] balls;

	private void Awake()
	{
		balls = new GameObject[base.transform.childCount];
		for (int i = 0; i < base.transform.childCount; i++)
		{
			balls[i] = base.transform.GetChild(i).gameObject;
		}
		Init();
	}

	private void Init()
	{
		for (int i = 0; i < 9; i++)
		{
			balls[i].SetActive(value: false);
		}
	}

	public void ShowBrightBall(int line)
	{
	}

	public void HideBrightBall(int line)
	{
		if (line < balls.Length)
		{
			balls[line].gameObject.SetActive(value: false);
		}
	}

	public void HideAllBall()
	{
		for (int i = 0; i < 9; i++)
		{
			balls[i].SetActive(value: false);
		}
	}

	public void ShowBalls(int total = 9)
	{
	}
}
