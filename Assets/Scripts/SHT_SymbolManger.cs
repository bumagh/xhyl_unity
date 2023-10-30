using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SHT_SymbolManger : MonoBehaviour
{
	private List<SHT_SymbolC> Symlist;

	private Sequence Seq_roll;

	private Sequence Seq_speed;

	private Sequence Seq_ani;

	private Sequence Seq_change;

	private List<int> Stopindexlist;

	private int startidx;

	private List<List<int>> Winsymlist;

	private List<int> Reslut;

	private int changenum;

	private int startnum;

	private bool Playwildaudio;

	private void Awake()
	{
		ChangeIma();
		Symlist = new List<SHT_SymbolC>();
		Stopindexlist = new List<int>(15);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 15; i++)
		{
			int item = num++ % 3 * 5 + num2++ / 3;
			Stopindexlist.Add(item);
		}
		Seq_roll = DOTween.Sequence();
		Seq_roll.Pause();
		Seq_roll.SetAutoKill(autoKillOnCompletion: false);
		Seq_roll.AppendInterval(1.5f);
		for (int j = 0; j < 5; j++)
		{
			Seq_roll.AppendCallback(delegate
			{
				for (int k = 0; k < 3; k++)
				{
					int index = Stopindexlist[startidx++];
					Symlist[index].StopRoll();
				}
			});
			Seq_roll.AppendInterval(0.25f);
		}
		Seq_ani = DOTween.Sequence();
		Seq_ani.Pause();
		Seq_ani.SetAutoKill(autoKillOnCompletion: false);
		Seq_ani.AppendCallback(Playwinsym);
		Seq_ani.AppendInterval(2f);
		Seq_ani.SetLoops(-1);
		NotifyAction.AddAction(Gamestate.Init, Init);
	}

	private void ChangeIma()
	{
		try
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				if (base.transform.GetChild(i).GetComponent<Image>() != null)
				{
					base.transform.GetChild(i).GetComponent<Image>().DOFade(1f, 0.5f);
				}
			}
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
		}
	}

	private void OnEnable()
	{
		if (!Cinstance<SHT_Gcore>.Instance.IsDic)
		{
			StartCoroutine(WiatGetCoin());
		}
		if (Symlist.Count <= 10 || Cinstance<SHT_Gcore>.Instance.IsDic)
		{
			return;
		}
		UnityEngine.Debug.LogError("非第一次进入,初始化图标");
		int num = 0;
		for (int i = 0; i < Symlist.Count; i++)
		{
			int symid = 0;
			if (num < Cinstance<SHT_Gcore>.Instance.Normallist.Count)
			{
				symid = Cinstance<SHT_Gcore>.Instance.Normallist[num];
			}
			Symlist[i].Init(num++, symid);
		}
	}

	private IEnumerator WiatGetCoin()
	{
		yield return new WaitForSeconds(0.25f);
		UnityEngine.Debug.LogError("发送取分");
		SHT_MB_Singleton<SHT_ScoreBank>.GetInstance().OnBtnWithdraw_Click(isAuto: true);
	}

	public void Init()
	{
		int num = 0;
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				SHT_SymbolC component = transform.GetComponent<SHT_SymbolC>();
				int num2 = 0;
				if (num >= Cinstance<SHT_Gcore>.Instance.Normallist.Count)
				{
					UnityEngine.Debug.LogError("索引超出长度!");
					break;
				}
				num2 = Cinstance<SHT_Gcore>.Instance.Normallist[num];
				component.Init(num++, num2);
				Symlist.Add(component);
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
		SHT_MB_Singleton<SHT_ScoreBank>.GetInstance().Init();
		UnityEngine.Debug.LogError("===========进入sym的初始化===========" + Symlist.Count);
	}

	public void Playani(List<List<int>> winsymlist)
	{
		Winsymlist = winsymlist;
		startnum = 0;
		Seq_ani.Restart();
	}

	public void PlayChange()
	{
		Seq_change.Restart();
		Seq_ani.Pause();
	}

	private void PlayChangeSym()
	{
		if (changenum >= 0)
		{
			Seq_change.Pause();
		}
	}

	public void Changesym(List<int> poslist, List<int> numlist)
	{
		for (int i = 0; i < numlist.Count; i++)
		{
			Symlist[poslist[i]].Startrollsym(i, numlist[i]);
		}
	}

	public void PlayScatter(List<int> scatter)
	{
		for (int i = 0; i < scatter.Count; i++)
		{
			int index = scatter[i];
			Symlist[index].Playani();
		}
	}

	private void ShowAllsymbol(bool isshow)
	{
		for (int i = 0; i < Symlist.Count; i++)
		{
			Symlist[i].ShowSymbol(isshow);
		}
	}

	private void Playallwinsymani()
	{
		for (int i = 0; i < Winsymlist.Count; i++)
		{
			for (int j = 0; j < Winsymlist[i].Count; j++)
			{
				int index = Winsymlist[i][j];
				Symlist[index].Playani();
			}
		}
	}

	public Vector3 GetSymPos(int index)
	{
		return Symlist[index].transform.position;
	}

	private void Playwinsym()
	{
		try
		{
			StopAllsymani();
			Vector3 vector = default(Vector3);
			List<int> list = Winsymlist[startnum];
			int count = list.Count;
			int num = 19;
			for (int i = 0; i < count; i++)
			{
				int num2 = list[i];
				Symlist[num2].Playani(startnum);
				if (num > num2)
				{
					num = num2;
				}
			}
			vector = Symlist[num].transform.position;
			startnum = (startnum + 1) % Winsymlist.Count;
			SHT_SoundManager.Instance.PlayMajorGameAwardAudio(startnum);
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
			throw;
		}
	}

	public void Stopani()
	{
		Seq_ani.Pause();
		Seq_change.Pause();
		StopAllsymani(Reset: true);
	}

	public void StopAllsymani(bool Reset = false)
	{
		for (int i = 0; i < Symlist.Count; i++)
		{
			Symlist[i].StopAni(Reset);
		}
	}

	private void Showsym(List<int> showlist)
	{
		for (int i = 0; i < showlist.Count; i++)
		{
			Symlist[showlist[i]].ShowSymbol(islight: true);
		}
	}

	public void RefushSymimg()
	{
		for (int i = 0; i < Symlist.Count; i++)
		{
			Symlist[i].ChangeSym();
		}
	}

	public void RollAllsym()
	{
		Playwildaudio = false;
		Reslut = Cinstance<SHT_Gcore>.Instance.Result;
		UnityEngine.Debug.LogError("Symlist: " + Symlist.Count);
		for (int i = 0; i < Symlist.Count; i++)
		{
			Symlist[i].PlayRollani(Reslut[i]);
		}
		changenum = 0;
		startidx = 0;
	}

	private void StartRoll()
	{
		int num = 0;
		if (num > 4)
		{
			Seq_roll.Restart();
			return;
		}
		Seq_speed = DOTween.Sequence();
		Seq_speed.AppendInterval(1.5f);
		for (int i = 0; i < 5; i++)
		{
			Seq_speed.AppendCallback(delegate
			{
				for (int j = 0; j < 3; j++)
				{
					int index = Stopindexlist[startidx++];
					Symlist[index].StopRoll();
				}
			});
			if (i < num - 1)
			{
				Seq_speed.AppendInterval(0.25f);
			}
			else
			{
				Seq_speed.AppendInterval((i != num - 1) ? 0.35f : 1.75f);
			}
		}
	}

	public void StopRoll()
	{
		Seq_roll.Pause();
		Seq_speed.Pause();
		for (int i = 0; i < 15; i++)
		{
			Symlist[i].StopRoll();
		}
	}
}
