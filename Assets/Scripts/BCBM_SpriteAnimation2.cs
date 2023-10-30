using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BCBM_SpriteAnimation2 : MonoBehaviour
{
	[Header("中文图片资源链表")]
	public List<Sprite> spritesList_Ch = new List<Sprite>();

	[Header("英文图片资源链表")]
	public List<Sprite> spritesList_En = new List<Sprite>();

	private List<Sprite> spritesList_Temp = new List<Sprite>();

	private Image thisimage;

	[Header("切换延时")]
	[Range(0.01f, 0.5f)]
	public float Sud = 0.1f;

	private void OnEnable()
	{
		base.transform.localScale = Vector3.one;
		thisimage = base.gameObject.GetComponent<Image>();
		spritesList_Temp = ((ZH2_GVars.language_enum != 0) ? spritesList_En : spritesList_Ch);
		StartCoroutine(Switch());
	}

	private IEnumerator Switch()
	{
		int luae = 0;
		while ((bool)base.gameObject)
		{
			thisimage.sprite = spritesList_Temp[luae];
			luae = (luae + 1) % spritesList_Temp.Count;
			yield return new WaitForSeconds(Sud);
		}
	}
}
