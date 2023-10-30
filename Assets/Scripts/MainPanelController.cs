using DG.Tweening;
using JsonFx.Json;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanelController : MonoBehaviour
{
    [SerializeField]
    private Text m_textGold;

    [SerializeField]
    private Text m_textExpeGold;

    [SerializeField]
    private Text m_textUserNickName;

    [SerializeField]
    private Text m_textLevel;

    [SerializeField]
    private Text m_textLevelName;

    [SerializeField]
    private Image m_imageUserIcon;

    [SerializeField]
    private Text m_ID;

    public Button btnService;

    [SerializeField]
    public UserIconDataConfig m_userIconDataConfig;

    private int layerOrder = 15;

    private bool isMorePanelShow;

    private string temp = string.Empty;

    private AndroidJavaObject jo;

    private AndroidJavaClass jc;

    private Coroutine waitUpCoin;

    private string serviceUrl = string.Empty;

    private string ServiceUrlJump = string.Empty;

    private string ServiceUrlJump2 = string.Empty;

    public Image ImageUserIcon => m_imageUserIcon;

    public Text ID => m_ID;

    private void Awake()
    {
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Open_CashPanel, this, OpenCashPanel);
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Open_RechargePanel, this, OpenRechargePanel);
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Open_StorePanel, this, OpenStorePanel);
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Close_StorePanel, this, CloseStorePanel);
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Open_LotteryCityPanel, this, OpenLotteryCityPanel);
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Close_LotteryCityPanel, this, CloseLotteryCityPanel);
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Open_SafeBoxPwdCheckPanel, this, OpenSafeBoxPwdCheckPanel);
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Open_SafeBoxPanel, this, OpenSafeBoxPanel);
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Fresh_GoldAndLottery, this, FreshGoldAndLottery);
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Open_UserCompletePanel, this, OpenUserCompletePanel);
        MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_Fresh_UserInfo, this, delegate
        {
            FreshUserInfo();
        });
        PlayerPrefs.SetString("isCanjump", "可以跳");
        PlayerPrefs.Save();
        if (Application.platform == RuntimePlatform.Android)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }

    private void Start()
    {
        if (ZH2_GVars.isStartedFromGame)
        {
            MB_Singleton<SoundManager>.Get().PlaySound(SoundType.BG_First);
        }
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("chargeNotice", HandleNetMsg_ChargeNotice);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("applyExchange", HandleNetMsg_ApplyExchange);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("applyPay", HandleNetMsg_ApplyPay);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("exChargeGiveUp", HandleNetMsg_exchargeFalseNotice);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("rechargeGiveUp", HandleNetMsg_rechargeFalseNotice);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("newGameGold", HandleNetMsg_newGameGoldNotice);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("changeScore", HandleNetMsg_changeScoreNotice);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("newLottery", HandleNetMsg_newLotteryNotice);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("userAward", HandleNetMsg_userAwardNotice);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("unOverflow", HandleNetMsg_userUnOverflow);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("overflow", HandleNetMsg_userOverflow);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("needSignIn", HandleNetMsg_SignIn);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("signIn", HandleNetMsg_SignIn2);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("getExpeGold", HandleNetMsg_AddExpeGold);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("cancelWaitingExpiryUser", HandleNetMsg_CancelWaitingExpiryUser);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("getActivityInfo", HandleNetMsg_GetActivityInfo);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("updateActivityInfo", HandleNetMsg_UpdateActivityInfo);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("newUserLevel", HandleNetMsg_NewUserLevel);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("newExpeGold", HandleNetMsg_NewExpeGold);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("newFinishActivity", HandleNetMsg_NewFinishActivity);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("newUserActivity", HandleNetMsg_NewUserActivity);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("lastGameScore", HandleNetMsg_LastGameScore);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("replyLastGameScore", HandleNetMsg_ReplyLastGameScore);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("modifySafeBoxPwd", HandleNetMsg_ModifySafeBoxPwd);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("updateRunStatus", HandleNetMsg_UpdateRunStatus);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("giftMode", HandleNetMsg_UpdategiftMode);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("getCustomerServiceUrl", HandleNetMsg_getCustomerServiceUrl);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("giftProp", HandleNetMsg_UpdategiftProp);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("shareNotice", HandleNetMsg_UpdateshareNotice);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("shareMode", HandleNetMsg_UpdateshareMode);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("shareBindingNotice", HandleNetMsg_ShareBindingNotice);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("shareClearingNotice", HandleNetMsg_ShareClearingNotice);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("getGameGold", HandleNetMsg_getGameGold);
        MB_Singleton<NetManager>.GetInstance().RegisterHandler("manageService/sendNotice", HandleNetMsg_sendNotice);
        Init();
        if (btnService != null)
        {
            btnService.onClick.AddListener(OnBtnClick_CustomerService);
        }
    }

    private void OnEnable()
    {
        ZH2_GVars.isCanSenEnterGame = false;
       // FindAllObjectsInScene.RefreshAllTxt();
        FreshPayModeRelationIcons();
        temp = string.Empty;
        ServiceUrlJump2 = string.Empty;
        ZH2_GVars.antiInitNum = 0;
        try
        {
            MB_Singleton<AppManager>.GetInstance().ClosePanel();
        }
        catch (Exception message)
        {
            Debug.LogError(message);
        }
        if (waitUpCoin != null)
        {
            StopCoroutine(waitUpCoin);
        }
        waitUpCoin = StartCoroutine(WaitUpCoin());
    }

    public void ClickBtnCopy()
    {
        ClickBtnCopy(ID);
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

    private void Update()
    {
        if (ZH2_GVars.user != null && temp != Convert.ToString(ZH2_GVars.user.gameGold))
        {
            temp = Convert.ToString(ZH2_GVars.user.gameGold);
            m_textGold.text = Convert.ToString(ZH2_GVars.user.gameGold);
        }
    }

    public void Init()
    {
        m_textGold.text = Convert.ToString(ZH2_GVars.user.gameGold);
        m_textExpeGold.GetComponent<Text>().text = Convert.ToString(ZH2_GVars.user.expeGold);
        //FindAllObjectsInScene.RefreshAllTxt();
        layerOrder = 15;
        FreshUserInfo();
    }

    public void OnBtnClick_TestAlert()
    {
        Debug.Log("OnBtnClick_TestAlert");
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog("测试alertDialog");
    }

    public void OnBtnClick_Update(Transform up)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Debug.Log("===刷新金币===");
        up.DOLocalRotate(Vector3.forward * 360f * 2f, 2f, RotateMode.FastBeyond360);
        UpdateCoin();
    }

    private IEnumerator WaitUpCoin()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            UpdateCoin();
        }
    }

    private void UpdateCoin()
    {
        if (MB_Singleton<NetManager>.GetInstance() != null)
        {
            MB_Singleton<NetManager>.GetInstance().Send("gcuserService/getGameGold", new object[1]
            {
                ZH2_GVars.user.id
            });
        }
    }

    public void OnBtnClick_CustomerService()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.user.type == 1)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游客账号无法操作此功能", "The tourist account cannot operate this function", "หมายเลขบัญชีของนักท่องเที่ยวไม่สามารถใช้งานฟังก์ชั่นนี้ได้", "Tài khoản khách không thể hoạt động chức năng này"), showOkCancel: false, HideMorePanel);
            return;
        }
        MB_Singleton<NetManager>.GetInstance().Send("gcuserService/getCustomerServiceUrl", new object[0]);
        HideMorePanel();
    }

    private IEnumerator GetService(string serviceUrl)
    {
        Debug.LogError("===短链获取客服===" + serviceUrl);
        bool isJump = false;
        UnityWebRequest www = UnityWebRequest.Get(serviceUrl);
        www.timeout = 10000;
        yield return www.Send();
        if (www.error == null)
        {
            try
            {
                ServiceUrlJump = www.downloadHandler.text;
                ServiceUrlJump = $"{ServiceUrlJump}&userid={ZH2_GVars.user.id}&username={ZH2_GVars.user.username}";
                isJump = true;
                Debug.LogError("ServiceUrlJump: " + ServiceUrlJump);
                Application.OpenURL(ServiceUrlJump);
            }
            catch (Exception arg)
            {
                Debug.LogError("     " + arg);
                if (ServiceUrlJump2 != string.Empty && !isJump)
                {
                    Debug.LogError("===ServiceUrlJump2===" + ServiceUrlJump2);
                    Application.OpenURL(ServiceUrlJump2);
                }
                throw;
            }
            yield break;
        }
        Debug.LogError("===访问错误===: " + www.error);
        for (int i = 0; i < 3; i++)
        {
            if (ServiceUrlJump2 != string.Empty && !isJump)
            {
                isJump = true;
                Debug.LogError("===ServiceUrlJump2===" + ServiceUrlJump2);
                Application.OpenURL(ServiceUrlJump2);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void HideMorePanel()
    {
        if (isMorePanelShow)
        {
        }
    }

    public void OnBtnClick_SwitchLanguage()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Chinese)
        {
            ZH2_GVars.language_enum = ZH2_GVars.LanguageType.English;
        }
        else if (ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English)
        {
            ZH2_GVars.language_enum = ZH2_GVars.LanguageType.Thai;
        }
        else if (ZH2_GVars.language_enum == ZH2_GVars.LanguageType.Thai)
        {
            ZH2_GVars.language_enum = ZH2_GVars.LanguageType.Chinese;
        }
        SwitchLang(ZH2_GVars.language_enum);
    }

    private void SwitchLang(ZH2_GVars.LanguageType lang)
    {
        Debug.LogError("当前语言: " + ZH2_GVars.language_enum);
        PlayerPrefs.SetInt("GVarsLanguage", (int)ZH2_GVars.language_enum);
        ZH2_GVars.language_enum = lang;
        //FindAllObjectsInScene.RefreshAllTxt();
    }

    public void OnBtnClick_AddGold()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.user.type == 1)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游客账号无法操作此功能", "The tourist account cannot operate this function", "หมายเลขบัญชีของนักท่องเที่ยวไม่สามารถใช้งานฟังก์ชั่นนี้ได้", "Tài khoản khách không thể hoạt động chức năng này"));
        }
        else if (ZH2_GVars.payMode == 1)
        {
            _openPanel("StorePanel");
        }
        else
        {
            RechargeProcess();
        }
    }

    public void OnBtnClick_AddExpeGold()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        string content = ZH2_GVars.ShowTip("体验币低于10000可申请，每次申请补足10000币", "Tickets less than 10000 may apply for each application make up 10000 tickets", "เงินน้อยกว่า 10,000 สามารถสมัครได้", "Tiền trải nghiệm thấp hơn 10.000 có thể xin, mỗi lần xin bổ sung 10.000 tệ");

        MB_Singleton<AlertDialog>.Get().ShowDialog(content, showOkCancel: true, delegate
        {
            if (ZH2_GVars.expeGold < 10000)
            {
                MB_Singleton<NetManager>.Get().Send("gcuserService/getExpeGold", new object[0]);
            }
        });
    }

    public void OnBtnClick_Service()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Debug.LogError("阻止===");
    }

    public void OnBtnClick_Store()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.user.type == 1)
        {

            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游客账号无法操作此功能", "The tourist account cannot operate this function", "หมายเลขบัญชีของนักท่องเที่ยวไม่สามารถใช้งานฟังก์ชั่นนี้ได้", "Tài khoản khách không thể hoạt động chức năng này"));
        }
        else if (ZH2_GVars.payMode == 1)
        {
            _openPanel("StorePanel");
        }
        else
        {
            RechargeProcess();
        }
    }

    public void OnBtnClick_SafeBox()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Debug.LogError("=====打开保险柜====");
        if (ZH2_GVars.OpenCheckSafeBoxPwdPanel != null)
        {
            ZH2_GVars.OpenCheckSafeBoxPwdPanel(ZH2_GVars.GameType_DJ.hall);
        }
    }

    public void OpenSafePanel()
    {
        Debug.LogError("=====手动打开保险柜====");
        _openPanel("SafeBoxPanel");
    }

    public void OnBtnClick_LotteryCity()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.IsOpen("exchange"))
        {
            if (ZH2_GVars.user.type == 1)
            {
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游客账号无法操作此功能", "The tourist account cannot operate this function", "หมายเลขบัญชีของนักท่องเที่ยวไม่สามารถใช้งานฟังก์ชั่นนี้ได้", "Tài khoản khách không thể hoạt động chức năng này"));
            }
            else
            {
                CashProcess("cash");
            }
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("暂未开放", "Temporarily not opened", "ยังไม่เปิดให้บริการในขณะนี้", "Chưa mở"));
        }
    }

    public void OnBtnClick_Accounts()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.user.type == 1)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游客账号无法操作此功能", "The tourist account cannot operate this function", "หมายเลขบัญชีของนักท่องเที่ยวไม่สามารถใช้งานฟังก์ชั่นนี้ได้", "Tài khoản khách không thể hoạt động chức năng này"));
        }
        else
        {
            _openPanel("AccountsPanel");
        }
    }

    public void OnBtnClick_Mail()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.IsOpen("mail"))
        {
            Debug.Log("OnBtnClick_Mail");
            _openPanel("MailPanel");
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("暂未开放", "Temporarily not opened", "ยังไม่เปิดให้บริการในขณะนี้", "Chưa mở"));
        }
    }

    public void OnBtnClick_Rank()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Debug.Log("OnBtnClick_Rank");
        if (ZH2_GVars.IsOpen("rank"))
        {
            MB_Singleton<NetManager>.GetInstance().Send("gcuserService/getRankList", new object[2]
            {
                1,
                1
            });
            _openPanel("RankPanel");
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("暂未开放", "Temporarily not opened", "ยังไม่เปิดให้บริการในขณะนี้", "Chưa mở"));
        }
    }

    public void OnBtnClick_Campagin()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.IsOpen("activity"))
        {
            if (ZH2_GVars.user.type == 1)
            {
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游客账号无法操作此功能", "The tourist account cannot operate this function", "หมายเลขบัญชีของนักท่องเที่ยวไม่สามารถใช้งานฟังก์ชั่นนี้ได้", "Tài khoản khách không thể hoạt động chức năng này"));
                return;
            }
            Debug.Log("OnBtnClick_Campagin");
            _openPanel("CampaignPanel");
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("暂未开放", "Temporarily not opened", "ยังไม่เปิดให้บริการในขณะนี้", "Chưa mở"));
        }
    }

    private void HandleNetMsg_UpdateActivityInfo(object[] obj)
    {
    }

    private void HandleNetMsg_GetActivityInfo(object[] obj)
    {
        _openPanel("CampaignPanel");
    }

    public void OnBtnClick_Recharge()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.IsOpen("topUp"))
        {
            MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
            Debug.LogError("=====打开充值====");
            if (ZH2_GVars.OpenPlyBoxPanel != null)
            {
                ZH2_GVars.OpenPlyBoxPanel(ZH2_GVars.GameType_DJ.hall);
            }
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("暂未开放", "Temporarily not opened", "ยังไม่เปิดให้บริการในขณะนี้", "Chưa mở"));
        }
    }

    public void OnBtnClick_Award()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Debug.Log("OnBtnClick_Award");
        _openPanel("AwardPanel");
    }

    public void OnBtnClick_UserIcon()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        _openPanel("UserSettingsPanel");
        MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_UserInfoPanel, 0);
    }

    public void OnBtnClick_More()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Debug.Log("OnBtnClick_More");
    }

    public void OnBtnClick_Announcement()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Debug.Log("OnBtnClick_Announcement");
        if (ZH2_GVars.IsOpen("notice"))
        {
            MB_Singleton<NetManager>.GetInstance().Send("gcuserService/getNotice", new object[1]
            {
                0
            });
            _openPanel("AnnouncementPanel");
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("暂未开放", "Temporarily not opened", "ยังไม่เปิดให้บริการในขณะนี้", "Chưa mở"));
        }
    }

    public void OnBtnClick_ProperSecurity()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.user.type == 1)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游客账号无法操作此功能", "The tourist account cannot operate this function", "หมายเลขบัญชีของนักท่องเที่ยวไม่สามารถใช้งานฟังก์ชั่นนี้ได้", "Tài khoản khách không thể hoạt động chức năng này"));
            return;
        }
        Debug.Log("OnBtnClick_ProperSecurity");
        MB_Singleton<NetManager>.GetInstance().Send("gcuserService/getAccountProtectionStatus", new object[0]);
        _openPanel("CashPanel");
    }

    public void OnBtnClick_Setting()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.IsOpen("setUp"))
        {
            Debug.Log("OnBtnClick_Setting");
            _openPanel("SettingPanel");
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("暂未开放", "Temporarily not opened", "ยังไม่เปิดให้บริการในขณะนี้", "Chưa mở"));
        }
    }

    public void OnBtnClick_Share()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Debug.Log("OnBtnClick_Share");
        if (ZH2_GVars.IsOpen("setUp"))
        {
            _openPanel("SharePanel");
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("暂未开放", "Temporarily not opened", "ยังไม่เปิดให้บริการในขณะนี้", "Chưa mở"));
        }
    }

    public void OnBtnClick_Spread()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.user.type == 1)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游客账号无法操作此功能", "The tourist account cannot operate this function", "หมายเลขบัญชีของนักท่องเที่ยวไม่สามารถใช้งานฟังก์ชั่นนี้ได้", "Tài khoản khách không thể hoạt động chức năng này"));
            return;
        }
        Debug.Log("OnBtnClick_Spread");
        _openPanel("SpreadPanel");
    }

    public void OnBtnClick_Dealer()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Debug.Log("OnBtnClick_Dealer");
        MeshUICamera.Get().ClickDealer();
    }

    public void OnBtnClick_DeliverRedEnvelope()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Debug.Log("OnBtnClick_DeliverRedEnvelope");
        _openPanel("DeliverRedEnvelopePanel");
    }

    public void OnBtnClick_CharityHall()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Debug.Log("OnBtnClick_CharityHall");
        _openPanel("CharityHallPanel");
    }

    private void OpenCashPanel(object obj)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        _openPanel("CashPanel");
    }

    private void OpenRechargePanel(object obj)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        _openPanel("RechargePanel");
    }

    private void OpenStorePanel(object obj)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        int index = (int)obj;
        _openPanel("StorePanel");
        base.transform.parent.Find("StorePanel").GetComponent<StorePanelController>().FocusTabByIndex(index);
    }

    private void CloseStorePanel(object obj)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        Transform transform = base.transform.parent.Find("StorePanel");
        if (transform != null)
        {
            transform.gameObject.SetActive(value: false);
        }
    }

    private void OpenLotteryCityPanel(object obj)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        _openPanel("CashPanel");
    }

    private void CloseLotteryCityPanel(object obj)
    {
        Transform transform = base.transform.parent.Find("LotteryCityPanel");
        if (transform != null)
        {
            transform.gameObject.SetActive(value: false);
        }
    }

    private void OpenSafeBoxPwdCheckPanel(object obj)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        _openPanel("SafeBoxPwdCheckPanel");
    }

    private void OpenSafeBoxPanel(object obj)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (ZH2_GVars.OpenSafeBoxPwdPanel != null)
        {
            ZH2_GVars.OpenSafeBoxPwdPanel();
        }
    }

    private void OpenUserCompletePanel(object obj)
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        ArrayList fromArgs = (ArrayList)obj;
        _openPanel("UserCompletePanel");
        base.transform.parent.Find("UserCompletePanel/AlertBg/Toggle").GetComponent<Toggle>().isOn = false;
        base.transform.parent.Find("UserCompletePanel").GetComponent<UserCompleteController>().FromArgs = fromArgs;
    }

    private void _openPanel(string panelName)
    {
        Transform transform = base.transform.parent.Find(panelName);
        Tween_SlowAction tween_SlowAction = null;
        if (transform != null)
        {
            transform.gameObject.SetActive(value: true);
            transform.GetComponent<Tween_SlowAction>()?.Show();
        }
    }

    public void CoslePanel(string panelName)
    {
        Transform transform = base.transform.parent.Find(panelName);
        if (transform != null)
        {
            transform.gameObject.SetActive(value: false);
        }
    }

    public void OppenPanel(string panelName)
    {
        _openPanel(panelName);
    }

    public void RechargeProcess()
    {
        _openPanel("RechargePanel");
    }

    public void CashProcess(string from)
    {
        if (ZH2_GVars.user.BankCard == "-1")
        {
            _openPanel("CashTipPanel");
        }
        else
        {
            _openPanel("CashPanel");
        }
    }

    private void FreshGoldAndLottery(object obj)
    {
        m_textGold.text = Convert.ToString(ZH2_GVars.user.gameGold);
        m_textExpeGold.text = Convert.ToString(ZH2_GVars.user.expeGold);
        if (UserInfoViewController.Get() != null)
        {
            UserInfoViewController.Get().RefreshGoldAndExp();
        }
    }

    public void UpdateBtnStoreStatus()
    {
    }

    public void FreshUserInfo()
    {
        m_textUserNickName.text = Convert.ToString(ZH2_GVars.user.nickname);
        try
        {
            if (m_textUserNickName.text.Length > 10)
            {
                m_textUserNickName.text = m_textUserNickName.text.Substring(0, 9);
            }
        }
        catch
        {
        }
        m_textLevel.text = "LV." + Convert.ToString(ZH2_GVars.user.level);
        Text textLevelName = m_textLevelName;
        int level = ZH2_GVars.user.level;
        int language_enum = (int)ZH2_GVars.language_enum;
        textLevelName.text = Utils.GetLevelName(level, language_enum.ToString());
        m_ID.text = ZH2_GVars.user.gameid.ToString();
        try
        {
            if (ZH2_GVars.user.photoId >= m_userIconDataConfig.list.Count)
            {
                Debug.LogError("photoId: " + ZH2_GVars.user.photoId);
                ZH2_GVars.user.photoId = 0;
            }
            m_imageUserIcon.sprite = m_userIconDataConfig.list[ZH2_GVars.user.photoId].sprite;
        }
        catch (Exception message)
        {
            Debug.LogError(message);
            if (m_userIconDataConfig.list.Count >= 1)
            {
                m_imageUserIcon.sprite = m_userIconDataConfig.list[0].sprite;
            }
        }
        if (UserInfoViewController.Get() != null)
        {
            UserInfoViewController.Get().RefreshLevelText();
        }
    }

    public void FreshPayModeRelationIcons()
    {
    }

    private void HandleNetMsg_ChargeNotice(object[] objs)
    {
    }

    private void HandleNetMsg_ApplyExchange(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        bool flag = (bool)dictionary["success"];
        if (dictionary.ContainsKey("newGameGold"))
        {
            ZH2_GVars.user.gameGold = Convert.ToInt32(dictionary["newGameGold"]);
        }
        string ch = (!flag) ? "兑换被拒绝" : "兑换成功";
        string en = (!flag) ? "Failure of exchange" : "Successful exchange";
        string ti = (!flag) ? "การแลกเปลี่ยนถูกปฏิเสธ" : "แลกสำเร็จ";
        string vn = (!flag) ? "Đổi hàng bị từ chối" : "Đổi thành công";
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip(ch, en, ti,vn));
    }

    private void HandleNetMsg_ApplyPay(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        bool flag = (bool)dictionary["success"];
        if (dictionary.ContainsKey("newGameGold"))
        {
            ZH2_GVars.user.gameGold = Convert.ToInt32(dictionary["newGameGold"]);
        }
        string ch = (!flag) ? "充值被拒绝" : "充值成功";
        string en = (!flag) ? "Failure of TopUp" : "Successful TopUp";
        string ti = (!flag) ? "การเติมเงินถูกปฏิเสธ" : "เติมเงินสำเร็จ";
        string vn = (!flag) ? "Nạp tiền bị từ chối" : "Nạp tiền thành công";
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip(ch, en, ti,vn));
    }

    private void HandleNetMsg_exchargeFalseNotice(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        Debug.Log(dictionary);
        bool flag = (bool)dictionary["success"];
        int num = Convert.ToInt32(dictionary["messageStatus"]);
        if (!flag)
        {
            if (ZH2_GVars.payMode == 1)
            {
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("领取失败，彩票已经返还", "reveive failed,lottery has returned", "การรับรางวัลล้มเหลวได้รับการคืนเงิน", "Nhận thất bại, vé số đã được trả lại"));
            }
            else
            {
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("兑换失败", "cash failed", "การแลกเปลี่ยนล้มเหลว", "Đổi thất bại"));
            }
        }
    }

    private void HandleNetMsg_rechargeFalseNotice(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        Debug.Log(dictionary);
        bool flag = (bool)dictionary["success"];
        int num = Convert.ToInt32(dictionary["messageStatus"]);
        if (!flag)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("充值失败", "recharge failed", "การเติมค่าล้มเหลว", "Nạp tiền thất bại"));
        }
    }

    private void HandleNetMsg_newLotteryNotice(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        ZH2_GVars.user.lottery = Convert.ToInt32(dictionary["lottery"]);
        MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);
    }

    private void HandleNetMsg_newGameGoldNotice(object[] objs)
    {
        try
        {
            if (objs == null)
            {
                Debug.LogError("=======objs为空======");
                return;
            }
            Debug.LogError("newGameGold: " + JsonFx.Json.JsonWriter.Serialize(objs));
            Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
            if (dictionary.ContainsKey("gameGold"))
            {
                ZH2_GVars.user.gameGold = Convert.ToInt32(dictionary["gameGold"]);
            }
            else
            {
                JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
                ZH2_GVars.user.gameGold = (int)jsonData[0];
            }
        }
        catch (Exception arg)
        {
            Debug.LogError("错误: " + arg);
        }
        MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);
    }

    private void HandleNetMsg_changeScoreNotice(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        ZH2_GVars.user.gameGold = Convert.ToInt32(dictionary["newGameGold"]);
        int num = Convert.ToInt32(dictionary["amount"]);
        int num2 = Convert.ToInt32(dictionary["status"]);
        string arg = string.Empty;
        string arg2 = string.Empty;
        string arg3 = string.Empty;
        string arg4 = string.Empty;
        switch (num2)
        {
            case 0:
                arg = "充值";
                arg2 = "TopUp";
                arg3 = "เติมเงิน";
                arg4 = "Nạp tiền";
                break;
            case 1:
                arg = "兑换";
                arg2 = "Conversion";
                arg3 = "แลก";
                arg4 = "Chuyển đổi";
                break;
            case 2:
                arg = "赠送";
                arg2 = "Gift";
                arg3 = "แจก";
                arg4 = "Giới thiệu";
                break;
            case 3:
                arg = "扣除";
                arg2 = "Deduct";
                arg3 = "หัก";
                arg4 = "Khấu trừ";
                break;
        }
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip($"{arg}成功! 金币{num}", $"Successful {arg2}! GoldCoin {num}", $"{arg3}ประสบความสำเร็จ! เหรียญทอง{num}", $"{arg4}Thành công rồi!Vàng{num}"));
    }

    private void HandleNetMsg_NewExpeGold(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        ZH2_GVars.user.expeGold = Convert.ToInt32(dictionary["newExpeGold"]);
        MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);
    }

    private void HandleNetMsg_userAwardNotice(object[] objs)
    {
    }

    private void HandleNetMsg_userUnOverflow(object[] objs)
    {
        ZH2_GVars.user.overflow = 0;
    }

    public void HandleNetMsg_SignIn(object[] objs)
    {
        ZH2_GVars.activity_signIn = new JsonData();
        ZH2_GVars.activity_signIn = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
    }

    public void HandleNetMsg_SignIn2(object[] objs)
    {
        JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
        if ((bool)jsonData["success"])
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("签到成功", "Successful check-in", "เช็คอินสำเร็จ", "Đăng ký thành công"));
            ZH2_GVars.user.gameGold = (int)jsonData["gameGold"];
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("签到失败,请重试", "Sign-in failed, please try again", "การเช็คอินล้มเหลวโปรดลองอีกครั้ง", "Đăng nhập thất bại, vui lòng thử lại"));
        }
    }

    private void HandleNetMsg_userOverflow(object[] objs)
    {
        ZH2_GVars.user.overflow = 1;
    }

    private void HandleNetMsg_NewUserActivity(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        int num = (int)dictionary["activityType"];
        Debug.Log(LogHelper.Red("activityType: " + num));
        MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_UseCampaignPanel, num);
    }

    private void HandleNetMsg_NewFinishActivity(object[] objs)
    {
    }

    private void HandleNetMsg_LastGameScore(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        int num = (int)dictionary["score"];
        ZH2_GVars.ScoreOverflow = false;
        MB_Singleton<NetManager>.GetInstance().Send("gcuserService/replyLastGameScore", new object[1]
        {
            ZH2_GVars.user.id
        });
        if (num != 0)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip($"上局的得分为{num}", $"Previous round scores{num}", $"คะแนนของเฟรมบน{num}", $"Điểm số của ván trước là{num}"));
        }
    }

    private void HandleNetMsg_ReplyLastGameScore(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        bool flag = (bool)dictionary["success"];
        Debug.Log(LogHelper.Green("replyLastGameScore"));
    }

    private void HandleNetMsg_UpdategiftMode(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        int num = ZH2_GVars.giftMode = (int)dictionary["giftMode"];
    }

    private void HandleNetMsg_getCustomerServiceUrl(object[] objs)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.LogError("getCustomerServiceUrl: " + JsonMapper.ToJson(objs));
        }
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        if ((bool)dictionary["success"])
        {
            string text = (string)dictionary["url"];
            Debug.LogError("获取到客服地址: " + text);
            Application.OpenURL(text);
        }
        else
        {
            MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("系统未设置客服", "No customer service is set in the system", "ระบบยังไม่ได้ตั้งค่าฝ่ายบริการลูกค้า", "Hệ thống chưa cài đặt dịch vụ khách hàng"));
        }
    }

    private void HandleNetMsg_UpdategiftProp(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        ZH2_GVars.ownedProps.Clear();
        object[] array = (object[])dictionary["prop"];
        for (int i = 0; i < array.Length; i++)
        {
            Dictionary<string, object> dictionary2 = array[i] as Dictionary<string, object>;
            ZH2_GVars.ownedProps.Add(Convert.ToInt32(dictionary2["propId"]), new OwnShopProp(Convert.ToInt32(dictionary2["propId"]), Convert.ToInt64(dictionary2["remainTime"]), (bool)dictionary2["show"]));
        }
        int num = 9;
        while (true)
        {
            if (num < 12)
            {
                if (ZH2_GVars.ownedProps.ContainsKey(num))
                {
                    break;
                }
                if (num == 11)
                {
                    MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_FreshDealer, 0);
                }
                num++;
                continue;
            }
            return;
        }
        MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_FreshDealer, num - 8);
    }

    private void HandleNetMsg_UpdateshareNotice(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        int num = (int)dictionary["awardGold"];
        int gameGold = (int)dictionary["gameGold"];
        ZH2_GVars.user.gameGold = gameGold;
        MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog(string.Format(ZH2_GVars.ShowTip("已为您自动结算通过分享获得的{0}游戏币，请查收", "Automatic settlement harvest {0} coins from sharing rewards,please check", "มีการแบ่งปันข้อมูลให้คุณโดยอัตโนมัติ{0}เงินในเกมกรุณาตรวจสอบ", "Đã tự động thanh toán {0} tiền trò chơi kiếm được bằng cách chia sẻ cho bạn, vui lòng kiểm tra và nhận"), num));
    }

    private void HandleNetMsg_ShareBindingNotice(object[] objs)
    {
        MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_ShareRedSign, true);
    }

    private void HandleNetMsg_ShareClearingNotice(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        int num = (int)dictionary["awardGold"];
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog(string.Format(ZH2_GVars.ShowTip("祝贺您，您在邀请朋友的活动中获得了{0}币，已发送至您的邮件中", "Congratulations , you get {0} coins from inviting activity and go check your mail", "ยินดีด้วยนะคะคุณได้รับบัตรเชิญจากเพื่อน ๆ แล้ว {0}เงิน ที่ถูกส่งไปยังจดหมายของคุณแล้ว ", "Xin chúc mừng, bạn đã nhận được {0} tiền xu tại sự kiện mời bạn bè, đã được gửi đến tin nhắn của bạn"), num));
    }

    private void HandleNetMsg_getGameGold(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        int gameGold = (int)dictionary["gameGold"];
        ZH2_GVars.user.gameGold = gameGold;
        m_textGold.text = Convert.ToString(ZH2_GVars.user.gameGold);
    }

    private void HandleNetMsg_sendNotice(object[] objs)
    {
        JsonData jsonData = JsonMapper.ToObject(JsonFx.Json.JsonWriter.Serialize(objs[0]));
        Debug.LogError("通知: " + jsonData.ToJson());
        if (All_NoticePanel.GetInstance() != null)
        {
            All_NoticePanel.GetInstance().AddTip(jsonData);
        }
        else
        {
            Debug.LogError("====== 为空");
        }
    }

    private void HandleNetMsg_UpdateshareMode(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        ZH2_GVars.shareMode = (int)dictionary["shareMode"];
        ZH2_GVars.specialMark = (int)dictionary["specialMark"];
        MB_Singleton<AppManager>.Get().Notify(UIGameMsgType.UINotify_Fresh_SharePanel, ZH2_GVars.shareMode);
    }

    private void HandleNetMsg_UpdateRunStatus(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        object args = dictionary["gameRunStatus"];
        InnerGameManager.Get().UpdateGameRunStatus(args);
    }

    private void HandleNetMsg_ModifySafeBoxPwd(object[] objs)
    {
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("口令修改成功", "The password of safe box has been modified.", "การแก้ไขรหัสผ่านสำเร็จ ", "Thay đổi mật khẩu thành công"));
    }

    private void HandleNetMsg_NewUserLevel(object[] obj)
    {
        Dictionary<string, object> dictionary = obj[0] as Dictionary<string, object>;
        int level = (int)dictionary["newLevel"];
        ZH2_GVars.user.level = level;
        FreshUserInfo();
    }

    private void HandleNetMsg_NewUserMail(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        int num = (int)dictionary["notReadCount"];
    }

    public void HandleNetMsg_QuitToLogin(object[] objs)
    {
        ZH2_GVars.lockRelogin = true;
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        int num = -1;
        string content = string.Empty;
        try
        {
            num = (int)dictionary["type"];
        }
        catch (Exception)
        {
            content = ((!dictionary.ContainsKey("type")) ? ZH2_GVars.ShowTip("账号被管理员登出!", "The account is logged out by the administrator. Procedure!", "หมายเลขบัญชีถูกโพสต์ออกโดยผู้ดูแลระบบ", "Tài khoản bị quản trị viên đăng xuất") : dictionary["type"].ToString());
        }
        MB_Singleton<NetManager>.Get().Disconnect();
        MB_Singleton<AppManager>.Get().ForbidNetDownHint();
        ZH2_GVars.lockReconnect = true;
        ZH2_GVars.ScoreOverflow = false;
        switch (num)
        {
            case -1:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, QuitToLogin);
                break;
            case 1:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("系统维护", "Server maintenance", "การดูแลระบบ", "Bảo trì hệ thống"), showOkCancel: false, QuitToLogin);
                break;
            case 2:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号被冻结", "This account has been frozen", "เลขบัญชีถูกระงับ", "Tài khoản bị đóng băng"), showOkCancel: false, QuitToLogin);
                break;
            case 3:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("账号被删除", "This account has been delete", "ลบบัญชีผู้ใช้", "Tài khoản bị xóa"), showOkCancel: false, QuitToLogin);
                break;
            case 4:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("该账号已在别处登录，您被迫下线", "The account has been logged in elsewhere", "หมายเลขบัญชีถูกล็อกอินจาก ที่อื่นแล้วคุณถูกบังคับให้ตัดสาย", "Tài khoản đã được đăng nhập ở nơi khác và bạn buộc phải offline"), showOkCancel: false, QuitToLogin);
                break;
            case 5:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("后台修改了密码", "The password has been modified", "รหัสถูกแก้ไขหลังเวที", "Thay đổi mật khẩu nền"), showOkCancel: false, QuitToLogin);
                break;
            default:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("请重新登录", "Please ReLogin", "โปรดเข้าสู่ระบบอีกครั้ง", "Vui lòng đăng nhập lại"), showOkCancel: false, QuitToLogin);
                break;
            case 33:
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("游客登录功能已关闭", "Tourists landing has been closed", "ระบบล็อกอินของนักท่องเ ที่ยวถูกปิด", "Chức năng đăng nhập khách đã tắt"), showOkCancel: false, QuitToLogin);
                break;
        }
    }

    private void HandleNetMsg_CancelWaitingExpiryUser(object[] objs)
    {
        MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("上级取消了本次操作", "The superior canceled the operation", "หัวหน้ายกเลิกปฏิบัติการนี้", "Cấp trên hủy bỏ chiến dịch này."), showOkCancel: false, delegate
        {
            base.transform.parent.Find("CashPanel").gameObject.SetActive(value: false);
        });
    }

    private void HandleNetMsg_AddExpeGold(object[] objs)
    {
        Dictionary<string, object> dictionary = objs[0] as Dictionary<string, object>;
        Debug.Log(dictionary);
        if ((bool)dictionary["success"])
        {
            ZH2_GVars.user.expeGold = (int)dictionary["expeGold"];
            MB_Singleton<AppManager>.GetInstance().Notify(UIGameMsgType.UINotify_Fresh_GoldAndLottery);
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("申请成功", "Application successful", "สำเร็จ", "Ứng dụng thành công"));
            return;
        }
        int num = (int)dictionary["msgCode"];
        Debug.Log(num);
        int num2 = num;
        if (num2 != 15)
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("登录失败", "login false", "การล็อกอินล้มเหลว", "Đăng nhập thất bại"));
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("体验币低于10000方可申请", "Tickets less than 10000 may apply for each application make up 10000 tickets", "เงินน้อยกว่า 10,000 เยนสามารถสมัครได้", "Ít hơn 10.000 Experience Coin có thể được yêu cầu"));
        }
    }

    public static string getPaymentErrorMsg(int errorMsg)
    {
        switch (errorMsg)
        {
            case 1:
                return ZH2_GVars.ShowTip("后台未开启此项功能", "This feature is closed", "ไม่มีการเปิดใช้งานคุณสมบัตินี้ หลังเวที", "Chức năng này không được bật trong nền");
            case 2:
                return ZH2_GVars.ShowTip("提交失敗", "Top-up fail", "การส่งล้มเหลว", "Gửi thất bại");
            case 3:
                return ZH2_GVars.ShowTip("充值额度必须大于零", "Top-up amount must be larger than zero", "ค่า ที่ชาร์จแบตจะต้องมากกว่า ศูนย์", "Số tiền nạp phải lớn hơn 0");
            case 4:
                return ZH2_GVars.ShowTip("余额不足", "Not enough balance", "ยอดคงเหลือน้อย", "Số dư không đủ");
            case 5:
                return ZH2_GVars.ShowTip("兑奖金额不能小于零", "The exchange amount cannot be smaller than zero", "จำนวนเงิน ที่สะสมจะน้อยกว่า ศูนย์ไม่ได้ ", "Số tiền thưởng không được nhỏ hơn 0");
            case 6:
                return ZH2_GVars.ShowTip("需要先输入完整个人资料", "Incomplete user profile", "ต้องป้อนข้อมูลทั้งหมดก่อน", "Bạn cần nhập đầy đủ profile trước.");
            default:
                return ZH2_GVars.ShowTip("来自服务器的未知错误消息", "unknow error message come form server", "ข้อความแสดงข้อผิดพลาดที่ไม่รู้จักจากเซิร์ฟเวอร์", "Thông báo lỗi không xác định từ máy chủ");
        }
    }

    private void QuitToLogin()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
