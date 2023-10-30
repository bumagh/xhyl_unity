using M__M.GameHall.Common;
using M__M.HaiWang.Demo;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class LoginMgr : BaseUIForm
{
	[SerializeField]
	private GameObject _loginGmObj;

	private void Awake()
	{
		uiType.uiFormType = UIFormTypes.Normal;
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
		string text = _loginGmObj.transform.Find("input_Account").GetComponent<InputField>().text;
		string text2 = _loginGmObj.transform.Find("input_Pwd").GetComponent<InputField>().text;
		UnityEngine.Debug.Log($"name:{text},password:{text2}");
		if (text == string.Empty)
		{
			HW2_AlertDialog.Get().ShowDialog("用户名不能为空");
			return;
		}
		if (text2 == string.Empty)
		{
			HW2_AlertDialog.Get().ShowDialog("密码不能为空");
			return;
		}
		HW2_MB_Singleton<HW2_AppManager>.Get().StartApp();
		ZH2_GVars.username = text;
		ZH2_GVars.pwd = text2;
		PlayerPrefs.SetString("userName", text);
		PlayerPrefs.SetString("passWord", text2);
	}
}
