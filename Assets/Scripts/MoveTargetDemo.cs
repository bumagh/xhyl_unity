using System.Collections;
using UnityEngine;

public class MoveTargetDemo : MonoBehaviour
{
	private Transform xform;

	private bool moveForward = true;

	private float speed = 20f;

	private float duration = 0.6f;

	private float delay = 3f;

	private void Start()
	{
		xform = base.transform;
		StartCoroutine(MoveTarget());
	}

	private IEnumerator MoveTarget()
	{
		yield return new WaitForSeconds(delay);
		float savedTime = Time.time;
		while (Time.time - savedTime < duration)
		{
			if (moveForward)
			{
				xform.Translate(Vector3.forward * (Time.deltaTime * speed));
			}
			else
			{
				xform.Translate(Vector3.back * (Time.deltaTime * speed));
			}
			yield return null;
		}
		moveForward = !moveForward;
		StartCoroutine(MoveTarget());
	}
}
