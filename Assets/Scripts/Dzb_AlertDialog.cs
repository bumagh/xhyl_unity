using System;
using UnityEngine;
using UnityEngine.UI;

public class Dzb_AlertDialog : Dzb_Singleton<Dzb_AlertDialog>
{
	private GameObject _goContainer;

	private Action _onOkCallback;

	private Text _textContent;

	public bool isTouchForbidden;

	private Button OkBtn;

	private Button EnterBtn;

	private Button CancelBtn;

	private Button CloseBtn;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		InitFinGame();
		if (Dzb_Singleton<Dzb_AlertDialog>._instance == null)
		{
			Dzb_Singleton<Dzb_AlertDialog>.SetInstance(this);
			PreInit();
		}
		base.gameObject.SetActive(value: false);
	}

	private void Start()
	{
		int num = (Dzb_MySqlConnection.language == string.Empty) ? 1 : 0;
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
		Dzb_Singleton<Dzb_GameRoot>.GetInstance().StartCoroutine(Dzb_Utils.DelayCall(delay, delegate
		{
			_showDialog(content, showOkCancel, callback);
		}));
	}

	private void _showDialog(string content, bool showEnterCancel = false, Action callback = null)
	{
		try
		{
			isTouchForbidden = !Dzb_Singleton<Dzb_GameInfo>.GetInstance().GetTouchEnable();
			if (isTouchForbidden)
			{
				Dzb_Singleton<Dzb_GameInfo>.GetInstance().SetTouchEnable(isEnable: true, "alert", includeEsc: false);
			}
			_textContent.text = content;
			_onOkCallback = callback;
			EnterBtn.gameObject.SetActive(showEnterCancel);
			CancelBtn.gameObject.SetActive(showEnterCancel);
			OkBtn.gameObject.SetActive(!showEnterCancel);
			CloseBtn.gameObject.SetActive(!showEnterCancel);
			_goContainer.SetActive(value: true);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("有报错：" + ex.ToString());
		}
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		Dzb_MySqlConnection.lockOnePoint = false;
		_goContainer.SetActive(value: false);
		if (isTouchForbidden)
		{
			Dzb_Singleton<Dzb_GameInfo>.GetInstance().SetTouchEnable(isEnable: false, "alert done, reforbidden", includeEsc: false);
		}
	}

	public void OnBtnOK_Click()
	{
		Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
		if (Dzb_MySqlConnection.tryLockOnePoint)
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
		Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
		if (Dzb_MySqlConnection.tryLockOnePoint)
		{
			UnityEngine.Debug.Log("触发了！");
		}
		else
		{
			Hide();
		}
	}

	public void OnBtnConfirm_Click()
	{
		Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
		if (Dzb_MySqlConnection.tryLockOnePoint)
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

	public void OnBtnClose_Click()
	{
		Dzb_Singleton<Dzb_AudioManager>.GetInstance().PlaySound("click");
		if (Dzb_MySqlConnection.tryLockOnePoint)
		{
			UnityEngine.Debug.Log("触发了！");
		}
		else
		{
			Hide();
		}
	}

	public bool IsShow()
	{
		return _goContainer.activeSelf;
	}
}
