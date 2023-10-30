using UnityEngine;
using UnityEngine.UI;

public class LLD_LoginController : LLD_MB_Singleton<LLD_LoginController>
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
		LLD_GVars.username = _inputUsername.text;
		LLD_GVars.pwd = _inputPassword.text;
		LLD_GVars.IPAddress = "127.0.0.1";
		LLD_GVars.IPPort = 10016;
		LLD_GVars.language = "zh";
		LLD_GVars.isInit = true;
		LLD_MB_Singleton<LLD_GameManager>.GetInstance().Login();
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
