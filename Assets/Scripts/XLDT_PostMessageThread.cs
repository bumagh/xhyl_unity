using System.Collections;
using UnityEngine;

public class XLDT_PostMessageThread : MonoBehaviour
{
	private static XLDT_PostMessageThread _MyThread;

	private bool m_flag;

	private Hashtable m_MyTable;

	private XLDT_MessageControl m_MyMessageControl;

	private XLDT_Transmit m_MyTransmit;

	public static XLDT_PostMessageThread GetSingleton()
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

	public void PostMessageThreadGetPoint(XLDT_MessageControl MyMessageControl, XLDT_Transmit MyTransmit)
	{
		m_MyMessageControl = MyMessageControl;
		m_MyTransmit = MyTransmit;
	}

	public void PostThread()
	{
		for (int i = 0; i < 11; i++)
		{
			if (!m_MyMessageControl.PostMessage(ref m_MyTable))
			{
				break;
			}
			m_MyTransmit.PostMsgControl(m_MyTable);
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
