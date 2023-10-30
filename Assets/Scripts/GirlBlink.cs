using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GirlBlink : MonoBehaviour
{
	private Image CloseEyeImg;

	private Color TraColor = Color.white;

	private void Start()
	{
		CloseEyeImg = base.transform.GetComponent<Image>();
		TraColor.a = 0f;
		InvokeRepeating("Blink", 1f, 5f);
	}

	private void Blink()
	{
		CloseEyeImg.DOFade(1f, 0.5f).SetEase(Ease.InOutExpo).OnComplete(delegate
		{
			CloseEyeImg.color = TraColor;
		});
	}
}
