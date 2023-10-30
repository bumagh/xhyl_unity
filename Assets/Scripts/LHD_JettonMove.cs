using System.Collections;
using UnityEngine;

public class LHD_JettonMove : MonoBehaviour
{
	public GameObject testTarget;

	private GameObject StartObj;

	private GameObject TargetObj;

	private Vector3 EndPosition;

	private float _value;

	private void Move()
	{
	}

	private void OnEnable()
	{
		TargetObj = testTarget;
	}

	private void OnDisable()
	{
		base.enabled = false;
	}

	public void SetTarget(GameObject start, GameObject target)
	{
		StartObj = start;
		TargetObj = target;
	}

	private void RandomLocation()
	{
		EndPosition = Vector3.right * UnityEngine.Random.Range((0f - TargetObj.GetComponent<RectTransform>().rect.width) / 2f, TargetObj.GetComponent<RectTransform>().rect.width / 2f) + Vector3.up * UnityEngine.Random.Range((0f - TargetObj.GetComponent<RectTransform>().rect.height) / 2f, TargetObj.GetComponent<RectTransform>().rect.height / 2f);
		StartCoroutine(JettonMove());
	}

	private IEnumerator JettonMove()
	{
		while (true)
		{
			base.transform.position = Vector2.Lerp(StartObj.transform.position, TargetObj.transform.position + EndPosition, _value);
			if (_value >= 1f)
			{
				break;
			}
			_value += Time.deltaTime * 2f;
			yield return new WaitForSeconds(0.02f);
		}
		base.transform.position = TargetObj.transform.position + EndPosition;
	}
}
