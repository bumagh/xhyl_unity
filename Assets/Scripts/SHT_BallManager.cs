using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHT_BallManager : MonoBehaviour
{
	private List<GameObject> tempBalls;

	private List<GameObject> balls;

	private Transform left;

	private Transform right;

	private Transform middel;

	private Sequence seq_line;

	private List<int> Lineindexlist;

	private int showidx;

	private void Awake()
	{
		left = base.transform.Find("Left");
		right = base.transform.Find("Right");
		middel = base.transform.Find("Middel");
		balls = new List<GameObject>();
		tempBalls = new List<GameObject>();
		Lineindexlist = new List<int>();
		for (int i = 0; i < left.childCount; i++)
		{
			tempBalls.Add(left.GetChild(i).gameObject);
			tempBalls.Add(right.GetChild(i).gameObject);
		}
		for (int j = 0; j < middel.childCount; j++)
		{
			tempBalls.Add(middel.GetChild(j).gameObject);
		}
		StartCoroutine(AddList());
		seq_line = DOTween.Sequence();
		seq_line.Pause();
		seq_line.SetAutoKill(autoKillOnCompletion: false);
		seq_line.AppendCallback(Showlineani);
		seq_line.AppendInterval(2f);
		seq_line.SetLoops(-1);
	}

	private IEnumerator AddList()
	{
		int temp = 0;
		while (tempBalls.Count > 0)
		{
			temp++;
			for (int i = 0; i < tempBalls.Count; i++)
			{
				if (tempBalls[i].name == temp.ToString())
				{
					balls.Add(tempBalls[i]);
					tempBalls.Remove(tempBalls[i]);
				}
			}
			yield return null;
		}
		Init();
	}

	private void Init()
	{
		string text = string.Empty;
		for (int i = 0; i < balls.Count; i++)
		{
			balls[i].SetActive(value: false);
			text = text + " " + balls[i].name;
		}
		UnityEngine.Debug.LogError("初始化balls: " + balls.Count + "  " + text);
	}

	public void Showline(List<int> lineindexlist)
	{
		Lineindexlist = lineindexlist;
		showidx = 0;
		seq_line.Restart();
	}

	private void Showlineani()
	{
		HideAllBall();
		ShowBrightBall(Lineindexlist[showidx]);
		showidx = (showidx + 1) % Lineindexlist.Count;
	}

	public void ResetLine()
	{
		seq_line.Pause();
		HideAllBall();
	}

	public void ShowBrightBall(int line)
	{
		if (line < balls.Count)
		{
			balls[line].gameObject.SetActive(value: true);
		}
	}

	public void HideAllBall()
	{
		for (int i = 0; i < balls.Count; i++)
		{
			balls[i].SetActive(value: false);
		}
	}
}
