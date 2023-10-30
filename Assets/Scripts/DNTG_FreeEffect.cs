using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DNTG_FreeEffect : MonoBehaviour
{
	private Image freeIma;

	private Transform freeBoom;

	private void Update()
	{
		if (Application.platform == RuntimePlatform.WindowsEditor && UnityEngine.Input.GetKeyDown(KeyCode.S))
		{
			ShowEff();
		}
		if (Application.platform == RuntimePlatform.WindowsEditor && UnityEngine.Input.GetKeyDown(KeyCode.E))
		{
			EndEff();
		}
	}

	public void ShowEff()
	{
		ResetEff();
		freeBoom.DOScale(Vector3.one * 6f, 0.35f).OnComplete(delegate
		{
			freeBoom.localScale = Vector3.zero;
		});
		freeIma.DOFade(1f, 0.8f);
	}

	public void EndEff()
	{
		freeIma.DOFade(0f, 0.5f);
	}

	public void ResetEff()
	{
		if (freeIma == null)
		{
			freeIma = base.transform.Find("Free").GetComponent<Image>();
		}
		if (freeBoom == null)
		{
			freeBoom = base.transform.Find("Boom");
		}
		freeBoom.localScale = Vector3.zero;
		freeIma.DOFade(0f, 0.02f);
	}
}
