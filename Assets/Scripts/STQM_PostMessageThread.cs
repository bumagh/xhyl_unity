using GameConfig;
using System.Collections;
using UnityEngine;

public class STQM_PostMessageThread : MonoBehaviour
{
	private static STQM_PostMessageThread _MyThread;

	private bool m_flag;

	private Hashtable m_MyTable;

	private STQM_MessageControl m_MyMessageControl;

	private STQM_Transmit m_MyTransmit;

	public static STQM_PostMessageThread GetSingleton()
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

	public void PostMessageThreadGetPoint(STQM_MessageControl MyMessageControl, STQM_Transmit MyTransmit)
	{
		m_MyMessageControl = MyMessageControl;
		m_MyTransmit = MyTransmit;
	}

	public void PostThread()
	{
		int num = (STQM_GameInfo.getInstance().currentState <= STQM_GameState.On_Loading) ? 1 : 10;
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
