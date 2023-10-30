using UnityEngine;
using UnityEngine.UI;

public class MailItem : MonoBehaviour
{
	public Sprite[] photo;

	public Image readImage;

	public Text textMailName;

	public Text textMailSender;

	public Text textMailSendTime;

	public Toggle toggleMail;

	public Mail mail;

	private Color color;

	public void Init(Mail mail)
	{
		this.mail = mail;
		readImage.sprite = ((mail.mailIsReadStatus == 0) ? photo[0] : photo[1]);
		textMailName.text = mail.mailName;
		textMailName.fontSize = 30;
		if (mail.mailName == "本周分享奖励结算" || mail.mailName == "本周分享奖励")
		{
			textMailName.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? mail.mailName : "Invitation Award");
			textMailName.fontSize = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? 30 : 22);
		}
		textMailSender.text = mail.mailSendPeople;
		textMailSendTime.text = mail.mailTime;
		color = new Color(0.74f, 0.74f, 0.74f, 0.63f);
		if (mail.mailIsReadStatus == 1)
		{
			textMailName.color = color;
			textMailSender.color = color;
			textMailSendTime.color = color;
			base.transform.Find("send").GetComponent<Text>().color = color;
			base.transform.Find("sendtime").GetComponent<Text>().color = color;
		}
		toggleMail.isOn = false;
	}

	public void CheckToggle(bool b)
	{
		toggleMail.isOn = b;
	}

	public void Read()
	{
		mail.mailIsReadStatus = 1;
		readImage.sprite = photo[1];
		textMailName.color = color;
		textMailSender.color = color;
		textMailSendTime.color = color;
		base.transform.Find("send").GetComponent<Text>().color = color;
		base.transform.Find("sendtime").GetComponent<Text>().color = color;
	}

	public void DeleteItem(bool check)
	{
		if (check)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (toggleMail.isOn)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
