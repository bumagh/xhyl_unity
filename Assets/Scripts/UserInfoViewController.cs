using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UserInfoViewController : Tween_SlowAction
{
    private static UserInfoViewController instance;

    public GameObject[] chooseText;

    [SerializeField]
    private GameObject cancelAccount;

    [SerializeField]
    private GameObject[] panels;

    [SerializeField]
    private Text input_nickName;

    [SerializeField]
    private Text text_accont;

    [SerializeField]
    private Text text_id;

    [SerializeField]
    private Text text_level;

    [SerializeField]
    private Text text_save;

    [SerializeField]
    private Toggle[] toggle_Panels;

    [SerializeField]
    private Toggle[] toggle_Sexs;

    [SerializeField]
    private Button btn_icon;

    [SerializeField]
    private Text text_Gold;

    [SerializeField]
    private Text text_ExpCoin;

    [SerializeField]
    private InputField m_OldPassword;

    [SerializeField]
    private InputField m_newPassword;

    [SerializeField]
    private InputField m_againPassword;

    [SerializeField]
    private Button m_bankNameBtn;

    [SerializeField]
    private InputField m_bankName;

    [SerializeField]
    private InputField m_bankNumber;

    [SerializeField]
    private InputField m_bankAccount;

    [SerializeField]
    private Button SubmitBtn;

    [SerializeField]
    private InputField m_InputName;

    [SerializeField]
    private GameObject ScrollView;

    [SerializeField]
    private GameObject BankContent;

    [SerializeField]
    private Sprite[] spi_btns;

    private Color[] colors = new Color[2]
    {
        new Color(0.98f, 0.95f, 0.85f, 1f),
        new Color(0.25f, 0.12f, 0f, 1f)
    };

    [SerializeField]
    private Button[] btn_panels;

    [SerializeField]
    private GameObject[] btn_arrows;

    private Text[] btn_texts;

    private int m_page;

    private string m_sex;

    private int m_iIconIndex;

    private int m_tempindex = 9;

    private string m_tempsex;

    private string m_tempname = string.Empty;

    private int m_userType;

    private string m_newPW;

    private string m_oldPW;

    private string m_bankname;

    private string m_banknumber;

    private string m_bankaccount;

    private string m_namestring;

    private string[] bankstrings = new string[100]
    {
        "国家开发银行",
        "中国工商银行",
        "中国农业银行",
        "中国银行",
        "中国建设银行",
        "中国邮政储蓄银行",
        "交通银行",
        "招商银行",
        "上海浦东发展银行",
        "兴业银行",
        "华夏银行",
        "广东发展银行",
        "中国名生银行",
        "中信银行",
        "中国光大银行",
        "恒丰银行",
        "浙商银行",
        "渤海银行",
        "平安银行",
        "上海农村商业银行",
        "玉溪市商业银行",
        "尧都农商行",
        "北京银行",
        "上海银行",
        "江苏银行",
        "杭州银行",
        "南京银行",
        "南京银行",
        "宁波银行",
        "微商银行",
        "长沙银行",
        "成都银行",
        "重庆银行",
        "大连银行",
        "南昌银行",
        "福建海峡银行",
        "汉口银行",
        "温州银行",
        "青岛银行",
        "台州银行",
        "嘉兴银行",
        "常熟农村商业银行",
        "南海农村信用联社",
        "常州农村信用联社",
        "内蒙古银行",
        "绍兴银行",
        "顺德农商银行",
        "吴江农商银行",
        "齐商银行",
        "贵阳市商业银行",
        "遵义市商业银行",
        "湖州市商业银行",
        "龙江银行",
        "晋城银行JCBANK",
        "浙江泰隆商业银行",
        "广东省农村信用联社",
        "东莞农村银行",
        "浙江民泰商业银行",
        "广州银行",
        "辽宁市商业银行",
        "江苏省农村信用联合社",
        "廊坊银行",
        "浙江稠州商业银行",
        "德阳商业银行",
        "晋中市商业银行",
        "苏州银行",
        "桂林银行",
        "乌鲁木齐市商业银行",
        "成都农商银行",
        "张家港农村商业银行",
        "东莞银行",
        "莱商银行",
        "北京农村商业银行",
        "天津农商银行",
        "上饶银行",
        "富滇银行",
        "重庆农村商业银行",
        "鞍山银行",
        "宁夏银行",
        "河北银行",
        "华融湘江银行",
        "云南省农村信用社",
        "吉林银行",
        "东营市商业银行",
        "昆仑银行",
        "厄多斯银行",
        "邢台银行",
        "晋商银行",
        "天津银行",
        "营口银行",
        "吉林农信",
        "山东农信",
        "西安银行",
        "河北省农村信用社",
        "宁夏黄河农商业银行",
        "贵州省农村信用社",
        "阜新银行",
        "湖北银行黄石分行",
        "新乡银行",
        "齐鲁银行"
    };

    private Image img_icon;

    [SerializeField]
    private bool isFaker;

    [SerializeField]
    private UserIconDataConfig m_userIconDataConfig;

    private AndroidJavaObject jo;

    private AndroidJavaClass jc;

    public Text lockText;

    public GameObject container;

    public GameObject ChoosePhoto;

    public static UserInfoViewController Get()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("updateUserInfo", HandleNetMsg_UpdateUserInfo);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("updateUserPwd", HandleNetMsg_UpdatePassword);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("addInformation", HandleNetMsg_AddInformation);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("bindingMachineCode", BindingMachineCode);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("unbindingMachineCode", UnbindingMachineCode);
        if (Application.platform == RuntimePlatform.Android)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        for (int i = 0; i < BankContent.transform.childCount; i++)
        {
            BankContent.transform.GetChild(i).GetChild(0).GetComponent<Text>()
                .text = bankstrings[i];
            string str = bankstrings[i];
            BankContent.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate
            {
                m_bankName.text = str;
                m_bankname = str;
                ScrollView.SetActive(value: false);
            });
        }
    }

    private void OnEnable()
    {
        Show();
        btn_texts = new Text[btn_panels.Length];
        for (int i = 0; i < btn_panels.Length; i++)
        {
            btn_texts[i] = btn_panels[i].transform.GetChild(0).GetComponent<Text>();
        }
        if ((bool)input_nickName)
        {
            input_nickName.text = ZH2_GVars.user.nickname;
        }
        SetLockText();
    }

    private void SetLockText()
    {
        if (lockText != null)
        {
            lockText.text = ((!ZH2_GVars.isLock) ? ZH2_GVars.ShowTip("锁定设备", "Lock Device", "อุปกรณ์ล็อค", "Thiết bị khóa") : ZH2_GVars.ShowTip("解锁设备", "Unlock device", "ปลดล็อคอุปกรณ์", "Mở khóa thiết bị"));
        }
    }

    private void Start()
    {
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Reset_UserInfoPanel, this, ResetUserInfoPanel);
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Fresh_UserInfoPanel, this, FreshUI);
        FreshUI(1);
    }

    public void FreshUI(object obj)
    {
        init();
    }

    private void init()
    {
        img_icon = btn_icon.GetComponent<Image>();
        _InitUserInfo();
    }

    private void _InitUserInfo()
    {
        if (isFaker)
        {
            text_ExpCoin.text = "88888";
            text_Gold.text = "66666";
            text_accont.text = "test";
            m_sex = "女";
            input_nickName.text = "小贺子";
            m_userType = 0;
            text_level.text = "LV.1 新手 ";
            text_save.text = "0";
            m_iIconIndex = 0;
            if (m_sex == "男")
            {
                toggle_Sexs[0].isOn = true;
                toggle_Sexs[1].isOn = false;
            }
            else
            {
                toggle_Sexs[1].isOn = true;
                toggle_Sexs[0].isOn = false;
            }
        }
        else
        {
            try
            {
                text_ExpCoin.text = ZH2_GVars.user.expeGold.ToString();
                text_Gold.text = ZH2_GVars.user.gameGold.ToString();
                text_save.text = ZH2_GVars.user.boxGameGold.ToCoinString();
                text_accont.text = ZH2_GVars.AccountName;
                text_id.text = ZH2_GVars.user.gameid.ToString();
                if (text_accont.text.Length > 21)
                {
                    text_accont.text = text_accont.text.Substring(0, 20);
                }
            }
            catch (Exception message)
            {
                Debug.LogError(message);
            }
            try
            {
                if (ZH2_GVars.user.sex == "男")
                {
                    m_sex = "男";
                }
                else
                {
                    m_sex = "女";
                }
                input_nickName.text = ZH2_GVars.user.nickname;
                if (input_nickName.text.Length > 10)
                {
                    input_nickName.text = input_nickName.text.Substring(0, 9);
                }
            }
            catch (Exception message2)
            {
                Debug.LogError(message2);
            }
            try
            {
                m_userType = ZH2_GVars.user.type;
                text_level.text = ZH2_GVars.ShowTip($"等级{ZH2_GVars.user.level}", $"LV.{ZH2_GVars.user.level}", $"เกรด{ZH2_GVars.user.level}", $"Lớp{ZH2_GVars.user.level}");
                toggle_Sexs[0].isOn = false;
                toggle_Sexs[1].isOn = false;
                if (ZH2_GVars.user.sex == "男")
                {
                    toggle_Sexs[0].isOn = true;
                    toggle_Sexs[1].isOn = false;
                    Debug.Log("男 ZH2_GVars.user.sex: " + ZH2_GVars.user.sex);
                }
                else
                {
                    toggle_Sexs[0].isOn = false;
                    toggle_Sexs[1].isOn = true;
                }
            }
            catch (Exception message3)
            {
                Debug.LogError(message3);
            }
            try
            {
                m_iIconIndex = ZH2_GVars.user.photoId;
                img_icon.sprite = m_userIconDataConfig.list[m_iIconIndex].sprite;
            }
            catch (Exception arg)
            {
                Debug.LogError(m_iIconIndex + " " + arg);
                ZH2_GVars.user.photoId = 0;
                m_iIconIndex = ZH2_GVars.user.photoId;
                img_icon.sprite = m_userIconDataConfig.list[m_iIconIndex].sprite;
            }
            Transform transform = base.transform.Find("ChoosePhoto/Container/Scroll View/Viewport/Content");
            for (int i = 0; i < m_userIconDataConfig.list.Count; i++)
            {
                try
                {
                    transform.Find("btn_" + i + "/Photo").GetComponent<Image>().sprite = m_userIconDataConfig.list[i].sprite;
                    if (i == m_iIconIndex)
                    {
                        transform.Find("btn_" + i + "/Xuan").gameObject.SetActive(value: true);
                    }
                }
                catch (Exception message4)
                {
                    Debug.LogError(message4);
                }
            }
        }
    }

    public void ClickBtnCopy()
    {
        ClickBtnCopy(text_id);
    }

    private void ClickBtnCopy(Text text)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        string empty = string.Empty;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            CopyTextToClipboard.instance.OnCopy(text.text);
        }
        else
        {
            TextEditor textEditor = new TextEditor();
            textEditor.text = text.text;
            TextEditor textEditor2 = textEditor;
            textEditor2.OnFocus();
            textEditor2.Copy();
        }
        All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("复制成功", "Copy successfully", "คัดลอกสำเร็จ", "Sao chép thành công"));
    }

    public void RefreshLevelText()
    {
        text_level.text = ZH2_GVars.ShowTip($"等级{ZH2_GVars.user.level}", $"LV.{ZH2_GVars.user.level}", $"เกรด{ZH2_GVars.user.level}", $"Lớp{ZH2_GVars.user.level}");
    }

    public void RefreshGoldAndExp()
    {
        text_ExpCoin.text = ZH2_GVars.user.expeGold.ToString();
        text_Gold.text = ZH2_GVars.user.gameGold.ToString();
        text_save.text = ZH2_GVars.user.boxGameGold.ToCoinString();
    }

    public void Send_UserInfo(string nickname, string sex, int photoId)
    {
        object[] args = new object[3]
        {
                nickname,
                sex,
                photoId
        };
        MB_Singleton<NetManager>.GetInstance().Send("gcuserService/updateUserInfo", args);
    }

    public void Send_ChangePassword(string _strOld, string _strNew)
    {
        object[] args = new object[2]
        {
                _strOld,
                _strNew
        };
        MB_Singleton<NetManager>.GetInstance().Send("gcuserService/updateUserPwd", args);
        Debug.Log("Send_ChangePassword: old: " + _strOld + " new: " + _strNew);
    }

    public void HandleNetMsg_UpdateUserInfo(object[] args)
    {
        Debug.Log("HandleNetMsg__UpdateUserInfo");
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary = (args[0] as Dictionary<string, object>);
        bool flag = (bool)dictionary["success"];
        Debug.Log("success:" + flag);
        if (flag)
        {
            m_tempindex = m_iIconIndex;
            m_tempname = input_nickName.text;
            m_tempsex = m_sex;
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("修改成功", "Change sucess", "แก้ไขสำเร็จ", "Sửa đổi thành công"), showOkCancel: false, delegate
            {
                ResetUserInfoPanel(true);
            });
            ZH2_GVars.user.photoId = m_iIconIndex;
            ZH2_GVars.user.nickname = input_nickName.text;
            ZH2_GVars.user.sex = m_sex;
            MB_Singleton<AppManager>.Get().Notify(UIGameMsgType.UINotify_Fresh_UserInfo);
        }
    }

    private void ResetUserInfoPanel(object obj)
    {
        Debug.Log("ResetUserInfoPanel: " + obj);
        GameObject[] array = chooseText;
        foreach (GameObject gameObject in array)
        {
            gameObject.SetActive(value: false);
        }
        bool flag = (bool)obj;
        m_OldPassword.text = string.Empty;
        m_newPassword.text = string.Empty;
        m_againPassword.text = string.Empty;
        if (!flag)
        {
            QuestionsViewController.instance.ResetSafePanel();
        }
    }

    public void HandleNetMsg_UpdatePassword(object[] args)
    {
        Debug.Log("HandleNetMsg_UpdatePassword");
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary = (args[0] as Dictionary<string, object>);
        bool flag = (bool)dictionary["success"];
        Debug.Log("success:" + flag);
        if (flag)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("修改成功", "Change sucess", "แก้ไขสำเร็จ", "Sửa đổi thành công"), showOkCancel: false, delegate
            {
                ResetUserInfoPanel(true);
            });
            return;
        }
        string content = (string)dictionary["msg"];
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog(content);
    }

    public void BindingMachineCode(object[] objs)
    {
        ZH2_GVars.isLock = true;
        SetLockText();
        All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("设备锁定成功", "Device locked successfully", "อุปกรณ์ล็อคสำเร็จ", "Khóa thiết bị thành công"));
    }

    public void UnbindingMachineCode(object[] objs)
    {
        ZH2_GVars.isLock = false;
        SetLockText();
        All_GameMiniTipPanel.publicMiniTip.ShowTip(ZH2_GVars.ShowTip("设备解锁成功", "Device unlocked successfully", "ปลดล็อคอุปกรณ์สำเร็จ", "Mở khóa thiết bị thành công"));
    }

    public void HandleNetMsg_AddInformation(object[] objs)
    {
        Debug.Log("HandleNetMsg_AddInformation");
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary = (objs[0] as Dictionary<string, object>);
        if ((bool)dictionary["success"])
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("绑定银行成功", "Successfully bound bank", "ผูกธนาคารสำเร็จ", "Mở khóa thiết bị thành công"), showOkCancel: false, delegate
            {
                m_bankName.interactable = false;
                m_bankNameBtn.interactable = false;
                m_bankNumber.interactable = false;
                m_bankAccount.interactable = false;
                m_bankName.interactable = false;
                SubmitBtn.interactable = false;
                ZH2_GVars.user.bankName = m_bankName.text;
                ZH2_GVars.user.accountBankName = m_bankNumber.text;
                ZH2_GVars.user.BankCard = m_bankAccount.text;
                ZH2_GVars.user.BankUserName = m_InputName.text;
            });
            return;
        }
        string content = (string)dictionary["msg"];
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog(content);
    }

    public void OnYoukeForbiddenClick(int type)
    {
        Debug.Log("OnYoukeForbiddenClick");
        if (m_userType == 1)
        {
            if (type == 0)
            {
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游客账号无法修改密码", "Only Vip Can Change Password", "หมายเลขผู้เข้าชมไม่สามารถแก้ไขรหัสผ่านได้", "Tài khoản khách không thể thay đổi mật khẩu"), showOkCancel: false, ResetUserPanelGroup);
            }
            if (type == 1)
            {
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游客账号无法进行设置", "Guest account can not be set", "ไม่สามารถตั้งค่าบัญชีผู้เข้าชมได้", "Tài khoản khách không thể thiết lập"), showOkCancel: false, ResetUserPanelGroup);
            }
        }
        else
        {
            MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        }
    }

    public void OnPlayMusic()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
    }

    public void ResetUserPanelGroup()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Debug.Log("ResetUserPanelGroup");
        toggle_Panels[0].isOn = true;
        toggle_Panels[1].isOn = false;
        toggle_Panels[2].isOn = false;
        panels[0].SetActive(value: true);
        panels[1].SetActive(value: false);
        panels[2].SetActive(value: false);
        toggle_Panels[0].interactable = false;
    }

    public void ResetUserPanels()
    {
        Debug.LogError("======重置个人信息页面=====");
        m_page = 0;
        btn_panels[0].image.sprite = spi_btns[0];
        btn_panels[1].image.sprite = spi_btns[1];
        btn_panels[2].image.sprite = spi_btns[1];
        btn_panels[3].image.sprite = spi_btns[1];
        SetArrow(0);
        for (int i = 0; i < btn_texts.Length; i++)
        {
            btn_texts[i].color = colors[1];
        }
        btn_texts[0].color = colors[0];
        panels[0].SetActive(value: true);
        panels[1].SetActive(value: false);
        panels[2].SetActive(value: false);
        panels[3].SetActive(value: false);
        cancelAccount.SetActive(value: false);
    }

    private void SetArrow(int index)
    {
        for (int i = 0; i < btn_arrows.Length; i++)
        {
            if (i == index)
            {
                btn_arrows[i].SetActive(value: true);
            }
            else
            {
                btn_arrows[i].SetActive(value: false);
            }
        }
    }

    public void ChangeSex()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (toggle_Sexs[1].isOn)
        {
            m_sex = "女";
        }
        else
        {
            m_sex = "男";
        }
    }

    public void ChangeIcon(IconButtonsInfo iconinfo)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        m_iIconIndex = iconinfo.index;
        img_icon.sprite = m_userIconDataConfig.list[m_iIconIndex].sprite;
        GameObject[] array = chooseText;
        foreach (GameObject gameObject in array)
        {
            gameObject.SetActive(value: false);
        }
        chooseText[m_iIconIndex].SetActive(value: true);
        Debug.Log("changeIcon: " + m_iIconIndex);
    }

    public void UpdateGoldNum()
    {
        text_ExpCoin.text = ZH2_GVars.user.expeGold.ToString();
        text_Gold.text = ZH2_GVars.user.gameGold.ToString();
        text_save.text = ZH2_GVars.user.boxGameGold.ToCoinString();
    }

    public void ChangePassword()
    {
        m_newPW = m_newPassword.text;
        m_oldPW = m_OldPassword.text;
    }

    public void LockEquipment()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        object[] array = new object[2]
        {
                ZH2_GVars.userId,
                ZH2_GVars.GetSoleIdentify
        };
        object[] array2 = new object[1]
        {
                ZH2_GVars.userId
        };
        string text = "gcuserService/bindingMachineCode";
        string text2 = "gcuserService/unbindingMachineCode";
        MB_Singleton<NetManager>.GetInstance().Send((!ZH2_GVars.isLock) ? text : text2, (!ZH2_GVars.isLock) ? array : array2);
    }

    public void ClickProperty()
    {
        if (ZH2_GVars.user.BankCard != "-1")
        {
            m_bankName.text = ZH2_GVars.user.bankName;
            m_bankNumber.text = ZH2_GVars.user.accountBankName;
            m_bankAccount.text = ZH2_GVars.user.BankCard;
            m_InputName.text = ZH2_GVars.user.BankUserName;
            m_bankName.interactable = false;
            m_bankNumber.interactable = false;
            m_bankAccount.interactable = false;
            m_InputName.interactable = false;
            m_bankNameBtn.interactable = false;
            SubmitBtn.interactable = false;
        }
        else
        {
            m_bankName.text = string.Empty;
            m_bankNumber.text = string.Empty;
            m_bankAccount.text = string.Empty;
            m_InputName.text = string.Empty;
            m_bankName.interactable = true;
            m_bankNumber.interactable = true;
            m_bankAccount.interactable = true;
            m_InputName.interactable = true;
            m_bankNameBtn.interactable = true;
            SubmitBtn.interactable = true;
        }
    }

    public void ClearProperty()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        m_bankName.interactable = true;
        m_bankNumber.interactable = true;
        m_bankAccount.interactable = true;
        m_InputName.interactable = true;
        m_bankNameBtn.interactable = true;
        SubmitBtn.interactable = true;
        m_bankName.text = string.Empty;
        m_bankNumber.text = string.Empty;
        m_bankAccount.text = string.Empty;
        m_InputName.text = string.Empty;
    }

    public void ClickBankNameBtn()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ScrollView.activeSelf)
        {
            ScrollView.SetActive(value: false);
        }
        else
        {
            ScrollView.SetActive(value: true);
        }
    }

    public void SendOnProperty()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (m_bankName.text == string.Empty)
        {
            MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("请输入正确的银行名称", "Please enter the correct bank name", "กรุณากรอกชื่อธนาคารที่ถูกต้อง", "Vui lòng nhập đúng tên ngân hàng"), showOkCancel: false, delegate
            {
                MB_Singleton<AlertDialog>.GetInstance().gameObject.SetActive(value: false);
            });
            return;
        }
        if (m_bankNumber.text == string.Empty)
        {
            MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("请输入正确的开户银行", "Please enter the correct bank", "กรุณากรอกธนาคารที่เปิดบัญชีถูกต้อง", "Vui lòng nhập đúng ngân hàng mở tài khoản"), showOkCancel: false, delegate
            {
                MB_Singleton<AlertDialog>.GetInstance().gameObject.SetActive(value: false);
            });
            return;
        }
        if (m_bankAccount.text == string.Empty)
        {
            MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("请输入正确的银行账号", "Please enter the correct bank account number", "กรุณากรอกหมายเลขบัญชีธนาคารที่ถูกต้อง", "Vui lòng nhập đúng số tài khoản ngân hàng"), showOkCancel: false, delegate
            {
                MB_Singleton<AlertDialog>.GetInstance().gameObject.SetActive(value: false);
            });
            return;
        }
        if (m_InputName.text == string.Empty)
        {
            MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("请输入正确的姓名", "Please enter the correct name", "กรุณากรอกชื่อที่ถูกต้อง", "Vui lòng nhập đúng tên"), showOkCancel: false, delegate
            {
                MB_Singleton<AlertDialog>.GetInstance().gameObject.SetActive(value: false);
            });
            return;
        }
        m_bankname = m_bankName.text;
        m_banknumber = m_bankNumber.text;
        m_bankaccount = m_bankAccount.text;
        m_namestring = m_InputName.text;
        MB_Singleton<NetManager>.GetInstance().Send("gcuserService/addInformation", new object[4]
        {
                m_bankname,
                m_banknumber,
                m_bankaccount,
                m_namestring
        });
    }

    public void OnPanelClose()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        ResetUserPanels();
        Hide(base.gameObject);
        if (input_nickName.text == m_tempname && m_iIconIndex == m_tempindex && m_sex == m_tempsex)
        {
            Debug.LogError("被放回1");
            return;
        }
        if (input_nickName.text == ZH2_GVars.user.nickname && m_iIconIndex == ZH2_GVars.user.photoId && m_sex == ZH2_GVars.user.sex)
        {
            Debug.LogError("被放回2");
            return;
        }
        Send_UserInfo(input_nickName.text, m_sex, m_iIconIndex + 1);
        ZH2_GVars.user.photoId = m_iIconIndex;
        ZH2_GVars.user.nickname = input_nickName.text;
        ZH2_GVars.user.sex = m_sex;
        MB_Singleton<AppManager>.Get().Notify(UIGameMsgType.UINotify_Fresh_UserInfo);
    }

    public void OnPanelClose2()
    {
        Debug.LogError("点击修改账号昵称");
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        MB_Singleton<AppManager>.Get().m_mainPanel.OppenPanel("ModifyNickname");
        base.gameObject.SetActive(value: false);
    }

    public void ShowChoosePhoto()
    {
        container.SetActive(value: false);
        Show(ChoosePhoto.transform);
    }

    public void CloseChoosePhoto()
    {
        container.SetActive(value: true);
        Hide(ChoosePhoto.transform, ChoosePhoto);
    }

    public void OnBtnClick_PasswordOK()
    {
        if (!InputCheck.CheckPassWord(m_newPassword.text))
        {
            return;
        }
        ChangePassword();
        if (m_againPassword.text == m_newPW)
        {
            if (m_OldPassword.text == m_newPassword.text)
            {
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("新旧密码重复，请重新输入", "Repeat Password,Reset Please", "รหัสผ่านเก่าทำซ้ำโปรดป้อนใหม่", "Mật khẩu cũ và mới được lặp lại, vui lòng nhập lại"));
                return;
            }
            Send_ChangePassword(m_oldPW, m_newPW);
            MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("密码输入不匹配", "Two input is inconsistent", "ป้อนรหัสผ่านไม่ตรงกัน", "Nhập mật khẩu không khớp"));
        }
    }

    public void OnClickToggles(int index)
    {
        Debug.LogError("=====OnClickToggles====" + index);
    }

    public void OnClickButtons(int index)
    {
        if (index == m_page)
        {
            Debug.LogError(index + "=====相同 返回======");
            return;
        }
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        panels[index].SetActive(value: true);
        SetArrow(index);
        panels[(index + 1) % 4].SetActive(value: false);
        panels[(index + 2) % 4].SetActive(value: false);
        panels[(index + 3) % 4].SetActive(value: false);
        btn_panels[(index + 1) % 4].interactable = true;
        btn_panels[(index + 2) % 4].interactable = true;
        btn_panels[(index + 3) % 4].interactable = true;
        btn_panels[index].image.sprite = spi_btns[0];
        btn_panels[m_page].image.sprite = spi_btns[1];
        btn_texts[index].color = colors[0];
        btn_texts[m_page].color = colors[1];
        m_page = index;
    }

    public void ShowCancelAccount(Transform transform)
    {
        Show(transform);
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
    }

    public void HieCancelAccount(Transform transform)
    {
        Hide(transform, transform.gameObject);
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
    }

    public void ChangeAccount()
    {
        ResetUserPanels();
        base.gameObject.SetActive(value: false);
        MB_Singleton<AppManager>.Get().HideAllPanels();
        MB_Singleton<AppManager>.Get().ChangeAccount();
        MB_Singleton<NetManager>.Get().isReady = false;
        MB_Singleton<NetManager>.Get().isLogined = false;
        MB_Singleton<NetManager>.Get().autoReady = false;
        ZH2_GVars.isStartedFromGame = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
