using UnityEngine;
using UnityEngine.UI;

public class CashoutPanel : MonoBehaviour
{
	private Button btnClose;

	private Button btnCash;

	private InputField inputFieldOut;

	private Text txtRest;

	private Text txtOut;

	private void Awake()
	{
		btnClose = base.transform.Find("BtnClose").GetComponent<Button>();
		btnCash = base.transform.Find("BtnCashOut").GetComponent<Button>();
		inputFieldOut = base.transform.Find("ImgCashOut/InputFieldOut").GetComponent<InputField>();
		txtRest = base.transform.Find("ImgCashOut/TxtRest").GetComponent<Text>();
		txtOut = inputFieldOut.transform.Find("Text").GetComponent<Text>();
		btnClose.onClick.AddListener(ClickBtnClose);
		btnCash.onClick.AddListener(ClickBtnCash);
	}

	public void Init()
	{
		txtRest.text = string.Empty;
		txtOut.text = string.Empty;
	}

	private void ClickBtnClose()
	{
		base.gameObject.SetActive(value: false);
	}

	private void ClickBtnCash()
	{
		string text = txtOut.text;
		int num = int.Parse(text);
	}
}
