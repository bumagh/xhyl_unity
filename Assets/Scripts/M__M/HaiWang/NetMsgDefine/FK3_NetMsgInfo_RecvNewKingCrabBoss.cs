using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class FK3_NetMsgInfo_RecvNewKingCrabBoss : FK3_NetMsgInfoBase
	{
		public FK3_NewKingCrabBossItemInfo item = new FK3_NewKingCrabBossItemInfo();

		public FK3_NetMsgInfo_RecvNewKingCrabBoss(object[] args = null)
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
					UnityEngine.Debug.Log($"fishObjs.count:{array.Length}");
					for (int i = 0; i < array.Length; i++)
					{
						Dictionary<string, object> dictionary2 = array[i] as Dictionary<string, object>;
						FK3_NewFishItemInfo fK3_NewFishItemInfo = new FK3_NewFishItemInfo();
						fK3_NewFishItemInfo.fishID = (int)dictionary2["fishId"];
						fK3_NewFishItemInfo.fishType = (int)dictionary2["fishType"];
						fK3_NewFishItemInfo.pathID = (int)dictionary2["pathId"];
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
