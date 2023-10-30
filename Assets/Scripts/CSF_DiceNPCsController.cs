using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSF_DiceNPCsController : MonoBehaviour
{
	private CSF_NPCAnimator leftNPC;

	private CSF_NPCAnimator midNPC;

	private CSF_NPCAnimator rightNPC;

	private Transform tfTable;

	private Transform tfGoldLeft;

	private Transform tfGoldRight;

	private Dictionary<string, bool> dicAniEvent = new Dictionary<string, bool>();

	private void Awake()
	{
		leftNPC = base.transform.Find("LeftPerson").GetComponent<CSF_NPCAnimator>();
		midNPC = base.transform.Find("MidPerson").GetComponent<CSF_NPCAnimator>();
		rightNPC = base.transform.Find("RightPerson").GetComponent<CSF_NPCAnimator>();
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
		CSF_SoundManager.Instance.PlayShakeDiceAudio();
		midNPC.Play("shake");
		leftNPC.Play("cheer");
		rightNPC.Play("cheer");
		yield return new WaitForSeconds(1.5f);
		ShakeTableAndGold();
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(CSF_SoundManager.Instance.PlayNPCsDefualtAudio());
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
				if (!CSF_SoundManager.bShouldStop)
				{
					midNPC.Play("touchMustache");
				}
			}
			else if (frequency == 2)
			{
				yield return new WaitForSeconds(5.5f);
				if (!CSF_SoundManager.bShouldStop)
				{
					midNPC.Play("touchMustache");
				}
			}
			else if (frequency == 3)
			{
				yield return new WaitForSeconds(4f);
				if (!CSF_SoundManager.bShouldStop)
				{
					midNPC.Play("touchDiceCup");
				}
			}
			else if (frequency > 3 && frequency % 2 == 0)
			{
				yield return new WaitForSeconds(7f);
				if (!CSF_SoundManager.bShouldStop)
				{
					midNPC.Play("touchDiceCup");
				}
			}
			else if (frequency > 3 && frequency % 2 == 1)
			{
				yield return new WaitForSeconds(5f);
				if (!CSF_SoundManager.bShouldStop)
				{
					midNPC.Play("touchMustache");
				}
			}
			yield return null;
		}
		while (!CSF_SoundManager.bShouldStop);
	}

	public IEnumerator Open(CSF_DiceGameResultType result)
	{
		midNPC.Play("open");
		switch (result)
		{
		case CSF_DiceGameResultType.Lose:
			yield return new WaitForSeconds(1f);
			midNPC.Play("win");
			leftNPC.Play("fail");
			rightNPC.Play("fail");
			CSF_SoundManager.Instance.PlayDiceLooseAudio();
			break;
		case CSF_DiceGameResultType.Win:
			yield return new WaitForSeconds(1f);
			midNPC.Play("fail");
			leftNPC.Play("win");
			rightNPC.Play("win");
			CSF_SoundManager.Instance.PlayDiceWinAudio();
			break;
		case CSF_DiceGameResultType.Overflow:
			yield return new WaitForSeconds(1f);
			UnityEngine.Debug.Log("Overflow");
			midNPC.Play("fail");
			leftNPC.Play("win");
			rightNPC.Play("win");
			CSF_SoundManager.Instance.PlayDiceWinAudio();
			yield return new WaitForSeconds(3.1f);
			CSF_SoundManager.Instance.PlayDiceOverFlowAudio();
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
