using System;
using UnityEngine;

public class LLD_OptionsController : LLD_MB_Singleton<LLD_OptionsController>
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
		if (LLD_MB_Singleton<LLD_OptionsController>._instance == null)
		{
			LLD_MB_Singleton<LLD_OptionsController>.SetInstance(this);
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
		if (!LLD_GVars.tryLockOnePoint)
		{
			LLD_SoundManager.Instance.PlayClickAudio();
			LLD_MB_Singleton<LLD_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!LLD_GVars.tryLockOnePoint)
		{
			LLD_SoundManager.Instance.PlayClickAudio();
			LLD_MB_Singleton<LLD_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (LLD_GVars.tryLockOnePoint)
		{
			return;
		}
		LLD_SoundManager.Instance.PlayClickAudio();
		if (!LLD_LockManager.IsLocked("ScoreBank"))
		{
			LLD_MB_Singleton<LLD_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemBank != null)
			{
				onItemBank();
			}
		}
	}

	public void OnBtnRules_Click()
	{
		if (!LLD_GVars.tryLockOnePoint)
		{
			LLD_SoundManager.Instance.PlayClickAudio();
			LLD_MB_Singleton<LLD_HUDController>.GetInstance().ResetSprite();
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
		if (!LLD_GVars.tryLockOnePoint)
		{
			Hide();
			LLD_MB_Singleton<LLD_HUDController>.GetInstance().ResetSprite();
		}
	}

	public void HideAllPanel()
	{
		LLD_MB_Singleton<LLD_HUDController>.GetInstance().ResetSprite();
		Hide();
		LLD_MB_Singleton<LLD_ScoreBank>.GetInstance().Hide();
		LLD_MB_Singleton<LLD_SettingsController>.GetInstance().Hide();
	}
}
