using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class BCBM_TNvLangCor : MonoBehaviour
{
	public SkeletonGraphic tuNvLang;

	public Transform tuNvLangLeftOldPos;

	public Transform tVLMindelPos;

	public Transform tVLRightOldPos;

	public Transform VLRightEndPos;

	public Image startBet;

	public Image endBet;

	public Material material;

	private void OnEnable()
	{
		material = Resources.Load<Material>("SkeletonGraphicDefault");
		tuNvLang.material = material;
	}

	public void StartBet()
	{
		BCBM_Audio.publicAudio.PlayBg(2, 0.6f);
		BCBM_Audio.publicAudio.EndBet(isEndBet: false);
		BCBM_BetScene.publicBetScene.ShowTips(1);
		tuNvLang.color = new Color(1f, 1f, 1f, 0f);
		startBet.color = new Color(1f, 1f, 1f, 0f);
		endBet.color = new Color(1f, 1f, 1f, 0f);
		tuNvLang.transform.localScale = new Vector3(-1f, 1f, 1f);
		tuNvLang.transform.localPosition = tuNvLangLeftOldPos.localPosition;
		tuNvLang.DOFade(1f, 0.25f);
		startBet.DOFade(1f, 0.25f);
		tuNvLang.transform.DOLocalMove(tVLMindelPos.localPosition, 0.35f).OnComplete(delegate
		{
			startBet.transform.DOScale(new Vector3(-1.25f, 1.25f, 1.25f), 0.3f).OnComplete(delegate
			{
				startBet.transform.DOScale(new Vector3(-1f, 1f, 1f), 0.15f).OnComplete(delegate
				{
					startBet.DOFade(0f, 0.15f);
				});
			});
			tuNvLang.DOFade(1f, 0.5f).OnComplete(delegate
			{
				tuNvLang.DOFade(0f, 0.45f);
				tuNvLang.transform.DOLocalMove(tVLRightOldPos.localPosition, 0.45f);
			});
		});
	}

	public void EndBet()
	{
		BCBM_Audio.publicAudio.EndBet(isEndBet: true);
		BCBM_Audio.publicAudio.PlayHongMIngStart();
		BCBM_BetScene.publicBetScene.ShowTips(2);
		tuNvLang.color = new Color(1f, 1f, 1f, 0f);
		startBet.color = new Color(1f, 1f, 1f, 0f);
		endBet.color = new Color(1f, 1f, 1f, 0f);
		tuNvLang.transform.localScale = Vector3.one;
		tuNvLang.transform.localPosition = tVLRightOldPos.localPosition;
		tuNvLang.DOFade(1f, 0.25f);
		endBet.DOFade(1f, 0.25f);
		tuNvLang.transform.DOLocalMove(tVLMindelPos.localPosition, 0.25f).OnComplete(delegate
		{
			endBet.transform.DOScale(Vector3.one * 1.25f, 0.3f).OnComplete(delegate
			{
				endBet.transform.DOScale(Vector3.one, 0.15f).OnComplete(delegate
				{
					endBet.DOFade(0f, 0.15f);
				});
			});
			tuNvLang.DOFade(1f, 0.5f).OnComplete(delegate
			{
				tuNvLang.DOFade(0f, 0.25f);
				tuNvLang.transform.DOLocalMove(VLRightEndPos.localPosition, 0.25f);
			});
		});
	}
}
