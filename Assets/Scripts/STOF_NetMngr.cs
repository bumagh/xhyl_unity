using System.Threading;
using UnityEngine;

public class STOF_NetMngr : MonoBehaviour
{
	public static STOF_NetMngr G_NetMngr;

	public bool mIsGetIP;

	public bool mIsFirstConnect = true;

	public STOF_Sockets MyCreateSocket;

	private STOF_Transmit MyTransmit;

	private STOF_MessageControl MyMessageControl;

	public STOF_PostMessageThread MyPostMessageThread;

	private STOF_DataEncrypt MyDataEncrypt;

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

	public static STOF_NetMngr GetSingleton()
	{
		return G_NetMngr;
	}

	private void Awake()
	{
		if (G_NetMngr == null)
		{
			G_NetMngr = this;
		}
		STOF_Constants.Version = "1.2.14";
	}

	private void ConnectServer()
	{
		MyCreateSocket = STOF_Sockets.GetSingleton();
		MyTransmit = STOF_Transmit.GetSingleton();
		MyMessageControl = STOF_MessageControl.GetSingleton();
		MyPostMessageThread = STOF_PostMessageThread.GetSingleton();
		MyDataEncrypt = STOF_DataEncrypt.GetSingleton();
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
		if (!STOF_GameInfo.getInstance().NetShouldBlocked && MyPostMessageThread != null)
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
		MyCreateSocket.SendQuitGame();
	}

	public void NetDestroy()
	{
		MyCreateSocket.SendQuitGame();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
