using FullInspector;

namespace M__M.HaiWang.Fish
{
	public class FishQueenData : fiInspectorOnly
	{
		public float delay;

		public float speed;

		public float interval;

		public IFishGenerator<FishType> generator;

		public FishQueenData()
		{
			delay = 0f;
			speed = 1f;
			interval = 1f;
			generator = new FG_SingleType<FishType>
			{
				type = FishType.Gurnard_迦魶鱼,
				count = 1
			};
		}
	}
}
