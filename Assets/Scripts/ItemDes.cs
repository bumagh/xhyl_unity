using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemDes : MonoBehaviour
{
	private int m_itemId;

	[SerializeField]
	private ShopPropList _shopProplist;

	private void Awake()
	{
		MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Close_StoreItemInfo, this, CloseItemInfo);
	}

	private void Start()
	{
		SwitchIntoEnglish();
		Tweener t = base.transform.GetComponent<RectTransform>().DOScale(0.3f, 0.2f);
		t.SetDelay(3f);
		t.SetUpdate(isIndependentUpdate: true);
		t.SetEase(Ease.Linear);
		t.OnComplete(delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
			base.transform.parent.Find("guang").gameObject.SetActive(value: false);
		});
	}

	private void SwitchIntoEnglish()
	{
		if (ZH2_GVars.language_enum != 0)
		{
			base.transform.Find("frame/itemName").GetComponent<Text>().fontSize = 28;
			base.transform.Find("frame/itemName").GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 25);
			base.transform.Find("frame/implement").GetComponent<Text>().fontSize = 20;
			base.transform.Find("frame/implement").GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 25);
			base.transform.Find("frame/desText").GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 25);
		}
	}

	public void ShowUI(int itemId)
	{
		m_itemId = itemId;
		if (m_itemId <= 4)
		{
			base.transform.Find("frame/implement").gameObject.SetActive(value: false);
		}
		else
		{
			base.transform.Find("frame/implement").GetComponent<Text>().text = string.Format((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Giving {0} ticket" : ((ZH2_GVars.language_enum != 0) ? "น\u0e35\u0e48ไง{0}ลอตเตอร\u0e35\u0e48" : "赠{0}彩票"), (m_itemId <= 8) ? 50 : 50);
		}
		base.transform.Find("frame/itemName").GetComponent<Text>().text = ((ZH2_GVars.language_enum != 0) ? _shopProplist.list[m_itemId - 1].en_name : _shopProplist.list[m_itemId - 1].zh_name);
		base.transform.Find("frame/desText").GetComponent<Text>().text = ((ZH2_GVars.language_enum != 0) ? _shopProplist.list[m_itemId - 1].en_des : _shopProplist.list[m_itemId - 1].zh_des);
		base.transform.Find("frame/itemImage").GetComponent<RawImage>().texture = (Resources.Load("Icon/" + _shopProplist.list[m_itemId - 1].iconName) as Texture);
		FixFrameSize();
		FixFramePos();
	}

	private void CloseItemInfo(object obj)
	{
		UnityEngine.Object.Destroy(base.gameObject);
		base.transform.parent.Find("guang").gameObject.SetActive(value: false);
	}

	private void FixFramePos()
	{
		RectTransform component = base.transform.Find("frame/desBg").GetComponent<RectTransform>();
		RectTransform component2 = base.transform.Find("frame").GetComponent<RectTransform>();
		Vector3 localPosition = component.localPosition;
		float num = 0f - localPosition.y;
		Vector3 localPosition2 = base.transform.Find("arrow").localPosition;
		float num2 = num + localPosition2.y;
		Vector2 sizeDelta = component.sizeDelta;
		float y = num2 + sizeDelta.y - 3f;
		Vector3 localPosition3 = base.transform.parent.localPosition;
		float x = localPosition3.x;
		Vector3 localPosition4 = base.transform.parent.parent.localPosition;
		float num3 = x + localPosition4.x;
		Vector2 sizeDelta2 = component.sizeDelta;
		float num4 = num3 - sizeDelta2.x / 2f + 640f;
		Vector3 localPosition5 = base.transform.parent.localPosition;
		float x2 = localPosition5.x;
		Vector3 localPosition6 = base.transform.parent.parent.localPosition;
		float num5 = x2 + localPosition6.x;
		Vector2 sizeDelta3 = component.sizeDelta;
		float num6 = num5 + sizeDelta3.x / 2f - 640f;
		float num7 = (!(num4 >= 0f)) ? (0f - num4) : ((!(num6 <= 0f)) ? (0f - num6) : 0f);
		Vector3 localPosition7 = component.localPosition;
		float x3 = localPosition7.x;
		Vector2 sizeDelta4 = component.sizeDelta;
		float x4 = 0f - (x3 + sizeDelta4.x / 2f) + num7;
		base.transform.Find("frame").localPosition = new Vector3(x4, y, 0f);
	}

	private void FixFrameSize()
	{
		float num = base.transform.Find("frame/implement").gameObject.activeSelf ? base.transform.Find("frame/implement").GetComponent<Text>().preferredWidth : 0f;
		base.transform.Find("frame/desText").GetComponent<RectTransform>().sizeDelta = new Vector2(300f + num, 1f);
		float preferredHeight = base.transform.Find("frame/desText").GetComponent<Text>().preferredHeight;
		base.transform.Find("frame/desBg").GetComponent<RectTransform>().sizeDelta = new Vector2(380f + num, 150f + preferredHeight);
		base.transform.Find("frame/desText").GetComponent<RectTransform>().sizeDelta = new Vector2(300f + num, preferredHeight);
		base.transform.Find("frame/desBg/gang").GetComponent<RectTransform>().sizeDelta = new Vector2(3f, 300f + num);
		base.transform.Find("frame/desBg/gang").GetComponent<RectTransform>().localPosition = new Vector2(190f + num / 2f, -92f);
	}
}
