using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Path-o-logical/PoolManager/SpawnPool")]
public sealed class BZJX_SpawnPool : MonoBehaviour, IList<Transform>, ICollection<Transform>, IEnumerable<Transform>, IEnumerable
{
	public string poolName = string.Empty;

	public bool matchPoolScale;

	public bool matchPoolLayer;

	public bool dontDestroyOnLoad;

	public bool logMessages;

	public List<BZJX_PrefabPool> _perPrefabPoolOptions = new List<BZJX_PrefabPool>();

	public Dictionary<object, bool> prefabsFoldOutStates = new Dictionary<object, bool>();

	[HideInInspector]
	public float maxParticleDespawnTime = 60f;

	public BZJX_PrefabsDict prefabs = new BZJX_PrefabsDict();

	public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();

	private List<BZJX_PrefabPool> _prefabPools = new List<BZJX_PrefabPool>();

	internal List<Transform> _spawned = new List<Transform>();

	public Transform group
	{
		get;
		private set;
	}

	public Dictionary<string, BZJX_PrefabPool> prefabPools
	{
		get
		{
			Dictionary<string, BZJX_PrefabPool> dictionary = new Dictionary<string, BZJX_PrefabPool>();
			foreach (BZJX_PrefabPool prefabPool in _prefabPools)
			{
				dictionary[prefabPool.prefabGO.name] = prefabPool;
			}
			return dictionary;
		}
	}

	public Transform this[int index]
	{
		get
		{
			return _spawned[index];
		}
		set
		{
			throw new NotImplementedException("Read-only.");
		}
	}

	public int Count => _spawned.Count;

	public bool IsReadOnly
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		foreach (Transform item in _spawned)
		{
			yield return item;
		}
	}

	bool ICollection<Transform>.Remove(Transform item)
	{
		throw new NotImplementedException();
	}

	private void Awake()
	{
		if (dontDestroyOnLoad)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		group = base.transform;
		if (poolName == string.Empty)
		{
			poolName = group.name.Replace("Pool", string.Empty);
			poolName = poolName.Replace("(Clone)", string.Empty);
		}
		if (logMessages)
		{
			UnityEngine.Debug.Log($"SpawnPool {poolName}: Initializing..");
		}
		foreach (BZJX_PrefabPool perPrefabPoolOption in _perPrefabPoolOptions)
		{
			if (perPrefabPoolOption.prefab == null)
			{
				UnityEngine.Debug.LogWarning($"Initialization Warning: Pool '{poolName}' contains a PrefabPool with no prefab reference. Skipping.");
			}
			else
			{
				perPrefabPoolOption.inspectorInstanceConstructor();
				CreatePrefabPool(perPrefabPoolOption);
			}
		}
		BZJX_PoolManager.Pools.Add(this);
	}

	private void OnDestroy()
	{
		if (logMessages)
		{
			UnityEngine.Debug.Log($"SpawnPool {poolName}: Destroying...");
		}
		BZJX_PoolManager.Pools.Remove(this);
		StopAllCoroutines();
		_spawned.Clear();
		foreach (BZJX_PrefabPool prefabPool in _prefabPools)
		{
			prefabPool.SelfDestruct();
		}
		_prefabPools.Clear();
		prefabs._Clear();
	}

	public void CreatePrefabPool(BZJX_PrefabPool prefabPool)
	{
		if (GetPrefab(prefabPool.prefab) == null)
		{
			prefabPool.spawnPool = this;
			_prefabPools.Add(prefabPool);
			prefabs._Add(prefabPool.prefab.name, prefabPool.prefab);
		}
		if (!prefabPool.preloaded)
		{
			if (logMessages)
			{
				UnityEngine.Debug.Log($"SpawnPool {poolName}: Preloading {prefabPool.preloadAmount} {prefabPool.prefab.name}");
			}
			prefabPool.PreloadInstances();
		}
	}

	public void Add(Transform instance, string prefabName, bool despawn, bool parent)
	{
		foreach (BZJX_PrefabPool prefabPool in _prefabPools)
		{
			if (prefabPool.prefabGO == null)
			{
				UnityEngine.Debug.LogError("Unexpected Error: PrefabPool.prefabGO is null");
				return;
			}
			if (prefabPool.prefabGO.name == prefabName)
			{
				prefabPool.AddUnpooled(instance, despawn);
				if (logMessages)
				{
					UnityEngine.Debug.Log($"SpawnPool {poolName}: Adding previously unpooled instance {instance.name}");
				}
				if (parent)
				{
					instance.parent = group;
				}
				if (!despawn)
				{
					_spawned.Add(instance);
				}
				return;
			}
		}
		UnityEngine.Debug.LogError($"SpawnPool {poolName}: PrefabPool {prefabName} not found.");
	}

	public void Add(Transform item)
	{
		string message = "Use SpawnPool.Spawn() to properly add items to the pool.";
		throw new NotImplementedException(message);
	}

	public void Remove(Transform item)
	{
		string message = "Use Despawn() to properly manage items that should remain in the pool but be deactivated.";
		throw new NotImplementedException(message);
	}

	public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot)
	{
		Transform transform;
		foreach (BZJX_PrefabPool prefabPool in _prefabPools)
		{
			if (prefabPool.prefabGO == prefab.gameObject)
			{
				transform = prefabPool.SpawnInstance(pos, rot);
				if (transform == null)
				{
					return null;
				}
				if (transform.parent != group)
				{
					transform.parent = group;
				}
				_spawned.Add(transform);
				return transform;
			}
		}
		BZJX_PrefabPool bZJX_PrefabPool = new BZJX_PrefabPool(prefab);
		CreatePrefabPool(bZJX_PrefabPool);
		transform = bZJX_PrefabPool.SpawnInstance(pos, rot);
		transform.parent = group;
		_spawned.Add(transform);
		return transform;
	}

	public Transform Spawn(Transform prefab)
	{
		return Spawn(prefab, Vector3.zero, Quaternion.identity);
	}

	public ParticleSystem Spawn(ParticleSystem prefab, Vector3 pos, Quaternion quat)
	{
		Transform transform = Spawn(prefab.transform, pos, quat);
		if (transform == null)
		{
			return null;
		}
		ParticleSystem component = transform.GetComponent<ParticleSystem>();
		StartCoroutine(ListenForEmitDespawn(component));
		return component;
	}

	public void Despawn(Transform xform)
	{
		bool flag = false;
		foreach (BZJX_PrefabPool prefabPool in _prefabPools)
		{
			if (prefabPool._spawned.Contains(xform))
			{
				flag = prefabPool.DespawnInstance(xform);
				break;
			}
			if (prefabPool._despawned.Contains(xform))
			{
				UnityEngine.Debug.LogError($"SpawnPool {poolName}: {xform.name} has already been despawned. You cannot despawn something more than once!");
				return;
			}
		}
		if (!flag)
		{
			UnityEngine.Debug.LogError($"SpawnPool {poolName}: {xform.name} not found in SpawnPool");
		}
		else
		{
			_spawned.Remove(xform);
		}
	}

	public void Despawn(Transform instance, float seconds)
	{
		StartCoroutine(DoDespawnAfterSeconds(instance, seconds));
	}

	private IEnumerator DoDespawnAfterSeconds(Transform instance, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Despawn(instance);
	}

	public void DespawnAll()
	{
		List<Transform> list = new List<Transform>(_spawned);
		foreach (Transform item in list)
		{
			Despawn(item);
		}
	}

	public bool IsSpawned(Transform instance)
	{
		return _spawned.Contains(instance);
	}

	public Transform GetPrefab(Transform prefab)
	{
		foreach (BZJX_PrefabPool prefabPool in _prefabPools)
		{
			if (prefabPool.prefabGO == null)
			{
				UnityEngine.Debug.LogError($"SpawnPool {poolName}: PrefabPool.prefabGO is null");
			}
			if (prefabPool.prefabGO == prefab.gameObject)
			{
				return prefabPool.prefab;
			}
		}
		return null;
	}

	public GameObject GetPrefab(GameObject prefab)
	{
		foreach (BZJX_PrefabPool prefabPool in _prefabPools)
		{
			if (prefabPool.prefabGO == null)
			{
				UnityEngine.Debug.LogError($"SpawnPool {poolName}: PrefabPool.prefabGO is null");
			}
			if (prefabPool.prefabGO == prefab)
			{
				return prefabPool.prefabGO;
			}
		}
		return null;
	}

	private IEnumerator ListenForEmitDespawn(ParticleSystem emitter)
	{
		yield return new WaitForSeconds(emitter.startDelay + 0.25f);
		float safetimer = 0f;
		while (emitter.IsAlive(withChildren: true))
		{
			if (!PoolManagerUtils.activeInHierarchy(emitter.gameObject))
			{
				emitter.Clear(withChildren: true);
				yield break;
			}
			safetimer += Time.deltaTime;
			if (safetimer > maxParticleDespawnTime)
			{
				UnityEngine.Debug.LogWarning($"SpawnPool {poolName}: Timed out while listening for all particles to die. Waited for {maxParticleDespawnTime}sec.");
			}
			yield return null;
		}
		Despawn(emitter.transform);
	}

	public override string ToString()
	{
		List<string> list = new List<string>();
		foreach (Transform item in _spawned)
		{
			list.Add(item.name);
		}
		return string.Join(", ", list.ToArray());
	}

	public bool Contains(Transform item)
	{
		string message = "Use IsSpawned(Transform instance) instead.";
		throw new NotImplementedException(message);
	}

	public void CopyTo(Transform[] array, int arrayIndex)
	{
		_spawned.CopyTo(array, arrayIndex);
	}

	public IEnumerator<Transform> GetEnumerator()
	{
		foreach (Transform item in _spawned)
		{
			yield return item;
		}
	}

	public int IndexOf(Transform item)
	{
		throw new NotImplementedException();
	}

	public void Insert(int index, Transform item)
	{
		throw new NotImplementedException();
	}

	public void RemoveAt(int index)
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		throw new NotImplementedException();
	}
}
