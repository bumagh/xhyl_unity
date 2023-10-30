using FullInspector;
using SWS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FK3_FishPathMgr : BaseBehavior<FullSerializerSerializer>
{
	private static FK3_FishPathMgr s_instance;

	private List<PathManager> pathList = new List<PathManager>();

	[InspectorCollapsedFoldout]
	[SerializeField]
	private Dictionary<string, PathManager> pathDic = new Dictionary<string, PathManager>();

	private int _traverseCount;

	public static FK3_FishPathMgr Get()
	{
		return s_instance;
	}

	protected override void Awake()
	{
		base.Awake();
		s_instance = this;
	}

	private void Start()
	{
	}

	public PathManager GetPathById(int id)
	{
		PathManager value = null;
		pathDic.TryGetValue(id.ToString(), out value);
		if (value == null)
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Red($"fishPath[id:{id}] not found"));
		}
		return value;
	}

	[InspectorName("侦测所有路径")]
	[InspectorButton]
	private void CheckPaths()
	{
		pathDic.Clear();
		pathList.Clear();
		_traverseCount = 0;
		TraversePaths(base.transform);
		UnityEngine.Debug.Log($"detect finish>total path count:{pathList.Count}");
	}

	private void TraversePaths(Transform node)
	{
		_traverseCount++;
		if (_traverseCount > 100)
		{
			throw new Exception("TraversePaths too large");
		}
		UnityEngine.Debug.Log($"TraversePaths node[{node.name}]");
		IEnumerator enumerator = node.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform transform = (Transform)current;
				if (!transform.Equals(node))
				{
					PathManager component = transform.GetComponent<PathManager>();
					if (component == null)
					{
						if (transform.childCount > 0)
						{
							TraversePaths(transform);
						}
					}
					else
					{
						pathList.Add(component);
						string name = transform.name;
						if (pathDic.ContainsKey(name))
						{
							UnityEngine.Debug.Log(FK3_LogHelper.Red($"fishPath[id:{name}] repeat"));
						}
						else
						{
							pathDic.Add(name, component);
						}
					}
				}
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
