using PathologicalGames;
using System.Collections;
using UnityEngine;

public class FK3_ReallySimpleAudioSpawner : MonoBehaviour
{
	public AudioSource prefab;

	public float spawnInterval = 2f;

	private FK3_SpawnPool pool;

	private void Start()
	{
		pool = GetComponent<FK3_SpawnPool>();
		StartCoroutine(Spawner());
	}

	private IEnumerator Spawner()
	{
		while (true)
		{
			AudioSource current = pool.Spawn(prefab, base.transform.position, base.transform.rotation);
			current.pitch = UnityEngine.Random.Range(0.7f, 1.4f);
			yield return new WaitForSeconds(spawnInterval);
		}
	}
}
