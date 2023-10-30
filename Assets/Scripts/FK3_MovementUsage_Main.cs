using SWS;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FK3_MovementUsage_Main : MonoBehaviour
{
	[SerializeField]
	private Transform m_prefab;

	[SerializeField]
	private PathManager m_path;

	private void Start()
	{
		StartCoroutine(IE_SplineMove_Test());
	}

	private void Update()
	{
	}

	private Transform Spawn()
	{
		Transform prefab = m_prefab;
		prefab.gameObject.SetActive(value: true);
		return prefab;
	}

	private Transform Despawn(Transform inst)
	{
		inst.gameObject.SetActive(value: false);
		return inst;
	}

	private IEnumerator IE_SplineMove_Test()
	{
		UnityEngine.Debug.Log("IE_SplineMove_Test");
		Transform inst = Spawn();
		splineMove movement = inst.gameObject.GetComponent<splineMove>();
		if (movement == null)
		{
			movement = inst.gameObject.AddComponent<splineMove>();
		}
		movement.SetPath(m_path);
		movement.StartMove();
		UnityEvent endEvent = movement.events[movement.events.Count - 1];
		endEvent.RemoveAllListeners();
		bool endFlag = false;
		endEvent.AddListener(delegate
		{
			movement.Stop();
			Despawn(inst);
			endFlag = true;
		});
		yield return new WaitForSeconds(1f);
		inst.gameObject.SetActive(value: false);
		yield return new WaitForSeconds(1f);
		inst.gameObject.SetActive(value: true);
		yield return new WaitUntil(() => endFlag);
		yield return new WaitForSeconds(1f);
		StartCoroutine(IE_SplineMove_Test());
	}
}
