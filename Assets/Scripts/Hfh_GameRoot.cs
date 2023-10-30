using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hfh_GameRoot : Hfh_Singleton<Hfh_GameRoot>
{
	private static GameObject _rootObj;

	private static List<Action> _singletonReleaseList = new List<Action>();

	public void Awake()
	{
		_rootObj = base.gameObject;
		if (Hfh_Singleton<Hfh_GameRoot>._instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(_rootObj);
			StartCoroutine(InitSingletons());
			Hfh_Singleton<Hfh_GameRoot>.SetInstance(this);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private IEnumerator InitSingletons()
	{
		yield return null;
	}

	private static void AddSingleton<T>() where T : Hfh_Singleton<T>
	{
		if ((UnityEngine.Object)_rootObj.GetComponent<T>() == (UnityEngine.Object)null)
		{
			T t = _rootObj.AddComponent<T>();
			Hfh_Singleton<T>.SetInstance(t);
			t.InitSingleton();
			_singletonReleaseList.Add(delegate
			{
				t.Release();
			});
		}
	}

	public static T GetSingleton<T>() where T : Hfh_Singleton<T>
	{
		T component = _rootObj.GetComponent<T>();
		if ((UnityEngine.Object)component == (UnityEngine.Object)null)
		{
			AddSingleton<T>();
		}
		return component;
	}

	public void ClearCanvas()
	{
		Transform transform = GameObject.Find("Canvas").transform;
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				Transform transform2 = (Transform)current;
				UnityEngine.Object.Destroy(transform2.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public static void PureAddReleaseAction<T>(T t) where T : Hfh_Singleton<T>
	{
		_singletonReleaseList.Add(delegate
		{
			t.Release();
		});
	}

	public void OnApplicationQuit()
	{
		if (!(Hfh_Singleton<Hfh_GameRoot>._instance != base.gameObject))
		{
			for (int num = _singletonReleaseList.Count - 1; num >= 0; num--)
			{
				_singletonReleaseList[num]();
			}
		}
	}
}
