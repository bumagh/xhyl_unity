using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_AnimStript : MonoBehaviour
{
	public List<Sprite> sprites;

	private Image image;

	private int index;

	public float wiatTime;

	private Coroutine playAnim;

	private void OnEnable()
	{
		image = GetComponent<Image>();
		if (playAnim != null)
		{
			StopCoroutine(playAnim);
		}
		playAnim = StartCoroutine(PlayAnim());
	}

	private IEnumerator PlayAnim()
	{
		while (true)
		{
			if (image != null && sprites.Count >= 1)
			{
				index++;
				if (index >= sprites.Count)
				{
					index = 0;
				}
				image.sprite = sprites[index];
				yield return new WaitForSeconds(wiatTime);
			}
		}
	}

	private void OnDisable()
	{
		if (playAnim != null)
		{
			StopCoroutine(playAnim);
		}
	}
}
