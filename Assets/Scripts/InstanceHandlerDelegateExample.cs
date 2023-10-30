using PathologicalGames;
using System;
using UnityEngine;

public class InstanceHandlerDelegateExample : MonoBehaviour
{
	public string poolName = string.Empty;

	private void Awake()
	{
		InstanceHandler.InstantiateDelegates = (InstanceHandler.InstantiateDelegate)Delegate.Combine(InstanceHandler.InstantiateDelegates, new InstanceHandler.InstantiateDelegate(InstantiateDelegate));
		InstanceHandler.DestroyDelegates = (InstanceHandler.DestroyDelegate)Delegate.Combine(InstanceHandler.DestroyDelegates, new InstanceHandler.DestroyDelegate(DestroyDelegate));
	}

	private void Start()
	{
		HW2_SpawnPool hW2_SpawnPool = HW2_PoolManager.Pools[poolName];
		hW2_SpawnPool.instantiateDelegates = (HW2_SpawnPool.InstantiateDelegate)Delegate.Combine(hW2_SpawnPool.instantiateDelegates, new HW2_SpawnPool.InstantiateDelegate(InstantiateDelegateForShapesPool));
	}

	public GameObject InstantiateDelegate(GameObject prefab, Vector3 pos, Quaternion rot)
	{
		UnityEngine.Debug.Log("Using my own instantiation delegate on prefab '" + prefab.name + "'!");
		return UnityEngine.Object.Instantiate(prefab, pos, rot);
	}

	public void DestroyDelegate(GameObject instance)
	{
		UnityEngine.Debug.Log("Using my own destroy delegate on '" + instance.name + "'!");
		UnityEngine.Object.Destroy(instance);
	}

	public GameObject InstantiateDelegateForShapesPool(GameObject prefab, Vector3 pos, Quaternion rot)
	{
		UnityEngine.Debug.Log("Using my own instantiation delegate for just the 'Shapes' pool on prefab '" + prefab.name + "'!");
		return UnityEngine.Object.Instantiate(prefab, pos, rot);
	}
}
