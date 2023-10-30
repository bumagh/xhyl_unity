using DG.Tweening;
using System;
using UnityEngine;

public class Tween_SlowAction : MonoBehaviour
{
	private Transform container;

	private void GetContainer()
	{
		if (container == null)
		{
			container = base.transform.Find("Container");
		}
		if (container == null)
		{
			container = base.transform.GetChild(0);
		}
		if (container == null)
		{
			container = base.transform;
		}
	}

	public void Show()
	{
		GetContainer();
		container.localScale = Vector3.zero;
		container.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(delegate
		{
			container.DOScale(Vector3.one, 0.1f);
		});
	}

	public void Show(Transform transform)
	{
		transform.gameObject.SetActive(value: true);
		transform.localScale = Vector3.zero;
		transform.DOScale(Vector3.one * 1.2f, 0.2f).OnComplete(delegate
		{
			transform.DOScale(Vector3.one, 0.1f);
		});
	}

	public void Hide(GameObject @object = null)
	{
		GetContainer();
		container.localScale = Vector3.one;
		container.DOScale(Vector3.one * 1.15f, 0.1f).OnComplete(delegate
		{
			container.DOScale(Vector3.zero, 0.2f).OnComplete(delegate
			{
				if (@object != null)
				{
					@object.SetActive(value: false);
				}
			});
		});
	}

	public void Hide(Action action, GameObject @object = null)
	{
		GetContainer();
		container.localScale = Vector3.one;
		container.DOScale(Vector3.one * 1.15f, 0.1f).OnComplete(delegate
		{
			container.DOScale(Vector3.zero, 0.2f).OnComplete(delegate
			{
				if (@object != null)
				{
					@object.SetActive(value: false);
					action?.Invoke();
				}
			});
		});
	}

	public void Hide(Transform transform, GameObject @object = null)
	{
		transform.localScale = Vector3.one;
		transform.DOScale(Vector3.one * 1.15f, 0.1f).OnComplete(delegate
		{
			transform.DOScale(Vector3.zero, 0.2f).OnComplete(delegate
			{
				if (@object != null)
				{
					@object.SetActive(value: false);
				}
			});
		});
	}
}
