using System.Collections;
using UnityEngine;

public class Transform_Demo : MonoBehaviour
{
	public Vector3 rotate = new Vector3(0f, 3f, 0f);

	private Transform xform;

	private bool moveForward = true;

	private float speed = 5f;

	private float duration = 0.6f;

	private float delay = 1.5f;

	private Vector3 bigScale = new Vector3(2f, 2f, 2f);

	private Vector3 smallScale = new Vector3(1f, 1f, 1f);

	private void Awake()
	{
		xform = base.transform;
	}

	private void Start()
	{
		StartCoroutine(MoveTarget());
		StartCoroutine(RotateTarget());
	}

	private IEnumerator RotateTarget()
	{
		yield return new WaitForSeconds(delay);
		while (true)
		{
			xform.Rotate(rotate);
			yield return null;
		}
	}

	private IEnumerator MoveTarget()
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			float savedTime = Time.time;
			while (Time.time - savedTime < duration)
			{
				if (moveForward)
				{
					xform.Translate(Vector3.up * (Time.deltaTime * speed));
					xform.localScale = Vector3.Lerp(xform.localScale, bigScale, Time.deltaTime * 4.75f);
				}
				else
				{
					xform.Translate(Vector3.down * (Time.deltaTime * speed));
					xform.localScale = Vector3.Lerp(xform.localScale, smallScale, Time.deltaTime * 4.75f);
				}
				yield return null;
			}
			moveForward = !moveForward;
		}
	}
}
