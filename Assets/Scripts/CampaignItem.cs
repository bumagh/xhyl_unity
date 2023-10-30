using UnityEngine;
using UnityEngine.UI;

public class CampaignItem : MonoBehaviour
{
	public Image imageCampaginSign;

	public Text textCampaignContent;

	public int campaignStatus;

	public Text textCampaginSchedule;

	public GameObject buttonComplete;

	public Text awardGold;

	public Campaign cap;

	private Color[] colorText = new Color[3]
	{
		new Color(0.96f, 0.92f, 0.69f),
		new Color(0.45f, 0.25f, 0f),
		new Color(0.74f, 0.74f, 0.74f)
	};

	private Sprite[] s;

	public void Init(Campaign c, Sprite[] s)
	{
		cap = c;
		this.s = s;
		textCampaignContent.text = c.content;
		if (c.type == 6)
		{
			textCampaignContent.GetComponent<RectTransform>().sizeDelta = new Vector2(870f, 66f);
			textCampaignContent.GetComponent<RectTransform>().localPosition = new Vector3(60f, 0f, 0f);
			textCampaginSchedule.gameObject.SetActive(value: false);
			buttonComplete.SetActive(value: false);
			return;
		}
		campaignStatus = c.status;
		if (c.userSchedule > c.targetSchedule)
		{
			c.userSchedule = c.targetSchedule;
		}
		textCampaginSchedule.text = c.userSchedule + "/" + c.targetSchedule;
		if (c.type == 4 || c.type == 5)
		{
			textCampaginSchedule.gameObject.SetActive(value: false);
		}
		buttonComplete.GetComponent<Image>().sprite = s[campaignStatus];
		if (campaignStatus != 1)
		{
			buttonComplete.GetComponent<Button>().enabled = false;
		}
		awardGold.text = c.awardGold + string.Empty;
		awardGold.color = colorText[campaignStatus];
		if (campaignStatus == 2)
		{
			switch(ZH2_GVars.language_enum)
			{
				case ZH2_GVars.LanguageType.Chinese:
                    awardGold.fontSize = 36;
                    awardGold.text = "已完成";
                    break;
				case ZH2_GVars.LanguageType .English:
                    awardGold.text = "Finished";
                    awardGold.fontSize = 28;
                    break;
				case ZH2_GVars.LanguageType.Thai:
                    awardGold.text = "เรียบร้อยแล้ว";
                    awardGold.fontSize = 28;
                    break;
				case ZH2_GVars.LanguageType.Vietnam:
                    awardGold.text = "Đã hoàn thành";
                    awardGold.fontSize = 28;
                    break;
			}			
		}
        switch (ZH2_GVars.language_enum)
        {
            case ZH2_GVars.LanguageType.Chinese:
                awardGold.font = MB_Singleton<AppManager>.Get().zh_font;
                break;
            case ZH2_GVars.LanguageType.English:
				awardGold.font = MB_Singleton<AppManager>.Get().en_font;
                break;
            case ZH2_GVars.LanguageType.Thai:
                awardGold.font = MB_Singleton<AppManager>.Get().th_font;
                break;
            case ZH2_GVars.LanguageType.Vietnam:
                awardGold.font = MB_Singleton<AppManager>.Get().vn_font;
                break;
        }     
	}

	public void ChangeButtonStatus()
	{
		cap.status = 2;
		campaignStatus = cap.status;
		buttonComplete.GetComponent<Image>().sprite = s[campaignStatus];
		buttonComplete.GetComponent<Button>().enabled = false;
        switch (ZH2_GVars.language_enum)
        {
            case ZH2_GVars.LanguageType.Chinese:
                awardGold.font = MB_Singleton<AppManager>.Get().zh_font;
                awardGold.fontSize = 36;
                awardGold.text = "已完成";
                break;
            case ZH2_GVars.LanguageType.English:
                awardGold.font = MB_Singleton<AppManager>.Get().en_font;
                awardGold.text = "Finished";
                awardGold.fontSize = 28;
                break;
            case ZH2_GVars.LanguageType.Thai:
                awardGold.font = MB_Singleton<AppManager>.Get().th_font;
                awardGold.text = "เรียบร้อยแล้ว";
                awardGold.fontSize = 28;
                break;
            case ZH2_GVars.LanguageType.Vietnam:
                awardGold.font = MB_Singleton<AppManager>.Get().vn_font;
                awardGold.text = "Đã hoàn thành";
                awardGold.fontSize = 28;
                break;
        }      
        awardGold.color = colorText[campaignStatus];
	}
}
