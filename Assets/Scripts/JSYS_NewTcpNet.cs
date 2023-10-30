using LitJson;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class JSYS_NewTcpNet
{
	public static bool IsQuit;

	private Thread threadClient;

	private Socket socketClient;

	public static JSYS_Loom _loom = null;

	private JSYS_OverDetection detection;

	public int prot = 7272;

	public static string ip = "47.106.191.250";

	public static JSYS_NewTcpNet instance;

	private int mode = JSYS_LoginInfo.Instance().mylogindata.choosegame;

	public static string countDown;

	public static string periods;

	public static string season;

	public static List<string> dnum = new List<string>();

	public static bool isUpdateAllDnum = false;

	public static bool isUpdateRate;

	public static bool isUpdate = false;

	private int resend;

	public static bool isFirst = false;

	private bool isPlayOver;

	private bool IsGet;

	public JSYS_NewTcpNet()
	{
		isPlayOver = false;
		IPAddress address = IPAddress.Parse(ip);
		IPEndPoint remoteEP = new IPEndPoint(address, prot);
		socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		_loom = new GameObject().AddComponent<JSYS_Loom>();
		_loom.name = "Loom";
		try
		{
			socketClient.Connect(remoteEP);
			JSYS_LoginData.IsConnect = true;
			IsQuit = false;
		}
		catch (SocketException)
		{
			JSYS_DisconnectPanel.GetInstance().Show();
			JSYS_DisconnectPanel.GetInstance().Modification(string.Empty, "连接服务器失败！！");
		}
		catch (Exception)
		{
			JSYS_DisconnectPanel.GetInstance().Show();
			JSYS_DisconnectPanel.GetInstance().Modification(string.Empty, "连接服务器失败！！");
		}
		JSYS_Loom.RunAsync(delegate
		{
			threadClient = new Thread(ReceiveMsg);
			threadClient.IsBackground = true;
			threadClient.Start();
		});
		JSYS_OnLogin obj = new JSYS_OnLogin("Login", JSYS_LoginInfo.Instance().mylogindata.user_id, JSYS_LoginInfo.Instance().mylogindata.room_id.ToString(), JSYS_LoginInfo.Instance().mylogindata.choosegame.ToString());
		string data = JsonMapper.ToJson(obj);
		SendMessage(data);
	}

	public JSYS_NewTcpNet(int a)
	{
		isPlayOver = false;
		IPAddress address = IPAddress.Parse(ip);
		IPEndPoint remoteEP = new IPEndPoint(address, prot);
		socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		_loom = new GameObject().AddComponent<JSYS_Loom>();
		_loom.name = "Loom";
		try
		{
			socketClient.Connect(remoteEP);
			JSYS_LoginData.IsConnect = true;
			IsQuit = false;
		}
		catch (SocketException)
		{
			JSYS_DisconnectPanel.GetInstance().Show();
			JSYS_DisconnectPanel.GetInstance().Modification(string.Empty, "连接服务器失败！！");
		}
		catch (Exception)
		{
			JSYS_DisconnectPanel.GetInstance().Show();
			JSYS_DisconnectPanel.GetInstance().Modification(string.Empty, "连接服务器失败！！");
		}
		JSYS_Loom.RunAsync(delegate
		{
			threadClient = new Thread(ReceiveMsg);
			threadClient.IsBackground = true;
			threadClient.Start();
		});
	}

	public static JSYS_NewTcpNet GetInstance()
	{
		if (instance == null)
		{
			instance = new JSYS_NewTcpNet();
		}
		return instance;
	}

	public static JSYS_NewTcpNet GetInstance2()
	{
		if (instance == null)
		{
			instance = new JSYS_NewTcpNet(0);
		}
		return instance;
	}

	public static void GetLoom()
	{
	}

	public void SendMessage(string data)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(data);
		byte[] array = new byte[bytes.Length];
		try
		{
			socketClient.Send(bytes);
		}
		catch (SocketException)
		{
		}
		catch (Exception)
		{
		}
	}

	private void ReceiveMsg()
	{
		while (true)
		{
			byte[] array = new byte[8192];
			int num = -1;
			try
			{
				num = socketClient.Receive(array);
			}
			catch (SocketException)
			{
				return;
			}
			catch (Exception)
			{
				return;
			}
			string @string = Encoding.UTF8.GetString(array, 0, num);
			string[] array2 = @string.Split(new string[1]
			{
				"xx=="
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array2.Length; i++)
			{
				try
				{
					JsonData jsonData = JsonMapper.ToObject(array2[i]);
					if (JSYS_LoginInfo.Instance().mylogindata.choosegame != 28 && JSYS_LoginInfo.Instance().mylogindata.choosegame != 32)
					{
						JSYS_LoginData.IsLogin = true;
						JSYS_LoginData.OverTime = 0f;
					}
				}
				catch (Exception)
				{
					if (JSYS_LoginInfo.Instance().mylogindata.choosegame != 28 && JSYS_LoginInfo.Instance().mylogindata.choosegame != 32)
					{
						JSYS_LoginData.IsLogin = true;
						JSYS_LoginData.OverTime = 2f;
					}
				}
				Message(array2[i]);
			}
		}
	}

	public bool GetConnectionStatus()
	{
		return socketClient.Connected;
	}

	public void SocketQuit()
	{
		if (_loom != null)
		{
			UnityEngine.Object.Destroy(_loom.gameObject);
		}
		JSYS_LoginData.IsLogin = false;
		JSYS_LoginData.OverTime = 0f;
		if (threadClient != null)
		{
			threadClient.Interrupt();
			threadClient.Abort();
			threadClient = null;
			instance = null;
		}
		if (socketClient != null)
		{
			socketClient.Close();
		}
	}

	public void Message(string str)
	{
		JsonData jd = JsonMapper.ToObject(str);
		int num = mode;
		if (num == 23)
		{
			DisposeJSYS(jd);
		}
	}

	public void DisposeJSYS(JsonData jd)
	{
		string text = jd["type"].ToString();
		if (text != null && !(text == "ping") && text == "Periods-jsys")
		{
			JSYS_Loom.QueueOnMainThread(delegate
			{
			});
		}
	}
}
