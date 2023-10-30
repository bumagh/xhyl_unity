using System.Collections.Generic;
using UnityEngine;

public class ESP_LockManager
{
	private static Dictionary<string, int> _lockDic = new Dictionary<string, int>();

	public static void Lock(string btnKey, int times = 1)
	{
		if (_lockDic.ContainsKey(btnKey))
		{
			Dictionary<string, int> lockDic;
			Dictionary<string, int> dictionary = lockDic = _lockDic;
			int num = lockDic[btnKey];
			dictionary[btnKey] = num + times;
		}
		else
		{
			_lockDic.Add(btnKey, times);
		}
		UnityEngine.Debug.Log($"Lock {ESP_LogHelper.Key(btnKey)} value {ESP_LogHelper.Key(GetValue(btnKey))}");
	}

	public static void UnLock(string btnKey, bool force = false)
	{
		if (_lockDic.ContainsKey(btnKey))
		{
			_lockDic[btnKey] = ((!force && _lockDic[btnKey] >= 1) ? (_lockDic[btnKey] - 1) : 0);
		}
		else
		{
			_lockDic.Add(btnKey, 0);
		}
	}

	public static bool IsLocked(string btnKey)
	{
		if (_lockDic.ContainsKey(btnKey))
		{
			if (_lockDic[btnKey] > 0)
			{
				UnityEngine.Debug.Log($"IsLocked {ESP_LogHelper.Key(btnKey)} value {ESP_LogHelper.Key(GetValue(btnKey))}");
			}
			return _lockDic[btnKey] > 0;
		}
		return false;
	}

	public static int GetValue(string btnKey)
	{
		if (_lockDic.ContainsKey(btnKey))
		{
			return _lockDic[btnKey];
		}
		return -1;
	}
}
