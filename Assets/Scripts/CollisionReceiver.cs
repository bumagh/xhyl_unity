using System;
using UnityEngine;

public class CollisionReceiver : MonoBehaviour
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
		UnityEngine.Debug.Log(HW2_LogHelper.Lime("CollisionReceiver.OnTriggerEnter"));
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
