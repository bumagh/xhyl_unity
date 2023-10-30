using System;
using System.Collections;
using UnityEngine;

public class XLDT_NetMain : MonoBehaviour
{
	public static XLDT_NetMain G_NetMngr;

	public bool mIsGetIP;

	public bool mIsFirstConnect = true;

	[HideInInspector]
	public XLDT_Sockets MyCreateSocket;

	private XLDT_Transmit MyTransmit;

	private XLDT_MessageControl MyMessageControl;

	private XLDT_PostMessageThread MyPostMessageThread;

	private XLDT_DataEncrypt MyDataEncrypt;

	private long lCurTickTime;

	public static XLDT_NetMain GetSingleton()
	{
		return G_NetMngr;
	}

	private void Awake()
	{
		if (G_NetMngr == null)
		{
			G_NetMngr = this;
		}
		XLDT_Constants.Version = "1.2.14";
	}

	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
	}

	private void ConnectServer()
	{
		MyCreateSocket = XLDT_Sockets.GetSingleton();
		MyTransmit = XLDT_Transmit.GetSingleton();
		MyMessageControl = XLDT_MessageControl.GetSingleton();
		MyPostMessageThread = XLDT_PostMessageThread.GetSingleton();
		MyDataEncrypt = XLDT_DataEncrypt.GetSingleton();
		MyMessageControl.MessageControlParaInit();
		MyPostMessageThread.PostMessageThreadParaInit();
		MyCreateSocket.CreateSocketGetPoint(MyMessageControl, MyDataEncrypt);
		MyTransmit.TransmitGetPoint(MyCreateSocket);
		MyPostMessageThread.PostMessageThreadGetPoint(MyMessageControl, MyTransmit);
	}

	private void Update()
	{
		if (mIsGetIP && mIsFirstConnect)
		{
			ConnectServer();
			mIsFirstConnect = false;
		}
		if (!XLDT_GameInfo.getInstance().NetShouldBlocked && MyPostMessageThread != null)
		{
			MyPostMessageThread.PostThread();
		}
	}

	private void OnDestroy()
	{
		UnityEngine.Debug.Log("socket close");
		if (MyCreateSocket != null)
		{
			MyCreateSocket.SocketClose();
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			lCurTickTime = DateTime.Now.Ticks;
			return;
		}
		long num = DateTime.Now.Ticks - lCurTickTime;
		if (num > 100000000 && lCurTickTime != 0 && MyPostMessageThread != null)
		{
			MyPostMessageThread.ClearAllState();
		}
	}
}
