using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class FK3_NetMsgInfo_RecvNewFishGroup : FK3_NetMsgInfoBase
	{
		public FK3_NewFishGroupItemInfo item = new FK3_NewFishGroupItemInfo();

		public FK3_NetMsgInfo_RecvNewFishGroup(object[] args = null)
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
					item.startFishID = (int)dictionary["startFishId"];
					item.fishCount = (int)dictionary["fishCount"];
					item.groupID = (int)dictionary["groupId"];
					item.pathID = (int)dictionary["pathId"];
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
