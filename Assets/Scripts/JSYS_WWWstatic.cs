using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JSYS_WWWstatic : MonoBehaviour
{
	public delegate void mymessagetlist(UnityWebRequest unitywebinfo);

	public delegate void mymessagetowwwlist(JSYS_wwwinfo infomessage);

	private IEnumerator loginIE;

	private IEnumerator reasgatinID;

	public Queue<UnityWebRequest> listforloginInfo;

	public Queue<UnityWebRequest> listforhallinfo;

	public Queue<UnityWebRequest> listforgameinfo;

	public Queue<JSYS_wwwinfo> listformwwwinfo;

	public GameObject lockgo;

	public event mymessagetlist logincallback;

	public event mymessagetlist regantsionback;

	public event mymessagetlist isaliveeventback;

	public event mymessagetlist gamelisteventback;

	public event mymessagetlist roomlistevnetback;

	public event mymessagetowwwlist gamestart;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public IEnumerator sendcoutdown(string URL)
	{
		yield return StartCoroutine(couwnt(URL));
	}

	public void sendWWW(string URL, JSYS_listtype state, JSYS_action act, bool isconver)
	{
		loginIE = wwwtosever(URL, state, act, isconver);
		StartCoroutine(loginIE);
	}

	public void stopieinlogin()
	{
		StopCoroutine(loginIE);
	}

	private IEnumerator wwwtosever(string URL, JSYS_listtype state, JSYS_action act, bool isconver)
	{
		if (URL == null)
		{
			yield break;
		}
		UnityWebRequest www = UnityWebRequest.Get(URL);
		UnityEngine.Debug.Log(URL);
		yield return www.Send();
		new JSYS_wwwinfo();
		if (www.error == null)
		{
			switch (state)
			{
			case JSYS_listtype.listforloginInfo:
				JSYS_LoginInfo.Instance().wwwinstance.listforloginInfo.Enqueue(www);
				break;
			case JSYS_listtype.listforhallinfo:
				JSYS_LoginInfo.Instance().wwwinstance.listforhallinfo.Enqueue(www);
				break;
			case JSYS_listtype.listforgameinfo:
				JSYS_LoginInfo.Instance().wwwinstance.listforgameinfo.Enqueue(www);
				break;
			}
			switch (act)
			{
			case JSYS_action.login:
				if (this.logincallback != null)
				{
					this.logincallback(JSYS_LoginInfo.Instance().wwwinstance.listforloginInfo.Dequeue());
				}
				break;
			case JSYS_action.reganstion:
				if (this.regantsionback != null)
				{
					this.regantsionback(JSYS_LoginInfo.Instance().wwwinstance.listforloginInfo.Dequeue());
				}
				break;
			case JSYS_action.isalive:
				if (this.isaliveeventback != null)
				{
					this.isaliveeventback(JSYS_LoginInfo.Instance().wwwinstance.listforgameinfo.Dequeue());
				}
				break;
			case JSYS_action.gethallgame:
				if (this.gamelisteventback != null)
				{
					this.gamelisteventback(JSYS_LoginInfo.Instance().wwwinstance.listforgameinfo.Dequeue());
				}
				break;
			case JSYS_action.getroomlist:
				if (this.roomlistevnetback != null)
				{
					this.roomlistevnetback(JSYS_LoginInfo.Instance().wwwinstance.listforhallinfo.Dequeue());
				}
				break;
			}
		}
		else if (isconver)
		{
			StartCoroutine(wwwtosever(URL, state, act, isconver));
		}
	}

	private IEnumerator couwnt(string URL)
	{
		UnityWebRequest www = UnityWebRequest.Get(URL);
		yield return www.Send();
		JsonData jd = JsonMapper.ToObject(www.downloadHandler.text);
		if (www.error == null)
		{
			JSYS_wwwinfo jSYS_wwwinfo = new JSYS_wwwinfo();
			if (jd["code"].ToString() == "200")
			{
				jSYS_wwwinfo.statetype = (int)jd["info"]["is_open"];
				jSYS_wwwinfo.textinfo = www.downloadHandler.text;
				if (listformwwwinfo.Count > 0)
				{
					if (JSYS_GameMagert.iscomeback)
					{
						listformwwwinfo.Enqueue(jSYS_wwwinfo);
						JSYS_GameMagert.iscomeback = false;
						JSYS_GameMagert.swithonislistchange = true;
					}
					else if (listformwwwinfo.Peek().statetype != jSYS_wwwinfo.statetype)
					{
						listformwwwinfo.Enqueue(jSYS_wwwinfo);
					}
				}
				else
				{
					listformwwwinfo.Enqueue(jSYS_wwwinfo);
				}
			}
			else
			{
				UnityEngine.Debug.Log(jd["msg"].ToString());
			}
		}
		else
		{
			UnityEngine.Debug.Log(www.error);
		}
	}
}
