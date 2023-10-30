using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class RechargePanelController : MonoBehaviour
{
    private InputField pwdItem;

    private InputField noteItem;

    private void Awake()
    {
        pwdItem = base.transform.Find("pwdItem/input").GetComponent<InputField>();
        noteItem = base.transform.Find("noteItem/input").GetComponent<InputField>();
    }

    private void Start()
    {
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("recharge", HandleNetMsg_Recharge);
    }

    private void OnEnable()
    {
        ResetView();
    }

    private void SwitchIntoEnglish()
    {
        base.transform.Find("titleBg/title").GetComponent<Text>().text = ZH2_GVars.ShowTip("充值", "Recharge", "เติมเงิน", "Nạp tiền");
        base.transform.Find("rechargeItem/itemName").GetComponent<Text>().text = ZH2_GVars.ShowTip("充值数目:", "Recharge quantity:", "จำนวนการเติมเงิน:", "Số tiền nạp:");
        base.transform.Find("rechargeItem/input/Placeholder").GetComponent<Text>().text = ZH2_GVars.ShowTip("输入数目", "Number of inputs", "จำนวนอินพุต", "Nhập số");
        base.transform.Find("noteItem/itemName").GetComponent<Text>().text = ZH2_GVars.ShowTip("备注信息:", "Remarks:", "หมายเหตุ ข้อมูล:", "Thông tin ghi chú:");
        base.transform.Find("noteItem/input/Placeholder").GetComponent<Text>().text = ZH2_GVars.ShowTip("输入备注信息", "Enter note information", "ป้อนข้อมูลหมายเหตุ", "Nhập thông tin ghi chú");
        base.transform.Find("sureBtn/Text").GetComponent<Text>().text = ZH2_GVars.ShowTip("确定", "determine", "ระบุ", "Xác định");
    }

    private void ResetView()
    {
        pwdItem.text = string.Empty;
        noteItem.text = string.Empty;
    }

    public void OnBtnClick_Close()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        base.gameObject.SetActive(value: false);
    }

    public void OnBtnClick_Sure()
    {
        string text = pwdItem.text;
        if (text == string.Empty)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请填入数字", "Please fill in the numbers", "กรุณาเติมตัวเลข", "Vui lòng điền số"));
            return;
        }
        int num = Convert.ToInt32(text);
        string text2 = noteItem.text;
        if (num > 100000 || num < 1)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("充值额度范围为：1-100000", "Recharge limit:1-10000", "ระยะวงโคจร ที่ชาร์จอยู่คือ 100-100,000", "Phạm vi nạp tiền là: 1-100000"));
            return;
        }
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        MB_Singleton<NetManager>.GetInstance().Send("gcuserService/recharge", new object[2]
        {
            num,
            text2
        });
    }

    public void RechargeNumChanged()
    {
        string text = pwdItem.text;
        base.transform.Find("rechargeItem/equalCurrency").GetComponent<Text>().text = string.Format(ZH2_GVars.ShowTip("共{0}游戏币", "Amount to {0} game gold", "เอาเลย{0}ชื่อเกม", "Tổng cộng {0} tiền trò chơi"), (text != string.Empty) ? Convert.ToInt64(text) : 0);
    }

    private void HandleNetMsg_Recharge(object[] objs)
    {
        pwdItem.text = string.Empty;
        noteItem.text = string.Empty;
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        if ((bool)dictionary["success"])
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("发送请求成功!\n(充值申请正在处理，稍后将提示您处理结果)", "Send Successful!\n(Your exchange application is being processed,later we will prompt you the result)", "ส่งสำเร็จ (ใบสมัครรับรางวัลกำลังดำเนินการอยู่จะแจ้งให้ทราบในภายหลัง) ", "Gửi yêu cầu thành công!\n(Yêu cầu nạp tiền đang được xử lý, bạn sẽ được nhắc để xử lý kết quả sau)"));
            return;
        }
        int num = 100;
        if (dictionary.ContainsKey("msgCode"))
        {
            num = (int)dictionary["msgCode"];
        }
        else if (dictionary.ContainsKey("messageStatus"))
        {
            num = (int)dictionary["messageStatus"];
        }
        if (num == 1)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请勿重复申请", "Please do not repeat the application", "ห้ามสมัครซ้ำซ้อน", "Không lặp lại ứng dụng"));
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请求失败,请稍后再试", "Request failed, please try again later", "ล\u0e49มเหลว", "Yêu cầu không thành công, vui lòng thử lại sau"));
        }
    }
}
