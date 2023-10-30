using System.Collections;
using UnityEngine;

public class DNTG_EffSimilarLightning : MonoBehaviour
{
	private Coroutine waitOver;

	private void OnEnable()
	{
		if (waitOver != null)
		{
			StopCoroutine(waitOver);
		}
		waitOver = StartCoroutine(WaitOver());
	}

	private IEnumerator WaitOver()
	{
		yield return new WaitForSeconds(2f);
		if (base.gameObject.activeInHierarchy)
		{
			if (ZH2_GVars.DestroyEffSimilarLightning != null)
			{
				UnityEngine.Debug.LogError("=====委托删除====");
				ZH2_GVars.DestroyEffSimilarLightning(base.gameObject);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		yield return new WaitForSeconds(0.5f);
		if (base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
