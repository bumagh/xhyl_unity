namespace M__M.HaiWang.Fish
{
	public class FishData4Hit
	{
		public int fishId;

		public int fishType;

		public FishData4Hit(int id, int type)
		{
			fishId = id;
			fishType = type;
		}

		public FishData4Hit(FishBehaviour fish)
		{
			fishId = fish.id;
			fishType = (int)fish.type;
		}
	}
}
