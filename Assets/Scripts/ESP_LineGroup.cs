using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESP_LineGroup : MonoBehaviour
{
	private List<Image> LineList;

	private Sequence seq_line;

	private Color showcolor = new Color(1f, 1f, 1f, 1f);

	private Color hidecolor = new Color(1f, 1f, 1f, 0f);

	private List<int> Lineindexlist;

	private List<int> Linescorelist;

	private int showidx;

	private int wincont;

	private int myIndex;

	private void Awake()
	{
		LineList = new List<Image>();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				Image component = transform.GetComponent<Image>();
				LineList.Add(component);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		seq_line = DOTween.Sequence();
		seq_line.Pause();
		seq_line.SetAutoKill(autoKillOnCompletion: false);
		seq_line.AppendCallback(Showlineani);
		seq_line.AppendInterval(2f);
		seq_line.SetLoops(-1);
	}

	public void Showline(List<int> lineindexlist)
	{
		Lineindexlist = lineindexlist;
		showidx = 0;
		myIndex = 0;
		seq_line.Restart();
	}

	private void Showlineani()
	{
		CloseAllline();
		ShowLine(Lineindexlist[showidx]);
		showidx = (showidx + 1) % Lineindexlist.Count;
	}

	private void ShowLine(int index)
	{
		try
		{
			if (myIndex >= Cinstance<ESP_Gcore>.Instance.WinlineList.Count)
			{
				myIndex = 0;
			}
			ESP_MB_Singleton<ESP_MajorGameController>.GetInstance().tipBGText.text = $"赢线: {Cinstance<ESP_Gcore>.Instance.WinlineList[myIndex] + 1}     赢分: {Cinstance<ESP_Gcore>.Instance.WinScoreList[myIndex]}";
			myIndex++;
		}
		catch (Exception)
		{
		}
		LineList[index].color = showcolor;
	}

	public void ResetLine()
	{
		seq_line.Pause();
		CloseAllline();
	}

	private void CloseAllline()
	{
		for (int i = 0; i < LineList.Count; i++)
		{
			LineList[i].color = hidecolor;
		}
	}
}
