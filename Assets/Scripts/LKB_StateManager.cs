using System.Collections.Generic;
using UnityEngine;

public class LKB_StateManager : LKB_MB_Singleton<LKB_StateManager>
{
	private Dictionary<string, bool> _dic;

	public new static LKB_StateManager GetInstance()
	{
		if (LKB_MB_Singleton<LKB_StateManager>._instance == null)
		{
			CreateInstance();
			UnityEngine.Debug.Log("_instance is null");
		}
		return LKB_MB_Singleton<LKB_StateManager>._instance;
	}

	public static void CreateInstance()
	{
		GameObject gameObject = Object.FindObjectOfType<LKB_GameManager>().gameObject;
		LKB_MB_Singleton<LKB_StateManager>._instance = gameObject.AddComponent<LKB_StateManager>();
		LKB_MB_Singleton<LKB_StateManager>._instance.Init();
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
