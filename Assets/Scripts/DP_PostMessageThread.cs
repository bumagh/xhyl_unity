using System.Collections;
using UnityEngine;

public class DP_PostMessageThread : MonoBehaviour
{
	private static DP_PostMessageThread _MyThread;

	private bool m_flag;

	private Hashtable m_MyTable;

	private DP_MessageControl m_MyMessageControl;

	private DP_Transmit m_MyTransmit;

	public static DP_PostMessageThread GetSingleton()
	{
		return _MyThread;
	}

	private void Awake()
	{
		if (_MyThread == null)
		{
			UnityEngine.Debug.Log("_MyThread");
			_MyThread = this;
		}
	}

	public void PostMessageThreadParaInit()
	{
		m_flag = false;
		m_MyTable = new Hashtable();
	}

	public void PostMessageThreadGetPoint(DP_MessageControl MyMessageControl, DP_Transmit MyTransmit)
	{
		m_MyMessageControl = MyMessageControl;
		m_MyTransmit = MyTransmit;
	}

	public void PostThread()
	{
		int num = DP_NetMngr.isInLoading ? 1 : 10;
		while (num > 0 && m_MyMessageControl.PostMessage(ref m_MyTable))
		{
			m_MyTransmit.PostMsgControl(m_MyTable);
			num--;
		}
	}

	public void ClearAllState()
	{
		while (m_MyMessageControl.PostMessage(ref m_MyTable))
		{
			m_MyTransmit.SelectPostMsgControl(m_MyTable);
		}
	}
}
