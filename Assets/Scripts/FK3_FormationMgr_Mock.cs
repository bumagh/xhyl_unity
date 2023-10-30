using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FK3_FormationMgr_Mock : MonoBehaviour
{
	private static FK3_FormationMgr_Mock _instance;

	public List<FK3_FormationData> formationList = new List<FK3_FormationData>();

	public Dictionary<int, FK3_FormationData> formationDic;

	public static FK3_FormationMgr_Mock Get()
	{
		return _instance;
	}

	private void Awake()
	{
		_instance = this;
		formationDic = new Dictionary<int, FK3_FormationData>();
		Init();
	}

	private void Init()
	{
		foreach (FK3_FormationData formation in formationList)
		{
			formationDic.Add(formation.id, formation);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void GatherFormation()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
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

	private FK3_FormationData CreateFormation(Transform root)
	{
		return new FK3_FormationData();
	}
}
