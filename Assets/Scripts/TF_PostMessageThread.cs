using GameConfig;
using System.Collections;
using UnityEngine;

public class TF_PostMessageThread : MonoBehaviour
{
	private static TF_PostMessageThread _MyThread;

	private bool m_flag;

	private Hashtable m_MyTable;

	private TF_MessageControl m_MyMessageControl;

	private TF_Transmit m_MyTransmit;

	public static TF_PostMessageThread GetSingleton()
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

	public void PostMessageThreadGetPoint(TF_MessageControl MyMessageControl, TF_Transmit MyTransmit)
	{
		m_MyMessageControl = MyMessageControl;
		m_MyTransmit = MyTransmit;
	}

	public void PostThread()
	{
		int num = (TF_GameInfo.getInstance().currentState <= TF_GameState.On_Loading) ? 1 : 10;
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
