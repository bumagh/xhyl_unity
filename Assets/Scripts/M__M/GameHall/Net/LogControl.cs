using System;
using UnityEngine;

namespace M__M.GameHall.Net
{
	[Serializable]
	public class LogControl
	{
		[SerializeField]
		private bool m_enable = true;

		[SerializeField]
		private bool m_send = true;

		[SerializeField]
		private bool m_sendBody;

		[SerializeField]
		private bool m_recv = true;

		[SerializeField]
		private bool m_recvBody;

		[SerializeField]
		private bool m_recvPacketInfo;

		[SerializeField]
		private bool m_recvBinLog;

		[SerializeField]
		private bool m_logHeart;

		public bool enable => m_enable;

		public bool send => m_enable && m_send;

		public bool sendBody => m_enable && m_send && m_sendBody;

		public bool recv => m_enable && m_recv;

		public bool recvBody => m_enable && m_recv && m_recvBody;

		public bool recvPacketInfo => m_enable && m_recv && m_recvPacketInfo;

		public bool recvBinLog => m_enable && m_recv && m_recvBinLog;

		public bool heart => m_enable && m_logHeart;
	}
}
