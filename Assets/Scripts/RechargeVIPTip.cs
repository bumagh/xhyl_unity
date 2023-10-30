using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RechargeVIPTip : MonoBehaviour
{
	private Button btnClose;

	private Button btnCopy;

	private Text txtType;

	private Text txtID;

	private Image imgType;

	private AndroidJavaClass jc;

	private AndroidJavaObject jo;

	private void Awake()
	{
		btnClose = base.transform.Find("BtnClose").GetComponent<Button>();
		btnCopy = base.transform.Find("BtnCopy").GetComponent<Button>();
		txtType = base.transform.Find("TxtType").GetComponent<Text>();
		txtID = base.transform.Find("TxtID").GetComponent<Text>();
		imgType = base.transform.Find("ImgType").GetComponent<Image>();
		if (Application.platform == RuntimePlatform.Android)
		{
			jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		}
		btnClose.onClick.AddListener(ClickBtnClose);
		btnCopy.onClick.AddListener(ClickBtnCopy);
		MB_Singleton<NetManager>.Get().RegisterHandler("gameLoadAddress", HandleNetMsg_CheckGameVersion);
		Send_CheckGameVersion(15, "0");
	}

	public void Send_CheckGameVersion(int gameId, string version)
	{
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/gameLoadAddress", new object[2]
		{
			gameId,
			version
		});
	}

	private void HandleNetMsg_CheckGameVersion(object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["gameType"];
		if (num == 15)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				txtID.text = (string)dictionary["downloadWindows"];
			}
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
			{
				txtID.text = (string)dictionary["downloadAndroid"];
			}
		}
	}

	private void ClickBtnCopy()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		string empty = string.Empty;
		if (Application.platform == RuntimePlatform.Android)
		{
			empty = jo.Call<string>("Docopy", new object[1]
			{
				txtID.text
			});
		}
		else if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			TextEditor textEditor = new TextEditor();
			textEditor.text = txtID.text;
			TextEditor textEditor2 = textEditor;
			textEditor2.OnFocus();
			textEditor2.Copy();
		}
		MB_Singleton<AlertDialog>.GetInstance().ShowDialog("已复制", showOkCancel: false, delegate
		{
			Application.OpenURL("weixin://");
		});
	}

	private void ClickBtnClose()
	{
		base.gameObject.SetActive(value: false);
	}
}
