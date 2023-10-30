using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class NetMsgInfo_RecvNewDeepSeaOctopus : NetMsgInfoBase
	{
		public NewDeepSeaOctopusBossItemInfo item = new NewDeepSeaOctopusBossItemInfo();

		public NetMsgInfo_RecvNewDeepSeaOctopus(object[] args = null)
		{
			m_args = args;
		}

		public override void Parse()
		{
			try
			{
				object[] args = m_args;
				Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
				code = (int)dictionary["code"];
				message = (dictionary["message"] as string);
				if (code == 0)
				{
					item.fishID = (int)dictionary["fishId"];
					item.pathID = (int)dictionary["pathId"];
					item.remainTime = (int)dictionary["remainTime"];
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.Message);
				valid = false;
			}
		}
	}
}
