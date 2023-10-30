using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CRL_ReconnectHint : CRL_MB_Singleton<CRL_ReconnectHint>
{
	private GameObject _goContainer;

	private Text _textInfo;

	private GameObject _goUI;

	private Coroutine _coTextInfoAni;

	private void Awake()
	{
		if (CRL_MB_Singleton<CRL_ReconnectHint>._instance == null)
		{
			CRL_MB_Singleton<CRL_ReconnectHint>.SetInstance(this);
			PreInit();
		}
	}

	public void PreInit()
	{
		_goContainer = base.gameObject;
		_goUI = base.transform.Find("Image").gameObject;
		_textInfo = _goUI.transform.Find("Text").GetComponent<Text>();
	}

	public void Show()
	{
		if (!_goContainer.activeSelf)
		{
			CRL_LockManager.Lock("Esc");
			UnityEngine.Debug.Log(CRL_LogHelper.Magenta("ReconnectHint Show"));
			_goContainer.SetActive(value: true);
			_setUIEnable(isEnable: false);
			_coTextInfoAni = StartCoroutine(_textInfoAni());
		}
	}

	public void Hide()
	{
		UnityEngine.Debug.Log(CRL_LogHelper.Magenta("ReconnectHint Hide"));
		CRL_LockManager.UnLock("Esc");
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
			string prefix = (CRL_MySqlConnection.language == "zh") ? "网络断开，正在重连 " : "Reconnecting to the network ";
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
