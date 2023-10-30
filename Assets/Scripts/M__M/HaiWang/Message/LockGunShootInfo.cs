using System;

namespace M__M.HaiWang.Message
{
	[Serializable]
	public class LockGunShootInfo
	{
		public int _gunValue;

		public int _gunAngle;

		public int _gunType;

		public int _fishId;

		public int _fishType;

		public LockGunShootInfo(int gunValue, int gunAngle, int gunType, int fishId, int fishType)
		{
			_gunValue = gunValue;
			_gunAngle = gunAngle;
			_gunType = gunType;
			_fishId = fishId;
			_fishType = fishType;
		}
	}
}
