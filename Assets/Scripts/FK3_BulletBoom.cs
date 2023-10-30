using DG.Tweening;
using UnityEngine;

public class FK3_BulletBoom : MonoBehaviour
{
	[HideInInspector]
	public SpriteRenderer SpriteRenderer;

	private Tween DoScale;

	private Tween DOFade;

	public Sprite[] sprites;

	private void OnEnable()
	{
		SpriteRenderer = base.transform.GetComponent<SpriteRenderer>();
		base.transform.localScale = Vector3.one * 110f;
		SpriteRenderer spriteRenderer = SpriteRenderer;
		Color color = SpriteRenderer.color;
		float r = color.r;
		Color color2 = SpriteRenderer.color;
		float g = color2.g;
		Color color3 = SpriteRenderer.color;
		spriteRenderer.color = new Color(r, g, color3.b, 1f);
		if (!base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(value: true);
		}
		DoScale = base.transform.DOScale(120f, 0.1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		DoScale.Play();
		DOFade = SpriteRenderer.DOFade(0f, 2.5f).OnComplete(delegate
		{
			base.gameObject.SetActive(value: false);
		});
		DOFade.Play();
	}

	private void OnDisable()
	{
		base.transform.localScale = Vector3.one * 110f;
		SpriteRenderer.sprite = null;
		DoScale.Kill();
		DOFade.Kill();
	}
}
