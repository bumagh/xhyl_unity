using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHT_RollManger : MonoBehaviour
{
	private List<SHT_Line> linelist;

	private void Awake()
	{
		linelist = new List<SHT_Line>(5);
		int num = 0;
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				SHT_Line component = transform.GetComponent<SHT_Line>();
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
			linelist[i].RollIcon(i, i > Cinstance<SHT_Gcore>.Instance.SpeedId);
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
