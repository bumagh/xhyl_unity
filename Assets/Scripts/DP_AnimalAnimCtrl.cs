using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DP_AnimalAnimCtrl : MonoBehaviour
{
	private Vector3 posCenter = Vector3.up * 1.8f;

	private Vector3 posInit;

	private Vector3 posParentInit;

	private Vector3 rotParentInit;

	private Animator anim;

	private Transform tfAnimal;

	private WaitForSeconds waitGoCenter = new WaitForSeconds(0.5f);

	private WaitForSeconds waitPlayWin = new WaitForSeconds(1.5f);

	private WaitForSeconds waitGoBack = new WaitForSeconds(8f);

	public void Init()
	{
		anim = base.transform.GetComponent<Animator>();
		tfAnimal = base.transform.Find("ScenePref/Animals").GetChild(0).GetChild(0);
		anim.Play("Idle");
		posInit = tfAnimal.localPosition;
		rotParentInit = tfAnimal.parent.localEulerAngles;
		posParentInit = tfAnimal.parent.localPosition;
	}

	public IEnumerator AnimGoCenter()
	{
		if (base.gameObject.CompareTag("monkey"))
		{
			anim.Play("Dance");
		}
		yield return waitGoCenter;
		Transform parent = tfAnimal.parent;
		Vector3 up = Vector3.up;
		Vector3 localEulerAngles = base.transform.parent.parent.localEulerAngles;
		parent.DOLocalRotate(up * (180f - localEulerAngles.y), 1.2f);
		tfAnimal.parent.DOLocalMove(posCenter, 1.5f);
		yield return waitPlayWin;
		if (!base.gameObject.CompareTag("monkey"))
		{
			anim.Play("Dance");
		}
		yield return waitGoBack;
		AnimGoBack();
	}

	private void AnimGoBack()
	{
		MonoBehaviour.print("GoBack");
		tfAnimal.parent.DOLocalRotate(rotParentInit, 1.2f).SetDelay(0.3f);
		tfAnimal.parent.DOLocalMove(posParentInit, 1.5f).OnComplete(Reset);
	}

	public void Reset()
	{
		anim.Play("Idle");
		DOTween.Kill(tfAnimal);
		DOTween.Kill(tfAnimal.parent);
		tfAnimal.localPosition = posInit;
		tfAnimal.parent.localEulerAngles = rotParentInit;
	}
}
