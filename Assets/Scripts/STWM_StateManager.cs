using System.Collections.Generic;
using UnityEngine;

public class STWM_StateManager : STWM_MB_Singleton<STWM_StateManager>
{
	private Dictionary<string, bool> _dic;

	public new static STWM_StateManager GetInstance()
	{
		if (STWM_MB_Singleton<STWM_StateManager>._instance == null)
		{
			CreateInstance();
		}
		return STWM_MB_Singleton<STWM_StateManager>._instance;
	}

	public static void CreateInstance()
	{
		GameObject gameObject = Object.FindObjectOfType<STWM_GameManager>().gameObject;
		STWM_MB_Singleton<STWM_StateManager>._instance = gameObject.AddComponent<STWM_StateManager>();
		STWM_MB_Singleton<STWM_StateManager>._instance.Init();
	}

	public void Init()
	{
		_dic = new Dictionary<string, bool>();
	}

	public bool IsWorkCompleted(string workName)
	{
		return !_dic.ContainsKey(workName) || _dic[workName];
	}

	public void RegisterWork(string workName, bool isCompleted = false)
	{
		_dic[workName] = isCompleted;
	}

	public void CompleteWork(string workName)
	{
		_dic.Remove(workName);
	}

	public void ClearWork(string workName)
	{
		_dic.Remove(workName);
	}

	public void ClearAllWorks()
	{
		_dic.Clear();
	}
}
