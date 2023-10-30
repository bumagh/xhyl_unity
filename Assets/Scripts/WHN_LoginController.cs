using UnityEngine;
using UnityEngine.UI;

public class WHN_LoginController : WHN_MB_Singleton<WHN_LoginController>
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
		WHN_GVars.username = _inputUsername.text;
		WHN_GVars.pwd = _inputPassword.text;
		WHN_GVars.IPAddress = "127.0.0.1";
		WHN_GVars.IPPort = 10016;
		WHN_GVars.language = "zh";
		WHN_GVars.isInit = true;
		WHN_MB_Singleton<WHN_GameManager>.GetInstance().Login();
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
