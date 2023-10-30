using System.Collections;
using UnityEngine;

public class BaiJiaLe_PostMessageThread : MonoBehaviour
{
	private static BaiJiaLe_PostMessageThread _MyThread;

	private bool m_flag;

	private Hashtable m_MyTable;

	private BaiJiaLe_MessageControl m_MyMessageControl;

	private BaiJiaLe_Transmit m_MyTransmit;

	public static BaiJiaLe_PostMessageThread GetSingleton()
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

	public void BaiJiaLe_PostMessageThreadParaInit()
	{
		m_flag = false;
		m_MyTable = new Hashtable();
	}

	public void BaiJiaLe_PostMessageThreadGetPoint(BaiJiaLe_MessageControl MyMessageControl, BaiJiaLe_Transmit MyTransmit)
	{
		m_MyMessageControl = MyMessageControl;
		m_MyTransmit = MyTransmit;
	}

	public void PostThread()
	{
		int num = BaiJiaLe_NetMngr.isInLoading ? 1 : 10;
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
