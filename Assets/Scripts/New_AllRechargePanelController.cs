using LitJson;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class New_AllRechargePanelController : MonoBehaviour
{
	private InputField pwdItem;

	private InputField noteItem;

	private Button buttonOk;

	private void Awake()
	{
		pwdItem = base.transform.Find("pwdItem/input").GetComponent<InputField>();
		noteItem = base.transform.Find("noteItem/input").GetComponent<InputField>();
		buttonOk = base.transform.Find("sureBtn").GetComponent<Button>();
		buttonOk.onClick.AddListener(OnBtnClick_Sure);
	}

	private void OnEnable()
	{
		ResetView();
	}

	private void ResetView()
	{
		pwdItem.text = string.Empty;
		noteItem.text = string.Empty;
	}

	public void OnBtnClick_Sure()
	{
		All_TipCanvas.GetInstance().SourcePlayClip();
		string text = pwdItem.text;
		if (text == string.Empty)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("请填入数字", "Please fill in the numbers", "กร\u0e38ณาเต\u0e34มต\u0e31วเลข", "Vui lòng điền số"));
			return;
		}
		int num = Convert.ToInt32(text);
		string text2 = noteItem.text;
		if (num > 100000 || num < 1)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("充值额度范围为：1-100000", "Recharge limit:1-10000", "ระยะวงโคจร ท\u0e35\u0e48ชาร\u0e4cจอย\u0e39\u0e48ค\u0e37อ 1-10000", "Phạm vi nạp tiền là: 1-100000"));
			return;
		}
		Hashtable hashtable = new Hashtable();
		hashtable.Add("userId", ZH2_GVars.userId);
		hashtable.Add("gameId", ZH2_GVars.allSetType);
		hashtable.Add("password", ZH2_GVars.pwd);
		hashtable.Add("amount", num);
		hashtable.Add("remark", text2);
		Hashtable obj = hashtable;
		StartCoroutine(GetRecharge(ZH2_GVars.EncodeMessage(obj)));
	}

	private IEnumerator GetRecharge(string msg)
	{
		string url2 = ZH2_GVars.shortConnection + "recharge";
		url2 = url2 + "?jsonStr=" + msg;
		UnityWebRequest www = UnityWebRequest.Get(url2);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error == null)
		{
			string text = www.downloadHandler.text;
			if (text != string.Empty)
			{
				text = ZH2_GVars.DecodeMessage(text);
				JsonData dictionary = JsonMapper.ToObject(text);
				HandleNetMsg_Recharge(dictionary);
			}
			else
			{
				ShowError();
			}
		}
		else
		{
			Debug.LogError("===访问错误===: " + www.error);
			ShowError();
		}
	}

	private void ShowError()
	{
		string tips = ZH2_GVars.ShowTip("系统错误", "System failure", "ข้อผิดพลาดของระบบ", "Lỗi hệ thống");
		All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
	}

	private void HandleNetMsg_Recharge(JsonData dictionary)
	{
		ResetView();
		Debug.LogError("充值：" + dictionary.ToJson());
		if ((bool)dictionary["success"])
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("发送请求成功!", "Send Successful!", "ส่งคำขอสำเร็จ!", "Gửi yêu cầu thành công!"));
			return;
		}
		int num = (int)dictionary["msg"];
		if (num == -2)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("请勿重复申请", "Please do not repeat the application", "ไม่ต้องทำการสมัครซ้ำ", "Không lặp lại ứng dụng"));
		}
		else
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("请求失败,请稍后再试", "Request failed, please try again later", "ล้มเหลว", "Yêu cầu không thành công, vui lòng thử lại sau"));
		}
	}
}
