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

public class BaiJiaLe_Sockets : MonoBehaviour
{
	public bool isReconnect = true;

	private BaiJiaLe_MessageControl m_MyMessageControl;

	private BaiJiaLe_DataEncrypt m_DataEncrypt;

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

	private static BaiJiaLe_Sockets _MyCreateSocket;

	public static BaiJiaLe_Sockets GetSingleton()
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

	public void CreateSocketGetPoint(BaiJiaLe_MessageControl MyMessageControl, BaiJiaLe_DataEncrypt MyDataEncrypt)
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
				SendCheckVersion(BaiJiaLe_Constants.VERSION_CODE);
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
		UnityEngine.Debug.Log(BaiJiaLe_GameInfo.getInstance().IP + "||" + BaiJiaLe_GameInfo.getInstance().Port);
		IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(BaiJiaLe_GameInfo.getInstance().IP), BaiJiaLe_GameInfo.getInstance().Port);
		try
		{
			MySocket.Connect(remoteEP);
			m_nRelineCount = 0;
			m_recriveHeartTime = _getCurTime();
			m_sendHeartTime = _getCurTime();
			return true;
		}
		catch (SocketException)
		{
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
				_receiveNetMessage(array2);
			}
		}
		UnityEngine.Debug.Log("nRecLenth <= 0  == " + num);
		_netDownControl();
	}

	private void _receiveNetMessage(byte[] msgBytes)
	{
		if (m_DataEncrypt.GetKey() != "none")
		{
			msgBytes = m_DataEncrypt.Decrypt(msgBytes);
		}
		string empty = string.Empty;
		Hashtable hashtable = new Hashtable();
		string @string = Encoding.UTF8.GetString(msgBytes);
		string text = string.Empty;
		if (@string.IndexOf(",") > 0)
		{
			text = @string.Substring(0, @string.IndexOf(","));
		}
		object[] array2;
		if (text == "newFish" || text == "fired" || text == "gunHitFish")
		{
			string text2 = @string.Remove(0, text.Length + 1);
			empty = text;
			string[] array = text2.Split(',');
			array2 = array;
			hashtable.Add("method", empty);
			hashtable.Add("args", array2);
		}
		else
		{
			try
			{
				hashtable = BaiJiaLe_ReadFileTool.JsonToClass<Hashtable>(@string);
			}
			catch
			{
				msgBytes = m_DataEncrypt.DecompressBytes(msgBytes);
				UnityEngine.Debug.Log(Encoding.UTF8.GetString(msgBytes));
				hashtable = BaiJiaLe_ReadFileTool.JsonToClass<Hashtable>(Encoding.UTF8.GetString(msgBytes));
				UnityEngine.Debug.Log("table：" + hashtable);
			}
			empty = hashtable["method"].ToString();
			array2 = (hashtable["args"] as object[]);
		}
		if (empty == "sendServerTime")
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary = (array2[0] as Dictionary<string, object>);
			long serverTime = Convert.ToInt64(dictionary["time"]);
			string str = (string)dictionary["key"];
			m_DataEncrypt.setServerTime(serverTime);
			m_DataEncrypt.DecryptKey(str);
		}
		else if (empty == "heart")
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
					UnityEngine.Debug.Log("ReceiveThread" + ex.Message);
				}
				_netDownControl();
				m_recriveHeartTime = num;
				continue;
			}
			if (num - m_sendHeartTime >= 2500 && MySocket != null)
			{
				SendHeart();
				m_sendHeartTime = num;
			}
			Thread.Sleep(500);
		}
	}

	private bool _sendMsg(string strMethod, object[] args)
	{
		UnityEngine.Debug.Log("发送" + strMethod + ":" + JsonMapper.ToJson(args).ToString());
		byte[] bytes;
		if (args == null)
		{
			bytes = Encoding.UTF8.GetBytes(strMethod);
		}
		else
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("method", strMethod);
			hashtable.Add("args", args);
			hashtable.Add("version", BaiJiaLe_Constants.Version);
			hashtable.Add("time", m_DataEncrypt.GetUnixTime());
			string s = BaiJiaLe_ReadFileTool.JsonByObject(hashtable);
			bytes = Encoding.UTF8.GetBytes(s);
		}
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
		catch (Exception)
		{
			return false;
		}
	}

	public void _netClose()
	{
		m_nReceiveThread.Abort();
		m_nReceiveThread = null;
		_netDownControl();
	}

	private void _netDownControl()
	{
		BaiJiaLe_GameInfo.IsBreak = true;
		if (MySocket != null)
		{
			UnityEngine.Debug.Log("断开连接");
			MySocket.Close();
			MySocket = null;
		}
		m_DataEncrypt.KeyReset();
		m_nRelineCount++;
		_tellUINetDown();
	}

	private void _tellUINetDown()
	{
		UnityEngine.Debug.Log("_tellUINetDown");
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
		object[] args = new object[4]
		{
			strUsername,
			strPassword,
			nUiId,
			versionCode
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendHeart()
	{
		string strMethod = "userService/heart";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool SendEnterRoom()
	{
		string strMethod = "userService/enterRoom";
		object[] args = new object[1]
		{
			0
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendUpdateRoom()
	{
		string strMethod = "userService/DeskInfo";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool SendLeaveRoom()
	{
		string strMethod = "userService/leaveRoom";
		object[] args = new object[1]
		{
			0
		};
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

	public bool SenddeskInfo1(int nDeskId)
	{
		string strMethod = "userService/deskInfo1";
		object[] args = new object[1]
		{
			nDeskId
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendCheckBindname(int nDeskId, string bindingName)
	{
		string strMethod = "userService/CheckBindname";
		object[] args = new object[2]
		{
			nDeskId,
			bindingName
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
		UnityEngine.Debug.Log("发送下注：" + nBet + "值" + nScore);
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
		UnityEngine.Debug.Log("取消");
		string strMethod = "userService/cancelBet";
		object[] args = new object[1]
		{
			nDeskID
		};
		return _sendMsg(strMethod, args);
	}

	public bool SendContinueBet(int nDeskID)
	{
		string strMethod = "userService/userContinueBet";
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

	public bool SendPublicKey()
	{
		UnityEngine.Debug.Log("SendPublicKey");
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
		hashtable.Add("version", BaiJiaLe_Constants.Version);
		hashtable.Add("args", value2);
		string s = BaiJiaLe_ReadFileTool.JsonByObject(hashtable);
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
}
