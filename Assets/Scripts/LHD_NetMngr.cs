using System.Threading;
using UnityEngine;

public class LHD_NetMngr : MonoBehaviour
{
	public static bool isInLoading;

	public static bool shouldBeBlocked;

	public static LHD_NetMngr G_NetMngr;

	public LHD_Sockets MyCreateSocket;

	private LHD_Transmit MyTransmit;

	private LHD_MessageControl MyMessageControl;

	public LHD_PostMessageThread MyPostMessageThread;

	private LHD_DataEncrypt MyDataEncrypt;

	private Thread _ConnectThread;

	private bool _isNetCheckVersionEnd;

	private float fSendTime;

	public bool mIsGetIP;

	public bool mIsFirstConnect = true;

	public static LHD_NetMngr GetSingleton()
	{
		return G_NetMngr;
	}

	private void Awake()
	{
		if (G_NetMngr == null)
		{
			G_NetMngr = this;
		}
		LHD_Constants.Version = "1.2.14";
	}

	private void ConnectServer()
	{
		MyCreateSocket = LHD_Sockets.GetSingleton();
		MyTransmit = LHD_Transmit.GetSingleton();
		MyMessageControl = LHD_MessageControl.GetSingleton();
		MyPostMessageThread = LHD_PostMessageThread.GetSingleton();
		MyDataEncrypt = LHD_DataEncrypt.GetSingleton();
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
		if (!shouldBeBlocked && mIsGetIP)
		{
			MyPostMessageThread.PostThread();
			fSendTime += Time.deltaTime;
		}
	}

	private void OnDestroy()
	{
		MyCreateSocket.SocketClose();
		if (_ConnectThread != null && _ConnectThread.IsAlive)
		{
			_ConnectThread.Abort();
		}
	}

	private void OnApplicationQuit()
	{
		MyCreateSocket.SendQuitGame();
	}
}
