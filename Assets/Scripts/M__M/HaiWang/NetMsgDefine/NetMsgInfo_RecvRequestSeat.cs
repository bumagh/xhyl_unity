using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class NetMsgInfo_RecvRequestSeat : NetMsgInfoBase
	{
		public List<SeatInfo2> seats = new List<SeatInfo2>();

		public int seatId;

		public int sceneId;

		public int stageId;

		public int stageType;

		public int sceneDuration;

		public int stageDuration;

		public NetMsgInfo_RecvRequestSeat(object[] args = null)
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
					seatId = (int)dictionary["seatId"];
					sceneId = (int)dictionary["scene"];
					stageId = (int)dictionary["stage"];
					stageType = (int)dictionary["stageType"];
					sceneDuration = (int)dictionary["sceneDuration"];
					stageDuration = (int)dictionary["stageDuration"];
					object[] array = dictionary["seats"] as object[];
					UnityEngine.Debug.Log($"seatObjs.count:{array.Length}");
					for (int i = 0; i < array.Length; i++)
					{
						Dictionary<string, object> dictionary2 = array[i] as Dictionary<string, object>;
						SeatInfo2 seatInfo = new SeatInfo2();
						seatInfo.id = (int)dictionary2["id"];
						seatInfo.isFree = (bool)dictionary2["isFree"];
						seatInfo.gunValue = (int)dictionary2["gunValue"];
						if (!seatInfo.isFree)
						{
							Dictionary<string, object> dictionary3 = dictionary2["user"] as Dictionary<string, object>;
							seatInfo.user = new HW2_UserInfo
							{
								id = (int)dictionary3["id"],
								nickname = (string)dictionary3["nickname"],
								gameScore = (int)dictionary3["gameScore"],
								level = (int)dictionary3["level"],
								photoId = (int)dictionary3["photoID"]
							};
						}
						seats.Add(seatInfo);
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
