using System;
using UnityEngine;
using UnityEngine.UI;

public class CSF_OptionsController : CSF_MB_Singleton<CSF_OptionsController>
{
	private GameObject _goContainer;

	public Action onItemReturn;

	public Action onItemBank;

	public Action onItemSettings;

	public Action onItemRules;

	private Text txtBack;

	private Text txtKeyInOut;

	private Text txtSet;

	private Text txtRule;

	private int language;

	private int temp = 2;

	public bool isShow => _goContainer.activeSelf;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		_goContainer = base.gameObject;
		txtBack = base.transform.Find("BtnBack/Text").GetComponent<Text>();
		txtKeyInOut = base.transform.Find("BtnInOut/Text").GetComponent<Text>();
		txtSet = base.transform.Find("BtnSet/Text").GetComponent<Text>();
		txtRule = base.transform.Find("BtnRule/Text").GetComponent<Text>();
		if (CSF_MB_Singleton<CSF_OptionsController>._instance == null)
		{
			CSF_MB_Singleton<CSF_OptionsController>.SetInstance(this);
			PreInit();
		}
	}

	public void PreInit()
	{
		if (_goContainer == null)
		{
			_goContainer = base.gameObject;
		}
		InitTxts();
	}

	private void InitTxts()
	{
		try
		{
			if (txtBack != null)
			{
				language = ((CSF_MySqlConnection.language == "en") ? 1 : 0);
				txtBack.text = ((language != 0) ? "Back" : "返    回");
				txtKeyInOut.text = ((language != 0) ? "KeyIn/Out" : "存取分");
				txtSet.text = ((language != 0) ? "Set" : "设    置");
				txtRule.text = ((language != 0) ? "Rules" : "规    则");
			}
			else
			{
				txtBack = base.transform.Find("BtnBack/Text").GetComponent<Text>();
				txtKeyInOut = base.transform.Find("BtnInOut/Text").GetComponent<Text>();
				txtSet = base.transform.Find("BtnSet/Text").GetComponent<Text>();
				txtRule = base.transform.Find("BtnRule/Text").GetComponent<Text>();
				temp--;
				if (temp > 0)
				{
					InitTxts();
				}
			}
		}
		catch (Exception)
		{
			throw;
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
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			CSF_SoundManager.Instance.PlayClickAudio();
			CSF_MB_Singleton<CSF_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			CSF_SoundManager.Instance.PlayClickAudio();
			CSF_MB_Singleton<CSF_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (CSF_MySqlConnection.tryLockOnePoint)
		{
			return;
		}
		CSF_SoundManager.Instance.PlayClickAudio();
		if (!CSF_LockManager.IsLocked("ScoreBank"))
		{
			CSF_MB_Singleton<CSF_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemBank != null)
			{
				onItemBank();
			}
		}
	}

	public void OnBtnRules_Click()
	{
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			CSF_SoundManager.Instance.PlayClickAudio();
			CSF_MB_Singleton<CSF_HUDController>.GetInstance().ResetSprite();
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
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			Hide();
			CSF_MB_Singleton<CSF_HUDController>.GetInstance().ResetSprite();
		}
	}

	public void HideAllPanel()
	{
		CSF_MB_Singleton<CSF_HUDController>.GetInstance().ResetSprite();
		Hide();
		CSF_MB_Singleton<CSF_ScoreBank>.GetInstance().Hide();
		CSF_MB_Singleton<CSF_SettingsController>.GetInstance().Hide();
	}
}
