using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FK3_SimpleSpawner : MonoBehaviour
{
	public string poolName;

	public Transform testPrefab;

	public int spawnAmount = 50;

	public float spawnInterval = 0.25f;

	public string particlesPoolName;

	public ParticleSystem particleSystemPrefab;

	private void Start()
	{
		StartCoroutine(Spawner());
		if (particlesPoolName != string.Empty)
		{
			StartCoroutine(ParticleSpawner());
		}
	}

	private IEnumerator ParticleSpawner()
	{
		FK3_SpawnPool particlesPool = FK3_PoolManager.Pools[particlesPoolName];
		ParticleSystem prefab = particleSystemPrefab;
		Vector3 prefabXform = particleSystemPrefab.transform.position;
		Quaternion prefabRot = particleSystemPrefab.transform.rotation;
		while (true)
		{
			ParticleSystem emitter = particlesPool.Spawn(prefab, prefabXform, prefabRot);
			while (emitter.IsAlive(withChildren: true))
			{
				yield return new WaitForSeconds(1f);
			}
			ParticleSystem inst = particlesPool.Spawn(prefab, prefabXform, prefabRot);
			particlesPool.Despawn(inst.transform, 2f);
			yield return new WaitForSeconds(2f);
		}
	}

	private IEnumerator Spawner()
	{
		int count = spawnAmount;
		FK3_SpawnPool shapesPool = FK3_PoolManager.Pools[poolName];
		while (count > 0)
		{
			Transform inst = shapesPool.Spawn(testPrefab);
			inst.localPosition = new Vector3(spawnAmount + 2 - count, 0f, 0f);
			count--;
			yield return new WaitForSeconds(spawnInterval);
		}
		StartCoroutine(Despawner());
		yield return null;
	}

	private IEnumerator Despawner()
	{
		FK3_SpawnPool shapesPool = FK3_PoolManager.Pools[poolName];
		List<Transform> spawnedCopy = new List<Transform>(shapesPool);
		UnityEngine.Debug.Log(shapesPool.ToString());
		foreach (Transform instance in spawnedCopy)
		{
			shapesPool.Despawn(instance);
			yield return new WaitForSeconds(spawnInterval);
		}
		StartCoroutine(Spawner());
		yield return null;
	}
}
