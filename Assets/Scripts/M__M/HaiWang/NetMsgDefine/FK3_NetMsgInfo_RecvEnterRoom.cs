using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class FK3_NetMsgInfo_RecvEnterRoom : FK3_NetMsgInfoBase
	{
		public List<FK3_DeskInfo> desks = new List<FK3_DeskInfo>();

		public FK3_NetMsgInfo_RecvEnterRoom(object[] args = null)
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
				base.message = (dictionary["message"] as string);
				if (code == 0)
				{
					object[] array = dictionary["desks"] as object[];
					for (int i = 0; i < array.Length; i++)
					{
						Dictionary<string, object> dictionary2 = array[i] as Dictionary<string, object>;
						FK3_DeskInfo fK3_DeskInfo = new FK3_DeskInfo(string.Empty);
						fK3_DeskInfo.name = (dictionary2["name"] as string);
						fK3_DeskInfo.id = (int)dictionary2["id"];
						fK3_DeskInfo.minGold = (int)dictionary2["minGold"];
						fK3_DeskInfo.minGunValue = (int)dictionary2["minGunValue"];
						fK3_DeskInfo.maxGunValue = (int)dictionary2["maxGunValue"];
						fK3_DeskInfo.addstepGunValue = (int)dictionary2["addstepGunValue"];
						fK3_DeskInfo.exchange = (int)dictionary2["exchange"];
						fK3_DeskInfo.onceExchangeValue = (int)dictionary2["onceExchangeValue"];
						object[] array2 = dictionary2["seats"] as object[];
						for (int j = 0; j < 4; j++)
						{
							Dictionary<string, object> dictionary3 = array2[j] as Dictionary<string, object>;
							int num = (int)dictionary3["id"];
							bool flag = (bool)dictionary3["isFree"];
							FK3_SeatInfo seat = fK3_DeskInfo.GetSeat(num);
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
						desks.Add(fK3_DeskInfo);
					}
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
				valid = false;
			}
		}

		public override void Parse2()
		{
			try
			{
				object[] args = m_args;
				Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
				code = (int)dictionary["code"];
				base.message = (dictionary["message"] as string);
				if (code == 0)
				{
					object[] array = dictionary["deskInfo"] as object[];
					for (int i = 0; i < array.Length; i++)
					{
						Dictionary<string, object> dictionary2 = array[i] as Dictionary<string, object>;
						FK3_DeskInfo fK3_DeskInfo = new FK3_DeskInfo(string.Empty);
						fK3_DeskInfo.name = (dictionary2["name"] as string);
						fK3_DeskInfo.id = (int)dictionary2["id"];
						fK3_DeskInfo.minGold = (int)dictionary2["minGold"];
						fK3_DeskInfo.minGunValue = (int)dictionary2["minGunValue"];
						fK3_DeskInfo.maxGunValue = (int)dictionary2["maxGunValue"];
						fK3_DeskInfo.addstepGunValue = (int)dictionary2["addstepGunValue"];
						fK3_DeskInfo.exchange = (int)dictionary2["exchange"];
						fK3_DeskInfo.onceExchangeValue = (int)dictionary2["onceExchangeValue"];
						object[] array2 = dictionary2["seats"] as object[];
						for (int j = 0; j < 4; j++)
						{
							Dictionary<string, object> dictionary3 = array2[j] as Dictionary<string, object>;
							int num = (int)dictionary3["id"];
							bool flag = (bool)dictionary3["isFree"];
							FK3_SeatInfo seat = fK3_DeskInfo.GetSeat(num);
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
						desks.Add(fK3_DeskInfo);
					}
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
				valid = false;
			}
		}
	}
}
