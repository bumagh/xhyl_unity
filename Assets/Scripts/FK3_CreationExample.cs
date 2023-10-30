using PathologicalGames;
using System.Collections;
using UnityEngine;

public class FK3_CreationExample : MonoBehaviour
{
	public Transform testPrefab;

	public string poolName = "Creator";

	public int spawnAmount = 50;

	public float spawnInterval = 0.25f;

	private FK3_SpawnPool pool;

	private void Start()
	{
		pool = FK3_PoolManager.Pools.Create(poolName);
		pool.group.parent = base.transform;
		pool.group.localPosition = new Vector3(1.5f, 0f, 0f);
		pool.group.localRotation = Quaternion.identity;
		FK3_PrefabPool fK3_PrefabPool = new FK3_PrefabPool(testPrefab);
		fK3_PrefabPool.preloadAmount = 5;
		fK3_PrefabPool.cullDespawned = true;
		fK3_PrefabPool.cullAbove = 10;
		fK3_PrefabPool.cullDelay = 1;
		fK3_PrefabPool.limitInstances = true;
		fK3_PrefabPool.limitAmount = 5;
		fK3_PrefabPool.limitFIFO = true;
		pool.CreatePrefabPool(fK3_PrefabPool);
		StartCoroutine(Spawner());
		Transform prefab = FK3_PoolManager.Pools["Shapes"].prefabs["Cube"];
		Transform transform = FK3_PoolManager.Pools["Shapes"].Spawn(prefab);
		transform.name = "Cube (Spawned By FK3_CreationExample.cs)";
	}

	private IEnumerator Spawner()
	{
		int count = spawnAmount;
		while (count > 0)
		{
			Transform inst = pool.Spawn(testPrefab, Vector3.zero, Quaternion.identity);
			inst.localPosition = new Vector3(spawnAmount - count, 0f, 0f);
			count--;
			yield return new WaitForSeconds(spawnInterval);
		}
		StartCoroutine(Despawner());
	}

	private IEnumerator Despawner()
	{
		while (pool.Count > 0)
		{
			Transform instance = pool[pool.Count - 1];
			pool.Despawn(instance);
			yield return new WaitForSeconds(spawnInterval);
		}
	}
}
