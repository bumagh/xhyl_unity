namespace M__M.HaiWang.Fish
{
	public class FK3_FishData4Hit
	{
		public int fishId;

		public int fishType;

		public FK3_FishData4Hit(int id, int type)
		{
			fishId = id;
			fishType = type;
		}

		public FK3_FishData4Hit(FK3_FishBehaviour fish)
		{
			fishId = fish.id;
			fishType = (int)fish.type;
		}
	}
}
