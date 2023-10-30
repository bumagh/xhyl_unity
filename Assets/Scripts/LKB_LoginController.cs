using UnityEngine;
using UnityEngine.UI;

public class LKB_LoginController : LKB_MB_Singleton<LKB_LoginController>
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
		LKB_GVars.username = _inputUsername.text;
		LKB_GVars.pwd = _inputPassword.text;
		LKB_GVars.IPAddress = "127.0.0.1";
		LKB_GVars.IPPort = 10016;
		LKB_GVars.language = "zh";
		LKB_GVars.isInit = true;
		LKB_MB_Singleton<LKB_GameManager>.GetInstance().Login();
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
