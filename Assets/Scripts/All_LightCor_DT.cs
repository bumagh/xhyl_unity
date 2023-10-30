using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class All_LightCor_DT : MonoBehaviour
{
	public bool isImage = true;

	public float waitTime = 0.35f;

	private Image[] images;

	private GameObject[] gameObjects;

	private WaitForSeconds waitSeconds;

	private void OnEnable()
	{
		waitSeconds = new WaitForSeconds(waitTime);
		int childCount = base.transform.childCount;
		if (isImage)
		{
			images = new Image[childCount];
			for (int i = 0; i < childCount; i++)
			{
				images[i] = base.transform.GetChild(i).GetComponent<Image>();
			}
		}
		else
		{
			gameObjects = new GameObject[childCount];
			for (int j = 0; j < childCount; j++)
			{
				gameObjects[j] = base.transform.GetChild(j).gameObject;
			}
		}
		if (isImage)
		{
			StartCoroutine(WaitLiht_Ima());
		}
		else
		{
			StartCoroutine(WaitLiht_GameObject());
		}
	}

	private IEnumerator WaitLiht_Ima()
	{
		while (true)
		{
			for (int i = 0; i < images.Length; i++)
			{
				images[i].enabled = (i % 2 == 0);
			}
			yield return waitSeconds;
			for (int j = 0; j < base.transform.childCount; j++)
			{
				images[j].enabled = (j % 2 != 0);
			}
			yield return waitSeconds;
		}
	}

	private IEnumerator WaitLiht_GameObject()
	{
		while (true)
		{
			for (int i = 0; i < gameObjects.Length; i++)
			{
				gameObjects[i].SetActive(i % 2 == 0);
			}
			yield return waitSeconds;
			for (int j = 0; j < base.transform.childCount; j++)
			{
				gameObjects[j].SetActive(j % 2 != 0);
			}
			yield return waitSeconds;
		}
	}
}
