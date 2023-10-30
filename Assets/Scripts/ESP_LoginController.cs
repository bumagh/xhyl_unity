using UnityEngine;
using UnityEngine.UI;

public class ESP_LoginController : ESP_MB_Singleton<ESP_LoginController>
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
		ESP_MySqlConnection.username = _inputUsername.text;
		ESP_MySqlConnection.pwd = _inputPassword.text;
		ESP_MySqlConnection.IPAddress = "127.0.0.1";
		ESP_MySqlConnection.IPPort = 10016;
		ESP_MySqlConnection.language = "zh";
		ESP_MySqlConnection.isInit = true;
		ESP_MB_Singleton<ESP_GameManager>.GetInstance().Login();
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
