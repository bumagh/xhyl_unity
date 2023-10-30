using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecoverPanelController : MonoBehaviour
{
	public GameObject firstStep;

	public GameObject secondStep;

	public GameObject thirdStep;

	public InputField m_account;

	public InputField text_answer1;

	public InputField text_answer2;

	public Text text_Q1;

	public Text text_Q2;

	public InputField passwordText;

	public InputField repeatPasswordText;

	[SerializeField]
	private GameObject alert;

	[SerializeField]
	private Text textAlert;

	private QuestionInfo info1 = new QuestionInfo();

	private QuestionInfo info2 = new QuestionInfo();

	private string m_userName;

	private void Start()
	{
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("usernameExist", HandleNetMsg_ConfirmUserName);
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("securityCheck", HandleNetMsg_ConfirmSecurity);
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("setPwd", HandleNetMsg_ConfirmPassword);
	}

	public void OnBtnClick_Next()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		if (m_account.text == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请输入要找回的账号", "Please enter account to get back", "โปรดป้อนเลข ที่บัญชี ที่ต้องการค้นหา", "Vui lòng nhập số tài khoản cần tìm lại"));
		}
		else if (InputCheck.CheckUserName(m_account.text))
		{
			MB_Singleton<NetManager>.GetInstance().Send("gcuserService/usernameExist", new object[1]
			{
				m_account.text
			});
		}
	}

	public void OnBtnClick_Next2()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		info1.Question = text_Q1.text;
		info1.Answer = text_answer1.text;
		info2.Question = text_Q2.text;
		info2.Answer = text_answer2.text;
		Dictionary<int, object> dictionary = new Dictionary<int, object>();
		dictionary.Add(1, info1);
		dictionary.Add(2, info2);
		Dictionary<int, object> dictionary2 = dictionary;
		if (info1.Answer == string.Empty || info2.Answer == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请输入密保问题答案", "Please enter the security question answers", "โปรดป้อนคำถามของคุณมิชิมะ", "Vui lòng nhập câu trả lời cho câu hỏi bảo mật"));
            return;
		}
		MB_Singleton<NetManager>.GetInstance().Send("gcsecurityService/securityCheck", new object[2]
		{
			m_userName,
			dictionary2
		});
		Debug.Log("username：" + m_userName + "问题ID：" + info1.ID + "问题：" + info1.Question + " 答案： " + info1.Answer);
		Debug.Log("username：" + m_userName + "问题ID：" + info2.ID + "问题：" + info2.Question + " 答案： " + info2.Answer);
	}

	public void OnBtnClick_Sure()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		UnityEngine.Debug.Log("passwordText: " + passwordText.text);
		if (InputCheck.CheckReceivePassWord(passwordText.text))
		{
			if (passwordText.text == repeatPasswordText.text)
			{
				MB_Singleton<NetManager>.GetInstance().Send("gcuserService/setPwd", new object[2]
				{
					m_userName,
					passwordText.text
				});
			}
			else
			{
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("两次输入不一致", "Two input is inconsistent", "ป้อนสองครั้งไม่ตรงกัน", "Hai đầu vào không phù hợp"));
            }
		}
	}

	public void OnBtnClick_Cancel()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		MB_Singleton<AppManager>.GetInstance().GetPanel("LoginPanel").SetActive(value: true);
		m_account.text = string.Empty;
		text_answer1.text = string.Empty;
		text_answer2.text = string.Empty;
		passwordText.text = string.Empty;
		repeatPasswordText.text = string.Empty;
		firstStep.SetActive(value: true);
		secondStep.SetActive(value: false);
		thirdStep.SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	private void HandleNetMsg_ConfirmUserName(object[] objs)
	{
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			m_userName = m_account.text;
			if (!(bool)dictionary["haveSecurity"])
			{
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("该账号未设置密保问题", "The account has no security problem", "หมายเลขบัญชีนี้ ไม่มีการตั้งค่าการรับประกัน", "Tài khoản không có vấn đề bảo mật"));
                return;
			}
			JsonData jsonData = JsonMapper.ToObject((string)dictionary["security"]);
			firstStep.SetActive(value: false);
			secondStep.SetActive(value: true);
			info1.ID = int.Parse((string)jsonData[0]["questionId"]);
			info2.ID = int.Parse((string)jsonData[1]["questionId"]);
			UnityEngine.Debug.Log("info1.ID: " + info1.ID);
			UnityEngine.Debug.Log("info2.ID: " + info2.ID);
			text_Q1.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? SecurityQuestion.securityQuestion_zh[info1.ID] : SecurityQuestion.securityQuestion_en[info1.ID]);
			text_Q2.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? SecurityQuestion.securityQuestion_zh[info2.ID] : SecurityQuestion.securityQuestion_en[info2.ID]);
			text_Q1.fontSize = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? 30 : 25);
			text_Q2.fontSize = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? 30 : 25);
			text_Q1.font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? MB_Singleton<AppManager>.Get().zh_font : MB_Singleton<AppManager>.Get().en_font);
			text_Q2.font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? MB_Singleton<AppManager>.Get().zh_font : MB_Singleton<AppManager>.Get().en_font);
		}
		else
		{
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号不存在", "The account does not exist", "เลข ที่บัญชีไม่มีอยู่", "Tài khoản không tồn tại"));
        }
	}

	private void HandleNetMsg_ConfirmSecurity(object[] objs)
	{
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			secondStep.SetActive(value: false);
			thirdStep.SetActive(value: true);
			return;
		}
		int num = (int)dictionary["errorCount"];
		UnityEngine.Debug.Log("密保问题测试" + num);
		if (num < 5)
		{
			if (num >= 3)
			{
				alert.SetActive(value: true);
				textAlert.text = ZH2_GVars.ShowTip($"您已连续输入{num}次密码错误,还剩{5 - num}次操作机会", $"You've entered the wrong password {num} times ,the rest of {5 - num} chance", $"คุณได้ป้อนข้อมูลติดต่อกันแล้ว{num}รหัสผ่านไม่ถูกต้องยังมีเหลือ{5 - num}", $"Bạn đã nhập {num} mật khẩu sai liên tiếp, còn {5 - num} cơ hội hoạt động");
            }
			else
			{
				alert.SetActive(value: true);
				textAlert.text = ZH2_GVars.ShowTip("密保问题答案输入错误", "Your answer was wrong", "ตอบคำถามแบบมิทราบข้อผิดพลาด", "Câu hỏi bảo mật Câu trả lời nhập sai");
			}
			return;
		}
		int num2 = (int)dictionary["remainTime"];
		int num3 = num2 / 1000 / 3600;
		int num4 = num2 / 1000 / 60 - num3 * 60;
		int num5 = num2 / 1000 - num3 * 3600 - num4 * 60;
		UnityEngine.Debug.Log("hour: " + num3);
		UnityEngine.Debug.Log("min: " + num4);
		UnityEngine.Debug.Log("seco: " + num5);
		if (num4 == 0)
		{
			alert.SetActive(value: true);
			textAlert.text = ZH2_GVars.ShowTip("密保问题答案多次错误，请3小时后再试", "you have answered incorrectly many times  , please try it again after 3 hours", "มีข้อผิดพลาดหลายข้อ ที่จะตอบคำถามมิทราบกรุณาลองใหม่อีกครั้งในอีก 3 ชั่วโมง", "Câu trả lời sai nhiều lần, vui lòng thử lại sau 3 giờ.");
		}
		else
		{
			alert.SetActive(value: true);
			textAlert.text = ZH2_GVars.ShowTip($"密保问题答案多次错误，请{num3}小时{num4}分钟{num5}秒后重试", $"you have answered incorrectly many times, plsese try it again at {num3} hours {num4} seconds {num5} points", $"มิทราบว่า มีข้อผิดพลาดในการตอบคำถามหลายครั้งโปรด{num3}ชั่วโมง{num4}นาที{num5}ลองใหม่อีกครั้งในวินาที", $"Câu hỏi bảo mật trả lời sai nhiều lần, vui lòng thử lại sau {num3} giờ {num4} phút {num5} giây");
		}
	}

	private void HandleNetMsg_ConfirmPassword(object[] objs)
	{
		Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("密码设置成功", "Password set successfully", "การตั้งค่ารหัสผ่านสำเร็จ", "Thiết lập mật khẩu thành công"), showOkCancel: false, OnBtnClick_Cancel);
		}
		else
		{
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("密码设置失败", "Password set failed", "การตั้งค่ารหัสผ่านล้มเหลว", "Thiết lập mật khẩu thất bại"), showOkCancel: false, OnBtnClick_Cancel);
        }
	}
}
