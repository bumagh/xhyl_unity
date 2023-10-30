using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class MailPanelController : MonoBehaviour
{
	public GameObject m_mailContent;

	public GameObject mailItem;

	public GameObject awardItem;

	private Text mailSend;

	private Text mailTime;

	private Text mailContent;

	private GameObject mailContent2;

	private GameObject onlyDeletBtn;

	private GameObject GainADelbtn;

	private GameObject grid;

	private GameObject[] awardItemList;

	private Mail currentMail;

	private bool isAllSelect;

	private string m_selectMail;

	private void Start()
	{
		grid = base.transform.Find("bg/ItemBg/Grid").gameObject;
		mailSend = base.transform.Find("Mask/MailContent/mailContent/sender").GetComponent<Text>();
		mailTime = base.transform.Find("Mask/MailContent/mailContent/sendTime").GetComponent<Text>();
		mailContent = base.transform.Find("Mask/MailContent/mailContent/scrollContent/content").GetComponent<Text>();
		mailContent2 = base.transform.Find("Mask/MailContent/mailContent/scrollContent2").gameObject;
		onlyDeletBtn = base.transform.Find("Mask/MailContent/DeleteButton").gameObject;
		GainADelbtn = base.transform.Find("Mask/MailContent/ReAndDe").gameObject;
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("getEmailList", HandleNetMsg_Mail);
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("updateEmail", HandleNetMsg_updateEmail);
		MB_Singleton<NetManager>.GetInstance().RegisterHandler("refreshEmailList", HandleNetMsg_refreshEmailList);
	}

	private void HandleNetMsg_refreshEmailList(object[] obj)
	{
		ClearItem();
		OnBtnClick_MailContentBack();
		OnBtnClick_Back();
		MB_Singleton<AlertDialog>.GetInstance().ShowDialog("后台修改邮件");
	}

	private void HandleNetMsg_updateEmail(object[] obj)
	{
		Dictionary<string, object> dictionary = obj[0] as Dictionary<string, object>;
		if ((bool)dictionary["success"])
		{
			if (dictionary.ContainsKey("gameGold"))
			{
				int num = (int)dictionary["gameGold"];
				MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? $"Congratulations,you get {num - ZH2_GVars.user.gameGold} coins from sharing rewards by binding account" : ((ZH2_GVars.language_enum != 0) ? $"ย\u0e34นด\u0e35ด\u0e49วยนะคะ ท\u0e35\u0e48ได\u0e49ร\u0e31บรางว\u0e31ลจากบ\u0e31ญช\u0e35 ท\u0e35\u0e48ผ\u0e39กไว\u0e49{num - ZH2_GVars.user.gameGold}ป\u0e38\u0e48ม ！" : $"恭喜您通过绑定帐号获得分享奖励{num - ZH2_GVars.user.gameGold}币！"));
				ZH2_GVars.user.gameGold = num;
				MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);
				if (GainADelbtn.transform.Find("ReceiveButton").gameObject.activeSelf)
				{
					GainADelbtn.transform.Find("ReceiveButton").GetComponent<Button>().interactable = false;
					GainADelbtn.transform.Find("DeleteButton").GetComponent<Button>().interactable = true;
				}
			}
			return;
		}
		switch ((int)dictionary["msgCode"])
		{
		default:
		{
			string content = (string)dictionary["msg"];
			MB_Singleton<AlertDialog>.Get().ShowDialog(content);
			break;
		}
		case 26:
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Received" : ((ZH2_GVars.language_enum != 0) ? "ได\u0e49ร\u0e31บแล\u0e49ว" : "已领取"));
			break;
		case 14:
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Invalid parameter" : ((ZH2_GVars.language_enum != 0) ? "อาร\u0e4cก\u0e34วเมนต\u0e4cไม\u0e48ถ\u0e39กต\u0e49อง" : "无效的参数"));
			break;
		case 0:
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Program exception" : ((ZH2_GVars.language_enum != 0) ? "โปรแกรม ท\u0e35\u0e48ผ\u0e34ดปกต\u0e34" : "程序异常"));
			break;
		}
	}

	private void HandleNetMsg_Mail(object[] obj)
	{
		Dictionary<string, object> dictionary = obj[0] as Dictionary<string, object>;
		bool flag = (bool)dictionary["success"];
		UnityEngine.Debug.Log("getResult: " + flag);
		if (!flag)
		{
			return;
		}
		object[] array = (object[])dictionary["mailList"];
		int num = array.Length;
		if (num != 0)
		{
			Mail[] array2 = new Mail[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = Mail.CreateWithDic((Dictionary<string, object>)array[i]);
			}
			UnityEngine.Debug.Log("mm: " + array2[0].mailName);
			if (num <= 3)
			{
				grid.GetComponent<RectTransform>().sizeDelta = new Vector2(992f, 400f);
			}
			if (num > 3)
			{
				grid.GetComponent<RectTransform>().sizeDelta = new Vector2(992f, 350 + (num - 3) * 100);
				grid.GetComponent<RectTransform>().localPosition = new Vector3(0f, (num - 3) * -52, 0f);
			}
			for (int j = 0; j < num; j++)
			{
				GameObject go = UnityEngine.Object.Instantiate(mailItem);
				go.name = "MailItem" + j;
				go.transform.SetParent(base.transform.Find("bg/ItemBg/Grid"));
				go.transform.localScale = new Vector3(1f, 1f, 1f);
				go.transform.localPosition = new Vector3(0f, 0f, 0f);
				go.tag = "MailItem";
				go.GetComponent<MailItem>().Init(array2[j]);
				go.GetComponent<Button>().onClick.AddListener(delegate
				{
					UnityEngine.Debug.Log("afsdf");
					m_selectMail = go.name;
					UnityEngine.Debug.Log(m_selectMail);
					go.GetComponent<MailItem>().Read();
					ShowMailContent(go.GetComponent<MailItem>().mail);
				});
			}
		}
	}

	public void OnBtnClick_SelectAll()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		isAllSelect = true;
		GameObject[] array = GameObject.FindGameObjectsWithTag("MailItem");
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (!gameObject.GetComponent<MailItem>().toggleMail.isOn)
			{
				isAllSelect = false;
			}
		}
		GameObject[] array3 = array;
		foreach (GameObject gameObject2 in array3)
		{
			gameObject2.GetComponent<MailItem>().CheckToggle(!isAllSelect);
		}
	}

	public void OnBtnClick_Delete()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Mail_Delete);
		string del = string.Empty;
		int num = 0;
		GameObject[] array = GameObject.FindGameObjectsWithTag("MailItem");
		if (array.Length == 0)
		{
			return;
		}
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (gameObject.transform.GetComponent<MailItem>().toggleMail.isOn)
			{
				del = del + gameObject.transform.GetComponent<MailItem>().mail.mailId + ",";
				if (gameObject.transform.GetComponent<MailItem>().mail.gainStatus == 1)
				{
					string input = gameObject.transform.GetComponent<MailItem>().mail.mailContent;
					string[] array3 = Regex.Split(input, ",");
					int num2 = int.Parse(array3[0]);
					num += num2;
				}
			}
			gameObject.transform.GetComponent<MailItem>().DeleteItem(check: false);
		}
		if (del == string.Empty)
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Please select the message" : ((ZH2_GVars.language_enum != 0) ? "โปรแกรม ท\u0e35\u0e48ผ\u0e34ดปกต\u0e34" : "请选择要删除的邮件"));
		}
		else if (num == 0)
		{
			SendUpdateMail(del);
		}
		else
		{
			MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? $"The message contains rewards,after deletion,you will receive {num} coins directly" : ((ZH2_GVars.language_enum != 0) ? $"จดหมายม\u0e35รางว\u0e31ล ท\u0e35\u0e48ย\u0e31งไม\u0e48ได\u0e49ร\u0e31บ และเม\u0e37\u0e48อลบแล\u0e49วค\u0e38ณจะได\u0e49ร\u0e31บโดยตรง{num}ช\u0e37\u0e48อเกม !" : $"邮件含有未领取的奖励，删除后您将直接获得{num}游戏币!"), showOkCancel: true, delegate
			{
				SendUpdateMail(del);
			});
		}
	}

	private void SendUpdateMail(string strDel)
	{
		UnityEngine.Debug.Log("strDel1: " + strDel);
		if (strDel.Substring(strDel.Length - 1, 1) == ",")
		{
			strDel = strDel.Substring(0, strDel.Length - 1);
		}
		UnityEngine.Debug.Log("strDel2: " + strDel);
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/updateEmail", new object[2]
		{
			strDel,
			2
		});
	}

	public void OnBtnClick_Back()
	{
		int num = 0;
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		GameObject[] array = GameObject.FindGameObjectsWithTag("MailItem");
		if (array.Length == 0)
		{
			UnityEngine.Debug.Log("nullnullnullnullnull");
		}
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (gameObject.transform.GetComponent<MailItem>().mail.mailIsReadStatus == 0)
			{
				num++;
			}
			gameObject.transform.GetComponent<MailItem>().DeleteItem(check: true);
		}
		UnityEngine.Debug.Log("mailNoReadCount: " + num);
		MB_Singleton<AppManager>.Get().Notify(UIGameMsgType.UINotify_Fresh_UseMailSign, num);
		base.gameObject.SetActive(value: false);
	}

	public void OnBtnClick_MailContentBack()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		UnityEngine.Debug.Log("close");
		ClearItem();
		m_mailContent.SetActive(value: false);
	}

	public void ReceiveShareAward()
	{
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/updateEmail", new object[2]
		{
			currentMail.mailId + string.Empty,
			3
		});
	}

	public void ShowMailContent(Mail mail)
	{
		currentMail = mail;
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Mail_Read);
		GameObject gameObject = base.transform.Find("bg/ItemBg/Grid").Find(m_selectMail).gameObject;
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/updateEmail", new object[2]
		{
			gameObject.GetComponent<MailItem>().mail.mailId + string.Empty,
			1
		});
		UnityEngine.Debug.Log("ShowMailContent" + base.name);
		m_mailContent.SetActive(value: true);
		mailSend.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? $"From：{mail.mailSendPeople}" : ((ZH2_GVars.language_enum != 0) ? $"ผ\u0e39\u0e49ส\u0e48ง：{mail.mailSendPeople}" : $"发件人：{mail.mailSendPeople}"));
		mailTime.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? $"Date：{mail.mailTime}" : ((ZH2_GVars.language_enum != 0) ? $"เวลา ท\u0e35\u0e48ส\u0e48งออก：{mail.mailTime}" : $"发件时间：{mail.mailTime}"));
		onlyDeletBtn.SetActive(mail.gainStatus == 0);
		GainADelbtn.SetActive(mail.gainStatus != 0);
		GainADelbtn.transform.Find("ReceiveButton").GetComponent<Button>().interactable = (mail.gainStatus == 1);
		GainADelbtn.transform.Find("DeleteButton").GetComponent<Button>().interactable = (mail.gainStatus == 2);
		UnityEngine.Debug.Log("mail.gainStatus: " + mail.gainStatus);
		mailContent.transform.parent.gameObject.SetActive(mail.gainStatus == 0);
		mailContent2.SetActive(mail.gainStatus != 0);
		if (mail.gainStatus == 0)
		{
			mailContent.text = mail.mailContent;
			return;
		}
		string[] array = Regex.Split(mail.mailContent, ",");
		int num = array.Length;
		int num2 = (num - 1) / 2;
		awardItemList = new GameObject[num2];
		mailContent2.transform.Find("content").GetComponent<Text>().text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? $"Congratulations,you get {array[0]} coins from sharing rewards by binding account" : ((ZH2_GVars.language_enum != 0) ? $"ย\u0e34นด\u0e35ด\u0e49วยนะคะ ท\u0e35\u0e48ได\u0e49ร\u0e31บรางว\u0e31ลจากบ\u0e31ญช\u0e35 ท\u0e35\u0e48ผ\u0e39กไว\u0e49{array[0]}ป\u0e38\u0e48ม " : $"恭喜您通过绑定账号获得分享奖励{array[0]}币"));
		mailContent2.transform.Find("content").GetComponent<Text>().fontSize = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? 30 : 20);
		for (int i = 0; i < num2; i++)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(awardItem);
			awardItemList.SetValue(gameObject2, i);
			gameObject2.transform.SetParent(mailContent2.transform.Find("scroll/Grid"));
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.transform.Find("account").GetComponent<Text>().text = array[i * 2 + 1];
			gameObject2.transform.Find("award").GetComponent<Text>().text = array[i * 2 + 2];
		}
		if (num2 < 4)
		{
			mailContent2.transform.Find("scroll/Grid").GetComponent<RectTransform>().sizeDelta = new Vector2(797f, 160f);
		}
		if (num2 > 4)
		{
			mailContent2.transform.Find("scroll/Grid").GetComponent<RectTransform>().sizeDelta = new Vector2(797f, 160 + (num2 - 3) * 50);
			mailContent2.transform.Find("scroll/Grid").GetComponent<RectTransform>().localPosition = new Vector3(0f, -50 * (num2 - 3), 0f);
		}
	}

	public void OnBtnClick_MailContentDelete()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Mail_Delete);
		GameObject gameObject = base.transform.Find("bg/ItemBg/Grid").Find(m_selectMail).gameObject;
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/updateEmail", new object[2]
		{
			gameObject.GetComponent<MailItem>().mail.mailId + string.Empty,
			2
		});
		UnityEngine.Object.Destroy(gameObject);
		ClearItem();
		m_mailContent.SetActive(value: false);
	}

	private void ClearItem()
	{
		if (awardItemList != null)
		{
			GameObject[] array = awardItemList;
			foreach (GameObject obj in array)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}
}
