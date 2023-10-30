using DG.Tweening;
using UnityEngine;

public class FK3_Scene_sqje : FK3_MyScene
{
	public Transform left;

	public Transform right;

	public override void OnBoosEvent()
	{
		left.DOMoveX(-9f, 1.5f).SetEase(Ease.InOutElastic);
		right.DOMoveX(9f, 1.5f).SetEase(Ease.InOutElastic);
	}
}
