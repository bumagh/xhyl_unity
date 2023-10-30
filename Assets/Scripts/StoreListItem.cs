using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class StoreListItem : MonoBehaviour
{
    private int m_itemId;

    public Transform _itemDesPrefab;

    [SerializeField]
    private ShopPropList _shopProplist;

    private void Awake()
    {
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Fresh_OwnedProps, this, FreshOwnedProps);
    }

    private void OnEnable()
    {
        SwitchIntoEnglish();
    }

    private void SwitchIntoEnglish()
    {
        if (ZH2_GVars.language_enum != 0)
        {
            transform.Find("ItemNum").GetComponent<Text>().fontSize = 25;
            transform.Find("ItemNum").GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 25);
            transform.Find("buyBtn/Text").GetComponent<Text>().fontSize = 30;
            transform.Find("remainTime/Text").GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Arial", 25);
        }
    }

    private void FreshOwnedProps(object obj)
    {
        if (ZH2_GVars.ownedProps.ContainsKey(m_itemId))
        {
            transform.Find("used").gameObject.SetActive(true);
            transform.Find("remainTime").gameObject.SetActive(true);
            transform.Find("remainTime/Text").GetComponent<Text>().text = string.Format("{0}{1}{2}{3}{4}", ZH2_GVars.ShowTip("剩余：", "Countdown:", "ส่วนที่เหลือ:", "Còn lại:"), Mathf.FloorToInt(ZH2_GVars.ownedProps[m_itemId].RemainTime / 86400000), ZH2_GVars.ShowTip("天", " d ", "วัน", "Ngày"), Mathf.FloorToInt(ZH2_GVars.ownedProps[m_itemId].RemainTime % 86400000 / 3600000), ZH2_GVars.ShowTip("小时", " h", "เวลา", "Khi"));
        }
        else
        {
            transform.Find("used").gameObject.SetActive(false);
            transform.Find("remainTime").gameObject.SetActive(false);
        }
    }

    public void ShowUI(int itemId)
    {
        m_itemId = itemId;
        transform.Find("ItemNum").GetComponent<Text>().text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese) ? _shopProplist.list[m_itemId - 1].zh_name : _shopProplist.list[m_itemId - 1].en_name);
        if (m_itemId <= 4)
        {
            transform.Find("buyBtn/Text").GetComponent<Text>().text = string.Format("{0}{1}", _shopProplist.list[m_itemId - 1].price, ZH2_GVars.ShowTip("元宝", "Gold", "หยวนเป่า", "Nguyên Bảo"));
        }
        else
        {
            transform.Find("buyBtn/Text").GetComponent<Text>().text = string.Format("{0}{1}", _shopProplist.list[m_itemId - 1].price, ZH2_GVars.ShowTip("币", "Coins", "เหรียญ", "Tiền xu"));
        }
        transform.Find("Item").GetComponent<RawImage>().texture = (Resources.Load("Icon/" + _shopProplist.list[m_itemId - 1].iconName) as Texture);
        if (ZH2_GVars.ownedProps.ContainsKey(m_itemId))
        {
            transform.Find("used").gameObject.SetActive(true);
            transform.Find("remainTime").gameObject.SetActive(true);
            transform.Find("remainTime/Text").GetComponent<Text>().text = string.Format("{0}{1}{2}{3}{4}", ZH2_GVars.ShowTip("剩余：", "Countdown:", "ส่วนที่เหลือ:", "Còn lại:"), Mathf.FloorToInt(ZH2_GVars.ownedProps[m_itemId].RemainTime / 86400000), ZH2_GVars.ShowTip("天", " d ", "วัน", "Ngày"), Mathf.FloorToInt(ZH2_GVars.ownedProps[m_itemId].RemainTime % 86400000 / 3600000), ZH2_GVars.ShowTip("小时", " h", "เวลา", "Khi"));
        }
    }

    public void OnBtnClick_ShowDes()
    {
        MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Close_StoreItemInfo);
        base.transform.Find("guang").gameObject.SetActive(true);
        Transform transform = Instantiate(_itemDesPrefab);
        transform.parent = base.transform;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.GetComponent<ItemDes>().ShowUI(m_itemId);
    }

    public void OnBtnClick_Buy()
    {
        if (ZH2_GVars.user.card == "-1" && !UserCompleteController.UserCompleteNoPrompt && !UserCompleteController.UserTempNoPrompt)
        {
            MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_UserCompletePanel, new ArrayList
            {
                "Store",
                m_itemId
            });
            return;
        }
        MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Close_StoreItemInfo);
        if (ZH2_GVars.ownedProps.ContainsKey(m_itemId))
        {
            MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_ContinueBuyShopItems, m_itemId);
        }
        else
        {
            MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_BuyShopItems, m_itemId);
        }
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
    }
}
