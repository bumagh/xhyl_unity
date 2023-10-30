using UnityEngine;

namespace PathologicalGames
{
	public static class FK3_InstanceManager
	{
		public static bool POOLING_ENABLED;

		public static Transform Spawn(string poolName, Transform prefab, Vector3 pos, Quaternion rot)
		{
			return Object.Instantiate(prefab, pos, rot);
		}

		public static void Despawn(string poolName, Transform instance)
		{
			UnityEngine.Object.Destroy(instance.gameObject);
		}
	}
}
