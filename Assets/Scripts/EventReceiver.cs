using SWS;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EventReceiver : MonoBehaviour
{
	public void MyMethod()
	{
	}

	public void PrintText(string text)
	{
		UnityEngine.Debug.Log(text);
	}

	public void RotateSprite(float newRot)
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		eulerAngles.y = newRot;
		base.transform.eulerAngles = eulerAngles;
	}

	public void SetDestination(UnityEngine.Object target)
	{
		StartCoroutine(SetDestinationRoutine(target));
	}

	private IEnumerator SetDestinationRoutine(UnityEngine.Object target)
	{
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		navMove myMove = GetComponent<navMove>();
		GameObject tar = (GameObject)target;
		myMove.ChangeSpeed(4f);
		agent.SetDestination(tar.transform.position);
		while (agent.pathPending)
		{
			yield return null;
		}
		float remain = agent.remainingDistance;
		while (remain == float.PositiveInfinity || remain - agent.stoppingDistance > float.Epsilon || agent.pathStatus != 0)
		{
			remain = agent.remainingDistance;
			yield return null;
		}
		yield return new WaitForSeconds(4f);
		myMove.ChangeSpeed(1.5f);
		myMove.moveToPath = true;
		myMove.StartMove();
	}

	public void ActivateForTime(UnityEngine.Object target)
	{
		StartCoroutine(ActivateForTimeRoutine(target));
	}

	private IEnumerator ActivateForTimeRoutine(UnityEngine.Object target)
	{
		GameObject tar = (GameObject)target;
		tar.SetActive(value: true);
		yield return new WaitForSeconds(6f);
		tar.SetActive(value: false);
	}
}
