using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSE_RollManger : MonoBehaviour
{
	private List<MSE_Line> linelist;

	private void Awake()
	{
		linelist = new List<MSE_Line>(5);
		int num = 0;
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				MSE_Line component = transform.GetComponent<MSE_Line>();
				component.Init(num++);
				linelist.Add(component);
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

	public void RollAllline()
	{
		for (int i = 0; i < linelist.Count; i++)
		{
			linelist[i].RollIcon(i, i > Cinstance<MSE_Gcore>.Instance.SpeedId);
		}
	}

	public void StopAllLine()
	{
		for (int i = 0; i < linelist.Count; i++)
		{
			linelist[i].Stopicon();
		}
	}

	public void Addspeedline()
	{
		int num = Cinstance<MSE_Gcore>.Instance.SpeedId + 1;
		for (int i = num; i < 5; i++)
		{
			linelist[i].AddSpeedsym();
		}
	}
}
