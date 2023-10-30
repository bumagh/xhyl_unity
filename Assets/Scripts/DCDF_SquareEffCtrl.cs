using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DCDF_SquareEffCtrl : MonoBehaviour
{
	private bool bFlash;

	private Color colSta = new Color(1f, 0.9f, 1f, 1f);

	private Color colEnd = new Color(0.96f, 0.78f, 0.96f, 0.78f);

	[SerializeField]
	private Image[] imgs = new Image[4];

	public void Play()
	{
		bFlash = true;
		StartCoroutine("FlashAnim");
	}

	public void Stop()
	{
		bFlash = false;
		StopCoroutine("FlashAnim");
		SetChildrenActive(bActive: false);
	}

	private IEnumerator FlashAnim()
	{
		while (bFlash)
		{
			SetChildrenActive(bActive: true);
			ColorAnim();
			yield return new WaitForSeconds(0.3f);
			SetChildrenActive(bActive: false);
			yield return new WaitForSeconds(0.3f);
		}
	}

	private void SetChildrenActive(bool bActive)
	{
		for (int i = 0; i < 4; i++)
		{
			imgs[i].color = colSta;
			imgs[i].gameObject.SetActive(bActive);
		}
	}

	private void ColorAnim()
	{
		for (int i = 0; i < 4; i++)
		{
			int index = i;
			imgs[index].DOColor(colEnd, 0.15f).OnComplete(delegate
			{
				imgs[index].DOColor(colSta, 0.15f);
			});
		}
	}
}
