using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("Path-o-logical/FK3_PoolManager/Pre-Runtime Pool Item")]
	public class FK3_PreRuntimePoolItem : MonoBehaviour
	{
		public string poolName = string.Empty;

		public string prefabName = string.Empty;

		public bool despawnOnStart = true;

		public bool doNotReparent;

		private void Start()
		{
			if (!FK3_PoolManager.Pools.TryGetValue(poolName, out FK3_SpawnPool spawnPool))
			{
				string format = "FK3_PreRuntimePoolItem Error ('{0}'): No pool with the name '{1}' exists! Create one using the FK3_PoolManager Inspector interface or FK3_PoolManager.CreatePool().See the online docs for more information at http://docs.poolmanager.path-o-logical.com";
				UnityEngine.Debug.LogError(string.Format(format, base.name, poolName));
			}
			else
			{
				spawnPool.Add(base.transform, prefabName, despawnOnStart, !doNotReparent);
			}
		}
	}
}
