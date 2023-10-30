using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProperSecurityPanelController : MonoBehaviour
{
	public class ReqData
	{
		public string from_id;

		public string to_id;

		public string id;

		public string bankName;

		public string money;

		public string bankAccount;

		public string name;

		public string bankOfDeposit;

		public string type;
	}

	public InputField input_BankName;

	public InputField input_BankOfDeposit;

	public InputField input_BankAccount;

	public InputField input_Name;

	public InputField input_MoneyNum;

	public Button btn_submit;

	public Button btn_close;

	public GameObject ProperSecurityWebSocket;

	private float sendImageTime = -100f;

	private void Awake()
	{
		InitUiEvent();
	}

	private void OnEnable()
	{
		base.transform.Find("Container").localScale = Vector3.zero;
		base.transform.Find("Container").DOScale(Vector3.one, 0.2f);
		input_BankName.text = PlayerPrefs.GetString("input_BankName", string.Empty);
		input_BankOfDeposit.text = PlayerPrefs.GetString("input_BankOfDeposit", string.Empty);
		input_BankAccount.text = PlayerPrefs.GetString("input_BankAccount", string.Empty);
		input_Name.text = PlayerPrefs.GetString("input_Name", string.Empty);
		input_MoneyNum.text = string.Empty;
		sendImageTime = -100f;
		if (WebSocket2.GetInstance() == null)
		{
			UnityEngine.Debug.LogError("初始化长链接");
		}
	}

	private void InitUiEvent()
	{
		btn_submit.onClick.AddListener(OnBtnSubmit_Click);
		btn_close.onClick.AddListener(OnBtnClose_Click);
	}

	private void OnBtnClose_Click()
	{
		base.gameObject.SetActive(value: false);
		if (MB_Singleton<AppManager>.Get().isShowAnnouce)
		{
			MB_Singleton<AppManager>.Get().ShowNotice();
			MB_Singleton<AppManager>.Get().isShowAnnouce = false;
		}
	}

	private void OnBtnSubmit_Click()
	{
		string text = input_BankName.text;
		string text2 = input_BankAccount.text;
		string text3 = input_Name.text;
		string text4 = input_BankOfDeposit.text;
		string text5 = input_MoneyNum.text;
		if (text == string.Empty)
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Bank name cannot be blank" : ((ZH2_GVars.language_enum != 0) ? "ช\u0e37\u0e48อธนาคารไม\u0e48ว\u0e48า ง" : "银行名称不能为空"));
			return;
		}
		if (text2 == string.Empty)
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Bank account number cannot be blank" : ((ZH2_GVars.language_enum != 0) ? "บ\u0e31ญช\u0e35ธนาคารไม\u0e48สามารถว\u0e48า งเปล\u0e48าได\u0e49" : "银行账号不能为空"));
			return;
		}
		if (text3 == string.Empty)
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "The name cannot be empty" : ((ZH2_GVars.language_enum != 0) ? "ช\u0e37\u0e48อไม\u0e48สามารถว\u0e48า งไว\u0e49ได\u0e49 " : "姓名不能为空"));
			return;
		}
		if (text5 == string.Empty || int.Parse(text5) == 0 || int.Parse(text5) < 50)
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Error in quota input" : ((ZH2_GVars.language_enum != 0) ? "ระด\u0e31บเส\u0e35ยงไม\u0e48ถ\u0e39กต\u0e49อง" : "额度输入有误,不得低于50"));
			return;
		}
		if (int.Parse(text5) > ZH2_GVars.user.gameGold)
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Insufficient quota" : ((ZH2_GVars.language_enum != 0) ? "ไม\u0e48ม\u0e35ระด\u0e31บ" : "账户余额不足"));
			return;
		}
		if (Time.time - sendImageTime < 5f)
		{
			int num = Mathf.CeilToInt(5f - (Time.time - sendImageTime));
			MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip($"操作过于频繁,请于 {num} 秒后发送兑换!", $"The operation is too frequent. Please send the exchange in {num} second!", string.Empty));
			return;
		}
		sendImageTime = Time.time;
		ReqData reqData = new ReqData();
		reqData.id = ZH2_GVars.user.id.ToString();
		reqData.from_id = ZH2_GVars.user.id.ToString();
		reqData.to_id = ZH2_GVars.user.promoterId.ToString();
		reqData.money = text5;
		reqData.bankName = text;
		reqData.bankAccount = text2;
		reqData.name = text3;
		reqData.bankOfDeposit = text4;
		reqData.type = "2";
		int num2 = 0;
		try
		{
			num2 = int.Parse(text5);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		ZH2_GVars.user.gameGold -= num2;
		string msg = JsonUtility.ToJson(reqData);
		WebSocket2.GetInstance().SendMsg(msg, isDuiHuan: true, num2);
		input_MoneyNum.text = string.Empty;
		PlayerPrefs.SetString("input_BankName", text);
		PlayerPrefs.SetString("input_BankOfDeposit", text4);
		PlayerPrefs.SetString("input_BankAccount", text2);
		PlayerPrefs.SetString("input_Name", text3);
		PlayerPrefs.Save();
	}

	private void CheckInput(string bankName)
	{
		if (bankName.IndexOf('-') != -1)
		{
			bankName = bankName.Trim('-');
			input_BankName.text = bankName;
		}
	}

	private void HandleNetMsg_UpdateAccountProtectionNumber(object[] args)
	{
		IDictionary dictionary = (IDictionary)args[0];
		if ((bool)dictionary["success"])
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Submitted successfully, thank you for your participation" : ((ZH2_GVars.language_enum != 0) ? "ขอบค\u0e38ณสำหร\u0e31บความสำเร\u0e47จของค\u0e38ณ" : "提交成功，感谢您的参与"), showOkCancel: false, delegate
			{
				base.gameObject.SetActive(value: false);
			});
			MB_Singleton<AppManager>.Get().Notify(UIGameMsgType.UINotify_Fresh_ProperSecuritySign, false);
		}
		else
		{
			int num = (int)dictionary["msgCode"];
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "请输入正确的手机号码" : ((ZH2_GVars.language_enum != 0) ? "กร\u0e38ณาใส\u0e48เบอร\u0e4cโทร ท\u0e35\u0e48ถ\u0e39กต\u0e49อง " : "请输入正确的手机号码"));
		}
	}
}
