using System;
using UnityEngine;
using UnityEngine.UI;

public class LKB_OptionsController : LKB_MB_Singleton<LKB_OptionsController>
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
		if (LKB_MB_Singleton<LKB_OptionsController>._instance == null)
		{
			LKB_MB_Singleton<LKB_OptionsController>.SetInstance(this);
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
				language = ((LKB_GVars.language == "en") ? 1 : 0);
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
		if (!LKB_GVars.tryLockOnePoint)
		{
			LKB_SoundManager.Instance.PlayClickAudio();
			LKB_MB_Singleton<LKB_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!LKB_GVars.tryLockOnePoint)
		{
			LKB_SoundManager.Instance.PlayClickAudio();
			LKB_MB_Singleton<LKB_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (LKB_GVars.tryLockOnePoint)
		{
			return;
		}
		LKB_SoundManager.Instance.PlayClickAudio();
		if (!LKB_LockManager.IsLocked("ScoreBank"))
		{
			LKB_MB_Singleton<LKB_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemBank != null)
			{
				onItemBank();
			}
		}
	}

	public void OnBtnRules_Click()
	{
		if (!LKB_GVars.tryLockOnePoint)
		{
			LKB_SoundManager.Instance.PlayClickAudio();
			LKB_MB_Singleton<LKB_HUDController>.GetInstance().ResetSprite();
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
		if (!LKB_GVars.tryLockOnePoint)
		{
			Hide();
			LKB_MB_Singleton<LKB_HUDController>.GetInstance().ResetSprite();
		}
	}

	public void HideAllPanel()
	{
		LKB_MB_Singleton<LKB_HUDController>.GetInstance().ResetSprite();
		Hide();
		LKB_MB_Singleton<LKB_ScoreBank>.GetInstance().Hide();
		LKB_MB_Singleton<LKB_SettingsController>.GetInstance().Hide();
	}
}
