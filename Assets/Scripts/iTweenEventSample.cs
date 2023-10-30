using UnityEngine;

public class iTweenEventSample : MonoBehaviour
{
	public void OnStartEvent()
	{
		UnityEngine.Debug.Log("Tween Started!");
	}

	public void OnCompleteEvent()
	{
		UnityEngine.Debug.Log("Tween Completed!");
	}
}
