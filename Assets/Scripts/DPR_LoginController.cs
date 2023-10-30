using UnityEngine;
using UnityEngine.UI;

public class DPR_LoginController : DPR_MB_Singleton<DPR_LoginController>
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
		DPR_MySqlConnection.username = _inputUsername.text;
		DPR_MySqlConnection.pwd = _inputPassword.text;
		DPR_MySqlConnection.IPAddress = "127.0.0.1";
		DPR_MySqlConnection.IPPort = 10016;
		DPR_MySqlConnection.language = "zh";
		DPR_MySqlConnection.isInit = true;
		DPR_MB_Singleton<DPR_GameManager>.GetInstance().Login();
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
