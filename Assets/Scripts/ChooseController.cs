using UnityEngine;
using UnityEngine.UI;

public class ChooseController : MonoBehaviour
{
	[SerializeField]
	public Text Placeholder;

	[SerializeField]
	public Text questionText;

	[SerializeField]
	public InputField answerText;

	[SerializeField]
	private QuestionItem[] current_item;

	[SerializeField]
	private QuestionItem[] other_item;

	[SerializeField]
	private string QuestionStr = string.Empty;

	[SerializeField]
	public int currentIndex;

	public void InitSelf(bool initself)
	{
		QuestionItem[] array = current_item;
		foreach (QuestionItem questionItem in array)
		{
			if (initself)
			{
				questionItem.Init();
				questionItem.enabled = true;
			}
			else if (questionItem.enabled)
			{
				questionItem.Init();
			}
		}
	}

	public void ResetQuestionObject()
	{
		InitSelf(initself: true);
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
			for (int i = 0; i < current_item.Length; i++)
			{
				if (current_item[i].transform.GetComponent<Text>().text == QuestionStr)
				{
					currentIndex = i;
				}
			}
			questionText.text = QuestionStr;
			if (ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese)
			{
				questionText.fontSize = 30;
			}
			else
			{
				questionText.fontSize = 23;
			}
			QuestionItem[] array = other_item;
			foreach (QuestionItem questionItem in array)
			{
				questionItem.Init();
				questionItem.enabled = true;
			}
			other_item[currentIndex].enabled = false;
			other_item[currentIndex].GetComponent<Text>().color = new Color(0.74f, 0.74f, 0.74f, 0.63f);
			Placeholder.gameObject.SetActive(value: false);
		}
		else
		{
			Placeholder.gameObject.SetActive(value: true);
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请选择密保问题", "Please choose the security question", "กรุณาเลือกประกันภัย", "Vui lòng chọn câu hỏi bảo mật"));
		}
	}

	public void SetQuestionStr(QuestionItem qi)
	{
		QuestionStr = qi.GetComponent<Text>().text;
	}
}
