using UnityEngine;
using UnityEngine.UI;

public class RechargeVIP : MonoBehaviour
{
	private Button[] btns;

	private Button btnCopy;

	private Text txtID;

	private GameObject vipRechargePanel;

	private AndroidJavaClass jc;

	private AndroidJavaObject jo;

	private void Awake()
	{
		btns = new Button[8];
		for (int i = 0; i < 8; i++)
		{
			btns[i] = base.transform.Find("Buttons").GetChild(i).GetComponent<Button>();
			int temp = i;
			btns[i].onClick.AddListener(delegate
			{
				ClickBtnRecharge(temp);
			});
		}
		btnCopy = base.transform.Find("BtnCopy").GetComponent<Button>();
		txtID = base.transform.Find("TxtID").GetComponent<Text>();
		txtID.text = ZH2_GVars.user.id.ToString();
		vipRechargePanel = base.transform.Find("VIPRechargeTip").gameObject;
		if (Application.platform == RuntimePlatform.Android)
		{
			jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		}
		btnCopy.onClick.AddListener(ClickBtnCopy);
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
		MB_Singleton<AlertDialog>.GetInstance().ShowDialog("已复制");
	}

	private void ClickBtnRecharge(int i)
	{
		if (i == 0)
		{
			vipRechargePanel.SetActive(value: true);
		}
		else
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog("未开放");
		}
	}
}
