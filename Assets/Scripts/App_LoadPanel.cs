using System.Collections;
using UnityEngine;

public class App_LoadPanel : MonoBehaviour
{
	private Coroutine waitSetAct;

	private void OnEnable()
	{
		if (waitSetAct != null)
		{
			StopCoroutine(waitSetAct);
		}
		waitSetAct = StartCoroutine(WaitSetAct());
	}

	private IEnumerator WaitSetAct()
	{
		yield return new WaitForSeconds(7f);
		if (base.gameObject.activeInHierarchy)
		{
			UnityEngine.Debug.LogError("===超时===");
			base.gameObject.SetActive(value: false);
			ZH2_GVars.isGetPublicKey = true;
		}
	}
}
