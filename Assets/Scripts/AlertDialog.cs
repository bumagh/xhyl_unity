using System;
using UnityEngine;
using UnityEngine.UI;

public class AlertDialog : MB_Singleton<AlertDialog>
{
	[SerializeField]
	private GameObject m_goContainer;

	[SerializeField]
	private GameObject m_goBtnOkCancelPanel;

	[SerializeField]
	private GameObject m_goBtnConfirm;

	[SerializeField]
	private Text m_textContent;

	private Action _onOkCallback;

	private void Awake()
	{
		if (MB_Singleton<AlertDialog>._instance == null)
		{
			MB_Singleton<AlertDialog>.SetInstance(this);
			PreInit();
		}
	}

	public void PreInit()
	{
		if (m_goContainer == null)
		{
			m_goContainer = base.gameObject;
		}
	}

	public void ShowDialog(string content, bool showOkCancel = false, Action callback = null)
	{
		try
		{
			MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Alert_Hint);
		}
		catch (Exception)
		{
		}
		UnityEngine.Debug.Log("ShowDialog: " + content);
		float delay = 0f;
		if (m_goContainer.activeSelf)
		{
			delay = 0.1f;
			Hide();
			if (m_goBtnConfirm.activeSelf && _onOkCallback != null)
			{
				_onOkCallback();
			}
		}
		MB_Singleton<AppRoot>.GetInstance().StartCoroutine(Utils.DelayCall(delay, delegate
		{
			_showDialog(content, showOkCancel, callback);
		}));
	}

	private void _showDialog(string content, bool showOkCancel = false, Action callback = null)
	{
		m_textContent.font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? MB_Singleton<AppManager>.Get().en_font : ((ZH2_GVars.language_enum != 0) ? MB_Singleton<AppManager>.Get().th_font : MB_Singleton<AppManager>.Get().zh_font));
		m_textContent.text = content;
		_onOkCallback = callback;
		m_goBtnOkCancelPanel.SetActive(showOkCancel);
		m_goBtnConfirm.SetActive(!showOkCancel);
		m_goContainer.SetActive(value: true);
	}

	public void Show()
	{
		m_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		m_goContainer.SetActive(value: false);
	}

	public void OnBtnOK_Click()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		Hide();
		if (_onOkCallback != null)
		{
			_onOkCallback();
		}
	}

	public void OnBtnCancel_Click()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		Hide();
	}

	public void OnBtnConfirm_Click()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		Hide();
		if (_onOkCallback != null)
		{
			_onOkCallback();
		}
	}

	public void OnBtnClose_Click()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		Hide();
	}

	public bool IsShow()
	{
		return m_goContainer.activeSelf;
	}
}
