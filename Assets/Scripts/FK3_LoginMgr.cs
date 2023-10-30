using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class FK3_LoginMgr : FK3_BaseUIForm
{
	[SerializeField]
	private GameObject _loginGmObj;

	private void Awake()
	{
		uiType.uiFormType = FK3_UIFormTypes.Normal;
		Init();
	}

	private void Init()
	{
		if (PlayerPrefs.HasKey("userName"))
		{
			_loginGmObj.transform.Find("input_Account").GetComponent<InputField>().text = PlayerPrefs.GetString("userName");
		}
		if (PlayerPrefs.HasKey("passWord"))
		{
			_loginGmObj.transform.Find("input_Pwd").GetComponent<InputField>().text = PlayerPrefs.GetString("passWord");
		}
	}

	public void SendLogin()
	{
	}
}
