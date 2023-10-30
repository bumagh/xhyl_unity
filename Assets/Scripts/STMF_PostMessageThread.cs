using GameConfig;
using System.Collections;
using UnityEngine;

public class STMF_PostMessageThread : MonoBehaviour
{
	private static STMF_PostMessageThread _MyThread;

	private bool m_flag;

	private Hashtable m_MyTable;

	private STMF_MessageControl m_MyMessageControl;

	private STMF_Transmit m_MyTransmit;

	public static STMF_PostMessageThread GetSingleton()
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

	public void PostMessageThreadGetPoint(STMF_MessageControl MyMessageControl, STMF_Transmit MyTransmit)
	{
		m_MyMessageControl = MyMessageControl;
		m_MyTransmit = MyTransmit;
	}

	public void PostThread()
	{
		int num = (STMF_GameInfo.getInstance().currentState <= STMF_GameState.On_Loading) ? 1 : 10;
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
