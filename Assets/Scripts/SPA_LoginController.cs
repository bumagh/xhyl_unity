using UnityEngine;
using UnityEngine.UI;

public class SPA_LoginController : SPA_MB_Singleton<SPA_LoginController>
{
	[SerializeField]
	private InputField _inputUsername;

	[SerializeField]
	private InputField _inputPassword;

	private GameObject _goContainer;

	public void PreInit()
	{
		_goContainer = base.gameObject;
	}

	public void OnBtnLogin_Click()
	{
		UnityEngine.Debug.Log("OnBtnLogin_Click");
		SPA_GVars.username = _inputUsername.text;
		SPA_GVars.pwd = _inputPassword.text;
		SPA_GVars.IPAddress = "127.0.0.1";
		SPA_GVars.IPPort = 10016;
		SPA_GVars.language = "zh";
		SPA_GVars.isInit = true;
		SPA_MB_Singleton<SPA_GameManager>.GetInstance().Login();
	}

	public bool IsShow()
	{
		return _goContainer.activeSelf;
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}
}
