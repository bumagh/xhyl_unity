using System;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Core
{
	[Serializable]
	public class SpawnerData
	{
		public float startDelay;

		public int count = 1;

		public float interval = 1f;

		public float finishDelay;

		public float totalDuration => startDelay + (float)Mathf.Max(0, count - 1) * interval + finishDelay;

		public SpawnerData Clone()
		{
			SpawnerData spawnerData = new SpawnerData();
			CopyTo(this, spawnerData);
			return spawnerData;
		}

		public static void CopyTo(SpawnerData source, SpawnerData dest)
		{
			dest.startDelay = source.startDelay;
			dest.count = source.count;
			dest.interval = source.interval;
			dest.finishDelay = source.finishDelay;
		}
	}
}
