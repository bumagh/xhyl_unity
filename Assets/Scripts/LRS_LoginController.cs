using UnityEngine;
using UnityEngine.UI;

public class LRS_LoginController : LRS_MB_Singleton<LRS_LoginController>
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
		LRS_GVars.username = _inputUsername.text;
		LRS_GVars.pwd = _inputPassword.text;
		LRS_GVars.IPAddress = "127.0.0.1";
		LRS_GVars.IPPort = 10016;
		LRS_GVars.language = "zh";
		LRS_GVars.isInit = true;
		LRS_MB_Singleton<LRS_GameManager>.GetInstance().Login();
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
