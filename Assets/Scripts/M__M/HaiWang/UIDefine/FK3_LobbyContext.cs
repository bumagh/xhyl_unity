using M__M.HaiWang.GameDefine;
using System.Collections.Generic;

namespace M__M.HaiWang.UIDefine
{
	public class FK3_LobbyContext
	{
		public string strAccount = "ding888";

		public string strPassword = "ding888";

		public int curRoomId = 1;

		public int curDeskId = 1;

		public int curSeatId = 1;

		public FK3_HW2_UserInfo user = new FK3_HW2_UserInfo();

		public FK3_RoomInfo[] rooms;

		public List<FK3_DeskInfo> desks;

		public List<FK3_SeatInfo2> inGameSeats;

		public FK3_LobbyContext()
		{
			strAccount = "ding888";
			strPassword = "ding888";
		}

		public FK3_RoomInfo GetCurRoom()
		{
			if (rooms == null)
			{
				return null;
			}
			FK3_RoomInfo[] array = rooms;
			foreach (FK3_RoomInfo fK3_RoomInfo in array)
			{
				if (fK3_RoomInfo.id == curRoomId)
				{
					return fK3_RoomInfo;
				}
			}
			return null;
		}

		public FK3_DeskInfo GetDeskById(int deskId)
		{
			if (desks == null)
			{
				return null;
			}
			foreach (FK3_DeskInfo desk in desks)
			{
				if (desk.id == deskId)
				{
					return desk;
				}
			}
			return null;
		}

		public FK3_DeskInfo GetCurDesk()
		{
			if (desks == null)
			{
				return null;
			}
			foreach (FK3_DeskInfo desk in desks)
			{
				if (desk.id == curDeskId)
				{
					return desk;
				}
			}
			return null;
		}

		public FK3_SeatInfo GetCurSeat()
		{
			return GetCurDesk()?.GetSeat(curSeatId);
		}

		public FK3_SeatInfo2 GetCurInGameSeat()
		{
			if (inGameSeats == null)
			{
				return null;
			}
			foreach (FK3_SeatInfo2 inGameSeat in inGameSeats)
			{
				if (inGameSeat.id == curSeatId)
				{
					return inGameSeat;
				}
			}
			return null;
		}
	}
}
