using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LotteryListItem : MonoBehaviour
{
	private int m_itemId;

	[SerializeField]
	private LotteryItemList _lotteryItemList;

	private void OnEnable()
	{
		SwitchIntoEnglish();
	}

	private void SwitchIntoEnglish()
	{
		if (ZH2_GVars.language_enum != 0)
		{
			base.transform.Find("ItemNum").GetComponent<Text>().fontSize = 23;
			base.transform.Find("buyBtn/Text").GetComponent<Text>().fontSize = 30;

			switch(ZH2_GVars.language_enum)
			{
				case ZH2_GVars.LanguageType.Chinese:
                    base.transform.Find("ItemNum").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().zh_font ;
                    base.transform.Find("buyBtn/Text").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().zh_font;
                    break;
					case ZH2_GVars.LanguageType.English:
                    base.transform.Find("ItemNum").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().en_font;
                    base.transform.Find("buyBtn/Text").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().en_font;
					break;
					case ZH2_GVars.LanguageType.Thai:
                    base.transform.Find("ItemNum").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().th_font;
                    base.transform.Find("buyBtn/Text").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().th_font; break;
				case ZH2_GVars.LanguageType.Vietnam:
                    base.transform.Find("ItemNum").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().vn_font;
                    base.transform.Find("buyBtn/Text").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().vn_font; break;
			}
			
		}
	}

	public void ShowUI(int itemId)
	{
		m_itemId = itemId;
		string str=ZH2_GVars.ShowTip("彩票", "Lottery", "ลอตเตอรี่", "Xổ số") ;
		transform.Find("buyBtn/Text").GetComponent<Text>().text = Convert.ToString(_lotteryItemList.list[m_itemId - 1].price) + str;
		transform.Find("Item").GetComponent<RawImage>().texture = (Resources.Load("Icon/" + _lotteryItemList.list[m_itemId - 1].iconName) as Texture);
		transform.Find("ItemNum").GetComponent<Text>().text = ZH2_GVars.ShowTip(_lotteryItemList.list[m_itemId - 1].zh_name, _lotteryItemList.list[m_itemId - 1].en_name, _lotteryItemList.list[m_itemId - 1].en_name, _lotteryItemList.list[m_itemId - 1].en_name);
	}

	public void OnBtnClick_Buy()
	{
		if (ZH2_GVars.user.card == "-1" && !UserCompleteController.UserCompleteNoPrompt && !UserCompleteController.UserTempNoPrompt)
		{
			MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_UserCompletePanel, new ArrayList
			{
				"LotteryCity",
				m_itemId
			});
			return;
		}
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_BuyLotteryItems, m_itemId);
	}
}
