using PathologicalGames;
using UnityEngine;

public class FK3_OnSpawnedExample : MonoBehaviour
{
	private void OnSpawned(FK3_SpawnPool pool)
	{
		UnityEngine.Debug.Log($"FK3_OnSpawnedExample | OnSpawned running for '{base.name}' in pool '{pool.poolName}'.");
	}

	private void OnDespawned(FK3_SpawnPool pool)
	{
		UnityEngine.Debug.Log($"FK3_OnSpawnedExample | OnDespawned unning for '{base.name}' in pool '{pool.poolName}'.");
	}
}
