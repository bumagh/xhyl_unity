using System.Threading;
using UnityEngine;

public class DNTG_NetMngr : MonoBehaviour
{
	public static DNTG_NetMngr G_NetMngr;

	public bool mIsGetIP;

	public bool mIsFirstConnect = true;

	public DNTG_Sockets MyCreateSocket;

	private DNTG_Transmit MyTransmit;

	private DNTG_MessageControl MyMessageControl;

	public DNTG_PostMessageThread MyPostMessageThread;

	private DNTG_DataEncrypt MyDataEncrypt;

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

	public static DNTG_NetMngr GetSingleton()
	{
		return G_NetMngr;
	}

	private void Awake()
	{
		if (G_NetMngr == null)
		{
			G_NetMngr = this;
		}
		DNTG_Constants.Version = "1.2.14";
	}

	private void ConnectServer()
	{
		MyCreateSocket = DNTG_Sockets.GetSingleton();
		MyTransmit = DNTG_Transmit.GetSingleton();
		MyMessageControl = DNTG_MessageControl.GetSingleton();
		MyPostMessageThread = DNTG_PostMessageThread.GetSingleton();
		MyDataEncrypt = DNTG_DataEncrypt.GetSingleton();
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
		if (!DNTG_GameInfo.getInstance().NetShouldBlocked && MyPostMessageThread != null)
		{
			MyPostMessageThread.PostThread();
		}
	}

	private void OnDestroy()
	{
		if (MyCreateSocket != null)
		{
			MyCreateSocket.SocketClose();
		}
	}

	private void OnApplicationQuit()
	{
		MyCreateSocket.SendQuitGame();
	}

	public void NetDestroy()
	{
		MyCreateSocket.SendQuitGame();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
