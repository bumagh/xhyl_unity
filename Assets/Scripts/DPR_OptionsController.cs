using System;
using UnityEngine;

public class DPR_OptionsController : DPR_MB_Singleton<DPR_OptionsController>
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
		if (DPR_MB_Singleton<DPR_OptionsController>._instance == null)
		{
			DPR_MB_Singleton<DPR_OptionsController>.SetInstance(this);
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
		if (!DPR_MySqlConnection.tryLockOnePoint)
		{
			DPR_SoundManager.Instance.PlayClickAudio();
			DPR_MB_Singleton<DPR_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!DPR_MySqlConnection.tryLockOnePoint)
		{
			DPR_SoundManager.Instance.PlayClickAudio();
			DPR_MB_Singleton<DPR_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (DPR_MySqlConnection.tryLockOnePoint)
		{
			return;
		}
		DPR_SoundManager.Instance.PlayClickAudio();
		if (!DPR_LockManager.IsLocked("ScoreBank"))
		{
			DPR_MB_Singleton<DPR_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemBank != null)
			{
				onItemBank();
			}
		}
	}

	public void OnBtnRules_Click()
	{
		if (!DPR_MySqlConnection.tryLockOnePoint)
		{
			DPR_SoundManager.Instance.PlayClickAudio();
			DPR_MB_Singleton<DPR_HUDController>.GetInstance().ResetSprite();
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
		if (!DPR_MySqlConnection.tryLockOnePoint)
		{
			Hide();
			DPR_MB_Singleton<DPR_HUDController>.GetInstance().ResetSprite();
		}
	}

	public void HideAllPanel()
	{
		DPR_MB_Singleton<DPR_HUDController>.GetInstance().ResetSprite();
		Hide();
		DPR_MB_Singleton<DPR_ScoreBank>.GetInstance().Hide();
		DPR_MB_Singleton<DPR_SettingsController>.GetInstance().Hide();
	}
}
