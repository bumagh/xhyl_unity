using UnityEngine;
using UnityEngine.UI;

public class PHG_LoginController : PHG_MB_Singleton<PHG_LoginController>
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
		PHG_GVars.username = _inputUsername.text;
		PHG_GVars.pwd = _inputPassword.text;
		PHG_GVars.IPAddress = "127.0.0.1";
		PHG_GVars.IPPort = 10016;
		PHG_GVars.language = "zh";
		PHG_GVars.isInit = true;
		PHG_MB_Singleton<PHG_GameManager>.GetInstance().Login();
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
