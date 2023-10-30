using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MSE_Gcore : Cinstance<MSE_Gcore>
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

	public List<int[][]> WinlineList = new List<int[][]>();

	public List<int> Winlinenumlist = new List<int>();

	public List<int> WinScoreList = new List<int>();

	public List<List<int>> WinSymlist = new List<List<int>>();

	public List<int> ScatterList = new List<int>();

	public bool IsRoll;

	public bool IsDic;

	private int symcont = 16;

	public int lineConut = 243;

	public int SpeedId = 5;

	private int scatterid = 12;

	public List<int> showLineList = new List<int>();

	public int Symcont => symcont;

	public void Init()
	{
		try
		{
			SymSprlist = new List<Sprite>();
			for (int i = 1; i <= symcont; i++)
			{
				string text = $"img{i.ToString()}_1_1";
				Sprite sprite = MSE_MB_Singleton<MSE_GameManager>.GetInstance().Getanisprite(text);
				SymSprlist.Add(sprite);
				if (sprite == null)
				{
					UnityEngine.Debug.LogError("图片: " + text + " 不存在,请检查名字");
				}
			}
			NotifyAction.ResushState(Gamestate.Init);
			UnityEngine.Debug.LogError("SymSprlist: " + SymSprlist.Count);
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
		MSE_MB_Singleton<MSE_MajorGameController>.GetInstance().AllStop();
	}

	public void QuickStop()
	{
		if (IsRoll)
		{
			MSE_MB_Singleton<MSE_MajorGameController>.GetInstance().StopRoll();
			IsRoll = false;
		}
	}

	public void GetWinsym()
	{
		WinSymlist.Clear();
		showLineList = new List<int>();
		UnityEngine.Debug.LogError("中奖线: " + JsonMapper.ToJson(WinlineList));
		UnityEngine.Debug.LogError("中奖线个数: " + JsonMapper.ToJson(Winlinenumlist));
		for (int i = 0; i < WinlineList.Count; i++)
		{
			List<int> item = Getwinlinepos(WinlineList[i], Winlinenumlist[i]);
			showLineList.Add(i);
			WinSymlist.Add(item);
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

	public static List<int> Getwinlinepos(int[][] lines, int conut)
	{
		List<int> list = new List<int>();
		string text = IntToStr(lines);
		int num = 0;
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				int num2 = i + j * 5;
				if (text[num2] != '0')
				{
					list.Add(num2);
					num++;
					if (num >= conut)
					{
						return list;
					}
				}
			}
		}
		return list;
	}

	private static string IntToStr(int[][] lines)
	{
		string text = string.Empty;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				text += ((lines[i][j] > 1) ? 1 : lines[i][j]);
			}
		}
		return text;
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
		MSE_MB_Singleton<MSE_MajorGameController>.GetInstance().StartRollSym();
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
