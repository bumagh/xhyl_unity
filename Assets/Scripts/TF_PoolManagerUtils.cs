using UnityEngine;

public static class TF_PoolManagerUtils
{
	internal static void SetActive(GameObject obj, bool state)
	{
		obj.SetActive(state);
	}

	internal static bool activeInHierarchy(GameObject obj)
	{
		return obj.activeInHierarchy;
	}
}
