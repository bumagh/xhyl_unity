using LHD_GameCommon;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LHD_LoadScene : MonoBehaviour
{
	private bool hasSendLogin;

	public static LHD_LoadScene instance;

	private void Awake()
	{
		instance = this;
		LHD_GameInfo.getInstance().IP = ZH2_GVars.IPAddress_Game;
		LHD_GameInfo.getInstance().UserId = ZH2_GVars.username;
		LHD_GameInfo.getInstance().Pwd = ZH2_GVars.pwd;
		UnityEngine.Debug.LogError($"ip: {LHD_GameInfo.getInstance().IP} 账号:{LHD_GameInfo.getInstance().UserId}  密码: {LHD_GameInfo.getInstance().Pwd}");
	}

	private void Start()
	{
		LHD_GameInfo.getInstance().LoadScene = this;
		LHD_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine(LoadData());
	}

	private void OnEnable()
	{
		hasSendLogin = false;
		LHD_GameInfo.getInstance().LoadStep = LoadType.On_Load;
	}

	private void Update()
	{
	}

	private IEnumerator LoadData()
	{
		try
		{
			LHD_GameInfo.getInstance().IP = LHD_CheckIP.DoGetHostAddresses(LHD_GameInfo.getInstance().IP);
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
		}
		while (true)
		{
			switch (LHD_GameInfo.getInstance().LoadStep)
			{
			case LoadType.On_Login:
				if (!hasSendLogin)
				{
					hasSendLogin = true;
					Login();
				}
				break;
			case LoadType.On_Hall:
				LHD_UIManager.instance.ShowHall();
				break;
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	private void Login()
	{
		UnityEngine.Debug.Log(LHD_GameInfo.getInstance().UserId);
		UnityEngine.Debug.Log(LHD_GameInfo.getInstance().Pwd);
		bool flag = LHD_NetMngr.GetSingleton().MyCreateSocket.SendUserLogin(LHD_GameInfo.getInstance().UserId, LHD_GameInfo.getInstance().Pwd, 0, "9.0.1");
	}

	public void Quit()
	{
		LHD_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
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
		SceneManager.LoadScene("MainScene");
	}
}
