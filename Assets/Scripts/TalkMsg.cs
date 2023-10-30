using System;
using UnityEngine;

public class TalkMsg
{
	public string sendUsername;

	public string targetUsername;

	public DateTime msgTime;

	public DateTime readTime;

	public string msg;

	private bool m_isRead;

	public Texture2D image;

	public long timeTicks
	{
		get
		{
			DateTime dateTime = msgTime;
			return msgTime.Ticks;
		}
	}

	public bool isRead
	{
		get
		{
			return m_isRead;
		}
		set
		{
			if (value && !m_isRead)
			{
				readTime = DateTime.Now;
			}
			m_isRead = value;
		}
	}

	public TalkMsg()
	{
	}

	public TalkMsg(string setUsername, string setMsg)
	{
		sendUsername = setUsername;
		msg = setMsg;
	}

	public override string ToString()
	{
		string empty = string.Empty;
		return empty + sendUsername + "," + targetUsername + "," + msgTime.Ticks + "," + readTime.Ticks + "," + msg + "," + (isRead ? 1 : 0);
	}

	public bool Equals(TalkMsg obj)
	{
		return sendUsername == obj.sendUsername && msgTime.Ticks == obj.msgTime.Ticks && msg == obj.msg;
	}
}
