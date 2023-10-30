using UnityEngine;

public class TF_EnergeGun : MonoBehaviour
{
	private Vector3 vecInit;

	private void Start()
	{
		vecInit = base.transform.position;
	}

	private void Update()
	{
		base.transform.RotateAround(vecInit + Vector3.left * 0.1f, Vector3.forward, 1000f * Time.deltaTime);
		base.transform.GetChild(0).rotation = Quaternion.identity;
	}
}
