using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaiJiaLe_TipManager : MonoBehaviour
{
	public static BaiJiaLe_TipManager instance;

	public Text txtTip;

	public Button btnConfirm;

	public Button btnCancel;

	public int errorNum;

	public void Awake()
	{
		instance = this;
		base.gameObject.SetActive(value: false);
	}

	private void Start()
	{
		btnConfirm.onClick.AddListener(delegate
		{
			ClickBtn();
		});
		btnCancel.onClick.AddListener(delegate
		{
			ClickBtn();
		});
	}

	public void ClickBtn()
	{
		int num = errorNum;
		if (num == -1)
		{
			base.gameObject.SetActive(value: false);
			SceneManager.LoadSceneAsync(0);
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void ShowTip(string str)
	{
		errorNum = 0;
		txtTip.text = str;
		base.gameObject.SetActive(value: true);
	}

	public void ShowError(int error, string str)
	{
		errorNum = error;
		txtTip.text = str;
		base.gameObject.SetActive(value: true);
	}
}
