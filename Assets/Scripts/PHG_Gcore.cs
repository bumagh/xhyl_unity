using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PHG_Gcore : Cinstance<PHG_Gcore>
{
	public List<int> Result = new List<int>
	{
		1,
		2,
		3,
		4,
		5,
		6,
		7,
		8,
		9,
		10,
		11,
		12,
		13,
		14,
		15
	};

	public List<int> Normallist = new List<int>();

	public List<Sprite> SymSprlist = new List<Sprite>();

	public List<int> WinlineList = new List<int>();

	public List<int> Winlinenumlist = new List<int>();

	public List<int> WinScoreList = new List<int>();

	public List<List<int>> WinSymlist = new List<List<int>>();

	public List<int> ScatterList = new List<int>();

	public List<List<int>> AllLinePos = new List<List<int>>();

	public bool IsRoll;

	public bool IsDic;

	private int symcont = 12;

	public int lineConut = 20;

	public int SpeedId = 5;

	private int scatterid = 12;

	public int Symcont => symcont;

	public void Init()
	{
		try
		{
			SymSprlist = new List<Sprite>();
			for (int i = 1; i <= symcont; i++)
			{
				string text = $"icon{i.ToString()}";
				Sprite sprite = PHG_MB_Singleton<PHG_GameManager>.GetInstance().Getanisprite(text);
				SymSprlist.Add(sprite);
				if (sprite == null)
				{
					UnityEngine.Debug.LogError("图片: " + text + " 不存在,请检查名字");
				}
			}
			NotifyAction.ResushState(Gamestate.Init);
			UnityEngine.Debug.LogError("SymSprlist: " + SymSprlist.Count);
			AllLinePos.Clear();
			for (int j = 0; j < lineConut; j++)
			{
				List<int> item = PHG_LinePosIdx.Getwinlinepos(j, 5);
				AllLinePos.Add(item);
			}
			UnityEngine.Debug.LogError("表: " + JsonMapper.ToJson(AllLinePos));
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
		}
	}

	public bool SendBet()
	{
		ScatterList = new List<int>();
		GetWinsym();
		DoRol();
		return true;
	}

	public void AllStop()
	{
		IsRoll = false;
		PHG_MB_Singleton<PHG_MajorGameController>.GetInstance().AllStop();
	}

	public void QuickStop()
	{
		if (IsRoll)
		{
			PHG_MB_Singleton<PHG_MajorGameController>.GetInstance().StopRoll();
			IsRoll = false;
		}
	}

	public void GetWinsym()
	{
		WinSymlist.Clear();
		UnityEngine.Debug.LogError("中奖线: " + JsonMapper.ToJson(WinlineList));
		UnityEngine.Debug.LogError("中奖线个数: " + JsonMapper.ToJson(Winlinenumlist));
		for (int i = 0; i < WinlineList.Count; i++)
		{
			List<int> list = Getlinepos(WinlineList[i], Winlinenumlist[i]);
			WinSymlist.Add(list);
			ShowListnumlog("中奖图片 =>  ", list);
		}
		for (int j = 0; j < 5; j++)
		{
			for (int k = 0; k < 3; k++)
			{
				int num = k * 5 + j;
				if (Result[num] == scatterid)
				{
					ScatterList.Add(num);
				}
			}
		}
		if (ScatterList.Count > 1)
		{
			SpeedId = ScatterList[1] % 5;
			UnityEngine.Debug.LogError("SpeedId is " + SpeedId);
		}
		UnityEngine.Debug.LogError("ScatterList: " + JsonMapper.ToJson(ScatterList));
		UnityEngine.Debug.LogError("中奖图标列表: " + JsonMapper.ToJson(WinSymlist));
	}

	private List<int> Getlinepos(int lineidx, int conut)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < conut; i++)
		{
			list.Add(AllLinePos[lineidx][i]);
		}
		return list;
	}

	public void ShowListnumlog(string name, List<int> list)
	{
		string text = name;
		for (int i = 0; i < list.Count; i++)
		{
			text = text + list[i] + " , ";
		}
		UnityEngine.Debug.LogError(text);
	}

	public void DoRol()
	{
		IsRoll = true;
		PHG_MB_Singleton<PHG_MajorGameController>.GetInstance().StartRollSym();
	}

	public void ClearList()
	{
		NotifyAction.Init();
		Result.Clear();
		WinlineList.Clear();
		WinSymlist.Clear();
		Winlinenumlist.Clear();
		ScatterList.Clear();
		Normallist.Clear();
	}
}
