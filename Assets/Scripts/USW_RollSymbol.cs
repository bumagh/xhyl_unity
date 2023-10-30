using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class USW_RollSymbol
{
	private List<USW_Rsymbol> rsymbollist = new List<USW_Rsymbol>();

	private Transform Tran;

	private Text Scoretext;

	private int Reslitid;

	private int index;

	public USW_RollSymbol(Transform trans, int idx, USW_Symdata data, Action stopaction, Text scoretext = null)
	{
		int num = 0;
		Tran = trans;
		index = idx;
		IEnumerator enumerator = trans.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform tran = (Transform)enumerator.Current;
				rsymbollist.Add(new USW_Rsymbol(tran, num++, this, data, stopaction));
				if (num >= 2)
				{
					break;
				}
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
		Scoretext = scoretext;
	}

	public void Initsym()
	{
		int num = 0;
		foreach (USW_Rsymbol item in rsymbollist)
		{
			item.Initsym(Tran.Find<Image>("Frame" + num++));
		}
	}

	public void RollSym(int index, int result)
	{
		Scoretext.text = string.Empty;
		Reslitid = result;
		for (int i = 0; i < rsymbollist.Count; i++)
		{
			rsymbollist[i].SetSymroll(index * 4 + 24, result);
		}
	}

	public void Playsymani()
	{
		rsymbollist[1].PLayAni();
	}

	public void Stopsymani()
	{
		rsymbollist[1].StopAni();
	}

	public void Showimg(bool isshow)
	{
		foreach (USW_Rsymbol item in rsymbollist)
		{
			item.Show(isshow);
		}
	}

	public void ShowScore()
	{
		if (Scoretext != null && Reslitid == 9)
		{
			long num = 0L;
			Scoretext.text = num * 0 + string.Empty;
		}
	}
}
