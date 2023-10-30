using GameConfig;
using System.Collections;
using UnityEngine;

public class DNTG_LoadScene : MonoBehaviour
{
	private bool hasSendLogin;

	private string userName = string.Empty;

	private string passWord = string.Empty;

	public GameObject mask;

	public DNTG_LoadType Step
	{
		set
		{
			DNTG_GameInfo.getInstance().LoadStep = value;
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
		DNTG_GameInfo.getInstance().LoadScene = this;
		DNTG_GameInfo.getInstance().currentState = DNTG_GameState.On_Loading;
		DNTG_TipManager.getInstance().InitTip();
		getIpAndConnectServer();
	}

	private IEnumerator LoadData()
	{
		UnityEngine.Debug.LogError("IP: " + DNTG_GameInfo.getInstance().IP);
		DNTG_GameInfo.getInstance().IP = DNTG_CheckIP.DoGetHostAddresses(DNTG_GameInfo.getInstance().IP);
		while (true)
		{
			switch (DNTG_GameInfo.getInstance().LoadStep)
			{
			case DNTG_LoadType.On_Login:
				if (!hasSendLogin)
				{
					hasSendLogin = true;
					login();
				}
				break;
			case DNTG_LoadType.On_LoadUserSetting:
				DNTG_LocalData.getInstance().loadUserSetting();
				break;
			case DNTG_LoadType.On_PrepareLoadScene:
				DNTG_GameInfo.getInstance().LoadStep = DNTG_LoadType.On_LoadScene;
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
		bool flag = DNTG_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(userName, passWord, 0, "9.0.1");
		DNTG_GameInfo.getInstance().UserId = userName;
		DNTG_GameInfo.getInstance().Pwd = passWord;
	}

	private void getIpAndConnectServer()
	{
		DNTG_GameInfo.getInstance().IP = ZH2_GVars.IPAddress;
		DNTG_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine("LoadData");
	}
}
