using System;
using UnityEngine;

public class WHN_OptionsController : WHN_MB_Singleton<WHN_OptionsController>
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
		if (WHN_MB_Singleton<WHN_OptionsController>._instance == null)
		{
			WHN_MB_Singleton<WHN_OptionsController>.SetInstance(this);
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
		if (!WHN_GVars.tryLockOnePoint)
		{
			WHN_SoundManager.Instance.PlayClickAudio();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!WHN_GVars.tryLockOnePoint)
		{
			WHN_SoundManager.Instance.PlayClickAudio();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (WHN_GVars.tryLockOnePoint)
		{
			return;
		}
		WHN_SoundManager.Instance.PlayClickAudio();
		if (!WHN_LockManager.IsLocked("ScoreBank"))
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
		if (!WHN_GVars.tryLockOnePoint)
		{
			WHN_SoundManager.Instance.PlayClickAudio();
			WHN_MB_Singleton<WHN_MajorGameController>.GetInstance().OnBtnRight_Click(isBg: true);
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
		if (!WHN_GVars.tryLockOnePoint)
		{
			Hide();
		}
	}

	public void HideAllPanel()
	{
		Hide();
		WHN_MB_Singleton<WHN_ScoreBank>.GetInstance().Hide();
		WHN_MB_Singleton<WHN_SettingsController>.GetInstance().Hide();
	}
}
