using GameConfig;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class STTF_LoadScene : MonoBehaviour
{
	private bool hasSendLogin;

	private string userName = "ceshi001";

	private string passWord = "qqq111";

	public GameObject mask;

	private Button blackBtn;

	public STTF_LoadType Step
	{
		set
		{
			STTF_GameInfo.getInstance().LoadStep = value;
		}
	}

	private void Start()
	{
		GameObject gameObject = GameObject.Find("NetMngr");
		if ((bool)gameObject)
		{
			Object.DontDestroyOnLoad(gameObject);
		}
		GameObject gameObject2 = GameObject.Find("SoundManager");
		if ((bool)gameObject2)
		{
			Object.DontDestroyOnLoad(gameObject2);
		}
		STTF_GameInfo.getInstance().LoadScene = this;
		STTF_GameInfo.getInstance().currentState = STTF_GameState.On_Loading;
		STTF_TipManager.getInstance().InitTip();
		getIpAndConnectServer();
		blackBtn = base.transform.Find("Title_Load/BtnBack").GetComponent<Button>();
		blackBtn.onClick.AddListener(BackToHall);
	}

	private IEnumerator LoadData()
	{
		STTF_GameInfo.getInstance().IP = STTF_CheckIP.DoGetHostAddresses(STTF_GameInfo.getInstance().IP);
		while (true)
		{
			switch (STTF_GameInfo.getInstance().LoadStep)
			{
			case STTF_LoadType.On_Login:
				if (!hasSendLogin)
				{
					hasSendLogin = true;
					login();
				}
				break;
			case STTF_LoadType.On_LoadUserSetting:
				STTF_LocalData.getInstance().loadUserSetting();
				break;
			case STTF_LoadType.On_PrepareLoadScene:
				STTF_GameInfo.getInstance().LoadStep = STTF_LoadType.On_LoadScene;
				base.gameObject.SetActive(value: false);
				mask.SetActive(value: true);
				break;
			}
			yield return 1;
		}
	}

	private void login()
	{
		userName = ZH2_GVars.username;
		passWord = ZH2_GVars.pwd;
		UnityEngine.Debug.Log("账号和密码：" + userName + "||" + passWord);
		bool flag = STTF_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(userName, passWord, 0, "9.0.1");
		STTF_GameInfo.getInstance().UserId = userName;
		STTF_GameInfo.getInstance().Pwd = passWord;
	}

	private void getIpAndConnectServer()
	{
		STTF_GameInfo.getInstance().IP = ZH2_GVars.IPAddress;
		UnityEngine.Debug.LogError("链接IP: " + STTF_GameInfo.getInstance().IP);
		STTF_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine("LoadData");
	}

	public void BackToHall()
	{
		UnityEngine.Debug.LogError("====在登录界面退出====");
		UnityEngine.Object.Destroy(GameObject.Find("NetMngr"));
		UnityEngine.Object.Destroy(GameObject.Find("SoundManager"));
		AssetBundleManager.GetInstance().UnloadAB("ToadFish");
		ZH2_GVars.isStartedFromGame = false;
		SceneManager.LoadScene("MainScene");
	}
}
