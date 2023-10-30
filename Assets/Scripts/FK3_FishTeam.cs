using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FK3_FishTeam : BaseBehavior<FullSerializerSerializer>
{
	public int id;

	public List<Transform> birthpoints = new List<Transform>();

	public Color gizmoColor = Color.cyan;

	private string tempName = string.Empty;

	private bool setId;

	protected override void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
	}

	private void OnDrawGizmos()
	{
		if (birthpoints.Count > 0)
		{
			if (base.gameObject.name.Contains(")") && !setId)
			{
				try
				{
					tempName = base.gameObject.name;
					tempName = tempName.Replace("FishTeam", string.Empty);
					tempName = tempName.Trim();
					tempName = tempName.Replace("(", string.Empty);
					tempName = tempName.Replace(")", string.Empty);
					id = int.Parse(tempName);
					setId = true;
				}
				catch (Exception message)
				{
					setId = true;
					UnityEngine.Debug.LogError(message);
				}
			}
			Gizmos.color = gizmoColor;
			Gizmos.DrawWireSphere(base.transform.position, 0.3f);
			for (int i = 0; i < birthpoints.Count; i++)
			{
				Gizmos.DrawCube(birthpoints[i].position, Vector3.one * 0.3f);
			}
		}
	}

	public Vector3 GetPointByIndex(int index)
	{
		Vector3 one = Vector3.one;
		return birthpoints[(index > 0) ? (index % birthpoints.Count) : 0].localPosition * 0.02f;
	}

	public int Count()
	{
		if (birthpoints == null)
		{
			return -1;
		}
		return birthpoints.Count;
	}

	[InspectorButton]
	[InspectorName("侦测出生点")]
	private void CheckChilds()
	{
		UnityEngine.Debug.Log("侦测出生点");
		birthpoints.Clear();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform item = (Transform)current;
				birthpoints.Add(item);
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
}
