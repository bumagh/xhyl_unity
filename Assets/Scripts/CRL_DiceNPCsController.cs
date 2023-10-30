using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRL_DiceNPCsController : MonoBehaviour
{
	private Transform tfTable;

	private Transform tfGoldLeft;

	private Transform tfGoldRight;

	private Dictionary<string, bool> dicAniEvent = new Dictionary<string, bool>();

	private void Awake()
	{
		tfTable = base.transform.Find("TableBg");
		tfGoldLeft = base.transform.Find("ImgGoldLeft");
		tfGoldRight = base.transform.Find("ImgGoldRight");
	}

	public IEnumerator Shake(float delay = 0f)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		CRL_SoundManager.Instance.PlayShakeDiceAudio();
		yield return new WaitForSeconds(1.5f);
		ShakeTableAndGold();
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(CRL_SoundManager.Instance.PlayNPCsDefualtAudio());
	}

	public IEnumerator Open(CRL_DiceGameResultType result)
	{
		switch (result)
		{
		case CRL_DiceGameResultType.Lose:
			yield return new WaitForSeconds(1f);
			CRL_SoundManager.Instance.PlayDiceLooseAudio();
			break;
		case CRL_DiceGameResultType.Win:
			yield return new WaitForSeconds(1f);
			CRL_SoundManager.Instance.PlayDiceWinAudio();
			break;
		case CRL_DiceGameResultType.Overflow:
			yield return new WaitForSeconds(1f);
			CRL_SoundManager.Instance.PlayDiceWinAudio();
			yield return new WaitForSeconds(3.1f);
			CRL_SoundManager.Instance.PlayDiceOverFlowAudio();
			break;
		}
	}

	private void ShakeTableAndGold()
	{
		tfTable.DOLocalMoveY(-155f, 0.05f).OnComplete(delegate
		{
			tfTable.DOLocalMoveY(-150f, 0.05f);
		});
		tfGoldLeft.DOLocalMoveY(-80f, 0.05f).OnComplete(delegate
		{
			tfGoldLeft.DOLocalMoveY(-85f, 0.05f);
		});
		tfGoldRight.DOLocalMoveY(-80f, 0.05f).OnComplete(delegate
		{
			tfGoldRight.DOLocalMoveY(-85f, 0.05f);
		});
	}
}
