using System.Threading;
using UnityEngine;

public class STMF_NetMngr : MonoBehaviour
{
	public static STMF_NetMngr G_NetMngr;

	public bool mIsGetIP;

	public bool mIsFirstConnect = true;

	public STMF_Sockets MyCreateSocket;

	private STMF_Transmit MyTransmit;

	private STMF_MessageControl MyMessageControl;

	public STMF_PostMessageThread MyPostMessageThread;

	private STMF_DataEncrypt MyDataEncrypt;

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

	public static STMF_NetMngr GetSingleton()
	{
		return G_NetMngr;
	}

	private void Awake()
	{
		if (G_NetMngr == null)
		{
			G_NetMngr = this;
		}
		STMF_Constants.Version = "1.2.14";
	}

	private void ConnectServer()
	{
		MyCreateSocket = STMF_Sockets.GetSingleton();
		MyTransmit = STMF_Transmit.GetSingleton();
		MyMessageControl = STMF_MessageControl.GetSingleton();
		MyPostMessageThread = STMF_PostMessageThread.GetSingleton();
		MyDataEncrypt = STMF_DataEncrypt.GetSingleton();
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
		if (!STMF_GameInfo.getInstance().NetShouldBlocked && MyPostMessageThread != null)
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
}
