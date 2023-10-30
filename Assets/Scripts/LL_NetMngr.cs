using System.Threading;
using UnityEngine;

public class LL_NetMngr : MonoBehaviour
{
	public static bool isInLoading;

	public static bool shouldBeBlocked;

	public static LL_NetMngr G_NetMngr;

	public LL_Sockets MyCreateSocket;

	private LL_Transmit MyTransmit;

	private LL_MessageControl MyMessageControl;

	public LL_PostMessageThread MyPostMessageThread;

	private LL_DataEncrypt MyDataEncrypt;

	private Thread _ConnectThread;

	private bool _isNetCheckVersionEnd;

	private float fSendTime;

	public bool mIsGetIP;

	public bool mIsFirstConnect = true;

	public static LL_NetMngr GetSingleton()
	{
		return G_NetMngr;
	}

	private void Awake()
	{
		if (G_NetMngr == null)
		{
			G_NetMngr = this;
		}
		LL_Constants.Version = "1.2.14";
	}

	private void ConnectServer()
	{
		MyCreateSocket = LL_Sockets.GetSingleton();
		MyTransmit = LL_Transmit.GetSingleton();
		MyMessageControl = LL_MessageControl.GetSingleton();
		MyPostMessageThread = LL_PostMessageThread.GetSingleton();
		MyDataEncrypt = LL_DataEncrypt.GetSingleton();
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
