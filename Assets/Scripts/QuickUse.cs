using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class QuickUse
{
	public static T Find<T>(this Transform tran, string name)
	{
		return tran.Find(name).GetComponent<T>();
	}

	public static void Addaction(this Button button, Action clickac, Action audioaction = null)
	{
		button.onClick.AddListener(delegate
		{
			clickac();
			if (audioaction != null)
			{
				audioaction();
			}
		});
	}

	public static void RemoveAction(this Button button)
	{
		button.onClick.RemoveAllListeners();
	}

	public static List<int> RandomGetlist(this List<int> Numlist, int len)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < len; i++)
		{
			int count = Numlist.Count;
			if (count == 0)
			{
				break;
			}
			int index = UnityEngine.Random.Range(0, count);
			int item = Numlist[index];
			Numlist.Remove(item);
			list.Add(item);
		}
		return list;
	}

	public static List<long> RandomGetlist(this List<long> Numlist, int len)
	{
		List<long> list = new List<long>();
		for (int i = 0; i < len; i++)
		{
			int count = Numlist.Count;
			if (count == 0)
			{
				break;
			}
			int index = UnityEngine.Random.Range(0, count);
			long item = Numlist[index];
			Numlist.Remove(item);
			list.Add(item);
		}
		return list;
	}

	public static List<ulong> RandomGetlist(this List<ulong> Numlist, int len)
	{
		List<ulong> list = new List<ulong>();
		for (int i = 0; i < len; i++)
		{
			int count = Numlist.Count;
			if (count == 0)
			{
				break;
			}
			int index = UnityEngine.Random.Range(0, count);
			ulong item = Numlist[index];
			Numlist.Remove(item);
			list.Add(item);
		}
		return list;
	}

	public static long TransNum(this long num)
	{
		return num;
	}

	public static ulong TransNum(this ulong num)
	{
		return num;
	}

	public static int TransNum(this int num)
	{
		return num;
	}

	public static uint TransNum(this uint num)
	{
		return num;
	}

	public static void SetCoinformat(this Text text, long num)
	{
		string text2 = string.Empty;
		bool flag = false;
		for (int num2 = 4; num2 >= 0; num2--)
		{
			long num3 = num / (long)Mathf.Pow(10f, num2 * 3);
			if (num3 > 0)
			{
				flag = true;
			}
			if (flag)
			{
				text2 += string.Format("{0:000}" + ((num2 != 0) ? "," : string.Empty), num3);
			}
			num %= (long)Mathf.Pow(10f, num2 * 3);
		}
		text.text = text2;
	}

	public static void SetCoinformat(this Text text, float nums)
	{
		string text2 = string.Empty;
		bool flag = false;
		long num = (long)nums;
		for (int num2 = 4; num2 >= 0; num2--)
		{
			long num3 = num / (long)Mathf.Pow(10f, num2 * 3);
			if (num3 > 0)
			{
				flag = true;
			}
			if (flag)
			{
				text2 += string.Format("{0:000}" + ((num2 != 0) ? "," : string.Empty), num3);
			}
			num %= (long)Mathf.Pow(10f, num2 * 3);
		}
		text2 = (text.text = text2 + (nums - (float)(long)nums) + string.Empty);
	}

	public static List<long> Randomlist(this List<long> list)
	{
		List<long> list2 = new List<long>();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			list2.Add(list.RandomGetlist(1)[0]);
		}
		return list2;
	}

	public static List<ulong> Randomlist(this List<ulong> list)
	{
		List<ulong> list2 = new List<ulong>();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			list2.Add(list.RandomGetlist(1)[0]);
		}
		return list2;
	}

	public static List<int> Randomlist(this List<int> list)
	{
		List<int> list2 = new List<int>();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			list2.Add(list.RandomGetlist(1)[0]);
		}
		return list2;
	}

	public static long GetListsum(this List<long> numlist)
	{
		long num = 0L;
		foreach (long item in numlist)
		{
			num += item;
		}
		return num;
	}

	public static ulong GetListsum(this List<ulong> numlist)
	{
		ulong num = 0uL;
		foreach (ulong item in numlist)
		{
			num += item;
		}
		return num;
	}

	public static int GetListsum(this List<int> numlist)
	{
		int num = 0;
		foreach (int item in numlist)
		{
			num += item;
		}
		return num;
	}

	public static void Countdowntime(this Text text, int time)
	{
		Sequence sequence = DOTween.Sequence();
		sequence.AppendCallback(delegate
		{
			text.text = --time + string.Empty;
		});
		sequence.AppendInterval(1f);
		sequence.SetLoops(time);
	}

	public static void Animatornum(this Text text, long from, long to, float time, bool Changeformat = false)
	{
		if (to - from == 0)
		{
			text.text = string.Empty + from;
			return;
		}
		bool add = to > from;
		int num = (int)(time / 0.1f);
		long addnum = (to - from) / num;
		Sequence seq = DOTween.Sequence();
		if (!DOTween.IsTweening(text))
		{
			seq.SetId(text);
		}
		else
		{
			DOTween.Pause(text);
			DOTween.Kill(text);
		}
		seq.AppendCallback(delegate
		{
			from += addnum;
			Settext(text, from, Changeformat);
			if (add)
			{
				if (from > to)
				{
					from = to;
					Settext(text, from, Changeformat);
					seq.Pause();
					seq.Kill();
				}
			}
			else if (from < to)
			{
				from = to;
				Settext(text, from, Changeformat);
				seq.Pause();
				seq.Kill();
			}
		});
		seq.AppendInterval(0.1f);
		seq.SetLoops(-1);
	}

	public static void Animatornum(this Text text, float from, float to, float time, bool Changeformat = false)
	{
		if (to - from != 0f)
		{
			bool add = to > from;
			int num = (int)(time / 0.1f);
			float addnum = (to - from) / (float)num;
			Sequence seq = DOTween.Sequence();
			if (!DOTween.IsTweening(text))
			{
				seq.SetId(text);
			}
			else
			{
				DOTween.Pause(text);
				UnityEngine.Debug.Log(text.name + " 的字体动画正在执行==========");
				DOTween.Kill(text);
			}
			seq.AppendCallback(delegate
			{
				from += addnum;
				Settext(text, from, Changeformat);
				if (add)
				{
					if (from > to)
					{
						from = to;
						Settext(text, from, Changeformat);
						seq.Pause();
					}
				}
				else if (from < to)
				{
					from = to;
					Settext(text, from, Changeformat);
					seq.Pause();
				}
			});
			seq.AppendInterval(0.1f);
			seq.SetLoops(-1);
		}
	}

	private static void Settext(Text text, long num, bool change)
	{
		if (change)
		{
			text.SetCoinformat(num);
		}
		else
		{
			text.text = num + string.Empty;
		}
	}

	private static void Settext(Text text, float num, bool change)
	{
		if (change)
		{
			text.SetCoinformat(num);
		}
		else
		{
			text.text = num + string.Empty;
		}
	}
}
