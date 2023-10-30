using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHG_RollManger : MonoBehaviour
{
	private List<PHG_Line> linelist;

	private void Awake()
	{
		linelist = new List<PHG_Line>(5);
		int num = 0;
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				PHG_Line component = transform.GetComponent<PHG_Line>();
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
			linelist[i].RollIcon(i, i > Cinstance<PHG_Gcore>.Instance.SpeedId);
		}
	}

	public void StopAllLine()
	{
		UnityEngine.Debug.LogError("StopAllLine");
		for (int i = 0; i < linelist.Count; i++)
		{
			linelist[i].Stopicon();
		}
	}
}
