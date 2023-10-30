using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishTeamMgr : BaseBehavior<FullSerializerSerializer>
{
	private static FishTeamMgr s_instance;

	private List<FishTeam> teamList = new List<FishTeam>();

	[SerializeField]
	[InspectorCollapsedFoldout]
	private Dictionary<string, FishTeam> teamDic = new Dictionary<string, FishTeam>();

	public static FishTeamMgr Get()
	{
		return s_instance;
	}

	protected override void Awake()
	{
		base.Awake();
		s_instance = this;
	}

	public FishTeam GetTeamById(int id)
	{
		FishTeam value = null;
		teamDic.TryGetValue(id.ToString(), out value);
		if (value == null)
		{
			UnityEngine.Debug.LogError($"鱼队[id:{id}]未找到");
		}
		return value;
	}

	private void Start()
	{
	}

	public void DisableAllTeams()
	{
		for (int i = 0; i < teamDic.Count; i++)
		{
			teamDic.ElementAt(i).Value.gameObject.SetActive(value: false);
		}
	}

	[InspectorButton]
	[InspectorName("侦测所有编队")]
	private void CheckTeams()
	{
		teamDic.Clear();
		teamList.Clear();
		IEnumerator enumerator = base.transform.GetEnumerator();
		UnityEngine.Debug.LogError("transform: " + base.transform.name);
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				UnityEngine.Debug.LogError("==============obj: " + current.ToString());
				Transform transform = (Transform)current;
				FishTeam component = transform.GetComponent<FishTeam>();
				teamList.Add(component);
				string text = component.id.ToString();
				if (teamDic.ContainsKey(text))
				{
					UnityEngine.Debug.Log(HW2_LogHelper.Red($"fishTeam[id:{text}] repeat"));
				}
				else
				{
					teamDic.Add(text, component);
				}
			}
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
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
}
