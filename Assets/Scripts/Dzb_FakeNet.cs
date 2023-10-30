using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dzb_FakeNet
{
	public delegate void ResponseHandler(object[] args);

	private Dzb_NetManager _net;

	private Queue<Dzb_NetMessage> _netMsgQueue;

	private Dictionary<string, string> _reqToRespMap = new Dictionary<string, string>();

	private Dictionary<string, ResponseHandler> _handerMap = new Dictionary<string, ResponseHandler>();

	public Dzb_FakeNet(Dzb_NetManager net, Queue<Dzb_NetMessage> netMsgQueue)
	{
		_net = net;
		_netMsgQueue = netMsgQueue;
	}

	public void RegisterHandler(string reqMethod, string respMethod, ResponseHandler hander)
	{
		_reqToRespMap.Add(reqMethod, respMethod);
		if (!_handerMap.ContainsKey(respMethod))
		{
			_handerMap.Add(respMethod, hander);
		}
	}

	public void Init()
	{
	}

	public IEnumerator MakeResponse(string reqMethod, object[] args)
	{
		yield return new WaitForSeconds(0.1f);
		if (_reqToRespMap.ContainsKey(reqMethod))
		{
			string text = _reqToRespMap[reqMethod];
			if (text != "empty")
			{
				ResponseHandler responseHandler = _handerMap[text];
				responseHandler(args);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("unregister method: " + reqMethod);
		}
	}
}
