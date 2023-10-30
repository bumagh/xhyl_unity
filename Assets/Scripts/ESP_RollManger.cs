using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESP_RollManger : MonoBehaviour
{
	private List<ESP_Line> linelist;

	private void Awake()
	{
		linelist = new List<ESP_Line>(5);
		int num = 0;
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				ESP_Line component = transform.GetComponent<ESP_Line>();
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
			linelist[i].RollIcon(i, i > Cinstance<ESP_Gcore>.Instance.SpeedId);
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
