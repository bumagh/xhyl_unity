using System;
using UnityEngine;
using UnityEngine.UI;

public class Hfh_AlertDialog : Hfh_Singleton<Hfh_AlertDialog>
{
	private GameObject _goContainer;

	private Action _onOkCallback;

	private Text _textContent;

	private Button OkBtn;

	private Button EnterBtn;

	private Button CancelBtn;

	private Button CloseBtn;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		InitFinGame();
		if (Hfh_Singleton<Hfh_AlertDialog>._instance == null)
		{
			Hfh_Singleton<Hfh_AlertDialog>.SetInstance(this);
			PreInit();
		}
		base.gameObject.SetActive(value: false);
	}

	private void Start()
	{
		int num = (Hfh_GVars.language == string.Empty) ? 1 : 0;
	}

	private void InitFinGame()
	{
		_goContainer = base.gameObject;
		_textContent = base.transform.Find("Text").GetComponent<Text>();
		OkBtn = base.transform.Find("OkBtn").GetComponent<Button>();
		EnterBtn = base.transform.Find("EnterBtn").GetComponent<Button>();
		CancelBtn = base.transform.Find("CancelBtn").GetComponent<Button>();
		CloseBtn = base.transform.Find("CloseBtn").GetComponent<Button>();
		OkBtn.onClick.AddListener(OnBtnOK_Click);
		EnterBtn.onClick.AddListener(OnBtnConfirm_Click);
		CancelBtn.onClick.AddListener(OnBtnCancel_Click);
		CloseBtn.onClick.AddListener(OnBtnClose_Click);
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
			if (OkBtn.gameObject.activeSelf && _onOkCallback != null)
			{
				_onOkCallback();
			}
		}
		Hfh_Singleton<Hfh_GameRoot>.GetInstance().StartCoroutine(Hfh_Utils.DelayCall(delay, delegate
		{
			_showDialog(content, showOkCancel, callback);
		}));
	}

	private void _showDialog(string content, bool showEnterCancel = false, Action callback = null)
	{
		_textContent.text = content;
		_onOkCallback = callback;
		EnterBtn.gameObject.SetActive(showEnterCancel);
		CancelBtn.gameObject.SetActive(showEnterCancel);
		OkBtn.gameObject.SetActive(!showEnterCancel);
		CloseBtn.gameObject.SetActive(!showEnterCancel);
		_goContainer.SetActive(value: true);
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		Hfh_GVars.lockOnePoint = false;
		_goContainer.SetActive(value: false);
	}

	public void OnBtnOK_Click()
	{
		if (Hfh_GVars.tryLockOnePoint)
		{
			UnityEngine.Debug.Log("触发了！");
			return;
		}
		Hide();
		if (_onOkCallback != null)
		{
			_onOkCallback();
		}
	}

	public void OnBtnCancel_Click()
	{
		if (Hfh_GVars.tryLockOnePoint)
		{
			UnityEngine.Debug.Log("触发了！");
			return;
		}
		Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
		Hide();
	}

	public void OnBtnConfirm_Click()
	{
		if (Hfh_GVars.tryLockOnePoint)
		{
			UnityEngine.Debug.Log("触发了！");
			return;
		}
		Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
		Hide();
		if (_onOkCallback != null)
		{
			_onOkCallback();
		}
	}

	public void OnBtnClose_Click()
	{
		if (Hfh_GVars.tryLockOnePoint)
		{
			UnityEngine.Debug.Log("触发了！");
			return;
		}
		Hfh_Singleton<Hfh_AudioManager>.GetInstance().PlaySound("click");
		Hide();
	}

	public bool IsShow()
	{
		return _goContainer.activeSelf;
	}
}
