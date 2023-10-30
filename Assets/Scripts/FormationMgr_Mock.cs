using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationMgr_Mock : MonoBehaviour
{
	private static FormationMgr_Mock _instance;

	public List<FormationData> formationList = new List<FormationData>();

	public Dictionary<int, FormationData> formationDic;

	public static FormationMgr_Mock Get()
	{
		return _instance;
	}

	private void Awake()
	{
		_instance = this;
		formationDic = new Dictionary<int, FormationData>();
		Init();
	}

	private void Init()
	{
		foreach (FormationData formation in formationList)
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

	private FormationData CreateFormation(Transform root)
	{
		return new FormationData();
	}
}
