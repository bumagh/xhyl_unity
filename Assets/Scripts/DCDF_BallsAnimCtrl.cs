using System.Collections;
using UnityEngine;

public class DCDF_BallsAnimCtrl : MonoBehaviour
{
	[SerializeField]
	private Material[] mats = new Material[2];

	private MeshRenderer[] mrs;

	private int index;

	private void Awake()
	{
		mrs = new MeshRenderer[28];
		for (int i = 0; i < 28; i++)
		{
			mrs[i] = base.transform.GetChild(i).GetChild(0).GetComponent<MeshRenderer>();
		}
	}

	private void OnEnable()
	{
		index = 0;
		SetMats(index);
		StartCoroutine("PlayAnim");
	}

	private IEnumerator PlayAnim()
	{
		while (index <= 1)
		{
			yield return new WaitForSeconds(0.2f);
			index++;
			if (index > 1)
			{
				index = 0;
			}
			SetMats(index);
		}
	}

	private void SetMats(int index)
	{
		for (int i = 0; i < 28; i++)
		{
			if (i % 2 == 0)
			{
				mrs[i].material = mats[index];
			}
			else
			{
				mrs[i].material = mats[1 - index];
			}
		}
	}
}
