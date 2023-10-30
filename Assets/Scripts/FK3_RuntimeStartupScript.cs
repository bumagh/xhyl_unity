using UnityEngine;

public class FK3_RuntimeStartupScript
{
	private static void OnRuntimeMethodLoad()
	{
		UnityEngine.Debug.Log("After scene is loaded and game is running");
	}
}
