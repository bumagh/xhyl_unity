using System;
using UnityEngine;

public class LRS_OptionsController : LRS_MB_Singleton<LRS_OptionsController>
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
		if (LRS_MB_Singleton<LRS_OptionsController>._instance == null)
		{
			LRS_MB_Singleton<LRS_OptionsController>.SetInstance(this);
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
		if (!LRS_GVars.tryLockOnePoint)
		{
			LRS_SoundManager.Instance.PlayClickAudio();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!LRS_GVars.tryLockOnePoint)
		{
			LRS_SoundManager.Instance.PlayClickAudio();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (LRS_GVars.tryLockOnePoint)
		{
			return;
		}
		LRS_SoundManager.Instance.PlayClickAudio();
		if (!LRS_LockManager.IsLocked("ScoreBank"))
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
		if (!LRS_GVars.tryLockOnePoint)
		{
			LRS_SoundManager.Instance.PlayClickAudio();
			LRS_MB_Singleton<LRS_MajorGameController>.GetInstance().OnBtnRight_Click(isBg: true);
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
		if (!LRS_GVars.tryLockOnePoint)
		{
			Hide();
		}
	}

	public void HideAllPanel()
	{
		Hide();
		LRS_MB_Singleton<LRS_ScoreBank>.GetInstance().Hide();
		LRS_MB_Singleton<LRS_SettingsController>.GetInstance().Hide();
	}
}
