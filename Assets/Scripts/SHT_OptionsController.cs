using System;
using UnityEngine;

public class SHT_OptionsController : SHT_MB_Singleton<SHT_OptionsController>
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
		if (SHT_MB_Singleton<SHT_OptionsController>._instance == null)
		{
			SHT_MB_Singleton<SHT_OptionsController>.SetInstance(this);
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
		if (!SHT_GVars.tryLockOnePoint)
		{
			SHT_SoundManager.Instance.PlayClickAudio();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!SHT_GVars.tryLockOnePoint)
		{
			SHT_SoundManager.Instance.PlayClickAudio();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (SHT_GVars.tryLockOnePoint)
		{
			return;
		}
		SHT_SoundManager.Instance.PlayClickAudio();
		if (!SHT_LockManager.IsLocked("ScoreBank"))
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
		if (!SHT_GVars.tryLockOnePoint)
		{
			SHT_SoundManager.Instance.PlayClickAudio();
			SHT_MB_Singleton<SHT_MajorGameController>.GetInstance().OnBtnRight_Click(isBg: true);
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
		if (!SHT_GVars.tryLockOnePoint)
		{
			Hide();
		}
	}

	public void HideAllPanel()
	{
		Hide();
		SHT_MB_Singleton<SHT_ScoreBank>.GetInstance().Hide();
		SHT_MB_Singleton<SHT_SettingsController>.GetInstance().Hide();
	}
}
