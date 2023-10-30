using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class NetMsgInfo_RecvEnterRoom : NetMsgInfoBase
	{
		public List<DeskInfo> desks = new List<DeskInfo>();

		public NetMsgInfo_RecvEnterRoom(object[] args = null)
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
					object[] array = dictionary["desks"] as object[];
					for (int i = 0; i < array.Length; i++)
					{
						Dictionary<string, object> dictionary2 = array[i] as Dictionary<string, object>;
						DeskInfo deskInfo = new DeskInfo(string.Empty);
						deskInfo.name = (dictionary2["name"] as string);
						deskInfo.id = (int)dictionary2["id"];
						deskInfo.minGold = (int)dictionary2["minGold"];
						deskInfo.minGunValue = (int)dictionary2["minGunValue"];
						deskInfo.maxGunValue = (int)dictionary2["maxGunValue"];
						deskInfo.addstepGunValue = (int)dictionary2["addstepGunValue"];
						deskInfo.exchange = (int)dictionary2["exchange"];
						deskInfo.onceExchangeValue = (int)dictionary2["onceExchangeValue"];
						object[] array2 = dictionary2["seats"] as object[];
						for (int j = 0; j < 4; j++)
						{
							Dictionary<string, object> dictionary3 = array2[j] as Dictionary<string, object>;
							int num = (int)dictionary3["id"];
							bool flag = (bool)dictionary3["isFree"];
							SeatInfo seat = deskInfo.GetSeat(num);
							if (seat == null)
							{
								UnityEngine.Debug.LogError($"seat[id:{num}] not valid");
							}
							seat.isUsed = !flag;
							if (seat.isUsed)
							{
								Dictionary<string, object> dictionary4 = (Dictionary<string, object>)dictionary3["user"];
								seat.username = (string)dictionary4["nickname"];
								seat.iconId = (int)dictionary4["photoID"];
								seat.level = (int)dictionary4["level"];
								if (dictionary4.ContainsKey("sex"))
								{
									string a = (string)dictionary4["sex"];
									seat.sex = ((!(a == "女")) ? 1 : 0);
								}
							}
						}
						desks.Add(deskInfo);
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
