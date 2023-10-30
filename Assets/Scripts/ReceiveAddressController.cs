using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ReceiveAddressController : MonoBehaviour
{
    public object[] buySendData;

    private void Start()
    {
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("bugByLottery", HandleNetMsg_Buy);
    }

    private void OnEnable()
    {
        ResetView();
        SwitchIntoEnglish();
    }

    private void SwitchIntoEnglish()
    {
        if (ZH2_GVars.language_enum != 0)
        {
            base.transform.Find("TelItem/itemName").GetComponent<Text>().text = "Recipient number:";
            base.transform.Find("TelItem/itemName").GetComponent<Text>().fontSize = 33;
            base.transform.Find("TelItem/itemName").localPosition = new Vector3(-177.95f, 145f, 0f);
            base.transform.Find("TelItem/input").GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 80f);
            base.transform.Find("TelItem/input").localPosition = new Vector3(409.4f, 145f, 0f);
            base.transform.Find("TelItem/input/Placeholder").GetComponent<Text>().text = "5-20 numbers";
            base.transform.Find("addressItem/itemName").GetComponent<Text>().text = "Shipping address:";
            base.transform.Find("addressItem/itemName").GetComponent<Text>().fontSize = 33;
            base.transform.Find("addressItem/itemName").localPosition = new Vector3(-177.95f, 145f, 0f);
            base.transform.Find("addressItem/input").GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 184.7f);
            base.transform.Find("addressItem/input").localPosition = new Vector3(409.4f, 185f, 0f);
            base.transform.Find("addressItem/input/Placeholder").GetComponent<Text>().text = "10-100 characters";
            base.transform.Find("sureBtn/Text").GetComponent<Text>().text = "Submit";
            switch (ZH2_GVars.language_enum)
            {
                case ZH2_GVars.LanguageType.Chinese:
                    base.transform.Find("TelItem/itemName").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().zh_font;
                    base.transform.Find("TelItem/input").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().zh_font;
                    base.transform.Find("TelItem/Placeholder").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().zh_font;
                    base.transform.Find("addressItem/itemName").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().zh_font;
                    base.transform.Find("addressItem/input").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().zh_font;
                    base.transform.Find("addressItem/input/Placeholder").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().zh_font;
                    base.transform.Find("sureBtn/Text").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().zh_font;
                    break;
                case ZH2_GVars.LanguageType.English:
                    base.transform.Find("TelItem/itemName").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().en_font;
                    base.transform.Find("TelItem/input").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().en_font;
                    base.transform.Find("TelItem/Placeholder").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().en_font;
                    base.transform.Find("addressItem/itemName").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().en_font;
                    base.transform.Find("addressItem/input").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().en_font;
                    base.transform.Find("addressItem/input/Placeholder").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().en_font;
                    base.transform.Find("sureBtn/Text").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().en_font;
                    break;
                case ZH2_GVars.LanguageType.Thai:
                    base.transform.Find("TelItem/itemName").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().th_font;
                    base.transform.Find("TelItem/input").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().th_font;
                    base.transform.Find("TelItem/Placeholder").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().th_font;
                    base.transform.Find("addressItem/itemName").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().th_font;
                    base.transform.Find("addressItem/input").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().th_font;
                    base.transform.Find("addressItem/input/Placeholder").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().th_font;
                    base.transform.Find("sureBtn/Text").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().th_font;
                    break;
                case ZH2_GVars.LanguageType.Vietnam:
                    base.transform.Find("TelItem/itemName").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().vn_font;
                    base.transform.Find("TelItem/input").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().vn_font;
                    base.transform.Find("TelItem/Placeholder").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().vn_font;
                    base.transform.Find("addressItem/itemName").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().vn_font;
                    base.transform.Find("addressItem/input").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().vn_font;
                    base.transform.Find("addressItem/input/Placeholder").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().vn_font;
                    base.transform.Find("sureBtn/Text").GetComponent<Text>().font = MB_Singleton<AppManager>.Get().vn_font;
                    break;
            }


        }
    }

    private void ResetView()
    {
        base.transform.Find("TelItem/input").GetComponent<InputField>().text = string.Empty;
        base.transform.Find("addressItem/input").GetComponent<InputField>().text = string.Empty;
    }

    public void OnBtnClick_Close()
    {
        base.gameObject.SetActive(value: false);
    }

    public void OnBtnClick_Sure()
    {
        string text = base.transform.Find("TelItem/input").GetComponent<InputField>().text;
        string text2 = base.transform.Find("addressItem/input").GetComponent<InputField>().text;
        if (text.Length < 5)
        {
            switch (ZH2_GVars.language_enum)
            {
                case ZH2_GVars.LanguageType.Chinese:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("请输入5-15手机号码");
                    break;
                case ZH2_GVars.LanguageType.English:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Wrong phone number");
                    break;
                case ZH2_GVars.LanguageType.Thai:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("กรุณาใส่เบอร์โทรศัพท์มือถือ 5-15");
                    break;
                case ZH2_GVars.LanguageType.Vietnam:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Vui lòng nhập số điện thoại 5-15");
                    break;
            }
            return;
        }
        if (text2.Length < 10)
        {
            switch (ZH2_GVars.language_enum)
            {
                case ZH2_GVars.LanguageType.Chinese:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("收货地址输入错误");
                    break;
                case ZH2_GVars.LanguageType.English:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Wrong shipping address");
                    break;
                case ZH2_GVars.LanguageType.Thai:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("ข้อผิดพลาดในการรับ ที่อยู่");
                    break;
                case ZH2_GVars.LanguageType.Vietnam:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Nhập sai địa chỉ nhận hàng");
                    break;
            }
            return;
        }
        UnityEngine.Debug.Log(string.Empty + buySendData[0] + buySendData[1] + buySendData[2]);
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        MB_Singleton<NetManager>.GetInstance().Send("gcshopService/bugByLottery", new object[5]
        {
            buySendData[0],
            buySendData[1],
            buySendData[2],
            text,
            text2
        });
    }

    private void HandleNetMsg_Buy(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        UnityEngine.Debug.Log(dictionary);
        if ((bool)dictionary["success"])
        {
            UnityEngine.Object.Destroy(base.gameObject);
            ZH2_GVars.user.lottery = (int)dictionary["lottery"];
            MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);

            switch (ZH2_GVars.language_enum)
            {
                case ZH2_GVars.LanguageType.Chinese:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("发送成功（兑奖申请正在处理，稍后将提示您处理结果）");
                    break;
                case ZH2_GVars.LanguageType.English:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Send Successful！Your exchange application is being processed,later we will prompt you the result");
                    break;
                case ZH2_GVars.LanguageType.Thai:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("ส่งสำเร็จ (ใบสมัครรับรางวัลกำลังดำเนินการอยู่จะแจ้งให้ทราบในภายหลัง)");
                    break;
                case ZH2_GVars.LanguageType.Vietnam:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Gửi thành công (đơn xin đổi thưởng đang được xử lý, bạn sẽ được nhắc xử lý kết quả sau)");
                    break;
            }
            return;
        }
        int num = (int)dictionary["msgCode"];
        UnityEngine.Debug.Log(num);
        switch (num)
        {
            case 0:
                switch (ZH2_GVars.language_enum)
                {
                    case ZH2_GVars.LanguageType.Chinese:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("程序异常");
                        break;
                    case ZH2_GVars.LanguageType.English:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Program exception");
                        break;
                    case ZH2_GVars.LanguageType.Thai:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("โปรแกรม ที่ผิดปกติ");
                        break;
                    case ZH2_GVars.LanguageType.Vietnam:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Chương trình ngoại lệ");
                        break;
                }
                break;
            case 2:
            case 3:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
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
            case 12:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("余额不足", "balance", "ยอดคงเหลือไม่เพียงพอ", "Số dư không đủ"));                 
                break;
            default:
                switch (ZH2_GVars.language_enum)
                {
                    case ZH2_GVars.LanguageType.Chinese:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("购买失败");
                        break;
                    case ZH2_GVars.LanguageType.English:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Purchase failed");
                        break;
                    case ZH2_GVars.LanguageType.Thai:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("การซื้อล้มเหลว");
                        break;
                    case ZH2_GVars.LanguageType.Vietnam:
                        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("Mua thất bại");
                        break;
                }            
                break;
        }
    }
}
