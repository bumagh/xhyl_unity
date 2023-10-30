using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;
using UnityEngine;
using wox.serial;

public class DP_Sockets : MonoBehaviour
{
	public bool isReconnect = true;

	private static DP_Sockets _MyCreateSocket;

	private DP_MessageControl m_MyMessageControl;

	private DP_DataEncrypt m_DataEncrypt;

	public Socket MySocket;

	private Thread m_nListenerThread;

	private bool isReconnectRestricted;

	private Coroutine coRestrictReconnect;

	private Thread m_nReceiveThread;

	private int m_nRelineCount;

	private bool m_nSocketStartFlag;

	private bool sendCheckVersion;

	private long m_recriveHeartTime;

	private long m_sendHeartTime;

	public static DP_Sockets GetSingleton()
	{
		return _MyCreateSocket;
	}

	private void Awake()
	{
		if (_MyCreateSocket == null)
		{
			UnityEngine.Debug.Log("Sockets");
			_MyCreateSocket = this;
		}
	}

	public void CreateSocketGetPoint(DP_MessageControl MyMessageControl, DP_DataEncrypt MyDataEncrypt)
	{
		CreateReceiveThread();
		CreateListenerThread();
		m_MyMessageControl = MyMessageControl;
		m_DataEncrypt = MyDataEncrypt;
	}

	private void CreateListenerThread()
	{
		isReconnect = true;
		try
		{
			m_nListenerThread.Abort();
			m_nListenerThread = null;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("ListenerThread:" + ex.Message);
		}
		m_nListenerThread = new Thread(socketListener);
		m_nListenerThread.Start();
	}

	private IEnumerator RestrictReconnect()
	{
		isReconnectRestricted = true;
		yield return new WaitForSeconds(2.1f);
		isReconnectRestricted = false;
	}

	public int GetRelineCount()
	{
		return m_nRelineCount;
	}

	public bool GetSocketStartFlag()
	{
		return m_nSocketStartFlag;
	}

	public void CreateReceiveThread()
	{
		try
		{
			m_nReceiveThread.Abort();
			m_nReceiveThread = null;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("ReceiveThread" + ex.Message);
		}
		if (coRestrictReconnect != null)
		{
			StopCoroutine("coRestrictReconnect");
		}
		coRestrictReconnect = StartCoroutine(RestrictReconnect());
		m_nReceiveThread = new Thread(SocketConnect);
		m_nReceiveThread.Start();
	}

	public void SocketClose()
	{
		if (MySocket != null)
		{
			MySocket.Close();
			MySocket = null;
		}
		try
		{
			m_nReceiveThread.Abort();
			m_nReceiveThread = null;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("ReceiveThread:" + ex.Message);
		}
		try
		{
			m_nListenerThread.Abort();
			m_nListenerThread = null;
		}
		catch (Exception ex2)
		{
			UnityEngine.Debug.Log("ListenerThread:" + ex2.Message);
		}
	}

	public void SocketConnect()
	{
		Thread.Sleep(isReconnectRestricted ? 2000 : 100);
		m_nSocketStartFlag = false;
		m_nSocketStartFlag = _socketStart();
		if (m_nSocketStartFlag)
		{
			if (!sendCheckVersion)
			{
				SendCheckVersion("9.0.1");
				sendCheckVersion = true;
			}
			else
			{
				sendPublicKey();
			}
			_myReceiveControl();
		}
	}

	private bool _socketStart()
	{
		MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(DP_GameInfo.getInstance().IP), 19925);
		try
		{
			MySocket.Connect(remoteEP);
			m_nRelineCount = 0;
			m_recriveHeartTime = _getCurTime();
			m_sendHeartTime = _getCurTime();
			return true;
		}
		catch (SocketException ex)
		{
			UnityEngine.Debug.Log("connect:" + ex.Message);
			_netDownControl();
			return false;
		}
	}

	private void _myReceiveControl()
	{
		int num = 0;
		MemoryStream memoryStream = new MemoryStream();
		while (true)
		{
			byte[] buffer = new byte[1024];
			try
			{
				num = MySocket.Receive(buffer);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("Receive:" + ex.Message);
				_netDownControl();
				return;
			}
			if (num <= 0)
			{
				break;
			}
			if (num > 0)
			{
				memoryStream.Position = memoryStream.Length;
				memoryStream.Write(buffer, 0, num);
			}
			while (memoryStream.Length >= 4)
			{
				memoryStream.Position = 0L;
				byte[] array = new byte[4];
				memoryStream.Read(array, 0, 4);
				Array.Reverse((Array)array);
				int num2 = BitConverter.ToInt32(array, 0);
				if (memoryStream.Length - 4 < num2)
				{
					break;
				}
				memoryStream.Position = 4L;
				byte[] array2 = new byte[num2];
				memoryStream.Read(array2, 0, num2);
				int num3 = (int)(memoryStream.Length - num2 - 4);
				byte[] buffer2 = new byte[num3];
				memoryStream.Position = num2 + 4;
				memoryStream.Read(buffer2, 0, num3);
				memoryStream = new MemoryStream();
				memoryStream.Position = 0L;
				memoryStream.Write(buffer2, 0, num3);
				string @string = Encoding.UTF8.GetString(array2);
				_receiveNetMessage(@string);
			}
		}
		_netDownControl();
	}

	private void _receiveNetMessage(string msg)
	{
		if (m_DataEncrypt.GetKey() != "none")
		{
			msg = m_DataEncrypt.Decrypt(msg);
		}
		XmlReader xmlReader = XmlReader.Create(new StringReader(msg));
		xmlReader.Read();
		SimpleReader simpleReader = new SimpleReader();
		Hashtable hashtable = simpleReader.read(xmlReader) as Hashtable;
		string text = hashtable["method"].ToString();
		object[] array = hashtable["args"] as object[];
		UnityEngine.Debug.Log("receiceMsg>>>Net:" + text);
		string text2 = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			string text3 = text2;
			text2 = text3 + i + ":" + array[i].ToString() + " ";
		}
		if (text == "sendServerTime")
		{
			Hashtable hashtable2 = new Hashtable();
			hashtable2 = (array[0] as Hashtable);
			long serverTime = (long)hashtable2["time"];
			string str = (string)hashtable2["key"];
			m_DataEncrypt.setServerTime(serverTime);
			m_DataEncrypt.DecryptKey(str);
		}
		else if (text == "heart")
		{
			m_recriveHeartTime = _getCurTime();
			return;
		}
		m_MyMessageControl.AddMessage(hashtable);
	}

	private long _getCurTime()
	{
		DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
		DateTime now = DateTime.Now;
		return (long)Math.Round((now - d).TotalMilliseconds, MidpointRounding.AwayFromZero);
	}

	public void socketListener()
	{
		Thread.Sleep(5000);
		long num = 0L;
		while (true)
		{
			num = _getCurTime();
			if (num - m_recriveHeartTime > 20000)
			{
				try
				{
					m_nReceiveThread.Abort();
					m_nReceiveThread = null;
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.Log("ReceiveThread:" + ex.Message);
				}
				_netDownControl();
				m_recriveHeartTime = num;
				continue;
			}
			if (num - m_sendHeartTime >= 5000 && MySocket != null)
			{
				SendHeart();
				m_sendHeartTime = num;
			}
			Thread.Sleep(500);
		}
	}

	private bool _sendMsg(string strMethod, object[] args)
	{
		LL_SimpleWriter lL_SimpleWriter = new LL_SimpleWriter();
		StringWriter stringWriter = new StringWriter();
		XmlTextWriter writer = new XmlTextWriter(stringWriter);
		lL_SimpleWriter.write(new Hashtable
		{
			{
				"method",
				strMethod
			},
			{
				"args",
				args
			},
			{
				"version",
				"1.2.14"
			},
			{
				"time",
				m_DataEncrypt.GetUnixTime()
			}
		}, writer);
		if (m_DataEncrypt.GetKey() != "none")
		{
			string str = string.Empty;
			for (int i = 0; i < args.Length; i++)
			{
				str = str + args[i].ToString() + " ";
			}
			return _send(m_DataEncrypt.Encrypt(stringWriter.ToString()));
		}
		return strMethod == "userService/checkVersion" && _send(stringWriter.ToString());
	}

	private bool _send(string msg)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(msg);
		byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
		byte[] array = new byte[bytes.Length + 4];
		Array.Reverse((Array)bytes2);
		Buffer.BlockCopy(bytes2, 0, array, 0, 4);
		Buffer.BlockCopy(bytes, 0, array, 4, bytes.Length);
		try
		{
			int num = MySocket.Send(array, array.Length, SocketFlags.None);
			if (num == array.Length)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(ex.ToString());
			return false;
		}
	}

	private void _netDownControl()
	{
		if (MySocket != null)
		{
			MySocket.Close();
			MySocket = null;
		}
		m_DataEncrypt.KeyReset();
		m_nRelineCount++;
		_tellUINetDown();
	}

	private void _tellUINetDown()
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("method", "NetThread/NetDown");
		hashtable.Add("args", new object[0]);
		m_MyMessageControl.AddMessage(hashtable);
	}

	public bool SendCheckVersion(string versionCode = "")
	{
		UnityEngine.Debug.Log("SendCheckVersion");
		string strMethod = "userService/checkVersion";
		object[] args = new object[1]
		{
			versionCode
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendUserLogin(string strUsername, string strPassword, int nUiId, string versionCode = "")
	{
		string strMethod = "userService/userLogin";
		object[] args = new object[4]
		{
			strUsername,
			strPassword,
			nUiId,
			versionCode
		};
		UnityEngine.Debug.Log("userService/userLogin");
		return _sendMsg(strMethod, args);
	}

	public bool SendHeart()
	{
		string strMethod = "userService/heart";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool SendEnterRoom(int roomId)
	{
		string strMethod = "userService/enterRoom";
		object[] args = new object[1]
		{
			roomId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendLeaveRoom(int nRoomId)
	{
		string strMethod = "userService/leaveRoom";
		object[] args = new object[1]
		{
			nRoomId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendAutoAddExpeGold()
	{
		string strMethod = "userService/addExpeGoldAuto";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool SendDeskInfo(int nDeskId)
	{
		string strMethod = "userService/deskInfo";
		object[] args = new object[1]
		{
			nDeskId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendLeaveDesk(int nDeskId)
	{
		string strMethod = "userService/leaveDesk";
		object[] args = new object[1]
		{
			nDeskId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendSelectSeat(int nDeskId, int nSeatId)
	{
		string strMethod = "userService/selectSeat";
		object[] args = new object[2]
		{
			nDeskId,
			nSeatId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendLeaveSeat(int nDeskId, int nSeatId)
	{
		string strMethod = "userService/leaveSeat";
		object[] args = new object[2]
		{
			nDeskId,
			nSeatId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendUserBet(int nBet, int nScore, int nDeskID)
	{
		string strMethod = "userService/userBet";
		object[] args = new object[3]
		{
			nBet,
			nScore,
			nDeskID
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendCancelBet(int nDeskID)
	{
		string strMethod = "userService/cancelBet";
		object[] args = new object[1]
		{
			nDeskID
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendPlayerInfo(int userId)
	{
		string strMethod = "userService/playerInfo";
		object[] args = new object[1]
		{
			userId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendSendChat(int nChatType, int nReceiverUserId, string chatMessage)
	{
		string strMethod = "userService/sendChat";
		object[] args = new object[3]
		{
			nChatType,
			nReceiverUserId,
			chatMessage
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendUserCoinIn(int nCoinInNumber)
	{
		string strMethod = "userService/userCoinIn";
		object[] args = new object[1]
		{
			nCoinInNumber
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendUserCoinOut(int nOutScore)
	{
		string strMethod = "userService/userCoinOut";
		object[] args = new object[1]
		{
			nOutScore
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendResultList(int nDeskId)
	{
		string strMethod = "deskResultService/resultList";
		object[] args = new object[1]
		{
			nDeskId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendQuitGame()
	{
		string strMethod = "userService/quitGame";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool sendPublicKey()
	{
		string[] array = new string[2];
		array = m_DataEncrypt.NetConnectGetRsaKey();
		string value = "userService/publicKey";
		object[] value2 = new object[2]
		{
			array[0],
			array[1]
		};
		LL_SimpleWriter lL_SimpleWriter = new LL_SimpleWriter();
		StringWriter stringWriter = new StringWriter();
		XmlTextWriter writer = new XmlTextWriter(stringWriter);
		lL_SimpleWriter.write(new Hashtable
		{
			{
				"method",
				value
			},
			{
				"version",
				"1.2.14"
			},
			{
				"args",
				value2
			}
		}, writer);
		string s = stringWriter.ToString();
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
		byte[] array2 = new byte[bytes.Length + 4];
		Array.Reverse((Array)bytes2);
		Buffer.BlockCopy(bytes2, 0, array2, 0, 4);
		Buffer.BlockCopy(bytes, 0, array2, 4, bytes.Length);
		try
		{
			int num = MySocket.Send(array2, array2.Length, SocketFlags.None);
			if (num == array2.Length)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.ToString());
			return false;
		}
	}
}
