using LitJson;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BCBM_link : MonoBehaviour
{
	public static bool startGame;

	public static int moreInfo = -1;

	public static int animalInfo = -1;

	public static BCBM_link publiclink;

	[Header("异地登录提示")]
	public GameObject OtherScene;

	public static int TimeValue_num;

	public static int Time_Dji;

	private float wait_time;

	public static int name_ani;

	public static int animal_ani;

	public static int coloer_ani;

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
		string url = "http://47.106.191.250:81/jsys-api/room-start?game_id=" + BCBM_LoginInfo.Instance().mylogindata.choosegame + "&user_id=" + BCBM_LoginInfo.Instance().mylogindata.user_id + "&unionuid=" + BCBM_LoginInfo.Instance().mylogindata.token + "&room_id=" + BCBM_LoginInfo.Instance().mylogindata.room_id;
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
			string url = "http://47.106.191.250:81/jsys-api/userinfo?user_id=" + BCBM_LoginInfo.Instance().mylogindata.user_id + "&unionuid=" + BCBM_LoginInfo.Instance().mylogindata.token;
			UnityWebRequest www = UnityWebRequest.Get(url);
			yield return www.Send();
			if (www.error == null)
			{
				MonoBehaviour.print("链接：" + url);
				JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
				if (jsonData["code"].ToString().Equals("200"))
				{
					BCBM_BetScene.publicBetScene.displaylaue[1] = double.Parse(jsonData["Userinfo"]["quick_credit"].ToString());
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
		string URL = "http://47.106.191.250:81/jsys-api/trigger-ent?room_id=" + BCBM_LoginInfo.Instance().mylogindata.room_id + "&game_id=" + BCBM_LoginInfo.Instance().mylogindata.choosegame + "&user_id=" + BCBM_LoginInfo.Instance().mylogindata.user_id;
		UnityWebRequest www = UnityWebRequest.Get(URL);
		yield return www.Send();
		if (www != null)
		{
			UnityEngine.Debug.Log(www.downloadHandler.text);
			JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
			if (jsonData["code"].ToString().Equals("200"))
			{
				BCBM_UI.publicUI.PromptMethon(jsonData["msg"].ToString());
				UnityEngine.SceneManagement.SceneManager.LoadScene(1);
			}
			else
			{
				BCBM_UI.publicUI.PromptMethon(jsonData["msg"].ToString());
			}
		}
	}

	public void BetMethon(string userBet, int Value, int label)
	{
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendUserBet(userBet, label, Value, BCBM_GameInfo.getInstance().UserInfo.TableId);
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
				publiclink.parameter(Time_Dji, animal_ani, name_ani, coloer_ani);
			}
		}
	}

	public void parameter(int TimeValue, int Aims, int space, int coloer)
	{
		if (TimeValue == -1)
		{
			BCBM_BetScene.publicBetScene.TimeMethon(TimeValue);
			return;
		}
		BCBM_Control.publicControl.ExceedTime = 0;
		if (TimeValue == 16)
		{
			BCBM_Control.publicControl.Init();
			MonoBehaviour.print("开启旋转");
			StartCoroutine(BCBM_Control.publicControl.Open(Aims, space));
			if (BCBM_BetScene.publicBetScene.coroutineEndSpeed != null)
			{
				StopCoroutine(BCBM_BetScene.publicBetScene.coroutineEndSpeed);
			}
			BCBM_BetScene.publicBetScene.coroutineEndSpeed = StartCoroutine(BCBM_BetScene.publicBetScene.EndSpeed());
		}
		TimeValue_num = TimeValue;
		BCBM_BetScene.publicBetScene.TimeMethon(TimeValue);
	}
}
