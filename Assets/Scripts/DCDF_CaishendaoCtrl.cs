using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

public class DCDF_CaishendaoCtrl : MonoBehaviour
{
	[SerializeField]
	private Animator anim;

	[SerializeField]
	private GameObject objYuanbao;

	private Vector3[] path1 = new Vector3[3]
	{
		new Vector3(11f, 1f, -5f),
		new Vector3(4.5f, -0.5f, -5f),
		new Vector3(0f, -1f, -5f)
	};

	private Vector3[] path2 = new Vector3[3]
	{
		new Vector3(0f, -1f, -5f),
		new Vector3(-4.5f, -3f, -5f),
		new Vector3(-11f, 0f, -5f)
	};

	private TweenerCore<Vector3, Path, PathOptions> _TC_Path;

	public void Move()
	{
		objYuanbao.SetActive(value: true);
		anim.Play("Empty");
		_TC_Path = base.transform.DOPath(path1, 1f, PathType.CatmullRom).SetEase(Ease.Linear);
		base.transform.DOLocalRotate(Vector3.zero, 1f);
		_TC_Path.Play();
		StartCoroutine("PlayAnim");
	}

	private void Move2()
	{
		_TC_Path = base.transform.DOPath(path2, 1f, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(delegate
		{
			base.transform.position = path1[0];
		});
		_TC_Path.Play();
	}

	private IEnumerator PlayAnim()
	{
		yield return new WaitForSeconds(1f);
		anim.Play("Move");
		DCDF_SoundManager.Instance.PlayLaughAudio();
		yield return new WaitForSeconds(3.5f);
		objYuanbao.SetActive(value: false);
		DCDF_SoundManager.Instance.PlayExplodeAudio();
		base.transform.DOLocalRotate(Vector3.up * 45f, 1f).OnComplete(Move2);
	}
}
