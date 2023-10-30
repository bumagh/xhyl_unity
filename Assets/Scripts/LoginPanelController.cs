using JsonFx.Json;

using LitJson;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LoginPanelController : Tween_SlowAction
{
    [SerializeField]
    private InputField m_inputName;

    [SerializeField]
    private InputField m_inputPwd;

    [SerializeField]
    private Toggle m_toggleRemeberPassWord;

    private bool m_hasInited;

    public GameObject btnShow;

    public GameObject LanguagePanel;

    public GameObject Container;

    private Button btnShowB;

    

    public Button xButton;

    public Text Version;

    public LoginuserListUI userList;

    public GameObject loadPanel;

    private Coroutine waitSendLogin;

    private bool isLogin;

    private void Awake()
    {
        btnShowB = btnShow.GetComponent<Button>();
        btnShowB.onClick.AddListener(OnClickBtnShow);
        xButton.onClick.AddListener(OnClickXBtn);
        Version.text = "v" + Application.version;
    }

    private void OnEnable()
    {
      //  FindAllObjectsInScene.RefreshAllTxt();
        switch (PlayerPrefs.GetInt("GVarsLanguage", 0))
        {
            case 0:
                ZH2_GVars.language_enum = ZH2_GVars.LanguageType.Chinese;
                break;
            case 1:
                ZH2_GVars.language_enum = ZH2_GVars.LanguageType.English;
                break;
            case 2:
                ZH2_GVars.language_enum = ZH2_GVars.LanguageType.Vietnam;
                break;
            case 3:
                ZH2_GVars.language_enum = ZH2_GVars.LanguageType.Thai;
                break;
        }
        base.transform.localScale = Vector3.one;
        SwitchLang(ZH2_GVars.language_enum);
        SetTransformScale();
        btnShow.SetActive(value: true);
        Container.SetActive(value: false);
        ZH2_GVars.sendLogin = (Action)Delegate.Combine(ZH2_GVars.sendLogin, new Action(SendLogin));
    }

    private void OnDisable()
    {
        ZH2_GVars.sendLogin = (Action)Delegate.Remove(ZH2_GVars.sendLogin, new Action(SendLogin));
    }

    private void OnClickBtnShow()
    {
        Container.SetActive(value: true);
        btnShow.SetActive(value: false);
        ZH2_GVars.LoadLoginUsers();
        Show();
    }

    private void OnClickXBtn()
    {
        btnShow.SetActive(value: true);
        Hide(Container);
    }

    public void ShowUserList()
    {
        string text = m_inputName.text;
        List<string> list = new List<string>();
        int num = 0;
        foreach (KeyValuePair<string, string> loginUser in ZH2_GVars.loginUsers)
        {
            string key = loginUser.Key;
            list.Add(key);
            num++;
        }
        userList.Show(list, text);
    }

    public void ChangeUser(string username)
    {
        m_inputName.text = username;
        m_inputPwd.text = ZH2_GVars.GetLoginUserPassword(username);
        userList.Hide();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (PlayerPrefs.GetInt("isONPassWord") == 0)
        {
            m_toggleRemeberPassWord.isOn = true;
        }
        else
        {
            m_toggleRemeberPassWord.isOn = false;
        }
        if (m_toggleRemeberPassWord.isOn)
        {
            m_inputPwd.text = PlayerPrefs.GetString("passWord");
            m_inputName.text = PlayerPrefs.GetString("userName");
        }
        //FindAllObjectsInScene.RefreshAllTxt();
        if (!m_hasInited)
        {
            m_hasInited = true;
            MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_FreshDealer, -1);
            MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Fresh_LoginUserName, this, SetUserName);
            MB_Singleton<NetManager>.GetInstance().RegisterHandler("login", HandleNetMsg_Login);
            MB_Singleton<NetManager>.GetInstance().RegisterHandler("needSignIn", HandleNetMsg_SignIn);
            MB_Singleton<NetManager>.GetInstance().RegisterHandler("visitorLogin", HandleNetMsg_Visitor);
            MB_Singleton<NetManager>.GetInstance().RegisterHandler("bonusRainStart", HandleNetMsg_BonusRainStart);
            MB_Singleton<NetManager>.GetInstance().RegisterHandler("bonusRainStop", HandleNetMsg_BonusRainStop);
            if (!ZH2_GVars.isStartedFromGame && !ZH2_GVars.isStartedFromLuaGame)
            {
                MB_Singleton<AppManager>.Get().StartCoroutine(Utils.DelayCall(0.1f, delegate
                {
                    MB_Singleton<AppManager>.Get().Notify(UIGameMsgType.UINotify_Allow_LoadingFinish);
                }));
            }
        }
    }

    public void ToggleRemberUserName(Toggle t)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
       Debug.Log("Toggle t.isON: " + t.isOn);
        PlayerPrefs.SetInt("isON", (!t.isOn) ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleRemberPassWord(Toggle t)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
       Debug.Log("Toggle t.isON: " + t.isOn);
        PlayerPrefs.SetInt("isONPassWord", (!t.isOn) ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnBtnClick_Login()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if ((object)loadPanel != null)
        {
            loadPanel.SetActive(value: true);
        }
        isLogin = false;
        if (InputCheck.CheckLoginUserName(m_inputName.text) && InputCheck.CheckLoginPassWord(m_inputPwd.text))
        {
            ZH2_GVars.username = m_inputName.text.Trim();
            ZH2_GVars.pwd = m_inputPwd.text.Trim();
            if (waitSendLogin != null)
            {
                StopCoroutine(waitSendLogin);
            }
            waitSendLogin = StartCoroutine(WaitSendLogin());
        }
        else if ((object)loadPanel != null)
        {
            loadPanel.SetActive(value: false);
        }
    }

    private IEnumerator WaitSendLogin()
    {
       Debug.LogError("===开始等待秘钥===");
        while (!ZH2_GVars.isGetPublicKey)
        {
            yield return null;
        }
       Debug.LogError("===秘钥获取完毕===");
        if (!MB_Singleton<AppManager>.Get().CheckStartConnectAndLogin(willLogin: true))
        {
            MB_Singleton<AppManager>.Get().Send_Login();
            isLogin = true;
        }
        else
        {
           Debug.LogError("=========执行2========");
        }
    }

    private void SendLogin()
    {
        if (!isLogin && loadPanel != null && loadPanel.activeInHierarchy)
        {
           Debug.LogError("===回调执行登录===");
            MB_Singleton<AppManager>.Get().Send_Login();
            isLogin = true;
        }
    }

    public void OnBtnClick_ForgetPassword()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("重置密码请联系客服", "To reset the password, contact customer service", "โปรดติดต่อฝ่ายบริการลูกค้าเพื่อตั้งรหัสผ่านใหม่", "Đặt lại mật khẩu Vui lòng liên hệ"));
    }

    public void OnBtnClick_VisitorLogin()
    {
    }

    public void SetUserName(object obj)
    {
        m_inputName.text = obj.ToString();
        m_inputPwd.text = string.Empty;
    }

    public void OnBtnClick_Register()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        MB_Singleton<AppManager>.GetInstance().GetPanel("RegisterPanel").SetActive(value: true);
        base.gameObject.SetActive(value: false);
    }

    public void OnBtnClick_SwitchLanguage()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        LanguagePanel.SetActive(true);

        /*
        btnShow.gameObject.SetActive(false);
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese)
        {
            ZH2_GVars.language_enum = ZH2_GVars.LanguageType.English;
        }
        else if (ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English)
        {
            ZH2_GVars.language_enum = ZH2_GVars.LanguageType.Vietnam;
        }
        else
        {
            ZH2_GVars.language_enum = ZH2_GVars.LanguageType.Chinese;
        }
        SwitchLang(ZH2_GVars.language_enum);
        */
    }

    private void SwitchLang(ZH2_GVars.LanguageType lang)
    {
        btnShow.gameObject.SetActive(true);
        Debug.LogError("当前语言: " + ZH2_GVars.language_enum);
        PlayerPrefs.SetInt("GVarsLanguage", (int)ZH2_GVars.language_enum);
        ZH2_GVars.language_enum = lang;
      // FindAllObjectsInScene.RefreshAllTxt();
    }

    public void ShowAnnouce()
    {
        if (ZH2_GVars.isStartedFromGame)
        {
           Debug.LogError("从游戏返回  不显示公告");
        }
        else if (ZH2_GVars.IsOpen("notice"))
        {
           Debug.LogError("=====公告开启====");
            MB_Singleton<NetManager>.GetInstance().Send("gcuserService/getNotice", new object[1]
            {
                0
            });
            MB_Singleton<AppManager>.GetInstance().GetPanel("AnnouncementPanel").SetActive(value: true);
        }
        else
        {
           Debug.LogError("=====公未开启====");
        }
    }

    private void SetTransformScale()
    {
        if (ZH2_GVars.isStartedFromGame)
        {
           Debug.LogError("从游戏返回 不显示登录页面");
            base.transform.localScale = Vector3.zero;
            if (base.gameObject.activeInHierarchy)
            {
                StartCoroutine(WaitSetScale());
            }
        }
        else
        {
            base.transform.localScale = Vector3.one;
        }
    }

    private IEnumerator WaitSetScale()
    {
        yield return new WaitForSeconds(1.5f);
        if (base.gameObject.activeInHierarchy)
        {
            base.transform.localScale = Vector3.one;
        }
    }

    public void HandleNetMsg_Login(object[] objs)
    {
        isLogin = true;
        if ((object)loadPanel != null)
        {
            loadPanel.SetActive(value: false);
        }
        if (objs != null)
        {
           Debug.LogError("Msg_Login: " + JsonMapper.ToJson(objs));
        }
        FindAllObjectsInScene.RefreshAllTxt();
        MB_Singleton<AppManager>.Get().Handle_RecLoginOrVisitorLogin();
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        if ((bool)dictionary["isLogin"])
        {
           Debug.LogError("==========登录成功");
            if (MB_Singleton<AppManager>.Get() != null)
            {
                MB_Singleton<AppManager>.Get().SendTransAll(false);
            }


            AppManager.initNet = true;
            if (m_toggleRemeberPassWord.isOn)
            {
                PlayerPrefs.SetString("passWord", m_inputPwd.text.Trim());
                PlayerPrefs.SetString("userName", m_inputName.text.Trim());
            }
            else
            {
                PlayerPrefs.SetString("passWord", string.Empty);
                m_inputPwd.text = string.Empty;
                PlayerPrefs.SetString("userName", string.Empty);
                m_inputName.text = string.Empty;
            }
            ZH2_GVars.AddLoginUser();
            PlayerPrefs.Save();
            ZH2_GVars.user = User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
            ZH2_GVars.payMode = (int)dictionary["payMode"];
            ZH2_GVars.userId = ZH2_GVars.user.id;
            ZH2_GVars.nickname = ZH2_GVars.user.nickname;
            base.gameObject.SetActive(value: false);
            MB_Singleton<AppManager>.Get().ShowMainPanel();
            MB_Singleton<AppManager>.Get().Notify(UIGameMsgType.UINotify_Allow_LoadingFinish);
            MB_Singleton<AppManager>.Get().Notify(UIGameMsgType.UINotify_Fresh_UseMailSign, 0);
            UserCompleteController.UserCompleteNoPrompt = false;
            string updateAddress = (string)dictionary["updateAddress"];
            InnerGameManager.Get().UpdateAddress(updateAddress);
            object args = dictionary["gameRunStatus"];

            JsonData status = JsonMapper.ToObject(JsonMapper.ToJson(dictionary["gameRunStatus"]));
            Debug.Log("游戏状态："+status.ToJson());

            InnerGameManager.Get().UpdateGameRunStatus(args);
            object args2 = dictionary["functionOpen"];
            InnerGameManager.Get().UpdateFunctionOpen(args2);
            ShowAnnouce();
            ZH2_GVars.ownedProps.Clear();
            object[] array = dictionary.ContainsKey("prop") ? ((object[])dictionary["prop"]) : new object[0];
            for (int i = 0; i < array.Length; i++)
            {
                Dictionary<string, object> dictionary2 = array[i] as Dictionary<string, object>;
                ZH2_GVars.ownedProps.Add(Convert.ToInt32(dictionary2["propId"]), new OwnShopProp(Convert.ToInt32(dictionary2["propId"]), Convert.ToInt64(dictionary2["remainTime"]), (bool)dictionary2["show"]));
            }
            for (int j = 9; j < 12; j++)
            {
                if (ZH2_GVars.ownedProps.ContainsKey(j))
                {
                    MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_FreshDealer, j - 8);
                    break;
                }
                if (j == 11)
                {
                    MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_FreshDealer, 0);
                }
            }
            MB_Singleton<NetManager>.GetInstance().isLogined = true;
        }
        else
        {
            switch ((int)dictionary["msgCode"])
            {
                case 2:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog( ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
                    break;
                case 3:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
                    break;
                default:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("登录失败,请检查网络", "Login failed", "การล็อกอินล้มเหลว", "Đăng nhập thất bại, vui lòng kiểm tra mạng"));
                    break;
                case 16:
                    MB_Singleton<AppManager>.Get().Send_Login();
                    break;
                case 6:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号被冻结,请联系管理员", "This account has been frozen", "เลขบัญชีถูกระงับ", "Tài khoản bị đóng băng, vui lòng liên hệ với quản trị viên"));
                    break;
                case 7:
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("系统维护,敬请谅解", "Server maintenance", "การดูแลระบบ", "Bảo trì hệ thống, xin thông cảm"), showOkCancel: false, delegate
                    {
                        base.gameObject.SetActive(value: true);
                    });
                    break;
            }
            MB_Singleton<AppManager>.Get().Handle_LoginFailed();
        }
    }

    public void HandleNetMsg_SignIn(object[] objs)
    {
        ZH2_GVars.activity_signIn = new JsonData();
        ZH2_GVars.activity_signIn = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
    }

    public void DeletelAll()
    {
        m_inputName.text = string.Empty;
        m_inputPwd.text = string.Empty;
        PlayerPrefs.SetString("userName", string.Empty);
        PlayerPrefs.SetString("passWord", string.Empty);
        PlayerPrefs.Save();
    }

    public void HandleNetMsg_BonusRainStart(object[] objs)
    {
        base.transform.parent.Find("RedBonusRain").gameObject.SetActive(value: true);
        RedBonusControl._redBonusControl.RedBonusStart();
    }

    public void HandleNetMsg_BonusRainStop(object[] objs)
    {
        if (base.transform.parent.Find("RedBonusRain").gameObject.activeSelf)
        {
            RedBonusControl._redBonusControl.RedBonusStop();
        }
    }

    public void HandleNetMsg_Visitor(object[] objs)
    {
       Debug.Log(LogHelper.NetHandle("HandleNetMsg_Visitor"));
        MB_Singleton<AppManager>.Get().Handle_RecLoginOrVisitorLogin();
       Debug.Log("游客登录了");
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
       Debug.LogError("是否登录: " + dictionary["isLogin"] + "   " + JsonMapper.ToJson(objs[0]));
        if ((bool)dictionary["isLogin"])
        {
           Debug.Log("游客登录了");
            base.gameObject.SetActive(value: false);
            ZH2_GVars.user = User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
            ZH2_GVars.payMode = (int)dictionary["payMode"];
            m_inputPwd.text = string.Empty;
            MB_Singleton<AppManager>.GetInstance().ShowMainPanel();
            MB_Singleton<AppManager>.Get().Notify(UIGameMsgType.UINotify_Allow_LoadingFinish);
            ShowAnnouce();
            object obj = dictionary["gameRunStatus"];
           Debug.LogError("args: " + obj);
            InnerGameManager.Get().UpdateGameRunStatus(obj);
            ZH2_GVars.ownedProps.Clear();
            for (int i = 9; i < 12; i++)
            {
                if (ZH2_GVars.ownedProps.ContainsKey(i))
                {
                    MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_FreshDealer, i - 8);
                    break;
                }
                if (i == 11)
                {
                    MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_FreshDealer, 0);
                }
            }
            MB_Singleton<NetManager>.GetInstance().isLogined = true;
            return;
        }
        int num = (int)dictionary["messageStatus"];
        int num2 = num;
        switch (num2 + 2)
        {
            case 0:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游客登录功能未开启", "Tourists landing function yet open", "คุณสมบัติการเข้าสู่ระบบของนักท่องเ ที่ยวยังไม่เปิด", "Chức năng đăng nhập khách không bật"));
                break;
            default:
                if (num2 != 16)
                {
                    MB_Singleton<AppManager>.Get().Send_Login(isTourist: true);
                }
                else
                {
                    MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("网络不稳定,请稍后重试", "Network instability", "ความไม่แน่นอนของเครือข่ายโปรดลองใหม่อีกครั้ง", "Mạng không ổn định, vui lòng thử lại sau"));
                }
                break;
            case 4:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
                break;
            case 5:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号或密码错误", "Wrong account or password", "หมายเลขบัญชี หรือรหัสผ่านไม่ถูกต้อง", "Lỗi tài khoản hoặc mật khẩu"));
                break;
            case 8:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号被冻结,请联系管理员", "This account has been frozen", "เลขบัญชีถูกระงับ", "Tài khoản bị đóng băng, vui lòng liên hệ với quản trị viên"));
                break;
            case 9:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("系统维护,敬请谅解", "Server maintenance", "การดูแลระบบ", "Bảo trì hệ thống, xin thông cảm"));
                break;
        }
        MB_Singleton<AppManager>.Get().Handle_LoginFailed();
    }

    public IEnumerator DirectLogin()
    {
       Debug.LogError("=====DirectLogin=====");
        yield return null;
        while (!MB_Singleton<NetManager>.Get().isReady)
        {
            yield return null;
        }
        bool isTourist = false;
        if (ZH2_GVars.pwd.Equals("123456") && ZH2_GVars.username.StartsWith(ZH2_GVars.language_enum.Equals("0") ? "游客" : "tourist"))
        {
            isTourist = true;
        }
        MB_Singleton<AppManager>.Get().Send_Login(isTourist);
    }
}
