using UnityEngine;

public class DK_RotateCtrl : MonoBehaviour
{
	private int dir;

	private float angle;

	private int childCount;

	private void Start()
	{
		dir = ((UnityEngine.Random.Range(0, 2) == 0) ? 1 : (-1));
		childCount = base.transform.childCount;
	}

	private void Update()
	{
		angle = UnityEngine.Random.Range(10f, 90f);
		if (childCount == 0)
		{
			base.transform.Rotate(Vector3.forward * dir, angle * Time.deltaTime);
			return;
		}
		for (int i = 0; i < childCount; i++)
		{
			base.transform.GetChild(i).Rotate(Vector3.forward * dir, angle * Time.deltaTime);
		}
	}
}
