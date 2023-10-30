using DG.Tweening;
using UnityEngine;

public class DP_StarAnim : MonoBehaviour
{
	private float interval = 3f;

	public float delay;

	public float endScaleValue;

	public float endRotateValue;

	public float duration;

	private void Start()
	{
		InvokeRepeating("PlayAnim", delay, interval);
	}

	private void PlayAnim()
	{
		base.transform.DOScale(endScaleValue, duration).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
		base.transform.DOBlendableLocalRotateBy(Vector3.back * endRotateValue, duration).SetEase(Ease.Linear).SetLoops(2, LoopType.Incremental);
	}
}
