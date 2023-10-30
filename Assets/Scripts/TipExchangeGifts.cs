using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TipExchangeGifts : MonoBehaviour
{
    private int m_itemId;

    private int m_buyCount;

    private string m_pwd = string.Empty;

    [SerializeField]
    private LotteryItemList _lotteryItemList;

    private void OnEnable()
    {
        SwitchIntoEnglish();
    }

    private void Start()
    {
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("checkUserPwd", HandleNetMsg_CheckUserPwd);
    }

    private void SwitchIntoEnglish()
    {
        if (ZH2_GVars.language_enum != 0)
        {
            base.transform.Find("btnSure/Text").GetComponent<Text>().text = "Buy";
            base.transform.Find("tip").GetComponent<Text>().fontSize = 27;
            base.transform.Find("InputPassword/Placeholder").GetComponent<Text>().text = "Enter password";

            switch (ZH2_GVars.language_enum)
            {
                case ZH2_GVars.LanguageType.Chinese:
                    base.transform.Find("btnSure/Text").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().zh_font;
                    base.transform.Find("tip").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().zh_font;
                    base.transform.Find("InputPassword/Placeholder").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().zh_font;
                    break;
                case ZH2_GVars.LanguageType.English:
                    base.transform.Find("btnSure/Text").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().en_font;
                    base.transform.Find("tip").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().en_font;
                    base.transform.Find("InputPassword/Placeholder").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().en_font;
                    break;
                case ZH2_GVars.LanguageType.Thai:
                    base.transform.Find("btnSure/Text").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().th_font;
                    base.transform.Find("tip").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().th_font;
                    base.transform.Find("InputPassword/Placeholder").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().th_font;
                    break;
                case ZH2_GVars.LanguageType.Vietnam:
                    base.transform.Find("btnSure/Text").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().vn_font;
                    base.transform.Find("tip").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().vn_font;
                    base.transform.Find("InputPassword/Placeholder").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().vn_font;
                    break;
            }
        }
    }

    public void ShowUI(int itemId)
    {
        m_itemId = itemId;
        m_buyCount = ((_lotteryItemList.list[m_itemId - 1].price <= ZH2_GVars.user.lottery) ? 1 : 0);
        UpdateBuycount();
        UpdateTopTip();
    }

    public void OnBtnClick_Sure()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (_lotteryItemList.list[m_itemId - 1].price > ZH2_GVars.user.lottery)
        {
            switch (ZH2_GVars.language_enum)
            {
                case ZH2_GVars.LanguageType.Chinese:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("彩票不足");
                    break;
                case ZH2_GVars.LanguageType.English:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Insufficient number of lottery tickets");
                    break;
                case ZH2_GVars.LanguageType.Thai:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("ไม่พอสำหรับการจับสลาก");
                    break;
                case ZH2_GVars.LanguageType.Vietnam:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Xổ số không đủ");
                    break;
            }
            return;
        }
        m_pwd = base.transform.Find("InputPassword").GetComponent<InputField>().text;
        if (m_buyCount == 0)
        {
            switch (ZH2_GVars.language_enum)
            {
                case ZH2_GVars.LanguageType.Chinese:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("请输入正确的购买数量");
                    break;
                case ZH2_GVars.LanguageType.English:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Please input correct num");
                    break;
                case ZH2_GVars.LanguageType.Thai:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("โปรดป้อนจำนวนการซื้อ ที่ถูกต้อง");
                    break;
                case ZH2_GVars.LanguageType.Vietnam:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Vui lòng nhập đúng số lượng mua");
                    break;
            }
        }
        else if (m_pwd == string.Empty)
        {
            switch (ZH2_GVars.language_enum)
            {
                case ZH2_GVars.LanguageType.Chinese:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("请输入密码");
                    break;
                case ZH2_GVars.LanguageType.English:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Please input password");
                    break;
                case ZH2_GVars.LanguageType.Thai:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("โปรดป้อนรหัสผ่าน");
                    break;
                case ZH2_GVars.LanguageType.Vietnam:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Vui lòng nhập mật khẩu");
                    break;
            }
        }
        else
        {
            MB_Singleton<NetManager>.GetInstance().Send("gcshopService/checkUserPwd", new object[1]
            {
                m_pwd
            });
        }
    }

    public void OnBtnClick_Cancel()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        UnityEngine.Object.Destroy(base.gameObject);
    }

    public void OnBtnClick_Add()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Click_PlusMinus);
        if ((m_buyCount + 1) * _lotteryItemList.list[m_itemId - 1].price > ZH2_GVars.user.lottery)
        {
            m_buyCount = ((_lotteryItemList.list[m_itemId - 1].price <= ZH2_GVars.user.lottery) ? 1 : 0);
        }
        else
        {
            m_buyCount++;
        }
        UpdateBuycount();
    }

    public void OnBtnClick_Reduce()
    {
        if (m_buyCount <= 0)
        {
            m_buyCount = 0;
        }
        else if (m_buyCount == 1)
        {
            m_buyCount = Convert.ToInt32(ZH2_GVars.user.lottery / _lotteryItemList.list[m_itemId - 1].price);
        }
        else
        {
            m_buyCount--;
        }
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Click_PlusMinus);
        UpdateBuycount();
    }

    private void UpdateBuycount()
    {
        base.transform.Find("InputBuyCount").GetComponent<InputField>().text = m_buyCount.ToString();
    }

    public void UpdateTopTip()
    {
        string text = base.transform.Find("InputBuyCount").GetComponent<InputField>().text;
        if (text.Contains("-"))
        {
            m_buyCount = Convert.ToInt32(ZH2_GVars.user.lottery / _lotteryItemList.list[m_itemId - 1].price);
        }
        else
        {
            m_buyCount = Convert.ToInt32(text);
        }
        if (m_buyCount * _lotteryItemList.list[m_itemId - 1].price > ZH2_GVars.user.lottery)
        {
            m_buyCount = Convert.ToInt32(ZH2_GVars.user.lottery / _lotteryItemList.list[m_itemId - 1].price);
        }
        else if (m_buyCount < 0)
        {
            switch (ZH2_GVars.language_enum)
            {
                case ZH2_GVars.LanguageType.Chinese:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("非法数字");
                    break;
                case ZH2_GVars.LanguageType.English:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Illegal number");
                    break;
                case ZH2_GVars.LanguageType.Thai:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("หมายเลขผิดกฎหมาย");
                    break;
                case ZH2_GVars.LanguageType.Vietnam:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Số bất hợp pháp");
                    break;
            }
            m_buyCount = Convert.ToInt32(ZH2_GVars.user.lottery / _lotteryItemList.list[m_itemId - 1].price);
        }
        UpdateBuycount();
        switch (ZH2_GVars.language_enum)
        {
            case ZH2_GVars.LanguageType.Chinese:
            case ZH2_GVars.LanguageType.Thai:
            case ZH2_GVars.LanguageType.Vietnam:
                base.transform.Find("tip").GetComponent<Text>().text = string.Format("确认花费<size=38><color=#E4C751>{0}彩票</color></size>购买{1}么？", m_buyCount * _lotteryItemList.list[m_itemId - 1].price, _lotteryItemList.list[m_itemId - 1].zh_name);
                break;
            case ZH2_GVars.LanguageType.English:
                base.transform.Find("tip").GetComponent<Text>().text = string.Format( "Confirm pay {0} lotteries for {1}?", m_buyCount * _lotteryItemList.list[m_itemId - 1].price, _lotteryItemList.list[m_itemId - 1].en_name);
                break;         
        }      
    }

    private void HandleNetMsg_CheckUserPwd(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        UnityEngine.Debug.Log(dictionary);
        if ((bool)dictionary["success"])
        {
            if ((bool)dictionary["isCorrect"])
            {
                UnityEngine.Object.Destroy(base.gameObject);
                MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_ReveiveAddressPanel, new object[3]
                {
                    m_itemId + 11,
                    m_buyCount,
                    m_pwd
                });
            }
            else
            {
                switch (ZH2_GVars.language_enum)
                {
                    case ZH2_GVars.LanguageType.Chinese:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog( "密码错误");
                        break;
                    case ZH2_GVars.LanguageType.English:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("password error");
                        break;
                    case ZH2_GVars.LanguageType.Thai:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("รหัสผ่านไม่ถูกต้อง");
                        break;
                    case ZH2_GVars.LanguageType.Vietnam:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Mật khẩu sai");
                        break;
                }
            }
            return;
        }
        switch ((int)dictionary["msgCode"])
        {
            case 2:
            case 3:
                switch (ZH2_GVars.language_enum)
                {
                    case ZH2_GVars.LanguageType.Chinese:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
                        break;
                    case ZH2_GVars.LanguageType.English:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Wrong account or password");
                        break;
                    case ZH2_GVars.LanguageType.Thai:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง");
                        break;
                    case ZH2_GVars.LanguageType.Vietnam:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Lỗi tài khoản hoặc mật khẩu");
                        break;
                }
                break;          
            case 6:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号被冻结,请联系管理员", "This account has been frozen", "เลขบัญชีถูกระงับ", "Tài khoản bị đóng băng, vui lòng liên hệ với quản trị viên"));
                break;
            case 7:
                switch (ZH2_GVars.language_enum)
                {
                    case ZH2_GVars.LanguageType.Chinese:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("系统维护");
                        break;
                    case ZH2_GVars.LanguageType.English:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Server maintenance");
                        break;
                    case ZH2_GVars.LanguageType.Thai:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("การดูแลระบบ");
                        break;
                    case ZH2_GVars.LanguageType.Vietnam:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Bảo trì hệ thống");
                        break;
                }
                break;
            case 11:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请勿重复申请", "Please do not repeat the application", "ห้ามสมัครซ้ำซ้อน", "Không lặp lại ứng dụng"));
                break;
            default:
                switch (ZH2_GVars.language_enum)
                {
                    case ZH2_GVars.LanguageType.Chinese:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("失败");
                        break;
                    case ZH2_GVars.LanguageType.English:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("failed");
                        break;
                    case ZH2_GVars.LanguageType.Thai:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("ล้มเหลว");
                        break;
                    case ZH2_GVars.LanguageType.Vietnam:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Thất bại");
                        break;
                }
                break;
        }
    }
}
