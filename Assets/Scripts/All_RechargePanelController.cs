using JsonFx.Json;
using LitJson;
using System;
using UnityEngine;
using UnityEngine.UI;

public class All_RechargePanelController : MonoBehaviour
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
		ZH2_GVars.getGamePay = (Action<object[]>)Delegate.Combine(ZH2_GVars.getGamePay, new Action<object[]>(HandleNetMsg_Recharge));
	}

	private void OnDisable()
	{
		ZH2_GVars.getGamePay = (Action<object[]>)Delegate.Remove(ZH2_GVars.getGamePay, new Action<object[]>(HandleNetMsg_Recharge));
	}

	private void ResetView()
	{
		pwdItem.text = string.Empty;
		noteItem.text = string.Empty;
	}

	public void OnBtnClick_Sure()
	{
		string text = pwdItem.text;
		if (text == string.Empty)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("请填入数字", "Please fill in the numbers", "กร\u0e38ณาเต\u0e34มต\u0e31วเลข"));
			return;
		}
		int num = Convert.ToInt32(text);
		string text2 = noteItem.text;
		if (num > 100000 || num < 1)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("充值额度范围为：1-100000", "Recharge limit:1-10000", "ระยะวงโคจร ท\u0e35\u0e48ชาร\u0e4cจอย\u0e39\u0e48ค\u0e37อ 1-10000"));
		}
		else if (ZH2_GVars.sendGamePay == null)
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("暂不支持,请前往大厅充值", "Not currently supported, please go to the lobby to recharge", "ล\u0e49มเหลว"));
		}
		else
		{
			ZH2_GVars.sendGamePay(new object[3]
			{
				ZH2_GVars.userId,
				num,
				text2
			});
		}
	}

	private void HandleNetMsg_Recharge(object[] objs)
	{
		ResetView();
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		if ((bool)jsonData["success"])
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("发送请求成功!", "Send Successful!", "ส\u0e48งสำเร\u0e47จ"));
			return;
		}
		int num = 3;
		try
		{
			num = (int)jsonData["msgCode"];
		}
		catch
		{
		}
		switch (num)
		{
		case 2:
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("系统禁止充值", "The system prohibits recharging", "ล\u0e49มเหลว"));
			break;
		case 3:
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("请勿重复申请", "Please do not repeat the application", "ห้ามสมัครซ้ำซ้อน", "Không lặp lại ứng dụng"));
			break;
		default:
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("请求失败,请稍后再试", "Request failed, please try again later", "ล\u0e49มเหลว"));
			break;
		}
	}
}
