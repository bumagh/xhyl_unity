using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeBoxPwdCheckPanelController : MonoBehaviour
{
	private InputField pwdItem;

	private void Start()
	{
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("checkSafeBoxPwd", HandleNetMsg_PwdCheck);
	}

	private void OnEnable()
	{
		pwdItem = base.transform.Find("Container/pwdItem/input").GetComponent<InputField>();
		pwdItem.text = string.Empty;
	}

	public void OnBtnClick_Sure()
	{
		string text = pwdItem.text;
		if (text == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? "口令不能为空" : "Please enter the safe password");
		}
		else
		{
			MB_Singleton<NetManager>.GetInstance().Send("gcsecurityService/checkSafeBoxPwd", new object[1]
			{
				text
			});
		}
	}

	private void HandleNetMsg_PwdCheck(object[] objs)
	{
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			Dictionary<string, object> dictionary2 = dictionary["safeBox"] as Dictionary<string, object>;
			ZH2_GVars.savedGameGold = (int)dictionary2["gameGold"];
			ZH2_GVars.savedLottery = (int)dictionary2["lottery"];
			base.gameObject.SetActive(value: false);
			MB_Singleton<AppManager>.GetInstance().m_mainPanel.OpenSafePanel();
			MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_SafeBoxPanel);
			return;
		}
		int num = (int)dictionary["msgCode"];
		UnityEngine.Debug.Log(num);
		switch (num)
		{
		case 2:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? "用户名不存在" : "The account does not exist");
			break;
		case 3:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? "口令输入错误，请联系上级找回" : "Order input error, please contact the superior to get it back");
			break;
		case 6:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号被冻结,请联系管理员", "This account has been frozen", "เลขบัญชีถูกระงับ", "Tài khoản bị đóng băng, vui lòng liên hệ với quản trị viên"));
			break;
		case 7:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? "系统维护" : "Server maintenance");
			break;
		case 11:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请勿重复申请", "Please do not repeat the application", "ห้ามสมัครซ้ำซ้อน", "Không lặp lại ứng dụng"));
			break;
		default:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? "输入失败" : "input failed");
			break;
		}
	}

	public void OnClosePanel()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		base.gameObject.SetActive(value: false);
	}
}
