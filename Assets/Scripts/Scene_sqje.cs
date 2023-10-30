using DG.Tweening;
using UnityEngine;

public class Scene_sqje : MyScene
{
	public Transform left;

	public Transform right;

	public override void OnBoosEvent()
	{
		left.DOMoveX(-9f, 1.5f).SetEase(Ease.InOutElastic);
		right.DOMoveX(9f, 1.5f).SetEase(Ease.InOutElastic);
	}
}
