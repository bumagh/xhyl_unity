using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESP_Line : MonoBehaviour
{
	private List<ESP_SymbolC> iconlist;

	private List<int> result;

	private int RandomIco;

	private List<int> Rollsymnum;

	private int Rollsymnumidx = -1;

	private Sequence seq_line;

	private bool stopline = true;

	private float posy;

	private float posyOver;

	private int LineIdx = -1;

	private List<int> numlist = new List<int>();

	private void Awake()
	{
		result = new List<int>();
		iconlist = new List<ESP_SymbolC>();
		Rollsymnum = new List<int>();
		RandomIco = Cinstance<ESP_Gcore>.Instance.Symcont;
		numlist = new List<int>();
		for (int i = 0; i < RandomIco; i++)
		{
			numlist.Add(i);
		}
		Vector3 localPosition = base.transform.localPosition;
		posy = localPosition.y;
		posyOver = posy - 40f;
		seq_line = DOTween.Sequence();
		seq_line.SetAutoKill(autoKillOnCompletion: false);
		seq_line.Pause();
		seq_line.AppendCallback(delegate
		{
			base.transform.DOLocalMoveY(posyOver, 0.2f);
		});
		seq_line.AppendInterval(0.25f);
		seq_line.AppendCallback(delegate
		{
			base.transform.DOLocalMoveY(posy, 0.2f);
		});
		NotifyAction.AddAction(Gamestate.Init, Initline);
	}

	public void Init(int idx)
	{
		LineIdx = idx;
		if (idx == 0)
		{
			numlist.Remove(9);
		}
	}

	private void Initline()
	{
		int num = 0;
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				ESP_SymbolC component = transform.GetComponent<ESP_SymbolC>();
				int index = LineIdx + num * 5;
				component.Init(symid: (num > 2) ? UnityEngine.Random.Range(0, 6) : Cinstance<ESP_Gcore>.Instance.Normallist[index], idx: num++, line: this);
				iconlist.Add(component);
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
	}

	private void Start()
	{
	}

	public int Getnum(int index = -1)
	{
		int num = -1;
		if (index == -1)
		{
			if (Rollsymnumidx != -1)
			{
				num = Rollsymnum[Rollsymnumidx];
			}
			Rollsymnumidx++;
			if (Rollsymnumidx == Rollsymnum.Count)
			{
				Rollsymnumidx = -1;
			}
		}
		else
		{
			num = Rollsymnum[index];
		}
		return num;
	}

	public void Recticon()
	{
		Playani();
	}

	public void Playani()
	{
		if (!stopline)
		{
			try
			{
				seq_line.Restart();
				stopline = true;
				if (LineIdx == 4)
				{
					Cinstance<ESP_Gcore>.Instance.IsRoll = false;
					Sequence t = DOTween.Sequence();
					t.SetDelay(1f).AppendCallback(Cinstance<ESP_Gcore>.Instance.AllStop);
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
				throw;
			}
		}
	}

	public void RollIcon(int idx, bool isspeed)
	{
		stopline = false;
		for (int i = 0; i < iconlist.Count; i++)
		{
			iconlist[i].Roll(idx, numlist, isspeed);
		}
	}

	public void Stopicon()
	{
		for (int i = 0; i < iconlist.Count; i++)
		{
			iconlist[i].StopRoll();
		}
		Recticon();
	}
}
