using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STQM_SpawnPoolsDict : IDictionary<string, STQM_SpawnPool>, ICollection<KeyValuePair<string, STQM_SpawnPool>>, IEnumerable<KeyValuePair<string, STQM_SpawnPool>>, IEnumerable
{
	private Dictionary<string, STQM_SpawnPool> _pools = new Dictionary<string, STQM_SpawnPool>();

	bool ICollection<KeyValuePair<string, STQM_SpawnPool>>.IsReadOnly => true;

	public int Count => _pools.Count;

	public STQM_SpawnPool this[string key]
	{
		get
		{
			try
			{
				return _pools[key];
			}
			catch (KeyNotFoundException)
			{
				string text = $"A Pool with the name '{key}' not found. \nPools={ToString()}";
				UnityEngine.Debug.Log(key);
				STQM_SpawnPool component = GameObject.Find(key).GetComponent<STQM_SpawnPool>();
				_pools.Add(key, component);
				return component;
			}
		}
		set
		{
			string message = "Cannot set PoolManager.Pools[key] directly. SpawnPools add themselves to PoolManager.Pools when created, so there is no need to set them explicitly. Create pools using PoolManager.Pools.Create() or add a SpawnPool component to a GameObject.";
			throw new NotImplementedException(message);
		}
	}

	public ICollection<string> Keys
	{
		get
		{
			string message = "If you need this, please request it.";
			throw new NotImplementedException(message);
		}
	}

	public ICollection<STQM_SpawnPool> Values
	{
		get
		{
			string message = "If you need this, please request it.";
			throw new NotImplementedException(message);
		}
	}

	private bool IsReadOnly => true;

	void ICollection<KeyValuePair<string, STQM_SpawnPool>>.CopyTo(KeyValuePair<string, STQM_SpawnPool>[] array, int arrayIndex)
	{
		string message = "PoolManager.Pools cannot be copied";
		throw new NotImplementedException(message);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _pools.GetEnumerator();
	}

	public STQM_SpawnPool Create(string poolName)
	{
		GameObject gameObject = new GameObject(poolName + "Pool");
		return gameObject.AddComponent<STQM_SpawnPool>();
	}

	public STQM_SpawnPool Create(string poolName, GameObject owner)
	{
		if (!assertValidPoolName(poolName))
		{
			return null;
		}
		string name = owner.gameObject.name;
		try
		{
			owner.gameObject.name = poolName;
			return owner.AddComponent<STQM_SpawnPool>();
		}
		finally
		{
			owner.gameObject.name = name;
		}
	}

	private bool assertValidPoolName(string poolName)
	{
		string text = poolName.Replace("Pool", string.Empty);
		if (text != poolName)
		{
			string message = $"'{poolName}' has the word 'Pool' in it. This word is reserved for GameObject defaul naming. The pool name has been changed to '{text}'";
			UnityEngine.Debug.LogWarning(message);
			poolName = text;
		}
		if (ContainsKey(poolName))
		{
			UnityEngine.Debug.Log($"A pool with the name '{poolName}' already exists");
			return false;
		}
		return true;
	}

	public override string ToString()
	{
		string[] array = new string[_pools.Count];
		_pools.Keys.CopyTo(array, 0);
		return string.Format("[{0}]", string.Join(", ", array));
	}

	public bool Destroy(string poolName)
	{
		if (!_pools.TryGetValue(poolName, out STQM_SpawnPool value))
		{
			UnityEngine.Debug.LogError($"PoolManager: Unable to destroy '{poolName}'. Not in PoolManager");
			return false;
		}
		UnityEngine.Object.Destroy(value.gameObject);
		return true;
	}

	public void DestroyAll()
	{
		foreach (KeyValuePair<string, STQM_SpawnPool> pool in _pools)
		{
			UnityEngine.Object.Destroy(pool.Value);
		}
	}

	internal void Add(STQM_SpawnPool spawnPool)
	{
		if (ContainsKey(spawnPool.poolName))
		{
			UnityEngine.Debug.LogError($"A pool with the name '{spawnPool.poolName}' already exists. This should only happen if a SpawnPool with this name is added to a scene twice.");
		}
		else
		{
			_pools.Add(spawnPool.poolName, spawnPool);
		}
	}

	public void Add(string key, STQM_SpawnPool value)
	{
		string message = "SpawnPools add themselves to PoolManager.Pools when created, so there is no need to Add() them explicitly. Create pools using PoolManager.Pools.Create() or add a SpawnPool component to a GameObject.";
		throw new NotImplementedException(message);
	}

	internal bool Remove(STQM_SpawnPool spawnPool)
	{
		if (!ContainsKey(spawnPool.poolName))
		{
			UnityEngine.Debug.LogError($"PoolManager: Unable to remove '{spawnPool.poolName}'. Pool not in PoolManager");
			return false;
		}
		_pools.Remove(spawnPool.poolName);
		return true;
	}

	public bool Remove(string poolName)
	{
		string message = "SpawnPools can only be destroyed, not removed and kept alive outside of PoolManager. There are only 2 legal ways to destroy a SpawnPool: Destroy the GameObject directly, if you have a reference, or use PoolManager.Destroy(string poolName).";
		throw new NotImplementedException(message);
	}

	public bool ContainsKey(string poolName)
	{
		return _pools.ContainsKey(poolName);
	}

	public bool TryGetValue(string poolName, out STQM_SpawnPool spawnPool)
	{
		return _pools.TryGetValue(poolName, out spawnPool);
	}

	public bool Contains(KeyValuePair<string, STQM_SpawnPool> item)
	{
		string message = "Use PoolManager.Pools.Contains(string poolName) instead.";
		throw new NotImplementedException(message);
	}

	public void Add(KeyValuePair<string, STQM_SpawnPool> item)
	{
		string message = "SpawnPools add themselves to PoolManager.Pools when created, so there is no need to Add() them explicitly. Create pools using PoolManager.Pools.Create() or add a SpawnPool component to a GameObject.";
		throw new NotImplementedException(message);
	}

	public void Clear()
	{
		string message = "Use PoolManager.Pools.DestroyAll() instead.";
		throw new NotImplementedException(message);
	}

	private void CopyTo(KeyValuePair<string, STQM_SpawnPool>[] array, int arrayIndex)
	{
		string message = "PoolManager.Pools cannot be copied";
		throw new NotImplementedException(message);
	}

	public bool Remove(KeyValuePair<string, STQM_SpawnPool> item)
	{
		string message = "SpawnPools can only be destroyed, not removed and kept alive outside of PoolManager. There are only 2 legal ways to destroy a SpawnPool: Destroy the GameObject directly, if you have a reference, or use PoolManager.Destroy(string poolName).";
		throw new NotImplementedException(message);
	}

	public IEnumerator<KeyValuePair<string, STQM_SpawnPool>> GetEnumerator()
	{
		return _pools.GetEnumerator();
	}
}
