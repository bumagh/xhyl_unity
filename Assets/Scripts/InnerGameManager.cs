using BestHTTP;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InnerGameManager : MonoBehaviour
{
	public static string urlInit = string.Empty;

	[SerializeField]
	private InnerGameConfig m_innerGameConfig;

	[SerializeField]
	private GameObject m_innerGamePrefab;

	[SerializeField]
	private GameObject m_innerGamePrefab_HaveName;

	private List<InnerGameItem> m_itemList;

	private Transform m_xformGrid;

	private bool isInit;

	public bool debug_Uninstall;

	private bool multiDown = true;

	private static int gameCount = 9;

	private string m_userName;

	private string m_pwd;

	private string m_ip;

	private string m_language;

	private string m_downDir;

	private static InnerGameManager m_instance;

	private List<InnerGameItem> m_downloadTaskList;

	private bool shouldCheckIOSGameStatus = true;

	private HTTPRequest m_httpRequest;

	private AssetBundleManager abm;

	public GameObject objLoad;

	public Button btnService;

	public List<GameObject> functionOpen = new List<GameObject>();

	private float time;

	private int luanguageTempInt = -1;

	private string[] shows = ZH2_GVars.XingLi;

	private Tweener tw;

	private float cellSizeXUP;

	private float spacingXUP;

	private float leftFl;

	private int m_itemListCount;

	private int showNum = -1;

	private int waitNum;

    public AndroidJavaObject jo;
    public AndroidJavaClass jc;

    private void Awake()
	{
        if (Application.platform == RuntimePlatform.Android)
        {
            this.jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            this.jo = this.jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        m_instance = this;
		m_itemList = new List<InnerGameItem>();
		m_xformGrid = base.transform.Find("Viewport/InnerGames");
		abm = AssetBundleManager.GetInstance();
		string @string = PlayerPrefs.GetString("downloadAndroid");
		if (@string.StartsWith("http"))
		{
			urlInit = @string;
		}
		UnityEngine.Debug.LogError("地址: " + urlInit);
	}

	private void Start()
	{
		MB_Singleton<NetManager>.Get().RegisterHandler("gameLoadAddress", HandleNetMsg_CheckGameVersion);
		if (btnService != null)
		{
		}
		if (!ZH2_GVars.isStartedFromGame)
		{
			Send_CheckGameVersion(101, "0");
		}
		Init();
		objLoad.SetActive(value: false);
	}

	private void Update()
	{
		if (luanguageTempInt != PlayerPrefs.GetInt("GVarsLanguage", 0))
		{
			luanguageTempInt = PlayerPrefs.GetInt("GVarsLanguage", 0);
			UnityEngine.Debug.LogError("检测到切换了语言");
			ChangeLanguage();
		}
		if (UnityEngine.Input.GetKey(KeyCode.W))
		{
			time += Time.deltaTime;
			if (time > 2f)
			{
				UnityEngine.Debug.LogError("清除");
				PlayerPrefs.SetString("假加载", "你大爷");
				PlayerPrefs.DeleteAll();
				PlayerPrefs.Save();
				time = 0f;
			}
		}
		else
		{
			time = 0f;
		}
	}

	private void ChakGameState()
	{
	}

	public static InnerGameManager Get()
	{
		return m_instance;
	}

	private void DeleteKey()
	{
		for (int i = 0; i < ZH2_GVars.orders.Length; i++)
		{
			int id = m_itemList[ZH2_GVars.orders[i] - 1].GetInnerGame().id;
			string text = id + "_SavedSize";
			string text2 = id + "_TotalSize";
			PlayerPrefs.DeleteKey(text);
			PlayerPrefs.DeleteKey(text2);
			UnityEngine.Debug.Log($"deleteKey:savedKey:{text},totalKey:{text2}");
		}
	}

	public void CleanCache()
	{
		Caching.ClearCache();
		ResetGameKey();
	}

	private void ResetGameKey()
	{
		for (int i = 0; i < ZH2_GVars.orders.Length; i++)
		{
			InnerGame innerGame = m_innerGameConfig.list[ZH2_GVars.orders[i]];
			PlayerPrefs.SetInt(innerGame.package, 0);
		}
	}

	public void Init()
	{
		if (isInit)
		{
			return;
		}
		//isInit = true;
		//m_instance = this;
		//m_userName = string.Empty;
		//m_pwd = string.Empty;
		//m_ip = string.Empty;
		//m_language = "0";
		//m_downDir = Application.persistentDataPath + "/download/";
		//if (Application.platform == RuntimePlatform.WindowsEditor)
		//{
		//	m_downDir = Path.GetFullPath(Application.dataPath + "/../download/").Replace('\\', '/');
		//}
		//if (!Directory.Exists(m_downDir))
		//{
		//	Directory.CreateDirectory(m_downDir);
		//}
		//m_downloadTaskList = new List<InnerGameItem>();
		//int[] orders = ZH2_GVars.orders;
		//string str = string.Empty;
		//for (int i = 0; i < m_innerGameConfig.list.Count; i++)
		//{
		//	InnerGame innerGame = m_innerGameConfig.list[orders[i]];
		//	Debug.Log(i+" : "+m_innerGameConfig.list[orders[i]].name_cn);
		//	InnerGameItem innerGameItem = CreateGameItem(innerGame);
		//	innerGameItem.Init(innerGame);
		//	str = str + "   " + innerGameItem.GetInnerGame().name_cn;

		//	innerGameItem.transform.Find("State").gameObject.SetActive(ZH2_GVars.IsObjectInTheArray(ZH2_GVars.ShowDown, innerGameItem.GetInnerGame().name_cn));

		//	if (innerGame.isOurGame)
		//	{
		//		innerGame.state = ((PlayerPrefs.GetInt(innerGame.package) == 1) ? InnerGameState.OK : InnerGameState.Default);
		//		innerGameItem.ChangeState(innerGame.state);
		//	}
		//	m_itemList.Add(innerGameItem);
		//}
		//FindAllObjectsInScene.InitAllTL(m_xformGrid);
		//FindAllObjectsInScene.RefreshAllTxt();
	}

	private void ChangShowObject()
	{
		shows = ZH2_GVars.XingLi;
		if (ZH2_GVars.isShowSelectionPanel)
		{
			switch (PlayerPrefs.GetInt(ZH2_GVars.selectionNum, 0))
			{
			case 0:
				shows = ZH2_GVars.ReMen;
				break;
			case 1:
				shows = ZH2_GVars.XingLi;
				break;
			case 2:
				shows = ZH2_GVars.ShiXun;
				break;
			case 3:
				shows = ZH2_GVars.DianZi;
				break;
			case 4:
				shows = ZH2_GVars.QiPai;
				break;
			case 5:
				shows = ZH2_GVars.TiYu;
				break;
			default:
				shows = ZH2_GVars.ReMen;
				break;
			}
		}
		for (int i = 0; i < m_itemList.Count; i++)
		{
			m_itemList[i].gameObject.SetActive(ZH2_GVars.IsObjectInTheArray(shows, m_itemList[i].GetInnerGame().name_cn));
			if (m_itemList[i].gameObject.activeInHierarchy)
			{
				m_itemList[i].transform.localScale = Vector3.zero;
				tw = m_itemList[i].transform.DOScale(Vector3.one, 0.3f);
			}
		}
		SizeDelta();
	}

	private void OnEnable()
	{
		showNum = -1;
		waitNum = 0;
		objLoad.SetActive(value: false);
		StartCoroutine(WaitEnable());
		CheckFunctionOpen();
	}

	private IEnumerator WaitEnable()
	{
		UnityEngine.Debug.LogError("====开始检查大厅资源1======");
		while (waitNum < 1)
		{
			if (!ZH2_GVars.isUnload)
			{
				UnityEngine.Debug.LogError("====开始检查大厅资源2====");
				yield return new WaitForSeconds(0.15f);
				ZH2_GVars.daTingResourceName = new List<string>();
				UnityEngine.Object[] objAry = Resources.FindObjectsOfTypeAll<Texture2D>();
				for (int i = 0; i < objAry.Length; i++)
				{
					if (objAry[i] != null)
					{
						ZH2_GVars.daTingResourceName.Add(objAry[i].name);
					}
				}
				waitNum = 2;
				UnityEngine.Debug.LogError("====停止检查大厅资源");
			}
			yield return new WaitForSeconds(0.5f);
		}
		SetInit();
	}

	private void SetInit()
	{
		if (STOF_GameInfo.getInstance() != null)
		{
			STOF_GameInfo.ClearGameInfo();
		}
		if (BZJX_GameInfo.getInstance() != null)
		{
			BZJX_GameInfo.ClearGameInfo();
		}
		if (JSYS_LL_GameInfo.getInstance() != null)
		{
			JSYS_LL_GameInfo.ClearGameInfo();
		}
		if (STMF_GameInfo.getInstance() != null)
		{
			STMF_GameInfo.ClearGameInfo();
		}
		if (BCBM_GameInfo.getInstance() != null)
		{
			BCBM_GameInfo.ClearGameInfo();
		}
	}

	private void LateUpdate()
	{
		if (PlayerPrefs.GetInt(ZH2_GVars.selectionNum, 0) != showNum)
		{
			showNum = PlayerPrefs.GetInt(ZH2_GVars.selectionNum, 0);
			ChangShowObject();
		}
	}

	private void SizeDelta()
	{
		m_itemListCount = 0;
		m_xformGrid.GetComponent<GridLayoutGroup>().enabled = false;
		m_xformGrid.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		for (int i = 0; i < m_xformGrid.childCount; i++)
		{
			if (m_xformGrid.GetChild(i).gameObject.activeInHierarchy)
			{
				m_itemListCount++;
			}
		}
		Vector2 cellSize = m_xformGrid.GetComponent<GridLayoutGroup>().cellSize;
		cellSizeXUP = cellSize.x;
		Vector2 spacing = m_xformGrid.GetComponent<GridLayoutGroup>().spacing;
		spacingXUP = spacing.x;
		leftFl = m_xformGrid.GetComponent<GridLayoutGroup>().padding.left;
		m_xformGrid.GetComponent<RectTransform>().sizeDelta = Vector2.right * (cellSizeXUP + spacingXUP) * Mathf.CeilToInt((float)m_itemListCount / 2f) + Vector2.right * leftFl;
		m_xformGrid.GetComponent<GridLayoutGroup>().enabled = true;
	}

	public void UpdateGameRunStatus(object args)
	{
		CheckStatus(args);
	}

	public void UpdateFunctionOpen(object args)
	{
		ZH2_GVars.functionOpen = new Dictionary<string, object>();
		ZH2_GVars.functionOpen = (args as Dictionary<string, object>);
		CheckFunctionOpen();
	}

	private void CheckFunctionOpen()
	{
		if (ZH2_GVars.functionOpen == null)
		{
			UnityEngine.Debug.LogError("CheckFunctionOpen为空");
			return;
		}
		for (int i = 0; i < functionOpen.Count; i++)
		{
			string name = functionOpen[i].name;
			if (ZH2_GVars.functionOpen.ContainsKey(name) && ZH2_GVars.functionOpen[name].ToString() != string.Empty && ZH2_GVars.functionOpen[name].ToString() != "null")
			{
				int num = int.Parse(ZH2_GVars.functionOpen[name].ToString());
				functionOpen[i].SetActive(num != 2);
			}
		}
	}

	private void CheckStatus(object args)
	{
		for (int i = 0; i < m_innerGameConfig.list.Count; i++)
		{
			InnerGame innerGame = m_innerGameConfig.list[i];
			string runStatusKey = innerGame.RunStatusKey;
			if (((Dictionary<string, object>)args).ContainsKey(runStatusKey))
			{
				innerGame.RunStatus = (int)((Dictionary<string, object>)args)[runStatusKey];
			}
			else
			{
				innerGame.RunStatus = 0;
			}
		}
	}

	private void ChangeLanguage()
	{
		int num = 0;
		InnerGame innerGame;
		while (true)
		{
			if (num < m_itemList.Count)
			{
				innerGame = m_itemList[num].GetInnerGame();
				if (innerGame == null)
				{
					break;
				}
				m_itemList[num].SetLanguage(innerGame);
				num++;
				continue;
			}
			return;
		}
		UnityEngine.Debug.LogError(innerGame.name_cn + "为空");
	}

	public InnerGameItem CreateGameItem(InnerGame innerGame)
	{
		GameObject gameObject = null;
		//gameObject = ((!(innerGame.spiName == null)) ? Instantiate(m_innerGamePrefab_HaveName) : Instantiate(m_innerGamePrefab));
		gameObject = (innerGame.spine == null) ? Instantiate(m_innerGamePrefab_HaveName) : Instantiate(m_innerGamePrefab);
        gameObject.name = innerGame.name_cn;
		gameObject.transform.SetParent(m_xformGrid, worldPositionStays: false);
		gameObject.SetActive(value: true);
		return gameObject.GetComponent<InnerGameItem>();
	}

	public void CheckGameStatus(InnerGameItem item)
	{
	}

	public IEnumerator CheckGameStatus_Lua(InnerGameItem item)
	{
		InnerGame innerGame = item.GetInnerGame();
		int id = innerGame.id;
		string url = innerGame.url;
		UnityEngine.Debug.Log("CheckGameStatus_Lua: " + innerGame.id);
		string action = innerGame.action;
		string text = ZH2_GVars.DataPath + action + "/" + ZH2_GVars.PlatformDir + "/";
		UnityEngine.Debug.Log("ABSaveDir: " + text);
		string text2 = text + "files.txt";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string empty = string.Empty;
		if (!File.Exists(text2))
		{
			item.ChangeState(InnerGameState.Default);
			UnityEngine.Debug.LogFormat("ABIndexFilePath:{0} not exsit", text2);
			yield break;
		}
		string text3 = File.ReadAllText(text2, Encoding.UTF8);
		UnityEngine.Debug.LogFormat("listContent:{0}", text3);
		string[] array = text3.Split('\n');
		UnityEngine.Debug.LogFormat("listItems.Length:{0}", array.Length);
		string[] array2 = array[0].Split('#');
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(Convert.ToInt64(array2[2]));
		UnityEngine.Debug.LogFormat("version:{0}, filecount:{1}, time:{2}", array2[0], array2[1], dateTime);
		innerGame.version = array2[0];
		for (int i = 1; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array3 = array[i].Split('|');
				string str = array3[0];
				string text4 = (text + str).Trim();
				string directoryName = Path.GetDirectoryName(text4);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				if (!File.Exists(text4))
				{
					item.ChangeState(InnerGameState.Default);
					UnityEngine.Debug.LogFormat("outfile:{0} not exsit", text4);
					yield break;
				}
			}
		}
		item.ChangeState(InnerGameState.OK);
	}

    public void StartGame(InnerGameItem item)
    {
        if (item.GetInnerGame().isOurGame)
        {
            switch (item.GetInnerGame().id)
            {
                case 1:
                    StartCoroutine(abm.LoadABWeb("LuckyLion", item, "LL_Game"));
                    break;
                case 2:
                    StartCoroutine(abm.LoadABWeb("MoneyFish", item, "STMF_UIScene"));
                    break;
                case 3:
                    StartCoroutine(abm.LoadABWeb("DanTiao", item, "STDT_Game"));
                    break;
                case 4:
                    StartCoroutine(abm.LoadABWeb("DemonKing", item, "STDK_UIScene"));
                    break;
                case 8:
                    StartCoroutine(abm.LoadABWeb("WaterMargin", item, "STWM_Ui"));
                    break;
                case 21:
                    StartCoroutine(abm.LoadABWeb("OverFish", item, "STOF_UIScene"));
                    break;
                case 38:
                    StartCoroutine(abm.LoadABWeb("ToadFish", item, "STTF_UIScene"));
                    break;
                case 39:
                    StartCoroutine(abm.LoadABWeb("BZJXFish", item, "BZJX_UIScene"));
                    break;
                case 31:
                    StartCoroutine(abm.LoadABWeb("DreamPark", item, "DP_LoadScene"));
                    break;
                case 40:
                    StartCoroutine(abm.LoadABWeb("HeiBao", item, "PTM_Load"));
                    break;
                case 41:
                    StartCoroutine(abm.LoadABWeb("3DBaiJiaLe", item, "BaiJiaLe_ChooseRoom"));
                    break;
                case 42:
                    StartCoroutine(abm.LoadABWeb("HuoFengHuang", item, "HfhLoad"));
                    break;
                case 43:
                    StartCoroutine(abm.LoadABWeb("JSYS", item, "JSYS_UIScene"));
                    break;
                case 44:
                    StartCoroutine(abm.LoadABWeb("PharaohGame", item, "PHG_Load"));
                    break;
                case 45:
                    StartCoroutine(abm.LoadABWeb("EasterSurprise", item, "ESP_Load"));
                    break;
                case 46:
                    StartCoroutine(abm.LoadABWeb("SafariHeart", item, "SHT_Load"));
                    break;
                case 47:
                    StartCoroutine(abm.LoadABWeb("UnderseaWorld", item, "USW_Load"));
                    break;
                case 48:
                    StartCoroutine(abm.LoadABWeb("WomanHeaven", item, "WHN_Load"));
                    break;
                case 49:
                    StartCoroutine(abm.LoadABWeb("LongtengRoars", item, "LRS_Load"));
                    break;
                case 20:
                    StartCoroutine(abm.LoadABWeb("CSF", item, "CSF_Load"));
                    break;
                case 50:
                    StartCoroutine(abm.LoadABWeb("LKB", item, "LKB_Load"));
                    break;
                case 51:
                    StartCoroutine(abm.LoadABWeb("NvXia", item, "MSE_Load"));
                    break;
                case 52:
                    StartCoroutine(abm.LoadABWeb("BCBM", item, "BCBM_Game"));
                    break;
                case 60:
                    StartCoroutine(abm.LoadABWeb("DaZiBan", item, "DzbLoad"));
                    break;
                case 70:
                    StartCoroutine(abm.LoadABWeb("Lreland", item, "LLD_Load"));
                    break;
                case 77:
                    StartCoroutine(abm.LoadABWeb("HaiW2", item, "FK3_DaTing"));
                    break;
                case 79:
                    StartCoroutine(abm.LoadABWeb("HaiW3", item, "LHW_DaTing"));
                    break;
                case 80:
                    StartCoroutine(abm.LoadABWeb("Sparta", item, "SPA_Load"));
                    break;
                case 90:
                    StartCoroutine(abm.LoadABWeb("CherryLove", item, "CRL_Load"));
                    break;
                case 91:
                    StartCoroutine(abm.LoadABWeb("DolphinReef", item, "DPR_Load"));
                    break;
                case 5:
                    StartCoroutine(abm.LoadABWeb("QQMermaid", item, "STQM_UIScene"));
                    break;
                case 88:
                    StartCoroutine(abm.LoadABWeb("RollDownHouse", item, "DCDF_Game"));
                    break;
                case 99:
                    StartCoroutine(abm.LoadABWeb("JsYs", item, "JSYSLoad1"));
                    break;
                case 92:
                    StartCoroutine(abm.LoadABWeb("DNTianG", item, "DNTG_UIScene"));
                    break;
                case 103:
                    StartCoroutine(abm.LoadABWeb("NLHD", item, "LHD_Scene"));
                    break;
            }
        }
    }

    public void StartApp_Android(InnerGameItem item)
	{
        InnerGame innerGame = item.GetInnerGame();
        string path = this.m_downDir + innerGame.id + ".apk";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        string text = "startApp";
        string text2 = "userMessage";
        string username = ZH2_GVars.username;
        string pwd = ZH2_GVars.pwd;
        string language = ZH2_GVars.language_enum.Equals("zh") ? "0" : "1";
        string ipaddress_Game = ZH2_GVars.IPAddress_Game;
        string action = innerGame.action;
        string text3 = this.jo.Call<string>("getClassSimpleName", new object[0]);
        string text4 = this.jo.Call<string>("getClassFullName", new object[0]);
        string identifier = Application.identifier;
        Debug.LogError("应用程序包标识符: " + Application.identifier);
        Debug.LogError("action: " + innerGame.action);
        Debug.LogError("获得包名: " + this.jo.Call<string>("getPackageName", new object[0]));
        Debug.LogError("获取简单类名: " + this.jo.Call<string>("getClassSimpleName", new object[0]));
        Debug.LogError("获取类全名: " + this.jo.Call<string>("getClassFullName", new object[0]));
        string text5 = this.GenerateStartAppValue_Android(username, pwd, language, ipaddress_Game, identifier, string.Empty, false);
        Debug.Log(string.Format("value: [{0}]", text5));
        this.jo.Call(text, new object[]
        {
            action,
            text2,
            text5,
            true
        });
    }

	public void StartApp_IOS(InnerGameItem item)
	{
		UnityEngine.Debug.Log("StartApp_IOS");
		InnerGame innerGame = item.GetInnerGame();
		string username = ZH2_GVars.username;
		string pwd = ZH2_GVars.pwd;
		string text = ZH2_GVars.language_enum.Equals("zh") ? "0" : "1";
		string iPAddress = ZH2_GVars.IPAddress;
		UnityEngine.Debug.Log(username);
		UnityEngine.Debug.Log(pwd);
		UnityEngine.Debug.Log(text);
		UnityEngine.Debug.Log(iPAddress);
		string iPAddress_Game = ZH2_GVars.IPAddress_Game;
		string str = GenerateStartAppValue_IOS(iPAddress_Game, username, pwd, text, iPAddress, string.Empty, string.Empty);
		string schema = innerGame.schema;
		string text2 = schema + str;
		UnityEngine.Debug.Log(text2);
		UnityEngine.Debug.Log(WWW.EscapeURL(text2));
		Application.OpenURL(text2);
		Application.Quit();
	}

	public void InstallGame(string path)
	{
	}

	public void UninstallGame(InnerGameItem item)
	{
		InnerGame innerGame = item.GetInnerGame();
		if (Application.platform != RuntimePlatform.Android || !item.GetInnerGame().isLuaGame)
		{
		}
		if (item.GetInnerGame().isLuaGame)
		{
			string action = innerGame.action;
			string path = ZH2_GVars.DataPath + action + "/" + ZH2_GVars.PlatformDir + "/";
			if (Directory.Exists(path))
			{
				try
				{
					Directory.Delete(path, recursive: true);
					UnityEngine.Debug.LogFormat("delete [id:{0}, name:{1}] success", innerGame.id, innerGame.name_cn);
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
				}
			}
		}
		item.ChangeState(InnerGameState.Default);
	}

	public void Download(InnerGameItem item)
	{
		InnerGame innerGame = item.GetInnerGame();
		int id = innerGame.id;
		string url = innerGame.url;
		UnityEngine.Debug.LogFormat("Download: {0}", id);
		if (Application.platform != RuntimePlatform.Android && !innerGame.isLuaGame)
		{
			Application.OpenURL(innerGame.url);
			return;
		}
		string text = m_downDir + id + ".apk";
		if (string.IsNullOrEmpty(url))
		{
			UnityEngine.Debug.LogErrorFormat("empty url");
			return;
		}
		HTTPRequest hTTPRequest = m_httpRequest = new HTTPRequest(new Uri(url), DownloadCallback);
		item.SetHTTPRequest(hTTPRequest);
		string key = id + "_SavedSize";
		bool flag = true;
		if (PlayerPrefs.HasKey(key) && File.Exists(text))
		{
			FileInfo fileInfo = new FileInfo(text);
			int @int = PlayerPrefs.GetInt(key);
			if (fileInfo != null)
			{
				UnityEngine.Debug.LogFormat("fileInfo.Length:{0}, savedSize:{1}", fileInfo.Length, @int);
				if (fileInfo.Length == @int)
				{
					flag = false;
				}
			}
		}
		if (!flag)
		{
			UnityEngine.Debug.Log($"恢复下载游戏[id:{id},name:{innerGame.name_cn}] total:{innerGame.totalSize}, progress:{innerGame.progress:0.00}, saved: {innerGame.savedSize}");
			hTTPRequest.SetRangeHeader(PlayerPrefs.GetInt(key));
		}
		else
		{
			File.Delete(text);
			UnityEngine.Debug.Log($"下载游戏[id:{id},name:{innerGame.name_cn},url:{innerGame.url}]");
			PlayerPrefs.SetInt(key, 0);
		}
		hTTPRequest.DisableCache = true;
		hTTPRequest.UseStreaming = true;
		hTTPRequest.StreamFragmentSize = 1048576;
		hTTPRequest.Tag = item;
		hTTPRequest.OnProgress = OnDownloadProgress;
		hTTPRequest.Send();
		item.lastDownloaded = 0;
		item.lastProgressTime = 0f;
	}

	public IEnumerator Download_AB(InnerGameItem item)
	{
		InnerGame game = item.GetInnerGame();
		int id = game.id;
		string url = game.url;
		UnityEngine.Debug.LogError("下载AB: " + game.id);
		UnityEngine.Debug.LogFormat("url:{2}, length:{0}, index:{1}", url.Length, url.LastIndexOf('/', url.Length - 1), url);
		string baseUrl = url.Substring(0, url.LastIndexOf('/', url.Length - 1) + 1);
		UnityEngine.Debug.Log("baseUrl: " + baseUrl);
		string gameName = game.action;
		string ABSaveDir = ZH2_GVars.DataPath + gameName + "/" + ZH2_GVars.PlatformDir + "/";
		UnityEngine.Debug.Log("ABSaveDir: " + ABSaveDir);
		string ABIndexFilePath = ABSaveDir + "files.txt";
		string listUrl = baseUrl + "files.txt";
		if (!Directory.Exists(ABSaveDir))
		{
			Directory.CreateDirectory(ABSaveDir);
		}
		string empty = string.Empty;
		if (File.Exists(ABIndexFilePath))
		{
			File.Delete(ABIndexFilePath);
		}
		string listContent;
		if (!File.Exists(ABIndexFilePath))
		{
			WWW wwwList = new WWW(listUrl);
			yield return wwwList;
			if (wwwList.error != null)
			{
				UnityEngine.Debug.LogError("wwwList cannot download");
				yield break;
			}
			File.WriteAllBytes(ABIndexFilePath, wwwList.bytes);
			listContent = wwwList.text;
		}
		else
		{
			listContent = File.ReadAllText(ABIndexFilePath, Encoding.UTF8);
		}
		UnityEngine.Debug.LogFormat("listContent:{0}", listContent);
		string[] listItems = listContent.Split('\n');
		UnityEngine.Debug.LogFormat("listItems.Length:{0}", listItems.Length);
		string[] publishInfos = listItems[0].Split('#');
		DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(Convert.ToInt64(publishInfos[2]));
		UnityEngine.Debug.LogFormat("version:{0}, filecount:{1}, time:{2}", publishInfos[0], publishInfos[1], time);
		game.version = publishInfos[0];
		for (int i = 1; i < listItems.Length; i++)
		{
			if (!string.IsNullOrEmpty(listItems[i]))
			{
				string[] keyValue = listItems[i].Split('|');
				string f = keyValue[0];
				string outfile = (ABSaveDir + f).Trim();
				string path = Path.GetDirectoryName(outfile);
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				string fileUrl = baseUrl + f;
				if (File.Exists(outfile))
				{
					File.Delete(outfile);
				}
				WWW www2 = new WWW(fileUrl);
				yield return www2;
				if (www2.isDone)
				{
					File.WriteAllBytes(outfile, www2.bytes);
				}
				if (www2.error != null)
				{
					UnityEngine.Debug.LogErrorFormat("file item [{0}] cannot download", f);
				}
			}
		}
		item.ChangeState(InnerGameState.OK);
	}

	private void AddToWaitDLTask(InnerGameItem gameItem)
	{
		m_downloadTaskList.Add(gameItem);
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

	private bool IsDownloading()
	{
		return m_httpRequest != null;
	}

	public void OnBtnClick_GameItem(InnerGameItem item)
	{
		InnerGame innerGame = item.GetInnerGame();
		int id = innerGame.id;
		ZH2_GVars.language = ((ZH2_GVars.language_enum != 0) ? "en" : "zh");
		if (ZH2_GVars.IsObjectInTheArray(ZH2_GVars.HotUpdatesGameID, id))
		{
            Debug.LogError(string.Concat(new object[]
              {
                "加载热更新游戏: ",
                item.name,
                " id: ",
                id
              }));
            this.StartGame(item);
            return;
        }
		 if (id == 17)
		{
            this.Send_CheckGameVersion(102, "0");
            return;
        }
		 if (innerGame.state != InnerGameState.Default)
		{
			if (innerGame.state == InnerGameState.ReadyInstall)
			{
                Debug.Log("InnerGameState.ReadyInstall");
                string text = id + "_SavedSize";
                string text2 = id + "_TotalSize";
                string text3 = this.m_downDir + id + ".apk";
                Debug.Log(string.Format("key_savedSize:{0},key_totalSize:{1}", PlayerPrefs.HasKey(text), PlayerPrefs.HasKey(text2)));
                Debug.Log(id + ".apk是否存在:" + File.Exists(text3));
                if (PlayerPrefs.HasKey(text) && PlayerPrefs.HasKey(text2))
                {
                    int @int = PlayerPrefs.GetInt(text2);
                    int int2 = PlayerPrefs.GetInt(text);
                    if (File.Exists(text3))
                    {
                        FileInfo fileInfo = new FileInfo(text3);
                        Debug.Log(string.Format("APKExist totalSize:{0},savedSize:{1},realSize:{2}", @int, int2, fileInfo.Length));
                        if ((long)@int == fileInfo.Length)
                        {
                            this.InstallGame(text3);
                            Debug.Log("InstallGame: " + id);
                            item.ChangeState(InnerGameState.OK);
                            return;
                        }
                    }
                    else
                    {
                        Debug.Log("File Not Exists!");
                    }
                }
            }
			else if (innerGame.state == InnerGameState.Waiting)
			{
                Debug.Log(string.Format("游戏[id:{0},name:{1}] 已经存在于下载列表", innerGame.id, innerGame.name_cn));
                MB_Singleton<AlertDialog>.Get().ShowDialog(string.Format((ZH2_GVars.language_enum != ZH2_GVars.LanguageType.English) ? ((ZH2_GVars.language_enum != ZH2_GVars.LanguageType.Chinese) ? "เกม[{0}]มีอยู่แล้วในรายการดาวน์โหลด" : "游戏[{0}]已经存在于下载列表") : "The [{0}] has been in the download list", (ZH2_GVars.language_enum != ZH2_GVars.LanguageType.Chinese) ? innerGame.name_en : innerGame.name_cn), false, null);
            }
			else if (innerGame.state == InnerGameState.Downloading)
			{
                Debug.Log(string.Format("游戏[id:{0},name:{1}] 正在下载中", innerGame.id, innerGame.name_cn));
                if (!innerGame.isLuaGame)
                {
                    MB_Singleton<AlertDialog>.Get().ShowDialog(string.Format((ZH2_GVars.language_enum != ZH2_GVars.LanguageType.English) ? ((ZH2_GVars.language_enum != ZH2_GVars.LanguageType.Chinese) ? "จะให้หยุดหรือไม่ [{0}]ดาวน์โหลดแล้ว?" : "是否暂停[{0}]的下载") : "Stop downloading the [{0}]？", (ZH2_GVars.language_enum != ZH2_GVars.LanguageType.Chinese) ? innerGame.name_en : innerGame.name_cn), true, delegate
                    {
                        HTTPRequest httprequest = item.GetHTTPRequest();
                        if (httprequest != null && httprequest.State < HTTPRequestStates.Finished)
                        {
                            httprequest.OnProgress = null;
                            httprequest.Callback = null;
                            httprequest.Abort();
                        }
                        item.SetHTTPRequest(null);
                        item.ChangeState(InnerGameState.Paused);
                    });
                }
            }
			else if (innerGame.state == InnerGameState.Installing)
			{
                Debug.Log(string.Format("游戏[id:{0},name:{1}] 正在安装中", innerGame.id, innerGame.name_cn));
            }
			else if (innerGame.state == InnerGameState.OK)
			{
                Debug.Log(string.Format("游戏[id:{0},name:{1}] 可以运行", innerGame.id, innerGame.name_cn));
                Debug.Log(LogHelper.Red(string.Concat(new object[]
                {
                    "game.name_cn: ",
                    innerGame.name_cn,
                    " game.id: ",
                    innerGame.id,
                    "  game.RunStatus: ",
                    innerGame.RunStatus
                })));
                if (innerGame.RunStatus == 0)
                {
                    MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("敬请期待", "The game is not yet open", "ยังไม่เปิดให้เล่น", "Mong chờ"), false, null);
                    return;
                }
                if (ZH2_GVars.ScoreOverflow)
                {
                    MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum != ZH2_GVars.LanguageType.English) ? ((ZH2_GVars.language_enum != ZH2_GVars.LanguageType.Chinese) ? "กรุณารอให้เกมจบก่อน" : "请等待上局游戏结束") : "Please wait for the end of a game", false, null);
                    return;
                }
                if (ZH2_GVars.user.overflow == 1)
                {
                    MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum != ZH2_GVars.LanguageType.English) ? ((ZH2_GVars.language_enum != ZH2_GVars.LanguageType.Chinese) ? "กรุณาพิมพ์เลขบัญชีของคุณให้ตรงเวลาด้วย" : "您的帐号已爆机，请及时兑奖，否则无法正常游戏") : "Your account is too high,please go to the reception desk and redeem prizes", false, null);
                    return;
                }
                string version = string.Empty;
                if (!innerGame.isLuaGame)
                {
                    this.CheckGameStatus(item);
                }
                if (Application.platform == RuntimePlatform.Android)
                {
                    version = innerGame.version;
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    version = "-2";
                }
                else
                {
                    version = "-2";
                }
                if (innerGame.isLuaGame)
                {
                    version = innerGame.version;
                }
                this.Send_CheckGameVersion(innerGame.id, version);
            }
			else if (innerGame.state == InnerGameState.NeedUpdate)
			{
                Debug.Log(string.Format("游戏[id:{0},name:{1}] 需要更新", innerGame.id, innerGame.name_cn));
            }
			else if (innerGame.state == InnerGameState.Paused)
			{
                Debug.Log(string.Format("游戏[id:{0},name:{1}] 准备恢复下载", innerGame.id, innerGame.name_cn));
                string version2 = "-1";
                this.Send_CheckGameVersion(innerGame.id, version2);
                return;
            }
            return;
        }
        Debug.Log(LogHelper.Red(string.Concat(new object[]
            {
            "game.name_cn: ",
            innerGame.name_cn,
            " game.id: ",
            innerGame.id,
            "  game.RunStatus: ",
            innerGame.RunStatus
            })));
        if (innerGame.RunStatus == 0)
        {
            MB_Singleton<AlertDialog>.Get().ShowDialog(ZH2_GVars.ShowTip("敬请期待", "The game is not yet open", "ยังไม่เปิดให้เล่น", "Mong chờ"), false, null);
            return;
        }
        string version3 = "-1";
        this.Send_CheckGameVersion(innerGame.id, version3);
    }

	private void DownloadCallback(HTTPRequest req, HTTPResponse resp)
	{
		InnerGameItem innerGameItem = req.Tag as InnerGameItem;
		InnerGame innerGame = innerGameItem.GetInnerGame();
		int id = innerGame.id;
		string text = m_downDir + id + ".apk";
		UnityEngine.Debug.Log($"state:{req.State}");
		string key = id + "_TotalSize";
		string key2 = id + "_SavedSize";
		switch (req.State)
		{
		case HTTPRequestStates.Processing:
		{
			if (!PlayerPrefs.HasKey(key))
			{
				string firstHeaderValue = resp.GetFirstHeaderValue("content-length");
				if (!string.IsNullOrEmpty(firstHeaderValue))
				{
					int num = int.Parse(firstHeaderValue);
					PlayerPrefs.SetInt(key, num);
					UnityEngine.Debug.Log($"totalSize:{firstHeaderValue}");
					UnityEngine.Debug.Log(text);
					innerGame.totalSize = num;
				}
			}
			List<byte[]> streamedFragments2 = resp.GetStreamedFragments();
			using (FileStream fileStream2 = new FileStream(text, FileMode.Append))
			{
				foreach (byte[] item in streamedFragments2)
				{
					fileStream2.Write(item, 0, item.Length);
					innerGame.savedSize += item.Length;
				}
			}
			PlayerPrefs.SetInt(key2, innerGame.savedSize);
			innerGameItem.UpdateProgress();
			break;
		}
		case HTTPRequestStates.Finished:
			if (resp.IsSuccess)
			{
				if (resp.IsStreamingFinished)
				{
					List<byte[]> streamedFragments = resp.GetStreamedFragments();
					UnityEngine.Debug.Log(streamedFragments == null);
					if (streamedFragments != null)
					{
						using (FileStream fileStream = new FileStream(text, FileMode.Append))
						{
							foreach (byte[] item2 in streamedFragments)
							{
								fileStream.Write(item2, 0, item2.Length);
								innerGame.savedSize += item2.Length;
							}
						}
					}
					m_httpRequest = null;
					innerGameItem.SetHTTPRequest(null);
					InstallGame(text);
					UnityEngine.Debug.LogFormat("保存地址:{0}", text);
					innerGameItem.ChangeState(InnerGameState.OK);
				}
			}
			else
			{
				string message5 = $"Request finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}";
				UnityEngine.Debug.Log(message5);
				m_httpRequest = null;
				innerGameItem.SetHTTPRequest(null);
				ReDownload(innerGameItem);
			}
			break;
		case HTTPRequestStates.Error:
		{
			string message4 = "Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
			UnityEngine.Debug.LogError(message4);
			m_httpRequest = null;
			innerGameItem.SetHTTPRequest(null);
			ReDownload(innerGameItem);
			break;
		}
		case HTTPRequestStates.Aborted:
		{
			string message3 = "Request Aborted!";
			UnityEngine.Debug.Log(message3);
			m_httpRequest = null;
			innerGameItem.SetHTTPRequest(null);
			break;
		}
		case HTTPRequestStates.ConnectionTimedOut:
		{
			string message2 = "Connection Timed Out!";
			UnityEngine.Debug.LogError(message2);
			m_httpRequest = null;
			innerGameItem.SetHTTPRequest(null);
			ReDownload(innerGameItem);
			break;
		}
		case HTTPRequestStates.TimedOut:
		{
			string message = "Processing the request Timed Out!";
			UnityEngine.Debug.LogError(message);
			m_httpRequest = null;
			innerGameItem.SetHTTPRequest(null);
			ReDownload(innerGameItem);
			break;
		}
		}
		UnityEngine.Debug.Log($"id:{id}, total:{innerGame.totalSize}, progress:{innerGame.progress:0.00}, saved:{innerGame.savedSize}");
	}

	private IEnumerator ReDownload(InnerGameItem item)
	{
		yield return new WaitForSeconds(1f);
		Download(item);
	}

	private void OnDownloadProgress(HTTPRequest req, int downloaded, int length)
	{
		if (req != null)
		{
			InnerGameItem innerGameItem = req.Tag as InnerGameItem;
			float num = Time.time - innerGameItem.lastProgressTime;
			float num2 = (float)(downloaded - innerGameItem.lastDownloaded) / (num * 1024f);
			innerGameItem.lastProgressTime = Time.time;
			UnityEngine.Debug.Log($"total:{length}, downloaded:{downloaded}, speed:{num2:0.0}KB/s");
			InnerGame innerGame = innerGameItem.GetInnerGame();
			innerGame.downSize += downloaded - innerGameItem.lastDownloaded;
			innerGameItem.lastDownloaded = downloaded;
			int num3 = Math.Max(innerGame.totalSize, length);
			innerGame.progress = innerGame.downSize / (float)num3;
			innerGameItem.UpdateProgress();
		}
	}

	private string GenerateStartAppValue_Android(string userName, string pwd, string language, string ip, string hallPackageName, string port = "", bool newVersion = false)
	{
		string empty = string.Empty;
		long num = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		string text = (num / 1000).ToString();
		if (newVersion)
		{
			return string.Join(" ", userName, pwd, hallPackageName, ip, port, language, text);
		}
		return string.Join(" ", userName, pwd, hallPackageName, ip, language, text);
	}

	private string GenerateStartAppValue_IOS(string doMain, string userName, string pwd, string language, string ip, string hallPackageName = "", string port = "", bool newVersion = false)
	{
		string result = string.Empty;
		if (!newVersion)
		{
			result = string.Format("www.xingli.com?+Domain={4}+NAME={0}+PWD={1}+IP={2}+LAN={3}", userName, pwd, ip, language, doMain);
		}
		return result;
	}

	public void Send_CheckGameVersion(int gameId, string version)
	{
		MB_Singleton<NetManager>.GetInstance().Send("gcuserService/gameLoadAddress", new object[2]
		{
			gameId,
			version
		});
	}

	private void HandleNetMsg_CheckGameVersion(object[] args)
	{
		UnityEngine.Debug.LogError("HandleNetMsg_CheckGameVersion");
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		int num = (int)dictionary["gameType"];
		switch (num)
		{
		case 101:
			return;
		case 17:
		case 100:
		case 102:
		{
			string text = (string)dictionary["downloadWindows"];
			string text2 = (string)dictionary["downloadAndroid"];
			text = ((text.Length > 0) ? text : "https://www.baidu.com");
			text2 = ((text2.Length > 0) ? text2 : "https://www.baidu.com");
			if (Application.platform == RuntimePlatform.Android)
			{
				Application.OpenURL(text2);
			}
			else
			{
				Application.OpenURL(text);
			}
			return;
		}
		}
		if (num == 102)
		{
			return;
		}
		InnerGameItem gameItem = GetInnerGameItemByType(num);
		InnerGame innerGame = gameItem.GetInnerGame();
		UnityEngine.Debug.LogError("名字: " + innerGame.name_cn + "  " + innerGame.isLuaGame);
		if (dictionary.ContainsKey("haveNewVersionIDFlag") && (bool)dictionary["haveNewVersionIDFlag"])
		{
			string text3 = (string)dictionary["versionCode"];
			string text4 = (string)dictionary["downloadWindows"];
			string text5 = (string)dictionary["downloadAndroid"];
			UnityEngine.Debug.LogError("苹果: " + text4 + "  安卓: " + text5);
			if (innerGame.isLuaGame)
			{
				UnityEngine.Debug.Log("luaGame");
				if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
				{
					innerGame.url = text4;
				}
				else
				{
					innerGame.url = text5;
				}
				if (innerGame.state == InnerGameState.OK)
				{
					MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Updated version in order to guarantee your gaming experience,the client must be the latest version" : ((ZH2_GVars.language_enum != 0) ? "ตรวจสอบว\u0e48า เกมม\u0e35เวอร\u0e4cช\u0e31นใหม\u0e48 หร\u0e37อไม\u0e48" : "检测到游戏有新版本，是否更新"), showOkCancel: true, delegate
					{
						gameItem.ChangeState(InnerGameState.Downloading);
						StartCoroutine(Download_AB(gameItem));
					});
					return;
				}
				UnityEngine.Debug.Log("down luaGame");
				gameItem.ChangeState(InnerGameState.Downloading);
				StartCoroutine(Download_AB(gameItem));
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				innerGame.url = text5;
				Action action = delegate
				{
					if (!multiDown && IsDownloading())
					{
						gameItem.ChangeState(InnerGameState.Waiting);
						AddToWaitDLTask(gameItem);
					}
					else
					{
						gameItem.ChangeState(InnerGameState.Downloading);
						Download(gameItem);
					}
				};
				if (innerGame.state == InnerGameState.OK)
				{
					gameItem.ChangeState(InnerGameState.Default);
					MB_Singleton<AlertDialog>.Get().ShowDialog((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? "Updated version in order to guarantee your gaming experience,the client must be the latest version" : ((ZH2_GVars.language_enum != 0) ? "ตรวจสอบว\u0e48า เกมม\u0e35เวอร\u0e4cช\u0e31นใหม\u0e48 หร\u0e37อไม\u0e48" : "检测到游戏有新版本，是否更新"), showOkCancel: true, action);
				}
				else
				{
					action();
				}
			}
			else if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				GetInnerGameByType(num).url = text5;
				Application.OpenURL(GetInnerGameByType(num).url);
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GetInnerGameByType(num).url = text4;
				Application.OpenURL(GetInnerGameByType(num).url);
			}
		}
		else
		{
			try
			{
				UnityEngine.Debug.LogError("StartGame  =>  gameType: " + num);
				StartGame(GetInnerGameItemByType(num));
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
		}
	}

	public void UpdateAddress(string updateAddress)
	{
		urlInit = updateAddress;
		if (urlInit == string.Empty)
		{
			UnityEngine.Debug.LogError("空的,写死");
			urlInit = "https://6-6.oss-cn-shenzhen.aliyuncs.com/Project/Swccd88/10.0.0/XinShiDai/HotUpdate/";
		}
		PlayerPrefs.SetString("downloadAndroid", urlInit);
		PlayerPrefs.Save();
		UnityEngine.Debug.LogError("热更新链接: " + urlInit);
	}

	private InnerGameItem GetInnerGameItemByType(int gameType)
	{
		foreach (InnerGameItem item in m_itemList)
		{
			if (item.GetInnerGame().id == gameType)
			{
				return item;
			}
		}
		return null;
	}

	private InnerGame GetInnerGameByType(int gameType)
	{
		foreach (InnerGameItem item in m_itemList)
		{
			if (item.GetInnerGame().id == gameType)
			{
				return item.GetInnerGame();
			}
		}
		return null;
	}

	private void OnDestroy()
	{
		PlayerPrefs.Save();
		DownloadAbort();
	}

	public void Refresh_UninstallBtnState()
	{
		foreach (InnerGameItem item in m_itemList)
		{
			if (item != null && item.GetInnerGame().state == InnerGameState.OK && (Application.platform != RuntimePlatform.IPhonePlayer || item.GetInnerGame().isLuaGame))
			{
				item.SetDisplayUninstallBtn(debug_Uninstall);
			}
		}
	}
}
