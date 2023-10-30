using System;
using UnityEngine;

public class MSE_OptionsController : MSE_MB_Singleton<MSE_OptionsController>
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
		if (MSE_MB_Singleton<MSE_OptionsController>._instance == null)
		{
			MSE_MB_Singleton<MSE_OptionsController>.SetInstance(this);
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
		if (!MSE_GVars.tryLockOnePoint)
		{
			MSE_SoundManager.Instance.PlayClickAudio();
			MSE_MB_Singleton<MSE_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!MSE_GVars.tryLockOnePoint)
		{
			MSE_SoundManager.Instance.PlayClickAudio();
			MSE_MB_Singleton<MSE_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (MSE_GVars.tryLockOnePoint)
		{
			return;
		}
		MSE_SoundManager.Instance.PlayClickAudio();
		if (!MSE_LockManager.IsLocked("ScoreBank"))
		{
			MSE_MB_Singleton<MSE_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemBank != null)
			{
				onItemBank();
			}
		}
	}

	public void OnBtnRules_Click()
	{
		if (!MSE_GVars.tryLockOnePoint)
		{
			MSE_SoundManager.Instance.PlayClickAudio();
			MSE_MB_Singleton<MSE_HUDController>.GetInstance().ResetSprite();
			MSE_MB_Singleton<MSE_MajorGameController>.GetInstance().OnBtnRight_Click(isBg: true);
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
		if (!MSE_GVars.tryLockOnePoint)
		{
			Hide();
			MSE_MB_Singleton<MSE_HUDController>.GetInstance().ResetSprite();
		}
	}

	public void HideAllPanel()
	{
		MSE_MB_Singleton<MSE_HUDController>.GetInstance().ResetSprite();
		Hide();
		MSE_MB_Singleton<MSE_ScoreBank>.GetInstance().Hide();
		MSE_MB_Singleton<MSE_SettingsController>.GetInstance().Hide();
	}
}
