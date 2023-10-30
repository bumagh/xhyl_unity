using GameConfig;
using System.Collections;
using UnityEngine;

public class STMF_LoadScene : MonoBehaviour
{
	private AsyncOperation async;

	private bool hasSendLogin;

	private string userName = "ceshi001";

	private string passWord = "qqq111";

	public GameObject mask;

	public STMF_LoadType Step
	{
		set
		{
			STMF_GameInfo.getInstance().LoadStep = value;
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
		STMF_GameInfo.getInstance().LoadScene = this;
		STMF_GameInfo.getInstance().currentState = STMF_GameState.On_Loading;
		STMF_TipManager.getInstance().InitTip();
		getIpAndConnectServer();
	}

	private IEnumerator LoadData()
	{
		STMF_GameInfo.getInstance().IP = STMF_CheckIP.DoGetHostAddresses(STMF_GameInfo.getInstance().IP);
		while (true)
		{
			switch (STMF_GameInfo.getInstance().LoadStep)
			{
			case STMF_LoadType.On_Login:
				if (!hasSendLogin)
				{
					hasSendLogin = true;
					login();
				}
				break;
			case STMF_LoadType.On_LoadUserSetting:
				STMF_LocalData.getInstance().loadUserSetting();
				break;
			case STMF_LoadType.On_PrepareLoadScene:
				STMF_GameInfo.getInstance().LoadStep = STMF_LoadType.On_LoadScene;
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
		bool flag = STMF_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(userName, passWord, 0, "9.0.1");
		STMF_GameInfo.getInstance().UserId = userName;
		STMF_GameInfo.getInstance().Pwd = passWord;
	}

	private void getIpAndConnectServer()
	{
		STMF_GameInfo.getInstance().IP = ZH2_GVars.IPAddress_Game;
		UnityEngine.Debug.LogError("IP: " + STMF_GameInfo.getInstance().IP);
		STMF_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine("LoadData");
	}
}
