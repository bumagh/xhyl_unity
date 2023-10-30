using UnityEngine;

public class RuntimeStartupScript
{
	private static void OnRuntimeMethodLoad()
	{
		UnityEngine.Debug.Log("After scene is loaded and game is running");
	}
}
