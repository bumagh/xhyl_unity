using System.Collections.Generic;
using UnityEngine;

public class MSE_StateManager : MSE_MB_Singleton<MSE_StateManager>
{
	private Dictionary<string, bool> _dic;

	public new static MSE_StateManager GetInstance()
	{
		if (MSE_MB_Singleton<MSE_StateManager>._instance == null)
		{
			CreateInstance();
			UnityEngine.Debug.Log("_instance is null");
		}
		return MSE_MB_Singleton<MSE_StateManager>._instance;
	}

	public static void CreateInstance()
	{
		GameObject gameObject = Object.FindObjectOfType<MSE_GameManager>().gameObject;
		MSE_MB_Singleton<MSE_StateManager>._instance = gameObject.AddComponent<MSE_StateManager>();
		MSE_MB_Singleton<MSE_StateManager>._instance.Init();
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
