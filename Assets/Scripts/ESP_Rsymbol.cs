using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESP_Rsymbol
{
	private Transform Tran;

	private Image img;

	private Image frame;

	private Sequence seq_move;

	private Sequence seq_stop;

	private Sequence seq_ani;

	private Sequence seq_frame;

	private Sequence seq_nomal;

	private Vector3 startpos;

	private ESP_Symdata Data;

	private float maxs;

	private float s;

	private float posy;

	private float stopy;

	private bool show;

	private int Index;

	private List<int> numlist = new List<int>();

	private int numidx;

	private ESP_RollSymbol Rs;

	private Action StopAction;

	private int resultnum;

	private bool isbig;

	public ESP_Rsymbol(Transform tran, int idx, ESP_RollSymbol RS, ESP_Symdata data, Action stopaction)
	{
		Rs = RS;
		Tran = tran;
		Data = data;
		Index = idx;
		StopAction = stopaction;
		img = Tran.GetComponent<Image>();
		startpos = Tran.localPosition;
		posy = startpos.y;
		seq_move = DOTween.Sequence();
		seq_move.Pause();
		seq_move.SetAutoKill(autoKillOnCompletion: false);
		seq_move.AppendInterval(0.025f);
		seq_move.AppendCallback(Move);
		seq_move.SetLoops(-1);
		seq_stop = DOTween.Sequence();
		seq_stop.Pause();
		seq_stop.SetAutoKill(autoKillOnCompletion: false);
		seq_stop.Append(Tran.DOLocalMoveY(startpos.y - 20f, 0.2f));
		seq_stop.Append(Tran.DOLocalMoveY(startpos.y, 0.2f));
		seq_stop.AppendCallback(Stop);
		seq_nomal = DOTween.Sequence();
		seq_nomal.Pause();
		seq_nomal.SetAutoKill(autoKillOnCompletion: false);
		seq_nomal.Append(Tran.DOScale(1.1f, 1f));
		seq_nomal.Append(Tran.DOScale(1f, 1f));
		seq_nomal.SetLoops(-1);
	}

	public void Initsym(Image frameimg)
	{
		frame = frameimg;
		seq_frame = frame.ESP_PlayAni("icon_guangxiao{0:0}", 1, 15f);
		seq_ani = img.ESP_PlayAni("spirit_big_{0:00}", 0, 15f);
		isbig = true;
	}

	public void Show(bool isshow)
	{
		Tran.gameObject.SetActive(isshow);
	}

	public void SetSymroll(int len, int result)
	{
		resultnum = result;
		Tran.gameObject.SetActive(value: true);
		maxs = (float)len * Data.symh;
		numlist.Clear();
		numidx = 0;
		for (int i = 0; i < len / 2; i++)
		{
			numlist.Add(UnityEngine.Random.Range(0, 12));
		}
		stopy = Data.topy - (Data.topy - posy + maxs % (Data.symh * 2f)) % (Data.topy - Data.bottomy + Data.symh);
		if (stopy < Data.topy)
		{
			numlist[numlist.Count - 1] = result;
		}
		seq_move.Restart();
	}

	public void PLayAni()
	{
		if (resultnum == 11)
		{
			seq_ani.Restart();
		}
		else
		{
			seq_nomal.Restart();
		}
		seq_frame.Restart();
		frame.gameObject.SetActive(value: true);
	}

	public void StopAni()
	{
		seq_ani.Pause();
		seq_nomal.Pause();
		seq_frame.Pause();
		frame.gameObject.SetActive(value: false);
	}

	private void Stop()
	{
		if (Index == 1)
		{
			StopAction();
		}
	}

	private void Setpos(float y)
	{
		Tran.localPosition = new Vector3(startpos.x, y, 0f);
	}

	private void Symtoshow()
	{
		int num = numlist[numidx++];
		UnityEngine.Debug.LogError("index: " + num + " isbig: " + isbig);
		if (isbig)
		{
			img.sprite = null;
		}
		else
		{
			img.sprite = null;
		}
	}

	private void Move()
	{
		float num = Time.deltaTime * Data.Speed;
		s += num;
		if (s >= maxs)
		{
			s = 0f;
			seq_move.Pause();
			posy = stopy;
			if (Index == 1)
			{
				Rs.ShowScore();
			}
			Setpos(posy);
			seq_stop.Restart();
			return;
		}
		posy = Data.topy - (Data.topy - posy + num) % (Data.topy - Data.bottomy + Data.symh);
		if (posy <= Data.bottomy)
		{
			show = false;
		}
		Setpos(posy);
		if (!show && posy < Data.topy && posy > Data.bottomy)
		{
			Symtoshow();
			show = true;
		}
	}
}
