using System.Collections.Generic;
using UnityEngine;

public class CRL_StateManager : CRL_MB_Singleton<CRL_StateManager>
{
	private Dictionary<string, bool> _dic;

	public new static CRL_StateManager GetInstance()
	{
		if (CRL_MB_Singleton<CRL_StateManager>._instance == null)
		{
			CreateInstance();
			UnityEngine.Debug.Log("_instance is null");
		}
		return CRL_MB_Singleton<CRL_StateManager>._instance;
	}

	public static void CreateInstance()
	{
		GameObject gameObject = Object.FindObjectOfType<CRL_GameManager>().gameObject;
		CRL_MB_Singleton<CRL_StateManager>._instance = gameObject.AddComponent<CRL_StateManager>();
		CRL_MB_Singleton<CRL_StateManager>._instance.Init();
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
