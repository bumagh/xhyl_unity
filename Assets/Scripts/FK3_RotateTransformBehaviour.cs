using UnityEngine;

public class FK3_RotateTransformBehaviour : MonoBehaviour
{
	public Vector3 Amount = Vector3.up;

	private void Update()
	{
		base.transform.Rotate(Amount * Time.smoothDeltaTime);
	}
}
