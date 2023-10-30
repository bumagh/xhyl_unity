using System;
using UnityEngine;

public class FK3_CollisionReceiver : MonoBehaviour
{
	public Collider xCollider;

	public Action<Transform, Collider> actionOnTriggerEnter;

	public Action actionOnMouseDown;

	private void Awake()
	{
		xCollider.isTrigger = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		UnityEngine.Debug.Log(FK3_LogHelper.Lime("FK3_CollisionReceiver.OnTriggerEnter"));
		if (actionOnTriggerEnter != null)
		{
			actionOnTriggerEnter(base.transform, other);
		}
	}

	private void OnMouseDown()
	{
		if (actionOnMouseDown != null)
		{
			actionOnMouseDown();
		}
	}
}
