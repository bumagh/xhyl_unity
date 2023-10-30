using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class FK3_NetMsgInfo_RecvNewPuffer : FK3_NetMsgInfoBase
	{
		public List<FK3_NewPufferItemInfo> items = new List<FK3_NewPufferItemInfo>();

		public FK3_NetMsgInfo_RecvNewPuffer(object[] args = null)
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
						FK3_NewPufferItemInfo fK3_NewPufferItemInfo = new FK3_NewPufferItemInfo();
						fK3_NewPufferItemInfo.fishID = (int)dictionary2["fishId"];
						fK3_NewPufferItemInfo.fishType = (int)dictionary2["fishType"];
						fK3_NewPufferItemInfo.pathID = (int)dictionary2["pathId"];
						fK3_NewPufferItemInfo.openTime = (int)dictionary2["openTime"];
						UnityEngine.Debug.LogError("newPufferItemInfo.openTime: " + fK3_NewPufferItemInfo.openTime);
						items.Add(fK3_NewPufferItemInfo);
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
