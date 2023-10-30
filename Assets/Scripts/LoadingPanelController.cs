using DG.Tweening;

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanelController : MonoBehaviour
{
    private bool m_allowLoadingFinish;

    public bool isInFirstTime = true;

    public Slider slider;

    public Text sliderDownText;

    public Text VersionText;

    public GameObject dengXiao;

    public GameObject load;

    private float _duration = 3f;

    private float _duration2 = 0.5f;

    private GameObject @object;

    [SerializeField]
    private UserIconDataConfig m_userIconDataConfig;

    private float resourcesSize = 57.32f;

    private Coroutine waitSelfDie;

    private void Start()
    {
        if (!isInFirstTime)
        {
            WaitStart();
        }
    }

    private void WaitStart()
    {
        StartCoroutine(_waitForPrepared());
        MB_Singleton<AppManager>.Get().Register(UIGameMsgType.UINotify_Allow_LoadingFinish, this, delegate
        {
            SetAllowLoadingFinish();
        });
    }

    private void OnEnable()
    {
        string @string = PlayerPrefs.GetString("假加载", "第一次");
        isInFirstTime = (@string != "完成");
        Debug.LogError("======开始登陆遮挡页面======== " + @string + "  " + isInFirstTime);
        if (isInFirstTime)
        {
            Debug.LogError("进入假加载");
            load.SetActive(value: true);
            dengXiao.SetActive(value: true);
            StartCoroutine(ShowSlider());
        }
        else
        {
            load.SetActive(value: false);
            dengXiao.SetActive(value: false);
        }
        base.gameObject.transform.localScale = Vector3.one;
        @object = base.transform.Find("Image").gameObject;
        VersionText.text = "v" + Application.version;
        if (ZH2_GVars.isStartedFromGame)
        {
            try
            {
                @object.GetComponent<Image>().DOFade(1f, 0f);
                @object.SetActive(value: true);
                if (ZH2_GVars.user.photoId >= m_userIconDataConfig.list.Count)
                {
                    ZH2_GVars.user.photoId = 0;
                }
                if (ZH2_GVars.user.photoId < 0)
                {
                    ZH2_GVars.user.photoId = 0;
                    return;
                }
            }
            catch (Exception message)
            {
                Debug.LogError(message);
            }
        }
        else if ((bool)@object)
        {
            @object.SetActive(value: false);
        }
        if (isInFirstTime)
        {
            Debug.LogError("=====第一次加载游戏=====");
            return;
        }
        if (waitSelfDie != null)
        {
            StopCoroutine(waitSelfDie);
        }
        waitSelfDie = StartCoroutine(WaitSelfDie());
    }

    private IEnumerator ShowSlider()
    {
        slider.value = 0f;
        while (slider.value < 0.99f)
        {
            float value = 1f / _duration * Time.deltaTime;
            slider.value += value;
            float textValue = slider.value * resourcesSize;
            sliderDownText.text = string.Format(ZH2_GVars.ShowTip("资源下载中({0}M/{1}M)", "Downloading resources({0}M/{1}M)", "ในการดาวน์โหลดทรัพยากร({0}M/{1}M)", "Đang tải xuống tài nguyên({0}M/{1}M)"), textValue.ToString("f2"), resourcesSize.ToString("f2"));
            yield return new WaitForSeconds(0.05f);
        }
        slider.value = 1f;
        isInFirstTime = false;
        PlayerPrefs.SetString("假加载", "完成");
        PlayerPrefs.Save();
        WaitStart();
        yield return new WaitForSeconds(0.15f);
        sliderDownText.text = ZH2_GVars.ShowTip("资源下载完成", "Resource download completed", "Resource download completed", "Tải xuống tài nguyên hoàn tất");
        yield return new WaitForSeconds(0.15f);
        sliderDownText.text = ZH2_GVars.ShowTip("拼命加载中", "Loading desperately", "Loading desperately", "Đang tải...");      
        MB_Singleton<AppManager>.GetInstance().ShowLoginPanel(0.15f);
        base.gameObject.SetActive(value: false);
    }

    private IEnumerator ShowSlider2()
    {
        slider.value = 0f;
        while (slider.value < 0.99f)
        {
            float value = 1f / _duration2 * Time.deltaTime;
            slider.value += value;
            float textValue = slider.value * 100f;
            sliderDownText.text = string.Format(ZH2_GVars.ShowTip("加载中,过程不消耗流量({0}%)", "During loading, the process does not consume traffic ({0}%)", "Đang tải, quá trình không tiêu thụ lưu lượng ({0}%)", "Đang tải, quá trình không tiêu thụ lưu lượng ({0}%)"), textValue.ToString("f2"));
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator WaitSelfDie()
    {
        yield return new WaitForSeconds(20f);
        Debug.LogError("========强制掉线========");
        if (base.gameObject.activeInHierarchy)
        {
            if (MB_Singleton<AlertDialog>.GetInstance().gameObject.activeInHierarchy)
            {
                Debug.LogError("已经在提示了");
            }
            else
            {
                MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("网络异常!请重新登录(Ex02)", "The network is abnormal! Please log in again(Ex02)", "ความผิดปกติของเครือข่าย! กรุณาเข้าสู่ระบบอีกครั้ง (Ex02)", "Mạng lưới bất thường! Vui lòng đăng nhập lại (EX02)"), showOkCancel: false, delegate
                {
                    ZH2_GVars.lockQuit = false;
                    ZH2_GVars.lockReconnect = false;
                    SceneManager.LoadSceneAsync(0);
                });
            }
        }
        else
        {
            Debug.LogError("====啊伟已经死了====");
        }
    }

    private IEnumerator _waitForPrepared()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.BG_First);
        yield return null;
        while (!m_allowLoadingFinish)
        {
            yield return null;
        }
        bool isShow = false;
        while (!isShow)
        {
            if (ZH2_GVars.lockRelogin || (!ZH2_GVars.isStartedFromGame && !ZH2_GVars.isStartedFromLuaGame))
            {
                isShow = true;
                MB_Singleton<AppManager>.GetInstance().ShowLoginPanel(0.15f);
            }
            yield return null;
        }
    }

    public void SetAllowLoadingFinish()
    {
        Debug.Log("===========所有加载完毕=========");
        m_allowLoadingFinish = true;
    }
}
