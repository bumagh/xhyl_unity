using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyNickname : Tween_SlowAction
{
	[SerializeField]
	private InputField input_nickName;

	private string m_tempname = string.Empty;

	private Button modifyNicknameBtn;

	private Button btnlCose;

	private void Awake()
	{
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("updateUserInfo", HandleNetMsg_UpdateUserInfo);
		modifyNicknameBtn = base.transform.Find("Container/modifyNicknameBtn").GetComponent<Button>();
		modifyNicknameBtn.onClick.AddListener(OnModifyNickname);
		btnlCose = base.transform.Find("Container/BtnClose").GetComponent<Button>();
		btnlCose.onClick.AddListener(OnPanelClose);
	}

	private void OnEnable()
	{
		input_nickName.text = string.Empty;
		Show();
	}

	public void OnModifyNickname()
	{
		Debug.LogError("点击修改账号昵称");
		if (InputCheck.CheckChangeNickName(input_nickName.text, null))
		{
			if (input_nickName.text == m_tempname || input_nickName.text == ZH2_GVars.user.nickname)
			{
				All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("修改成功", "Modify successfully", "แก้ไขสำเร็จ", "Sửa đổi thành công"));
				return;
			}
			Send_UserInfo(input_nickName.text);
			ZH2_GVars.user.nickname = input_nickName.text;
			Debug.LogError("缩略昵称: " + ZH2_GVars.GetBreviaryName(ZH2_GVars.user.nickname));
		}
	}

	public void Send_UserInfo(string nickname)
	{
		object[] args = new object[3]
		{
			nickname,
			ZH2_GVars.user.sex,
			ZH2_GVars.user.photoId + 1
		};
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/updateUserInfo", args);
	}

	public void HandleNetMsg_UpdateUserInfo(object[] args)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary = (args[0] as Dictionary<string, object>);
		if ((bool)dictionary["success"])
		{
			m_tempname = input_nickName.text;
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("修改成功", "Modify successfully", "แก้ไขสำเร็จ", "Sửa đổi thành công"));
            ZH2_GVars.user.nickname = input_nickName.text;
			MB_Singleton<AppManager>.Get().Notify(UIGameMsgType.UINotify_Fresh_UserInfo);
		}
		else
		{
			All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("修改失败", "Change failed", "การแก้ไขล้มเหลว", "Sửa đổi thất bại"));
		}
		MB_Singleton<AppManager>.Get().m_mainPanel.OppenPanel("UserSettingsPanel");
		base.gameObject.SetActive(value: false);
	}

	public void OnPanelClose()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		MB_Singleton<AppManager>.Get().m_mainPanel.OppenPanel("UserSettingsPanel");
		Hide(base.gameObject);
	}
}
