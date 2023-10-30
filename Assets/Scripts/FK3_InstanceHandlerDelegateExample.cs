using PathologicalGames;
using System;
using UnityEngine;

public class FK3_InstanceHandlerDelegateExample : MonoBehaviour
{
	public string poolName = string.Empty;

	private void Awake()
	{
		FK3_InstanceHandler.InstantiateDelegates = (FK3_InstanceHandler.InstantiateDelegate)Delegate.Combine(FK3_InstanceHandler.InstantiateDelegates, new FK3_InstanceHandler.InstantiateDelegate(InstantiateDelegate));
		FK3_InstanceHandler.DestroyDelegates = (FK3_InstanceHandler.DestroyDelegate)Delegate.Combine(FK3_InstanceHandler.DestroyDelegates, new FK3_InstanceHandler.DestroyDelegate(DestroyDelegate));
	}

	private void Start()
	{
		FK3_SpawnPool fK3_SpawnPool = FK3_PoolManager.Pools[poolName];
		fK3_SpawnPool.instantiateDelegates = (FK3_SpawnPool.InstantiateDelegate)Delegate.Combine(fK3_SpawnPool.instantiateDelegates, new FK3_SpawnPool.InstantiateDelegate(InstantiateDelegateForShapesPool));
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
