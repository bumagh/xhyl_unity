using System;
using UnityEngine;
using UnityEngine.UI;

public class STWM_AlertDialog : STWM_MB_Singleton<STWM_AlertDialog>
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

	[HideInInspector]
	public bool isTouchForbidden;

	[SerializeField]
	private Text txtBtnOK;

	[SerializeField]
	private Text txtBtnCancel;

	[SerializeField]
	private Text txtBtnConfirm;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		if (STWM_MB_Singleton<STWM_AlertDialog>._instance == null)
		{
			STWM_MB_Singleton<STWM_AlertDialog>.SetInstance(this);
			PreInit();
		}
	}

	private void Start()
	{
	
		txtBtnOK.text = ZH2_GVars.ShowTip("确定", "Confirm", "Confirm", "Xác định");
		txtBtnCancel.text = ZH2_GVars.ShowTip("取消", "cancellation", "การยกเลิก", "Hủy bỏ");
        txtBtnConfirm.text = ZH2_GVars.ShowTip("确定", "Confirm", "Confirm", "Xác định");
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
		STWM_MB_Singleton<STWM_GameRoot>.GetInstance().StartCoroutine(STWM_Utils.DelayCall(delay, delegate
		{
			_showDialog(content, showOkCancel, callback);
		}));
	}

	private void _showDialog(string content, bool showOkCancel = false, Action callback = null)
	{
		isTouchForbidden = !STWM_MB_Singleton<STWM_GameManager>.GetInstance().GetTouchEnable();
		if (isTouchForbidden)
		{
			STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: true, "alert", includeEsc: false);
		}
		_textContent.text = content;
		_onOkCallback = callback;
		_goBtnOkCancelPanel.SetActive(showOkCancel);
		_goBtnConfirm.SetActive(!showOkCancel);
		_goContainer.SetActive(value: true);
		if (STWM_MB_Singleton<STWM_OptionsController>.GetInstance() != null)
		{
			STWM_MB_Singleton<STWM_OptionsController>.GetInstance().HideAllPanel();
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
			STWM_MB_Singleton<STWM_GameManager>.GetInstance().SetTouchEnable(isEnable: false, "alert done, reforbidden", includeEsc: false);
		}
	}

	public void OnBtnOK_Click()
	{
		if (!STWM_GVars.tryLockOnePoint)
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
		if (!STWM_GVars.tryLockOnePoint)
		{
			if (STWM_SoundManager.Instance != null)
			{
				STWM_SoundManager.Instance.PlayClickAudio();
			}
			Hide();
		}
	}

	public void OnBtnConfirm_Click()
	{
		if (!STWM_GVars.tryLockOnePoint)
		{
			if (STWM_SoundManager.Instance != null)
			{
				STWM_SoundManager.Instance.PlayClickAudio();
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
		if (!STWM_GVars.tryLockOnePoint)
		{
			if (STWM_SoundManager.Instance != null)
			{
				STWM_SoundManager.Instance.PlayClickAudio();
			}
			Hide();
		}
	}

	public bool IsShow()
	{
		return _goContainer.activeSelf;
	}
}
