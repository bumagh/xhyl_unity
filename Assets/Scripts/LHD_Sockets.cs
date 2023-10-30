using LHD_wox.serial;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;
using UnityEngine;

public class LHD_Sockets : MonoBehaviour
{
	public bool isReconnect = true;

	private static LHD_Sockets _MyCreateSocket;

	private LHD_MessageControl m_MyMessageControl;

	private LHD_DataEncrypt m_DataEncrypt;

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

	public static LHD_Sockets GetSingleton()
	{
		return _MyCreateSocket;
	}

	private void Awake()
	{
		if (_MyCreateSocket == null)
		{
			_MyCreateSocket = this;
		}
	}

	public void CreateSocketGetPoint(LHD_MessageControl MyMessageControl, LHD_DataEncrypt MyDataEncrypt)
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
			if (m_nListenerThread != null)
			{
				m_nListenerThread.Abort();
				m_nListenerThread = null;
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("侦听器线程异常:" + ex.Message);
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
			if (m_nReceiveThread != null)
			{
				m_nReceiveThread.Abort();
				m_nReceiveThread = null;
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("接收线程异常: " + ex.Message);
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
			UnityEngine.Debug.LogError("ReceiveThread:" + ex.Message);
		}
		try
		{
			m_nListenerThread.Abort();
			m_nListenerThread = null;
		}
		catch (Exception ex2)
		{
			UnityEngine.Debug.LogError("ListenerThread:" + ex2.Message);
		}
	}

	public void SocketConnect()
	{
		Thread.Sleep(50);
		m_nSocketStartFlag = false;
		m_nSocketStartFlag = _socketStart();
		if (m_nSocketStartFlag)
		{
			if (!sendCheckVersion)
			{
				SendCheckVersion(LHD_Constants.VERSION_CODE);
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
		IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(LHD_GameInfo.getInstance().IP), 10033);
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
			UnityEngine.Debug.LogError(ex.Message);
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
			catch (Exception)
			{
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
				MemoryStream memoryStream2 = new MemoryStream();
				memoryStream2.Position = 0L;
				memoryStream = memoryStream2;
				memoryStream.Write(buffer2, 0, num3);
				string @string = Encoding.UTF8.GetString(array2);
				_receiveNetMessage(@string);
			}
		}
		UnityEngine.Debug.Log("没有数据？？？");
		UnityEngine.Debug.LogError("======_myReceiveControl======");
		_netDownControl();
	}

	private void _receiveNetMessage(string msg)
	{
		if (m_DataEncrypt.GetKey() != "none")
		{
			msg = m_DataEncrypt.Decrypt(msg);
		}
		try
		{
			XmlReader xmlReader = XmlReader.Create(new StringReader(msg));
			xmlReader.Read();
			LHD_SimpleReader lHD_SimpleReader = new LHD_SimpleReader();
			Hashtable hashtable = lHD_SimpleReader.read(xmlReader) as Hashtable;
			string a = hashtable["method"].ToString();
			object[] array = hashtable["args"] as object[];
			if (a == "sendServerTime")
			{
				Hashtable hashtable2 = new Hashtable();
				hashtable2 = (array[0] as Hashtable);
				long serverTime = (long)hashtable2["time"];
				string str = (string)hashtable2["key"];
				m_DataEncrypt.setServerTime(serverTime);
				m_DataEncrypt.DecryptKey(str);
				goto IL_00f9;
			}
			if (!(a == "heart"))
			{
				goto IL_00f9;
			}
			m_recriveHeartTime = _getCurTime();
			goto end_IL_0028;
			IL_00f9:
			m_MyMessageControl.AddMessage(hashtable);
			end_IL_0028:;
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
			UnityEngine.Debug.LogError(msg);
		}
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
					UnityEngine.Debug.LogError("ReceiveThread:" + ex.Message);
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
		SimpleWriter simpleWriter = new SimpleWriter();
		StringWriter stringWriter = new StringWriter();
		XmlTextWriter writer = new XmlTextWriter(stringWriter);
		simpleWriter.write(new Hashtable
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
				LHD_Constants.Version
			},
			{
				"time",
				GetUnixTime()
			}
		}, writer);
		if (strMethod != "userService/heart")
		{
		}
		if (m_DataEncrypt.GetKey() != "none")
		{
			return _send(m_DataEncrypt.Encrypt(stringWriter.ToString()));
		}
		return strMethod == "userService/checkVersion" && _send(stringWriter.ToString());
	}

	private long GetUnixTime()
	{
		if (m_DataEncrypt != null)
		{
			return m_DataEncrypt.GetUnixTime();
		}
		return DateTime.Today.Ticks;
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
			UnityEngine.Debug.Log(ex.ToString());
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
		UnityEngine.Debug.Log("ID:" + strUsername + "pwd:" + strPassword);
		object[] args = new object[4]
		{
			strUsername,
			strPassword,
			nUiId,
			versionCode
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendLuDan()
	{
		string strMethod = "dragonTigerDeskResultService/resultList";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool SendTime()
	{
		string strMethod = "userService/deskStatus";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool SendHeart()
	{
		string strMethod = "userService/heart";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool SendEnterHall(int roomid)
	{
		string strMethod = "userService/selectHall";
		object[] args = new object[1]
		{
			roomid
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendEnterRoom(int roomId)
	{
		return true;
	}

	public bool SendLeaveRoom(int nRoomId)
	{
		return true;
	}

	public bool SendAutoAddExpeGold()
	{
		string strMethod = "userService/addExpeGoldAuto";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool SendDeskInfo(int roomId, int nDeskId)
	{
		return true;
	}

	public bool SendLeaveDesk(int nDeskId)
	{
		return true;
	}

	public bool SendSelectSeat(int nDeskId, int nSeatId)
	{
		string strMethod = "userService/selectSeat";
		object[] args = new object[1]
		{
			nDeskId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendLeaveSeat(int nDeskId, int nSeatId)
	{
		string strMethod = "userService/leaveSeat";
		object[] args = new object[1]
		{
			nDeskId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendUserBet(string betInfo, int nBet, int nScore, int nDeskID)
	{
		string strMethod = "userService/userBet";
		object[] args = new object[4]
		{
			betInfo,
			nBet,
			nScore,
			nDeskID
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendPlayerList(int deskId)
	{
		string strMethod = "userService/playerList";
		object[] args = new object[1]
		{
			deskId
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

	public bool SendUserCoinIn()
	{
		string strMethod = "userService/userCoinIn";
		object[] args = new object[0];
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

	public bool SendUserCoinOut()
	{
		string strMethod = "userService/userCoinOut";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool SendResultList(int nDeskId)
	{
		string strMethod = "benzSecondDeskResultService/resultList";
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

	public bool SenAutoBet(int id, int deskId)
	{
		string strMethod = "userService/autoBet";
		object[] args = new object[2]
		{
			id,
			deskId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SenContinueBet(int userId, int deskId)
	{
		string strMethod = "userService/continueBet";
		object[] args = new object[2]
		{
			userId,
			deskId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SenCancelBet(int deskId)
	{
		string strMethod = "userService/cancelBet";
		object[] args = new object[1]
		{
			deskId
		};
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
		SimpleWriter simpleWriter = new SimpleWriter();
		StringWriter stringWriter = new StringWriter();
		XmlTextWriter writer = new XmlTextWriter(stringWriter);
		simpleWriter.write(new Hashtable
		{
			{
				"method",
				value
			},
			{
				"version",
				LHD_Constants.Version
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
			UnityEngine.Debug.LogError(ex.ToString());
			return false;
		}
	}

	private void Start()
	{
		ZH2_GVars.sendCheckSafeBoxPwd = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendCheckSafeBoxPwd, new Action<object[]>(SendCheckSafeBoxPwd));
		ZH2_GVars.sendChangeSafeBoxPwd = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendChangeSafeBoxPwd, new Action<object[]>(SendChangeSafeBoxPwd));
		ZH2_GVars.sendDeposit = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendDeposit, new Action<object[]>(SendDeposit));
		ZH2_GVars.sendExtract = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendExtract, new Action<object[]>(SendExtract));
		ZH2_GVars.sendTransactionRecord = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendTransactionRecord, new Action<object[]>(SendGetTransactionRecord));
	}

	private void SendCheckSafeBoxPwd(object[] msgs)
	{
		string strMethod = "userService/checkSafeBoxPwd";
		_sendMsg(strMethod, msgs);
	}

	private void SendChangeSafeBoxPwd(object[] msgs)
	{
		string strMethod = "userService/changeSafeBoxPwd";
		_sendMsg(strMethod, msgs);
	}

	private void SendDeposit(object[] msgs)
	{
		string strMethod = "userService/deposit";
		_sendMsg(strMethod, msgs);
	}

	private void SendExtract(object[] msgs)
	{
		string strMethod = "userService/extract";
		_sendMsg(strMethod, msgs);
	}

	private void SendGetTransactionRecord(object[] msgs)
	{
		string strMethod = "userService/getTransactionRecord";
		_sendMsg(strMethod, msgs);
	}

	private void OnDisable()
	{
		ZH2_GVars.sendCheckSafeBoxPwd = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendCheckSafeBoxPwd, new Action<object[]>(SendCheckSafeBoxPwd));
		ZH2_GVars.sendChangeSafeBoxPwd = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendChangeSafeBoxPwd, new Action<object[]>(SendChangeSafeBoxPwd));
		ZH2_GVars.sendDeposit = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendDeposit, new Action<object[]>(SendDeposit));
		ZH2_GVars.sendExtract = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendExtract, new Action<object[]>(SendExtract));
		ZH2_GVars.sendTransactionRecord = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendTransactionRecord, new Action<object[]>(SendGetTransactionRecord));
	}
}
