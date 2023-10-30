using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
	public class FK3_SpawnPoolsDict : IDictionary<string, FK3_SpawnPool>, IEnumerable, ICollection<KeyValuePair<string, FK3_SpawnPool>>, IEnumerable<KeyValuePair<string, FK3_SpawnPool>>
	{
		public delegate void OnCreatedDelegate(FK3_SpawnPool pool);

		internal Dictionary<string, OnCreatedDelegate> onCreatedDelegates = new Dictionary<string, OnCreatedDelegate>();

		private Dictionary<string, FK3_SpawnPool> _pools = new Dictionary<string, FK3_SpawnPool>();

		bool ICollection<KeyValuePair<string, FK3_SpawnPool>>.IsReadOnly => true;

		public int Count => _pools.Count;

		public FK3_SpawnPool this[string key]
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
				string message = "Cannot set FK3_PoolManager.Pools[key] directly. SpawnPools add themselves to FK3_PoolManager.Pools when created, so there is no need to set them explicitly. Create pools using FK3_PoolManager.Pools.Create() or add a FK3_SpawnPool component to a GameObject.";
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

		public ICollection<FK3_SpawnPool> Values
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

		public FK3_SpawnPool Create(string poolName)
		{
			GameObject gameObject = new GameObject(poolName + "Pool");
			return gameObject.AddComponent<FK3_SpawnPool>();
		}

		public FK3_SpawnPool Create(string poolName, GameObject owner)
		{
			if (!assertValidPoolName(poolName))
			{
				return null;
			}
			string name = owner.gameObject.name;
			try
			{
				owner.gameObject.name = poolName;
				return owner.AddComponent<FK3_SpawnPool>();
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
			if (!_pools.TryGetValue(poolName, out FK3_SpawnPool value))
			{
				UnityEngine.Debug.LogError($"FK3_PoolManager: Unable to destroy '{poolName}'. Not in FK3_PoolManager");
				return false;
			}
			UnityEngine.Object.Destroy(value.gameObject);
			return true;
		}

		public void DestroyAll()
		{
			foreach (KeyValuePair<string, FK3_SpawnPool> pool in _pools)
			{
				UnityEngine.Object.Destroy(pool.Value);
			}
			_pools.Clear();
		}

		internal void Add(FK3_SpawnPool spawnPool)
		{
			if (ContainsKey(spawnPool.poolName))
			{
				UnityEngine.Debug.LogError($"A pool with the name '{spawnPool.poolName}' already exists. This should only happen if a FK3_SpawnPool with this name is added to a scene twice.");
				return;
			}
			_pools.Add(spawnPool.poolName, spawnPool);
			if (onCreatedDelegates.ContainsKey(spawnPool.poolName))
			{
				onCreatedDelegates[spawnPool.poolName](spawnPool);
			}
		}

		public void Add(string key, FK3_SpawnPool value)
		{
			string message = "SpawnPools add themselves to FK3_PoolManager.Pools when created, so there is no need to Add() them explicitly. Create pools using FK3_PoolManager.Pools.Create() or add a FK3_SpawnPool component to a GameObject.";
			throw new NotImplementedException(message);
		}

		internal bool Remove(FK3_SpawnPool spawnPool)
		{
			if (!ContainsKey(spawnPool.poolName) & Application.isPlaying)
			{
				UnityEngine.Debug.LogError($"FK3_PoolManager: Unable to remove '{spawnPool.poolName}'. Pool not in FK3_PoolManager");
				return false;
			}
			_pools.Remove(spawnPool.poolName);
			return true;
		}

		public bool Remove(string poolName)
		{
			string message = "SpawnPools can only be destroyed, not removed and kept alive outside of FK3_PoolManager. There are only 2 legal ways to destroy a FK3_SpawnPool: Destroy the GameObject directly, if you have a reference, or use FK3_PoolManager.Destroy(string poolName).";
			throw new NotImplementedException(message);
		}

		public bool ContainsKey(string poolName)
		{
			return _pools.ContainsKey(poolName);
		}

		public bool TryGetValue(string poolName, out FK3_SpawnPool spawnPool)
		{
			return _pools.TryGetValue(poolName, out spawnPool);
		}

		public bool Contains(KeyValuePair<string, FK3_SpawnPool> item)
		{
			string message = "Use FK3_PoolManager.Pools.Contains(string poolName) instead.";
			throw new NotImplementedException(message);
		}

		public void Add(KeyValuePair<string, FK3_SpawnPool> item)
		{
			string message = "SpawnPools add themselves to FK3_PoolManager.Pools when created, so there is no need to Add() them explicitly. Create pools using FK3_PoolManager.Pools.Create() or add a FK3_SpawnPool component to a GameObject.";
			throw new NotImplementedException(message);
		}

		public void Clear()
		{
			string message = "Use FK3_PoolManager.Pools.DestroyAll() instead.";
			throw new NotImplementedException(message);
		}

		private void CopyTo(KeyValuePair<string, FK3_SpawnPool>[] array, int arrayIndex)
		{
			string message = "FK3_PoolManager.Pools cannot be copied";
			throw new NotImplementedException(message);
		}

		void ICollection<KeyValuePair<string, FK3_SpawnPool>>.CopyTo(KeyValuePair<string, FK3_SpawnPool>[] array, int arrayIndex)
		{
			string message = "FK3_PoolManager.Pools cannot be copied";
			throw new NotImplementedException(message);
		}

		public bool Remove(KeyValuePair<string, FK3_SpawnPool> item)
		{
			string message = "SpawnPools can only be destroyed, not removed and kept alive outside of FK3_PoolManager. There are only 2 legal ways to destroy a FK3_SpawnPool: Destroy the GameObject directly, if you have a reference, or use FK3_PoolManager.Destroy(string poolName).";
			throw new NotImplementedException(message);
		}

		public IEnumerator<KeyValuePair<string, FK3_SpawnPool>> GetEnumerator()
		{
			return _pools.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _pools.GetEnumerator();
		}
	}
}
