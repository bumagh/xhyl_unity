public class Hfh_SendMsgManager
{
	public static void Send_Login(string username, string password)
	{
		object[] args = new object[2]
		{
			username,
			password
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/userLogin", args);
	}

	public static void Send_ReLogin(string username, string password, int type)
	{
		object[] args = new object[3]
		{
			username,
			password,
			type
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/reLogin", args);
	}

	public static void Send_QuitGame()
	{
		object[] args = new object[0];
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/quitGame", args);
	}

	public static void Send_PlayerInfo(int userId)
	{
		object[] args = new object[1]
		{
			userId
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/playerInfo", args);
	}

	public static void Send_RoomInfo()
	{
		object[] args = new object[0];
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/getRoomInfo", args);
	}

	public static void Send_EnterRoom(int roomType)
	{
		object[] args = new object[1]
		{
			roomType
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/enterRoom", args);
	}

	public static void Send_GetRoomInfo(int roomID)
	{
		object[] args = new object[1]
		{
			roomID
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/getDesksInfo", args);
	}

	public static void Send_EnterDesk(int deskId, int zhuanTai)
	{
		object[] args = new object[2]
		{
			deskId,
			zhuanTai
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/enterDesk", args);
	}

	public static void Send_LeaveRoom(int roomType)
	{
		object[] args = new object[1]
		{
			roomType
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/leaveRoom", args);
	}

	public static void Send_LeaveDesk(int deskId)
	{
		object[] args = new object[1]
		{
			deskId
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/leaveDesk", args);
	}

	public static void Send_UserCoinIn(int coin)
	{
		object[] args = new object[1]
		{
			coin
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/userCoinIn", args);
	}

	public static void Send_UserCoinOut(int score)
	{
		object[] args = new object[1]
		{
			score
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/userCoinOut", args);
	}

	public static void Send_StartGame()
	{
		object[] args = new object[0];
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/", args);
	}

	public static void Send_OneCard(int deskId, int bet)
	{
		object[] args = new object[2]
		{
			deskId,
			bet
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/OneCard", args);
	}

	public static void Send_TwiceCard(int deskId, int[] holdcards)
	{
		object[] args = new object[2]
		{
			deskId,
			holdcards
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/TwiceCard", args);
	}

	public static void Send_startMultiple(int deskId, int mode)
	{
		object[] args = new object[2]
		{
			deskId,
			mode
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/startMultiple", args);
	}

	public static void Send_multipleInfo(int deskId, int BigOrSmall)
	{
		object[] args = new object[2]
		{
			deskId,
			BigOrSmall
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/multipleInfo", args);
	}

	public static void Send_EndMultiple(int deskId)
	{
		object[] args = new object[1]
		{
			deskId
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/EndMultiple", args);
	}

	public static void Send_getDeskHistory(int deskId)
	{
		object[] args = new object[1]
		{
			deskId
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/getDeskHistory", args);
	}

	public static void Send_LogoutKeep(int deskId, int timeNum)
	{
		object[] args = new object[2]
		{
			deskId,
			timeNum
		};
		Hfh_Singleton<Hfh_NetManager>.GetInstance().Send("userService/LogoutKeep", args);
	}
}
