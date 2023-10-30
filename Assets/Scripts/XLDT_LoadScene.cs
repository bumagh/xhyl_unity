using STDT_GameConfig;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class XLDT_LoadScene : MonoBehaviour
{
	private AsyncOperation async;

	private bool hasSendLogin;

	private string userName = "qqqqq";

	private string passWord = "a123456";

	public GameObject mask;

	[HideInInspector]
	public Button blackBtn;

	public XLDT_LoadType Step
	{
		set
		{
			XLDT_GameInfo.getInstance().LoadStep = value;
		}
	}

	private void Start()
	{
		GameObject gameObject = GameObject.Find("SoundManager");
		if ((bool)gameObject)
		{
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		GameObject gameObject2 = GameObject.Find("GameManager");
		if ((bool)gameObject2)
		{
			UnityEngine.Object.DontDestroyOnLoad(gameObject2);
		}
		GameObject gameObject3 = GameObject.Find("NetManager");
		if ((bool)gameObject3)
		{
			UnityEngine.Object.DontDestroyOnLoad(gameObject3);
		}
		XLDT_GameManager.getInstance()._mGameInfo.LoadScene = this;
		XLDT_GameManager.getInstance()._mGameInfo.currentState = XLDT_GameState.On_Loading;
		getIpAndConnectServer();
		blackBtn = base.transform.Find("Room/Title_Load/BtnBack").GetComponent<Button>();
		blackBtn.onClick.AddListener(BackToHall);
	}

	private IEnumerator LoadData()
	{
		XLDT_GameInfo.getInstance().IP = XLDT_CheckIP.DoGetHostAddresses(XLDT_GameInfo.getInstance().IP);
		UnityEngine.Debug.Log(XLDT_GameInfo.getInstance().LoadStep);
		while (true)
		{
			switch (XLDT_GameInfo.getInstance().LoadStep)
			{
			case XLDT_LoadType.On_Login:
				if (!hasSendLogin)
				{
					hasSendLogin = true;
					login();
				}
				break;
			case XLDT_LoadType.On_LoadUserSetting:
				XLDT_localData.getInstance().loadUserSetting();
				XLDT_GameInfo.getInstance().LoadStep = XLDT_LoadType.On_PrepareLoadScene;
				XLDT_SoundManage.getInstance().IsGameMusic = XLDT_GameInfo.getInstance().Setted.bIsGameVolum;
				if (XLDT_GameInfo.getInstance().Setted.bIsGameVolum)
				{
					XLDT_SoundManage.getInstance().setGameBgMusic(isPlay: true);
				}
				break;
			case XLDT_LoadType.On_PrepareLoadScene:
				XLDT_GameInfo.getInstance().LoadStep = XLDT_LoadType.On_LoadScene;
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
		UnityEngine.Debug.LogError("账号: " + userName + " 密码: " + passWord);
		bool flag = XLDT_NetMain.GetSingleton().MyCreateSocket.SendUserLogin(userName, passWord, 0, "9.0.1");
		XLDT_GameManager.getInstance()._mGameInfo.UserId = userName;
		XLDT_GameManager.getInstance()._mGameInfo.Pwd = passWord;
		XLDT_GameManager.getInstance().Write("UserId=" + XLDT_GameInfo.getInstance().UserId);
		XLDT_GameManager.getInstance().Write("Pwd=" + XLDT_GameInfo.getInstance().Pwd);
		try
		{
			WebSocket2 instance = WebSocket2.GetInstance();
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
		}
	}

	private void getIpAndConnectServer()
	{
		XLDT_GameInfo.getInstance().IP = ZH2_GVars.IPAddress_Game;
		XLDT_NetMain.GetSingleton().mIsGetIP = true;
		StartCoroutine("LoadData");
	}

	public void BackToHall()
	{
		try
		{
			UnityEngine.Object.Destroy(GameObject.Find("SoundManager"));
			UnityEngine.Object.Destroy(GameObject.Find("GameManager"));
			UnityEngine.Object.Destroy(GameObject.Find("NetManager"));
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		AssetBundleManager.GetInstance().UnloadAB("DanTiao");
		SceneManager.LoadScene("MainScene");
	}
}
