using System;
using UnityEngine;

public class Yhua : MonoBehaviour
{
	private float fl;

	private void Update()
	{
		fl += Time.deltaTime;
		if (fl > 3f)
		{
			fl = 0f;
			Clear();
		}
	}

	private void Clear()
	{
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll<Material>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = null;
		}
		UnityEngine.Object[] array2 = Resources.FindObjectsOfTypeAll<Texture>();
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = null;
		}
		UnityEngine.Object[] array3 = Resources.FindObjectsOfTypeAll<AudioClip>();
		for (int k = 0; k < array3.Length; k++)
		{
			array3[k] = null;
		}
		UnityEngine.Object[] array4 = Resources.FindObjectsOfTypeAll<Sprite>();
		for (int l = 0; l < array4.Length; l++)
		{
			array4[l] = null;
		}
		Resources.UnloadUnusedAssets();
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}

	private void OnDestroy()
	{
		Clear();
	}
}
