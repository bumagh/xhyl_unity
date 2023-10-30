using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;

public class SafeBoxPwdSetPanelController : MonoBehaviour
{
    private Transform container;

    private InputField pwdItem;

    private InputField repwdItem;

    private void Start()
    {
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("setSafeBoxPwd", HandleNetMsg_PwdSet);
    }

    private void OnEnable()
    {
        container = base.transform.Find("Container");
        pwdItem = container.Find("pwdItem/input").GetComponent<InputField>();
        repwdItem = container.Find("repwdItem/input").GetComponent<InputField>();
        pwdItem.text = string.Empty;
        repwdItem.text = string.Empty;
    }

    public void OnBtnClick_Sure()
    {
        string text = pwdItem.text;
        string text2 = repwdItem.text;
        if (!Regex.IsMatch(text, "^[0-9]+$"))
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("密码格式错误", "Password format error", "รูปแบบรหัสผ่านไม่ถูกต้อง", "Định dạng mật khẩu sai"));
        }
        else if (text.Length != 6)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("密码格式错误", "Password format error", "รูปแบบรหัสผ่านไม่ถูกต้อง", "Định dạng mật khẩu sai"));
        }
        else if (text == text2)
        {
            MB_Singleton<NetManager>.GetInstance().Send("gcsecurityService/setSafeBoxPwd", new object[1]
            {
                text
            });
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("", "", string.Empty));
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("两次输入不一致!", "Entered passwords differ!", "การป้อนข้อมูลสองครั้งไม่สอดคล้องกัน", "Hai đầu vào không phù hợp"));
        }
    }

    private void HandleNetMsg_PwdSet(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        UnityEngine.Debug.Log(dictionary);
        if ((bool)dictionary["success"])
        {
            base.gameObject.SetActive(value: false);
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("设置成功!", "Set success!", "ตั้งค่าสำเร็จ", "Thiết lập thành công"), showOkCancel: false, delegate
            {
                MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Open_SafeBoxPwdCheckPanel);
                MB_Singleton<AppManager>.GetInstance().m_mainPanel.OpenSafePanel();
            });
            ZH2_GVars.user.safeBox = 1;
            return;
        }
        int num = (int)dictionary["msgCode"];
        UnityEngine.Debug.Log(num);
        switch (num)
        {
            case 6:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号被冻结", "This account has been frozen", "หมายเลขบัญชีถูกระงับ", "Tài khoản bị đóng băng"));
                break;
            case 7:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("系统维护", "Server maintenance", "การบำรุงรักษาระบบ", "Bảo trì hệ thống"));
                break;
            case 11:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请勿重复申请", "Please do not repeat the application", "ห้ามสมัครซ้ำซ้อน", "Không lặp lại ứng dụng"));
                break;
            case 13:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("用户名不存在", "The account does not exist", "ไม่มีชื่อผู้ใช้", "Tên người dùng không tồn tại"));
                break;
            case 14:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请输入6位密码", "Please enter the 6-digit password!", "กรุณากรอกรหัสผ่าน 6 หลัก", "Vui lòng nhập mật khẩu 6 chữ số"));
                break;
            default:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("此功能未开启", "set failed!", "ฟังก์ชันนี้ยังไม่เปิด", "Chức năng này không được bật"));
                break;
        }
    }
}
