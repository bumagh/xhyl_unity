using PathologicalGames;
using UnityEngine;

public class FK3_OnCreatedDelegateExample : MonoBehaviour
{
	private void Awake()
	{
		FK3_PoolManager.Pools.AddOnCreatedDelegate("Audio", RunMe);
	}

	public void RunMe(FK3_SpawnPool pool)
	{
		UnityEngine.Debug.Log("Delegate ran for pool " + pool.poolName);
	}
}
