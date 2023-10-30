using UnityEngine;

public class LKB_BallManager : MonoBehaviour
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
		balls[line].gameObject.SetActive(value: true);
	}

	public void HideBrightBall(int line)
	{
		balls[line].gameObject.SetActive(value: false);
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
		for (int i = 0; i < total; i++)
		{
			balls[i].SetActive(value: true);
		}
	}
}
