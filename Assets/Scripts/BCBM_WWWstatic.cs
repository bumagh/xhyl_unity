using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BCBM_WWWstatic : MonoBehaviour
{
	public delegate void mymessagetlist(UnityWebRequest unitywebinfo);

	public delegate void mymessagetowwwlist(BCBM_wwwinfo infomessage);

	private IEnumerator loginIE;

	private IEnumerator reasgatinID;

	public Queue<UnityWebRequest> listforloginInfo;

	public Queue<UnityWebRequest> listforhallinfo;

	public Queue<UnityWebRequest> listforgameinfo;

	public Queue<BCBM_wwwinfo> listformwwwinfo;

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

	public void sendWWW(string URL, BCBM_listtype state, BCBM_action act, bool isconver)
	{
		loginIE = wwwtosever(URL, state, act, isconver);
		StartCoroutine(loginIE);
	}

	public void stopieinlogin()
	{
		StopCoroutine(loginIE);
	}

	private IEnumerator wwwtosever(string URL, BCBM_listtype state, BCBM_action act, bool isconver)
	{
		if (URL == null)
		{
			yield break;
		}
		UnityWebRequest www = UnityWebRequest.Get(URL);
		UnityEngine.Debug.Log(URL);
		yield return www.Send();
		new BCBM_wwwinfo();
		if (www.error == null)
		{
			switch (state)
			{
			case BCBM_listtype.listforloginInfo:
				BCBM_LoginInfo.Instance().wwwinstance.listforloginInfo.Enqueue(www);
				break;
			case BCBM_listtype.listforhallinfo:
				BCBM_LoginInfo.Instance().wwwinstance.listforhallinfo.Enqueue(www);
				break;
			case BCBM_listtype.listforgameinfo:
				BCBM_LoginInfo.Instance().wwwinstance.listforgameinfo.Enqueue(www);
				break;
			}
			switch (act)
			{
			case BCBM_action.login:
				if (this.logincallback != null)
				{
					this.logincallback(BCBM_LoginInfo.Instance().wwwinstance.listforloginInfo.Dequeue());
				}
				break;
			case BCBM_action.reganstion:
				if (this.regantsionback != null)
				{
					this.regantsionback(BCBM_LoginInfo.Instance().wwwinstance.listforloginInfo.Dequeue());
				}
				break;
			case BCBM_action.isalive:
				if (this.isaliveeventback != null)
				{
					this.isaliveeventback(BCBM_LoginInfo.Instance().wwwinstance.listforgameinfo.Dequeue());
				}
				break;
			case BCBM_action.gethallgame:
				if (this.gamelisteventback != null)
				{
					this.gamelisteventback(BCBM_LoginInfo.Instance().wwwinstance.listforgameinfo.Dequeue());
				}
				break;
			case BCBM_action.getroomlist:
				if (this.roomlistevnetback != null)
				{
					this.roomlistevnetback(BCBM_LoginInfo.Instance().wwwinstance.listforhallinfo.Dequeue());
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
			BCBM_wwwinfo bCBM_wwwinfo = new BCBM_wwwinfo();
			if (jd["code"].ToString() == "200")
			{
				bCBM_wwwinfo.statetype = (int)jd["info"]["is_open"];
				bCBM_wwwinfo.textinfo = www.downloadHandler.text;
				if (listformwwwinfo.Count > 0)
				{
					if (BCBM_GameMagert.iscomeback)
					{
						listformwwwinfo.Enqueue(bCBM_wwwinfo);
						BCBM_GameMagert.iscomeback = false;
						BCBM_GameMagert.swithonislistchange = true;
					}
					else if (listformwwwinfo.Peek().statetype != bCBM_wwwinfo.statetype)
					{
						listformwwwinfo.Enqueue(bCBM_wwwinfo);
					}
				}
				else
				{
					listformwwwinfo.Enqueue(bCBM_wwwinfo);
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
