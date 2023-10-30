using GameConfig;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class STOF_LoadScene : MonoBehaviour
{
	private bool hasSendLogin;

	private string userName = string.Empty;

	private string passWord = string.Empty;

	public GameObject mask;

	private Button btnBlack;

	public STOF_LoadType Step
	{
		set
		{
			STOF_GameInfo.getInstance().LoadStep = value;
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
		STOF_GameInfo.getInstance().LoadScene = this;
		STOF_GameInfo.getInstance().currentState = STOF_GameState.On_Loading;
		STOF_TipManager.getInstance().InitTip();
		getIpAndConnectServer();
		btnBlack = base.transform.Find("Title_Load/BtnBack").GetComponent<Button>();
		btnBlack.onClick.AddListener(BackToHall);
	}

	private IEnumerator LoadData()
	{
		STOF_GameInfo.getInstance().IP = STOF_CheckIP.DoGetHostAddresses(STOF_GameInfo.getInstance().IP);
		while (true)
		{
			switch (STOF_GameInfo.getInstance().LoadStep)
			{
			case STOF_LoadType.On_Login:
				if (!hasSendLogin)
				{
					hasSendLogin = true;
					login();
				}
				break;
			case STOF_LoadType.On_LoadUserSetting:
				STOF_LocalData.getInstance().loadUserSetting();
				break;
			case STOF_LoadType.On_PrepareLoadScene:
				STOF_GameInfo.getInstance().LoadStep = STOF_LoadType.On_LoadScene;
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
		bool flag = STOF_NetMngr.GetSingleton().MyCreateSocket.SendCheckLogin(userName, passWord, 0, "9.0.1");
		STOF_GameInfo.getInstance().UserId = userName;
		STOF_GameInfo.getInstance().Pwd = passWord;
	}

	private void getIpAndConnectServer()
	{
		STOF_GameInfo.getInstance().IP = ZH2_GVars.IPAddress;
		STOF_NetMngr.GetSingleton().mIsGetIP = true;
		StartCoroutine("LoadData");
	}

	public void BackToHall()
	{
		UnityEngine.Object.Destroy(GameObject.Find("NetMngr"));
		UnityEngine.Object.Destroy(GameObject.Find("SoundManager"));
		if (AssetBundleManager.GetInstance() != null)
		{
			AssetBundleManager.GetInstance().UnloadAB("OverFish");
		}
		ZH2_GVars.isStartedFromGame = true;
		SceneManager.LoadScene("MainScene");
	}
}
