using M__M.HaiWang.GameDefine;
using System.Collections.Generic;

namespace M__M.HaiWang.UIDefine
{
	public class LobbyContext
	{
		public string strAccount = "ding888";

		public string strPassword = "ding888";

		public int curRoomId = 1;

		public int curDeskId = 1;

		public int curSeatId = 1;

		public HW2_UserInfo user = new HW2_UserInfo();

		public RoomInfo[] rooms;

		public List<DeskInfo> desks;

		public List<SeatInfo2> inGameSeats;

		public LobbyContext()
		{
			strAccount = "ding888";
			strPassword = "ding888";
		}

		public RoomInfo GetCurRoom()
		{
			if (rooms == null)
			{
				return null;
			}
			RoomInfo[] array = rooms;
			foreach (RoomInfo roomInfo in array)
			{
				if (roomInfo.id == curRoomId)
				{
					return roomInfo;
				}
			}
			return null;
		}

		public DeskInfo GetDeskById(int deskId)
		{
			if (desks == null)
			{
				return null;
			}
			foreach (DeskInfo desk in desks)
			{
				if (desk.id == deskId)
				{
					return desk;
				}
			}
			return null;
		}

		public DeskInfo GetCurDesk()
		{
			if (desks == null)
			{
				return null;
			}
			foreach (DeskInfo desk in desks)
			{
				if (desk.id == curDeskId)
				{
					return desk;
				}
			}
			return null;
		}

		public SeatInfo GetCurSeat()
		{
			return GetCurDesk()?.GetSeat(curSeatId);
		}

		public SeatInfo2 GetCurInGameSeat()
		{
			if (inGameSeats == null)
			{
				return null;
			}
			foreach (SeatInfo2 inGameSeat in inGameSeats)
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
