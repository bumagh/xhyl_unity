using UnityEngine;
using UnityEngine.UI;

public class PTM_LoginController : PTM_MB_Singleton<PTM_LoginController>
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
		PTM_GVars.username = _inputUsername.text;
		PTM_GVars.pwd = _inputPassword.text;
		PTM_GVars.IPAddress = "127.0.0.1";
		PTM_GVars.IPPort = 10016;
		PTM_GVars.language = "zh";
		PTM_GVars.isInit = true;
		PTM_MB_Singleton<PTM_GameManager>.GetInstance().Login();
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
