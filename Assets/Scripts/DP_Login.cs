using DP_GameCommon;
using System.Collections;
using UnityEngine;

public class DP_Login : MonoBehaviour
{
	private bool hasSendLogin;

	private string userId = "testasd";

	private string pwd = "123asd";

	public DP_SceneUi sceneUi;

	private void Start()
	{
		DP_TipManager.GetSingleton().Init();
		sceneUi.Init();
		if (!DP_NetMngr.GetSingleton().mIsGetIP)
		{
			DP_NetMngr.isInLoading = true;
			Object.DontDestroyOnLoad(GameObject.Find("netMngr"));
			getIpAndConnectServer();
		}
	}

	private IEnumerator LoadData()
	{
		DP_GameInfo.getInstance().IP = DP_CheckIP.DoGetHostAddresses(DP_GameInfo.getInstance().IP);
		while (true)
		{
			switch (DP_GameInfo.getInstance().LoadStep)
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
				DP_LocalData.getInstance().loadUserSetting();
				MonoBehaviour.print("Setting");
				break;
			case LoadType.On_PrepareLoadScene:
				DP_GameInfo.getInstance().LoadStep = LoadType.On_LoadScene;
				MonoBehaviour.print("OpenRoom");
				DP_NetMngr.isInLoading = false;
				DP_GameInfo.getInstance().GetAppState = AppState.App_On_RoomList_Panel;
				DP_NetMngr.GetSingleton().MyCreateSocket.SendEnterRoom(1);
				break;
			}
			yield return 1;
		}
	}

	private void login()
	{
		string username = ZH2_GVars.username;
		string strPassword = ZH2_GVars.pwd;
		bool flag = DP_NetMngr.GetSingleton().MyCreateSocket.SendUserLogin(username, strPassword, 0, "9.0.1");
		DP_GameInfo.getInstance().UserId = username;
		DP_GameInfo.getInstance().Pwd = strPassword;
		MonoBehaviour.print("testuserLogin");
	}

	private void getIpAndConnectServer()
	{
		DP_GameInfo.getInstance().IP = ZH2_GVars.IPAddress_Game;
		DP_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine("LoadData");
	}
}
