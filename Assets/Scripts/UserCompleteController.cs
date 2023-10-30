using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserCompleteController : MonoBehaviour
{
	[SerializeField]
	private GameObject m_alert;

	[SerializeField]
	private GameObject m_complete;

	[SerializeField]
	private GameObject m_chooseBox;

	[SerializeField]
	private Toggle m_toggleNoPrompt;

	[SerializeField]
	private InputField m_textIDNumber;

	[SerializeField]
	private Text m_textQuestion;

	[SerializeField]
	private InputField m_textAnswer;

	[SerializeField]
	public ArrayList FromArgs;

	public static bool UserCompleteNoPrompt;

	public static bool UserTempNoPrompt;

	public static int fromType;

	private void Start()
	{
		m_alert.SetActive(value: true);
		m_complete.SetActive(value: false);
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("addUserInfo", HandleNetMsg_addUserInfo);
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("notShowUserInfo", HandleNetMsg_notShowUserInfo);
	}

	public void ToggleNoPrompt()
	{
		UnityEngine.Debug.Log(LogHelper.Yellow("m_toggleNoPrompt.isOn: " + m_toggleNoPrompt.isOn));
		UserCompleteNoPrompt = m_toggleNoPrompt.isOn;
	}

	private void HandleNetMsg_notShowUserInfo(object[] obj)
	{
		Dictionary<string, object> dictionary = obj[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			ZH2_GVars.user.card = "1";
		}
	}

	private void HandleNetMsg_addUserInfo(object[] obj)
	{
		Dictionary<string, object> dictionary = obj[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			ZH2_GVars.user.card = "1";
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip( "资料完善成功！", "Successfully completed the information!", "ความสมบูรณ์แบบของข้อมูลที่ประสบความสำเร็จ!", "Tư liệu hoàn thiện thành công!"), showOkCancel: false, OnBtnClick_Close);
			base.gameObject.SetActive(value: false);
		}
	}

	public void OnBtnClick_QuitComplete()
	{
		base.gameObject.SetActive(value: false);
		if (fromType == 1)
		{
			if (ZH2_GVars.payMode == 1)
			{
				UserTempNoPrompt = true;
			}
			else
			{
				MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_RechargePanel);
			}
		}
		else if (fromType == 3)
		{
			UserTempNoPrompt = true;
		}
		else if (fromType == 2)
		{
			MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_CashPanel);
		}
	}

	public void OnBtnClick_SureComplete()
	{
		if (UserCompleteNoPrompt)
		{
			MB_Singleton<NetManager>.GetInstance().Send("gcuserService/notShowUserInfo", new object[1]
			{
				"1"
			});
		}
		m_alert.SetActive(value: false);
		m_complete.SetActive(value: true);
	}

	public void OnBtnClick_Choose()
	{
		m_chooseBox.SetActive(value: true);
	}

	public void OnBtnClick_Skip()
	{
		UserCompleteNoPrompt = true;
		OnBtnClick_Close();
	}

	public void OnBtnClick_Sure()
	{
		if (m_textIDNumber.text == string.Empty)
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("请输入身份证号", "Please enter the ID number", "กรุณาใส่เลขบัตรประจำตัว", "Vui lòng nhập số ID"));
		}
		else if (m_textQuestion.text == string.Empty)
		{
            MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("请选择安全问题", "Please choose the security question", "โปรดเลือกคำถามด้านความปลอดภัย", "Vui lòng chọn câu hỏi bảo mật"));
        }
		else if (m_textAnswer.text == string.Empty)
		{
            MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("答案不能为空", "The answer cannot be empty", "คำตอบคือ ไม่ว่า ง", "Câu trả lời không được để trống."));
        }
		else
		{
			MB_Singleton<NetManager>.GetInstance().Send("gcuserService/addUserInfo", new object[3]
			{
				m_textIDNumber.text,
				m_textQuestion.text,
				m_textAnswer.text
			});
		}
	}

	public void OnBtnClick_Close()
	{
		m_alert.SetActive(value: true);
		m_complete.SetActive(value: false);
		base.gameObject.SetActive(value: false);
		m_textIDNumber.text = string.Empty;
		m_chooseBox.GetComponentInChildren<CompleteController>().ResetQuestionObject();
		if (fromType == 1)
		{
			if (ZH2_GVars.payMode == 1)
			{
				UserTempNoPrompt = true;
			}
			else
			{
				MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_RechargePanel);
			}
		}
		else if (fromType == 3)
		{
			UserTempNoPrompt = true;
		}
		else if (fromType == 2)
		{
			MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_CashPanel);
		}
	}
}
