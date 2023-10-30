using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class LKB_ClientSocket
{
	public delegate void LogDelegate(string msg);

	private const int HEAD_SIZE = 4;

	private const int MAX_MESSAGE_SIZE = 16384;

	public Action connectHandler;

	public Action<Exception> exceptionHandler;

	public Action<byte[], int, int> dataHandler;

	public Action<int, int, LKB_ClientSocket> checkTimeoutHandler;

	public List<bool> doneList;

	private TcpClient _pTcpClient;

	private object __lock;

	private NetworkStream _pNetwrokStream;

	private byte[] _receiveBuffer;

	private int _nLastMsgBodySize;

	private MemoryStream _pLastMsgBodyStream;

	private int _packetId;

	private LogDelegate _logInfoFunc;

	private LogDelegate _logErrorFunc;

	public bool Connected => _pTcpClient.Connected;

	public LKB_ClientSocket(bool useTcpClient)
	{
		_pTcpClient = new TcpClient();
		_receiveBuffer = new byte[65536];
		_pTcpClient.NoDelay = true;
		_pTcpClient.Client.NoDelay = true;
		doneList = new List<bool>();
		_nLastMsgBodySize = 0;
		_pLastMsgBodyStream = new MemoryStream();
		dataHandler = _defaultDataHandler;
		exceptionHandler = _defaultExceptionHandler;
		__lock = new object();
	}

	public TcpClient GetTcpClient()
	{
		return _pTcpClient;
	}

	public void Close()
	{
		if (_pTcpClient != null)
		{
			object _lock = __lock;
			lock (_lock)
			{
				_pTcpClient.Close();
				_pLastMsgBodyStream.Close();
				if (_pNetwrokStream != null)
				{
					_pNetwrokStream.Close();
				}
			}
		}
		UnityEngine.Debug.Log("socket closed");
	}

	public void Connect(string host, int port)
	{
		UnityEngine.Debug.Log("Connect");
		try
		{
			_pTcpClient.BeginConnect(host, port, _onConnected, this);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(ex);
			exceptionHandler(ex);
		}
	}

	private AsyncCallback AsyncTimeoutWrapper(Action<IAsyncResult, int> func, int timeout)
	{
		doneList.Add(item: false);
		int checkId = doneList.Count - 1;
		checkTimeoutHandler(checkId, timeout, this);
		return delegate(IAsyncResult ar)
		{
			func(ar, checkId);
		};
	}

	private void _onConnected(IAsyncResult ar)
	{
		UnityEngine.Debug.Log("_onConnected");
		object _lock = __lock;
		lock (_lock)
		{
			try
			{
				if (!ar.IsCompleted)
				{
					throw new Exception("_onConnected is not completed");
				}
				_pTcpClient.EndConnect(ar);
				_pTcpClient.NoDelay = true;
				_pNetwrokStream = _pTcpClient.GetStream();
				_beginReceive();
				if (connectHandler != null)
				{
					connectHandler();
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("Exception happened in _onConnected " + ex.Message);
				exceptionHandler(ex);
			}
		}
	}

	private void _onConnected(IAsyncResult ar, int checkId)
	{
		object _lock = __lock;
		lock (_lock)
		{
			try
			{
				if (!ar.IsCompleted)
				{
					throw new Exception("_onConnected is not completed");
				}
				_pTcpClient.EndConnect(ar);
				_pTcpClient.NoDelay = true;
				_pNetwrokStream = _pTcpClient.GetStream();
				UnityEngine.Debug.Log($"ReadTimeout: {_pNetwrokStream.ReadTimeout}, WriteTimeout: {_pNetwrokStream.WriteTimeout}");
				_pNetwrokStream.ReadTimeout = 2000;
				_pNetwrokStream.WriteTimeout = 2000;
				UnityEngine.Debug.Log($"ReadTimeout: {_pNetwrokStream.ReadTimeout}, WriteTimeout: {_pNetwrokStream.WriteTimeout}, CanTimeout: {_pNetwrokStream.CanTimeout}");
				_beginReceive();
				if (connectHandler != null)
				{
					connectHandler();
				}
				doneList[checkId] = true;
			}
			catch (Exception obj)
			{
				exceptionHandler(obj);
			}
		}
	}

	protected void _beginReceive()
	{
		object _lock = __lock;
		lock (_lock)
		{
			try
			{
				if (_pNetwrokStream == null)
				{
					_pNetwrokStream = _pTcpClient.GetStream();
				}
				_pNetwrokStream.BeginRead(_receiveBuffer, 0, 16384, EndReceiveHandle, null);
			}
			catch (Exception obj)
			{
				exceptionHandler(obj);
			}
		}
	}

	public void EndReceiveHandle(IAsyncResult ar)
	{
		object _lock = __lock;
		lock (_lock)
		{
			try
			{
				if (!ar.IsCompleted)
				{
					throw new Exception("EndReceiveHandle is not completed");
				}
				if (!_pTcpClient.Connected)
				{
					exceptionHandler(new Exception("EndReceiveHandle> Connected is false"));
					return;
				}
				int num = _pNetwrokStream.EndRead(ar);
				if (num == 0)
				{
					exceptionHandler(new Exception("EndReceiveHandle> bytesRead == 0"));
					return;
				}
				int num2 = 0;
				MemoryStream memoryStream = new MemoryStream(_receiveBuffer, 0, num);
				BinaryReader binaryReader = new BinaryReader(memoryStream);
				while (num2 < num)
				{
					if (_nLastMsgBodySize != 0 && _nLastMsgBodySize > _pLastMsgBodyStream.Length)
					{
						int num3 = (int)(_nLastMsgBodySize - _pLastMsgBodyStream.Length);
						int num4 = (int)(memoryStream.Length - memoryStream.Position);
						int num5 = (num3 > num4) ? num4 : num3;
						_pLastMsgBodyStream.Write(binaryReader.ReadBytes(num5), 0, num5);
						num2 += num5;
						if (_pLastMsgBodyStream.Length != _nLastMsgBodySize)
						{
							continue;
						}
						byte[] arg = _pLastMsgBodyStream.ToArray();
						if (dataHandler != null)
						{
							dataHandler(arg, _packetId, num2);
						}
						_nLastMsgBodySize = 0;
						_pLastMsgBodyStream.Close();
						_pLastMsgBodyStream = new MemoryStream();
					}
					if (num2 >= num)
					{
						break;
					}
					byte[] array = binaryReader.ReadBytes(4);
					Array.Reverse((Array)array);
					int num6 = BitConverter.ToInt32(array, 0);
					num2 += 4;
					if (num6 <= 0)
					{
						exceptionHandler(new Exception("wrong bodysize: " + num6));
						UnityEngine.Debug.LogError("wrong bodysize: " + num6);
						break;
					}
					if (num6 + num2 > num)
					{
						_pLastMsgBodyStream = new MemoryStream();
						_nLastMsgBodySize = num6;
					}
					else
					{
						byte[] arg2 = binaryReader.ReadBytes(num6);
						num2 += num6;
						if (dataHandler != null)
						{
							dataHandler(arg2, _packetId, num2);
						}
					}
				}
				binaryReader.Close();
				memoryStream.Close();
				_beginReceive();
			}
			catch (ObjectDisposedException obj)
			{
				exceptionHandler(obj);
				LogInfo("Receive Closed");
			}
			catch (Exception ex)
			{
				exceptionHandler(ex);
				LogError(ex.Message + "\n " + ex.StackTrace + "\n" + ex.Source);
			}
			_packetId++;
		}
	}

	protected void _defaultDataHandler(byte[] msgBuffer, int packetId, int endIndex)
	{
		if (msgBuffer == null || msgBuffer.Length == 0)
		{
			UnityEngine.Debug.LogError("HandleRecvMsgAsString>msgBuffer is empty");
			return;
		}
		string @string = Encoding.UTF8.GetString(msgBuffer);
		UnityEngine.Debug.Log($"recevie>size: {msgBuffer.Length}, message: {@string}");
	}

	public void Send(byte[] msgBytes, bool useHead = true)
	{
		object _lock = __lock;
		lock (_lock)
		{
			try
			{
				int value = msgBytes.Length;
				MemoryStream memoryStream = new MemoryStream();
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				if (useHead)
				{
					byte[] bytes = BitConverter.GetBytes(value);
					Array.Reverse((Array)bytes);
					binaryWriter.Write(bytes);
				}
				binaryWriter.Write(msgBytes);
				byte[] array = memoryStream.ToArray();
				PrintBin(array, "send");
				_pNetwrokStream.BeginWrite(array, 0, array.Length, _onEndWrite, _pNetwrokStream);
				memoryStream.Close();
			}
			catch (ObjectDisposedException obj)
			{
				UnityEngine.Debug.Log("Send Closed");
				exceptionHandler(obj);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("BeginSend: " + ex);
				exceptionHandler(ex);
			}
		}
	}

	protected void _onEndWrite(IAsyncResult ar)
	{
		object _lock = __lock;
		lock (_lock)
		{
			try
			{
				if (!ar.IsCompleted)
				{
					throw new Exception("_onEndWrite is not completed");
				}
				NetworkStream networkStream = (NetworkStream)ar.AsyncState;
				networkStream.EndWrite(ar);
			}
			catch (Exception ex)
			{
				LogError(ex.Message);
				exceptionHandler(ex);
			}
		}
	}

	private void _defaultExceptionHandler(Exception ex)
	{
	}

	public void RegisterLog(LogDelegate logInfoFunc, LogDelegate logErrorFunc)
	{
		_logInfoFunc = logInfoFunc;
		_logErrorFunc = logErrorFunc;
	}

	private void LogInfo(string msg)
	{
		if (_logInfoFunc != null)
		{
			_logInfoFunc(msg);
		}
	}

	private void LogError(string msg)
	{
		if (_logErrorFunc != null)
		{
			_logErrorFunc(msg);
		}
	}

	private void PrintBin(byte[] bytes, string head = "")
	{
	}
}
