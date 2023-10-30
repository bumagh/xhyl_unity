using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundControl : MonoBehaviour
{
	[SerializeField]
	private Text title;

	[SerializeField]
	private GameObject ExchargeNum;

	[SerializeField]
	private GameObject PwdInput;

	[SerializeField]
	private Text CountdownText;

	[SerializeField]
	private GameObject cancelButton;

	[SerializeField]
	private GameObject sureNutton;

	private float runTime;

	private int totalTime;

	private bool isStartCountDown;

	private int nType;

	private void Start()
	{
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("tellServerUserPassword", HandleNetMsg_TellServerUserPassword);
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("cancelExpiryPwd", HandleNetMsg_CancelExpiryPwd);
	}

	private void Update()
	{
		if (isStartCountDown)
		{
			runTime += Time.deltaTime;
			if (runTime >= 1f)
			{
				runTime = 0f;
				totalTime--;
			}
			UnityEngine.Debug.Log("totalTime: " + totalTime);
			if (totalTime == -1)
			{
				TimeOut();
			}
			else
			{
				CountdownText.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? $"{totalTime / 60} minutes {totalTime % 60} seconds remaining" : ((ZH2_GVars.language_enum != 0) ? $"ท\u0e35\u0e48เหล\u0e37อ{totalTime / 60}นาท\u0e35{totalTime % 60}ว\u0e34นาท\u0e35" : $"剩余{totalTime / 60}分钟{totalTime % 60}秒"));
			}
		}
	}

	public void TimeOut()
	{
		CountdownText.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Timeout" : ((ZH2_GVars.language_enum != 0) ? "หมดเวลา" : "超时"));
		runTime = 0f;
		isStartCountDown = false;
		OnBtnClick_Canel();
		ResetPanel();
	}

	public void Init(int type, int gameScore)
	{
		nType = type;
		base.gameObject.SetActive(value: true);
		if (type == 0)
		{
			title.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Exchange" : ((ZH2_GVars.language_enum != 0) ? "รางว\u0e31ลการแลกเปล\u0e35\u0e48ยน" : "兑奖"));
			ExchargeNum.SetActive(value: true);
			ExchargeNum.transform.Find("Goldbg/Gold").GetComponent<Text>().text = gameScore + string.Empty;
		}
		if (type == 1)
		{
			title.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Rent tablet" : ((ZH2_GVars.language_enum != 0) ? "แท\u0e47บเล\u0e47ตให\u0e49เช\u0e48า" : "平板租借"));
			ExchargeNum.SetActive(value: false);
		}
		if (type == 2)
		{
			title.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Return tablet" : ((ZH2_GVars.language_enum != 0) ? "เอาแท\u0e47บเล\u0e47ตค\u0e37นมา" : "平板归还"));
			ExchargeNum.SetActive(value: false);
		}
		if (type == 3)
		{
			title.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Extension exchange" : ((ZH2_GVars.language_enum != 0) ? "น\u0e31กเร\u0e35ยนแลกเปล\u0e35\u0e48ยน" : "推广员兑换"));
			ExchargeNum.SetActive(value: false);
		}
		title.font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? MB_Singleton<AppManager>.Get().en_font_title : ((ZH2_GVars.language_enum != 0) ? MB_Singleton<AppManager>.Get().th_font_title : MB_Singleton<AppManager>.Get().zh_font_title));
		CountdownControl();
	}

	private void ResetPanel()
	{
		PwdInput.transform.Find("PwdInputField").GetComponent<InputField>().text = string.Empty;
		base.gameObject.SetActive(value: false);
	}

	public void OnBtnClick_Sure()
	{
		string text = PwdInput.transform.Find("PwdInputField").GetComponent<InputField>().text;
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/tellServerUserPassword", new object[2]
		{
			nType,
			text
		});
		ResetPanel();
	}

	public void OnBtnClick_Canel()
	{
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/cancelExpiryPwd", new object[1]
		{
			ZH2_GVars.user.id
		});
	}

	public void CountdownControl()
	{
		CountdownText.gameObject.SetActive(value: true);
		isStartCountDown = true;
		totalTime = 120;
		runTime = 0f;
		CountdownText.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? $"{totalTime / 60} minutes {totalTime % 60} seconds remaining" : ((ZH2_GVars.language_enum != 0) ? $"ท\u0e35\u0e48เหล\u0e37อ{totalTime / 60}นาท\u0e35{totalTime % 60}ว\u0e34นาท\u0e35" : $"剩余{totalTime / 60}分钟{totalTime % 60}秒"));
		CountdownText.font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? MB_Singleton<AppManager>.Get().en_font : ((ZH2_GVars.language_enum != 0) ? MB_Singleton<AppManager>.Get().th_font : MB_Singleton<AppManager>.Get().zh_font));
	}

	private void HandleNetMsg_TellServerUserPassword(object[] objs)
	{
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		int num = (int)dictionary["nType"];
		if ((bool)dictionary["bFlag"])
		{
			switch (num)
			{
			case 1:
				MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Tablet rental successful" : ((ZH2_GVars.language_enum != 0) ? "การเช\u0e48าแท\u0e47บเล\u0e47ตสำเร\u0e47จ" : "平板租借成功"));
				break;
			case 2:
				MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Tablet returned successfully" : ((ZH2_GVars.language_enum != 0) ? "ค\u0e37นแท\u0e47บเล\u0e47ตให\u0e49สำเร\u0e47จ " : "平板归还成功"));
				break;
			case 3:
				MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Promoter success" : ((ZH2_GVars.language_enum != 0) ? "ประสบความสำเร\u0e47จ " : "推广员成功"));
				break;
			}
		}
		else
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "password error" : ((ZH2_GVars.language_enum != 0) ? "รห\u0e31สผ\u0e48านไม\u0e48ถ\u0e39กต\u0e49อง" : "密码错误"));
		}
	}

	private void HandleNetMsg_CancelExpiryPwd(object[] objs)
	{
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["success"];
		string content = (string)dictionary["msg"];
		if (flag)
		{
			ResetPanel();
		}
		else
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog(content);
		}
	}
}
