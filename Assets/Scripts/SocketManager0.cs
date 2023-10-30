using ProtoBuf;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class SocketManager0
{
	private static SocketManager0 _instance;

	private string _currIP;

	private int _currPort;

	private bool _isConnected;

	private Socket clientSocket;

	private Thread receiveThread;

	private DataBuffer _databuffer = new DataBuffer();

	private byte[] _tmpReceiveBuff = new byte[4096];

	private sSocketData _socketData = default(sSocketData);

	public static SocketManager0 Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SocketManager0();
			}
			return _instance;
		}
	}

	public bool IsConnceted => _isConnected;

	private void _close()
	{
		if (_isConnected)
		{
			_isConnected = false;
			if (receiveThread != null)
			{
				receiveThread.Abort();
				receiveThread = null;
			}
			if (clientSocket != null && clientSocket.Connected)
			{
				clientSocket.Close();
				clientSocket = null;
			}
		}
	}

	private void _ReConnect()
	{
	}

	private void _onConnet()
	{
		try
		{
			UnityEngine.Debug.LogError("开始连接");
			clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPAddress address = IPAddress.Parse(_currIP);
			IPEndPoint remoteEP = new IPEndPoint(address, _currPort);
			IAsyncResult asyncResult = clientSocket.BeginConnect(remoteEP, _onConnect_Sucess, clientSocket);
			if (!asyncResult.AsyncWaitHandle.WaitOne(5000, exitContext: true))
			{
				_onConnect_Outtime();
			}
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
			_onConnect_Fail();
		}
	}

	private void _onConnect_Sucess(IAsyncResult iar)
	{
		try
		{
			Socket socket = (Socket)iar.AsyncState;
			UnityEngine.Debug.LogError("0 " + socket);
			socket.EndConnect(iar);
			UnityEngine.Debug.LogError("1");
			receiveThread = new Thread(_onReceiveSocket);
			UnityEngine.Debug.LogError("2");
			receiveThread.IsBackground = true;
			UnityEngine.Debug.LogError("3");
			receiveThread.Start();
			UnityEngine.Debug.LogError("4");
			_isConnected = true;
			UnityEngine.Debug.Log("连接成功");
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("错误: " + arg);
			Close();
		}
	}

	private void _onConnect_Outtime()
	{
		UnityEngine.Debug.Log("连接超时");
		_close();
	}

	private void _onConnect_Fail()
	{
		UnityEngine.Debug.Log("连接失败");
		_close();
	}

	private void _onSendMsg(IAsyncResult asyncSend)
	{
		try
		{
			Socket socket = (Socket)asyncSend.AsyncState;
			socket.EndSend(asyncSend);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("send msg exception:" + ex.StackTrace);
		}
	}

	private void _onReceiveSocket()
	{
		while (clientSocket.Connected)
		{
			try
			{
				int num = clientSocket.Receive(_tmpReceiveBuff);
				if (num > 0)
				{
					_databuffer.AddBuffer(_tmpReceiveBuff, num);
					while (_databuffer.GetData(out _socketData))
					{
						sEvent_NetMessageData item = default(sEvent_NetMessageData);
						item._eventType = _socketData._protocallType;
						item._eventData = _socketData._data;
						lock (SingletonMonoBehaviour<MessageCenter>.Instance._netMessageDataQueue)
						{
							UnityEngine.Debug.Log(item._eventType);
							SingletonMonoBehaviour<MessageCenter>.Instance._netMessageDataQueue.Enqueue(item);
						}
					}
				}
			}
			catch (Exception)
			{
				clientSocket.Disconnect(reuseSocket: true);
				clientSocket.Shutdown(SocketShutdown.Both);
				clientSocket.Close();
				return;
			}
		}
		_isConnected = false;
		_ReConnect();
	}

	private sSocketData BytesToSocketData(eProtocalCommand _protocalType, byte[] _data)
	{
		sSocketData result = default(sSocketData);
		result._buffLength = Constants.HEAD_LEN + _data.Length;
		result._dataLength = _data.Length;
		result._protocallType = _protocalType;
		result._data = _data;
		return result;
	}

	private byte[] SocketDataToBytes(sSocketData tmpSocketData)
	{
		byte[] array = new byte[tmpSocketData._buffLength];
		byte[] bytes = BitConverter.GetBytes(tmpSocketData._buffLength);
		byte[] bytes2 = BitConverter.GetBytes((ushort)tmpSocketData._protocallType);
		Array.Copy(bytes, 0, array, 0, Constants.HEAD_DATA_LEN);
		Array.Copy(bytes2, 0, array, Constants.HEAD_DATA_LEN, Constants.HEAD_TYPE_LEN);
		Array.Copy(tmpSocketData._data, 0, array, Constants.HEAD_LEN, tmpSocketData._dataLength);
		return array;
	}

	private byte[] DataToBytes(eProtocalCommand _protocalType, byte[] _data)
	{
		return SocketDataToBytes(BytesToSocketData(_protocalType, _data));
	}

	public static byte[] ProtoBuf_Serializer(IExtensible data)
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			byte[] array = null;
			Serializer.Serialize(memoryStream, data);
			memoryStream.Position = 0L;
			int num = (int)memoryStream.Length;
			array = new byte[num];
			memoryStream.Read(array, 0, num);
			return array;
		}
	}

	public static T ProtoBuf_Deserialize<T>(byte[] _data)
	{
		using (MemoryStream source = new MemoryStream(_data))
		{
			return Serializer.Deserialize<T>(source);
		}
	}

	public void Connect(string _currIP, int _currPort)
	{
		if (!IsConnceted)
		{
			this._currIP = _currIP;
			this._currPort = _currPort;
			_onConnet();
		}
	}

	private void SendMsgBase(eProtocalCommand _protocalType, byte[] _data)
	{
		if (clientSocket == null || !clientSocket.Connected)
		{
			_ReConnect();
			return;
		}
		byte[] array = DataToBytes(_protocalType, _data);
		clientSocket.BeginSend(array, 0, array.Length, SocketFlags.None, _onSendMsg, clientSocket);
	}

	public void SendMsg(eProtocalCommand _protocalType, ByteStreamBuff _byteStreamBuff)
	{
		SendMsgBase(_protocalType, _byteStreamBuff.ToArray());
	}

	public void SendMsg(eProtocalCommand _protocalType, IExtensible data)
	{
		SendMsgBase(_protocalType, ProtoBuf_Serializer(data));
	}

	public void Close()
	{
		_close();
	}
}
