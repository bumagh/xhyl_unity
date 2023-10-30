using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.UseFullInspector;
using System.Collections.Generic;

namespace M__M.HaiWang.FishSpawn.StatsPart
{
	public class FormationStats
	{
		public class FormationReporter
		{
			public Dictionary<string, object> raw;
		}

		public class FishInfo
		{
			public int id;

			public FishType type;

			public int formationId;

			public int index;

			public FishBehaviour fish;

			public float startTime;

			public float endTime;

			public float GetLifetime()
			{
				return endTime - startTime;
			}
		}

		private Dictionary<int, FishInfo> fishMap = new Dictionary<int, FishInfo>();

		private FormationReporter reporter;

		public void MakeReporter()
		{
		}

		public FormationReporter GetReporter()
		{
			return null;
		}

		public static void OnFormationPlay(FishFormationBehaviour formation)
		{
		}

		public static void OnFishSpawn(FishBehaviour fish)
		{
		}

		public static void OnFishDie(FishBehaviour fish)
		{
		}
	}
}
