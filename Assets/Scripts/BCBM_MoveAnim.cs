using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCBM_MoveAnim : MonoBehaviour
{
	public List<Transform> moveTr = new List<Transform>();

	public List<Vector3> vector3s = new List<Vector3>();

	private void OnEnable()
	{
		moveTr = new List<Transform>();
		vector3s = new List<Vector3>();
		ZH2_GVars.moveTime = 0.7f;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			moveTr.Add(base.transform.GetChild(i));
		}
		for (int j = 0; j < moveTr.Count; j++)
		{
			vector3s.Add(moveTr[j].localPosition);
		}
		StartCoroutine(WaitMove());
		StartCoroutine(WaitTime());
	}

	private IEnumerator WaitTime()
	{
		while (true)
		{
			if (ZH2_GVars.moveTime > 0.15f)
			{
				ZH2_GVars.moveTime -= 0.05f;
				if (ZH2_GVars.moveTime <= 0.15f)
				{
					break;
				}
			}
			yield return new WaitForSeconds(0.25f);
		}
		ZH2_GVars.moveTime = 0.15f;
	}

	private IEnumerator WaitMove()
	{
		while (true)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				if (i < base.transform.childCount - 1)
				{
					Transform child2 = base.transform.GetChild(i + 1);
					child.DOLocalMove(child2.localPosition, ZH2_GVars.moveTime);
					child.DOScale(child2.localScale, ZH2_GVars.moveTime);
				}
				else
				{
					child.localPosition = vector3s[0];
					child.localScale = Vector3.zero;
				}
			}
			base.transform.GetChild(base.transform.childCount - 1).SetAsFirstSibling();
			yield return new WaitForSeconds(ZH2_GVars.moveTime);
		}
	}
}
