using JsonFx.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NetManager : MB_Singleton<NetManager>
{
	public delegate void NetMessageHandler(object[] args);

	private ClientSocket _pClient;

	private DataEncrypt _pCrypto;

	public bool isConnected;

	public bool isReady;

	public bool isLogined;

	private FakeNet _fake;

	public bool useFake = true;

	public int connectCount;

	public float connectTimeCount;

	public int connectMaxTimes = 20;

	public float connectMaxTimeout;

	public bool dropSend;

	public bool dropDispatch;

	public bool isHeartPong = true;

	public bool printSendLog;

	public bool printRecvLog;

	public bool autoReady;

	private Coroutine _coKeepHeart;

	private Queue<NetMessage> _netMsgQueue = new Queue<NetMessage>();

	private Dictionary<string, NetMessageHandler> _netMsgHandlerDic = new Dictionary<string, NetMessageHandler>();

	private Coroutine checkHeartTimeout;

	private void Awake()
	{
        if (_instance == null)
        {
            SetInstance(this, false);
        }
        RegisterHandler("netConnected", new NetMessageHandler(_onConnected));
    }

    private void _initClientSocket()
    {
        Debug.Log("_initClientSocket");
        isHeartPong = true;
        if (useFake)
        {
            Debug.Log("use <color=magenta>fake</color> net");
            isConnected = true;
            isReady = true;
            isLogined = true;
            _fake = new FakeNet(this, _netMsgQueue);
            _fake.Init();
        }
        else
        {
            isConnected = false;
            isReady = false;
            isLogined = false;
            if (_pClient != null)
            {
                _pClient.Close();
            }
            if (_pCrypto != null)
            {
                _pCrypto = null;
            }
            _pClient = new ClientSocket(true);
            _pCrypto = new DataEncrypt();
            _pClient.dataHandler = new Action<byte[], int, int>(_handleData);
            _pClient.exceptionHandler = new Action<Exception>(_handleException);
            _pClient.connectHandler = new Action(_handleConnected);
            _pClient.checkTimeoutHandler = new Action<int, int, ClientSocket>(_checkTimeoutStart);
        }
    }

    private void _onConnected(object[] args)
	{
		ZH2_GVars.isConnect = true;
		UnityEngine.Debug.LogError("===大厅链接成功!!!");
		if (autoReady)
		{
			SendPublicKey();
		}
        if (_coKeepHeart != null)
        {
            MB_Singleton<AppManager>.Get().StopCoroutine(_coKeepHeart);
        }
        _coKeepHeart = MB_Singleton<AppManager>.Get().StartCoroutine(GetInstance().KeepHeart());
        GetInstance().connectCount = 0;
    }

	private void _handleException(Exception ex)
	{
        Debug.LogError("_handleException");
        isConnected = false;
        isReady = false;
        isLogined = false;
        Queue<NetMessage> netMsgQueue = _netMsgQueue;
        lock (netMsgQueue)
        {
            _netMsgQueue.Enqueue(new NetMessage("netDown", new object[]
            {
                    ex
            }));
        }
    }

	public void Connect(string ip, int port)
	{
        Debug.Log("Connect");
        connectCount++;
        _initClientSocket();
        if (useFake)
        {
            return;
        }
        _pClient.Connect(ip, port);
    }

	private void _handleConnected()
	{
        try
        {
            isConnected = true;
            Queue<NetMessage> netMsgQueue = _netMsgQueue;
            lock (netMsgQueue)
            {
                _netMsgQueue.Enqueue(new NetMessage("netConnected", new object[0]));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("_onConnected> exception: " + ex.Message);
            _handleException(ex);
        }
    }

	public void SendPublicKey()
	{
        string[] array = new string[2];
        array = _pCrypto.NetConnectGetRsaKey();
        object[] args = new object[]
        {
                array[0],
                array[1]
        };
        Send("gcuserService/publicKey", args);
    }

	public IEnumerator KeepHeart()
	{
        while (true)
        {
            if (isConnected && isReady)
            {
                Send("gcuserService/heart", new object[0]);
                isHeartPong = false;
            }
            yield return new WaitForSeconds(5f);
        }
    }

	public void Handle_sendServerTime(object[] args)
	{
        Debug.Log("Handle_sendServerTime");
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary = (args[0] as Dictionary<string, object>);
        long serverTime = Convert.ToInt64(dictionary["time"]);
        string str = (string)dictionary["key"];
        _pCrypto.setServerTime(serverTime);
        _pCrypto.DecryptKey(str);
        isReady = true;
    }

    protected void _handleData(byte[] msgBuffer, int packetId, int endIndex)
    {
        _pCrypto.GetKey();
        if (_pCrypto.GetKey() != "none")
        {
            msgBuffer = _pCrypto.Decrypt(msgBuffer);
        }
        Hashtable hashtable = new Hashtable();
        string @string = Encoding.UTF8.GetString(msgBuffer);
        hashtable = JsonReader.Deserialize<Hashtable>(@string);
        string text = hashtable["method"].ToString();
        object[] args = hashtable["args"] as object[];
        connectTimeCount = 0f;
        if (text == "publicKey")
        {
            Handle_sendServerTime(args);
            return;
        }
        if (text == "heart")
        {
            //Debug.Log(LogHelper.Aqua("heart pong"));
            isHeartPong = true;
            return;
        }
        Queue<NetMessage> netMsgQueue = _netMsgQueue;
        lock (netMsgQueue)
        {
            NetMessage netMessage = new NetMessage(text, args);
            netMessage.jsonString = @string;
            netMessage.packetId = packetId;
            netMessage.endIndex = endIndex;
            _netMsgQueue.Enqueue(netMessage);
        }
    }

    private IEnumerator _checkTimeout(int id, int timeout, ClientSocket cs)
    {

        Debug.Log(LogHelper.Aqua("_checkTimeout start id: {0}", new object[]
        {
                id
        }));

        yield return new WaitForSeconds(timeout / 1000f);
        bool isTimeout = false;
        if (cs != null && cs.doneList.Count > id && !cs.doneList[id])
        {
            Queue<NetMessage> netMsgQueue = _netMsgQueue;
            lock (netMsgQueue)
            {
                _netMsgQueue.Enqueue(new NetMessage("timeout", new object[]
                {
                        id
                }));
                isTimeout = true;
            }
        }
        if (isTimeout)
        {
            Debug.Log(LogHelper.Aqua("_checkTimeout start id: {0} timeout: {1}", new object[]
            {
                    id,
                    isTimeout
            }));
        }
        yield break;
    }

    private IEnumerator _checkHeartTimeout(int timeout)
    {
        yield return new WaitForSeconds(timeout / 1000f);
        bool isTimeout = false;
        if (isReady && !isHeartPong)
        {
            Queue<NetMessage> netMsgQueue = _netMsgQueue;
            lock (netMsgQueue)
            {
                isConnected = false;
                isReady = false;
                isLogined = false;
                _netMsgQueue.Enqueue(new NetMessage("heart timeout", new object[0]));
                isTimeout = true;
            }
        }
        if (isTimeout)
        {
            Debug.Log(LogHelper.Aqua("_checkHeartTimeout is timeout"));
        }
        yield break;
    }


    public void _checkTimeoutStart(int id, int timeout, ClientSocket cs)
	{
		StartCoroutine(_checkTimeout(id, timeout, cs));
	}

    public void RegisterHandler(string method, NetMessageHandler hander)
    {
        if (!_netMsgHandlerDic.ContainsKey(method))
        {
            NetMessageHandler value = new NetMessageHandler(hander.Invoke);
            _netMsgHandlerDic.Add(method, value);
        }
        else
        {
            _netMsgHandlerDic[method] = hander;
            Debug.Log(method + " has multicast hander");
        }
    }
    public void UnRegisterHandler(string method)
    {
        if (this._netMsgHandlerDic.ContainsKey(method))
        {
            this._netMsgHandlerDic.Remove(method);
        }
    }

    public bool IsMethodRegistered(string method)
	{
		return _netMsgHandlerDic.ContainsKey(method);
	}

    public void Send(string method, object[] args)
    {
        if (method != "gcuserService/heart")
        {
            Debug.Log(LogHelper.Olive("send>>method: ") + LogHelper.Key(method, "#AAffffff"));
        }
        else
        {
            StartCoroutine(_checkHeartTimeout(3000));
        }
        if (dropSend && method != "gcuserService/heart")
        {
            Debug.Log(LogHelper.Magenta("drop send>>method: {0}", new object[]
            {
                    method
            }));
            return;
        }
        if (useFake)
        {
            StartCoroutine(_fake.MakeResponse(method, args));
            return;
        }
        Hashtable hashtable = new Hashtable();
        hashtable.Add("method", method);
        hashtable.Add("args", args);
        hashtable.Add("version",ZH2_GVars.netVersion);
        if (isReady)
        {
            hashtable.Add("time", _pCrypto.GetUnixTime());
        }
        string text = JsonWriter.Serialize(hashtable);
        byte[] array = Encoding.UTF8.GetBytes(text);
        if (method != "gcuserService/heart")
        {
            if (method == "userService/publicKey")
            {
                Debug.Log("send>>json: " + "*".Repeat(10));
            }
            else
            {
                Debug.Log("send>>json: " + text);
            }
        }
        if (_pCrypto.GetKey() != "none")
        {
            array = _pCrypto.Encrypt(array);
        }
        _pClient.Send(array, true);
    }

    private void Update()
	{
		Dispatching();
	}

	private void Dispatching()
	{
		NetMessage netMsg;
		while (GetNetMessage(out netMsg))
		{
			DispatchNetMessage(netMsg);
		}
	}

	private bool GetNetMessage(out NetMessage netMsg)
	{
        Queue<NetMessage> netMsgQueue = _netMsgQueue;
        bool result;
        lock (netMsgQueue)
        {
            if (_netMsgQueue.Count == 0)
            {
                netMsg = null;
                result = false;
            }
            else
            {
                netMsg = _netMsgQueue.Dequeue();
                result = true;
            }
        }
        return result;
    }

	private void DispatchNetMessage(NetMessage netMsg)
	{
        Debug.Log(string.Format("{0}> packetId: {1}, endIndex: {2}", netMsg.method, netMsg.packetId, netMsg.endIndex));
        if (netMsg.jsonString != null && netMsg.jsonString.Length > 0)
        {
            Debug.Log("recv<<json: " + netMsg.jsonString);
        }
        if (dropDispatch)
        {
            Debug.Log(LogHelper.Magenta("drop send>>method: {0}", new object[]
            {
                    netMsg.method
            }));
            return;
        }
        if (_netMsgHandlerDic.ContainsKey(netMsg.method))
        {
            _netMsgHandlerDic[netMsg.method](netMsg.args);
        }
        else
        {
            //Debug.LogError("DispatchNetMessage>unknown msg: " + netMsg.method);
        }
    }

	public void OnDestroy()
	{
        Debug.LogError("NetworkManager OnDestory");
        if (_pClient != null)
        {
            _pClient.Close();
        }
    }

	public void Disconnect()
	{
		if (_pClient != null)
		{
			_pClient.Close();
		}
	}
}
