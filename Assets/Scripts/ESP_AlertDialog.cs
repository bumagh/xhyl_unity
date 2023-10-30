using System;
using UnityEngine;
using UnityEngine.UI;

public class ESP_AlertDialog : ESP_MB_Singleton<ESP_AlertDialog>
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

	private Button btnConfirm;

	private Button btnConcel;

	private Button btnConfirmOut;

	private void InitFinGame()
	{
		_goContainer = base.gameObject;
		_goBtnOkCancelPanel = base.transform.Find("BtnOkCancelPanel").gameObject;
		_goBtnConfirm = base.transform.Find("BtnConfirm").gameObject;
		_textContent = base.transform.Find("TxtTip").GetComponent<Text>();
		txtBtnOK = _goBtnOkCancelPanel.transform.Find("BtnConfirm/Text").GetComponent<Text>();
		txtBtnCancel = _goBtnOkCancelPanel.transform.Find("BtnConcel/Text").GetComponent<Text>();
		txtBtnConfirm = base.transform.Find("BtnConfirm/Text").GetComponent<Text>();
		btnConfirm = _goBtnOkCancelPanel.transform.Find("BtnConfirm").GetComponent<Button>();
		btnConcel = _goBtnOkCancelPanel.transform.Find("BtnConcel").GetComponent<Button>();
		btnConfirmOut = _goBtnConfirm.transform.GetComponent<Button>();
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		InitFinGame();
		if (ESP_MB_Singleton<ESP_AlertDialog>._instance == null)
		{
			ESP_MB_Singleton<ESP_AlertDialog>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		int num = (ESP_MySqlConnection.language == "en") ? 1 : 0;
		txtBtnOK.text = ((num != 0) ? "Confirm" : "确定");
		txtBtnCancel.text = ((num != 0) ? "Concel" : "取消");
		txtBtnConfirm.text = ((num != 0) ? "Confirm" : "确定");
		btnConfirm.onClick.AddListener(OnBtnOK_Click);
		btnConcel.onClick.AddListener(OnBtnCancel_Click);
		btnConfirmOut.onClick.AddListener(OnBtnConfirm_Click);
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
		ESP_MB_Singleton<ESP_GameRoot>.GetInstance().StartCoroutine(ESP_Utils.DelayCall(delay, delegate
		{
			_showDialog(content, showOkCancel, callback);
		}));
	}

	private void _showDialog(string content, bool showOkCancel = false, Action callback = null)
	{
		isTouchForbidden = !ESP_MB_Singleton<ESP_GameManager>.GetInstance().GetTouchEnable();
		if (isTouchForbidden)
		{
			ESP_MB_Singleton<ESP_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "alert", includeEsc: false);
		}
		_textContent.text = content;
		_onOkCallback = callback;
		_goBtnOkCancelPanel.SetActive(showOkCancel);
		_goBtnConfirm.SetActive(!showOkCancel);
		_goContainer.SetActive(value: true);
		if (ESP_MB_Singleton<ESP_OptionsController>.GetInstance() != null)
		{
			ESP_MB_Singleton<ESP_OptionsController>.GetInstance().HideAllPanel();
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
			ESP_MB_Singleton<ESP_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "alert done, reforbidden", includeEsc: false);
		}
	}

	public void OnBtnOK_Click()
	{
		if (!ESP_MySqlConnection.tryLockOnePoint)
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
		if (!ESP_MySqlConnection.tryLockOnePoint)
		{
			if (ESP_SoundManager.Instance != null)
			{
				ESP_SoundManager.Instance.PlayClickAudio();
			}
			Hide();
		}
	}

	public void OnBtnConfirm_Click()
	{
		if (!ESP_MySqlConnection.tryLockOnePoint)
		{
			if (ESP_SoundManager.Instance != null)
			{
				ESP_SoundManager.Instance.PlayClickAudio();
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
		if (!ESP_MySqlConnection.tryLockOnePoint)
		{
			if (ESP_SoundManager.Instance != null)
			{
				ESP_SoundManager.Instance.PlayClickAudio();
			}
			Hide();
		}
	}

	public bool IsShow()
	{
		return _goContainer.activeSelf;
	}
}
