using M__M.GameHall.Common;
using System.Collections.Generic;
using UnityEngine;

public class FK3_GetDaTingRoot : FK3_MB_Singleton<FK3_GetDaTingRoot>
{
	public List<GameObject> daTingRoots;

	private void Awake()
	{
		if (FK3_MB_Singleton<FK3_GetDaTingRoot>._instance == null)
		{
			FK3_MB_Singleton<FK3_GetDaTingRoot>.SetInstance(this);
		}
		Object.DontDestroyOnLoad(base.gameObject);
		FK3_GVars.dontDestroyOnLoadList.Add(base.gameObject);
	}

	public void KeepOnLoad()
	{
		foreach (GameObject daTingRoot in daTingRoots)
		{
			if (daTingRoot != null)
			{
				Object.DontDestroyOnLoad(daTingRoot.gameObject);
				FK3_GVars.dontDestroyOnLoadList.Add(daTingRoot.gameObject);
			}
		}
	}

	public void DisableScene()
	{
		foreach (GameObject daTingRoot in daTingRoots)
		{
			if (daTingRoot != null)
			{
				daTingRoot.SetActive(value: true);
			}
		}
	}

	public void EnableScene()
	{
		foreach (GameObject daTingRoot in daTingRoots)
		{
			if (daTingRoot != null)
			{
				daTingRoot.SetActive(value: false);
			}
		}
	}
}
