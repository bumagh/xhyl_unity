using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.UseFullInspector;
using System.Collections.Generic;

namespace M__M.HaiWang.FishSpawn.StatsPart
{
	public class FK3_FormationStats
	{
		public class FormationReporter
		{
			public Dictionary<string, object> raw;
		}

		public class FishInfo
		{
			public int id;

			public FK3_FishType type;

			public int formationId;

			public int index;

			public FK3_FishBehaviour fish;

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

		public static void OnFormationPlay(FK3_FishFormationBehaviour formation)
		{
		}

		public static void OnFishSpawn(FK3_FishBehaviour fish)
		{
		}

		public static void OnFishDie(FK3_FishBehaviour fish)
		{
		}
	}
}
