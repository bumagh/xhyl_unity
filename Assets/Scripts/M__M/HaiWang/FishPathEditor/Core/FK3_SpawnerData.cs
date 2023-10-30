using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Core
{
	[Serializable]
	public class FK3_SpawnerData
	{
		public float startDelay;

		public int count = 1;

		public float interval = 1f;

		public float finishDelay;

		public float totalDuration => startDelay + (float)Mathf.Max(0, count - 1) * interval + finishDelay;

		public FK3_SpawnerData Clone()
		{
			FK3_SpawnerData fK3_SpawnerData = new FK3_SpawnerData();
			CopyTo(this, fK3_SpawnerData);
			return fK3_SpawnerData;
		}

		public static void CopyTo(FK3_SpawnerData source, FK3_SpawnerData dest)
		{
			dest.startDelay = source.startDelay;
			dest.count = source.count;
			dest.interval = source.interval;
			dest.finishDelay = source.finishDelay;
		}
	}
}
