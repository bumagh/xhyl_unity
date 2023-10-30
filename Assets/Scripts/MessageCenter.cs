using System;
using System.Collections.Generic;

public class MessageCenter : SingletonMonoBehaviour<MessageCenter>
{
	private Dictionary<eProtocalCommand, Callback_NetMessage_Handle> _netMessage_EventList = new Dictionary<eProtocalCommand, Callback_NetMessage_Handle>();

	public Queue<sEvent_NetMessageData> _netMessageDataQueue = new Queue<sEvent_NetMessageData>();

	private Dictionary<eGameLogicEventType, Callback_GameLogic_Handle> _gameLogic_EventList = new Dictionary<eGameLogicEventType, Callback_GameLogic_Handle>();

	public Queue<sEvent_GameLogicData> _gameLogicDataQueue = new Queue<sEvent_GameLogicData>();

	public void addObsever(eProtocalCommand _protocalType, Callback_NetMessage_Handle _callback)
	{
		if (_netMessage_EventList.ContainsKey(_protocalType))
		{
			Dictionary<eProtocalCommand, Callback_NetMessage_Handle> netMessage_EventList;
			eProtocalCommand key;
			(netMessage_EventList = _netMessage_EventList)[key = _protocalType] = (Callback_NetMessage_Handle)Delegate.Combine(netMessage_EventList[key], _callback);
		}
		else
		{
			_netMessage_EventList.Add(_protocalType, _callback);
		}
	}

	public void removeObserver(eProtocalCommand _protocalType, Callback_NetMessage_Handle _callback)
	{
		if (_netMessage_EventList.ContainsKey(_protocalType))
		{
			Dictionary<eProtocalCommand, Callback_NetMessage_Handle> netMessage_EventList;
			eProtocalCommand key;
			(netMessage_EventList = _netMessage_EventList)[key = _protocalType] = (Callback_NetMessage_Handle)Delegate.Remove(netMessage_EventList[key], _callback);
			if (_netMessage_EventList[_protocalType] == null)
			{
				_netMessage_EventList.Remove(_protocalType);
			}
		}
	}

	public void AddEventListener(eGameLogicEventType _eventType, Callback_GameLogic_Handle _callback)
	{
		if (_gameLogic_EventList.ContainsKey(_eventType))
		{
			Dictionary<eGameLogicEventType, Callback_GameLogic_Handle> gameLogic_EventList;
			eGameLogicEventType key;
			(gameLogic_EventList = _gameLogic_EventList)[key = _eventType] = (Callback_GameLogic_Handle)Delegate.Combine(gameLogic_EventList[key], _callback);
		}
		else
		{
			_gameLogic_EventList.Add(_eventType, _callback);
		}
	}

	public void RemoveEventListener(eGameLogicEventType _eventType, Callback_GameLogic_Handle _callback)
	{
		if (_gameLogic_EventList.ContainsKey(_eventType))
		{
			Dictionary<eGameLogicEventType, Callback_GameLogic_Handle> gameLogic_EventList;
			eGameLogicEventType key;
			(gameLogic_EventList = _gameLogic_EventList)[key = _eventType] = (Callback_GameLogic_Handle)Delegate.Remove(gameLogic_EventList[key], _callback);
			if (_gameLogic_EventList[_eventType] == null)
			{
				_gameLogic_EventList.Remove(_eventType);
			}
		}
	}

	public void PostEvent(eGameLogicEventType _eventType, object data = null)
	{
		if (_gameLogic_EventList.ContainsKey(_eventType))
		{
			_gameLogic_EventList[_eventType](data);
		}
	}

	private void Update()
	{
		while (_gameLogicDataQueue.Count > 0)
		{
			sEvent_GameLogicData sEvent_GameLogicData = _gameLogicDataQueue.Dequeue();
			if (_gameLogic_EventList.ContainsKey(sEvent_GameLogicData._eventType))
			{
				_gameLogic_EventList[sEvent_GameLogicData._eventType](sEvent_GameLogicData._eventData);
			}
		}
		while (_netMessageDataQueue.Count > 0)
		{
			lock (_netMessageDataQueue)
			{
				sEvent_NetMessageData sEvent_NetMessageData = _netMessageDataQueue.Dequeue();
				if (_netMessage_EventList.ContainsKey(sEvent_NetMessageData._eventType))
				{
					_netMessage_EventList[sEvent_NetMessageData._eventType](sEvent_NetMessageData._eventData);
				}
			}
		}
	}
}
