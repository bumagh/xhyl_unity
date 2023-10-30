using JSYS_LL_GameCommon;
using System;
using System.Collections;
using UnityEngine;

public class JSYS_LL_LoadScene : MonoBehaviour
{
	private bool hasSendLogin;

	private string userId = "testasd";

	private string pwd = "123asd";

	public GameObject gameMngr;

	public GameObject mask;

	private void Awake()
	{
		JSYS_LL_GameInfo.getInstance().UserId = ZH2_GVars.username;
		JSYS_LL_GameInfo.getInstance().Pwd = ZH2_GVars.pwd;
	}

	private void OnEnable()
	{
		hasSendLogin = false;
		JSYS_LL_GameInfo.getInstance().LoadStep = LoadType.On_ConnectNet;
	}

	private void Start()
	{
		getIpAndConnectServer();
		JSYS_LL_NetMngr.isInLoading = true;
		UnityEngine.Object.DontDestroyOnLoad(GameObject.Find("netMngr"));
		JSYS_LL_GameInfo.getInstance().LoadScene = this;
	}

	private IEnumerator LoadData()
	{
		UnityEngine.Debug.Log("CheckIP IP: " + JSYS_LL_GameInfo.getInstance().IP);
		if (!JSYS_LL_CheckIP.IPCheck(JSYS_LL_GameInfo.getInstance().IP))
		{
			UnityEngine.Debug.LogError("ip校验不通过!  退出!!!");
			Application.Quit();
			yield break;
		}
		try
		{
			JSYS_LL_GameInfo.getInstance().IP = JSYS_LL_CheckIP.DoGetHostAddresses(JSYS_LL_GameInfo.getInstance().IP);
			UnityEngine.Debug.Log("IP: " + JSYS_LL_GameInfo.getInstance().IP);
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
		}
		while (true)
		{
			switch (JSYS_LL_GameInfo.getInstance().LoadStep)
			{
			case LoadType.On_Login:
				if (!hasSendLogin)
				{
					UnityEngine.Debug.Log("Login");
					hasSendLogin = true;
					login();
				}
				break;
			case LoadType.On_LoadUserSetting:
				JSYS_LL_LocalData.getInstance().loadUserSetting();
				break;
			case LoadType.On_PrepareLoadScene:
				JSYS_LL_GameInfo.getInstance().LoadStep = LoadType.On_LoadScene;
				MonoBehaviour.print("LoadGame");
				break;
			}
			yield return 1;
		}
	}

	private void FixedUpdate()
	{
		if (JSYS_LL_GameInfo.kaishi)
		{
			JSYS_LL_GameInfo.kaishi = false;
			gameMngr.SetActive(value: true);
			mask.SetActive(value: true);
			base.gameObject.SetActive(value: false);
		}
	}

	private void login()
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		empty = ZH2_GVars.username;
		empty2 = ZH2_GVars.pwd;
		UnityEngine.Debug.Log("账号和密码：" + empty + "||" + empty2);
		bool flag = JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendUserLogin(empty, empty2, 0, "9.0.1");
		JSYS_LL_GameInfo.getInstance().UserId = empty;
		JSYS_LL_GameInfo.getInstance().Pwd = empty2;
		MonoBehaviour.print("testuserLogin");
	}

	private void getIpAndConnectServer()
	{
		JSYS_LL_GameInfo.getInstance().IP = ZH2_GVars.IPAddress;
		UnityEngine.Debug.LogError("链接IP: " + JSYS_LL_GameInfo.getInstance().IP);
		JSYS_LL_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine("LoadData");
	}
}
