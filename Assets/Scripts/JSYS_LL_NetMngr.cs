using System.Threading;
using UnityEngine;

public class JSYS_LL_NetMngr : MonoBehaviour
{
	public static bool isInLoading;

	public static bool shouldBeBlocked;

	public static JSYS_LL_NetMngr G_NetMngr;

	public JSYS_LL_Sockets MyCreateSocket;

	private JSYS_LL_Transmit MyTransmit;

	private JSYS_LL_MessageControl MyMessageControl;

	public JSYS_LL_PostMessageThread MyPostMessageThread;

	private JSYS_LL_DataEncrypt MyDataEncrypt;

	private Thread _ConnectThread;

	private bool _isNetCheckVersionEnd;

	private float fSendTime;

	public bool mIsGetIP;

	public bool mIsFirstConnect = true;

	public static JSYS_LL_NetMngr GetSingleton()
	{
		return G_NetMngr;
	}

	private void Awake()
	{
		if (G_NetMngr == null)
		{
			G_NetMngr = this;
		}
		JSYS_LL_Constants.Version = "1.2.14";
	}

	private void ConnectServer()
	{
		MyCreateSocket = JSYS_LL_Sockets.GetSingleton();
		MyTransmit = JSYS_LL_Transmit.GetSingleton();
		MyMessageControl = JSYS_LL_MessageControl.GetSingleton();
		MyPostMessageThread = JSYS_LL_PostMessageThread.GetSingleton();
		MyDataEncrypt = JSYS_LL_DataEncrypt.GetSingleton();
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
