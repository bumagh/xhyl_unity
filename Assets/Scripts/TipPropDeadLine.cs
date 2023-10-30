using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPropDeadLine : MonoBehaviour
{
	private int m_itemId;

	[SerializeField]
	private ShopPropList _shopProplist;

	private void Start()
	{
		SwitchIntoEnglish();
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("bugByGameGold", HandleNetMsg_Buy);
	}

	private void SwitchIntoEnglish()
	{
		if (ZH2_GVars.language_enum != 0)
		{
			base.transform.Find("btnSure/Text").GetComponent<Text>().text = "Buy";
		}
	}

	public void ShowUI(int itemId)
	{
		m_itemId = itemId;
		base.transform.Find("tip").GetComponent<Text>().text = string.Format((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? "您的{0}有效期仅剩一天，是否继续购买?" : "You have only one day validity of {0} , whether to buy again?", (ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? _shopProplist.list[m_itemId - 1].zh_name : _shopProplist.list[m_itemId - 1].en_name);
	}

	public void OnBtnClick_Sure()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		int num = (m_itemId <= 8) ? 2 : 3;
		MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_StorePanel, num);
	}

	public void OnBtnClick_Cancel()
	{
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
		default:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "failed" : ((ZH2_GVars.language_enum != 0) ? "ล\u0e49มเหลว" : "失败"));
			break;
		}
	}
}
