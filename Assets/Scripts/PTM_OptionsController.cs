using System;
using UnityEngine;

public class PTM_OptionsController : PTM_MB_Singleton<PTM_OptionsController>
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
		if (PTM_MB_Singleton<PTM_OptionsController>._instance == null)
		{
			PTM_MB_Singleton<PTM_OptionsController>.SetInstance(this);
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
		if (!PTM_GVars.tryLockOnePoint)
		{
			PTM_SoundManager.Instance.PlayClickAudio();
			PTM_MB_Singleton<PTM_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!PTM_GVars.tryLockOnePoint)
		{
			PTM_SoundManager.Instance.PlayClickAudio();
			PTM_MB_Singleton<PTM_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (PTM_GVars.tryLockOnePoint)
		{
			return;
		}
		PTM_SoundManager.Instance.PlayClickAudio();
		if (!PTM_LockManager.IsLocked("ScoreBank"))
		{
			PTM_MB_Singleton<PTM_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemBank != null)
			{
				onItemBank();
			}
		}
	}

	public void OnBtnRules_Click()
	{
		if (!PTM_GVars.tryLockOnePoint)
		{
			PTM_SoundManager.Instance.PlayClickAudio();
			PTM_MB_Singleton<PTM_HUDController>.GetInstance().ResetSprite();
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
		if (!PTM_GVars.tryLockOnePoint)
		{
			Hide();
			PTM_MB_Singleton<PTM_HUDController>.GetInstance().ResetSprite();
		}
	}

	public void HideAllPanel()
	{
		PTM_MB_Singleton<PTM_HUDController>.GetInstance().ResetSprite();
		Hide();
		PTM_MB_Singleton<PTM_ScoreBank>.GetInstance().Hide();
		PTM_MB_Singleton<PTM_SettingsController>.GetInstance().Hide();
	}
}
