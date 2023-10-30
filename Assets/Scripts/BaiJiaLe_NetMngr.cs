using System.Threading;
using UnityEngine;

public class BaiJiaLe_NetMngr : MonoBehaviour
{
	public static BaiJiaLe_NetMngr G_NetMngr;

	public static bool isInLoading;

	public BaiJiaLe_Sockets MyCreateSocket;

	private BaiJiaLe_Transmit MyTransmit;

	private BaiJiaLe_MessageControl MyMessageControl;

	private BaiJiaLe_DataEncrypt MyDataEncrypt;

	private Thread _ConnectThread;

	private bool _isNetCheckVersionEnd;

	private float fSendTime;

	public BaiJiaLe_PostMessageThread MyBaiJiaLe_PostMessageThread;

	public bool mIsGetIP;

	public bool mIsFirstConnect = true;

	public static BaiJiaLe_NetMngr GetSingleton()
	{
		return G_NetMngr;
	}

	private void Awake()
	{
		if (G_NetMngr == null)
		{
			G_NetMngr = this;
		}
		BaiJiaLe_Constants.Version = "1.2.14";
	}

	private void ConnectServer()
	{
		MyCreateSocket = BaiJiaLe_Sockets.GetSingleton();
		MyTransmit = BaiJiaLe_Transmit.GetSingleton();
		MyMessageControl = BaiJiaLe_MessageControl.GetSingleton();
		MyBaiJiaLe_PostMessageThread = BaiJiaLe_PostMessageThread.GetSingleton();
		MyDataEncrypt = BaiJiaLe_DataEncrypt.GetSingleton();
		MyMessageControl.MessageControlParaInit();
		MyBaiJiaLe_PostMessageThread.BaiJiaLe_PostMessageThreadParaInit();
		MyCreateSocket.CreateSocketGetPoint(MyMessageControl, MyDataEncrypt);
		MyTransmit.TransmitGetPoint(MyCreateSocket);
		MyBaiJiaLe_PostMessageThread.BaiJiaLe_PostMessageThreadGetPoint(MyMessageControl, MyTransmit);
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
		if (!BaiJiaLe_GameInfo.getInstance().NetShouldBlocked && mIsGetIP)
		{
			MyBaiJiaLe_PostMessageThread.PostThread();
			fSendTime += Time.deltaTime;
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
