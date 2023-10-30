using System.Collections;
using UnityEngine;

public class BCBM_PostMessageThread : MonoBehaviour
{
	private static BCBM_PostMessageThread _MyThread;

	private bool m_flag;

	private Hashtable m_MyTable;

	private BCBM_MessageControl m_MyMessageControl;

	private BCBM_Transmit m_MyTransmit;

	public static BCBM_PostMessageThread GetSingleton()
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

	public void PostMessageThreadGetPoint(BCBM_MessageControl MyMessageControl, BCBM_Transmit MyTransmit)
	{
		m_MyMessageControl = MyMessageControl;
		m_MyTransmit = MyTransmit;
	}

	public void PostThread()
	{
		int num = BCBM_NetMngr.isInLoading ? 1 : 10;
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
