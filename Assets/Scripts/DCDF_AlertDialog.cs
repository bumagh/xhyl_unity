using System;
using UnityEngine;
using UnityEngine.UI;

public class DCDF_AlertDialog : DCDF_MB_Singleton<DCDF_AlertDialog>
{
	[SerializeField]
	private GameObject _goContainer;

	[SerializeField]
	private GameObject _goBtnOkCancelPanel;

	[SerializeField]
	private GameObject _goBtnConfirm;

	[SerializeField]
	private Text _textContent;

	private Action _onOkCallback;

	[SerializeField]
	private Text txtBtnOK;

	[SerializeField]
	private Text txtBtnCancel;

	[SerializeField]
	private Text txtBtnConfirm;

	private void Awake()
	{
		if (DCDF_MB_Singleton<DCDF_AlertDialog>._instance == null)
		{
			DCDF_MB_Singleton<DCDF_AlertDialog>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		int num = (DCDF_MySqlConnection.language == "en") ? 1 : 0;
		txtBtnOK.text = ((num != 0) ? "Confirm" : "确定");
		txtBtnCancel.text = ((num != 0) ? "Concel" : "取消");
		txtBtnConfirm.text = ((num != 0) ? "Confirm" : "确定");
	}

	public void PreInit()
	{
		if (_goContainer == null)
		{
			_goContainer = base.gameObject;
		}
	}

	public void ShowDialog(string content, bool showOkCancel = false, Action callback = null)
	{
		UnityEngine.Debug.Log("ShowDialog: " + content);
		float delay = 0f;
		if (_goContainer.activeSelf)
		{
			delay = 0.1f;
			Hide();
			if (_goBtnConfirm.activeSelf && _onOkCallback != null)
			{
				_onOkCallback();
			}
		}
		StartCoroutine(DCDF_Utils.DelayCall(delay, delegate
		{
			_showDialog(content, showOkCancel, callback);
		}));
	}

	private void _showDialog(string content, bool showOkCancel = false, Action callback = null)
	{
		_textContent.text = content;
		_onOkCallback = callback;
		_goBtnOkCancelPanel.SetActive(showOkCancel);
		_goBtnConfirm.SetActive(!showOkCancel);
		_goContainer.SetActive(value: true);
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	public void OnBtnOK_Click()
	{
		if (!DCDF_MySqlConnection.tryLockOnePoint)
		{
			Hide();
			if (_onOkCallback != null)
			{
				_onOkCallback();
			}
		}
	}

	public void OnBtnCancel_Click()
	{
		if (!DCDF_MySqlConnection.tryLockOnePoint)
		{
			if (DCDF_SoundManager.Instance != null)
			{
				DCDF_SoundManager.Instance.PlayClickAudio();
			}
			Hide();
		}
	}

	public void OnBtnConfirm_Click()
	{
		if (!DCDF_MySqlConnection.tryLockOnePoint)
		{
			if (DCDF_SoundManager.Instance != null)
			{
				DCDF_SoundManager.Instance.PlayClickAudio();
			}
			Hide();
			if (_onOkCallback != null)
			{
				_onOkCallback();
			}
		}
	}

	public void OnBtnClose_Click()
	{
		if (!DCDF_MySqlConnection.tryLockOnePoint)
		{
			if (DCDF_SoundManager.Instance != null)
			{
				DCDF_SoundManager.Instance.PlayClickAudio();
			}
			Hide();
		}
	}

	public bool IsShow()
	{
		return _goContainer.activeSelf;
	}
}
