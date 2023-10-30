using PathologicalGames;
using UnityEngine;

public class OnCreatedDelegateExample : MonoBehaviour
{
	private void Awake()
	{
		HW2_PoolManager.Pools.AddOnCreatedDelegate("Audio", RunMe);
	}

	public void RunMe(HW2_SpawnPool pool)
	{
		UnityEngine.Debug.Log("Delegate ran for pool " + pool.poolName);
	}
}
