using DG.Tweening;
using UnityEngine;

public class Scene_bwx : MyScene
{
	public Transform store;

	private Tweener tw;

	private float scaleMin = 0.95f;

	private float scaleMax = 1f;

	private void Start()
	{
		reduce();
	}

	private void reduce()
	{
		tw = store.DOScale(scaleMin, 3f).OnComplete(enlarge);
	}

	private void enlarge()
	{
		tw = store.DOScale(scaleMax, 3f).OnComplete(reduce);
	}

	public override void OnBoosEvent()
	{
		scaleMax = 0.9f;
		scaleMin = 0.85f;
		if (tw != null)
		{
			tw.Kill();
			reduce();
		}
	}
}
