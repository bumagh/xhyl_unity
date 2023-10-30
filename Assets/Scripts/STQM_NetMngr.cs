using System.Threading;
using UnityEngine;

public class STQM_NetMngr : MonoBehaviour
{
	public static STQM_NetMngr G_NetMngr;

	public bool mIsGetIP;

	public bool mIsFirstConnect = true;

	public STQM_Sockets MyCreateSocket;

	private STQM_Transmit MyTransmit;

	private STQM_MessageControl MyMessageControl;

	public STQM_PostMessageThread MyPostMessageThread;

	private STQM_DataEncrypt MyDataEncrypt;

	private Thread _ConnectThread;

	private float fSendTime;

	private bool _isGameSceneLoadOk;

	public int mSceneBg = -1;

	private int s_lastHeart = -999;

	private string s_lastStrError = string.Empty;

	private int s_lastRcvCount = -999;

	private string s_timeTooLongStr = string.Empty;

	public bool IsGameSceneLoadOk
	{
		get
		{
			return _isGameSceneLoadOk;
		}
		set
		{
			_isGameSceneLoadOk = value;
		}
	}

	public static STQM_NetMngr GetSingleton()
	{
		return G_NetMngr;
	}

	private void Awake()
	{
		if (G_NetMngr == null)
		{
			G_NetMngr = this;
		}
		STQM_Constants.Version = "1.2.14";
	}

	private void ConnectServer()
	{
		MyCreateSocket = STQM_Sockets.GetSingleton();
		MyTransmit = STQM_Transmit.GetSingleton();
		MyMessageControl = STQM_MessageControl.GetSingleton();
		MyPostMessageThread = STQM_PostMessageThread.GetSingleton();
		MyDataEncrypt = STQM_DataEncrypt.GetSingleton();
		MyMessageControl.MessageControlParaInit();
		MyPostMessageThread.PostMessageThreadParaInit();
		MyCreateSocket.CreateSocketGetPoint(MyMessageControl, MyDataEncrypt);
		MyTransmit.TransmitGetPoint(MyCreateSocket);
		MyPostMessageThread.PostMessageThreadGetPoint(MyMessageControl, MyTransmit);
	}

	public void CreateRcvThread()
	{
		if (_ConnectThread == null || !_ConnectThread.IsAlive)
		{
			UnityEngine.Debug.Log("-------");
			_ConnectThread = new Thread(MyCreateSocket.SocketConnect);
			_ConnectThread.Start();
		}
	}

	private void Update()
	{
		if (mIsGetIP && mIsFirstConnect)
		{
			ConnectServer();
			mIsFirstConnect = false;
		}
		if (!STQM_GameInfo.getInstance().NetShouldBlocked && MyPostMessageThread != null)
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

	private void OnApplicationQuit()
	{
	}
}
