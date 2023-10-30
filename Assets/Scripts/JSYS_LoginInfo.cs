using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class JSYS_LoginInfo : MonoBehaviour
{
	private static JSYS_LoginInfo instance;

	private static GameObject loginGO;

	public JSYS_WWWstatic wwwinstance;

	public int mytest;

	public JSYS_LoginData mylogindata;

	public static JSYS_LoginInfo Instance()
	{
		if (instance == null)
		{
			loginGO = new GameObject("LoginInfo");
			instance = (loginGO.AddComponent(typeof(JSYS_LoginInfo)) as JSYS_LoginInfo);
			instance.wwwinstance = (loginGO.AddComponent(typeof(JSYS_WWWstatic)) as JSYS_WWWstatic);
			instance.wwwinstance.listforloginInfo = new Queue<UnityWebRequest>();
			instance.wwwinstance.listforhallinfo = new Queue<UnityWebRequest>();
			instance.wwwinstance.listforgameinfo = new Queue<UnityWebRequest>();
			instance.wwwinstance.listformwwwinfo = new Queue<JSYS_wwwinfo>();
			instance.mylogindata = new JSYS_LoginData("http://47.106.191.250:81/", "login?", "register?", "game-list", "room-list?", "room-start?", "room-end?", "countdown-dt", "dt-win-list", "user-task-dt?", "bets-dt?", "dt-win-info?", "cancel-all?", "userinfo?", "version?", "password-chang?", "win-history?", "user-cut?", "user-cut-send?", "game-room-odds?", "mpzzs-bets?", "win-list?", "room-user-data?", "user-bets-data?", "win-info?", "mpzzs-cancel-all?", "bl-bets?", "bl-cancel-all?", "ds-bets?", "ds-cancel-all?", "elb-bets?", "elb-cancel-all?", "pj-bets?", "pj-cancel-all?", "lh-bets?", "lh-cancel-all?", "live-video?", "xwy-bets?", "xwy-cancel-all?", "dxb-bets?", "dxb-cancel-all?", "services-info");
			instance.mylogindata.game_id = new List<int>
			{
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				11,
				12,
				13,
				14
			};
			instance.mylogindata.snid = new List<string>();
			UnityEngine.Object.DontDestroyOnLoad(loginGO);
		}
		return instance;
	}

	private void FixedUpdate()
	{
		if (!JSYS_LoginData.IsLogin)
		{
			return;
		}
		JSYS_LoginData.OverTime += Time.deltaTime;
		if (JSYS_LoginData.OverTime >= 3f)
		{
			JSYS_NewTcpNet.GetInstance().SocketQuit();
			if (mylogindata.choosegame == 19)
			{
				JSYS_NewTcpNet.GetInstance2();
			}
			else
			{
				JSYS_NewTcpNet.GetInstance();
			}
		}
	}

	public void cleanmylogindata()
	{
		if (instance.mylogindata != null)
		{
			instance.mylogindata.user_id = string.Empty;
			instance.mylogindata.token = string.Empty;
			instance.mylogindata.username = string.Empty;
			instance.mylogindata.ALLScroce = string.Empty;
			instance.mylogindata.login_ip = string.Empty;
			instance.mylogindata.telephone = string.Empty;
			instance.mylogindata.status = string.Empty;
			instance.mylogindata.userStatus = string.Empty;
			instance.mylogindata.coindown = 0;
			instance.mylogindata.room_id = 0;
			instance.mylogindata.choosegame = 0;
			instance.mylogindata.roomlitmit = string.Empty;
			instance.mylogindata.roomcount = string.Empty;
			instance.mylogindata.seating = string.Empty;
		}
	}

	public void loguserinformation(string where)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(where + "\n");
		stringBuilder.Append(instance.mylogindata.user_id + "\n");
		stringBuilder.Append(instance.mylogindata.token + "\n");
		stringBuilder.Append(instance.mylogindata.username + "\n");
		stringBuilder.Append(instance.mylogindata.ALLScroce + "\n");
		stringBuilder.Append(instance.mylogindata.login_ip + "\n");
		stringBuilder.Append(instance.mylogindata.telephone + "\n");
		stringBuilder.Append(instance.mylogindata.status + "\n");
		UnityEngine.Debug.Log(stringBuilder.ToString());
	}

	public void cheakisalive()
	{
	}

	public void cheakUPdate()
	{
		StartCoroutine(vesrionupdate());
	}

	private IEnumerator vesrionupdate()
	{
		while (true)
		{
			yield return getversioningame(Instance().mylogindata.URL + "api/" + Instance().mylogindata.VersioninfoAPI + "type=" + Instance().mylogindata.gameType);
			yield return new WaitForSeconds(5f);
		}
	}

	private IEnumerator getversioningame(string URL)
	{
		UnityWebRequest www = UnityWebRequest.Get(URL);
		yield return www.Send();
		JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
		if (www.error == null && www.isDone && jd["code"].ToString() == "200" && !mylogindata.version.Equals(jd["androidVersion"].ToString()))
		{
			AsyncOperation asyncOperation = Application.LoadLevelAsync(0);
			asyncOperation.allowSceneActivation = true;
		}
	}

	public void GetOnPing()
	{
		JSYS_LoginData.IsOnPing = true;
		StartCoroutine(OnWebGet(Instance().mylogindata.URL + "api/update-seat?game_id=" + Instance().mylogindata.choosegame + "&room_id=" + Instance().mylogindata.room_id + "&user_id=" + Instance().mylogindata.user_id));
	}

	public void GetOffPing()
	{
		JSYS_LoginData.IsOnPing = false;
	}

	private IEnumerator OnWebGet(string url)
	{
		yield return new WaitForSeconds(3f);
		while (true)
		{
			UnityWebRequest www = UnityWebRequest.Get(url);
			www.timeout = 1;
			yield return www.Send();
			if (www.error == null)
			{
				try
				{
					JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
					if (!(jsonData["code"].ToString() == "200"))
					{
						if (JSYS_LoginData.IsConnect)
						{
							JSYS_DisconnectPanel.GetInstance().Show();
							JSYS_DisconnectPanel.GetInstance().Modification(string.Empty, "长时间未操作，你已被移除房间");
							StartCoroutine(OnWebGet2());
						}
						yield break;
					}
					if (bool.Parse(jsonData["bool"].ToString()))
					{
						if (JSYS_LoginData.IsConnect)
						{
							JSYS_DisconnectPanel.GetInstance().Show();
							JSYS_DisconnectPanel.GetInstance().Modification(string.Empty, "长时间未操作，你已被移除房间");
							StartCoroutine(OnWebGet2());
						}
						yield break;
					}
					bool flag = true;
					for (int i = 0; i < jsonData["userData"].Count; i++)
					{
						if (jsonData["userData"][i]["id"].ToString() == Instance().mylogindata.user_id)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						if (JSYS_LoginData.IsConnect)
						{
							JSYS_DisconnectPanel.GetInstance().Show();
							JSYS_DisconnectPanel.GetInstance().Modification(string.Empty, "长时间未操作，你已被移除房间");
							StartCoroutine(OnWebGet2());
						}
						yield break;
					}
				}
				catch (Exception)
				{
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	private IEnumerator OnWebGet2()
	{
		UnityWebRequest www = UnityWebRequest.Get(Instance().mylogindata.URL + "api/room-end?user_id=" + Instance().mylogindata.user_id + "&game_id=" + Instance().mylogindata.choosegame);
		yield return www.Send();
		if (www.error == null)
		{
			try
			{
				JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
				if (jsonData["code"].ToString() == "200" && JSYS_NewTcpNet.instance != null)
				{
					JSYS_NewTcpNet.GetInstance().SocketQuit();
				}
			}
			catch
			{
			}
		}
	}

	public static bool JsonDataContainsKey(JsonData data, string key)
	{
		bool result = false;
		if (data == null)
		{
			return result;
		}
		if (!data.IsObject)
		{
			return result;
		}
		if (data == null)
		{
			return result;
		}
		if (((IDictionary)data).Contains((object)key))
		{
			result = true;
		}
		return result;
	}
}
