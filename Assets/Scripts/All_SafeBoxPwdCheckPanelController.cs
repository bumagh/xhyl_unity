using JsonFx.Json;
using LitJson;
using System;
using UnityEngine;
using UnityEngine.UI;

public class All_SafeBoxPwdCheckPanelController : MonoBehaviour
{
	private InputField pwdItem;

	private Transform safeBoxPanel;

	private void OnEnable()
	{
		pwdItem = base.transform.Find("Container/pwdItem/input").GetComponent<InputField>();
		safeBoxPanel = base.transform.parent.Find("SafeBoxPanel");
		pwdItem.text = string.Empty;
		ZH2_GVars.getCheckSafeBoxPwd = (Action<object[]>)Delegate.Combine(ZH2_GVars.getCheckSafeBoxPwd, new Action<object[]>(HandleNetMsg_PwdCheck));
	}

	private void OnDisable()
	{
		ZH2_GVars.getCheckSafeBoxPwd = (Action<object[]>)Delegate.Remove(ZH2_GVars.getCheckSafeBoxPwd, new Action<object[]>(HandleNetMsg_PwdCheck));
	}

	public void OnBtnClick_Sure()
	{
		string text = pwdItem.text;
		if (text == string.Empty)
		{
			string tips = ZH2_GVars.ShowTip("密码不能为空", "Please enter the safe password", string.Empty);
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
		}
		else
		{
			ZH2_GVars.sendCheckSafeBoxPwd(new object[2]
			{
				ZH2_GVars.userId,
				text
			});
		}
	}

	private void HandleNetMsg_PwdCheck(object[] objs)
	{
		JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
		if ((bool)jsonData["success"])
		{
			JsonData jsonData2 = jsonData["safeBox"];
			ZH2_GVars.savedGameGold = (int)jsonData2["gameGold"];
			ZH2_GVars.savedLottery = (int)jsonData2["lottery"];
			ZH2_GVars.gameGold = (int)jsonData["gameGold"];
			base.gameObject.SetActive(value: false);
			if (safeBoxPanel != null)
			{
				safeBoxPanel.gameObject.SetActive(value: true);
				return;
			}
			string tips = ZH2_GVars.ShowTip("未知错误,请联系客服", "Unknown error, please contact customer service", string.Empty);
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips);
			return;
		}
		int num = (int)jsonData["msgCode"];
		UnityEngine.Debug.Log(num);
		switch (num)
		{
		case 1:
		{
			string tips3 = ZH2_GVars.ShowTip("密码格式错误", "Password format error", string.Empty);
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips3);
			break;
		}
		case 2:
		{
			string tips3 = ZH2_GVars.ShowTip("密码错误", "Password Error", string.Empty);
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips3);
			break;
		}
		default:
		{
			string tips2 = ZH2_GVars.ShowTip("请求超时", "Request timeout", string.Empty);
			All_GameMiniTipPanel.publicMiniTip.ShowTip(tips2);
			break;
		}
		}
	}
}
