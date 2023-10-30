using UnityEngine;
using UnityEngine.UI;

public class CRL_LoginController : CRL_MB_Singleton<CRL_LoginController>
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
		CRL_MySqlConnection.username = _inputUsername.text;
		CRL_MySqlConnection.pwd = _inputPassword.text;
		CRL_MySqlConnection.IPAddress = "127.0.0.1";
		CRL_MySqlConnection.IPPort = 10016;
		CRL_MySqlConnection.language = "zh";
		CRL_MySqlConnection.isInit = true;
		CRL_MB_Singleton<CRL_GameManager>.GetInstance().Login();
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
