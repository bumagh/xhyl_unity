using System;

namespace M__M.HaiWang.Message
{
	[Serializable]
	public class FK3_LockGunSelectInfo
	{
		public int _seatId;

		public int _fishId;

		public int _gunValue;

		public FK3_LockGunSelectInfo(int seatId, int fishId, int gunValue)
		{
			_seatId = seatId;
			_fishId = fishId;
			_gunValue = gunValue;
		}
	}
}
