using System;
using UnityEngine;

public class ESP_OptionsController : ESP_MB_Singleton<ESP_OptionsController>
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
		if (ESP_MB_Singleton<ESP_OptionsController>._instance == null)
		{
			ESP_MB_Singleton<ESP_OptionsController>.SetInstance(this);
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
		if (!ESP_MySqlConnection.tryLockOnePoint)
		{
			ESP_SoundManager.Instance.PlayClickAudio();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!ESP_MySqlConnection.tryLockOnePoint)
		{
			ESP_SoundManager.Instance.PlayClickAudio();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (ESP_MySqlConnection.tryLockOnePoint)
		{
			return;
		}
		ESP_SoundManager.Instance.PlayClickAudio();
		if (!ESP_LockManager.IsLocked("ScoreBank"))
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
		if (!ESP_MySqlConnection.tryLockOnePoint)
		{
			ESP_SoundManager.Instance.PlayClickAudio();
			ESP_MB_Singleton<ESP_MajorGameController>.GetInstance().OnBtnRight_Click(isBg: true);
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
		if (!ESP_MySqlConnection.tryLockOnePoint)
		{
			Hide();
		}
	}

	public void HideAllPanel()
	{
		Hide();
		ESP_MB_Singleton<ESP_ScoreBank>.GetInstance().Hide();
		ESP_MB_Singleton<ESP_SettingsController>.GetInstance().Hide();
	}
}
