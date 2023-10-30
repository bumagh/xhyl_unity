using UnityEngine;
using UnityEngine.UI;

public class XLDT_DlgChatCommonUse : MonoBehaviour
{
	[HideInInspector]
	public Button[] btnCommonUse;

	[HideInInspector]
	public Text[] txtCommonUse;

	[HideInInspector]
	public string[] words = new string[11];

	private void Awake()
	{
		Init();
	}

	public void Init()
	{
		btnCommonUse = new Button[11];
		txtCommonUse = new Text[11];
		for (int i = 0; i < 11; i++)
		{
			btnCommonUse[i] = base.transform.GetChild(i).GetComponent<Button>();
			txtCommonUse[i] = btnCommonUse[i].transform.Find("Text").GetComponent<Text>();
			words[i] = XLDT_Localization.Get($"DlgChatCommonPhrase{i + 1}");
			txtCommonUse[i].text = words[i];
		}
	}

	public void SetBtnsInteractable(bool b)
	{
		for (int i = 0; i < 11; i++)
		{
			btnCommonUse[i].interactable = b;
		}
	}
}
