using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AssetBundleManager : MonoBehaviour
{
    private static AssetBundleManager instance;

    public static List<AssetBundleSystem> AssetBundleList = new List<AssetBundleSystem>();

    private bool haveGet;

    private string[] liuShiResourceName = new string[10]
    {
        "bingzhui02",
        "xionmao",
        "tuzi",
        "HOUZI",
        "shizi",
        "zhxAtlas1",
        "zhxAtlas 1",
        "C02_andy",
        "Burst_Anim_3",
        "DTLoading"
    };

    public static AssetBundleManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        AssetBundleList = new List<AssetBundleSystem>();
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    private void Update()
    {
        if (!haveGet && InnerGameManager.urlInit != string.Empty)
        {
            haveGet = true;
            StartCoroutine(GetInstance().StartXML());
        }
    }

    public IEnumerator StartXML()
    {
        string empty = string.Empty;
        string path = (Application.platform == RuntimePlatform.Android) ? (InnerGameManager.urlInit + "Android/Version.xml") : ((Application.platform != RuntimePlatform.IPhonePlayer) ? (InnerGameManager.urlInit + "Android/Version.xml") : (InnerGameManager.urlInit + "iOS/Version.xml"));
        Debug.Log("=====配置表地址=====" + path);
        UnityWebRequest www2 = UnityWebRequest.Get(path);
        yield return www2.Send();
        LoadXmlFlie(www2.downloadHandler.text);
        for (int i = 0; i < AssetBundleList.Count; i++)
        {
            if (PlayerPrefs.GetInt(AssetBundleList[i].GameTag + "_Version") == AssetBundleList[i].Versions)
            {
                int _Ison = PlayerPrefs.GetInt(PlayerPrefs.GetString(AssetBundleList[i].GameTag));
                if (_Ison == 1)
                {
                    string empty2 = string.Empty;
                    string url = (Application.platform == RuntimePlatform.Android) ? (InnerGameManager.urlInit + "Android/" + AssetBundleList[i].GameTag + "/" + AssetBundleList[i].GameTag.ToLower() + ".unity3d") : ((Application.platform != RuntimePlatform.IPhonePlayer) ? (InnerGameManager.urlInit + "Android/" + AssetBundleList[i].GameTag + "/" + AssetBundleList[i].GameTag.ToLower() + ".unity3d") : (InnerGameManager.urlInit + "iOS/" + AssetBundleList[i].GameTag + "/" + AssetBundleList[i].GameTag.ToLower() + ".unity3d"));
                    WWW www = WWW.LoadFromCacheOrDownload(url, AssetBundleList[i].Versions);
                    yield return www;
                    if (www.isDone && www.error == null && AssetBundleList[i].SceneAsset == null)
                    {
                        AssetBundle assetBundle = www.assetBundle;
                        AssetBundleList[i].SceneAsset = assetBundle;
                    }
                }
            }
            else
            {
                if (PlayerPrefs.HasKey(PlayerPrefs.GetString(AssetBundleList[i].GameTag)))
                {
                    PlayerPrefs.DeleteKey(PlayerPrefs.GetString(AssetBundleList[i].GameTag));
                }
                if (PlayerPrefs.HasKey(AssetBundleList[i].GameTag))
                {
                    PlayerPrefs.DeleteKey(AssetBundleList[i].GameTag);
                }
                if (PlayerPrefs.HasKey(AssetBundleList[i].GameTag + "_Version"))
                {
                    PlayerPrefs.DeleteKey(AssetBundleList[i].GameTag + "_Version");
                }
            }
        }
    }

    public void LoadXmlFlie(string path)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(path);
        Debug.LogError("===读取Xml文件===" + path);
        XmlNodeList childNodes = xmlDocument.SelectSingleNode("GameList").ChildNodes;
        IEnumerator enumerator = childNodes.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                XmlElement xmlElement = (XmlElement)enumerator.Current;
                AssetBundleSystem assetBundleSystem = new AssetBundleSystem();
                assetBundleSystem.AssetType = int.Parse(xmlElement.GetAttribute("type"));
                assetBundleSystem.GameID = int.Parse(xmlElement.GetAttribute("gameid"));
                assetBundleSystem.GameName = xmlElement.GetAttribute("name");
                assetBundleSystem.GameTag = xmlElement.GetAttribute("tag");
                assetBundleSystem.Versions = (int)float.Parse(xmlElement.GetAttribute("versions"));
                assetBundleSystem.ScenesName = new string[xmlElement.ChildNodes.Count];
                for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
                {
                    XmlElement xmlElement2 = xmlElement.ChildNodes[i] as XmlElement;
                    assetBundleSystem.ScenesName[i] = xmlElement2.GetAttribute("SceneName");
                }
                AssetBundleList.Add(assetBundleSystem);
            }
        }
        finally
        {
            IDisposable disposable;
            if ((disposable = (enumerator as IDisposable)) != null)
            {
                disposable.Dispose();
            }
        }
    }

    public IEnumerator LoadABWeb(string tag, InnerGameItem item, string sceneName)
    {
        yield return null;
        Debug.LogError(ZH2_GVars.tagGame + "    " + tag);
        if (ZH2_GVars.isUnload && ZH2_GVars.tagGame == tag)
        {
            Debug.LogError("资源未卸载完毕,禁止开局");
            string str = string.Empty;
            switch (tag)
            {
                case "LuckyLion":
                    str = "幸运六狮 ";
                    goto IL_237;
                case "BCBM":
                    str = "奔驰宝马";
                    goto IL_237;
                case "MoneyFish":
                    str = "摇钱树 ";
                    goto IL_237;
                case "DanTiao":
                    str = "单挑 ";
                    goto IL_237;
                case "DemonKing":
                    str = "牛魔王 ";
                    goto IL_237;
                case "WaterMargin":
                    str = "水浒传 ";
                    goto IL_237;
                case "HaiW3":
                    str = "海王2 ";
                    goto IL_237;
                case "QQMermaid":
                    str = "QQ美人鱼 ";
                    goto IL_237;
                case "OverFish":
                    str = "李逵劈鱼 ";
                    goto IL_237;
                case "ToadFish":
                    str = "金蟾捕鱼 ";
                    goto IL_237;
                case "BZJXFish":
                    str = "金象送福 ";
                    goto IL_237;
                case "STWM":
                    str = "水浒传 ";
                    goto IL_237;
            }
            str = tag + " ";
        IL_237:
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip(str + "点击过快! 请稍后再试!", str + "Click too fast! Please try again later!", string.Empty), false, null);
        }

        else
        {
            AssetBundleSystem AssetGame = null;
            for (int i = 0; i < AssetBundleManager.AssetBundleList.Count; i++)
            {
                if (AssetBundleManager.AssetBundleList[i].GameTag == tag)
                {
                    AssetGame = AssetBundleManager.AssetBundleList[i];
                    break;
                }
            }
            string name = item.GetInnerGame().package;
            if (AssetGame != null)
            {
                int _Value = -1;
                int _IsOn = 0;
                if (PlayerPrefs.HasKey(name) && PlayerPrefs.HasKey(tag + "_Version"))
                {
                    _IsOn = PlayerPrefs.GetInt(name);
                    _Value = PlayerPrefs.GetInt(tag + "_Version");
                }
                Debug.LogError(string.Concat(new object[]
                {
                    "_IsOn: ",
                    _IsOn,
                    "  _Value: ",
                    _Value,
                    "  键值: ",
                    tag,
                    "_Version"
                }));
                if (_Value == AssetGame.Versions && AssetGame.SceneAsset != null)
                {
                    Debug.LogError(string.Concat(new object[]
                    {
                        "加载的标签: ",
                        tag,
                        " package: ",
                        item.GetInnerGame().package,
                        " 游戏名: ",
                        item.GetInnerGame().name_cn,
                        " rankType: ",
                        item.GetInnerGame().rankType,
                        " url: ",
                        item.GetInnerGame().url,
                        " version: ",
                        item.GetInnerGame().version,
                        " 场景名: ",
                        sceneName
                    }));
                    if (InnerGameManager.Get() != null && InnerGameManager.Get().objLoad != null)
                    {
                        InnerGameManager.Get().objLoad.SetActive(true);
                        Debug.LogError("打开遮挡");
                    }
                    else
                    {
                        Debug.LogError("打开遮挡失败");
                    }
                    ZH2_GVars.isFirstToDaTing = true;
                    ZH2_GVars.isGameToDaTing = false;
                    SceneManager.LoadScene(sceneName);
                    yield break;
                }
                string url = string.Empty;
                if (Application.platform == RuntimePlatform.Android)
                {
                    url = string.Concat(new string[]
                    {
                        InnerGameManager.urlInit,
                        "Android/",
                        tag,
                        "/",
                        tag.ToLower(),
                        ".unity3d"
                    });
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    url = string.Concat(new string[]
                    {
                        InnerGameManager.urlInit,
                        "iOS/",
                        tag,
                        "/",
                        tag.ToLower(),
                        ".unity3d"
                    });
                }
                else
                {
                    Debug.Log("平台：" + Application.platform);
                    url = string.Concat(new string[]
                    {
                        InnerGameManager.urlInit,
                        "iOS/",
                        tag,
                        "/",
                        tag.ToLower(),
                        ".unity3d"
                    });
                }
                Debug.Log("地址：" + url);
                WWW www = WWW.LoadFromCacheOrDownload(url, AssetGame.Versions);
                if (_IsOn != 1)
                {
                    item.ChangeState(InnerGameState.Downloading);
                    while (!www.isDone)
                    {
                        item.UpdateProgress(www.progress);
                        yield return new WaitForSeconds(0.02f);
                    }
                    item.UpdateProgress(www.progress);
                }
                yield return www;
                Debug.LogError("下载完成!");
                if (www.error != null)
                {
                    MonoBehaviour.print(www.error);
                }
                if (www.isDone && www.error == null && tag == AssetGame.GameTag)
                {
                    if (AssetGame.SceneAsset == null)
                    {
                        yield return new WaitForSeconds(0.1f);
                        AssetBundle assetbundle = www.assetBundle;
                        AssetGame.SceneAsset = assetbundle;
                    }
                    if (_IsOn != 1)
                    {
                        item.ChangeState(InnerGameState.OK);
                        _IsOn = 1;
                        _Value = AssetGame.Versions;
                        PlayerPrefs.SetString(tag, name);
                        PlayerPrefs.SetInt(tag + "_Version", _Value);
                        PlayerPrefs.SetInt(name, _IsOn);
                    }
                }
            }
            else
            {
                Debug.Log("空的？");
            }
        }
        yield break;
    }


    /*
    public IEnumerator LoadABWeb(string tag, InnerGameItem item, string sceneName)
    {
        if (ZH2_GVars.isUnload && ZH2_GVars.tagGame == tag)
        {
            Debug.LogError("资源未卸载完毕,禁止开局");
            string empty = string.Empty;
            string str;
            switch (tag)
            {
                case "LuckyLion":
                    str = "幸运六狮 ";
                    break;
                case "BCBM":
                    str = "奔驰宝马";
                    break;
                case "MoneyFish":
                    str = "摇钱树 ";
                    break;
                case "DanTiao":
                    str = "单挑 ";
                    break;
                case "DemonKing":
                    str = "牛魔王 ";
                    break;
                case "WaterMargin":
                    str = "水浒传 ";
                    break;
                case "HaiW3":
                    str = "海王2 ";
                    break;
                case "QQMermaid":
                    str = "QQ美人鱼 ";
                    break;
                case "OverFish":
                    str = "李逵劈鱼 ";
                    break;
                case "ToadFish":
                    str = "金蟾捕鱼 ";
                    break;
                case "BZJXFish":
                    str = "金象送福 ";
                    break;
                case "STWM":
                    str = "水浒传 ";
                    break;
                case "DNTianG":
                    str = "大闹天宫 ";
                    break;
                case "NLHD":
                    str = "龙虎斗 ";
                    break;
                default:
                    str = tag + " ";
                    break;
            }
            MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip(str + "点击过快! 请稍后再试!", str + "Click too fast! Please try again later!", string.Empty));
            yield break;
        }

        for (int i = 0; i < AssetBundleList.Count; i++)
        {
            if (tag == AssetBundleList[i].GameTag)
            {
                if (AssetBundleList[i].SceneAsset != null)
                {
                    InnerGameManager.Get().objLoad.SetActive(true);
                    SceneManager.LoadSceneAsync(sceneName);
                    yield break;
                }

            }
        }
        string name = item.GetInnerGame().package;
        //string url = Url01 + "/" + tag + "/" + tag.ToLower() + ".unity3d";
        string url = (Application.platform == RuntimePlatform.Android) ? (InnerGameManager.urlInit + "Android/" + tag + "/" + tag.ToLower() + ".unity3d") : (InnerGameManager.urlInit + "iOS/" + tag + "/" + tag.ToLower() + ".unity3d");
        WWW www = WWW.LoadFromCacheOrDownload(url, 1);
        if (PlayerPrefs.GetInt(name) != 1)
        {
            item.ChangeState(InnerGameState.Downloading);
            while (!www.isDone)
            {
                item.btn.interactable = false;
                item.UpdateProgress(www.progress);
                yield return new WaitForSeconds(0.02f);
            }
            item.UpdateProgress(www.progress);
        }
        else
            InnerGameManager.Get().objLoad.SetActive(true);
        yield return www;
        print("isDone");
        item.btn.interactable = true;
        if (www.error != null)
            print(www.error);
        if (www.isDone && www.error == null)
        {
            for (int i = 0; i < AssetBundleList.Count; i++)
            {
                if (tag == AssetBundleList[i].GameTag)
                {
                    if (AssetBundleList[i].SceneAsset == null)
                    {
                        AssetBundle assetbundle = www.assetBundle;
                        AssetBundleList[i].SceneAsset = assetbundle;
                    }
                    if (PlayerPrefs.GetInt(name) != 1)
                    {
                        item.ChangeState(InnerGameState.OK);
                        PlayerPrefs.SetInt(name, 1);
                    }
                    //SceneManager.LoadSceneAsync(sceneName);
                    break;
                }
            }
        }
    }
	*/

    public void UnloadAB(string tag)
    {
        ZH2_GVars.isUnload = true;
        ZH2_GVars.tagGame = tag;
        UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll<AudioClip>();
        Debug.LogError("音频: " + array.Length);
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != null)
            {
                Resources.UnloadAsset(array[i]);
            }
        }
        StartCoroutine(WaitUnload());
    }

    private IEnumerator WaitUnload()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.LogError("===卸载非大厅热更新资源===");
        UnityEngine.Object[] objAryTexture2D = Resources.FindObjectsOfTypeAll<Texture2D>();
        int tempNum = 0;
        for (int i = 0; i < objAryTexture2D.Length; i++)
        {
            if (objAryTexture2D[i] != null && !ZH2_GVars.IsObjectInTheArray(ZH2_GVars.daTingResourceName, objAryTexture2D[i].name) && !ZH2_GVars.IsObjectInTheArray(this.liuShiResourceName, objAryTexture2D[i].name))
            {
                Resources.UnloadAsset(objAryTexture2D[i]);
                if (tempNum > 2)
                {
                    yield return null;
                    tempNum = 0;
                }
                tempNum++;
            }
        }
        Resources.UnloadUnusedAssets();
        GC.Collect();
        yield return new WaitForSeconds(0.15f);
        ZH2_GVars.isUnload = false;
        ZH2_GVars.tagGame = string.Empty;
        Debug.LogError("===卸载完毕===");
        yield break;
    }

    public IEnumerator LoadABWeb(InnerGameItem item)
    {
        string name = item.GetInnerGame().package;
        string url = string.Concat(new string[]
        {
            InnerGameManager.urlInit,
            "/",
            name,
            "/",
            name.ToLower(),
            ".unity3d"
        });
        WWW www = WWW.LoadFromCacheOrDownload(url, 1);
        item.ChangeState(InnerGameState.Downloading);
        while (!www.isDone)
        {
            item.btn.interactable = false;
            item.UpdateProgress(www.progress);
            yield return new WaitForSeconds(0.02f);
        }
        item.UpdateProgress(www.progress);
        yield return www;
        for (int i = 0; i < AssetBundleManager.AssetBundleList.Count; i++)
        {
            MonoBehaviour.print(name + "," + AssetBundleManager.AssetBundleList[i].GameTag);
            if (name == AssetBundleManager.AssetBundleList[i].GameTag)
            {
                MonoBehaviour.print(AssetBundleManager.AssetBundleList[i].GameTag);
                if (AssetBundleManager.AssetBundleList[i].SceneAsset == null)
                {
                    AssetBundle assetBundle = www.assetBundle;
                    AssetBundleManager.AssetBundleList[i].SceneAsset = assetBundle;
                    item.ChangeState(InnerGameState.OK);
                    PlayerPrefs.SetInt(name, 1);
                }
                break;
            }
        }
        yield break;
    }
}
