using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STWM_DiceNPCsController : MonoBehaviour
{
	[SerializeField]
	private STWM_NPCAnimator leftNPC;

	[SerializeField]
	private STWM_NPCAnimator midNPC;

	[SerializeField]
	private STWM_NPCAnimator rightNPC;

	[SerializeField]
	private Transform tfTable;

	[SerializeField]
	private Transform tfGoldLeft;

	[SerializeField]
	private Transform tfGoldRight;

	private Dictionary<string, bool> dicAniEvent = new Dictionary<string, bool>();

	public IEnumerator Shake(float delay = 0f)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		STWM_SoundManager.Instance.PlayShakeDiceAudio();
		midNPC.Play("shake");
		leftNPC.Play("cheer");
		rightNPC.Play("cheer");
		yield return new WaitForSeconds(1.5f);
		ShakeTableAndGold();
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(STWM_SoundManager.Instance.PlayNPCsDefualtAudio());
	}

	public IEnumerator TouchMustache()
	{
		int frequency = 0;
		do
		{
			frequency++;
			if (frequency < 2)
			{
				yield return new WaitForSeconds(2f);
				if (!STWM_SoundManager.bShouldStop)
				{
					midNPC.Play("touchMustache");
				}
			}
			else if (frequency == 2)
			{
				yield return new WaitForSeconds(5.5f);
				if (!STWM_SoundManager.bShouldStop)
				{
					midNPC.Play("touchMustache");
				}
			}
			else if (frequency == 3)
			{
				yield return new WaitForSeconds(4f);
				if (!STWM_SoundManager.bShouldStop)
				{
					midNPC.Play("touchDiceCup");
				}
			}
			else if (frequency > 3 && frequency % 2 == 0)
			{
				yield return new WaitForSeconds(7f);
				if (!STWM_SoundManager.bShouldStop)
				{
					midNPC.Play("touchDiceCup");
				}
			}
			else if (frequency > 3 && frequency % 2 == 1)
			{
				yield return new WaitForSeconds(5f);
				if (!STWM_SoundManager.bShouldStop)
				{
					midNPC.Play("touchMustache");
				}
			}
			yield return null;
		}
		while (!STWM_SoundManager.bShouldStop);
	}

	public IEnumerator Open(STWM_DiceGameResultType result)
	{
		midNPC.Play("open");
		switch (result)
		{
		case STWM_DiceGameResultType.Lose:
			yield return new WaitForSeconds(1f);
			midNPC.Play("win");
			leftNPC.Play("fail");
			rightNPC.Play("fail");
			STWM_SoundManager.Instance.PlayDiceLooseAudio();
			break;
		case STWM_DiceGameResultType.Win:
			yield return new WaitForSeconds(1f);
			midNPC.Play("fail");
			leftNPC.Play("win");
			rightNPC.Play("win");
			STWM_SoundManager.Instance.PlayDiceWinAudio();
			break;
		case STWM_DiceGameResultType.Overflow:
			yield return new WaitForSeconds(1f);
			UnityEngine.Debug.Log("Overflow");
			midNPC.Play("fail");
			leftNPC.Play("win");
			rightNPC.Play("win");
			STWM_SoundManager.Instance.PlayDiceWinAudio();
			yield return new WaitForSeconds(3.1f);
			STWM_SoundManager.Instance.PlayDiceOverFlowAudio();
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
