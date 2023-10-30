using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashOperatePanelController : MonoBehaviour
{
	private float m_remaintime;

	private Text m_textMinute;

	private Text m_textSecond;

	private void Start()
	{
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("excharge", HandleNetMsg_Excharge);
		m_textMinute = base.transform.Find("remaintimeItem/minute/num").GetComponent<Text>();
		m_textSecond = base.transform.Find("remaintimeItem/second/num").GetComponent<Text>();
	}

	private void OnEnable()
	{
		ResetView();
	}

	private void SwitchIntoEnglish()
	{
		if (ZH2_GVars.language_enum != 0)
		{
			base.transform.Find("titleBg/title").GetComponent<Text>().text = "Exchange";
			base.transform.Find("remaintimeItem/itemName").GetComponent<Text>().text = "countdown:";
			base.transform.Find("remaintimeItem/itemName").GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 25);
			base.transform.Find("remaintimeItem/itemName").GetComponent<Text>().fontSize = 45;
			base.transform.Find("cashItem/itemName").GetComponent<Text>().text = "Amount:";
			base.transform.Find("cashItem/itemName").GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 25);
			base.transform.Find("cashItem/itemName").GetComponent<Text>().fontSize = 45;
			base.transform.Find("password/itemName").GetComponent<Text>().text = "Enter password:";
			base.transform.Find("password/itemName").GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 25);
			base.transform.Find("password/itemName").GetComponent<Text>().fontSize = 40;
			base.transform.Find("password/input/Placeholder").GetComponent<Text>().text = "Enter password";
			base.transform.Find("password/input/Placeholder").GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 25);
			base.transform.Find("password/input/Placeholder").GetComponent<Text>().fontSize = 30;
			base.transform.Find("sureBtn/Text").GetComponent<Text>().text = "Confirm";
		}
	}

	private void FixedUpdate()
	{
		if (m_remaintime > 0f)
		{
			m_remaintime -= Time.deltaTime;
			if (m_remaintime < 0f)
			{
				m_remaintime = 0f;
			}
			m_textMinute.text = $"{Mathf.FloorToInt(m_remaintime / 60f):D2}";
			m_textSecond.text = $"{Mathf.FloorToInt(m_remaintime % 60f):D2}";
		}
	}

	private void ResetView()
	{
		base.transform.Find("password/input").GetComponent<InputField>().text = string.Empty;
		m_remaintime = 120f;
	}

	public void OnBtnClick_Close()
	{
		base.gameObject.SetActive(value: false);
	}

	public void OnBtnClick_Sure()
	{
		string text = base.transform.Find("cashItem/num").GetComponent<InputField>().text;
		string text2 = base.transform.Find("password/input").GetComponent<InputField>().text;
		if (text2 == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please enter password" : ((ZH2_GVars.language_enum != 0) ? "โปรดป\u0e49อนรห\u0e31สผ\u0e48าน" : "请输入密码"));
		}
		else if (m_remaintime <= 0f)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "time out" : ((ZH2_GVars.language_enum != 0) ? "การปฏ\u0e34บ\u0e31ต\u0e34การหมดเวลา" : "操作超时"));
		}
		else
		{
			MB_Singleton<NetManager>.GetInstance().Send("gcuserService/excharge", new object[2]
			{
				Convert.ToInt32(text),
				text2
			});
		}
	}

	private void HandleNetMsg_Excharge(object[] objs)
	{
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		UnityEngine.Debug.Log(dictionary);
		if ((bool)dictionary["success"])
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Cash success" : ((ZH2_GVars.language_enum != 0) ? "การแลกเปล\u0e35\u0e48ยนสำเร\u0e47จ" : "兑换成功"));
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
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "password error" : ((ZH2_GVars.language_enum != 0) ? "รห\u0e31สผ\u0e48านไม\u0e48ถ\u0e39กต\u0e49อง" : "密码错误"));
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
		default:
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Cash failed" : ((ZH2_GVars.language_enum != 0) ? "การแลกเปล\u0e35\u0e48ยนล\u0e49มเหลว" : "兑换失败"));
			break;
		}
	}
}
