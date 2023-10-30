using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	public class HW2_SpawnPoolsDict : IDictionary<string, HW2_SpawnPool>, IEnumerable, ICollection<KeyValuePair<string, HW2_SpawnPool>>, IEnumerable<KeyValuePair<string, HW2_SpawnPool>>
	{
		public delegate void OnCreatedDelegate(HW2_SpawnPool pool);

		internal Dictionary<string, OnCreatedDelegate> onCreatedDelegates = new Dictionary<string, OnCreatedDelegate>();

		private Dictionary<string, HW2_SpawnPool> _pools = new Dictionary<string, HW2_SpawnPool>();

		bool ICollection<KeyValuePair<string, HW2_SpawnPool>>.IsReadOnly => true;

		public int Count => _pools.Count;

		public HW2_SpawnPool this[string key]
		{
			get
			{
				try
				{
					return _pools[key];
				}
				catch (KeyNotFoundException)
				{
					string message = $"A Pool with the name '{key}' not found. \nPools={ToString()}";
					throw new KeyNotFoundException(message);
				}
			}
			set
			{
				string message = "Cannot set HW2_PoolManager.Pools[key] directly. SpawnPools add themselves to HW2_PoolManager.Pools when created, so there is no need to set them explicitly. Create pools using HW2_PoolManager.Pools.Create() or add a HW2_SpawnPool component to a GameObject.";
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

		public ICollection<HW2_SpawnPool> Values
		{
			get
			{
				string message = "If you need this, please request it.";
				throw new NotImplementedException(message);
			}
		}

		private bool IsReadOnly => true;

		public void AddOnCreatedDelegate(string poolName, OnCreatedDelegate createdDelegate)
		{
			if (!onCreatedDelegates.ContainsKey(poolName))
			{
				onCreatedDelegates.Add(poolName, createdDelegate);
				return;
			}
			Dictionary<string, OnCreatedDelegate> dictionary = new Dictionary<string, OnCreatedDelegate>();
			(dictionary = onCreatedDelegates)[poolName] = (OnCreatedDelegate)Delegate.Combine(dictionary[poolName], createdDelegate);
		}

		public void RemoveOnCreatedDelegate(string poolName, OnCreatedDelegate createdDelegate)
		{
			if (!onCreatedDelegates.ContainsKey(poolName))
			{
				throw new KeyNotFoundException("No OnCreatedDelegates found for pool name '" + poolName + "'.");
			}
			Dictionary<string, OnCreatedDelegate> dictionary = new Dictionary<string, OnCreatedDelegate>();
			(dictionary = onCreatedDelegates)[poolName] = (OnCreatedDelegate)Delegate.Remove(dictionary[poolName], createdDelegate);
		}

		public HW2_SpawnPool Create(string poolName)
		{
			GameObject gameObject = new GameObject(poolName + "Pool");
			return gameObject.AddComponent<HW2_SpawnPool>();
		}

		public HW2_SpawnPool Create(string poolName, GameObject owner)
		{
			if (!assertValidPoolName(poolName))
			{
				return null;
			}
			string name = owner.gameObject.name;
			try
			{
				owner.gameObject.name = poolName;
				return owner.AddComponent<HW2_SpawnPool>();
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
			if (!_pools.TryGetValue(poolName, out HW2_SpawnPool value))
			{
				UnityEngine.Debug.LogError($"HW2_PoolManager: Unable to destroy '{poolName}'. Not in HW2_PoolManager");
				return false;
			}
			UnityEngine.Object.Destroy(value.gameObject);
			return true;
		}

		public void DestroyAll()
		{
			foreach (KeyValuePair<string, HW2_SpawnPool> pool in _pools)
			{
				UnityEngine.Object.Destroy(pool.Value);
			}
			_pools.Clear();
		}

		internal void Add(HW2_SpawnPool spawnPool)
		{
			if (ContainsKey(spawnPool.poolName))
			{
				UnityEngine.Debug.LogError($"A pool with the name '{spawnPool.poolName}' already exists. This should only happen if a HW2_SpawnPool with this name is added to a scene twice.");
				return;
			}
			_pools.Add(spawnPool.poolName, spawnPool);
			if (onCreatedDelegates.ContainsKey(spawnPool.poolName))
			{
				onCreatedDelegates[spawnPool.poolName](spawnPool);
			}
		}

		public void Add(string key, HW2_SpawnPool value)
		{
			string message = "SpawnPools add themselves to HW2_PoolManager.Pools when created, so there is no need to Add() them explicitly. Create pools using HW2_PoolManager.Pools.Create() or add a HW2_SpawnPool component to a GameObject.";
			throw new NotImplementedException(message);
		}

		internal bool Remove(HW2_SpawnPool spawnPool)
		{
			if (!ContainsKey(spawnPool.poolName) & Application.isPlaying)
			{
				UnityEngine.Debug.LogError($"HW2_PoolManager: Unable to remove '{spawnPool.poolName}'. Pool not in HW2_PoolManager");
				return false;
			}
			_pools.Remove(spawnPool.poolName);
			return true;
		}

		public bool Remove(string poolName)
		{
			string message = "SpawnPools can only be destroyed, not removed and kept alive outside of HW2_PoolManager. There are only 2 legal ways to destroy a HW2_SpawnPool: Destroy the GameObject directly, if you have a reference, or use HW2_PoolManager.Destroy(string poolName).";
			throw new NotImplementedException(message);
		}

		public bool ContainsKey(string poolName)
		{
			return _pools.ContainsKey(poolName);
		}

		public bool TryGetValue(string poolName, out HW2_SpawnPool spawnPool)
		{
			return _pools.TryGetValue(poolName, out spawnPool);
		}

		public bool Contains(KeyValuePair<string, HW2_SpawnPool> item)
		{
			string message = "Use HW2_PoolManager.Pools.Contains(string poolName) instead.";
			throw new NotImplementedException(message);
		}

		public void Add(KeyValuePair<string, HW2_SpawnPool> item)
		{
			string message = "SpawnPools add themselves to HW2_PoolManager.Pools when created, so there is no need to Add() them explicitly. Create pools using HW2_PoolManager.Pools.Create() or add a HW2_SpawnPool component to a GameObject.";
			throw new NotImplementedException(message);
		}

		public void Clear()
		{
			string message = "Use HW2_PoolManager.Pools.DestroyAll() instead.";
			throw new NotImplementedException(message);
		}

		private void CopyTo(KeyValuePair<string, HW2_SpawnPool>[] array, int arrayIndex)
		{
			string message = "HW2_PoolManager.Pools cannot be copied";
			throw new NotImplementedException(message);
		}

		void ICollection<KeyValuePair<string, HW2_SpawnPool>>.CopyTo(KeyValuePair<string, HW2_SpawnPool>[] array, int arrayIndex)
		{
			string message = "HW2_PoolManager.Pools cannot be copied";
			throw new NotImplementedException(message);
		}

		public bool Remove(KeyValuePair<string, HW2_SpawnPool> item)
		{
			string message = "SpawnPools can only be destroyed, not removed and kept alive outside of HW2_PoolManager. There are only 2 legal ways to destroy a HW2_SpawnPool: Destroy the GameObject directly, if you have a reference, or use HW2_PoolManager.Destroy(string poolName).";
			throw new NotImplementedException(message);
		}

		public IEnumerator<KeyValuePair<string, HW2_SpawnPool>> GetEnumerator()
		{
			return _pools.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _pools.GetEnumerator();
		}
	}
}
