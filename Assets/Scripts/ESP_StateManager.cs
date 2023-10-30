using System.Collections.Generic;
using UnityEngine;

public class ESP_StateManager : ESP_MB_Singleton<ESP_StateManager>
{
	private Dictionary<string, bool> _dic;

	public new static ESP_StateManager GetInstance()
	{
		if (ESP_MB_Singleton<ESP_StateManager>._instance == null)
		{
			CreateInstance();
			UnityEngine.Debug.Log("_instance is null");
		}
		return ESP_MB_Singleton<ESP_StateManager>._instance;
	}

	public static void CreateInstance()
	{
		GameObject gameObject = Object.FindObjectOfType<ESP_GameManager>().gameObject;
		ESP_MB_Singleton<ESP_StateManager>._instance = gameObject.AddComponent<ESP_StateManager>();
		ESP_MB_Singleton<ESP_StateManager>._instance.Init();
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
