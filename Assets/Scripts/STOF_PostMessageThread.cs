using GameConfig;
using System.Collections;
using UnityEngine;

public class STOF_PostMessageThread : MonoBehaviour
{
	private static STOF_PostMessageThread _MyThread;

	private bool m_flag;

	private Hashtable m_MyTable;

	private STOF_MessageControl m_MyMessageControl;

	private STOF_Transmit m_MyTransmit;

	public static STOF_PostMessageThread GetSingleton()
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

	public void PostMessageThreadGetPoint(STOF_MessageControl MyMessageControl, STOF_Transmit MyTransmit)
	{
		m_MyMessageControl = MyMessageControl;
		m_MyTransmit = MyTransmit;
	}

	public void PostThread()
	{
		int num = (STOF_GameInfo.getInstance().currentState <= STOF_GameState.On_Loading) ? 1 : 10;
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
