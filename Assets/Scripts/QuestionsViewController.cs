using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsViewController : MonoBehaviour
{
	public static QuestionsViewController instance;

	private QuestionInfo[] questions;

	public int i_security;

	[SerializeField]
	private GameObject notSet;

	[SerializeField]
	private GameObject wasSet;

	[SerializeField]
	private GameObject go_set;

	[SerializeField]
	private Text m_Q1;

	[SerializeField]
	private Text m_Q2;

	[SerializeField]
	private InputField m_answer1;

	[SerializeField]
	private InputField m_answer2;

	[SerializeField]
	private Text text_Hint;

	[SerializeField]
	private GameObject m_dropList1;

	[SerializeField]
	private GameObject m_dropList2;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Start()
	{
		i_security = ZH2_GVars.user.security;
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("SecuritySet", Handle_Questions);
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("getIfQuestion", Check_Questions);
		init();
	}

	public void init()
	{
		i_security = ZH2_GVars.user.security;
		Debug.Log("set: " + i_security);
		_ShowSafetyPanelByState(i_security);
	}

	public void Send_Questions(Dictionary<int, object> dic)
	{
		object[] args = new object[1]
		{
			dic
		};
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/SecuritySet", args);
		Debug.Log("Send_Questions");
	}

	public void Handle_Questions(object[] args)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary = (args[0] as Dictionary<string, object>);
		bool flag = (bool)dictionary["success"];
		Debug.Log("success:" + flag);
		if (flag)
		{
			i_security = 1;
			ZH2_GVars.user.security = 1;
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("密保问题设置成功", "Setting Success", "การตั้งค่าปัญหาการรับประกันสำเร็จ", "Thiết lập vấn đề bảo mật thành công"), showOkCancel: false, delegate
			{
				m_dropList1.GetComponent<ChooseController>().ResetQuestionObject();
				m_dropList2.GetComponent<ChooseController>().ResetQuestionObject();
			});
			_UpdateSafePanelState();
			base.gameObject.SetActive(value: false);
		}
		else
		{
			string content = (string)dictionary["msg"];
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(content);
		}
	}

	public void OnBtnClick_Sure()
	{
		QuestionInfo questionInfo = new QuestionInfo();
		QuestionInfo questionInfo2 = new QuestionInfo();
		questionInfo.ID = m_dropList1.GetComponent<ChooseController>().currentIndex;
		questionInfo.Question = m_Q1.text;
		questionInfo.Answer = m_answer1.text;
		questionInfo2.ID = m_dropList2.GetComponent<ChooseController>().currentIndex;
		questionInfo2.Question = m_Q2.text;
		questionInfo2.Answer = m_answer2.text;
		string value = "[{" + $"\"questionId\":\"{questionInfo.ID}\",\"question\":\"{questionInfo.Question}\",\"answer\":\"{questionInfo.Answer}\"" + "},{" + $"\"questionId\":\"{questionInfo2.ID}\",\"question\":\"{questionInfo2.Question}\",\"answer\":\"{questionInfo2.Answer}\"" + "}]";
		Dictionary<int, object> dictionary = new Dictionary<int, object>();
		dictionary.Add(0, value);
		Dictionary<int, object> dic = dictionary;
		Debug.Log("info1.ID: " + questionInfo.ID);
		Debug.Log("info2.ID: " + questionInfo2.ID);
		if (questionInfo.Question == string.Empty || questionInfo2.Question == string.Empty)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请选择密保问题", "Please choose the security question", "กรุณาเลือกประกันภัย", "Vui lòng chọn câu hỏi bảo mật"));
			return;
		}
		if (questionInfo.Question == questionInfo2.Question)
		{
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("密保问题不能相同", "The security question cannot be the same", "ปัญหาการรับประกันไม่ได้อยู่ในระดับเดียวกัน", "Vấn đề bảo mật không giống nha"));
            return;
		}
		if (questionInfo.Answer == string.Empty || questionInfo2.Answer == string.Empty)
		{
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("答案格式错误", "Answer format error", "รูปแบบคำตอบผิด", "Định dạng câu trả lời sai"));
            return;
		}
		if (questionInfo.Answer.Length > 20 || questionInfo2.Answer.Length > 20)
		{

            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("答案格式错误", "Answer format error", "รูปแบบคำตอบผิด", "Định dạng câu trả lời sai"));
            return;
		}
		Send_Questions(dic);
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		Debug.Log("问题ID：" + questionInfo.ID + "问题：" + questionInfo.Question + " 答案： " + questionInfo.Answer);
		Debug.Log("问题ID：" + questionInfo2.ID + "问题：" + questionInfo.Question + " 答案： " + questionInfo2.Answer);
	}

	private void _ShowSafetyPanelByState(int set)
	{
		if (set == 1)
		{
			WasSet();
		}
		else
		{
			NotSet();
		}
	}

	public void ResetSafePanel()
	{
		NotSet();
	}

	private void _UpdateSafePanelState()
	{
		if (i_security == 1)
		{
			WasSet();
		}
	}

	private void NotSet()
	{
		wasSet.SetActive(value: false);
		notSet.SetActive(value: true);
		go_set.SetActive(value: true);
		text_Hint.transform.localPosition = new Vector3(220f, 0f, 0f);
	}

	private void WasSet()
	{
		wasSet.SetActive(value: true);
		notSet.SetActive(value: false);
		go_set.SetActive(value: false);
		text_Hint.transform.localPosition = new Vector3(220f, -60f, 0f);
	}

	public void OnBtnClick_DropList(int index)
	{
		if (index == 1)
		{
			m_dropList1.SetActive(value: true);
			m_dropList2.SetActive(value: false);
		}
		if (index == 2)
		{
			m_dropList1.SetActive(value: false);
			m_dropList2.SetActive(value: true);
		}
	}

	public void Get_Questions()
	{
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/getIfQuestion", new object[0]);
	}

	public void Check_Questions(object[] args)
	{
		new Dictionary<string, object>();
		bool flag = (bool)(args[0] as Dictionary<string, object>)["success"];
		Debug.Log("success:" + flag.ToString());
		if (flag)
		{
			WasSet();
		}
		else
		{
			NotSet();
		}
	}
}
