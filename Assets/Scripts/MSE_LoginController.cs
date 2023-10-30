using UnityEngine;
using UnityEngine.UI;

public class MSE_LoginController : MSE_MB_Singleton<MSE_LoginController>
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
		MSE_GVars.username = _inputUsername.text;
		MSE_GVars.pwd = _inputPassword.text;
		MSE_GVars.IPAddress = "127.0.0.1";
		MSE_GVars.IPPort = 10016;
		MSE_GVars.language = "zh";
		MSE_GVars.isInit = true;
		MSE_MB_Singleton<MSE_GameManager>.GetInstance().Login();
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
