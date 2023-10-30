using LitJson;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CashPanelController : MonoBehaviour
{
    private int m_cashCount;

    public Sprite[] FlagSprites;

    public InputField[] CashInputField;

    public GameObject HelpPanel;

    public Button FlagBtn;

    public GameObject FlagPanel;

    public GameObject FlagContent;

    private string flagNum = string.Empty;

    private void Start()
    {
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("excharge", HandleNetMsg_Excharge);
        for (int i = 0; i < FlagContent.transform.childCount; i++)
        {
            int _num = i;
            FlagContent.transform.GetChild(i).GetChild(0).GetComponent<Image>()
                .sprite = FlagSprites[i];
            string text = string.Empty;
            string text2 = string.Empty;
            switch (i)
            {
                case 0:
                    text = "+60";
                    text2 = "马来西亚";
                    break;
                case 1:
                    text = "+62";
                    text2 = "印尼";
                    break;
                case 2:
                    text = "+63";
                    text2 = "菲律宾";
                    break;
                case 3:
                    text = "+66";
                    text2 = "泰国";
                    break;
                case 4:
                    text = "+84";
                    text2 = "越南";
                    break;
                case 5:
                    text = "+852";
                    text2 = "香港";
                    break;
                case 6:
                    text = "+86";
                    text2 = "中国";
                    break;
                case 7:
                    text = "+886";
                    text2 = "台湾";
                    break;
                case 8:
                    text = "+91";
                    text2 = "印度";
                    break;
                case 9:
                    text = "+95";
                    text2 = "缅甸";
                    break;
            }
            FlagContent.transform.GetChild(i).GetChild(2).GetComponent<Text>()
                .text = text;
            FlagContent.transform.GetChild(i).GetChild(1).GetComponent<Text>()
                .text = text2;
            FlagContent.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate
            {
                OnBtnClick_Flag(_num);
            });
        }
    }

    private void OnEnable()
    {
        ResetView();
    }

    public void ResetView()
    {
        for (int i = 0; i < CashInputField.Length; i++)
        {
            CashInputField[i].text = string.Empty;
        }
        FlagBtn.transform.Find("Flag/Image").gameObject.SetActive(value: false);
        FlagBtn.transform.Find("Text").GetComponent<Text>().text = "区号";
        FlagPanel.SetActive(value: false);
    }

    public void OnBtnClick_Close()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        base.gameObject.SetActive(value: false);
    }

    public void ClickHelp(int type)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        HelpPanel.SetActive(value: true);
        Text component = HelpPanel.transform.Find("Container/Text").GetComponent<Text>();
        switch (type)
        {
            case 1:
                component.text = ZH2_GVars.ShowTip("本日替换额度100000000金币（上限\n100000000金币 下限5000金币）", "Current replacement limit of 100000000 gold coins (upper limit of 100000000 gold coins and lower limit of 5000 gold coins)", "วงเงินทดแทนภายในวันนี้ 1,000,000,000 เหรียญทอง (เพดาน 1,000,000,000 เหรียญทอง วงเงินต่ำสุด 5,000 เหรียญทอง)", "Hạn mức thay thế trong ngày là 100.000.000 đồng vàng (giới hạn trên là 100.000 đồng vàng, giới hạn dưới là 5.000 đồng vàng)");
                break;
            case 2:
                component.text = ZH2_GVars.ShowTip("请您务必填写当前正确的联系方式 以便我们\n能及时与您取得联系", "Please make sure to fill in the current correct contact information so that we can contact you in a timely manner", "โปรดตรวจสอบให้แน่ใจว่าคุณกรอกข้อมูลการติดต่อที่ถูกต้องในปัจจุบันเพื่อให้เราสามารถติดต่อคุณได้ทันเวลา", "Vui lòng điền đầy đủ thông tin liên lạc để chúng tôi có thể liên lạc kịp thời.");
                break;
            case 3:
                component.text = ZH2_GVars.ShowTip("系统将直接导入您设置的银行账号 用户可以\n在大厅的【财信】中确定相关信息", "The system will directly import the bank account you have set. Users can confirm the relevant information in the 'Financial Information' section of the lobby", "ระบบจะนำเข้าบัญชีธนาคารที่คุณตั้งค่าไว้โดยตรง ผู้ใช้สามารถระบุข้อมูลที่เกี่ยวข้องใน [Caixin] ที่ล็อบบี้", "Hệ thống sẽ trực tiếp nhập vào tài khoản ngân hàng mà bạn đã thiết lập, người dùng có thể xác định thông tin liên quan trong [Tài chính] ở sảnh");
                break;
        }
    }

    public void OnCloseHelpPanel()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        HelpPanel.gameObject.SetActive(value: false);
    }

    public void OnBtnClick_Flag()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (FlagPanel.activeSelf)
        {
            FlagPanel.SetActive(value: false);
        }
        else
        {
            FlagPanel.SetActive(value: true);
        }
    }

    public void OnBtnClick_Flag(int type)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        FlagBtn.transform.Find("Flag/Image").gameObject.SetActive(value: true);
        FlagBtn.transform.Find("Flag/Image").GetComponent<Image>().sprite = FlagSprites[type];
        switch (type)
        {
            case 0:
                flagNum = "+60";
                FlagBtn.transform.Find("Text").GetComponent<Text>().text = "马来西亚";
                break;
            case 1:
                flagNum = "+62";
                FlagBtn.transform.Find("Text").GetComponent<Text>().text = "印尼";
                break;
            case 2:
                flagNum = "+63";
                FlagBtn.transform.Find("Text").GetComponent<Text>().text = "菲律宾";
                break;
            case 3:
                flagNum = "+66";
                FlagBtn.transform.Find("Text").GetComponent<Text>().text = "泰国";
                break;
            case 4:
                flagNum = "+84";
                FlagBtn.transform.Find("Text").GetComponent<Text>().text = "越南";
                break;
            case 5:
                flagNum = "+852";
                FlagBtn.transform.Find("Text").GetComponent<Text>().text = "香港";
                break;
            case 6:
                flagNum = "+86";
                FlagBtn.transform.Find("Text").GetComponent<Text>().text = "中国";
                break;
            case 7:
                flagNum = "+886";
                FlagBtn.transform.Find("Text").GetComponent<Text>().text = "台湾";
                break;
            case 8:
                flagNum = "+91";
                FlagBtn.transform.Find("Text").GetComponent<Text>().text = "印度";
                break;
            case 9:
                flagNum = "+95";
                FlagBtn.transform.Find("Text").GetComponent<Text>().text = "缅甸";
                break;
        }
        FlagPanel.SetActive(value: false);
    }

    public void OnBtnClick_Sure()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (flagNum == string.Empty)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请输入区号", "Please enter the area code", "กรุณากรอกรหัสพื้นที่", "Vui lòng nhập mã vùng"));
            return;
        }
        string num = base.transform.Find("Container/cashItem/cashItem_InputField").GetComponent<InputField>().text;
        string text = base.transform.Find("Container/phoneItem/input").GetComponent<InputField>().text;
        string text2 = base.transform.Find("Container/noteItem/input").GetComponent<InputField>().text;
        string text3 = base.transform.Find("Container/password/input").GetComponent<InputField>().text;
        if (num == string.Empty)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("兑换额度不能为空", "The amount cannot be blank", "วงเงินแลกไม่สามารถว่างได้", "Hạn mức đổi không được để trống"), showOkCancel: false, delegate
            {
                ResetView();
            });
        }
        else if (Convert.ToInt32(num) == 0)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("兑换额度不能为零", "The amount cannot be 0", "ระดับการแลกเปลี่ยนจะไม่เป็นศูนย์", "Hạn mức đổi không được bằng 0"), showOkCancel: false, delegate
            {
                ResetView();
            });
        }
        else if (Convert.ToInt32(num) < 0)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("兑奖额度输入有误", "Incorrect input of redemption amount", "ที่ระดับเสียงสะสม", "Giải thưởng được nhập sai"), showOkCancel: false, delegate
            {
                ResetView();
            });
        }
        else if (text == string.Empty)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请输入您的手机号码", "Please enter your mobile phone number", "กรุณากรอกหมายเลขโทรศัพท์มือถือ", "Vui lòng nhập số điện thoại di động của bạn"));
        }
        else if (text3 == string.Empty)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游戏密码不能为空", "please enter the password", "รหัสผ่านของเกมต้องไม่ว่างเปล่า", "Mật khẩu trò chơi không được để trống"), showOkCancel: false, delegate
            {
                if (Convert.ToInt64(num) > ZH2_GVars.user.gameGold)
                {
                    ResetView();
                }
            });
        }
        else if (Convert.ToInt64(num) > ZH2_GVars.user.gameGold)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游戏币不足", "Coin shortage", "เงินเกมไม่เพียงพอ", "Game không đủ tiền"), showOkCancel: false, delegate
            {
                ResetView();
            });
        }
        else
        {
            MB_Singleton<NetManager>.GetInstance().Send("gcuserService/exCharge", new object[3]
            {
                            Convert.ToInt64(num),
                            flagNum + text,
                            text3
            });
        }
    }

    public void CashNumChanged()
    {
        string text = base.transform.Find("cashItem/input").GetComponent<InputField>().text;
        if (text.Contains("-"))
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("兑奖额度输入有误", "Incorrect input of redemption amount", "ที่ระดับเสียงสะสม", "Giải thưởng được nhập sai"));
        }
        else if (text != string.Empty)
        {
            string text2 = (Convert.ToInt64(text) > 100000) ? "100000" : text;
            base.transform.Find("cashItem/input").GetComponent<InputField>().text = text2;
            text = text2;
            m_cashCount = Convert.ToInt32(text);
        }
        else
        {
            m_cashCount = 0;
        }
        UpdateCashcount();
        base.transform.Find("cashItem/equalMoney").GetComponent<Text>().text = string.Format(ZH2_GVars.ShowTip("兑换{0}元宝", "Amount to {0} yuan", "แลกเปลี่ยน{0}พอล", "Đổi {0} Nguyên Bảo"), m_cashCount);
    }

    private void UpdateCashcount()
    {
        base.transform.Find("cashItem/input").GetComponent<InputField>().text = m_cashCount.ToString();
    }

    private void HandleNetMsg_Excharge(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        Debug.Log("HandleNetMsg_Excharge: " + JsonMapper.ToJson(objs));
        if ((bool)dictionary["success"])
        {
            base.gameObject.SetActive(value: false);
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("发送成功 \n（兑奖申请正在处理，稍后将提示您处理结果）", "Send Successful！\n (Your exchange application is being processed,later we will prompt you the result)", "ส่งสำเร็จ \n (ใบสมัครรับรางวัลกำลังดำเนินการอยู่จะแจ้งให้ทราบในภายหลัง)", "Gửi thành công \n (Đơn xin đổi thưởng đang được xử lý, bạn sẽ được nhắc nhở về kết quả sau)"));
            return;
        }
        Debug.LogError("=====兑换失败====");
        int num = 0;
        try
        {
            num = (int)dictionary["messageStatus"];
        }
        catch (Exception message)
        {
            Debug.LogError(message);
        }
        Debug.Log(num);
        switch (num)
        {
            case 1:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
                break;
            case 3:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请勿重复申请", "Please do not repeat the application", "ห้ามสมัครซ้ำซ้อน", "Không lặp lại ứng dụng"));
                break;
            default:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("兑换失败", "Cash failed", "การแลกเปลี่ยนล้มเหลว", "Đổi thất bại"));
                break;
        }
    }
}
