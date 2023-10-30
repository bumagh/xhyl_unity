using UnityEngine;
using UnityEngine.UI;

public class STWM_LoginController : STWM_MB_Singleton<STWM_LoginController>
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
		STWM_GVars.username = _inputUsername.text;
		STWM_GVars.pwd = _inputPassword.text;
		STWM_GVars.IPAddress = "114.117.251.114";
		STWM_GVars.IPPort = 10016;
		STWM_GVars.language = ((ZH2_GVars.language_enum != 0) ? "en" : "zh");
		STWM_GVars.isInit = true;
		STWM_MB_Singleton<STWM_GameManager>.GetInstance().Login();
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
