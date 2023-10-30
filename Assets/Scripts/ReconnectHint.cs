using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReconnectHint : MB_Singleton<ReconnectHint>
{
	private GameObject _goContainer;

	[SerializeField]
	private Text _textInfo;

	[SerializeField]
	private GameObject _goUI;

	private Coroutine _coTextInfoAni;

	private void Awake()
	{
		if (MB_Singleton<ReconnectHint>._instance == null)
		{
			MB_Singleton<ReconnectHint>.SetInstance(this);
			PreInit();
		}
	}

	public void PreInit()
	{
		_goContainer = base.gameObject;
	}

	public void Show()
	{
		if (!_goContainer.activeSelf)
		{
			UnityEngine.Debug.Log(LogHelper.Magenta("ReconnectHint Show"));
			_goContainer.SetActive(value: true);
			_setUIEnable(isEnable: false);
			_coTextInfoAni = StartCoroutine(_textInfoAni());
		}
	}

	public void Hide()
	{
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
			string prefix = ZH2_GVars.ShowTip("网络断开，正在重连", "Reconnecting to the network", "การยุติการเชื่อมต่ออินเทอร์เน็ตเริ่มต้นการเชื่อมต่อ", "Mạng bị ngắt kết nối. Đang nối lại.");
			string postfix = string.Empty;
			count2++;
			count2 %= 8;
			for (int i = 0; i < count2; i++)
			{
				postfix += ((i == 3) ? " " : "‧");
			}
			_textInfo.text = prefix + postfix;
			yield return new WaitForSeconds(0.3f);
		}
	}
}
