using LL_GameCommon;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LL_LoadScene : MonoBehaviour
{
	private bool hasSendLogin;

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
		btnBlack = base.transform.Find("Title_Load/BtnBack").GetComponent<Button>();
		btnBlack.onClick.AddListener(ClickOk);
	}

	private void Start()
	{
		LL_NetMngr.isInLoading = true;
		Object.DontDestroyOnLoad(GameObject.Find("netMngr"));
		LL_GameInfo.getInstance().LoadScene = this;
		getIpAndConnectServer();
	}

	public void ClickOk()
	{
		base.gameObject.SetActive(value: false);
		ZH2_GVars.isStartedFromGame = false;
		UnityEngine.Object.Destroy(GameObject.Find("netMngr"));
		AssetBundleManager.GetInstance().UnloadAB("LuckyLion");
		SceneManager.LoadScene("MainScene");
	}

	private IEnumerator LoadData()
	{
		LL_GameInfo.getInstance().IP = LL_CheckIP.DoGetHostAddresses(LL_GameInfo.getInstance().IP);
		UnityEngine.Debug.LogError("IP: " + LL_GameInfo.getInstance().IP);
		while (true)
		{
			switch (LL_GameInfo.getInstance().LoadStep)
			{
			case LoadType.On_Login:
				if (!hasSendLogin)
				{
					hasSendLogin = true;
					login();
				}
				break;
			case LoadType.On_LoadUserSetting:
				LL_LocalData.getInstance().loadUserSetting();
				break;
			case LoadType.On_PrepareLoadScene:
				LL_GameInfo.getInstance().LoadStep = LoadType.On_LoadScene;
				base.gameObject.SetActive(value: false);
				mask.gameObject.SetActive(value: true);
				GameMngr.gameObject.SetActive(value: true);
				UIRoot.gameObject.SetActive(value: true);
				break;
			}
			yield return 1;
		}
	}

	private void login()
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		empty = ZH2_GVars.username;
		empty2 = ZH2_GVars.pwd;
		UnityEngine.Debug.LogError("账号: " + empty + " 密码: " + empty2);
		bool flag = LL_NetMngr.GetSingleton().MyCreateSocket.SendUserLogin(empty, empty2, 0, "9.0.1");
		LL_GameInfo.getInstance().UserId = empty;
		LL_GameInfo.getInstance().Pwd = empty2;
	}

	private void getIpAndConnectServer()
	{
		LL_GameInfo.getInstance().IP = ZH2_GVars.IPAddress_Game;
		UnityEngine.Debug.LogError("IP: " + LL_GameInfo.getInstance().IP);
		LL_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine("LoadData");
	}
}
