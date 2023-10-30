using M__M.GameHall.Common;
using System.Collections.Generic;
using UnityEngine;

public class GetDaTingRoot : HW2_MB_Singleton<GetDaTingRoot>
{
	public List<GameObject> daTingRoots;

	private void Awake()
	{
		if (HW2_MB_Singleton<GetDaTingRoot>._instance == null)
		{
			HW2_MB_Singleton<GetDaTingRoot>.SetInstance(this);
		}
		Object.DontDestroyOnLoad(base.gameObject);
		HW2_GVars.dontDestroyOnLoadList.Add(base.gameObject);
	}

	public void KeepOnLoad()
	{
		foreach (GameObject daTingRoot in daTingRoots)
		{
			if (daTingRoot != null)
			{
				Object.DontDestroyOnLoad(daTingRoot.gameObject);
				HW2_GVars.dontDestroyOnLoadList.Add(daTingRoot.gameObject);
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
