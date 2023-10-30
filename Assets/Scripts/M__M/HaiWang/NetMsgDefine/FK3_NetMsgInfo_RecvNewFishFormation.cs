using M__M.HaiWang.Fish;
using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class FK3_NetMsgInfo_RecvNewFishFormation : FK3_NetMsgInfoBase
	{
		public List<FK3_NewFishFormationItemInfo> items = new List<FK3_NewFishFormationItemInfo>();

		public float startTime;

		public FK3_NetMsgInfo_RecvNewFishFormation(object[] args = null)
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
					Dictionary<string, object> dictionary2 = dictionary["fishFormation"] as Dictionary<string, object>;
					startTime = 5f;
					FK3_NewFishFormationItemInfo fK3_NewFishFormationItemInfo = new FK3_NewFishFormationItemInfo();
					fK3_NewFishFormationItemInfo.startFishID = (int)dictionary2["startFishId"];
					fK3_NewFishFormationItemInfo.fishCount = (int)dictionary2["fishCount"];
					fK3_NewFishFormationItemInfo.formationID = (int)dictionary2["formationId"];
					string text = (string)dictionary2["position"];
					string[] array = text.Split('#');
					fK3_NewFishFormationItemInfo.posX = float.Parse(array[0]);
					fK3_NewFishFormationItemInfo.posY = float.Parse(array[1]);
					object[] array2 = dictionary2["randomList"] as object[];
					List<FK3_RandomDataInput> list = fK3_NewFishFormationItemInfo.randomList = new List<FK3_RandomDataInput>();
					object[] array3 = array2;
					foreach (object obj in array3)
					{
						Dictionary<string, object> dictionary3 = obj as Dictionary<string, object>;
						list.Add(new FK3_RandomDataInput
						{
							randID = (int)dictionary3["randIndex"],
							type = (FK3_FishType)dictionary3["fishType"]
						});
					}
					items.Add(fK3_NewFishFormationItemInfo);
				}
				else
				{
					UnityEngine.Debug.LogError("======code======" + code);
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
