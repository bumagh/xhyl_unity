using UnityEngine;
using UnityEngine.UI;

public class SHT_LoginController : SHT_MB_Singleton<SHT_LoginController>
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
		SHT_GVars.username = _inputUsername.text;
		SHT_GVars.pwd = _inputPassword.text;
		SHT_GVars.IPAddress = "127.0.0.1";
		SHT_GVars.IPPort = 10016;
		SHT_GVars.language = "zh";
		SHT_GVars.isInit = true;
		SHT_MB_Singleton<SHT_GameManager>.GetInstance().Login();
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
