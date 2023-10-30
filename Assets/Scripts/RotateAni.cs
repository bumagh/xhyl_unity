using UnityEngine;

public class RotateAni : MonoBehaviour
{
	private float speed = -360f;

	private void Update()
	{
		base.transform.Rotate(Vector3.forward * speed * Time.deltaTime);
	}
}
