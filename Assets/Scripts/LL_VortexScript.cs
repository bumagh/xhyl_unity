using UnityEngine;

public class LL_VortexScript : MonoBehaviour
{
	public bool isClockwize = true;

	public float SpinSpeed = 30f;

	private void Update()
	{
		if (isClockwize)
		{
			base.transform.Rotate(Vector3.up, SpinSpeed * Time.deltaTime);
		}
		else
		{
			base.transform.Rotate(Vector3.down, SpinSpeed * Time.deltaTime);
		}
	}
}
