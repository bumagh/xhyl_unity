using FullInspector;

namespace M__M.HaiWang.Fish
{
	public class FK3_FishQueenData : fiInspectorOnly
	{
		public float delay;

		public float speed;

		public float interval;

		public FK3_IFishGenerator<FK3_FishType> generator;

		public FK3_FishQueenData()
		{
			delay = 0f;
			speed = 1f;
			interval = 1f;
			generator = new FK3_FG_SingleType<FK3_FishType>
			{
				type = FK3_FishType.Gurnard_迦魶鱼,
				count = 1
			};
		}
	}
}
