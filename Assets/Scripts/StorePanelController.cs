using System;
using UnityEngine;
using UnityEngine.UI;

public class StorePanelController : MonoBehaviour
{
	public Transform m_listItemPrefab;

	public Transform _tipBuyGameCurrency;

	public Transform _tipBuyProps;

	public Transform _tipContinueBuyShopItems;

	private Transform m_listContent;

	private int m_curTabIndex;

	private void Awake()
	{
		m_listContent = base.transform.Find("listPanel/listScrollView/Viewport/Content");
		MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Fresh_GoldAndLottery, this, FreshGoldAndLottery);
		MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Open_BuyShopItems, this, OpenBuyShopItems);
		MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Open_ContinueBuyShopItems, this, OpenContinueBuyShopItems);
	}

	private void OnEnable()
	{
		base.transform.Find("topBg/goldItem/num").GetComponent<Text>().text = Convert.ToString(ZH2_GVars.user.gameGold);
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

	private void SwitchIntoEnglish()
	{
		if (ZH2_GVars.language_enum != 0)
		{
			base.transform.Find("TitleBg/Title").GetComponent<Text>().text = "Mall";
			base.transform.Find("TitleBg/Title").GetComponent<Text>().fontSize = 30;
			base.transform.Find("tab/tab1/Text").GetComponent<Text>().text = "Coins";
			base.transform.Find("tab/tab1/tabHighlight/Text").GetComponent<Text>().text = "Coins";
			base.transform.Find("tab/tab2/Text").GetComponent<Text>().text = "Props";
			base.transform.Find("tab/tab2/tabHighlight/Text").GetComponent<Text>().text = "Props";
			base.transform.Find("tab/tab3/Text").GetComponent<Text>().text = "Beauty dealers";
			base.transform.Find("tab/tab3/tabHighlight/Text").GetComponent<Text>().text = "Beauty dealers";
		}
	}

	private void UpdateList()
	{
		int[] array = new int[3]
		{
			4,
			4,
			3
		};
		DestroyList();
		m_listContent.localPosition = new Vector3(-574f, 146f, 0f);
		for (int i = 0; i < array[m_curTabIndex - 1]; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate(m_listItemPrefab);
			transform.parent = m_listContent;
			transform.localPosition = new Vector3((m_curTabIndex < 3) ? (150 + 300 * i) : (200 + 370 * i), -146f, 0f);
			transform.localScale = Vector3.one;
			transform.name = string.Empty + i;
			transform.GetComponent<StoreListItem>().ShowUI(i + 1 + (m_curTabIndex - 1) * 4);
		}
		float x = array[m_curTabIndex - 1] * 300;
		float height = m_listContent.GetComponent<RectTransform>().rect.height;
		m_listContent.GetComponent<RectTransform>().sizeDelta = new Vector2(x, height);
	}

	public void OnBtnClick_Fader()
	{
		MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Close_StoreItemInfo);
	}

	public void OnBtnClick_Close()
	{
		MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
		base.gameObject.SetActive(value: false);
	}

	public void OnBtnClick_Return()
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

	public void FocusTabByIndex(int index)
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
		base.transform.Find("topBg/goldItem/num").GetComponent<Text>().text = Convert.ToString(ZH2_GVars.user.gameGold);
		base.transform.Find("topBg/lotteryItem/num").GetComponent<Text>().text = Convert.ToString(ZH2_GVars.user.lottery);
	}

	private void OpenBuyShopItems(object obj)
	{
		Transform transform = UnityEngine.Object.Instantiate(((int)obj <= 4) ? _tipBuyGameCurrency : _tipBuyProps);
		transform.parent = base.transform;
		transform.localPosition = new Vector3(0f, 216f, 0f);
		transform.localScale = Vector3.one;
		if ((int)obj <= 4)
		{
			transform.GetComponent<TipBuyGameCurrency>().ShowUI((int)obj);
		}
		else
		{
			transform.GetComponent<TipBuyProps>().ShowUI((int)obj);
		}
	}

	private void OpenContinueBuyShopItems(object obj)
	{
		Transform transform = UnityEngine.Object.Instantiate(_tipContinueBuyShopItems);
		transform.parent = base.transform;
		transform.localPosition = new Vector3(0f, 216f, 0f);
		transform.localScale = Vector3.one;
		transform.GetComponent<TipContinueBuyShopItems>().ShowUI((int)obj);
	}
}
