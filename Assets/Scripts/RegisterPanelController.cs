using LitJson;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class RegisterPanelController : MonoBehaviour
{
    public InputField[] m_inputRegister;

    [SerializeField]
    private GameObject registerInvoke;

    private void Start()
    {
        if (registerInvoke == null)
        {
            registerInvoke = base.transform.parent.Find("loginBg").gameObject;
        }
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("register", HandleNetMsg_Register);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("isRegistOK", IsRisterOK);
        m_inputRegister[4].transform.parent.gameObject.SetActive(ZH2_GVars.isInternational);
    }

    private void OnEnable()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string empty = string.Empty;
            empty = CopyTextToClipboard.instance.OnPaste();
            if (empty.Contains("CHECK://"))
            {
                try
                {
                    JsonData jsonData = JsonMapper.ToObject(empty.Replace("CHECK://", string.Empty));
                    m_inputRegister[4].text = jsonData["id"].ToString();
                    m_inputRegister[4].interactable = false;
                }
                catch (Exception)
                {
                    m_inputRegister[4].interactable = true;
                }
            }
            else
            {
                m_inputRegister[4].interactable = true;
            }
        }
        else
        {
            m_inputRegister[4].interactable = true;
        }
         
    }

    public void OnBtnClick_Register()
    {
        if (!InputCheck.CheckUserName(m_inputRegister[0].text.Trim()) || !InputCheck.CheckNickName(m_inputRegister[1].text.Trim()) || !InputCheck.CheckPassWord(m_inputRegister[2].text.Trim()))
        {
            return;
        }
        if (m_inputRegister[3].text.Trim() == m_inputRegister[2].text.Trim())
        {
            if (!ZH2_GVars.isInternational)
            {
                m_inputRegister[4].text = "admin";
            }
            if (m_inputRegister[4].text == string.Empty)
            {
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请输入推荐人账号", "Please enter the recommender account number", "กรุณากรอกหมายเลขอ้างอิง", "Vui lòng nhập tài khoản người giới thiệu"));
                return;
            }
            User user = new User();
            user.username = m_inputRegister[0].text.Trim();
            user.phone = "987654321";
            user.password = m_inputRegister[2].text.Trim();
            user.type = 0;
            user.promoterUsername = m_inputRegister[4].text.Trim().ToLower();
            User user2 = user;
            MB_Singleton<NetManager>.GetInstance().Send("gcuserService/register", new object[1]
            {
                user2
            });
            MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
            MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Register_Invoke, user2.username);
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("两次密码不一致，请重新输入", "The two passwords do not match,please re-enter", "สองครั้งรหัสไม่ตรงกันโปรดป้อนใหม่", "Hai lần mật khẩu không phù hợp, vui lòng nhập lại"));
        }
    }

    public void OnBtnClick_Close()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        InputField[] inputRegister = m_inputRegister;
        foreach (InputField inputField in inputRegister)
        {
            inputField.text = string.Empty;
        }
        if (!base.transform.parent.Find("MainPanel").gameObject.activeSelf)
        {
            MB_Singleton<AppManager>.GetInstance().GetPanel("LoginPanel").SetActive(value: true);
        }
        base.gameObject.SetActive(value: false);
    }

    public void HandleNetMsg_Register(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        if ((bool)dictionary["isRegister"])
        {
            Debug.Log("zhucesssss");
            registerInvoke.GetComponent<RegisterInvoke>().CancelRepeat();
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("注册成功", "Registration Successful", "ลงทะเบียนเรียบร้อยแล้ว", "Đăng ký thành công"), showOkCancel: false, delegate
            {
                MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_LoginUserName, m_inputRegister[0].text);
                OnBtnClick_Close();
            });
            return;
        }
        int num = (int)dictionary["msgCode"];
        switch (num)
        {
            default:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("注册失败", "Registration failed", "การลงทะเบียนล้มเหลว", "Đăng ký thất bại"));
                break;
            case 5:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("推广员不存在", "This extension is not exist", "มันไม่มีอยู่จริงหรอก", "Promoter không tồn tại"));
                break;
            case 4:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号已被注册", "The account has been registered", "ชื่อผู้ใช้ซ้ำ", "Tài khoản đã được đăng ký"));
                break;
            case -3:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("注册已达到上限", "The registration has reached the limit", "การลงทะเบียนมีขีด จำกัด สูงสุด", "Đăng ký đã đạt đến giới hạn"));
                break;
            case -4:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("等待后台验证", "Please wait for the system process", "รอการตรวจสอบเบื้องหลัง", "Chờ background validation"));
                break;
        }
        if (num != -4)
        {
            registerInvoke.GetComponent<RegisterInvoke>().CancelRepeat();
        }
    }

    private void IsRisterOK(object[] objs)
    {
        Debug.Log("IsRisterOK");
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        bool flag = (bool)dictionary["success"];
        switch ((int)dictionary["messageStatus"])
        {
            case 0:
                return;
            case 1:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("注册成功", "Registration Successful", "ลงทะเบียนเรียบร้อยแล้ว", "Đăng ký thành công"), showOkCancel: false, delegate
                {
                    MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_LoginUserName, m_inputRegister[0].text);
                    OnBtnClick_Close();
                });
                break;
            case 2:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("申请未通过", "Application failed", "การสมัครไม่ผ่าน", "Ứng dụng không được thông qua"), showOkCancel: false, OnBtnClick_Close);
                break;
        }
        registerInvoke.GetComponent<RegisterInvoke>().CancelRepeat();
    }
}
