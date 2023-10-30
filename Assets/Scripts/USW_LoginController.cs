using UnityEngine;
using UnityEngine.UI;

public class USW_LoginController : USW_MB_Singleton<USW_LoginController>
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
		USW_GVars.username = _inputUsername.text;
		USW_GVars.pwd = _inputPassword.text;
		USW_GVars.IPAddress = "127.0.0.1";
		USW_GVars.IPPort = 10016;
		USW_GVars.language = "zh";
		USW_GVars.isInit = true;
		USW_MB_Singleton<USW_GameManager>.GetInstance().Login();
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
