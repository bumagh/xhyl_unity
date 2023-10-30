using System;
using UnityEngine;
using UnityEngine.UI;

public class CSF_AlertDialog : CSF_MB_Singleton<CSF_AlertDialog>
{
	private GameObject _goContainer;

	private GameObject _goBtnOkCancelPanel;

	private GameObject _goBtnConfirm;

	private Text _textContent;

	private Action _onOkCallback;

	public bool isTouchForbidden;

	private Text txtBtnOK;

	private Text txtBtnCancel;

	private Text txtBtnConfirm;

	private void InitFinGame()
	{
		_goContainer = base.gameObject;
		_goBtnOkCancelPanel = base.transform.Find("BtnOkCancelPanel").gameObject;
		_goBtnConfirm = base.transform.Find("BtnConfirm").gameObject;
		_textContent = base.transform.Find("TxtTip").GetComponent<Text>();
		txtBtnOK = _goBtnOkCancelPanel.transform.Find("BtnConfirm/Text").GetComponent<Text>();
		txtBtnCancel = _goBtnOkCancelPanel.transform.Find("BtnConcel/Text").GetComponent<Text>();
		txtBtnConfirm = base.transform.Find("BtnConfirm/Text").GetComponent<Text>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		InitFinGame();
		if (CSF_MB_Singleton<CSF_AlertDialog>._instance == null)
		{
			CSF_MB_Singleton<CSF_AlertDialog>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		int num = (CSF_MySqlConnection.language == "en") ? 1 : 0;
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
		UnityEngine.Debug.Log("信息提示: " + content);
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
		CSF_MB_Singleton<CSF_GameRoot>.GetInstance().StartCoroutine(CSF_Utils.DelayCall(delay, delegate
		{
			_showDialog(content, showOkCancel, callback);
		}));
	}

	private void _showDialog(string content, bool showOkCancel = false, Action callback = null)
	{
		isTouchForbidden = !CSF_MB_Singleton<CSF_GameManager>.GetInstance().GetTouchEnable();
		if (isTouchForbidden)
		{
			CSF_MB_Singleton<CSF_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "alert", includeEsc: false);
		}
		_textContent.text = content;
		_onOkCallback = callback;
		_goBtnOkCancelPanel.SetActive(showOkCancel);
		_goBtnConfirm.SetActive(!showOkCancel);
		_goContainer.SetActive(value: true);
		if (CSF_MB_Singleton<CSF_OptionsController>.GetInstance() != null)
		{
			CSF_MB_Singleton<CSF_OptionsController>.GetInstance().HideAllPanel();
		}
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
		if (isTouchForbidden)
		{
			CSF_MB_Singleton<CSF_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "alert done, reforbidden", includeEsc: false);
		}
	}

	public void OnBtnOK_Click()
	{
		if (!CSF_MySqlConnection.tryLockOnePoint)
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
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			if (CSF_SoundManager.Instance != null)
			{
				CSF_SoundManager.Instance.PlayClickAudio();
			}
			Hide();
		}
	}

	public void OnBtnConfirm_Click()
	{
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			if (CSF_SoundManager.Instance != null)
			{
				CSF_SoundManager.Instance.PlayClickAudio();
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
		if (!CSF_MySqlConnection.tryLockOnePoint)
		{
			if (CSF_SoundManager.Instance != null)
			{
				CSF_SoundManager.Instance.PlayClickAudio();
			}
			Hide();
		}
	}

	public bool IsShow()
	{
		return _goContainer.activeSelf;
	}
}
