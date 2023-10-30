using PathologicalGames;
using System.Collections;
using UnityEngine;

public class FK3_ReallySimpleParticleSpawner : MonoBehaviour
{
	public string poolName;

	public ParticleSystem prefab;

	public float spawnInterval = 1f;

	private void Start()
	{
		StartCoroutine(ParticleSpawner());
	}

	private IEnumerator ParticleSpawner()
	{
		while (true)
		{
			FK3_PoolManager.Pools[poolName].Spawn(prefab, base.transform.position, base.transform.rotation);
			yield return new WaitForSeconds(spawnInterval);
		}
	}
}
