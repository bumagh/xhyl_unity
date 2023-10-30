using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.miracle9.game.entity1
{
	public class BCBM_User
	{
		public string username;

		public string uid;

		public string unionuid;

		public string surname;

		public string password;

		public int liveUid;

		public int robotType;

		public string code;

		public string wechat;

		public string openid;

		public int integral;

		public int accountType;

		public int sex;

		public int PhotoId;

		public string pic;

		public string telephone;

		public string area;

		public string city;

		public string province;

		public string site;

		public int roleId;

		public int isBetting;

		public int agentOne;

		public int agentTwo;

		public int agentThree;

		public int agentFour;

		public int parentId;

		public int handleId;

		public int status;

		public int jurisdiction;

		public int usernameStatus;

		public string loginIp;

		public int diamondsGrade;

		public int starsGrade;

		public string loginArea;

		public double userCredit;

		public double usedCredit;

		public double quickCredit;

		public double quicdCredit;

		public int safetyCode;

		public string fraction;

		public string temporary;

		public string lastRoom;

		public int lastDeskId;

		public int shareRate;

		public string userWinSum;

		public int playNum;

		public string giveWin;

		public int proportion;

		public int usercontrol;

		public double expeGold;

		public int type;

		public static BCBM_User CreateWithDic(Dictionary<string, object> data)
		{
			BCBM_User bCBM_User = new BCBM_User();
			try
			{
				bCBM_User.username = (string)data["username"];
				bCBM_User.surname = (string)data["surname"];
				bCBM_User.telephone = TryGetValue(data, "telephone", string.Empty);
				bCBM_User.PhotoId = (int)data["PhotoId"] - 1;
				bCBM_User.sex = (int)data["sex"];
				bCBM_User.quickCredit = (int)data["quickCredit"];
				bCBM_User.expeGold = (int)data["expeGold"];
				bCBM_User.type = (int)data["type"];
				UnityEngine.Debug.Log((int)data["uid"]);
				bCBM_User.uid = (string)data["uid"];
				return bCBM_User;
			}
			catch (Exception)
			{
				string str = string.Empty;
				foreach (string key in data.Keys)
				{
					str = str + key + ", ";
				}
				return bCBM_User;
			}
		}

		private static T TryGetValue<T>(Dictionary<string, object> data, string key, T defValue)
		{
			if (data.ContainsKey(key))
			{
				return (T)data[key];
			}
			return defValue;
		}
	}
}
