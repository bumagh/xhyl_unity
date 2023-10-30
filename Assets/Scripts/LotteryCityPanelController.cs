using System;
using UnityEngine;
using UnityEngine.UI;

public class LotteryCityPanelController : MonoBehaviour
{
	public Transform _listItemPrefab;

	public Transform _receiveAddressPrefab;

	public Transform _tipExchangeGifts;

	private Transform m_listContent;

	private int m_curTabIndex;

	private void Awake()
	{
		MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Fresh_GoldAndLottery, this, FreshGoldAndLottery);
		MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Open_ReveiveAddressPanel, this, OpenReveiveAddressPanel);
		MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Open_BuyLotteryItems, this, OpenBuyLotteryItems);
		m_listContent = base.transform.Find("listPanel/listScrollView/Viewport/Content");
	}

	private void OnEnable()
	{
		base.transform.Find("topBg/lotteryItem/num").GetComponent<Text>().text = Convert.ToString(ZH2_GVars.user.lottery);
		FocusTabByIndex(1);
	}

	private void OnDisable()
	{
		m_curTabIndex = 0;
		UserCompleteController.UserTempNoPrompt = false;
	}

	private void DestroyList()
	{
		for (int i = 0; i < m_listContent.childCount; i++)
		{
			UnityEngine.Object.DestroyObject(m_listContent.GetChild(i).gameObject);
		}
	}

	private void UpdateList()
	{
		DestroyList();
		m_listContent.localPosition = new Vector3(-574f, 146f, 0f);
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate(_listItemPrefab);
			transform.parent = m_listContent;
			transform.localPosition = new Vector3(200 + 370 * i, -146f, 0f);
			transform.localScale = Vector3.one;
			transform.name = string.Empty + i;
			transform.GetComponent<LotteryListItem>().ShowUI(i + 1 + (m_curTabIndex - 1) * 3);
		}
		float x = num * 400;
		float height = m_listContent.GetComponent<RectTransform>().rect.height;
		m_listContent.GetComponent<RectTransform>().sizeDelta = new Vector2(x, height);
	}

	private void SwitchIntoEnglish()
	{
		if (ZH2_GVars.language_enum != 0)
		{
			base.transform.Find("TitleBg/Title").GetComponent<Text>().text = "Lottery Mall";
			base.transform.Find("TitleBg/Title").GetComponent<Text>().fontSize = 30;
			base.transform.Find("tab/tab1/Text").GetComponent<Text>().text = "Accessories";
			base.transform.Find("tab/tab1/tabHighlight/Text").GetComponent<Text>().text = "Accessories";
			base.transform.Find("tab/tab2/Text").GetComponent<Text>().text = "Toy";
			base.transform.Find("tab/tab2/tabHighlight/Text").GetComponent<Text>().text = "Toy";
			base.transform.Find("tab/tab3/Text").GetComponent<Text>().text = "Apple Series";
			base.transform.Find("tab/tab3/tabHighlight/Text").GetComponent<Text>().text = "Apple Series";
			base.transform.Find("TitleBg/Title").GetComponent<Text>().font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? MB_Singleton<AppManager>.Get().zh_font : MB_Singleton<AppManager>.Get().en_font);
			base.transform.Find("tab/tab1/Text").GetComponent<Text>().font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? MB_Singleton<AppManager>.Get().zh_font : MB_Singleton<AppManager>.Get().en_font);
			base.transform.Find("tab/tab1/tabHighlight/Text").GetComponent<Text>().font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? MB_Singleton<AppManager>.Get().zh_font : MB_Singleton<AppManager>.Get().en_font);
			base.transform.Find("tab/tab2/Text").GetComponent<Text>().font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? MB_Singleton<AppManager>.Get().zh_font : MB_Singleton<AppManager>.Get().en_font);
			base.transform.Find("tab/tab2/tabHighlight/Text").GetComponent<Text>().font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? MB_Singleton<AppManager>.Get().zh_font : MB_Singleton<AppManager>.Get().en_font);
			base.transform.Find("tab/tab3/Text").GetComponent<Text>().font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? MB_Singleton<AppManager>.Get().zh_font : MB_Singleton<AppManager>.Get().en_font);
			base.transform.Find("tab/tab3/tabHighlight/Text").GetComponent<Text>().font = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? MB_Singleton<AppManager>.Get().zh_font : MB_Singleton<AppManager>.Get().en_font);
		}
	}

	public void OnBtnClick_Close()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		base.gameObject.SetActive(value: false);
	}

	public void OnBtnClick_Tab1()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		FocusTabByIndex(1);
	}

	public void OnBtnClick_Tab2()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		FocusTabByIndex(2);
	}

	public void OnBtnClick_Tab3()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		FocusTabByIndex(3);
	}

	private void FocusTabByIndex(int index)
	{
		if (m_curTabIndex != index)
		{
			m_curTabIndex = index;
			UpdateList();
			for (int i = 1; i < 4; i++)
			{
				base.transform.Find($"tab/tab{i}/tabHighlight").gameObject.SetActive(i == index);
			}
		}
	}

	private void FreshGoldAndLottery(object obj)
	{
		base.transform.Find("topBg/lotteryItem/num").GetComponent<Text>().text = Convert.ToString(ZH2_GVars.user.lottery);
	}

	private void OpenReveiveAddressPanel(object obj)
	{
		Transform transform = UnityEngine.Object.Instantiate(_receiveAddressPrefab);
		transform.parent = base.transform;
		transform.localPosition = new Vector3(0f, 216f, 0f);
		transform.localScale = Vector3.one;
		transform.GetComponent<ReceiveAddressController>().buySendData = (object[])obj;
	}

	private void OpenBuyLotteryItems(object obj)
	{
		Transform transform = UnityEngine.Object.Instantiate(_tipExchangeGifts);
		transform.parent = base.transform;
		transform.localPosition = new Vector3(0f, 216f, 0f);
		transform.localScale = Vector3.one;
		transform.GetComponent<TipExchangeGifts>().ShowUI((int)obj);
	}
}
