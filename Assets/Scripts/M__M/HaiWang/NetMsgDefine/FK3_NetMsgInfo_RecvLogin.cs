using M__M.GameHall.Common;
using M__M.HaiWang.Demo;
using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.NetMsgDefine
{
	public class FK3_NetMsgInfo_RecvLogin : FK3_NetMsgInfoBase
	{
		public FK3_HW2_UserInfo user;

		public bool isShutup;

		public FK3_NetMsgInfo_RecvLogin(object[] args = null)
		{
			m_args = args;
			user = new FK3_HW2_UserInfo();
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
					try
					{
						isShutup = (bool)dictionary["isShutup"];
						Dictionary<string, object> dictionary2 = dictionary["user"] as Dictionary<string, object>;
						user.username = (dictionary2["username"] as string);
						user.nickname = (dictionary2["nickname"] as string);
						user.gameGold = (int)dictionary2["gameGold"];
						user.expeGold = (int)dictionary2["expeGold"];
						user.level = (int)dictionary2["level"];
						user.photoId = (int)dictionary2["photoId"];
						user.shutupStatus = (int)dictionary2["shutupStatus"];
						user.sex = (string)dictionary2["sex"];
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError(ex.Message);
						string content = $"账号登录异常,请联系客服";
						FK3_AlertDialog.GetInstance().ShowDialog(content, showOkCancel: false, delegate
						{
							FK3_MB_Singleton<FK3_GameUIController>.Get().QuitToLogin();
						});
					}
				}
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogError(ex2.Message);
				valid = false;
			}
		}
	}
}
