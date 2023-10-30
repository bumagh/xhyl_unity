using PathologicalGames;
using System.Collections;
using UnityEngine;

public class CreationExample : MonoBehaviour
{
	public Transform testPrefab;

	public string poolName = "Creator";

	public int spawnAmount = 50;

	public float spawnInterval = 0.25f;

	private HW2_SpawnPool pool;

	private void Start()
	{
		pool = HW2_PoolManager.Pools.Create(poolName);
		pool.group.parent = base.transform;
		pool.group.localPosition = new Vector3(1.5f, 0f, 0f);
		pool.group.localRotation = Quaternion.identity;
		HW2_PrefabPool hW2_PrefabPool = new HW2_PrefabPool(testPrefab);
		hW2_PrefabPool.preloadAmount = 5;
		hW2_PrefabPool.cullDespawned = true;
		hW2_PrefabPool.cullAbove = 10;
		hW2_PrefabPool.cullDelay = 1;
		hW2_PrefabPool.limitInstances = true;
		hW2_PrefabPool.limitAmount = 5;
		hW2_PrefabPool.limitFIFO = true;
		pool.CreatePrefabPool(hW2_PrefabPool);
		StartCoroutine(Spawner());
		Transform prefab = HW2_PoolManager.Pools["Shapes"].prefabs["Cube"];
		Transform transform = HW2_PoolManager.Pools["Shapes"].Spawn(prefab);
		transform.name = "Cube (Spawned By CreationExample.cs)";
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
