using System.Collections;
using UnityEngine;

public class FK3_CaymanEffectController : MonoBehaviour
{
	private Transform start;

	private Transform loop;

	private Coroutine waitLoop;

	private void OnEnable()
	{
		start = base.transform.Find("start");
		loop = base.transform.Find("loop");
		start.gameObject.SetActive(value: true);
		loop.gameObject.SetActive(value: false);
		if (waitLoop != null)
		{
			StopCoroutine(waitLoop);
		}
		waitLoop = StartCoroutine(WaitLoop());
	}

	private IEnumerator WaitLoop()
	{
		yield return new WaitForSeconds(0.5f);
		loop.gameObject.SetActive(value: true);
	}
}
