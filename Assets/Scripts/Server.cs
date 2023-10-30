using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class Server : MonoBehaviour
{
	private static byte[] result = new byte[1024];

	private static Socket serverSocket;

	private Thread myThread;

	[CompilerGenerated]
	private static ParameterizedThreadStart _003C_003Ef__mg_0024cache0;

	private void Start()
	{
		IPAddress address = IPAddress.Parse(ZH2_GVars.IPAddress);
		serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		serverSocket.Bind(new IPEndPoint(address, ZH2_GVars.payPort1));
		serverSocket.Listen(10);
		myThread = new Thread(ListenClientConnect);
		myThread.Start();
		Console.ReadLine();
	}

	private void ListenClientConnect()
	{
		while (true)
		{
			Socket socket = serverSocket.Accept();
			Thread thread = new Thread(ReceiveMessage);
			thread.Start(socket);
			UnityEngine.Debug.Log(socket.RemoteEndPoint.ToString());
		}
	}

	private static void ReceiveMessage(object clientSocket)
	{
		Socket socket = (Socket)clientSocket;
		while (true)
		{
			try
			{
				int num = socket.Receive(result);
				if (num > 0)
				{
					socket.Send(result, num, SocketFlags.None);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				socket.Shutdown(SocketShutdown.Both);
				socket.Close();
				return;
			}
		}
	}

	private void OnApplicationQuit()
	{
		if (myThread != null)
		{
			myThread.Abort();
			myThread = null;
		}
	}
}
