using BCBM_GameCommon;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BCBM_LoadScene : MonoBehaviour
{
	private bool hasSendLogin;

	private string userId = "testasd";

	private string pwd = "123asd";

	public GameObject mask;

	public GameObject GameMngr;

	public GameObject UIRoot;

	public Button btnBlack;

	private void Awake()
	{
		CanvasScaler component = base.transform.GetComponent<CanvasScaler>();
		if ((float)Screen.width / (float)Screen.height > 1.77777779f)
		{
			component.matchWidthOrHeight = 1f;
		}
		else
		{
			component.matchWidthOrHeight = 0f;
		}
		BCBM_GameInfo.getInstance().UserId = ZH2_GVars.username;
		BCBM_GameInfo.getInstance().Pwd = ZH2_GVars.pwd;
		btnBlack = base.transform.Find("Title/BtnBack").GetComponent<Button>();
		btnBlack.onClick.AddListener(dropOutButton);
	}

	private void OnEnable()
	{
		hasSendLogin = false;
		BCBM_NetMngr.isInLoading = true;
		BCBM_GameInfo.getInstance().LoadStep = LoadType.On_ConnectNet;
	}

	private void Start()
	{
		getIpAndConnectServer();
		UnityEngine.Object.DontDestroyOnLoad(GameObject.Find("netMngr"));
		BCBM_GameInfo.getInstance().LoadScene = this;
	}

	private IEnumerator LoadData()
	{		
		try
		{
			BCBM_GameInfo.getInstance().IP = BCBM_CheckIP.DoGetHostAddresses(BCBM_GameInfo.getInstance().IP);
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
		}
		while (true)
		{
			switch (BCBM_GameInfo.getInstance().LoadStep)
			{
			case LoadType.On_Login:
				if (!hasSendLogin)
				{
					hasSendLogin = true;
					login();
				}
				break;
			case LoadType.On_LoadUserSetting:
				BCBM_LocalData.getInstance().loadUserSetting();
				break;
			case LoadType.On_PrepareLoadScene:
				BCBM_GameInfo.getInstance().LoadStep = LoadType.On_LoadScene;
				base.gameObject.SetActive(value: false);
				mask.gameObject.SetActive(value: true);
				GameMngr.gameObject.SetActive(value: true);
				break;
			}
			yield return 1;
		}
	}

	public void dropOutButton()
	{
		UnityEngine.Debug.LogError("=======3");
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		ZH2_GVars.isStartedFromGame = true;
		GameObject gameObject = GameObject.Find("netMngr");
		GameObject gameObject2 = GameObject.Find("GameMngr");
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		else
		{
			UnityEngine.Debug.LogError("====netMngr===为空");
		}
		if (gameObject2 != null)
		{
			UnityEngine.Object.Destroy(gameObject2);
		}
		else
		{
			UnityEngine.Debug.LogError("====gameMngr===为空");
		}
		AssetBundleManager.GetInstance().UnloadAB("BCBM");
		SceneManager.LoadScene("MainScene");
	}

	private void login()
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		empty = ZH2_GVars.username;
		empty2 = ZH2_GVars.pwd;
		UnityEngine.Debug.Log("账号和密码：" + empty + "||" + empty2);
		bool flag = BCBM_NetMngr.GetSingleton().MyCreateSocket.SendUserLogin(empty, empty2, 0, "9.0.1");
		BCBM_GameInfo.getInstance().UserId = empty;
		BCBM_GameInfo.getInstance().Pwd = empty2;
	}

	private void getIpAndConnectServer()
	{
		BCBM_GameInfo.getInstance().IP = ZH2_GVars.IPAddress;
		BCBM_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine("LoadData");
	}
}
