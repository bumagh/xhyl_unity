using UnityEngine;
using UnityEngine.UI;

public class ShareSetRakebake : MonoBehaviour
{
	private Text txtID;

	private Text txtName;

	private InputField inputField;

	private Text txtInputField;

	private Button btnClose;

	private Button btnConfirm;

	private void Awake()
	{
		txtID = base.transform.Find("ImgSet/TxtID").GetComponent<Text>();
		txtName = base.transform.Find("ImgSet/TxtNickname").GetComponent<Text>();
		inputField = base.transform.Find("ImgSet/InputField").GetComponent<InputField>();
		txtInputField = inputField.transform.Find("Text").GetComponent<Text>();
		btnClose = base.transform.Find("ImgSet/BtnClose").GetComponent<Button>();
		btnConfirm = base.transform.Find("ImgSet/BtnConfirm").GetComponent<Button>();
		btnClose.onClick.AddListener(ClickBtnClose);
		btnConfirm.onClick.AddListener(ClickBtnConfirm);
	}

	public void Init(string id, string name)
	{
		txtID.text = id;
		txtName.text = name;
		txtInputField.text = string.Empty;
	}

	private void ClickBtnClose()
	{
		base.gameObject.SetActive(value: false);
	}

	private void ClickBtnConfirm()
	{
		string text = txtInputField.text;
	}
}
