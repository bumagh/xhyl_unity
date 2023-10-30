using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class XLDT_Sockets : MonoBehaviour
{
	public bool isReconnect = true;

	private static XLDT_Sockets _MyCreateSocket;

	private Thread m_nListenerThread;

	private bool isReconnectRestricted;

	private Coroutine coRestrictReconnect;

	private XLDT_MessageControl m_MyMessageControl;

	private XLDT_DataEncrypt m_DataEncrypt;

	public Socket MySocket;

	private Thread m_nReceiveThread;

	private int m_nRelineCount;

	private bool m_nSocketStartFlag;

	private bool sendCheckVersion;

	private long m_recriveHeartTime;

	private long m_sendHeartTime;

	public static XLDT_Sockets GetSingleton()
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

	private void Start()
	{
		ZH2_GVars.sendCheckSafeBoxPwd = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendCheckSafeBoxPwd, new Action<object[]>(SendCheckSafeBoxPwd));
		ZH2_GVars.sendChangeSafeBoxPwd = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendChangeSafeBoxPwd, new Action<object[]>(SendChangeSafeBoxPwd));
		ZH2_GVars.sendDeposit = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendDeposit, new Action<object[]>(SendDeposit));
		ZH2_GVars.sendExtract = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendExtract, new Action<object[]>(SendExtract));
		ZH2_GVars.sendTransactionRecord = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendTransactionRecord, new Action<object[]>(SendGetTransactionRecord));
		ZH2_GVars.sendGamePay = (Action<object[]>)Delegate.Combine(ZH2_GVars.sendGamePay, new Action<object[]>(SendGamePay));
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
			Console.WriteLine("ListenerThread:" + ex.Message);
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

	public void CreateSocketGetPoint(XLDT_MessageControl MyMessageControl, XLDT_DataEncrypt MyDataEncrypt)
	{
		CreateReceiveThread();
		CreateListenerThread();
		m_MyMessageControl = MyMessageControl;
		m_DataEncrypt = MyDataEncrypt;
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
			Console.WriteLine("ReceiveThread" + ex.Message);
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
			Console.WriteLine("ReceiveThread:" + ex.Message);
		}
		try
		{
			m_nListenerThread.Abort();
			m_nListenerThread = null;
		}
		catch (Exception ex2)
		{
			Console.WriteLine("ListenerThread:" + ex2.Message);
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
				SendCheckVersion(XLDT_Constants.VERSION_CODE);
				sendCheckVersion = true;
			}
			else
			{
				SendPublicKey();
			}
			_myReceiveControl();
		}
	}

	private bool _socketStart()
	{
		MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(XLDT_GameInfo.getInstance().IP), 10020);
		UnityEngine.Debug.LogError(iPEndPoint);
		try
		{
			MySocket.Connect(iPEndPoint);
			m_nRelineCount = 0;
			m_recriveHeartTime = _getCurTime();
			m_sendHeartTime = _getCurTime();
			return true;
		}
		catch (SocketException ex)
		{
			Console.WriteLine("connect:" + ex.Message);
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
				Console.WriteLine("Receive:" + ex.Message);
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
				_receiveNetMessage(array2);
			}
		}
		Console.WriteLine("nRecLenth <= 0  == " + num);
		_netDownControl();
	}

	private void _receiveNetMessage(byte[] msgBytes)
	{
		if (m_DataEncrypt.GetKey() != "none")
		{
			msgBytes = m_DataEncrypt.Decrypt(msgBytes);
		}
		Hashtable hashtable = new Hashtable();
		string @string = Encoding.UTF8.GetString(msgBytes);
		hashtable = XLDT_ReadFileTool.JsonToClass<Hashtable>(@string);
		string a = hashtable["method"].ToString();
		object[] array = hashtable["args"] as object[];
		if (a != "heart")
		{
			UnityEngine.Debug.Log("接收: " + @string);
		}
		if (a == "sendServerTime")
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary = (array[0] as Dictionary<string, object>);
			long serverTime = Convert.ToInt64(dictionary["time"]);
			string str = (string)dictionary["key"];
			m_DataEncrypt.setServerTime(serverTime);
			m_DataEncrypt.DecryptKey(str);
		}
		else if (a == "heart")
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
					Console.WriteLine("ReceiveThread:" + ex.Message);
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

	public bool SendCheckVersion(string versionCode = "")
	{
		string strMethod = "userService/checkVersion";
		object[] args = new object[1]
		{
			versionCode
		};
		return _sendMsg(strMethod, args);
	}

	private bool _sendMsg(string strMethod, object[] args)
	{
		if (strMethod != "userService/heart")
		{
			UnityEngine.Debug.Log("发送 " + strMethod + "  " + JsonMapper.ToJson(args));
		}
		Hashtable hashtable = new Hashtable();
		hashtable.Add("method", strMethod);
		hashtable.Add("args", args);
		hashtable.Add("version", XLDT_Constants.Version);
		hashtable.Add("time", m_DataEncrypt.GetUnixTime());
		string s = XLDT_ReadFileTool.JsonByObject(hashtable);
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		if (m_DataEncrypt.GetKey() != "none")
		{
			return _send(m_DataEncrypt.Encrypt(bytes));
		}
		return strMethod == "userService/checkVersion" && _send(bytes);
	}

	private bool _send(byte[] msg)
	{
		byte[] bytes = BitConverter.GetBytes(msg.Length);
		byte[] array = new byte[msg.Length + 4];
		Array.Reverse((Array)bytes);
		Buffer.BlockCopy(bytes, 0, array, 0, 4);
		Buffer.BlockCopy(msg, 0, array, 4, msg.Length);
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
			MonoBehaviour.print(ex.ToString());
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

	public bool SendUserLogin(string userName, string passWord, int uiId, string versionCode = "")
	{
		string strMethod = "userService/userLogin";
		object[] args = new object[4]
		{
			userName,
			passWord,
			uiId,
			versionCode
		};
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

	public bool SendEnterRoom(int roomType)
	{
		string strMethod = "userService/enterRoom";
		object[] args = new object[1]
		{
			roomType
		};
		Console.WriteLine("userService/enterRoom");
		return _sendMsg(strMethod, args);
	}

	public bool SendLeaveRoom(int roomId)
	{
		string strMethod = "userService/leaveRoom";
		object[] args = new object[1]
		{
			roomId
		};
		Console.WriteLine("userService/leaveRoom");
		return _sendMsg(strMethod, args);
	}

	public bool SendAddExpeGoldAuto()
	{
		string strMethod = "userService/addExpeGoldAuto";
		object[] args = new object[0];
		Console.WriteLine("userService/addExpeGoldAuto");
		return _sendMsg(strMethod, args);
	}

	public bool SendSelectSeat(int deskId, int seatId)
	{
		SendEnterRoom(XLDT_GameManager.getInstance().mRoomId);
		string strMethod = "userService/selectSeat";
		object[] args = new object[2]
		{
			deskId,
			seatId
		};
		Console.WriteLine("userService/selectSeat");
		return _sendMsg(strMethod, args);
	}

	public bool SendLeaveSeat(int deskId, int seatId)
	{
		string strMethod = "userService/leaveSeat";
		object[] args = new object[2]
		{
			deskId,
			seatId
		};
		Console.WriteLine("userService/leaveSeat");
		return _sendMsg(strMethod, args);
	}

	public bool SendUserBet(int nBet, int nScore, int nDeskId)
	{
		string strMethod = "userService/userBet";
		object[] args = new object[3]
		{
			nBet,
			nScore,
			nDeskId
		};
		Console.WriteLine("userService/userBet");
		return _sendMsg(strMethod, args);
	}

	public bool SendCancelBet(int nDeskId)
	{
		string strMethod = "userService/cancelBet";
		object[] args = new object[1]
		{
			nDeskId
		};
		Console.WriteLine("userService/cancelBet");
		return _sendMsg(strMethod, args);
	}

	public bool SendPlayerInfo(int nUserId)
	{
		string strMethod = "userService/playerInfo";
		object[] args = new object[1]
		{
			nUserId
		};
		Console.WriteLine("userService/playerInfo");
		return _sendMsg(strMethod, args);
	}

	public bool SendSendChat(int nChatType, int nReceiverUserId, string strChatMessage)
	{
		string strMethod = "userService/sendChat";
		object[] args = new object[3]
		{
			nChatType,
			nReceiverUserId,
			strChatMessage
		};
		Console.WriteLine("userService/sendChat");
		return _sendMsg(strMethod, args);
	}

	public bool SendUserCoinIn(int nCoinInNumber)
	{
		string strMethod = "userService/userCoinIn";
		object[] args = new object[1]
		{
			nCoinInNumber
		};
		Console.WriteLine("userService/userCoinIn");
		return _sendMsg(strMethod, args);
	}

	public bool SendUserCoinIn()
	{
		string strMethod = "userService/userCoinIn";
		object[] args = new object[0];
		Console.WriteLine("userService/userCoinIn");
		return _sendMsg(strMethod, args);
	}

	public bool SendUserCoinOut(int nOutScore)
	{
		string strMethod = "userService/userCoinOut";
		object[] args = new object[1]
		{
			nOutScore
		};
		Console.WriteLine("userService/userCoinOut");
		return _sendMsg(strMethod, args);
	}

	public bool SendUserCoinOut()
	{
		string strMethod = "userService/userCoinOut";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool SendResultList()
	{
		string strMethod = "userService/resultList";
		object[] args = new object[0];
		Console.WriteLine("userService/resultList");
		return _sendMsg(strMethod, args);
	}

	public bool SendContinueBet(int nDeskId, XLDT_UserBets userBets, int type)
	{
		string strMethod = "userService/continueBet";
		object[] args = new object[3]
		{
			nDeskId,
			userBets,
			type
		};
		Console.WriteLine("userService/continueBet");
		return _sendMsg(strMethod, args);
	}

	public bool SendHeart()
	{
		string strMethod = "userService/heart";
		object[] args = new object[0];
		Console.WriteLine("userService/heart");
		return _sendMsg(strMethod, args);
	}

	public bool SendPublicKey()
	{
		string[] array = new string[2];
		array = m_DataEncrypt.NetConnectGetRsaKey();
		string value = "userService/publicKey";
		object[] value2 = new object[2]
		{
			array[0],
			array[1]
		};
		Hashtable hashtable = new Hashtable();
		hashtable.Add("method", value);
		hashtable.Add("version", XLDT_Constants.Version);
		hashtable.Add("args", value2);
		string s = XLDT_ReadFileTool.JsonByObject(hashtable);
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
			Console.WriteLine(ex.ToString());
			return false;
		}
	}

	public bool SendQuitGame()
	{
		return true;
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

	private void SendGamePay(object[] msgs)
	{
		string strMethod = "userService/pay";
		_sendMsg(strMethod, msgs);
	}

	private void OnDisable()
	{
		ZH2_GVars.sendCheckSafeBoxPwd = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendCheckSafeBoxPwd, new Action<object[]>(SendCheckSafeBoxPwd));
		ZH2_GVars.sendChangeSafeBoxPwd = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendChangeSafeBoxPwd, new Action<object[]>(SendChangeSafeBoxPwd));
		ZH2_GVars.sendDeposit = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendDeposit, new Action<object[]>(SendDeposit));
		ZH2_GVars.sendExtract = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendExtract, new Action<object[]>(SendExtract));
		ZH2_GVars.sendTransactionRecord = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendTransactionRecord, new Action<object[]>(SendGetTransactionRecord));
		ZH2_GVars.sendGamePay = (Action<object[]>)Delegate.Remove(ZH2_GVars.sendGamePay, new Action<object[]>(SendGamePay));
	}
}
