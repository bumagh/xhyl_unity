using PathologicalGames;
using System.Collections;
using UnityEngine;

public class FK3_ScriptedExample : MonoBehaviour
{
	public float moveSpeed = 1f;

	public float turnSpeed = 1f;

	public float newDirectionInterval = 3f;

	private FK3_SmoothLookAtConstraint lookCns;

	private FK3_TransformConstraint xformCns;

	private Transform xform;

	private void Awake()
	{
		xform = base.transform;
		xformCns = base.gameObject.AddComponent<FK3_TransformConstraint>();
		xformCns.noTargetMode = FK3_UnityConstraints.NO_TARGET_OPTIONS.SetByScript;
		xformCns.constrainRotation = false;
		lookCns = base.gameObject.AddComponent<FK3_SmoothLookAtConstraint>();
		lookCns.noTargetMode = FK3_UnityConstraints.NO_TARGET_OPTIONS.SetByScript;
		lookCns.pointAxis = Vector3.up;
		lookCns.upAxis = Vector3.forward;
		lookCns.speed = turnSpeed;
		StartCoroutine(LookAtRandom());
		StartCoroutine(MoveRandom());
	}

	private IEnumerator MoveRandom()
	{
		yield return new WaitForSeconds(newDirectionInterval + 0.001f);
		while (true)
		{
			yield return null;
			Vector3 targetDirection = lookCns.position - xform.position;
			Vector3 moveVect = targetDirection.normalized * moveSpeed * 0.1f;
			xformCns.position = xform.position + moveVect;
			UnityEngine.Debug.DrawRay(xform.position, xform.up * 2f, Color.grey);
			UnityEngine.Debug.DrawRay(xform.position, targetDirection.normalized * 2f, Color.green);
		}
	}

	private IEnumerator LookAtRandom()
	{
		while (true)
		{
			yield return new WaitForSeconds(newDirectionInterval);
			Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * 100f;
			lookCns.position = randomPosition + xform.position;
		}
	}
}
