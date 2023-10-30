using System;
using UnityEngine;

public class USW_OptionsController : USW_MB_Singleton<USW_OptionsController>
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
		if (USW_MB_Singleton<USW_OptionsController>._instance == null)
		{
			USW_MB_Singleton<USW_OptionsController>.SetInstance(this);
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
		if (!USW_GVars.tryLockOnePoint)
		{
			USW_SoundManager.Instance.PlayClickAudio();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!USW_GVars.tryLockOnePoint)
		{
			USW_SoundManager.Instance.PlayClickAudio();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (USW_GVars.tryLockOnePoint)
		{
			return;
		}
		USW_SoundManager.Instance.PlayClickAudio();
		if (!USW_LockManager.IsLocked("ScoreBank"))
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
		if (!USW_GVars.tryLockOnePoint)
		{
			USW_SoundManager.Instance.PlayClickAudio();
			USW_MB_Singleton<USW_MajorGameController>.GetInstance().OnBtnRight_Click(isBg: true);
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
		if (!USW_GVars.tryLockOnePoint)
		{
			Hide();
		}
	}

	public void HideAllPanel()
	{
		Hide();
		USW_MB_Singleton<USW_ScoreBank>.GetInstance().Hide();
		USW_MB_Singleton<USW_SettingsController>.GetInstance().Hide();
	}
}
