using GameConfig;
using System;
using System.Collections;
using UnityEngine;

public class DK_LoadScene : MonoBehaviour
{
	private bool hasSendLogin;

	private string userName = "ceshi001";

	private string passWord = "qqq111";

	public GameObject mask;

	public DK_LoadType Step
	{
		set
		{
			DK_GameInfo.getInstance().LoadStep = value;
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
		DK_GameInfo.getInstance().LoadScene = this;
		DK_GameInfo.getInstance().currentState = DK_GameState.On_Loading;
		DK_TipManager.getInstance().InitTip();
		getIpAndConnectServer();
	}

	private IEnumerator LoadData()
	{
		DK_GameInfo.getInstance().IP = DK_CheckIP.DoGetHostAddresses(DK_GameInfo.getInstance().IP);
		while (true)
		{
			switch (DK_GameInfo.getInstance().LoadStep)
			{
			case DK_LoadType.On_Login:
				if (!hasSendLogin)
				{
					hasSendLogin = true;
					login();
				}
				break;
			case DK_LoadType.On_LoadUserSetting:
				DK_LocalData.getInstance().loadUserSetting();
				break;
			case DK_LoadType.On_PrepareLoadScene:
				DK_GameInfo.getInstance().LoadStep = DK_LoadType.On_LoadScene;
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
		bool flag = DK_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(userName, passWord, 0, "9.0.1");
		DK_GameInfo.getInstance().UserId = userName;
		DK_GameInfo.getInstance().Pwd = passWord;
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
		DK_GameInfo.getInstance().IP = ZH2_GVars.IPAddress_Game;
		UnityEngine.Debug.LogError("IP: " + DK_GameInfo.getInstance().IP);
		DK_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine(LoadData());
	}
}
