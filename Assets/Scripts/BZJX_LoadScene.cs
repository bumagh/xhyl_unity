using GameConfig;
using System.Collections;
using UnityEngine;

public class BZJX_LoadScene : MonoBehaviour
{
	private bool hasSendLogin;

	private string userName = string.Empty;

	private string passWord = string.Empty;

	public GameObject mask;

	public BZJX_LoadType Step
	{
		set
		{
			BZJX_GameInfo.getInstance().LoadStep = value;
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
		BZJX_GameInfo.getInstance().LoadScene = this;
		BZJX_GameInfo.getInstance().currentState = BZJX_GameState.On_Loading;
		BZJX_TipManager.getInstance().InitTip();
		getIpAndConnectServer();
	}

	private IEnumerator LoadData()
	{
		UnityEngine.Debug.LogError("IP: " + BZJX_GameInfo.getInstance().IP);
		BZJX_GameInfo.getInstance().IP = BZJX_CheckIP.DoGetHostAddresses(BZJX_GameInfo.getInstance().IP);
		while (true)
		{
			switch (BZJX_GameInfo.getInstance().LoadStep)
			{
			case BZJX_LoadType.On_Login:
				if (!hasSendLogin)
				{
					hasSendLogin = true;
					login();
				}
				break;
			case BZJX_LoadType.On_LoadUserSetting:
				BZJX_LocalData.getInstance().loadUserSetting();
				break;
			case BZJX_LoadType.On_PrepareLoadScene:
				BZJX_GameInfo.getInstance().LoadStep = BZJX_LoadType.On_LoadScene;
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
		bool flag = BZJX_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(userName, passWord, 0, "9.0.1");
		BZJX_GameInfo.getInstance().UserId = userName;
		BZJX_GameInfo.getInstance().Pwd = passWord;
	}

	private void getIpAndConnectServer()
	{
		BZJX_GameInfo.getInstance().IP = ZH2_GVars.IPAddress;
		BZJX_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine("LoadData");
	}
}
