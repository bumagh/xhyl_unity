using System.Collections.Generic;
using UnityEngine;

public class SHT_StateManager : SHT_MB_Singleton<SHT_StateManager>
{
	private Dictionary<string, bool> _dic;

	public new static SHT_StateManager GetInstance()
	{
		if (SHT_MB_Singleton<SHT_StateManager>._instance == null)
		{
			CreateInstance();
			UnityEngine.Debug.Log("_instance is null");
		}
		return SHT_MB_Singleton<SHT_StateManager>._instance;
	}

	public static void CreateInstance()
	{
		GameObject gameObject = Object.FindObjectOfType<SHT_GameManager>().gameObject;
		SHT_MB_Singleton<SHT_StateManager>._instance = gameObject.AddComponent<SHT_StateManager>();
		SHT_MB_Singleton<SHT_StateManager>._instance.Init();
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
