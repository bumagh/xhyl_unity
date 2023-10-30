using LitJson;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AppManager : MB_Singleton<AppManager>
{
    public bool devScene;

    [SerializeField]
    private LoadingPanelController m_loadingPanel;

    [SerializeField]
    public LoginPanelController m_loginPanel;

    [SerializeField]
    public MainPanelController m_mainPanel;

    [SerializeField]
    private ZH2_GVars.LanguageType m_isEnglish;

    private ZH2_GVars.LanguageType m_lastIsEnglish;

    private Transform m_xformCanvas;

    public bool localIP;

    [Header("是否点击桌子进入游戏")]
    public bool isClickTableEnterGame;

    private bool m_firstAwake = true;

    public Font zh_font_title;

    public Font zh_font;

    public Font en_font_title;

    public Font en_font;

    public Font th_font_title;

    public Font th_font;

    public Font vn_font_title;
    public Font vn_font;


    public static bool initNet;

    public bool isShowAnnouce;

    private bool netDownHintLocker;

    private bool _hasInited;

    public bool hasLogined;

    public bool isConnecting;

    private bool _isReconnecting;

    private bool m_hasVersionChecked;

    private Dictionary<UIGameMsgType, HashSet<NotifyObject>> m_notifys = new Dictionary<UIGameMsgType, HashSet<NotifyObject>>();

    private Coroutine _coLogin;

    private static InnerGame m_curInnerGame;

    private bool m_versionOK;

    private int tempNum;

    private Coroutine waitCoroutine;

    private Coroutine loadingLoginTimeout;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name.Contains("MainScene") && Screen.orientation != ScreenOrientation.LandscapeLeft)
        {
            ScreenOrientation orientation = Screen.orientation;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        if (m_firstAwake)
        {
            MB_Singleton<AppManager>.SetInstance(this);
            if (devScene)
            {
                DataFaker.Init();
            }
        }
        m_firstAwake = false;
        Screen.sleepTimeout = -1;
    }

    public void ForbidNetDownHint()
    {
        Debug.Log("ForbidNetDownHint");
        netDownHintLocker = true;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (_hasInited)
        {
            Debug.LogError("====AppManager已经初始化过了====");
            return;
        }
        Debug.LogError("====AppManager开始初始化====");
        _hasInited = true;
        ZH2_GVars.isConnect = false;
        if (!ZH2_GVars.isStartedFromGame)
        {
            if (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified || Application.systemLanguage == SystemLanguage.ChineseTraditional)
            {
                ZH2_GVars.language_enum = ZH2_GVars.LanguageType.Chinese;
            }
            else if (Application.systemLanguage == SystemLanguage.Thai)
            {
                ZH2_GVars.language_enum = ZH2_GVars.LanguageType.Thai;
            }
            else if (Application.systemLanguage == SystemLanguage.Vietnamese)
            {
                ZH2_GVars.language_enum = ZH2_GVars.LanguageType.Vietnam;
            }
            else
            {
                ZH2_GVars.language_enum = ZH2_GVars.LanguageType.English;
            }
        }
        RefreshLocalization();
        if (initNet)
        {
            InitNet(switchUser: true);
        }
        else
        {
            Debug.LogError("=====初始化失败====");
        }
        _basicInit();
        if (!devScene)
        {
            _initPanels();
        }
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("netDown", Handle_NetDown);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("reLogin", HandleNetMsg_ReLogin);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("international", HandleNetMsg_International);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("checkVersion", HandleNetMsg_CheckVersion);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("quitToLogin", m_mainPanel.HandleNetMsg_QuitToLogin);
        m_loginPanel.Init();
    }

    public void ShowNotice()
    {
        m_loginPanel.ShowAnnouce();
    }

    public void ShowProperSecurity()
    {
        m_mainPanel.OnBtnClick_ProperSecurity();
    }

    private void OnValidate()
    {
        if (m_lastIsEnglish != m_isEnglish)
        {
            ZH2_GVars.language_enum = m_isEnglish;
            RefreshLocalization();
        }
        m_lastIsEnglish = m_isEnglish;
    }

    public void RefreshLocalization()
    {
    }

    private IEnumerator _connectAndLoginCoroutine(bool willLogin = false)
    {
        Debug.LogError("=====执行=====" + willLogin);
        m_hasVersionChecked = false;
        isConnecting = true;
        if (!hasLogined)
        {
            MB_Singleton<NetManager>.GetInstance().connectMaxTimes = 20;
            MB_Singleton<NetManager>.GetInstance().connectMaxTimeout = 20f;
            Debug.LogError("设置最大连接次数 最大超时时间");
        }
        if (!_isReconnecting)
        {
            Debug.LogError("初始化链接");
            InitData();
        }
        else
        {
            Debug.LogError("====打断链接====");
            MB_Singleton<NetManager>.GetInstance().Disconnect();
            yield return new WaitForSeconds(0.5f);
        }
        Debug.LogError("IP: " + ZH2_GVars.IPAddress);
        MB_Singleton<NetManager>.Get().autoReady = true;
        MB_Singleton<NetManager>.GetInstance().Connect(ZH2_GVars.IPAddress, ZH2_GVars.IPPort);
        Debug.Log("等待连接...");
        while (!MB_Singleton<NetManager>.GetInstance().isConnected)
        {
            yield return null;
        }
        while (!MB_Singleton<NetManager>.GetInstance().isReady)
        {
            yield return null;
        }
        if (!willLogin)
        {
            ReconnectSuccess();
            isConnecting = false;
            yield break;
        }
        yield return new WaitForSeconds(0.2f);
        if (hasLogined)
        {
            Debug.LogError("已经登陆过 重登陆");
            Send_ReLogin();
            yield return null;
        }
        else
        {
            Debug.LogError("====第一次登陆====");
            Send_Login();
            yield return null;
        }
        if (!_isReconnecting)
        {
            if (loadingLoginTimeout != null)
            {
                StopCoroutine(loadingLoginTimeout);
            }
            loadingLoginTimeout = StartCoroutine(_loadingLoginTimeout());
        }
        isConnecting = false;
    }

    public void Send_Login(bool isTourist = false)
    {
        if (!Can_Send())
        {
            Debug.LogError("======不可发送,被返回了====");
            return;
        }
        ZH2_GVars.lockSend = true;
        ZH2_GVars.isTourist = isTourist;
        Debug.LogError("=======开始发送登录=====");
        MB_Singleton<NetManager>.Get().Send("gcuserService/login", new object[]
        {
            ZH2_GVars.username,
            ZH2_GVars.pwd,
            (int)ZH2_GVars.language_enum
        });
        if (!_isReconnecting)
        {
            if (loadingLoginTimeout != null)
            {
                StopCoroutine(loadingLoginTimeout);
            }
            loadingLoginTimeout = StartCoroutine(_loadingLoginTimeout());
        }
    }

    public void Send_ReLogin()
    {
        MB_Singleton<NetManager>.Get().Send("gcuserService/reLogin", new object[]
        {
            ZH2_GVars.username,
            ZH2_GVars.pwd,
            (int)ZH2_GVars.language_enum
        });
    }

    private void HandleNetMsg_ReLogin(object[] objs)
    {
        Debug.Log(LogHelper.NetHandle("HandleNetMsg_ReLogin"));
        ZH2_GVars.lockSend = false;
        ZH2_GVars.lockRelogin = false;
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        bool flag = false;
        try
        {
            flag = (bool)dictionary["reLogin"];
        }
        catch (Exception message)
        {
            Debug.LogWarning(message);
        }
        try
        {
            flag = (bool)dictionary["isLogin"];
        }
        catch (Exception message2)
        {
            Debug.LogWarning(message2);
        }
        if (MB_Singleton<ReconnectHint>.GetInstance() != null)
        {
            MB_Singleton<ReconnectHint>.GetInstance().Hide();
        }
        if (flag)
        {
            ZH2_GVars.user = User.CreateWithDic(dictionary["user"] as Dictionary<string, object>);
            MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);
            MB_Singleton<AppManager>.Get().Notify(UIGameMsgType.UINotify_Fresh_UserInfo);
            ZH2_GVars.lockReconnect = false;
            ZH2_GVars.lockQuit = false;
            netDownHintLocker = false;
            Debug.Log(LogHelper.Magenta("relogin success"));
        }
        else
        {
            ZH2_GVars.lockReconnect = true;
            ZH2_GVars.lockQuit = true;
            Debug.Log(LogHelper.Magenta("relogin failure"));
            try
            {
                if (m_xformCanvas.Find("RedBonusRain").gameObject.activeSelf)
                {
                    RedBonusControl._redBonusControl.MaintenanceRedBonusClose();
                }
            }
            catch (Exception message3)
            {
                Debug.LogWarning(message3);
            }
            MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("断线重连失败", "Reconnection failure", "การเชื่อมต่อล้มเหลว", "Mất liên lạc lại thất bại"), showOkCancel: false, delegate
            {
                SceneManager.LoadSceneAsync(0);
            });
        }
    }

    public void Handle_RecLoginOrVisitorLogin()
    {
        hasLogined = true;
        ZH2_GVars.lockSend = false;
        _isReconnecting = false;
        ZH2_GVars.lockRelogin = false;
        MB_Singleton<NetManager>.GetInstance().connectMaxTimes = 50;
        MB_Singleton<NetManager>.GetInstance().connectMaxTimeout = 30f;
        MB_Singleton<NetManager>.GetInstance().connectMaxTimes = 30;
        MB_Singleton<NetManager>.GetInstance().connectMaxTimeout = 20f;
        StopCoLogin();
        if (MB_Singleton<ReconnectHint>.GetInstance() != null)
        {
            MB_Singleton<ReconnectHint>.GetInstance().Hide();
        }
    }

    private void StopCoLogin()
    {
        if (_coLogin != null)
        {
            StopCoroutine(_coLogin);
            _coLogin = null;
            isConnecting = false;
        }
    }

    public void Handle_LoginFailed()
    {
        Debug.LogError("====游戏登录失败===");
        m_loginPanel.gameObject.SetActive(value: true);
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
        }
        waitCoroutine = StartCoroutine(Wait(1.5f));
        hasLogined = false;
    }

    public void ChangeAccount()
    {
        Debug.LogError("====切换账号====");
        InitNet(switchUser: true);
        m_loginPanel.gameObject.SetActive(value: true);
        hasLogined = false;
    }

    public bool Can_Send()
    {
        return !ZH2_GVars.lockSend && MB_Singleton<NetManager>.Get().isReady;
    }

    private void InitData()
    {
        ZH2_GVars.isClickTableEnterGame = isClickTableEnterGame;
        if (localIP)
        {
            ZH2_GVars.IPAddress = "192.168.3.143";
            ZH2_GVars.IPAddress_Game = "192.168.3.143";
            ZH2_GVars.IPPort = 8877;
            ZH2_GVars.payPort3 = 8080;
        }
        else
        {
            ZH2_GVars.IPAddress = "inx.fnf0.cn";//IP修改

            ZH2_GVars.IPAddress_Game = "inx.fnf0.cn";
            ZH2_GVars.IPPort = 8877;
            ZH2_GVars.payPort3 = 8081;
        }
        ZH2_GVars.shortConnection = string.Format("http://{0}:{1}/game-1.0.1/user/", ZH2_GVars.IPAddress, ZH2_GVars.payPort3);

    }

    public void InitNet(bool switchUser = false)
    {
        try
        {
            netDownHintLocker = switchUser;
            InitData();
            MB_Singleton<NetManager>.GetInstance().Connect(ZH2_GVars.IPAddress, ZH2_GVars.IPPort);
        }
        catch (Exception arg)
        {
            Debug.LogError("====" + arg + "====");
        }
        _coLogin = StartCoroutine(CheckStartupParams());
    }

    private void _initPanels()
    {
        m_loadingPanel.gameObject.SetActive(value: true);
        m_loginPanel.gameObject.SetActive(value: false);
        m_mainPanel.gameObject.SetActive(value: false);
        if (MB_Singleton<AlertDialog>.GetInstance() != null)
        {
            MB_Singleton<AlertDialog>.GetInstance().gameObject.SetActive(value: false);
        }
    }


    public void SendTransAll(bool bo)
    {
        StartCoroutine(SendTransAllGame("http://" + ZH2_GVars.IPAddress + ":8091/live/api/userTransAll", bo));
    }


    public IEnumerator SendTransAllGame(string url, bool bo)
    {
        Debug.LogError("Url: " + url);
        WWWForm wwwform = new WWWForm();
        wwwform.AddField("username", ZH2_GVars.AccountName);
        UnityWebRequest request = UnityWebRequest.Post(url, wwwform);
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            Debug.Log("访问外连接：" + request.error);
            yield break;
        }
        if (!string.IsNullOrEmpty(request.error))
        {
            Debug.Log(request.error);
            yield break;
        }
        JsonData jsonData = JsonMapper.ToObject(request.downloadHandler.text);
        MonoBehaviour.print(jsonData.ToJson());

        MonoBehaviour.print(jsonData["msg"].ToString());
        if ((int)jsonData["code"] == 10000)
        {
            try
            {
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("转出额度:", "Transfer out limit:", "วงเงินโอนออก:", "Hạn mức chuyển:") + jsonData["data"]["balanceAll"].ToString());
                ZH2_GVars.user.gameGold += int.Parse(jsonData["data"]["balanceAll"].ToString());
            }
            catch
            {
                print("暂无平台额度转出");
            }
        }
        else
        {
            if (bo)
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(jsonData["msg"].ToString());
        }
    }

    public void HideAllPanels()
    {
        string[] array = new string[25]
        {
            "LoadingPanel",
            "MainPanel",
            "AlertDialog",
            "CashPanel",
            "CashOperatePanel",
            "LotteryCityPanel",
            "RegisterPanel",
            "UserSettingsPanel",
            "SafeBoxPanel",
            "SafeBoxPwdSetPanel",
            "SafeBoxPwdCheckPanel",
            "FakePanel",
            "StorePanel",
            "RankPanel",
            "RechargePanel",
            "AwardPanel",
            "UserCompletePanel",
            "RecoverPasswordPanel",
            "CampaignPanel",
            "SettingPanel",
            "AnnouncementPanel",
            "SharePanel",
            "MailPanel",
            "NoticeTip",
            "RedBonusRain"
        };
        string[] array2 = array;
        foreach (string n in array2)
        {
            Transform transform = m_xformCanvas.Find(n);
            if (transform != null)
            {
                transform.gameObject.SetActive(value: false);
            }
        }
    }

    private void _basicInit()
    {
        m_xformCanvas = ((GameObject.Find("Canvas/DTRoot") != null) ? GameObject.Find("Canvas/DTRoot").transform : GameObject.Find("Canvas").transform);
        FindAllObjectsInScene.list.Clear();
        FindAllObjectsInScene.InitAllTL(m_xformCanvas);
        //FindAllObjectsInScene.RefreshAllTxt();
        MB_Singleton<AlertDialog>.SetInstanceByGameObject(m_xformCanvas.Find("AlertDialog").gameObject).PreInit();
        GameObject gameObject = m_xformCanvas.Find("NoticeTip").gameObject;
        gameObject.SetActive(value: false);
        gameObject.GetComponent<NoticeTipController>().PreInit();
        MB_Singleton<ReconnectHint>.SetInstanceByGameObject(m_xformCanvas.Find("ReconnectHint").gameObject).PreInit();
        if (MB_Singleton<ReconnectHint>.GetInstance() != null)
        {
            MB_Singleton<ReconnectHint>.GetInstance().Hide();
        }
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("scrollMessage", gameObject.GetComponent<NoticeTipController>().HandleNetMsg_Notice);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("timeout", delegate (object[] args)
        {
            Debug.Log(LogHelper.Magenta("timeout>  id: " + (int)args[0]));
        });
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("heart timeout", delegate
        {
            Debug.Log(LogHelper.Magenta("heart timeout"));
            Handle_NetDown(new object[1]
            {
                new Exception("heart timeout")
            });
        });
    }

    private void Update()
    {
        MB_Singleton<NetManager>.GetInstance().connectTimeCount += Time.deltaTime;
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            QuitApp();
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GameObject gameObject = m_xformCanvas.Find("NoticeTip").gameObject;
            NoticeTipController component = gameObject.GetComponent<NoticeTipController>();
            tempNum++;
            string arg = "这是测试消息_";
            arg += tempNum;
            component.AddMessage(arg);
        }
    }

    public void ShowLoginPanel(float time)
    {
        Debug.LogError("====ShowLoginPanel====");
        SetTransformScale(m_loginPanel.gameObject);
        m_loginPanel.gameObject.SetActive(value: true);
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
        }
        waitCoroutine = StartCoroutine(Wait(time));
    }

    public void ClosePanel()
    {
        if ((bool)m_loginPanel && m_loginPanel.gameObject.activeInHierarchy)
        {
            m_loginPanel.gameObject.SetActive(value: false);
        }
        if ((bool)m_loadingPanel && m_loadingPanel.gameObject.activeInHierarchy)
        {
            m_loadingPanel.gameObject.SetActive(value: false);
        }
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.LogError("==========关闭加载页===========");
        m_loadingPanel.gameObject.SetActive(value: false);
    }

    private void SetTransformScale(GameObject @object)
    {
        if (ZH2_GVars.isStartedFromGame)
        {
            Debug.LogError("从游戏返回 不显示登录页面");
            @object.transform.localScale = Vector3.zero;
        }
        else
        {
            base.transform.localScale = Vector3.one;
        }
    }

    public void ShowMainPanel()
    {
        m_mainPanel.gameObject.SetActive(value: true);
        m_mainPanel.Init();
    }

    public void Register(UIGameMsgType type, UnityEngine.Object obj, NotifyCallBack callBack, int times = 0)
    {
        if (!m_notifys.ContainsKey(type))
        {
            m_notifys[type] = new HashSet<NotifyObject>();
        }
        if (times > 0)
        {
            HashSet<NotifyObject> hashSet = m_notifys[type];
            if (hashSet.Count >= times)
            {
                return;
            }
        }
        NotifyObject item = default(NotifyObject);
        item.Instance = obj;
        item.CallBack = callBack;
        m_notifys[type].Add(item);
    }

    public void Notify(UIGameMsgType Type, object obj = null)
    {
        ClearInvalidNOtify();
        if (m_notifys.ContainsKey(Type))
        {
            HashSet<NotifyObject> hashSet = m_notifys[Type];
            foreach (NotifyObject item in hashSet)
            {
                NotifyObject current = item;
                if (null != current.Instance)
                {
                    current.CallBack(obj);
                }
            }
        }
    }

    private void ClearInvalidNOtify()
    {
        foreach (KeyValuePair<UIGameMsgType, HashSet<NotifyObject>> notify in m_notifys)
        {
            HashSet<NotifyObject> value = notify.Value;
            HashSet<NotifyObject> hashSet = null;
            foreach (NotifyObject item in value)
            {
                NotifyObject current = item;
                if (null == current.Instance)
                {
                    if (hashSet == null)
                    {
                        hashSet = new HashSet<NotifyObject>();
                    }
                    hashSet.Add(current);
                }
            }
            if (hashSet != null)
            {
                value.ExceptWith(hashSet);
            }
        }
    }

    public GameObject GetPanel(string panelName)
    {
        if (m_xformCanvas == null)
        {
            m_xformCanvas = ((GameObject.Find("Canvas/Root") != null) ? GameObject.Find("Canvas/Root").transform : GameObject.Find("Canvas").transform);
        }
        return m_xformCanvas.Find(panelName).gameObject;
    }

    public void QuitApp()
    {
        MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("是否退出大厅?", "Quit the gaming center?", "ออกจากห้องโถง หรือไม่?", "Ra khỏi sảnh hay không?"), showOkCancel: true, delegate
        {
            MB_Singleton<NetManager>.Get().Disconnect();
            Application.Quit();
        });
    }

    public void Handle_NetDown(object[] args)
    {
        isConnecting = false;
        if (MB_Singleton<NetManager>.GetInstance().isReady)
        {
            if (MB_Singleton<ReconnectHint>.GetInstance() != null)
            {
                MB_Singleton<ReconnectHint>.GetInstance().Hide();
            }
            Debug.LogError("======isReady 这儿返回了");
            return;
        }
        if (ZH2_GVars.lockQuit || ZH2_GVars.lockReconnect)
        {
            Debug.LogError("=======locked 这返回了");
            return;
        }
        _isReconnecting = true;
        Exception ex = null;
        if (args != null)
        {
            ex = (args[0] as Exception);
        }
        bool isEditor = Application.isEditor;
        StopCoLogin();
        if (MB_Singleton<NetManager>.GetInstance().connectCount > MB_Singleton<NetManager>.GetInstance().connectMaxTimes && MB_Singleton<NetManager>.GetInstance().connectTimeCount > MB_Singleton<NetManager>.GetInstance().connectMaxTimeout)
        {
            _timeoutQuit();
            return;
        }
        bool flag = !ZH2_GVars.lockRelogin && (hasLogined || ZH2_GVars.isStartedFromGame || ZH2_GVars.isStartedFromLuaGame);
        Debug.LogError("====是否能登录2: " + flag);
        if (!flag)
        {
            Debug.LogError(ZH2_GVars.lockRelogin + "--" + hasLogined + "--" + ZH2_GVars.isStartedFromGame + "--" + ZH2_GVars.isStartedFromLuaGame);
        }
        _coLogin = StartCoroutine(_connectAndLoginCoroutine(flag));
        if (MB_Singleton<ReconnectHint>.GetInstance() != null && !netDownHintLocker)
        {
            MB_Singleton<ReconnectHint>.GetInstance().Show();
        }
    }

    public bool CheckStartConnectAndLogin(bool willLogin = false)
    {
        bool result = false;
        ZH2_GVars.lockQuit = false;
        ZH2_GVars.lockReconnect = false;
        ZH2_GVars.lockSend = false;
        if (!MB_Singleton<NetManager>.Get().isReady)
        {
            if (!isConnecting)
            {
                result = true;
                bool flag = !ZH2_GVars.lockRelogin && willLogin;
                Debug.LogError("===是否能登录: " + flag);
                if (!flag)
                {
                    Debug.LogError(ZH2_GVars.lockRelogin + "--" + willLogin);
                }
                _coLogin = StartCoroutine(_connectAndLoginCoroutine(flag));
            }
            return result;
        }
        return result;
    }

    private void ReconnectSuccess()
    {
        Debug.Log("ReconnectSuccess");
        if (MB_Singleton<ReconnectHint>.GetInstance() != null)
        {
            MB_Singleton<ReconnectHint>.GetInstance().Hide();
        }
        _isReconnecting = false;
        netDownHintLocker = false;
        ZH2_GVars.lockSend = false;
        StopCoLogin();
    }

    private void _timeoutQuit()
    {
        Debug.Log($"连接次数: {MB_Singleton<NetManager>.GetInstance().connectCount}, 连接时间: {MB_Singleton<NetManager>.GetInstance().connectTimeCount}");
        Debug.Log($"最大连接时间: {MB_Singleton<NetManager>.GetInstance().connectMaxTimes}, 最大超时时间: {MB_Singleton<NetManager>.GetInstance().connectMaxTimeout}");
        string content = ZH2_GVars.ShowTip("网络连接错误，请检查网络连接", "Unable to connect to server，Please check your network connection", "การเชื่อมต่อเครือข่ายผิดพลาดโปรดตรวจสอบการเชื่อมต่อเครือข่าย", "Lỗi kết nối mạng, vui lòng kiểm tra kết nối mạng");
        if (MB_Singleton<ReconnectHint>.GetInstance() != null)
        {
            MB_Singleton<ReconnectHint>.GetInstance().Hide();
        }
        ZH2_GVars.lockQuit = true;
        ZH2_GVars.lockReconnect = true;
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
        {
            initNet = false;
            QuitToLogin();
            ZH2_GVars.lockQuit = false;
            ZH2_GVars.lockReconnect = false;
        });
    }

    private IEnumerator _loadingLoginTimeout()
    {
        yield return new WaitForSeconds(9f);
        if (!hasLogined && !MB_Singleton<NetManager>.GetInstance().useFake)
        {
            Debug.Log(LogHelper.Magenta("_loadingLoginTimeout"));
            ZH2_GVars.lockSend = false;
            Handle_NetDown(new object[1]
            {
                new Exception("loading timeout")
            });
        }
    }

    public void LoadGameHall()
    {
        SceneManager.LoadScene("GameHall");
    }

    public InnerGame InnerGame()
    {
        return m_curInnerGame;
    }

    public void OnEnterLuaGame()
    {
        MB_Singleton<NetManager>.Get().Disconnect();
    }

    public IEnumerator CheckStartupParams()
    {
        MB_Singleton<NetManager>.Get().autoReady = false;
        yield return 2;
        if (!hasLogined)
        {
            if (ZH2_GVars.isStartedFromGame)
            {
                yield return new WaitForSeconds(0.5f);
                if (!hasLogined && !ZH2_GVars.isConnect)
                {
                    Debug.LogError("======isStartedFromGame======");
                    InitNet(switchUser: true);
                }
                else
                {
                    Debug.LogError("=======已经连接成功======");
                }
            }
            int language_enum = (int)ZH2_GVars.language_enum;
            I2Localization.SetCurLanguage(language_enum.ToString());
            RefreshLocalization();
            //FindAllObjectsInScene.RefreshAllTxt();
        }
        while (!MB_Singleton<NetManager>.Get().isConnected)
        {
            yield return null;
        }
        Send_CheckVersion();
        while (!m_versionOK)
        {
            yield return null;
        }
        MB_Singleton<NetManager>.Get().autoReady = true;
        MB_Singleton<NetManager>.Get().SendPublicKey();
        if (!ZH2_GVars.lockRelogin && (ZH2_GVars.isStartedFromGame || ZH2_GVars.isStartedFromLuaGame))
        {
            StartCoroutine(m_loginPanel.DirectLogin());
        }
    }

    public void Send_CheckVersion()
    {
        string version = Application.version;
        ZH2_GVars.isGetPublicKey = false;
        MB_Singleton<NetManager>.Get().Send("gcuserService/checkVersion", new object[2]
        {
            version,
            0
        });
    }

    private void HandleNetMsg_International(object[] objs)
    {
        Debug.Log(LogHelper.NetHandle("HandleNetMsg_International"));
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        bool isInternational = (bool)dictionary["international"];
        ZH2_GVars.isInternational = isInternational;
    }

    private void HandleNetMsg_CheckVersion(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        bool flag = false;
        string empty = string.Empty;
        empty = ((Application.platform == RuntimePlatform.Android) ? dictionary["androidVersion"].ToString() : ((Application.platform != RuntimePlatform.IPhonePlayer) ? dictionary["androidVersion"].ToString() : dictionary["iOSVersion"].ToString()));
        flag = (empty == ZH2_GVars.downloadVersion);
        Debug.LogError(LogHelper.NetHandle("===检查版本===" + (flag ? "不需要更新" : "需要更新")));
        if (Application.platform == RuntimePlatform.Android)
        {
            ZH2_GVars.downloadStr = (string)dictionary["downloadAndroid"];
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            ZH2_GVars.downloadStr = (string)dictionary["downloadWindows"];
        }
        else
        {
            ZH2_GVars.downloadStr = (string)dictionary["downloadAndroid"];
        }
        ZH2_GVars.isGetPublicKey = true;
        if (ZH2_GVars.sendLogin != null)
        {
            ZH2_GVars.sendLogin();
        }
        if (!flag)
        {
            ZH2_GVars.lockQuit = true;
            ZH2_GVars.lockReconnect = true;
            ZH2_GVars.lockSend = true;
            string urlAndroid = (string)dictionary["downloadAndroid"];
            string content = ZH2_GVars.ShowTip("为保证您的游戏体验\n请使用最新版本\n否则将无法正常游戏\n祝游戏愉快", "Note: to ensure that your user experience, the client must use the latest version, otherwise unable to run the game", "หมายเหตุ: เพื่อยืนยันประสบการณ์ของผู้ใช้ของคุณโปรแกรมลูกค้าต้องใช้รุ่นล่าสุด หรือไม่ก็ไม่สามารถเล่นได้อย่างถูกต้อง", "Để đảm bảo trải nghiệm chơi game của bạn\nVui lòng sử dụng phiên bản mới nhất\nNếu không sẽ không chơi bình thường.\nGame vui vẻ");
            MB_Singleton<AlertDialog>.Get().ShowDialog(content, showOkCancel: false, delegate
            {
                Application.OpenURL(urlAndroid);
                Application.Quit();
            });
        }
        else
        {
            m_versionOK = true;
            if (dictionary.ContainsKey("downloadWindows"))
            {
                ZH2_GVars.downloadStr = (string)dictionary["downloadAndroid"];
            }
        }
    }

    public void QuitToLogin()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private string DoGetHostAddresses(string hostname)
    {
        if (IPAddress.TryParse(hostname, out IPAddress address))
        {
            return address.ToString();
        }
        return Dns.GetHostEntry(hostname).AddressList[0].ToString();
    }

    private void OnDestroy()
    {
        ZH2_GVars.isStartedFromLuaGame = false;
        ZH2_GVars.isStartedFromGame = false;
        ZH2_GVars.lockQuit = false;
        ZH2_GVars.lockReconnect = false;
        ZH2_GVars.lockSend = false;
        PlayerPrefs.Save();
        MB_Singleton<AppManager>._instance = null;
    }

    static AppManager()
    {
        initNet = true;
    }
}
