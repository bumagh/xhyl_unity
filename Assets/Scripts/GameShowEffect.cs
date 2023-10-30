using DG.Tweening;
using UnityEngine;

public class GameShowEffect : MonoBehaviour
{
	private Vector3 oldScle;

	public float magnification = 1.2f;

	public float time = 0.5f;

	private void Awake()
	{
		oldScle = base.transform.localScale;
	}

	private void OnEnable()
	{
		if (magnification <= 0f)
		{
			magnification = 1.2f;
		}
		base.transform.localScale = oldScle * magnification;
		base.transform.DOScale(oldScle, time);
	}

	private void OnDisable()
	{
		base.transform.localScale = oldScle;
	}
}
