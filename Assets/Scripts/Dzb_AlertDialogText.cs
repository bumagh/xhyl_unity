using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dzb_AlertDialogText : Dzb_Singleton<Dzb_AlertDialogText>
{
	private GameObject _goContainer;

	private Action _onOkCallback;

	private Text _textContent;

	private Button CloseBtn;

	private float CountDown;

	private Coroutine _coCountDown;

	private void Awake()
	{
		InitFinGame();
		if (Dzb_Singleton<Dzb_AlertDialogText>._instance == null)
		{
			Dzb_Singleton<Dzb_AlertDialogText>.SetInstance(this);
		}
		_goContainer.SetActive(value: false);
	}

	private void Start()
	{
		int num = (Dzb_MySqlConnection.language == string.Empty) ? 1 : 0;
	}

	private void InitFinGame()
	{
		_goContainer = base.transform.Find("Panel").gameObject;
		_textContent = base.transform.Find("Panel/Text").GetComponent<Text>();
		CloseBtn = base.transform.Find("Panel/CloseBtn").GetComponent<Button>();
		CloseBtn.onClick.AddListener(delegate
		{
			Hide();
		});
	}

	public void ShowDialogText(string content, float Rtimne)
	{
		_goContainer.SetActive(value: true);
		if (Rtimne > 0f)
		{
			CloseBtn.gameObject.SetActive(value: false);
			if (_coCountDown != null)
			{
				StopCoroutine(_coCountDown);
				_coCountDown = null;
			}
			_coCountDown = StartCoroutine(CountDown_IE());
		}
		else
		{
			CloseBtn.gameObject.SetActive(value: true);
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

	private IEnumerator CountDown_IE()
	{
		float _time = 0f;
		while (true)
		{
			if (_time >= CountDown)
			{
				Hide();
			}
			yield return new WaitForSeconds(0.02f);
			_time += 0.02f;
		}
	}
}
