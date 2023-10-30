using System;
using UnityEngine;

public class PHG_OptionsController : PHG_MB_Singleton<PHG_OptionsController>
{
	private GameObject _goContainer;

	public Action onItemReturn;

	public Action onItemBank;

	public Action onItemSettings;

	public Action onItemRules;

	private int language;

	public bool isShow => _goContainer.activeSelf;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		_goContainer = base.gameObject;
		if (PHG_MB_Singleton<PHG_OptionsController>._instance == null)
		{
			PHG_MB_Singleton<PHG_OptionsController>.SetInstance(this);
			PreInit();
		}
	}

	public void PreInit()
	{
		if (_goContainer == null)
		{
			_goContainer = base.gameObject;
		}
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	public void OnBtnReturn_Click()
	{
		if (!PHG_GVars.tryLockOnePoint)
		{
			PHG_SoundManager.Instance.PlayClickAudio();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!PHG_GVars.tryLockOnePoint)
		{
			PHG_SoundManager.Instance.PlayClickAudio();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (PHG_GVars.tryLockOnePoint)
		{
			return;
		}
		PHG_SoundManager.Instance.PlayClickAudio();
		if (!PHG_LockManager.IsLocked("ScoreBank"))
		{
			Hide();
			if (onItemBank != null)
			{
				onItemBank();
			}
		}
	}

	public void OnBtnRules_Click()
	{
		if (!PHG_GVars.tryLockOnePoint)
		{
			PHG_SoundManager.Instance.PlayClickAudio();
			PHG_MB_Singleton<PHG_MajorGameController>.GetInstance().OnBtnRight_Click(isBg: true);
			Hide();
			UnityEngine.Debug.Log("OnBtnRules_Click");
			if (onItemRules != null)
			{
				onItemRules();
			}
		}
	}

	public void OnBtnBgClose_Click()
	{
		if (!PHG_GVars.tryLockOnePoint)
		{
			Hide();
		}
	}

	public void HideAllPanel()
	{
		Hide();
		PHG_MB_Singleton<PHG_ScoreBank>.GetInstance().Hide();
		PHG_MB_Singleton<PHG_SettingsController>.GetInstance().Hide();
	}
}
