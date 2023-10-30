using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipBuyGameCurrency : MonoBehaviour
{
	private int m_itemIndex;

	private void Start()
	{
		SwitchIntoEnglish();
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("bugByGameGold", HandleNetMsg_Buy);
	}

	private void SwitchIntoEnglish()
	{
		if (ZH2_GVars.language_enum != 0)
		{
			base.transform.Find("btnSure/Text").GetComponent<Text>().text = "Sure";
			base.transform.Find("btnClose/Text").GetComponent<Text>().text = "Cancel";
		}
	}

	public void ShowUI(int itemIndex)
	{
		m_itemIndex = itemIndex;
		base.transform.Find("tip").GetComponent<Text>().text = string.Format((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Confirm to buy {0} coins?" : ((ZH2_GVars.language_enum != 0) ? "ย\u0e37นย\u0e31นการซ\u0e37\u0e49อ{0}เหร\u0e35ยญเหรอ？" : "确认购买{0}游戏币吗？"), Mathf.Pow(10f, itemIndex));
	}

	public void OnBtnClick_Sure()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		MB_Singleton<NetManager>.GetInstance().Send("gcshopService/bugByGameGold", new object[3]
		{
			m_itemIndex,
			1,
			string.Empty
		});
	}

	public void OnBtnClick_Cancel()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void HandleNetMsg_Buy(object[] objs)
	{
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		UnityEngine.Debug.Log(dictionary);
		if ((bool)dictionary["success"])
		{
			UnityEngine.Object.Destroy(base.gameObject);
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please wait for the system process" : ((ZH2_GVars.language_enum != 0) ? "กร\u0e38ณารอการประมวลผลระบบ " : "请等候系统进行处理"));
			return;
		}
		int num = (int)dictionary["msgCode"];
		UnityEngine.Debug.Log(num);
		switch (num)
		{
		case 2:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
			break;
		case 3:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
			break;
		default:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "failed" : ((ZH2_GVars.language_enum != 0) ? "ล\u0e49มเหลว" : "失败"));
			break;
		case 22:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Background did not open this feature" : ((ZH2_GVars.language_enum != 0) ? "ไม\u0e48ม\u0e35การเป\u0e34ดใช\u0e49งานค\u0e38ณสมบ\u0e31ต\u0e34น\u0e35\u0e49 หล\u0e31งเวท\u0e35 " : "后台未开启此项功能"));
			break;
		case 6:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号被冻结,请联系管理员", "This account has been frozen", "เลขบัญชีถูกระงับ", "Tài khoản bị đóng băng, vui lòng liên hệ với quản trị viên"));
			break;
		case 7:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Server maintenance" : ((ZH2_GVars.language_enum != 0) ? "การด\u0e39แลระบบ" : "系统维护"));
			break;
		case 11:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请勿重复申请", "Please do not repeat the application", "ห้ามสมัครซ้ำซ้อน", "Không lặp lại ứng dụng"));
			break;
		case 12:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("余额不足", "balance", "ยอดคงเหลือไม่เพียงพอ", "Số dư không đủ"));
			break;
		}
	}
}
