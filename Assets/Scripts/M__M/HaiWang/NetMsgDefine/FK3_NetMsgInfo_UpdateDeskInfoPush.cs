using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class FK3_NetMsgInfo_UpdateDeskInfoPush : FK3_NetMsgInfoBase
	{
		public List<FK3_SeatInfo2> seats = new List<FK3_SeatInfo2>();

		public FK3_NetMsgInfo_UpdateDeskInfoPush(object[] args = null)
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
					object[] array = dictionary["seats"] as object[];
					UnityEngine.Debug.Log($"seatObjs.count:{array.Length}");
					for (int i = 0; i < array.Length; i++)
					{
						Dictionary<string, object> dictionary2 = array[i] as Dictionary<string, object>;
						FK3_SeatInfo2 fK3_SeatInfo = new FK3_SeatInfo2();
						fK3_SeatInfo.id = (int)dictionary2["id"];
						fK3_SeatInfo.isFree = (bool)dictionary2["isFree"];
						fK3_SeatInfo.gunValue = (int)dictionary2["gunValue"];
						if (!fK3_SeatInfo.isFree)
						{
							Dictionary<string, object> dictionary3 = dictionary2["user"] as Dictionary<string, object>;
							fK3_SeatInfo.user = new FK3_HW2_UserInfo
							{
								id = (int)dictionary3["id"],
								nickname = (string)dictionary3["nickname"],
								gameScore = (int)dictionary3["gameScore"],
								level = (int)dictionary3["level"],
								photoId = (int)dictionary3["photoID"]
							};
						}
						seats.Add(fK3_SeatInfo);
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
