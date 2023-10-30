using LitJson;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class JSYS_link : MonoBehaviour
{
	public static bool startGame;

	public static string moreInfo = string.Empty;

	public static JSYS_link publiclink;

	[Header("异地登录提示")]
	public GameObject OtherScene;

	public static int TimeValue_num;

	public static int Time_Dji;

	private float wait_time;

	public static string name_ani = string.Empty;

	private void Awake()
	{
		startGame = true;
		publiclink = this;
	}

	private void Start()
	{
	}

	public void OpenNewTcpNet()
	{
		MonoBehaviour.print("长链接开启成功");
	}

	public IEnumerator OpebLink()
	{
		string url = "http://47.106.191.250:81/jsys-api/room-start?game_id=" + JSYS_LoginInfo.Instance().mylogindata.choosegame + "&user_id=" + JSYS_LoginInfo.Instance().mylogindata.user_id + "&unionuid=" + JSYS_LoginInfo.Instance().mylogindata.token + "&room_id=" + JSYS_LoginInfo.Instance().mylogindata.room_id;
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();
		UnityEngine.Debug.LogError(url);
		if (www.error == null)
		{
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			if (jsonData["code"].ToString().Equals("200"))
			{
				MonoBehaviour.print("进入房间成功");
				OpenNewTcpNet();
				StartCoroutine(Userlauelink());
			}
			else
			{
				MonoBehaviour.print(jsonData["msg"].ToString());
			}
		}
	}

	public IEnumerator Userlauelink()
	{
		MonoBehaviour.print("是否进入？");
		while (true)
		{
			string url = "http://47.106.191.250:81/jsys-api/userinfo?user_id=" + JSYS_LoginInfo.Instance().mylogindata.user_id + "&unionuid=" + JSYS_LoginInfo.Instance().mylogindata.token;
			UnityWebRequest www = UnityWebRequest.Get(url);
			yield return www.Send();
			if (www.error == null)
			{
				MonoBehaviour.print("链接：" + url);
				JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
				if (jsonData["code"].ToString().Equals("200"))
				{
					JSYS_BetScene.publicBetScene.displaylaue[1] = double.Parse(jsonData["Userinfo"]["quick_credit"].ToString());
				}
				else
				{
					OtherScene.SetActive(value: true);
				}
			}
			yield return new WaitForSeconds(0.05f);
		}
	}

	public IEnumerator dropOut()
	{
		string URL = "http://47.106.191.250:81/jsys-api/trigger-ent?room_id=" + JSYS_LoginInfo.Instance().mylogindata.room_id + "&game_id=" + JSYS_LoginInfo.Instance().mylogindata.choosegame + "&user_id=" + JSYS_LoginInfo.Instance().mylogindata.user_id;
		UnityWebRequest www = UnityWebRequest.Get(URL);
		yield return www.Send();
		if (www != null)
		{
			UnityEngine.Debug.Log(www.downloadHandler.text);
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			if (jsonData["code"].ToString().Equals("200"))
			{
				JSYS_UI.publicUI.PromptMethon(jsonData["msg"].ToString());
				UnityEngine.SceneManagement.SceneManager.LoadScene(1);
			}
			else
			{
				JSYS_UI.publicUI.PromptMethon(jsonData["msg"].ToString());
			}
		}
	}

	public void BetMethon(int Value, string label, int ID, int Inix)
	{
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendUserBet(int.Parse(label), Value, JSYS_LL_GameInfo.getInstance().UserInfo.TableId);
	}

	private void Update()
	{
		if (Time_Dji > 18 || (Time_Dji <= 17 && Time_Dji >= 16))
		{
			wait_time += Time.deltaTime;
			if (wait_time > 1f)
			{
				wait_time = 0f;
				Time_Dji--;
				publiclink.parameter(Time_Dji, name_ani);
			}
		}
	}

	public void parameter(int TimeValue, string Aims)
	{
		if (TimeValue == -1)
		{
			JSYS_BetScene.publicBetScene.TimeMethon(TimeValue);
			return;
		}
		JSYS_Control.publicControl.ExceedTime = 0;
		if (TimeValue == 16 && Aims.Length >= 1)
		{
			JSYS_Control.publicControl.Init();
			MonoBehaviour.print("开启旋转");
			StartCoroutine(JSYS_Control.publicControl.Open(Aims));
		}
		TimeValue_num = TimeValue;
		JSYS_BetScene.publicBetScene.TimeMethon(TimeValue);
	}
}
