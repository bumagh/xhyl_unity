using BestHTTP.WebSocket;
using BestHTTP.WebSocket.Frames;
using LitJson;
using System;
using System.Collections;
using UnityEngine;

public class NewWebSocket : MonoBehaviour
{
	public class ReqData
	{
		public string from_id;

		public string to_id;

		public string type;
	}

	public class HeartBeatData
	{
		public string type;
	}

	public class ServiceMsgData
	{
		public string from_id;

		public string to_id;

		public string type;

		public string content;

		public string send_time;
	}

	private static NewWebSocket instance;

	public static WebSocket webSocket;

	public static string tempMsg = string.Empty;

	private Action<TalkMsg> talkFinishEvent;

	private bool IsInit;

	private bool isStop;

	private Coroutine coroutine;

	private int count;

	public static bool IsExist => instance != null;

	public static NewWebSocket GetInstance()
	{
		if (instance == null)
		{
			UnityEngine.Debug.LogError("instance为空");
			instance = new GameObject("NewWebSocket").AddComponent<NewWebSocket>();
			instance.Init();
			UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);
		}
		return instance;
	}

	private void Awake()
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);
	}

	private void OnEnable()
	{
		isStop = false;
		IsInit = false;
		Init();
		instance = this;
	}

	private void Init()
	{
		if (!IsInit)
		{
			IsInit = true;
			UnityEngine.Debug.Log(" ssssss   " + ZH2_GVars.IPAddress + ":" + ZH2_GVars.payPort1);
			webSocket = new WebSocket(new Uri("ws://" + ZH2_GVars.IPAddress + ":" + ZH2_GVars.payPort1));
			WebSocket obj = webSocket;
			obj.OnOpen = (OnWebSocketOpenDelegate)Delegate.Combine(obj.OnOpen, new OnWebSocketOpenDelegate(OnOpen));
			WebSocket obj2 = webSocket;
			obj2.OnMessage = (OnWebSocketMessageDelegate)Delegate.Combine(obj2.OnMessage, new OnWebSocketMessageDelegate(OnMessageReceived));
			WebSocket obj3 = webSocket;
			obj3.OnError = (OnWebSocketErrorDelegate)Delegate.Combine(obj3.OnError, new OnWebSocketErrorDelegate(OnError));
			WebSocket obj4 = webSocket;
			obj4.OnClosed = (OnWebSocketClosedDelegate)Delegate.Combine(obj4.OnClosed, new OnWebSocketClosedDelegate(OnClosed));
			WebSocket obj5 = webSocket;
			obj5.OnBinary = (OnWebSocketBinaryDelegate)Delegate.Combine(obj5.OnBinary, new OnWebSocketBinaryDelegate(Onbinary));
			WebSocket obj6 = webSocket;
			obj6.OnErrorDesc = (OnWebSocketErrorDescriptionDelegate)Delegate.Combine(obj6.OnErrorDesc, new OnWebSocketErrorDescriptionDelegate(OnErrorDesc));
			WebSocket obj7 = webSocket;
			obj7.OnIncompleteFrame = (OnWebSocketIncompleteFrameDelegate)Delegate.Combine(obj7.OnIncompleteFrame, new OnWebSocketIncompleteFrameDelegate(OnIncompleteFrame));
			webSocket.Open();
			if (tempMsg != string.Empty && webSocket != null)
			{
				UnityEngine.Debug.LogError("重新发送: " + tempMsg);
				webSocket.Send(tempMsg);
			}
			else
			{
				UnityEngine.Debug.Log("初始化[已打开]");
			}
		}
	}

	private void AntiInit()
	{
		if (!isStop)
		{
			UnityEngine.Debug.Log("反初始化");
			if (webSocket != null)
			{
				webSocket.Close();
				webSocket.OnOpen = null;
				webSocket.OnMessage = null;
				webSocket.OnError = null;
				webSocket.OnClosed = null;
				webSocket.OnBinary = null;
				webSocket.OnErrorDesc = null;
				webSocket.OnIncompleteFrame = null;
				webSocket = null;
			}
			IsInit = false;
			Init();
		}
	}

	public void Connect()
	{
		AntiInit();
	}

	public void CloseConnect()
	{
		webSocket.Close();
	}

	public void SwitchServer()
	{
		UnityEngine.Debug.Log(ZH2_GVars.IPAddress);
		AntiInit();
	}

	public void DisConnectServer()
	{
		if (webSocket != null)
		{
			webSocket.Close();
		}
	}

	public void SendMsg(int type, byte[] data)
	{
	}

	public void SenServiceMsg(string from_id, string to_id, string content, string send_time, Action<TalkMsg> setEvent)
	{
		ServiceMsgData serviceMsgData = new ServiceMsgData();
		serviceMsgData.from_id = from_id;
		serviceMsgData.to_id = to_id;
		serviceMsgData.content = content;
		serviceMsgData.send_time = send_time;
		serviceMsgData.type = "say";
		talkFinishEvent = setEvent;
		string msg = JsonUtility.ToJson(serviceMsgData);
		SendMsg(msg);
	}

	public void SenServiceIma(Texture2D image, int maxSize, Action<TalkMsg> setEvent)
	{
		string msg = Convert.ToBase64String(image.ScaleTexture(maxSize).EncodeToPNG());
		SendMsg(msg);
	}

	private void TalkCallback()
	{
		if (talkFinishEvent != null)
		{
			talkFinishEvent(null);
		}
	}

	public void SendMsg(string msg)
	{
		if (webSocket == null)
		{
			IsInit = false;
			Init();
			webSocket.Open();
		}
		UnityEngine.Debug.Log("发送数据: " + msg);
		TalkCallback();
		tempMsg = msg;
		webSocket.Send(msg);
	}

	private void OnOpen(WebSocket webSocket)
	{
		count++;
		if (count < 2)
		{
			UnityEngine.Debug.LogError("客服长连接连接成功！");
		}
		else
		{
			UnityEngine.Debug.LogError(count);
		}
		ReqData reqData = new ReqData();
		reqData.from_id = ZH2_GVars.user.id.ToString();
		reqData.to_id = ZH2_GVars.user.promoterId.ToString();
		reqData.type = "bind";
		HeartBeatData heartBeatData = new HeartBeatData();
		heartBeatData.type = "ping";
		string msg = JsonUtility.ToJson(reqData);
		SendMsg(msg);
		string heartBeatString = JsonUtility.ToJson(heartBeatData);
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		coroutine = StartCoroutine(SenHeartbeat(heartBeatString));
	}

	private IEnumerator SenHeartbeat(string heartBeatString)
	{
		while (true && !isStop)
		{
			yield return new WaitForSeconds(5f);
			SendMsg(heartBeatString);
		}
	}

	private void OnApplicationQuit()
	{
		StopWebSocket();
		UnityEngine.Object.Destroy(this);
	}

	private void OnDisable()
	{
		StopWebSocket();
	}

	private void StopWebSocket()
	{
		IsInit = true;
		isStop = true;
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		if (webSocket != null)
		{
			webSocket.Close();
			webSocket.OnOpen = null;
			webSocket.OnMessage = null;
			webSocket.OnError = null;
			webSocket.OnClosed = null;
			webSocket.OnBinary = null;
			webSocket.OnErrorDesc = null;
			webSocket.OnIncompleteFrame = null;
			webSocket = null;
			instance = null;
		}
	}

	private void OnClosed(WebSocket webSocket, ushort code, string message)
	{
		UnityEngine.Debug.Log("断开连接" + code + ":" + message);
		if (webSocket != null)
		{
			AntiInit();
		}
	}

	private void OnError(WebSocket webSocket, Exception ex)
	{
		string message = string.Empty;
		if (webSocket.InternalRequest.Response != null)
		{
			message = $"Status Code from Server: {webSocket.InternalRequest.Response.StatusCode} and Message: {webSocket.InternalRequest.Response.Message}";
		}
		UnityEngine.Debug.Log(message);
		if (webSocket != null)
		{
			AntiInit();
		}
	}

	private void OnMessageReceived(WebSocket webSocket, string message)
	{
		JsonData jsonData = JsonMapper.ToObject(message);
		UnityEngine.Debug.Log("接收到消息 JD: " + jsonData.ToJson());
		try
		{
			switch (jsonData["type"].ToString())
			{
			case "1":
				ZH2_GVars.user.gameGold = int.Parse(jsonData["game_gold"].ToString());
				break;
			case "2":
			{
				string content = jsonData["msg"].ToString();
				MB_Singleton<AlertDialog>.GetInstance().ShowDialog(content);
				break;
			}
			default:
				UnityEngine.Debug.LogError("接收到其他类型消息");
				break;
			}
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("     " + arg);
			throw;
		}
	}

	private void OnIncompleteFrame(WebSocket webSocket, WebSocketFrameReader frame)
	{
		UnityEngine.Debug.Log("接收到消息:");
	}

	private void OnErrorDesc(WebSocket webSocket, string reason)
	{
		UnityEngine.Debug.Log("接收到消息：" + reason);
	}

	private void Onbinary(WebSocket webSocket, byte[] data)
	{
		byte[] bytes = BitConverter.GetBytes(124512);
		byte[] array = new byte[bytes.Length];
		Array.Copy(data, array, array.Length);
		if (BitConverter.ToInt32(array, 0) == 124512)
		{
			byte[] array2 = new byte[data.Length - array.Length];
			Array.Copy(data, array.Length, array2, 0, array2.Length);
		}
		else
		{
			UnityEngine.Debug.Log("接收到未加密的错误信息【无法解析】");
			DisConnectServer();
			try
			{
				UnityEngine.Debug.Log(JsonMapper.ToJson(data));
			}
			catch (Exception)
			{
			}
		}
	}
}
