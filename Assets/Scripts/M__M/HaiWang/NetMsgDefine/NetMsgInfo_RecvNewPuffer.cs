using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class NetMsgInfo_RecvNewPuffer : NetMsgInfoBase
	{
		public List<NewPufferItemInfo> items = new List<NewPufferItemInfo>();

		public NetMsgInfo_RecvNewPuffer(object[] args = null)
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
					object[] array = dictionary["fishes"] as object[];
					for (int i = 0; i < array.Length; i++)
					{
						Dictionary<string, object> dictionary2 = array[i] as Dictionary<string, object>;
						NewPufferItemInfo newPufferItemInfo = new NewPufferItemInfo();
						newPufferItemInfo.fishID = (int)dictionary2["fishId"];
						newPufferItemInfo.fishType = (int)dictionary2["fishType"];
						newPufferItemInfo.pathID = (int)dictionary2["pathId"];
						newPufferItemInfo.openTime = (int)dictionary2["openTime"];
						items.Add(newPufferItemInfo);
					}
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
