using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CompleteItem : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	private CompleteController CompleteController;

	private bool isChange;

	private int questionIndex;

	private Color tpColor;

	private Color color1;

	private Color color2;

	private Color color3;

	private void Start()
	{
		isChange = true;
		questionIndex = int.Parse(base.name.Substring(base.name.Length - 1, 1));
		tpColor = new Color(1f, 1f, 1f, 0f);
		color1 = new Color(0.89f, 0.71f, 0.35f, 1f);
		color2 = new Color(0.98f, 0.91f, 0.57f, 1f);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		UnityEngine.Debug.Log("OnPointerEnter");
		GetComponent<Text>().color = color1;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		UnityEngine.Debug.Log("OnPointerExit");
		if (isChange)
		{
			GetComponent<Text>().color = color2;
		}
		UnityEngine.Debug.Log("isChange: " + isChange);
	}

	public void Init()
	{
		isChange = true;
		GetComponent<Text>().color = color2;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		UnityEngine.Debug.Log("OnPointerClick");
		CompleteController.InitSelf();
		isChange = false;
		GetComponent<Text>().color = color1;
		CompleteController.SetQuestionStr(this);
	}
}
