using System;
using UnityEngine;
using UnityEngine.UI;

public class STWM_OptionsController : STWM_MB_Singleton<STWM_OptionsController>
{
	[SerializeField]
	private GameObject _goContainer;

	public Action onItemReturn;

	public Action onItemBank;

	public Action onItemSettings;

	public Action onItemRules;

	[SerializeField]
	private Text txtBack;

	[SerializeField]
	private Text txtKeyInOut;

	[SerializeField]
	private Text txtSet;

	[SerializeField]
	private Text txtRule;

	public bool isShow => _goContainer.activeSelf;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		if (STWM_MB_Singleton<STWM_OptionsController>._instance == null)
		{
			STWM_MB_Singleton<STWM_OptionsController>.SetInstance(this);
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
		txtBack.text = ZH2_GVars.ShowTip("返    回", "Back", string.Empty);
		txtKeyInOut.text = ZH2_GVars.ShowTip("存取分", "KeyIn/Out", string.Empty);
		txtSet.text = ZH2_GVars.ShowTip("设    置", "Set", string.Empty);
		txtRule.text = ZH2_GVars.ShowTip("规    则", "Rules", string.Empty);
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
		if (!STWM_GVars.tryLockOnePoint)
		{
			STWM_SoundManager.Instance.PlayClickAudio();
			STWM_MB_Singleton<STWM_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemReturn != null)
			{
				onItemReturn();
			}
		}
	}

	public void OnClickSafeBox()
	{
		if (ZH2_GVars.OpenCheckSafeBoxPwdPanel != null)
		{
			ZH2_GVars.OpenCheckSafeBoxPwdPanel(ZH2_GVars.GameType_DJ.water_desk);
		}
	}

	public void OnClickToUp()
	{
		if (ZH2_GVars.OpenPlyBoxPanel != null)
		{
			ZH2_GVars.OpenPlyBoxPanel(ZH2_GVars.GameType_DJ.water_desk);
		}
	}

	public void OnBtnSettings_Click()
	{
		if (!STWM_GVars.tryLockOnePoint)
		{
			STWM_SoundManager.Instance.PlayClickAudio();
			STWM_MB_Singleton<STWM_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemSettings != null)
			{
				onItemSettings();
			}
		}
	}

	public void OnBtnBank_Click()
	{
		if (STWM_GVars.tryLockOnePoint)
		{
			return;
		}
		STWM_SoundManager.Instance.PlayClickAudio();
		if (!STWM_LockManager.IsLocked("ScoreBank"))
		{
			STWM_MB_Singleton<STWM_HUDController>.GetInstance().ResetSprite();
			Hide();
			if (onItemBank != null)
			{
				onItemBank();
			}
		}
	}

	public void OnBtnRules_Click()
	{
		if (!STWM_GVars.tryLockOnePoint)
		{
			STWM_SoundManager.Instance.PlayClickAudio();
			STWM_MB_Singleton<STWM_HUDController>.GetInstance().ResetSprite();
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
		if (!STWM_GVars.tryLockOnePoint)
		{
			Hide();
			STWM_MB_Singleton<STWM_HUDController>.GetInstance().ResetSprite();
		}
	}

	public void HideAllPanel()
	{
		STWM_MB_Singleton<STWM_HUDController>.GetInstance().ResetSprite();
		Hide();
		STWM_MB_Singleton<STWM_ScoreBank>.GetInstance().Hide();
		STWM_MB_Singleton<STWM_SettingsController>.GetInstance().Hide();
	}
}
