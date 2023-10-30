using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class NetMsgInfo_RecvNewFish : NetMsgInfoBase
	{
		public List<NewFishItemInfo> items = new List<NewFishItemInfo>();

		public NetMsgInfo_RecvNewFish(object[] args = null)
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
						NewFishItemInfo newFishItemInfo = new NewFishItemInfo();
						newFishItemInfo.fishID = (int)dictionary2["fishId"];
						newFishItemInfo.fishType = (int)dictionary2["fishType"];
						newFishItemInfo.pathID = (int)dictionary2["pathId"];
						items.Add(newFishItemInfo);
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
