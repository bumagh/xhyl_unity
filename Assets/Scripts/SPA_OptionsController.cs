using System;
using UnityEngine;

public class SPA_OptionsController : SPA_MB_Singleton<SPA_OptionsController>
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
		if (SPA_MB_Singleton<SPA_OptionsController>._instance == null)
		{
			SPA_MB_Singleton<SPA_OptionsController>.SetInstance(this);
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
		if (!SPA_GVars.tryLockOnePoint)
		{
			SPA_SoundManager.Instance.PlayClickAudio();
			SPA_MB_Singleton<SPA_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!SPA_GVars.tryLockOnePoint)
		{
			SPA_SoundManager.Instance.PlayClickAudio();
			SPA_MB_Singleton<SPA_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (SPA_GVars.tryLockOnePoint)
		{
			return;
		}
		SPA_SoundManager.Instance.PlayClickAudio();
		if (!SPA_LockManager.IsLocked("ScoreBank"))
		{
			SPA_MB_Singleton<SPA_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemBank != null)
			{
				onItemBank();
			}
		}
	}

	public void OnBtnRules_Click()
	{
		if (!SPA_GVars.tryLockOnePoint)
		{
			SPA_SoundManager.Instance.PlayClickAudio();
			SPA_MB_Singleton<SPA_HUDController>.GetInstance().ResetSprite();
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
		if (!SPA_GVars.tryLockOnePoint)
		{
			Hide();
			SPA_MB_Singleton<SPA_HUDController>.GetInstance().ResetSprite();
		}
	}

	public void HideAllPanel()
	{
		SPA_MB_Singleton<SPA_HUDController>.GetInstance().ResetSprite();
		Hide();
		SPA_MB_Singleton<SPA_ScoreBank>.GetInstance().Hide();
		SPA_MB_Singleton<SPA_SettingsController>.GetInstance().Hide();
	}
}
