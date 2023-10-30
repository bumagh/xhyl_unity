using System.Collections;
using UnityEngine;

public class JSYS_LL_PostMessageThread : MonoBehaviour
{
	private static JSYS_LL_PostMessageThread _MyThread;

	private bool m_flag;

	private Hashtable m_MyTable;

	private JSYS_LL_MessageControl m_MyMessageControl;

	private JSYS_LL_Transmit m_MyTransmit;

	public static JSYS_LL_PostMessageThread GetSingleton()
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

	public void PostMessageThreadGetPoint(JSYS_LL_MessageControl MyMessageControl, JSYS_LL_Transmit MyTransmit)
	{
		m_MyMessageControl = MyMessageControl;
		m_MyTransmit = MyTransmit;
	}

	public void PostThread()
	{
		int num = JSYS_LL_NetMngr.isInLoading ? 1 : 10;
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
