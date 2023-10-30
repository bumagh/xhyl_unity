using System;

namespace M__M.HaiWang.Message
{
	[Serializable]
	public class LockGunSelectInfo
	{
		public int _seatId;

		public int _fishId;

		public int _gunValue;

		public LockGunSelectInfo(int seatId, int fishId, int gunValue)
		{
			_seatId = seatId;
			_fishId = fishId;
			_gunValue = gunValue;
		}
	}
}
