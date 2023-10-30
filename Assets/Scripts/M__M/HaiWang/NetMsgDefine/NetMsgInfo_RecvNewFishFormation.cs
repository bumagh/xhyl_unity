using M__M.HaiWang.Fish;
using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class NetMsgInfo_RecvNewFishFormation : NetMsgInfoBase
	{
		public List<NewFishFormationItemInfo> items = new List<NewFishFormationItemInfo>();

		public float startTime;

		public NetMsgInfo_RecvNewFishFormation(object[] args = null)
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
					NewFishFormationItemInfo newFishFormationItemInfo = new NewFishFormationItemInfo();
					newFishFormationItemInfo.startFishID = (int)dictionary2["startFishId"];
					newFishFormationItemInfo.fishCount = (int)dictionary2["fishCount"];
					newFishFormationItemInfo.formationID = (int)dictionary2["formationId"];
					string text = (string)dictionary2["position"];
					string[] array = text.Split('#');
					newFishFormationItemInfo.posX = float.Parse(array[0]);
					newFishFormationItemInfo.posY = float.Parse(array[1]);
					object[] array2 = dictionary2["randomList"] as object[];
					List<RandomDataInput> list = newFishFormationItemInfo.randomList = new List<RandomDataInput>();
					object[] array3 = array2;
					foreach (object obj in array3)
					{
						Dictionary<string, object> dictionary3 = obj as Dictionary<string, object>;
						list.Add(new RandomDataInput
						{
							randID = (int)dictionary3["randIndex"],
							type = (FishType)dictionary3["fishType"]
						});
					}
					items.Add(newFishFormationItemInfo);
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
