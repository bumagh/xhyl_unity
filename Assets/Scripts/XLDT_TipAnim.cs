using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class XLDT_TipAnim : MonoBehaviour
{
	public Image imgTip;

	public Text txtTip;

	public Color imgColor;

	public Color txtColor;

	public float moveValue;

	private Vector3 initPos;

	private Vector3 targetPos;

	private bool bMove;

	private void Awake()
	{
		initPos = base.transform.localPosition;
		targetPos = initPos + Vector3.up * moveValue;
		bMove = false;
	}

	public void Play(string words)
	{
		if (!bMove)
		{
			bMove = true;
			txtTip.text = words;
			base.transform.DOScale(1f, 0.02f).OnComplete(delegate
			{
				txtTip.transform.localScale = Vector3.one;
				imgTip.transform.localScale = Vector3.one;
			});
			base.transform.localPosition = initPos;
			imgTip.color = imgColor;
			txtTip.color = txtColor;
			imgTip.DOFade(0.5f, 1f);
			txtTip.DOFade(0.5f, 1f);
			base.transform.DOLocalMove(targetPos, 1f).OnComplete(delegate
			{
				bMove = false;
				txtTip.transform.localScale = Vector3.zero;
				imgTip.transform.localScale = Vector3.zero;
			});
		}
	}
}
