using System.Collections;
using System.Threading;
using UnityEngine;

public class STOF_MessageControl : MonoBehaviour
{
	private Mutex m_MessageControlMutex;

	private Queue m_MsgQueue;

	private static STOF_MessageControl _MyMessageControl;

	public static STOF_MessageControl GetSingleton()
	{
		return _MyMessageControl;
	}

	private void Awake()
	{
		if (_MyMessageControl == null)
		{
			_MyMessageControl = this;
		}
	}

	public void MessageControlParaInit()
	{
		m_MessageControlMutex = new Mutex();
		m_MsgQueue = new Queue();
	}

	public void AddMessage(Hashtable MyTable)
	{
		m_MessageControlMutex.WaitOne();
		m_MsgQueue.Enqueue(MyTable);
		m_MessageControlMutex.ReleaseMutex();
	}

	public bool PostMessage(ref Hashtable MyTable)
	{
		m_MessageControlMutex.WaitOne();
		if (m_MsgQueue.Count <= 0)
		{
			m_MessageControlMutex.ReleaseMutex();
			return false;
		}
		MyTable = (Hashtable)m_MsgQueue.Dequeue();
		m_MessageControlMutex.ReleaseMutex();
		return true;
	}
}
