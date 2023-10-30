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

public class STOF_Sockets : MonoBehaviour
{
	public bool isReconnect = true;

	private static STOF_Sockets _MyCreateSocket;

	private Thread m_nListenerThread;

	private bool isReconnectRestricted;

	private Coroutine coRestrictReconnect;

	private STOF_MessageControl m_MyMessageControl;

	private STOF_DataEncrypt m_DataEncrypt;

	public Socket MySocket;

	private Thread m_nReceiveThread;

	private int m_nRelineCount;

	private bool m_nSocketStartFlag;

	private bool sendCheckVersion;

	private long m_recriveHeartTime;

	private long m_sendHeartTime;

	public static STOF_Sockets GetSingleton()
	{
		return _MyCreateSocket;
	}

	private void Awake()
	{
		if (_MyCreateSocket == null)
		{
			_MyCreateSocket = this;
			isReconnect = true;
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

	public void CreateSocketGetPoint(STOF_MessageControl MyMessageControl, STOF_DataEncrypt MyDataEncrypt)
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

	public void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			SendPublicKey();
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
				SendCheckVersion(STOF_Constants.VERSION_CODE);
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
		int port = 10030;
		MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(STOF_GameInfo.getInstance().IP), port);
		UnityEngine.Debug.LogError("remoteEP: " + iPEndPoint);
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
		Console.WriteLine("jsonStr：" + @string);
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
				hashtable = STOF_ReadFileTool.JsonToClass<Hashtable>(@string);
			}
			catch
			{
				msgBytes = m_DataEncrypt.DecompressBytes(msgBytes);
				Console.WriteLine(Encoding.UTF8.GetString(msgBytes));
				hashtable = STOF_ReadFileTool.JsonToClass<Hashtable>(Encoding.UTF8.GetString(msgBytes));
				Console.WriteLine("table：" + hashtable);
			}
			empty = hashtable["method"].ToString();
			array2 = (hashtable["args"] as object[]);
		}
		Console.WriteLine("receiceMsg>>>Net:" + empty);
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

	public bool _sendMsg(string strMethod, object[] args)
	{
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
			hashtable.Add("version", STOF_Constants.Version);
			hashtable.Add("time", m_DataEncrypt.GetUnixTime());
			string text = STOF_ReadFileTool.JsonByObject(hashtable);
			bytes = Encoding.UTF8.GetBytes(text);
			if (Application.platform == RuntimePlatform.WindowsEditor && strMethod != "userService/heart")
			{
				UnityEngine.Debug.Log("发送: " + text);
			}
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

	public void _tellUINetDown()
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

	public bool SendCheckLogin(string userName, string passWord, int uiid, string versionCode = "")
	{
		string strMethod = "userService/checkLogin";
		object[] args = new object[4]
		{
			userName,
			passWord,
			uiid,
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

	public bool SendEnterRoom(int roomid)
	{
		string strMethod = "userService/enterRoom";
		object[] args = new object[1]
		{
			roomid
		};
		Console.WriteLine("userService/enterRoom");
		return _sendMsg(strMethod, args);
	}

	public bool SendEnterDesk(int deskid)
	{
		string strMethod = "userService/enterDesk";
		object[] args = new object[1]
		{
			deskid
		};
		Console.WriteLine("userService/enterDesk");
		return _sendMsg(strMethod, args);
	}

	public bool SendLeaveRoom(int roomid)
	{
		string strMethod = "userService/leaveRoom";
		object[] args = new object[1]
		{
			roomid
		};
		Console.WriteLine("userService/leaveRoom");
		return _sendMsg(strMethod, args);
	}

	public bool SendRequestSeat(int roomId, int deskid, int seatid)
	{
		SendEnterRoom(roomId);
		ZH2_GVars.isStartGame = true;
		string strMethod = "userService/requestSeat";
		object[] args = new object[2]
		{
			deskid,
			seatid
		};
		Console.WriteLine("userService/requestSeat");
		return _sendMsg(strMethod, args);
	}

	public bool SendLeaveDesk(int deskid)
	{
		return true;
	}

	public bool SendCoinIn(int coinInNumber = 0)
	{
		string strMethod = "userService/userCoinIn";
		object[] array = new object[0];
		UnityEngine.Debug.LogError("上分: " + JsonMapper.ToJson(array));
		return _sendMsg(strMethod, array);
	}

	public bool SendCoinOut(int outScore)
	{
		string strMethod = "userService/userCoinOut";
		object[] args = new object[1]
		{
			outScore
		};
		Console.WriteLine("userService/userCoinOut");
		return _sendMsg(strMethod, args);
	}

	public bool SendCoinOut()
	{
		string strMethod = "userService/userCoinOut";
		object[] args = new object[0];
		Console.WriteLine("userService/userCoinOut");
		return _sendMsg(strMethod, args);
	}

	public bool SendFired(int gunid, int seatid, float fRot, int gunValue, bool bX2Gun)
	{
		string strMethod = "fired," + gunid + "," + Math.Round(fRot, 2) + "," + gunValue + "," + bX2Gun;
		return _sendMsg(strMethod, null);
	}

	public bool SendGunHitfish(int gunid, STOF_HitFish[] hitfish1, bool bHaveKnife, STOF_HitFish[] hitfish2)
	{
		string empty = string.Empty;
		object[] array = new object[hitfish1.Length];
		string text = string.Empty;
		for (int i = 0; i < hitfish1.Length; i++)
		{
			array[i] = new object[5]
			{
				hitfish1[i].fishid,
				hitfish1[i].fishtype,
				hitfish1[i].fx,
				hitfish1[i].fy,
				hitfish1[i].bet
			};
			string str = hitfish1[i].fishid + "#" + hitfish1[i].fishtype + "#" + hitfish1[i].fx + "#" + hitfish1[i].fy + "#" + hitfish1[i].bet;
			if (i > 0)
			{
				text += "|";
			}
			text += str;
		}
		empty = "gunHitFish," + gunid + "," + text;
		if (bHaveKnife)
		{
			string text2 = string.Empty;
			if (hitfish2 != null)
			{
				object[] array2 = new object[hitfish2.Length];
				for (int j = 0; j < hitfish2.Length; j++)
				{
					array2[j] = new object[5]
					{
						hitfish2[j].fishid,
						hitfish2[j].fishtype,
						hitfish2[j].fx,
						hitfish2[j].fy,
						hitfish2[j].bet
					};
					string str2 = hitfish2[j].fishid + "#" + hitfish2[j].fishtype + "#" + hitfish2[j].fx + "#" + hitfish2[j].fy + "#" + hitfish2[j].bet;
					if (j > 0)
					{
						text2 += "|";
					}
					text2 += str2;
				}
			}
			empty = "gunHitFish," + gunid + "," + text + "," + text2;
		}
		return _sendMsg(empty, null);
	}

	public bool SendLockFish(int nFishId)
	{
		string strMethod = "userService/lockFish";
		object[] args = new object[1]
		{
			nFishId
		};
		Console.WriteLine("userService/lockFish");
		return _sendMsg(strMethod, args);
	}

	public bool SendUnLockFish()
	{
		string strMethod = "userService/unLockFish";
		object[] args = new object[0];
		Console.WriteLine("userService/unLockFish");
		return _sendMsg(strMethod, args);
	}

	public bool SendLeaveSeat(int deskid, int seatid)
	{
		string strMethod = "userService/leaveSeat";
		object[] args = new object[2]
		{
			deskid,
			seatid
		};
		Console.WriteLine("userService/leaveSeat");
		return _sendMsg(strMethod, args);
	}

	public bool SendRequestPlayerInfo(int userid)
	{
		string strMethod = "userService/playerInfo";
		object[] args = new object[1]
		{
			userid
		};
		Console.WriteLine("userService/playerInfo");
		return _sendMsg(strMethod, args);
	}

	public bool SendChat(int chatType, int receiverUserId, string chatMessage)
	{
		string strMethod = "userService/sendChat";
		object[] args = new object[3]
		{
			chatType,
			receiverUserId,
			chatMessage
		};
		Console.WriteLine("userService/sendChat");
		return _sendMsg(strMethod, args);
	}

	public bool SendGetUserAward()
	{
		string strMethod = "userService/getUserAward";
		object[] args = new object[0];
		Console.WriteLine("userService/getUserAward");
		return _sendMsg(strMethod, args);
	}

	public bool SendHeart()
	{
		string strMethod = "userService/heart";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool SendAddExpeGoldAuto()
	{
		string strMethod = "userService/addExpeGoldAuto";
		object[] args = new object[0];
		Console.WriteLine("userService/addExpeGoldAuto");
		return _sendMsg(strMethod, args);
	}

	public bool SendQuitGame()
	{
		string strMethod = "userService/quitGame";
		object[] args = new object[0];
		return _sendMsg(strMethod, args);
	}

	public bool SendMissionTakeTreasure()
	{
		string strMethod = "missionService/missionTakeTreasure";
		object[] args = new object[0];
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
		hashtable.Add("version", STOF_Constants.Version);
		hashtable.Add("args", value2);
		string s = STOF_ReadFileTool.JsonByObject(hashtable);
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
