using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WHN_ReconnectHint : WHN_MB_Singleton<WHN_ReconnectHint>
{
	private GameObject _goContainer;

	private Text _textInfo;

	private GameObject _goUI;

	private Coroutine _coTextInfoAni;

	private void Awake()
	{
		if (WHN_MB_Singleton<WHN_ReconnectHint>._instance == null)
		{
			WHN_MB_Singleton<WHN_ReconnectHint>.SetInstance(this);
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
			WHN_LockManager.Lock("Esc");
			UnityEngine.Debug.Log(WHN_LogHelper.Magenta("ReconnectHint Show"));
			_goContainer.SetActive(value: true);
			_setUIEnable(isEnable: false);
			_coTextInfoAni = StartCoroutine(_textInfoAni());
		}
	}

	public void Hide()
	{
		UnityEngine.Debug.Log(WHN_LogHelper.Magenta("ReconnectHint Hide"));
		WHN_LockManager.UnLock("Esc");
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
			string prefix = (WHN_GVars.language == "zh") ? "网络断开，正在重连 " : "Reconnecting to the network ";
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
