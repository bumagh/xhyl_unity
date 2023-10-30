using System.Collections;
using UnityEngine;

public class DK_LightCor : MonoBehaviour
{
	private void OnEnable()
	{
		StartCoroutine(WaitLiht());
	}

	private IEnumerator WaitLiht()
	{
		while (true)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(i % 2 == 0);
			}
			yield return new WaitForSeconds(0.35f);
			for (int j = 0; j < base.transform.childCount; j++)
			{
				base.transform.GetChild(j).gameObject.SetActive(j % 2 != 0);
			}
			yield return new WaitForSeconds(0.35f);
		}
	}
}
