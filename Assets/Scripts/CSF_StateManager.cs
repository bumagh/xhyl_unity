using System.Collections.Generic;
using UnityEngine;

public class CSF_StateManager : CSF_MB_Singleton<CSF_StateManager>
{
	private Dictionary<string, bool> _dic;

	public new static CSF_StateManager GetInstance()
	{
		if (CSF_MB_Singleton<CSF_StateManager>._instance == null)
		{
			CreateInstance();
			UnityEngine.Debug.Log("_instance is null");
		}
		return CSF_MB_Singleton<CSF_StateManager>._instance;
	}

	public static void CreateInstance()
	{
		GameObject gameObject = Object.FindObjectOfType<CSF_GameManager>().gameObject;
		CSF_MB_Singleton<CSF_StateManager>._instance = gameObject.AddComponent<CSF_StateManager>();
		CSF_MB_Singleton<CSF_StateManager>._instance.Init();
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
