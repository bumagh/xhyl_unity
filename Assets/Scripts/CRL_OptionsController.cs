using System;
using UnityEngine;

public class CRL_OptionsController : CRL_MB_Singleton<CRL_OptionsController>
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
		if (CRL_MB_Singleton<CRL_OptionsController>._instance == null)
		{
			CRL_MB_Singleton<CRL_OptionsController>.SetInstance(this);
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
		if (!CRL_MySqlConnection.tryLockOnePoint)
		{
			CRL_SoundManager.Instance.PlayClickAudio();
			CRL_MB_Singleton<CRL_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!CRL_MySqlConnection.tryLockOnePoint)
		{
			CRL_SoundManager.Instance.PlayClickAudio();
			CRL_MB_Singleton<CRL_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (CRL_MySqlConnection.tryLockOnePoint)
		{
			return;
		}
		CRL_SoundManager.Instance.PlayClickAudio();
		if (!CRL_LockManager.IsLocked("ScoreBank"))
		{
			CRL_MB_Singleton<CRL_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemBank != null)
			{
				onItemBank();
			}
		}
	}

	public void OnBtnRules_Click()
	{
		if (!CRL_MySqlConnection.tryLockOnePoint)
		{
			CRL_SoundManager.Instance.PlayClickAudio();
			CRL_MB_Singleton<CRL_HUDController>.GetInstance().ResetSprite();
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
		if (!CRL_MySqlConnection.tryLockOnePoint)
		{
			Hide();
			CRL_MB_Singleton<CRL_HUDController>.GetInstance().ResetSprite();
		}
	}

	public void HideAllPanel()
	{
		CRL_MB_Singleton<CRL_HUDController>.GetInstance().ResetSprite();
		Hide();
		CRL_MB_Singleton<CRL_ScoreBank>.GetInstance().Hide();
		CRL_MB_Singleton<CRL_SettingsController>.GetInstance().Hide();
	}
}
