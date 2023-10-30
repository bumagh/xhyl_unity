using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dzb_ReconnectHint : Dzb_Singleton<Dzb_ReconnectHint>
{
	private GameObject _goContainer;

	[SerializeField]
	private Text _textInfo;

	[SerializeField]
	private GameObject _goUI;

	private Coroutine _coTextInfoAni;

	private void Awake()
	{
		if (Dzb_Singleton<Dzb_ReconnectHint>._instance == null)
		{
			Dzb_Singleton<Dzb_ReconnectHint>.SetInstance(this);
			PreInit();
		}
	}

	public void PreInit()
	{
		_goContainer = base.gameObject;
		Hide();
	}

	public void Show()
	{
		if (!_goContainer.activeSelf)
		{
			Dzb_LockManager.Lock("Esc");
			UnityEngine.Debug.Log(Dzb_LogHelper.Magenta("ReconnectHint Show"));
			_goContainer.SetActive(value: true);
			_setUIEnable(isEnable: false);
			_coTextInfoAni = StartCoroutine(_textInfoAni());
		}
	}

	public void Hide()
	{
		UnityEngine.Debug.Log(Dzb_LogHelper.Magenta("ReconnectHint Hide"));
		Dzb_LockManager.UnLock("Esc");
		_goContainer.SetActive(value: false);
		if (_coTextInfoAni != null)
		{
			StopCoroutine(_coTextInfoAni);
		}
		_coTextInfoAni = null;
	}

	private void _setUIEnable(bool isEnable)
	{
		_goUI.SetActive(isEnable);
	}

	private IEnumerator _textInfoAni()
	{
		int count2 = 0;
		yield return new WaitForSeconds(0f);
		_setUIEnable(isEnable: true);
		while (true)
		{
			string prefix = (Dzb_MySqlConnection.language == "zh") ? "网络断开，正在重连 " : "Reconnecting to the network ";
			string postfix = string.Empty;
			count2++;
			count2 %= 8;
			for (int i = 0; i < count2; i++)
			{
				postfix += ((i == 3) ? " " : "·");
			}
			_textInfo.text = prefix + postfix;
			yield return new WaitForSeconds(0.3f);
		}
	}
}
