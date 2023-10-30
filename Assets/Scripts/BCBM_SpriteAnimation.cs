using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BCBM_SpriteAnimation : MonoBehaviour
{
	[Header("图片资源链表")]
	public List<Sprite> spritesList = new List<Sprite>();

	private Image thisimage;

	[Range(0.01f, 0.5f)]
	[Header("切换延时")]
	public float Sud = 0.1f;

	private void OnEnable()
	{
		base.transform.localScale = Vector3.one;
		thisimage = base.gameObject.GetComponent<Image>();
		StartCoroutine(Switch());
	}

	private IEnumerator Switch()
	{
		int luae = 0;
		while ((bool)base.gameObject)
		{
			thisimage.sprite = spritesList[luae];
			luae = (luae + 1) % spritesList.Count;
			yield return new WaitForSeconds(Sud);
		}
	}
}
