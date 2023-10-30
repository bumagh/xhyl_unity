using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteButton : MonoBehaviour
{
	[Header("基本缩放比例")]
	public float basicScale = 1f;

	[Header("点击缩放倍率")]
	public float clickZoom = 1.3f;

	[Header("缩放时长")]
	public float scaleDuration = 0.2f;

	public Action onClick;

	public Action onPress;

	public Action onExit;

	public bool log;

	private void Start()
	{
	}

	private void OnValidate()
	{
		base.transform.localScale = Vector3.one * basicScale;
	}

	private void OnMouseDown()
	{
		if (onPress != null)
		{
			onPress();
		}
	}

	private void OnMouseExit()
	{
		UnityEngine.Debug.LogError("=======OnMouseExit" + base.transform.name);
		if (onExit != null)
		{
			onExit();
		}
	}

	private void OnMouseUpAsButton()
	{
		UnityEngine.Debug.LogError(base.gameObject.name + "=========释放鼠标========");
		base.transform.localScale = Vector3.one * basicScale;
		DOTween.Kill(base.transform);
		base.transform.DOScale(basicScale * clickZoom, scaleDuration / 2f).SetLoops(2, LoopType.Yoyo);
		base.transform.GetComponent<Image>().DOColor(new Color(0.7f, 0.7f, 0.7f), 0.1f);
		StartCoroutine(DelayCall(0.2f, delegate
		{
			base.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f);
		}));
		if (onClick != null)
		{
			onClick();
		}
	}

	private IEnumerator DelayCall(float delay, Action call)
	{
		yield return new WaitForSeconds(delay);
		call();
	}
}
