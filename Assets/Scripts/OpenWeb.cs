using LitJson;

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OpenWeb : MonoBehaviour
{
    public static OpenWeb intance;

    private UniWebView m_UniWebView;



    public GameObject soud;

    public GameObject loadIma;

    public GameObject BG;

    public Button btn;

    public RectTransform m_UniWebRectTransform;
    public RectTransform transform;

    private void Awake()
    {
        intance = this;
        btn.onClick.AddListener(CloseUrl);
        btn.gameObject.SetActive(value: false);
        BG.gameObject.SetActive(value: false);
        m_UniWebRectTransform.gameObject.SetActive(value: false);
        soud.SetActive(value: true);
        loadIma.gameObject.SetActive(value: false);
        LeftScele();
    }

    public void OnOpenWebUrl(string url)
    {
        UnityEngine.Debug.LogError("打开链接: " + url);
        soud.SetActive(value: false);
        if (!(url == string.Empty))
        {
            AuToScele();
            btn.gameObject.SetActive(value: true);
            BG.gameObject.SetActive(value: true);
            m_UniWebRectTransform.gameObject.SetActive(value: true);
            loadIma.gameObject.SetActive(value: true);
            soud.SetActive(value: false);
            CreateUniWebView();
            OnLoaded(url);
        }
    }

    private void AuToScele()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
    }

    public void LeftScele()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void CreateUniWebView()
    {
        InitWeb();
        m_UniWebView = null;
        m_UniWebView = m_UniWebRectTransform.gameObject.AddComponent<UniWebView>();
        m_UniWebView.ReferenceRectTransform = transform;
        m_UniWebView.OnMessageReceived += OnMessageReceived;
        m_UniWebView.OnPageStarted += OnPageStarted;
        m_UniWebView.OnPageFinished += OnPageFinished;
        m_UniWebView.OnKeyCodeReceived += OnKeyCodeReceived;
        m_UniWebView.OnPageErrorReceived += OnPageErrorReceived;
        m_UniWebView.OnShouldClose += OnShouldClose;
        m_UniWebView.SetBackButtonEnabled(enabled: true);
        m_UniWebView.SetAllowFileAccessFromFileURLs(flag: true);
        m_UniWebView.SetShowSpinnerWhileLoading(flag: true);
        m_UniWebView.SetHorizontalScrollBarEnabled(enabled: false);
        m_UniWebView.SetVerticalScrollBarEnabled(enabled: false);
        m_UniWebView.BackgroundColor = Color.white;
    }

    private void InitWeb()
    {
        if (m_UniWebRectTransform.childCount > 0)
        {
            for (int i = 0; i < m_UniWebRectTransform.childCount; i++)
            {
                UnityEngine.Object.Destroy(m_UniWebRectTransform.GetChild(i));
            }
        }
        m_UniWebView = m_UniWebRectTransform.gameObject.GetComponent<UniWebView>();
        if (m_UniWebView != null)
        {
            UnityEngine.Debug.LogError("=======m_UniWebView存在======");
            try
            {
                m_UniWebView.Stop();
                UnityEngine.Object.Destroy(m_UniWebView);
            }
            catch (Exception arg)
            {
                UnityEngine.Debug.LogError("移除组件异常: " + arg);
            }
        }
    }

    public void OnLoaded(string m_Url)
    {
        m_UniWebView.Load(m_Url);
        m_UniWebView.Show();
    }

    private void OnReLoaded()
    {
        if (m_UniWebView.isActiveAndEnabled)
        {
            m_UniWebView.Reload();
        }
    }

    private void OnClose()
    {
        m_UniWebView.Hide();
        InitWeb();
    }

    private void OnPageStarted(UniWebView webView, string url)
    {
        UnityEngine.Debug.Log("[UbiWebPresenter]  OnPageStarted " + url);
    }

    private void OnPageFinished(UniWebView webView, int statusCode, string url)
    {
        UnityEngine.Debug.Log("[UbiWebPresenter]  OnPageFinished statusCode:" + $"statusCode:{statusCode},url{url}");
    }

    private void OnPageErrorReceived(UniWebView webView, int errorCode, string errorMessage)
    {
        if (errorCode == -2)
        {
            OnClose();
        }
        UnityEngine.Debug.Log("[UbiWebPresenter]  OnPageErrorReceived ：" + $"errorCode:{errorCode},errorMessage{errorMessage}");
    }

    private void OnKeyCodeReceived(UniWebView webView, int keyCode)
    {
        if (keyCode == 4)
        {
            OnClose();
        }
        UnityEngine.Debug.Log("[UbiWebPresenter]  OnKeyCodeReceived keycode:" + keyCode);
    }

    private void OnMessageReceived(UniWebView webView, UniWebViewMessage message)
    {
        UnityEngine.Debug.Log("[UbiWebPresenter]  OnMessageReceived :" + message.RawMessage);
    }

    private bool OnShouldClose(UniWebView webView)
    {
        webView.CleanCache();
        webView = null;
        CloseUrl();
        InitWeb();
        return true;
    }

    private void View_OnLoadComplete(UniWebView webView, bool success, string errorMessage)
    {
        if (success)
        {
            webView.Show();
            loadIma.gameObject.SetActive(value: false);
        }
        else
        {
            UnityEngine.Debug.LogError("Something wrong in webview loading: " + errorMessage);
        }
    }

    private void OnDisable()
    {
        if (MB_Singleton<AppManager>.Get() != null)
        {
            MB_Singleton<AppManager>.Get().SendTransAll(true);
        }

        CloseUrl();
        LeftScele();
        btn.gameObject.SetActive(value: false);
        BG.gameObject.SetActive(value: false);
        m_UniWebRectTransform.gameObject.SetActive(value: false);
        if (soud != null)
        {
            soud.SetActive(value: true);
        }
        loadIma.gameObject.SetActive(value: false);
    }

    public void CloseUrl()
    {
        /*
        if (MB_Singleton<AppManager>.Get() != null)
        {
            MB_Singleton<AppManager>.Get().SendTransAll();
        }
        */

        if (btn != null)
        {
            btn.gameObject.SetActive(value: false);
            BG.gameObject.SetActive(value: false);
        }
        if (m_UniWebRectTransform != null)
        {
            m_UniWebRectTransform.gameObject.SetActive(value: false);
        }
        if (loadIma != null)
        {
            loadIma.gameObject.SetActive(value: false);
        }
        LeftScele();
        if (soud != null)
        {
            soud.SetActive(value: true);
        }
        if (btn != null)
        {
            btn.gameObject.SetActive(value: false);
            BG.gameObject.SetActive(value: false);
        }
        if (soud != null)
        {
            soud.SetActive(value: true);
        }


    }




    public void OnSound()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = false;
        btn.gameObject.SetActive(value: false);
        BG.gameObject.SetActive(value: false);
        m_UniWebRectTransform.gameObject.SetActive(value: false);
        soud.SetActive(value: true);
    }
}
