using GameConfig;
using System.Collections;
using UnityEngine;

public class BZJX_PostMessageThread : MonoBehaviour
{
	private static BZJX_PostMessageThread _MyThread;

	private bool m_flag;

	private Hashtable m_MyTable;

	private BZJX_MessageControl m_MyMessageControl;

	private BZJX_Transmit m_MyTransmit;

	public static BZJX_PostMessageThread GetSingleton()
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

	public void PostMessageThreadGetPoint(BZJX_MessageControl MyMessageControl, BZJX_Transmit MyTransmit)
	{
		m_MyMessageControl = MyMessageControl;
		m_MyTransmit = MyTransmit;
	}

	public void PostThread()
	{
		int num = (BZJX_GameInfo.getInstance().currentState <= BZJX_GameState.On_Loading) ? 1 : 10;
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
