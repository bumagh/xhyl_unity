using DG.Tweening;
using System;
using UnityEngine;

public class FK3_Scene_ayjs : FK3_MyScene
{
	public class test
	{
		public int a;

		public int b = 1;
	}

	public SpriteRenderer spriteBg;

	public Transform left;

	public Transform right;

	public float speed = 0.1f;

	private float _wight;

	private Vector3 _originPos;

	private Vector3 _leftpos;

	private void Start()
	{
		_wight = spriteBg.sprite.rect.width / spriteBg.sprite.pixelsPerUnit;
		_originPos = right.position;
		_leftpos = left.position;
	}

	public override void OnBoosEvent()
	{
		Move2Right(left, OnComplete);
		Move2Right(right, OnComplete);
	}

	private void Move2Right(Transform bg, Action<Transform> complete)
	{
		float num = _originPos.x + _wight;
		Vector3 position = bg.position;
		float num2 = Mathf.Abs(position.x - num);
		bg.DOMoveX(num, num2 / speed).OnComplete(delegate
		{
			complete(bg);
		}).SetEase(Ease.Linear);
	}

	private void OnComplete(Transform bg)
	{
		bg.position = new Vector3(_leftpos.x, _leftpos.y, _leftpos.z);
		Move2Right(bg, OnComplete);
	}
}
