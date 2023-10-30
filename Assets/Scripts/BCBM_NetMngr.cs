using System.Threading;
using UnityEngine;

public class BCBM_NetMngr : MonoBehaviour
{
	public static bool isInLoading;

	public static bool shouldBeBlocked;

	public static BCBM_NetMngr G_NetMngr;

	public BCBM_Sockets MyCreateSocket;

	private BCBM_Transmit MyTransmit;

	private BCBM_MessageControl MyMessageControl;

	public BCBM_PostMessageThread MyPostMessageThread;

	private BCBM_DataEncrypt MyDataEncrypt;

	private Thread _ConnectThread;

	private bool _isNetCheckVersionEnd;

	private float fSendTime;

	public bool mIsGetIP;

	public bool mIsFirstConnect = true;

	public static BCBM_NetMngr GetSingleton()
	{
		return G_NetMngr;
	}

	private void Awake()
	{
		if (G_NetMngr == null)
		{
			G_NetMngr = this;
		}
		BCBM_Constants.Version = "1.2.14";
	}

	private void ConnectServer()
	{
		MyCreateSocket = BCBM_Sockets.GetSingleton();
		MyTransmit = BCBM_Transmit.GetSingleton();
		MyMessageControl = BCBM_MessageControl.GetSingleton();
		MyPostMessageThread = BCBM_PostMessageThread.GetSingleton();
		MyDataEncrypt = BCBM_DataEncrypt.GetSingleton();
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
		UnityEngine.Debug.LogError("=======2");
		MyCreateSocket.SendQuitGame();
	}
}
