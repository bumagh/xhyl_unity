using System.Collections;
using UnityEngine;

public class LHD_PostMessageThread : MonoBehaviour
{
	private static LHD_PostMessageThread _MyThread;

	private bool m_flag;

	private Hashtable m_MyTable;

	private LHD_MessageControl m_MyMessageControl;

	private LHD_Transmit m_MyTransmit;

	public static LHD_PostMessageThread GetSingleton()
	{
		return _MyThread;
	}

	private void Awake()
	{
		if (_MyThread == null)
		{
			_MyThread = this;
		}
	}

	public void PostMessageThreadParaInit()
	{
		m_flag = false;
		m_MyTable = new Hashtable();
	}

	public void PostMessageThreadGetPoint(LHD_MessageControl MyMessageControl, LHD_Transmit MyTransmit)
	{
		m_MyMessageControl = MyMessageControl;
		m_MyTransmit = MyTransmit;
	}

	public void PostThread()
	{
		int num = LHD_NetMngr.isInLoading ? 1 : 10;
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
