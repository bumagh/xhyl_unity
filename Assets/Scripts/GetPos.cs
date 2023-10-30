using UnityEngine;

public class GetPos : MonoBehaviour
{
	public bool isDeBug;

	private void Update()
	{
		if (isDeBug)
		{
			UnityEngine.Debug.LogError(base.transform.localPosition);
		}
	}
}
