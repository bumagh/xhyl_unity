using UnityEngine;
using UnityEngine.UI;

public class CSF_LoginController : CSF_MB_Singleton<CSF_LoginController>
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
		CSF_MySqlConnection.username = _inputUsername.text;
		CSF_MySqlConnection.pwd = _inputPassword.text;
		CSF_MySqlConnection.IPAddress = "127.0.0.1";
		CSF_MySqlConnection.IPPort = 10016;
		CSF_MySqlConnection.language = "zh";
		CSF_MySqlConnection.isInit = true;
		CSF_MB_Singleton<CSF_GameManager>.GetInstance().Login();
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
