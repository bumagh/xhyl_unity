using GameConfig;
using System;
using System.Collections;
using UnityEngine;

public class STQM_LoadScene : MonoBehaviour
{
	private AsyncOperation async;

	private bool hasSendLogin;

	private string userName = "ceshi001";

	private string passWord = "qqq111";

	public GameObject mask;

	public STQM_LoadType Step
	{
		set
		{
			STQM_GameInfo.getInstance().LoadStep = value;
		}
	}

	private void Start()
	{
		GameObject gameObject = GameObject.Find("NetMngr");
		if ((bool)gameObject)
		{
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		GameObject gameObject2 = GameObject.Find("SoundManager");
		if ((bool)gameObject2)
		{
			UnityEngine.Object.DontDestroyOnLoad(gameObject2);
		}
		STQM_GameInfo.getInstance().LoadScene = this;
		STQM_GameInfo.getInstance().currentState = STQM_GameState.On_Loading;
		STQM_TipManager.getInstance().InitTip();
		getIpAndConnectServer();
	}

	private IEnumerator LoadData()
	{
		STQM_GameInfo.getInstance().IP = STQM_CheckIP.DoGetHostAddresses(STQM_GameInfo.getInstance().IP);
		while (true)
		{
			switch (STQM_GameInfo.getInstance().LoadStep)
			{
			case STQM_LoadType.On_Login:
				if (!hasSendLogin)
				{
					hasSendLogin = true;
					login();
				}
				break;
			case STQM_LoadType.On_LoadUserSetting:
				STQM_LocalData.getInstance().loadUserSetting();
				break;
			case STQM_LoadType.On_PrepareLoadScene:
				STQM_GameInfo.getInstance().LoadStep = STQM_LoadType.On_LoadScene;
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
		bool flag = STQM_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(userName, passWord, 0, "9.0.1");
		STQM_GameInfo.getInstance().UserId = userName;
		STQM_GameInfo.getInstance().Pwd = passWord;
		try
		{
			WebSocket2 instance = WebSocket2.GetInstance();
			UnityEngine.Debug.LogError("webSocket: " + instance.name);
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
		}
	}

	private void getIpAndConnectServer()
	{
		STQM_GameInfo.getInstance().IP = ZH2_GVars.IPAddress_Game;
		UnityEngine.Debug.LogError("IP: " + STQM_GameInfo.getInstance().IP);
		STQM_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine("LoadData");
	}
}
