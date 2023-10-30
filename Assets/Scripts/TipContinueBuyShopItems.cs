using UnityEngine;
using UnityEngine.UI;

public class TipContinueBuyShopItems : MonoBehaviour
{
	private int m_itemId;

	[SerializeField]
	private ShopPropList _shopProplist;

	private void Start()
	{
		SwitchIntoEnglish();
	}

	private void SwitchIntoEnglish()
	{
		if (ZH2_GVars.language_enum != 0)
		{
			base.transform.Find("btnSure/Text").GetComponent<Text>().text = "Buy";
		}
	}

	public void ShowUI(int itemId)
	{
		m_itemId = itemId;
		base.transform.Find("tip").GetComponent<Text>().text = string.Format((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? "您的{0}道具仅剩{1}天，是否继续购买?" : "You have {1} day validity of {0} , whether to buy again?", (ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? _shopProplist.list[m_itemId - 1].zh_name : _shopProplist.list[m_itemId - 1].en_name, string.Empty + Mathf.CeilToInt((float)ZH2_GVars.ownedProps[m_itemId].RemainTime / 8.64E+07f));
	}

	public void OnBtnClick_Sure()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Close_StoreItemInfo);
		MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_BuyShopItems, m_itemId);
	}

	public void OnBtnClick_Close()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
