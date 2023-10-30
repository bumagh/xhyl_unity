using LitJson;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class New_AllSafeBoxPwdCheckPanel : MonoBehaviour
{
	private InputField pwdItem;

	private Button btnSure;

	private Button btnClose;

	private string url = string.Empty;

	private Tween_SlowAction tween_SlowAction;

	private void Awake()
	{
		btnSure = base.transform.Find("Container/sureBtn").GetComponent<Button>();
		btnSure.onClick.AddListener(OnBtnClick_Sure);
		btnClose = base.transform.Find("Container/BtnClose").GetComponent<Button>();
		btnClose.onClick.AddListener(OnBtnClick_Close);
		tween_SlowAction = GetComponent<Tween_SlowAction>();
	}

	private void OnEnable()
	{
		if ((object)tween_SlowAction != null)
		{
			tween_SlowAction.Show();
		}
		pwdItem = base.transform.Find("Container/pwdItem/input").GetComponent<InputField>();
		pwdItem.text = string.Empty;
		url = ZH2_GVars.shortConnection + "verifySafeBoxPassword";
		if (ZH2_GVars.saveScore != null)
		{
			ZH2_GVars.saveScore();
		}
	}

	public void OnBtnClick_Sure()
	{
		All_TipCanvas.GetInstance().SourcePlayClip();
		string text = pwdItem.text;
		if (text == string.Empty)
		{
			string tips = ZH2_GVars.ShowTip("密码不能为空", "Please enter the safe password", "รหัสผ่านต้องไม่ว่างเปล่า", "Mật khẩu không được để trống");
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
		}
		else
		{
			SenCheckSafeBoxPwd(int.Parse(text));
		}
	}

	public void OnBtnClick_Close()
	{
		if (ZH2_GVars.closeSafeBox != null)
		{
			ZH2_GVars.closeSafeBox();
		}
		if ((object)tween_SlowAction != null)
		{
			tween_SlowAction.Hide(base.gameObject);
		}
	}

	private void SenCheckSafeBoxPwd(int safeBoxPassword)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("userId", ZH2_GVars.userId);
		hashtable.Add("gameId", ZH2_GVars.allSetType);
		hashtable.Add("password", ZH2_GVars.pwd);
		hashtable.Add("safeBoxPassword", safeBoxPassword);
		Hashtable obj = hashtable;
		ZH2_GVars.safeBoxPassword = safeBoxPassword;
		StartCoroutine(GetService(url, ZH2_GVars.EncodeMessage(obj)));
	}

	private IEnumerator GetService(string url, string msg)
	{
		url = url + "?jsonStr=" + msg;
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			string text = www.downloadHandler.text;
			if (text != string.Empty)
			{
				text = ZH2_GVars.DecodeMessage(text);
				JsonData dictionary = JsonMapper.ToObject(text);
				HandleNetMsg_PwdCheck(dictionary);
			}
			else
			{
				ShowError();
			}
		}
		else
		{
			UnityEngine.Debug.LogError("===访问错误===: " + www.error + "\n" + url);
			ShowError();
		}
	}

	private void ShowError()
	{
		string tips = ZH2_GVars.ShowTip("系统错误", "System failure", "ข้อผิดพลาดของระบบ", "Lỗi hệ thống");
		All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
	}

	private void HandleNetMsg_PwdCheck(JsonData dictionary)
	{
		UnityEngine.Debug.LogError("保险柜密码：" + dictionary.ToJson());
		if ((bool)dictionary["success"])
		{
			ZH2_GVars.savedGameGold = (int)dictionary["safeBoxGold"];
			ZH2_GVars.gameGold = (int)dictionary["gameGold"];
			if (ZH2_GVars.user != null)
			{
				ZH2_GVars.user.gameGold = ZH2_GVars.gameGold;
			}
			base.gameObject.SetActive(value: false);
			if (ZH2_GVars.OpenSafeBoxPwdPanel != null)
			{
				ZH2_GVars.OpenSafeBoxPwdPanel();
				return;
			}
			string tips = ZH2_GVars.ShowTip("未知错误,请联系客服", "Unknown error, please contact customer service", "ข้อผิดพลาดที่ไม่รู้จักโปรดติดต่อฝ่ายบริการลูกค้า", "Không rõ lỗi, vui lòng liên hệ");
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
			return;
		}
		switch ((int)dictionary["msg"])
		{
		case 1:
		{
			string tips2 = ZH2_GVars.ShowTip("密码格式错误", "Password format error", "รูปแบบรหัสผ่านไม่ถูกต้อง", "Định dạng mật khẩu sai");
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips2);
			break;
		}
		case -2:
		{
			string tips2 = ZH2_GVars.ShowTip("密码错误", "Password Error", "รหัสผ่านไม่ถูกต้อง", "Mật khẩu sai");
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips2);
			break;
		}
		default:
			ShowError();
			break;
		}
	}
}
