using PathologicalGames;
using System.Collections;
using UnityEngine;

public class FK3_ParticleWaitTest : MonoBehaviour
{
	public float spawnInterval = 0.25f;

	public string particlesPoolName;

	public ParticleSystem particleSystemPrefab;

	private IEnumerator Start()
	{
		FK3_SpawnPool particlesPool = FK3_PoolManager.Pools[particlesPoolName];
		ParticleSystem prefab = particleSystemPrefab;
		Vector3 prefabXform = particleSystemPrefab.transform.position;
		Quaternion prefabRot = particleSystemPrefab.transform.rotation;
		while (true)
		{
			yield return new WaitForSeconds(spawnInterval);
			ParticleSystem emitter = particlesPool.Spawn(prefab, prefabXform, prefabRot);
			while (emitter.IsAlive(withChildren: true))
			{
				yield return new WaitForSeconds(3f);
			}
			ParticleSystem inst = particlesPool.Spawn(prefab, prefabXform, prefabRot);
			particlesPool.Despawn(inst.transform, 2f);
			yield return new WaitForSeconds(2f);
			particlesPool.Spawn(prefab, prefabXform, prefabRot);
		}
	}
}
