using System;
using System.Collections;
using UnityEngine;

public class USW_MaryMovieController : USW_MB_Singleton<USW_MaryMovieController>
{
	public Action OnMiddle;

	public Action OnEnd;

	private Animator animDragon;

	private Coroutine _coroutine;

	private USW_RulePicController ruleController;

	private void Awake()
	{
		animDragon = base.transform.Find("Dragon").GetComponent<Animator>();
		ruleController = base.transform.parent.Find("Rule").GetComponent<USW_RulePicController>();
		if (!(USW_MB_Singleton<USW_MaryMovieController>._instance != null))
		{
			USW_MB_Singleton<USW_MaryMovieController>.SetInstance(this);
		}
	}

	private void Start()
	{
	}

	public void Play()
	{
		HidePanels();
		base.gameObject.SetActive(value: true);
		animDragon.gameObject.SetActive(value: true);
		USW_SoundManager.Instance.PlayEnterMaryAudio();
		_coroutine = StartCoroutine(_doPlay());
	}

	public void HidePanels()
	{
		USW_MB_Singleton<USW_OptionsController>.GetInstance().HideAllPanel();
		if (ruleController == null)
		{
			ruleController = base.transform.parent.Find("Rule").GetComponent<USW_RulePicController>();
		}
		if (ruleController != null)
		{
			ruleController.Hide();
		}
	}

	public void ForceStop()
	{
		if (_coroutine != null)
		{
			USW_SoundManager.Instance.StopMajorAudio();
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
		animDragon.Play("MaryMovie");
		yield return new WaitForSeconds(1.5f);
		float flagTime = 4f;
		yield return new WaitForSeconds(flagTime / 2f);
		if (OnMiddle != null)
		{
			OnMiddle();
		}
		USW_MB_Singleton<USW_MajorGameController>.GetInstance().Auto_FreenGame();
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
