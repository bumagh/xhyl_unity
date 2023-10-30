using BestHTTP;

using DG.Tweening;

using LitJson;

using Spine.Unity;

using System.Collections;
//using System.Security.Policy;
using System.Security;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InnerGameItem : MonoBehaviour
{
    private InnerGame m_innerGame;

    [SerializeField]
    private GameObject m_goWait;

    [SerializeField]
    private GameObject m_goEmpty;

    [SerializeField]
    private GameObject m_goProgress;

    [SerializeField]
    private Text m_goText;

    [SerializeField]
    private Image m_imageProgress;

    private HTTPRequest m_httpRequest;

    public int lastDownloaded;

    public float lastProgressTime;

    [HideInInspector]
    public Button btn;

    private Image icon;

    private Image imgName;

    public void Init(InnerGame innerGame)
    {
        m_innerGame = innerGame;
        if (innerGame.spine != null)
        {
            RectTransform rect = transform.Find("Spine").GetComponent<RectTransform>();
            rect.anchoredPosition = OnSpinePosition(gameObject.name);
            SkeletonGraphic spineAni = transform.Find("Spine").GetComponent<SkeletonGraphic>();
            spineAni.skeletonDataAsset = innerGame.spine;
            spineAni.Initialize(true);
            string aniName = spineAni.SkeletonData.Animations.Items[0].Name;
            // string aniName = "animation";
            spineAni.AnimationState.SetAnimation(0, aniName, true);

        }
        else
        {
            transform.Find("ToadFish").GetComponent<Image>().sprite = innerGame.spriteIcon;
        }
        if (innerGame.spiName != null)
        {
            imgName = base.transform.Find("Name").GetComponent<Image>();
            switch (ZH2_GVars.language_enum)
            {
                case ZH2_GVars.LanguageType.Chinese:
                    imgName.sprite = innerGame.spiName;
                    break;
                case ZH2_GVars.LanguageType.English:
                    imgName.sprite = innerGame.spiNameEN;
                    break;
                case ZH2_GVars.LanguageType.Vietnam:
                    imgName.sprite = innerGame.spiNameVN;
                    break;
                default:
                    imgName.sprite = innerGame.spiName;
                    break;
            }
            imgName.SetNativeSize();
        }
        btn = base.transform.Find("Button").GetComponent<Button>();
        btn.onClick.AddListener(OnItem_Click);
        ChangeState(InnerGameState.Default);
        if (innerGame.isHttpGame)
        {
            m_goText.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (imgName != null)
        {
            switch (ZH2_GVars.language_enum)
            {
                case ZH2_GVars.LanguageType.Chinese:
                    imgName.sprite = m_innerGame.spiName;
                    break;
                case ZH2_GVars.LanguageType.English:
                    imgName.sprite = m_innerGame.spiNameEN;
                    break;
                case ZH2_GVars.LanguageType.Vietnam:
                    imgName.sprite = m_innerGame.spiNameVN;
                    break;
                default:
                    imgName.sprite = m_innerGame.spiName;
                    break;
            }
            imgName.SetNativeSize();
        }

    }


    Vector2 OnSpinePosition(string name)
    {
        switch (name)
        {

            case "牛魔王":
                return new Vector2(0, 33.31f);
            case "幸运六狮":
                return new Vector2(-5.18f, -11.61f);
            case "QQ美人鱼":
                return new Vector2(-21.72f, 32.93f);
            case "摇钱树":
                return new Vector2(0, 32);
            case "金蟾捕鱼":
                return new Vector2(0, 16);
            case "李逵劈鱼":
                return new Vector2(-25.61f, 14.28f);
            case "单挑":
                return new Vector2(0f, 13);
            case "大闹天宫":
                return new Vector2(0, 30);
            case "奔驰宝马":
                return new Vector2(0, 0);
            case "金沙银沙":
                return new Vector2(0, 26);
            case "龙虎斗":
                return new Vector2(0, 50);
            case "水果机":
                return new Vector2(-9f, 0);
            case "抢庄牛牛":
                return new Vector2(-22.91f, 41.82f);
            case "炸金花":
                return new Vector2(-13.99f, 6.1f);
            case "三公":
                return new Vector2(0, 6.5f);
            case "欧洲厅百家乐":
                return new Vector2(0, 24.6f);
            case "AsiaGaming":
                return new Vector2(17.7f, 22.93f);
            case "押宝抢庄牛牛":
                return new Vector2(0, 13.8f);
            case "龙虎":
                return new Vector2(0, 15.35f);
            case "21点":
                return new Vector2(6, 9.35f);
            case "旗舰国际厅":
                return new Vector2(0, 0);
            case "多台":
                return new Vector2(0, 7.19f);
            case "牛牛2":
                return new Vector2(0, 11.98f);
            case "二八杠":
                return new Vector2(0, 10);
            case "通比牛牛":
                return new Vector2(0, 10);
            case "抢庄牌九":
                return new Vector2(0, 0);
            case "斗地主":
                return new Vector2(0, 0);
            case "百人牛牛":
                return new Vector2(-30, 0);
            case "血流成河":
                return new Vector2(0, 0);
            case "二人麻将":
                return new Vector2(0, 0);
            case "单挑牛牛":
                return new Vector2(0, 0);
            case "鱼虾蟹":
                return new Vector2(0, 0);
            case "极速炸金花":
                return new Vector2(0, 0);
            case "十三水":
                return new Vector2(0, 0);
            case "百家乐":
                return new Vector2(0, 7);
            case "万人炸金花":
                return new Vector2(0, 0);
            case "沙巴体育":
                return new Vector2(-2, 21.48f);
            case "小爱神":
                return new Vector2(0, 20);
            case "孙二娘":
                return new Vector2(0, 15);
            case "金鸡报喜":
                return new Vector2(5, 20);
            case "跳起来":
                return new Vector2(-3, 30);
            case "跳更高":
                return new Vector2(-3, 30);
            case "五福临门":
                return new Vector2(0, 30);
            case "武圣":
                return new Vector2(0, 30);
            case "宙斯":
                return new Vector2(0, 35);
            case "洪福齐天":
                return new Vector2(0, 33);
            case "一炮捕鱼":
                return new Vector2(0, 10);
            case "直式蹦迪":
                return new Vector2(0, 26);
            case "百人骰宝":
                return new Vector2(0, 32f);
            case "跑得快":
                return new Vector2(0, 0);
            case "福神报喜":
                return new Vector2(-1.89f, 28.19f);
            case "精灵":
                return new Vector2(-2, 30);
            case "跳高高":
                return new Vector2(0, 23);
            case "跳高高2":
                return new Vector2(0, 23);
            case "跳过来":
                return new Vector2(0, 30);
            case "直式洪福齐天":
                return new Vector2(0, 28);
            case "CQ9":
                return new Vector2(0, 28);
            case "东方神起":
                return new Vector2(0, 28);
            case "冰雪女王":
                return new Vector2(0, 15);
            case "火烧连环船2":
                return new Vector2(0, 25);
            case "金钱树":
                return new Vector2(0, 25);
            case "火爆777":
                return new Vector2(0, 0);


            default: return new Vector2(0, 0);
        }
    }

    IEnumerator OnSpine(SkeletonGraphic skeleton)
    {
        yield return new WaitForSeconds(0.2f);
        skeleton.enabled = true;
    }

    public void SetLanguage(InnerGame innerGame)
    {
    }

    public InnerGame GetInnerGame()
    {
        return m_innerGame;
    }

    public void OnItem_Click()
    {
        if (m_innerGame.RunStatus == 0 )
        {
            Debug.LogError("====敬请期待=====" + base.name);
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("敬请期待", "The game is not open yet", "ยังไม่เปิดให้เล่น", "Mong chờ"));
        }
        else if (!m_innerGame.isHttpGame)
        {
            Debug.LogError("====点击了热更有戏=====" + base.name);
            InnerGameManager.Get().OnBtnClick_GameItem(this);
        }
        else if (m_innerGame.isHttpGame)
        {
            Debug.LogError("====点击了网页游戏=====" + base.name);
            // StartCoroutine(SendHttpGame("http://" + ZH2_GVars.IPAddress + ":80/live/api/login?", m_innerGame.plat_type, m_innerGame.game_code, m_innerGame.game_type));
            StartCoroutine(SendHttpGame("http://"+ZH2_GVars.IPAddress+":8091/live/api/login?", m_innerGame.plat_type, m_innerGame.game_code, m_innerGame.game_type));
        }
        else
        {
            Debug.LogError("====不是热更  敬请期待=====");
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("敬请期待", "The game is not open yet", "ยังไม่เปิดให้เล่น", "Mong chờ"));
        }
    }

    private IEnumerator SendHttpGame(string url, string plat_type, string game_code, string game_type)
    {
        Debug.LogError("Url: " + url + "  username: " + ZH2_GVars.AccountName + " plat_type: " + plat_type + "  game_code: " + game_code + "  game_type: " + game_type + "  is_mobile_url: 3");
        WWWForm wwwform = new WWWForm();
        wwwform.AddField("username", ZH2_GVars.AccountName);
        wwwform.AddField("plat_type", plat_type);
        wwwform.AddField("game_code", game_code);
        wwwform.AddField("is_mobile_url", "3");
        wwwform.AddField("game_type", game_type);
        MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("正在进入游戏，请稍等", "Entering the game, please wait a moment", "กำลังเข้าสู่เกมรอสักครู่นะคะ", "Đang vào game, vui lòng chờ"));
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
            MB_Singleton<AlertDialog>.GetInstance().Hide();
            string url2 = jsonData["data"]["url"].ToString();
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
               // Application.OpenURL(url2);
                OpenWeb.intance.OnOpenWebUrl(url2);
            }
            else
            {
                OpenWeb.intance.OnOpenWebUrl(url2);
            }
        }
        else
        {
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(jsonData["msg"].ToString());
        }
    }

    public void OnBtnUninstall_Click()
    {
        InnerGameManager.Get().UninstallGame(this);
    }

    public void UpdateProgress(float newProgress)
    {
        m_goText.text = ((int)(newProgress * 100f)).ToString() + "%";
        m_imageProgress.fillAmount = newProgress;
    }

    public void UpdateProgress()
    {
        m_goText.text = ((int)(m_innerGame.progress * 100f)).ToString() + "%";
        m_imageProgress.fillAmount = m_innerGame.progress;
    }

    private IEnumerator AniDownloading()
    {
        m_imageProgress.fillAmount = 0.25f;
        float speed = -360f;
        while (true)
        {
            m_imageProgress.rectTransform.Rotate(new Vector3(0f, 0f, speed * Time.deltaTime));
            yield return null;
        }
    }

    public void ChangeState(InnerGameState newState)
    {
        switch (newState)
        {
            case InnerGameState.Default:
                m_goEmpty.transform.parent.gameObject.SetActive(value: true);
                m_goEmpty.SetActive(value: true);
                m_goWait.SetActive(value: true);
                m_goProgress.SetActive(value: false);
                m_goText.text = ZH2_GVars.ShowTip("下 载 游 戏", "DownloadGame", "ดาวน์โหลดเกม", "Tải về trò chơi");
                m_goEmpty.transform.GetChild(0).gameObject.SetActive(value: true);
                m_goEmpty.transform.GetChild(1).gameObject.SetActive(value: false);
                break;
            case InnerGameState.Waiting:
                m_goEmpty.transform.parent.gameObject.SetActive(value: true);
                m_goEmpty.SetActive(value: false);
                m_goWait.SetActive(value: true);
                m_goProgress.SetActive(value: false);
                m_goEmpty.transform.GetChild(0).gameObject.SetActive(value: false);
                m_goEmpty.transform.GetChild(1).gameObject.SetActive(value: false);
                break;
            case InnerGameState.Downloading:
                m_goEmpty.transform.parent.gameObject.SetActive(value: true);
                m_goEmpty.SetActive(value: true);
                m_goWait.SetActive(value: true);
                m_goProgress.SetActive(value: true);
                m_goEmpty.transform.GetChild(0).gameObject.SetActive(value: false);
                m_goEmpty.transform.GetChild(1).gameObject.SetActive(value: true);
                if (!m_innerGame.isOurGame)
                {
                    if (Application.platform == RuntimePlatform.IPhonePlayer || m_innerGame.isLuaGame)
                    {
                        StartCoroutine(AniDownloading());
                    }
                    else
                    {
                        UpdateProgress();
                    }
                }
                break;
            case InnerGameState.ReadyInstall:
            case InnerGameState.OK:
                m_goEmpty.transform.parent.gameObject.SetActive(value: true);
                m_goEmpty.SetActive(value: false);
                m_goWait.SetActive(value: true);
                m_goProgress.SetActive(value: false);
                m_goEmpty.transform.GetChild(0).gameObject.SetActive(value: false);
                m_goEmpty.transform.GetChild(1).gameObject.SetActive(value: false);
                m_goText.text = "开 始 游 戏";
                m_goText.text = ZH2_GVars.ShowTip("开 始 游 戏", "STARTGAME", "เริ่มเกม", "Bắt đầu trò chơi");
                break;
            case InnerGameState.Paused:
                m_goEmpty.transform.parent.gameObject.SetActive(value: true);
                m_goEmpty.transform.GetChild(0).gameObject.SetActive(value: true);
                m_goEmpty.transform.GetChild(1).gameObject.SetActive(value: false);
                m_goEmpty.SetActive(value: true);
                m_goWait.SetActive(value: true);
                m_goProgress.SetActive(value: true);
                if (Application.platform != RuntimePlatform.IPhonePlayer)
                {
                    UpdateProgress();
                }
                break;
        }
        m_innerGame.state = newState;
    }

    public void SetHTTPRequest(HTTPRequest req)
    {
        m_httpRequest = req;
    }

    public HTTPRequest GetHTTPRequest()
    {
        return m_httpRequest;
    }

    public bool IsDownloading()
    {
        return m_httpRequest != null;
    }

    private void OnDestroy()
    {
        DownloadAbort();
    }

    private void DownloadAbort()
    {
        HTTPRequest httpRequest = m_httpRequest;
        if (httpRequest != null && httpRequest.State < HTTPRequestStates.Finished)
        {
            httpRequest.OnProgress = null;
            httpRequest.Callback = null;
            httpRequest.Abort();
        }
    }

    public void SetDisplayUninstallBtn(bool value)
    {
        base.transform.Find("btnUninstall").gameObject.SetActive(value);
    }
}
