using System.Collections;
using UnityEngine;

public class DNTG_ShowEffAct : MonoBehaviour
{
	private int tempNum;

	private int tempIndex;

	public float waitTime = 0.5f;

	public float waitActivTime = 0.3f;

	private void OnEnable()
	{
		tempNum = base.transform.childCount;
		tempIndex = 0;
		StartCoroutine(WaitShowEff());
	}

	private IEnumerator WaitShowEff()
	{
		while (true)
		{
			tempNum = base.transform.childCount;
			for (int i = 0; i < tempNum; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(value: false);
			}
			yield return new WaitForSeconds(waitActivTime);
			if (tempIndex >= tempNum)
			{
				tempIndex = 0;
			}
			base.transform.GetChild(tempIndex).gameObject.SetActive(value: true);
			tempIndex++;
			yield return new WaitForSeconds(waitTime);
		}
	}
}
