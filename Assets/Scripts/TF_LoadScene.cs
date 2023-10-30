using GameConfig;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TF_LoadScene : MonoBehaviour
{
	private AsyncOperation async;

	private Slider bar;

	private bool hasSendLogin;

	private string userName = string.Empty;

	private string passWord = string.Empty;

	public TF_LoadType Step
	{
		set
		{
			TF_GameInfo.getInstance().LoadStep = value;
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
		bar = GameObject.Find("Loading/Slider").GetComponent<Slider>();
		TF_GameInfo.getInstance().LoadScene = this;
		TF_GameInfo.getInstance().currentState = TF_GameState.On_Loading;
		TF_TipManager.getInstance().InitTip();
		bar.value = 0f;
		getIpAndConnectServer();
	}

	private IEnumerator LoadData()
	{
		UnityEngine.Debug.Log("gameInfo.getInstance().IP: " + TF_GameInfo.getInstance().IP);
		TF_GameInfo.getInstance().IP = TF_CheckIP.DoGetHostAddresses(TF_GameInfo.getInstance().IP);
		while (true)
		{
			switch (TF_GameInfo.getInstance().LoadStep)
			{
			case TF_LoadType.On_Login:
				if (!hasSendLogin)
				{
					hasSendLogin = true;
					login();
				}
				break;
			case TF_LoadType.On_LoadUserSetting:
				TF_LocalData.getInstance().loadUserSetting();
				break;
			case TF_LoadType.On_PrepareLoadScene:
				TF_GameInfo.getInstance().LoadStep = TF_LoadType.On_LoadScene;
				async = SceneManager.LoadSceneAsync("STTF_UIScene");
				break;
			}
			yield return 1;
		}
	}

	private void Update()
	{
		if (TF_GameInfo.getInstance().LoadStep == TF_LoadType.On_LoadScene)
		{
			if ((bool)bar && async != null)
			{
				bar.value = 0.3f + async.progress * 0.7f;
			}
		}
		else if ((bool)bar && async != null)
		{
			bar.value = (float)TF_GameInfo.getInstance().LoadStep * 0.1f;
		}
	}

	private void login()
	{
		userName = ZH2_GVars.username;
		passWord = ZH2_GVars.pwd;
		UnityEngine.Debug.Log("账号和密码：" + userName + "||" + passWord);
		bool flag = TF_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(userName, passWord, 0, "9.0.1");
		TF_GameInfo.getInstance().UserId = userName;
		TF_GameInfo.getInstance().Pwd = passWord;
	}

	private void getIpAndConnectServer()
	{
		TF_GameInfo.getInstance().IP = ZH2_GVars.IPAddress;
		TF_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine("LoadData");
	}
}
