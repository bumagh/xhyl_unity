using PathologicalGames;
using UnityEngine;

public class OnSpawnedExample : MonoBehaviour
{
	private void OnSpawned(HW2_SpawnPool pool)
	{
		UnityEngine.Debug.Log($"OnSpawnedExample | OnSpawned running for '{base.name}' in pool '{pool.poolName}'.");
	}

	private void OnDespawned(HW2_SpawnPool pool)
	{
		UnityEngine.Debug.Log($"OnSpawnedExample | OnDespawned unning for '{base.name}' in pool '{pool.poolName}'.");
	}
}
