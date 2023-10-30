using DG.Tweening;
using UnityEngine;

public class WHN_SelectTableController : MonoBehaviour
{
	public static WHN_SelectTableController instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Start()
	{
		SelectTableAniMove();
	}

	private void SelectTableAniMove()
	{
		Tweener t = base.transform.DOLocalMove(Vector3.up * 30f, 0.5f);
		t.SetLoops(2, LoopType.Yoyo);
		Sequence myseq = DOTween.Sequence();
		myseq.Join(t);
		myseq.onComplete = delegate
		{
			base.transform.DOLocalMove(Vector3.up * 80f, 0.5f);
			myseq.onKill = delegate
			{
				SelectTableAniMove();
			};
		};
	}
}
