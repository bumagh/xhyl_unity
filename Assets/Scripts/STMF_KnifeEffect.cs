using DG.Tweening;
using UnityEngine;

public class STMF_KnifeEffect : MonoBehaviour
{
	private int childCount;

	private SpriteRenderer[] sr;

	private void Awake()
	{
		childCount = base.transform.childCount;
		sr = new SpriteRenderer[childCount];
		for (int i = 0; i < childCount; i++)
		{
			float d = 360f / (float)base.transform.childCount;
			base.transform.GetChild(i).localEulerAngles = Vector3.forward * d * i;
			sr[i] = base.transform.GetChild(i).GetComponent<SpriteRenderer>();
			sr[i].sortingOrder = 644;
		}
	}

	public void PlayEffect(Vector3 pos)
	{
		base.transform.position = pos;
		for (int i = 0; i < childCount; i++)
		{
			float d = 360f / (float)childCount;
			base.transform.GetChild(i).localPosition = Vector3.zero;
			base.transform.GetChild(i).localEulerAngles = Vector3.forward * d * i;
			Quaternion localRotation = base.transform.GetChild(i).localRotation;
			sr[i].enabled = true;
			base.transform.GetChild(i).DOKill();
			if (i == 0)
			{
				base.transform.GetChild(i).DOLocalMove(localRotation * Vector3.right * 7f, 2f).SetEase(Ease.Linear)
					.OnComplete(_hide);
			}
			else
			{
				base.transform.GetChild(i).DOLocalMove(localRotation * Vector3.right * 7f, 2f).SetEase(Ease.Linear);
			}
		}
	}

	private void _hide()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			sr[i].enabled = false;
			base.transform.GetChild(i).DOKill();
		}
		STMF_EffectMngr.GetSingleton().DestroyEffectObj(base.gameObject);
	}
}
