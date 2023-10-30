using System.Threading;
using UnityEngine;

public class DP_NetMngr : MonoBehaviour
{
	public static bool isInLoading;

	public static bool shouldBeBlocked;

	public static DP_NetMngr G_NetMngr;

	public DP_Sockets MyCreateSocket;

	private DP_Transmit MyTransmit;

	private DP_MessageControl MyMessageControl;

	public DP_PostMessageThread MyPostMessageThread;

	private DP_DataEncrypt MyDataEncrypt;

	private Thread _ConnectThread;

	private bool _isNetCheckVersionEnd;

	private float fSendTime;

	public bool mIsGetIP;

	public bool mIsFirstConnect = true;

	public static DP_NetMngr GetSingleton()
	{
		return G_NetMngr;
	}

	private void Awake()
	{
		if (G_NetMngr == null)
		{
			G_NetMngr = this;
		}
	}

	private void ConnectServer()
	{
		MyCreateSocket = DP_Sockets.GetSingleton();
		MyTransmit = DP_Transmit.GetSingleton();
		MyMessageControl = DP_MessageControl.GetSingleton();
		MyPostMessageThread = DP_PostMessageThread.GetSingleton();
		MyDataEncrypt = DP_DataEncrypt.GetSingleton();
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
