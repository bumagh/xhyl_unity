using UnityEngine;
using UnityEngine.UI;

public class CompleteController : MonoBehaviour
{
	[SerializeField]
	public Text Placeholder;

	[SerializeField]
	public Text questionText;

	[SerializeField]
	public InputField answerText;

	[SerializeField]
	private CompleteItem[] current_item;

	private string QuestionStr = string.Empty;

	public void InitSelf()
	{
		CompleteItem[] array = current_item;
		foreach (CompleteItem completeItem in array)
		{
			completeItem.Init();
		}
	}

	public void ResetQuestionObject()
	{
		InitSelf();
		Placeholder.gameObject.SetActive(value: true);
		questionText.text = string.Empty;
		questionText.gameObject.SetActive(value: false);
		answerText.text = string.Empty;
		QuestionStr = string.Empty;
	}

	public void ChooseQuestion()
	{
		if (QuestionStr != string.Empty)
		{
			questionText.text = QuestionStr;
			if (ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese)
			{
				questionText.fontSize = 30;
			}
			else
			{
				questionText.fontSize = 23;
			}
			Placeholder.gameObject.SetActive(value: false);
		}
		else
		{
			Placeholder.gameObject.SetActive(value: true);
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请选择安全问题", "Please choose the security question", "โปรดเลือกคำถามด้านความปลอดภัย", "Vui lòng chọn câu hỏi bảo mật"));
		}
	}

	public void SetQuestionStr(CompleteItem qi)
	{
		QuestionStr = qi.GetComponent<Text>().text;
	}
}
