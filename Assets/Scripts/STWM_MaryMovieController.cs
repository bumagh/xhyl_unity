using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class STWM_MaryMovieController : STWM_MB_Singleton<STWM_MaryMovieController>
{
	public Action OnMiddle;

	public Action OnEnd;

	public bool AutoPlay;

	[SerializeField]
	private STWM_SpiAnim animDragon;

	[SerializeField]
	private Transform _leftFlag;

	private Vector3 _leftFlagInitPos;

	[SerializeField]
	private Transform _rightFlag;

	private Vector3 _rightFlagInitPos;

	private Coroutine _coroutine;

	[SerializeField]
	private STWM_RulePicController ruleController;

	private void Awake()
	{
		if (!(STWM_MB_Singleton<STWM_MaryMovieController>._instance != null))
		{
			STWM_MB_Singleton<STWM_MaryMovieController>.SetInstance(this);
			PreInit();
		}
	}

	public void PreInit()
	{
		_leftFlagInitPos = _leftFlag.localPosition;
		_rightFlagInitPos = _rightFlag.localPosition;
	}

	private void Start()
	{
		if (AutoPlay)
		{
			Play();
		}
	}

	public void Play()
	{
		HidePanels();
		base.gameObject.SetActive(value: true);
		animDragon.gameObject.SetActive(value: true);
		_leftFlag.localPosition = _leftFlagInitPos;
		_rightFlag.localPosition = _rightFlagInitPos;
		STWM_SoundManager.Instance.PlayEnterMaryAudio();
		_coroutine = StartCoroutine(_doPlay());
	}

	public void HidePanels()
	{
		STWM_MB_Singleton<STWM_OptionsController>.GetInstance().HideAllPanel();
		ruleController.Hide();
	}

	public void ForceStop()
	{
		if (_coroutine != null)
		{
			STWM_SoundManager.Instance.StopMajorAudio();
			StopCoroutine(_coroutine);
			_coroutine = null;
			base.gameObject.SetActive(value: false);
		}
	}

	public void Hide()
	{
		ForceStop();
	}

	private IEnumerator _doPlay()
	{
		animDragon.PlayOnce(bActive: true);
		yield return new WaitForSeconds(1.5f);
		float flagTime = 3f;
		_leftFlag.DOLocalMoveX(1280f, flagTime);
		_rightFlag.DOLocalMoveX(-1280f, flagTime);
		yield return new WaitForSeconds(flagTime / 2f);
		if (OnMiddle != null)
		{
			OnMiddle();
		}
		animDragon.gameObject.SetActive(value: false);
		yield return new WaitForSeconds(flagTime / 2f);
		_coroutine = null;
		base.gameObject.SetActive(value: false);
		if (OnEnd != null)
		{
			OnEnd();
		}
	}
}
