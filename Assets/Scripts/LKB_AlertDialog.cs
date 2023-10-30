using System;
using UnityEngine;
using UnityEngine.UI;

public class LKB_AlertDialog : LKB_MB_Singleton<LKB_AlertDialog>
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
		btnConfirm.onClick.AddListener(OnBtnOK_Click);
		btnConcel = _goBtnOkCancelPanel.transform.Find("BtnConcel").GetComponent<Button>();
		btnConfirm.onClick.AddListener(OnBtnCancel_Click);
		btnConfirmOut = _goBtnConfirm.transform.GetComponent<Button>();
		btnConfirmOut.onClick.AddListener(OnBtnConfirm_Click);
	}

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		InitFinGame();
		if (LKB_MB_Singleton<LKB_AlertDialog>._instance == null)
		{
			LKB_MB_Singleton<LKB_AlertDialog>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
		int num = (LKB_GVars.language == "en") ? 1 : 0;
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
		if (_goContainer == null)
		{
			_goContainer = base.gameObject;
		}
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
		LKB_MB_Singleton<LKB_GameRoot>.GetInstance().StartCoroutine(LKB_Utils.DelayCall(delay, delegate
		{
			_showDialog(content, showOkCancel, callback);
		}));
	}

	private void _showDialog(string content, bool showOkCancel = false, Action callback = null)
	{
		isTouchForbidden = !LKB_MB_Singleton<LKB_GameManager>.GetInstance().GetTouchEnable();
		if (isTouchForbidden)
		{
			LKB_MB_Singleton<LKB_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "alert", includeEsc: false);
		}
		_textContent.text = content;
		_onOkCallback = callback;
		_goBtnOkCancelPanel.SetActive(showOkCancel);
		_goBtnConfirm.SetActive(!showOkCancel);
		_goContainer.SetActive(value: true);
		if (LKB_MB_Singleton<LKB_OptionsController>.GetInstance() != null)
		{
			LKB_MB_Singleton<LKB_OptionsController>.GetInstance().HideAllPanel();
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
			LKB_MB_Singleton<LKB_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "alert done, reforbidden", includeEsc: false);
		}
	}

	public void OnBtnOK_Click()
	{
		if (!LKB_GVars.tryLockOnePoint)
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
		if (!LKB_GVars.tryLockOnePoint)
		{
			if (LKB_SoundManager.Instance != null)
			{
				LKB_SoundManager.Instance.PlayClickAudio();
			}
			Hide();
		}
	}

	public void OnBtnConfirm_Click()
	{
		if (!LKB_GVars.tryLockOnePoint)
		{
			if (LKB_SoundManager.Instance != null)
			{
				LKB_SoundManager.Instance.PlayClickAudio();
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
		if (!LKB_GVars.tryLockOnePoint)
		{
			if (LKB_SoundManager.Instance != null)
			{
				LKB_SoundManager.Instance.PlayClickAudio();
			}
			Hide();
		}
	}

	public bool IsShow()
	{
		return _goContainer.activeSelf;
	}
}
