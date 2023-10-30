using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestionItem : MonoBehaviour, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	private ChooseController chooseController;

	[SerializeField]
	private bool isChange;

	[SerializeField]
	private int questionIndex;

	private void Start()
	{
		isChange = true;
		questionIndex = int.Parse(base.name.Substring(base.name.Length - 1, 1));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (isChange)
		{
			GetComponent<Text>().color = new Color(0.98f, 0.91f, 0.57f, 1f);
		}
	}

	public void Init()
	{
		isChange = true;
		GetComponent<Text>().color = new Color(0.98f, 0.91f, 0.57f, 1f);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		MonoBehaviour.print(questionIndex);
		chooseController.InitSelf(initself: false);
		isChange = false;
		GetComponent<Text>().color = new Color(0.89f, 0.71f, 0.35f, 1f);
		chooseController.SetQuestionStr(this);
	}
}
