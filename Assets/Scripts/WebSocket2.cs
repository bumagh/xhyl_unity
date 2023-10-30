using BestHTTP.WebSocket;
using BestHTTP.WebSocket.Frames;
using LitJson;
using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class WebSocket2 : MonoBehaviour
{
	public class BangDingReqData
	{
		public string from_id;

		public string to_id;

		public string type;
	}

	private static WebSocket2 instance;

	public static WebSocket webSocket;

	public static string tempMsg = string.Empty;

	private float _updateInterval = 1f;

	private float _accum;

	private int _frames;

	private float _timeLeft;

	private Coroutine waitGet;

	private bool IsInit;

	private string iPAddressTemp = string.Empty;

	private bool isStop;

	private bool isSendDuiHuan;

	private Coroutine waitSenEnterOrOut;

	private int SenAgainNum;

	private Coroutine coroutine;

	public static WebSocket2 GetInstance()
	{
		if (instance == null)
		{
			UnityEngine.Debug.LogError("instance为空");
			GameObject gameObject = GameObject.Find("WebSocket2");
			if ((bool)gameObject)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			instance = new GameObject("WebSocket2").AddComponent<WebSocket2>();
			instance.isStop = false;
			instance.IsInit = false;
			instance.Init();
			instance.StartWaitGet();
			UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);
		}
		return instance;
	}

	private void StartWaitGet()
	{
	}

	private IEnumerator WaitGet()
	{
		while (true)
		{
			if (instance == null)
			{
				instance = GetInstance();
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	private void Awake()
	{
		UnityEngine.Debug.LogError("=======长链接Awake======");
		isStop = false;
		IsInit = false;
		instance = this;
		base.gameObject.name = "WebSocket2";
	}

	private void Start()
	{
	}

	private void Init()
	{
		IsInit = true;
	}

	private string DoGetHostAddresses(string hostname)
	{
		IPAddress address = null;
		if (IPAddress.TryParse(hostname, out address))
		{
			return address.ToString();
		}
		return Dns.GetHostEntry(hostname).AddressList[0].ToString();
	}

	private void AntiInit()
	{
	}

	public void DisConnectServer()
	{
		UnityEngine.Debug.LogError("====DisConnectServer====");
		if (webSocket != null)
		{
			webSocket.Close();
		}
	}

	public void SendMsg(string msg, bool isDuiHuan = false, int duiMoney = 0, bool isEnterOrOutRoom = false)
	{
	}

	public void SenEnterGame(bool isEnterGame, ZH2_GVars.GameType gameType, string desk_id, string game_score)
	{
	}

	private IEnumerator WaitSenEnterOrOut()
	{
		yield return new WaitForSeconds(2f);
	}

	private IEnumerator SenHeartbeat()
	{
		yield return new WaitForSeconds(1f);
	}

	private IEnumerator SenHeartbeat(int duiMoney)
	{
		yield return new WaitForSeconds(3.5f);
	}

	public void SendMsg(byte[] data)
	{
		byte[] bytes = BitConverter.GetBytes(29099);
		byte[] array = new byte[data.Length + bytes.Length];
		bytes.CopyTo(array, 0);
		data.CopyTo(array, bytes.Length);
		if (webSocket == null)
		{
			IsInit = false;
			Init();
			webSocket.Open();
		}
		WebSocketFrame frame = new WebSocketFrame(webSocket, WebSocketFrameTypes.Binary, array);
		webSocket.Send(frame);
	}

	private void OnOpen(WebSocket webSocket)
	{
	}

	private void OnApplicationQuit()
	{
		UnityEngine.Debug.LogError("=====OnApplicationQuit====");
		instance = null;
		StopWebSocket();
		DisConnectServer();
		UnityEngine.Object.Destroy(this);
	}

	private void StopWebSocket()
	{
		UnityEngine.Debug.LogError("====停止StopWebSocket====");
		IsInit = true;
		isStop = true;
		if (webSocket != null)
		{
			UnityEngine.Debug.LogError("====StopWebSocket====");
			webSocket.Close();
			webSocket.OnOpen = null;
			webSocket.OnMessage = null;
			webSocket.OnError = null;
			webSocket.OnClosed = null;
			webSocket.OnBinary = null;
			webSocket.OnErrorDesc = null;
			webSocket.OnIncompleteFrame = null;
			webSocket = null;
			UnityEngine.Debug.LogError("====instance置空====");
			instance = null;
		}
	}

	private void OnClosed(WebSocket webSocket, ushort code, string message)
	{
		UnityEngine.Debug.LogError("断开连接" + code + ":" + message);
		if (webSocket != null)
		{
			AntiInit();
		}
	}

	private void OnError(WebSocket webSocket, Exception ex)
	{
		string text = string.Empty;
		if (webSocket.InternalRequest.Response != null)
		{
			text = $"Status Code from Server: {webSocket.InternalRequest.Response.StatusCode} and Message: {webSocket.InternalRequest.Response.Message}";
		}
		if (text != string.Empty)
		{
			UnityEngine.Debug.LogError(text);
		}
		if (webSocket != null)
		{
			AntiInit();
		}
	}

	private void OnMessageReceived(WebSocket webSocket, string message)
	{
	}

	private DT_GameTipManager GetDTTip()
	{
		GameObject gameObject = GameObject.Find("DTGameTipManager");
		if (gameObject != null)
		{
			return gameObject.GetComponent<DT_GameTipManager>();
		}
		gameObject = Resources.Load<GameObject>("GameTipPanel");
		GameObject gameObject2 = GameObject.Find("Canvas");
		GameObject gameObject3 = null;
		if (gameObject2 != null)
		{
			Transform transform = gameObject2.transform.Find("DTRoot");
			gameObject3 = ((!(transform != null)) ? UnityEngine.Object.Instantiate(gameObject, gameObject2.transform) : UnityEngine.Object.Instantiate(gameObject, transform));
		}
		if (gameObject3 != null)
		{
			gameObject3.name = "DTGameTipManager";
			return gameObject3.GetComponent<DT_GameTipManager>();
		}
		gameObject3 = UnityEngine.Object.Instantiate(gameObject);
		return gameObject3.GetComponent<DT_GameTipManager>();
	}

	private IEnumerator SenSave(string url)
	{
		UnityEngine.Debug.LogError("访问了: " + url);
		UnityWebRequest www = UnityWebRequest.Get(url);
		www.timeout = 10000;
		yield return www.Send();
		if (www.error != null)
		{
			UnityEngine.Debug.LogError("错误: " + www.error);
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("订单未响应,请联系客服", "No response to the order, please contact customer service", string.Empty), showOkCancel: false, delegate
			{
				CosleProperSecurityPanel();
			});
			yield break;
		}
		JsonData jsonData = JsonMapper.ToObject(www.downloadHandler.text);
		string empty = string.Empty;
		string content;
		try
		{
			content = jsonData["msg"].ToString();
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
			content = ZH2_GVars.ShowTip("订单未响应,请联系客服", "No response to the order, please contact customer service", string.Empty);
		}
		try
		{
			ZH2_GVars.user.gameGold = int.Parse(jsonData["game_gold"].ToString());
		}
		catch (Exception message2)
		{
			UnityEngine.Debug.LogError(message2);
		}
		MB_Singleton<AlertDialog>.GetInstance().ShowDialog(content, showOkCancel: false, delegate
		{
			CosleProperSecurityPanel();
		});
	}

	private void CosleProperSecurityPanel()
	{
		MB_Singleton<AppManager>.Get().m_mainPanel.CoslePanel("CashPanel");
	}

	private void OnIncompleteFrame(WebSocket webSocket, WebSocketFrameReader frame)
	{
		UnityEngine.Debug.LogWarning("接收到消息:");
	}

	private void OnErrorDesc(WebSocket webSocket, string reason)
	{
		UnityEngine.Debug.LogWarning("接收到消息：" + reason);
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
